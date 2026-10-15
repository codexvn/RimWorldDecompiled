using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class PreceptWorker
{
	[CompilerGenerated]
	private sealed class _003CThingDefsForIdeo_003Ed__7 : IEnumerable<PreceptThingChance>, IEnumerable, IEnumerator<PreceptThingChance>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private PreceptThingChance _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PreceptWorker _003C_003E4__this;

		private Ideo ideo;

		public Ideo _003C_003E3__ideo;

		private FactionDef generatingFor;

		public FactionDef _003C_003E3__generatingFor;

		private IEnumerator<PreceptThingChance> _003C_003E7__wrap1;

		PreceptThingChance IEnumerator<PreceptThingChance>.Current
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
		public _003CThingDefsForIdeo_003Ed__7(int _003C_003E1__state)
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
				PreceptWorker preceptWorker = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = preceptWorker.ThingDefs.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					PreceptThingChance current = _003C_003E7__wrap1.Current;
					if ((bool)preceptWorker.CanUse(current.def, ideo, generatingFor))
					{
						_003C_003E2__current = current;
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
		IEnumerator<PreceptThingChance> IEnumerable<PreceptThingChance>.GetEnumerator()
		{
			_003CThingDefsForIdeo_003Ed__7 _003CThingDefsForIdeo_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CThingDefsForIdeo_003Ed__ = this;
			}
			else
			{
				_003CThingDefsForIdeo_003Ed__ = new _003CThingDefsForIdeo_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CThingDefsForIdeo_003Ed__.ideo = _003C_003E3__ideo;
			_003CThingDefsForIdeo_003Ed__.generatingFor = _003C_003E3__generatingFor;
			return _003CThingDefsForIdeo_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<PreceptThingChance>)this).GetEnumerator();
		}
	}

	public PreceptDef def;

	public virtual IEnumerable<PreceptThingChance> ThingDefs => ((IEnumerable<PreceptThingChanceClass>)def.buildingDefChances).Select((Func<PreceptThingChanceClass, PreceptThingChance>)((PreceptThingChanceClass dc) => dc));

	public virtual bool ShouldSkipThing(Ideo ideo, ThingDef thingDef)
	{
		return false;
	}

	public virtual float GetThingOrder(PreceptThingChance thingChance)
	{
		return 0f;
	}

	public virtual AcceptanceReport CanUse(ThingDef def, Ideo ideo, FactionDef generatingFor)
	{
		return true;
	}

	[Obsolete("Will be removed in a future update")]
	public virtual AcceptanceReport CanUse(ThingDef def, Ideo ideo)
	{
		return CanUse(def, ideo, null);
	}

	[IteratorStateMachine(typeof(_003CThingDefsForIdeo_003Ed__7))]
	public virtual IEnumerable<PreceptThingChance> ThingDefsForIdeo(Ideo ideo, FactionDef generatingFor = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CThingDefsForIdeo_003Ed__7(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__ideo = ideo,
			_003C_003E3__generatingFor = generatingFor
		};
	}

	[Obsolete("Will be removed in a future update")]
	public virtual IEnumerable<PreceptThingChance> ThingDefsForIdeo(Ideo ideo)
	{
		return ThingDefsForIdeo(ideo, null);
	}
}
