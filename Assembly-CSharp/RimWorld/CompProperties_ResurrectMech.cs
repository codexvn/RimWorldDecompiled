using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompProperties_ResurrectMech : CompProperties_AbilityEffect
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__5 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_ResurrectMech _003C_003E4__this;

		private AbilityDef parentDef;

		public AbilityDef _003C_003E3__parentDef;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<MechChargeCosts>.Enumerator _003C_003E7__wrap2;

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
		public _003CConfigErrors_003Ed__5(int _003C_003E1__state)
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
			case 3:
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
			_003C_003E7__wrap2 = default(List<MechChargeCosts>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompProperties_ResurrectMech compProperties_ResurrectMech = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((AbilityCompProperties)compProperties_ResurrectMech).ConfigErrors(parentDef).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007c;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007c;
				case 2:
					_003C_003E1__state = -1;
					break;
				case 3:
					{
						_003C_003E1__state = -4;
						goto IL_013d;
					}
					IL_007c:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (compProperties_ResurrectMech.costs.NullOrEmpty())
					{
						_003C_003E2__current = "costs list is null";
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E7__wrap2 = compProperties_ResurrectMech.costs.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_013d;
					IL_013d:
					while (_003C_003E7__wrap2.MoveNext())
					{
						MechChargeCosts current2 = _003C_003E7__wrap2.Current;
						if (current2.weightClass == null)
						{
							_003C_003E2__current = $"costs list contains null weight class with cost {current2.cost}";
							_003C_003E1__state = 3;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<MechChargeCosts>.Enumerator);
					break;
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__5 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.parentDef = _003C_003E3__parentDef;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public int maxCorpseAgeTicks = int.MaxValue;

	public List<MechChargeCosts> costs = new List<MechChargeCosts>();

	public EffecterDef appliedEffecterDef;

	public EffecterDef centerEffecterDef;

	public CompProperties_ResurrectMech()
	{
		compClass = typeof(CompAbilityEffect_ResurrectMech);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__5))]
	public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__5(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}
}
