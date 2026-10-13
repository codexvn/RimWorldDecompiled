using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002011 RID: 8209
	public class GenStep_GravshipMarker : GenStep
	{
		// Token: 0x17001B88 RID: 7048
		// (get) Token: 0x0600B4C1 RID: 46273 RVA: 0x0033DCF7 File Offset: 0x0033BEF7
		public override int SeedPart
		{
			get
			{
				return 2031711911;
			}
		}

		// Token: 0x0600B4C2 RID: 46274 RVA: 0x0033DD00 File Offset: 0x0033BF00
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Gravship gravship = parms.gravship;
			if (gravship == null)
			{
				return;
			}
			IntVec3 playerStartSpot = MapGenerator.PlayerStartSpot;
			HashSet<IntVec3> cellsAdjacentToSubstructure = GravshipPlacementUtility.GetCellsAdjacentToSubstructure(gravship.OccupiedRects, 2);
			GravshipPlacementUtility.ClearArea(map, playerStartSpot, cellsAdjacentToSubstructure, GravshipPlacementUtility.ClearMode.BlockingBuildingsOnly);
			GravshipLandingMarker gravshipLandingMarker = ThingMaker.MakeThing(ThingDefOf.GravshipLandingMarker, null) as GravshipLandingMarker;
			gravshipLandingMarker.gravship = gravship;
			GenSpawn.Spawn(gravshipLandingMarker, playerStartSpot, map, WipeMode.Vanish);
		}

		// Token: 0x04007BF6 RID: 31734
		public const int ClearEdgeRadius = 2;
	}
}
