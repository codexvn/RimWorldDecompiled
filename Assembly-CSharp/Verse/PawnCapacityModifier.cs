using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200028C RID: 652
	public class PawnCapacityModifier
	{
		// Token: 0x170002BC RID: 700
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x0004D033 File Offset: 0x0004B233
		public bool SetMaxDefined
		{
			get
			{
				return !Mathf.Approximately(this.setMax, 999f) || (this.setMaxCurveOverride != null && this.setMaxCurveEvaluateStat != null);
			}
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0004D05C File Offset: 0x0004B25C
		public float EvaluateSetMax(Pawn pawn)
		{
			if (this.setMaxCurveOverride == null || this.setMaxCurveEvaluateStat == null)
			{
				return this.setMax;
			}
			return this.setMaxCurveOverride.Evaluate(pawn.GetStatValue(this.setMaxCurveEvaluateStat, true, -1));
		}

		// Token: 0x04000D3D RID: 3389
		public PawnCapacityDef capacity;

		// Token: 0x04000D3E RID: 3390
		public float offset;

		// Token: 0x04000D3F RID: 3391
		public float setMax = 999f;

		// Token: 0x04000D40 RID: 3392
		public float postFactor = 1f;

		// Token: 0x04000D41 RID: 3393
		public StatDef statFactorMod;

		// Token: 0x04000D42 RID: 3394
		public SimpleCurve setMaxCurveOverride;

		// Token: 0x04000D43 RID: 3395
		public StatDef setMaxCurveEvaluateStat;
	}
}
