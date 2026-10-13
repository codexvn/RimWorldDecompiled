namespace Waylith.Rendering;

public readonly record struct TextureEncoding(BcEncodeTarget Target, TextureSwizzle Swizzle = default(TextureSwizzle));
