using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003192 RID: 12690
	public class Dialog_ManageAreas : Window
	{
		// Token: 0x17002E67 RID: 11879
		// (get) Token: 0x06011D9E RID: 73118 RVA: 0x0052E86C File Offset: 0x0052CA6C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(550f, 400f);
			}
		}

		// Token: 0x06011D9F RID: 73119 RVA: 0x0052E87D File Offset: 0x0052CA7D
		public Dialog_ManageAreas(Map map) : base(null)
		{
			this.map = map;
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06011DA0 RID: 73120 RVA: 0x0052E8B0 File Offset: 0x0052CAB0
		public override void DoWindowContents(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = inRect.width;
			listing_Standard.Begin(inRect);
			List<Area> allAreas = this.map.areaManager.AllAreas;
			int i = 0;
			for (int j = 0; j < allAreas.Count; j++)
			{
				if (allAreas[j].Mutable)
				{
					Rect rect = listing_Standard.GetRect(24f, 1f);
					this.DoAreaRow(rect, allAreas[j], j);
					listing_Standard.Gap(6f);
					i++;
				}
			}
			if (this.map.areaManager.CanMakeNewAllowed())
			{
				while (i < 9)
				{
					listing_Standard.Gap(30f);
					i++;
				}
				if (listing_Standard.ButtonText("NewArea".Translate(), null, 1f))
				{
					Area_Allowed area_Allowed;
					this.map.areaManager.TryMakeNewAllowed(out area_Allowed);
				}
			}
			listing_Standard.End();
		}

		// Token: 0x06011DA1 RID: 73121 RVA: 0x0052E998 File Offset: 0x0052CB98
		private void DoAreaRow(Rect rect, Area area, int i)
		{
			if (Mouse.IsOver(rect))
			{
				area.MarkForDraw();
				GUI.color = area.Color;
				Widgets.DrawHighlight(rect);
				GUI.color = Color.white;
			}
			if (i % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Widgets.BeginGroup(rect);
			WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			Rect butRect = widgetRow.Icon(area.ColorTexture, null, 0f);
			Area_Allowed area_Allowed = area as Area_Allowed;
			if (area_Allowed != null && Widgets.ButtonInvisible(butRect, true))
			{
				Find.WindowStack.Add(new Dialog_AllowedAreaColorPicker(area_Allowed));
			}
			widgetRow.Gap(4f);
			using (new TextBlock(TextAnchor.LowerLeft))
			{
				widgetRow.LabelEllipses(area.Label, 160f, null, -1f);
			}
			if (widgetRow.ButtonText("ExpandArea".Translate(), null, true, true, true, new float?((float)60)))
			{
				this.SelectDesignator<Designator_AreaAllowedExpand>(area);
			}
			if (widgetRow.ButtonText("ShrinkArea".Translate(), null, true, true, true, new float?((float)60)))
			{
				this.SelectDesignator<Designator_AreaAllowedClear>(area);
			}
			if (widgetRow.ButtonText("InvertArea".Translate(), null, true, true, true, new float?((float)60)))
			{
				area.Invert();
			}
			if (widgetRow.ButtonIcon(TexButton.Rename, null, new Color?(GenUI.SubtleMouseoverColor), null, null, true, -1f))
			{
				Find.WindowStack.Add(new Dialog_RenameArea(area));
			}
			if (widgetRow.ButtonIcon(TexButton.Copy, null, new Color?(GenUI.SubtleMouseoverColor), null, null, true, -1f))
			{
				Area_Allowed area_Allowed2;
				if (this.map.areaManager.TryMakeNewAllowed(out area_Allowed2))
				{
					using (IEnumerator<IntVec3> enumerator = area.ActiveCells.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IntVec3 c = enumerator.Current;
							area_Allowed2[c] = true;
						}
						goto IL_226;
					}
				}
				Messages.Message("MaxAreasReached".Translate(10), MessageTypeDefOf.RejectInput, true);
			}
			IL_226:
			if (widgetRow.ButtonIcon(TexButton.Delete, null, new Color?(GenUI.SubtleMouseoverColor), null, null, true, -1f))
			{
				area.Delete();
			}
			Widgets.EndGroup();
		}

		// Token: 0x06011DA2 RID: 73122 RVA: 0x0052EC24 File Offset: 0x0052CE24
		private void SelectDesignator<T>(Area area) where T : Designator_AreaAllowed
		{
			Find.MainTabsRoot.EscapeCurrentTab(true);
			foreach (Designator designator in DesignationCategoryDefOf.Zone.AllResolvedDesignators)
			{
				T t = designator as T;
				if (t != null)
				{
					Designator_AreaAllowed.selectedArea = area;
					Find.DesignatorManager.Select(t);
				}
			}
			Find.WindowStack.TryRemove(this, true);
		}

		// Token: 0x0400B8AE RID: 47278
		private Map map;
	}
}
