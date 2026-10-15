using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class Tale_DoublePawn : Tale
{
	[CompilerGenerated]
	private sealed class _003CSpecialTextGenerationRules_003Ed__11 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Tale_DoublePawn _003C_003E4__this;

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
		public _003CSpecialTextGenerationRules_003Ed__11(int _003C_003E1__state)
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
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -6:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally4();
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
				Tale_DoublePawn tale_DoublePawn = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (tale_DoublePawn.def.firstPawnSymbol.NullOrEmpty() || tale_DoublePawn.def.secondPawnSymbol.NullOrEmpty())
					{
						Log.Error(tale_DoublePawn.def?.ToString() + " uses DoublePawn tale class but firstPawnSymbol and secondPawnSymbol are not both set");
					}
					_003C_003E7__wrap1 = tale_DoublePawn.firstPawnData.GetRules("ANYPAWN").GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00ca;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00ca;
				case 2:
					_003C_003E1__state = -4;
					goto IL_0140;
				case 3:
					_003C_003E1__state = -5;
					goto IL_01b6;
				case 4:
					{
						_003C_003E1__state = -6;
						goto IL_0229;
					}
					IL_01b6:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Rule current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = tale_DoublePawn.secondPawnData.GetRules(tale_DoublePawn.def.secondPawnSymbol, outConstants).GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_0229;
					IL_00ca:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Rule current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = tale_DoublePawn.firstPawnData.GetRules(tale_DoublePawn.def.firstPawnSymbol, outConstants).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0140;
					IL_0229:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Rule current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap1 = null;
					break;
					IL_0140:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Rule current4 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					if (tale_DoublePawn.secondPawnData == null)
					{
						break;
					}
					_003C_003E7__wrap1 = tale_DoublePawn.firstPawnData.GetRules("ANYPAWN").GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_01b6;
				}
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

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
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
			_003CSpecialTextGenerationRules_003Ed__11 _003CSpecialTextGenerationRules_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialTextGenerationRules_003Ed__ = this;
			}
			else
			{
				_003CSpecialTextGenerationRules_003Ed__ = new _003CSpecialTextGenerationRules_003Ed__11(0)
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

	public TaleData_Pawn firstPawnData;

	public TaleData_Pawn secondPawnData;

	public override Pawn DominantPawn => firstPawnData.pawn;

	public override string ShortSummary
	{
		get
		{
			string text = string.Concat(def.LabelCap + ": ", firstPawnData.name?.ToString());
			if (secondPawnData != null)
			{
				text = text + ", " + secondPawnData.name;
			}
			return text;
		}
	}

	public Tale_DoublePawn()
	{
	}

	public Tale_DoublePawn(Pawn firstPawn, Pawn secondPawn)
	{
		firstPawnData = TaleData_Pawn.GenerateFrom(firstPawn);
		if (secondPawn != null)
		{
			secondPawnData = TaleData_Pawn.GenerateFrom(secondPawn);
		}
		if (firstPawn.SpawnedOrAnyParentSpawned)
		{
			surroundings = TaleData_Surroundings.GenerateFrom(firstPawn.PositionHeld, firstPawn.MapHeld);
		}
	}

	public override bool Concerns(Thing th)
	{
		if (secondPawnData != null && secondPawnData.pawn == th)
		{
			return true;
		}
		if (!base.Concerns(th))
		{
			return firstPawnData.pawn == th;
		}
		return true;
	}

	public override void Notify_FactionRemoved(Faction faction)
	{
		if (firstPawnData.faction == faction)
		{
			firstPawnData.faction = null;
		}
		if (secondPawnData != null && secondPawnData.faction == faction)
		{
			secondPawnData.faction = null;
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Deep.Look(ref firstPawnData, "firstPawnData");
		Scribe_Deep.Look(ref secondPawnData, "secondPawnData");
	}

	[IteratorStateMachine(typeof(_003CSpecialTextGenerationRules_003Ed__11))]
	protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialTextGenerationRules_003Ed__11(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__outConstants = outConstants
		};
	}

	public override void GenerateTestData()
	{
		base.GenerateTestData();
		firstPawnData = TaleData_Pawn.GenerateRandom();
		secondPawnData = TaleData_Pawn.GenerateRandom();
	}
}
