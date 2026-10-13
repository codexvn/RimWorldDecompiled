using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038B4 RID: 14516
	public class SketchResolver_AncientUtilityBuilding : SketchResolver
	{
		// Token: 0x060140CB RID: 82123 RVA: 0x005F386F File Offset: 0x005F1A6F
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.sketch != null;
		}

		// Token: 0x060140CC RID: 82124 RVA: 0x005F4644 File Offset: 0x005F2844
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckIdeology("Ancient utility building"))
			{
				return;
			}
			ThingDef thingDef = BaseGenUtility.RandomCheapWallStuff(Faction.OfAncients, true);
			LayoutSketch layoutSketch = new LayoutSketch
			{
				wallStuff = thingDef,
				doorStuff = thingDef
			};
			IntVec2 intVec = parms.utilityBuildingSize ?? new IntVec2(10, 10);
			StructureLayout structureLayout = RoomLayoutGenerator.GenerateRandomLayout(new CellRect(0, 0, intVec.x, intVec.z), 4, 4, 0f, false, true, null, 2, new IntRange?(IntRange.Zero), CorridorShape.All, true, false);
			layoutSketch.structureLayout = structureLayout;
			layoutSketch.FlushLayoutToSketch();
			List<LayoutRoom> rooms = structureLayout.Rooms;
			rooms.SortByDescending((LayoutRoom a) => a.Area);
			LayoutRoom layoutRoom = null;
			for (int i = 0; i < rooms.Count; i++)
			{
				if (structureLayout.IsAdjacentToLayoutEdge(rooms[i]))
				{
					foreach (IntVec3 intVec2 in rooms[i].Cells)
					{
						if (structureLayout.container.IsOnEdge(intVec2) && structureLayout.IsWallAt(intVec2))
						{
							layoutSketch.AddThing(ThingDefOf.AncientFence, intVec2, Rot4.North, null, 1, null, null, true, 2f);
						}
					}
					IEnumerable<IntVec3> cellsToCheck = rooms[i].rects.SelectMany((CellRect r) => r.Cells).InRandomOrder(null);
					IntVec3 pos;
					if (this.TryFindThingPositionWithGap(ThingDefOf.AncientGenerator, cellsToCheck, layoutSketch, out pos, 1))
					{
						layoutSketch.AddThing(ThingDefOf.AncientGenerator, pos, ThingDefOf.AncientGenerator.defaultPlacingRot, null, 1, null, null, true, 2f);
					}
					foreach (CellRect cellRect in rooms[i].rects)
					{
						foreach (IntVec3 pos2 in cellRect)
						{
							layoutSketch.AddTerrain(TerrainDefOf.AncientConcrete, pos2, true);
						}
					}
					layoutRoom = rooms[i];
					break;
				}
			}
			for (int j = 0; j < rooms.Count; j++)
			{
				if (rooms[j] != layoutRoom)
				{
					foreach (IntVec3 pos3 in rooms[j].rects.SelectMany((CellRect r) => r.Cells))
					{
						layoutSketch.AddTerrain(TerrainDefOf.AncientConcrete, pos3, true);
					}
				}
			}
			parms.sketch.Merge(layoutSketch, true);
			SketchResolveParams parms2 = parms;
			parms2.wallEdgeThing = ThingDefOf.Table1x2c;
			parms2.requireFloor = new bool?(true);
			parms2.allowWood = new bool?(false);
			SketchResolverDefOf.AddWallEdgeThings.Resolve(parms2);
			SketchResolveParams parms3 = parms;
			parms3.destroyChanceExp = new float?(1.5f);
			SketchResolverDefOf.DamageBuildings.Resolve(parms3);
		}

		// Token: 0x060140CD RID: 82125 RVA: 0x005F49EC File Offset: 0x005F2BEC
		private bool TryFindThingPositionWithGap(ThingDef thingDef, IEnumerable<IntVec3> cellsToCheck, Sketch sketch, out IntVec3 position, int gap = 1)
		{
			foreach (IntVec3 intVec in cellsToCheck)
			{
				CellRect cellRect = GenAdj.OccupiedRect(intVec, thingDef.defaultPlacingRot, thingDef.size).ExpandedBy(gap);
				bool flag = true;
				foreach (IntVec3 pos in cellRect.Cells)
				{
					if (sketch.EdificeAt(pos) != null)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					position = intVec;
					return true;
				}
			}
			position = IntVec3.Invalid;
			return false;
		}
	}
}
