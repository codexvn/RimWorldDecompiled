using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ChoiceLetter_BabyToChild : ChoiceLetter
{
	[CompilerGenerated]
	private sealed class _003Cget_Choices_003Ed__8 : IEnumerable<DiaOption>, IEnumerable, IEnumerator<DiaOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DiaOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ChoiceLetter_BabyToChild _003C_003E4__this;

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
			ChoiceLetter_BabyToChild choiceLetter_BabyToChild = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!choiceLetter_BabyToChild.ArchivedOnly)
				{
					Faction faction = choiceLetter_BabyToChild.pawn.Faction;
					if (faction != null && faction.IsPlayer)
					{
						_003C_003E2__current = new DiaOption(choiceLetter_BabyToChild.ChoseColonistLabel)
						{
							action = choiceLetter_BabyToChild.ChoseColonist,
							disabled = (choiceLetter_BabyToChild.bornSlave != choiceLetter_BabyToChild.pawn.IsSlave),
							disabledReason = "CannotChangeChildStatusReason".Translate(choiceLetter_BabyToChild.pawn),
							resolveTree = true
						};
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0156;
				}
				_003C_003E2__current = choiceLetter_BabyToChild.Option_Close;
				_003C_003E1__state = 5;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new DiaOption(choiceLetter_BabyToChild.ChoseSlaveLabel)
				{
					action = choiceLetter_BabyToChild.ChoseSlave,
					disabled = (choiceLetter_BabyToChild.bornSlave != choiceLetter_BabyToChild.pawn.IsSlave),
					disabledReason = "CannotChangeChildStatusReason".Translate(choiceLetter_BabyToChild.pawn),
					resolveTree = true
				};
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0156;
			case 3:
				_003C_003E1__state = -1;
				goto IL_01c1;
			case 4:
				_003C_003E1__state = -1;
				goto IL_01c1;
			case 5:
				_003C_003E1__state = -1;
				goto IL_01c1;
			case 6:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_01c1:
				_003C_003E2__current = choiceLetter_BabyToChild.Option_JumpToLocationAndPostpone;
				_003C_003E1__state = 6;
				return true;
				IL_0156:
				if (choiceLetter_BabyToChild.bornSlave != choiceLetter_BabyToChild.pawn.IsSlave)
				{
					_003C_003E2__current = choiceLetter_BabyToChild.Option_Close;
					_003C_003E1__state = 3;
					return true;
				}
				_003C_003E2__current = choiceLetter_BabyToChild.Option_Postpone;
				_003C_003E1__state = 4;
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

	private const int TimeoutTicks = 30000;

	private Pawn pawn;

	private bool bornSlave;

	private TaggedString ChoseColonistLabel;

	private TaggedString ChoseSlaveLabel;

	public override bool CanShowInLetterStack => pawn.Faction?.IsPlayer ?? false;

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

	public void Start()
	{
		StartTimeout(30000);
		pawn = lookTargets.TryGetPrimaryTarget().Thing as Pawn;
		bornSlave = pawn.IsSlave;
		if (bornSlave)
		{
			ChoseColonistLabel = "Emancipate".Translate().CapitalizeFirst();
			ChoseSlaveLabel = "RemainX".Translate(pawn.LegalStatus).CapitalizeFirst();
		}
		else
		{
			ChoseColonistLabel = "RemainX".Translate(pawn.LegalStatus).CapitalizeFirst();
			ChoseSlaveLabel = "Enslave".Translate().CapitalizeFirst();
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref bornSlave, "bornSlave", defaultValue: false);
		Scribe_Values.Look(ref ChoseColonistLabel, "ChoseColonistLabel");
		Scribe_Values.Look(ref ChoseSlaveLabel, "ChoseSlaveLabel");
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			pawn = lookTargets.TryGetPrimaryTarget().Thing as Pawn;
		}
	}

	private void ChoseColonist()
	{
		if (bornSlave)
		{
			GenGuest.SlaveRelease(pawn);
		}
		Find.LetterStack.RemoveLetter(this);
	}

	private void ChoseSlave()
	{
		if (!bornSlave)
		{
			pawn.guest.SetGuestStatus(pawn.Faction, GuestStatus.Slave);
		}
		Find.LetterStack.RemoveLetter(this);
	}
}
