using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A15 RID: 6677
	public class ColorDef : Def
	{
		// Token: 0x040062DA RID: 25306
		public Color color;

		// Token: 0x040062DB RID: 25307
		public ColorType colorType;

		// Token: 0x040062DC RID: 25308
		public int displayOrder = int.MaxValue;

		// Token: 0x040062DD RID: 25309
		public bool displayInStylingStationUI = true;

		// Token: 0x040062DE RID: 25310
		public bool randomlyPickable = true;
	}
}
