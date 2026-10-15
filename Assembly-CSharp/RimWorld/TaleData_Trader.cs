using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class TaleData_Trader : TaleData
{
	[CompilerGenerated]
	private sealed class _003CGetRules_003Ed__6 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TaleData_Trader _003C_003E4__this;

		private string prefix;

		public string _003C_003E3__prefix;

		private string _003CnameShortIndefinite_003E5__2;

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
		public _003CGetRules_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CnameShortIndefinite_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			TaleData_Trader taleData_Trader = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(output: (!taleData_Trader.IsPawn) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(taleData_Trader.name) : taleData_Trader.name, keyword: prefix + "_nameFull");
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (taleData_Trader.IsPawn)
				{
					_003CnameShortIndefinite_003E5__2 = taleData_Trader.name;
				}
				else
				{
					_003CnameShortIndefinite_003E5__2 = Find.ActiveLanguageWorker.WithIndefiniteArticle(taleData_Trader.name);
				}
				_003C_003E2__current = new Rule_String(prefix + "_indefinite", _003CnameShortIndefinite_003E5__2);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "_nameIndef", _003CnameShortIndefinite_003E5__2);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003CnameShortIndefinite_003E5__2 = null;
				if (taleData_Trader.IsPawn)
				{
					_003CnameShortIndefinite_003E5__2 = taleData_Trader.name;
				}
				else
				{
					_003CnameShortIndefinite_003E5__2 = Find.ActiveLanguageWorker.WithDefiniteArticle(taleData_Trader.name);
				}
				_003C_003E2__current = new Rule_String(prefix + "_definite", _003CnameShortIndefinite_003E5__2);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "_nameDef", _003CnameShortIndefinite_003E5__2);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003CnameShortIndefinite_003E5__2 = null;
				_003C_003E2__current = new Rule_String(prefix + "_pronoun", taleData_Trader.gender.GetPronoun());
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "_possessive", taleData_Trader.gender.GetPossessive());
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				return false;
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
		IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
		{
			_003CGetRules_003Ed__6 _003CGetRules_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetRules_003Ed__ = this;
			}
			else
			{
				_003CGetRules_003Ed__ = new _003CGetRules_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetRules_003Ed__.prefix = _003C_003E3__prefix;
			return _003CGetRules_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	public string name;

	public int pawnID = -1;

	public Gender gender = Gender.Male;

	private bool IsPawn => pawnID >= 0;

	public override void ExposeData()
	{
		Scribe_Values.Look(ref name, "name");
		Scribe_Values.Look(ref pawnID, "pawnID", -1);
		Scribe_Values.Look(ref gender, "gender", Gender.Male);
	}

	[IteratorStateMachine(typeof(_003CGetRules_003Ed__6))]
	public override IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetRules_003Ed__6(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__prefix = prefix
		};
	}

	public static TaleData_Trader GenerateFrom(ITrader trader)
	{
		TaleData_Trader taleData_Trader = new TaleData_Trader();
		taleData_Trader.name = trader.TraderName;
		if (trader is Pawn pawn)
		{
			taleData_Trader.pawnID = pawn.thingIDNumber;
			taleData_Trader.gender = pawn.gender;
		}
		return taleData_Trader;
	}

	public static TaleData_Trader GenerateRandom()
	{
		PawnKindDef pawnKindDef = DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef d) => d.trader).RandomElement();
		Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionDef), null);
		pawn.mindState.wantsToTradeWithColony = true;
		PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, actAsIfSpawned: true);
		return GenerateFrom(pawn);
	}
}
