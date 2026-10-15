using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ThingSetMaker_Nutrition : ThingSetMaker
{
	[CompilerGenerated]
	private sealed class _003CAllGeneratableThingsDebugSub_003Ed__7 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingSetMakerParams parms;

		public ThingSetMakerParams _003C_003E3__parms;

		public ThingSetMaker_Nutrition _003C_003E4__this;

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
		public _003CAllGeneratableThingsDebugSub_003Ed__7(int _003C_003E1__state)
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
				ThingSetMaker_Nutrition thingSetMaker_Nutrition = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CtechLevel_003E5__2 = parms.techLevel.GetValueOrDefault();
					_003C_003E7__wrap2 = thingSetMaker_Nutrition.AllowedThingDefs(parms).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap2.Current;
					if ((!parms.maxTotalMass.HasValue || parms.maxTotalMass.GetValueOrDefault() == float.MaxValue || !(ThingSetMakerUtility.GetMinMass(current, _003CtechLevel_003E5__2) > parms.maxTotalMass)) && (!parms.totalNutritionRange.HasValue || parms.totalNutritionRange.Value.max == float.MaxValue || !current.IsNutritionGivingIngestible || !(current.ingestible.CachedNutrition > parms.totalNutritionRange.Value.max)))
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
			_003CAllGeneratableThingsDebugSub_003Ed__7 _003CAllGeneratableThingsDebugSub_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllGeneratableThingsDebugSub_003Ed__ = this;
			}
			else
			{
				_003CAllGeneratableThingsDebugSub_003Ed__ = new _003CAllGeneratableThingsDebugSub_003Ed__7(0)
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

	public ThingSetMaker_Nutrition()
	{
		nextSeed = Rand.Int;
	}

	protected override bool CanGenerateSub(ThingSetMakerParams parms)
	{
		if (!AllowedThingDefs(parms).Any())
		{
			return false;
		}
		if (parms.countRange.HasValue && parms.countRange.Value.max <= 0)
		{
			return false;
		}
		if (!parms.totalNutritionRange.HasValue || parms.totalNutritionRange.Value.max <= 0f)
		{
			return false;
		}
		if (parms.maxTotalMass.HasValue && parms.maxTotalMass.GetValueOrDefault() != float.MaxValue && !ThingSetMakerUtility.PossibleToWeighNoMoreThan(AllowedThingDefs(parms), parms.techLevel.GetValueOrDefault(), parms.maxTotalMass.Value, (!parms.countRange.HasValue) ? 1 : parms.countRange.Value.min))
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
		float totalNutrition;
		List<ThingStuffPairWithQuality> list = GeneratePossibleDefs(parms, out totalNutrition, nextSeed);
		for (int i = 0; i < list.Count; i++)
		{
			outThings.Add(list[i].MakeThing());
		}
		ThingSetMakerByTotalStatUtility.IncreaseStackCountsToTotalValue(outThings, totalNutrition, (Thing x) => x.GetStatValue(StatDefOf.Nutrition), maxMass);
		nextSeed++;
	}

	protected virtual IEnumerable<ThingDef> AllowedThingDefs(ThingSetMakerParams parms)
	{
		return ThingSetMakerUtility.GetAllowedThingDefs(parms);
	}

	private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalNutrition, int seed)
	{
		Rand.PushState(seed);
		List<ThingStuffPairWithQuality> result = GeneratePossibleDefs(parms, out totalNutrition);
		Rand.PopState();
		return result;
	}

	private List<ThingStuffPairWithQuality> GeneratePossibleDefs(ThingSetMakerParams parms, out float totalNutrition)
	{
		IEnumerable<ThingDef> enumerable = AllowedThingDefs(parms);
		if (!enumerable.Any())
		{
			totalNutrition = 0f;
			return new List<ThingStuffPairWithQuality>();
		}
		IntRange countRange = parms.countRange ?? new IntRange(1, int.MaxValue);
		FloatRange floatRange = parms.totalNutritionRange ?? FloatRange.Zero;
		TechLevel valueOrDefault = parms.techLevel.GetValueOrDefault();
		float maxMass = parms.maxTotalMass ?? float.MaxValue;
		QualityGenerator valueOrDefault2 = parms.qualityGenerator.GetValueOrDefault();
		totalNutrition = floatRange.RandomInRange;
		int numMeats = enumerable.Count((ThingDef x) => x.IsMeat);
		int numLeathers = enumerable.Count((ThingDef x) => x.IsLeather);
		int numEggs = enumerable.Count((ThingDef x) => x.IsEgg);
		return ThingSetMakerByTotalStatUtility.GenerateDefsWithPossibleTotalValue(countRange, totalNutrition, enumerable, valueOrDefault, valueOrDefault2, (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition), (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition) * (float)x.thing.stackLimit, (ThingStuffPairWithQuality x) => x.GetStatValue(StatDefOf.Nutrition), WeightSelector, 100, maxMass);
		float WeightSelector(ThingDef x)
		{
			return ThingSetMakerUtility.AdjustedBigCategoriesSelectionWeight(x, numMeats, numLeathers, numEggs);
		}
	}

	[IteratorStateMachine(typeof(_003CAllGeneratableThingsDebugSub_003Ed__7))]
	protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllGeneratableThingsDebugSub_003Ed__7(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parms = parms
		};
	}
}
