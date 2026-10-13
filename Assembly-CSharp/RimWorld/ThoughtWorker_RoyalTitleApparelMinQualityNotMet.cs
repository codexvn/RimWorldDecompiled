using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A1 RID: 6305
	public class ThoughtWorker_RoyalTitleApparelMinQualityNotMet : ThoughtWorker
	{
		// Token: 0x06009040 RID: 36928 RVA: 0x002A8CC0 File Offset: 0x002A6EC0
		private RoyalTitle Validate(Pawn p, out QualityCategory minQuality)
		{
			minQuality = QualityCategory.Awful;
			foreach (RoyalTitle royalTitle in p.royalty.AllTitlesInEffectForReading)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				minQuality = royalTitle.def.requiredMinimumApparelQuality;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					QualityCategory qualityCategory;
					if (wornApparel[i].TryGetQuality(out qualityCategory) && qualityCategory < royalTitle.def.requiredMinimumApparelQuality)
					{
						return royalTitle;
					}
				}
			}
			return null;
		}

		// Token: 0x06009041 RID: 36929 RVA: 0x002A8D6C File Offset: 0x002A6F6C
		public override string PostProcessLabel(Pawn p, string label)
		{
			QualityCategory qualityCategory;
			RoyalTitle royalTitle = this.Validate(p, out qualityCategory);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return label.Formatted(royalTitle.Named("TITLE"), p.Named("PAWN"));
		}

		// Token: 0x06009042 RID: 36930 RVA: 0x002A8DB0 File Offset: 0x002A6FB0
		public override string PostProcessDescription(Pawn p, string description)
		{
			QualityCategory cat;
			RoyalTitle royalTitle = this.Validate(p, out cat);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return description.Formatted(royalTitle.Named("TITLE"), cat.GetLabel().Named("QUALITY"), p.Named("PAWN")).CapitalizeFirst();
		}

		// Token: 0x06009043 RID: 36931 RVA: 0x002A8E0C File Offset: 0x002A700C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.royalty == null)
			{
				return false;
			}
			QualityCategory qualityCategory;
			if (this.Validate(p, out qualityCategory) == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(0);
		}
	}
}
