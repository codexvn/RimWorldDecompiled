using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A7A RID: 6778
	public class PrefabThingData
	{
		// Token: 0x06009802 RID: 38914 RVA: 0x002C52F6 File Offset: 0x002C34F6
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x040065BA RID: 26042
		public ThingDef def;

		// Token: 0x040065BB RID: 26043
		public ThingDef stuff;

		// Token: 0x040065BC RID: 26044
		public ColorDef colorDef;

		// Token: 0x040065BD RID: 26045
		public Color color;

		// Token: 0x040065BE RID: 26046
		public IntRange stackCountRange = IntRange.One;

		// Token: 0x040065BF RID: 26047
		public int hp;

		// Token: 0x040065C0 RID: 26048
		public float chance = 1f;

		// Token: 0x040065C1 RID: 26049
		public bool canOverrideData = true;

		// Token: 0x040065C2 RID: 26050
		public QualityCategory? quality;

		// Token: 0x040065C3 RID: 26051
		public IntVec3 position;

		// Token: 0x040065C4 RID: 26052
		public List<CellRect> rects;

		// Token: 0x040065C5 RID: 26053
		public List<IntVec3> positions;

		// Token: 0x040065C6 RID: 26054
		public RotationDirection relativeRotation;
	}
}
