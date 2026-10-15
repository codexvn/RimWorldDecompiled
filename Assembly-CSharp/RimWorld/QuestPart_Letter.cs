using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_Letter : QuestPart
{
	[CompilerGenerated]
	private sealed class _003Cget_InvolvedFactions_003Ed__14 : IEnumerable<Faction>, IEnumerable, IEnumerator<Faction>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Faction _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_Letter _003C_003E4__this;

		private IEnumerator<Faction> _003C_003E7__wrap1;

		Faction IEnumerator<Faction>.Current
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
		public _003Cget_InvolvedFactions_003Ed__14(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				QuestPart_Letter questPart_Letter = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_Letter).InvolvedFactions.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Faction current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_Letter.letter.relatedFaction != null)
					{
						_003C_003E2__current = questPart_Letter.letter.relatedFaction;
						_003C_003E1__state = 2;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Faction> IEnumerable<Faction>.GetEnumerator()
		{
			_003Cget_InvolvedFactions_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_InvolvedFactions_003Ed__14(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Faction>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__12 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_Letter _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

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
		public _003Cget_QuestLookTargets_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				QuestPart_Letter questPart_Letter = _003C_003E4__this;
				GlobalTargetInfo globalTargetInfo;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_Letter).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0071;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0071;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0071:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					globalTargetInfo = questPart_Letter.letter.lookTargets.TryGetPrimaryTarget();
					if (globalTargetInfo.IsValid)
					{
						_003C_003E2__current = globalTargetInfo;
						_003C_003E1__state = 2;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__12(0)
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

	public Letter letter;

	public bool getLookTargetsFromSignal = true;

	public MapParent useColonistsOnMap;

	public string getColonistsFromSignal;

	public bool useColonistsFromCaravanArg;

	public string chosenPawnSignal;

	public string acceptedVisitorsSignal;

	public List<Pawn> visitors;

	public bool filterDeadPawnsFromLookTargets;

	private List<Pawn> colonistsFromSignal = new List<Pawn>();

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__12))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__12(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override IEnumerable<Faction> InvolvedFactions
	{
		[IteratorStateMachine(typeof(_003Cget_InvolvedFactions_003Ed__14))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_InvolvedFactions_003Ed__14(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void Notify_FactionRemoved(Faction faction)
	{
		if (letter.relatedFaction == faction)
		{
			letter.relatedFaction = null;
		}
	}

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (!string.IsNullOrEmpty(getColonistsFromSignal) && signal.tag == getColonistsFromSignal)
		{
			if (signal.args.TryGetArg("SUBJECT", out var arg))
			{
				ReadPawns(arg.arg);
			}
			if (signal.args.TryGetArg("SENT", out var arg2))
			{
				ReadPawns(arg2.arg);
			}
		}
		if (!(signal.tag == inSignal))
		{
			return;
		}
		Letter letter = Gen.MemberwiseClone(this.letter);
		letter.ID = Find.UniqueIDsManager.GetNextLetterID();
		if (letter is ChoiceLetter choiceLetter)
		{
			choiceLetter.quest = quest;
		}
		if (letter is ChoiceLetter_ChoosePawn choiceLetter_ChoosePawn)
		{
			if (useColonistsOnMap != null && useColonistsOnMap.HasMap)
			{
				choiceLetter_ChoosePawn.pawns.Clear();
				choiceLetter_ChoosePawn.pawns.AddRange(useColonistsOnMap.Map.mapPawns.FreeColonists);
				choiceLetter_ChoosePawn.chosenPawnSignal = chosenPawnSignal;
			}
			if (useColonistsFromCaravanArg && signal.args.TryGetArg("CARAVAN", out Caravan arg3) && arg3 != null)
			{
				choiceLetter_ChoosePawn.pawns.Clear();
				choiceLetter_ChoosePawn.pawns.AddRange(arg3.PawnsListForReading.Where((Pawn x) => x.IsFreeColonist));
				choiceLetter_ChoosePawn.chosenPawnSignal = chosenPawnSignal;
			}
			if (!string.IsNullOrEmpty(getColonistsFromSignal))
			{
				colonistsFromSignal.RemoveAll((Pawn x) => x.Dead);
				choiceLetter_ChoosePawn.pawns.Clear();
				choiceLetter_ChoosePawn.pawns.AddRange(colonistsFromSignal);
				choiceLetter_ChoosePawn.chosenPawnSignal = chosenPawnSignal;
			}
		}
		if (letter is ChoiceLetter_AcceptVisitors choiceLetter_AcceptVisitors)
		{
			choiceLetter_AcceptVisitors.acceptedSignal = acceptedVisitorsSignal;
			if (visitors != null)
			{
				choiceLetter_AcceptVisitors.pawns.AddRange(visitors);
			}
		}
		if (getLookTargetsFromSignal && !letter.lookTargets.IsValid() && SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out var lookTargets))
		{
			letter.lookTargets = lookTargets;
		}
		letter.Label = signal.args.GetFormattedText(letter.Label);
		ChoiceLetter choiceLetter2 = letter as ChoiceLetter;
		bool flag = true;
		if (choiceLetter2 != null)
		{
			choiceLetter2.title = signal.args.GetFormattedText(choiceLetter2.title);
			choiceLetter2.Text = signal.args.GetFormattedText(choiceLetter2.Text);
			if (choiceLetter2.Text.NullOrEmpty())
			{
				flag = false;
			}
		}
		if (filterDeadPawnsFromLookTargets)
		{
			for (int num = letter.lookTargets.targets.Count - 1; num >= 0; num--)
			{
				Thing thing = letter.lookTargets.targets[num].Thing;
				if (thing is Pawn { Dead: not false })
				{
					letter.lookTargets.targets.Remove(thing);
				}
			}
		}
		if (flag)
		{
			Find.LetterStack.ReceiveLetter(letter);
		}
		void ReadPawns(object obj)
		{
			if (obj is Pawn item && !colonistsFromSignal.Contains(item))
			{
				colonistsFromSignal.Add(item);
			}
			if (obj is List<Pawn> source)
			{
				colonistsFromSignal.AddRange(source.Where((Pawn p) => !colonistsFromSignal.Contains(p)));
			}
			if (obj is List<Thing> source2)
			{
				colonistsFromSignal.AddRange(from Pawn p in source2.Where((Thing t) => t is Pawn)
					where !colonistsFromSignal.Contains(p)
					select p);
			}
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref inSignal, "inSignal");
		Scribe_Deep.Look(ref letter, "letter");
		Scribe_Values.Look(ref getLookTargetsFromSignal, "getLookTargetsFromSignal", defaultValue: true);
		Scribe_References.Look(ref useColonistsOnMap, "useColonistsOnMap");
		Scribe_Values.Look(ref useColonistsFromCaravanArg, "useColonistsFromCaravanArg", defaultValue: false);
		Scribe_Values.Look(ref chosenPawnSignal, "chosenPawnSignal");
		Scribe_Values.Look(ref filterDeadPawnsFromLookTargets, "filterDeadPawnsFromLookTargets", defaultValue: false);
		Scribe_Values.Look(ref getColonistsFromSignal, "getColonistsFromSignal");
		Scribe_Collections.Look(ref colonistsFromSignal, "colonistsFromSignal", LookMode.Reference);
		if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
		{
			colonistsFromSignal.RemoveAll((Pawn x) => x == null);
		}
		Scribe_Values.Look(ref acceptedVisitorsSignal, "acceptedVisitorsSignal");
		Scribe_Collections.Look(ref visitors, "visitors", LookMode.Reference);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			visitors?.RemoveAll((Pawn x) => x == null);
		}
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		inSignal = "DebugSignal" + Rand.Int;
		letter = LetterMaker.MakeLetter("DEV: Test", "Test text", LetterDefOf.PositiveEvent);
	}
}
