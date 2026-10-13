using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace Waylith.Rendering;

public static class AssetPrefetcher
{
	private enum SlotKind
	{
		Texture,
		CachedTexture,
		Audio
	}

	private enum SlotState
	{
		Queued,
		Running,
		Done,
		Taken,
		Dropped
	}

	private sealed class Slot
	{
		public required string Path;

		public required int Index;

		public required SlotKind Kind;

		public SlotState State;

		public bool Claimed;

		public bool DropWhenDone;

		public LoadedTextureData Data;

		public IPrefetchedAsset? Asset;

		public bool Flag;

		public bool Cached;

		public ExceptionDispatchInfo? Error;

		public long Bytes;

		public void ReleaseResult()
		{
			Data = default(LoadedTextureData);
			Asset?.Release();
			Asset = null;
		}
	}

	private static readonly object Gate = new object();

	private static readonly Dictionary<string, Slot> Slots = new Dictionary<string, Slot>(StringComparer.Ordinal);

	private static readonly List<Slot> Ordered = new List<Slot>();

	private static readonly Queue<Slot> Pending = new Queue<Slot>();

	private static readonly int MaxWorkers = System.Math.Clamp(Environment.ProcessorCount / 2 - 1, 1, 8);

	private static readonly bool Disabled = Environment.GetEnvironmentVariable("WAYLITH_NO_ASSET_PREFETCH") == "1";

	private const long BudgetBytes = 67108864L;

	private static long _heldBytes;

	private static int _workers;

	private static int _dropCursor;

	public static Func<string, IPrefetchedAsset>? AudioLoader { get; set; }

	public static void EnqueueTextures(IEnumerable<string> paths)
	{
		Enqueue(paths, audio: false);
	}

	public static void EnqueueAudio(IEnumerable<string> paths)
	{
		if (AudioLoader != null)
		{
			Enqueue(paths, audio: true);
		}
	}

	private static void Enqueue(IEnumerable<string> paths, bool audio)
	{
		if (Disabled)
		{
			return;
		}
		lock (Gate)
		{
			foreach (string path in paths)
			{
				string fullPath = Path.GetFullPath(path);
				if (!Slots.ContainsKey(fullPath))
				{
					SlotKind slotKind = (audio ? SlotKind.Audio : (fullPath.EndsWith(".dds", StringComparison.OrdinalIgnoreCase) ? SlotKind.CachedTexture : SlotKind.Texture));
					if (slotKind != 0 || !Texture2D.DeferringLoads || !GpuTextureService.TryDescribeTexture(fullPath, premultiplied: false, out var _))
					{
						Slot slot = new Slot
						{
							Path = fullPath,
							Index = Ordered.Count,
							Kind = slotKind
						};
						Slots[fullPath] = slot;
						Ordered.Add(slot);
						Pending.Enqueue(slot);
					}
				}
			}
			while (_workers < MaxWorkers && Pending.Count > 0)
			{
				_workers++;
				Thread thread = new Thread(WorkerLoop);
				thread.IsBackground = true;
				thread.Name = "AssetPrefetch";
				thread.Start();
			}
			Monitor.PulseAll(Gate);
		}
	}

	public static void Drain()
	{
		lock (Gate)
		{
			foreach (Slot item in Ordered)
			{
				if (!item.Claimed)
				{
					DropUnclaimed(item);
				}
			}
			Slots.Clear();
			Ordered.Clear();
			Pending.Clear();
			_dropCursor = 0;
			Monitor.PulseAll(Gate);
		}
	}

	internal static bool TryTake(string fullPath, out LoadedTextureData data, out bool gpuBuild, out bool cached)
	{
		return TryTakeTexture(fullPath, SlotKind.Texture, out data, out gpuBuild, out cached);
	}

	internal static bool TryTakeCached(string fullPath, out LoadedTextureData data, out bool hit)
	{
		bool cached;
		return TryTakeTexture(fullPath, SlotKind.CachedTexture, out data, out hit, out cached);
	}

	private static bool TryTakeTexture(string fullPath, SlotKind kind, out LoadedTextureData data, out bool flag, out bool cached)
	{
		data = default(LoadedTextureData);
		flag = false;
		cached = false;
		if (!TryClaim(fullPath, kind, out Slot slot))
		{
			return false;
		}
		data = slot.Data;
		flag = slot.Flag;
		cached = slot.Cached;
		slot.Data = default(LoadedTextureData);
		slot.Error?.Throw();
		return true;
	}

