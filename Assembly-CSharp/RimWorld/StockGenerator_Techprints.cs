using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class StockGenerator_Techprints : StockGenerator
{
	[CompilerGenerated]
	private sealed class _003CGenerateThings_003Ed__2 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StockGenerator_Techprints _003C_003E4__this;

		private Faction faction;

		public Faction _003C_003E3__faction;

		private int _003CcountToGenerate_003E5__2;

		private int _003Ci_003E5__3;

		private IEnumerator<Thing> _003C_003E7__wrap3;

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
		public _003CGenerateThings_003Ed__2(int _003C_003E1__state)
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
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				StockGenerator_Techprints stockGenerator_Techprints = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_00c6;
				}
				_003C_003E1__state = -1;
				stockGenerator_Techprints.tmpGenerated.Clear();
				_003CcountToGenerate_003E5__2 = CountChanceUtility.RandomCount(stockGenerator_Techprints.countChances);
				_003Ci_003E5__3 = 0;
				goto IL_00f2;
				IL_00c6:
				if (_003C_003E7__wrap3.MoveNext())
				{
					Thing current = _003C_003E7__wrap3.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap3 = null;
				_003Ci_003E5__3++;
				goto IL_00f2;
				IL_00f2:
				if (_003Ci_003E5__3 < _003CcountToGenerate_003E5__2)
				{
					if (!TechprintUtility.TryGetTechprintDefToGenerate_NewTemp(faction, out var result, stockGenerator_Techprints.tmpGenerated))
					{
						return false;
					}
					stockGenerator_Techprints.tmpGenerated.Add(result);
					_003C_003E7__wrap3 = StockGeneratorUtility.TryMakeForStock(result, 1, faction).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00c6;
				}
				stockGenerator_Techprints.tmpGenerated.Clear();
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
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CGenerateThings_003Ed__2 _003CGenerateThings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGenerateThings_003Ed__ = this;
			}
			else
			{
				_003CGenerateThings_003Ed__ = new _003CGenerateThings_003Ed__2(0)
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

	private List<CountChance> countChances;

	private List<ThingDef> tmpGenerated = new List<ThingDef>();

	[IteratorStateMachine(typeof(_003CGenerateThings_003Ed__2))]
	public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGenerateThings_003Ed__2(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__faction = faction
		};
	}

	public override bool HandlesThingDef(ThingDef thingDef)
	{
		if (thingDef.tradeTags != null && thingDef.tradeability != 0 && (int)thingDef.techLevel <= (int)maxTechLevelBuy)
		{
			return thingDef.tradeTags.Contains("Techprint");
		}
		return false;
	}
}
