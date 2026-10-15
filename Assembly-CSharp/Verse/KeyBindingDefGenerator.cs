using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public static class KeyBindingDefGenerator
{
	[CompilerGenerated]
	private sealed class _003CImpliedKeyBindingCategoryDefs_003Ed__0 : IEnumerable<KeyBindingCategoryDef>, IEnumerable, IEnumerator<KeyBindingCategoryDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private KeyBindingCategoryDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		private List<KeyBindingCategoryDef> _003CgameUniversalCats_003E5__2;

		private IEnumerator<DesignationCategoryDef> _003C_003E7__wrap2;

		KeyBindingCategoryDef IEnumerator<KeyBindingCategoryDef>.Current
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
		public _003CImpliedKeyBindingCategoryDefs_003Ed__0(int _003C_003E1__state)
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
			_003CgameUniversalCats_003E5__2 = null;
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
					_003CgameUniversalCats_003E5__2 = DefDatabase<KeyBindingCategoryDef>.AllDefs.Where((KeyBindingCategoryDef d) => d.isGameUniversal).ToList();
					_003C_003E7__wrap2 = DefDatabase<DesignationCategoryDef>.AllDefs.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					DesignationCategoryDef current = _003C_003E7__wrap2.Current;
					string defName = "Architect_" + current.defName;
					KeyBindingCategoryDef keyBindingCategoryDef = (hotReload ? (DefDatabase<KeyBindingCategoryDef>.GetNamed(defName, errorOnFail: false) ?? new KeyBindingCategoryDef()) : new KeyBindingCategoryDef());
					keyBindingCategoryDef.defName = defName;
					keyBindingCategoryDef.label = current.label + " tab";
					keyBindingCategoryDef.description = "Key bindings for the \"" + current.LabelCap + "\" section of the Architect menu";
					keyBindingCategoryDef.modContentPack = current.modContentPack;
					keyBindingCategoryDef.checkForConflicts.AddRange(_003CgameUniversalCats_003E5__2);
					for (int i = 0; i < _003CgameUniversalCats_003E5__2.Count; i++)
					{
						_003CgameUniversalCats_003E5__2[i].checkForConflicts.Add(keyBindingCategoryDef);
					}
					current.bindingCatDef = keyBindingCategoryDef;
					_003C_003E2__current = keyBindingCategoryDef;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
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
		IEnumerator<KeyBindingCategoryDef> IEnumerable<KeyBindingCategoryDef>.GetEnumerator()
		{
			_003CImpliedKeyBindingCategoryDefs_003Ed__0 _003CImpliedKeyBindingCategoryDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CImpliedKeyBindingCategoryDefs_003Ed__ = this;
			}
			else
			{
				_003CImpliedKeyBindingCategoryDefs_003Ed__ = new _003CImpliedKeyBindingCategoryDefs_003Ed__0(0);
			}
			_003CImpliedKeyBindingCategoryDefs_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CImpliedKeyBindingCategoryDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyBindingCategoryDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CImpliedKeyBindingDefs_003Ed__1 : IEnumerable<KeyBindingDef>, IEnumerable, IEnumerator<KeyBindingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private KeyBindingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		private IEnumerator<MainButtonDef> _003C_003E7__wrap1;

		KeyBindingDef IEnumerator<KeyBindingDef>.Current
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
		public _003CImpliedKeyBindingDefs_003Ed__1(int _003C_003E1__state)
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
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = DefDatabase<MainButtonDef>.AllDefs.OrderBy((MainButtonDef td) => td.order).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					MainButtonDef current = _003C_003E7__wrap1.Current;
					if (current.defaultHotKey != 0)
					{
						string defName = "MainTab_" + current.defName;
						KeyBindingDef keyBindingDef = (hotReload ? (DefDatabase<KeyBindingDef>.GetNamed(defName, errorOnFail: false) ?? new KeyBindingDef()) : new KeyBindingDef());
						keyBindingDef.label = "Toggle " + current.label + " tab";
						keyBindingDef.defName = defName;
						keyBindingDef.category = KeyBindingCategoryDefOf.MainTabs;
						keyBindingDef.defaultKeyCodeA = current.defaultHotKey;
						keyBindingDef.modContentPack = current.modContentPack;
						current.hotKey = keyBindingDef;
						_003C_003E2__current = keyBindingDef;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<KeyBindingDef> IEnumerable<KeyBindingDef>.GetEnumerator()
		{
			_003CImpliedKeyBindingDefs_003Ed__1 _003CImpliedKeyBindingDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CImpliedKeyBindingDefs_003Ed__ = this;
			}
			else
			{
				_003CImpliedKeyBindingDefs_003Ed__ = new _003CImpliedKeyBindingDefs_003Ed__1(0);
			}
			_003CImpliedKeyBindingDefs_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CImpliedKeyBindingDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyBindingDef>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CImpliedKeyBindingCategoryDefs_003Ed__0))]
	public static IEnumerable<KeyBindingCategoryDef> ImpliedKeyBindingCategoryDefs(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CImpliedKeyBindingCategoryDefs_003Ed__0(-2)
		{
			_003C_003E3__hotReload = hotReload
		};
	}

	[IteratorStateMachine(typeof(_003CImpliedKeyBindingDefs_003Ed__1))]
	public static IEnumerable<KeyBindingDef> ImpliedKeyBindingDefs(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CImpliedKeyBindingDefs_003Ed__1(-2)
		{
			_003C_003E3__hotReload = hotReload
		};
	}
}
