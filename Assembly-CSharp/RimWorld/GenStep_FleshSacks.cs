using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FB1 RID: 8113
	public class GenStep_FleshSacks : GenStep
	{
		// Token: 0x17001B41 RID: 6977
		// (get) Token: 0x0600B306 RID: 45830 RVA: 0x00334BD0 File Offset: 0x00332DD0
		public override int SeedPart
		{
			get
			{
				return 1234731256;
			}
		}

		// Token: 0x0600B307 RID: 45831 RVA: 0x00334BD8 File Offset: 0x00332DD8
		public override void Generate(Map map, GenStepParams parms)
		{
			SitePart sitePart = parms.sitePart;
			float? num;
			if (sitePart == null)
			{
				num = null;
			}
			else
			{
				Site site = sitePart.site;
				num = ((site != null) ? new float?(site.desiredThreatPoints) : null);
			}
			float x = num ?? StorytellerUtility.DefaultThreatPointsNow(map);
			SimpleCurve simpleCurve = this.numFleshSacksFromPoints;
			int num2 = Mathf.RoundToInt((simpleCurve != null) ? simpleCurve.Evaluate(x) : 0f);
			int i = 0;
			Predicate<IntVec3> <>9__0;
			Predicate<IntVec3> <>9__1;
			while (i < num2)
			{
				if (!this.trySpawnInRoom)
				{
					goto IL_C5;
				}
				Map map2 = map;
				Predicate<IntVec3> validator;
				if ((validator = <>9__0) == null)
				{
					validator = (<>9__0 = ((IntVec3 c) => this.Validator(c, map, true)));
				}
				IntVec3 intVec;
				if (!CellFinder.TryFindRandomCell(map2, validator, out intVec))
				{
					goto IL_C5;
				}
				goto IL_F8;
				IL_241:
				i++;
				continue;
				IL_C5:
				Map map3 = map;
				Predicate<IntVec3> validator2;
				if ((validator2 = <>9__1) == null)
				{
					validator2 = (<>9__1 = ((IntVec3 c) => this.Validator(c, map, false)));
				}
				if (!CellFinder.TryFindRandomCell(map3, validator2, out intVec))
				{
					goto IL_241;
				}
				IL_F8:
				foreach (IntVec3 intVec2 in GridShapeMaker.IrregularLump(intVec, map, this.sackClumpSize, null))
				{
					if (this.spawnSurroundingFleshmass)
					{
						GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Fleshmass, null), intVec2, map, Rot4.Random, WipeMode.Vanish, false, false).SetFaction(Faction.OfEntities, null);
					}
					map.terrainGrid.SetTerrain(intVec2, TerrainDefOf.Flesh);
				}
				List<Thing> list;
				if (this.forceAtLeastOneShard && i == 0)
				{
					list = Gen.YieldSingle<Thing>(ThingMaker.MakeThing(ThingDefOf.Shard, null)).ToList<Thing>();
				}
				else
				{
					ThingSetMakerParams parms2 = default(ThingSetMakerParams);
					parms2.qualityGenerator = new QualityGenerator?(QualityGenerator.Reward);
					parms2.makingFaction = Faction.OfEntities;
					list = ThingSetMakerDefOf.MapGen_FleshSackLoot.root.Generate(parms2);
				}
				Building_Casket building_Casket = ThingMaker.MakeThing(ThingDefOf.FleshSack, null) as Building_Casket;
				GenSpawn.Spawn(building_Casket, intVec, map, WipeMode.Vanish);
				building_Casket.SetFaction(Faction.OfEntities, null);
				for (int j = list.Count - 1; j >= 0; j--)
				{
					Thing thing = list[j];
					if (!building_Casket.TryAcceptThing(thing, false))
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
				goto IL_241;
			}
		}

		// Token: 0x0600B308 RID: 45832 RVA: 0x00334E44 File Offset: 0x00333044
		private bool Validator(IntVec3 c, Map map, bool mustBeInRoom)
		{
			return GenSpawn.CanSpawnAt(ThingDefOf.FleshSack, c, map, null, true) && c.DistanceToEdge(map) > 2 && (!mustBeInRoom || c.GetRoom(map) != null) && c.GetRoom(map).ProperRoom && (map.generatorDef.isUnderground || map.reachability.CanReachMapEdge(c, TraverseMode.PassDoors));
		}

		// Token: 0x04007AAA RID: 31402
		private int sackClumpSize = 50;

		// Token: 0x04007AAB RID: 31403
		private SimpleCurve numFleshSacksFromPoints;

		// Token: 0x04007AAC RID: 31404
		private bool forceAtLeastOneShard;

		// Token: 0x04007AAD RID: 31405
		private bool spawnSurroundingFleshmass = true;

		// Token: 0x04007AAE RID: 31406
		private bool trySpawnInRoom;
	}
}
