using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003164 RID: 12644
	[StaticConstructorOnStartup]
	public class Dialog_EditIdeoStyleItems : Window
	{
		// Token: 0x17002E33 RID: 11827
		// (get) Token: 0x06011C79 RID: 72825 RVA: 0x00524D57 File Offset: 0x00522F57
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(Mathf.Min(1200f, (float)UI.screenWidth), 750f);
			}
		}

		// Token: 0x17002E34 RID: 11828
		// (get) Token: 0x06011C7A RID: 72826 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06011C7B RID: 72827 RVA: 0x00524D74 File Offset: 0x00522F74
		public Dialog_EditIdeoStyleItems(Ideo ideo, StyleItemTab category, IdeoEditMode editMode) : base(null)
		{
			this.ideo = ideo;
			this.curTab = category;
			this.editMode = editMode;
			this.absorbInputAroundWindow = true;
			this.allFrequencies = (StyleItemFrequency[])Enum.GetValues(typeof(StyleItemFrequency));
			this.expandedInfos = new List<Dialog_EditIdeoStyleItems.ExpandedInfo>();
			foreach (Dialog_EditIdeoStyleItems.ItemType itemType in (Dialog_EditIdeoStyleItems.ItemType[])Enum.GetValues(typeof(Dialog_EditIdeoStyleItems.ItemType)))
			{
				foreach (StyleItemCategoryDef styleItemCategoryDef in DefDatabase<StyleItemCategoryDef>.AllDefs)
				{
					this.expandedInfos.Add(new Dialog_EditIdeoStyleItems.ExpandedInfo(styleItemCategoryDef, itemType, this.CanList(styleItemCategoryDef, itemType)));
				}
			}
			this.Reset();
		}

		// Token: 0x06011C7C RID: 72828 RVA: 0x00524E64 File Offset: 0x00523064
		private void Reset()
		{
			this.hairFrequencies = new DefMap<HairDef, StyleItemSpawningProperties>();
			foreach (KeyValuePair<HairDef, StyleItemSpawningProperties> keyValuePair in this.hairFrequencies)
			{
				this.hairFrequencies[keyValuePair.Key].frequency = this.ideo.style.GetFrequency(keyValuePair.Key);
				this.hairFrequencies[keyValuePair.Key].gender = this.ideo.style.GetGender(keyValuePair.Key);
			}
			this.beardFrequencies = new DefMap<BeardDef, StyleItemSpawningProperties>();
			foreach (KeyValuePair<BeardDef, StyleItemSpawningProperties> keyValuePair2 in this.beardFrequencies)
			{
				this.beardFrequencies[keyValuePair2.Key].frequency = this.ideo.style.GetFrequency(keyValuePair2.Key);
				this.beardFrequencies[keyValuePair2.Key].gender = this.ideo.style.GetGender(keyValuePair2.Key);
			}
			this.tattooFrequencies = new DefMap<TattooDef, StyleItemSpawningProperties>();
			foreach (KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair3 in this.tattooFrequencies)
			{
				this.tattooFrequencies[keyValuePair3.Key].frequency = this.ideo.style.GetFrequency(keyValuePair3.Key);
				this.tattooFrequencies[keyValuePair3.Key].gender = this.ideo.style.GetGender(keyValuePair3.Key);
			}
		}

		// Token: 0x06011C7D RID: 72829 RVA: 0x00525050 File Offset: 0x00523250
		public override void PostOpen()
		{
			base.PostOpen();
			if (!ModLister.CheckIdeology("Appearance editing"))
			{
				this.Close(true);
			}
		}

		// Token: 0x06011C7E RID: 72830 RVA: 0x0052506C File Offset: 0x0052326C
		public override void DoWindowContents(Rect rect)
		{
			this.hover = null;
			Rect rect2 = new Rect(rect);
			rect2.xMin += 18f;
			rect2.yMin += 10f;
			rect2.height = 35f;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, "EditAppearanceItems".Translate());
			Text.Font = GameFont.Small;
			Rect rect3 = rect;
			rect3.yMin = rect2.yMax + 35f;
			rect3.yMax -= Dialog_EditIdeoStyleItems.ButSize.y + 10f;
			this.FillDialog(rect3);
			Rect rect4 = new Rect(rect.xMax - Dialog_EditIdeoStyleItems.ButSize.x - 10f, rect.y + 10f, Dialog_EditIdeoStyleItems.ButSize.x, 30f);
			if (Widgets.ButtonText(rect4, "ExpandAllCategories".Translate().CapitalizeFirst(), true, true, true, null))
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				foreach (Dialog_EditIdeoStyleItems.ExpandedInfo expandedInfo in this.expandedInfos)
				{
					expandedInfo.expanded = true;
				}
			}
			if (Widgets.ButtonText(new Rect(rect4.x, rect4.yMax + 4f, Dialog_EditIdeoStyleItems.ButSize.x, 30f), "CollapseAllCategories".Translate().CapitalizeFirst(), true, true, true, null))
			{
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				foreach (Dialog_EditIdeoStyleItems.ExpandedInfo expandedInfo2 in this.expandedInfos)
				{
					expandedInfo2.expanded = false;
				}
			}
			rect = rect.ContractedBy(18f);
			float y = rect.height - Dialog_EditIdeoStyleItems.ButSize.y + 18f;
			Rect rect5 = new Rect(rect.x + rect.width / 2f - Dialog_EditIdeoStyleItems.ButSize.x / 2f, y, Dialog_EditIdeoStyleItems.ButSize.x, Dialog_EditIdeoStyleItems.ButSize.y);
			if (this.editMode == IdeoEditMode.None)
			{
				if (Widgets.ButtonText(rect5, "Back".Translate(), true, true, true, null))
				{
					this.Close(true);
					return;
				}
			}
			else
			{
				if (Widgets.ButtonText(new Rect(rect.x, y, Dialog_EditIdeoStyleItems.ButSize.x, Dialog_EditIdeoStyleItems.ButSize.y), "Cancel".Translate(), true, true, true, null))
				{
					this.Close(true);
				}
				TooltipHandler.TipRegion(rect5, "ResetButtonDesc".Translate());
				if (Widgets.ButtonText(rect5, "Reset".Translate(), true, true, true, null))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.Reset();
				}
				if (Widgets.ButtonText(new Rect(rect.width - Dialog_EditIdeoStyleItems.ButSize.x + 18f, y, Dialog_EditIdeoStyleItems.ButSize.x, Dialog_EditIdeoStyleItems.ButSize.y), "DoneButton".Translate(), true, true, true, null))
				{
					this.Done();
				}
			}
		}

		// Token: 0x06011C7F RID: 72831 RVA: 0x005253FC File Offset: 0x005235FC
		public override void OnAcceptKeyPressed()
		{
			this.Done();
			Event.current.Use();
		}

		// Token: 0x06011C80 RID: 72832 RVA: 0x00525410 File Offset: 0x00523610
		private void FillDialog(Rect rect)
		{
			this.DrawTabs(rect);
			rect = rect.ContractedBy(18f);
			Rect rect2 = new Rect(rect.x, rect.y, rect.width / 2f - 5f, rect.height);
			this.DrawSection(rect2, ref this.scrollPositionLeft, ref this.scrollViewHeightLeft, (this.curTab == StyleItemTab.HairAndBeard) ? Dialog_EditIdeoStyleItems.ItemType.Hair : Dialog_EditIdeoStyleItems.ItemType.FaceTattoo);
			Rect rect3 = new Rect(rect2.xMax + 10f, rect.y, rect.width / 2f - 10f, rect.height);
			this.DrawSection(rect3, ref this.scrollPositionRight, ref this.scrollViewHeightRight, (this.curTab == StyleItemTab.HairAndBeard) ? Dialog_EditIdeoStyleItems.ItemType.Beard : Dialog_EditIdeoStyleItems.ItemType.BodyTattoo);
			if (!Input.GetMouseButton(0))
			{
				this.painting = false;
			}
		}

		// Token: 0x06011C81 RID: 72833 RVA: 0x005254E4 File Offset: 0x005236E4
		private void DrawTabs(Rect rect)
		{
			this.tabs.Clear();
			this.tabs.Add(new TabRecord("HairAndBeards".Translate(), delegate()
			{
				this.curTab = StyleItemTab.HairAndBeard;
				this.scrollPositionLeft = (this.scrollPositionRight = Vector2.zero);
			}, this.curTab == StyleItemTab.HairAndBeard));
			this.tabs.Add(new TabRecord("Tattoos".Translate(), delegate()
			{
				this.curTab = StyleItemTab.Tattoo;
				this.scrollPositionLeft = (this.scrollPositionRight = Vector2.zero);
			}, this.curTab == StyleItemTab.Tattoo));
			TabDrawer.DrawTabs<TabRecord>(rect, this.tabs, 200f);
		}

		// Token: 0x06011C82 RID: 72834 RVA: 0x00525578 File Offset: 0x00523778
		private void DrawSection(Rect rect, ref Vector2 scrollPosition, ref float scrollViewHeight, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(new Rect(rect.x + 10f, rect.y, rect.width, 30f), this.GetSectionLabel(itemType).CapitalizeFirst());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			rect.yMin += 30f;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleCenter;
			for (int i = 0; i < this.allFrequencies.Length; i++)
			{
				Rect rect2 = this.FrequencyPosition(new Rect(rect.x, rect.y, rect.width, Text.LineHeight), i);
				string label = this.allFrequencies[i].GetLabel();
				string text = label.Truncate(rect2.width, null);
				Widgets.Label(rect2, text);
				if (label != text)
				{
					TooltipHandler.TipRegion(rect2, label);
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			rect.yMin += Text.LineHeight;
			Widgets.BeginGroup(rect);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, scrollViewHeight);
			float num = 0f;
			float num2 = 28f;
			Widgets.BeginScrollView(rect.AtZero(), ref scrollPosition, viewRect, true);
			using (IEnumerator<StyleItemCategoryDef> enumerator = DefDatabase<StyleItemCategoryDef>.AllDefs.GetEnumerator())
			{
				Func<StyleItemDef, bool> <>9__1;
				while (enumerator.MoveNext())
				{
					StyleItemCategoryDef c = enumerator.Current;
					Dialog_EditIdeoStyleItems.ExpandedInfo expandedInfo = this.expandedInfos.FirstOrDefault((Dialog_EditIdeoStyleItems.ExpandedInfo x) => x.categoryDef == c && itemType == x.itemType);
					if (expandedInfo != null && expandedInfo.any)
					{
						this.ListStyleItemCategory(c, ref num, viewRect, expandedInfo, itemType);
						float num3 = num2;
						float num4 = 28f;
						float num5 = 28f;
						IEnumerable<StyleItemDef> itemsInCategory = c.ItemsInCategory;
						Func<StyleItemDef, bool> predicate;
						if ((predicate = <>9__1) == null)
						{
							predicate = (<>9__1 = ((StyleItemDef x) => this.CanList(x, itemType)));
						}
						num2 = num3 + (num4 + num5 * (float)itemsInCategory.Where(predicate).Count<StyleItemDef>());
					}
				}
			}
			if (Event.current.type == EventType.Layout)
			{
				scrollViewHeight = num2;
			}
			Widgets.EndScrollView();
			Widgets.EndGroup();
		}

		// Token: 0x06011C83 RID: 72835 RVA: 0x005257FC File Offset: 0x005239FC
		private void ListStyleItemCategory(StyleItemCategoryDef category, ref float curY, Rect viewRect, Dialog_EditIdeoStyleItems.ExpandedInfo expandedInfo, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			Rect rect = new Rect(viewRect.x, viewRect.y + curY, viewRect.width, 28f);
			Widgets.DrawHighlightSelected(rect);
			Rect rect2 = new Rect(viewRect.x, curY, 28f, 28f);
			GUI.DrawTexture(rect2.ContractedBy(4f), expandedInfo.expanded ? Dialog_EditIdeoStyleItems.Minus : Dialog_EditIdeoStyleItems.Plus);
			Widgets.DrawHighlightIfMouseover(rect);
			Rect rect3 = new Rect(rect2.xMax + 4f, curY, 110f, 28f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, category.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			if (Widgets.ButtonInvisible(new Rect(rect2.x, rect2.y, rect2.width + rect3.width + 4f, 28f), true))
			{
				expandedInfo.expanded = !expandedInfo.expanded;
				if (expandedInfo.expanded)
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			Gender? gender = null;
			if (itemType != Dialog_EditIdeoStyleItems.ItemType.Beard)
			{
				gender = new Gender?(this.TryGetGender(category, itemType));
			}
			this.DrawConfigInfo(viewRect, curY, delegate(Gender g)
			{
				foreach (StyleItemDef styleItemDef2 in category.ItemsInCategory)
				{
					if (this.CanList(styleItemDef2, itemType))
					{
						this.ChangeGender(styleItemDef2, g);
					}
				}
			}, delegate(StyleItemFrequency f)
			{
				foreach (StyleItemDef styleItemDef2 in category.ItemsInCategory)
				{
					if (this.CanList(styleItemDef2, itemType))
					{
						this.ChangeFrequency(styleItemDef2, f);
					}
				}
			}, gender, this.TryGetFrequency(category, itemType), (StyleItemFrequency f) => category.ItemsInCategory.Any((StyleItemDef x) => this.CanList(x, itemType) && this.GetFrequency(x) == f));
			curY += rect.height;
			if (expandedInfo.expanded)
			{
				int num = 0;
				foreach (StyleItemDef styleItemDef in category.ItemsInCategory)
				{
					if (this.CanList(styleItemDef, itemType))
					{
						this.ListStyleItem(styleItemDef, ref curY, num, viewRect);
						num++;
					}
				}
			}
			curY += 4f;
		}

		// Token: 0x06011C84 RID: 72836 RVA: 0x00525A30 File Offset: 0x00523C30
		private void ListStyleItem(StyleItemDef styleItem, ref float curY, int index, Rect viewRect)
		{
			Rect rect = new Rect(viewRect.x + 17f, viewRect.y + curY, viewRect.width - 17f, 28f);
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(new Rect(rect.x + 28f, rect.y, rect.width - 28f, rect.height));
			}
			if (Mouse.IsOver(rect))
			{
				this.hover = styleItem;
				Rect r = new Rect(UI.MousePositionOnUI.x + 10f, UI.MousePositionOnUIInverted.y, 100f, 100f + Text.LineHeight);
				Find.WindowStack.ImmediateWindow(12918217, r, WindowLayer.Super, delegate
				{
					Rect rect5 = r.AtZero();
					rect5.height -= Text.LineHeight;
					Widgets.DrawHighlight(rect5);
					if (this.hover != null)
					{
						Text.Anchor = TextAnchor.UpperCenter;
						Widgets.LabelFit(new Rect(0f, rect5.yMax, rect5.width, Text.LineHeight), this.hover.LabelCap);
						Text.Anchor = TextAnchor.UpperLeft;
						float scale = 1.1f;
						if (this.hover is HairDef)
						{
							GUI.color = Dialog_EditIdeoStyleItems.HairColor;
							rect5.y += 10f;
						}
						else if (this.hover is BeardDef)
						{
							GUI.color = Dialog_EditIdeoStyleItems.HairColor;
							rect5.y -= 10f;
						}
						else if (this.hover is TattooDef)
						{
							Widgets.DrawRectFast(rect5, Dialog_EditIdeoStyleItems.SemiSelectedColor, null);
							scale = 1.25f;
						}
						Widgets.DefIcon(rect5, this.hover, null, scale, null, false, null, null, null, 1f);
						GUI.color = Color.white;
					}
				}, true, false, 1f, null, false);
			}
			Widgets.DrawHighlightIfMouseover(rect);
			Rect rect2 = new Rect(rect.x, curY, 28f, 28f);
			Rect rect3 = rect2.ContractedBy(2f);
			Widgets.DrawHighlight(rect3);
			if (styleItem is HairDef || styleItem is BeardDef)
			{
				GUI.color = Dialog_EditIdeoStyleItems.HairColor;
			}
			else if (styleItem is TattooDef)
			{
				Widgets.DrawHighlight(rect3);
			}
			Widgets.DefIcon(rect2, styleItem, null, 1.25f, null, false, null, null, null, 1f);
			GUI.color = Color.white;
			Rect rect4 = new Rect(rect2.xMax + 4f, curY, 110f, 28f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect4, styleItem.LabelCap.Truncate(rect4.width, this.labelCache));
			Text.Anchor = TextAnchor.UpperLeft;
			this.DrawConfigInfo(viewRect, curY, delegate(Gender g)
			{
				this.ChangeGender(styleItem, g);
			}, delegate(StyleItemFrequency f)
			{
				this.ChangeFrequency(styleItem, f);
			}, this.GetGender(styleItem), new StyleItemFrequency?(this.GetFrequency(styleItem)), null);
			curY += 28f;
		}

		// Token: 0x06011C85 RID: 72837 RVA: 0x00525C8C File Offset: 0x00523E8C
		private void DrawConfigInfo(Rect viewRect, float curY, Action<Gender> changeGenderAction, Action<StyleItemFrequency> changeFrequencyAction, Gender? gender = null, StyleItemFrequency? curFrequency = null, Func<StyleItemFrequency, bool> semiSelectedValidator = null)
		{
			Rect inRect = new Rect(viewRect.x, curY, viewRect.width + 16f, 28f);
			Rect rect = this.FrequencyPosition(inRect, 0);
			if (gender != null)
			{
				Rect rect2 = new Rect(rect.xMin - rect.width / 2f, curY, 28f, 28f);
				if (Mouse.IsOver(rect2))
				{
					Widgets.DrawHighlight(rect2);
					if (gender != null)
					{
						TooltipHandler.TipRegion(rect2, Dialog_EditIdeoStyleItems.<DrawConfigInfo>g__GenderLabel|42_0(gender.Value).CapitalizeFirst());
					}
				}
				GUI.DrawTexture(rect2.ContractedBy(4f), gender.Value.GetIcon());
				if (Widgets.ButtonInvisible(rect2, true))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					using (IEnumerator enumerator = Enum.GetValues(typeof(Gender)).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Gender g = (Gender)enumerator.Current;
							list.Add(new FloatMenuOption(Dialog_EditIdeoStyleItems.<DrawConfigInfo>g__GenderLabel|42_0(g).CapitalizeFirst(), delegate()
							{
								changeGenderAction(g);
							}, g.GetIcon(), Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
						}
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
			}
			for (int i = 0; i < this.allFrequencies.Length; i++)
			{
				StyleItemFrequency styleItemFrequency = this.allFrequencies[i];
				StyleItemFrequency? styleItemFrequency2 = curFrequency;
				StyleItemFrequency styleItemFrequency3 = styleItemFrequency;
				bool flag = styleItemFrequency2.GetValueOrDefault() == styleItemFrequency3 & styleItemFrequency2 != null;
				Rect rect3 = this.FrequencyPosition(inRect, i);
				Widgets.DrawHighlightIfMouseover(rect3);
				Vector2 vector = rect3.center - new Vector2(12f, 12f);
				if (this.editMode != IdeoEditMode.None)
				{
					Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect3, false);
					if (draggableResult == Widgets.DraggableResult.Dragged)
					{
						this.painting = true;
					}
					Widgets.RadioButton(vector, flag, false);
					if (!flag && semiSelectedValidator != null && semiSelectedValidator(styleItemFrequency))
					{
						GUI.DrawTexture(new Rect(vector.x, vector.y, 24f, 24f), Dialog_EditIdeoStyleItems.RadioButSemiOn);
					}
					if (!flag && ((this.painting && Mouse.IsOver(rect3)) || draggableResult.AnyPressed()))
					{
						SoundDefOf.Designate_DragStandard_Changed_NoCam.PlayOneShotOnCamera(null);
						changeFrequencyAction(styleItemFrequency);
					}
				}
				else if (flag)
				{
					Widgets.RadioButton(vector, flag, false);
				}
			}
		}

		// Token: 0x06011C86 RID: 72838 RVA: 0x00525F48 File Offset: 0x00524148
		private StyleItemFrequency GetFrequency(StyleItemDef def)
		{
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				return this.hairFrequencies[hairDef].frequency;
			}
			BeardDef beardDef = def as BeardDef;
			if (beardDef != null)
			{
				return this.beardFrequencies[beardDef].frequency;
			}
			TattooDef tattooDef = def as TattooDef;
			if (tattooDef != null)
			{
				return this.tattooFrequencies[tattooDef].frequency;
			}
			return StyleItemFrequency.Never;
		}

		// Token: 0x06011C87 RID: 72839 RVA: 0x00525FAC File Offset: 0x005241AC
		private Gender? GetGender(StyleItemDef def)
		{
			StyleGender styleGender = StyleGender.Any;
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				styleGender = this.hairFrequencies[hairDef].gender;
			}
			else
			{
				if (def is BeardDef)
				{
					return null;
				}
				TattooDef tattooDef = def as TattooDef;
				if (tattooDef != null)
				{
					styleGender = this.tattooFrequencies[tattooDef].gender;
				}
			}
			switch (styleGender)
			{
			case StyleGender.Male:
			case StyleGender.MaleUsually:
				return new Gender?(Gender.Male);
			case StyleGender.FemaleUsually:
			case StyleGender.Female:
				return new Gender?(Gender.Female);
			}
			return new Gender?(Gender.None);
		}

		// Token: 0x06011C88 RID: 72840 RVA: 0x00526038 File Offset: 0x00524238
		private void ChangeFrequency(StyleItemDef def, StyleItemFrequency freq)
		{
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				this.hairFrequencies[hairDef].frequency = freq;
				return;
			}
			BeardDef beardDef = def as BeardDef;
			if (beardDef != null)
			{
				this.beardFrequencies[beardDef].frequency = freq;
				return;
			}
			TattooDef tattooDef = def as TattooDef;
			if (tattooDef != null)
			{
				this.tattooFrequencies[tattooDef].frequency = freq;
			}
		}

		// Token: 0x06011C89 RID: 72841 RVA: 0x0052609C File Offset: 0x0052429C
		private void ChangeGender(StyleItemDef def, StyleGender gender)
		{
			HairDef hairDef = def as HairDef;
			if (hairDef != null)
			{
				this.hairFrequencies[hairDef].gender = gender;
				return;
			}
			BeardDef beardDef = def as BeardDef;
			if (beardDef != null)
			{
				this.beardFrequencies[beardDef].gender = gender;
				return;
			}
			TattooDef tattooDef = def as TattooDef;
			if (tattooDef != null)
			{
				this.tattooFrequencies[tattooDef].gender = gender;
			}
		}

		// Token: 0x06011C8A RID: 72842 RVA: 0x00526100 File Offset: 0x00524300
		private void ChangeGender(StyleItemDef def, Gender gender)
		{
			StyleGender gender2 = StyleGender.Any;
			if (gender == Gender.Male)
			{
				gender2 = StyleGender.Male;
			}
			else if (gender == Gender.Female)
			{
				gender2 = StyleGender.Female;
			}
			this.ChangeGender(def, gender2);
		}

		// Token: 0x06011C8B RID: 72843 RVA: 0x00526128 File Offset: 0x00524328
		private bool CanList(StyleItemDef s, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			switch (itemType)
			{
			case Dialog_EditIdeoStyleItems.ItemType.Hair:
				return s is HairDef;
			case Dialog_EditIdeoStyleItems.ItemType.Beard:
				return s is BeardDef;
			case Dialog_EditIdeoStyleItems.ItemType.FaceTattoo:
			{
				TattooDef tattooDef = s as TattooDef;
				return tattooDef != null && tattooDef.tattooType == TattooType.Face;
			}
			case Dialog_EditIdeoStyleItems.ItemType.BodyTattoo:
			{
				TattooDef tattooDef2 = s as TattooDef;
				return tattooDef2 != null && tattooDef2.tattooType == TattooType.Body;
			}
			default:
				return false;
			}
		}

		// Token: 0x06011C8C RID: 72844 RVA: 0x00526190 File Offset: 0x00524390
		private Dialog_EditIdeoStyleItems.ItemType GetItemType(StyleItemDef s)
		{
			if (s is HairDef)
			{
				return Dialog_EditIdeoStyleItems.ItemType.Hair;
			}
			if (s is BeardDef)
			{
				return Dialog_EditIdeoStyleItems.ItemType.Beard;
			}
			TattooDef tattooDef = s as TattooDef;
			if (tattooDef == null)
			{
				return Dialog_EditIdeoStyleItems.ItemType.Hair;
			}
			if (tattooDef.tattooType == TattooType.Body)
			{
				return Dialog_EditIdeoStyleItems.ItemType.BodyTattoo;
			}
			return Dialog_EditIdeoStyleItems.ItemType.FaceTattoo;
		}

		// Token: 0x06011C8D RID: 72845 RVA: 0x005261CC File Offset: 0x005243CC
		private bool CanList(StyleItemCategoryDef category, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			foreach (StyleItemDef s in category.ItemsInCategory)
			{
				if (this.GetItemType(s) == itemType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06011C8E RID: 72846 RVA: 0x0052622C File Offset: 0x0052442C
		private string GetSectionLabel(Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			switch (itemType)
			{
			case Dialog_EditIdeoStyleItems.ItemType.Hair:
				return "Hair".Translate();
			case Dialog_EditIdeoStyleItems.ItemType.Beard:
				return "Beard".Translate();
			case Dialog_EditIdeoStyleItems.ItemType.FaceTattoo:
				return "TattooFace".Translate();
			case Dialog_EditIdeoStyleItems.ItemType.BodyTattoo:
				return "TattooBody".Translate();
			default:
				return string.Empty;
			}
		}

		// Token: 0x06011C8F RID: 72847 RVA: 0x00526298 File Offset: 0x00524498
		private Gender TryGetGender(StyleItemCategoryDef def, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			Gender? gender = null;
			foreach (StyleItemDef styleItemDef in def.ItemsInCategory)
			{
				if (this.CanList(styleItemDef, itemType))
				{
					if (gender == null)
					{
						gender = this.GetGender(styleItemDef);
					}
					else
					{
						Gender? gender2 = this.GetGender(styleItemDef);
						Gender? gender3 = gender;
						if (!(gender2.GetValueOrDefault() == gender3.GetValueOrDefault() & gender2 != null == (gender3 != null)))
						{
							return Gender.None;
						}
					}
				}
			}
			return gender.GetValueOrDefault();
		}

		// Token: 0x06011C90 RID: 72848 RVA: 0x00526348 File Offset: 0x00524548
		private StyleItemFrequency? TryGetFrequency(StyleItemCategoryDef def, Dialog_EditIdeoStyleItems.ItemType itemType)
		{
			StyleItemFrequency? styleItemFrequency = null;
			foreach (StyleItemDef styleItemDef in def.ItemsInCategory)
			{
				if (this.CanList(styleItemDef, itemType))
				{
					if (styleItemFrequency == null)
					{
						styleItemFrequency = new StyleItemFrequency?(this.GetFrequency(styleItemDef));
					}
					else
					{
						StyleItemFrequency frequency = this.GetFrequency(styleItemDef);
						StyleItemFrequency? result = styleItemFrequency;
						if (!(frequency == result.GetValueOrDefault() & result != null))
						{
							result = null;
							return result;
						}
					}
				}
			}
			return styleItemFrequency;
		}

		// Token: 0x06011C91 RID: 72849 RVA: 0x005263EC File Offset: 0x005245EC
		private Rect FrequencyPosition(Rect inRect, int index)
		{
			Rect result = inRect;
			result.width = (inRect.width - 178f) / (float)this.allFrequencies.Length - 4f;
			result.x = inRect.x + 178f + result.width * (float)index;
			return result;
		}

		// Token: 0x06011C92 RID: 72850 RVA: 0x00526440 File Offset: 0x00524640
		private void Done()
		{
			if (this.editMode != IdeoEditMode.None)
			{
				foreach (KeyValuePair<HairDef, StyleItemSpawningProperties> keyValuePair in this.hairFrequencies)
				{
					this.ideo.style.SetFrequency(keyValuePair.Key, keyValuePair.Value.frequency);
					this.ideo.style.SetGender(keyValuePair.Key, keyValuePair.Value.gender);
				}
				foreach (KeyValuePair<BeardDef, StyleItemSpawningProperties> keyValuePair2 in this.beardFrequencies)
				{
					this.ideo.style.SetFrequency(keyValuePair2.Key, keyValuePair2.Value.frequency);
					this.ideo.style.SetGender(keyValuePair2.Key, keyValuePair2.Value.gender);
				}
				foreach (KeyValuePair<TattooDef, StyleItemSpawningProperties> keyValuePair3 in this.tattooFrequencies)
				{
					this.ideo.style.SetFrequency(keyValuePair3.Key, keyValuePair3.Value.frequency);
					this.ideo.style.SetGender(keyValuePair3.Key, keyValuePair3.Value.gender);
				}
				this.ideo.style.EnsureAtLeastOneStyleItemAvailable();
			}
			this.Close(true);
		}

		// Token: 0x06011C96 RID: 72854 RVA: 0x005266B8 File Offset: 0x005248B8
		[CompilerGenerated]
		internal static string <DrawConfigInfo>g__GenderLabel|42_0(Gender g)
		{
			if (g == Gender.None)
			{
				return "MaleAndFemale".Translate();
			}
			return g.GetLabel(false);
		}

		// Token: 0x0400B79D RID: 47005
		private Ideo ideo;

		// Token: 0x0400B79E RID: 47006
		private StyleItemDef hover;

		// Token: 0x0400B79F RID: 47007
		private StyleItemTab curTab;

		// Token: 0x0400B7A0 RID: 47008
		private DefMap<HairDef, StyleItemSpawningProperties> hairFrequencies;

		// Token: 0x0400B7A1 RID: 47009
		private DefMap<BeardDef, StyleItemSpawningProperties> beardFrequencies;

		// Token: 0x0400B7A2 RID: 47010
		private DefMap<TattooDef, StyleItemSpawningProperties> tattooFrequencies;

		// Token: 0x0400B7A3 RID: 47011
		private List<Dialog_EditIdeoStyleItems.ExpandedInfo> expandedInfos;

		// Token: 0x0400B7A4 RID: 47012
		private List<TabRecord> tabs = new List<TabRecord>();

		// Token: 0x0400B7A5 RID: 47013
		private Vector2 scrollPositionLeft;

		// Token: 0x0400B7A6 RID: 47014
		private Vector2 scrollPositionRight;

		// Token: 0x0400B7A7 RID: 47015
		private float scrollViewHeightLeft;

		// Token: 0x0400B7A8 RID: 47016
		private float scrollViewHeightRight;

		// Token: 0x0400B7A9 RID: 47017
		private bool painting;

		// Token: 0x0400B7AA RID: 47018
		private StyleItemFrequency[] allFrequencies;

		// Token: 0x0400B7AB RID: 47019
		private IdeoEditMode editMode;

		// Token: 0x0400B7AC RID: 47020
		private static readonly Vector2 ButSize = new Vector2(150f, 38f);

		// Token: 0x0400B7AD RID: 47021
		private static readonly Color HairColor = PawnHairColors.ReddishBrown;

		// Token: 0x0400B7AE RID: 47022
		private static readonly Color SemiSelectedColor = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x0400B7AF RID: 47023
		private const float HeaderHeight = 35f;

		// Token: 0x0400B7B0 RID: 47024
		private const float TabsSpacing = 45f;

		// Token: 0x0400B7B1 RID: 47025
		private const float ItemHeight = 28f;

		// Token: 0x0400B7B2 RID: 47026
		private const float ItemLabelWidth = 110f;

		// Token: 0x0400B7B3 RID: 47027
		private static readonly Texture2D Plus = ContentFinder<Texture2D>.Get("UI/Buttons/Plus", true);

		// Token: 0x0400B7B4 RID: 47028
		private static readonly Texture2D Minus = ContentFinder<Texture2D>.Get("UI/Buttons/Minus", true);

		// Token: 0x0400B7B5 RID: 47029
		private static readonly Texture2D RadioButSemiOn = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButSemiOn", true);

		// Token: 0x0400B7B6 RID: 47030
		private Dictionary<string, string> labelCache = new Dictionary<string, string>();

		// Token: 0x02003165 RID: 12645
		public enum ItemType
		{
			// Token: 0x0400B7B8 RID: 47032
			Hair,
			// Token: 0x0400B7B9 RID: 47033
			Beard,
			// Token: 0x0400B7BA RID: 47034
			FaceTattoo,
			// Token: 0x0400B7BB RID: 47035
			BodyTattoo
		}

		// Token: 0x02003166 RID: 12646
		public class ExpandedInfo
		{
			// Token: 0x06011C97 RID: 72855 RVA: 0x005266D4 File Offset: 0x005248D4
			public ExpandedInfo(StyleItemCategoryDef categoryDef, Dialog_EditIdeoStyleItems.ItemType itemType, bool any)
			{
				this.categoryDef = categoryDef;
				this.itemType = itemType;
				this.any = any;
				this.expanded = false;
			}

			// Token: 0x0400B7BC RID: 47036
			public StyleItemCategoryDef categoryDef;

			// Token: 0x0400B7BD RID: 47037
			public Dialog_EditIdeoStyleItems.ItemType itemType;

			// Token: 0x0400B7BE RID: 47038
			public bool any;

			// Token: 0x0400B7BF RID: 47039
			public bool expanded;
		}
	}
}
