using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public class PsychicRitualDef_Brainwipe : PsychicRitualDef_InvocationCircle
{
	[CompilerGenerated]
	private sealed class _003CGetPawnTooltipExtras_003Ed__3 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		string IEnumerator<string>.Current
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
		public _003CGetPawnTooltipExtras_003Ed__3(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (pawn.guest != null && !pawn.guest.Recruitable)
				{
					_003C_003E2__current = "NonRecruitable".Translate();
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGetPawnTooltipExtras_003Ed__3 _003CGetPawnTooltipExtras_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetPawnTooltipExtras_003Ed__ = this;
			}
			else
			{
				_003CGetPawnTooltipExtras_003Ed__ = new _003CGetPawnTooltipExtras_003Ed__3(0);
			}
			_003CGetPawnTooltipExtras_003Ed__.pawn = _003C_003E3__pawn;
			return _003CGetPawnTooltipExtras_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public SimpleCurve comaDurationDaysFromQualityCurve;

	public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
	{
		List<PsychicRitualToil> list = base.CreateToils(psychicRitual, parent);
		list.Add(new PsychicRitualToil_Brainwipe(InvokerRole, TargetRole));
		list.Add(new PsychicRitualToil_TargetCleanup(InvokerRole, TargetRole));
		return list;
	}

	public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
	{
		return outcomeDescription.Formatted(Mathf.FloorToInt(comaDurationDaysFromQualityCurve.Evaluate(qualityRange.min) * 60000f).ToStringTicksToDays());
	}

	[IteratorStateMachine(typeof(_003CGetPawnTooltipExtras_003Ed__3))]
	public override IEnumerable<string> GetPawnTooltipExtras(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPawnTooltipExtras_003Ed__3(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}
}
