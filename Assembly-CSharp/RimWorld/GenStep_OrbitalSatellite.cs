using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FD5 RID: 8149
	public class GenStep_OrbitalSatellite : GenStep
	{
		// Token: 0x17001B5E RID: 7006
		// (get) Token: 0x0600B39D RID: 45981 RVA: 0x00337AFA File Offset: 0x00335CFA
		protected Faction Faction
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

		// Token: 0x17001B5F RID: 7007
		// (get) Token: 0x0600B39E RID: 45982 RVA: 0x00337B18 File Offset: 0x00335D18
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

		// Token: 0x17001B60 RID: 7008
		// (get) Token: 0x0600B39F RID: 45983 RVA: 0x00337B42 File Offset: 0x00335D42
		public override int SeedPart
		{
			get
			{
				return 749387154;
			}
		}

		// Token: 0x0600B3A0 RID: 45984 RVA: 0x00337B4C File Offset: 0x00335D4C
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Orbital Satellite"))
			{
				return;
			}
			CellRect cellRect = this.GeneratePlatform(map, parms);
			SpaceGenUtility.GenerateRing(map, cellRect, TerrainDefOf.OrbitalPlatform, this.bridgeDistance, 8, this.ringWidth, this.ringExtraOffset, this.ringExtraChance);
			this.GenerateSidePlatforms(cellRect, map, parms);
			map.FogOfWarColor = new Color?(ColorLibrary.SpaceHumanFog);
		}

		// Token: 0x0600B3A1 RID: 45985 RVA: 0x00337BB0 File Offset: 0x00335DB0
		private CellRect GeneratePlatform(Map map, GenStepParams parms)
		{
			int randomInRange = GenStep_OrbitalSatellite.CenterSizeRange.RandomInRange;
			IntVec2 size = new IntVec2(randomInRange, randomInRange);
			IntVec3 center = map.Center;
			Rot4 random = Rot4.Random;
			CellRect cellRect = center.RectAbout(size, random).ClipInsideMap(map);
			CellRect var = cellRect.ContractedBy(5);
			GenStep_OrbitalSatellite.SpawnPlatformTerrain(cellRect, map, 2);
			StructureGenParams parms2 = new StructureGenParams
			{
				size = var.Size
			};
			LayoutWorker worker = this.layoutDef.Worker;
			LayoutStructureSketch layoutStructureSketch = worker.GenerateStructureSketch(parms2);
			map.layoutStructureSketches.Add(layoutStructureSketch);
			LayoutStructureSketch layoutStructureSketch2 = layoutStructureSketch;
			IntVec3 min = var.Min;
			Faction faction = this.Faction;
			worker.Spawn(layoutStructureSketch2, map, min, null, null, true, false, faction);
			MapGenerator.SetVar<CellRect>("SpawnRect", var);
			return cellRect;
		}

		// Token: 0x0600B3A2 RID: 45986 RVA: 0x00337C6E File Offset: 0x00335E6E
		public override void PostMapInitialized(Map map, GenStepParams parms)
		{
			MapGenUtility.SetMapRoomTemperature(map, this.layoutDef, this.SpawnTemp);
			if (this.spawnSentryDrones)
			{
				BaseGenUtility.ScatterSentryDronesInMap(GenStep_OrbitalSatellite.SentryCountFromPointsCurve, map, this.Faction, parms);
			}
		}

		// Token: 0x0600B3A3 RID: 45987 RVA: 0x00337C9C File Offset: 0x00335E9C
		private void GenerateSidePlatforms(CellRect platformRect, Map map, GenStepParams parms)
		{
			int num = 0;
			int amountToPlace = Rand.Range(2, 4);
			int num2 = Rand.Range(0, 3);
			for (int i = 0; i < 4; i++)
			{
				Rot4 rot = new Rot4((i + num2) % 4);
				CellRect cellRect = new CellRect(0, 0, 23, 23);
				IntVec2 platformPosition = this.GetPlatformPosition(platformRect, cellRect, rot);
				cellRect = cellRect.MovedBy(platformPosition).ClipInsideMap(map);
				map.landingBlockers.Add(cellRect);
				GenStep_OrbitalSatellite.SpawnPlatformTerrain(cellRect, map, 1);
				if (Rand.DynamicChance(num, amountToPlace, 3 - i))
				{
					num++;
					GenStep_OrbitalSatellite.SpawnWalkway(platformRect, cellRect, rot, map);
				}
				this.SpawnPrefab(map, cellRect);
			}
		}

		// Token: 0x0600B3A4 RID: 45988 RVA: 0x00337D3C File Offset: 0x00335F3C
		private void SpawnPrefab(Map map, CellRect rect)
		{
			if (this.prefabs.NullOrEmpty<GenStep_OrbitalSatellite.LayoutPrefabParms>())
			{
				return;
			}
			GenStep_OrbitalSatellite.LayoutPrefabParms layoutPrefabParms = this.prefabs.RandomElementByWeight((GenStep_OrbitalSatellite.LayoutPrefabParms p) => p.weight);
			Rot4 random = Rot4.Random;
			IntVec3 centerCell = rect.CenterCell;
			if (rect.Width % 2 == 0)
			{
				centerCell.x--;
			}
			if (rect.Height % 2 == 0)
			{
				centerCell.z--;
			}
			for (int i = 0; i < 4; i++)
			{
				random.Rotate(RotationDirection.Clockwise);
				if (PrefabUtility.CanSpawnPrefab(layoutPrefabParms.def, map, centerCell, random, false, 0))
				{
					PrefabUtility.SpawnPrefab(layoutPrefabParms.def, map, centerCell, random, this.Faction, null, null, new Action<Thing>(GenStep_OrbitalSatellite.<SpawnPrefab>g__OnSpawned|28_1), false);
					return;
				}
			}
		}

		// Token: 0x0600B3A5 RID: 45989 RVA: 0x00337E08 File Offset: 0x00336008
		private static void SpawnWalkway(CellRect platform, CellRect side, Rot4 sideRot, Map map)
		{
			Rot4 opposite = sideRot.Opposite;
			int randomInRange = GenStep_OrbitalSatellite.PlatformConnectorHeightRange.RandomInRange;
			IntVec3 centerCellOnEdge = side.GetCenterCellOnEdge(opposite, -randomInRange);
			IntVec3 centerCellOnEdge2 = platform.GetCenterCellOnEdge(sideRot, randomInRange);
			CellRect cellRect = CellRect.FromLimits(centerCellOnEdge, centerCellOnEdge2).ClipInsideMap(map);
			CellRect cellRect2 = cellRect.ContractedBy(sideRot.IsVertical ? 1 : 0, sideRot.IsHorizontal ? 1 : 0);
			cellRect2 = cellRect2.ExpandedBy(sideRot.IsHorizontal ? 2 : 0, sideRot.IsVertical ? 2 : 0);
			foreach (IntVec3 c in cellRect)
			{
				if (c.GetTerrain(map) == TerrainDefOf.Space)
				{
					map.terrainGrid.SetTerrain(c, TerrainDefOf.OrbitalPlatform);
				}
			}
			foreach (IntVec3 c2 in cellRect2)
			{
				map.terrainGrid.SetTerrain(c2, TerrainDefOf.AncientTile);
			}
		}

		// Token: 0x0600B3A6 RID: 45990 RVA: 0x00337F40 File Offset: 0x00336140
		private IntVec2 GetPlatformPosition(CellRect centerRect, CellRect sideRect, Rot4 rot)
		{
			IntVec3 centerCell = centerRect.CenterCell;
			if (rot == Rot4.North)
			{
				return new IntVec2(centerCell.x - sideRect.Height / 2, centerRect.maxZ + this.bridgeDistance);
			}
			if (rot == Rot4.East)
			{
				return new IntVec2(centerRect.maxX + this.bridgeDistance, centerCell.z - sideRect.Width / 2);
			}
			if (rot == Rot4.South)
			{
				return new IntVec2(centerCell.x - sideRect.Height / 2, centerRect.minZ - this.bridgeDistance - sideRect.Height);
			}
			return new IntVec2(centerRect.minX - this.bridgeDistance - sideRect.Width, centerCell.z - sideRect.Height / 2);
		}

		// Token: 0x0600B3A7 RID: 45991 RVA: 0x00338018 File Offset: 0x00336218
		private static void SpawnPlatformTerrain(CellRect rect, Map map, int border = 1)
		{
			CellRect cellRect = rect.ContractedBy(border);
			foreach (IntVec3 c in rect)
			{
				map.terrainGrid.SetTerrain(c, TerrainDefOf.OrbitalPlatform);
				if (cellRect.Contains(c))
				{
					map.terrainGrid.SetTerrain(c, TerrainDefOf.AncientTile);
				}
			}
		}

		// Token: 0x0600B3AA RID: 45994 RVA: 0x00338170 File Offset: 0x00336370
		[CompilerGenerated]
		internal static void <SpawnPrefab>g__OnSpawned|28_1(Thing thing)
		{
			CompPowerBattery compPowerBattery;
			if (thing.TryGetComp(out compPowerBattery))
			{
				compPowerBattery.SetStoredEnergyPct(1f);
			}
			CompPowerTrader compPowerTrader;
			if (thing.TryGetComp(out compPowerTrader))
			{
				compPowerTrader.PowerOn = true;
			}
		}

		// Token: 0x04007B15 RID: 31509
		private LayoutDef layoutDef;

		// Token: 0x04007B16 RID: 31510
		private FactionDef factionDef;

		// Token: 0x04007B17 RID: 31511
		private List<GenStep_OrbitalSatellite.LayoutPrefabParms> prefabs = new List<GenStep_OrbitalSatellite.LayoutPrefabParms>();

		// Token: 0x04007B18 RID: 31512
		private float? temperature;

		// Token: 0x04007B19 RID: 31513
		private bool spawnSentryDrones;

		// Token: 0x04007B1A RID: 31514
		private int bridgeDistance = 34;

		// Token: 0x04007B1B RID: 31515
		private float ringWidth = 4.9f;

		// Token: 0x04007B1C RID: 31516
		private float ringExtraOffset = 0.5f;

		// Token: 0x04007B1D RID: 31517
		private float ringExtraChance;

		// Token: 0x04007B1E RID: 31518
		private const int ShipPadding = 5;

		// Token: 0x04007B1F RID: 31519
		private const int CenterPadding = 5;

		// Token: 0x04007B20 RID: 31520
		private static readonly IntRange CenterSizeRange = new IntRange(48, 65);

		// Token: 0x04007B21 RID: 31521
		private const int PlatformSize = 23;

		// Token: 0x04007B22 RID: 31522
		private const int RingSize = 8;

		// Token: 0x04007B23 RID: 31523
		private static readonly IntRange PlatformConnectorHeightRange = new IntRange(2, 2);

		// Token: 0x04007B24 RID: 31524
		private static readonly CellRect MinLandingArea = new CellRect(-10, -10, 20, 20);

		// Token: 0x04007B25 RID: 31525
		private static readonly SimpleCurve SentryCountFromPointsCurve = new SimpleCurve(new CurvePoint[]
		{
			new CurvePoint(0f, 0f),
			new CurvePoint(100f, 2f),
			new CurvePoint(1000f, 8f),
			new CurvePoint(5000f, 20f)
		});

		// Token: 0x02001FD6 RID: 8150
		public class LayoutPrefabParms
		{
			// Token: 0x0600B3AB RID: 45995 RVA: 0x002C52BF File Offset: 0x002C34BF
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				XmlHelper.ParseElements(this, xmlRoot, "def", "weight");
			}

			// Token: 0x04007B26 RID: 31526
			public PrefabDef def;

			// Token: 0x04007B27 RID: 31527
			public float weight = 1f;
		}
	}
}
