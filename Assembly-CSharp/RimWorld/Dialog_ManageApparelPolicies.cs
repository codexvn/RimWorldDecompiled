using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020031E6 RID: 12774
	public class Dialog_ManageApparelPolicies : Dialog_ManagePolicies<ApparelPolicy>
	{
		// Token: 0x17002E9C RID: 11932
		// (get) Token: 0x06011F57 RID: 73559 RVA: 0x0053AD0C File Offset: 0x00538F0C
		private static ThingFilter ApparelGlobalFilter
		{
			get
			{
				if (Dialog_ManageApparelPolicies.apparelGlobalFilter == null)
				{
					Dialog_ManageApparelPolicies.apparelGlobalFilter = new ThingFilter();
					Dialog_ManageApparelPolicies.apparelGlobalFilter.SetAllow(ThingCategoryDefOf.Apparel, true, null, null);
				}
				return Dialog_ManageApparelPolicies.apparelGlobalFilter;
			}
		}

		// Token: 0x17002E9D RID: 11933
		// (get) Token: 0x06011F58 RID: 73560 RVA: 0x0053AD36 File Offset: 0x00538F36
		protected override string TitleKey
		{
			get
			{
				return "ApparelPolicyTitle";
			}
		}

		// Token: 0x17002E9E RID: 11934
		// (get) Token: 0x06011F59 RID: 73561 RVA: 0x0053AD3D File Offset: 0x00538F3D
		protected override string TipKey
		{
			get
			{
				return "ApparelPolicyTip";
			}
		}

		// Token: 0x17002E9F RID: 11935
		// (get) Token: 0x06011F5A RID: 73562 RVA: 0x0053AD44 File Offset: 0x00538F44
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 700f);
			}
		}

		// Token: 0x06011F5B RID: 73563 RVA: 0x0053AD55 File Offset: 0x00538F55
		public Dialog_ManageApparelPolicies(ApparelPolicy policy) : base(policy)
		{
		}

		// Token: 0x06011F5C RID: 73564 RVA: 0x0053AD69 File Offset: 0x00538F69
		public override void PreOpen()
		{
			base.PreOpen();
			this.thingFilterState.quickSearch.Reset();
		}

		// Token: 0x06011F5D RID: 73565 RVA: 0x0053AD81 File Offset: 0x00538F81
		protected override ApparelPolicy CreateNewPolicy()
		{
			return Current.Game.outfitDatabase.MakeNewOutfit();
		}

		// Token: 0x06011F5E RID: 73566 RVA: 0x0053AD92 File Offset: 0x00538F92
		protected override ApparelPolicy GetDefaultPolicy()
		{
			return Current.Game.outfitDatabase.DefaultOutfit();
		}

		// Token: 0x06011F5F RID: 73567 RVA: 0x0053ADA3 File Offset: 0x00538FA3
		protected override void SetDefaultPolicy(ApparelPolicy policy)
		{
			Current.Game.outfitDatabase.SetDefault(policy);
		}

		// Token: 0x06011F60 RID: 73568 RVA: 0x0053ADB5 File Offset: 0x00538FB5
		protected override AcceptanceReport TryDeletePolicy(ApparelPolicy policy)
		{
			return Current.Game.outfitDatabase.TryDelete(policy);
		}

		// Token: 0x06011F61 RID: 73569 RVA: 0x0053ADC7 File Offset: 0x00538FC7
		protected override List<ApparelPolicy> GetPolicies()
		{
			return Current.Game.outfitDatabase.AllOutfits;
		}

		// Token: 0x06011F62 RID: 73570 RVA: 0x0053ADD8 File Offset: 0x00538FD8
		protected override void DoContentsRect(Rect rect)
		{
			ThingFilterUI.DoThingFilterConfigWindow(rect, this.thingFilterState, base.SelectedPolicy.filter, Dialog_ManageApparelPolicies.ApparelGlobalFilter, 16, null, this.HiddenSpecialThingFilters(), false, false, false, null, null);
		}

		// Token: 0x06011F63 RID: 73571 RVA: 0x0053AE0F File Offset: 0x0053900F
		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			yield return SpecialThingFilterDefOf.AllowNonDeadmansApparel;
			if (ModsConfig.IdeologyActive)
			{
				yield return SpecialThingFilterDefOf.AllowVegetarian;
				yield return SpecialThingFilterDefOf.AllowCarnivore;
				yield return SpecialThingFilterDefOf.AllowCannibal;
				yield return SpecialThingFilterDefOf.AllowInsectMeat;
			}
			yield break;
		}

		// Token: 0x0400BA11 RID: 47633
		private readonly ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

		// Token: 0x0400BA12 RID: 47634
		private static ThingFilter apparelGlobalFilter;
	}
}
