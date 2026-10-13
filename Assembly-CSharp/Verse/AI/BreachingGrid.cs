using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;
using Unity.Collections;
using UnityEngine;
using Verse.AI.Group;
using Verse.Noise;

namespace Verse.AI
{
	// Token: 0x02001047 RID: 4167
	public class BreachingGrid : IExposable
	{
		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x06006592 RID: 26002 RVA: 0x00202D70 File Offset: 0x00200F70
		public BoolGrid WalkGrid
		{
			get
			{
				return this.walkGrid;
			}
		}

		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x06006593 RID: 26003 RVA: 0x00202D78 File Offset: 0x00200F78
		public BoolGrid BreachGrid
		{
			get
			{
				return this.breachGrid;
			}
		}

		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x06006594 RID: 26004 RVA: 0x00202D80 File Offset: 0x00200F80
		public ByteGrid MarkerGrid
		{
			get
			{
				this.RegenerateCachedGridIfDirty();
				return this.markerGrid;
			}
		}

		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x06006595 RID: 26005 RVA: 0x00202D8E File Offset: 0x00200F8E
		public BoolGrid ReachableGrid
		{
			get
			{
				this.RegenerateCachedGridIfDirty();
				return this.reachableGrid;
			}
		}

		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x06006596 RID: 26006 RVA: 0x00202D9C File Offset: 0x00200F9C
		public Map Map
		{
			get
			{
				return this.map;
			}
		}

		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x06006597 RID: 26007 RVA: 0x00202DA4 File Offset: 0x00200FA4
		public int BreachRadius
		{
			get
			{
				return this.breachRadius;
			}
		}

		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x06006598 RID: 26008 RVA: 0x00202DAC File Offset: 0x00200FAC
		public Perlin Noise
		{
			get
			{
				Perlin result;
				if ((result = this.perlinCached) == null)
				{
					result = (this.perlinCached = BreachingGrid.CreatePerlinNoise(this.perlinSeed));
				}
				return result;
			}
		}

		// Token: 0x06006599 RID: 26009 RVA: 0x00202DD7 File Offset: 0x00200FD7
		public BreachingGrid()
		{
		}

		// Token: 0x0600659A RID: 26010 RVA: 0x00202DF8 File Offset: 0x00200FF8
		public BreachingGrid(Map map, Lord lord)
		{
			this.map = map;
			this.lord = lord;
			this.walkGrid = new BoolGrid(map);
			this.breachGrid = new BoolGrid(map);
			this.perlinSeed = Rand.Int;
		}

		// Token: 0x0600659B RID: 26011 RVA: 0x00202E55 File Offset: 0x00201055
		public static Perlin CreatePerlinNoise(int seed)
		{
			return new Perlin((double)BreachingGrid.perlinFrequency, (double)BreachingGrid.perlinLacunarity, (double)BreachingGrid.perlinPersistence, (int)BreachingGrid.perlinOctaves, seed, QualityMode.Medium);
		}

		// Token: 0x0600659C RID: 26012 RVA: 0x00202E76 File Offset: 0x00201076
		public void Notify_PawnStateChanged(Pawn pawn)
		{
			this.cachedGridsDirty = true;
		}

		// Token: 0x0600659D RID: 26013 RVA: 0x00202E76 File Offset: 0x00201076
		public void Notify_BuildingStateChanged(Building b)
		{
			this.cachedGridsDirty = true;
		}

		// Token: 0x0600659E RID: 26014 RVA: 0x00202E7F File Offset: 0x0020107F
		public bool WithinNoise(IntVec3 cell)
		{
			return this.Noise.GetValue(cell) >= BreachingGrid.perlinThres;
		}

