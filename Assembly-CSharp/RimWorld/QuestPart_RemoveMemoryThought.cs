using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_RemoveMemoryThought : QuestPart
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__7 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_RemoveMemoryThought _003C_003E4__this;

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
		public _003Cget_QuestLookTargets_003Ed__7(int _003C_003E1__state)
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
				QuestPart_RemoveMemoryThought questPart_RemoveMemoryThought = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_RemoveMemoryThought).QuestLookTargets.GetEnumerator();
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
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_RemoveMemoryThought.pawn != null && questPart_RemoveMemoryThought.addToLookTargets)
					{
						_003C_003E2__current = questPart_RemoveMemoryThought.pawn;
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
			_003Cget_QuestLookTargets_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__7(0)
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

	public ThoughtDef def;

	public Pawn pawn;

	public Pawn otherPawn;

	public int? count;

	public bool addToLookTargets = true;

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__7))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__7(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (!(signal.tag == inSignal) || pawn == null || pawn.needs == null)
		{
			return;
		}
		if (count.HasValue)
		{
			for (int i = 0; i < count.Value; i++)
			{
				Thought_Memory thought_Memory = pawn.needs.mood.thoughts.memories.Memories.FirstOrDefault((Thought_Memory m) => def == m.def && (otherPawn == null || m.otherPawn == otherPawn));
				if (thought_Memory != null)
				{
					pawn.needs.mood.thoughts.memories.RemoveMemory(thought_Memory);
					continue;
				}
				break;
			}
		}
		else if (otherPawn == null)
		{
			pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(def);
		}
		else
		{
			pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(def, otherPawn);
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref inSignal, "inSignal");
		Scribe_Values.Look(ref addToLookTargets, "addToLookTargets", defaultValue: false);
		Scribe_Values.Look(ref count, "count", null);
		Scribe_Defs.Look(ref def, "def");
		Scribe_References.Look(ref pawn, "pawn");
		Scribe_References.Look(ref otherPawn, "otherPawn");
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		def = ThoughtDefOf.DecreeMet ?? ThoughtDefOf.DebugGood;
		pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault();
	}

	public override void ReplacePawnReferences(Pawn replace, Pawn with)
	{
		if (pawn == replace)
		{
			pawn = with;
		}
		if (otherPawn == replace)
		{
			otherPawn = with;
		}
	}
}
