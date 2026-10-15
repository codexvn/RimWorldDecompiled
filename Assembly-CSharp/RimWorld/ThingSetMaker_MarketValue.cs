using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ThingSetMaker_MarketValue : ThingSetMaker
{
	[CompilerGenerated]
	private sealed class _003CAllGeneratableThingsDebugSub_003Ed__10 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingSetMakerParams parms;

		public ThingSetMakerParams _003C_003E3__parms;

		public ThingSetMaker_MarketValue _003C_003E4__this;

		private TechLevel _003CtechLevel_003E5__2;

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
		public _003CAllGeneratableThingsDebugSub_003Ed__10(int _003C_003E1__state)
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
				ThingSetMaker_MarketValue thingSetMaker_MarketValue = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CtechLevel_003E5__2 = parms.techLevel.GetValueOrDefault();
					_003C_003E7__wrap2 = thingSetMaker_MarketValue.AllowedThingDefs(parms).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap2.Current;
					if ((!parms.maxTotalMass.HasValue || parms.maxTotalMass.GetValueOrDefault() == float.MaxValue || !(ThingSetMakerUtility.GetMinMass(current, _003CtechLevel_003E5__2) > parms.maxTotalMass)) && (!parms.totalMarketValueRange.HasValue || parms.totalMarketValueRange.Value.max == float.MaxValue || !(ThingSetMakerUtility.GetMinMarketValue(current, _003CtechLevel_003E5__2) > parms.totalMarketValueRange.Value.max)))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
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
			_003CAllGeneratableThingsDebugSub_003Ed__10 _003CAllGeneratableThingsDebugSub_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllGeneratableThingsDebugSub_003Ed__ = this;
			}
			else
			{
				_003CAllGeneratableThingsDebugSub_003Ed__ = new _003CAllGeneratableThingsDebugSub_003Ed__10(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
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

	private int nextSeed;

	public ThingSetMaker_MarketValue()
	{
		nextSeed = Rand.Int;
	}

	protected override bool CanGenerateSub(ThingSetMakerParams parms)
	{
		List<ThingDef> list = AllowedThingDefs(parms).ToList();
		if (!list.Any())
		{
			return false;
		}
		if (parms.countRange.HasValue && parms.countRange.Value.max <= 0)
		{
			return false;
		}
		if (!parms.totalMarketValueRange.HasValue || parms.totalMarketValueRange.Value.max <= 0f)
		{
			return false;
		}
		if (parms.maxTotalMass.HasValue && parms.maxTotalMass.GetValueOrDefault() != float.MaxValue && !ThingSetMakerUtility.PossibleToWeighNoMoreThan(list, parms.techLevel.GetValueOrDefault(), parms.maxTotalMass.Value, (!parms.countRange.HasValue) ? 1 : parms.countRange.Value.min))
		{
			return false;
		}
		if (!GeneratePossibleDefs(parms, out var _, nextSeed).Any())
		{
			return false;
		}
		return true;
	}

	protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
	{
		float maxMass = parms.maxTotalMass ?? float.MaxValue;
		float totalMarketValue;
		List<ThingStuffPairWithQuality> list = GeneratePossibleDefs(parms, out totalMarketValue, nextSeed);
		for (int i = 0; i < list.Count; i++)
		{
			outThings.Add(list[i].MakeThing());
		}
		ThingSetMakerByTotalStatUtility.IncreaseStackCountsToTotalValue(outThings, totalMarketValue, (Thing x) => x.MarketValue, maxMass, satisfyMinRewardCount: true);
		nextSeed++;
	}

	protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
	{
		return ThingSetMakerUtility.GetAllowedThingDefs(parms);
	}

	private float GetSingleThingValue(ThingStuffPairWithQuality thingStuffPair)
	{
		return thingStuffPair.GetStatValue(StatDefOf.MarketValue);
	}

	private float GetMinValue(ThingStuffPairWithQuality thingStuffPair)
	{
		return thingStuffPair.GetStatValue(StatDefOf.MarketValue) * (float)thingStuffPair.thing.minRewardCount;
	}

	private float GetMaxValue(ThingStuffPairWithQuality thingStuffPair)
	{
		return thingStuffPair.GetStatValue(StatDefOf.MarketValue) * (float)thingStuffPair.thing.stackLimit;
	}

	private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalMarketValue, int seed)
	{
		Rand.PushState(seed);
		List<ThingStuffPairWithQuality> result = GeneratePossibleDefs(parms, out totalMarketValue);
		Rand.PopState();
		return result;
	}

	private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalMarketValue)
	{
		IEnumerable<ThingDef> enumerable = AllowedThingDefs(parms);
		if (!enumerable.Any())
		{
			totalMarketValue = 0f;
			return new List<ThingStuffPairWithQuality>();
		}
		TechLevel valueOrDefault = parms.techLevel.GetValueOrDefault();
		IntRange countRange = parms.countRange ?? new IntRange(1, int.MaxValue);
		FloatRange floatRange = parms.totalMarketValueRange ?? FloatRange.Zero;
		float maxMass = parms.maxTotalMass ?? float.MaxValue;
		QualityGenerator valueOrDefault2 = parms.qualityGenerator.GetValueOrDefault();
		totalMarketValue = floatRange.RandomInRange;
		return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue(countRange, totalMarketValue, enumerable, valueOrDefault, valueOrDefault2, GetMinValue, GetMaxValue, GetSingleThingValue, null, 100, maxMass, parms.allowNonStackableDuplicates ?? true, totalMarketValue * parms.minSingleItemMarketValuePct.GetValueOrDefault());
	}

	[IteratorStateMachine(typeof(_003CAllGeneratableThingsDebugSub_003Ed__10))]
	protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllGeneratableThingsDebugSub_003Ed__10(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parms = parms
		};
	}
}
