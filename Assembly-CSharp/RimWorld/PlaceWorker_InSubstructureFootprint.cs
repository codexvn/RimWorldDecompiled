using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200382E RID: 14382
	public class PlaceWorker_InSubstructureFootprint : PlaceWorker
	{
		// Token: 0x06013E13 RID: 81427 RVA: 0x005E6BFD File Offset: 0x005E4DFD
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			SubstructureGrid.DrawSubstructureFootprint(null, null);
		}

		// Token: 0x06013E14 RID: 81428 RVA: 0x005E6C06 File Offset: 0x005E4E06
		public override void DrawOnGUIExtra(BuildableDef def)
		{
			SubstructureGrid.DrawSubstructureCountOnGUI();
		}

		// Token: 0x06013E15 RID: 81429 RVA: 0x005E6C10 File Offset: 0x005E4E10
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			RoofDef roofDef = map.roofGrid.RoofAt(loc);
			if (roofDef != null && roofDef.isNatural)
			{
				return "MessageCannotPlaceUnderRockRoof".Translate();
			}
			if (!GravshipUtility.InsideFootprint(loc, map, null, null))
			{
				return "MessageMustBePlacedInRangeOfGravEngineOrExtender".Translate();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06013E16 RID: 81430 RVA: 0x005E6C68 File Offset: 0x005E4E68
		public override void DrawPlaceMouseAttachments(float curX, ref float curY, BuildableDef bdef, IntVec3 center, Rot4 rot)
		{
			if (!center.InBounds(Find.CurrentMap) || center.Fogged(Find.CurrentMap))
			{
				return;
			}
			GUI.color = ColorLibrary.RedReadable;
			RoofDef roofDef = Find.CurrentMap.roofGrid.RoofAt(center);
			if (roofDef != null && roofDef.isNatural)
			{
				PlaceWorker.DrawTextLine(curX, ref curY, "MessageCannotPlaceUnderRockRoof".Translate());
			}
			if (!GravshipUtility.InsideFootprint(center, Find.CurrentMap, null, null))
			{
				PlaceWorker.DrawTextLine(curX, ref curY, "MessageMustBePlacedInRangeOfGravEngineOrExtender".Translate());
			}
			GUI.color = Color.white;
		}

		// Token: 0x06013E17 RID: 81431 RVA: 0x005E6D00 File Offset: 0x005E4F00
		public override bool ForceAllowPlaceOver(BuildableDef otherDef)
		{
			ThingDef thingDef = otherDef as ThingDef;
			return (thingDef != null && thingDef.Fillage == FillCategory.Full) || base.ForceAllowPlaceOver(otherDef);
		}
	}
}
