namespace UnityEngine;

public struct RenderParams
{
	public Material material { get; set; }

	public MaterialPropertyBlock matProps { get; set; }

	public int layer { get; set; }

	public RenderParams(Material mat)
	{
		material = mat;
		matProps = null;
		layer = 0;
	}
}
