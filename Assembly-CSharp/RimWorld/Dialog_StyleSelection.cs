using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020031DA RID: 12762
	public class Dialog_StyleSelection : Window
	{
		// Token: 0x17002E97 RID: 11927
		// (get) Token: 0x06011F29 RID: 73513 RVA: 0x005395B8 File Offset: 0x005377B8
		public override Vector2 InitialSize
		{
			get
			{
				float x = Mathf.Max(Text.CalcSize("StylesInUse".Translate()).x, 84f) + 10f + this.Margin * 2f;
				float y = Text.LineHeightOf(GameFont.Small) + 20f + 24f + this.Margin * 2f;
				return new Vector2(x, y);
			}
		}

		// Token: 0x17002E98 RID: 11928
		// (get) Token: 0x06011F2A RID: 73514 RVA: 0x0000FF5A File Offset: 0x0000E15A
		protected override float Margin
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x06011F2B RID: 73515 RVA: 0x00539621 File Offset: 0x00537821
		public Dialog_StyleSelection() : base(null)
		{
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.doWindowBackground = false;
			this.drawShadow = false;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x06011F2C RID: 73516 RVA: 0x00539658 File Offset: 0x00537858
		public override void DoWindowContents(Rect inRect)
		{
			this.UpdateBaseColor();
			GUI.color = this.baseColor;
			Text.Font = GameFont.Small;
			Widgets.DrawWindowBackground(inRect, this.baseColor);
			GUI.BeginGroup(inRect.ContractedBy(this.Margin));
			Widgets.Label(new Rect(0f, 0f, inRect.width, Text.LineHeight), "StylesInUse".Translate());
			float num = 0f;
			for (int i = 0; i < Find.IdeoManager.selectedStyleCategories.Count; i++)
			{
				StyleCategoryDef cat = Find.IdeoManager.selectedStyleCategories[i];
				Rect rect = new Rect(num, Text.LineHeight + 10f, 24f, 24f);
				if (Widgets.ButtonImage(rect, cat.Icon, this.baseColor, true, null))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (StyleCategoryDef styleCategoryDef in DefDatabase<StyleCategoryDef>.AllDefs)
					{
						StyleCategoryDef category = styleCategoryDef;
						if (category != cat && !Find.IdeoManager.selectedStyleCategories.Contains(styleCategoryDef))
						{
							list.Add(new FloatMenuOption(category.LabelCap, delegate()
							{
								Find.IdeoManager.selectedStyleCategories.Replace(cat, category);
								this.ClearCaches();
							}, category.Icon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
						}
					}
					list.Add(new FloatMenuOption("Remove".Translate().CapitalizeFirst(), delegate()
					{
						Find.IdeoManager.selectedStyleCategories.Remove(cat);
						this.ClearCaches();
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					Find.WindowStack.Add(new FloatMenu(list));
				}
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
					TooltipHandler.TipRegion(rect, IdeoUIUtility.StyleTooltip(cat, IdeoEditMode.GameStart, null, Dialog_StyleSelection.tmpStyles, true));
				}
				num += 28f;
			}
			if (Find.IdeoManager.selectedStyleCategories.Count < 3)
			{
				Rect rect2 = new Rect(num, Text.LineHeight + 10f, 24f, 24f);
				if (Widgets.ButtonImage(rect2, TexButton.Plus, this.baseColor, true, null))
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					foreach (StyleCategoryDef cat2 in DefDatabase<StyleCategoryDef>.AllDefs)
					{
						StyleCategoryDef cat = cat2;
						if (!Find.IdeoManager.selectedStyleCategories.Contains(cat))
						{
							list2.Add(new FloatMenuOption(cat.LabelCap, delegate()
							{
								Find.IdeoManager.selectedStyleCategories.Add(cat);
								this.ClearCaches();
							}, cat.Icon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
						}
					}
					if (list2.Any<FloatMenuOption>())
					{
						Find.WindowStack.Add(new FloatMenu(list2));
					}
				}
				if (Mouse.IsOver(rect2))
				{
					Widgets.DrawHighlight(rect2);
					TooltipHandler.TipRegion(rect2, string.Concat(new string[]
					{
						"AddStyleCategory".Translate().Colorize(ColoredText.TipSectionTitleColor),
						"\n\n",
						"StyleCategoryDescriptionAbstract".Translate().Colorize(ColoredText.SubtleGrayColor),
						"\n\n",
						"ClickToEdit".Translate().CapitalizeFirst().Colorize(ColorLibrary.Green)
					}));
				}
			}
			GUI.EndGroup();
			GUI.color = Color.white;
		}

		// Token: 0x06011F2D RID: 73517 RVA: 0x00539A58 File Offset: 0x00537C58
		private void ClearCaches()
		{
			Faction.OfPlayer.ideos.PrimaryIdeo.style.ResetStylesForThingDef();
			Faction.OfPlayer.ideos.PrimaryIdeo.RecachePossibleBuildables();
			foreach (DesignationCategoryDef designationCategoryDef in DefDatabase<DesignationCategoryDef>.AllDefs)
			{
				designationCategoryDef.DirtyCache();
			}
		}

		// Token: 0x06011F2E RID: 73518 RVA: 0x00539AD0 File Offset: 0x00537CD0
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 initialSize = this.InitialSize;
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			this.windowRect = new Rect(mousePositionOnUIInverted.x, mousePositionOnUIInverted.y - initialSize.y, initialSize.x, initialSize.y).Rounded();
		}

		// Token: 0x06011F2F RID: 73519 RVA: 0x00539B1C File Offset: 0x00537D1C
		private void UpdateBaseColor()
		{
			if (Find.WindowStack.FloatMenu != null)
			{
				this.baseColor = Color.white;
				return;
			}
			Rect r = new Rect(0f, 0f, this.InitialSize.x, this.InitialSize.y).ExpandedBy(5f);
			if (!r.Contains(Event.current.mousePosition))
			{
				float num = GenUI.DistFromRect(r, Event.current.mousePosition);
				this.baseColor = new Color(1f, 1f, 1f, 1f - num / 145f);
				if (num > 145f)
				{
					this.Close(false);
				}
			}
		}

		// Token: 0x0400B9D6 RID: 47574
		private Color baseColor = Color.white;

		// Token: 0x0400B9D7 RID: 47575
		private static readonly List<ThingStyleCategoryWithPriority> tmpStyles = new List<ThingStyleCategoryWithPriority>();

		// Token: 0x0400B9D8 RID: 47576
		private const float FadeStartMouseDist = 5f;

		// Token: 0x0400B9D9 RID: 47577
		private const float FadeFinishMouseDist = 150f;

		// Token: 0x0400B9DA RID: 47578
		private const int MaxButtonCount = 3;
	}
}
