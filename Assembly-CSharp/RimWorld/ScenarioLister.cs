using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class ScenarioLister
{
	[CompilerGenerated]
	private sealed class _003CAllScenarios_003Ed__1 : IEnumerable<Scenario>, IEnumerable, IEnumerator<Scenario>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Scenario _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerator<ScenarioDef> _003C_003E7__wrap1;

		private IEnumerator<Scenario> _003C_003E7__wrap2;

		Scenario IEnumerator<Scenario>.Current
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
		public _003CAllScenarios_003Ed__1(int _003C_003E1__state)
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
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					RecacheIfDirty();
					_003C_003E7__wrap1 = DefDatabase<ScenarioDef>.AllDefs.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00d5;
				case 3:
					{
						_003C_003E1__state = -5;
						break;
					}
					IL_0078:
					if (_003C_003E7__wrap1.MoveNext())
					{
						ScenarioDef current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current.scenario;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = ScenarioFiles.AllScenariosLocal.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00d5;
					IL_00d5:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Scenario current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					_003C_003E7__wrap2 = ScenarioFiles.AllScenariosWorkshop.GetEnumerator();
					_003C_003E1__state = -5;
					break;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					Scenario current3 = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current3;
					_003C_003E1__state = 3;
					return true;
				}
				_003C_003Em__Finally3();
				_003C_003E7__wrap2 = null;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Scenario> IEnumerable<Scenario>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CAllScenarios_003Ed__1(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Scenario>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CScenariosInCategory_003Ed__2 : IEnumerable<Scenario>, IEnumerable, IEnumerator<Scenario>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Scenario _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ScenarioCategory cat;

		public ScenarioCategory _003C_003E3__cat;

		private IEnumerator<ScenarioDef> _003C_003E7__wrap1;

		private IEnumerator<Scenario> _003C_003E7__wrap2;

		Scenario IEnumerator<Scenario>.Current
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
		public _003CScenariosInCategory_003Ed__2(int _003C_003E1__state)
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
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					RecacheIfDirty();
					if (cat == ScenarioCategory.FromDef)
					{
						_003C_003E7__wrap1 = DefDatabase<ScenarioDef>.AllDefs.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0081;
					}
					if (cat == ScenarioCategory.CustomLocal)
					{
						_003C_003E7__wrap2 = ScenarioFiles.AllScenariosLocal.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00ec;
					}
					if (cat != ScenarioCategory.SteamWorkshop)
					{
						break;
					}
					_003C_003E7__wrap2 = ScenarioFiles.AllScenariosWorkshop.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_0153;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0081;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00ec;
				case 3:
					{
						_003C_003E1__state = -5;
						goto IL_0153;
					}
					IL_00ec:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Scenario current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					break;
					IL_0081:
					if (_003C_003E7__wrap1.MoveNext())
					{
						ScenarioDef current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2.scenario;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					break;
					IL_0153:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Scenario current3 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap2 = null;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Scenario> IEnumerable<Scenario>.GetEnumerator()
		{
			_003CScenariosInCategory_003Ed__2 _003CScenariosInCategory_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CScenariosInCategory_003Ed__ = this;
			}
			else
			{
				_003CScenariosInCategory_003Ed__ = new _003CScenariosInCategory_003Ed__2(0);
			}
			_003CScenariosInCategory_003Ed__.cat = _003C_003E3__cat;
			return _003CScenariosInCategory_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Scenario>)this).GetEnumerator();
		}
	}

	private static bool dirty = true;

	[IteratorStateMachine(typeof(_003CAllScenarios_003Ed__1))]
	public static IEnumerable<Scenario> AllScenarios()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllScenarios_003Ed__1(-2);
	}

	[IteratorStateMachine(typeof(_003CScenariosInCategory_003Ed__2))]
	public static IEnumerable<Scenario> ScenariosInCategory(ScenarioCategory cat)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CScenariosInCategory_003Ed__2(-2)
		{
			_003C_003E3__cat = cat
		};
	}

	public static bool ScenarioIsListedAnywhere(Scenario scen)
	{
		RecacheIfDirty();
		foreach (ScenarioDef allDef in DefDatabase<ScenarioDef>.AllDefs)
		{
			if (allDef.scenario == scen)
			{
				return true;
			}
		}
		foreach (Scenario item in ScenarioFiles.AllScenariosLocal)
		{
			if (scen == item)
			{
				return true;
			}
		}
		return false;
	}

	public static void MarkDirty()
	{
		dirty = true;
	}

	private static void RecacheIfDirty()
	{
		if (dirty)
		{
			RecacheData();
		}
	}

	private static void RecacheData()
	{
		dirty = false;
		int num = ScenarioListHash();
		ScenarioFiles.RecacheData();
		if (ScenarioListHash() != num && !LongEventHandler.ShouldWaitForEvent)
		{
			Find.WindowStack.WindowOfType<Page_SelectScenario>()?.Notify_ScenarioListChanged();
		}
	}

	public static int ScenarioListHash()
	{
		int num = 9826121;
		foreach (Scenario item in AllScenarios())
		{
			num ^= 791 * item.GetHashCode() * 6121;
		}
		return num;
	}
}
