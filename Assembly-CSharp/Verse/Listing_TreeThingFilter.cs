using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BC0 RID: 3008
	public class Listing_TreeThingFilter : Listing_Tree
	{
		// Token: 0x06004CF0 RID: 19696 RVA: 0x00187640 File Offset: 0x00185840
		public Listing_TreeThingFilter(ThingFilter filter, ThingFilter parentFilter, IEnumerable<ThingDef> forceHiddenDefs, IEnumerable<SpecialThingFilterDef> forceHiddenFilters, List<ThingDef> suppressSmallVolumeTags, QuickSearchFilter searchFilter)
		{
			this.filter = filter;
			this.parentFilter = parentFilter;
			if (forceHiddenDefs != null)
			{
				this.forceHiddenDefs = forceHiddenDefs.ToList<ThingDef>();
			}
			if (forceHiddenFilters != null)
			{
				this.tempForceHiddenSpecialFilters = forceHiddenFilters.ToList<SpecialThingFilterDef>();
			}
			this.suppressSmallVolumeTags = suppressSmallVolumeTags;
			this.searchFilter = searchFilter;
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x00187694 File Offset: 0x00185894
		public void ListCategoryChildren(TreeNode_ThingCategory node, int openMask, Map map, Rect visibleRect)
		{
			this.visibleRect = visibleRect;
			int num = 0;
			foreach (SpecialThingFilterDef specialThingFilterDef in node.catDef.ParentsSpecialThingFilterDefs)
			{
				if (this.Visible(specialThingFilterDef, node))
				{
					this.DoSpecialFilter(specialThingFilterDef, num);
				}
			}
			this.DoCategoryChildren(node, num, openMask, map, false);
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x00187708 File Offset: 0x00185908
		private void DoCategoryChildren(TreeNode_ThingCategory node, int indentLevel, int openMask, Map map, bool subtreeMatchedSearch)
		{
			Listing_TreeThingFilter.<>c__DisplayClass15_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.subtreeMatchedSearch = subtreeMatchedSearch;
			List<SpecialThingFilterDef> childSpecialFilters = node.catDef.childSpecialFilters;
			for (int i = 0; i < childSpecialFilters.Count; i++)
			{
				if (this.Visible(childSpecialFilters[i], node))
				{
					this.DoSpecialFilter(childSpecialFilters[i], indentLevel);
				}
			}
			foreach (TreeNode_ThingCategory treeNode_ThingCategory in node.ChildCategoryNodes)
			{
				if (this.Visible(treeNode_ThingCategory) && !this.<DoCategoryChildren>g__HideCategoryDueToSearch|15_0(treeNode_ThingCategory, ref CS$<>8__locals1))
				{
					this.DoCategory(treeNode_ThingCategory, indentLevel, openMask, map, CS$<>8__locals1.subtreeMatchedSearch);
				}
			}
			List<ThingDef> list = new List<ThingDef>();
			foreach (ThingDef thingDef in node.catDef.SortedChildThingDefs)
			{
				if (Find.HiddenItemsManager.Hidden(thingDef))
				{
					list.Add(thingDef);
				}
				else if (this.Visible(thingDef) && !this.<DoCategoryChildren>g__HideThingDueToSearch|15_1(thingDef, ref CS$<>8__locals1))
				{
					this.DoThingDef(thingDef, indentLevel, map);
				}
			}
			if (!this.searchFilter.Active && list.Count > 0)
			{
				this.DoUndiscoveredEntry(indentLevel, node.catDef.parent != ThingCategoryDefOf.Corpses, list);
			}
		}

		// Token: 0x06004CF3 RID: 19699 RVA: 0x0018787C File Offset: 0x00185A7C
		private void DoSpecialFilter(SpecialThingFilterDef sfDef, int nestLevel)
		{
			if (!sfDef.configurable)
			{
				return;
			}
			Color? textColor = null;
			if (this.searchFilter.Matches(sfDef))
			{
				this.matchCount++;
			}
			else
			{
				textColor = new Color?(Listing_TreeThingFilter.NoMatchColor);
			}
			if (this.CurrentRowVisibleOnScreen())
			{
				base.LabelLeft("*" + sfDef.LabelCap, sfDef.description, nestLevel, 0f, textColor, 0f);
				bool flag = this.filter.Allows(sfDef);
				bool flag2 = flag;
				Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag, this.lineHeight, false, true, null, null);
				if (flag != flag2)
				{
					this.filter.SetAllow(sfDef, flag);
				}
			}
			base.EndLine();
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x00187944 File Offset: 0x00185B44
		private void DoCategory(TreeNode_ThingCategory node, int indentLevel, int openMask, Map map, bool subtreeMatchedSearch)
		{
			Color? textColor = null;
			if (this.searchFilter.Active)
			{
				if (this.CategoryMatches(node))
				{
					subtreeMatchedSearch = true;
					this.matchCount++;
				}
				else
				{
					textColor = new Color?(Listing_TreeThingFilter.NoMatchColor);
				}
			}
			if (this.CurrentRowVisibleOnScreen())
			{
				base.OpenCloseWidget(node, indentLevel, openMask);
				base.LabelLeft(node.LabelCap, node.catDef.description, indentLevel, 0f, textColor, 0f);
				MultiCheckboxState multiCheckboxState = this.AllowanceStateOf(node);
				MultiCheckboxState multiCheckboxState2 = Widgets.CheckboxMulti(new Rect(this.LabelWidth, this.curY, this.lineHeight, this.lineHeight), multiCheckboxState, true);
				if (multiCheckboxState != multiCheckboxState2)
				{
					this.filter.SetAllow(node.catDef, multiCheckboxState2 == MultiCheckboxState.On, this.forceHiddenDefs, this.hiddenSpecialFilters);
				}
			}
			base.EndLine();
			if (this.IsOpen(node, openMask))
			{
				this.DoCategoryChildren(node, indentLevel + 1, openMask, map, subtreeMatchedSearch);
			}
		}

		// Token: 0x06004CF5 RID: 19701 RVA: 0x00187A34 File Offset: 0x00185C34
		private void DoThingDef(ThingDef tDef, int nestLevel, Map map)
		{
			Color? color = null;
			if (this.searchFilter.Matches(tDef))
			{
				this.matchCount++;
			}
			else
			{
				color = new Color?(Listing_TreeThingFilter.NoMatchColor);
			}
			if (tDef.uiIcon != null && tDef.uiIcon != BaseContent.BadTex)
			{
				nestLevel++;
				Widgets.DefIcon(new Rect(base.XAtIndentLevel(nestLevel) - 6f, this.curY, 20f, 20f), tDef, null, 1f, null, true, color, null, null, 1f);
			}
			if (this.CurrentRowVisibleOnScreen())
			{
				object obj = (this.suppressSmallVolumeTags == null || !this.suppressSmallVolumeTags.Contains(tDef)) && tDef.IsStuff && tDef.smallVolume;
				string text = tDef.DescriptionDetailed;
				object obj2 = obj;
				if (obj2 != null)
				{
					text += "\n\n" + "ThisIsSmallVolume".Translate(10.ToStringCached());
				}
				float num = -4f;
				if (obj2 != null)
				{
					Rect rect = new Rect(this.LabelWidth - 19f, this.curY, 19f, 20f);
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperRight;
					GUI.color = Color.gray;
					Widgets.Label(rect, "/" + 10.ToStringCached());
					Text.Font = GameFont.Small;
					GenUI.ResetLabelAlign();
					GUI.color = Color.white;
				}
				num -= 19f;
				if (map != null)
				{
					int count = map.resourceCounter.GetCount(tDef);
					if (count > 0)
					{
						string text2 = count.ToStringCached();
						Rect rect2 = new Rect(0f, this.curY, this.LabelWidth + num, 40f);
						Text.Font = GameFont.Small;
						Text.Anchor = TextAnchor.UpperRight;
						GUI.color = new Color(0.5f, 0.5f, 0.1f);
						Widgets.Label(rect2, text2);
						num -= Text.CalcSize(text2).x;
						GenUI.ResetLabelAlign();
						Text.Font = GameFont.Small;
						GUI.color = Color.white;
					}
				}
				base.LabelLeft(tDef.LabelCap, text, nestLevel, num, color, 0f);
				bool flag = this.filter.Allows(tDef);
				bool flag2 = flag;
				Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag, this.lineHeight, false, true, null, null);
				if (flag != flag2)
				{
					this.filter.SetAllow(tDef, flag);
				}
			}
			base.EndLine();
		}

		// Token: 0x06004CF6 RID: 19702 RVA: 0x00187CAC File Offset: 0x00185EAC
		private void DoUndiscoveredEntry(int nestLevel, bool useIconOffset, List<ThingDef> toggledThingDefs)
		{
			if (this.CurrentRowVisibleOnScreen())
			{
				TaggedString taggedString = "UndiscoveredItemLabel".Translate();
				string tipText = "UndiscoveredItemDesc".Translate().Resolve();
				base.LabelLeft(taggedString, tipText, nestLevel, 0f, null, useIconOffset ? 10f : 0f);
				bool flag = this.filter.Allows(toggledThingDefs[0]);
				bool flag2 = flag;
				Widgets.Checkbox(new Vector2(this.LabelWidth, this.curY), ref flag, this.lineHeight, false, true, null, null);
				if (flag != flag2)
				{
					foreach (ThingDef thingDef in toggledThingDefs)
					{
						this.filter.SetAllow(thingDef, flag);
					}
				}
			}
			base.EndLine();
		}

		// Token: 0x06004CF7 RID: 19703 RVA: 0x00187D9C File Offset: 0x00185F9C
		public MultiCheckboxState AllowanceStateOf(TreeNode_ThingCategory cat)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			foreach (ThingDef thingDef in cat.catDef.DescendantThingDefs)
			{
				if (this.Visible(thingDef))
				{
					num++;
					if (this.filter.Allows(thingDef))
					{
						num2++;
					}
				}
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in cat.catDef.DescendantSpecialThingFilterDefs)
			{
				if (this.Visible(specialThingFilterDef, cat))
				{
					num3++;
					if (this.filter.Allows(specialThingFilterDef))
					{
						num4++;
					}
				}
			}
			if (this.filter.OnlySpecialFilters)
			{
				if (num4 == 0)
				{
					return MultiCheckboxState.Off;
				}
				if (num4 < num3)
				{
					return MultiCheckboxState.Partial;
				}
				return MultiCheckboxState.On;
			}
			else
			{
				if (num2 == 0)
				{
					return MultiCheckboxState.Off;
				}
				if (num == num2 && num3 == num4)
				{
					return MultiCheckboxState.On;
				}
				return MultiCheckboxState.Partial;
			}
		}

		// Token: 0x06004CF8 RID: 19704 RVA: 0x00187EA4 File Offset: 0x001860A4
		private bool Visible(ThingDef td)
		{
			if (!td.PlayerAcquirable)
			{
				return false;
			}
			if (td.virtualDefParent != null)
			{
				return false;
			}
			if (this.forceHiddenDefs != null && this.forceHiddenDefs.Contains(td))
			{
				return false;
			}
			if (this.parentFilter != null)
			{
				if (!this.parentFilter.Allows(td))
				{
					return false;
				}
				if (this.parentFilter.IsAlwaysDisallowedDueToSpecialFilters(td))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004CF9 RID: 19705 RVA: 0x00187F08 File Offset: 0x00186108
		public override bool IsOpen(TreeNode node, int openMask)
		{
			if (base.IsOpen(node, openMask))
			{
				return true;
			}
			TreeNode_ThingCategory treeNode_ThingCategory = node as TreeNode_ThingCategory;
			return treeNode_ThingCategory != null && this.searchFilter.Active && this.ThisOrDescendantsVisibleAndMatchesSearch(treeNode_ThingCategory);
		}

		// Token: 0x06004CFA RID: 19706 RVA: 0x00187F44 File Offset: 0x00186144
		private bool ThisOrDescendantsVisibleAndMatchesSearch(TreeNode_ThingCategory node)
		{
			if (this.Visible(node) && this.CategoryMatches(node))
			{
				return true;
			}
			foreach (ThingDef td in node.catDef.childThingDefs)
			{
				if (this.<ThisOrDescendantsVisibleAndMatchesSearch>g__ThingDefVisibleAndMatches|23_0(td))
				{
					return true;
				}
			}
			foreach (SpecialThingFilterDef sf in node.catDef.childSpecialFilters)
			{
				if (this.<ThisOrDescendantsVisibleAndMatchesSearch>g__SpecialFilterVisibleAndMatches|23_1(sf, node))
				{
					return true;
				}
			}
			foreach (ThingCategoryDef thingCategoryDef in node.catDef.childCategories)
			{
				if (this.ThisOrDescendantsVisibleAndMatchesSearch(thingCategoryDef.treeNode))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x00188064 File Offset: 0x00186264
		private bool CategoryMatches(TreeNode_ThingCategory node)
		{
			return this.searchFilter.Matches(node.catDef.label);
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x0018807C File Offset: 0x0018627C
		private bool Visible(TreeNode_ThingCategory node)
		{
			if (this.filter.OnlySpecialFilters)
			{
				return node.catDef.DescendantSpecialThingFilterDefs.Any(new Func<SpecialThingFilterDef, bool>(this.Visible));
			}
			return node.catDef.DescendantThingDefs.Any(new Func<ThingDef, bool>(this.Visible));
		}

		// Token: 0x06004CFD RID: 19709 RVA: 0x001880CF File Offset: 0x001862CF
		private bool Visible(SpecialThingFilterDef f)
		{
			return this.parentFilter == null || this.parentFilter.Allows(f);
		}

		// Token: 0x06004CFE RID: 19710 RVA: 0x001880EC File Offset: 0x001862EC
		private bool Visible(SpecialThingFilterDef filterDef, TreeNode_ThingCategory node)
		{
			if (this.parentFilter != null && !this.parentFilter.Allows(filterDef))
			{
				return false;
			}
			if (this.filter.OnlySpecialFilters)
			{
				return true;
			}
			if (this.hiddenSpecialFilters == null)
			{
				this.CalculateHiddenSpecialFilters(node);
			}
			for (int i = 0; i < this.hiddenSpecialFilters.Count; i++)
			{
				if (this.hiddenSpecialFilters[i] == filterDef)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004CFF RID: 19711 RVA: 0x00188158 File Offset: 0x00186358
		private bool CurrentRowVisibleOnScreen()
		{
			Rect other = new Rect(0f, this.curY, base.ColumnWidth, this.lineHeight);
			return this.visibleRect.Overlaps(other);
		}

		// Token: 0x06004D00 RID: 19712 RVA: 0x0018818F File Offset: 0x0018638F
		private void CalculateHiddenSpecialFilters(TreeNode_ThingCategory node)
		{
			this.hiddenSpecialFilters = Listing_TreeThingFilter.GetCachedHiddenSpecialFilters(node, this.parentFilter);
			if (this.tempForceHiddenSpecialFilters == null)
			{
				return;
			}
			this.hiddenSpecialFilters = new List<SpecialThingFilterDef>(this.hiddenSpecialFilters);
			this.hiddenSpecialFilters.AddRange(this.tempForceHiddenSpecialFilters);
		}

		// Token: 0x06004D01 RID: 19713 RVA: 0x001881D0 File Offset: 0x001863D0
		private static List<SpecialThingFilterDef> GetCachedHiddenSpecialFilters(TreeNode_ThingCategory node, ThingFilter parentFilter)
		{
			ValueTuple<TreeNode_ThingCategory, ThingFilter> key = new ValueTuple<TreeNode_ThingCategory, ThingFilter>(node, parentFilter);
			List<SpecialThingFilterDef> list;
			if (Listing_TreeThingFilter.cachedHiddenSpecialFilters.TryGetValue(key, out list))
			{
				return list;
			}
			list = Listing_TreeThingFilter.CalculateHiddenSpecialFilters(node, parentFilter);
			Listing_TreeThingFilter.cachedHiddenSpecialFilters.Add(key, list);
			return list;
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x0018820C File Offset: 0x0018640C
		private static List<SpecialThingFilterDef> CalculateHiddenSpecialFilters(TreeNode_ThingCategory node, ThingFilter parentFilter)
		{
			List<SpecialThingFilterDef> list = new List<SpecialThingFilterDef>();
			IEnumerable<SpecialThingFilterDef> enumerable = node.catDef.ParentsSpecialThingFilterDefs.Concat(node.catDef.DescendantSpecialThingFilterDefs);
			IEnumerable<ThingDef> enumerable2 = node.catDef.DescendantThingDefs;
			if (parentFilter != null)
			{
				enumerable2 = enumerable2.Where(new Func<ThingDef, bool>(parentFilter.Allows));
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in enumerable)
			{
				bool flag = false;
				foreach (ThingDef def in enumerable2)
				{
					if (specialThingFilterDef.Worker.CanEverMatch(def))
					{
						flag = true;
						break;
					}
				}
				if (parentFilter != null && parentFilter.hiddenSpecialFilters.Contains(specialThingFilterDef))
				{
					flag = false;
				}
				if (!flag)
				{
					list.Add(specialThingFilterDef);
				}
			}
			return list;
		}

		// Token: 0x06004D03 RID: 19715 RVA: 0x001882FC File Offset: 0x001864FC
		public static void ResetStaticData()
		{
			Listing_TreeThingFilter.cachedHiddenSpecialFilters.Clear();
		}

		// Token: 0x06004D05 RID: 19717 RVA: 0x00188323 File Offset: 0x00186523
		[CompilerGenerated]
		private bool <DoCategoryChildren>g__HideCategoryDueToSearch|15_0(TreeNode_ThingCategory subCat, ref Listing_TreeThingFilter.<>c__DisplayClass15_0 A_2)
		{
			return !(!this.searchFilter.Active | A_2.subtreeMatchedSearch) && !this.CategoryMatches(subCat) && !this.ThisOrDescendantsVisibleAndMatchesSearch(subCat);
		}

		// Token: 0x06004D06 RID: 19718 RVA: 0x00188355 File Offset: 0x00186555
		[CompilerGenerated]
		private bool <DoCategoryChildren>g__HideThingDueToSearch|15_1(ThingDef tDef, ref Listing_TreeThingFilter.<>c__DisplayClass15_0 A_2)
		{
			return !(!this.searchFilter.Active | A_2.subtreeMatchedSearch) && !this.searchFilter.Matches(tDef);
		}

		// Token: 0x06004D07 RID: 19719 RVA: 0x0018837F File Offset: 0x0018657F
		[CompilerGenerated]
		private bool <ThisOrDescendantsVisibleAndMatchesSearch>g__ThingDefVisibleAndMatches|23_0(ThingDef td)
		{
			return this.Visible(td) && this.searchFilter.Matches(td);
		}

		// Token: 0x06004D08 RID: 19720 RVA: 0x00188398 File Offset: 0x00186598
		[CompilerGenerated]
		private bool <ThisOrDescendantsVisibleAndMatchesSearch>g__SpecialFilterVisibleAndMatches|23_1(SpecialThingFilterDef sf, TreeNode_ThingCategory subCat)
		{
			return this.Visible(sf, subCat) && this.searchFilter.Matches(sf);
		}

		// Token: 0x040036B5 RID: 14005
		private static readonly Color NoMatchColor = Color.grey;

		// Token: 0x040036B6 RID: 14006
		private static readonly LRUCache<ValueTuple<TreeNode_ThingCategory, ThingFilter>, List<SpecialThingFilterDef>> cachedHiddenSpecialFilters = new LRUCache<ValueTuple<TreeNode_ThingCategory, ThingFilter>, List<SpecialThingFilterDef>>(500);

		// Token: 0x040036B7 RID: 14007
		private ThingFilter filter;

		// Token: 0x040036B8 RID: 14008
		private ThingFilter parentFilter;

		// Token: 0x040036B9 RID: 14009
		private const float IconSize = 20f;

		// Token: 0x040036BA RID: 14010
		private const float IconOffset = 6f;

		// Token: 0x040036BB RID: 14011
		private List<SpecialThingFilterDef> hiddenSpecialFilters;

		// Token: 0x040036BC RID: 14012
		private List<ThingDef> forceHiddenDefs;

		// Token: 0x040036BD RID: 14013
		private List<SpecialThingFilterDef> tempForceHiddenSpecialFilters;

		// Token: 0x040036BE RID: 14014
		private List<ThingDef> suppressSmallVolumeTags;

		// Token: 0x040036BF RID: 14015
		protected QuickSearchFilter searchFilter;

		// Token: 0x040036C0 RID: 14016
		public int matchCount;

		// Token: 0x040036C1 RID: 14017
		private Rect visibleRect;
	}
}
