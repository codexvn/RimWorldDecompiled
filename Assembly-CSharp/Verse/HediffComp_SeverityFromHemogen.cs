using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200070A RID: 1802
	public class HediffComp_SeverityFromHemogen : HediffComp
	{
		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x06002C0B RID: 11275 RVA: 0x000E5030 File Offset: 0x000E3230
		public HediffCompProperties_SeverityFromHemogen Props
		{
			get
			{
				return (HediffCompProperties_SeverityFromHemogen)this.props;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06002C0C RID: 11276 RVA: 0x000E503D File Offset: 0x000E323D
		public override bool CompShouldRemove
		{
			get
			{
				Pawn_GeneTracker genes = base.Pawn.genes;
				return ((genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null) == null;
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06002C0D RID: 11277 RVA: 0x000E505C File Offset: 0x000E325C
		private Gene_Hemogen Hemogen
		{
			get
			{
				Gene_Hemogen result;
				if ((result = this.cachedHemogenGene) == null)
				{
					result = (this.cachedHemogenGene = base.Pawn.genes.GetFirstGeneOfType<Gene_Hemogen>());
				}
				return result;
			}
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x000E508C File Offset: 0x000E328C
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.Hemogen != null)
			{
				severityAdjustment += ((this.Hemogen.Value > 0f) ? this.Props.severityPerHourHemogen : this.Props.severityPerHourEmpty) / 2500f;
			}
		}

		// Token: 0x04002367 RID: 9063
		private Gene_Hemogen cachedHemogenGene;
	}
}
