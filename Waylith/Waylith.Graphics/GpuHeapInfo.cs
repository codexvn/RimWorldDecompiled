namespace Waylith.Graphics;

public readonly record struct GpuHeapInfo(int HeapIndex, bool DeviceLocal, long TotalBytes, long DriverUsageBytes, long BudgetBytes, long VmaBlockBytes, long VmaAllocationBytes, int VmaBlockCount, int VmaAllocationCount);
