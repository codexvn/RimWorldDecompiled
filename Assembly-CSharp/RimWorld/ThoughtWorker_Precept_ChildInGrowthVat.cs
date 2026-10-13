using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001832 RID: 6194
	public class ThoughtWorker_Precept_ChildInGrowthVat : ThoughtWorker_Precept
	{
		// Token: 0x06008F30 RID: 36656 RVA: 0x002A5228 File Offset: 0x002A3428
		public override string PostProcessLabel(Pawn p, string label)
		{
			int num = Mathf.RoundToInt(this.MoodMultiplier(p));
			if (num <= 1)
			{
				return base.PostProcessLabel(p, label);
			}
			return base.PostProcessLabel(p, label) + " x" + num.ToString();
		}

		// Token: 0x06008F31 RID: 36657 RVA: 0x002A5268 File Offset: 0x002A3468
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.IdeologyActive || !ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (this.ChildrenInGrowthVatCount(p) <= 0)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}

		// Token: 0x06008F32 RID: 36658 RVA: 0x002A5294 File Offset: 0x002A3494
		private int ChildrenInGrowthVatCount(Pawn pawn)
		{
			int num = 0;
			if (pawn.relations.ChildrenCount > 0)
			{
				foreach (Pawn pawn2 in pawn.relations.Children)
				{
					if (pawn2.MapHeld == pawn.MapHeld && !pawn2.DevelopmentalStage.Adult() && pawn2.ParentHolder is Building_GrowthVat)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06008F33 RID: 36659 RVA: 0x002A531C File Offset: 0x002A351C
		public override float MoodMultiplier(Pawn p)
		{
			return (float)Mathf.Min(this.def.stackLimit, this.ChildrenInGrowthVatCount(p));
		}
	}
}
