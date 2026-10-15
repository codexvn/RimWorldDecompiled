using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class TraderKindDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__15 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TraderKindDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<StockGenerator>.Enumerator _003C_003E7__wrap2;

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
		public _003CConfigErrors_003Ed__15(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -5:
			case -4:
			case 2:
				try
				{
					if (num == -5 || num == 2)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally3();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<StockGenerator>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				TraderKindDef traderKindDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)traderKindDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -5;
						goto IL_00f9;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = traderKindDef.stockGenerators.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0113;
					IL_0113:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StockGenerator current2 = _003C_003E7__wrap2.Current;
						_003C_003E7__wrap1 = current2.ConfigErrors(traderKindDef).GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_00f9;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<StockGenerator>.Enumerator);
					return false;
					IL_00f9:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					goto IL_0113;
				}
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
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
			_003CConfigErrors_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__15(0)
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

	public List<StockGenerator> stockGenerators = new List<StockGenerator>();

	public bool orbital;

	public bool requestable = true;

	public bool hideThingsNotWillingToTrade;

	public float commonality = 1f;

	public string category;

	public TradeCurrency tradeCurrency;

	public SimpleCurve commonalityMultFromPopulationIntent;

	public FactionDef faction;

	public RoyalTitlePermitDef permitRequiredForTrading;

	public float CalculatedCommonality
	{
		get
		{
			float num = commonality;
			if (commonalityMultFromPopulationIntent != null)
			{
				num *= commonalityMultFromPopulationIntent.Evaluate(StorytellerUtilityPopulation.PopulationIntent);
			}
			return num;
		}
	}

	public RoyalTitleDef TitleRequiredToTrade
	{
		get
		{
			if (permitRequiredForTrading != null)
			{
				RoyalTitleDef royalTitleDef = faction.RoyalTitlesAwardableInSeniorityOrderForReading.FirstOrDefault((RoyalTitleDef x) => x.permits != null && x.permits.Contains(permitRequiredForTrading));
				if (royalTitleDef != null)
				{
					return royalTitleDef;
				}
			}
			return null;
		}
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		foreach (StockGenerator stockGenerator in stockGenerators)
		{
			stockGenerator.ResolveReferences(this);
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__15))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}

	public bool WillTrade(ThingDef td)
	{
		for (int i = 0; i < stockGenerators.Count; i++)
		{
			if (stockGenerators[i].HandlesThingDef(td))
			{
				return true;
			}
		}
		return false;
	}

	public PriceType PriceTypeFor(ThingDef thingDef, TradeAction action)
	{
		if (thingDef == ThingDefOf.Silver)
		{
			return PriceType.Undefined;
		}
		if (action == TradeAction.PlayerBuys)
		{
			for (int i = 0; i < stockGenerators.Count; i++)
			{
				if (stockGenerators[i].TryGetPriceType(thingDef, action, out var priceType))
				{
					return priceType;
				}
			}
		}
		return PriceType.Normal;
	}
}
