using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002493 RID: 9363
	public abstract class Need_Seeker : Need
	{
		// Token: 0x17001EED RID: 7917
		// (get) Token: 0x0600CB08 RID: 51976 RVA: 0x003B7ACC File Offset: 0x003B5CCC
		public override int GUIChangeArrow
		{
			get
			{
				if (!this.pawn.Awake())
				{
					return 0;
				}
				float curInstantLevelPercentage = base.CurInstantLevelPercentage;
				if (curInstantLevelPercentage > base.CurLevelPercentage + 0.05f)
				{
					return 1;
				}
				if (curInstantLevelPercentage < base.CurLevelPercentage - 0.05f)
				{
					return -1;
				}
				return 0;
			}
		}

		// Token: 0x0600CB09 RID: 51977 RVA: 0x003B5049 File Offset: 0x003B3249
		public Need_Seeker(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600CB0A RID: 51978 RVA: 0x003B7B14 File Offset: 0x003B5D14
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				float curInstantLevel = this.CurInstantLevel;
				if (curInstantLevel > this.CurLevel)
				{
					this.CurLevel += this.def.seekerRisePerHour * 0.06f;
					this.CurLevel = Mathf.Min(this.CurLevel, curInstantLevel);
				}
				if (curInstantLevel < this.CurLevel)
				{
					this.CurLevel -= this.def.seekerFallPerHour * 0.06f;
					this.CurLevel = Mathf.Max(this.CurLevel, curInstantLevel);
				}
			}
		}

		// Token: 0x04008A84 RID: 35460
		private const float GUIArrowTolerance = 0.05f;
	}
}
