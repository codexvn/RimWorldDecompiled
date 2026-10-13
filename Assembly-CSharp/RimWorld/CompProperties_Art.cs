using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001952 RID: 6482
	public class CompProperties_Art : CompProperties
	{
		// Token: 0x0600936A RID: 37738 RVA: 0x002B5AF0 File Offset: 0x002B3CF0
		public CompProperties_Art()
		{
			this.compClass = typeof(CompArt);
		}

		// Token: 0x04005ED2 RID: 24274
		public RulePackDef nameMaker;

		// Token: 0x04005ED3 RID: 24275
		public RulePackDef descriptionMaker;

		// Token: 0x04005ED4 RID: 24276
		public QualityCategory minQualityForArtistic;

		// Token: 0x04005ED5 RID: 24277
		public bool mustBeFullGrave;

		// Token: 0x04005ED6 RID: 24278
		public bool canBeEnjoyedAsArt;
	}
}
