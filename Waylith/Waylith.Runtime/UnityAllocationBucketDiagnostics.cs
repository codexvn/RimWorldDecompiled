namespace Waylith.Runtime;

public readonly record struct UnityAllocationBucketDiagnostics(AllocationKind Kind, int Count, long Bytes);
