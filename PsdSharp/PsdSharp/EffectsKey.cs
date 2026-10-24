namespace PsdSharp;

public class EffectsKey
{
	public readonly string Key;

	public readonly string Description;

	public static EffectsKey CommonState = new EffectsKey("cmnS", "CommonState");

	public static EffectsKey DropShadow = new EffectsKey("dsdw", "DropShadow");

	public static EffectsKey InnerShadow = new EffectsKey("isdw", "InnerShadow");

	public static EffectsKey OuterGlow = new EffectsKey("oglw", "OuterGlow");

	public static EffectsKey InnerGlow = new EffectsKey("iglw", "InnerGlow");

	public static EffectsKey Bevel = new EffectsKey("bevl", "Bevel");

	public static EffectsKey SolidFill = new EffectsKey("sofi", "SolidFill");

	private EffectsKey(string key, string description)
	{
		Key = key;
		Description = description;
	}
}
