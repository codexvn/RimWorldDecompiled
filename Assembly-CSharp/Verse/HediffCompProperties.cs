using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse;

public class HediffCompProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__3 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public HediffCompProperties _003C_003E4__this;

		private HediffDef parentDef;

		public HediffDef _003C_003E3__parentDef;

		private int _003Ci_003E5__2;

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
		public _003CConfigErrors_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			HediffCompProperties hediffCompProperties = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (hediffCompProperties.compClass == null)
				{
					_003C_003E2__current = "compClass is null";
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0052;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0052;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_00d1;
				}
				IL_0052:
				_003Ci_003E5__2 = 0;
				goto IL_00e1;
				IL_00e1:
				if (_003Ci_003E5__2 < parentDef.comps.Count)
				{
					if (parentDef.comps[_003Ci_003E5__2] != hediffCompProperties && parentDef.comps[_003Ci_003E5__2].compClass == hediffCompProperties.compClass)
					{
						_003C_003E2__current = "two comps with same compClass: " + hediffCompProperties.compClass;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00d1;
				}
				return false;
				IL_00d1:
				_003Ci_003E5__2++;
				goto IL_00e1;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__3 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__3(0)
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

	[TranslationHandle]
	public Type compClass;

	public virtual void PostLoad()
	{
	}

	public virtual void ResolveReferences(HediffDef parent)
	{
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__3))]
	public virtual IEnumerable<string> ConfigErrors(HediffDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__3(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}
}
