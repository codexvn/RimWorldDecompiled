using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_GiveNearPawn : QuestPart
{
	[CompilerGenerated]
	private sealed class _003Cget_Hyperlinks_003Ed__19 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_GiveNearPawn _003C_003E4__this;

		private IEnumerator<Dialog_InfoCard.Hyperlink> _003C_003E7__wrap1;

		private List<Thing>.Enumerator _003C_003E7__wrap2;

		Dialog_InfoCard.Hyperlink IEnumerator<Dialog_InfoCard.Hyperlink>.Current
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
		public _003Cget_Hyperlinks_003Ed__19(int _003C_003E1__state)
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
			case 2:
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
			_003C_003E7__wrap2 = default(List<Thing>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				QuestPart_GiveNearPawn questPart_GiveNearPawn = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_GiveNearPawn).Hyperlinks.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Dialog_InfoCard.Hyperlink current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = questPart_GiveNearPawn.items.GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					ThingDef def = _003C_003E7__wrap2.Current.GetInnerIfMinified().def;
					_003C_003E2__current = new Dialog_InfoCard.Hyperlink(def);
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = default(List<Thing>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Dialog_InfoCard.Hyperlink> IEnumerable<Dialog_InfoCard.Hyperlink>.GetEnumerator()
		{
			_003Cget_Hyperlinks_003Ed__19 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Hyperlinks_003Ed__19(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Dialog_InfoCard.Hyperlink>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__21 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_GiveNearPawn _003C_003E4__this;

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
		public _003Cget_QuestLookTargets_003Ed__21(int _003C_003E1__state)
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
			case 2:
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
				QuestPart_GiveNearPawn questPart_GiveNearPawn = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_GiveNearPawn).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00dd;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = PawnsArriveQuestPartUtility.GetQuestLookTargets(questPart_GiveNearPawn.pawns).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00dd;
					IL_00dd:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Pawn current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					if (questPart_GiveNearPawn.importantLookTarget != null)
					{
						_003C_003E2__current = questPart_GiveNearPawn.importantLookTarget;
						_003C_003E1__state = 3;
						return true;
					}
					break;
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
			_003Cget_QuestLookTargets_003Ed__21 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__21(0)
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

	public Pawn nearPawn;

	private List<Thing> items = new List<Thing>();

	private List<Pawn> pawns = new List<Pawn>();

	public List<ThingDefCountClass> thingDefs = new List<ThingDefCountClass>();

	public bool joinPlayer;

	public bool makePrisoners;

	public string customDropPodsLetterText;

	public string customDropPodsLetterLabel;

	public string customCaravanInventoryLetterText;

	public string customCaravanInventoryLetterLabel;

	public LetterDef customLetterDef;

	public bool sendStandardLetter = true;

	private Thing importantLookTarget;

	private List<Thing> tmpThingsToGive = new List<Thing>();

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
			foreach (Thing item2 in value)
			{
				if (item2 is Pawn item)
				{
					pawns.Add(item);
				}
				else
				{
					items.Add(item2);
				}
			}
		}
	}

	public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
	{
		[IteratorStateMachine(typeof(_003Cget_Hyperlinks_003Ed__19))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Hyperlinks_003Ed__19(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__21))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__21(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override bool IncreasesPopulation => PawnsArriveQuestPartUtility.IncreasesPopulation(pawns, joinPlayer, makePrisoners);

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (!(signal.tag == inSignal) || nearPawn == null || (!nearPawn.SpawnedOrAnyParentSpawned && !nearPawn.IsCaravanMember()))
		{
			return;
		}
		pawns.RemoveAll((Pawn x) => x.Destroyed);
		items.RemoveAll((Thing x) => x.Destroyed);
		tmpThingsToGive.Clear();
		tmpThingsToGive.AddRange(Things);
		for (int i = 0; i < thingDefs.Count; i++)
		{
			Thing thing = ThingMaker.MakeThing(thingDefs[i].thingDef, GenStuff.RandomStuffByCommonalityFor(thingDefs[i].thingDef));
			thing.stackCount = thingDefs[i].count;
			tmpThingsToGive.Add(thing);
		}
		tmpThingsToGive.RemoveAll((Thing x) => x.Spawned);
		Thing thing2 = tmpThingsToGive.Where((Thing x) => x is Pawn).MaxByWithFallback((Thing x) => x.MarketValue);
		Thing thing3 = tmpThingsToGive.MaxByWithFallback((Thing x) => x.MarketValue * (float)x.stackCount);
		if (!tmpThingsToGive.Any())
		{
			return;
		}
		TaggedString text = null;
		TaggedString label = null;
		if (sendStandardLetter)
		{
			if (nearPawn.SpawnedOrAnyParentSpawned)
			{
				if (joinPlayer && pawns.Count == 1 && pawns[0].RaceProps.Humanlike)
				{
					text = "LetterRefugeeJoins".Translate(pawns[0].Named("PAWN"));
					label = "LetterLabelRefugeeJoins".Translate(pawns[0].Named("PAWN"));
					PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawns[0]);
				}
				else
				{
					text = "LetterQuestDropPodsArrived".Translate(GenLabel.ThingsLabel(tmpThingsToGive));
					label = "LetterLabelQuestDropPodsArrived".Translate();
					PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(pawns, ref label, ref text, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), informEvenIfSeenBefore: true);
				}
				label = (customDropPodsLetterLabel.NullOrEmpty() ? label : customDropPodsLetterLabel.Formatted(label.Named("BASELABEL")));
				text = (customDropPodsLetterText.NullOrEmpty() ? text : customDropPodsLetterText.Formatted(text.Named("BASETEXT")));
			}
			else if (nearPawn.IsCaravanMember())
			{
				text = "LetterQuestItemsAddedToCaravanInventory".Translate(nearPawn.GetCaravan().Named("CARAVAN"), GenLabel.ThingsLabel(tmpThingsToGive).Named("THINGS"));
				label = "LetterLabelQuestItemsAddedToCaravanInventory".Translate(nearPawn.GetCaravan().Named("CARAVAN"));
				label = (customCaravanInventoryLetterLabel.NullOrEmpty() ? label : customCaravanInventoryLetterLabel.Formatted(label.Named("BASELABEL")));
				text = (customCaravanInventoryLetterText.NullOrEmpty() ? text : customCaravanInventoryLetterText.Formatted(text.Named("BASETEXT")));
			}
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
		GlobalTargetInfo globalTargetInfo = GlobalTargetInfo.Invalid;
		if (nearPawn.SpawnedOrAnyParentSpawned)
		{
			IntVec3 intVec = DropCellFinder.TradeDropSpot(nearPawn.MapHeld);
			DropPodUtility.DropThingsNear(intVec, nearPawn.MapHeld, tmpThingsToGive, 110, canInstaDropDuringInit: false, leaveSlag: false, canRoofPunch: false, forbid: false);
			globalTargetInfo = new GlobalTargetInfo(intVec, nearPawn.MapHeld);
			for (int l = 0; l < tmpThingsToGive.Count; l++)
			{
				if (tmpThingsToGive[l].SpawnedOrAnyParentSpawned)
				{
					globalTargetInfo = new GlobalTargetInfo(tmpThingsToGive[l].PositionHeld, nearPawn.MapHeld);
					break;
				}
			}
		}
		else if (nearPawn.IsCaravanMember())
		{
			for (int m = 0; m < tmpThingsToGive.Count; m++)
			{
				if (tmpThingsToGive[m] is Pawn pawn)
				{
					if (pawn.Faction != Faction.OfPlayer)
					{
						pawn.SetFaction(Faction.OfPlayer);
					}
					nearPawn.GetCaravan().AddPawn(pawn, addCarriedPawnToWorldPawnsIfAny: true);
				}
				else
				{
					CaravanInventoryUtility.GiveThing(nearPawn.GetCaravan(), tmpThingsToGive[m]);
				}
			}
			globalTargetInfo = nearPawn.GetCaravan();
		}
		if (sendStandardLetter)
		{
			Find.LetterStack.ReceiveLetter(label, text, customLetterDef ?? LetterDefOf.PositiveEvent, globalTargetInfo, null, quest);
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
		return pawns.Contains(p);
	}

	public override void Cleanup()
	{
		base.Cleanup();
		for (int i = 0; i < items.Count; i++)
		{
			if (!items[i].Destroyed)
			{
				items[i].Destroy();
			}
		}
		items.Clear();
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref inSignal, "inSignal");
		Scribe_Values.Look(ref outSignalResult, "outSignalResult");
		Scribe_References.Look(ref nearPawn, "nearPawn");
		Scribe_Collections.Look(ref items, "items", LookMode.Deep);
		Scribe_Collections.Look(ref pawns, "pawns", LookMode.Reference);
		Scribe_Collections.Look(ref thingDefs, "thingDefs", LookMode.Deep);
		Scribe_Values.Look(ref joinPlayer, "joinPlayer", defaultValue: false);
		Scribe_Values.Look(ref makePrisoners, "makePrisoners", defaultValue: false);
		Scribe_Values.Look(ref customDropPodsLetterLabel, "customDropPodsLetterLabel");
		Scribe_Values.Look(ref customDropPodsLetterText, "customDropPodsLetterText");
		Scribe_Values.Look(ref customCaravanInventoryLetterLabel, "customCaravanInventoryLetterLabel");
		Scribe_Values.Look(ref customCaravanInventoryLetterText, "customCaravanInventoryLetterText");
		Scribe_Defs.Look(ref customLetterDef, "customLetterDef");
		Scribe_Values.Look(ref sendStandardLetter, "sendStandardLetter", defaultValue: true);
		Scribe_References.Look(ref importantLookTarget, "importantLookTarget");
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			items.RemoveAll((Thing x) => x == null);
			pawns.RemoveAll((Pawn x) => x == null);
		}
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		inSignal = "DebugSignal" + Rand.Int;
	}
}
