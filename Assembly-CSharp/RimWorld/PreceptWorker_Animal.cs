using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class PreceptWorker_Animal : PreceptWorker
{
	[CompilerGenerated]
	private sealed class _003Cget_ThingDefs_003Ed__2 : IEnumerable<PreceptThingChance>, IEnumerable, IEnumerator<PreceptThingChance>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private PreceptThingChance _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerator<ThingDef> _003C_003E7__wrap1;

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
					_003C_003E7__wrap1 = DefDatabase<ThingDef>.AllDefs.Where((ThingDef x) => x.thingCategories != null && x.thingCategories.Contains(ThingCategoryDefOf.Animals) && !x.race.Insect && !x.race.Dryad).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap1.Current;
					PreceptThingChance preceptThingChance = default(PreceptThingChance);
					preceptThingChance.def = current;
					preceptThingChance.chance = 1f;
					_003C_003E2__current = preceptThingChance;
					_003C_003E1__state = 1;
					return true;
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

	public override AcceptanceReport CanUse(ThingDef def, Ideo ideo, FactionDef generatingFor)
	{
		foreach (Precept item in ideo.PreceptsListForReading)
		{
			if (item is Precept_Animal precept_Animal && precept_Animal.ThingDef == def)
			{
				return false;
			}
		}
		return true;
	}
}
