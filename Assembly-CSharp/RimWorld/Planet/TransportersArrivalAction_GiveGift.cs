using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E07 RID: 15879
	public class TransportersArrivalAction_GiveGift : TransportersArrivalAction
	{
		// Token: 0x170035F8 RID: 13816
		// (get) Token: 0x06015A9C RID: 88732 RVA: 0x00002501 File Offset: 0x00000701
		public override bool GeneratesMap
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06015A9D RID: 88733 RVA: 0x0067B2C7 File Offset: 0x006794C7
		public TransportersArrivalAction_GiveGift()
		{
		}

		// Token: 0x06015A9E RID: 88734 RVA: 0x0067BAE5 File Offset: 0x00679CE5
		public TransportersArrivalAction_GiveGift(Settlement settlement)
		{
			this.settlement = settlement;
		}

		// Token: 0x06015A9F RID: 88735 RVA: 0x0067BAF4 File Offset: 0x00679CF4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Settlement>(ref this.settlement, "settlement", false);
		}

		// Token: 0x06015AA0 RID: 88736 RVA: 0x0067BB10 File Offset: 0x00679D10
		public override FloatMenuAcceptanceReport StillValid(IEnumerable<IThingHolder> pods, PlanetTile destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(pods, destinationTile);
			if (!floatMenuAcceptanceReport)
			{
				return floatMenuAcceptanceReport;
			}
			if (this.settlement != null && this.settlement.Tile != destinationTile)
			{
				return false;
			}
			return TransportersArrivalAction_GiveGift.CanGiveGiftTo(pods, this.settlement);
		}

		// Token: 0x06015AA1 RID: 88737 RVA: 0x0067BB60 File Offset: 0x00679D60
		public override void Arrived(List<ActiveTransporterInfo> transporters, PlanetTile tile)
		{
			for (int i = 0; i < transporters.Count; i++)
			{
				for (int j = 0; j < transporters[i].innerContainer.Count; j++)
				{
					Pawn pawn = transporters[i].innerContainer[j] as Pawn;
					if (pawn != null)
					{
						if (pawn.RaceProps.Humanlike)
						{
							Pawn arg;
							if (pawn.HomeFaction == this.settlement.Faction)
							{
								GenGuest.AddHealthyPrisonerReleasedThoughts(pawn);
							}
							else if (PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.TryRandomElement(out arg))
							{
								Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SoldSlave, arg.Named(HistoryEventArgsNames.Doer)), true);
							}
						}
						else if (pawn.RaceProps.Animal && pawn.relations != null)
						{
							Pawn firstDirectRelationPawn = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, null);
							if (firstDirectRelationPawn != null && firstDirectRelationPawn.needs.mood != null)
							{
								pawn.relations.RemoveDirectRelation(PawnRelationDefOf.Bond, firstDirectRelationPawn);
								firstDirectRelationPawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SoldMyBondedAnimalMood, null, null);
							}
						}
					}
					transporters[i].innerContainer[j].Notify_AbandonedAtTile(tile);
				}
			}
			FactionGiftUtility.GiveGift(transporters, this.settlement);
		}

		// Token: 0x06015AA2 RID: 88738 RVA: 0x0067BCB4 File Offset: 0x00679EB4
		public static FloatMenuAcceptanceReport CanGiveGiftTo(IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			foreach (IThingHolder thingHolder in pods)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				for (int i = 0; i < directlyHeldThings.Count; i++)
				{
					Pawn pawn = directlyHeldThings[i] as Pawn;
					if (pawn != null && pawn.IsQuestLodger())
					{
						return false;
					}
				}
			}
			return settlement != null && settlement.Spawned && settlement.Faction != null && settlement.Faction != Faction.OfPlayer && !settlement.Faction.def.permanentEnemy && !settlement.HasMap;
		}

		// Token: 0x06015AA3 RID: 88739 RVA: 0x0067BD74 File Offset: 0x00679F74
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Action<PlanetTile, TransportersArrivalAction> launchAction, IEnumerable<IThingHolder> pods, Settlement settlement)
		{
			if (settlement.Faction == Faction.OfPlayer)
			{
				return Enumerable.Empty<FloatMenuOption>();
			}
			return TransportersArrivalActionUtility.GetFloatMenuOptions<TransportersArrivalAction_GiveGift>(() => TransportersArrivalAction_GiveGift.CanGiveGiftTo(pods, settlement), () => new TransportersArrivalAction_GiveGift(settlement), "GiveGiftViaTransportPods".Translate(settlement.Faction.Name, FactionGiftUtility.GetGoodwillChange(pods, settlement).ToStringWithSign()), launchAction, settlement.Tile, delegate(Action action)
			{
				TradeRequestComp tradeReqComp = settlement.GetComponent<TradeRequestComp>();
				if (tradeReqComp != null && tradeReqComp.ActiveRequest && pods.Any((IThingHolder p) => p.GetDirectlyHeldThings().Contains(tradeReqComp.requestThingDef)))
				{
					Find.WindowStack.Add(new Dialog_MessageBox("GiveGiftViaTransportPodsTradeRequestWarning".Translate(), "Yes".Translate(), delegate()
					{
						action();
					}, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
					return;
				}
				action();
			});
		}

		// Token: 0x0400F0AF RID: 61615
		private Settlement settlement;
	}
}
