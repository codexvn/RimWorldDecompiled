using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003815 RID: 14357
	public class PlaceWorker_ConnectionStrengthOffsetBuildingsNear : PlaceWorker
	{
		// Token: 0x06013DC7 RID: 81351 RVA: 0x005E57C4 File Offset: 0x005E39C4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			CompProperties_TreeConnection compProperties_TreeConnection = (CompProperties_TreeConnection)def.CompDefFor<CompTreeConnection>();
			List<Thing> list = GauranlenUtility.BuildingsAffectingConnectionStrengthAt(center, Find.CurrentMap, compProperties_TreeConnection);
			GenDraw.DrawRadiusRing(center, compProperties_TreeConnection.radiusToBuildingForConnectionStrengthLoss, Color.white, null);
			if (!list.NullOrEmpty<Thing>())
			{
				int num = 0;
				foreach (Thing t in list)
				{
					if (num++ > 10)
					{
						break;
					}
					GenDraw.DrawLineBetween(GenThing.TrueCenter(center, Rot4.North, def.size, def.Altitude), t.TrueCenter(), SimpleColor.Red, 0.2f);
				}
			}
		}
	}
}
