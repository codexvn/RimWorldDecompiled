using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018FB RID: 6395
	public class GeneTemplateDef : Def
	{
		// Token: 0x06009158 RID: 37208 RVA: 0x002AC56E File Offset: 0x002AA76E
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(Gene).IsAssignableFrom(this.geneClass))
			{
				yield return "geneClass is not Gene or child thereof.";
			}
			yield break;
			yield break;
		}

		// Token: 0x04005C7E RID: 23678
		public Type geneClass = typeof(Gene);

		// Token: 0x04005C7F RID: 23679
		public int biostatCpx;

		// Token: 0x04005C80 RID: 23680
		public int biostatMet;

		// Token: 0x04005C81 RID: 23681
		public int aptitudeOffset;

		// Token: 0x04005C82 RID: 23682
		public float addictionChanceFactor = 1f;

		// Token: 0x04005C83 RID: 23683
		public PassionMod.PassionModType passionModType;

		// Token: 0x04005C84 RID: 23684
		public GeneTemplateDef.GeneTemplateType geneTemplateType;

		// Token: 0x04005C85 RID: 23685
		public float minAgeActive;

		// Token: 0x04005C86 RID: 23686
		public GeneCategoryDef displayCategory;

		// Token: 0x04005C87 RID: 23687
		public int displayOrderOffset;

		// Token: 0x04005C88 RID: 23688
		public float selectionWeight = 1f;

		// Token: 0x04005C89 RID: 23689
		public List<GeneTemplateDef.ChemicalBiostatOverride> chemicalBiostatOverrides;

		// Token: 0x04005C8A RID: 23690
		[MustTranslate]
		public string labelShortAdj;

		// Token: 0x04005C8B RID: 23691
		[NoTranslate]
		public string iconPath;

		// Token: 0x04005C8C RID: 23692
		[NoTranslate]
		public string exclusionTagPrefix;

		// Token: 0x020018FC RID: 6396
		public enum GeneTemplateType
		{
			// Token: 0x04005C8E RID: 23694
			Skill,
			// Token: 0x04005C8F RID: 23695
			Chemical
		}

		// Token: 0x020018FD RID: 6397
		public class ChemicalBiostatOverride
		{
			// Token: 0x04005C90 RID: 23696
			public ChemicalDef chemical;

			// Token: 0x04005C91 RID: 23697
			public int? biostatCpx;

			// Token: 0x04005C92 RID: 23698
			public int? biostatMet;

			// Token: 0x04005C93 RID: 23699
			public int? biostatArc;
		}
	}
}
