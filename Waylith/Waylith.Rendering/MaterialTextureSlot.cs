namespace Waylith.Rendering;

public readonly record struct MaterialTextureSlot(int ResourceSlot, int Binding, string PropertyName, int PropertyId, bool VertexVisible = false, MaterialTextureKind Kind = MaterialTextureKind.Combined, MaterialTextureDefault Default = MaterialTextureDefault.White);
