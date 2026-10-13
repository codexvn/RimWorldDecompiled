using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020022EA RID: 8938
	public class Hediff_Addiction : HediffWithComps
	{
		// Token: 0x17001DC1 RID: 7617
		// (get) Token: 0x0600C24E RID: 49742 RVA: 0x00385808 File Offset: 0x00383A08
		public Need_Chemical Need
		{
			get
			{
				if (this.pawn.Dead)
				{
					return null;
				}
				Need need;
				if (this.pawn.needs.TryGetNeed(this.def.chemicalNeed, out need))
				{
					return (Need_Chemical)need;
				}
				return null;
			}
		}

		// Token: 0x17001DC2 RID: 7618
		// (get) Token: 0x0600C24F RID: 49743 RVA: 0x0038584C File Offset: 0x00383A4C
		public ChemicalDef Chemical
		{
			get
			{
				List<ChemicalDef> allDefsListForReading = DefDatabase<ChemicalDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].addictionHediff == this.def)
					{
						return allDefsListForReading[i];
					}
				}
				return null;
			}
		}

		// Token: 0x17001DC3 RID: 7619
		// (get) Token: 0x0600C250 RID: 49744 RVA: 0x00385890 File Offset: 0x00383A90
		public override string LabelInBrackets
		{
			get
			{
				string labelInBrackets = base.LabelInBrackets;
				string text = (1f - this.Severity).ToStringPercent("F0");
				if (this.def.CompProps<HediffCompProperties_SeverityPerDay>() == null)
				{
					return labelInBrackets;
				}
				if (!labelInBrackets.NullOrEmpty())
				{
					return labelInBrackets + " " + text;
				}
				return text;
			}
		}

		// Token: 0x17001DC4 RID: 7620
		// (get) Token: 0x0600C251 RID: 49745 RVA: 0x003858E0 File Offset: 0x00383AE0
		public override string TipStringExtra
		{
			get
			{
				string text = base.TipStringExtra;
				Need_Chemical need = this.Need;
				if (need != null)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "CreatesNeed".Translate() + ": " + need.LabelCap + " (" + need.CurLevelPercentage.ToStringPercent("F0") + ")";
				}
				return text;
			}
		}

		// Token: 0x17001DC5 RID: 7621
		// (get) Token: 0x0600C252 RID: 49746 RVA: 0x00385968 File Offset: 0x00383B68
		public override int CurStageIndex
		{
			get
			{
				Need_Chemical need = this.Need;
				if (need == null || need.CurCategory != DrugDesireCategory.Withdrawal)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x0600C253 RID: 49747 RVA: 0x0038598A File Offset: 0x00383B8A
		public void Notify_NeedCategoryChanged()
		{
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x040084AC RID: 33964
		private const int DefaultStageIndex = 0;

		// Token: 0x040084AD RID: 33965
		private const int WithdrawalStageIndex = 1;
	}
}
