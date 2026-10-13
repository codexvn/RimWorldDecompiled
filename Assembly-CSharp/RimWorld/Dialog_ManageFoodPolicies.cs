using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020031E9 RID: 12777
	public class Dialog_ManageFoodPolicies : Dialog_ManagePolicies<FoodPolicy>
	{
		// Token: 0x17002EA6 RID: 11942
		// (get) Token: 0x06011F7D RID: 73597 RVA: 0x0053B778 File Offset: 0x00539978
		public static ThingFilter FoodGlobalFilter
		{
			get
			{
				if (Dialog_ManageFoodPolicies.foodGlobalFilter == null)
				{
					Dialog_ManageFoodPolicies.foodGlobalFilter = new ThingFilter();
					foreach (ThingDef thingDef in from x in DefDatabase<ThingDef>.AllDefs
					where x.GetStatValueAbstract(StatDefOf.Nutrition, null) > 0f
					select x)
					{
						Dialog_ManageFoodPolicies.foodGlobalFilter.SetAllow(thingDef, true);
					}
				}
				return Dialog_ManageFoodPolicies.foodGlobalFilter;
			}
		}

		// Token: 0x17002EA7 RID: 11943
		// (get) Token: 0x06011F7E RID: 73598 RVA: 0x0053B804 File Offset: 0x00539A04
		protected override string TitleKey
		{
			get
			{
				return "FoodPolicyTitle";
			}
		}

		// Token: 0x17002EA8 RID: 11944
		// (get) Token: 0x06011F7F RID: 73599 RVA: 0x0053B80B File Offset: 0x00539A0B
		protected override string TipKey
		{
			get
			{
				return "FoodPolicyTip";
			}
		}

		// Token: 0x17002EA9 RID: 11945
		// (get) Token: 0x06011F80 RID: 73600 RVA: 0x0053AD44 File Offset: 0x00538F44
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 700f);
			}
		}

		// Token: 0x06011F81 RID: 73601 RVA: 0x0053B812 File Offset: 0x00539A12
		public Dialog_ManageFoodPolicies(FoodPolicy policy) : base(policy)
		{
		}

		// Token: 0x06011F82 RID: 73602 RVA: 0x0053B826 File Offset: 0x00539A26
		public override void PreOpen()
		{
			base.PreOpen();
			this.thingFilterState.quickSearch.Reset();
		}

		// Token: 0x06011F83 RID: 73603 RVA: 0x0053B83E File Offset: 0x00539A3E
		protected override FoodPolicy CreateNewPolicy()
		{
			return Current.Game.foodRestrictionDatabase.MakeNewFoodRestriction();
		}

		// Token: 0x06011F84 RID: 73604 RVA: 0x0053B84F File Offset: 0x00539A4F
		protected override FoodPolicy GetDefaultPolicy()
		{
			return Current.Game.foodRestrictionDatabase.DefaultFoodRestriction();
		}

		// Token: 0x06011F85 RID: 73605 RVA: 0x0053B860 File Offset: 0x00539A60
		protected override void SetDefaultPolicy(FoodPolicy policy)
		{
			Current.Game.foodRestrictionDatabase.SetDefault(policy);
		}

		// Token: 0x06011F86 RID: 73606 RVA: 0x0053B872 File Offset: 0x00539A72
		protected override AcceptanceReport TryDeletePolicy(FoodPolicy policy)
		{
			return Current.Game.foodRestrictionDatabase.TryDelete(policy);
		}

		// Token: 0x06011F87 RID: 73607 RVA: 0x0053B884 File Offset: 0x00539A84
		protected override List<FoodPolicy> GetPolicies()
		{
			return Current.Game.foodRestrictionDatabase.AllFoodRestrictions;
		}

		// Token: 0x06011F88 RID: 73608 RVA: 0x0053B898 File Offset: 0x00539A98
		protected override void DoContentsRect(Rect rect)
		{
			ThingFilterUI.DoThingFilterConfigWindow(rect, this.thingFilterState, base.SelectedPolicy.filter, Dialog_ManageFoodPolicies.FoodGlobalFilter, 1, null, this.HiddenSpecialThingFilters(), true, false, false, null, null);
		}

		// Token: 0x06011F89 RID: 73609 RVA: 0x0053B8CE File Offset: 0x00539ACE
		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			yield return SpecialThingFilterDefOf.AllowFresh;
			yield break;
		}

		// Token: 0x0400BA20 RID: 47648
		private readonly ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

		// Token: 0x0400BA21 RID: 47649
		private static ThingFilter foodGlobalFilter;
	}
}
