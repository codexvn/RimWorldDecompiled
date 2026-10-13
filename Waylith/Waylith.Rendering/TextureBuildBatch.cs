using System;
using System.Collections.Generic;
using System.Threading;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi;
using Waylith.Rendering.Compute;
using Waylith.Runtime;

namespace Waylith.Rendering;

public static class TextureBuildBatch
{
	internal sealed class Job
	{
		public required Texture2D Texture;

		public required long StagingOffset;

		public required byte[] Mip0;

		public required bool Premultiplied;

		public BcEncodeTarget Target;

		public float Quality;

		public bool Cancelled;

		public bool Submitted;

		public bool Encode => Target != BcEncodeTarget.None;
	}

	private sealed class InFlight
	{
		public required List<(Job job, TextureBlockEncoder.BuildItem item)> Built;

		public required GpuComputeBuffer Staging;

		public required TextureBlockEncoder.InFlightBuild Gpu;
	}

	private const long MaxBatchTexels = 8388608L;

	private const int MaxTextureSize = 16384;

	private const int IdleFramesBeforeFlush = 2;

	private const int IdleFramesBeforeStagingFree = 120;

	private const long DefaultStagingBytes = 33554432L;

	private const int KeptStagingBuffers = 2;

	private static readonly Lock Gate = new Lock();

	private static readonly List<Job> Jobs = new List<Job>();

	private static readonly Queue<InFlight> InFlightBatches = new Queue<InFlight>();

	private static readonly Stack<GpuComputeBuffer> FreeStaging = new Stack<GpuComputeBuffer>();

	private static GpuComputeBuffer? _staging;

	private static long _stagingUsed;

	private static long _batchTexels;

	private static long _frame;

	private static long _lastEnqueueFrame;

	internal static bool CanBuild(in LoadedTextureData loaded)
	{
		bool flag = TextureBlockEncoder.IsConfigured;
		if (flag)
		{
			MipFilter rgba32MipChainFilter = TexturePixelData.Rgba32MipChainFilter;
			bool flag2 = (uint)(rgba32MipChainFilter - 1) <= 1u;
			flag = flag2;
		}
		if (flag && loaded.Format == LoadedTextureFormat.Rgba32)
		{
			byte[][] levels = loaded.Levels;
			if (levels != null && levels.Length == 1)
			{
				byte[] array = levels[0];
				if (array != null && array.Length == loaded.Width * loaded.Height * 4 && loaded.Width <= 16384)
				{
					return loaded.Height <= 16384;
				}
			}
		}
		return false;
	}

	internal static void Enqueue(Texture2D texture, byte[] mip0, bool premultiplied)
	{
		using (Gate.EnterScope())
		{
			long num = (long)texture.width * (long)texture.height;
			long num2 = num * 4;
			if (Jobs.Count > 0 && (_batchTexels + num > 8388608 || _stagingUsed + num2 > (long)_staging.Size))
			{
				FlushLocked(wait: false);
			}
			if (Jobs.Count == 0)
			{
				_stagingUsed = 0L;
				_batchTexels = 0L;
				if (_staging != null && num2 > (long)_staging.Size)
				{
					ReturnStaging(_staging);
					_staging = null;
				}
			}
			if (_staging == null)
			{
				_staging = TakeStaging(num2);
			}
			_staging.Upload<byte>(mip0, _stagingUsed);
			Job job = new Job
			{
				Texture = texture,
				StagingOffset = _stagingUsed,
				Mip0 = mip0,
				Premultiplied = premultiplied
			};
			_stagingUsed += (num2 + 255) & -256;
			_batchTexels += num;
			Jobs.Add(job);
			texture.PendingBuild = job;
			_lastEnqueueFrame = _frame;
		}
	}

	internal static bool RequestEncode(Texture2D texture, BcEncodeTarget target, float quality)
	{
		using (Gate.EnterScope())
		{
			Job pendingBuild = texture.PendingBuild;
			if (pendingBuild == null || pendingBuild.Submitted || !TextureBlockEncoder.CanBatchEncode(target))
			{
				return false;
			}
			pendingBuild.Target = target;
			pendingBuild.Quality = quality;
			return true;
		}
	}

