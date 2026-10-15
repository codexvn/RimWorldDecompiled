using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld;

public class ThingSetMaker_RandomGeneralGoods : ThingSetMaker
{
	private enum GoodsType
	{
		None,
		Meals,
		RawFood,
		Medicine,
		Drugs,
		Resources
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public TechLevel techLevel;

		public Func<ThingDef, bool> _003C_003E9__1;

		internal bool _003CAllGeneratableThingsDebugSub_003Eb__1(ThingDef x)
		{
			if (x.IsDrug)
			{
				return (int)x.techLevel <= (int)techLevel;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CAllGeneratableThingsDebugSub_003Ed__10 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingSetMakerParams parms;

		public ThingSetMakerParams _003C_003E3__parms;

		public ThingSetMaker_RandomGeneralGoods _003C_003E4__this;

		private _003C_003Ec__DisplayClass10_0 _003C_003E8__1;

		private IEnumerator<ThingDef> _003C_003E7__wrap1;

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
			switch (_003C_003E1__state)
			{
			case -3:
			case 5:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 6:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 7:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -6:
			case 9:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ThingSetMaker_RandomGeneralGoods thingSetMaker_RandomGeneralGoods = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass10_0();
					_003C_003E8__1.techLevel = parms.techLevel.GetValueOrDefault();
					if (_003C_003E8__1.techLevel.IsNeolithicOrWorse())
					{
						_003C_003E2__current = ThingDefOf.Pemmican;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003E2__current = ThingDefOf.MealSimple;
					_003C_003E1__state = 2;
					return true;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0104;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = ThingDefOf.MealFine;
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = ThingDefOf.MealSurvivalPack;
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					goto IL_0104;
				case 5:
					_003C_003E1__state = -3;
					goto IL_0153;
				case 6:
					_003C_003E1__state = -4;
					goto IL_01d6;
				case 7:
					_003C_003E1__state = -5;
					goto IL_026a;
				case 8:
					_003C_003E1__state = -1;
					break;
				case 9:
					{
						_003C_003E1__state = -6;
						goto IL_0333;
					}
					IL_0104:
					_003C_003E7__wrap1 = thingSetMaker_RandomGeneralGoods.PossibleRawFood(_003C_003E8__1.techLevel).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0153;
					IL_0153:
					if (_003C_003E7__wrap1.MoveNext())
					{
						ThingDef current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 5;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = ThingSetMakerUtility.allGeneratableItems.Where((ThingDef x) => x.IsMedicine).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_01d6;
					IL_0333:
					if (_003C_003E7__wrap1.MoveNext())
					{
						ThingDef current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 9;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap1 = null;
					break;
					IL_026a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						ThingDef current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 7;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					if (_003C_003E8__1.techLevel.IsNeolithicOrWorse())
					{
						_003C_003E2__current = ThingDefOf.WoodLog;
						_003C_003E1__state = 8;
						return true;
					}
					_003C_003E7__wrap1 = (from d in GenStuff.AllowedStuffsFor(ThingDefOf.Wall, _003C_003E8__1.techLevel)
						where d.BaseMarketValue / d.VolumePerUnit < 5f
						select d).GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_0333;
					IL_01d6:
					if (_003C_003E7__wrap1.MoveNext())
					{
						ThingDef current4 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 6;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = ThingSetMakerUtility.allGeneratableItems.Where((ThingDef x) => x.IsDrug && (int)x.techLevel <= (int)_003C_003E8__1.techLevel).GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_026a;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
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

	private static Pair<GoodsType, float>[] GoodsWeights = new Pair<GoodsType, float>[5]
	{
		new Pair<GoodsType, float>(GoodsType.Meals, 1f),
		new Pair<GoodsType, float>(GoodsType.RawFood, 0.75f),
		new Pair<GoodsType, float>(GoodsType.Medicine, 0.234f),
		new Pair<GoodsType, float>(GoodsType.Drugs, 0.234f),
		new Pair<GoodsType, float>(GoodsType.Resources, 0.234f)
	};

	protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
	{
		IntRange intRange = parms.countRange ?? new IntRange(10, 20);
		TechLevel valueOrDefault = parms.techLevel.GetValueOrDefault();
		int num = Mathf.Max(intRange.RandomInRange, 1);
		for (int i = 0; i < num; i++)
		{
			outThings.Add(GenerateSingle(valueOrDefault));
		}
	}

	private Thing GenerateSingle(TechLevel techLevel)
	{
		Thing thing = null;
		int num = 0;
		while (thing == null && num < 50)
		{
			thing = GoodsWeights.RandomElementByWeight((Pair<GoodsType, float> x) => x.Second).First switch
			{
				GoodsType.Meals => RandomMeals(techLevel), 
				GoodsType.RawFood => RandomRawFood(techLevel), 
				GoodsType.Medicine => RandomMedicine(techLevel), 
				GoodsType.Drugs => RandomDrugs(techLevel), 
				GoodsType.Resources => RandomResources(techLevel), 
				_ => throw new Exception(), 
			};
			num++;
		}
		return thing;
	}

	private Thing RandomMeals(TechLevel techLevel)
	{
		ThingDef thingDef;
		if (techLevel.IsNeolithicOrWorse())
		{
			thingDef = ThingDefOf.Pemmican;
		}
		else
		{
			float value = Rand.Value;
			thingDef = ((value < 0.5f) ? ThingDefOf.MealSimple : ((!((double)value < 0.75)) ? ThingDefOf.MealSurvivalPack : ThingDefOf.MealFine));
		}
		Thing thing = ThingMaker.MakeThing(thingDef);
		int num = Mathf.Min(thingDef.stackLimit, 10);
		thing.stackCount = Rand.RangeInclusive(num / 2, num);
		return thing;
	}

	private Thing RandomRawFood(TechLevel techLevel)
	{
		if (!PossibleRawFood(techLevel).TryRandomElement(out var result))
		{
			return null;
		}
		Thing thing = ThingMaker.MakeThing(result);
		int maxInclusive = Mathf.Min(result.stackLimit, 75);
		thing.stackCount = Rand.RangeInclusive(1, maxInclusive);
		return thing;
	}

	private IEnumerable<ThingDef> PossibleRawFood(TechLevel techLevel)
	{
		return ThingSetMakerUtility.allGeneratableItems.Where((ThingDef x) => x.IsNutritionGivingIngestible && !x.IsCorpse && x.ingestible.HumanEdible && !x.HasComp(typeof(CompHatcher)) && (int)x.techLevel <= (int)techLevel && (int)x.ingestible.preferability < 7);
	}

	private Thing RandomMedicine(TechLevel techLevel)
	{
		ThingDef result;
		if (Rand.Value < 0.75f && (int)techLevel >= (int)ThingDefOf.MedicineHerbal.techLevel)
		{
			result = ThingSetMakerUtility.allGeneratableItems.Where((ThingDef x) => x.IsMedicine && (int)x.techLevel <= (int)techLevel).MaxBy((ThingDef x) => x.GetStatValueAbstract(StatDefOf.MedicalPotency));
		}
		else if (!ThingSetMakerUtility.allGeneratableItems.Where((ThingDef x) => x.IsMedicine).TryRandomElement(out result))
		{
			return null;
		}
		if (techLevel.IsNeolithicOrWorse())
		{
			result = ThingDefOf.MedicineHerbal;
		}
		Thing thing = ThingMaker.MakeThing(result);
		int maxInclusive = Mathf.Min(result.stackLimit, 20);
		thing.stackCount = Rand.RangeInclusive(1, maxInclusive);
		return thing;
	}

	private Thing RandomDrugs(TechLevel techLevel)
	{
		if (!ThingSetMakerUtility.allGeneratableItems.Where((ThingDef x) => x.IsDrug && (int)x.techLevel <= (int)techLevel).TryRandomElement(out var result))
		{
			return null;
		}
		Thing thing = ThingMaker.MakeThing(result);
		int maxInclusive = Mathf.Min(result.stackLimit, 25);
		thing.stackCount = Rand.RangeInclusive(1, maxInclusive);
		return thing;
	}

	private Thing RandomResources(TechLevel techLevel)
	{
		ThingDef thingDef = BaseGenUtility.RandomCheapWallStuff(techLevel);
		Thing thing = ThingMaker.MakeThing(thingDef);
		int num = Mathf.Min(thingDef.stackLimit, 75);
		thing.stackCount = Rand.RangeInclusive(num / 2, num);
		return thing;
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
