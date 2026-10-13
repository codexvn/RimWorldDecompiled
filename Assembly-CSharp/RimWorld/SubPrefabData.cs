using System;
using System.Collections.Generic;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A7B RID: 6779
	public class SubPrefabData
	{
		// Token: 0x06009804 RID: 38916 RVA: 0x002C5C99 File Offset: 0x002C3E99
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", "position");
		}

		// Token: 0x040065C7 RID: 26055
		public PrefabDef def;

		// Token: 0x040065C8 RID: 26056
		public float chance = 1f;

		// Token: 0x040065C9 RID: 26057
		public IntVec3 position;

		// Token: 0x040065CA RID: 26058
		public List<IntVec3> positions;

		// Token: 0x040065CB RID: 26059
		public RotationDirection relativeRotation;
	}
}
