namespace UnityEngine;

public class Light : Behaviour
{
	public LightType type { get; set; }

	public Color color { get; set; } = Color.white;

	public float intensity { get; set; } = 1f;

	public float range { get; set; } = 10f;

	public float shadowStrength { get; set; } = 1f;
}
