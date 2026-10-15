using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_DropPods : QuestPart
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__26 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_DropPods _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

		private IEnumerator<Pawn> _003C_003E7__wrap2;

		GlobalTargetInfo IEnumerator<GlobalTargetInfo>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003Cget_QuestLookTargets_003Ed__26(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				QuestPart_DropPods questPart_DropPods = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_DropPods).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00c2;
				case 3:
					_003C_003E1__state = -4;
					goto IL_011a;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_DropPods.mapParent != null)
					{
						_003C_003E2__current = questPart_DropPods.mapParent;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00c2;
					IL_011a:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Pawn current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					if (questPart_DropPods.importantLookTarget != null)
					{
						_003C_003E2__current = questPart_DropPods.importantLookTarget;
						_003C_003E1__state = 4;
						return true;
					}
					break;
					IL_00c2:
					_003C_003E7__wrap2 = PawnsArriveQuestPartUtility.GetQuestLookTargets(questPart_DropPods.pawns.Concat(questPart_DropPods.pawnsInContainers)).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_011a;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__26 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__26(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GlobalTargetInfo>)this).GetEnumerator();
		}
	}

	public string inSignal;

	public string outSignalResult;

	public IntVec3 dropSpot = IntVec3.Invalid;

	public bool useTradeDropSpot;

	public MapParent mapParent;

	private List<Thing> items = new List<Thing>();

	private List<Pawn> pawns = new List<Pawn>();

	private List<Pawn> pawnsInContainers = new List<Pawn>();

	public List<ThingDefCountClass> thingDefs = new List<ThingDefCountClass>();

	public List<ThingDef> thingsToExcludeFromHyperlinks = new List<ThingDef>();

	public bool joinPlayer;

	public bool makePrisoners;

	public bool destroyItemsOnCleanup = true;

	public bool dropAllInSamePod;

	public bool allowFogged;

	public Faction faction;

	public bool canRetargetAnyMap;

	public string customLetterText;

	public string customLetterLabel;

	public LetterDef customLetterDef;

	public bool sendStandardLetter = true;

	private Thing importantLookTarget;

	private List<Thing> tmpThingsToDrop = new List<Thing>();

	public IEnumerable<Thing> Things
	{
		get
		{
			return items.Concat(pawns.Cast<Thing>());
		}
		set
		{
			items.Clear();
			pawns.Clear();
			if (value == null)
			{
				return;
			}
			foreach (Thing item3 in value)
			{
				if (item3.Destroyed)
				{
					Log.Error("Tried to add a destroyed thing to QuestPart_DropPods: " + item3.ToStringSafe());
					continue;
				}
				if (item3 is Pawn item)
				{
					pawns.Add(item);
					continue;
				}
				items.Add(item3);
				ThingOwner thingOwner = item3.TryGetInnerInteractableThingOwner();
				if (thingOwner == null)
				{
					continue;
				}
				for (int i = 0; i < thingOwner.Count; i++)
				{
					if (thingOwner[i] is Pawn item2)
					{
						pawnsInContainers.Add(item2);
					}
				}
			}
		}
	}

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__26))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__26(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override bool IncreasesPopulation => PawnsArriveQuestPartUtility.IncreasesPopulation(pawns, joinPlayer, makePrisoners);

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (signal.tag != inSignal)
		{
			return;
		}
		if (mapParent == null || !mapParent.HasMap || !quest.IsParentSuitableForQuest(mapParent))
		{
			if (canRetargetAnyMap)
			{
				mapParent = quest.TryFindNewSuitablePlayerMapParentForRetarget(checkQuestScript: false);
			}
			else
			{
				mapParent = quest.TryFindNewSuitablePlayerMapParentForRetarget();
			}
		}
		if (mapParent == null || !mapParent.HasMap)
		{
			return;
		}
		pawns.RemoveAll((Pawn x) => x.Destroyed);
		pawnsInContainers.RemoveAll((Pawn x) => x.Destroyed);
		items.RemoveAll((Thing x) => x.Destroyed);
		tmpThingsToDrop.Clear();
		tmpThingsToDrop.AddRange(Things);
		for (int i = 0; i < thingDefs.Count; i++)
		{
			Thing thing = ThingMaker.MakeThing(thingDefs[i].thingDef, GenStuff.RandomStuffByCommonalityFor(thingDefs[i].thingDef));
			thing.stackCount = thingDefs[i].count;
			tmpThingsToDrop.Add(thing);
		}
		tmpThingsToDrop.RemoveAll((Thing x) => x.Spawned);
		Thing thing2 = tmpThingsToDrop.Where((Thing x) => x is Pawn).MaxByWithFallback((Thing x) => x.MarketValue);
		Thing thing3 = tmpThingsToDrop.MaxByWithFallback((Thing x) => x.MarketValue * (float)x.stackCount);
		if (!tmpThingsToDrop.Any())
		{
			return;
		}
		Map map = mapParent.Map;
		IntVec3 intVec = (dropSpot.IsValid ? dropSpot : GetRandomDropSpot());
		TaggedString text = null;
		TaggedString label = null;
		if (sendStandardLetter)
		{
			if (joinPlayer && pawns.Count == 1 && pawns[0].RaceProps.Humanlike)
			{
				text = "LetterRefugeeJoins".Translate(pawns[0].Named("PAWN"));
				label = "LetterLabelRefugeeJoins".Translate(pawns[0].Named("PAWN"));
				PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawns[0]);
			}
			else
			{
				text = "LetterQuestDropPodsArrived".Translate(GenLabel.ThingsLabel(tmpThingsToDrop));
				label = "LetterLabelQuestDropPodsArrived".Translate();
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(pawns, ref label, ref text, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), informEvenIfSeenBefore: true);
			}
			label = (customLetterLabel.NullOrEmpty() ? label : customLetterLabel.Formatted(label.Named("BASELABEL")));
			text = (customLetterText.NullOrEmpty() ? text : customLetterText.Formatted(text.Named("BASETEXT")));
		}
		if (joinPlayer)
		{
			for (int j = 0; j < pawns.Count; j++)
			{
				if (pawns[j].Faction != Faction.OfPlayer)
				{
					pawns[j].SetFaction(Faction.OfPlayer);
				}
			}
		}
		else if (makePrisoners)
		{
			for (int k = 0; k < pawns.Count; k++)
			{
				if (pawns[k].RaceProps.Humanlike)
				{
					if (!pawns[k].IsPrisonerOfColony)
					{
						pawns[k].guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Prisoner);
					}
					HealthUtility.TryAnesthetize(pawns[k]);
				}
			}
		}
		if (dropAllInSamePod)
		{
			DropPodUtility.DropThingGroupsNear(intVec, map, new List<List<Thing>> { tmpThingsToDrop }, 110, instaDrop: false, leaveSlag: false, !useTradeDropSpot, forbid: false, allowFogged, canTransfer: false, faction);
		}
		else
		{
			DropPodUtility.DropThingsNear(intVec, map, tmpThingsToDrop, 110, canInstaDropDuringInit: false, leaveSlag: false, !useTradeDropSpot, forbid: false, allowFogged, faction);
		}
		for (int l = 0; l < pawns.Count; l++)
		{
			pawns[l].needs.SetInitialLevels();
			pawns[l].mindState?.SetupLastHumanMeatTick();
		}
		if (sendStandardLetter)
		{
			IntVec3 cell = intVec;
			for (int m = 0; m < tmpThingsToDrop.Count; m++)
			{
				if (tmpThingsToDrop[m].SpawnedOrAnyParentSpawned)
				{
					cell = tmpThingsToDrop[m].PositionHeld;
					break;
				}
			}
			Find.LetterStack.ReceiveLetter(label, text, customLetterDef ?? LetterDefOf.PositiveEvent, new TargetInfo(cell, map), null, quest);
		}
		importantLookTarget = items.Find((Thing x) => x.GetInnerIfMinified() is MonumentMarker).GetInnerIfMinified();
		items.Clear();
		if (!outSignalResult.NullOrEmpty())
		{
			if (thing2 != null)
			{
				Find.SignalManager.SendSignal(new Signal(outSignalResult, thing2.Named("SUBJECT")));
			}
			else if (thing3 != null)
			{
				Find.SignalManager.SendSignal(new Signal(outSignalResult, thing3.Named("SUBJECT")));
			}
			else
			{
				Find.SignalManager.SendSignal(new Signal(outSignalResult));
			}
		}
	}

	public override bool QuestPartReserves(Pawn p)
	{
		if (!pawns.Contains(p))
		{
			return pawnsInContainers.Contains(p);
		}
		return true;
	}

	public override void ReplacePawnReferences(Pawn replace, Pawn with)
	{
		pawns.Replace(replace, with);
	}

	public override void PostQuestAdded()
	{
		base.PostQuestAdded();
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].def == ThingDefOf.PsychicAmplifier)
			{
				Find.History.Notify_PsylinkAvailable();
				break;
			}
		}
	}

	public override void Cleanup()
	{
		base.Cleanup();
		if (!destroyItemsOnCleanup)
		{
			return;
		}
		for (int i = 0; i < items.Count; i++)
		{
			if (!items[i].Destroyed)
			{
				items[i].Destroy();
			}
		}
		items.Clear();
	}

	private IntVec3 GetRandomDropSpot()
	{
		Map map = mapParent.Map;
		if (useTradeDropSpot)
		{
			return DropCellFinder.TradeDropSpot(map);
		}
		if (CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => x.Standable(map) && !x.Roofed(map) && (allowFogged || !x.Fogged(map)) && map.reachability.CanReachColony(x), map, 1000, out var result))
		{
			return result;
		}
		return DropCellFinder.RandomDropSpot(map);
	}

	public override void Notify_FactionRemoved(Faction f)
	{
		if (faction == f)
		{
			faction = null;
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref inSignal, "inSignal");
		Scribe_Values.Look(ref outSignalResult, "outSignalResult");
		Scribe_Values.Look(ref dropSpot, "dropSpot");
		Scribe_Values.Look(ref useTradeDropSpot, "useTradeDropSpot", defaultValue: false);
		Scribe_References.Look(ref mapParent, "mapParent");
		Scribe_Collections.Look(ref items, "items", LookMode.Deep);
		Scribe_Collections.Look(ref pawns, "pawns", LookMode.Reference);
		Scribe_Collections.Look(ref pawnsInContainers, "pawnsInContainers", LookMode.Reference);
		Scribe_Collections.Look(ref thingDefs, "thingDefs", LookMode.Deep);
		Scribe_Values.Look(ref joinPlayer, "joinPlayer", defaultValue: false);
		Scribe_Values.Look(ref makePrisoners, "makePrisoners", defaultValue: false);
		Scribe_Values.Look(ref customLetterLabel, "customLetterLabel");
		Scribe_Values.Look(ref customLetterText, "customLetterText");
		Scribe_Defs.Look(ref customLetterDef, "customLetterDef");
		Scribe_Values.Look(ref sendStandardLetter, "sendStandardLetter", defaultValue: true);
		Scribe_References.Look(ref importantLookTarget, "importantLookTarget");
		Scribe_Collections.Look(ref thingsToExcludeFromHyperlinks, "thingsToExcludeFromHyperlinks", LookMode.Def);
		Scribe_Values.Look(ref destroyItemsOnCleanup, "destroyItemsOnCleanup", defaultValue: false);
		Scribe_Values.Look(ref allowFogged, "allowFogged", defaultValue: false);
		Scribe_References.Look(ref faction, "faction");
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			if (thingsToExcludeFromHyperlinks == null)
			{
				thingsToExcludeFromHyperlinks = new List<ThingDef>();
			}
			items.RemoveAll((Thing x) => x == null);
			pawns.RemoveAll((Pawn x) => x == null);
			pawnsInContainers.RemoveAll((Pawn x) => x == null);
		}
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		inSignal = "DebugSignal" + Rand.Int;
		if (Find.AnyPlayerHomeMap == null)
		{
			return;
		}
		mapParent = Find.RandomPlayerHomeMap.Parent;
		List<Thing> list = ThingSetMakerDefOf.DebugQuestDropPodsContents.root.Generate();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] is Pawn pawn)
			{
				pawn.relations.everSeenByPlayer = true;
				if (!pawn.IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawn);
				}
			}
		}
		Things = list;
	}
}
