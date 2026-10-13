using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002000 RID: 8192
	public class GenStep_ScatterRoadDebris : GenStep_Scatterer
	{
		// Token: 0x17001B7D RID: 7037
		// (get) Token: 0x0600B477 RID: 46199 RVA: 0x0033C622 File Offset: 0x0033A822
		public override int SeedPart
		{
			get
			{
				return 765346456;
			}
		}

		// Token: 0x17001B7E RID: 7038
		// (get) Token: 0x0600B478 RID: 46200 RVA: 0x0033C629 File Offset: 0x0033A829
		public IEnumerable<ThingDef> Debris
		{
			get
			{
				yield return ThingDefOf.AncientRustedCarFrame;
				yield return ThingDefOf.AncientRustedJeep;
				yield return ThingDefOf.AncientRustedCar;
				yield return ThingDefOf.AncientTank;
				yield return ThingDefOf.AncientPodCar;
				yield return ThingDefOf.AncientHydrant;
				yield break;
			}
		}

		// Token: 0x0600B479 RID: 46201 RVA: 0x0033C634 File Offset: 0x0033A834
		protected override float GetPlacementFactor(Map map)
		{
			float num = 1f;
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				num *= tileMutatorDef.junkDensityFactor;
			}
			return num;
		}

		// Token: 0x0600B47A RID: 46202 RVA: 0x0033C690 File Offset: 0x0033A890
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter road debris"))
			{
				return;
			}
			this.allowInWaterBiome = false;
			this.mapHasRoads = this.HasAncientRoads(map);
			this.count = (this.mapHasRoads ? GenStep_ScatterRoadDebris.VehicleRangeRoadMap : GenStep_ScatterRoadDebris.VehicleRangeNonRoadMap).RandomInRange;
			this.thingToPlace = this.Debris.RandomElement<ThingDef>();
			base.Generate(map, parms);
		}

		// Token: 0x0600B47B RID: 46203 RVA: 0x0033C6FC File Offset: 0x0033A8FC
		private bool HasAncientRoads(Map map)
		{
			SurfaceTile surfaceTile;
			if (map.TileInfo.Isnt(out surfaceTile) || surfaceTile.Roads == null)
			{
				return false;
			}
			for (int i = 0; i < surfaceTile.Roads.Count; i++)
			{
				if (surfaceTile.Roads[i].road == RoadDefOf.AncientAsphaltHighway || surfaceTile.Roads[i].road == RoadDefOf.AncientAsphaltRoad)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B47C RID: 46204 RVA: 0x0033C76C File Offset: 0x0033A96C
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (this.mapHasRoads && !c.GetTerrain(map).IsRoad)
			{
				return false;
			}
			if (this.thingToPlace.rotatable)
			{
				int num = Rand.RangeInclusive(1, 4);
				for (int i = 0; i < 4; i++)
				{
					this.rotation = new Rot4((i + num) % 4);
					if (this.CanPlaceThingAt(c, this.rotation, map, this.thingToPlace))
					{
						return true;
					}
				}
				return false;
			}
			this.rotation = this.thingToPlace.defaultPlacingRot;
			return this.CanPlaceThingAt(c, this.rotation, map, this.thingToPlace);
		}

		// Token: 0x0600B47D RID: 46205 RVA: 0x0033B0CF File Offset: 0x003392CF
		private bool CanPlaceThingAt(IntVec3 c, Rot4 rot, Map map, ThingDef thingDef)
		{
			return ScatterDebrisUtility.CanPlaceThingAt(c, rot, map, thingDef);
		}

		// Token: 0x0600B47E RID: 46206 RVA: 0x0033C810 File Offset: 0x0033AA10
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(this.thingToPlace, null), loc, map, this.rotation, WipeMode.Vanish, false, false);
			ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_MachineBits, 0.75f, 1, int.MaxValue, null);
			ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_OilSmear, 0.25f, 0, int.MaxValue, null);
			ThingDef engineBlockDef = Rand.Bool ? ThingDefOf.AncientRustedEngineBlock : ThingDefOf.AncientLargeRustedEngineBlock;
			IntVec3 loc2;
			if (Rand.Chance(0.5f) && RCellFinder.TryFindRandomCellNearWith(loc, (IntVec3 c) => this.CanPlaceThingAt(c, Rot4.North, map, engineBlockDef), map, out loc2, 10, 2147483647))
			{
				ScatterDebrisUtility.ScatterFilthAroundThing(GenSpawn.Spawn(ThingMaker.MakeThing(engineBlockDef, null), loc2, map, Rot4.North, WipeMode.Vanish, false, false), map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
				ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_OilSmear, 0.15f, 0, int.MaxValue, null);
			}
			this.thingToPlace = this.Debris.RandomElement<ThingDef>();
		}

		// Token: 0x04007BB9 RID: 31673
		private const int EngineBlockMinDistance = 10;

		// Token: 0x04007BBA RID: 31674
		private const float EngineBlockChance = 0.5f;

		// Token: 0x04007BBB RID: 31675
		private const float OilSmearChance = 0.15f;

		// Token: 0x04007BBC RID: 31676
		private static readonly IntRange VehicleRangeNonRoadMap = new IntRange(1, 2);

		// Token: 0x04007BBD RID: 31677
		private static readonly IntRange VehicleRangeRoadMap = new IntRange(2, 3);

		// Token: 0x04007BBE RID: 31678
		private ThingDef thingToPlace;

		// Token: 0x04007BBF RID: 31679
		private Rot4 rotation;

		// Token: 0x04007BC0 RID: 31680
		private bool mapHasRoads;
	}
}
