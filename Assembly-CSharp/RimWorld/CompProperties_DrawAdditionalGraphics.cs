using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C65 RID: 11365
	public class CompProperties_DrawAdditionalGraphics : CompProperties
	{
		// Token: 0x0600FED6 RID: 65238 RVA: 0x0049DAF4 File Offset: 0x0049BCF4
		public CompProperties_DrawAdditionalGraphics()
		{
			this.compClass = typeof(CompDrawAdditionalGraphics);
		}

		// Token: 0x0400A6EC RID: 42732
		public List<GraphicData> graphics;
	}
}
