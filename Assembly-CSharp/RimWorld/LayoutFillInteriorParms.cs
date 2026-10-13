using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A6E RID: 6766
	public class LayoutFillInteriorParms
	{
		// Token: 0x060097E0 RID: 38880 RVA: 0x002C52F6 File Offset: 0x002C34F6
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x0400656F RID: 25967
		public ThingDef def;

		// Token: 0x04006570 RID: 25968
		public ThingDef stuff;

		// Token: 0x04006571 RID: 25969
		public Rot4? fixedRot;

		// Token: 0x04006572 RID: 25970
		public bool alignWithRect;

		// Token: 0x04006573 RID: 25971
		public int contractedBy = 2;

		// Token: 0x04006574 RID: 25972
		public bool snapToGrid;

		// Token: 0x04006575 RID: 25973
		public IntRange countRange = IntRange.Invalid;

		// Token: 0x04006576 RID: 25974
		public FloatRange thingsPerHundredCells = new FloatRange(2f, 3f);
	}
}
