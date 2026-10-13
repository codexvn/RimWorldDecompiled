using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FD8 RID: 8152
	public class GenStep_OrbitalWreck : GenStep_BaseRuins
	{
		// Token: 0x17001B61 RID: 7009
		// (get) Token: 0x0600B3B0 RID: 46000 RVA: 0x003381CA File Offset: 0x003363CA
		protected override LayoutDef LayoutDef
		{
			get
			{
				return this.layouts.RandomElementByWeight((LayoutParms parms) => parms.weight).def;
			}
		}

		// Token: 0x17001B62 RID: 7010
		// (get) Token: 0x0600B3B1 RID: 46001 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool UseUsedRects
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001B63 RID: 7011
		// (get) Token: 0x0600B3B2 RID: 46002 RVA: 0x003381FB File Offset: 0x003363FB
		protected override Faction Faction
		{
			get
			{
				if (this.factionDef != null)
				{
					return Find.FactionManager.FirstFactionOfDef(this.factionDef);
				}
				return null;
			}
		}

		// Token: 0x17001B64 RID: 7012
		// (get) Token: 0x0600B3B3 RID: 46003 RVA: 0x0001B99B File Offset: 0x00019B9B
		protected override FloatRange DefaultMapFillPercentRange
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		// Token: 0x17001B65 RID: 7013
		// (get) Token: 0x0600B3B4 RID: 46004 RVA: 0x00091D22 File Offset: 0x0008FF22
		protected override FloatRange MergeRange
		{
			get
			{
				return FloatRange.One;
			}
		}

		// Token: 0x17001B66 RID: 7014
		// (get) Token: 0x0600B3B5 RID: 46005 RVA: 0x00338217 File Offset: 0x00336417
		public override int SeedPart
		{
			get
			{
				return 982581;
			}
		}

		// Token: 0x0600B3B6 RID: 46006 RVA: 0x0033821E File Offset: 0x0033641E
		protected override CellRect GetBounds(Map map)
		{
			return GenStep_OrbitalWreck.intBoundary.ContractedBy(2);
		}

		// Token: 0x0600B3B7 RID: 46007 RVA: 0x0033822B File Offset: 0x0033642B
		protected override IEnumerable<CellRect> GetRects(CellRect area, Map map)
		{
			int num = 0;
			int num2 = Mathf.RoundToInt(GenStep_OrbitalWreck.RuinCoverageRange.RandomInRange * (float)GenStep_OrbitalWreck.chunks.ApproximateArea);
			List<CellRect> list = new List<CellRect>();
			foreach (CellRect cellRect in from r in GenStep_OrbitalWreck.chunks.EnumeratedRects
			orderby Mathf.Abs(0.5f - (float)r.Height / (float)r.Area)
			select r)
			{
				CellRect item = cellRect;
				IntVec2 intVec = new IntVec2(Mathf.Min(cellRect.Size.x, GenStep_OrbitalWreck.MaxRuinSizeRange.RandomInRange), Mathf.Min(cellRect.Size.z, GenStep_OrbitalWreck.MaxRuinSizeRange.RandomInRange));
				if (intVec.x < cellRect.Size.x || intVec.z < cellRect.Size.z)
				{
					cellRect.TryFindRandomInnerRect(intVec, out item, null);
				}
				item = item.ContractedBy(GenStep_OrbitalWreck.RuinContractRange.RandomInRange, GenStep_OrbitalWreck.RuinContractRange.RandomInRange, GenStep_OrbitalWreck.RuinContractRange.RandomInRange, GenStep_OrbitalWreck.RuinContractRange.RandomInRange);
				list.Add(item);
				num += item.Area;
				if (num >= num2)
				{
					break;
				}
			}
			MapGenUtility.TryFixInvalidRects(list, map, this.MinRegionSize, this.UseUsedRects, this.MinAffordance, this.AvoidWaterRoads);
			MapGenUtility.RemoveInvalidRects(list, map, this.UseUsedRects, this.MinAffordance, this.AvoidWaterRoads);
			foreach (CellRect cellRect2 in list)
			{
				yield return cellRect2;
			}
			List<CellRect>.Enumerator enumerator2 = default(List<CellRect>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600B3B8 RID: 46008 RVA: 0x00009C2B File Offset: 0x00007E2B
		protected override IEnumerable<CellRect> GetRectOrder(IEnumerable<CellRect> rects, Map map)
		{
			return rects;
		}

		// Token: 0x0600B3B9 RID: 46009 RVA: 0x00338244 File Offset: 0x00336444
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Orbital Wreck"))
			{
				return;
			}
			using (map.pathing.DisableIncrementalScope())
			{
				this.GenerateSpawnBasePlatform(map);
			}
			int approximateArea = GenStep_OrbitalWreck.chunks.ApproximateArea;
			int maximum = Mathf.RoundToInt((float)approximateArea * GenStep_OrbitalWreck.DamagedMaxPercentageRange.RandomInRange);
			Chunks chunks = GenStep_OrbitalWreck.chunks;
			List<CellRect> spaces = GenStep_OrbitalWreck.importantRects;
			SpaceGenUtility.ScatterExtrusions(map, chunks, null, null, null, null, spaces);
			this.GenerateRuins(map, parms, this.DefaultMapFillPercentRange);
			this.PostStructuresDamage(map, maximum, ref approximateArea);
			SpaceGenUtility.ScatterPrefabs(map, GenStep_OrbitalWreck.chunks, this.Faction, this.prefabs, new Predicate<CellRect>(GenStep_OrbitalWreck.ValidRect), null);
			MapGenUtility.SpawnExteriorLumps(map, ThingDefOf.RubblePile, GenStep_OrbitalWreck.RubblePilesPer10K, GenStep_OrbitalWreck.RubblePileCountRange, GenStep_OrbitalWreck.RubblePileDistanceRange, 6);
			MapGenUtility.SpawnExteriorLumps(map, ThingDefOf.ChunkSlagSteel, GenStep_OrbitalWreck.ChunksPer10K, GenStep_OrbitalWreck.ChunksCountRange, GenStep_OrbitalWreck.ChunksDistanceRange, 6);
			GenStep_OrbitalWreck.importantRects.Clear();
			map.FogOfWarColor = new Color?(ColorLibrary.SpaceHumanFog);
			map.OrbitalDebris = this.orbitalDebrisDef;
		}

		// Token: 0x0600B3BA RID: 46010 RVA: 0x0033838C File Offset: 0x0033658C
		private void GenerateSpawnBasePlatform(Map map)
		{
			GenStep_OrbitalWreck.chunks = new Chunks(map.BoundsRect(20), 22);
			GenStep_OrbitalWreck.intBoundary = GenStep_OrbitalWreck.chunks.boundary;
			SpaceGenUtility.ChunksSetSparsley(GenStep_OrbitalWreck.chunks, GenStep_OrbitalWreck.InitialPercentageRange);
			foreach (CellRect cellRect in GenStep_OrbitalWreck.chunks.EnumeratedRects)
			{
				foreach (IntVec3 c in cellRect.Cells)
				{
					map.terrainGrid.SetTerrain(c, TerrainDefOf.OrbitalPlatform);
				}
				CellRect cellRect2 = cellRect;
				MapGenUtility.DoRectEdgeLumps(map, TerrainDefOf.OrbitalPlatform, ref cellRect2, GenStep_OrbitalWreck.PlatformPadBorderLumpLengthRange, GenStep_OrbitalWreck.PlatformPadBorderLumpOffsetRange, null);
			}
		}

		// Token: 0x0600B3BB RID: 46011 RVA: 0x0033846C File Offset: 0x0033666C
		protected override LayoutStructureSketch GenerateAndSpawn(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
		{
			LayoutStructureSketch layoutStructureSketch = base.GenerateAndSpawn(rect, map, parms, layoutDef);
			foreach (LayoutRoom layoutRoom in layoutStructureSketch.structureLayout.Rooms)
			{
				foreach (LayoutRoomDef layoutRoomDef in layoutRoom.defs)
				{
					if (!layoutRoomDef.canRemoveBorderDoors || !layoutRoomDef.canRemoveBorderWalls)
					{
						GenStep_OrbitalWreck.importantRects.AddRange(layoutRoom.rects);
						break;
					}
				}
			}
			MapGenerator.SetVar<CellRect>("SpawnRect", rect);
			MapGenerator.SetVar<CellRect>("RectOfInterest", rect);
			MapGenerator.UsedRects.Add(rect);
			return layoutStructureSketch;
		}

		// Token: 0x0600B3BC RID: 46012 RVA: 0x0033854C File Offset: 0x0033674C
		private void PostStructuresDamage(Map map, int maximum, ref int cells)
		{
			SpaceGenUtility.DamageHoles(GenStep_OrbitalWreck.chunks, map, maximum, ref cells, new IntRange?(GenStep_OrbitalWreck.HoleSizeRange), new FloatRange?(GenStep_OrbitalWreck.HolesPerHundredCellsRange), 1f, new Predicate<CellRect>(GenStep_OrbitalWreck.ValidRect), GenStep_OrbitalWreck.importantRects);
		}

		// Token: 0x0600B3BD RID: 46013 RVA: 0x00338590 File Offset: 0x00336790
		private static bool ValidRect(CellRect rect)
		{
			return SpaceGenUtility.NoIntersection(rect, GenStep_OrbitalWreck.importantRects, GenStep_OrbitalWreck.HoleLumpOffsetRange);
		}

		// Token: 0x04007B2A RID: 31530
		private FactionDef factionDef;

		// Token: 0x04007B2B RID: 31531
		private List<PrefabParms> prefabs = new List<PrefabParms>();

		// Token: 0x04007B2C RID: 31532
		private List<LayoutParms> layouts = new List<LayoutParms>();

		// Token: 0x04007B2D RID: 31533
		private OrbitalDebrisDef orbitalDebrisDef;

		// Token: 0x04007B2E RID: 31534
		private float? temperature;

		// Token: 0x04007B2F RID: 31535
		private static readonly List<CellRect> importantRects = new List<CellRect>();

		// Token: 0x04007B30 RID: 31536
		private static CellRect intBoundary;

		// Token: 0x04007B31 RID: 31537
		public static Chunks chunks;

		// Token: 0x04007B32 RID: 31538
		private const int MapBorderPadding = 20;

		// Token: 0x04007B33 RID: 31539
		private static readonly FloatRange InitialPercentageRange = new FloatRange(0.65f, 0.65f);

		// Token: 0x04007B34 RID: 31540
		private static readonly FloatRange DamagedMaxPercentageRange = new FloatRange(0.5f, 0.5f);

		// Token: 0x04007B35 RID: 31541
		private static readonly IntRange PlatformPadBorderLumpLengthRange = new IntRange(8, 12);

		// Token: 0x04007B36 RID: 31542
		private static readonly IntRange PlatformPadBorderLumpOffsetRange = new IntRange(-2, 3);

		// Token: 0x04007B37 RID: 31543
		private static readonly IntRange HoleLumpOffsetRange = new IntRange(-1, 2);

		// Token: 0x04007B38 RID: 31544
		private static readonly IntRange HoleSizeRange = new IntRange(3, 6);

		// Token: 0x04007B39 RID: 31545
		private static readonly FloatRange HolesPerHundredCellsRange = new FloatRange(0.025f, 0.025f);

		// Token: 0x04007B3A RID: 31546
		private static readonly FloatRange RubblePilesPer10K = new FloatRange(15f, 18f);

		// Token: 0x04007B3B RID: 31547
		private static readonly IntRange RubblePileCountRange = new IntRange(2, 12);

		// Token: 0x04007B3C RID: 31548
		private static readonly IntRange RubblePileDistanceRange = new IntRange(3, 10);

		// Token: 0x04007B3D RID: 31549
		private static readonly FloatRange ChunksPer10K = new FloatRange(1f, 2f);

		// Token: 0x04007B3E RID: 31550
		private static readonly IntRange ChunksCountRange = new IntRange(2, 4);

		// Token: 0x04007B3F RID: 31551
		private static readonly IntRange ChunksDistanceRange = new IntRange(3, 6);

		// Token: 0x04007B40 RID: 31552
		private static readonly IntRange MaxRuinSizeRange = new IntRange(60, 80);

		// Token: 0x04007B41 RID: 31553
		private static readonly IntRange RuinContractRange = new IntRange(1, 2);

		// Token: 0x04007B42 RID: 31554
		private static readonly FloatRange RuinCoverageRange = new FloatRange(0.6f, 0.65f);

		// Token: 0x04007B43 RID: 31555
		private const int PlatformRegionSize = 22;
	}
}
