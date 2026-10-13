using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003813 RID: 14355
	public class PlaceWorker_DrawLinesToDeathrestCaskets : PlaceWorker
	{
		// Token: 0x06013DC2 RID: 81346 RVA: 0x005E553C File Offset: 0x005E373C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			Room room = center.GetRoom(Find.CurrentMap);
			if (room == null)
			{
				return;
			}
			foreach (Region region in room.Regions)
			{
				foreach (Thing thing2 in region.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
				{
					if (thing2.def == ThingDefOf.DeathrestCasket && GenSight.LineOfSight(center, thing2.OccupiedRect().CenterCell, Find.CurrentMap))
					{
						GenDraw.DrawLineBetween(center.ToVector3Shifted(), thing2.TrueCenter());
					}
				}
			}
		}

		// Token: 0x06013DC3 RID: 81347 RVA: 0x005E561C File Offset: 0x005E381C
		public override void DrawPlaceMouseAttachments(float curX, ref float curY, BuildableDef bdef, IntVec3 center, Rot4 rot)
		{
			PlaceWorker_DrawLinesToDeathrestCaskets.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.curX = curX;
			if (ModsConfig.BiotechActive)
			{
				PlaceWorker_DrawLinesToDeathrestCaskets.tmpLinkedThings.Clear();
				Room room = center.GetRoom(Find.CurrentMap);
				if (room != null)
				{
					foreach (Region region in room.Regions)
					{
						foreach (Thing thing in region.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
						{
							if (!PlaceWorker_DrawLinesToDeathrestCaskets.tmpLinkedThings.Contains(thing) && thing.def == ThingDefOf.DeathrestCasket && GenSight.LineOfSight(center, thing.OccupiedRect().CenterCell, Find.CurrentMap))
							{
								PlaceWorker_DrawLinesToDeathrestCaskets.tmpLinkedThings.Add(thing);
								if (PlaceWorker_DrawLinesToDeathrestCaskets.tmpLinkedThings.Count == 1)
								{
									PlaceWorker_DrawLinesToDeathrestCaskets.<DrawPlaceMouseAttachments>g__DrawTextLine|2_0(ref curY, "FacilityPotentiallyLinkedTo".Translate() + ":", ref CS$<>8__locals1);
								}
								PlaceWorker_DrawLinesToDeathrestCaskets.<DrawPlaceMouseAttachments>g__DrawTextLine|2_0(ref curY, "  - " + thing.LabelCap, ref CS$<>8__locals1);
							}
						}
					}
				}
			}
			base.DrawPlaceMouseAttachments(CS$<>8__locals1.curX, ref curY, bdef, center, rot);
		}

		// Token: 0x06013DC6 RID: 81350 RVA: 0x005E5790 File Offset: 0x005E3990
		[CompilerGenerated]
		internal static void <DrawPlaceMouseAttachments>g__DrawTextLine|2_0(ref float y, string text, ref PlaceWorker_DrawLinesToDeathrestCaskets.<>c__DisplayClass2_0 A_2)
		{
			float lineHeight = Text.LineHeight;
			Widgets.Label(new Rect(A_2.curX, y, 999f, lineHeight), text);
			y += lineHeight;
		}

		// Token: 0x0400DBB6 RID: 56246
		private static List<Thing> tmpLinkedThings = new List<Thing>();
	}
}
