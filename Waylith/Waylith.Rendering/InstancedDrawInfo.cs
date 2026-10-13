using System;
using System.Numerics;
using Waylith.Rendering.Compute;

namespace Waylith.Rendering;

public sealed class InstancedDrawInfo
{
	public InstanceDrawMode Mode { get; private set; }

	public int InstanceCount { get; private set; }

	public InstanceRange[]? Ranges { get; private set; }

	public InstanceDataBuffer? IndirectArgsDynamic { get; private set; }

	public int IndirectArgsGeneration { get; private set; } = -1;

	public GpuComputeBuffer? IndirectArgsPersistent { get; private set; }

	public int IndirectArgsOffset { get; private set; }

	public InstanceStorageBinding[] StorageBindings { get; private set; } = Array.Empty<InstanceStorageBinding>();

	public Vector3? SortCentre { get; private set; }

	public bool IsFullyPersistent
	{
		get
		{
			if (Mode == InstanceDrawMode.Indirect && IndirectArgsPersistent == null)
			{
				return false;
			}
			InstanceStorageBinding[] storageBindings = StorageBindings;
			foreach (InstanceStorageBinding instanceStorageBinding in storageBindings)
			{
				if (!instanceStorageBinding.IsPersistent)
				{
					return false;
				}
			}
			return true;
		}
	}

	public static InstancedDrawInfo Direct(int instanceCount, InstanceStorageBinding[] storageBindings, Vector3? sortCentre = null)
	{
		return new InstancedDrawInfo
		{
			Mode = InstanceDrawMode.Direct,
			InstanceCount = instanceCount,
			StorageBindings = (storageBindings ?? Array.Empty<InstanceStorageBinding>()),
			SortCentre = sortCentre
		};
	}

	public static InstancedDrawInfo Indirect(InstanceDataBuffer args, int argsGeneration, int argsOffset, InstanceStorageBinding[] storageBindings, Vector3? sortCentre = null)
	{
		return new InstancedDrawInfo
		{
			Mode = InstanceDrawMode.Indirect,
			IndirectArgsDynamic = args,
			IndirectArgsGeneration = argsGeneration,
			IndirectArgsOffset = argsOffset,
			StorageBindings = (storageBindings ?? Array.Empty<InstanceStorageBinding>()),
			SortCentre = sortCentre
		};
	}

	public static InstancedDrawInfo Indirect(GpuComputeBuffer argsBuffer, int argsOffset, InstanceStorageBinding[] storageBindings)
	{
		return new InstancedDrawInfo
		{
			Mode = InstanceDrawMode.Indirect,
			IndirectArgsPersistent = argsBuffer,
			IndirectArgsOffset = argsOffset,
			StorageBindings = (storageBindings ?? Array.Empty<InstanceStorageBinding>())
		};
	}

	public static InstancedDrawInfo DirectRanges(InstanceRange[] ranges, InstanceStorageBinding[] storageBindings)
	{
		return new InstancedDrawInfo
		{
			Mode = InstanceDrawMode.Direct,
			Ranges = ranges,
			StorageBindings = (storageBindings ?? Array.Empty<InstanceStorageBinding>())
		};
	}

	public void ReconfigureDirect(int instanceCount, InstanceStorageBinding[] storageBindings)
	{
		Mode = InstanceDrawMode.Direct;
		InstanceCount = instanceCount;
		Ranges = null;
		IndirectArgsDynamic = null;
		IndirectArgsPersistent = null;
		IndirectArgsGeneration = -1;
		IndirectArgsOffset = 0;
		StorageBindings = storageBindings ?? Array.Empty<InstanceStorageBinding>();
		SortCentre = null;
	}

	public void ReconfigureDirectRanges(InstanceRange[] ranges, InstanceStorageBinding[] storageBindings)
	{
		Mode = InstanceDrawMode.Direct;
		Ranges = ranges;
		InstanceCount = 0;
		IndirectArgsDynamic = null;
		IndirectArgsPersistent = null;
		IndirectArgsGeneration = -1;
		IndirectArgsOffset = 0;
		StorageBindings = storageBindings ?? Array.Empty<InstanceStorageBinding>();
		SortCentre = null;
	}
}
