using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001FC5 RID: 8133
	public class GenStep_OrbitalMechhive : GenStep_LargeRuins
	{
		// Token: 0x17001B4F RID: 6991
		// (get) Token: 0x0600B349 RID: 45897 RVA: 0x00335B54 File Offset: 0x00333D54
		protected override int RegionSize
		{
			get
			{
				return 65;
			}
		}

		// Token: 0x17001B50 RID: 6992
		// (get) Token: 0x0600B34A RID: 45898 RVA: 0x00335B58 File Offset: 0x00333D58
		protected override FloatRange DefaultMapFillPercentRange { get; } = new FloatRange(0.5f, 0.6f);

		// Token: 0x17001B51 RID: 6993
		// (get) Token: 0x0600B34B RID: 45899 RVA: 0x00335B60 File Offset: 0x00333D60
		protected override FloatRange MergeRange { get; } = new FloatRange(1f, 1f);

		// Token: 0x17001B52 RID: 6994
		// (get) Token: 0x0600B34C RID: 45900 RVA: 0x00335B68 File Offset: 0x00333D68
		protected override IntRange RuinsMinMaxRange { get; } = new IntRange(3, 4);

		// Token: 0x17001B53 RID: 6995
		// (get) Token: 0x0600B34D RID: 45901 RVA: 0x00123991 File Offset: 0x00121B91
		protected override int MoveRangeLimit
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17001B54 RID: 6996
		// (get) Token: 0x0600B34E RID: 45902 RVA: 0x00123991 File Offset: 0x00121B91
		protected override int ContractLimit
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17001B55 RID: 6997
		// (get) Token: 0x0600B34F RID: 45903 RVA: 0x00335B70 File Offset: 0x00333D70
		protected override int MinRegionSize
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x17001B56 RID: 6998
		// (get) Token: 0x0600B350 RID: 45904 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool UseUsedRects
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B57 RID: 6999
		// (get) Token: 0x0600B351 RID: 45905 RVA: 0x00335B74 File Offset: 0x00333D74
		protected override LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.Mechhive;
			}
		}

		// Token: 0x17001B58 RID: 7000
		// (get) Token: 0x0600B352 RID: 45906 RVA: 0x00003764 File Offset: 0x00001964
		protected override Faction Faction
		{
			get
			{
				return Faction.OfMechanoids;
			}
		}

		// Token: 0x17001B59 RID: 7001
		// (get) Token: 0x0600B353 RID: 45907 RVA: 0x00335B7B File Offset: 0x00333D7B
		public override int SeedPart
		{
			get
			{
				return 345879234;
			}
		}

		// Token: 0x0600B354 RID: 45908 RVA: 0x00335B84 File Offset: 0x00333D84
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Orbital Mechhive"))
			{
				return;
			}
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			this.stabilizerCount = 0;
			GenStep_OrbitalMechhive.chunks = new Chunks(map.BoundsRect(20), 22);
			using (ProfilerBlock.Scope("CreatePlatform"))
			{
				try
				{
					GenStep_OrbitalMechhive.CreatePlatform(map);
				}
				catch (Exception ex)
				{
					string str = "Error in CreatePlatform ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			using (ProfilerBlock.Scope("GenerateCerebrexCore"))
			{
				try
				{
					this.GenerateCerebrexCore(map, orGenerateVar);
				}
				catch (Exception ex3)
				{
					string str2 = "Error in GenerateCerebrexCore ";
					Exception ex4 = ex3;
					Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
				}
			}
			using (ProfilerBlock.Scope("SetupPlayerStartSpot"))
			{
				try
				{
					this.SetupPlayerStartSpot(map, parms, orGenerateVar);
				}
				catch (Exception ex5)
				{
					string str3 = "Error in SetupPlayerStartSpot ";
					Exception ex6 = ex5;
					Log.Error(str3 + ((ex6 != null) ? ex6.ToString() : null));
				}
			}
			using (ProfilerBlock.Scope("GenerateGaussCannons"))
			{
				try
				{
					this.GenerateCannons(map, orGenerateVar);
				}
				catch (Exception ex7)
				{
					string str4 = "Error in GenerateGaussCannons ";
					Exception ex8 = ex7;
					Log.Error(str4 + ((ex8 != null) ? ex8.ToString() : null));
				}
			}
			using (ProfilerBlock.Scope("GenerateRuins"))
			{
				try
				{
					this.GenerateRuins(map, parms, this.DefaultMapFillPercentRange);
				}
				catch (Exception ex9)
				{
					string str5 = "Error in GenerateRuins ";
					Exception ex10 = ex9;
					Log.Error(str5 + ((ex10 != null) ? ex10.ToString() : null));
				}
			}
			using (ProfilerBlock.Scope("GenerateStandaloneRooms"))
			{
				try
				{
					this.GenerateStandaloneRooms(map, parms, orGenerateVar);
				}
				catch (Exception ex11)
				{
					string str6 = "Error in GenerateStandaloneRooms ";
					Exception ex12 = ex11;
					Log.Error(str6 + ((ex12 != null) ? ex12.ToString() : null));
				}
			}
			using (ProfilerBlock.Scope("CreateHoles"))
			{
				try
				{
					GenStep_OrbitalMechhive.CreateHoles(map, orGenerateVar);
				}
				catch (Exception ex13)
				{
					string str7 = "Error in CreateHoles ";
					Exception ex14 = ex13;
					Log.Error(str7 + ((ex14 != null) ? ex14.ToString() : null));
				}
			}
			using (ProfilerBlock.Scope("GenerateMiniStructures"))
			{
				try
				{
					this.GenerateMiniStructures(map, orGenerateVar);
				}
				catch (Exception ex15)
				{
					string str8 = "Error in GenerateMiniStructures ";
					Exception ex16 = ex15;
					Log.Error(str8 + ((ex16 != null) ? ex16.ToString() : null));
				}
			}
			map.FogOfWarColor = new Color?(ColorLibrary.SpaceMechFog);
			map.OrbitalDebris = OrbitalDebrisDefOf.Mechanoid;
		}

		// Token: 0x0600B355 RID: 45909 RVA: 0x00335EC4 File Offset: 0x003340C4
		private static void CreatePlatform(Map map)
		{
			using (map.pathing.DisableIncrementalScope())
			{
				SpaceGenUtility.ChunksSetSparsley(GenStep_OrbitalMechhive.chunks, FloatRange.One);
				foreach (CellRect cellRect in GenStep_OrbitalMechhive.chunks.EnumeratedRects)
				{
					foreach (IntVec3 c in cellRect.Cells)
					{
						map.terrainGrid.SetTerrain(c, TerrainDefOf.MechanoidPlatform);
					}
					CellRect cellRect2 = cellRect;
					MapGenUtility.DoRectEdgeLumps(map, TerrainDefOf.MechanoidPlatform, ref cellRect2, GenStep_OrbitalMechhive.PlatformPadBorderLumpLengthRange, GenStep_OrbitalMechhive.PlatformPadBorderLumpOffsetRange, null);
				}
			}
		}

		// Token: 0x0600B356 RID: 45910 RVA: 0x00335FA8 File Offset: 0x003341A8
		private void GenerateCerebrexCore(Map map, List<CellRect> usedRects)
		{
			SketchResolveParams parms = default(SketchResolveParams);
			parms.sketch = new Sketch();
			SketchGen.Generate(SketchResolverDefOf.CerebrexCore, parms).Spawn(map, map.Center, Faction.OfMechanoids, Sketch.SpawnPosType.OccupiedCenter, Sketch.SpawnMode.Normal, true, true, true, null, false, false, null, null, null);
			CellRect item = CellRect.CenteredOn(map.Center, Mathf.FloorToInt(30.5f));
			foreach (IntVec3 c in item.Cells)
			{
				map.roofGrid.SetRoof(c, RoofDefOf.RoofConstructed);
			}
			item = item.ExpandedBy(4);
			usedRects.Add(item);
		}

		// Token: 0x0600B357 RID: 45911 RVA: 0x00336064 File Offset: 0x00334264
		private void SetupPlayerStartSpot(Map map, GenStepParams parms, List<CellRect> usedRects)
		{
			if (parms.gravship == null)
			{
				return;
			}
			IntVec2 size = parms.gravship.Bounds.ExpandedBy(5).Size;
			Func<IntVec3, bool> <>9__1;
			CellRect item;
			map.BoundsRect(5).TryFindRandomInnerRect(size, out item, delegate(CellRect r)
			{
				IEnumerable<IntVec3> cells = r.Cells;
				Func<IntVec3, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((IntVec3 c) => c.GetTerrain(map) != TerrainDefOf.Space));
				}
				return cells.All(predicate) && usedRects.All((CellRect ur) => !ur.Overlaps(r));
			});
			foreach (IntVec3 loc in item.ContractedBy(2).Corners)
			{
				GenSpawn.Spawn(ThingDefOf.AncientShipBeacon, loc, map, WipeMode.Vanish);
			}
			MapGenerator.PlayerStartSpot = item.CenterCell;
			usedRects.Add(item);
			MapGenerator.SetVar<bool>("DontGenerateClearedGravShipTerrain", true);
		}

		// Token: 0x0600B358 RID: 45912 RVA: 0x00336150 File Offset: 0x00334350
		private void GenerateCannons(Map map, List<CellRect> usedRects)
		{
			List<IntVec3> list = GenStep_OrbitalMechhive.chunks.boundary.Corners.ToList<IntVec3>();
			for (int i = 0; i < 4; i++)
			{
				CellRect item = CellRect.CenteredOn(list[i], 13, 13);
				foreach (IntVec3 c in item.Cells)
				{
					if (c.InHorDistOf(list[i], 5.9f))
					{
						map.terrainGrid.SetTerrain(c, TerrainDefOf.AncientTile);
					}
					else if (c.InHorDistOf(list[i], 8f))
					{
						map.terrainGrid.SetTerrain(c, TerrainDefOf.MechanoidPlatform);
					}
				}
				GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AnticraftBeam, null), list[i], map, new Rot4((i + 2) % 4), WipeMode.Vanish, false, false);
				usedRects.Add(item);
			}
		}

		// Token: 0x0600B359 RID: 45913 RVA: 0x00336258 File Offset: 0x00334458
		private void GenerateStandaloneRooms(Map map, GenStepParams parms, List<CellRect> usedRects)
		{
			int randomInRange = GenStep_OrbitalMechhive.NumStandaloneRooms.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				CellRect empty = CellRect.Empty;
				if (this.TryGetFreeRect(map, GenStep_OrbitalMechhive.StandaloneRoomSize.RandomInRange, GenStep_OrbitalMechhive.StandaloneRoomSize.RandomInRange, usedRects, out empty))
				{
					StructureGenParams parms2 = new StructureGenParams
					{
						size = empty.Size
					};
					LayoutWorker worker = LayoutDefOf.Mechhive_SingleRoom.Worker;
					LayoutStructureSketch layoutStructureSketch = worker.GenerateStructureSketch(parms2);
					map.layoutStructureSketches.Add(layoutStructureSketch);
					this.structureSketches.Add(layoutStructureSketch);
					float? threatPoints = null;
					if (parms.sitePart != null)
					{
						threatPoints = new float?(parms.sitePart.parms.points);
					}
					worker.Spawn(layoutStructureSketch, map, empty.Min, threatPoints, null, true, false, Faction.OfMechanoids);
					usedRects.Add(empty.ExpandedBy(1));
				}
			}
		}

		// Token: 0x0600B35A RID: 45914 RVA: 0x00336344 File Offset: 0x00334544
		private static void CreateHoles(Map map, List<CellRect> usedRects)
		{
			int approximateArea = GenStep_OrbitalMechhive.chunks.ApproximateArea;
			int num = Mathf.RoundToInt((float)approximateArea * GenStep_OrbitalMechhive.DamagedMaxPercentageRange.RandomInRange);
			List<CellRect> list = new List<CellRect>();
			Chunks chunks = GenStep_OrbitalMechhive.chunks;
			int maximum = num;
			List<CellRect> spaces = list;
			SpaceGenUtility.DamageHoles(chunks, map, maximum, ref approximateArea, null, null, 1f, (CellRect r) => !usedRects.Any((CellRect ur) => ur.Overlaps(r)), spaces);
			foreach (CellRect item in list)
			{
				usedRects.Add(item);
			}
		}

		// Token: 0x0600B35B RID: 45915 RVA: 0x00336408 File Offset: 0x00334608
		private void GenerateMiniStructures(Map map, List<CellRect> usedRects)
		{
			for (int i = 0; i < GenStep_OrbitalMechhive.NumLaunchPads.RandomInRange; i++)
			{
				CellRect cellRect;
				if (this.TryGetFreeRect(map, 5, 5, usedRects, out cellRect))
				{
					this.GenerateSketch(SketchResolverDefOf.MechhiveLaunchPad, map, cellRect);
					usedRects.Add(cellRect);
				}
			}
			for (int j = 0; j < GenStep_OrbitalMechhive.NumGuardOutposts.RandomInRange; j++)
			{
				CellRect cellRect2;
				if (this.TryGetFreeRect(map, 5, 5, usedRects, out cellRect2))
				{
					this.GenerateSketch(SketchResolverDefOf.MechhiveGuardOutpost, map, cellRect2);
					usedRects.Add(cellRect2);
					IEnumerable<Pawn> enumerable = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
					{
						groupKind = PawnGroupKindDefOf.Combat,
						tile = map.Tile,
						faction = Faction.OfMechanoids,
						points = (float)GenStep_OrbitalMechhive.GuardOutpostPointsRange.RandomInRange
					}, true);
					if (enumerable.Any<Pawn>())
					{
						Lord lord = LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_DefendPoint(cellRect2.CenterCell, null, null, false, true), map, null);
						foreach (Pawn pawn in enumerable)
						{
							GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(cellRect2.CenterCell, map, 5, null), map, WipeMode.Vanish);
							lord.AddPawn(pawn);
						}
					}
				}
			}
			for (int k = 0; k < GenStep_OrbitalMechhive.NumProcessorArrays.RandomInRange; k++)
			{
				int width = Rand.RangeInclusive(2, 3) * (ThingDefOf.MechanoidProcessor.size.x + 1) + 1;
				int height = Rand.RangeInclusive(2, 3) * (ThingDefOf.MechanoidProcessor.size.z + 1) + 1;
				CellRect cellRect3;
				if (this.TryGetFreeRect(map, width, height, usedRects, out cellRect3))
				{
					this.GenerateSketch(SketchResolverDefOf.MechhiveProcessorArray, map, cellRect3);
					usedRects.Add(cellRect3);
				}
			}
		}

		// Token: 0x0600B35C RID: 45916 RVA: 0x003365EC File Offset: 0x003347EC
		private void GenerateSketch(SketchResolverDef sketchDef, Map map, CellRect rect)
		{
			SketchGen.Generate(sketchDef, new SketchResolveParams
			{
				sketch = new Sketch(),
				rect = new CellRect?(new CellRect(0, 0, rect.Width, rect.Height))
			}).Spawn(map, rect.CenterCell, Faction.OfMechanoids, Sketch.SpawnPosType.OccupiedCenter, Sketch.SpawnMode.Normal, true, true, true, null, false, false, null, null, null);
		}

		// Token: 0x0600B35D RID: 45917 RVA: 0x00336654 File Offset: 0x00334854
		private bool TryGetFreeRect(Map map, int width, int height, List<CellRect> usedRects, out CellRect rect)
		{
			rect = CellRect.Empty;
			Func<IntVec3, bool> <>9__2;
			IntVec3 center;
			if (!CellFinder.TryFindRandomCell(map, delegate(IntVec3 c)
			{
				CellRect tmpRect = CellRect.CenteredOn(c, width, height);
				if (tmpRect.InBounds(map) && usedRects.All((CellRect ur) => !ur.Overlaps(tmpRect)))
				{
					IEnumerable<IntVec3> cells = tmpRect.Cells;
					Func<IntVec3, bool> predicate;
					if ((predicate = <>9__2) == null)
					{
						predicate = (<>9__2 = ((IntVec3 cell) => cell.GetTerrain(map) == TerrainDefOf.MechanoidPlatform));
					}
					return cells.All(predicate);
				}
				return false;
			}, out center))
			{
				return false;
			}
			rect = CellRect.CenteredOn(center, width, height);
			return true;
		}

		// Token: 0x0600B35E RID: 45918 RVA: 0x003366C7 File Offset: 0x003348C7
		protected override StructureGenParams GetStructureGenParams(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
		{
			StructureGenParams structureGenParams = base.GetStructureGenParams(rect, map, parms, layoutDef);
			structureGenParams.spawnImportantRoom = (this.stabilizerCount < 3);
			this.stabilizerCount++;
			return structureGenParams;
		}

		// Token: 0x04007ACB RID: 31435
		private const int NumStabilizers = 3;

		// Token: 0x04007ACC RID: 31436
		private static readonly IntRange NumStandaloneRooms = new IntRange(10, 12);

		// Token: 0x04007ACD RID: 31437
		private static readonly IntRange StandaloneRoomSize = new IntRange(8, 25);

		// Token: 0x04007ACE RID: 31438
		private static readonly IntRange NumLaunchPads = new IntRange(8, 10);

		// Token: 0x04007ACF RID: 31439
		private static readonly IntRange NumProcessorArrays = new IntRange(8, 12);

		// Token: 0x04007AD0 RID: 31440
		private static readonly IntRange NumGuardOutposts = new IntRange(10, 12);

		// Token: 0x04007AD1 RID: 31441
		private static readonly IntRange GuardOutpostPointsRange = new IntRange(200, 350);

		// Token: 0x04007AD2 RID: 31442
		private const int MapBorderPadding = 20;

		// Token: 0x04007AD3 RID: 31443
		private const int PlatformRegionSize = 22;

		// Token: 0x04007AD4 RID: 31444
		private static readonly IntRange PlatformPadBorderLumpLengthRange = new IntRange(8, 12);

		// Token: 0x04007AD5 RID: 31445
		private static readonly IntRange PlatformPadBorderLumpOffsetRange = new IntRange(-2, 3);

		// Token: 0x04007AD6 RID: 31446
		private static readonly FloatRange DamagedMaxPercentageRange = new FloatRange(0.5f, 0.5f);

		// Token: 0x04007AD7 RID: 31447
		private int stabilizerCount;

		// Token: 0x04007AD8 RID: 31448
		private static Chunks chunks;

		// Token: 0x04007ADC RID: 31452
		private static List<IntVec3> tmpHoleCells = new List<IntVec3>();
	}
}
