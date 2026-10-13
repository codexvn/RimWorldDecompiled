using System.Numerics;

namespace Waylith.Rendering;

public readonly record struct MaterialTextureTransform(Vector2 Offset, Vector2 Scale)
{
	public static MaterialTextureTransform Identity => new MaterialTextureTransform(Vector2.Zero, Vector2.One);
}
