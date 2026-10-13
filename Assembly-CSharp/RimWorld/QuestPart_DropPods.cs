using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001CDB RID: 7387
	public class QuestPart_DropPods : QuestPart
	{
		// Token: 0x17001963 RID: 6499
		// (get) Token: 0x0600A4AE RID: 42158 RVA: 0x002F2957 File Offset: 0x002F0B57
		// (set) Token: 0x0600A4AF RID: 42159 RVA: 0x002F2970 File Offset: 0x002F0B70
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
						if (thing.Destroyed)
						{
							Log.Error("Tried to add a destroyed thing to QuestPart_DropPods: " + thing.ToStringSafe<Thing>());
						}
						else
						{
							Pawn pawn = thing as Pawn;
							if (pawn != null)
							{
								this.pawns.Add(pawn);
							}
							else
							{
								this.items.Add(thing);
								ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
								if (thingOwner != null)
								{
									for (int i = 0; i < thingOwner.Count; i++)
									{
										Pawn pawn2 = thingOwner[i] as Pawn;
										if (pawn2 != null)
										{
											this.pawnsInContainers.Add(pawn2);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x17001964 RID: 6500
		// (get) Token: 0x0600A4B0 RID: 42160 RVA: 0x002F2A58 File Offset: 0x002F0C58
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				foreach (Pawn t in PawnsArriveQuestPartUtility.GetQuestLookTargets(this.pawns.Concat(this.pawnsInContainers)))
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

		// Token: 0x17001965 RID: 6501
		// (get) Token: 0x0600A4B1 RID: 42161 RVA: 0x002F2A68 File Offset: 0x002F0C68
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, this.makePrisoners);
			}
		}

		// Token: 0x0600A4B2 RID: 42162 RVA: 0x002F2A84 File Offset: 0x002F0C84
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.mapParent == null || !this.mapParent.HasMap)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				this.mapParent = ((anyPlayerHomeMap != null) ? anyPlayerHomeMap.Parent : null);
			}
			if (signal.tag == this.inSignal && this.mapParent != null && this.mapParent.HasMap)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				this.pawnsInContainers.RemoveAll((Pawn x) => x.Destroyed);
				this.items.RemoveAll((Thing x) => x.Destroyed);
				this.tmpThingsToDrop.Clear();
				this.tmpThingsToDrop.AddRange(this.Things);
				for (int i = 0; i < this.thingDefs.Count; i++)
				{
					Thing thing = ThingMaker.MakeThing(this.thingDefs[i].thingDef, GenStuff.RandomStuffByCommonalityFor(this.thingDefs[i].thingDef, TechLevel.Undefined));
					thing.stackCount = this.thingDefs[i].count;
					this.tmpThingsToDrop.Add(thing);
				}
				this.tmpThingsToDrop.RemoveAll((Thing x) => x.Spawned);
				Thing thing2 = (from x in this.tmpThingsToDrop
				where x is Pawn
				select x).MaxByWithFallback((Thing x) => x.MarketValue, null);
				Thing thing3 = this.tmpThingsToDrop.MaxByWithFallback((Thing x) => x.MarketValue * (float)x.stackCount, null);
				if (this.tmpThingsToDrop.Any<Thing>())
				{
					Map map = this.mapParent.Map;
					IntVec3 intVec = this.dropSpot.IsValid ? this.dropSpot : this.GetRandomDropSpot();
					TaggedString taggedString = null;
					TaggedString taggedString2 = null;
					if (this.sendStandardLetter)
					{
						if (this.joinPlayer && this.pawns.Count == 1 && this.pawns[0].RaceProps.Humanlike)
						{
							taggedString = "LetterRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
							taggedString2 = "LetterLabelRefugeeJoins".Translate(this.pawns[0].Named("PAWN"));
							PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref taggedString2, this.pawns[0]);
						}
						else
						{
							taggedString = "LetterQuestDropPodsArrived".Translate(GenLabel.ThingsLabel(this.tmpThingsToDrop, "  - "));
							taggedString2 = "LetterLabelQuestDropPodsArrived".Translate();
							PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(this.pawns, ref taggedString2, ref taggedString, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
						}
						taggedString2 = (this.customLetterLabel.NullOrEmpty() ? taggedString2 : this.customLetterLabel.Formatted(taggedString2.Named("BASELABEL")));
						taggedString = (this.customLetterText.NullOrEmpty() ? taggedString : this.customLetterText.Formatted(taggedString.Named("BASETEXT")));
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
					if (this.dropAllInSamePod)
					{
						DropPodUtility.DropThingGroupsNear(intVec, map, new List<List<Thing>>
						{
							this.tmpThingsToDrop
						}, 110, false, false, !this.useTradeDropSpot, false, this.allowFogged, false, this.faction);
					}
					else
					{
						DropPodUtility.DropThingsNear(intVec, map, this.tmpThingsToDrop, 110, false, false, !this.useTradeDropSpot, false, this.allowFogged, this.faction);
					}
					for (int l = 0; l < this.pawns.Count; l++)
					{
						this.pawns[l].needs.SetInitialLevels();
						Pawn_MindState mindState = this.pawns[l].mindState;
						if (mindState != null)
						{
							mindState.SetupLastHumanMeatTick();
						}
					}
					if (this.sendStandardLetter)
					{
						IntVec3 cell = intVec;
						for (int m = 0; m < this.tmpThingsToDrop.Count; m++)
						{
							if (this.tmpThingsToDrop[m].SpawnedOrAnyParentSpawned)
							{
								cell = this.tmpThingsToDrop[m].PositionHeld;
								break;
							}
						}
						Find.LetterStack.ReceiveLetter(taggedString2, taggedString, this.customLetterDef ?? LetterDefOf.PositiveEvent, new TargetInfo(cell, map, false), null, this.quest, null, null, 0, true);
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

		// Token: 0x0600A4B3 RID: 42163 RVA: 0x002F30FD File Offset: 0x002F12FD
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p) || this.pawnsInContainers.Contains(p);
		}

		// Token: 0x0600A4B4 RID: 42164 RVA: 0x002F311B File Offset: 0x002F131B
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x0600A4B5 RID: 42165 RVA: 0x002F312C File Offset: 0x002F132C
		public override void PostQuestAdded()
		{
			base.PostQuestAdded();
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this.items[i].def == ThingDefOf.PsychicAmplifier)
				{
					Find.History.Notify_PsylinkAvailable();
					return;
				}
			}
		}

		// Token: 0x0600A4B6 RID: 42166 RVA: 0x002F3178 File Offset: 0x002F1378
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.destroyItemsOnCleanup)
			{
				for (int i = 0; i < this.items.Count; i++)
				{
					if (!this.items[i].Destroyed)
					{
						this.items[i].Destroy(DestroyMode.Vanish);
					}
				}
				this.items.Clear();
			}
		}

		// Token: 0x0600A4B7 RID: 42167 RVA: 0x002F31DC File Offset: 0x002F13DC
		private IntVec3 GetRandomDropSpot()
		{
			Map map = this.mapParent.Map;
			if (this.useTradeDropSpot)
			{
				return DropCellFinder.TradeDropSpot(map);
			}
			IntVec3 result;
			if (CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => x.Standable(map) && !x.Roofed(map) && (this.allowFogged || !x.Fogged(map)) && map.reachability.CanReachColony(x), map, 1000, out result))
			{
				return result;
			}
			return DropCellFinder.RandomDropSpot(map, true);
		}

		// Token: 0x0600A4B8 RID: 42168 RVA: 0x002F3249 File Offset: 0x002F1449
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x0600A4B9 RID: 42169 RVA: 0x002F325C File Offset: 0x002F145C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_Values.Look<IntVec3>(ref this.dropSpot, "dropSpot", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.useTradeDropSpot, "useTradeDropSpot", false, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawnsInContainers, "pawnsInContainers", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<ThingDefCountClass>(ref this.thingDefs, "thingDefs", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.joinPlayer, "joinPlayer", false, false);
			Scribe_Values.Look<bool>(ref this.makePrisoners, "makePrisoners", false, false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_References.Look<Thing>(ref this.importantLookTarget, "importantLookTarget", false);
			Scribe_Collections.Look<ThingDef>(ref this.thingsToExcludeFromHyperlinks, "thingsToExcludeFromHyperlinks", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.destroyItemsOnCleanup, "destroyItemsOnCleanup", false, false);
			Scribe_Values.Look<bool>(ref this.allowFogged, "allowFogged", false, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.thingsToExcludeFromHyperlinks == null)
				{
					this.thingsToExcludeFromHyperlinks = new List<ThingDef>();
				}
				this.items.RemoveAll((Thing x) => x == null);
				this.pawns.RemoveAll((Pawn x) => x == null);
				this.pawnsInContainers.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600A4BA RID: 42170 RVA: 0x002F3490 File Offset: 0x002F1690
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				List<Thing> list = ThingSetMakerDefOf.DebugQuestDropPodsContents.root.Generate();
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i] as Pawn;
					if (pawn != null)
					{
						pawn.relations.everSeenByPlayer = true;
						if (!pawn.IsWorldPawn())
						{
							Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						}
					}
				}
				this.Things = list;
			}
		}

		// Token: 0x040071AF RID: 29103
		public string inSignal;

		// Token: 0x040071B0 RID: 29104
		public string outSignalResult;

		// Token: 0x040071B1 RID: 29105
		public IntVec3 dropSpot = IntVec3.Invalid;

		// Token: 0x040071B2 RID: 29106
		public bool useTradeDropSpot;

		// Token: 0x040071B3 RID: 29107
		public MapParent mapParent;

		// Token: 0x040071B4 RID: 29108
		private List<Thing> items = new List<Thing>();

		// Token: 0x040071B5 RID: 29109
		private List<Pawn> pawns = new List<Pawn>();

		// Token: 0x040071B6 RID: 29110
		private List<Pawn> pawnsInContainers = new List<Pawn>();

		// Token: 0x040071B7 RID: 29111
		public List<ThingDefCountClass> thingDefs = new List<ThingDefCountClass>();

		// Token: 0x040071B8 RID: 29112
		public List<ThingDef> thingsToExcludeFromHyperlinks = new List<ThingDef>();

		// Token: 0x040071B9 RID: 29113
		public bool joinPlayer;

		// Token: 0x040071BA RID: 29114
		public bool makePrisoners;

		// Token: 0x040071BB RID: 29115
		public bool destroyItemsOnCleanup = true;

		// Token: 0x040071BC RID: 29116
		public bool dropAllInSamePod;

		// Token: 0x040071BD RID: 29117
		public bool allowFogged;

		// Token: 0x040071BE RID: 29118
		public Faction faction;

		// Token: 0x040071BF RID: 29119
		public string customLetterText;

		// Token: 0x040071C0 RID: 29120
		public string customLetterLabel;

		// Token: 0x040071C1 RID: 29121
		public LetterDef customLetterDef;

		// Token: 0x040071C2 RID: 29122
		public bool sendStandardLetter = true;

		// Token: 0x040071C3 RID: 29123
		private Thing importantLookTarget;

		// Token: 0x040071C4 RID: 29124
		private List<Thing> tmpThingsToDrop = new List<Thing>();
	}
}
