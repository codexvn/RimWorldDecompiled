using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

public class CompAnimalPenMarker : CompAutoCut, IRenameable
{
	[CompilerGenerated]
	private sealed class _003CGetAutoCutCells_003Ed__29 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompAnimalPenMarker _003C_003E4__this;

		private List<Region>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<IntVec3> _003C_003E7__wrap2;

		IntVec3 IEnumerator<IntVec3>.Current
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
		public _003CGetAutoCutCells_003Ed__29(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<Region>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompAnimalPenMarker compAnimalPenMarker = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_0092;
				}
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = compAnimalPenMarker.PenState.DirectlyConnectedRegions.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00ac;
				IL_0092:
				if (_003C_003E7__wrap2.MoveNext())
				{
					IntVec3 current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = null;
				goto IL_00ac;
				IL_00ac:
				if (_003C_003E7__wrap1.MoveNext())
				{
					Region current2 = _003C_003E7__wrap1.Current;
					_003C_003E7__wrap2 = current2.Cells.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0092;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Region>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
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
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CGetAutoCutCells_003Ed__29 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetAutoCutCells_003Ed__29(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	private const int FoodCalculatorCacheTicks = 20;

	public string label = "";

	private ThingFilter animalFilter = new ThingFilter();

	private List<ThingDef> forceDisplayedAnimalDefs = new List<ThingDef>();

	private PenFoodCalculator cachedFoodCalculator;

	private int? cachedFoodCalculator_cachedAtTick;

	public CompProperties_AnimalPenMarker Props => props as CompProperties_AnimalPenMarker;

	public PenMarkerState PenState => parent?.Map?.animalPenManager.GetPenMarkerState(this);

	public ThingFilter AnimalFilter => animalFilter;

	public List<ThingDef> ForceDisplayedAnimalDefs => forceDisplayedAnimalDefs;

	public override bool CanDesignatePlants
	{
		get
		{
			PenMarkerState penState = PenState;
			if (penState != null)
			{
				return !penState.Unenclosed;
			}
			return false;
		}
	}

	public string RenamableLabel
	{
		get
		{
			return label ?? BaseLabel;
		}
		set
		{
			label = value;
		}
	}

	public string BaseLabel => parent.def.label.CapitalizeFirst();

	public string InspectLabel => RenamableLabel;

	public PenFoodCalculator PenFoodCalculator
	{
		get
		{
			if (cachedFoodCalculator == null)
			{
				cachedFoodCalculator = new PenFoodCalculator();
				cachedFoodCalculator_cachedAtTick = null;
			}
			if (!cachedFoodCalculator_cachedAtTick.HasValue || Find.TickManager.TicksGame > cachedFoodCalculator_cachedAtTick + 20)
			{
				cachedFoodCalculator.ResetAndProcessPen(this);
				cachedFoodCalculator_cachedAtTick = Find.TickManager.TicksGame;
			}
			return cachedFoodCalculator;
		}
	}

	public void AddForceDisplayedAnimal(ThingDef animalDef)
	{
		if (!forceDisplayedAnimalDefs.Contains(animalDef))
		{
			forceDisplayedAnimalDefs.Add(animalDef);
		}
	}

	public void RemoveForceDisplayedAnimal(ThingDef animalDef)
	{
		forceDisplayedAnimalDefs.Remove(animalDef);
	}

	public bool AcceptsToPen(Pawn animal)
	{
		if (animalFilter.Allows(animal))
		{
			return AnimalPenUtility.GetFixedAnimalFilter().Allows(animal);
		}
		return false;
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		if (!respawningAfterLoad && !parent.BeingTransportedOnGravship && label.NullOrEmpty())
		{
			label = parent.Map.animalPenManager.MakeNewAnimalPenName() ?? ((string)"AnimalPenMarkerDefaultLabel".Translate(1));
			forceDisplayedAnimalDefs.Add(ThingDefOf.Cow);
			forceDisplayedAnimalDefs.Add(ThingDefOf.Goat);
			forceDisplayedAnimalDefs.Add(ThingDefOf.Chicken);
			base.AutoCutFilter.CopyAllowancesFrom(parent.Map.animalPenManager.GetDefaultAutoCutFilter());
			animalFilter.CopyAllowancesFrom(AnimalPenUtility.GetDefaultAnimalFilter());
		}
	}

	[IteratorStateMachine(typeof(_003CGetAutoCutCells_003Ed__29))]
	public override IEnumerable<IntVec3> GetAutoCutCells()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetAutoCutCells_003Ed__29(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override ThingFilter GetDefaultAutoCutFilter()
	{
		return parent.Map.animalPenManager.GetDefaultAutoCutFilter();
	}

	public override ThingFilter GetFixedAutoCutFilter()
	{
		return parent.Map.animalPenManager.GetDefaultAutoCutFilter();
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref label, "label");
		Scribe_Deep.Look(ref animalFilter, "animalFilter");
		Scribe_Collections.Look(ref forceDisplayedAnimalDefs, "forceDisplayedAnimalDefs", LookMode.Undefined);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && forceDisplayedAnimalDefs == null)
		{
			forceDisplayedAnimalDefs = new List<ThingDef>();
		}
	}

	public override string CompInspectStringExtra()
	{
		if (!parent.Spawned)
		{
			return string.Empty;
		}
		PenFoodCalculator penFoodCalculator = PenFoodCalculator;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(label).Append(": ").AppendLine(penFoodCalculator.PenSizeDescription());
		PenMarkerState penState = PenState;
		if (penState.Unenclosed)
		{
			stringBuilder.Append("PenUnenclosedLabel".Translate());
			if (penState.PassableDoors)
			{
				stringBuilder.Append(" (" + "PenOpenDoorLabel".Translate() + ")");
			}
			else
			{
				stringBuilder.Append(".");
			}
			stringBuilder.AppendLine();
		}
		else if (!penState.HasOutsideAccess)
		{
			stringBuilder.Append("NoExteriorAccess".Translate());
			stringBuilder.AppendLine();
		}
		bool num = penFoodCalculator.SumNutritionConsumptionPerDay > penFoodCalculator.NutritionPerDayToday;
		string value = PenFoodCalculator.NutritionPerDayToString(penFoodCalculator.NutritionPerDayToday, withUnits: false);
		string text = PenFoodCalculator.NutritionPerDayToString(penFoodCalculator.SumNutritionConsumptionPerDay, withUnits: false);
		if (num)
		{
			text = text.Colorize(ColorLibrary.RedReadable);
		}
		stringBuilder.Append("PenFoodTab_NaturalNutritionGrowthRate".Translate()).Append(": ").AppendLine(value);
		stringBuilder.Append("PenFoodTab_TotalNutritionConsumptionRate".Translate()).Append(": ").AppendLine(text);
		if (penFoodCalculator.sumStockpiledNutritionAvailableNow > 0f)
		{
			stringBuilder.Append("PenFoodTab_StockpileTotal".Translate()).Append(": ").AppendLine(PenFoodCalculator.NutritionToString(penFoodCalculator.sumStockpiledNutritionAvailableNow, withUnits: false));
		}
		return stringBuilder.ToString().TrimEnd();
	}
}
