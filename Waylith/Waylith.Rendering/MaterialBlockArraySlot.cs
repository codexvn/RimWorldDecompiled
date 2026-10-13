namespace Waylith.Rendering;

public readonly record struct MaterialBlockArraySlot(int Offset, int PropertyId, MaterialBlockArrayKind Kind, int Length, int Stride, int ElementBytes);
