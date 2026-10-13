using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E2C RID: 11820
	public class CompSpawnerHives : ThingComp
	{
		// Token: 0x17002AA4 RID: 10916
		// (get) Token: 0x06010A49 RID: 68169 RVA: 0x004CCA41 File Offset: 0x004CAC41
		private CompProperties_SpawnerHives Props
		{
			get
			{
				return (CompProperties_SpawnerHives)this.props;
			}
		}

		// Token: 0x17002AA5 RID: 10917
		// (get) Token: 0x06010A4A RID: 68170 RVA: 0x004CCA4E File Offset: 0x004CAC4E
		private bool CanSpawnChildHive
		{
			get
			{
				return this.canSpawnHives && HiveUtility.TotalSpawnedHivesCount(this.parent.Map, false) < 30 && Find.Storyteller.difficulty.enemyReproductionRateFactor > 0f;
			}
		}

		// Token: 0x06010A4B RID: 68171 RVA: 0x004CCA85 File Offset: 0x004CAC85
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.CalculateNextHiveSpawnTick();
			}
		}

		// Token: 0x06010A4C RID: 68172 RVA: 0x004CCA90 File Offset: 0x004CAC90
		public override void CompTick()
		{
			base.CompTick();
			CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
			if ((comp == null || comp.Awake) && !this.wasActivated)
			{
				this.CalculateNextHiveSpawnTick();
				this.wasActivated = true;
			}
			if ((comp == null || comp.Awake) && Find.TickManager.TicksGame >= this.nextHiveSpawnTick)
			{
				Hive t;
				if (this.TrySpawnChildHive(false, out t))
				{
					Messages.Message("MessageHiveReproduced".Translate(), t, MessageTypeDefOf.NegativeEvent, true);
					return;
				}
				this.CalculateNextHiveSpawnTick();
			}
		}

		// Token: 0x06010A4D RID: 68173 RVA: 0x004CCB24 File Offset: 0x004CAD24
		public override string CompInspectStringExtra()
		{
			if (!this.canSpawnHives || Find.Storyteller.difficulty.enemyReproductionRateFactor <= 0f)
			{
				return "DormantHiveNotReproducing".Translate();
			}
			if (this.CanSpawnChildHive)
			{
				return "HiveReproducesIn".Translate() + ": " + (this.nextHiveSpawnTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false);
			}
			return null;
		}

		// Token: 0x06010A4E RID: 68174 RVA: 0x004CCBA4 File Offset: 0x004CADA4
		public void CalculateNextHiveSpawnTick()
		{
			Room room = this.parent.GetRoom(RegionType.Set_All);
			int num = 0;
			int num2 = GenRadial.NumCellsInRadius(9f);
			for (int i = 0; i < num2; i++)
			{
				IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.parent.Map) && intVec.GetRoom(this.parent.Map) == room)
				{
					if (intVec.GetThingList(this.parent.Map).Any((Thing t) => t is Hive))
					{
						num++;
					}
				}
			}
			float num3 = this.Props.ReproduceRateFactorFromNearbyHiveCountCurve.Evaluate((float)num);
			if (Find.Storyteller.difficulty.enemyReproductionRateFactor > 0f)
			{
				this.nextHiveSpawnTick = Find.TickManager.TicksGame + (int)(this.Props.HiveSpawnIntervalDays.RandomInRange * 60000f / (num3 * Find.Storyteller.difficulty.enemyReproductionRateFactor));
				return;
			}
			this.nextHiveSpawnTick = Find.TickManager.TicksGame + (int)this.Props.HiveSpawnIntervalDays.RandomInRange * 60000;
		}

		// Token: 0x06010A4F RID: 68175 RVA: 0x004CCCF4 File Offset: 0x004CAEF4
		public bool TrySpawnChildHive(bool ignoreRoofedRequirement, out Hive newHive)
		{
			IntVec3 loc = CompSpawnerHives.FindChildHiveLocation(this.parent.Position, this.parent.Map, this.parent.def, this.Props, ignoreRoofedRequirement, false);
			return this.TrySpawnChildHive(loc, out newHive);
		}

		// Token: 0x06010A50 RID: 68176 RVA: 0x004CCD38 File Offset: 0x004CAF38
		public bool TrySpawnChildHive(IntVec3 loc, out Hive newHive)
		{
			if (!this.CanSpawnChildHive)
			{
				newHive = null;
				return false;
			}
			if (!loc.IsValid)
			{
				newHive = null;
				return false;
			}
			newHive = (Hive)ThingMaker.MakeThing(this.parent.def, null);
			if (newHive.Faction != this.parent.Faction)
			{
				newHive.SetFaction(this.parent.Faction, null);
			}
			Hive hive = this.parent as Hive;
			if (hive != null)
			{
				if (hive.CompDormant.Awake)
				{
					newHive.CompDormant.WakeUp();
				}
				newHive.questTags = hive.questTags;
			}
			GenSpawn.Spawn(newHive, loc, this.parent.Map, WipeMode.FullRefund);
			this.CalculateNextHiveSpawnTick();
			return true;
		}

		// Token: 0x06010A51 RID: 68177 RVA: 0x004CCDF0 File Offset: 0x004CAFF0
		public static IntVec3 FindChildHiveLocation(IntVec3 pos, Map map, ThingDef parentDef, CompProperties_SpawnerHives props, bool ignoreRoofedRequirement, bool allowUnreachable)
		{
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = 0; i < 3; i++)
			{
				float minDist = props.HiveSpawnPreferredMinDist;
				bool flag;
				if (i < 2)
				{
					if (i == 1)
					{
						minDist = 0f;
					}
					flag = CellFinder.TryFindRandomReachableNearbyCell(pos, map, props.HiveSpawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), (IntVec3 c) => CompSpawnerHives.CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement), null, out intVec, 999999);
				}
				else
				{
					flag = (allowUnreachable && CellFinder.TryFindRandomCellNear(pos, map, (int)props.HiveSpawnRadius, (IntVec3 c) => CompSpawnerHives.CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement), out intVec, -1));
				}
				if (flag)
				{
					intVec = CellFinder.FindNoWipeSpawnLocNear(intVec, map, parentDef, Rot4.North, 2, (IntVec3 c) => CompSpawnerHives.CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement));
					break;
				}
			}
			return intVec;
		}

		// Token: 0x06010A52 RID: 68178 RVA: 0x004CCF1C File Offset: 0x004CB11C
		private static bool CanSpawnHiveAt(IntVec3 c, Map map, IntVec3 parentPos, ThingDef parentDef, float minDist, bool ignoreRoofedRequirement)
		{
			if ((!ignoreRoofedRequirement && !c.Roofed(map)) || (!c.Walkable(map) || (minDist != 0f && (float)c.DistanceToSquared(parentPos) < minDist * minDist)) || c.GetFirstThing(map, ThingDefOf.InsectJelly) != null || c.GetFirstThing(map, ThingDefOf.GlowPod) != null)
			{
				return false;
			}
			for (int i = 0; i < 9; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
				if (c2.InBounds(map))
				{
					List<Thing> thingList = c2.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j] is Hive || thingList[j] is TunnelHiveSpawner)
						{
							return false;
						}
					}
				}
			}
			List<Thing> thingList2 = c.GetThingList(map);
			for (int k = 0; k < thingList2.Count; k++)
			{
				Thing thing = thingList2[k];
				if (thing.def.category == ThingCategory.Building && thing.def.passability == Traversability.Impassable && GenSpawn.SpawningWipes(parentDef, thing.def, false))
				{
					return true;
				}
			}
			return true;
		}

		// Token: 0x06010A53 RID: 68179 RVA: 0x004CD037 File Offset: 0x004CB237
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Reproduce",
					icon = TexCommand.GatherSpotActive,
					action = delegate()
					{
						Hive hive;
						this.TrySpawnChildHive(false, out hive);
					}
				};
			}
			yield break;
		}

		// Token: 0x06010A54 RID: 68180 RVA: 0x004CD047 File Offset: 0x004CB247
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextHiveSpawnTick, "nextHiveSpawnTick", 0, false);
			Scribe_Values.Look<bool>(ref this.canSpawnHives, "canSpawnHives", true, false);
			Scribe_Values.Look<bool>(ref this.wasActivated, "wasActivated", true, false);
		}

		// Token: 0x0400AD72 RID: 44402
		private int nextHiveSpawnTick = -1;

		// Token: 0x0400AD73 RID: 44403
		public bool canSpawnHives = true;

		// Token: 0x0400AD74 RID: 44404
		private bool wasActivated;

		// Token: 0x0400AD75 RID: 44405
		public const int MaxHivesPerMap = 30;
	}
}
