using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020020E6 RID: 8422
	public class TileMutatorWorker_Harbor : TileMutatorWorker
	{
		// Token: 0x0600B803 RID: 47107 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_Harbor(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B804 RID: 47108 RVA: 0x00350C58 File Offset: 0x0034EE58
		public override bool IsValidTile(PlanetTile tile, PlanetLayer layer)
		{
			return Find.FactionManager.GetFactions(false, false, true, TechLevel.Undefined, false).Any<Faction>() && base.IsValidTile(tile, layer);
		}

		// Token: 0x0600B805 RID: 47109 RVA: 0x00350C7C File Offset: 0x0034EE7C
		public override void GenerateCriticalStructures(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			List<CellRect> usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			Func<IntVec3, bool> <>9__1;
			CellRect cellRect;
			if (!MapGenUtility.TryGetClosestClearRectTo(out cellRect, new IntVec2(16, 16), map.Center, delegate(CellRect r)
			{
				IEnumerable<IntVec3> cells = r.Cells;
				Func<IntVec3, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((IntVec3 c) => c.GetTerrain(map).IsWater));
				}
				return !cells.Any(predicate) && !usedRects.Any((CellRect ur) => ur.Overlaps(r) && ur.CenterCell.InHorDistOf(map.Center, 75f));
			}, -1f, 0.7f, 8))
			{
				IntVec3 center;
				if (!CellFinder.TryFindRandomCellNear(map.Center, map, 75, (IntVec3 c) => !c.GetTerrain(map).IsWater && c.GetEdifice(map) == null, out center, -1))
				{
					Log.Error("Failed to find location for harbor");
					return;
				}
				cellRect = CellRect.CenteredOn(center, 8);
			}
			Faction parentFaction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				Find.FactionManager.GetFactions(false, false, true, TechLevel.Undefined, false).TryRandomElement(out parentFaction);
			}
			else
			{
				parentFaction = map.ParentFaction;
			}
			if (parentFaction == null)
			{
				return;
			}
			MapGenUtility.PostProcessSettlementParams postProcessSettlementParams = new MapGenUtility.PostProcessSettlementParams
			{
				clearBuildingFaction = true,
				faction = parentFaction,
				damageBuildings = true,
				canDamageWalls = false,
				noFuel = true,
				ageCorpses = true
			};
			new GenStep_Outpost
			{
				size = 16,
				forcedRect = new CellRect?(cellRect),
				overrideFaction = parentFaction,
				postProcessSettlementParams = postProcessSettlementParams,
				settlementDontGeneratePawns = true,
				generateLoot = false
			}.Generate(map, default(GenStepParams));
			List<IntVec3> list = new List<IntVec3>();
			IntVec3 centerCell = cellRect.CenterCell;
			this.waterDir = Rot4.FromAngleFlat((map.Center - centerCell).AngleFlat);
			IntVec3 b;
			int num2;
			if (this.waterDir == Rot4.North || this.waterDir == Rot4.South)
			{
				int num = centerCell.x - map.Center.x;
				b = ((num > 0) ? Rot4.West.AsIntVec3 : Rot4.East.AsIntVec3);
				num2 = Mathf.Abs(num);
			}
			else
			{
				int num3 = centerCell.z - map.Center.z;
				b = ((num3 > 0) ? Rot4.South.AsIntVec3 : Rot4.North.AsIntVec3);
				num2 = Mathf.Abs(num3);
			}
			IntVec3 intVec = centerCell;
			for (int i = 0; i < num2; i++)
			{
				list.Add(intVec);
				intVec += b;
			}
			int num4 = 99;
			while (!intVec.GetTerrain(map).IsOcean || num4-- < 0)
			{
				list.Add(intVec);
				intVec += this.waterDir.AsIntVec3;
			}
			this.ExtendDock(intVec, map, list, TileMutatorWorker_Harbor.MainDockSectionLengthRange.RandomInRange, this.waterDir, 1f, true);
			list = this.FindDockStartPoint(list, cellRect);
			foreach (IntVec3 cell in list)
			{
				TileMutatorWorker_Harbor.SetDockAround(map, cell);
			}
		}

		// Token: 0x0600B806 RID: 47110 RVA: 0x00350FAC File Offset: 0x0034F1AC
		private List<IntVec3> FindDockStartPoint(List<IntVec3> path, CellRect outpostRect)
		{
			path.Reverse();
			for (int i = 0; i < path.Count; i++)
			{
				IntVec3 a = path[i];
				foreach (IntVec3 b in GenAdj.AdjacentCellsAndInside)
				{
					if (outpostRect.Contains(a + b))
					{
						path = path.GetRange(0, i);
						path.Reverse();
						return path;
					}
				}
			}
			path.Reverse();
			return path;
		}

		// Token: 0x0600B807 RID: 47111 RVA: 0x00351020 File Offset: 0x0034F220
		private bool CheckCollision(IntVec3 checkCell, Rot4 dir, Map map, List<IntVec3> path)
		{
			TileMutatorWorker_Harbor.<>c__DisplayClass10_0 CS$<>8__locals1;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.path = path;
			return !TileMutatorWorker_Harbor.<CheckCollision>g__ValidateCell|10_0(checkCell, dir, ref CS$<>8__locals1) || (!TileMutatorWorker_Harbor.<CheckCollision>g__ValidateCell|10_0(checkCell, dir.Rotated(RotationDirection.Clockwise), ref CS$<>8__locals1) || !TileMutatorWorker_Harbor.<CheckCollision>g__ValidateCell|10_0(checkCell, dir.Rotated(RotationDirection.Counterclockwise), ref CS$<>8__locals1));
		}

		// Token: 0x0600B808 RID: 47112 RVA: 0x00351074 File Offset: 0x0034F274
		private void ExtendDock(IntVec3 startCell, Map map, List<IntVec3> path, int length, Rot4 direction, float branchChance, bool mainBranch)
		{
			int num = TileMutatorWorker_Harbor.BranchDistRange.RandomInRange;
			for (int i = 0; i < length; i++)
			{
				IntVec3 intVec = startCell + direction.AsIntVec3 * i;
				if (intVec.InBounds(map))
				{
					if (this.CheckCollision(intVec, direction, map, path))
					{
						if (mainBranch && i > 10)
						{
							break;
						}
						if (!mainBranch && i > 0 && this.CheckCollision(intVec, direction, map, path))
						{
							for (int j = i; j > 1; j--)
							{
								path.Pop<IntVec3>();
							}
							return;
						}
					}
					path.Add(intVec);
					num--;
					if (num == 0)
					{
						num = TileMutatorWorker_Harbor.BranchDistRange.RandomInRange;
						if (Rand.Chance(branchChance))
						{
							Rot4 rot;
							do
							{
								rot = direction.Rotated(Rand.Bool ? RotationDirection.Clockwise : RotationDirection.Counterclockwise);
							}
							while (rot == this.waterDir.Opposite);
							this.ExtendDock(intVec, map, path, TileMutatorWorker_Harbor.DockSectionLengthRange.RandomInRange, rot, branchChance - 0.7f, false);
							if (mainBranch && Rand.Chance(0.75f))
							{
								this.ExtendDock(intVec, map, path, TileMutatorWorker_Harbor.DockSectionLengthRange.RandomInRange, rot.Opposite, branchChance - 0.7f, false);
							}
							else if (Rand.Bool)
							{
								path.Add(intVec + rot.Opposite.AsIntVec3);
								path.Add(intVec + rot.Opposite.AsIntVec3 * 2);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B809 RID: 47113 RVA: 0x003511FC File Offset: 0x0034F3FC
		private static void SetDockAround(Map map, IntVec3 cell)
		{
			foreach (IntVec3 b in GenAdj.AdjacentCellsAndInside)
			{
				IntVec3 c = cell + b;
				if (c.InBounds(map))
				{
					TerrainDef terrain = c.GetTerrain(map);
					if (!terrain.IsFloor)
					{
						Building edifice = c.GetEdifice(map);
						if (edifice != null)
						{
							if (edifice.def.building.isNaturalRock)
							{
								edifice.Destroy(DestroyMode.Vanish);
								map.roofGrid.SetRoof(c, null);
							}
							else if (edifice.def.fillPercent >= 0.99f)
							{
								goto IL_A3;
							}
						}
						map.terrainGrid.SetTerrain(c, terrain.IsWater ? TerrainDefOf.Bridge : TerrainDefOf.PackedDirt);
					}
				}
				IL_A3:;
			}
		}

		// Token: 0x0600B80B RID: 47115 RVA: 0x003512E4 File Offset: 0x0034F4E4
		[CompilerGenerated]
		internal static bool <CheckCollision>g__ValidateCell|10_0(IntVec3 cell, Rot4 dir, ref TileMutatorWorker_Harbor.<>c__DisplayClass10_0 A_2)
		{
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = cell + dir.AsIntVec3 * i;
				if (!intVec.GetTerrain(A_2.map).IsOcean || A_2.path.Contains(intVec))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04007EB0 RID: 32432
		private const int OutpostSize = 16;

		// Token: 0x04007EB1 RID: 32433
		private const int MaxDistFromCenter = 75;

		// Token: 0x04007EB2 RID: 32434
		private static readonly IntRange MainDockSectionLengthRange = new IntRange(19, 27);

		// Token: 0x04007EB3 RID: 32435
		private static readonly IntRange DockSectionLengthRange = new IntRange(8, 11);

		// Token: 0x04007EB4 RID: 32436
		private static readonly IntRange BranchDistRange = new IntRange(9, 11);

		// Token: 0x04007EB5 RID: 32437
		private Rot4 waterDir;
	}
}
