namespace UnityEngine;

public class Sprite : Object
{
	public Texture2D texture { get; internal set; }

	public Texture2D associatedAlphaSplitTexture => null;

	public Rect rect { get; internal set; }

	public Rect textureRect { get; internal set; }

	public Vector2 textureRectOffset { get; internal set; }

	public Vector2 pivot { get; internal set; }

	public float pixelsPerUnit { get; internal set; } = 100f;

	public Vector4 border { get; internal set; }

	public bool packed => false;

	public SpritePackingMode packingMode => SpritePackingMode.Tight;

	public float spriteAtlasTextureScale => 1f;

	public Bounds bounds
	{
		get
		{
			Vector3 size = new Vector3(rect.width / pixelsPerUnit, rect.height / pixelsPerUnit, 0f);
			return new Bounds(new Vector3((0.5f - pivot.x) * size.x, (0.5f - pivot.y) * size.y, 0f), size);
		}
	}

	internal Sprite()
	{
	}

	public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot)
	{
		return Create(texture, rect, pivot, 100f, 0u, SpriteMeshType.FullRect, Vector4.zero);
	}

	public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit)
	{
		return Create(texture, rect, pivot, pixelsPerUnit, 0u, SpriteMeshType.FullRect, Vector4.zero);
	}

	public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude)
	{
		return Create(texture, rect, pivot, pixelsPerUnit, extrude, SpriteMeshType.FullRect, Vector4.zero);
	}

	public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, SpriteMeshType meshType)
	{
		return Create(texture, rect, pivot, pixelsPerUnit, extrude, meshType, Vector4.zero);
	}

	public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, SpriteMeshType meshType, Vector4 border)
	{
		return Create(texture, rect, pivot, pixelsPerUnit, extrude, meshType, border, generateFallbackPhysicsShape: false);
	}

	public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, SpriteMeshType meshType, Vector4 border, bool generateFallbackPhysicsShape)
	{
		return new Sprite
		{
			name = texture?.name,
			texture = texture,
			rect = rect,
			textureRect = rect,
			pivot = pivot,
			pixelsPerUnit = ((pixelsPerUnit <= 0f) ? 100f : pixelsPerUnit),
			border = border
		};
	}
}
