using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Verse;

namespace RimWorld;

public class ChoiceLetter_GameEnded : ChoiceLetter
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003C_003Ec__DisplayClass3_0
	{
		public float hoursRemaining;
	}

	[CompilerGenerated]
	private sealed class _003Cget_Choices_003Ed__3 : IEnumerable<DiaOption>, IEnumerable, IEnumerator<DiaOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DiaOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ChoiceLetter_GameEnded _003C_003E4__this;

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
		public _003Cget_Choices_003Ed__3(int _003C_003E1__state)
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
			ChoiceLetter_GameEnded choiceLetter_GameEnded = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (choiceLetter_GameEnded.ArchivedOnly)
				{
					_003C_003E2__current = choiceLetter_GameEnded.Option_Close;
					_003C_003E1__state = 1;
					return true;
				}
				if (!Find.GameEnder.CanSpawnNewWanderers())
				{
					_003C_003E2__current = new DiaOption("GameOverKeepWatching".Translate())
					{
						resolveTree = true
					};
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E2__current = new DiaOption("GameOverKeepWatchingForNow".Translate())
				{
					resolveTree = true
				};
				_003C_003E1__state = 3;
				return true;
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0171;
			case 3:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass3_0 _003C_003Ec__DisplayClass3_ = default(_003C_003Ec__DisplayClass3_0);
				_003C_003Ec__DisplayClass3_.hoursRemaining = (float)(20000 - (GenTicks.TicksGame - choiceLetter_GameEnded.arrivalTick)) / 2500f;
				_003C_003E2__current = new DiaOption("GameOverCreateNewWanderers".Translate())
				{
					action = delegate
					{
						Find.WindowStack.Add(new Dialog_ChooseNewWanderers());
					},
					resolveTree = true,
					disabled = !_003Cget_Choices_003Eg__CanCreateNewWanderers_007C3_0(ref _003C_003Ec__DisplayClass3_),
					disabledReason = _003Cget_Choices_003Eg__CanCreateNewWanderers_007C3_0(ref _003C_003Ec__DisplayClass3_).Reason
				};
				_003C_003E1__state = 4;
				return true;
			}
			case 4:
				_003C_003E1__state = -1;
				goto IL_0171;
			case 5:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0171:
				_003C_003E2__current = new DiaOption("GameOverMainMenu".Translate())
				{
					action = GenScene.GoToMainMenu,
					resolveTree = true
				};
				_003C_003E1__state = 5;
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
			_003Cget_Choices_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Choices_003Ed__3(0)
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

	public override bool CanDismissWithRightClick => false;

	public override IEnumerable<DiaOption> Choices
	{
		[IteratorStateMachine(typeof(_003Cget_Choices_003Ed__3))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Choices_003Ed__3(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	[CompilerGenerated]
	internal static AcceptanceReport _003Cget_Choices_003Eg__CanCreateNewWanderers_007C3_0(ref _003C_003Ec__DisplayClass3_0 P_0)
	{
		if (P_0.hoursRemaining > 0f)
		{
			return "GameOverCreateNewWanderersWait".Translate(Math.Ceiling(P_0.hoursRemaining));
		}
		bool flag = false;
		if (Current.Game == null)
		{
			return false;
		}
		foreach (Map playerHomeMap in Current.Game.PlayerHomeMaps)
		{
			if (playerHomeMap.Tile.Layer.IsRootSurface)
			{
				return true;
			}
			flag = true;
		}
		if (flag)
		{
			return "NoWandererDestination".Translate();
		}
		return "NoColony".Translate();
	}
}
