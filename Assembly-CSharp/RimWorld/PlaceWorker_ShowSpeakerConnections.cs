using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020037F2 RID: 14322
	public class PlaceWorker_ShowSpeakerConnections : PlaceWorker
	{
		// Token: 0x06013D66 RID: 81254 RVA: 0x005E3D41 File Offset: 0x005E1F41
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			PlaceWorker_ShowSpeakerConnections.DrawConnections((ThingDef)checkingDef, loc, map);
			return true;
		}

		// Token: 0x06013D67 RID: 81255 RVA: 0x005E3D58 File Offset: 0x005E1F58
		public static void DrawConnections(ThingDef checker, IntVec3 loc, Map map)
		{
			ThingDef thingDef = (checker == ThingDefOf.LightBall) ? ThingDefOf.Loudspeaker : ThingDefOf.LightBall;
			int num = GenRadial.NumCellsInRadius(12f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = loc + GenRadial.RadialPattern[i];
				Thing firstThing = c.GetFirstThing(map, thingDef);
				if (firstThing != null && firstThing.def == thingDef)
				{
					GenDraw.DrawLineBetween(loc.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays.AltitudeFor()), c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays.AltitudeFor()), SimpleColor.Green, 0.2f);
				}
			}
		}
	}
}
