using Waylith.Rendering.Compute;

namespace Waylith.Rendering;

public readonly struct InstanceStorageBinding
{
	public int PropertyId { get; }

	public GpuComputeBuffer? Persistent { get; }

	public InstanceDataBuffer? Dynamic { get; }

	public int Generation { get; }

	public bool IsPersistent => Persistent != null;

	public InstanceStorageBinding(int propertyId, GpuComputeBuffer persistent)
	{
		PropertyId = propertyId;
		Persistent = persistent;
		Dynamic = null;
		Generation = -1;
	}

	public InstanceStorageBinding(int propertyId, InstanceDataBuffer dynamic, int generation)
	{
		PropertyId = propertyId;
		Persistent = null;
		Dynamic = dynamic;
		Generation = generation;
	}
}
