using System.Collections.Generic;
using System.Linq;

namespace PsdSharp;

public class BlendModeKey
{
	public readonly string Key;

	public readonly string Description;

	public static BlendModeKey Passthrough = new BlendModeKey("pass", "Passthrough");

	public static BlendModeKey Normal = new BlendModeKey("norm", "Normal");

	public static BlendModeKey Dissolve = new BlendModeKey("diss", "Dissolve");

	public static BlendModeKey Darken = new BlendModeKey("dark", "Darken");

	public static BlendModeKey Multiply = new BlendModeKey("mul ", "Multiply");

	public static BlendModeKey ColorBurn = new BlendModeKey("idiv", "ColorBurn");

	public static BlendModeKey LinearBurn = new BlendModeKey("lbrn", "LinearBurn");

	public static BlendModeKey DarkerColor = new BlendModeKey("dkCl", "DarkerColor");

	public static BlendModeKey Lighten = new BlendModeKey("lite", "Lighten");

	public static BlendModeKey Screen = new BlendModeKey("scrn", "Screen");

	public static BlendModeKey ColorDodge = new BlendModeKey("div ", "ColorDodge");

	public static BlendModeKey LinearDodge = new BlendModeKey("lddg", "LinearDodge");

	public static BlendModeKey LighterColor = new BlendModeKey("lgCl", "LighterColor");

	public static BlendModeKey Overlay = new BlendModeKey("over", "Overlay");

	public static BlendModeKey SoftLight = new BlendModeKey("sLit", "SoftLight");

	public static BlendModeKey HardLight = new BlendModeKey("hLit", "HardLight");

	public static BlendModeKey VividLight = new BlendModeKey("vLit", "VividLight");

	public static BlendModeKey LinearLight = new BlendModeKey("lLit", "LinearLight");

	public static BlendModeKey PinLight = new BlendModeKey("pLit", "PinLight");

	public static BlendModeKey HardMix = new BlendModeKey("hMix", "HardMix");

	public static BlendModeKey Difference = new BlendModeKey("diff", "Difference");

	public static BlendModeKey Exclusion = new BlendModeKey("smud", "Exclusion");

	public static BlendModeKey Subtract = new BlendModeKey("fsub", "Subtract");

	public static BlendModeKey Divide = new BlendModeKey("fdiv", "Divide");

	public static BlendModeKey Hue = new BlendModeKey("hue ", "Hue");

	public static BlendModeKey Saturation = new BlendModeKey("sat ", "Saturation");

	public static BlendModeKey Color = new BlendModeKey("colr", "Color");

	public static BlendModeKey Luminosity = new BlendModeKey("lum ", "Luminosity");

	public static BlendModeKey[] All = new BlendModeKey[28]
	{
		Passthrough, Normal, Dissolve, Darken, Multiply, ColorBurn, LinearBurn, DarkerColor, Lighten, Screen,
		ColorDodge, LinearDodge, LighterColor, Overlay, SoftLight, HardLight, VividLight, LinearLight, PinLight, HardMix,
		Difference, Exclusion, Subtract, Divide, Hue, Saturation, Color, Luminosity
	};

	public static Dictionary<string, BlendModeKey> ByKey = All.ToDictionary((BlendModeKey x) => x.Key);

	private BlendModeKey(string key, string description)
	{
		Key = key;
		Description = description;
	}
}
