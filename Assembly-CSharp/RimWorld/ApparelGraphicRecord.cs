using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003834 RID: 14388
	public struct ApparelGraphicRecord
	{
		// Token: 0x06013E26 RID: 81446 RVA: 0x005E724C File Offset: 0x005E544C
		public ApparelGraphicRecord(Graphic graphic, Apparel sourceApparel)
		{
			this.graphic = graphic;
			this.sourceApparel = sourceApparel;
		}

		// Token: 0x0400DBD7 RID: 56279
		public Graphic graphic;

		// Token: 0x0400DBD8 RID: 56280
		public Apparel sourceApparel;
	}
}
