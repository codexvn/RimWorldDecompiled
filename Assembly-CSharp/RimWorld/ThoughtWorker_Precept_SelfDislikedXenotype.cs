using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200183A RID: 6202
	public class ThoughtWorker_Precept_SelfDislikedXenotype : ThoughtWorker_Precept
	{
		// Token: 0x06008F43 RID: 36675 RVA: 0x002A56D0 File Offset: 0x002A38D0
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.genes.XenotypeLabel.Named("XENOTYPENAME"));
		}

		// Token: 0x06008F44 RID: 36676 RVA: 0x002A56F2 File Offset: 0x002A38F2
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || p.genes == null)
			{
				return ThoughtState.Inactive;
			}
			return !p.Ideo.IsPreferredXenotype(p);
		}
	}
}
