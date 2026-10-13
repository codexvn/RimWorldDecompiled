using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020019FB RID: 6651
	public class BiomeAnimalRecord
	{
		// Token: 0x06009618 RID: 38424 RVA: 0x002BEFAD File Offset: 0x002BD1AD
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "animal", xmlRoot, null, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04006222 RID: 25122
		public PawnKindDef animal;

		// Token: 0x04006223 RID: 25123
		public float commonality;
	}
}