		// Token: 0x0600659F RID: 26015 RVA: 0x00202E98 File Offset: 0x00201098
		public void CreateBreachPath(IntVec3 start, IntVec3 end, int breachRadius, int walkMargin, bool useAvoidGrid = false)
		{
			this.breachRadius = breachRadius;
			this.breachStart = start;
			PathFinderCostTuning pathFinderCostTuning = new PathFinderCostTuning
			{
				costBlockedDoor = BreachingGrid.tweakWallCost,
				costBlockedWallBase = BreachingGrid.tweakWallCost,
				costBlockedDoorPerHitPoint = BreachingGrid.tweakWallHpCost,
				costBlockedWallExtraPerHitPoint = BreachingGrid.tweakWallHpCost,
				costOffLordWalkGrid = BreachingGrid.tweakOffWalkGridPathCost,
				costBlockedWallExtraForNaturalWalls = BreachingGrid.tweakNaturalWallExtraCost
			};
			using (BreachingGrid.CustomTuning customTuning = new BreachingGrid.CustomTuning(this, pathFinderCostTuning))
			{
				TraverseParms traverseParms = TraverseParms.For(TraverseMode.PassAllDestroyableThings, Danger.Deadly, false, useAvoidGrid, false, true, false);
				using (PawnPath pawnPath = this.map.pathFinder.FindPathNow(start, end, traverseParms, new PathFinderCostTuning?(pathFinderCostTuning), PathEndMode.OnCell, customTuning))
				{
					foreach (IntVec3 c in pawnPath.NodesReversed)
					{
						this.breachGrid[c] = true;
						this.walkGrid[c] = true;
					}
				}
				for (int i = 0; i < breachRadius; i++)
				{
					this.WidenGrid(this.breachGrid);
					this.WidenGrid(this.walkGrid);
				}
				for (int j = 0; j < walkMargin; j++)
				{
					this.WidenGrid(this.walkGrid);
				}
			}
		}

		// Token: 0x060065A0 RID: 26016 RVA: 0x00203010 File Offset: 0x00201210
		public Thing FindBuildingToBreach()
		{
			Building bestBuilding = null;
			int bestBuildingDist = int.MaxValue;
			int bestBuildingReachableSideCount = 0;
			this.RegenerateCachedGridIfDirty();
			this.Map.floodFiller.FloodFill(this.breachStart, (IntVec3 c) => this.BreachGrid[c], delegate(IntVec3 c, int dist)
			{
				List<Thing> thingList = c.GetThingList(this.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Building building = thingList[i] as Building;
					if (building != null && BreachingUtility.ShouldBreachBuilding(building) && BreachingUtility.IsWorthBreachingBuilding(this, building))
					{
						int num = BreachingUtility.CountReachableAdjacentCells(this, building);
						if (num > 0 && num > bestBuildingReachableSideCount)
						{
							bestBuilding = building;
							bestBuildingDist = dist;
							bestBuildingReachableSideCount = num;
							break;
						}
					}
				}
				return dist - 2 > bestBuildingDist;
			}, int.MaxValue, false, null);
			return bestBuilding;
		}

