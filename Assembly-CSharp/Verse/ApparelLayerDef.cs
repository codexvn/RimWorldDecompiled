using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000207 RID: 519
	public class ApparelLayerDef : Def
	{
		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x00041A49 File Offset: 0x0003FC49
		public bool IsUtilityLayer
		{
			get
			{
				return this == ApparelLayerDefOf.Belt;
			}
		}

		// Token: 0x04000822 RID: 2082
		public int drawOrder;
	}
}
