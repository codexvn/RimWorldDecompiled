using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A6D RID: 6765
	public class LayoutFillEdgesParms
	{
		// Token: 0x060097DE RID: 38878 RVA: 0x002C52F6 File Offset: 0x002C34F6
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x04006567 RID: 25959
		public ThingDef def;

		// Token: 0x04006568 RID: 25960
		public ThingDef stuff;

		// Token: 0x04006569 RID: 25961
		public int padding;

		// Token: 0x0400656A RID: 25962
		public int contractedBy = 1;

		// Token: 0x0400656B RID: 25963
		public IntRange countRange = IntRange.Invalid;

		// Token: 0x0400656C RID: 25964
		public FloatRange groupsPerTenEdgeCells = new FloatRange(1f, 1f);

		// Token: 0x0400656D RID: 25965
		public IntRange groupCountRange = new IntRange(1, 2);

		// Token: 0x0400656E RID: 25966
		public RotationDirection rotOffset = RotationDirection.Opposite;
	}
}
