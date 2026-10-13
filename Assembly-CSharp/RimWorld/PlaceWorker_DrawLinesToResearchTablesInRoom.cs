using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200382A RID: 14378
	public class PlaceWorker_DrawLinesToResearchTablesInRoom : PlaceWorker
	{
		// Token: 0x06013E06 RID: 81414 RVA: 0x005E68F4 File Offset: 0x005E4AF4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Room room = center.GetRoom(Find.CurrentMap);
			if (room == null || !room.ProperRoom || room.PsychologicallyOutdoors)
			{
				return;
			}
			room.DrawFieldEdges();
			foreach (Region region in room.Regions)
			{
				foreach (Thing thing2 in region.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
				{
					if (thing2 is Building_ResearchBench)
					{
						GenDraw.DrawLineBetween(center.ToVector3Shifted(), thing2.TrueCenter());
					}
				}
			}
		}

		// Token: 0x06013E07 RID: 81415 RVA: 0x005E69C0 File Offset: 0x005E4BC0
		public override void DrawPlaceMouseAttachments(float curX, ref float curY, BuildableDef bdef, IntVec3 center, Rot4 rot)
		{
			PlaceWorker_DrawLinesToResearchTablesInRoom.tmpLinkedThings.Clear();
			Room room = center.GetRoom(Find.CurrentMap);
			if (room != null)
			{
				foreach (Region region in room.Regions)
				{
					foreach (Thing thing in region.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
					{
						if (!PlaceWorker_DrawLinesToResearchTablesInRoom.tmpLinkedThings.Contains(thing) && thing is Building_ResearchBench)
						{
							PlaceWorker_DrawLinesToResearchTablesInRoom.tmpLinkedThings.Add(thing);
							if (PlaceWorker_DrawLinesToResearchTablesInRoom.tmpLinkedThings.Count == 1)
							{
								PlaceWorker.DrawTextLine(curX, ref curY, "FacilityPotentiallyLinkedTo".Translate() + ":");
							}
							PlaceWorker.DrawTextLine(curX, ref curY, "  - " + thing.LabelCap);
						}
					}
				}
			}
			base.DrawPlaceMouseAttachments(curX, ref curY, bdef, center, rot);
		}

		// Token: 0x0400DBD2 RID: 56274
		private static readonly List<Thing> tmpLinkedThings = new List<Thing>();
	}
}
