using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StockGeneratorUtility
{
	[CompilerGenerated]
	private sealed class _003CTryMakeForStock_003Ed__0 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingDef thingDef;

		public ThingDef _003C_003E3__thingDef;

		private Faction faction;

		public Faction _003C_003E3__faction;

		private int count;

		public int _003C_003E3__count;

		private int _003Ci_003E5__2;

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
		public _003CTryMakeForStock_003Ed__0(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (thingDef.MadeFromStuff || thingDef.tradeNeverStack || thingDef.tradeNeverGenerateStacked)
				{
					_003Ci_003E5__2 = 0;
					goto IL_008f;
				}
				Thing thing = TryMakeForStockSingle(thingDef, count, faction);
				if (thing != null)
				{
					_003C_003E2__current = thing;
					_003C_003E1__state = 2;
					return true;
				}
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_007f;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_008f:
				if (_003Ci_003E5__2 < count)
				{
					Thing thing2 = TryMakeForStockSingle(thingDef, 1, faction);
					if (thing2 != null)
					{
						_003C_003E2__current = thing2;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_007f;
				}
				break;
				IL_007f:
				_003Ci_003E5__2++;
				goto IL_008f;
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
			_003CTryMakeForStock_003Ed__0 _003CTryMakeForStock_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CTryMakeForStock_003Ed__ = this;
			}
			else
			{
				_003CTryMakeForStock_003Ed__ = new _003CTryMakeForStock_003Ed__0(0);
			}
			_003CTryMakeForStock_003Ed__.thingDef = _003C_003E3__thingDef;
			_003CTryMakeForStock_003Ed__.count = _003C_003E3__count;
			_003CTryMakeForStock_003Ed__.faction = _003C_003E3__faction;
			return _003CTryMakeForStock_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CTryMakeForStock_003Ed__0))]
	public static IEnumerable<Thing> TryMakeForStock(ThingDef thingDef, int count, Faction faction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CTryMakeForStock_003Ed__0(-2)
		{
			_003C_003E3__thingDef = thingDef,
			_003C_003E3__count = count,
			_003C_003E3__faction = faction
		};
	}

	public static Thing TryMakeForStockSingle(ThingDef thingDef, int stackCount, Faction faction)
	{
		if (stackCount <= 0)
		{
			return null;
		}
		if (!thingDef.tradeability.TraderCanSell())
		{
			Log.Error("Tried to make non-trader-sellable thing for trader stock: " + thingDef);
			return null;
		}
		ThingDef result = null;
		if (thingDef.MadeFromStuff && !(from x in GenStuff.AllowedStuffsFor(thingDef, TechLevel.Undefined, checkAllowedInStuffGeneration: true)
			where !PawnWeaponGenerator.IsDerpWeapon(thingDef, x)
			select x).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out result))
		{
			result = GenStuff.RandomStuffByCommonalityFor(thingDef);
		}
		Thing thing = ThingMaker.MakeThing(thingDef, result);
		thing.stackCount = stackCount;
		return thing;
	}
}
