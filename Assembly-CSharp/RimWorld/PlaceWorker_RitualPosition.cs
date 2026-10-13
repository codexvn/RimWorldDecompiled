using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200381C RID: 14364
	public class PlaceWorker_RitualPosition : PlaceWorker
	{
		// Token: 0x06013DDC RID: 81372 RVA: 0x005E5D0B File Offset: 0x005E3F0B
		public static List<Thing> GetRitualFocusInRange(IntVec3 ritualPosition, Thing ritualPositionThing = null)
		{
			return Find.CurrentMap.listerBuildingWithTagInProximity.GetForCell(ritualPosition, 4.9f, "RitualFocus", ritualPositionThing);
		}

		// Token: 0x06013DDD RID: 81373 RVA: 0x005E5D28 File Offset: 0x005E3F28
		public static IEnumerable<Thing> RitualSeatsInRange(ThingDef def, IntVec3 center, Rot4 rot, Thing thing = null)
		{
			List<Thing> buildings = null;
			try
			{
				if (GatheringsUtility.UseWholeRoomAsGatheringArea(center, Find.CurrentMap))
				{
					buildings = PlaceWorker_RitualPosition.tmpRitualSeatsInRoom;
					using (List<Thing>.Enumerator enumerator = center.GetRoom(Find.CurrentMap).ContainedAndAdjacentThings.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Thing thing2 = enumerator.Current;
							if (thing2.TryGetComp<CompRitualSeat>() != null)
							{
								buildings.Add(thing2);
							}
						}
						goto IL_C8;
					}
				}
				buildings = Find.CurrentMap.listerBuildingWithTagInProximity.GetForCell(center, 18f, "RitualSeat", thing);
				IL_C8:
				int num;
				for (int i = 0; i < buildings.Count; i = num + 1)
				{
					Thing thing3 = buildings[i];
					if (GatheringsUtility.InGatheringArea(thing3.Position, center, Find.CurrentMap) && SpectatorCellFinder.CorrectlyRotatedChairAt(thing3.Position, Find.CurrentMap, GenAdj.OccupiedRect(center, rot, def.size)))
					{
						yield return thing3;
					}
					num = i;
				}
			}
			finally
			{
				PlaceWorker_RitualPosition.tmpRitualSeatsInRoom.Clear();
			}
			yield break;
			yield break;
		}

		// Token: 0x06013DDE RID: 81374 RVA: 0x005E5D50 File Offset: 0x005E3F50
		public static void DrawRitualSeatConnections(ThingDef def, IntVec3 center, Rot4 rot, Thing thing = null, List<Thing> except = null)
		{
			foreach (Thing thing2 in PlaceWorker_RitualPosition.RitualSeatsInRange(def, center, rot, thing))
			{
				if (except == null || !except.Contains(thing2))
				{
					GenDraw.DrawLineBetween(GenThing.TrueCenter(center, rot, def.size, AltitudeLayer.MetaOverlays.AltitudeFor()), thing2.TrueCenter(), SimpleColor.Yellow, 0.2f);
				}
			}
		}

		// Token: 0x06013DDF RID: 81375 RVA: 0x005E5DCC File Offset: 0x005E3FCC
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			List<Thing> ritualFocusInRange = PlaceWorker_RitualPosition.GetRitualFocusInRange(center, thing);
			for (int i = 0; i < ritualFocusInRange.Count; i++)
			{
				Thing thing2 = ritualFocusInRange[i];
				if (thing2.def != def)
				{
					GenDraw.DrawLineBetween(GenThing.TrueCenter(center, Rot4.North, def.size, def.Altitude), thing2.TrueCenter(), SimpleColor.Green, 0.2f);
				}
			}
			PlaceWorker_RitualPosition.DrawRitualSeatConnections(def, center, rot, thing, null);
		}

		// Token: 0x0400DBBF RID: 56255
		public const float RitualFocusRange = 4.9f;

		// Token: 0x0400DBC0 RID: 56256
		public const SimpleColor RitualFocusConnectionColor = SimpleColor.Green;

		// Token: 0x0400DBC1 RID: 56257
		public const SimpleColor RitualSeatConnectionColor = SimpleColor.Yellow;

		// Token: 0x0400DBC2 RID: 56258
		private static List<Thing> tmpRitualSeatsInRoom = new List<Thing>();
	}
}
