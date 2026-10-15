using UnityEngine;

namespace Verse;

public class MaterialTextureProperty : MaterialNamedProperty
{
	[NoTranslate]
	public string path;

	public Vector2 scale = Vector2.one;

	public Vector2 offset = Vector2.zero;

	public bool alphaFromGrayscale;
}
