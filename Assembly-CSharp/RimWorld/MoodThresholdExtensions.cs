using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003071 RID: 12401
	public static class MoodThresholdExtensions
	{
		// Token: 0x06011690 RID: 71312 RVA: 0x00500900 File Offset: 0x004FEB00
		public static Color GetColor(this MoodThreshold m)
		{
			Color c = Color.clear;
			float transparency = 0f;
			switch (m)
			{
			case MoodThreshold.Minor:
				c = ColorLibrary.Gold;
				transparency = 0.9f;
				break;
			case MoodThreshold.Major:
				c = ColorLibrary.Orange;
				transparency = 0.9f;
				break;
			case MoodThreshold.Extreme:
				c = ColorLibrary.RedReadable;
				transparency = 1f * Pulser.PulseBrightness(0.9f, 0.6f) + 0.1f;
				break;
			}
			return c.ToTransparent(transparency);
		}

		// Token: 0x06011691 RID: 71313 RVA: 0x00500974 File Offset: 0x004FEB74
		public static float EdgeExpansion(this MoodThreshold m)
		{
			switch (m)
			{
			case MoodThreshold.None:
				return 0f;
			case MoodThreshold.Minor:
				return 0f;
			case MoodThreshold.Major:
				return 5f;
			case MoodThreshold.Extreme:
				return 6f;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06011692 RID: 71314 RVA: 0x005009AC File Offset: 0x004FEBAC
		public static MoodThreshold CurrentMoodThresholdFor(Pawn pawn)
		{
			if (pawn.mindState == null)
			{
				return MoodThreshold.None;
			}
			if (pawn.mindState.mentalBreaker.CanDoRandomMentalBreaks)
			{
				if (pawn.mindState.mentalBreaker.BreakExtremeIsImminent)
				{
					return MoodThreshold.Extreme;
				}
				if (pawn.mindState.mentalBreaker.BreakMajorIsImminent)
				{
					return MoodThreshold.Major;
				}
				if (pawn.mindState.mentalBreaker.BreakMinorIsImminent)
				{
					return MoodThreshold.Minor;
				}
			}
			return MoodThreshold.None;
		}

		// Token: 0x0400B465 RID: 46181
		private const float Alpha_Off = 0f;

		// Token: 0x0400B466 RID: 46182
		private const float Alpha_Subtle = 0.9f;

		// Token: 0x0400B467 RID: 46183
		private const float Alpha_Intense = 1f;
	}
}
