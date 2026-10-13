using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003829 RID: 14377
	public class PlaceWorker_DrawLinesToBookcasesInRoom : PlaceWorker
	{
		// Token: 0x06013E02 RID: 81410 RVA: 0x005E66FC File Offset: 0x005E48FC
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
					if (thing2 is Building_Bookcase)
					{
						GenDraw.DrawLineBetween(center.ToVector3Shifted(), thing2.TrueCenter());
					}
				}
			}
		}

		// Token: 0x06013E03 RID: 81411 RVA: 0x005E67C8 File Offset: 0x005E49C8
		public override void DrawPlaceMouseAttachments(float curX, ref float curY, BuildableDef bdef, IntVec3 center, Rot4 rot)
		{
			PlaceWorker_DrawLinesToBookcasesInRoom.tmpLinkedThings.Clear();
			Room room = center.GetRoom(Find.CurrentMap);
			if (room != null)
			{
				foreach (Region region in room.Regions)
				{
					foreach (Thing thing in region.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
					{
						if (!PlaceWorker_DrawLinesToBookcasesInRoom.tmpLinkedThings.Contains(thing) && thing is Building_Bookcase)
						{
							PlaceWorker_DrawLinesToBookcasesInRoom.tmpLinkedThings.Add(thing);
							if (PlaceWorker_DrawLinesToBookcasesInRoom.tmpLinkedThings.Count == 1)
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

		// Token: 0x0400DBD1 RID: 56273
		private static readonly List<Thing> tmpLinkedThings = new List<Thing>();
	}
}
