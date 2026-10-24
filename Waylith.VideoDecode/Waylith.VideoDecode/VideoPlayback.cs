using System;
using System.Collections.Generic;
using System.Threading;
using Brutal.VulkanApi;
using Waylith.Graphics;
using Waylith.Rendering;

namespace Waylith.VideoDecode;

internal sealed class VideoPlayback : IDisposable
{
	private sealed class StopRequestedException : Exception
	{
	}

	private const long ReadyBudgetBytes = 67108864L;

	private readonly object gate = new object();

	private readonly VideoMedia media;

	private readonly Thread worker;

	private readonly Queue<VideoFrame> ready = new Queue<VideoFrame>();

	private readonly Stack<GpuTextureAllocation> freeImages = new Stack<GpuTextureAllocation>();

	private readonly List<GpuTextureAllocation> allImages = new List<GpuTextureAllocation>();

	private readonly List<DecodedPicture> decoded = new List<DecodedPicture>();

	private VideoFrameSource source;

	private int poolCapacity;

	private int readyTarget = 3;

	private bool stop;

	private bool disposed;

	private bool seekPending;

	private double seekTarget;

	private int generation;

	public volatile bool Looping;

	public volatile bool Prepared;

	public volatile bool Ended;

	public volatile string Error;

	public int Width { get; private set; }

	public int Height { get; private set; }

	public double FrameRate { get; private set; }

	public long FrameCount { get; private set; }

	public double Length { get; private set; }

	public VideoAudio Audio { get; private set; }

	public bool HasQueuedFrames
	{
		get
		{
			lock (gate)
			{
				return ready.Count > 0;
			}
		}
	}

	public VideoPlayback(VideoMedia media, bool looping)
	{
		this.media = media;
		Looping = looping;
		VideoDecodeService.Track(this);
		worker = new Thread(Run)
		{
			IsBackground = true,
			Name = "VideoPlayback"
		};
		worker.Start();
	}

	public void Seek(double time)
	{
		lock (gate)
		{
			seekPending = true;
			seekTarget = System.Math.Max(0.0, time);
			generation++;
			Ended = false;
			VideoFrame result;
			while (ready.TryDequeue(out result))
			{
				freeImages.Push(result.Image);
			}
			Monitor.PulseAll(gate);
		}
	}

	public VideoFrame TakeDue(double streamTime, bool skip)
	{
		lock (gate)
		{
			VideoFrame videoFrame = null;
			VideoFrame result;
			while (ready.TryPeek(out result) && result.StreamTime <= streamTime)
			{
				if (videoFrame != null)
				{
					freeImages.Push(videoFrame.Image);
				}
				videoFrame = ready.Dequeue();
				if (!skip)
				{
					break;
				}
			}
			if (videoFrame != null)
			{
				Monitor.PulseAll(gate);
			}
			return videoFrame;
		}
	}

	public VideoFrame TakeFirst()
	{
		lock (gate)
		{
			if (!ready.TryDequeue(out var result))
			{
				return null;
			}
			Monitor.PulseAll(gate);
			return result;
		}
	}

	public void Release(VideoFrame frame)
	{
		lock (gate)
		{
			freeImages.Push(frame.Image);
			Monitor.PulseAll(gate);
		}
	}

	private void Run()
	{
		try
		{
			source = VideoFrameSource.Open(media);
			source.AcquireImage = AcquireImage;
			source.ReleaseImage = ReleaseImage;
			Width = source.Width;
			Height = source.Height;
			Length = source.Length;
			FrameCount = source.FrameCount;
			FrameRate = source.FrameRate;
			readyTarget = (int)System.Math.Clamp(67108864 / ((long)Width * (long)Height * 4), 3L, 16L);
			poolCapacity = source.ReorderDepth + readyTarget + 2;
			Audio = VideoAudio.TryOpen(media);
			int num = 0;
			double num2 = 0.0;
			double num3 = 0.0;
			int num4 = 0;
			double num5 = 0.5 / FrameRate;
			long num6 = -1L;
			while (true)
			{
				lock (gate)
				{
					while (!stop && !seekPending && (ready.Count >= readyTarget || Ended))
					{
						Monitor.Wait(gate);
					}
					if (stop)
					{
						break;
					}
					if (seekPending)
					{
						seekPending = false;
						num2 = System.Math.Min(seekTarget, Length);
						num3 = num2;
						num = 0;
						num6 = -1L;
						source.Seek(num2);
					}
					num4 = generation;
				}
				decoded.Clear();
				bool flag = !source.ReadNext(decoded);
				lock (gate)
				{
					foreach (DecodedPicture item in decoded)
					{
						if (generation != num4 || item.Time < num2 - num5)
						{
							freeImages.Push(item.Image);
							continue;
						}
						num6 = System.Math.Max(num6 + 1, (long)System.Math.Round(item.Time * FrameRate));
						ready.Enqueue(new VideoFrame
						{
							Image = item.Image,
							Time = item.Time,
							StreamTime = (double)num * Length + item.Time - num3,
							Index = num6,
							Loop = num
						});
						Prepared = true;
					}
					if (flag && generation == num4)
					{
						if (Looping)
						{
							source.Seek(0.0);
							num2 = 0.0;
							num6 = -1L;
							num++;
						}
						else
						{
							Ended = true;
						}
					}
				}
			}
		}
		catch (StopRequestedException)
		{
		}
		catch (Exception ex2)
		{
			if (!stop)
			{
				Error = ex2.Message;
			}
		}
	}

	private GpuTextureAllocation AcquireImage()
	{
		lock (gate)
		{
			while (true)
			{
				if (stop)
				{
					throw new StopRequestedException();
				}
				if (freeImages.TryPop(out var result))
				{
					return result;
				}
				if (allImages.Count < poolCapacity)
				{
					break;
				}
				Monitor.Wait(gate);
			}
		}
		GpuTextureAllocation gpuTextureAllocation = GpuTextureService.CreateEmptyTexture2D("Video." + media.Label, Width, Height, 1, VkFormat.R8G8B8A8UNorm, VkImageUsageFlags.StorageBit, GpuMemoryCategory.RenderTarget) ?? throw new InvalidOperationException("could not allocate a video frame image");
		lock (gate)
		{
			allImages.Add(gpuTextureAllocation);
			return gpuTextureAllocation;
		}
	}

	private void ReleaseImage(GpuTextureAllocation image)
	{
		lock (gate)
		{
			freeImages.Push(image);
			Monitor.PulseAll(gate);
		}
	}

	public void Dispose()
	{
		lock (gate)
		{
			if (disposed)
			{
				return;
			}
			disposed = true;
			stop = true;
			Monitor.PulseAll(gate);
		}
		worker.Join();
		Audio?.Dispose();
		source?.Dispose();
		foreach (GpuTextureAllocation allImage in allImages)
		{
			allImage.Dispose();
		}
		allImages.Clear();
		VideoDecodeService.Untrack(this);
	}
}
