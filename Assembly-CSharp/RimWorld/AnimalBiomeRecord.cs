using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020019FC RID: 6652
	public class AnimalBiomeRecord
	{
		// Token: 0x0600961A RID: 38426 RVA: 0x002BEFD4 File Offset: 0x002BD1D4
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot, null, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04006224 RID: 25124
		public BiomeDef biome;

		// Token: 0x04006225 RID: 25125
		public float commonality;
	}
}
