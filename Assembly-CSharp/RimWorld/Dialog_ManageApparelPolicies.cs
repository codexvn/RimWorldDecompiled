using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Dialog_ManageApparelPolicies : Dialog_ManagePolicies<ApparelPolicy>
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
				_003C_003E2__current = SpecialThingFilterDefOf.AllowNonDeadmansApparel;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (ModsConfig.IdeologyActive)
				{
					_003C_003E2__current = SpecialThingFilterDefOf.AllowVegetarian;
					_003C_003E1__state = 2;
					return true;
				}
				break;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = SpecialThingFilterDefOf.AllowCarnivore;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = SpecialThingFilterDefOf.AllowCannibal;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = SpecialThingFilterDefOf.AllowInsectMeat;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				break;
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

	private static ThingFilter apparelGlobalFilter;

	private static ThingFilter ApparelGlobalFilter
	{
		get
		{
			if (apparelGlobalFilter == null)
			{
				apparelGlobalFilter = new ThingFilter();
				apparelGlobalFilter.SetAllow(ThingCategoryDefOf.Apparel, allow: true);
			}
			return apparelGlobalFilter;
		}
	}

	protected override string TitleKey => "ApparelPolicyTitle";

	protected override string TipKey => "ApparelPolicyTip";

	public override Vector2 InitialSize => new Vector2(700f, 700f);

	public Dialog_ManageApparelPolicies(ApparelPolicy policy)
		: base(policy)
	{
	}

	public override void PreOpen()
	{
		base.PreOpen();
		thingFilterState.quickSearch.Reset();
	}

	protected override ApparelPolicy CreateNewPolicy()
	{
		return Current.Game.outfitDatabase.MakeNewOutfit();
	}

	protected override ApparelPolicy GetDefaultPolicy()
	{
		return Current.Game.outfitDatabase.DefaultOutfit();
	}

	protected override void SetDefaultPolicy(ApparelPolicy policy)
	{
		Current.Game.outfitDatabase.SetDefault(policy);
	}

	protected override AcceptanceReport TryDeletePolicy(ApparelPolicy policy)
	{
		return Current.Game.outfitDatabase.TryDelete(policy);
	}

	protected override List<ApparelPolicy> GetPolicies()
	{
		return Current.Game.outfitDatabase.AllOutfits;
	}

	protected override void DoContentsRect(Rect rect)
	{
		ThingFilterUI.DoThingFilterConfigWindow(rect, thingFilterState, base.SelectedPolicy.filter, ApparelGlobalFilter, 16, null, HiddenSpecialThingFilters());
	}

	[IteratorStateMachine(typeof(_003CHiddenSpecialThingFilters_003Ed__18))]
	private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CHiddenSpecialThingFilters_003Ed__18(-2);
	}
}
