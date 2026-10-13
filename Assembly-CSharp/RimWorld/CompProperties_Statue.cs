using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E56 RID: 11862
	public class CompProperties_Statue : CompProperties_Art
	{
		// Token: 0x06010B20 RID: 68384 RVA: 0x004CFF61 File Offset: 0x004CE161
		public CompProperties_Statue()
		{
			this.compClass = typeof(CompStatue);
		}

		// Token: 0x0400AE07 RID: 44551
		public GraphicData statueBaseGraphic;
	}
}
