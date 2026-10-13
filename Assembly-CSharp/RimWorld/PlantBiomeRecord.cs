using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020019FA RID: 6650
	public class PlantBiomeRecord
	{
		// Token: 0x06009616 RID: 38422 RVA: 0x002BEF86 File Offset: 0x002BD186
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot, null, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04006220 RID: 25120
		public BiomeDef biome;

		// Token: 0x04006221 RID: 25121
		public float commonality;
	}
}
