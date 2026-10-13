using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001825 RID: 6181
	public class ThoughtWorker_StyleDominance : ThoughtWorker_Precept
	{
		// Token: 0x06008F0B RID: 36619 RVA: 0x002A4AD4 File Offset: 0x002A2CD4
		public override string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(p.Ideo.adjective.Named("ADJECTIVE")).CapitalizeFirst();
		}

		// Token: 0x06008F0C RID: 36620 RVA: 0x002A4B0C File Offset: 0x002A2D0C
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.Ideo.adjective.Named("ADJECTIVE"), (from s in p.Ideo.thingStyleCategories
			select s.category.LabelCap.Resolve()).ToCommaList(true, false).Named("STYLES")).CapitalizeFirst();
		}

		// Token: 0x06008F0D RID: 36621 RVA: 0x002A4B84 File Offset: 0x002A2D84
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			int styleDominanceThoughtIndex = p.styleObserver.StyleDominanceThoughtIndex;
			if (styleDominanceThoughtIndex >= 0)
			{
				return ThoughtState.ActiveAtStage(styleDominanceThoughtIndex);
			}
			return false;
		}
	}
}
