using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A6B RID: 6763
	public class LayoutScatterTerrainParms
	{
		// Token: 0x060097DA RID: 38874 RVA: 0x002C52F6 File Offset: 0x002C34F6
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x0400655C RID: 25948
		public TerrainDef def;

		// Token: 0x0400655D RID: 25949
		public IntRange groupCount = IntRange.Invalid;

		// Token: 0x0400655E RID: 25950
		public FloatRange groupsPerHundredCells = new FloatRange(0f, 2f);

		// Token: 0x0400655F RID: 25951
		public IntRange itemsPerGroup = new IntRange(2, 4);

		// Token: 0x04006560 RID: 25952
		public IntRange groupDistRange = new IntRange(2, 5);

		// Token: 0x04006561 RID: 25953
		public int minGroups;
	}
}
