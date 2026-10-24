using System;
using Brutal.Numerics;
using Brutal.VulkanApi;
using Waylith.Audio;
using Waylith.Graphics;
using Waylith.Rendering;

namespace Waylith.VideoDecode;

public sealed class VideoPlayer : IDisposable
{
	private VideoMedia media;

	private VideoPlayback playback;

	private VideoFrame current;

	private GpuTextureAllocation output;

	private GpuTextureAllocation clearedTarget;

	private bool looping;

	private bool playing;

	private bool paused;

	private bool prepared;

	private bool reachedEnd;

	private bool failed;

	private double playhead;

	private double lastRealtime;

	private double pendingSeek = -1.0;

	private bool showAfterSeek;

	public float PlaybackSpeed { get; set; } = 1f;

	public bool SkipOnDrop { get; set; } = true;

	public bool AudioEnabled { get; set; } = true;

	public float AudioVolume { get; set; } = 1f;

	public bool AudioMuted { get; set; }

	public IAudioGroup AudioGroup { get; set; }

	public Func<VideoTarget> TargetProvider { get; set; }

	public GpuTextureAllocation Output => output;

	public bool IsPrepared => prepared;

	public bool IsPlaying
	{
		get
		{
			if (playing && !paused)
			{
				return prepared;
			}
			return false;
		}
	}

	public bool IsPaused => paused;

	public bool HasMedia => media != null;

	public int Width
	{
		get
		{
			if (!prepared)
			{
				return 0;
			}
			return playback.Width;
		}
	}

	public int Height
	{
		get
		{
			if (!prepared)
			{
				return 0;
			}
			return playback.Height;
		}
	}

	public double Length
	{
		get
		{
			if (!prepared)
			{
				return 0.0;
			}
			return playback.Length;
		}
	}

	public double FrameRate
	{
		get
		{
			if (!prepared)
			{
				return 0.0;
			}
			return playback.FrameRate;
		}
	}

	public long FrameCount
	{
		get
		{
			if (!prepared)
			{
				return 0L;
			}
			return playback.FrameCount;
		}
	}

	public bool HasAudio
	{
		get
		{
			if (prepared)
			{
				return playback.Audio != null;
			}
			return false;
		}
	}

	public int AudioChannels
	{
		get
		{
			if (!HasAudio)
			{
				return 0;
			}
			return playback.Audio.Channels;
		}
	}

	public int AudioSampleRate
	{
		get
		{
			if (!HasAudio)
			{
				return 0;
			}
			return playback.Audio.SampleRate;
		}
	}

	public double Time => current?.Time ?? 0.0;

	public long Frame => current?.Index ?? (-1);

	public VideoMedia Media
	{
		get
		{
			return media;
		}
		set
		{
			Close();
			failed = false;
			media = value;
		}
	}

	public bool Looping
	{
		get
		{
			return looping;
		}
		set
		{
			looping = value;
			if (playback != null)
			{
				playback.Looping = value;
			}
		}
	}

	public event Action Prepared;

	public event Action<long> FrameReady;

	public event Action LoopPointReached;

	public event Action<string> ErrorReceived;

	public void Prepare()
	{
		if (playback == null && !failed && media != null)
		{
			playback = new VideoPlayback(media, looping);
		}
	}

	public void Play(double realtime)
	{
		Prepare();
		if (reachedEnd)
		{
			reachedEnd = false;
			Seek(0.0);
		}
		playing = true;
		paused = false;
		lastRealtime = realtime;
	}

	public void Pause()
	{
		paused = true;
	}

	public void Stop()
	{
		Close();
	}

	public void Seek(double seconds)
	{
		if (!prepared)
		{
			pendingSeek = ((seconds > 0.0) ? seconds : (-1.0));
			return;
		}
		playback.Seek(seconds);
		playhead = 0.0;
		reachedEnd = false;
		showAfterSeek = true;
	}

	public void SeekFrame(long frame)
	{
		Seek((double)frame / ((prepared && playback.FrameRate > 0.0) ? playback.FrameRate : 30.0));
	}

	public void GetAudioOutput(Span<float> samples, int channel)
	{
		if (HasAudio)
		{
			playback.Audio.Latest(samples, channel);
		}
		else
		{
			samples.Clear();
		}
	}

	private void Close()
	{
		if (playback != null)
		{
			playback.Dispose();
			playback = null;
		}
		current = null;
		prepared = false;
		playing = false;
		paused = false;
		reachedEnd = false;
		playhead = 0.0;
		pendingSeek = -1.0;
		showAfterSeek = false;
	}

	public void Update(double realtime)
	{
		Advance(realtime);
		if (prepared)
		{
			VideoAudio videoAudio = playback?.Audio;
			if (videoAudio != null)
			{
				bool flag = AudioEnabled && playing && !paused && !showAfterSeek && PlaybackSpeed > 0f && current != null;
				videoAudio.Update(flag, flag ? (playhead + current.Time - current.StreamTime) : 0.0, PlaybackSpeed, AudioVolume, AudioMuted, AudioGroup);
			}
		}
	}

