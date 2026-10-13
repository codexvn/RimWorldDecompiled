using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A27 RID: 6695
	public class MemeWeight
	{
		// Token: 0x060096E6 RID: 38630 RVA: 0x002C1BC6 File Offset: 0x002BFDC6
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "meme", xmlRoot.Name, null, null, null);
			this.selectionWeight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0400636C RID: 25452
		public MemeDef meme;

		// Token: 0x0400636D RID: 25453
		public float selectionWeight;
	}
}
