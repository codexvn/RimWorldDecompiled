using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse;

public class KeyPrefsData
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public KeyBindingDef def;

		internal bool _003CConflictingBindings_003Eb__0(string tag)
		{
			return def.extraConflictTags.Contains(tag);
		}
	}

	[CompilerGenerated]
	private sealed class _003CConflictingBindings_003Ed__5 : IEnumerable<KeyBindingDef>, IEnumerable, IEnumerator<KeyBindingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private KeyBindingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private KeyBindingDef keyDef;

		public KeyBindingDef _003C_003E3__keyDef;

		public KeyPrefsData _003C_003E4__this;

		private KeyCode code;

		public KeyCode _003C_003E3__code;

		private IEnumerator<KeyBindingDef> _003C_003E7__wrap1;

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
		public _003CConflictingBindings_003Ed__5(int _003C_003E1__state)
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
				KeyPrefsData keyPrefsData = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = DefDatabase<KeyBindingDef>.AllDefs.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					_003C_003Ec__DisplayClass5_0 CS_0024_003C_003E8__locals9 = new _003C_003Ec__DisplayClass5_0
					{
						def = _003C_003E7__wrap1.Current
					};
					if (CS_0024_003C_003E8__locals9.def != keyDef && (keyDef.ignoreConflictsWith == null || !keyDef.ignoreConflictsWith.Contains(CS_0024_003C_003E8__locals9.def)) && ((CS_0024_003C_003E8__locals9.def.category == keyDef.category && CS_0024_003C_003E8__locals9.def.category.selfConflicting) || keyDef.category.checkForConflicts.Contains(CS_0024_003C_003E8__locals9.def.category) || (keyDef.extraConflictTags != null && CS_0024_003C_003E8__locals9.def.extraConflictTags != null && keyDef.extraConflictTags.Any((string tag) => CS_0024_003C_003E8__locals9.def.extraConflictTags.Contains(tag)))) && keyPrefsData.keyPrefs.TryGetValue(CS_0024_003C_003E8__locals9.def, out var value) && (value.keyBindingA == code || value.keyBindingB == code))
					{
						_003C_003E2__current = CS_0024_003C_003E8__locals9.def;
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
			_003CConflictingBindings_003Ed__5 _003CConflictingBindings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConflictingBindings_003Ed__ = this;
			}
			else
			{
				_003CConflictingBindings_003Ed__ = new _003CConflictingBindings_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConflictingBindings_003Ed__.keyDef = _003C_003E3__keyDef;
			_003CConflictingBindings_003Ed__.code = _003C_003E3__code;
			return _003CConflictingBindings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyBindingDef>)this).GetEnumerator();
		}
	}

	public Dictionary<KeyBindingDef, KeyBindingData> keyPrefs = new Dictionary<KeyBindingDef, KeyBindingData>();

	public void ResetToDefaults()
	{
		keyPrefs.Clear();
		AddMissingDefaultBindings();
	}

	public void AddMissingDefaultBindings()
	{
		foreach (KeyBindingDef allDef in DefDatabase<KeyBindingDef>.AllDefs)
		{
			if (!keyPrefs.ContainsKey(allDef))
			{
				keyPrefs.Add(allDef, new KeyBindingData(allDef.defaultKeyCodeA, allDef.defaultKeyCodeB));
			}
		}
	}

	public bool SetBinding(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot, KeyCode keyCode)
	{
		if (keyPrefs.TryGetValue(keyDef, out var value))
		{
			switch (slot)
			{
			case KeyPrefs.BindingSlot.A:
				value.keyBindingA = keyCode;
				break;
			case KeyPrefs.BindingSlot.B:
				value.keyBindingB = keyCode;
				break;
			default:
				Log.Error("Tried to set a key binding for \"" + keyDef.LabelCap + "\" on a nonexistent slot: " + slot.ToString());
				return false;
			}
			return true;
		}
		Log.Error("Key not found in keyprefs: \"" + keyDef.LabelCap + "\"");
		return false;
	}

	public KeyCode GetBoundKeyCode(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
	{
		if (!keyPrefs.TryGetValue(keyDef, out var value))
		{
			Log.Error("Key not found in keyprefs: \"" + keyDef.LabelCap + "\"");
			return KeyCode.None;
		}
		return slot switch
		{
			KeyPrefs.BindingSlot.A => value.keyBindingA, 
			KeyPrefs.BindingSlot.B => value.keyBindingB, 
			_ => throw new InvalidOperationException(), 
		};
	}

	[IteratorStateMachine(typeof(_003CConflictingBindings_003Ed__5))]
	private IEnumerable<KeyBindingDef> ConflictingBindings(KeyBindingDef keyDef, KeyCode code)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConflictingBindings_003Ed__5(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__keyDef = keyDef,
			_003C_003E3__code = code
		};
	}

	public void EraseConflictingBindingsForKeyCode(KeyBindingDef keyDef, KeyCode keyCode, Action<KeyBindingDef> callBackOnErase = null)
	{
		foreach (KeyBindingDef item in ConflictingBindings(keyDef, keyCode))
		{
			KeyBindingData keyBindingData = keyPrefs[item];
			if (keyBindingData.keyBindingA == keyCode)
			{
				keyBindingData.keyBindingA = KeyCode.None;
			}
			if (keyBindingData.keyBindingB == keyCode)
			{
				keyBindingData.keyBindingB = KeyCode.None;
			}
			callBackOnErase?.Invoke(item);
		}
	}

	public void CheckConflictsFor(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
	{
		KeyCode boundKeyCode = GetBoundKeyCode(keyDef, slot);
		if (boundKeyCode != 0)
		{
			EraseConflictingBindingsForKeyCode(keyDef, boundKeyCode);
			SetBinding(keyDef, slot, boundKeyCode);
		}
	}

	public KeyPrefsData Clone()
	{
		KeyPrefsData keyPrefsData = new KeyPrefsData();
		foreach (KeyValuePair<KeyBindingDef, KeyBindingData> keyPref in keyPrefs)
		{
			keyPrefsData.keyPrefs[keyPref.Key] = new KeyBindingData(keyPref.Value.keyBindingA, keyPref.Value.keyBindingB);
		}
		return keyPrefsData;
	}

	public void ErrorCheck()
	{
		foreach (KeyBindingDef allDef in DefDatabase<KeyBindingDef>.AllDefs)
		{
			ErrorCheckOn(allDef, KeyPrefs.BindingSlot.A);
			ErrorCheckOn(allDef, KeyPrefs.BindingSlot.B);
		}
	}

	private void ErrorCheckOn(KeyBindingDef keyDef, KeyPrefs.BindingSlot slot)
	{
		KeyCode boundKeyCode = GetBoundKeyCode(keyDef, slot);
		if (boundKeyCode == KeyCode.None)
		{
			return;
		}
		foreach (KeyBindingDef item in ConflictingBindings(keyDef, boundKeyCode))
		{
			bool flag = boundKeyCode != keyDef.GetDefaultKeyCode(slot);
			Log.Warning("Key binding conflict: " + item?.ToString() + " and " + keyDef?.ToString() + " are both bound to " + boundKeyCode.ToString() + "." + (flag ? " Fixed automatically." : ""));
			if (flag)
			{
				if (slot == KeyPrefs.BindingSlot.A)
				{
					keyPrefs[keyDef].keyBindingA = keyDef.defaultKeyCodeA;
				}
				else
				{
					keyPrefs[keyDef].keyBindingB = keyDef.defaultKeyCodeB;
				}
				KeyPrefs.Save();
			}
		}
	}
}
