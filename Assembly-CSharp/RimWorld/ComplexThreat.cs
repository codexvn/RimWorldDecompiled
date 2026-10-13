using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A63 RID: 6755
	public class ComplexThreat
	{
		// Token: 0x060097C4 RID: 38852 RVA: 0x002C4F7A File Offset: 0x002C317A
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", "selectionWeight");
		}

		// Token: 0x04006521 RID: 25889
		public ComplexThreatDef def;

		// Token: 0x04006522 RID: 25890
		public float chancePerComplex = 1f;

		// Token: 0x04006523 RID: 25891
		public int maxPerComplex = int.MaxValue;

		// Token: 0x04006524 RID: 25892
		public int maxPerRoom = 1;

		// Token: 0x04006525 RID: 25893
		public float selectionWeight = 1f;
	}
}
