using Waylith.Rendering;

namespace RimWorldWaylith.Imgui;

internal readonly record struct ImGuiMaterialDraw(Material? Material, Texture? MainTexture, nint FallbackTextureId, MaterialUniformBytes Uniforms, MaterialTextureResourceSet<Texture?> Resources, MaterialStorageArray[] StorageArrays);
