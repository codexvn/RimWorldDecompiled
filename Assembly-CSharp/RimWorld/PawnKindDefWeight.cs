using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A0C RID: 6668
	public class PawnKindDefWeight
	{
		// Token: 0x06009665 RID: 38501 RVA: 0x002C03A6 File Offset: 0x002BE5A6
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "kindDef", "weight");
		}

		// Token: 0x040062AC RID: 25260
		public PawnKindDef kindDef;

		// Token: 0x040062AD RID: 25261
		public float weight = 1f;
	}
}
