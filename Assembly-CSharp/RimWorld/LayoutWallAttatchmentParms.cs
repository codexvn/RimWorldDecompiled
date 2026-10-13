using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A6C RID: 6764
	public class LayoutWallAttatchmentParms
	{
		// Token: 0x060097DC RID: 38876 RVA: 0x002C52F6 File Offset: 0x002C34F6
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x04006562 RID: 25954
		public ThingDef def;

		// Token: 0x04006563 RID: 25955
		public ThingDef stuff;

		// Token: 0x04006564 RID: 25956
		public IntRange countRange = IntRange.Invalid;

		// Token: 0x04006565 RID: 25957
		public FloatRange thingsPer10EdgeCells = new FloatRange(1f, 1f);

		// Token: 0x04006566 RID: 25958
		public float spawnChancePerPosition = 1f;
	}
}
