using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D54 RID: 7508
	public class GameCondition_VolcanicWinter : GameCondition
	{
		// Token: 0x17001A00 RID: 6656
		// (get) Token: 0x0600A84C RID: 43084 RVA: 0x00302BED File Offset: 0x00300DED
		public override int TransitionTicks
		{
			get
			{
				return 50000;
			}
		}

		// Token: 0x0600A84D RID: 43085 RVA: 0x00302BF4 File Offset: 0x00300DF4
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.3f);
		}

		// Token: 0x0600A84E RID: 43086 RVA: 0x00302C08 File Offset: 0x00300E08
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.55f, this.VolcanicWinterColors, 1f, 1f));
		}

		// Token: 0x0600A84F RID: 43087 RVA: 0x00302C29 File Offset: 0x00300E29
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, -7f);
		}

		// Token: 0x0600A850 RID: 43088 RVA: 0x00302C3D File Offset: 0x00300E3D
		public override float AnimalDensityFactor(Map map)
		{
			return 1f - GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.5f);
		}

		// Token: 0x0600A851 RID: 43089 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return false;
		}

		// Token: 0x0400746A RID: 29802
		private const float MaxTempOffset = -7f;

		// Token: 0x0400746B RID: 29803
		private const float AnimalDensityImpact = 0.5f;

		// Token: 0x0400746C RID: 29804
		private const float SkyGlow = 0.55f;

		// Token: 0x0400746D RID: 29805
		private const float MaxSkyLerpFactor = 0.3f;

		// Token: 0x0400746E RID: 29806
		private readonly SkyColorSet VolcanicWinterColors = new SkyColorSet(new ColorInt(0, 0, 0).ToColor, Color.white, new Color(0.6f, 0.6f, 0.6f), 0.65f);
	}
}
