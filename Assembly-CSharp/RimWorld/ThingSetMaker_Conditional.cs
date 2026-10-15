using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public abstract class ThingSetMaker_Conditional : ThingSetMaker
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__6 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThingSetMaker_Conditional _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

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
		public _003CConfigErrors_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
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
				ThingSetMaker_Conditional thingSetMaker_Conditional = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (thingSetMaker_Conditional.thingSetMaker == null)
					{
						_003C_003E2__current = "thingSetMaker is null.";
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003E7__wrap1 = thingSetMaker_Conditional.thingSetMaker.ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_009b;
				case 1:
					_003C_003E1__state = -1;
					break;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_009b;
					}
					IL_009b:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__6(0)
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

	public ThingSetMaker thingSetMaker;

	protected override bool CanGenerateSub(ThingSetMakerParams parms)
	{
		if (Condition(parms))
		{
			return thingSetMaker.CanGenerate(parms);
		}
		return false;
	}

	protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
	{
		outThings.AddRange(thingSetMaker.Generate(parms));
	}

	protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
	{
		return thingSetMaker.AllGeneratableThingsDebug(parms);
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		thingSetMaker.ResolveReferences();
	}

	protected abstract bool Condition(ThingSetMakerParams parms);

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__6))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}
}
