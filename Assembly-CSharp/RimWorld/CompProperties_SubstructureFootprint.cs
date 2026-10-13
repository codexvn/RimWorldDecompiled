using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E70 RID: 11888
	public class CompProperties_SubstructureFootprint : CompProperties
	{
		// Token: 0x06010BDC RID: 68572 RVA: 0x004D35D9 File Offset: 0x004D17D9
		public CompProperties_SubstructureFootprint()
		{
			this.compClass = typeof(CompSubstructureFootprint);
		}

		// Token: 0x06010BDD RID: 68573 RVA: 0x004D35F8 File Offset: 0x004D17F8
		public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
			SubstructureGrid.DrawSubstructureFootprintWithExtra(this, center, thing);
		}

		// Token: 0x0400AE74 RID: 44660
		public float radius;

		// Token: 0x0400AE75 RID: 44661
		public bool displaySubstructureOverlayWhenSelected = true;
	}
}
