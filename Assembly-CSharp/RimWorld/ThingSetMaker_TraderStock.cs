using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class ThingSetMaker_TraderStock : ThingSetMaker
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public StockGenerator stock;

		public Func<ThingDef, bool> _003C_003E9__0;

		internal bool _003CAllGeneratableThingsDebugSub_003Eb__0(ThingDef x)
		{
			if (x.tradeability.TraderCanSell())
			{
				return stock.HandlesThingDef(x);
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CAllGeneratableThingsDebugSub_003Ed__3 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingSetMakerParams parms;

		public ThingSetMakerParams _003C_003E3__parms;

		private int _003Ci_003E5__2;

		private IEnumerator<ThingDef> _003C_003E7__wrap2;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003CAllGeneratableThingsDebugSub_003Ed__3(int _003C_003E1__state)
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
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_00cc;
				}
				_003C_003E1__state = -1;
				if (parms.traderDef == null)
				{
					return false;
				}
				_003Ci_003E5__2 = 0;
				goto IL_00f8;
				IL_00cc:
				if (_003C_003E7__wrap2.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
				_003Ci_003E5__2++;
				goto IL_00f8;
				IL_00f8:
				if (_003Ci_003E5__2 < parms.traderDef.stockGenerators.Count)
				{
					_003C_003Ec__DisplayClass3_0 CS_0024_003C_003E8__locals1 = new _003C_003Ec__DisplayClass3_0
					{
						stock = parms.traderDef.stockGenerators[_003Ci_003E5__2]
					};
					_003C_003E7__wrap2 = DefDatabase<ThingDef>.AllDefs.Where((ThingDef x) => x.tradeability.TraderCanSell() && CS_0024_003C_003E8__locals1.stock.HandlesThingDef(x)).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00cc;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003CAllGeneratableThingsDebugSub_003Ed__3 _003CAllGeneratableThingsDebugSub_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllGeneratableThingsDebugSub_003Ed__ = this;
			}
			else
			{
				_003CAllGeneratableThingsDebugSub_003Ed__ = new _003CAllGeneratableThingsDebugSub_003Ed__3(0);
			}
			_003CAllGeneratableThingsDebugSub_003Ed__.parms = _003C_003E3__parms;
			return _003CAllGeneratableThingsDebugSub_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
	{
		TraderKindDef traderKindDef = parms.traderDef ?? DefDatabase<TraderKindDef>.AllDefsListForReading.RandomElement();
		Faction makingFaction = parms.makingFaction;
		PlanetTile forTile = (parms.tile.HasValue ? parms.tile.Value : ((Find.AnyPlayerHomeMap != null) ? Find.AnyPlayerHomeMap.Tile : ((Find.CurrentMap == null) ? PlanetTile.Invalid : Find.CurrentMap.Tile)));
		for (int i = 0; i < traderKindDef.stockGenerators.Count; i++)
		{
			foreach (Thing item in traderKindDef.stockGenerators[i].GenerateThings(forTile, parms.makingFaction))
			{
				if (!item.def.tradeability.TraderCanSell())
				{
					Log.Error(traderKindDef?.ToString() + " generated carrying " + item?.ToString() + " which can't be sold by traders. Ignoring...");
					continue;
				}
				item.PostGeneratedForTrader(traderKindDef, forTile, makingFaction);
				outThings.Add(item);
			}
		}
	}

	public float DebugAverageTotalStockValue(TraderKindDef td)
	{
		ThingSetMakerParams thingSetMakerParams = default(ThingSetMakerParams);
		thingSetMakerParams.traderDef = td;
		thingSetMakerParams.tile = PlanetTile.Invalid;
		ThingSetMakerParams parms = thingSetMakerParams;
		float num = 0f;
		for (int i = 0; i < 50; i++)
		{
			foreach (Thing item in Generate(parms))
			{
				num += item.MarketValue * (float)item.stackCount;
			}
		}
		return num / 50f;
	}

	public string DebugGenerationDataFor(TraderKindDef td)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(td.defName);
		stringBuilder.AppendLine("Average total market value:" + DebugAverageTotalStockValue(td).ToString("F0"));
		ThingSetMakerParams thingSetMakerParams = default(ThingSetMakerParams);
		thingSetMakerParams.traderDef = td;
		thingSetMakerParams.tile = PlanetTile.Invalid;
		ThingSetMakerParams parms = thingSetMakerParams;
		Find.FactionManager.AllFactionsListForReading.Where((Faction x) => x.def.baseTraderKinds.Contains(td) || x.def.visitorTraderKinds.Contains(td) || x.def.caravanTraderKinds.Contains(td)).TryRandomElement(out parms.makingFaction);
		stringBuilder.AppendLine("Example generated stock:\n\n");
		foreach (Thing item in Generate(parms))
		{
			Thing thing = ((!(item is MinifiedThing minifiedThing)) ? item : minifiedThing.InnerThing);
			string text = thing.LabelCap;
			if (thing is Book book)
			{
				text = "[" + book.def.defName + "]: " + text;
			}
			text = text + " [" + (thing.MarketValue * (float)thing.stackCount).ToString("F0") + "]";
			stringBuilder.AppendLine(text);
		}
		return stringBuilder.ToString();
	}

	[IteratorStateMachine(typeof(_003CAllGeneratableThingsDebugSub_003Ed__3))]
	protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllGeneratableThingsDebugSub_003Ed__3(-2)
		{
			_003C_003E3__parms = parms
		};
	}
}
