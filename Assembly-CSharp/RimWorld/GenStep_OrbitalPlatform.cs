using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld.BaseGen;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FCC RID: 8140
	public class GenStep_OrbitalPlatform : GenStep
	{
		// Token: 0x17001B5A RID: 7002
		// (get) Token: 0x0600B36F RID: 45935 RVA: 0x0033694F File Offset: 0x00334B4F
		protected virtual LayoutDef LayoutDef
		{
			get
			{
				return this.layoutDef;
			}
		}

		// Token: 0x17001B5B RID: 7003
		// (get) Token: 0x0600B370 RID: 45936 RVA: 0x00336958 File Offset: 0x00334B58
		protected virtual float SpawnTemp
		{
			get
			{
				float? num = this.temperature;
				if (num == null)
				{
					return -75f;
				}
				return num.GetValueOrDefault();
			}
		}

		// Token: 0x17001B5C RID: 7004
		// (get) Token: 0x0600B371 RID: 45937 RVA: 0x00336982 File Offset: 0x00334B82
		public override int SeedPart
		{
			get
			{
				return 8256151;
			}
		}

		// Token: 0x0600B372 RID: 45938 RVA: 0x00336989 File Offset: 0x00334B89
		protected virtual Faction GetFaction(Map map)
		{
			if (this.useSiteFaction && map.Parent.Faction != null)
			{
				return map.Parent.Faction;
			}
			if (this.factionDef != null)
			{
				return Find.FactionManager.FirstFactionOfDef(this.factionDef);
			}
			return null;
		}

		// Token: 0x0600B373 RID: 45939 RVA: 0x003369C8 File Offset: 0x00334BC8
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Orbital Platform"))
			{
				return;
			}
			float? threatPoints = null;
			if (parms.sitePart != null)
			{
				threatPoints = new float?(parms.sitePart.parms.points);
			}
			if (threatPoints == null)
			{
				Site site = map.Parent as Site;
				if (site != null)
				{
					threatPoints = new float?(site.ActualThreatPoints);
				}
			}
			Faction faction = this.GetFaction(map);
			CellRect rect = this.GeneratePlatform(map, faction, threatPoints);
			if (Rand.Chance(0.33f))
			{
				this.DoRing(map, rect);
			}
			else if (Rand.Chance(0.5f))
			{
				this.DoLargePlatforms(map, rect);
			}
			else
			{
				this.DoSmallPlatforms(map, rect);
			}
			this.SpawnCannons(map, rect.ExpandedBy(6));
			map.FogOfWarColor = new Color?(this.fogOfWarColor.ToColor);
			map.OrbitalDebris = this.orbitalDebrisDef;
			this.SpawnExteriorPrefabs(map, rect.ExpandedBy(6), faction);
		}

		// Token: 0x0600B374 RID: 45940 RVA: 0x00336AB8 File Offset: 0x00334CB8
		private void DoLargePlatforms(Map map, CellRect rect)
		{
			int randomInRange = GenStep_OrbitalPlatform.LargeDockRange.RandomInRange;
			List<Rot4> list = new List<Rot4>
			{
				Rot4.North,
				Rot4.East,
				Rot4.South,
				Rot4.West
			};
			int num = 0;
			while (num < randomInRange && list.Any<Rot4>())
			{
				Rot4 rot = list.RandomElement<Rot4>();
				list.Remove(rot);
				SpaceGenUtility.GenerateConnectedPlatform(map, this.platformTerrain, rect, GenStep_OrbitalPlatform.LargeLandingAreaWidthRange, GenStep_OrbitalPlatform.LargeLandingAreaHeightRange, rot, 14, 0.2f, 2, null, null, null, null);
				num++;
			}
		}

		// Token: 0x0600B375 RID: 45941 RVA: 0x00336B74 File Offset: 0x00334D74
		private void DoSmallPlatforms(Map map, CellRect rect)
		{
			ValueTuple<CellRect, CellRect, CellRect, CellRect> valueTuple = rect.Subdivide(1);
			int randomInRange = GenStep_OrbitalPlatform.SmallPlatformRange.RandomInRange;
			List<ValueTuple<CellRect, Rot4>> list = new List<ValueTuple<CellRect, Rot4>>
			{
				new ValueTuple<CellRect, Rot4>(valueTuple.Item1, Rot4.South),
				new ValueTuple<CellRect, Rot4>(valueTuple.Item1, Rot4.West),
				new ValueTuple<CellRect, Rot4>(valueTuple.Item3, Rot4.South),
				new ValueTuple<CellRect, Rot4>(valueTuple.Item3, Rot4.East),
				new ValueTuple<CellRect, Rot4>(valueTuple.Item2, Rot4.North),
				new ValueTuple<CellRect, Rot4>(valueTuple.Item2, Rot4.West),
				new ValueTuple<CellRect, Rot4>(valueTuple.Item4, Rot4.North),
				new ValueTuple<CellRect, Rot4>(valueTuple.Item4, Rot4.East)
			};
			int num = 0;
			while (num < randomInRange && list.Any<ValueTuple<CellRect, Rot4>>())
			{
				ValueTuple<CellRect, Rot4> valueTuple2 = list.RandomElement<ValueTuple<CellRect, Rot4>>();
				list.Remove(valueTuple2);
				ValueTuple<CellRect, Rot4> valueTuple3 = valueTuple2;
				CellRect item = valueTuple3.Item1;
				Rot4 item2 = valueTuple3.Item2;
				SpaceGenUtility.GenerateConnectedPlatform(map, this.platformTerrain, item, GenStep_OrbitalPlatform.SmallPlatformSizeRange, GenStep_OrbitalPlatform.SmallPlatformSizeRange, item2, GenStep_OrbitalPlatform.SmallPlatformDistanceRange.RandomInRange, 0.2f, 2, null, null, null, null);
				num++;
			}
		}

		// Token: 0x0600B376 RID: 45942 RVA: 0x00336CE4 File Offset: 0x00334EE4
		private void DoRing(Map map, CellRect rect)
		{
			float num = Mathf.Sqrt((float)(rect.Width * rect.Width + rect.Height * rect.Height)) - (float)rect.Width - 12f;
			SpaceGenUtility.GenerateRing(map, rect, this.platformTerrain, Mathf.RoundToInt(num / 2f), 0, 13.9f, 0.5f, 0f);
		}

		// Token: 0x0600B377 RID: 45943 RVA: 0x00336D50 File Offset: 0x00334F50
		private void SpawnCannons(Map map, CellRect rect)
		{
			if (this.cannonDef == null)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				Rot4 rot = new Rot4(i);
				IntVec3 corner = rect.GetCorner(rot, true);
				int num = Mathf.Max(this.cannonDef.size.x, this.cannonDef.size.z) + 4;
				CellRect item = corner.RectAbout(num, num);
				MapGenUtility.Line(this.platformTerrain, map, corner, rect.CenterCell, 6f, true);
				foreach (IntVec3 c in item.Cells)
				{
					if (c.InHorDistOf(corner, (float)num / 2f - 0.6f))
					{
						map.terrainGrid.SetTerrain(c, TerrainDefOf.AncientTile);
					}
					else if (c.InHorDistOf(corner, (float)num / 2f + 0.5f))
					{
						map.terrainGrid.SetTerrain(c, this.platformTerrain);
					}
				}
				GenSpawn.Spawn(ThingMaker.MakeThing(this.cannonDef, null), corner, map, new Rot4(i % 4), WipeMode.Vanish, false, false);
				MapGenerator.UsedRects.Add(item);
			}
		}

		// Token: 0x0600B378 RID: 45944 RVA: 0x00336E98 File Offset: 0x00335098
		private void SpawnExteriorPrefabs(Map map, CellRect rect, Faction faction)
		{
			GenStep_OrbitalPlatform.<>c__DisplayClass33_0 CS$<>8__locals1 = new GenStep_OrbitalPlatform.<>c__DisplayClass33_0();
			CS$<>8__locals1.rect = rect;
			CS$<>8__locals1.map = map;
			using (List<GenStep_OrbitalPlatform.PrefabRange>.Enumerator enumerator = this.exteriorPrefabs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GenStep_OrbitalPlatform.<>c__DisplayClass33_1 CS$<>8__locals2 = new GenStep_OrbitalPlatform.<>c__DisplayClass33_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.set = enumerator.Current;
					int randomInRange = CS$<>8__locals2.set.countRange.RandomInRange;
					for (int i = 0; i < randomInRange; i++)
					{
						IntVec3 intVec;
						if (CS$<>8__locals2.CS$<>8__locals1.rect.TryFindRandomCell(out intVec, new Predicate<IntVec3>(CS$<>8__locals2.<SpawnExteriorPrefabs>g__Validator|0)))
						{
							Rot4 opposite = CS$<>8__locals2.CS$<>8__locals1.rect.GetClosestEdge(intVec).Opposite;
							PrefabUtility.SpawnPrefab(CS$<>8__locals2.set.prefab, CS$<>8__locals2.CS$<>8__locals1.map, intVec, opposite, faction, null, null, null, false);
						}
					}
				}
			}
		}

		// Token: 0x0600B379 RID: 45945 RVA: 0x00336F94 File Offset: 0x00335194
		private CellRect GeneratePlatform(Map map, Faction faction, float? threatPoints)
		{
			IntVec2 size = new IntVec2(GenStep_OrbitalPlatform.SizeRange.RandomInRange, GenStep_OrbitalPlatform.SizeRange.RandomInRange);
			Rot4 random = Rot4.Random;
			CellRect cellRect = map.Center.RectAbout(size, random).ClipInsideMap(map);
			StructureGenParams parms = new StructureGenParams
			{
				size = cellRect.Size
			};
			LayoutWorker worker = this.LayoutDef.Worker;
			LayoutStructureSketch layoutStructureSketch = worker.GenerateStructureSketch(parms);
			map.layoutStructureSketches.Add(layoutStructureSketch);
			worker.Spawn(layoutStructureSketch, map, cellRect.Min, threatPoints, null, true, false, faction);
			MapGenerator.SetVar<CellRect>("SpawnRect", cellRect);
			MapGenerator.UsedRects.Add(cellRect);
			return cellRect;
		}

		// Token: 0x0600B37A RID: 45946 RVA: 0x00337040 File Offset: 0x00335240
		public override void PostMapInitialized(Map map, GenStepParams parms)
		{
			MapGenUtility.SetMapRoomTemperature(map, this.layoutDef, this.SpawnTemp);
			if (this.spawnSentryDrones)
			{
				BaseGenUtility.ScatterSentryDronesInMap(GenStep_OrbitalPlatform.SentryCountFromPointsCurve, map, this.GetFaction(map), parms);
			}
		}

		// Token: 0x04007AE9 RID: 31465
		private FactionDef factionDef;

		// Token: 0x04007AEA RID: 31466
		private LayoutDef layoutDef;

		// Token: 0x04007AEB RID: 31467
		private bool useSiteFaction;

		// Token: 0x04007AEC RID: 31468
		private float? temperature;

		// Token: 0x04007AED RID: 31469
		private bool spawnSentryDrones;

		// Token: 0x04007AEE RID: 31470
		private ThingDef cannonDef;

		// Token: 0x04007AEF RID: 31471
		private TerrainDef platformTerrain;

		// Token: 0x04007AF0 RID: 31472
		private OrbitalDebrisDef orbitalDebrisDef;

		// Token: 0x04007AF1 RID: 31473
		private ColorInt fogOfWarColor = new ColorInt(43, 46, 47);

		// Token: 0x04007AF2 RID: 31474
		private List<GenStep_OrbitalPlatform.PrefabRange> exteriorPrefabs = new List<GenStep_OrbitalPlatform.PrefabRange>();

		// Token: 0x04007AF3 RID: 31475
		private static readonly IntRange LargeDockRange = new IntRange(1, 2);

		// Token: 0x04007AF4 RID: 31476
		private static readonly IntRange SmallPlatformRange = new IntRange(4, 6);

		// Token: 0x04007AF5 RID: 31477
		private static readonly IntRange SmallPlatformSizeRange = new IntRange(16, 20);

		// Token: 0x04007AF6 RID: 31478
		private static readonly IntRange SmallPlatformDistanceRange = new IntRange(10, 18);

		// Token: 0x04007AF7 RID: 31479
		private static readonly IntRange SizeRange = new IntRange(70, 80);

		// Token: 0x04007AF8 RID: 31480
		private static readonly IntRange LargeLandingAreaWidthRange = new IntRange(30, 40);

		// Token: 0x04007AF9 RID: 31481
		private static readonly IntRange LargeLandingAreaHeightRange = new IntRange(50, 60);

		// Token: 0x04007AFA RID: 31482
		public static readonly IntRange LandingPadBorderLumpLengthRange = new IntRange(6, 10);

		// Token: 0x04007AFB RID: 31483
		public static readonly IntRange LandingPadBorderLumpOffsetRange = new IntRange(-1, 1);

		// Token: 0x04007AFC RID: 31484
		private static readonly SimpleCurve SentryCountFromPointsCurve = new SimpleCurve(new CurvePoint[]
		{
			new CurvePoint(0f, 0f),
			new CurvePoint(100f, 2f),
			new CurvePoint(1000f, 8f),
			new CurvePoint(5000f, 16f)
		});

		// Token: 0x02001FCD RID: 8141
		private class PrefabRange
		{
			// Token: 0x0600B37D RID: 45949 RVA: 0x00337180 File Offset: 0x00335380
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				XmlHelper.ParseElements(this, xmlRoot, "prefab", "countRange");
			}

			// Token: 0x04007AFD RID: 31485
			public PrefabDef prefab;

			// Token: 0x04007AFE RID: 31486
			public IntRange countRange;
		}
	}
}
