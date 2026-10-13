using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020019F9 RID: 6649
	public class BiomePlantRecord
	{
		// Token: 0x06009614 RID: 38420 RVA: 0x002BEF5F File Offset: 0x002BD15F
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "plant", xmlRoot, null, null, null);
			this.commonality = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0400621E RID: 25118
		public ThingDef plant;

		// Token: 0x0400621F RID: 25119
		public float commonality;
	}
}
