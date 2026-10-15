using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StorytellerCompProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__11 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StorytellerCompProperties _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__11(int _003C_003E1__state)
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
			StorytellerCompProperties storytellerCompProperties = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (storytellerCompProperties.compClass == null)
				{
					_003C_003E2__current = "a StorytellerCompProperties has null compClass.";
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
					break;
				}
				IL_0052:
				if (!storytellerCompProperties.enableIfAnyModActive.NullOrEmpty() && !storytellerCompProperties.disableIfAnyModActive.NullOrEmpty())
				{
					_003C_003E2__current = "enableIfAnyModActive and disableIfAnyModActive can't be used simultaneously";
					_003C_003E1__state = 2;
					return true;
				}
				break;
			}
			return false;
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
			_003CConfigErrors_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__11(0)
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

	[TranslationHandle]
	public Type compClass;

	public float minDaysPassed;

	public List<IncidentTargetTagDef> allowedTargetTags;

	public List<IncidentTargetTagDef> disallowedTargetTags;

	public float minIncChancePopulationIntentFactor = 0.05f;

	public List<string> enableIfAnyModActive;

	public List<string> disableIfAnyModActive;

	public bool Enabled
	{
		get
		{
			if (!enableIfAnyModActive.NullOrEmpty())
			{
				for (int i = 0; i < enableIfAnyModActive.Count; i++)
				{
					if (ModsConfig.IsActive(enableIfAnyModActive[i]))
					{
						return true;
					}
				}
				return false;
			}
			if (!disableIfAnyModActive.NullOrEmpty())
			{
				for (int j = 0; j < disableIfAnyModActive.Count; j++)
				{
					if (ModsConfig.IsActive(disableIfAnyModActive[j]))
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public StorytellerCompProperties()
	{
	}

	public StorytellerCompProperties(Type compClass)
	{
		this.compClass = compClass;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__11))]
	public virtual IEnumerable<string> ConfigErrors(StorytellerDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__11(-2)
		{
			_003C_003E4__this = this
		};
	}

	public virtual void ResolveReferences(StorytellerDef parentDef)
	{
	}
}
