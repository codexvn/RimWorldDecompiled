using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Dialog_ManageFoodPolicies : Dialog_ManagePolicies<FoodPolicy>
{
	[CompilerGenerated]
	private sealed class _003CHiddenSpecialThingFilters_003Ed__18 : IEnumerable<SpecialThingFilterDef>, IEnumerable, IEnumerator<SpecialThingFilterDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private SpecialThingFilterDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		SpecialThingFilterDef IEnumerator<SpecialThingFilterDef>.Current
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
		public _003CHiddenSpecialThingFilters_003Ed__18(int _003C_003E1__state)
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
				_003C_003E1__state = -1;
				_003C_003E2__current = SpecialThingFilterDefOf.AllowFresh;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<SpecialThingFilterDef> IEnumerable<SpecialThingFilterDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CHiddenSpecialThingFilters_003Ed__18(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<SpecialThingFilterDef>)this).GetEnumerator();
		}
	}

	private readonly ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

	private static ThingFilter foodGlobalFilter;

	public static ThingFilter FoodGlobalFilter
	{
		get
		{
			if (foodGlobalFilter == null)
			{
				foodGlobalFilter = new ThingFilter();
				foreach (ThingDef item in DefDatabase<ThingDef>.AllDefs.Where((ThingDef x) => x.GetStatValueAbstract(StatDefOf.Nutrition) > 0f))
				{
					foodGlobalFilter.SetAllow(item, allow: true);
				}
			}
			return foodGlobalFilter;
		}
	}

	protected override string TitleKey => "FoodPolicyTitle";

	protected override string TipKey => "FoodPolicyTip";

	public override Vector2 InitialSize => new Vector2(700f, 700f);

	public Dialog_ManageFoodPolicies(FoodPolicy policy)
		: base(policy)
	{
	}

	public override void PreOpen()
	{
		base.PreOpen();
		thingFilterState.quickSearch.Reset();
	}

	protected override FoodPolicy CreateNewPolicy()
	{
		return Current.Game.foodRestrictionDatabase.MakeNewFoodRestriction();
	}

	protected override FoodPolicy GetDefaultPolicy()
	{
		return Current.Game.foodRestrictionDatabase.DefaultFoodRestriction();
	}

	protected override void SetDefaultPolicy(FoodPolicy policy)
	{
		Current.Game.foodRestrictionDatabase.SetDefault(policy);
	}

	protected override AcceptanceReport TryDeletePolicy(FoodPolicy policy)
	{
		return Current.Game.foodRestrictionDatabase.TryDelete(policy);
	}

	protected override List<FoodPolicy> GetPolicies()
	{
		return Current.Game.foodRestrictionDatabase.AllFoodRestrictions;
	}

	protected override void DoContentsRect(Rect rect)
	{
		ThingFilterUI.DoThingFilterConfigWindow(rect, thingFilterState, base.SelectedPolicy.filter, FoodGlobalFilter, 1, null, HiddenSpecialThingFilters(), forceHideHitPointsConfig: true);
	}

	[IteratorStateMachine(typeof(_003CHiddenSpecialThingFilters_003Ed__18))]
	private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CHiddenSpecialThingFilters_003Ed__18(-2);
	}
}
