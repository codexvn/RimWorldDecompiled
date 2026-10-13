using System;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B32 RID: 11058
	public static class PlantFallColors
	{
		// Token: 0x0600F754 RID: 63316 RVA: 0x0047E0A8 File Offset: 0x0047C2A8
		public static float GetFallColorFactor(float latitude, int dayOfYear)
		{
			float a = GenCelestial.AverageGlow(latitude, dayOfYear);
			float b = GenCelestial.AverageGlow(latitude, dayOfYear + 1);
			float x = Mathf.LerpUnclamped(a, b, PlantFallColors.FallSlopeComponent);
			return GenMath.LerpDoubleClamped(PlantFallColors.FallColorBegin, PlantFallColors.FallColorEnd, 0f, 1f, x);
		}

		// Token: 0x0600F755 RID: 63317 RVA: 0x0047E0EC File Offset: 0x0047C2EC
		public static void SetFallShaderGlobals(Map map)
		{
			if (PlantFallColors.FallIntensityOverride)
			{
				Shader.SetGlobalFloat(ShaderPropertyIDs.FallIntensity, PlantFallColors.FallIntensity);
			}
			else
			{
				Vector2 vector = Find.WorldGrid.LongLatOf(map.Tile);
				Shader.SetGlobalFloat(ShaderPropertyIDs.FallIntensity, PlantFallColors.GetFallColorFactor(vector.y, GenLocalDate.DayOfYear(map)));
			}
			Shader.SetGlobalInt(PlantFallColors.Controls, PlantFallColors.FallGlobalControls ? 1 : 0);
			if (PlantFallColors.FallGlobalControls)
			{
				Shader.SetGlobalVector(PlantFallColors.FallSrc, new Vector3(PlantFallColors.FallSrcR, PlantFallColors.FallSrcG, PlantFallColors.FallSrcB));
				Shader.SetGlobalVector(PlantFallColors.FallDst, new Vector3(PlantFallColors.FallDstR, PlantFallColors.FallDstG, PlantFallColors.FallDstB));
				Shader.SetGlobalVector(PlantFallColors.FallRange, new Vector3(PlantFallColors.FallRangeBegin, PlantFallColors.FallRangeEnd));
			}
		}

		// Token: 0x0400A2BD RID: 41661
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallColorBegin = 0.55f;

		// Token: 0x0400A2BE RID: 41662
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallColorEnd = 0.45f;

		// Token: 0x0400A2BF RID: 41663
		[TweakValue("Graphics", 0f, 30f)]
		private static float FallSlopeComponent = 15f;

		// Token: 0x0400A2C0 RID: 41664
		[TweakValue("Graphics", 0f, 100f)]
		private static bool FallIntensityOverride = false;

		// Token: 0x0400A2C1 RID: 41665
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallIntensity = 0f;

		// Token: 0x0400A2C2 RID: 41666
		[TweakValue("Graphics", 0f, 100f)]
		private static bool FallGlobalControls = false;

		// Token: 0x0400A2C3 RID: 41667
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcR = 0.3803f;

		// Token: 0x0400A2C4 RID: 41668
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcG = 0.4352f;

		// Token: 0x0400A2C5 RID: 41669
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcB = 0.1451f;

		// Token: 0x0400A2C6 RID: 41670
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstR = 0.4392f;

		// Token: 0x0400A2C7 RID: 41671
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstG = 0.3254f;

		// Token: 0x0400A2C8 RID: 41672
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstB = 0.1765f;

		// Token: 0x0400A2C9 RID: 41673
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallRangeBegin = 0.02f;

		// Token: 0x0400A2CA RID: 41674
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallRangeEnd = 0.1f;

		// Token: 0x0400A2CB RID: 41675
		private static readonly int FallSrc = Shader.PropertyToID("_FallSrc");

		// Token: 0x0400A2CC RID: 41676
		private static readonly int FallDst = Shader.PropertyToID("_FallDst");

		// Token: 0x0400A2CD RID: 41677
		private static readonly int FallRange = Shader.PropertyToID("_FallRange");

		// Token: 0x0400A2CE RID: 41678
		private static readonly int Controls = Shader.PropertyToID("_FallGlobalControls");
	}
}
