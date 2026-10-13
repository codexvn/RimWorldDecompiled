using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003819 RID: 14361
	[StaticConstructorOnStartup]
	public class PlaceWorker_RitualFocus : PlaceWorker
	{
		// Token: 0x170032A3 RID: 12963
		// (get) Token: 0x06013DD3 RID: 81363 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool UseArrow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06013DD4 RID: 81364 RVA: 0x005E5B54 File Offset: 0x005E3D54
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			CellRect cellRect;
			PlaceWorker_SpectatorPreview.DrawSpectatorPreview(def, center, rot, ghostCol, this.UseArrow, out cellRect, thing);
			List<Thing> forCell = Find.CurrentMap.listerBuldingOfDefInProximity.GetForCell(center, 35f, ThingDefOf.Lectern, null);
			try
			{
				foreach (Thing thing2 in forCell)
				{
					PlaceWorker_RitualPosition placeWorker_RitualPosition = (PlaceWorker_RitualPosition)thing2.def.PlaceWorkers.FirstOrDefault((PlaceWorker w) => w is PlaceWorker_RitualPosition);
					if (placeWorker_RitualPosition != null)
					{
						placeWorker_RitualPosition.DrawGhost(thing2.def, thing2.Position, thing2.Rotation, ghostCol, thing2);
						if (cellRect.ClosestCellTo(thing2.Position).InHorDistOf(thing2.Position, 4.9f))
						{
							PlaceWorker_RitualFocus.tmpConnectedRitualSeats.AddRange(PlaceWorker_RitualPosition.RitualSeatsInRange(thing2.def, thing2.Position, thing2.Rotation, thing2));
							GenDraw.DrawLineBetween(GenThing.TrueCenter(center, Rot4.North, def.size, def.Altitude), thing2.TrueCenter(), SimpleColor.Green, 0.2f);
						}
					}
				}
				PlaceWorker_RitualPosition.DrawRitualSeatConnections(def, center, rot, thing, PlaceWorker_RitualFocus.tmpConnectedRitualSeats);
			}
			finally
			{
				PlaceWorker_RitualFocus.tmpConnectedRitualSeats.Clear();
			}
		}

		// Token: 0x0400DBBC RID: 56252
		private static List<Thing> tmpConnectedRitualSeats = new List<Thing>();
	}
}
