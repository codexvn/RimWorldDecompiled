namespace Waylith.Rendering;

public sealed class SceneGrabPoint
{
	public string Name { get; }

	public int Queue { get; internal set; }

	public string TexturePropertyName { get; }

	public int TexturePropertyId { get; }

	public Texture2D Texture { get; }

	internal SceneGrabPoint(string name, int queue)
	{
		Name = name;
		Queue = queue;
		TexturePropertyName = "_Grab" + name;
		TexturePropertyId = ShaderPropertyId.GetOrCreate(TexturePropertyName);
		Texture = new Texture2D(1, 1)
		{
			Name = "SceneGrab_" + name,
			filterMode = FilterMode.Bilinear,
			wrapMode = TextureWrapMode.Clamp
		};
	}
}
