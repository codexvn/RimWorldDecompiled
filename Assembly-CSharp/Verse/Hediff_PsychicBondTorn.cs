using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000761 RID: 1889
	public class Hediff_PsychicBondTorn : HediffWithTarget
	{
		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06002E5C RID: 11868 RVA: 0x000EE9BB File Offset: 0x000ECBBB
		public override string LabelBase
		{
			get
			{
				string labelBase = base.LabelBase;
				string str = " (";
				Thing target = this.target;
				return labelBase + str + ((target != null) ? target.LabelShortCap : null) + ")";
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06002E5D RID: 11869 RVA: 0x00002501 File Offset: 0x00000701
		public override bool Visible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x000EEA9D File Offset: 0x000ECC9D
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.creationTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x000EEAC0 File Offset: 0x000ECCC0
		public override void Notify_Resurrected()
		{
			bool flag = false;
			Pawn pawn = this.target as Pawn;
			if (pawn != null && !pawn.Dead && !pawn.Destroyed && this.creationTick >= 0 && Find.TickManager.TicksGame - this.creationTick <= ThoughtDefOf.PsychicBondTorn.DurationTicks)
			{
				Pawn_GeneTracker genes = this.pawn.genes;
				Gene_PsychicBonding gene_PsychicBonding = (genes != null) ? genes.GetFirstGeneOfType<Gene_PsychicBonding>() : null;
				if (gene_PsychicBonding != null)
				{
					gene_PsychicBonding.BondTo(pawn);
				}
				else
				{
					Pawn_GeneTracker genes2 = pawn.genes;
					if (genes2 != null)
					{
						Gene_PsychicBonding firstGeneOfType = genes2.GetFirstGeneOfType<Gene_PsychicBonding>();
						if (firstGeneOfType != null)
						{
							firstGeneOfType.BondTo(this.pawn);
						}
					}
				}
				flag = true;
			}
			if (!flag)
			{
				this.pawn.health.RemoveHediff(this);
			}
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x000EEB70 File Offset: 0x000ECD70
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.creationTick, "creationTick", -1, false);
		}

		// Token: 0x04002463 RID: 9315
		private int creationTick = -1;
	}
}
