using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DBF RID: 15807
	[StaticConstructorOnStartup]
	public class Settlement : MapParent, ITrader, ITraderRestockingInfoProvider, INameableWorldObject
	{
		// Token: 0x1700359A RID: 13722
		// (get) Token: 0x060158C1 RID: 88257 RVA: 0x0067411A File Offset: 0x0067231A
		// (set) Token: 0x060158C2 RID: 88258 RVA: 0x00674122 File Offset: 0x00672322
		public string Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		// Token: 0x1700359B RID: 13723
		// (get) Token: 0x060158C3 RID: 88259 RVA: 0x0067412B File Offset: 0x0067232B
		public override Texture2D ExpandingIcon
		{
			get
			{
				return base.Faction.def.FactionIcon;
			}
		}

		// Token: 0x1700359C RID: 13724
		// (get) Token: 0x060158C4 RID: 88260 RVA: 0x0067413D File Offset: 0x0067233D
		public override string Label
		{
			get
			{
				return this.nameInt ?? base.Label;
			}
		}

		// Token: 0x1700359D RID: 13725
		// (get) Token: 0x060158C5 RID: 88261 RVA: 0x0067414F File Offset: 0x0067234F
		public override bool HasName
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x1700359E RID: 13726
		// (get) Token: 0x060158C6 RID: 88262 RVA: 0x0067415F File Offset: 0x0067235F
		protected override bool UseGenericEnterMapFloatMenuOption
		{
			get
			{
				return !this.Attackable;
			}
		}

		// Token: 0x1700359F RID: 13727
		// (get) Token: 0x060158C7 RID: 88263 RVA: 0x0067416C File Offset: 0x0067236C
		public virtual bool Visitable
		{
			get
			{
				return base.Faction != Faction.OfPlayer && (base.Faction == null || !base.Faction.HostileTo(Faction.OfPlayer)) && !base.Tile.LayerDef.isSpace;
			}
		}

		// Token: 0x170035A0 RID: 13728
		// (get) Token: 0x060158C8 RID: 88264 RVA: 0x006741B8 File Offset: 0x006723B8
		public virtual bool Attackable
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x170035A1 RID: 13729
		// (get) Token: 0x060158C9 RID: 88265 RVA: 0x006741B8 File Offset: 0x006723B8
		public override bool ShowRelatedQuests
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x170035A2 RID: 13730
		// (get) Token: 0x060158CA RID: 88266 RVA: 0x006741B8 File Offset: 0x006723B8
		public override bool GravShipCanLandOn
		{
			get
			{
				return base.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x170035A3 RID: 13731
		// (get) Token: 0x060158CB RID: 88267 RVA: 0x000F9927 File Offset: 0x000F7B27
		public override AcceptanceReport CanBeSettled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170035A4 RID: 13732
		// (get) Token: 0x060158CC RID: 88268 RVA: 0x006741CC File Offset: 0x006723CC
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					this.cachedMat = MaterialPool.MatFrom(base.Faction.def.settlementTexturePath, ShaderDatabase.WorldOverlayTransparentLit, base.Faction.Color, 3550);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x170035A5 RID: 13733
		// (get) Token: 0x060158CD RID: 88269 RVA: 0x0067421D File Offset: 0x0067241D
		public override MapGeneratorDef MapGeneratorDef
		{
			get
			{
				if (this.def.mapGenerator != null)
				{
					return this.def.mapGenerator;
				}
				if (base.Faction == Faction.OfPlayer)
				{
					return MapGeneratorDefOf.Base_Player;
				}
				return MapGeneratorDefOf.Base_Faction;
			}
		}

		// Token: 0x170035A6 RID: 13734
		// (get) Token: 0x060158CE RID: 88270 RVA: 0x00674250 File Offset: 0x00672450
		public TraderKindDef TraderKind
		{
			get
			{
				Settlement_TraderTracker settlement_TraderTracker = this.trader;
				if (settlement_TraderTracker == null)
				{
					return null;
				}
				return settlement_TraderTracker.TraderKind;
			}
		}

		// Token: 0x170035A7 RID: 13735
		// (get) Token: 0x060158CF RID: 88271 RVA: 0x00674263 File Offset: 0x00672463
		public IEnumerable<Thing> Goods
		{
			get
			{
				Settlement_TraderTracker settlement_TraderTracker = this.trader;
				if (settlement_TraderTracker == null)
				{
					return null;
				}
				return settlement_TraderTracker.StockListForReading;
			}
		}

		// Token: 0x170035A8 RID: 13736
		// (get) Token: 0x060158D0 RID: 88272 RVA: 0x00674276 File Offset: 0x00672476
		public int RandomPriceFactorSeed
		{
			get
			{
				Settlement_TraderTracker settlement_TraderTracker = this.trader;
				if (settlement_TraderTracker == null)
				{
					return 0;
				}
				return settlement_TraderTracker.RandomPriceFactorSeed;
			}
		}

		// Token: 0x170035A9 RID: 13737
		// (get) Token: 0x060158D1 RID: 88273 RVA: 0x00674289 File Offset: 0x00672489
		public string TraderName
		{
			get
			{
				Settlement_TraderTracker settlement_TraderTracker = this.trader;
				if (settlement_TraderTracker == null)
				{
					return null;
				}
				return settlement_TraderTracker.TraderName;
			}
		}

		// Token: 0x170035AA RID: 13738
		// (get) Token: 0x060158D2 RID: 88274 RVA: 0x0067429C File Offset: 0x0067249C
		public bool CanTradeNow
		{
			get
			{
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		// Token: 0x170035AB RID: 13739
		// (get) Token: 0x060158D3 RID: 88275 RVA: 0x006742B3 File Offset: 0x006724B3
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				Settlement_TraderTracker settlement_TraderTracker = this.trader;
				if (settlement_TraderTracker == null)
				{
					return 0f;
				}
				return settlement_TraderTracker.TradePriceImprovementOffsetForPlayer;
			}
		}

		// Token: 0x170035AC RID: 13740
		// (get) Token: 0x060158D4 RID: 88276 RVA: 0x006742CA File Offset: 0x006724CA
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		// Token: 0x060158D5 RID: 88277 RVA: 0x006742D7 File Offset: 0x006724D7
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			Settlement_TraderTracker settlement_TraderTracker = this.trader;
			if (settlement_TraderTracker == null)
			{
				return null;
			}
			return settlement_TraderTracker.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x060158D6 RID: 88278 RVA: 0x006742EB File Offset: 0x006724EB
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x060158D7 RID: 88279 RVA: 0x006742FB File Offset: 0x006724FB
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x170035AD RID: 13741
		// (get) Token: 0x060158D8 RID: 88280 RVA: 0x0067430B File Offset: 0x0067250B
		public bool EverVisited
		{
			get
			{
				return this.trader.EverVisited;
			}
		}

		// Token: 0x170035AE RID: 13742
		// (get) Token: 0x060158D9 RID: 88281 RVA: 0x00674318 File Offset: 0x00672518
		public bool RestockedSinceLastVisit
		{
			get
			{
				return this.trader.RestockedSinceLastVisit;
			}
		}

		// Token: 0x170035AF RID: 13743
		// (get) Token: 0x060158DA RID: 88282 RVA: 0x00674325 File Offset: 0x00672525
		public int NextRestockTick
		{
			get
			{
				return this.trader.NextRestockTick;
			}
		}

		// Token: 0x060158DB RID: 88283 RVA: 0x00674332 File Offset: 0x00672532
		public Settlement()
		{
			this.trader = new Settlement_TraderTracker(this);
		}

		// Token: 0x060158DC RID: 88284 RVA: 0x00674351 File Offset: 0x00672551
		public override IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			foreach (IncidentTargetTagDef incidentTargetTagDef in base.IncidentTargetTags())
			{
				yield return incidentTargetTagDef;
			}
			IEnumerator<IncidentTargetTagDef> enumerator = null;
			if (base.Faction == null || base.Faction == Faction.OfPlayer || SettlementDefeatUtility.IsDefeated(base.Map, base.Faction))
			{
				yield return IncidentTargetTagDefOf.Map_PlayerHome;
			}
			else
			{
				yield return IncidentTargetTagDefOf.Map_Misc;
			}
			yield break;
			yield break;
		}

		// Token: 0x060158DD RID: 88285 RVA: 0x00674364 File Offset: 0x00672564
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.previouslyGeneratedInhabitants, "previouslyGeneratedInhabitants", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<Settlement_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Values.Look<string>(ref this.nameInt, "nameInt", null, false);
			Scribe_Values.Look<bool>(ref this.namedByPlayer, "namedByPlayer", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.previouslyGeneratedInhabitants.RemoveAll((Pawn x) => x == null);
				if (base.Faction == null)
				{
					Log.Warning("Settlement '" + this.Name + "' had null faction on load - destroying.");
					this.Destroy();
				}
			}
		}

		// Token: 0x060158DE RID: 88286 RVA: 0x00674428 File Offset: 0x00672628
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			TimedDetectionRaids timedDetectionRaids;
			if (!base.Map.IsPlayerHome && base.TryGetComponent<TimedDetectionRaids>(out timedDetectionRaids))
			{
				timedDetectionRaids.StartDetectionCountdown(240000, -1);
			}
		}

		// Token: 0x060158DF RID: 88287 RVA: 0x0067445E File Offset: 0x0067265E
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.trader != null)
			{
				this.trader.TraderTrackerTick();
			}
			SettlementDefeatUtility.CheckDefeated(this);
		}

		// Token: 0x060158E0 RID: 88288 RVA: 0x00674480 File Offset: 0x00672680
		public override void Notify_MyMapRemoved(Map map)
		{
			base.Notify_MyMapRemoved(map);
			for (int i = this.previouslyGeneratedInhabitants.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.previouslyGeneratedInhabitants[i];
				if (pawn.DestroyedOrNull() || !pawn.IsWorldPawn())
				{
					this.previouslyGeneratedInhabitants.RemoveAt(i);
				}
			}
		}

		// Token: 0x060158E1 RID: 88289 RVA: 0x006744D8 File Offset: 0x006726D8
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			return (!ModsConfig.OdysseyActive || !base.Map.listerThings.AnyThingWithDef(ThingDefOf.GravAnchor)) && !base.Map.IsPlayerHome && !base.Map.mapPawns.AnyPawnBlockingMapRemoval && !TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map);
		}

		// Token: 0x060158E2 RID: 88290 RVA: 0x00674539 File Offset: 0x00672739
		public override void PostRemove()
		{
			base.PostRemove();
			if (this.trader != null)
			{
				this.trader.TryDestroyStock();
			}
		}

		// Token: 0x060158E3 RID: 88291 RVA: 0x00674554 File Offset: 0x00672754
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Faction != Faction.OfPlayer)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += base.Faction.PlayerRelationKind.GetLabelCap();
				if (!base.Faction.Hidden)
				{
					text = text + " (" + base.Faction.PlayerGoodwill.ToStringWithSign() + ")";
				}
				TraderKindDef traderKind = this.TraderKind;
				RoyalTitleDef royalTitleDef = (traderKind != null) ? traderKind.TitleRequiredToTrade : null;
				if (royalTitleDef != null)
				{
					text += "\n" + "RequiresTradePermission".Translate(royalTitleDef.GetLabelCapForBothGenders());
				}
			}
			else
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "Settled".Translate() + ": " + GenDate.DateShortStringAt((long)GenDate.TickGameToAbs(this.creationGameTicks), Find.WorldGrid.LongLatOf(base.Tile));
				text += " (";
				text += "TimeAgo".Translate((Find.TickManager.TicksGame - this.creationGameTicks).ToStringTicksToPeriodVague(true, true));
				text += ")";
			}
			return text;
		}

		// Token: 0x060158E4 RID: 88292 RVA: 0x006746C0 File Offset: 0x006728C0
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.TraderKind != null && !base.Faction.def.permanentEnemy)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandShowSellableItems".Translate(),
					defaultDesc = "CommandShowSellableItemsDesc".Translate(),
					icon = Settlement.ShowSellableItemsCommand,
					action = delegate()
					{
						Find.WindowStack.Add(new Dialog_SellableItems(this));
						RoyalTitleDef titleRequiredToTrade = this.TraderKind.TitleRequiredToTrade;
						if (titleRequiredToTrade != null)
						{
							TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.TradingRequiresPermit, new string[]
							{
								titleRequiredToTrade.GetLabelCapForBothGenders()
							});
						}
					}
				};
			}
			if (base.Faction != Faction.OfPlayer && !PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.FormCaravan) && base.Tile.LayerDef.canFormCaravans)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "CommandFormCaravan".Translate();
				command_Action.defaultDesc = "CommandFormCaravanDesc".Translate();
				command_Action.icon = Settlement.FormCaravanCommand;
				command_Action.action = delegate()
				{
					Find.Tutor.learningReadout.TryActivateConcept(ConceptDefOf.FormCaravan);
					Messages.Message("MessageSelectOwnBaseToFormCaravan".Translate(), MessageTypeDefOf.RejectInput, false);
				};
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x060158E5 RID: 88293 RVA: 0x006746D0 File Offset: 0x006728D0
		public override IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			if (this.CanTradeNow && CaravanVisitUtility.SettlementVisitedNow(caravan) == this)
			{
				yield return CaravanVisitUtility.TradeCommand(caravan, base.Faction, this.TraderKind);
			}
			if (CaravanArrivalAction_OfferGifts.CanOfferGiftsTo(caravan, this))
			{
				yield return FactionGiftUtility.OfferGiftsCommand(caravan, this);
			}
			foreach (Gizmo gizmo in base.GetCaravanGizmos(caravan))
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.Attackable)
			{
				yield return new Command_Action
				{
					icon = Settlement.AttackCommand,
					defaultLabel = "CommandAttackSettlement".Translate(),
					defaultDesc = "CommandAttackSettlementDesc".Translate(),
					action = delegate()
					{
						SettlementUtility.Attack(caravan, this);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x060158E6 RID: 88294 RVA: 0x006746E7 File Offset: 0x006728E7
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(caravan))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (CaravanVisitUtility.SettlementVisitedNow(caravan) != this)
			{
				foreach (FloatMenuOption floatMenuOption2 in CaravanArrivalAction_VisitSettlement.GetFloatMenuOptions(caravan, this))
				{
					yield return floatMenuOption2;
				}
				enumerator = null;
			}
			foreach (FloatMenuOption floatMenuOption3 in CaravanArrivalAction_Trade.GetFloatMenuOptions(caravan, this))
			{
				yield return floatMenuOption3;
			}
			enumerator = null;
			foreach (FloatMenuOption floatMenuOption4 in CaravanArrivalAction_OfferGifts.GetFloatMenuOptions(caravan, this))
			{
				yield return floatMenuOption4;
			}
			enumerator = null;
			foreach (FloatMenuOption floatMenuOption5 in CaravanArrivalAction_AttackSettlement.GetFloatMenuOptions(caravan, this))
			{
				yield return floatMenuOption5;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060158E7 RID: 88295 RVA: 0x006746FE File Offset: 0x006728FE
		public override IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetTransportersFloatMenuOptions(pods, launchAction))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (FloatMenuOption floatMenuOption2 in TransportersArrivalAction_VisitSettlement.GetFloatMenuOptions(launchAction, pods, this, false))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			foreach (FloatMenuOption floatMenuOption3 in TransportersArrivalAction_GiveGift.GetFloatMenuOptions(launchAction, pods, this))
			{
				yield return floatMenuOption3;
			}
			enumerator = null;
			if (!base.HasMap)
			{
				foreach (FloatMenuOption floatMenuOption4 in TransportersArrivalAction_AttackSettlement.GetFloatMenuOptions(launchAction, pods, this))
				{
					yield return floatMenuOption4;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060158E8 RID: 88296 RVA: 0x0067471C File Offset: 0x0067291C
		public override IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetShuttleFloatMenuOptions(pods, launchAction))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (TransportersArrivalAction_VisitSettlement.CanVisit(pods, this))
			{
				yield return new FloatMenuOption("VisitSettlement".Translate(this.Label), delegate()
				{
					launchAction(this.Tile, new TransportersArrivalAction_VisitSettlement(this, "MessageShuttleArrived"));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (TransportersArrivalAction_Trade.CanTradeWith(pods, this))
			{
				yield return new FloatMenuOption("TradeWithSettlement".Translate(this.Label), delegate()
				{
					launchAction(this.Tile, new TransportersArrivalAction_Trade(this, "MessageShuttleArrived"));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!base.HasMap)
			{
				IThingHolder thingHolder = pods.FirstOrDefault<IThingHolder>();
				CompTransporter firstPod = thingHolder as CompTransporter;
				if (firstPod != null && firstPod.Shuttle.shipParent != null)
				{
					TaggedString message = base.Faction.HostileTo(Faction.OfPlayer) ? "ConfirmLandOnHostileFactionBase".Translate(base.Faction) : "ConfirmLandOnNeutralFactionBase".Translate(base.Faction);
					Func<FloatMenuAcceptanceReport> <>9__2;
					Func<FloatMenuAcceptanceReport> acceptanceReportGetter;
					if ((acceptanceReportGetter = <>9__2) == null)
					{
						acceptanceReportGetter = (<>9__2 = (() => TransportersArrivalAction_AttackSettlement.CanAttack(pods, this)));
					}
					Func<TransportersArrivalAction_TransportShip> <>9__3;
					Func<TransportersArrivalAction_TransportShip> arrivalActionGetter;
					if ((arrivalActionGetter = <>9__3) == null)
					{
						arrivalActionGetter = (<>9__3 = (() => new TransportersArrivalAction_TransportShip(this, firstPod.Shuttle.shipParent)));
					}
					string label = "AttackShuttle".Translate(this.Label);
					Action<PlanetTile, TransportersArrivalAction> launchAction2;
					Action<PlanetTile, TransportersArrivalAction> <>9__4;
					if ((launchAction2 = <>9__4) == null)
					{
						launchAction2 = (<>9__4 = delegate(PlanetTile t, TransportersArrivalAction s)
						{
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(message, delegate()
							{
								launchAction(t, s);
							}, false, null, WindowLayer.Dialog));
						});
					}
					foreach (FloatMenuOption floatMenuOption2 in TransportersArrivalActionUtility.GetFloatMenuOptions<TransportersArrivalAction_TransportShip>(acceptanceReportGetter, arrivalActionGetter, label, launchAction2, base.Tile, null))
					{
						yield return floatMenuOption2;
					}
					enumerator = null;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060158E9 RID: 88297 RVA: 0x0067473A File Offset: 0x0067293A
		public override void GetChildHolders(List<IThingHolder> outChildren)
		{
			base.GetChildHolders(outChildren);
			if (this.trader != null)
			{
				outChildren.Add(this.trader);
			}
		}

		// Token: 0x060158EA RID: 88298 RVA: 0x00674758 File Offset: 0x00672958
		public override void Abandon(bool wasGravshipLaunch)
		{
			base.Abandon(wasGravshipLaunch);
			if (!wasGravshipLaunch && base.Tile.LayerDef == PlanetLayerDefOf.Surface)
			{
				WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedSettlement);
				worldObject.Tile = base.Tile;
				worldObject.SetFaction(base.Faction);
				Find.WorldObjects.Add(worldObject);
			}
		}

		// Token: 0x0400EF96 RID: 61334
		public Settlement_TraderTracker trader;

		// Token: 0x0400EF97 RID: 61335
		public List<Pawn> previouslyGeneratedInhabitants = new List<Pawn>();

		// Token: 0x0400EF98 RID: 61336
		private string nameInt;

		// Token: 0x0400EF99 RID: 61337
		public bool namedByPlayer;

		// Token: 0x0400EF9A RID: 61338
		private Material cachedMat;

		// Token: 0x0400EF9B RID: 61339
		public static readonly Texture2D ShowSellableItemsCommand = ContentFinder<Texture2D>.Get("UI/Commands/SellableItems", true);

		// Token: 0x0400EF9C RID: 61340
		public static readonly Texture2D FormCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/FormCaravan", true);

		// Token: 0x0400EF9D RID: 61341
		public static readonly Texture2D AttackCommand = ContentFinder<Texture2D>.Get("UI/Commands/AttackSettlement", true);
	}
}
