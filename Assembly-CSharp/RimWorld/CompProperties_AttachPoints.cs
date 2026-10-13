using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BFE RID: 11262
	public class CompProperties_AttachPoints : CompProperties
	{
		// Token: 0x0600FC5B RID: 64603 RVA: 0x004930F3 File Offset: 0x004912F3
		public CompProperties_AttachPoints()
		{
			this.compClass = typeof(CompAttachPoints);
		}

		// Token: 0x0400A55B RID: 42331
		public List<AttachPoint> points;
	}
}
