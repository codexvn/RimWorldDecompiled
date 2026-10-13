using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A51 RID: 6737
	public class MTBByBiome
	{
		// Token: 0x06009772 RID: 38770 RVA: 0x002C3760 File Offset: 0x002C1960
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured MTBByBiome: " + xmlRoot.OuterXml);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot.Name, null, null, null);
			this.mtbDays = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04006486 RID: 25734
		public BiomeDef biome;

		// Token: 0x04006487 RID: 25735
		public float mtbDays;
	}
}
