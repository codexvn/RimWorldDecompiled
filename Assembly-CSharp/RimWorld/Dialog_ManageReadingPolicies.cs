using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020031EF RID: 12783
	public class Dialog_ManageReadingPolicies : Dialog_ManagePolicies<ReadingPolicy>
	{
		// Token: 0x17002EB1 RID: 11953
		// (get) Token: 0x06011FAD RID: 73645 RVA: 0x0053C2B0 File Offset: 0x0053A4B0
		private static ThingFilter PolicyGlobalFilter
		{
			get
			{
				if (Dialog_ManageReadingPolicies.policyGlobalFilter == null)
				{
					Dialog_ManageReadingPolicies.policyGlobalFilter = new ThingFilter();
					foreach (ThingDef thingDef in from x in DefDatabase<ThingDef>.AllDefs
					where x.HasComp<CompBook>()
					select x)
					{
						Dialog_ManageReadingPolicies.policyGlobalFilter.SetAllow(thingDef, true);
					}
				}
				return Dialog_ManageReadingPolicies.policyGlobalFilter;
			}
		}

		// Token: 0x17002EB2 RID: 11954
		// (get) Token: 0x06011FAE RID: 73646 RVA: 0x0053C33C File Offset: 0x0053A53C
		protected override string TitleKey
		{
			get
			{
				return "ReadingPolicyTitle";
			}
		}

		// Token: 0x17002EB3 RID: 11955
		// (get) Token: 0x06011FAF RID: 73647 RVA: 0x0053C343 File Offset: 0x0053A543
		protected override string TipKey
		{
			get
			{
				return "ReadingPolicyTip";
			}
		}

		// Token: 0x17002EB4 RID: 11956
		// (get) Token: 0x06011FB0 RID: 73648 RVA: 0x0053C34A File Offset: 0x0053A54A
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(860f, 700f);
			}
		}

		// Token: 0x06011FB1 RID: 73649 RVA: 0x0053C35B File Offset: 0x0053A55B
		public Dialog_ManageReadingPolicies(ReadingPolicy policy) : base(policy)
		{
		}

		// Token: 0x06011FB2 RID: 73650 RVA: 0x0053C37A File Offset: 0x0053A57A
		public override void PreOpen()
		{
			base.PreOpen();
			this.thingFilterState.quickSearch.Reset();
			this.effectFilterState.quickSearch.Reset();
		}

		// Token: 0x06011FB3 RID: 73651 RVA: 0x0053C3A2 File Offset: 0x0053A5A2
		protected override ReadingPolicy CreateNewPolicy()
		{
			return Current.Game.readingPolicyDatabase.MakeNewReadingPolicy();
		}

		// Token: 0x06011FB4 RID: 73652 RVA: 0x0053C3B3 File Offset: 0x0053A5B3
		protected override ReadingPolicy GetDefaultPolicy()
		{
			return Current.Game.readingPolicyDatabase.DefaultReadingPolicy();
		}

		// Token: 0x06011FB5 RID: 73653 RVA: 0x0053C3C4 File Offset: 0x0053A5C4
		protected override void SetDefaultPolicy(ReadingPolicy policy)
		{
			Current.Game.readingPolicyDatabase.SetDefault(policy);
		}

		// Token: 0x06011FB6 RID: 73654 RVA: 0x0053C3D6 File Offset: 0x0053A5D6
		protected override AcceptanceReport TryDeletePolicy(ReadingPolicy policy)
		{
			return Current.Game.readingPolicyDatabase.TryDelete(policy);
		}

		// Token: 0x06011FB7 RID: 73655 RVA: 0x0053C3E8 File Offset: 0x0053A5E8
		protected override List<ReadingPolicy> GetPolicies()
		{
			return Current.Game.readingPolicyDatabase.AllReadingPolicies;
		}

		// Token: 0x06011FB8 RID: 73656 RVA: 0x0053C3FC File Offset: 0x0053A5FC
		protected override void DoContentsRect(Rect rect)
		{
			Rect rect2;
			Rect rect3;
			rect.SplitVerticallyWithMargin(out rect2, out rect3, 10f);
			ThingFilterUI.DoThingFilterConfigWindow(rect2, this.thingFilterState, base.SelectedPolicy.defFilter, Dialog_ManageReadingPolicies.PolicyGlobalFilter, 1, null, null, true, false, false, null, null);
			ThingFilterUI.DoThingFilterConfigWindow(rect3, this.effectFilterState, base.SelectedPolicy.effectFilter, null, 1, null, null, true, true, true, null, null);
		}

		// Token: 0x0400BA31 RID: 47665
		private readonly ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

		// Token: 0x0400BA32 RID: 47666
		private readonly ThingFilterUI.UIState effectFilterState = new ThingFilterUI.UIState();

		// Token: 0x0400BA33 RID: 47667
		private static ThingFilter policyGlobalFilter;
	}
}
