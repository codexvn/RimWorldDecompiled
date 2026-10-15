using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public class NewQuestLetter : ChoiceLetter
{
	[CompilerGenerated]
	private sealed class _003Cget_Choices_003Ed__1 : IEnumerable<DiaOption>, IEnumerable, IEnumerator<DiaOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DiaOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public NewQuestLetter _003C_003E4__this;

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
		public _003Cget_Choices_003Ed__1(int _003C_003E1__state)
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
			NewQuestLetter newQuestLetter = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (newQuestLetter.quest != null && !newQuestLetter.quest.hidden)
				{
					_003C_003E2__current = newQuestLetter.Option_ViewInQuestsTab("ViewQuest");
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0064;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0064;
			case 2:
				_003C_003E1__state = -1;
				goto IL_008d;
			case 3:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_008d:
				_003C_003E2__current = newQuestLetter.Option_Close;
				_003C_003E1__state = 3;
				return true;
				IL_0064:
				if (newQuestLetter.lookTargets.IsValid())
				{
					_003C_003E2__current = newQuestLetter.Option_JumpToLocation;
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_008d;
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
			_003Cget_Choices_003Ed__1 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Choices_003Ed__1(0)
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

	public override IEnumerable<DiaOption> Choices
	{
		[IteratorStateMachine(typeof(_003Cget_Choices_003Ed__1))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Choices_003Ed__1(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void OpenLetter()
	{
		if (quest != null && !base.ArchivedOnly)
		{
			Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests);
			((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(quest);
			Find.LetterStack.RemoveLetter(this);
		}
		else
		{
			base.OpenLetter();
		}
	}
}
