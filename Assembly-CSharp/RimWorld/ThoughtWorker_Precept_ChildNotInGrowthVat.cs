using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001831 RID: 6193
	public class ThoughtWorker_Precept_ChildNotInGrowthVat : ThoughtWorker_Precept
	{
		// Token: 0x06008F2B RID: 36651 RVA: 0x002A5110 File Offset: 0x002A3310
		public override string PostProcessLabel(Pawn p, string label)
		{
			int num = Mathf.RoundToInt(this.MoodMultiplier(p));
			if (num <= 1)
			{
				return base.PostProcessLabel(p, label);
			}
			return base.PostProcessLabel(p, label) + " x" + num.ToString();
		}

		// Token: 0x06008F2C RID: 36652 RVA: 0x002A5150 File Offset: 0x002A3350
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.IdeologyActive || !ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (this.ChildrenNotInGrowthVatCount(p) <= 0)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}

		// Token: 0x06008F2D RID: 36653 RVA: 0x002A517C File Offset: 0x002A337C
		private int ChildrenNotInGrowthVatCount(Pawn pawn)
		{
			int num = 0;
			if (pawn.relations.ChildrenCount > 0)
			{
				foreach (Pawn pawn2 in pawn.relations.Children)
				{
					if (!pawn2.Dead && pawn2.MapHeld == pawn.MapHeld && !pawn2.DevelopmentalStage.Adult() && !(pawn2.ParentHolder is Building_GrowthVat))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06008F2E RID: 36654 RVA: 0x002A520C File Offset: 0x002A340C
		public override float MoodMultiplier(Pawn p)
		{
			return (float)Mathf.Min(this.def.stackLimit, this.ChildrenNotInGrowthVatCount(p));
		}
	}
}
