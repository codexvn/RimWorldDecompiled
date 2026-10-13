using System;
using System.Collections.Generic;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A7C RID: 6780
	public class PrefabTerrainData
	{
		// Token: 0x06009806 RID: 38918 RVA: 0x002C52F6 File Offset: 0x002C34F6
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", null);
		}

		// Token: 0x040065CC RID: 26060
		public TerrainDef def;

		// Token: 0x040065CD RID: 26061
		public float chance = 1f;

		// Token: 0x040065CE RID: 26062
		public ColorDef color;

		// Token: 0x040065CF RID: 26063
		public List<CellRect> rects;
	}
}
