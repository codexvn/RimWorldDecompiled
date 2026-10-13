namespace Waylith.Runtime;

public readonly record struct NativeAllocationRuntimeDiagnosticsSnapshot(int CurrentFrame, int TotalCount, long TotalBytes, UnityAllocationBucketDiagnostics[] Buckets);
