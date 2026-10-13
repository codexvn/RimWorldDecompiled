using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D47 RID: 7495
	public class GameCondition_NoSunlight : GameCondition
	{
		// Token: 0x170019F1 RID: 6641
		// (get) Token: 0x0600A7EE RID: 42990 RVA: 0x00222D4F File Offset: 0x00220F4F
		public override int TransitionTicks
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x0600A7EF RID: 42991 RVA: 0x003000A3 File Offset: 0x002FE2A3
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x0600A7F0 RID: 42992 RVA: 0x003016FF File Offset: 0x002FF8FF
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0f, GameCondition_NoSunlight.EclipseSkyColors, 1f, 0f));
		}

		// Token: 0x04007442 RID: 29762
		public static readonly SkyColorSet EclipseSkyColors = new SkyColorSet(new Color(0.482f, 0.603f, 0.682f), Color.white, new Color(0.6f, 0.6f, 0.6f), 1f);
	}
}
