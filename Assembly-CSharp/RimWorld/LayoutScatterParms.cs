using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A6A RID: 6762
	public class LayoutScatterParms
	{
		// Token: 0x060097D8 RID: 38872 RVA: 0x002C52F6 File Offset: 0x002C34F6
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x04006555 RID: 25941
		public ThingDef def;

		// Token: 0x04006556 RID: 25942
		public ThingDef stuff;

		// Token: 0x04006557 RID: 25943
		public IntRange groupCount = IntRange.Invalid;

		// Token: 0x04006558 RID: 25944
		public FloatRange groupsPerHundredCells = new FloatRange(0f, 2f);

		// Token: 0x04006559 RID: 25945
		public IntRange itemsPerGroup = new IntRange(2, 4);

		// Token: 0x0400655A RID: 25946
		public IntRange groupDistRange = new IntRange(2, 5);

		// Token: 0x0400655B RID: 25947
		public int minGroups;
	}
}
