using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.Planet
{
	// Token: 0x02003C1E RID: 15390
	[StaticConstructorOnStartup]
	public static class FactionGiftUtility
	{
		// Token: 0x06014DFA RID: 85498 RVA: 0x006405E4 File Offset: 0x0063E7E4
		public static Command OfferGiftsCommand(Caravan caravan, Settlement settlement)
		{
			return new Command_Action
			{
				defaultLabel = "CommandOfferGifts".Translate(),
				defaultDesc = "CommandOfferGiftsDesc".Translate(),
				icon = FactionGiftUtility.OfferGiftsCommandTex,
				action = delegate()
				{
					Pawn playerNegotiator = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null);
					Find.WindowStack.Add(new Dialog_Trade(playerNegotiator, settlement, true));
				}
			};
		}

		// Token: 0x06014DFB RID: 85499 RVA: 0x00640654 File Offset: 0x0063E854
		public static void GiveGift(List<Tradeable> tradeables, Faction giveTo, GlobalTargetInfo lookTarget)
		{
			int goodwillChange = FactionGiftUtility.GetGoodwillChange(tradeables, giveTo);
			for (int i = 0; i < tradeables.Count; i++)
			{
				if (tradeables[i].ActionToDo == TradeAction.PlayerSells)
				{
					tradeables[i].ResolveTrade();
				}
			}
			if (giveTo.PlayerGoodwill == 100)
			{
				FactionGiftUtility.SendGiftNotAppreciatedMessage(giveTo, lookTarget);
			}
			Faction.OfPlayer.TryAffectGoodwillWith(giveTo, goodwillChange, true, true, HistoryEventDefOf.GaveGift, null);
		}

		// Token: 0x06014DFC RID: 85500 RVA: 0x006406C4 File Offset: 0x0063E8C4
		public static void GiveGift(List<ActiveTransporterInfo> pods, Settlement giveTo)
		{
			int goodwillChange = FactionGiftUtility.GetGoodwillChange(pods.Cast<IThingHolder>(), giveTo);
			for (int i = 0; i < pods.Count; i++)
			{
				ThingOwner innerContainer = pods[i].innerContainer;
				for (int j = innerContainer.Count - 1; j >= 0; j--)
				{
					FactionGiftUtility.GiveGiftInternal(innerContainer[j], innerContainer[j].stackCount, giveTo.Faction);
					if (j < innerContainer.Count)
					{
						innerContainer.RemoveAt(j);
					}
				}
			}
			if (giveTo.Faction.PlayerGoodwill == 100)
			{
				FactionGiftUtility.SendGiftNotAppreciatedMessage(giveTo.Faction, giveTo);
			}
			Faction.OfPlayer.TryAffectGoodwillWith(giveTo.Faction, goodwillChange, true, true, HistoryEventDefOf.GaveGift, null);
		}

		// Token: 0x06014DFD RID: 85501 RVA: 0x00640780 File Offset: 0x0063E980
		private static void GiveGiftInternal(Thing thing, int count, Faction giveTo)
		{
			Thing thing2 = thing.SplitOff(count);
			Pawn pawn = thing2 as Pawn;
			if (pawn != null)
			{
				pawn.SetFaction(giveTo, null);
			}
			thing2.DestroyOrPassToWorld(DestroyMode.Vanish);
		}

		// Token: 0x06014DFE RID: 85502 RVA: 0x006407AC File Offset: 0x0063E9AC
		public static bool CheckCanCarryGift(List<Tradeable> tradeables, ITrader trader)
		{
			Pawn pawn = trader as Pawn;
			if (pawn == null)
			{
				return true;
			}
			float num = 0f;
			float num2 = 0f;
			Lord lord = pawn.GetLord();
			if (lord != null)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn2 = lord.ownedPawns[i];
					TraderCaravanRole traderCaravanRole = pawn2.GetTraderCaravanRole();
					if ((pawn2.RaceProps.Humanlike && traderCaravanRole != TraderCaravanRole.Guard) || traderCaravanRole == TraderCaravanRole.Carrier)
					{
						num += MassUtility.Capacity(pawn2, null);
						num2 += MassUtility.GearAndInventoryMass(pawn2);
					}
				}
			}
			else
			{
				num = MassUtility.Capacity(pawn, null);
				num2 = MassUtility.GearAndInventoryMass(pawn);
			}
			float num3 = 0f;
			for (int j = 0; j < tradeables.Count; j++)
			{
				if (tradeables[j].ActionToDo == TradeAction.PlayerSells)
				{
					int num4 = Mathf.Min(tradeables[j].CountToTransferToDestination, tradeables[j].CountHeldBy(Transactor.Colony));
					if (num4 > 0)
					{
						num3 += tradeables[j].AnyThing.GetStatValue(StatDefOf.Mass, true, -1) * (float)num4;
					}
				}
			}
			if (num2 + num3 <= num)
			{
				return true;
			}
			float num5 = num - num2;
			if (num5 <= 0f)
			{
				Messages.Message("MessageCantGiveGiftBecauseCantCarryEncumbered".Translate(), MessageTypeDefOf.RejectInput, false);
			}
			else
			{
				Messages.Message("MessageCantGiveGiftBecauseCantCarry".Translate(num3.ToStringMass(), num5.ToStringMass()), MessageTypeDefOf.RejectInput, false);
			}
			return false;
		}

		// Token: 0x06014DFF RID: 85503 RVA: 0x00640928 File Offset: 0x0063EB28
		public static int GetGoodwillChange(IEnumerable<IThingHolder> pods, Settlement giveTo)
		{
			float num = 0f;
			foreach (IThingHolder thingHolder in pods)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				for (int i = 0; i < directlyHeldThings.Count; i++)
				{
					float singlePrice;
					if (directlyHeldThings[i].def == ThingDefOf.Silver)
					{
						singlePrice = directlyHeldThings[i].MarketValue;
					}
					else
					{
						float priceFactorSell_TraderPriceType = (giveTo.TraderKind != null) ? giveTo.TraderKind.PriceTypeFor(directlyHeldThings[i].def, TradeAction.PlayerSells).PriceMultiplier() : 1f;
						singlePrice = TradeUtility.GetPricePlayerSell(directlyHeldThings[i], priceFactorSell_TraderPriceType, 1f, 0f, giveTo.TradePriceImprovementOffsetForPlayer, 0f, 0f, TradeCurrency.Silver);
					}
					num += FactionGiftUtility.GetBaseGoodwillChange(directlyHeldThings[i], directlyHeldThings[i].stackCount, singlePrice, giveTo.Faction);
				}
			}
			return FactionGiftUtility.PostProcessedGoodwillChange(num, giveTo.Faction);
		}

		// Token: 0x06014E00 RID: 85504 RVA: 0x00640A3C File Offset: 0x0063EC3C
		public static int GetGoodwillChange(List<Tradeable> tradeables, Faction theirFaction)
		{
			float num = 0f;
			for (int i = 0; i < tradeables.Count; i++)
			{
				if (tradeables[i].ActionToDo == TradeAction.PlayerSells)
				{
					int count = Mathf.Min(tradeables[i].CountToTransferToDestination, tradeables[i].CountHeldBy(Transactor.Colony));
					num += FactionGiftUtility.GetBaseGoodwillChange(tradeables[i].AnyThing, count, tradeables[i].GetPriceFor(TradeAction.PlayerSells), theirFaction);
				}
			}
			return FactionGiftUtility.PostProcessedGoodwillChange(num, theirFaction);
		}

		// Token: 0x06014E01 RID: 85505 RVA: 0x00640AB8 File Offset: 0x0063ECB8
		private static float GetBaseGoodwillChange(Thing anyThing, int count, float singlePrice, Faction theirFaction)
		{
			if (count <= 0)
			{
				return 0f;
			}
			float num = singlePrice * (float)count;
			Pawn pawn = anyThing as Pawn;
			if (pawn != null && pawn.IsPrisoner && pawn.Faction == theirFaction)
			{
				num *= 2f;
			}
			return num / 40f;
		}

		// Token: 0x06014E02 RID: 85506 RVA: 0x00640B00 File Offset: 0x0063ED00
		private static int PostProcessedGoodwillChange(float goodwillChange, Faction theirFaction)
		{
			float num = (float)theirFaction.PlayerGoodwill;
			float num2 = 0f;
			SimpleCurve giftGoodwillFactorRelationsCurve = DiplomacyTuning.GiftGoodwillFactorRelationsCurve;
			while (goodwillChange >= 0.25f)
			{
				num2 += 0.25f * giftGoodwillFactorRelationsCurve.Evaluate(Mathf.Min(num + num2, 100f));
				goodwillChange -= 0.25f;
				if (num2 >= 200f)
				{
					break;
				}
			}
			if (num2 < 200f)
			{
				num2 += goodwillChange * giftGoodwillFactorRelationsCurve.Evaluate(Mathf.Min(num + num2, 100f));
			}
			return (int)Mathf.Min(num2, 200f);
		}

		// Token: 0x06014E03 RID: 85507 RVA: 0x00640B84 File Offset: 0x0063ED84
		private static void SendGiftNotAppreciatedMessage(Faction giveTo, GlobalTargetInfo lookTarget)
		{
			Messages.Message("MessageGiftGivenButNotAppreciated".Translate(giveTo.Name), lookTarget, MessageTypeDefOf.NegativeEvent, true);
		}

		// Token: 0x0400E8A5 RID: 59557
		private static readonly Texture2D OfferGiftsCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/OfferGifts", true);
	}
}
