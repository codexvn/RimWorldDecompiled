using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200396C RID: 14700
	public class SymbolResolver_OutdoorLighting : SymbolResolver
	{
		// Token: 0x0601435D RID: 82781 RVA: 0x00607484 File Offset: 0x00605684
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef thingDef;
			if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
			{
				thingDef = ThingDefOf.StandingLamp;
			}
			else
			{
				thingDef = ThingDefOf.TorchLamp;
			}
			this.FindNearbyGlowers(rp.rect);
			for (int i = 0; i < rp.rect.Area / 4; i++)
			{
				IntVec3 randomCell = rp.rect.RandomCell;
				if (randomCell.Standable(map) && randomCell.GetFirstItem(map) == null && randomCell.GetFirstPawn(map) == null && randomCell.GetFirstBuilding(map) == null)
				{
					Region region = randomCell.GetRegion(map, RegionType.Set_Passable);
					if (region != null && region.Room.PsychologicallyOutdoors && region.Room.UsesOutdoorTemperature && !this.AnyGlowerNearby(randomCell) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(randomCell, map))
					{
						if (rp.spawnBridgeIfTerrainCantSupportThing == null || rp.spawnBridgeIfTerrainCantSupportThing.Value)
						{
							BaseGenUtility.CheckSpawnBridgeUnder(thingDef, randomCell, Rot4.North);
						}
						Thing thing = GenSpawn.Spawn(thingDef, randomCell, map, WipeMode.Vanish);
						if (thing.def.CanHaveFaction && thing.Faction != rp.faction)
						{
							thing.SetFaction(rp.faction, null);
							Thing thing2 = thing;
							FactionIdeosTracker ideos = rp.faction.ideos;
							ThingStyleDef styleDef;
							if (ideos == null)
							{
								styleDef = null;
							}
							else
							{
								Ideo primaryIdeo = ideos.PrimaryIdeo;
								styleDef = ((primaryIdeo != null) ? primaryIdeo.GetStyleFor(thing.def) : null);
							}
							thing2.SetStyleDef(styleDef);
						}
						SymbolResolver_OutdoorLighting.nearbyGlowers.Add(thing.TryGetComp<CompGlower>());
					}
				}
			}
			SymbolResolver_OutdoorLighting.nearbyGlowers.Clear();
		}

		// Token: 0x0601435E RID: 82782 RVA: 0x00607624 File Offset: 0x00605824
		private void FindNearbyGlowers(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_OutdoorLighting.nearbyGlowers.Clear();
			rect = rect.ExpandedBy(4);
			rect = rect.ClipInsideMap(map);
			foreach (IntVec3 intVec in rect)
			{
				Region region = intVec.GetRegion(map, RegionType.Set_Passable);
				if (region != null && region.Room.PsychologicallyOutdoors)
				{
					List<Thing> thingList = intVec.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						CompGlower compGlower = thingList[i].TryGetComp<CompGlower>();
						if (compGlower != null)
						{
							SymbolResolver_OutdoorLighting.nearbyGlowers.Add(compGlower);
						}
					}
				}
			}
		}

		// Token: 0x0601435F RID: 82783 RVA: 0x006076F0 File Offset: 0x006058F0
		private bool AnyGlowerNearby(IntVec3 c)
		{
			for (int i = 0; i < SymbolResolver_OutdoorLighting.nearbyGlowers.Count; i++)
			{
				if (c.InHorDistOf(SymbolResolver_OutdoorLighting.nearbyGlowers[i].parent.Position, SymbolResolver_OutdoorLighting.nearbyGlowers[i].GlowRadius + 2f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400DF5B RID: 57179
		private static List<CompGlower> nearbyGlowers = new List<CompGlower>();

		// Token: 0x0400DF5C RID: 57180
		private const float Margin = 2f;
	}
}
