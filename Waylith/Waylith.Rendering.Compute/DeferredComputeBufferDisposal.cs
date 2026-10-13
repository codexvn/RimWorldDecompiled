namespace Waylith.Rendering.Compute;

internal readonly record struct DeferredComputeBufferDisposal(GpuComputeBuffer Buffer, long RetireFrame);
