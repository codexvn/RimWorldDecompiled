using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001925 RID: 6437
	public class CompProperties_Book : CompProperties_Readable
	{
		// Token: 0x0600926B RID: 37483 RVA: 0x002B23AF File Offset: 0x002B05AF
		public CompProperties_Book()
		{
			this.compClass = typeof(CompBook);
		}

		// Token: 0x04005D72 RID: 23922
		public RulePackDef nameMaker;

		// Token: 0x04005D73 RID: 23923
		public RulePackDef descriptionMaker;

		// Token: 0x04005D74 RID: 23924
		public float pickWeight = 1f;

		// Token: 0x04005D75 RID: 23925
		public GraphicData openGraphic;

		// Token: 0x04005D76 RID: 23926
		public GraphicData verticalGraphic;

		// Token: 0x04005D77 RID: 23927
		public FloatRange ageYearsRange = new FloatRange(5f, 10f);

		// Token: 0x04005D78 RID: 23928
		public float questChance;
	}
}
