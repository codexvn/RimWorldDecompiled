using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B35 RID: 6965
	public class XenotypeChance
	{
		// Token: 0x06009B9B RID: 39835 RVA: 0x000024DF File Offset: 0x000006DF
		public XenotypeChance()
		{
		}

		// Token: 0x06009B9C RID: 39836 RVA: 0x002D2407 File Offset: 0x002D0607
		public XenotypeChance(XenotypeDef xenotype, float chance)
		{
			this.xenotype = xenotype;
			this.chance = chance;
		}

		// Token: 0x06009B9D RID: 39837 RVA: 0x002D241D File Offset: 0x002D061D
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "xenotype", xmlRoot.Name, null, null, null);
			this.chance = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04006C10 RID: 27664
		public XenotypeDef xenotype;

		// Token: 0x04006C11 RID: 27665
		public float chance;
	}
}
