using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200248C RID: 9356
	public class Need_Play : Need
	{
		// Token: 0x17001EE1 RID: 7905
		// (get) Token: 0x0600CAEC RID: 51948 RVA: 0x003B723C File Offset: 0x003B543C
		public bool IsLow
		{
			get
			{
				return base.CurLevelPercentage <= 0.15f;
			}
		}

		// Token: 0x17001EE2 RID: 7906
		// (get) Token: 0x0600CAED RID: 51949 RVA: 0x003B7250 File Offset: 0x003B5450
		public PlayCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return PlayCategory.Empty;
				}
				if (this.CurLevel < 0.15f)
				{
					return PlayCategory.VeryLow;
				}
				if (this.CurLevel < 0.3f)
				{
					return PlayCategory.Low;
				}
				if (this.CurLevel < 0.7f)
				{
					return PlayCategory.Satisfied;
				}
				if (this.CurLevel < 0.85f)
				{
					return PlayCategory.High;
				}
				return PlayCategory.Extreme;
			}
		}

		// Token: 0x0600CAEE RID: 51950 RVA: 0x003B64D5 File Offset: 0x003B46D5
		public Need_Play(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>
			{
				0.15f,
				0.3f,
				0.7f,
				0.85f
			};
		}

		// Token: 0x0600CAEF RID: 51951 RVA: 0x003B72A9 File Offset: 0x003B54A9
		public void Play(float amount)
		{
			base.CurLevelPercentage += amount;
			base.CurLevelPercentage = Mathf.Clamp01(base.CurLevelPercentage);
		}

		// Token: 0x0600CAF0 RID: 51952 RVA: 0x003B72CA File Offset: 0x003B54CA
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= 0.00025f;
			}
		}

		// Token: 0x04008A62 RID: 35426
		public const float BaseFallPerInterval = 0.00025f;

		// Token: 0x04008A63 RID: 35427
		public const float ThresholdEmpty = 0.01f;

		// Token: 0x04008A64 RID: 35428
		public const float ThresholdLow = 0.15f;

		// Token: 0x04008A65 RID: 35429
		public const float ThresholdSatisfied = 0.3f;

		// Token: 0x04008A66 RID: 35430
		public const float ThresholdHigh = 0.7f;

		// Token: 0x04008A67 RID: 35431
		public const float ThresholdVeryHigh = 0.85f;
	}
}
