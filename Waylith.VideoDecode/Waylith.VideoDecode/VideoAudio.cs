using System;
using System.IO;
using System.Threading;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Waylith.Audio;

namespace Waylith.VideoDecode;

internal sealed class VideoAudio : IDisposable, IAudioStreamSource
{
	private const double ResyncThreshold = 0.1;

	private const double NudgeFrom = 0.04;

	private const double NudgeUntil = 0.01;

	private const float NudgeRate = 0.02f;

	private const int TailSeconds = 600;

	private static readonly LogCategory Log = LogCategory.Make("VideoDecode");

	private readonly Lock gate = new Lock();

	private readonly Mp4AacAudio track;

	private readonly IAudioSound sound;

	private readonly long streamLength;

	private IAudioVoice voice;

	private IAudioGroup group;

	private bool paused;

	private bool nudging;

	private bool muted;

	private float volume;

	private float pitch;

	private bool tapped;

	private bool disposed;

	public int Channels => track.Channels;

	public int SampleRate => track.SampleRate;

	public static VideoAudio TryOpen(VideoMedia media)
	{
		AudioBackend current = AudioBackend.Current;
		if (current == null || !current.IsInitialized)
		{
			return null;
		}
		Stream stream = null;
		try
		{
			stream = media.OpenStream();
			Span<byte> span = stackalloc byte[4];
			span = span.Slice(0, stream.ReadAtLeast(span, span.Length, throwOnEndOfStream: false));
			if (((ReadOnlySpan<byte>)span).SequenceEqual("OggS"u8) || MatroskaDemuxer.IsMatroska(span))
			{
				stream.Dispose();
				return null;
			}
			Mp4AacAudio mp4AacAudio = Mp4AacAudio.TryOpen(stream);
			if (mp4AacAudio != null)
			{
				return new VideoAudio(mp4AacAudio);
			}
		}
		catch (Exception ex)
		{
			LogCategory log = Log;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(24, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("'");
				message.AppendFormatted(media.Path ?? media.Label);
				message.AppendLiteral("' plays without sound: ");
				message.AppendFormatted(ex.Message);
			}
			log.Warning(message, "TryOpen", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith.VideoDecode\\VideoAudio.cs", 61);
		}
		stream?.Dispose();
		return null;
	}

	private VideoAudio(Mp4AacAudio track)
	{
		this.track = track;
		int num = track.Channels * 4;
		streamLength = System.Math.Min(track.Length + 600L * (long)track.SampleRate, int.MaxValue / num);
		sound = AudioBackend.Current.CreateStreamSound(track.Channels, track.SampleRate, streamLength, this);
	}

	public void Update(bool playing, double mediaTime, float speed, float newVolume, bool newMuted, IAudioGroup newGroup)
	{
		if (!playing)
		{
			if (voice != null && !paused)
			{
				voice.SetPaused(paused: true);
				paused = true;
			}
			return;
		}
		bool flag = false;
		if (voice == null || !voice.IsPlaying)
		{
			voice?.Stop();
			group = newGroup;
			voice = AudioBackend.Current.Play(sound, group, paused: true, autoRelease: false);
			if (voice == null)
			{
				return;
			}
			paused = true;
			volume = -1f;
			pitch = -1f;
			muted = !newMuted;
			flag = true;
			if (tapped)
			{
				voice.EnableOutputTap();
			}
		}
		if (group != newGroup)
		{
			group = newGroup;
			voice.SetGroup(newGroup);
		}
		if (volume != newVolume)
		{
			voice.SetVolume(volume = newVolume);
		}
		if (muted != newMuted)
		{
			voice.SetMuted(muted = newMuted);
		}
		long num = System.Math.Clamp((long)(mediaTime * (double)track.SampleRate), 0L, streamLength - 1);
		double num2 = 0.0;
		if (!flag)
		{
			num2 = (double)(voice.PositionFrames - num) / (double)track.SampleRate / (double)speed;
			double num3 = System.Math.Abs(num2);
			flag = num3 > (paused ? 0.01 : 0.1);
			nudging = num3 > 0.04 || (nudging && num3 > 0.01);
		}
		if (flag)
		{
			voice.PositionFrames = num;
			nudging = false;
		}
		float num4 = (nudging ? (speed * ((num2 > 0.0) ? 0.98f : 1.02f)) : speed);
		if (pitch != num4)
		{
			voice.SetPitch(pitch = num4);
		}
		if (paused)
		{
			voice.SetPaused(paused: false);
			paused = false;
		}
	}

	public void Latest(Span<float> samples, int channelIndex)
	{
		tapped = true;
		if (voice == null)
		{
			samples.Clear();
			return;
		}
		voice.EnableOutputTap();
		voice.ReadOutput(samples, channelIndex);
		float num = ((muted || paused) ? 0f : volume);
		for (int i = 0; i < samples.Length; i++)
		{
			samples[i] *= num;
		}
	}

	public int Read(Span<float> interleaved)
	{
		try
		{
			using (gate.EnterScope())
			{
				if (!disposed)
				{
					return track.Read(interleaved);
				}
			}
		}
		catch
		{
		}
		return 0;
	}

	public bool Seek(long frame)
	{
		try
		{
			using (gate.EnterScope())
			{
				if (!disposed)
				{
					track.Seek(frame);
				}
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	public void Dispose()
	{
		using (gate.EnterScope())
		{
			if (disposed)
			{
				return;
			}
			disposed = true;
		}
		voice?.Stop();
		voice = null;
		sound.Release();
		track.Dispose();
	}
}
