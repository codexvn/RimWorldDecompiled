namespace UnityEngine;

public struct UIVertex
{
	public Vector3 position;

	public Vector3 normal;

	public Vector4 tangent;

	public Color32 color;

	public Vector4 uv0;

	public Vector4 uv1;

	public Vector4 uv2;

	public Vector4 uv3;

	public static UIVertex simpleVert = new UIVertex
	{
		position = Vector3.zero,
		normal = Vector3.back,
		tangent = new Vector4(1f, 0f, 0f, -1f),
		color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
	};
}
