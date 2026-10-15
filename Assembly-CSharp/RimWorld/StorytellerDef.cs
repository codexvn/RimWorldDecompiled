using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class StorytellerDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__23 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StorytellerDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

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
		public _003CConfigErrors_003Ed__23(int _003C_003E1__state)
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
			case 6:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 7:
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
				StorytellerDef storytellerDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (storytellerDef.pointsFactorFromAdaptDays == null)
					{
						_003C_003E2__current = "pointsFactorFromAdaptDays is null";
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_006a;
				case 1:
					_003C_003E1__state = -1;
					goto IL_006a;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0092;
				case 3:
					_003C_003E1__state = -1;
					goto IL_00ba;
				case 4:
					_003C_003E1__state = -1;
					goto IL_00e2;
				case 5:
					_003C_003E1__state = -1;
					goto IL_010a;
				case 6:
					_003C_003E1__state = -3;
					goto IL_014e;
				case 7:
					{
						_003C_003E1__state = -4;
						goto IL_01c8;
					}
					IL_010a:
					_003C_003E7__wrap1 = ((Def)storytellerDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_014e;
					IL_006a:
					if (storytellerDef.adaptDaysLossFromColonistLostByPostPopulation == null)
					{
						_003C_003E2__current = "adaptDaysLossFromColonistLostByPostPopulation is null";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0092;
					IL_014e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 6;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__3 = 0;
					goto IL_01f4;
					IL_0092:
					if (storytellerDef.adaptDaysLossFromColonistViolentlyDownedByPopulation == null)
					{
						_003C_003E2__current = "adaptDaysLossFromColonistViolentlyDownedByPopulation is null";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_00ba;
					IL_00ba:
					if (storytellerDef.adaptDaysGrowthRateCurve == null)
					{
						_003C_003E2__current = "adaptDaysGrowthRateCurve is null";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_00e2;
					IL_01f4:
					if (_003Ci_003E5__3 < storytellerDef.comps.Count)
					{
						_003C_003E7__wrap1 = storytellerDef.comps[_003Ci_003E5__3].ConfigErrors(storytellerDef).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_01c8;
					}
					return false;
					IL_00e2:
					if (storytellerDef.pointsFactorFromDaysPassed == null)
					{
						_003C_003E2__current = "pointsFactorFromDaysPassed is null";
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_010a;
					IL_01c8:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 7;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__3++;
					goto IL_01f4;
				}
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__23 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__23(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public int listOrder = 9999;

	public bool listVisible = true;

	public bool tutorialMode;

	public bool disableAdaptiveTraining;

	public bool disableAlerts;

	public bool disablePermadeath;

	public DifficultyDef forcedDifficulty;

	[NoTranslate]
	private string portraitLarge;

	[NoTranslate]
	private string portraitTiny;

	public List<StorytellerCompProperties> comps = new List<StorytellerCompProperties>();

	public SimpleCurve populationIntentFactorFromPopCurve;

	public SimpleCurve populationIntentFactorFromPopAdaptDaysCurve;

	public SimpleCurve pointsFactorFromDaysPassed;

	public float adaptDaysMin;

	public float adaptDaysMax = 100f;

	public float adaptDaysGameStartGraceDays;

	public SimpleCurve pointsFactorFromAdaptDays;

	public SimpleCurve adaptDaysLossFromColonistLostByPostPopulation;

	public SimpleCurve adaptDaysLossFromColonistViolentlyDownedByPopulation;

	public SimpleCurve adaptDaysGrowthRateCurve;

	[Unsaved(false)]
	public Texture2D portraitLargeTex;

	[Unsaved(false)]
	public Texture2D portraitTinyTex;

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		LongEventHandler.ExecuteWhenFinished(delegate
		{
			if (!portraitTiny.NullOrEmpty())
			{
				portraitTinyTex = ContentFinder<Texture2D>.Get(portraitTiny);
				portraitLargeTex = ContentFinder<Texture2D>.Get(portraitLarge);
			}
		});
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].ResolveReferences(this);
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__23))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__23(-2)
		{
			_003C_003E4__this = this
		};
	}
}
