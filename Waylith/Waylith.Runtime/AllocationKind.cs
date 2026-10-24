namespace Waylith.Runtime;

public enum AllocationKind
{
	Invalid,
	None,
	Temp,
	TempJob,
	Persistent,
	AudioKernel,
	Domain,
	MeshBacking,
	UI
}
