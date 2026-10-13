namespace Waylith.Rendering;

public readonly record struct LoadedTextureData(int Width, int Height, int MipCount, LoadedTextureFormat Format, byte[][] Levels, bool Opaque = false, (long Length, long WriteTicks)? SourceStamp = null, TextureSwizzle Swizzle = default(TextureSwizzle));
