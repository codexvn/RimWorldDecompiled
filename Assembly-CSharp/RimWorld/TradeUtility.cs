using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002FBA RID: 12218
	public static class TradeUtility
	{
		// Token: 0x06011368 RID: 70504 RVA: 0x004EE8D4 File Offset: 0x004ECAD4
		public static bool EverPlayerSellable(ThingDef def)
		{
			return def.tradeability.PlayerCanSell() && def.GetStatValueAbstract(StatDefOf.MarketValue, null) > 0f && (def.category == ThingCategory.Item || def.category == ThingCategory.Pawn || def.category == ThingCategory.Building) && (def.category != ThingCategory.Building || def.Minifiable);
		}

		// Token: 0x06011369 RID: 70505 RVA: 0x004EE938 File Offset: 0x004ECB38
		public static bool PlayerSellableNow(Thing t, ITrader trader)
		{
			t = t.GetInnerIfMinified();
			if (!TradeUtility.EverPlayerSellable(t.def))
			{
				return false;
			}
			if (t.IsNotFresh())
			{
				return false;
			}
			Apparel apparel = t as Apparel;
			if (apparel != null && apparel.WornByCorpse)
			{
				return false;
			}
			if (CompBiocodable.IsBiocoded(t))
			{
				return false;
			}
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if ((pawn.GetExtraHostFaction(null) != null && pawn.GetExtraHostFaction(null) == trader.Faction) || (pawn.IsQuestLodger() && pawn.GetExtraHomeFaction(null) == trader.Faction) || (pawn.HomeFaction != null && pawn.HomeFaction == trader.Faction && !pawn.IsSlave && !pawn.IsPrisoner))
				{
					return false;
				}
				if (pawn.RaceProps.Animal && pawn.IsFormingCaravan())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0601136A RID: 70506 RVA: 0x004EEA00 File Offset: 0x004ECC00
		public static void SpawnDropPod(IntVec3 dropSpot, Map map, Thing t)
		{
			DropPodUtility.MakeDropPodAt(dropSpot, map, new ActiveTransporterInfo
			{
				SingleContainedThing = t,
				leaveSlag = false
			}, null);
		}

		// Token: 0x0601136B RID: 70507 RVA: 0x004EEA2A File Offset: 0x004ECC2A
		public static IEnumerable<Thing> AllLaunchableThingsForTrade(Map map, ITrader trader = null)
		{
			HashSet<Thing> yieldedThings = new HashSet<Thing>();
			foreach (Building_OrbitalTradeBeacon building_OrbitalTradeBeacon in Building_OrbitalTradeBeacon.AllPowered(map))
			{
				foreach (IntVec3 c in building_OrbitalTradeBeacon.TradeableCells)
				{
					List<Thing> thingList = c.GetThingList(map);
					int num;
					for (int i = 0; i < thingList.Count; i = num + 1)
					{
						Thing t = thingList[i];
						if (ModsConfig.BiotechActive && t.def == ThingDefOf.GeneBank)
						{
							CompGenepackContainer compGenepackContainer = t.TryGetComp<CompGenepackContainer>();
							if (compGenepackContainer != null)
							{
								List<Genepack> containedGenepacks = compGenepackContainer.ContainedGenepacks;
								foreach (Genepack genepack in containedGenepacks)
								{
									if (TradeUtility.PlayerSellableNow(t, trader) && !yieldedThings.Contains(genepack))
									{
										yieldedThings.Add(genepack);
										yield return genepack;
									}
								}
								List<Genepack>.Enumerator enumerator3 = default(List<Genepack>.Enumerator);
							}
						}
						else
						{
							Building_Bookcase building_Bookcase = t as Building_Bookcase;
							if (building_Bookcase != null)
							{
								foreach (Book book in building_Bookcase.HeldBooks)
								{
									if (TradeUtility.PlayerSellableNow(t, trader) && !yieldedThings.Contains(book))
									{
										yieldedThings.Add(book);
										yield return book;
									}
								}
								IEnumerator<Book> enumerator4 = null;
							}
							else
							{
								Building_OutfitStand building_OutfitStand = t as Building_OutfitStand;
								if (building_OutfitStand != null)
								{
									foreach (Thing thing in building_OutfitStand.HeldItems)
									{
										if (TradeUtility.PlayerSellableNow(t, trader) && !yieldedThings.Contains(thing))
										{
											yieldedThings.Add(thing);
											yield return thing;
										}
									}
									IEnumerator<Thing> enumerator5 = null;
								}
								else if (t.def.category == ThingCategory.Item && TradeUtility.PlayerSellableNow(t, trader) && !yieldedThings.Contains(t))
								{
									yieldedThings.Add(t);
									yield return t;
								}
							}
						}
						t = null;
						num = i;
					}
					thingList = null;
				}
				IEnumerator<IntVec3> enumerator2 = null;
			}
			IEnumerator<Building_OrbitalTradeBeacon> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0601136C RID: 70508 RVA: 0x004EEA41 File Offset: 0x004ECC41
		public static IEnumerable<Pawn> AllSellableColonyPawns(Map map, bool checkAcceptableTemperatureOfAnimals = true)
		{
			foreach (Pawn pawn in map.mapPawns.PrisonersOfColonySpawned)
			{
				if (pawn.guest.PrisonerIsSecure)
				{
					yield return pawn;
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			foreach (Pawn pawn2 in map.mapPawns.SlavesOfColonySpawned)
			{
				if (pawn2.guest.SlaveIsSecure)
				{
					yield return pawn2;
				}
			}
			enumerator = default(List<Pawn>.Enumerator);
			foreach (Pawn pawn3 in map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (pawn3.IsAnimal && pawn3.HostFaction == null && !pawn3.InMentalState && !pawn3.Downed && (!checkAcceptableTemperatureOfAnimals || map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(pawn3.def, 0f)))
				{
					yield return pawn3;
				}
			}
			enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0601136D RID: 70509 RVA: 0x004EEA58 File Offset: 0x004ECC58
		public static Thing ThingFromStockToMergeWith(ITrader trader, Thing thing)
		{
			if (thing is Pawn)
			{
				return null;
			}
			foreach (Thing thing2 in trader.Goods)
			{
				if (TransferableUtility.TransferAsOne(thing2, thing, TransferAsOneMode.Normal) && thing2.CanStackWith(thing) && thing2.def.stackLimit != 1)
				{
					return thing2;
				}
			}
			return null;
		}

		// Token: 0x0601136E RID: 70510 RVA: 0x004EEAD0 File Offset: 0x004ECCD0
		public static void LaunchThingsOfType(ThingDef resDef, int debt, Map map, TradeShip trader)
		{
			while (debt > 0)
			{
				Thing thing = null;
				foreach (Building_OrbitalTradeBeacon building_OrbitalTradeBeacon in Building_OrbitalTradeBeacon.AllPowered(map))
				{
					foreach (IntVec3 c in building_OrbitalTradeBeacon.TradeableCells)
					{
						foreach (Thing thing2 in map.thingGrid.ThingsAt(c))
						{
							if (thing2.def == resDef)
							{
								thing = thing2;
								goto IL_9D;
							}
						}
					}
				}
				IL_9D:
				if (thing == null)
				{
					Log.Error("Could not find any " + ((resDef != null) ? resDef.ToString() : null) + " to transfer to trader.");
					return;
				}
				int num = Math.Min(debt, thing.stackCount);
				if (trader != null)
				{
					trader.GiveSoldThingToTrader(thing, num, TradeSession.playerNegotiator);
				}
				else
				{
					thing.SplitOff(num).Destroy(DestroyMode.Vanish);
				}
				debt -= num;
			}
		}

		// Token: 0x0601136F RID: 70511 RVA: 0x004EEC00 File Offset: 0x004ECE00
		public static void LaunchSilver(Map map, int fee)
		{
			TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, fee, map, null);
		}

		// Token: 0x06011370 RID: 70512 RVA: 0x004EEC10 File Offset: 0x004ECE10
		public static Map PlayerHomeMapWithMostLaunchableSilver()
		{
			return (from x in Find.Maps
			where x.IsPlayerHome
			select x).MaxBy((Map x) => (from t in TradeUtility.AllLaunchableThingsForTrade(x, null)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount));
		}

		// Token: 0x06011371 RID: 70513 RVA: 0x004EEC6C File Offset: 0x004ECE6C
		public static bool ColonyHasEnoughSilver(Map map, int fee)
		{
			return (from t in TradeUtility.AllLaunchableThingsForTrade(map, null)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount) >= fee;
		}

		// Token: 0x06011372 RID: 70514 RVA: 0x004EECD0 File Offset: 0x004ECED0
		public static void CheckInteractWithTradersTeachOpportunity(Pawn pawn)
		{
			if (pawn.Dead)
			{
				return;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && lord.CurLordToil is LordToil_DefendTraderCaravan)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.InteractingWithTraders, pawn, OpportunityType.Important);
			}
		}

		// Token: 0x06011373 RID: 70515 RVA: 0x004EED0C File Offset: 0x004ECF0C
		public static float GetPricePlayerSell(Thing thing, float priceFactorSell_TraderPriceType, float priceFactorSell_HumanPawn, float priceGain_PlayerNegotiator, float priceGain_FactionBase, float priceGain_DrugBonus, float priceGain_AnimalProduceBonus, TradeCurrency currency = TradeCurrency.Silver)
		{
			if (currency == TradeCurrency.Favor)
			{
				return thing.RoyalFavorValue;
			}
			float statValue = thing.GetStatValue(StatDefOf.SellPriceFactor, true, -1);
			float num = thing.MarketValue * 0.6f * priceFactorSell_TraderPriceType * statValue * priceFactorSell_HumanPawn * (1f - Find.Storyteller.difficulty.tradePriceFactorLoss);
			num *= 1f + priceGain_PlayerNegotiator + priceGain_DrugBonus + priceGain_FactionBase + priceGain_AnimalProduceBonus;
			num = Mathf.Max(num, 0.01f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x06011374 RID: 70516 RVA: 0x004EED8C File Offset: 0x004ECF8C
		public static float GetPricePlayerBuy(Thing thing, float priceFactorBuy_TraderPriceType, float priceFactorBuy_JoinAs, float priceGain_PlayerNegotiator, float priceGain_FactionBase)
		{
			float num = thing.MarketValue * 1.4f * priceFactorBuy_TraderPriceType * priceFactorBuy_JoinAs * (1f + Find.Storyteller.difficulty.tradePriceFactorLoss);
			num *= 1f - priceGain_PlayerNegotiator - priceGain_FactionBase;
			num = Mathf.Max(num, 0.5f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x06011375 RID: 70517 RVA: 0x004EEDEC File Offset: 0x004ECFEC
		public static float TotalMarketValue(IEnumerable<Thing> things)
		{
			if (things == null)
			{
				return 0f;
			}
			float num = 0f;
			foreach (Thing thing in things)
			{
				num += thing.MarketValue * (float)thing.stackCount;
			}
			return num;
		}

		// Token: 0x06011376 RID: 70518 RVA: 0x004EEE50 File Offset: 0x004ED050
		public static void CheckGiveTraderQuest(Pawn trader)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			if (!Rand.Chance(Find.Storyteller.QuestGiverTraderChanceNow))
			{
				return;
			}
			trader.mindState.hasQuest = true;
			Find.Storyteller.Notify_QuestGiverTraderSpawned();
		}

		// Token: 0x06011377 RID: 70519 RVA: 0x004EEE84 File Offset: 0x004ED084
		public static void ReceiveQuestFromTrader(Pawn trader, Pawn negotiator)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Slate slate = new Slate();
			slate.Set<float>("points", StorytellerUtility.DefaultThreatPointsNow(negotiator.Map), false);
			slate.Set<TaggedString>("discoveryMethod", "QuestDiscoveredFromTrader".Translate(trader.Named("TRADER"), negotiator.Named("NEGOTIATOR")), false);
			List<QuestScriptDef> list = QuestUtility.GetGiverQuests(QuestGiverTag.Traders).ToList<QuestScriptDef>();
			if (list.NullOrEmpty<QuestScriptDef>())
			{
				return;
			}
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(list.RandomElementByWeight((QuestScriptDef q) => q.rootSelectionWeight), slate);
			Messages.Message("MessageTraderGaveQuest".Translate(quest.name, trader.Named("TRADER"), negotiator.Named("NEGOTIATOR")), MessageTypeDefOf.PositiveEvent, true);
			if (!quest.hidden && quest.root.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, slate.Get<string>("discoveryMethod", null, false));
			}
			trader.mindState.hasQuest = false;
		}

		// Token: 0x0400B29C RID: 45724
		public const float MinimumBuyPrice = 0.5f;

		// Token: 0x0400B29D RID: 45725
		public const float MinimumSellPrice = 0.01f;

		// Token: 0x0400B29E RID: 45726
		public const float PriceFactorBuy_Global = 1.4f;

		// Token: 0x0400B29F RID: 45727
		public const float PriceFactorSell_Global = 0.6f;
	}
}
