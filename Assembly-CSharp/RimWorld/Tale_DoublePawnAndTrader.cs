using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class Tale_DoublePawnAndTrader : Tale_DoublePawn
{
	[CompilerGenerated]
	private sealed class _003CSpecialTextGenerationRules_003Ed__5 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Tale_DoublePawnAndTrader _003C_003E4__this;

		private Dictionary<string, string> outConstants;

		public Dictionary<string, string> _003C_003E3__outConstants;

		private IEnumerator<Rule> _003C_003E7__wrap1;

		Rule IEnumerator<Rule>.Current
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
		public _003CSpecialTextGenerationRules_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Tale_DoublePawnAndTrader tale_DoublePawnAndTrader = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Tale_DoublePawn)tale_DoublePawnAndTrader).SpecialTextGenerationRules(outConstants).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0078:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Rule current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = tale_DoublePawnAndTrader.traderData.GetRules("TRADER").GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Rule current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap1 = null;
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

		private void _003C_003Em__Finally2()
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
		IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
		{
			_003CSpecialTextGenerationRules_003Ed__5 _003CSpecialTextGenerationRules_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialTextGenerationRules_003Ed__ = this;
			}
			else
			{
				_003CSpecialTextGenerationRules_003Ed__ = new _003CSpecialTextGenerationRules_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CSpecialTextGenerationRules_003Ed__.outConstants = _003C_003E3__outConstants;
			return _003CSpecialTextGenerationRules_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	public TaleData_Trader traderData;

	public Tale_DoublePawnAndTrader()
	{
	}

	public Tale_DoublePawnAndTrader(Pawn firstPawn, Pawn secondPawn, ITrader trader)
		: base(firstPawn, secondPawn)
	{
		traderData = TaleData_Trader.GenerateFrom(trader);
	}

	public override bool Concerns(Thing th)
	{
		if (!base.Concerns(th))
		{
			return traderData.pawnID == th.thingIDNumber;
		}
		return true;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Deep.Look(ref traderData, "traderData");
	}

	[IteratorStateMachine(typeof(_003CSpecialTextGenerationRules_003Ed__5))]
	protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialTextGenerationRules_003Ed__5(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__outConstants = outConstants
		};
	}

	public override void GenerateTestData()
	{
		base.GenerateTestData();
		traderData = TaleData_Trader.GenerateRandom();
	}
}