	internal static void Cancel(Texture2D texture)
	{
		using (Gate.EnterScope())
		{
			Job pendingBuild = texture.PendingBuild;
			if (pendingBuild != null)
			{
				pendingBuild.Cancelled = true;
				Jobs.Remove(pendingBuild);
			}
			texture.PendingBuild = null;
		}
	}

	internal static void Resolve(Texture texture)
	{
		int? num = WaylithRuntime.Current?.App.MainThreadId;
		if ((num.HasValue && num != Environment.CurrentManagedThreadId) || !Gate.TryEnter())
		{
			return;
		}
		try
		{
			if (texture.PendingBuild != null)
			{
				FlushLocked(wait: true);
			}
		}
		finally
		{
			Gate.Exit();
		}
	}

	public static void Flush()
	{
		using (Gate.EnterScope())
		{
			FlushLocked(wait: true);
		}
	}

	internal static void Shutdown()
	{
		using (Gate.EnterScope())
		{
			foreach (Job job in Jobs)
			{
				job.Texture.PendingBuild = null;
			}
			Jobs.Clear();
			while (InFlightBatches.Count > 0)
			{
				Complete(InFlightBatches.Dequeue());
			}
			DisposeStagingLocked();
		}
	}

	internal static void OnFrameStart()
	{
		using (Gate.EnterScope())
		{
			_frame++;
			if ((Jobs.Count > 0 || InFlightBatches.Count > 0) && _frame - _lastEnqueueFrame >= 2)
			{
				FlushLocked(wait: true);
			}
			if (Jobs.Count == 0 && _frame - _lastEnqueueFrame >= 120)
			{
				DisposeStagingLocked();
			}
		}
		TextureCacheStore.FlushIfIdle();
	}

	private static void FlushLocked(bool wait)
	{
		if (Jobs.Count > 0)
		{
			Submit();
		}
		while (InFlightBatches.Count > ((!wait) ? 1 : 0))
		{
			Complete(InFlightBatches.Dequeue());
		}
	}

	private static GpuComputeBuffer TakeStaging(long bytes)
	{
		if (bytes <= 33554432 && FreeStaging.TryPop(out GpuComputeBuffer result))
		{
			return result;
		}
		return TextureBlockEncoder.CreateUploadBuffer("TextureBuild.Staging", System.Math.Max(33554432L, bytes));
	}

	private static void DisposeStagingLocked()
	{
		_staging?.DisposeNow();
		_staging = null;
		GpuComputeBuffer result;
		while (FreeStaging.TryPop(out result))
		{
			result.DisposeNow();
		}
	}

	private static void ReturnStaging(GpuComputeBuffer staging)
	{
		if ((long)staging.Size == 33554432 && FreeStaging.Count < 2)
		{
			FreeStaging.Push(staging);
		}
		else
		{
			staging.DisposeNow();
		}
	}

	private static void Submit()
	{
		List<Job> list = new List<Job>(Jobs);
		Jobs.Clear();
		_batchTexels = 0L;
		_stagingUsed = 0L;
		GpuComputeBuffer staging = _staging;
		_staging = null;
		List<TextureBlockEncoder.BuildItem> list2 = new List<TextureBlockEncoder.BuildItem>(list.Count);
		List<(Job, TextureBlockEncoder.BuildItem)> list3 = new List<(Job, TextureBlockEncoder.BuildItem)>(list.Count);
		BcEncodeTarget target = BcEncodeTarget.None;
		float quality = 0f;
		try
		{
			foreach (Job item in list)
			{
				item.Submitted = true;
				Texture2D texture = item.Texture;
				GpuTextureAllocation image = GpuTextureService.AllocateTexture2D(texture.AllocationLabel, texture.width, texture.height, texture.MipCount, VkFormat.R8G8B8A8UNorm, texture.MemoryCategory) ?? throw new InvalidOperationException("texture allocation failed");
				TextureBlockEncoder.BuildItem buildItem = new TextureBlockEncoder.BuildItem
				{
					Image = image,
					Width = texture.width,
					Height = texture.height,
					MipCount = texture.MipCount,
					StagingOffset = item.StagingOffset,
					Encode = item.Encode,
					Premultiplied = item.Premultiplied
				};
				list2.Add(buildItem);
				list3.Add((item, buildItem));
				if (item.Encode)
				{
					target = item.Target;
					quality = item.Quality;
				}
			}
			TextureBlockEncoder.InFlightBuild gpu = TextureBlockEncoder.SubmitBuildBatch(staging, list2, TexturePixelData.Rgba32MipChainFilter, target, quality);
			InFlightBatches.Enqueue(new InFlight
			{
				Built = list3,
				Staging = staging,
				Gpu = gpu
			});
		}
		catch (Exception ex)
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Error> message = new LogString<Error>(76, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("TextureBuildBatch: GPU build of ");
				message.AppendFormatted(list.Count);
				message.AppendLiteral(" textures failed, building them on the CPU: ");
				message.AppendFormatted(ex.Message);
			}
			log.Error(message, "Submit", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureBuildBatch.cs", 247);
			foreach (TextureBlockEncoder.BuildItem item2 in list2)
			{
				item2.Image.DisposeNow();
			}
			BuildOnCpu(list);
			ReturnStaging(staging);
		}
	}

