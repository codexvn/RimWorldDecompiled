namespace Waylith.Rendering;

public readonly record struct DdsInfo(int Width, int Height, int MipCount, LoadedTextureFormat Format, DdsAlphaMode AlphaMode, TextureSwizzle Swizzle = default(TextureSwizzle));
