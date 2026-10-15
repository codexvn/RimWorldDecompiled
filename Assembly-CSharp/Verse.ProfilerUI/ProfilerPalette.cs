using UnityEngine;
using Waylith.Profiling;

namespace Verse.ProfilerUI;

internal static class ProfilerPalette
{
	private static readonly Color[] BakedColors = new Color[7]
	{
		new Color32(90, 120, 200, byte.MaxValue),
		new Color32(96, 160, 96, byte.MaxValue),
		new Color32(150, 100, 190, byte.MaxValue),
		new Color32(84, 160, 176, byte.MaxValue),
		new Color32(70, 140, 150, byte.MaxValue),
		new Color32(190, 160, 80, byte.MaxValue),
		new Color32(96, 96, 104, byte.MaxValue)
	};

	private const int GeneratedHues = 6;

	private const int HashedColorCount = 13;

	public const int GcColorIndex = 13;

	public const int ColorCount = 14;

	public static readonly Color UnprofiledColor = new Color32(110, 110, 110, byte.MaxValue);

	public static readonly Color GcPauseColor = new Color32(224, 112, 95, byte.MaxValue);

	public static readonly Color ZoneLabelColor = ProfilerGui.Hex(4043309055u);

	public static int ColorIndexFor(ProfilerLabelId labelId)
	{
		if (FrameProfiler.IsPauseLabel(labelId))
		{
			return 13;
		}
		return (int)((uint)(labelId.Value * 1103515245 + 12345) % 13u);
	}

	public static Color ColorAt(int colorIndex)
	{
		if (colorIndex < BakedColors.Length)
		{
			return BakedColors[colorIndex];
		}
		if (colorIndex == 13)
		{
			return GcPauseColor;
		}
		return Color.HSVToRGB((float)(colorIndex - BakedColors.Length) / 6f, 0.35f, 0.5f);
	}

	public static Color ColorFor(ProfilerLabelId labelId)
	{
		if (!labelId.IsValid)
		{
			return UnprofiledColor;
		}
		return ColorAt(ColorIndexFor(labelId));
	}
}
