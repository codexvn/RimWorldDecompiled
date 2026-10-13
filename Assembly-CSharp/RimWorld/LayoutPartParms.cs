using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A70 RID: 6768
	public class LayoutPartParms
	{
		// Token: 0x060097E4 RID: 38884 RVA: 0x002C54B8 File Offset: 0x002C36B8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", "chance");
		}

		// Token: 0x0400657E RID: 25982
		public RoomPartDef def;

		// Token: 0x0400657F RID: 25983
		public float chance = 1f;

		// Token: 0x04006580 RID: 25984
		public IntRange threatPointsRange = IntRange.Invalid;

		// Token: 0x04006581 RID: 25985
		public IntRange countRange = IntRange.Invalid;
	}
}
