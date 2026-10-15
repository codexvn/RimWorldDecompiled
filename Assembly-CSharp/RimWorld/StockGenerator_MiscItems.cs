using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public abstract class StockGenerator_MiscItems : StockGenerator
{
	[CompilerGenerated]
	private sealed class _003CGenerateThings_003Ed__0 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StockGenerator_MiscItems _003C_003E4__this;

		private Faction faction;

		public Faction _003C_003E3__faction;

		private int _003Ccount_003E5__2;

		private int _003Ci_003E5__3;

		Thing IEnumerator<Thing>.Current
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
		public _003CGenerateThings_003Ed__0(int _003C_003E1__state)
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
			StockGenerator_MiscItems CS_0024_003C_003E8__locals5 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ccount_003E5__2 = CS_0024_003C_003E8__locals5.countRange.RandomInRange;
				_003Ci_003E5__3 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < _003Ccount_003E5__2)
			{
				if (!DefDatabase<ThingDef>.AllDefs.Where((ThingDef t) => CS_0024_003C_003E8__locals5.HandlesThingDef(t) && t.tradeability.TraderCanSell() && (int)t.techLevel <= (int)CS_0024_003C_003E8__locals5.maxTechLevelGenerate).TryRandomElementByWeight(CS_0024_003C_003E8__locals5.SelectionWeight, out var result))
				{
					return false;
				}
				_003C_003E2__current = CS_0024_003C_003E8__locals5.MakeThing(result, faction);
				_003C_003E1__state = 1;
				return true;
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CGenerateThings_003Ed__0 _003CGenerateThings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGenerateThings_003Ed__ = this;
			}
			else
			{
				_003CGenerateThings_003Ed__ = new _003CGenerateThings_003Ed__0(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGenerateThings_003Ed__.faction = _003C_003E3__faction;
			return _003CGenerateThings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CGenerateThings_003Ed__0))]
	public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGenerateThings_003Ed__0(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__faction = faction
		};
	}

	protected virtual Thing MakeThing(ThingDef def, Faction faction)
	{
		return StockGeneratorUtility.TryMakeForStockSingle(def, 1, faction);
	}

	public override bool HandlesThingDef(ThingDef thingDef)
	{
		if (thingDef.tradeability != 0)
		{
			return (int)thingDef.techLevel <= (int)maxTechLevelBuy;
		}
		return false;
	}

	protected virtual float SelectionWeight(ThingDef thingDef)
	{
		return 1f;
	}
}
