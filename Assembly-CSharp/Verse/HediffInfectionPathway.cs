using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D22 RID: 3362
	public class HediffInfectionPathway
	{
		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x060055CE RID: 21966 RVA: 0x001BB530 File Offset: 0x001B9730
		public InfectionPathwayDef PathwayDef
		{
			get
			{
				return this.pathwayDef;
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x060055CF RID: 21967 RVA: 0x001BB538 File Offset: 0x001B9738
		public string Explanation
		{
			get
			{
				return this.explanation;
			}
		}

		// Token: 0x060055D0 RID: 21968 RVA: 0x001BB540 File Offset: 0x001B9740
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.Name == "li")
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "pathwayDef", xmlRoot.FirstChild.Value, null, null, null);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "pathwayDef", xmlRoot.Name, null, null, null);
			if (xmlRoot.HasChildNodes)
			{
				this.explanation = ParseHelper.FromString<string>(xmlRoot.FirstChild.Value);
			}
		}

		// Token: 0x04003E0C RID: 15884
		private InfectionPathwayDef pathwayDef;

		// Token: 0x04003E0D RID: 15885
		[MustTranslate]
		private string explanation;
	}
}
