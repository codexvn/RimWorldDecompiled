using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

[Obsolete("Betray visitors is now a subquest of refugee quest.")]
public class ChoiceLetter_BetrayVisitors : ChoiceLetter
{
	[CompilerGenerated]
	private sealed class _003Cget_Choices_003Ed__8 : IEnumerable<DiaOption>, IEnumerable, IEnumerator<DiaOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DiaOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ChoiceLetter_BetrayVisitors _003C_003E4__this;

		DiaOption IEnumerator<DiaOption>.Current
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
		public _003Cget_Choices_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			ChoiceLetter_BetrayVisitors choiceLetter_BetrayVisitors = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (choiceLetter_BetrayVisitors.lookTargets.IsValid())
				{
					_003C_003E2__current = choiceLetter_BetrayVisitors.Option_JumpToLocationAndPostpone;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0056;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0056;
			case 2:
				_003C_003E1__state = -1;
				goto IL_008d;
			case 3:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_0056:
				if (choiceLetter_BetrayVisitors.quest != null && !choiceLetter_BetrayVisitors.quest.hidden)
				{
					_003C_003E2__current = choiceLetter_BetrayVisitors.Option_ViewInQuestsTab("ViewRelatedQuest", postpone: true);
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_008d;
				IL_008d:
				_003C_003E2__current = choiceLetter_BetrayVisitors.Option_Close;
				_003C_003E1__state = 3;
				return true;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<DiaOption> IEnumerable<DiaOption>.GetEnumerator()
		{
			_003Cget_Choices_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Choices_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<DiaOption>)this).GetEnumerator();
		}
	}

	public List<Pawn> pawns = new List<Pawn>();

	public Pawn asker;

	public bool requiresAliveAsker;

	public override bool CanDismissWithRightClick => false;

	public override bool CanShowInLetterStack
	{
		get
		{
			if (!base.CanShowInLetterStack)
			{
				return false;
			}
			if (quest == null || quest.State != QuestState.Ongoing)
			{
				return false;
			}
			if (requiresAliveAsker && (asker == null || asker.Dead))
			{
				return false;
			}
			for (int i = 0; i < pawns.Count; i++)
			{
				if (!pawns[i].Spawned && !pawns[i].Destroyed)
				{
					return false;
				}
			}
			return true;
		}
	}

	public override IEnumerable<DiaOption> Choices
	{
		[IteratorStateMachine(typeof(_003Cget_Choices_003Ed__8))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Choices_003Ed__8(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref pawns, "pawns", LookMode.Reference);
		Scribe_References.Look(ref asker, "asker");
		Scribe_Values.Look(ref requiresAliveAsker, "requiresAliveAsker", defaultValue: false);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			pawns.RemoveAll((Pawn x) => x == null);
		}
	}
}
