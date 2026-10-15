using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class PreceptWorker_Relic : PreceptWorker
{
	[CompilerGenerated]
	private sealed class _003Cget_ThingDefs_003Ed__2 : IEnumerable<PreceptThingChance>, IEnumerable, IEnumerator<PreceptThingChance>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private PreceptThingChance _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<ThingDef>.Enumerator _003C_003E7__wrap1;

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
		public _003Cget_ThingDefs_003Ed__2(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
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
					_003C_003E7__wrap1 = DefDatabase<ThingDef>.AllDefsListForReading.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap1.Current;
					if (current.relicChance != 0f)
					{
						_003C_003E2__current = new PreceptThingChance
						{
							chance = current.relicChance,
							def = current
						};
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<PreceptThingChance> IEnumerable<PreceptThingChance>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003Cget_ThingDefs_003Ed__2(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<PreceptThingChance>)this).GetEnumerator();
		}
	}

	public override IEnumerable<PreceptThingChance> ThingDefs
	{
		[IteratorStateMachine(typeof(_003Cget_ThingDefs_003Ed__2))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_ThingDefs_003Ed__2(-2);
		}
	}

	public override float GetThingOrder(PreceptThingChance thingChance)
	{
		return 0f - thingChance.chance;
	}
}
