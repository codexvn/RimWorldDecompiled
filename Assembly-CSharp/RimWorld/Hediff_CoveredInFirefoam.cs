using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200385D RID: 14429
	public class Hediff_CoveredInFirefoam : HediffWithComps
	{
		// Token: 0x06013F2D RID: 81709 RVA: 0x005EB2D9 File Offset: 0x005E94D9
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.pawn.Drawer.renderer.FirefoamOverlays.coveredInFoam = true;
		}

		// Token: 0x06013F2E RID: 81710 RVA: 0x005EB2FD File Offset: 0x005E94FD
		public override void PostRemoved()
		{
			base.PostRemoved();
			this.pawn.Drawer.renderer.FirefoamOverlays.coveredInFoam = this.pawn.health.hediffSet.HasHediff(HediffDefOf.CoveredInFirefoam, false);
		}

		// Token: 0x06013F2F RID: 81711 RVA: 0x005EB33A File Offset: 0x005E953A
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawn.Drawer.renderer.FirefoamOverlays.coveredInFoam = true;
			}
		}
	}
}