		// Token: 0x060065A1 RID: 26017 RVA: 0x00203084 File Offset: 0x00201284
		private void WidenGrid(BoolGrid grid)
		{
			BreachingGrid.tmpWidenGrid.ClearAndResizeTo(this.map);
			foreach (IntVec3 a in grid.ActiveCells)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c = a + GenAdj.AdjacentCells[i];
					if (c.InBounds(this.map))
					{
						BreachingGrid.tmpWidenGrid[c] = true;
					}
				}
			}
			foreach (IntVec3 c2 in BreachingGrid.tmpWidenGrid.ActiveCells)
			{
				grid[c2] = true;
			}
		}

		// Token: 0x060065A2 RID: 26018 RVA: 0x00203158 File Offset: 0x00201358
		public void Reset()
		{
			this.breachGrid.Clear();
			this.walkGrid.Clear();
			ByteGrid byteGrid = this.markerGrid;
			if (byteGrid != null)
			{
				byteGrid.Clear(0);
			}
			BoolGrid boolGrid = this.reachableGrid;
			if (boolGrid != null)
			{
				boolGrid.Clear();
			}
			this.cachedGridsDirty = true;
			BreachingGridDebug.ClearDebugPath();
		}

		// Token: 0x060065A3 RID: 26019 RVA: 0x002031AA File Offset: 0x002013AA
		private void RegenerateCachedGridIfDirty()
		{
			if (this.cachedGridsDirty)
			{
				this.RegenerateCachedGrid();
			}
		}

		// Token: 0x060065A4 RID: 26020 RVA: 0x002031BC File Offset: 0x002013BC
		private void RegenerateCachedGrid()
		{
			this.cachedGridsDirty = false;
			if (this.markerGrid == null)
			{
				this.markerGrid = new ByteGrid(this.Map);
			}
			else
			{
				this.markerGrid.Clear(0);
			}
			if (this.reachableGrid == null)
			{
				this.reachableGrid = new BoolGrid(this.Map);
			}
			else
			{
				this.reachableGrid.Clear();
			}
			BreachingGrid.cachedWalkReachabilityPainter.PaintWalkReachability(this);
			if (this.lord != null)
			{
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					Pawn pawn = this.lord.ownedPawns[i];
					Pawn_MindState mindState = pawn.mindState;
					bool flag;
					if (mindState == null)
					{
						flag = (null != null);
					}
					else
					{
						BreachingTargetData breachingTarget = mindState.breachingTarget;
						flag = (((breachingTarget != null) ? breachingTarget.target : null) != null);
					}
					if (flag && !pawn.mindState.breachingTarget.target.Destroyed)
					{
						this.PaintDangerFromPawn(pawn);
					}
				}
			}
		}

		// Token: 0x060065A5 RID: 26021 RVA: 0x0020329C File Offset: 0x0020149C
		private void PaintDangerFromPawn(Pawn pawn)
		{
			BreachingTargetData breachingTarget = pawn.mindState.breachingTarget;
			if (breachingTarget == null)
			{
				return;
			}
			IntVec3 position = breachingTarget.target.Position;
			if (!position.IsValid)
			{
				return;
			}
			Verb verb = BreachingUtility.FindVerbToUseForBreaching(pawn);
			if (verb != null)
			{
				IntVec3 firingPosition = breachingTarget.firingPosition;
				if (firingPosition.IsValid)
				{
					if (this.markerGrid[firingPosition] == 0)
					{
						this.markerGrid[firingPosition] = 180;
					}
					this.VisitDangerousCellsOfAttack(firingPosition, position, verb, new Action<IntVec3>(this.<PaintDangerFromPawn>g__DangerSetter|56_0));
				}
			}
		}

		// Token: 0x060065A6 RID: 26022 RVA: 0x0020331D File Offset: 0x0020151D
		public void VisitDangerousCellsOfAttack(IntVec3 firingPosition, IntVec3 targetPosition, Verb verb, Action<IntVec3> visitor)
		{
			if (!verb.IsMeleeAttack)
			{
				BreachingGrid.cachedDangerLineOfSightPainter.PaintLoS(this.map, firingPosition, targetPosition, visitor);
				this.PaintSplashDamage(verb, targetPosition, visitor);
			}
		}

		// Token: 0x060065A7 RID: 26023 RVA: 0x00203348 File Offset: 0x00201548
		private void PaintSplashDamage(Verb verb, IntVec3 center, Action<IntVec3> visitor)
		{
			float num = 2f;
			ThingDef projectile = verb.GetProjectile();
			if (projectile != null && projectile.projectile.explosionRadius > 0f)
			{
				num = Mathf.Max(num, projectile.projectile.explosionRadius);
			}
			int num2 = GenRadial.NumCellsInRadius(num);
			for (int i = 0; i < num2; i++)
			{
				IntVec3 obj = (center + GenRadial.RadialPattern[i]).ClampInsideMap(this.map);
				visitor(obj);
			}
		}

		// Token: 0x060065A8 RID: 26024 RVA: 0x002033C8 File Offset: 0x002015C8
		private ushort DangerousRoomCost(Room room)
		{
			if (!room.Fogged)
			{
				return 0;
			}
			foreach (Thing thing in room.ContainedAndAdjacentThings)
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.mindState != null && !pawn.mindState.Active)
				{
					return 600;
				}
				if (thing.def == ThingDefOf.Hive)
				{
					return 600;
				}
				if (thing.def == ThingDefOf.AncientCryptosleepCasket)
				{
					return 600;
				}
			}
			return 0;
		}

		// Token: 0x060065A9 RID: 26025 RVA: 0x00203474 File Offset: 0x00201674
		public void ExposeData()
		{
			Scribe_Deep.Look<BoolGrid>(ref this.walkGrid, "walkGrid", Array.Empty<object>());
			Scribe_Deep.Look<BoolGrid>(ref this.breachGrid, "breachGrid", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.perlinSeed, "perlinSeed", 0, false);
			Scribe_Values.Look<int>(ref this.breachRadius, "breachRadius", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.breachStart, "breachStart", default(IntVec3), false);
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
		}

		// Token: 0x060065AB RID: 26027 RVA: 0x0020359E File Offset: 0x0020179E
		[CompilerGenerated]
		private void <PaintDangerFromPawn>g__DangerSetter|56_0(IntVec3 cell)
		{
			this.markerGrid[cell] = 10;
		}

		// Token: 0x040048AE RID: 18606
		public const byte Marker_FiringPosition = 180;

		// Token: 0x040048AF RID: 18607
		public const byte Marker_Dangerous = 10;

		// Token: 0x040048B0 RID: 18608
		public const byte Marker_UnUsed = 0;

		// Token: 0x040048B1 RID: 18609
		private const ushort DangerousRoomPathCost = 600;

		// Token: 0x040048B2 RID: 18610
		private static readonly BreachingGrid.WalkReachabilityPainter cachedWalkReachabilityPainter = new BreachingGrid.WalkReachabilityPainter();

		// Token: 0x040048B3 RID: 18611
		private static readonly BreachingGrid.DangerLineOfSightPainter cachedDangerLineOfSightPainter = new BreachingGrid.DangerLineOfSightPainter();

		// Token: 0x040048B4 RID: 18612
		private BoolGrid walkGrid;

		// Token: 0x040048B5 RID: 18613
		private BoolGrid breachGrid;

		// Token: 0x040048B6 RID: 18614
		private int perlinSeed;

		// Token: 0x040048B7 RID: 18615
		private int breachRadius = 1;

		// Token: 0x040048B8 RID: 18616
		private IntVec3 breachStart = IntVec3.Invalid;

		// Token: 0x040048B9 RID: 18617
		private Map map;

		// Token: 0x040048BA RID: 18618
		private Lord lord;

		// Token: 0x040048BB RID: 18619
		private Perlin perlinCached;

		// Token: 0x040048BC RID: 18620
		private BoolGrid reachableGrid;

		// Token: 0x040048BD RID: 18621
		private ByteGrid markerGrid;

		// Token: 0x040048BE RID: 18622
		private bool cachedGridsDirty = true;

		// Token: 0x040048BF RID: 18623
		[TweakValue("Breaching", 0f, 70f)]
		private static int tweakWallCost = 20;

		// Token: 0x040048C0 RID: 18624
		[TweakValue("Breaching", 0f, 1f)]
		private static float tweakWallHpCost = 0.015f;

		// Token: 0x040048C1 RID: 18625
		[TweakValue("Breaching", 0f, 100f)]
		private static bool tweakUsePerlin = true;

		// Token: 0x040048C2 RID: 18626
		[TweakValue("Breaching", -70f, 70f)]
		private static ushort tweakPerlinCost = 30;

		// Token: 0x040048C3 RID: 18627
		[TweakValue("Breaching", 1f, 7f)]
		public static int tweakOffWalkGridPathCost = 140;

		// Token: 0x040048C4 RID: 18628
		[TweakValue("Breaching", 0f, 100f)]
		private static bool tweakAvoidDangerousRooms = true;

		// Token: 0x040048C5 RID: 18629
		[TweakValue("Breaching", -70f, 70f)]
		private static int tweakNaturalWallExtraCost = 20;

		// Token: 0x040048C6 RID: 18630
		[TweakValue("Breaching", 0f, 0.1f)]
		private static float perlinFrequency = 0.06581f;

		// Token: 0x040048C7 RID: 18631
		[TweakValue("Breaching", 1f, 2f)]
		private static float perlinLacunarity = 1.5516f;

		// Token: 0x040048C8 RID: 18632
		[TweakValue("Breaching", 0f, 2f)]
		private static float perlinPersistence = 1.6569f;

		// Token: 0x040048C9 RID: 18633
		[TweakValue("Breaching", 1f, 5f)]
		private static float perlinOctaves = 4f;

		// Token: 0x040048CA RID: 18634
		[TweakValue("Breaching", 0f, 1f)]
		private static float perlinThres = 0.5f;

		// Token: 0x040048CB RID: 18635
		private static readonly BoolGrid tmpWidenGrid = new BoolGrid();

		// Token: 0x02001048 RID: 4168
		private class CustomTuning : PathRequest.IPathGridCustomizer, IDisposable
		{
			// Token: 0x060065AC RID: 26028 RVA: 0x002035AE File Offset: 0x002017AE
			public NativeArray<ushort> GetOffsetGrid()
			{
				return this.offsets;
			}

			// Token: 0x060065AD RID: 26029 RVA: 0x002035B6 File Offset: 0x002017B6
			public CustomTuning(BreachingGrid grid, PathFinderCostTuning tuning)
			{
				this.grid = grid;
				this.tuning = tuning;
				this.offsets = new NativeArray<ushort>(grid.map.cellIndices.NumGridCells, Allocator.TempJob, NativeArrayOptions.ClearMemory);
				this.SetupOffsetGrid();
			}

			// Token: 0x060065AE RID: 26030 RVA: 0x002035EF File Offset: 0x002017EF
			public void Dispose()
			{
				this.offsets.Dispose();
			}

			// Token: 0x060065AF RID: 26031 RVA: 0x002035FC File Offset: 0x002017FC
			private void SetupOffsetGrid()
			{
				this.offsets.Clear<ushort>();
				if (!BreachingGrid.tweakAvoidDangerousRooms)
				{
					return;
				}
				CellIndices cellIndices = this.grid.map.cellIndices;
				foreach (Room room in this.grid.map.regionGrid.AllRooms)
				{
					ushort num = this.grid.DangerousRoomCost(room);
					if (num != 0)
					{
						foreach (IntVec3 cell in room.Cells)
						{
							this.offsets[cellIndices[cell]] = num;
						}
						foreach (IntVec3 intVec in room.BorderCells)
						{
							if (intVec.InBounds(this.grid.map))
							{
								this.offsets[cellIndices[intVec]] = num;
							}
						}
					}
				}
				for (int i = 0; i < cellIndices.NumGridCells; i++)
				{
					if (BreachingGrid.tweakUsePerlin && this.grid.WithinNoise(cellIndices[i]))
					{
						ref NativeArray<ushort> ptr = ref this.offsets;
						int index = i;
						ptr[index] += BreachingGrid.tweakPerlinCost;
					}
					Building edifice = cellIndices[i].GetEdifice(this.grid.Map);
					if (edifice != null && PathUtility.IsDestroyable(edifice))
					{
						ref NativeArray<ushort> ptr = ref this.offsets;
						int index = i;
						ptr[index] += (ushort)((float)this.tuning.costBlockedWallBase + (float)edifice.HitPoints * this.tuning.costBlockedWallExtraPerHitPoint);
						ptr = ref this.offsets;
						index = i;
						ptr[index] += (ushort)(edifice.def.IsBuildingArtificial ? 0 : this.tuning.costBlockedWallExtraForNaturalWalls);
					}
				}
			}

			// Token: 0x040048CC RID: 18636
			private readonly BreachingGrid grid;

			// Token: 0x040048CD RID: 18637
			private NativeArray<ushort> offsets;

			// Token: 0x040048CE RID: 18638
			private readonly PathFinderCostTuning tuning;
		}

		// Token: 0x02001049 RID: 4169
		private class DangerLineOfSightPainter
		{
			// Token: 0x060065B0 RID: 26032 RVA: 0x0020384C File Offset: 0x00201A4C
			public DangerLineOfSightPainter()
			{
				this.skipThenVisitFunc = new Action<IntVec3>(this.SkipThenVisit);
			}

			// Token: 0x060065B1 RID: 26033 RVA: 0x00203866 File Offset: 0x00201A66
			private void SkipThenVisit(IntVec3 cell)
			{
				if (this.skipCount <= 0)
				{
					this.visitor(cell);
				}
				this.skipCount--;
			}

			// Token: 0x060065B2 RID: 26034 RVA: 0x0020388B File Offset: 0x00201A8B
			public void PaintLoS(Map map, IntVec3 start, IntVec3 end, Action<IntVec3> visitor)
			{
				if (!start.InBounds(map) || !end.InBounds(map))
				{
					return;
				}
				this.visitor = visitor;
				this.skipCount = Mathf.FloorToInt(5f);
				GenSight.PointsOnLineOfSight(start, end, this.skipThenVisitFunc);
			}

			// Token: 0x040048CF RID: 18639
			private Action<IntVec3> visitor;

			// Token: 0x040048D0 RID: 18640
			private int skipCount;

			// Token: 0x040048D1 RID: 18641
			private readonly Action<IntVec3> skipThenVisitFunc;
		}

		// Token: 0x0200104A RID: 4170
		private class WalkReachabilityPainter
		{
			// Token: 0x060065B3 RID: 26035 RVA: 0x002038C5 File Offset: 0x00201AC5
			public WalkReachabilityPainter()
			{
				this.floodFillPassCheckFunc = new Predicate<IntVec3>(this.FloodFillPassCheck);
				this.floodFillProcessorFunc = new Func<IntVec3, int, bool>(this.FloodFillProcessor);
			}

			// Token: 0x060065B4 RID: 26036 RVA: 0x002038F1 File Offset: 0x00201AF1
			public void PaintWalkReachability(BreachingGrid breachingGrid)
			{
				this.breachingGrid = breachingGrid;
				breachingGrid.map.floodFiller.FloodFill(this.breachingGrid.breachStart, this.floodFillPassCheckFunc, this.floodFillProcessorFunc, int.MaxValue, false, null);
			}

			// Token: 0x060065B5 RID: 26037 RVA: 0x00203928 File Offset: 0x00201B28
			private bool FloodFillProcessor(IntVec3 c, int traversalDist)
			{
				this.breachingGrid.reachableGrid[c] = true;
				return false;
			}

			// Token: 0x060065B6 RID: 26038 RVA: 0x0020393D File Offset: 0x00201B3D
			private bool FloodFillPassCheck(IntVec3 c)
			{
				return this.breachingGrid.WalkGrid[c] && !BreachingUtility.BlocksBreaching(this.breachingGrid.map, c);
			}

			// Token: 0x040048D2 RID: 18642
			private BreachingGrid breachingGrid;

			// Token: 0x040048D3 RID: 18643
			private readonly Predicate<IntVec3> floodFillPassCheckFunc;

			// Token: 0x040048D4 RID: 18644
			private readonly Func<IntVec3, int, bool> floodFillProcessorFunc;
		}
	}
}
