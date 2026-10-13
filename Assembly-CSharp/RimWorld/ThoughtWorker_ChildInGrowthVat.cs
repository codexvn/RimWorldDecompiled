using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D4 RID: 6356
	public class ThoughtWorker_ChildInGrowthVat : ThoughtWorker
	{
		// Token: 0x060090CF RID: 37071 RVA: 0x002AA4F8 File Offset: 0x002A86F8
		public override string PostProcessLabel(Pawn p, string label)
		{
			int num = Mathf.RoundToInt(this.MoodMultiplier(p));
			if (num <= 1)
			{
				return base.PostProcessLabel(p, label);
			}
			return base.PostProcessLabel(p, label) + " x" + num.ToString();
		}

		// Token: 0x060090D0 RID: 37072 RVA: 0x002AA538 File Offset: 0x002A8738
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (this.ChildrenInGrowthVatCount(p) <= 0)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}

		// Token: 0x060090D1 RID: 37073 RVA: 0x002AA55C File Offset: 0x002A875C
		private int ChildrenInGrowthVatCount(Pawn pawn)
		{
			int num = 0;
			if (pawn.relations.ChildrenCount > 0)
			{
				foreach (Pawn pawn2 in pawn.relations.Children)
				{
					if (!pawn2.DevelopmentalStage.Adult() && pawn2.ParentHolder != null && pawn2.ParentHolder is Building_GrowthVat)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x060090D2 RID: 37074 RVA: 0x002AA5E0 File Offset: 0x002A87E0
		public override float MoodMultiplier(Pawn p)
		{
			return (float)Mathf.Min(this.def.stackLimit, this.ChildrenInGrowthVatCount(p));
		}
	}
}
