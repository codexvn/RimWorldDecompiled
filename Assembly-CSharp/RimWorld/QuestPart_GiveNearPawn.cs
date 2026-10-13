using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CE6 RID: 7398
	public class QuestPart_GiveNearPawn : QuestPart
	{
		// Token: 0x17001974 RID: 6516
		// (get) Token: 0x0600A50F RID: 42255 RVA: 0x002F44C3 File Offset: 0x002F26C3
		// (set) Token: 0x0600A510 RID: 42256 RVA: 0x002F44DC File Offset: 0x002F26DC
		public IEnumerable<Thing> Things
		{
			get
			{
				return this.items.Concat(this.pawns.Cast<Thing>());
			}
			set
			{
				this.items.Clear();
				this.pawns.Clear();
				if (value != null)
				{
					foreach (Thing thing in value)
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							this.pawns.Add(pawn);
						}
						else
						{
							this.items.Add(thing);
						}
					}
				}
			}
		}

		// Token: 0x17001975 RID: 6517
		// (get) Token: 0x0600A511 RID: 42257 RVA: 0x002F455C File Offset: 0x002F275C
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in base.Hyperlinks)
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				foreach (Thing outerThing in this.items)
				{
					ThingDef def = outerThing.GetInnerIfMinified().def;
					yield return new Dialog_InfoCard.Hyperlink(def, -1);
				}
				List<Thing>.Enumerator enumerator2 = default(List<Thing>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17001976 RID: 6518
		// (get) Token: 0x0600A512 RID: 42258 RVA: 0x002F456C File Offset: 0x002F276C
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				foreach (Pawn t in PawnsArriveQuestPartUtility.GetQuestLookTargets(this.pawns))
				{
					yield return t;
				}
				IEnumerator<Pawn> enumerator2 = null;
				if (this.importantLookTarget != null)
				{
					yield return this.importantLookTarget;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17001977 RID: 6519
		// (get) Token: 0x0600A513 RID: 42259 RVA: 0x002F457C File Offset: 0x002F277C
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, this.makePrisoners);
			}
		}

		// Token: 0x0600A514 RID: 42260 RVA: 0x002F4598 File Offset: 0x002F2798
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.nearPawn != null && (this.nearPawn.SpawnedOrAnyParentSpawned || this.nearPawn.IsCaravanMember()))
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				this.items.RemoveAll((Thing x) => x.Destroyed);
				this.tmpThingsToGive.Clear();
				this.tmpThingsToGive.AddRange(this.Things);
				for (int i = 0; i < this.thingDefs.Count; i++)
				{
					Thing thing = ThingMaker.MakeThing(this.thingDefs[i].thingDef, GenStuff.RandomStuffByCommonalityFor(this.thingDefs[i].thingDef, TechLevel.Undefined));
					thing.stackCount = this.thingDefs[i].count;
					this.tmpThingsToGive.Add(thing);
				}
				this.tmpThingsToGive.RemoveAll((Thing x) => x.Spawned);
				Thing thing2 = (from x in this.tmpThingsToGive
				where x is Pawn
				select x).MaxByWithFallback((Thing x) => x.MarketValue, null);
				Thing thing3 = this.tmpThingsToGive.MaxByWithFallback((Thing x) => x.MarketValue * (float)x.stackCount, null);
				if (this.tmpThingsToGive.Any<Thing>())
				{
					TaggedString taggedString = null;
					TaggedString taggedString2 = null;
					if (this.sendStandardLetter)
					{
						if (this.nearPawn.SpawnedOrAnyParentSpawned)
						{
							if (this.joinPlayer && this.pawns.Count == 1 && this.pawns[0].RaceProps.Humanlike)
							{
								taggedString = "LetterRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
								taggedString2 = "LetterLabelRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
								PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref taggedString2, this.pawns[0]);
							}
							else
							{
								taggedString = "LetterQuestDropPodsArrived".Translate(GenLabel.ThingsLabel(this.tmpThingsToGive, "  - "));
								taggedString2 = "LetterLabelQuestDropPodsArrived".Translate();
								PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(this.pawns, ref taggedString2, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
							}
							taggedString2 = (this.customDropPodsLetterLabel.NullOrEmpty() ? taggedString2 : this.customDropPodsLetterLabel.Formatted(taggedString2.Named("BASELABEL")));
							taggedString = (this.customDropPodsLetterText.NullOrEmpty() ? taggedString : this.customDropPodsLetterText.Formatted(taggedString.Named("BASETEXT")));
						}
						else if (this.nearPawn.IsCaravanMember())
						{
							taggedString = "LetterQuestItemsAddedToCaravanInventory".Translate(this.nearPawn.GetCaravan().Named("CARAVAN"), GenLabel.ThingsLabel(this.tmpThingsToGive, "  - ").Named("THINGS"));
							taggedString2 = "LetterLabelQuestItemsAddedToCaravanInventory".Translate(this.nearPawn.GetCaravan().Named("CARAVAN"));
							taggedString2 = (this.customCaravanInventoryLetterLabel.NullOrEmpty() ? taggedString2 : this.customCaravanInventoryLetterLabel.Formatted(taggedString2.Named("BASELABEL")));
							taggedString = (this.customCaravanInventoryLetterText.NullOrEmpty() ? taggedString : this.customCaravanInventoryLetterText.Formatted(taggedString.Named("BASETEXT")));
						}
					}
					if (this.joinPlayer)
					{
						for (int j = 0; j < this.pawns.Count; j++)
						{
							if (this.pawns[j].Faction != Faction.OfPlayer)
							{
								this.pawns[j].SetFaction(Faction.OfPlayer, null);
							}
						}
					}
					else if (this.makePrisoners)
					{
						for (int k = 0; k < this.pawns.Count; k++)
						{
							if (this.pawns[k].RaceProps.Humanlike)
							{
								if (!this.pawns[k].IsPrisonerOfColony)
								{
									this.pawns[k].guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Prisoner);
								}
								HealthUtility.TryAnesthetize(this.pawns[k]);
							}
						}
					}
					GlobalTargetInfo target = GlobalTargetInfo.Invalid;
					if (this.nearPawn.SpawnedOrAnyParentSpawned)
					{
						IntVec3 intVec = DropCellFinder.TradeDropSpot(this.nearPawn.MapHeld);
						DropPodUtility.DropThingsNear(intVec, this.nearPawn.MapHeld, this.tmpThingsToGive, 110, false, false, false, false, true, null);
						target = new GlobalTargetInfo(intVec, this.nearPawn.MapHeld, false);
						for (int l = 0; l < this.tmpThingsToGive.Count; l++)
						{
							if (this.tmpThingsToGive[l].SpawnedOrAnyParentSpawned)
							{
								target = new GlobalTargetInfo(this.tmpThingsToGive[l].PositionHeld, this.nearPawn.MapHeld, false);
								break;
							}
						}
					}
					else if (this.nearPawn.IsCaravanMember())
					{
						for (int m = 0; m < this.tmpThingsToGive.Count; m++)
						{
							Pawn pawn = this.tmpThingsToGive[m] as Pawn;
							if (pawn != null)
							{
								if (pawn.Faction != Faction.OfPlayer)
								{
									pawn.SetFaction(Faction.OfPlayer, null);
								}
								this.nearPawn.GetCaravan().AddPawn(pawn, true);
							}
							else
							{
								CaravanInventoryUtility.GiveThing(this.nearPawn.GetCaravan(), this.tmpThingsToGive[m]);
							}
						}
						target = this.nearPawn.GetCaravan();
					}
					if (this.sendStandardLetter)
					{
						Find.LetterStack.ReceiveLetter(taggedString2, taggedString, this.customLetterDef ?? LetterDefOf.PositiveEvent, target, null, this.quest, null, null, 0, true);
					}
					this.importantLookTarget = this.items.Find((Thing x) => x.GetInnerIfMinified() is MonumentMarker).GetInnerIfMinified();
					this.items.Clear();
					if (!this.outSignalResult.NullOrEmpty())
					{
						if (thing2 != null)
						{
							Find.SignalManager.SendSignal(new Signal(this.outSignalResult, thing2.Named("SUBJECT")));
							return;
						}
						if (thing3 != null)
						{
							Find.SignalManager.SendSignal(new Signal(this.outSignalResult, thing3.Named("SUBJECT")));
							return;
						}
						Find.SignalManager.SendSignal(new Signal(this.outSignalResult, false));
					}
				}
			}
		}

		// Token: 0x0600A515 RID: 42261 RVA: 0x002F4CD2 File Offset: 0x002F2ED2
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x0600A516 RID: 42262 RVA: 0x002F4CE0 File Offset: 0x002F2EE0
		public override void Cleanup()
		{
			base.Cleanup();
			for (int i = 0; i < this.items.Count; i++)
			{
				if (!this.items[i].Destroyed)
				{
					this.items[i].Destroy(DestroyMode.Vanish);
				}
			}
			this.items.Clear();
		}

		// Token: 0x0600A517 RID: 42263 RVA: 0x002F4D3C File Offset: 0x002F2F3C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_References.Look<Pawn>(ref this.nearPawn, "nearPawn", false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<ThingDefCountClass>(ref this.thingDefs, "thingDefs", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.joinPlayer, "joinPlayer", false, false);
			Scribe_Values.Look<bool>(ref this.makePrisoners, "makePrisoners", false, false);
			Scribe_Values.Look<string>(ref this.customDropPodsLetterLabel, "customDropPodsLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customDropPodsLetterText, "customDropPodsLetterText", null, false);
			Scribe_Values.Look<string>(ref this.customCaravanInventoryLetterLabel, "customCaravanInventoryLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customCaravanInventoryLetterText, "customCaravanInventoryLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_References.Look<Thing>(ref this.importantLookTarget, "importantLookTarget", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.items.RemoveAll((Thing x) => x == null);
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600A518 RID: 42264 RVA: 0x002F4EC4 File Offset: 0x002F30C4
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x04007202 RID: 29186
		public string inSignal;

		// Token: 0x04007203 RID: 29187
		public string outSignalResult;

		// Token: 0x04007204 RID: 29188
		public Pawn nearPawn;

		// Token: 0x04007205 RID: 29189
		private List<Thing> items = new List<Thing>();

		// Token: 0x04007206 RID: 29190
		private List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04007207 RID: 29191
		public List<ThingDefCountClass> thingDefs = new List<ThingDefCountClass>();

		// Token: 0x04007208 RID: 29192
		public bool joinPlayer;

		// Token: 0x04007209 RID: 29193
		public bool makePrisoners;

		// Token: 0x0400720A RID: 29194
		public string customDropPodsLetterText;

		// Token: 0x0400720B RID: 29195
		public string customDropPodsLetterLabel;

		// Token: 0x0400720C RID: 29196
		public string customCaravanInventoryLetterText;

		// Token: 0x0400720D RID: 29197
		public string customCaravanInventoryLetterLabel;

		// Token: 0x0400720E RID: 29198
		public LetterDef customLetterDef;

		// Token: 0x0400720F RID: 29199
		public bool sendStandardLetter = true;

		// Token: 0x04007210 RID: 29200
		private Thing importantLookTarget;

		// Token: 0x04007211 RID: 29201
		private List<Thing> tmpThingsToGive = new List<Thing>();
	}
}
