using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020036EF RID: 14063
	public class StatPart_CorpseCasket : StatPart
	{
		// Token: 0x06013815 RID: 79893 RVA: 0x005C9FF1 File Offset: 0x005C81F1
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ApplyTo(req))
			{
				val += (float)this.offsetOccupied;
			}
		}

		// Token: 0x06013816 RID: 79894 RVA: 0x005CA008 File Offset: 0x005C8208
		public override string ExplanationPart(StatRequest req)
		{
			if (!this.ApplyTo(req))
			{
				return null;
			}
			return "StatsReport_OccupiedCorpseCasket".Translate() + ": " + this.offsetOccupied.ToString();
		}

		// Token: 0x06013817 RID: 79895 RVA: 0x005CA040 File Offset: 0x005C8240
		private bool ApplyTo(StatRequest req)
		{
			Building_CorpseCasket building_CorpseCasket = req.Thing as Building_CorpseCasket;
			return building_CorpseCasket != null && building_CorpseCasket.HasCorpse && this.offsetOccupied != 0 && (this.thingDefs == null || this.thingDefs.Contains(req.Thing.def));
		}

		// Token: 0x0400D896 RID: 55446
		public int offsetOccupied;

		// Token: 0x0400D897 RID: 55447
		public List<ThingDef> thingDefs;
	}
}
