using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020024E5 RID: 9445
	public class Pawn_TraderTracker : IExposable
	{
		// Token: 0x17001FA4 RID: 8100
		// (get) Token: 0x0600CE45 RID: 52805 RVA: 0x003C8F99 File Offset: 0x003C7199
		public IEnumerable<Thing> Goods
		{
			get
			{
				Lord lord = this.pawn.GetLord();
				if (lord == null || !(lord.LordJob is LordJob_TradeWithColony))
				{
					int num;
					for (int i = 0; i < this.pawn.inventory.innerContainer.Count; i = num + 1)
					{
						Thing thing = this.pawn.inventory.innerContainer[i];
						if (!this.pawn.inventory.NotForSale(thing))
						{
							yield return thing;
						}
						num = i;
					}
				}
				if (lord != null)
				{
					int num;
					for (int i = 0; i < lord.ownedPawns.Count; i = num + 1)
					{
						Pawn p = lord.ownedPawns[i];
						TraderCaravanRole traderCaravanRole = p.GetTraderCaravanRole();
						if (traderCaravanRole == TraderCaravanRole.Carrier)
						{
							for (int j = 0; j < p.inventory.innerContainer.Count; j = num + 1)
							{
								yield return p.inventory.innerContainer[j];
								num = j;
							}
						}
						else if (traderCaravanRole == TraderCaravanRole.Chattel && !this.soldPrisoners.Contains(p))
						{
							yield return p;
						}
						p = null;
						num = i;
					}
				}
				yield break;
			}
		}

		// Token: 0x17001FA5 RID: 8101
		// (get) Token: 0x0600CE46 RID: 52806 RVA: 0x003C8FA9 File Offset: 0x003C71A9
		public int RandomPriceFactorSeed
		{
			get
			{
				return Gen.HashCombineInt(this.pawn.thingIDNumber, 1149275593);
			}
		}

		// Token: 0x17001FA6 RID: 8102
		// (get) Token: 0x0600CE47 RID: 52807 RVA: 0x003C8FC0 File Offset: 0x003C71C0
		public string TraderName
		{
			get
			{
				return this.pawn.LabelShort;
			}
		}

		// Token: 0x17001FA7 RID: 8103
		// (get) Token: 0x0600CE48 RID: 52808 RVA: 0x003C8FD0 File Offset: 0x003C71D0
		public bool CanTradeNow
		{
			get
			{
				return !this.pawn.Dead && this.pawn.Spawned && this.pawn.mindState.wantsToTradeWithColony && this.pawn.CanCasuallyInteractNow(false, false, false, false) && !this.pawn.Downed && !this.pawn.IsPrisoner && this.pawn.Faction != Faction.OfPlayer && (this.pawn.Faction == null || !this.pawn.Faction.HostileTo(Faction.OfPlayer)) && (this.Goods.Any((Thing x) => this.traderKind.WillTrade(x.def)) || this.traderKind.tradeCurrency == TradeCurrency.Favor);
			}
		}

		// Token: 0x0600CE49 RID: 52809 RVA: 0x003C909E File Offset: 0x003C729E
		public Pawn_TraderTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CE4A RID: 52810 RVA: 0x003C90B8 File Offset: 0x003C72B8
		public void ExposeData()
		{
			Scribe_Defs.Look<TraderKindDef>(ref this.traderKind, "traderKind");
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600CE4B RID: 52811 RVA: 0x003C911E File Offset: 0x003C731E
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			IEnumerable<Thing> enumerable = from x in this.pawn.Map.listerThings.AllThings
			where x.def.category == ThingCategory.Item && TradeUtility.PlayerSellableNow(x, this.pawn) && !x.Position.Fogged(x.Map) && (this.pawn.Map.areaManager.Home[x.Position] || x.IsInAnyStorage()) && this.ReachableForTrade(x)
			select x;
			foreach (Thing thing in enumerable)
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			if (ModsConfig.BiotechActive)
			{
				List<Building> list = this.pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.GeneBank);
				foreach (Building thing2 in list)
				{
					if (this.ReachableForTrade(thing2))
					{
						CompGenepackContainer compGenepackContainer = thing2.TryGetComp<CompGenepackContainer>();
						if (compGenepackContainer != null)
						{
							List<Genepack> containedGenepacks = compGenepackContainer.ContainedGenepacks;
							foreach (Genepack genepack in containedGenepacks)
							{
								yield return genepack;
							}
							List<Genepack>.Enumerator enumerator3 = default(List<Genepack>.Enumerator);
						}
					}
				}
				List<Building>.Enumerator enumerator2 = default(List<Building>.Enumerator);
			}
			IEnumerable<IHaulSource> enumerable2 = this.pawn.Map.listerBuildings.AllColonistBuildingsOfType<IHaulSource>();
			foreach (IHaulSource haulSource in enumerable2)
			{
				Building thing3 = (Building)haulSource;
				if (this.ReachableForTrade(thing3))
				{
					foreach (Thing thing4 in ((IEnumerable<Thing>)haulSource.GetDirectlyHeldThings()))
					{
						yield return thing4;
					}
					enumerator = null;
				}
			}
			IEnumerator<IHaulSource> enumerator4 = null;
			if (this.pawn.GetLord() != null)
			{
				foreach (Pawn pawn in from x in TradeUtility.AllSellableColonyPawns(this.pawn.Map, true)
				where !x.Downed && this.ReachableForTrade(x)
				select x)
				{
					yield return pawn;
				}
				IEnumerator<Pawn> enumerator5 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600CE4C RID: 52812 RVA: 0x003C9130 File Offset: 0x003C7330
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			if (this.Goods.Contains(toGive))
			{
				Log.Error("Tried to add " + ((toGive != null) ? toGive.ToString() : null) + " to stock (pawn's trader tracker), but it's already here.");
				return;
			}
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				pawn.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.pawn);
				this.AddPawnToStock(pawn);
				return;
			}
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this.pawn);
			Thing thing2 = TradeUtility.ThingFromStockToMergeWith(this.pawn, thing);
			if (thing2 != null)
			{
				if (!thing2.TryAbsorbStack(thing, false))
				{
					thing.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			else
			{
				this.AddThingToRandomInventory(thing);
			}
		}

		// Token: 0x0600CE4D RID: 52813 RVA: 0x003C91CC File Offset: 0x003C73CC
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Pawn pawn = toGive as Pawn;
			if (pawn != null)
			{
				pawn.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.pawn);
				Lord lord = pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.Undefined, null);
				}
				if (this.soldPrisoners.Contains(pawn))
				{
					this.soldPrisoners.Remove(pawn);
					return;
				}
			}
			else
			{
				IntVec3 positionHeld = toGive.PositionHeld;
				Map mapHeld = toGive.MapHeld;
				Thing thing = toGive.SplitOff(countToGive);
				thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this.pawn);
				if (GenPlace.TryPlaceThing(thing, positionHeld, mapHeld, ThingPlaceMode.Near, null, null, null, 1))
				{
					Lord lord2 = this.pawn.GetLord();
					if (lord2 != null)
					{
						lord2.extraForbiddenThings.Add(thing);
						return;
					}
				}
				else
				{
					string str = "Could not place bought thing ";
					Thing thing2 = thing;
					string str2 = (thing2 != null) ? thing2.ToString() : null;
					string str3 = " at ";
					IntVec3 intVec = positionHeld;
					Log.Error(str + str2 + str3 + intVec.ToString());
					thing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600CE4E RID: 52814 RVA: 0x003C92C8 File Offset: 0x003C74C8
		private void AddPawnToStock(Pawn newPawn)
		{
			if (!newPawn.Spawned)
			{
				GenSpawn.Spawn(newPawn, this.pawn.Position, this.pawn.Map, WipeMode.Vanish);
			}
			if (newPawn.Faction != this.pawn.Faction)
			{
				newPawn.SetFaction(this.pawn.Faction, null);
			}
			if (newPawn.RaceProps.Humanlike)
			{
				newPawn.kindDef = PawnKindDefOf.Slave;
			}
			Lord lord = this.pawn.GetLord();
			if (lord == null)
			{
				newPawn.Destroy(DestroyMode.Vanish);
				string[] array = new string[7];
				array[0] = "Tried to sell pawn ";
				array[1] = ((newPawn != null) ? newPawn.ToString() : null);
				array[2] = " to ";
				int num = 3;
				Pawn pawn = this.pawn;
				array[num] = ((pawn != null) ? pawn.ToString() : null);
				array[4] = ", but ";
				int num2 = 5;
				Pawn pawn2 = this.pawn;
				array[num2] = ((pawn2 != null) ? pawn2.ToString() : null);
				array[6] = " has no lord. Traders without lord can't buy pawns.";
				Log.Error(string.Concat(array));
				return;
			}
			if (newPawn.RaceProps.Humanlike)
			{
				this.soldPrisoners.Add(newPawn);
			}
			lord.AddPawn(newPawn);
		}

		// Token: 0x0600CE4F RID: 52815 RVA: 0x003C93DC File Offset: 0x003C75DC
		private void AddThingToRandomInventory(Thing thing)
		{
			Lord lord = this.pawn.GetLord();
			IEnumerable<Pawn> source = Enumerable.Empty<Pawn>();
			if (lord != null)
			{
				source = from x in lord.ownedPawns
				where x.GetTraderCaravanRole() == TraderCaravanRole.Carrier
				select x;
			}
			if (source.Any<Pawn>())
			{
				if (!source.RandomElement<Pawn>().inventory.innerContainer.TryAdd(thing, true))
				{
					thing.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			else if (!this.pawn.inventory.innerContainer.TryAdd(thing, true))
			{
				thing.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600CE50 RID: 52816 RVA: 0x003C9474 File Offset: 0x003C7674
		private bool ReachableForTrade(Thing thing)
		{
			Thing thing2 = thing;
			if (HaulAIUtility.IsInHaulableInventory(thing))
			{
				thing2 = thing.SpawnedParentOrMe;
			}
			return this.pawn.Map == thing2.MapHeld && this.pawn.Map.reachability.CanReach(this.pawn.Position, thing2, PathEndMode.Touch, TraverseMode.PassDoors, Danger.Some);
		}

		// Token: 0x04008C5D RID: 35933
		private Pawn pawn;

		// Token: 0x04008C5E RID: 35934
		public TraderKindDef traderKind;

		// Token: 0x04008C5F RID: 35935
		private List<Pawn> soldPrisoners = new List<Pawn>();
	}
}
