using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class StockGenerator_Category : StockGenerator
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass4_0
	{
		public StockGenerator_Category _003C_003E4__this;

		public List<ThingDef> generatedDefs;

		public Func<ThingDef, bool> _003C_003E9__0;

		internal bool _003CGenerateThings_003Eb__0(ThingDef t)
		{
			_003C_003Ec__DisplayClass4_1 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass4_1
			{
				t = t
			};
			if (CS_0024_003C_003E8__locals5.t.tradeability.TraderCanSell() && (int)CS_0024_003C_003E8__locals5.t.techLevel <= (int)_003C_003E4__this.maxTechLevelGenerate && !generatedDefs.Contains(CS_0024_003C_003E8__locals5.t) && (_003C_003E4__this.excludedThingDefs == null || !_003C_003E4__this.excludedThingDefs.Contains(CS_0024_003C_003E8__locals5.t)))
			{
				if (_003C_003E4__this.excludedCategories != null)
				{
					return !_003C_003E4__this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(CS_0024_003C_003E8__locals5.t));
				}
				return true;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass4_1
	{
		public ThingDef t;

		internal bool _003CGenerateThings_003Eb__1(ThingCategoryDef c)
		{
			return c.DescendantThingDefs.Contains(t);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGenerateThings_003Ed__4 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StockGenerator_Category _003C_003E4__this;

		private _003C_003Ec__DisplayClass4_0 _003C_003E8__1;

		private Faction faction;

		public Faction _003C_003E3__faction;

		private int _003CnumThingDefsToUse_003E5__2;

		private int _003Ci_003E5__3;

		private ThingDef _003CchosenThingDef_003E5__4;

		private IEnumerator<Thing> _003C_003E7__wrap4;

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
		public _003CGenerateThings_003Ed__4(int _003C_003E1__state)
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
			_003C_003E8__1 = null;
			_003CchosenThingDef_003E5__4 = null;
			_003C_003E7__wrap4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				StockGenerator_Category stockGenerator_Category = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_0117;
				}
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass4_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				_003C_003E8__1.generatedDefs = new List<ThingDef>();
				_003CnumThingDefsToUse_003E5__2 = stockGenerator_Category.thingDefCountRange.RandomInRange;
				_003Ci_003E5__3 = 0;
				goto IL_0160;
				IL_0117:
				if (_003C_003E7__wrap4.MoveNext())
				{
					Thing current = _003C_003E7__wrap4.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap4 = null;
				_003C_003E8__1.generatedDefs.Add(_003CchosenThingDef_003E5__4);
				_003CchosenThingDef_003E5__4 = null;
				_003Ci_003E5__3++;
				goto IL_0160;
				IL_0160:
				if (_003Ci_003E5__3 < _003CnumThingDefsToUse_003E5__2 && stockGenerator_Category.categoryDef.DescendantThingDefs.Where(delegate(ThingDef t)
				{
					_003C_003Ec__DisplayClass4_1 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass4_1
					{
						t = t
					};
					return CS_0024_003C_003E8__locals5.t.tradeability.TraderCanSell() && (int)CS_0024_003C_003E8__locals5.t.techLevel <= (int)_003C_003E8__1._003C_003E4__this.maxTechLevelGenerate && !_003C_003E8__1.generatedDefs.Contains(CS_0024_003C_003E8__locals5.t) && (_003C_003E8__1._003C_003E4__this.excludedThingDefs == null || !_003C_003E8__1._003C_003E4__this.excludedThingDefs.Contains(CS_0024_003C_003E8__locals5.t)) && (_003C_003E8__1._003C_003E4__this.excludedCategories == null || !_003C_003E8__1._003C_003E4__this.excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(CS_0024_003C_003E8__locals5.t)));
				}).TryRandomElement(out _003CchosenThingDef_003E5__4))
				{
					_003C_003E7__wrap4 = StockGeneratorUtility.TryMakeForStock(_003CchosenThingDef_003E5__4, stockGenerator_Category.RandomCountOf(_003CchosenThingDef_003E5__4), faction).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0117;
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
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
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
			_003CGenerateThings_003Ed__4 _003CGenerateThings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGenerateThings_003Ed__ = this;
			}
			else
			{
				_003CGenerateThings_003Ed__ = new _003CGenerateThings_003Ed__4(0)
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

	private ThingCategoryDef categoryDef;

	private IntRange thingDefCountRange = IntRange.One;

	private List<ThingDef> excludedThingDefs;

	private List<ThingCategoryDef> excludedCategories;

	[IteratorStateMachine(typeof(_003CGenerateThings_003Ed__4))]
	public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGenerateThings_003Ed__4(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__faction = faction
		};
	}

	public override bool HandlesThingDef(ThingDef t)
	{
		if (categoryDef.DescendantThingDefs.Contains(t) && t.tradeability != 0 && (int)t.techLevel <= (int)maxTechLevelBuy && (excludedThingDefs == null || !excludedThingDefs.Contains(t)))
		{
			if (excludedCategories != null)
			{
				return !excludedCategories.Any((ThingCategoryDef c) => c.DescendantThingDefs.Contains(t));
			}
			return true;
		}
		return false;
	}
}
