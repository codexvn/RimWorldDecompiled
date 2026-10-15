using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse;

public class StandardLetter : ChoiceLetter
{
	[CompilerGenerated]
	private sealed class _003Cget_Choices_003Ed__1 : IEnumerable<DiaOption>, IEnumerable, IEnumerator<DiaOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DiaOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StandardLetter _003C_003E4__this;

		private int _003Ci_003E5__2;

		private int _003Ci_003E5__3;

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
			StandardLetter standardLetter = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = standardLetter.Option_Close;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (standardLetter.lookTargets.IsValid())
				{
					_003C_003E2__current = standardLetter.Option_JumpToLocation;
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_007a;
			case 2:
				_003C_003E1__state = -1;
				goto IL_007a;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00b1;
			case 4:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				goto IL_00f9;
			case 5:
				{
					_003C_003E1__state = -1;
					_003Ci_003E5__3++;
					goto IL_0177;
				}
				IL_007a:
				if (standardLetter.quest != null && !standardLetter.quest.hidden)
				{
					_003C_003E2__current = standardLetter.Option_ViewInQuestsTab();
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_00b1;
				IL_010c:
				if (standardLetter.hyperlinkHediffDefs.NullOrEmpty())
				{
					break;
				}
				_003Ci_003E5__2 = ((standardLetter.hyperlinkThingDefs != null) ? standardLetter.hyperlinkThingDefs.Count : 0);
				_003Ci_003E5__3 = 0;
				goto IL_0177;
				IL_0177:
				if (_003Ci_003E5__3 < standardLetter.hyperlinkHediffDefs.Count)
				{
					_003C_003E2__current = standardLetter.Option_ViewInfoCard(_003Ci_003E5__2 + _003Ci_003E5__3);
					_003C_003E1__state = 5;
					return true;
				}
				break;
				IL_00b1:
				if (!standardLetter.hyperlinkThingDefs.NullOrEmpty())
				{
					_003Ci_003E5__2 = 0;
					goto IL_00f9;
				}
				goto IL_010c;
				IL_00f9:
				if (_003Ci_003E5__2 < standardLetter.hyperlinkThingDefs.Count)
				{
					_003C_003E2__current = standardLetter.Option_ViewInfoCard(_003Ci_003E5__2);
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_010c;
			}
			return false;
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
}
