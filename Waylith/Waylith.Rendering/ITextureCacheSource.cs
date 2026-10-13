using System;

namespace Waylith.Rendering;

public interface ITextureCacheSource : IDisposable
{
	int Width { get; }

	int Height { get; }

	int MipCount { get; }

	LoadedTextureFormat Format { get; }

	bool Opaque { get; }

	TextureSwizzle Swizzle { get; }

	int LevelLength(int level);

	void ReadLevel(int level, Span<byte> destination);

	bool TryReadAll(out LoadedTextureData loadedTexture);
}
