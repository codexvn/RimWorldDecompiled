using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200380E RID: 14350
	public class PlaceWorker_SmokeCloudMaker : PlaceWorker
	{
		// Token: 0x06013DB5 RID: 81333 RVA: 0x005E511C File Offset: 0x005E331C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_SmokeCloudMaker compProperties_SmokeCloudMaker;
			if ((compProperties_SmokeCloudMaker = def.GetCompProperties<CompProperties_SmokeCloudMaker>()) == null)
			{
				ThingDef thingDef = def.entityDefToBuild as ThingDef;
				compProperties_SmokeCloudMaker = ((thingDef != null) ? thingDef.GetCompProperties<CompProperties_SmokeCloudMaker>() : null);
			}
			CompProperties_SmokeCloudMaker compProperties_SmokeCloudMaker2 = compProperties_SmokeCloudMaker;
			if (compProperties_SmokeCloudMaker2 != null)
			{
				GenDraw.DrawCircleOutline(center.ToVector3Shifted(), compProperties_SmokeCloudMaker2.cloudRadius);
			}
		}
	}
}
