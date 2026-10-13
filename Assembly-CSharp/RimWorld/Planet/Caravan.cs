using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld.Planet
{
	// Token: 0x02003D34 RID: 15668
	[StaticConstructorOnStartup]
	public class Caravan : WorldObject, IThingHolder, IIncidentTarget, ILoadReferenceable, ITrader
	{
		// Token: 0x17003527 RID: 13607
		// (get) Token: 0x06015532 RID: 87346 RVA: 0x00660187 File Offset: 0x0065E387
		public List<Pawn> PawnsListForReading
		{
			get
			{
				return this.pawns.InnerListForReading;
			}
		}

		// Token: 0x17003528 RID: 13608
		// (get) Token: 0x06015533 RID: 87347 RVA: 0x00660194 File Offset: 0x0065E394
		public override float DrawAltitude
		{
			get
			{
				return 0.035f;
			}
		}

		// Token: 0x17003529 RID: 13609
		// (get) Token: 0x06015534 RID: 87348 RVA: 0x0066019C File Offset: 0x0065E39C
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (base.Faction == null)
					{
						color = Color.white;
					}
					else if (base.Faction.IsPlayer)
					{
						color = Caravan.PlayerCaravanColor;
					}
					else
					{
						color = base.Faction.Color;
					}
					this.cachedMat = MaterialPool.MatFrom(this.def.texture, ShaderDatabase.WorldOverlayTransparentLit, color, 3600);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x1700352A RID: 13610
		// (get) Token: 0x06015535 RID: 87349 RVA: 0x0066020F File Offset: 0x0065E40F
		// (set) Token: 0x06015536 RID: 87350 RVA: 0x00660217 File Offset: 0x0065E417
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

		// Token: 0x1700352B RID: 13611
		// (get) Token: 0x06015537 RID: 87351 RVA: 0x00660220 File Offset: 0x0065E420
		public override Vector3 DrawPos
		{
			get
			{
				return this.tweener.TweenedPos;
			}
		}

		// Token: 0x1700352C RID: 13612
		// (get) Token: 0x06015538 RID: 87352 RVA: 0x0066022D File Offset: 0x0065E42D
		public bool IsPlayerControlled
		{
			get
			{
				return base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x1700352D RID: 13613
		// (get) Token: 0x06015539 RID: 87353 RVA: 0x0066023C File Offset: 0x0065E43C
		public bool ImmobilizedByMass
		{
			get
			{
				if (Find.TickManager.TicksGame - this.cachedImmobilizedForTicks < 60)
				{
					return this.cachedImmobilized;
				}
				this.cachedImmobilized = (this.MassUsage > this.MassCapacity);
				this.cachedImmobilizedForTicks = Find.TickManager.TicksGame;
				return this.cachedImmobilized;
			}
		}

		// Token: 0x1700352E RID: 13614
		// (get) Token: 0x0601553A RID: 87354 RVA: 0x00660290 File Offset: 0x0065E490
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		public ValueTuple<float, float> DaysWorthOfFood
		{
			[return: TupleElementNames(new string[]
			{
				"days",
				"tillRot"
			})]
			get
			{
				if (Find.TickManager.TicksGame - this.cachedDaysWorthOfFoodForTicks < 3000)
				{
					return this.cachedDaysWorthOfFood;
				}
				this.cachedDaysWorthOfFood = new ValueTuple<float, float>(DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this), DaysUntilRotCalculator.ApproxDaysUntilRot(this));
				this.cachedDaysWorthOfFoodForTicks = Find.TickManager.TicksGame;
				return this.cachedDaysWorthOfFood;
			}
		}

		// Token: 0x1700352F RID: 13615
		// (get) Token: 0x0601553B RID: 87355 RVA: 0x006602E9 File Offset: 0x0065E4E9
		public bool CantMove
		{
			get
			{
				return this.NightResting || this.pather.Paused || this.AllOwnersHaveMentalBreak || this.AllOwnersDowned || this.ImmobilizedByMass || this.Shuttle != null;
			}
		}

		// Token: 0x17003530 RID: 13616
		// (get) Token: 0x0601553C RID: 87356 RVA: 0x00660323 File Offset: 0x0065E523
		public float MassCapacity
		{
			get
			{
				return CollectionsMassCalculator.Capacity<Pawn>(this.PawnsListForReading, null);
			}
		}

		// Token: 0x17003531 RID: 13617
		// (get) Token: 0x0601553D RID: 87357 RVA: 0x00660334 File Offset: 0x0065E534
		public string MassCapacityExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CollectionsMassCalculator.Capacity<Pawn>(this.PawnsListForReading, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17003532 RID: 13618
		// (get) Token: 0x0601553E RID: 87358 RVA: 0x0066035A File Offset: 0x0065E55A
		public float MassUsage
		{
			get
			{
				return CollectionsMassCalculator.MassUsage<Pawn>(this.PawnsListForReading, IgnorePawnsInventoryMode.DontIgnore, false, false);
			}
		}

		// Token: 0x17003533 RID: 13619
		// (get) Token: 0x0601553F RID: 87359 RVA: 0x0066036C File Offset: 0x0065E56C
		public bool AllOwnersDowned
		{
			get
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.IsOwner(this.pawns[i]) && !this.pawns[i].Downed)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17003534 RID: 13620
		// (get) Token: 0x06015540 RID: 87360 RVA: 0x006603BC File Offset: 0x0065E5BC
		public bool AllOwnersHaveMentalBreak
		{
			get
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.IsOwner(this.pawns[i]) && !this.pawns[i].InMentalState)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17003535 RID: 13621
		// (get) Token: 0x06015541 RID: 87361 RVA: 0x0066040C File Offset: 0x0065E60C
		public bool NightResting
		{
			get
			{
				return base.Spawned && this.needs.AnyPawnsNeedRest && (!this.pather.Moving || !(this.pather.nextTile == this.pather.Destination) || !Caravan_PathFollower.IsValidFinalPushDestination(this.pather.Destination) || Mathf.CeilToInt(this.pather.nextTileCostLeft / 1f) > 10000) && CaravanNightRestUtility.RestingNowAt(base.Tile);
			}
		}

		// Token: 0x17003536 RID: 13622
		// (get) Token: 0x06015542 RID: 87362 RVA: 0x00660498 File Offset: 0x0065E698
		public int LeftRestTicks
		{
			get
			{
				if (!this.NightResting)
				{
					return 0;
				}
				return CaravanNightRestUtility.LeftRestTicksAt(base.Tile);
			}
		}

		// Token: 0x17003537 RID: 13623
		// (get) Token: 0x06015543 RID: 87363 RVA: 0x006604AF File Offset: 0x0065E6AF
		public int LeftNonRestTicks
		{
			get
			{
				if (this.NightResting)
				{
					return 0;
				}
				return CaravanNightRestUtility.LeftNonRestTicksAt(base.Tile);
			}
		}

		// Token: 0x17003538 RID: 13624
		// (get) Token: 0x06015544 RID: 87364 RVA: 0x006604C6 File Offset: 0x0065E6C6
		public override string Label
		{
			get
			{
				if (this.nameInt != null)
				{
					return this.nameInt;
				}
				return base.Label;
			}
		}

		// Token: 0x17003539 RID: 13625
		// (get) Token: 0x06015545 RID: 87365 RVA: 0x006604DD File Offset: 0x0065E6DD
		public override bool HasName
		{
			get
			{
				return !this.nameInt.NullOrEmpty();
			}
		}

		// Token: 0x1700353A RID: 13626
		// (get) Token: 0x06015546 RID: 87366 RVA: 0x006604ED File Offset: 0x0065E6ED
		public int TicksPerMove
		{
			get
			{
				return CaravanTicksPerMoveUtility.GetTicksPerMove(this, null);
			}
		}

		// Token: 0x1700353B RID: 13627
		// (get) Token: 0x06015547 RID: 87367 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AppendFactionToInspectString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700353C RID: 13628
		// (get) Token: 0x06015548 RID: 87368 RVA: 0x006604F6 File Offset: 0x0065E6F6
		public float Visibility
		{
			get
			{
				return CaravanVisibilityCalculator.Visibility(this, null);
			}
		}

		// Token: 0x1700353D RID: 13629
		// (get) Token: 0x06015549 RID: 87369 RVA: 0x00660500 File Offset: 0x0065E700
		public string VisibilityExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CaravanVisibilityCalculator.Visibility(this, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700353E RID: 13630
		// (get) Token: 0x0601554A RID: 87370 RVA: 0x00660524 File Offset: 0x0065E724
		public string TicksPerMoveExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				CaravanTicksPerMoveUtility.GetTicksPerMove(this, stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700353F RID: 13631
		// (get) Token: 0x0601554B RID: 87371 RVA: 0x00660545 File Offset: 0x0065E745
		public IEnumerable<Thing> AllThings
		{
			get
			{
				return CaravanInventoryUtility.AllInventoryItems(this).Concat(this.pawns);
			}
		}

		// Token: 0x17003540 RID: 13632
		// (get) Token: 0x0601554C RID: 87372 RVA: 0x00660558 File Offset: 0x0065E758
		public Building_PassengerShuttle Shuttle
		{
			get
			{
				if (this.hasShuttleDirty)
				{
					this.hasShuttleDirty = false;
					this.cachedShuttle = CaravanInventoryUtility.FindShuttle(this);
				}
				return this.cachedShuttle;
			}
		}

		// Token: 0x17003541 RID: 13633
		// (get) Token: 0x0601554D RID: 87373 RVA: 0x0066057B File Offset: 0x0065E77B
		public int ConstantRandSeed
		{
			get
			{
				return this.uniqueId ^ 728241121;
			}
		}

		// Token: 0x17003542 RID: 13634
		// (get) Token: 0x0601554E RID: 87374 RVA: 0x00660589 File Offset: 0x0065E789
		public StoryState StoryState
		{
			get
			{
				return this.storyState;
			}
		}

		// Token: 0x17003543 RID: 13635
		// (get) Token: 0x0601554F RID: 87375 RVA: 0x00660591 File Offset: 0x0065E791
		public GameConditionManager GameConditionManager
		{
			get
			{
				Log.ErrorOnce("Attempted to retrieve condition manager directly from caravan", 13291050);
				return null;
			}
		}

		// Token: 0x17003544 RID: 13636
		// (get) Token: 0x06015550 RID: 87376 RVA: 0x006605A4 File Offset: 0x0065E7A4
		public float PlayerWealthForStoryteller
		{
			get
			{
				if (!this.IsPlayerControlled)
				{
					return 0f;
				}
				float num = 0f;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					num += WealthWatcher.GetEquipmentApparelAndInventoryWealth(this.pawns[i]);
					if (this.pawns[i].Faction == Faction.OfPlayer)
					{
						float num2 = this.pawns[i].MarketValue;
						if (this.pawns[i].IsSlave)
						{
							num2 *= 0.75f;
						}
						num += num2;
					}
				}
				return num * 0.7f;
			}
		}

		// Token: 0x17003545 RID: 13637
		// (get) Token: 0x06015551 RID: 87377 RVA: 0x0066063F File Offset: 0x0065E83F
		public IEnumerable<Pawn> PlayerPawnsForStoryteller
		{
			get
			{
				if (!this.IsPlayerControlled)
				{
					return Enumerable.Empty<Pawn>();
				}
				return from x in this.PawnsListForReading
				where x.Faction == Faction.OfPlayer
				select x;
			}
		}

		// Token: 0x17003546 RID: 13638
		// (get) Token: 0x06015552 RID: 87378 RVA: 0x00660679 File Offset: 0x0065E879
		public FloatRange IncidentPointsRandomFactorRange
		{
			get
			{
				return StorytellerUtility.CaravanPointsRandomFactorRange;
			}
		}

		// Token: 0x06015553 RID: 87379 RVA: 0x00660680 File Offset: 0x0065E880
		public void SetUniqueId(int newId)
		{
			if (this.uniqueId != -1 || newId < 0)
			{
				Log.Error("Tried to set caravan with uniqueId " + this.uniqueId.ToString() + " to have uniqueId " + newId.ToString());
			}
			this.uniqueId = newId;
		}

		// Token: 0x17003547 RID: 13639
		// (get) Token: 0x06015554 RID: 87380 RVA: 0x006606BC File Offset: 0x0065E8BC
		public TraderKindDef TraderKind
		{
			get
			{
				return this.trader.TraderKind;
			}
		}

		// Token: 0x17003548 RID: 13640
		// (get) Token: 0x06015555 RID: 87381 RVA: 0x006606C9 File Offset: 0x0065E8C9
		public IEnumerable<Thing> Goods
		{
			get
			{
				return this.trader.Goods;
			}
		}

		// Token: 0x17003549 RID: 13641
		// (get) Token: 0x06015556 RID: 87382 RVA: 0x006606D6 File Offset: 0x0065E8D6
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.trader.RandomPriceFactorSeed;
			}
		}

		// Token: 0x1700354A RID: 13642
		// (get) Token: 0x06015557 RID: 87383 RVA: 0x006606E3 File Offset: 0x0065E8E3
		public string TraderName
		{
			get
			{
				return this.trader.TraderName;
			}
		}

		// Token: 0x1700354B RID: 13643
		// (get) Token: 0x06015558 RID: 87384 RVA: 0x006606F0 File Offset: 0x0065E8F0
		public bool CanTradeNow
		{
			get
			{
				return this.trader.CanTradeNow;
			}
		}

		// Token: 0x1700354C RID: 13644
		// (get) Token: 0x06015559 RID: 87385 RVA: 0x00006AAC File Offset: 0x00004CAC
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700354D RID: 13645
		// (get) Token: 0x0601555A RID: 87386 RVA: 0x006606FD File Offset: 0x0065E8FD
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		// Token: 0x0601555B RID: 87387 RVA: 0x0066070A File Offset: 0x0065E90A
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x0601555C RID: 87388 RVA: 0x00660718 File Offset: 0x0065E918
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x0601555D RID: 87389 RVA: 0x00660728 File Offset: 0x0065E928
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x0601555E RID: 87390 RVA: 0x00660738 File Offset: 0x0065E938
		public Caravan()
		{
			this.pawns = new ThingOwner<Pawn>(this, false, LookMode.Reference, true);
			this.pather = new Caravan_PathFollower(this);
			this.gotoMote = new Caravan_GotoMoteRenderer();
			this.tweener = new Caravan_Tweener(this);
			this.trader = new Caravan_TraderTracker(this);
			this.forage = new Caravan_ForageTracker(this);
			this.needs = new Caravan_NeedsTracker(this);
			this.carryTracker = new Caravan_CarryTracker(this);
			this.beds = new Caravan_BedsTracker(this);
			if (ModsConfig.BiotechActive)
			{
				this.babies = new Caravan_BabyTracker(this);
			}
			this.storyState = new StoryState(this);
		}

		// Token: 0x0601555F RID: 87391 RVA: 0x006607FC File Offset: 0x0065E9FC
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
			}
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.nameInt, "name", null, false);
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.pawns, "pawns", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.autoJoinable, "autoJoinable", false, false);
			Scribe_Deep.Look<Caravan_PathFollower>(ref this.pather, "pather", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_ForageTracker>(ref this.forage, "forage", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_NeedsTracker>(ref this.needs, "needs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_CarryTracker>(ref this.carryTracker, "carryTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Caravan_BedsTracker>(ref this.beds, "beds", new object[]
			{
				this
			});
			Scribe_Deep.Look<StoryState>(ref this.storyState, "storyState", new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Caravan_BabyTracker caravan_BabyTracker = this.babies;
				if (caravan_BabyTracker == null)
				{
					return;
				}
				caravan_BabyTracker.Recache();
			}
		}

		// Token: 0x06015560 RID: 87392 RVA: 0x00660966 File Offset: 0x0065EB66
		public override void PostAdd()
		{
			base.PostAdd();
			this.carryTracker.Notify_CaravanSpawned();
			this.beds.Notify_CaravanSpawned();
			Caravan_BabyTracker caravan_BabyTracker = this.babies;
			if (caravan_BabyTracker != null)
			{
				caravan_BabyTracker.Recache();
			}
			Find.ColonistBar.MarkColonistsDirty();
			this.hasShuttleDirty = true;
		}

		// Token: 0x06015561 RID: 87393 RVA: 0x006609A6 File Offset: 0x0065EBA6
		public override void PostRemove()
		{
			base.PostRemove();
			this.pather.StopDead();
			Find.ColonistBar.MarkColonistsDirty();
		}

		// Token: 0x06015562 RID: 87394 RVA: 0x006609C4 File Offset: 0x0065EBC4
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.CheckAnyNonWorldPawns();
			this.pather.PatherTickInterval(delta);
			this.tweener.TweenerTickInterval(delta);
			this.forage.ForageTrackerTickInterval(delta);
			this.carryTracker.CarryTrackerTickInterval(delta);
			this.beds.BedsTrackerTickInterval(delta);
			this.needs.NeedsTrackerTickInterval(delta);
			Caravan_BabyTracker caravan_BabyTracker = this.babies;
			if (caravan_BabyTracker != null)
			{
				caravan_BabyTracker.TickInterval(delta);
			}
			CaravanDrugPolicyUtility.CheckTakeScheduledDrugs(this, delta);
			CaravanTendUtility.CheckTend(this, delta);
			if (ModsConfig.BiotechActive)
			{
				CaravanPollutionUtility.CheckDamageFromPollution(this, delta);
			}
		}

		// Token: 0x06015563 RID: 87395 RVA: 0x00660A54 File Offset: 0x0065EC54
		public override void SpawnSetup()
		{
			base.SpawnSetup();
			this.tweener.ResetTweenedPosToRoot();
		}

		// Token: 0x06015564 RID: 87396 RVA: 0x00660A67 File Offset: 0x0065EC67
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.IsPlayerControlled && this.pather.curPath != null)
			{
				this.pather.curPath.DrawPath(this);
			}
			this.gotoMote.RenderMote();
		}

		// Token: 0x06015565 RID: 87397 RVA: 0x00660AA0 File Offset: 0x0065ECA0
		public void AddPawn(Pawn p, bool addCarriedPawnToWorldPawnsIfAny)
		{
			if (p == null)
			{
				Log.Warning("Tried to add a null pawn to " + ((this != null) ? this.ToString() : null));
				return;
			}
			if (p.Dead)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to add ",
					(p != null) ? p.ToString() : null,
					" to ",
					(this != null) ? this.ToString() : null,
					", but this pawn is dead."
				}));
				return;
			}
			Pawn pawn = p.carryTracker.CarriedThing as Pawn;
			if (pawn != null)
			{
				p.carryTracker.innerContainer.Remove(pawn);
			}
			p.DeSpawnOrDeselect(DestroyMode.Vanish);
			if (this.pawns.TryAdd(p, true))
			{
				if (this.ShouldAutoCapture(p))
				{
					p.guest.CapturedBy(base.Faction, null);
				}
				if (pawn != null)
				{
					if (this.ShouldAutoCapture(pawn))
					{
						pawn.guest.CapturedBy(base.Faction, p);
					}
					this.AddPawn(pawn, addCarriedPawnToWorldPawnsIfAny);
					if (addCarriedPawnToWorldPawnsIfAny)
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						return;
					}
				}
			}
			else
			{
				Log.Error("Couldn't add pawn " + ((p != null) ? p.ToString() : null) + " to caravan.");
			}
		}

		// Token: 0x06015566 RID: 87398 RVA: 0x00660BD0 File Offset: 0x0065EDD0
		public void AddPawnOrItem(Thing thing, bool addCarriedPawnToWorldPawnsIfAny)
		{
			if (thing == null)
			{
				Log.Warning("Tried to add a null thing to " + ((this != null) ? this.ToString() : null));
				return;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				this.AddPawn(pawn, addCarriedPawnToWorldPawnsIfAny);
				return;
			}
			CaravanInventoryUtility.GiveThing(this, thing);
		}

		// Token: 0x06015567 RID: 87399 RVA: 0x00660C17 File Offset: 0x0065EE17
		public bool ContainsPawn(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x06015568 RID: 87400 RVA: 0x00660C25 File Offset: 0x0065EE25
		public void RemovePawn(Pawn p)
		{
			this.pawns.Remove(p);
		}

		// Token: 0x06015569 RID: 87401 RVA: 0x00660C34 File Offset: 0x0065EE34
		public void RemoveAllPawns()
		{
			this.pawns.Clear();
		}

		// Token: 0x0601556A RID: 87402 RVA: 0x00660C41 File Offset: 0x0065EE41
		public bool IsOwner(Pawn p)
		{
			return this.pawns.Contains(p) && CaravanUtility.IsOwner(p, base.Faction);
		}

		// Token: 0x0601556B RID: 87403 RVA: 0x00660C60 File Offset: 0x0065EE60
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.pawns[i].IsColonist)
				{
					num++;
				}
				else if (this.pawns[i].RaceProps.Animal)
				{
					num2++;
				}
				else if (this.pawns[i].IsPrisoner)
				{
					num3++;
				}
				else if (this.pawns[i].RaceProps.IsMechanoid)
				{
					num6++;
				}
				if (this.pawns[i].Downed && !this.pawns[i].ageTracker.CurLifeStage.alwaysDowned)
				{
					num4++;
				}
				if (this.pawns[i].InMentalState)
				{
					num5++;
				}
			}
			stringBuilder.Append("CaravanColonistsCount".Translate(num, (num == 1) ? Faction.OfPlayer.def.pawnSingular : Faction.OfPlayer.def.pawnsPlural));
			if (num2 == 1)
			{
				stringBuilder.Append(", " + "CaravanAnimal".Translate());
			}
			else if (num2 > 1)
			{
				stringBuilder.Append(", " + "CaravanAnimalsCount".Translate(num2));
			}
			if (num3 == 1)
			{
				stringBuilder.Append(", " + "CaravanPrisoner".Translate());
			}
			else if (num3 > 1)
			{
				stringBuilder.Append(", " + "CaravanPrisonersCount".Translate(num3));
			}
			if (ModsConfig.BiotechActive)
			{
				if (num6 == 1)
				{
					stringBuilder.Append(", " + "CaravanMech".Translate());
				}
				else if (num6 > 1)
				{
					stringBuilder.Append(", " + "CaravanMechsCount".Translate(num6));
				}
			}
			stringBuilder.AppendLine();
			if (num5 > 0)
			{
				stringBuilder.Append("CaravanPawnsInMentalState".Translate(num5));
			}
			if (num4 > 0)
			{
				if (num5 > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append("CaravanPawnsDowned".Translate(num4));
			}
			if (num5 > 0 || num4 > 0)
			{
				stringBuilder.AppendLine();
			}
			if (this.pather.Moving)
			{
				if (this.pather.ArrivalAction != null)
				{
					stringBuilder.Append(this.pather.ArrivalAction.ReportString);
				}
				else
				{
					stringBuilder.Append("CaravanTraveling".Translate());
				}
			}
			else
			{
				Settlement settlement = CaravanVisitUtility.SettlementVisitedNow(this);
				if (settlement != null)
				{
					stringBuilder.Append("CaravanVisiting".Translate(settlement.Label));
				}
				else
				{
					stringBuilder.Append("CaravanWaiting".Translate());
				}
			}
			if (this.pather.Moving)
			{
				float num7 = (float)CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this, true) / 60000f;
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanEstimatedTimeToDestination".Translate(num7.ToString("0.#")));
			}
			if (this.AllOwnersDowned)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AllCaravanMembersDowned".Translate());
			}
			else if (this.AllOwnersHaveMentalBreak)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AllCaravanMembersMentalBreak".Translate());
			}
			else if (this.ImmobilizedByMass)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanImmobilizedByMass".Translate());
			}
			string text;
			if (this.needs.AnyPawnOutOfFood(out text))
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("CaravanOutOfFood".Translate());
				if (!text.NullOrEmpty())
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(text);
					stringBuilder.Append(".");
				}
			}
			if (!this.pather.MovingNow)
			{
				int usedBedCount = this.beds.GetUsedBedCount();
				stringBuilder.AppendLine();
				stringBuilder.Append(CaravanBedUtility.AppendUsingBedsLabel("CaravanResting".Translate(), usedBedCount));
			}
			else
			{
				string inspectStringLine = this.carryTracker.GetInspectStringLine();
				if (!inspectStringLine.NullOrEmpty())
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(inspectStringLine);
				}
				string inBedForMedicalReasonsInspectStringLine = this.beds.GetInBedForMedicalReasonsInspectStringLine();
				if (!inBedForMedicalReasonsInspectStringLine.NullOrEmpty())
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(inBedForMedicalReasonsInspectStringLine);
				}
			}
			if (ModsConfig.OdysseyActive && this.Shuttle != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append(string.Format("{0}: {1:F0} / {2:F0}", "CaravanShuttleFuel".Translate(), this.Shuttle.FuelLevel, this.Shuttle.MaxFuelLevel));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0601556C RID: 87404 RVA: 0x006611BB File Offset: 0x0065F3BB
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (Find.WorldSelector.SingleSelectedObject == this)
			{
				yield return new Gizmo_CaravanInfo(this);
			}
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.IsPlayerControlled)
			{
				if (Find.WorldSelector.SingleSelectedObject == this)
				{
					yield return SettleInEmptyTileUtility.SettleCommand(this);
					yield return SettleInEmptyTileUtility.SetupCamp(this);
					foreach (Pawn p in this.pawns)
					{
						if (p.royalty != null)
						{
							foreach (FactionPermit factionPermit in p.royalty.AllFactionPermits)
							{
								IEnumerable<Gizmo> caravanGizmos = factionPermit.Permit.Worker.GetCaravanGizmos(p, factionPermit.Faction);
								if (caravanGizmos != null)
								{
									foreach (Gizmo gizmo2 in caravanGizmos)
									{
										yield return gizmo2;
									}
									enumerator = null;
								}
							}
							List<FactionPermit>.Enumerator enumerator3 = default(List<FactionPermit>.Enumerator);
						}
						p = null;
					}
					List<Pawn>.Enumerator enumerator2 = default(List<Pawn>.Enumerator);
					if (this.PawnsListForReading.Count((Pawn x) => x.IsColonist) >= 2)
					{
						yield return new Command_Action
						{
							defaultLabel = "CommandSplitCaravan".Translate(),
							defaultDesc = "CommandSplitCaravanDesc".Translate(),
							icon = Caravan.SplitCommand,
							hotKey = KeyBindingDefOf.Misc5,
							action = delegate()
							{
								Find.WindowStack.Add(new Dialog_SplitCaravan(this));
							}
						};
					}
				}
				if (this.pather.Moving)
				{
					yield return new Command_Toggle
					{
						hotKey = KeyBindingDefOf.Misc1,
						isActive = (() => this.pather.Paused),
						toggleAction = delegate()
						{
							if (!this.pather.Moving)
							{
								return;
							}
							this.pather.Paused = !this.pather.Paused;
						},
						defaultDesc = "CommandToggleCaravanPauseDesc".Translate(2f.ToString("0.#"), 0.3f.ToStringPercent()),
						icon = TexCommand.PauseCaravan,
						defaultLabel = "CommandPauseCaravan".Translate()
					};
				}
				if (CaravanMergeUtility.ShouldShowMergeCommand)
				{
					yield return CaravanMergeUtility.MergeCommand(this);
				}
				foreach (Gizmo gizmo3 in this.forage.GetGizmos())
				{
					yield return gizmo3;
				}
				enumerator = null;
				foreach (WorldObject worldObject in Find.WorldObjects.ObjectsAt(base.Tile))
				{
					foreach (Gizmo gizmo4 in worldObject.GetCaravanGizmos(this))
					{
						yield return gizmo4;
					}
					enumerator = null;
				}
				IEnumerator<WorldObject> enumerator4 = null;
				if (Find.WorldSelector.SingleSelectedObject == this)
				{
					foreach (Pawn p in this.pawns)
					{
						if (p.abilities != null && !p.Downed && !p.IsPrisoner)
						{
							foreach (Ability ability in p.abilities.abilities)
							{
								if (ability.def.showGizmoOnWorldView)
								{
									foreach (Command command in ability.GetGizmos())
									{
										yield return command;
									}
									IEnumerator<Command> enumerator6 = null;
								}
							}
							List<Ability>.Enumerator enumerator5 = default(List<Ability>.Enumerator);
						}
						if (p.genes != null)
						{
							foreach (Gene gene in p.genes.GenesListForReading)
							{
								if (gene.def.showGizmoOnWorldView)
								{
									foreach (Gizmo gizmo5 in gene.GetGizmos())
									{
										yield return gizmo5;
									}
									enumerator = null;
								}
							}
							List<Gene>.Enumerator enumerator7 = default(List<Gene>.Enumerator);
						}
						p = null;
					}
					List<Pawn>.Enumerator enumerator2 = default(List<Pawn>.Enumerator);
				}
				if (ModsConfig.OdysseyActive && this.Shuttle != null && Find.WorldSelector.NumSelectedObjects == 1)
				{
					yield return this.GetLaunchGizmo();
					yield return this.GetRefuelGizmo();
					if (DebugSettings.ShowDevGizmos)
					{
						yield return new Command_Action
						{
							defaultLabel = "DEV: End cooldown",
							action = delegate()
							{
								this.Shuttle.LaunchableComp.lastLaunchTick = Find.TickManager.TicksGame - this.Shuttle.LaunchableComp.Props.cooldownTicks;
							}
						};
					}
				}
			}
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Mental break",
					action = delegate()
					{
						Pawn pawn;
						if ((from x in this.PawnsListForReading
						where x.RaceProps.Humanlike && !x.InMentalState
						select x).TryRandomElement(out pawn))
						{
							pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad, null, false, false, false, null, false, false, false);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Make random pawn hungry",
					action = delegate()
					{
						Pawn pawn;
						if ((from x in this.PawnsListForReading
						where x.needs.food != null
						select x).TryRandomElement(out pawn))
						{
							pawn.needs.food.CurLevelPercentage = 0f;
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Kill random pawn",
					action = delegate()
					{
						Pawn pawn;
						if (this.PawnsListForReading.TryRandomElement(out pawn))
						{
							pawn.Kill(null, null);
							Messages.Message("DEV: Killed " + pawn.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
						}
					}
				};
				if (ModsConfig.IdeologyActive)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Kill all non-slave pawns",
						action = delegate()
						{
							for (int i = this.PawnsListForReading.Count - 1; i >= 0; i--)
							{
								Pawn pawn = this.PawnsListForReading[i];
								if (!pawn.IsSlave)
								{
									pawn.Kill(null, null);
									Messages.Message("DEV: Killed " + pawn.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
								}
							}
						}
					};
				}
				yield return new Command_Action
				{
					defaultLabel = "DEV: Harm random pawn",
					action = delegate()
					{
						Pawn pawn;
						if (this.PawnsListForReading.TryRandomElement(out pawn))
						{
							DamageInfo dinfo = new DamageInfo(DamageDefOf.Scratch, 10f, 999f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
							pawn.TakeDamage(dinfo);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Down random pawn",
					action = delegate()
					{
						Pawn pawn;
						if ((from x in this.PawnsListForReading
						where !x.Downed
						select x).TryRandomElement(out pawn))
						{
							HealthUtility.DamageUntilDowned(pawn, true, null, null, null);
							Messages.Message("DEV: Downed " + pawn.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Plague on random pawn",
					action = delegate()
					{
						Pawn pawn;
						if ((from x in this.PawnsListForReading
						where !x.Downed && x.RaceProps.IsFlesh
						select x).TryRandomElement(out pawn))
						{
							Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Plague, pawn, null);
							hediff.Severity = HediffDefOf.Plague.stages[1].minSeverity - 0.001f;
							pawn.health.AddHediff(hediff, null, null, null);
							Messages.Message("DEV: Gave advanced plague to " + pawn.LabelShort, this, MessageTypeDefOf.TaskCompletion, false);
						}
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Teleport to destination",
					action = delegate()
					{
						base.Tile = this.pather.Destination;
						this.pather.StopDead();
					}
				};
				if (ModsConfig.RoyaltyActive)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: +20% psyfocus",
						action = delegate()
						{
							for (int i = 0; i < this.PawnsListForReading.Count; i++)
							{
								Pawn_PsychicEntropyTracker psychicEntropy = this.PawnsListForReading[i].psychicEntropy;
								if (psychicEntropy != null)
								{
									psychicEntropy.OffsetPsyfocusDirectly(0.2f);
								}
							}
						}
					};
				}
				if (ModsConfig.BiotechActive)
				{
					IEnumerable<Thing> allDissolvingThings = CaravanInventoryUtility.GetAllDissolvingThings(this);
					if (allDissolvingThings.Any<Thing>())
					{
						yield return new Command_Action
						{
							defaultLabel = "DEV: Trigger random dissolution event",
							action = delegate()
							{
								allDissolvingThings.RandomElement<Thing>().TryGetComp<CompDissolution>().TriggerDissolutionEvent(1);
							}
						};
					}
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0601556D RID: 87405 RVA: 0x006611CC File Offset: 0x0065F3CC
		private Gizmo GetLaunchGizmo()
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandLaunchGroup".Translate();
			command_Action.defaultDesc = "CommandLaunchGroupDesc".Translate();
			command_Action.icon = CompLaunchable.LaunchCommandTex;
			command_Action.action = delegate()
			{
				this.Shuttle.LaunchableComp.StartChoosingDestination(delegate(PlanetTile tile, TransportersArrivalAction action)
				{
					CaravanShuttleUtility.LaunchShuttle(this, tile, action);
				}, null);
			};
			AcceptanceReport acceptanceReport = CaravanShuttleUtility.CanLaunchCaravanShuttle(this);
			if (!acceptanceReport.Accepted)
			{
				command_Action.Disable(acceptanceReport.Reason);
			}
			return command_Action;
		}

		// Token: 0x0601556E RID: 87406 RVA: 0x00661244 File Offset: 0x0065F444
		private Gizmo GetRefuelGizmo()
		{
			float fuelInCaravan = CaravanShuttleUtility.FuelInCaravan(this);
			string text = null;
			if (fuelInCaravan <= 0f)
			{
				text = "NoFuelInCaravan".Translate();
			}
			if (Mathf.Approximately(this.Shuttle.FuelLevel, this.Shuttle.MaxFuelLevel))
			{
				text = "ShuttleFullyFueled".Translate();
			}
			Action<int> <>9__2;
			return new Command_Action
			{
				defaultLabel = "CommandRefuelShuttleFromCargo".Translate(),
				defaultDesc = "CommandRefuelShuttleFromCargoDesc".Translate(),
				icon = Building_PassengerShuttle.RefuelFromCargoIcon.Texture,
				action = delegate()
				{
					int num = Mathf.FloorToInt(Mathf.Min(fuelInCaravan, this.Shuttle.MaxFuelLevel - this.Shuttle.FuelLevel));
					Func<int, string> textGetter = (int val) => "RefuelShuttleCount".Translate(val);
					int from = 1;
					int to = num;
					Action<int> confirmAction;
					if ((confirmAction = <>9__2) == null)
					{
						confirmAction = (<>9__2 = delegate(int count)
						{
							CaravanShuttleUtility.ConsumeFuelFromCaravanInventory(this, count);
							this.Shuttle.RefuelableComp.Refuel((float)count);
						});
					}
					Dialog_Slider window = new Dialog_Slider(textGetter, from, to, confirmAction, int.MinValue, 1f);
					Find.WindowStack.Add(window);
				},
				Disabled = !text.NullOrEmpty(),
				disabledReason = text
			};
		}

		// Token: 0x0601556F RID: 87407 RVA: 0x0066131D File Offset: 0x0065F51D
		public override IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetTransportersFloatMenuOptions(pods, launchAction))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (FloatMenuOption floatMenuOption2 in TransportersArrivalAction_GiveToCaravan.GetFloatMenuOptions(launchAction, pods, this))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06015570 RID: 87408 RVA: 0x0066133B File Offset: 0x0065F53B
		public void RecacheInventory()
		{
			this.cachedImmobilizedForTicks = -99999;
			this.cachedDaysWorthOfFoodForTicks = -99999;
			this.hasShuttleDirty = true;
		}

		// Token: 0x06015571 RID: 87409 RVA: 0x0066135C File Offset: 0x0065F55C
		public virtual void Notify_MemberDied(Pawn member)
		{
			if (!base.Spawned)
			{
				Log.Error("Caravan member died in an unspawned caravan. Unspawned caravans shouldn't be kept for more than a single frame.");
			}
			if (!this.PawnsListForReading.Any((Pawn x) => x != member && this.IsOwner(x)))
			{
				List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this);
				for (int i = 0; i < list.Count; i++)
				{
					list[i].Notify_AbandonedAtTile(base.Tile);
				}
				this.RemovePawn(member);
				if (base.Faction == Faction.OfPlayer)
				{
					if (ModsConfig.IdeologyActive && this.PawnsListForReading.Any((Pawn x) => x != member && x.IsSlave))
					{
						Find.LetterStack.ReceiveLetter("LetterLabelAllCaravanColonistsDied".Translate(), "LetterOnlySlaveCaravanColonistsLeft".Translate(this.Name).CapitalizeFirst(), LetterDefOf.NegativeEvent, new GlobalTargetInfo(base.Tile), null, null, null, null, 0, true);
					}
					else
					{
						Find.LetterStack.ReceiveLetter("LetterLabelAllCaravanColonistsDied".Translate(), "LetterAllCaravanColonistsDied".Translate(this.Name).CapitalizeFirst(), LetterDefOf.NegativeEvent, new GlobalTargetInfo(base.Tile), null, null, null, null, 0, true);
					}
				}
				this.pawns.Clear();
				this.Destroy();
				return;
			}
			member.Strip(true);
			this.RemovePawn(member);
		}

		// Token: 0x06015572 RID: 87410 RVA: 0x006614D5 File Offset: 0x0065F6D5
		public virtual void Notify_Merged(List<Caravan> group)
		{
			this.notifiedOutOfFood = false;
		}

		// Token: 0x06015573 RID: 87411 RVA: 0x006614D5 File Offset: 0x0065F6D5
		public virtual void Notify_StartedTrading()
		{
			this.notifiedOutOfFood = false;
		}

		// Token: 0x06015574 RID: 87412 RVA: 0x006614E0 File Offset: 0x0065F6E0
		private void CheckAnyNonWorldPawns()
		{
			for (int i = this.pawns.Count - 1; i >= 0; i--)
			{
				if (!this.pawns[i].IsWorldPawn())
				{
					Log.Error(string.Format("Caravan member {0} is not a world pawn. Removing...", this.pawns[i]));
					this.pawns.Remove(this.pawns[i]);
				}
			}
		}

		// Token: 0x06015575 RID: 87413 RVA: 0x0066154B File Offset: 0x0065F74B
		private bool ShouldAutoCapture(Pawn p)
		{
			return CaravanUtility.ShouldAutoCapture(p, base.Faction);
		}

		// Token: 0x06015576 RID: 87414 RVA: 0x0066155C File Offset: 0x0065F75C
		public void Notify_PawnRemoved(Pawn p)
		{
			Find.ColonistBar.MarkColonistsDirty();
			this.RecacheInventory();
			this.carryTracker.Notify_PawnRemoved();
			this.beds.Notify_PawnRemoved();
			Caravan_BabyTracker caravan_BabyTracker = this.babies;
			if (caravan_BabyTracker != null)
			{
				caravan_BabyTracker.Recache();
			}
			Pawn_MindState mindState = p.mindState;
			if (mindState == null)
			{
				return;
			}
			mindState.ClearBreastfeedCaravan();
		}

		// Token: 0x06015577 RID: 87415 RVA: 0x006615B0 File Offset: 0x0065F7B0
		public void Notify_PawnAdded(Pawn p)
		{
			Find.ColonistBar.MarkColonistsDirty();
			this.RecacheInventory();
			Caravan_BabyTracker caravan_BabyTracker = this.babies;
			if (caravan_BabyTracker == null)
			{
				return;
			}
			caravan_BabyTracker.Recache();
		}

		// Token: 0x06015578 RID: 87416 RVA: 0x006615D2 File Offset: 0x0065F7D2
		public void Notify_DestinationOrPauseStatusChanged()
		{
			this.cachedDaysWorthOfFoodForTicks = -99999;
		}

		// Token: 0x06015579 RID: 87417 RVA: 0x006615DF File Offset: 0x0065F7DF
		public void Notify_Teleported()
		{
			this.tweener.ResetTweenedPosToRoot();
			this.pather.Notify_Teleported_Int();
		}

		// Token: 0x0601557A RID: 87418 RVA: 0x006615F7 File Offset: 0x0065F7F7
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.pawns;
		}

		// Token: 0x0601557B RID: 87419 RVA: 0x006615FF File Offset: 0x0065F7FF
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0400ED9B RID: 60827
		private int uniqueId = -1;

		// Token: 0x0400ED9C RID: 60828
		private string nameInt;

		// Token: 0x0400ED9D RID: 60829
		public ThingOwner<Pawn> pawns;

		// Token: 0x0400ED9E RID: 60830
		public bool autoJoinable;

		// Token: 0x0400ED9F RID: 60831
		public Caravan_PathFollower pather;

		// Token: 0x0400EDA0 RID: 60832
		public Caravan_GotoMoteRenderer gotoMote;

		// Token: 0x0400EDA1 RID: 60833
		public Caravan_Tweener tweener;

		// Token: 0x0400EDA2 RID: 60834
		public Caravan_TraderTracker trader;

		// Token: 0x0400EDA3 RID: 60835
		public Caravan_ForageTracker forage;

		// Token: 0x0400EDA4 RID: 60836
		public Caravan_NeedsTracker needs;

		// Token: 0x0400EDA5 RID: 60837
		public Caravan_CarryTracker carryTracker;

		// Token: 0x0400EDA6 RID: 60838
		public Caravan_BedsTracker beds;

		// Token: 0x0400EDA7 RID: 60839
		public Caravan_BabyTracker babies;

		// Token: 0x0400EDA8 RID: 60840
		public StoryState storyState;

		// Token: 0x0400EDA9 RID: 60841
		private Material cachedMat;

		// Token: 0x0400EDAA RID: 60842
		private bool cachedImmobilized;

		// Token: 0x0400EDAB RID: 60843
		private int cachedImmobilizedForTicks = -99999;

		// Token: 0x0400EDAC RID: 60844
		[TupleElementNames(new string[]
		{
			"days",
			"tillRot"
		})]
		private ValueTuple<float, float> cachedDaysWorthOfFood;

		// Token: 0x0400EDAD RID: 60845
		private int cachedDaysWorthOfFoodForTicks = -99999;

		// Token: 0x0400EDAE RID: 60846
		public bool notifiedOutOfFood;

		// Token: 0x0400EDAF RID: 60847
		private const int ImmobilizedCacheDuration = 60;

		// Token: 0x0400EDB0 RID: 60848
		private const int DaysWorthOfFoodCacheDuration = 3000;

		// Token: 0x0400EDB1 RID: 60849
		private static readonly Texture2D SplitCommand = ContentFinder<Texture2D>.Get("UI/Commands/SplitCaravan", true);

		// Token: 0x0400EDB2 RID: 60850
		private static readonly Color PlayerCaravanColor = new Color(1f, 0.863f, 0.33f);

		// Token: 0x0400EDB3 RID: 60851
		public bool hasShuttleDirty = true;

		// Token: 0x0400EDB4 RID: 60852
		private Building_PassengerShuttle cachedShuttle;
	}
}
