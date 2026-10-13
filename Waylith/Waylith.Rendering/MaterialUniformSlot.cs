namespace Waylith.Rendering;

public readonly record struct MaterialUniformSlot(int Offset, MaterialUniformType Type, string PropertyName, int PropertyId, MaterialUniformValue DefaultValue, int TextureResourceSlot, bool IsColor = false, bool HasAuthoredDefault = false);