	private static void Complete(InFlight batch)
	{
		try
		{
			TextureBlockEncoder.CompleteBuildBatch(batch.Gpu);
		}
		catch (Exception ex)
		{
			LogCategory log = RenderLog.Log;
			bool shouldAppend;
			LogString<Error> message = new LogString<Error>(76, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("TextureBuildBatch: GPU build of ");
				message.AppendFormatted(batch.Built.Count);
				message.AppendLiteral(" textures failed, building them on the CPU: ");
				message.AppendFormatted(ex.Message);
			}
			log.Error(message, "Complete", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\TextureBuildBatch.cs", 263);
			List<Job> list = new List<Job>(batch.Built.Count);
			foreach (var item4 in batch.Built)
			{
				var (item, _) = item4;
				item4.item.Image.Dispose();
				list.Add(item);
			}
			BuildOnCpu(list);
			ReturnStaging(batch.Staging);
			return;
		}
		ReturnStaging(batch.Staging);
		foreach (var item5 in batch.Built)
		{
			Job item2 = item5.job;
			TextureBlockEncoder.BuildItem item3 = item5.item;
			Texture2D texture = item2.Texture;
			if (item2.Cancelled)
			{
				item3.Image.DisposeNow();
				continue;
			}
			texture.PendingBuild = null;
			if (item3.Blocks != null)
			{
				item3.Image.DisposeNow();
				LoadedTextureFormat format = GpuTextureService.ToLoadedFormat(item2.Target);
				texture.CompleteBuild(format, item3.Blocks, texture.IsReadable);
				if (texture.SourcePath != null)
				{
					GpuTextureService.WriteTextureCache(texture.SourcePath, item2.Premultiplied, new LoadedTextureData(texture.width, texture.height, item3.Blocks.Length, format, item3.Blocks, texture.Opaque, texture.SourceStamp));
				}
			}
			else
			{
				byte[][] mipLevels = item3.MipLevels;
				mipLevels[0] = item2.Mip0;
				texture.CompleteBuild(item3.Image);
				if (texture.SourcePath != null)
				{
					GpuTextureService.WriteTextureCache(texture.SourcePath, item2.Premultiplied, new LoadedTextureData(texture.width, texture.height, mipLevels.Length, LoadedTextureFormat.Rgba32, mipLevels, texture.Opaque, texture.SourceStamp));
				}
			}
		}
	}

	private static void BuildOnCpu(List<Job> jobs)
	{
		foreach (Job job in jobs)
		{
			if (!job.Cancelled)
			{
				Texture2D texture = job.Texture;
				texture.PendingBuild = null;
				byte[][] array = TexturePixelData.GenerateRgba32MipChain(job.Mip0, texture.width, texture.height, texture.MipCount, parallel: false, null, job.Premultiplied);
				bool isReadable = texture.IsReadable;
				texture.CompleteBuild(LoadedTextureFormat.Rgba32, array, keepCpuData: true);
				if (texture.SourcePath != null)
				{
					GpuTextureService.WriteTextureCache(texture.SourcePath, job.Premultiplied, new LoadedTextureData(texture.width, texture.height, array.Length, LoadedTextureFormat.Rgba32, array, texture.Opaque, texture.SourceStamp));
				}
				if (job.Encode)
				{
					texture.CompressInPlace(job.Target, job.Quality);
				}
				if (!isReadable)
				{
					texture.ReleaseCpuData();
				}
			}
		}
	}
}
