using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003826 RID: 14374
	public class PlaceWorker_PollutionPumpArtificialBuildings : PlaceWorker
	{
		// Token: 0x06013DFB RID: 81403 RVA: 0x005E64F0 File Offset: 0x005E46F0
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			CompProperties_PollutionPump compProperties = def.GetCompProperties<CompProperties_PollutionPump>();
			if (compProperties == null)
			{
				return;
			}
			List<Thing> forCell = Find.CurrentMap.listerArtificialBuildingsForMeditation.GetForCell(center, compProperties.radius);
			GenDraw.DrawRadiusRing(center, compProperties.radius, Color.white, null);
			if (!forCell.NullOrEmpty<Thing>())
			{
				int num = 0;
				foreach (Thing t in forCell)
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
