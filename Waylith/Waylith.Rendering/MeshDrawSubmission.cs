using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using Waylith.Graphics;
using Waylith.Graphics.Meshes;

namespace Waylith.Rendering;

public static class MeshDrawSubmission
{
	private readonly struct SubmittedMeshDraw
	{
		public long Order { get; }

		public long Sequence { get; }

		public MeshDrawCommand Command { get; }

		public SubmittedMeshDraw(long order, long sequence, MeshDrawCommand command)
		{
			Order = order;
			Sequence = sequence;
			Command = command;
		}
	}

	public readonly struct SubmissionOrderScope : IDisposable
	{
		private readonly bool previousHasScopedOrder;

		private readonly long previousScopedOrder;

		private readonly long previousScopedOffset;

		internal SubmissionOrderScope(long order)
		{
			previousHasScopedOrder = HasScopedOrder;
			previousScopedOrder = ScopedOrder;
			previousScopedOffset = ScopedOffset;
			HasScopedOrder = true;
			ScopedOrder = order;
			ScopedOffset = 0L;
		}

		public void Dispose()
		{
			HasScopedOrder = previousHasScopedOrder;
			ScopedOrder = previousScopedOrder;
			ScopedOffset = previousScopedOffset;
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct CaptureScope : IDisposable
	{
		public void Dispose()
		{
			captureTarget = null;
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public readonly struct PropertyArenaScope : IDisposable
	{
		public void Dispose()
		{
			captureArena = null;
		}
	}

	private static GraphicsDevice? graphicsDeviceCached;

	public static DrawItemFactory? drawItemMaker;

	private static readonly Lock SyncRoot = new Lock();

	private static readonly List<SubmittedMeshDraw> Commands = new List<SubmittedMeshDraw>();

	private static readonly DrawPropertyArena[] arenas = new DrawPropertyArena[2]
	{
		new DrawPropertyArena(),
		new DrawPropertyArena()
	};

	private static readonly MeshDrawCommand[][] snapshotBuffers = new MeshDrawCommand[2][]
	{
		Array.Empty<MeshDrawCommand>(),
		Array.Empty<MeshDrawCommand>()
	};

	private static int currentArenaIndex;

	private static readonly List<PersistentDrawCallSource>[] submittedPersistentSources = new List<PersistentDrawCallSource>[2]
	{
		new List<PersistentDrawCallSource>(),
		new List<PersistentDrawCallSource>()
	};

	private static readonly List<InstanceStorageBinding> instanceStorageScratch = new List<InstanceStorageBinding>();

	private static readonly List<InstanceStorageBinding[]>[] instanceBindingPool = NewInstanceSlots<InstanceStorageBinding[]>();

	private static readonly List<InstancedDrawInfo>[] instanceInfoPool = NewInstanceSlots<InstancedDrawInfo>();

	private static int instanceRing;

	private static int instanceBindingCursor;

	private static int instanceInfoCursor;

	private static long nextSubmissionOrder;

	private static long nextSubmissionSequence;

	private static MeshDrawSubmissionSnapshot? cachedSnapshot;

	private static readonly List<InstanceDataBuffer> InstanceDataPool = new List<InstanceDataBuffer>();

	private static int instanceDataCursor;

	[ThreadStatic]
	private static bool HasScopedOrder;

	[ThreadStatic]
	private static long ScopedOrder;

	[ThreadStatic]
	private static long ScopedOffset;

	[ThreadStatic]
	private static List<MeshDrawCommand>? captureTarget;

	[ThreadStatic]
	private static DrawPropertyArena? captureArena;

	private static readonly int InstanceTransformsPropertyId = ShaderPropertyId.GetOrCreate("_InstanceTransforms");

	public static DrawItemFactory? DrawItemMaker => drawItemMaker;

	private static DrawPropertyArena CurrentArena => arenas[currentArenaIndex];

	public static bool IsCapturing => captureTarget != null;

	public static DrawPropertyArena ActiveArena => arenas[currentArenaIndex];

	private static List<T>[] NewInstanceSlots<T>()
	{
		List<T>[] array = new List<T>[16];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new List<T>();
		}
		return array;
	}

	public static IDisposable BeginCapture(List<MeshDrawCommand> into)
	{
		captureTarget = into;
		return default(CaptureScope);
	}

	public static PropertyArenaScope UsePropertyArena(DrawPropertyArena arena)
	{
		captureArena = arena;
		return default(PropertyArenaScope);
	}

	public static void BeginFrame(GraphicsDevice device, IRenderContext renderContext)
	{
		if (device != graphicsDeviceCached)
		{
			drawItemMaker?.Dispose();
			drawItemMaker = new DrawItemFactory(device);
			graphicsDeviceCached = device;
		}
		using (SyncRoot.EnterScope())
		{
			Commands.Clear();
			CurrentArena.Clear();
			submittedPersistentSources[currentArenaIndex].Clear();
			nextSubmissionOrder = 0L;
			nextSubmissionSequence = 0L;
			cachedSnapshot = null;
			instanceDataCursor = 0;
			instanceRing = (instanceRing + 1) % 16;
			instanceBindingCursor = 0;
			instanceInfoCursor = 0;
		}
	}

	public static void ClearFrameSubmissions()
	{
		using (SyncRoot.EnterScope())
		{
			Commands.Clear();
			CurrentArena.Clear();
			submittedPersistentSources[0].Clear();
			submittedPersistentSources[1].Clear();
			nextSubmissionOrder = 0L;
			nextSubmissionSequence = 0L;
			cachedSnapshot = null;
			instanceDataCursor = 0;
			instanceRing = (instanceRing + 1) % 16;
			instanceBindingCursor = 0;
			instanceInfoCursor = 0;
		}
	}

	public static InstanceDataBuffer RentStorageBuffer()
	{
		if (!IsCapturing)
		{
			return RentInstanceDataBuffer();
		}
		return new InstanceDataBuffer();
	}

	public static InstanceStorageBinding[]? CollectDrawStorage(IDrawPropertyProvider? properties)
	{
		if (properties == null || !properties.HasStorageBindings)
		{
			return null;
		}
		List<InstanceStorageBinding> list = new List<InstanceStorageBinding>();
		properties.CollectStorageBindings(list);
		if (list.Count != 0)
		{
			return list.ToArray();
		}
		return null;
	}

	public static InstanceDataBuffer RentInstanceDataBuffer()
	{
		using (SyncRoot.EnterScope())
		{
			if (instanceDataCursor == InstanceDataPool.Count)
			{
				InstanceDataPool.Add(new InstanceDataBuffer());
			}
			return InstanceDataPool[instanceDataCursor++];
		}
	}

	public static void SubmitPersistentSource(PersistentDrawCallSource source)
	{
		submittedPersistentSources[currentArenaIndex].Add(source);
	}

	public static FinalDrawItem? TryMakeDrawItem(in MeshDrawCommand command, string globalKeywordKey, int queueOrder = 0)
	{
		if (graphicsDeviceCached == null)
		{
			return null;
		}
		graphicsDeviceCached.MeshUploads.DrainPendingUploads();
		return drawItemMaker.TryMake(in command, CurrentArena, globalKeywordKey, queueOrder, null, 0);
	}

	public static long ReserveSubmissionOrderRange(long count)
	{
		if (count <= 0)
		{
			return 0L;
		}
		using (SyncRoot.EnterScope())
		{
			long result = nextSubmissionOrder;
			nextSubmissionOrder += count;
			return result;
		}
	}

	public static SubmissionOrderScope UseSubmissionOrder(long order)
	{
		return new SubmissionOrderScope(order);
	}

	public static DrawPropertyRange CopyProperties(IDrawPropertyProvider? properties)
	{
		if (properties == null)
		{
			return default(DrawPropertyRange);
		}
		DrawPropertyArena drawPropertyArena = captureArena;
		if (drawPropertyArena != null)
		{
			DrawPropertyRange range;
			Span<DrawProperty> destination = drawPropertyArena.Allocate(properties.PropertyCount, out range);
			properties.CopyTo(destination);
			return range;
		}
		using (SyncRoot.EnterScope())
		{
			DrawPropertyRange range2;
			Span<DrawProperty> destination2 = CurrentArena.Allocate(properties.PropertyCount, out range2);
			properties.CopyTo(destination2);
			return range2;
		}
	}

	public static DrawPropertyRange CopyProperties(DrawPropertyBlock properties)
	{
		DrawPropertyArena drawPropertyArena = captureArena;
		if (drawPropertyArena != null)
		{
			return drawPropertyArena.Copy(properties);
		}
		using (SyncRoot.EnterScope())
		{
			return CurrentArena.Copy(properties);
		}
	}

	public static void Enqueue(in MeshDrawCommand command)
	{
		if (captureTarget != null)
		{
			captureTarget.Add(command);
			return;
		}
		using (SyncRoot.EnterScope())
		{
			long order = (HasScopedOrder ? (ScopedOrder + ScopedOffset++) : nextSubmissionOrder++);
			Commands.Add(new SubmittedMeshDraw(order, nextSubmissionSequence++, command));
		}
	}

	public static void Enqueue(in QueuedMeshDraw command)
	{
		Enqueue(ToMeshDrawCommand(in command));
	}

	public static void CopyPendingCommands(List<MeshDrawCommand> into, int layerMask)
	{
		List<SubmittedMeshDraw> list = new List<SubmittedMeshDraw>();
		using (SyncRoot.EnterScope())
		{
			foreach (SubmittedMeshDraw command in Commands)
			{
				int layer = command.Command.Layer;
				if ((uint)layer < 32u && (layerMask & (1 << layer)) != 0)
				{
					list.Add(command);
				}
			}
		}
		list.Sort(delegate(SubmittedMeshDraw left, SubmittedMeshDraw right)
		{
			int num = left.Order.CompareTo(right.Order);
			return (num == 0) ? left.Sequence.CompareTo(right.Sequence) : num;
		});
		foreach (SubmittedMeshDraw item in list)
		{
			into.Add(item.Command);
		}
	}

	public static void EnqueueInstanced(Mesh mesh, int submeshIndex, Material material, ReadOnlySpan<Matrix4x4> matrices, MaterialPropertyBlock? properties, int layer)
	{
		if (matrices.Length == 0)
		{
			return;
		}
		int length = matrices.Length;
		InstanceDataBuffer instanceDataBuffer = (IsCapturing ? new InstanceDataBuffer() : RentInstanceDataBuffer());
		int generation = instanceDataBuffer.Write(MemoryMarshal.AsBytes(matrices), 64, length);
		DrawPropertyRange propertyRange = CopyProperties(properties);
		InstancedDrawInfo instancing;
		if (IsCapturing)
		{
			List<InstanceStorageBinding> list = new List<InstanceStorageBinding>
			{
				new InstanceStorageBinding(InstanceTransformsPropertyId, instanceDataBuffer, generation)
			};
			properties?.CollectStorageBindings(list);
			instancing = InstancedDrawInfo.Direct(length, list.ToArray(), null);
		}
		else
		{
			using (SyncRoot.EnterScope())
			{
				instanceStorageScratch.Clear();
				instanceStorageScratch.Add(new InstanceStorageBinding(InstanceTransformsPropertyId, instanceDataBuffer, generation));
				properties?.CollectStorageBindings(instanceStorageScratch);
				instancing = RentInstancedDirect(length, instanceStorageScratch);
			}
		}
		Enqueue(new MeshDrawCommand(mesh, material, propertyRange, Matrix4x4.Identity, submeshIndex, LayerPassRouter.Resolve(layer), instancing));
	}

	public static void EnqueueInstancedIndirect(Mesh mesh, int submeshIndex, Material material, ComputeBuffer argsBuffer, int argsOffset, MaterialPropertyBlock? properties, int layer)
	{
		List<InstanceStorageBinding> list = new List<InstanceStorageBinding>();
		properties?.CollectStorageBindings(list);
		DrawPropertyRange propertyRange = CopyProperties(properties);
		InstancedDrawInfo instancing = InstancedDrawInfo.Indirect(argsBuffer.Data, argsBuffer.LastGeneration, argsOffset, list.ToArray(), null);
		Enqueue(new MeshDrawCommand(mesh, material, propertyRange, Matrix4x4.Identity, submeshIndex, LayerPassRouter.Resolve(layer), instancing));
	}

	public static void EnqueueBatch(ReadOnlySpan<BatchedMeshDraw> draws, ReadOnlySpan<DrawProperty> properties)
	{
		if (draws.IsEmpty)
		{
			return;
		}
		List<MeshDrawCommand> list = captureTarget;
		using (SyncRoot.EnterScope())
		{
			DrawPropertyRange drawPropertyRange = (captureArena ?? CurrentArena).Copy(properties);
			for (int i = 0; i < draws.Length; i++)
			{
				BatchedMeshDraw batchedMeshDraw = draws[i];
				DrawPropertyRange propertyRange = (batchedMeshDraw.LocalProperties.IsEmpty ? default(DrawPropertyRange) : new DrawPropertyRange(drawPropertyRange.Start + batchedMeshDraw.LocalProperties.Start, batchedMeshDraw.LocalProperties.Count));
				MeshDrawCommand meshDrawCommand = ToMeshDrawCommand(batchedMeshDraw.Draw, propertyRange);
				if (list != null)
				{
					list.Add(meshDrawCommand);
				}
				else
				{
					Commands.Add(new SubmittedMeshDraw(batchedMeshDraw.SubmissionOrder, nextSubmissionSequence++, meshDrawCommand));
				}
			}
		}
	}

	public static MeshDrawSubmissionSnapshot CaptureFlushedSnapshot(GraphicsDevice graphicsDevice)
	{
		GpuTextureService.FlushPendingTextureCommands();
		graphicsDevice.MeshUploads.DrainPendingUploads();
		return CaptureSnapshot();
	}

	public static MeshDrawSubmissionSnapshot CaptureSnapshot()
	{
		using (SyncRoot.EnterScope())
		{
			MeshDrawSubmissionSnapshot? meshDrawSubmissionSnapshot = cachedSnapshot;
			if (meshDrawSubmissionSnapshot.HasValue)
			{
				return meshDrawSubmissionSnapshot.GetValueOrDefault();
			}
			CollectionsMarshal.AsSpan(Commands).Sort(delegate(SubmittedMeshDraw left, SubmittedMeshDraw right)
			{
				int num = left.Order.CompareTo(right.Order);
				return (num == 0) ? left.Sequence.CompareTo(right.Sequence) : num;
			});
			int count = Commands.Count;
			MeshDrawCommand[] array = snapshotBuffers[currentArenaIndex];
			if (array.Length < count)
			{
				array = new MeshDrawCommand[System.Math.Max(count, System.Math.Max(256, array.Length * 2))];
				snapshotBuffers[currentArenaIndex] = array;
			}
			for (int i = 0; i < count; i++)
			{
				array[i] = Commands[i].Command;
			}
			DrawPropertyArena propertyArena = arenas[currentArenaIndex];
			List<PersistentDrawCallSource> persistentSources = submittedPersistentSources[currentArenaIndex];
			currentArenaIndex ^= 1;
			arenas[currentArenaIndex].Clear();
			submittedPersistentSources[currentArenaIndex].Clear();
			MeshDrawSubmissionSnapshot meshDrawSubmissionSnapshot2 = new MeshDrawSubmissionSnapshot(array, count, propertyArena, persistentSources);
			cachedSnapshot = meshDrawSubmissionSnapshot2;
			return meshDrawSubmissionSnapshot2;
		}
	}

	private static InstancedDrawInfo RentInstancedDirect(int count, List<InstanceStorageBinding> storage)
	{
		List<InstanceStorageBinding[]> list = instanceBindingPool[instanceRing];
		if (instanceBindingCursor == list.Count)
		{
			list.Add(new InstanceStorageBinding[storage.Count]);
		}
		else if (list[instanceBindingCursor].Length != storage.Count)
		{
			list[instanceBindingCursor] = new InstanceStorageBinding[storage.Count];
		}
		InstanceStorageBinding[] array = list[instanceBindingCursor++];
		storage.CopyTo(array);
		List<InstancedDrawInfo> list2 = instanceInfoPool[instanceRing];
		if (instanceInfoCursor == list2.Count)
		{
			list2.Add(new InstancedDrawInfo());
		}
		InstancedDrawInfo instancedDrawInfo = list2[instanceInfoCursor++];
		instancedDrawInfo.ReconfigureDirect(count, array);
		return instancedDrawInfo;
	}

	public static void Dispose()
	{
		drawItemMaker?.Dispose();
	}

	private static MeshDrawCommand ToMeshDrawCommand(in QueuedMeshDraw command)
	{
		return ToMeshDrawCommand(in command, command.PropertyRange);
	}

	private static MeshDrawCommand ToMeshDrawCommand(in QueuedMeshDraw command, DrawPropertyRange propertyRange)
	{
		return new MeshDrawCommand(command.Mesh, command.Material, propertyRange, command.Matrix, command.SubmeshIndex, LayerPassRouter.Resolve(command.Layer), null, command.RenderQueueOverride, 0, command.Storage);
	}
}
