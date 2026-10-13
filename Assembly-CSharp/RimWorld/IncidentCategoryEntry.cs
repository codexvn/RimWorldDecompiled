using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E62 RID: 7778
	public class IncidentCategoryEntry
	{
		// Token: 0x0600ACE1 RID: 44257 RVA: 0x00315983 File Offset: 0x00313B83
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "category", xmlRoot.Name, null, null, null);
			this.weight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040076E6 RID: 30438
		public IncidentCategoryDef category;

		// Token: 0x040076E7 RID: 30439
		public float weight;
	}
}
