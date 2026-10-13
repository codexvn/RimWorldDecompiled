using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E25 RID: 11813
	public class CompSpawner : ThingComp
	{
		// Token: 0x17002A9D RID: 10909
		// (get) Token: 0x06010A20 RID: 68128 RVA: 0x004CC0AE File Offset: 0x004CA2AE
		public CompProperties_Spawner PropsSpawner
		{
			get
			{
				return (CompProperties_Spawner)this.props;
			}
		}

		// Token: 0x17002A9E RID: 10910
		// (get) Token: 0x06010A21 RID: 68129 RVA: 0x004CC0BC File Offset: 0x004CA2BC
		private bool PowerOn
		{
			get
			{
				CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
				return comp != null && comp.PowerOn;
			}
		}

		// Token: 0x06010A22 RID: 68130 RVA: 0x004CC0E0 File Offset: 0x004CA2E0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.ResetCountdown();
			}
		}

		// Token: 0x06010A23 RID: 68131 RVA: 0x004CC0F8 File Offset: 0x004CA2F8
		public override void CompTick()
		{
			this.TickIntervalDelta(1);
		}

		// Token: 0x06010A24 RID: 68132 RVA: 0x004CC101 File Offset: 0x004CA301
		public override void CompTickRare()
		{
			this.TickIntervalDelta(250);
		}

		// Token: 0x06010A25 RID: 68133 RVA: 0x004CC110 File Offset: 0x004CA310
		private void TickIntervalDelta(int interval)
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
			if (comp != null)
			{
				if (!comp.Awake)
				{
					return;
				}
			}
			else if (this.parent.Position.Fogged(this.parent.Map))
			{
				return;
			}
			if (this.PropsSpawner.requiresPower && !this.PowerOn)
			{
				return;
			}
			this.ticksUntilSpawn -= interval;
			this.CheckShouldSpawn();
		}

		// Token: 0x06010A26 RID: 68134 RVA: 0x004CC18B File Offset: 0x004CA38B
		private void CheckShouldSpawn()
		{
			if (this.ticksUntilSpawn <= 0)
			{
				this.ResetCountdown();
				this.TryDoSpawn();
			}
		}

		// Token: 0x06010A27 RID: 68135 RVA: 0x004CC1A4 File Offset: 0x004CA3A4
		public bool TryDoSpawn()
		{
			if (!this.parent.Spawned)
			{
				return false;
			}
			if (this.PropsSpawner.spawnMaxAdjacent >= 0)
			{
				int num = 0;
				for (int i = 0; i < 9; i++)
				{
					IntVec3 c = this.parent.Position + GenAdj.AdjacentCellsAndInside[i];
					if (c.InBounds(this.parent.Map))
					{
						List<Thing> thingList = c.GetThingList(this.parent.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j].def == this.PropsSpawner.thingToSpawn)
							{
								num += thingList[j].stackCount;
								if (num >= this.PropsSpawner.spawnMaxAdjacent)
								{
									return false;
								}
							}
						}
					}
				}
			}
			IntVec3 center;
			if (!CompSpawner.TryFindSpawnCell(this.parent, this.PropsSpawner.thingToSpawn, this.PropsSpawner.spawnCount, out center))
			{
				return false;
			}
			Thing thing = ThingMaker.MakeThing(this.PropsSpawner.thingToSpawn, null);
			thing.stackCount = this.PropsSpawner.spawnCount;
			if (this.PropsSpawner.inheritFaction && thing.Faction != this.parent.Faction)
			{
				thing.SetFaction(this.parent.Faction, null);
			}
			Thing t;
			if (!GenPlace.TryPlaceThing(thing, center, this.parent.Map, ThingPlaceMode.Direct, out t, null, null, null, 1))
			{
				return false;
			}
			if (this.PropsSpawner.spawnForbidden)
			{
				t.SetForbidden(true, true);
			}
			if (this.PropsSpawner.showMessageIfOwned && this.parent.Faction == Faction.OfPlayer)
			{
				Messages.Message("MessageCompSpawnerSpawnedItem".Translate(this.PropsSpawner.thingToSpawn.LabelCap), thing, MessageTypeDefOf.PositiveEvent, true);
			}
			return true;
		}

		// Token: 0x06010A28 RID: 68136 RVA: 0x004CC390 File Offset: 0x004CA590
		public static bool TryFindSpawnCell(Thing parent, ThingDef thingToSpawn, int spawnCount, out IntVec3 result)
		{
			foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(parent).InRandomOrder(null))
			{
				if (intVec.Walkable(parent.Map))
				{
					Building edifice = intVec.GetEdifice(parent.Map);
					if (edifice != null)
					{
						if (thingToSpawn.IsEdifice() || edifice is IHaulDestination)
						{
							continue;
						}
						Building_Door building_Door = edifice as Building_Door;
						if (building_Door != null && !building_Door.FreePassage)
						{
							continue;
						}
					}
					if (parent.def.passability == Traversability.Impassable || GenSight.LineOfSight(parent.Position, intVec, parent.Map))
					{
						bool flag = false;
						List<Thing> thingList = intVec.GetThingList(parent.Map);
						for (int i = 0; i < thingList.Count; i++)
						{
							Thing thing = thingList[i];
							if (thing.def.category == ThingCategory.Item && (thing.def != thingToSpawn || thing.stackCount > thingToSpawn.stackLimit - spawnCount))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							result = intVec;
							return true;
						}
					}
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06010A29 RID: 68137 RVA: 0x004CC4D0 File Offset: 0x004CA6D0
		private void ResetCountdown()
		{
			this.ticksUntilSpawn = this.PropsSpawner.spawnIntervalRange.RandomInRange;
		}

		// Token: 0x06010A2A RID: 68138 RVA: 0x004CC4E8 File Offset: 0x004CA6E8
		public override void PostExposeData()
		{
			string str = this.PropsSpawner.saveKeysPrefix.NullOrEmpty() ? null : (this.PropsSpawner.saveKeysPrefix + "_");
			Scribe_Values.Look<int>(ref this.ticksUntilSpawn, str + "ticksUntilSpawn", 0, false);
		}

		// Token: 0x06010A2B RID: 68139 RVA: 0x004CC538 File Offset: 0x004CA738
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Spawn " + this.PropsSpawner.thingToSpawn.label,
					icon = TexCommand.DesirePower,
					action = delegate()
					{
						this.ResetCountdown();
						this.TryDoSpawn();
					}
				};
			}
			yield break;
		}

		// Token: 0x06010A2C RID: 68140 RVA: 0x004CC548 File Offset: 0x004CA748
		public override string CompInspectStringExtra()
		{
			if (this.PropsSpawner.writeTimeLeftToSpawn && (!this.PropsSpawner.requiresPower || this.PowerOn))
			{
				return "NextSpawnedItemIn".Translate(GenLabel.ThingLabel(this.PropsSpawner.thingToSpawn, null, this.PropsSpawner.spawnCount)).Resolve() + ": " + this.ticksUntilSpawn.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor);
			}
			return null;
		}

		// Token: 0x0400AD61 RID: 44385
		private int ticksUntilSpawn;
	}
}
