using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000D6A RID: 3434
	public static class ThingFilterUI
	{
		// Token: 0x06005770 RID: 22384 RVA: 0x001C2BCC File Offset: 0x001C0DCC
		public static void DoThingFilterConfigWindow(Rect rect, ThingFilterUI.UIState state, ThingFilter filter, ThingFilter parentFilter = null, int openMask = 1, IEnumerable<ThingDef> forceHiddenDefs = null, IEnumerable<SpecialThingFilterDef> forceHiddenFilters = null, bool forceHideHitPointsConfig = false, bool forceHideQualityConfig = false, bool showMentalBreakChanceRange = false, List<ThingDef> suppressSmallVolumeTags = null, Map map = null)
		{
			Widgets.DrawMenuSection(rect);
			float num = rect.width - 2f;
			Rect rect2 = new Rect(rect.x + 3f, rect.y + 3f, num / 2f - 3f - 1.5f, 24f);
			if (Widgets.ButtonText(rect2, "ClearAll".Translate(), true, true, true, null))
			{
				filter.SetDisallowAll(forceHiddenDefs, forceHiddenFilters);
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect2.xMax + 3f, rect2.y, rect2.width, 24f), "AllowAll".Translate(), true, true, true, null))
			{
				filter.SetAllowAll(parentFilter, false);
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
			}
			rect.yMin = rect2.yMax;
			Rect rect3 = new Rect(rect.x + 3f, rect.y + 3f, rect.width - 16f - 6f, 24f);
			state.quickSearch.OnGUI(rect3, null, null);
			rect.yMin = rect3.yMax + 3f;
			TreeNode_ThingCategory node = filter.RootNode;
			bool flag = true;
			bool flag2 = true;
			if (parentFilter != null)
			{
				node = parentFilter.DisplayRootCategory;
				flag = parentFilter.allowedHitPointsConfigurable;
				flag2 = parentFilter.allowedQualitiesConfigurable;
			}
			rect.xMax -= 4f;
			rect.yMax -= 6f;
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, ThingFilterUI.viewHeight);
			Rect visibleRect = new Rect(0f, 0f, rect.width, rect.height);
			visibleRect.position += state.scrollPosition;
			Widgets.BeginScrollView(rect, ref state.scrollPosition, viewRect, true);
			float num2 = 2f;
			if (flag && !forceHideHitPointsConfig)
			{
				ThingFilterUI.DrawHitPointsFilterConfig(ref num2, viewRect.width, filter);
			}
			if (flag2 && !forceHideQualityConfig)
			{
				ThingFilterUI.DrawQualityFilterConfig(ref num2, viewRect.width, filter);
			}
			if (ModsConfig.AnomalyActive && showMentalBreakChanceRange)
			{
				ThingFilterUI.DrawMentalBreakFilterConfig(ref num2, viewRect.width, filter);
			}
			float num3 = num2;
			Rect rect4 = new Rect(0f, num2, viewRect.width, 9999f);
			visibleRect.position -= rect4.position;
			Listing_TreeThingFilter listing_TreeThingFilter = new Listing_TreeThingFilter(filter, parentFilter, forceHiddenDefs, forceHiddenFilters, suppressSmallVolumeTags, state.quickSearch.filter);
			listing_TreeThingFilter.Begin(rect4);
			listing_TreeThingFilter.ListCategoryChildren(node, openMask, map, visibleRect);
			listing_TreeThingFilter.End();
			state.quickSearch.noResultsMatched = (listing_TreeThingFilter.matchCount == 0);
			if (Event.current.type == EventType.Layout)
			{
				ThingFilterUI.viewHeight = num3 + listing_TreeThingFilter.CurHeight + 90f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06005771 RID: 22385 RVA: 0x001C2ED0 File Offset: 0x001C10D0
		private static void DrawHitPointsFilterConfig(ref float y, float width, ThingFilter filter)
		{
			Rect rect = new Rect(20f, y, width - 20f, 32f);
			FloatRange allowedHitPointsPercents = filter.AllowedHitPointsPercents;
			Widgets.FloatRange(rect, 1, ref allowedHitPointsPercents, 0f, 1f, "HitPoints", ToStringStyle.PercentZero, 0f, GameFont.Small, null, 0.01f);
			filter.AllowedHitPointsPercents = allowedHitPointsPercents;
			y += 32f;
			y += 5f;
			Text.Font = GameFont.Small;
		}

		// Token: 0x06005772 RID: 22386 RVA: 0x001C2F4C File Offset: 0x001C114C
		private static void DrawQualityFilterConfig(ref float y, float width, ThingFilter filter)
		{
			Rect rect = new Rect(20f, y, width - 20f, 32f);
			QualityRange allowedQualityLevels = filter.AllowedQualityLevels;
			Widgets.QualityRange(rect, 876813230, ref allowedQualityLevels);
			filter.AllowedQualityLevels = allowedQualityLevels;
			y += 32f;
			y += 5f;
			Text.Font = GameFont.Small;
		}

		// Token: 0x06005773 RID: 22387 RVA: 0x001C2FA8 File Offset: 0x001C11A8
		private static void DrawMentalBreakFilterConfig(ref float y, float width, ThingFilter filter)
		{
			Rect rect = new Rect(20f, y, width - 20f, 32f);
			FloatRange allowedMentalBreakChance = filter.AllowedMentalBreakChance;
			Widgets.FloatRange(rect, 968573221, ref allowedMentalBreakChance, 0f, 1f, "MaxMentalBreakChance", ToStringStyle.PercentZero, 0f, GameFont.Small, null, 0f);
			filter.AllowedMentalBreakChance = allowedMentalBreakChance;
			y += 32f;
			y += 5f;
			Text.Font = GameFont.Small;
		}

		// Token: 0x04003FBB RID: 16315
		private static float viewHeight;

		// Token: 0x04003FBC RID: 16316
		private const float ExtraViewHeight = 90f;

		// Token: 0x04003FBD RID: 16317
		private const float RangeLabelTab = 10f;

		// Token: 0x04003FBE RID: 16318
		private const float RangeLabelHeight = 19f;

		// Token: 0x04003FBF RID: 16319
		private const float SliderHeight = 32f;

		// Token: 0x04003FC0 RID: 16320
		private const float SliderTab = 20f;

		// Token: 0x02000D6B RID: 3435
		public class UIState
		{
			// Token: 0x04003FC1 RID: 16321
			public Vector2 scrollPosition;

			// Token: 0x04003FC2 RID: 16322
			public QuickSearchWidget quickSearch = new QuickSearchWidget();
		}
	}
}
