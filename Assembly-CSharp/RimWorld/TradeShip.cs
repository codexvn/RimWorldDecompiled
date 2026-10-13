using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FB4 RID: 12212
	public class TradeShip : PassingShip, ITrader, IThingHolder
	{
		// Token: 0x17002C8D RID: 11405
		// (get) Token: 0x06011331 RID: 70449 RVA: 0x004EDFF5 File Offset: 0x004EC1F5
		public override string FullTitle
		{
			get
			{
				return this.name + " (" + this.def.label + ")";
			}
		}

		// Token: 0x17002C8E RID: 11406
		// (get) Token: 0x06011332 RID: 70450 RVA: 0x004EE017 File Offset: 0x004EC217
		public int Silver
		{
			get
			{
				return this.CountHeldOf(ThingDefOf.Silver, null);
			}
		}

		// Token: 0x17002C8F RID: 11407
		// (get) Token: 0x06011333 RID: 70451 RVA: 0x004EE025 File Offset: 0x004EC225
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.def.tradeCurrency;
			}
		}

		// Token: 0x17002C90 RID: 11408
		// (get) Token: 0x06011334 RID: 70452 RVA: 0x004EE032 File Offset: 0x004EC232
		public IThingHolder ParentHolder
		{
			get
			{
				return base.Map;
			}
		}

		// Token: 0x17002C91 RID: 11409
		// (get) Token: 0x06011335 RID: 70453 RVA: 0x004EE03A File Offset: 0x004EC23A
		public TraderKindDef TraderKind
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x17002C92 RID: 11410
		// (get) Token: 0x06011336 RID: 70454 RVA: 0x004EE042 File Offset: 0x004EC242
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.randomPriceFactorSeed;
			}
		}

		// Token: 0x17002C93 RID: 11411
		// (get) Token: 0x06011337 RID: 70455 RVA: 0x004E92EB File Offset: 0x004E74EB
		public string TraderName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17002C94 RID: 11412
		// (get) Token: 0x06011338 RID: 70456 RVA: 0x004EE04A File Offset: 0x004EC24A
		public bool CanTradeNow
		{
			get
			{
				return !base.Departed;
			}
		}

		// Token: 0x17002C95 RID: 11413
		// (get) Token: 0x06011339 RID: 70457 RVA: 0x00006AAC File Offset: 0x00004CAC
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17002C96 RID: 11414
		// (get) Token: 0x0601133A RID: 70458 RVA: 0x004EE055 File Offset: 0x004EC255
		// (set) Token: 0x0601133B RID: 70459 RVA: 0x004EE05D File Offset: 0x004EC25D
		public bool WasAnnounced
		{
			get
			{
				return this.wasAnnounced;
			}
			set
			{
				this.wasAnnounced = value;
			}
		}

		// Token: 0x17002C97 RID: 11415
		// (get) Token: 0x0601133C RID: 70460 RVA: 0x004EE066 File Offset: 0x004EC266
		public IEnumerable<Thing> Goods
		{
			get
			{
				int num;
				for (int i = 0; i < this.things.Count; i = num + 1)
				{
					Pawn pawn = this.things[i] as Pawn;
					if (pawn == null || !this.soldPrisoners.Contains(pawn))
					{
						yield return this.things[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x0601133D RID: 70461 RVA: 0x004EE076 File Offset: 0x004EC276
		public TradeShip()
		{
		}

		// Token: 0x0601133E RID: 70462 RVA: 0x004EE090 File Offset: 0x004EC290
		public TradeShip(TraderKindDef def, Faction faction = null) : base(faction)
		{
			this.def = def;
			this.things = new ThingOwner<Thing>(this);
			TradeShip.tmpExtantNames.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				TradeShip.tmpExtantNames.AddRange(from x in maps[i].passingShipManager.passingShips
				select x.name);
			}
			this.name = NameGenerator.GenerateName(RulePackDefOf.NamerTraderGeneral, TradeShip.tmpExtantNames, false, null);
			if (faction != null)
			{
				this.name = string.Format("{0} {1} {2}", this.name, "OfLower".Translate(), faction.Name);
			}
			this.randomPriceFactorSeed = Rand.RangeInclusive(1, 10000000);
			this.loadID = Find.UniqueIDsManager.GetNextPassingShipID();
		}

		// Token: 0x0601133F RID: 70463 RVA: 0x004EE18F File Offset: 0x004EC38F
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			foreach (Thing thing in TradeUtility.AllLaunchableThingsForTrade(base.Map, this))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			foreach (Pawn pawn in TradeUtility.AllSellableColonyPawns(base.Map, false))
			{
				yield return pawn;
			}
			IEnumerator<Pawn> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x06011340 RID: 70464 RVA: 0x004EE1A0 File Offset: 0x004EC3A0
		public void GenerateThings()
		{
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.traderDef = this.def;
			parms.tile = new PlanetTile?(base.Map.Tile);
			this.things.TryAddRangeOrTransfer(ThingSetMakerDefOf.TraderStock.root.Generate(parms), true, false);
		}

		// Token: 0x06011341 RID: 70465 RVA: 0x004EE1F8 File Offset: 0x004EC3F8
		public override void PassingShipTick()
		{
			base.PassingShipTick();
			for (int i = this.things.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.things[i] as Pawn;
				if (pawn != null)
				{
					pawn.DoTick();
					if (pawn.Dead)
					{
						this.things.Remove(pawn);
					}
				}
			}
		}

		// Token: 0x06011342 RID: 70466 RVA: 0x004EE254 File Offset: 0x004EC454
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<TraderKindDef>(ref this.def, "def");
			Scribe_Deep.Look<ThingOwner>(ref this.things, "things", new object[]
			{
				this
			});
			Scribe_Collections.Look<Pawn>(ref this.soldPrisoners, "soldPrisoners", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.randomPriceFactorSeed, "randomPriceFactorSeed", 0, false);
			Scribe_Values.Look<bool>(ref this.wasAnnounced, "wasAnnounced", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.soldPrisoners.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06011343 RID: 70467 RVA: 0x004EE300 File Offset: 0x004EC500
		public override void TryOpenComms(Pawn negotiator)
		{
			if (!this.CanTradeNow)
			{
				return;
			}
			Find.WindowStack.Add(new Dialog_Trade(negotiator, this, false));
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.BuildOrbitalTradeBeacon, OpportunityType.Critical);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(this.Goods.OfType<Pawn>(), "LetterRelatedPawnsTradeShip".Translate(Faction.OfPlayer.def.pawnsPlural), LetterDefOf.NeutralEvent, false, true);
			TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.TradeGoodsMustBeNearBeacon, Array.Empty<string>());
		}

		// Token: 0x06011344 RID: 70468 RVA: 0x004EE37C File Offset: 0x004EC57C
		public override void Depart()
		{
			base.Depart();
			this.things.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
			this.soldPrisoners.Clear();
		}

		// Token: 0x06011345 RID: 70469 RVA: 0x004EDFF5 File Offset: 0x004EC1F5
		public override string GetCallLabel()
		{
			return this.name + " (" + this.def.label + ")";
		}

		// Token: 0x06011346 RID: 70470 RVA: 0x004EE39C File Offset: 0x004EC59C
		protected override AcceptanceReport CanCommunicateWith(Pawn negotiator)
		{
			AcceptanceReport result = base.CanCommunicateWith(negotiator);
			if (!result.Accepted)
			{
				return result;
			}
			return negotiator.CanTradeWith(base.Faction, this.TraderKind);
		}

		// Token: 0x06011347 RID: 70471 RVA: 0x004EE3D0 File Offset: 0x004EC5D0
		public int CountHeldOf(ThingDef thingDef, ThingDef stuffDef = null)
		{
			Thing thing = this.HeldThingMatching(thingDef, stuffDef);
			if (thing != null)
			{
				return thing.stackCount;
			}
			return 0;
		}

		// Token: 0x06011348 RID: 70472 RVA: 0x004EE3F4 File Offset: 0x004EC5F4
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerSells, playerNegotiator, this);
			Thing thing2 = TradeUtility.ThingFromStockToMergeWith(this, thing);
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
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.RaceProps.Humanlike)
				{
					this.soldPrisoners.Add(pawn);
				}
				this.things.TryAdd(thing, false);
			}
		}

		// Token: 0x06011349 RID: 70473 RVA: 0x004EE460 File Offset: 0x004EC660
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			Thing thing = toGive.SplitOff(countToGive);
			thing.PreTraded(TradeAction.PlayerBuys, playerNegotiator, this);
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				this.soldPrisoners.Remove(pawn);
			}
			TradeUtility.SpawnDropPod(DropCellFinder.TradeDropSpot(base.Map), base.Map, thing);
		}

		// Token: 0x0601134A RID: 70474 RVA: 0x004EE4AC File Offset: 0x004EC6AC
		private Thing HeldThingMatching(ThingDef thingDef, ThingDef stuffDef)
		{
			for (int i = 0; i < this.things.Count; i++)
			{
				if (this.things[i].def == thingDef && this.things[i].Stuff == stuffDef)
				{
					return this.things[i];
				}
			}
			return null;
		}

		// Token: 0x0601134B RID: 70475 RVA: 0x004EE505 File Offset: 0x004EC705
		public void ChangeCountHeldOf(ThingDef thingDef, ThingDef stuffDef, int count)
		{
			Thing thing = this.HeldThingMatching(thingDef, stuffDef);
			if (thing == null)
			{
				Log.Error("Changing count of thing trader doesn't have: " + ((thingDef != null) ? thingDef.ToString() : null));
			}
			thing.stackCount += count;
		}

		// Token: 0x0601134C RID: 70476 RVA: 0x004E92F3 File Offset: 0x004E74F3
		public override string ToString()
		{
			return this.FullTitle;
		}

		// Token: 0x0601134D RID: 70477 RVA: 0x004EE53B File Offset: 0x004EC73B
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.things;
		}

		// Token: 0x0601134E RID: 70478 RVA: 0x004EE543 File Offset: 0x004EC743
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0400B283 RID: 45699
		public TraderKindDef def;

		// Token: 0x0400B284 RID: 45700
		private ThingOwner things;

		// Token: 0x0400B285 RID: 45701
		private List<Pawn> soldPrisoners = new List<Pawn>();

		// Token: 0x0400B286 RID: 45702
		private int randomPriceFactorSeed = -1;

		// Token: 0x0400B287 RID: 45703
		private bool wasAnnounced;

		// Token: 0x0400B288 RID: 45704
		private static List<string> tmpExtantNames = new List<string>();
	}
}
