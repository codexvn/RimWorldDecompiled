using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A8F RID: 10895
	public class BuildingGroundSpawner : GroundSpawner
	{
		// Token: 0x170025E4 RID: 9700
		// (get) Token: 0x0600F294 RID: 62100 RVA: 0x00467180 File Offset: 0x00465380
		protected override IntRange ResultSpawnDelay
		{
			get
			{
				IntRange? intRange = this.emergeDelay;
				if (intRange == null)
				{
					return this.def.building.groundSpawnerSpawnDelay;
				}
				return intRange.GetValueOrDefault();
			}
		}

		// Token: 0x170025E5 RID: 9701
		// (get) Token: 0x0600F295 RID: 62101 RVA: 0x004671B5 File Offset: 0x004653B5
		protected override SoundDef SustainerSound
		{
			get
			{
				return this.def.building.groundSpawnerSustainerSound ?? SoundDefOf.Tunnel;
			}
		}

		// Token: 0x170025E6 RID: 9702
		// (get) Token: 0x0600F296 RID: 62102 RVA: 0x004671D0 File Offset: 0x004653D0
		protected virtual ThingDef ThingDefToSpawn
		{
			get
			{
				return this.def.building.groundSpawnerThingToSpawn;
			}
		}

		// Token: 0x170025E7 RID: 9703
		// (get) Token: 0x0600F297 RID: 62103 RVA: 0x004671E2 File Offset: 0x004653E2
		public Thing ThingToSpawn
		{
			get
			{
				return this.thingToSpawn;
			}
		}

		// Token: 0x0600F298 RID: 62104 RVA: 0x004671EA File Offset: 0x004653EA
		public override void PostMake()
		{
			base.PostMake();
			this.PostMakeInt();
		}

		// Token: 0x0600F299 RID: 62105 RVA: 0x004671F8 File Offset: 0x004653F8
		protected virtual void PostMakeInt()
		{
			this.thingToSpawn = ThingMaker.MakeThing(this.ThingDefToSpawn, null);
		}

		// Token: 0x0600F29A RID: 62106 RVA: 0x0046720C File Offset: 0x0046540C
		protected override void Spawn(Map map, IntVec3 pos)
		{
			TerrainDef newTerr = map.Biome.TerrainForAffordance(this.ThingDefToSpawn.terrainAffordanceNeeded);
			foreach (IntVec3 c in GenAdj.OccupiedRect(pos, Rot4.North, this.ThingDefToSpawn.Size))
			{
				map.terrainGrid.RemoveTopLayer(c, false);
				if (!c.GetAffordances(map).Contains(this.ThingDefToSpawn.terrainAffordanceNeeded))
				{
					map.terrainGrid.SetTerrain(c, newTerr);
				}
			}
			GenSpawn.Spawn(this.thingToSpawn, pos, map, Rot4.North, WipeMode.FullRefund, false, true);
			this.thingToSpawn.questTags = this.questTagsToForward;
			BuildingProperties building = this.def.building;
			if (building != null && building.groundSpawnerDestroyAdjacent)
			{
				foreach (IntVec3 c2 in GenAdj.CellsAdjacentCardinal(this.thingToSpawn))
				{
					Building edifice = c2.GetEdifice(map);
					if (edifice != null && edifice.def.destroyable)
					{
						edifice.Destroy(DestroyMode.Refund);
					}
				}
			}
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			BuildingProperties building2 = this.def.building;
			if (((building2 != null) ? building2.groundSpawnerLetterLabel : null) != null)
			{
				BuildingProperties building3 = this.def.building;
				if (((building3 != null) ? building3.groundSpawnerLetterText : null) != null)
				{
					Find.LetterStack.ReceiveLetter(this.def.building.groundSpawnerLetterLabel, this.def.building.groundSpawnerLetterText, LetterDefOf.NegativeEvent, new TargetInfo(this.thingToSpawn), null, null, null, null, 0, true);
				}
			}
		}

		// Token: 0x0600F29B RID: 62107 RVA: 0x004673E8 File Offset: 0x004655E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange?>(ref this.emergeDelay, "emergeDelay", null, false);
			Scribe_Deep.Look<Thing>(ref this.thingToSpawn, "thingToSpawn", Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.questTagsToForward, "questTagsToForward", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x04009FD6 RID: 40918
		protected Thing thingToSpawn;

		// Token: 0x04009FD7 RID: 40919
		public IntRange? emergeDelay;

		// Token: 0x04009FD8 RID: 40920
		public List<string> questTagsToForward;
	}
}