	public static bool TryTakeAudio(string fullPath, out IPrefetchedAsset? asset)
	{
		asset = null;
		if (!TryClaim(fullPath, SlotKind.Audio, out Slot slot))
		{
			return false;
		}
		asset = slot.Asset;
		slot.Asset = null;
		slot.Error?.Throw();
		return true;
	}

	private static bool TryClaim(string fullPath, SlotKind kind, out Slot slot)
	{
		bool flag;
		lock (Gate)
		{
			if (!Slots.TryGetValue(fullPath, out slot) || slot.Kind != kind)
			{
				return false;
			}
			Slots.Remove(fullPath);
			slot.Claimed = true;
			DropSkippedBefore(slot.Index);
			flag = slot.State == SlotState.Queued;
			if (flag)
			{
				slot.State = SlotState.Running;
			}
			else
			{
				while (slot.State == SlotState.Running)
				{
					Monitor.Wait(Gate);
				}
			}
		}
		if (flag)
		{
			Process(slot);
		}
		lock (Gate)
		{
			if (!flag)
			{
				_heldBytes -= slot.Bytes;
			}
			slot.State = SlotState.Taken;
			Monitor.PulseAll(Gate);
		}
		return true;
	}

	private static void DropSkippedBefore(int index)
	{
		while (_dropCursor < index)
		{
			Slot slot = Ordered[_dropCursor];
			if (!slot.Claimed)
			{
				Slots.Remove(slot.Path);
				DropUnclaimed(slot);
			}
			_dropCursor++;
		}
		_dropCursor = System.Math.Max(_dropCursor, index + 1);
	}

	private static void DropUnclaimed(Slot slot)
	{
		if (slot.State == SlotState.Running)
		{
			slot.DropWhenDone = true;
			return;
		}
		SlotState state = slot.State;
		if ((state == SlotState.Queued || state == SlotState.Done) ? true : false)
		{
			Drop(slot);
		}
	}

	private static void Drop(Slot slot)
	{
		if (slot.State == SlotState.Done)
		{
			_heldBytes -= slot.Bytes;
		}
		slot.State = SlotState.Dropped;
		slot.ReleaseResult();
	}

	private static void WorkerLoop()
	{
		while (true)
		{
			Slot slot;
			lock (Gate)
			{
				while (true)
				{
					if (Pending.Count > 0 && Pending.Peek().State != 0)
					{
						Pending.Dequeue();
						continue;
					}
					if (Pending.Count == 0)
					{
						_workers--;
						return;
					}
					if (_heldBytes < 67108864)
					{
						break;
					}
					Monitor.Wait(Gate);
				}
				slot = Pending.Dequeue();
				slot.State = SlotState.Running;
			}
			Process(slot);
			lock (Gate)
			{
				if (slot.DropWhenDone)
				{
					Drop(slot);
				}
				else
				{
					_heldBytes += slot.Bytes;
					slot.State = SlotState.Done;
				}
				Monitor.PulseAll(Gate);
			}
		}
	}

	private static void Process(Slot slot)
	{
		try
		{
			switch (slot.Kind)
			{
			case SlotKind.CachedTexture:
				slot.Flag = GpuTextureService.TryLoadTextureFromCacheDirect(slot.Path, premultiplied: false, out slot.Data);
				break;
			case SlotKind.Audio:
				slot.Asset = AudioLoader(slot.Path);
				break;
			default:
				slot.Cached = GpuTextureService.TryLoadTextureFromCacheDirect(slot.Path, premultiplied: false, out slot.Data);
				if (!slot.Cached)
				{
					slot.Data = GpuTextureService.LoadTextureFromFile(slot.Path, premultiplyAlpha: false, allowGpuBuild: true, out slot.Flag, out slot.Cached);
				}
				break;
			}
		}
		catch (Exception source)
		{
			slot.Error = ExceptionDispatchInfo.Capture(source);
		}
		long num = slot.Asset?.Bytes ?? 0;
		if (slot.Data.Levels != null)
		{
			byte[][] levels = slot.Data.Levels;
			foreach (byte[] array in levels)
			{
				num += ((array != null) ? array.Length : 0);
			}
		}
		slot.Bytes = num;
	}
}
