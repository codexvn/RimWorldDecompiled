using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001958 RID: 6488
	public class CompProperties_DarklightOverlay : CompProperties_FireOverlay
	{
		// Token: 0x06009370 RID: 37744 RVA: 0x002B5BBD File Offset: 0x002B3DBD
		public CompProperties_DarklightOverlay()
		{
			this.compClass = typeof(CompDarklightOverlay);
		}

		// Token: 0x06009371 RID: 37745 RVA: 0x002B5BD5 File Offset: 0x002B3DD5
		public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
			GhostUtility.GhostGraphicFor(CompDarklightOverlay.DarklightGraphic, thingDef, ghostCol, null).DrawFromDef(center.ToVector3ShiftedWithAltitude(drawAltitude), rot, thingDef, 0f);
		}
	}
}
