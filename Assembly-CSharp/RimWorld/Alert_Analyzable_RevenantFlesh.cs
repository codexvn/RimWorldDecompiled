using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200305F RID: 12383
	public class Alert_Analyzable_RevenantFlesh : Alert_Analyzable
	{
		// Token: 0x17002CFC RID: 11516
		// (get) Token: 0x06011633 RID: 71219 RVA: 0x004FCECF File Offset: 0x004FB0CF
		protected override ThingDef Def
		{
			get
			{
				return ThingDefOf.RevenantFleshChunk;
			}
		}

		// Token: 0x06011634 RID: 71220 RVA: 0x004FCED6 File Offset: 0x004FB0D6
		public Alert_Analyzable_RevenantFlesh()
		{
			this.requireAnomaly = true;
			this.defaultLabel = "AlertRevenantFlesh".Translate();
			this.defaultExplanation = "AlertRevenantFleshDesc".Translate();
		}
	}
}
