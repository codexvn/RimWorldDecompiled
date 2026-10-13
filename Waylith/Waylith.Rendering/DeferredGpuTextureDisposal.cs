namespace Waylith.Rendering;

internal readonly record struct DeferredGpuTextureDisposal(GpuTextureAllocation Allocation, long RetireFrame, long Bytes, bool MovedAway, long MinForcedRetireFrame);
