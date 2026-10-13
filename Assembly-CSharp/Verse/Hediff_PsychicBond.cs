using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000760 RID: 1888
	public class Hediff_PsychicBond : HediffWithTarget
	{
		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06002E57 RID: 11863 RVA: 0x000EE9BB File Offset: 0x000ECBBB
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

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06002E58 RID: 11864 RVA: 0x000EE9E4 File Offset: 0x000ECBE4
		public override bool ShouldRemove
		{
			get
			{
				return base.ShouldRemove || this.pawn.Dead;
			}
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x000EE9FC File Offset: 0x000ECBFC
		public override void PostRemoved()
		{
			Pawn_GeneTracker genes = this.pawn.genes;
			Gene_PsychicBonding gene_PsychicBonding = (genes != null) ? genes.GetFirstGeneOfType<Gene_PsychicBonding>() : null;
			if (gene_PsychicBonding != null)
			{
				gene_PsychicBonding.RemoveBond();
				return;
			}
			if (this.target != null)
			{
				Pawn pawn = this.target as Pawn;
				if (pawn != null)
				{
					Pawn_GeneTracker genes2 = pawn.genes;
					if (genes2 == null)
					{
						return;
					}
					Gene_PsychicBonding firstGeneOfType = genes2.GetFirstGeneOfType<Gene_PsychicBonding>();
					if (firstGeneOfType == null)
					{
						return;
					}
					firstGeneOfType.RemoveBond();
				}
			}
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x000EEA5C File Offset: 0x000ECC5C
		public override void PostTickInterval(int delta)
		{
			base.PostTickInterval(delta);
			if (this.pawn.IsHashIntervalTick(65, delta))
			{
				this.Severity = (ThoughtWorker_PsychicBondProximity.NearPsychicBondedPerson(this.pawn, this) ? 0.5f : 1.5f);
			}
		}

		// Token: 0x04002462 RID: 9314
		private const int HediffCheckInterval = 65;
	}
}
