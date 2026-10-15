using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompProperties_Stunnable : CompProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__7 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_Stunnable _003C_003E4__this;

		private ThingDef parentDef;

		public ThingDef _003C_003E3__parentDef;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<DamageDef>.Enumerator _003C_003E7__wrap2;

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
		public _003CConfigErrors_003Ed__7(int _003C_003E1__state)
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
			case -5:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<DamageDef>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompProperties_Stunnable compProperties_Stunnable = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompProperties)compProperties_Stunnable).ConfigErrors(parentDef).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0080;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0080;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00c7;
				case 3:
					_003C_003E1__state = -4;
					goto IL_012a;
				case 4:
					{
						_003C_003E1__state = -5;
						break;
					}
					IL_0080:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (compProperties_Stunnable.affectedDamageDefs.Count == 0)
					{
						_003C_003E2__current = "CompProperties_Stunnable requires at least one affectedDamageDef";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00c7;
					IL_012a:
					while (_003C_003E7__wrap2.MoveNext())
					{
						DamageDef current2 = _003C_003E7__wrap2.Current;
						if (!current2.causeStun)
						{
							_003C_003E2__current = "CompProperties_Stunnable: " + current2.defName + " does not cause stun";
							_003C_003E1__state = 3;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<DamageDef>.Enumerator);
					_003C_003E7__wrap2 = compProperties_Stunnable.adaptableDamageDefs.GetEnumerator();
					_003C_003E1__state = -5;
					break;
					IL_00c7:
					_003C_003E7__wrap2 = compProperties_Stunnable.affectedDamageDefs.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_012a;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					DamageDef current3 = _003C_003E7__wrap2.Current;
					if (current3.stunAdaptationTicks <= 0)
					{
						_003C_003E2__current = "CompProperties_Stunnable: " + current3.defName + " does not have stunAdaptationTicks set";
						_003C_003E1__state = 4;
						return true;
					}
				}
				_003C_003Em__Finally3();
				_003C_003E7__wrap2 = default(List<DamageDef>.Enumerator);
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

		private void _003C_003Em__Finally3()
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
			_003CConfigErrors_003Ed__7 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__7(0)
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

	public List<DamageDef> affectedDamageDefs = new List<DamageDef>();

	public List<DamageDef> adaptableDamageDefs = new List<DamageDef>();

	public bool useLargeEMPEffecter;

	public Vector3? empEffecterDimensions;

	public Vector3? empEffecterOffset;

	public float? empChancePerTick;

	public CompProperties_Stunnable()
	{
		compClass = typeof(CompStunnable);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__7))]
	public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__7(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}
}
