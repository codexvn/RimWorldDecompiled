using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ChoiceLetter_ChoosePawn : ChoiceLetter
{
	[CompilerGenerated]
	private sealed class _003Cget_Choices_003Ed__7 : IEnumerable<DiaOption>, IEnumerable, IEnumerator<DiaOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DiaOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ChoiceLetter_ChoosePawn _003C_003E4__this;

		private int _003Ci_003E5__2;

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
		public _003Cget_Choices_003Ed__7(int _003C_003E1__state)
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
			ChoiceLetter_ChoosePawn choiceLetter_ChoosePawn = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!choiceLetter_ChoosePawn.ArchivedOnly)
				{
					_003Ci_003E5__2 = 0;
					goto IL_009e;
				}
				_003C_003E2__current = choiceLetter_ChoosePawn.Option_Close;
				_003C_003E1__state = 3;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_008e;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00eb;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00eb;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0114;
			case 5:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00eb:
				if (choiceLetter_ChoosePawn.lookTargets.IsValid())
				{
					_003C_003E2__current = choiceLetter_ChoosePawn.Option_JumpToLocationAndPostpone;
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_0114;
				IL_0114:
				if (choiceLetter_ChoosePawn.quest != null && !choiceLetter_ChoosePawn.quest.hidden)
				{
					_003C_003E2__current = choiceLetter_ChoosePawn.Option_ViewInQuestsTab("ViewRelatedQuest", postpone: true);
					_003C_003E1__state = 5;
					return true;
				}
				break;
				IL_009e:
				if (_003Ci_003E5__2 < choiceLetter_ChoosePawn.pawns.Count)
				{
					if (!choiceLetter_ChoosePawn.pawns[_003Ci_003E5__2].DestroyedOrNull())
					{
						_003C_003E2__current = choiceLetter_ChoosePawn.Option_ChoosePawn(choiceLetter_ChoosePawn.pawns[_003Ci_003E5__2]);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_008e;
				}
				_003C_003E2__current = choiceLetter_ChoosePawn.Option_Postpone;
				_003C_003E1__state = 2;
				return true;
				IL_008e:
				_003Ci_003E5__2++;
				goto IL_009e;
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
			_003Cget_Choices_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Choices_003Ed__7(0)
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

	public string chosenPawnSignal;

	public override bool CanDismissWithRightClick => false;

	public override bool CanShowInLetterStack
	{
		get
		{
			if (!base.CanShowInLetterStack)
			{
				return false;
			}
			if (chosenPawnSignal.NullOrEmpty())
			{
				return false;
			}
			bool result = false;
			for (int i = 0; i < pawns.Count; i++)
			{
				if (!pawns[i].DestroyedOrNull())
				{
					result = true;
					break;
				}
			}
			return result;
		}
	}

	public override IEnumerable<DiaOption> Choices
	{
		[IteratorStateMachine(typeof(_003Cget_Choices_003Ed__7))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Choices_003Ed__7(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	private DiaOption Option_ChoosePawn(Pawn p)
	{
		return new DiaOption(p.LabelCap)
		{
			action = delegate
			{
				if (!chosenPawnSignal.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(chosenPawnSignal, p.Named("CHOSEN")));
				}
				Find.LetterStack.RemoveLetter(this);
			},
			resolveTree = true
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref pawns, "pawns", LookMode.Reference);
		Scribe_Values.Look(ref chosenPawnSignal, "chosenPawnSignal");
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			pawns.RemoveAll((Pawn x) => x == null);
		}
	}
}