	private void Advance(double realtime)
	{
		if (playback == null)
		{
			return;
		}
		string error = playback.Error;
		if (error != null)
		{
			string obj = "cannot play '" + (media.Path ?? media.Label) + "': " + error;
			Close();
			failed = true;
			this.ErrorReceived?.Invoke(obj);
		}
		else
		{
			if (!playback.Prepared)
			{
				return;
			}
			if (!prepared)
			{
				prepared = true;
				lastRealtime = realtime;
				if (pendingSeek > 0.0)
				{
					double seconds = pendingSeek;
					pendingSeek = -1.0;
					Seek(seconds);
				}
				else
				{
					VideoFrame videoFrame = playback.TakeFirst();
					if (videoFrame != null)
					{
						Present(videoFrame);
					}
				}
				if (playback == null)
				{
					return;
				}
				this.Prepared?.Invoke();
				if (playback == null)
				{
					return;
				}
			}
			if (!playing || paused)
			{
				lastRealtime = realtime;
				if (showAfterSeek)
				{
					VideoFrame videoFrame2 = playback.TakeFirst();
					if (videoFrame2 != null)
					{
						Present(videoFrame2);
					}
				}
				return;
			}
			playhead += (realtime - lastRealtime) * (double)PlaybackSpeed;
			lastRealtime = realtime;
			VideoFrame videoFrame3 = playback.TakeDue(playhead, SkipOnDrop);
			if (videoFrame3 != null)
			{
				Present(videoFrame3);
			}
			if (playback != null && current != null && !playback.HasQueuedFrames)
			{
				double num = current.StreamTime + 1.0 / playback.FrameRate;
				if (!playback.Ended)
				{
					playhead = System.Math.Min(playhead, num);
				}
				else if (videoFrame3 == null && playhead >= num)
				{
					playing = false;
					reachedEnd = true;
					this.LoopPointReached?.Invoke();
				}
			}
		}
	}

	private void Present(VideoFrame next)
	{
		showAfterSeek = false;
		bool num = current != null && next.Loop > current.Loop;
		if (current != null)
		{
			playback.Release(current);
		}
		current = next;
		VideoTarget target = TargetProvider?.Invoke() ?? OwnTarget();
		if (target.Texture != null)
		{
			Blit(next.Image, target);
		}
		if (num)
		{
			this.LoopPointReached?.Invoke();
		}
		if (playback != null)
		{
			this.FrameReady?.Invoke(next.Index);
		}
	}

	private VideoTarget OwnTarget()
	{
		if (output == null)
		{
			output = GpuTextureService.CreateEmptyTexture2D("VideoOutput", playback.Width, playback.Height, 1, VkFormat.R8G8B8A8UNorm, VkImageUsageFlags.None, GpuMemoryCategory.RenderTarget);
		}
		return new VideoTarget(output, playback.Width, playback.Height, VideoAspect.Stretch);
	}

	private void Blit(GpuTextureAllocation image, VideoTarget target)
	{
		int width = playback.Width;
		int height = playback.Height;
		float num = target.Width;
		float num2 = target.Height;
		float num3 = num / (float)width;
		float num4 = num2 / (float)height;
		switch (target.Aspect)
		{
		case VideoAspect.NoScaling:
			num3 = (num4 = 1f);
			break;
		case VideoAspect.FitVertically:
			num3 = num4;
			break;
		case VideoAspect.FitHorizontally:
			num4 = num3;
			break;
		case VideoAspect.FitInside:
			num3 = (num4 = System.Math.Min(num3, num4));
			break;
		case VideoAspect.FitOutside:
			num3 = (num4 = System.Math.Max(num3, num4));
			break;
		}
		float num5 = (float)width * num3;
		float num6 = (float)height * num4;
		float num7 = (num - num5) * 0.5f;
		float num8 = (num2 - num6) * 0.5f;
		float num9 = System.Math.Max(0f, num7);
		float num10 = System.Math.Max(0f, num8);
		float num11 = System.Math.Min(num, num7 + num5);
		float num12 = System.Math.Min(num2, num8 + num6);
		int num13 = (int)MathF.Round(num9);
		int num14 = (int)MathF.Round(num10);
		int num15 = (int)MathF.Round(num11) - num13;
		int num16 = (int)MathF.Round(num12) - num14;
		int num17 = (int)MathF.Round((num9 - num7) / num3);
		int num18 = (int)MathF.Round((num10 - num8) / num4);
		int num19 = System.Math.Min(width - num17, (int)MathF.Round((num11 - num9) / num3));
		int num20 = System.Math.Min(height - num18, (int)MathF.Round((num12 - num10) / num4));
		if (num15 > 0 && num16 > 0 && num19 > 0 && num20 > 0)
		{
			if (clearedTarget != target.Texture && (num15 < target.Width || num16 < target.Height))
			{
				GpuTextureService.Clear(target.Texture, new float4(0f, 0f, 0f, 1f));
				clearedTarget = target.Texture;
			}
			VkFilter filter = ((num15 != num19 || num16 != num20) ? VkFilter.Linear : VkFilter.Nearest);
			GpuTextureService.BlitTexture(image, width, height, 0, num17, num18, num19, num20, target.Texture, 0, num13, num14, num15, num16, filter);
		}
	}

	public void Dispose()
	{
		Close();
		output?.Dispose();
		output = null;
	}
}
