using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A6F RID: 6767
	public class LayoutPrefabParms
	{
		// Token: 0x060097E2 RID: 38882 RVA: 0x002C52F6 File Offset: 0x002C34F6
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x04006577 RID: 25975
		public PrefabDef def;

		// Token: 0x04006578 RID: 25976
		public IntRange countRange = IntRange.Invalid;

		// Token: 0x04006579 RID: 25977
		public IntRange minMaxRange = new IntRange(-1, int.MaxValue);

		// Token: 0x0400657A RID: 25978
		public bool ensureNoBlocks = true;

		// Token: 0x0400657B RID: 25979
		public FloatRange countPerHundredCells = new FloatRange(1f, 2f);

		// Token: 0x0400657C RID: 25980
		public FloatRange countPerTenEdgeCells = new FloatRange(1f, 1f);

		// Token: 0x0400657D RID: 25981
		public RotationDirection rotOffset;
	}
}
