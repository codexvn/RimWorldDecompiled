using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class IngestionOutcomeDoer_GiveHediff : IngestionOutcomeDoer
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__6 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingDef parentDef;

		public ThingDef _003C_003E3__parentDef;

		public IngestionOutcomeDoer_GiveHediff _003C_003E4__this;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__6(int _003C_003E1__state)
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
				IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoer_GiveHediff = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
				}
				else
				{
					_003C_003E1__state = -1;
					if (!parentDef.IsDrug || !(ingestionOutcomeDoer_GiveHediff.chance >= 1f))
					{
						goto IL_00a2;
					}
					_003C_003E7__wrap1 = ingestionOutcomeDoer_GiveHediff.hediffDef.SpecialDisplayStats(StatRequest.ForEmpty()).GetEnumerator();
					_003C_003E1__state = -3;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					StatDrawEntry current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				goto IL_00a2;
				IL_00a2:
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__6 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CSpecialDisplayStats_003Ed__.parentDef = _003C_003E3__parentDef;
			return _003CSpecialDisplayStats_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	public HediffDef hediffDef;

	public float severity = -1f;

	public ChemicalDef toleranceChemical;

	private bool divideByBodySize;

	public bool multiplyByGeneToleranceFactors;

	protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
	{
		Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn);
		float effect = ((!(severity > 0f)) ? hediffDef.initialSeverity : severity);
		AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(pawn, toleranceChemical, ref effect, multiplyByGeneToleranceFactors, divideByBodySize);
		hediff.Severity = effect;
		pawn.health.AddHediff(hediff, null, null);
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__6))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__6(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}
}
