using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E5 RID: 6373
	public class ThoughtWorker_DeathrestChamber : ThoughtWorker
	{
		// Token: 0x06009111 RID: 37137 RVA: 0x002AAD80 File Offset: 0x002A8F80
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			Pawn_GeneTracker genes = p.genes;
			Gene_Deathrest gene_Deathrest = (genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null;
			if (gene_Deathrest == null || gene_Deathrest.chamberThoughtIndex < 0)
			{
				return false;
			}
			if (p.Deathresting)
			{
				return false;
			}
			int chamberThoughtIndex = gene_Deathrest.chamberThoughtIndex;
			if (chamberThoughtIndex == 2 || chamberThoughtIndex == 3)
			{
				return false;
			}
			return ThoughtState.ActiveAtStage(chamberThoughtIndex);
		}

		// Token: 0x04005C6C RID: 23660
		private const int DullIndex = 2;

		// Token: 0x04005C6D RID: 23661
		private const int MediocreIndex = 3;
	}
}
