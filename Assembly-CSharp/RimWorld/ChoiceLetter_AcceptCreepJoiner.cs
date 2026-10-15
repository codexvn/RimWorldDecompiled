using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ChoiceLetter_AcceptCreepJoiner : ChoiceLetter
{
	[CompilerGenerated]
	private sealed class _003Cget_Choices_003Ed__10 : IEnumerable<DiaOption>, IEnumerable, IEnumerator<DiaOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DiaOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ChoiceLetter_AcceptCreepJoiner _003C_003E4__this;

		private DiaOption _003CoptionCapture_003E5__2;

		private DiaOption _003CoptionReject_003E5__3;

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
		public _003Cget_Choices_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CoptionCapture_003E5__2 = null;
			_003CoptionReject_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			ChoiceLetter_AcceptCreepJoiner CS_0024_003C_003E8__locals23 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals23.ArchivedOnly || !CS_0024_003C_003E8__locals23.pawn.Spawned)
				{
					_003C_003E2__current = CS_0024_003C_003E8__locals23.Option_Close;
					_003C_003E1__state = 1;
					return true;
				}
				DiaOption diaOption = new DiaOption("AcceptCreeper".Translate(CS_0024_003C_003E8__locals23.pawn.Named("PAWN"), CS_0024_003C_003E8__locals23.speaker.Named("SPEAKER")));
				_003CoptionCapture_003E5__2 = new DiaOption("CaptureCreeper".Translate(CS_0024_003C_003E8__locals23.pawn.Named("PAWN"), CS_0024_003C_003E8__locals23.speaker.Named("SPEAKER")));
				_003CoptionReject_003E5__3 = new DiaOption("RejectCreeper".Translate(CS_0024_003C_003E8__locals23.pawn.Named("PAWN"), CS_0024_003C_003E8__locals23.speaker.Named("SPEAKER")));
				diaOption.action = delegate
				{
					if (CS_0024_003C_003E8__locals23.pawn.Spawned)
					{
						Find.SignalManager.SendSignal(new Signal(CS_0024_003C_003E8__locals23.signalAccept));
						Find.LetterStack.RemoveLetter(CS_0024_003C_003E8__locals23);
					}
				};
				diaOption.resolveTree = true;
				_003CoptionCapture_003E5__2.action = delegate
				{
					if (CS_0024_003C_003E8__locals23.pawn.Spawned)
					{
						Find.SignalManager.SendSignal(new Signal(CS_0024_003C_003E8__locals23.signalCapture));
					}
				};
				_003CoptionCapture_003E5__2.resolveTree = true;
				if (CS_0024_003C_003E8__locals23.pawn.Spawned && !CaptureUtility.CanArrest(CS_0024_003C_003E8__locals23.speaker, CS_0024_003C_003E8__locals23.pawn, out var reason))
				{
					_003CoptionCapture_003E5__2.Disable(reason);
				}
				_003CoptionReject_003E5__3.action = delegate
				{
					if (CS_0024_003C_003E8__locals23.pawn.Spawned)
					{
						Find.SignalManager.SendSignal(new Signal(CS_0024_003C_003E8__locals23.signalReject));
						Find.LetterStack.RemoveLetter(CS_0024_003C_003E8__locals23);
					}
				};
				_003CoptionReject_003E5__3.resolveTree = true;
				_003C_003E2__current = diaOption;
				_003C_003E1__state = 2;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CoptionCapture_003E5__2;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CoptionReject_003E5__3;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals23.lookTargets.IsValid())
				{
					_003C_003E2__current = CS_0024_003C_003E8__locals23.Option_JumpToLocationAndPostpone;
					_003C_003E1__state = 5;
					return true;
				}
				goto IL_021d;
			case 5:
				_003C_003E1__state = -1;
				goto IL_021d;
			case 6:
				{
					_003C_003E1__state = -1;
					_003CoptionCapture_003E5__2 = null;
					_003CoptionReject_003E5__3 = null;
					break;
				}
				IL_021d:
				_003C_003E2__current = CS_0024_003C_003E8__locals23.Option_Postpone;
				_003C_003E1__state = 6;
				return true;
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
			_003Cget_Choices_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Choices_003Ed__10(0)
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

	public string signalAccept;

	public string signalCapture;

	public string signalReject;

	public Pawn pawn;

	public Pawn speaker;

	public override bool CanDismissWithRightClick => false;

	public override bool CanShowInLetterStack
	{
		get
		{
			if (base.CanShowInLetterStack && quest != null)
			{
				if (quest.State != QuestState.Ongoing)
				{
					return quest.State == QuestState.NotYetAccepted;
				}
				return true;
			}
			return false;
		}
	}

	public override IEnumerable<DiaOption> Choices
	{
		[IteratorStateMachine(typeof(_003Cget_Choices_003Ed__10))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Choices_003Ed__10(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref signalAccept, "signalAccept");
		Scribe_Values.Look(ref signalCapture, "signalCapture");
		Scribe_Values.Look(ref signalReject, "signalReject");
		Scribe_References.Look(ref pawn, "pawn");
		Scribe_References.Look(ref speaker, "speaker");
	}
}
