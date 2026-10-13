using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200337A RID: 13178
	public class ArchitectCategoryTab
	{
		// Token: 0x1700305D RID: 12381
		// (get) Token: 0x0601286C RID: 75884 RVA: 0x00577783 File Offset: 0x00575983
		public bool AnySearchMatches
		{
			get
			{
				return this.anySearchMatches;
			}
		}

		// Token: 0x1700305E RID: 12382
		// (get) Token: 0x0601286D RID: 75885 RVA: 0x0057778B File Offset: 0x0057598B
		public Designator UniqueSearchMatch
		{
			get
			{
				return this.uniqueSearchMatch;
			}
		}

		// Token: 0x1700305F RID: 12383
		// (get) Token: 0x0601286E RID: 75886 RVA: 0x00577793 File Offset: 0x00575993
		public bool Visible
		{
			get
			{
				return this.def.Visible;
			}
		}

		// Token: 0x17003060 RID: 12384
		// (get) Token: 0x0601286F RID: 75887 RVA: 0x005777A0 File Offset: 0x005759A0
		public int? PreferredColumn
		{
			get
			{
				return this.def.preferredColumn;
			}
		}

		// Token: 0x17003061 RID: 12385
		// (get) Token: 0x06012870 RID: 75888 RVA: 0x005777AD File Offset: 0x005759AD
		public static Rect InfoRect
		{
			get
			{
				return new Rect(0f, (float)(UI.screenHeight - 35) - ((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).WinHeight - 270f, 200f, 270f);
			}
		}

		// Token: 0x06012871 RID: 75889 RVA: 0x005777E7 File Offset: 0x005759E7
		public ArchitectCategoryTab(DesignationCategoryDef def, QuickSearchFilter quickSearchFilter)
		{
			this.def = def;
			this.quickSearchFilter = quickSearchFilter;
			this.shouldLowLightGizmoFunc = new Func<Gizmo, bool>(this.ShouldLowLightGizmo);
			this.shouldHighLightGizmoFunc = new Func<Gizmo, bool>(this.ShouldHighLightGizmo);
		}

		// Token: 0x06012872 RID: 75890 RVA: 0x00577824 File Offset: 0x00575A24
		public void DesignationTabOnGUI(Designator forceActivatedCommand)
		{
			if (Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0f, (float)(UI.screenHeight - 35) - ((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).WinHeight - 270f);
			}
			Func<Gizmo, bool> customActivatorFunc = (forceActivatedCommand == null) ? null : new Func<Gizmo, bool>((Gizmo cmd) => cmd == forceActivatedCommand);
			float startX = 210f;
			Gizmo selectedDesignator;
			GizmoGridDrawer.DrawGizmoGrid(this.def.ResolvedAllowedDesignators, startX, out selectedDesignator, customActivatorFunc, this.shouldHighLightGizmoFunc, this.shouldLowLightGizmoFunc, false);
			if (selectedDesignator == null && Find.DesignatorManager.SelectedDesignator != null)
			{
				selectedDesignator = Find.DesignatorManager.SelectedDesignator;
			}
			this.DoInfoBox(ArchitectCategoryTab.InfoRect, (Designator)selectedDesignator);
		}

		// Token: 0x06012873 RID: 75891 RVA: 0x005778EC File Offset: 0x00575AEC
		private bool ShouldLowLightGizmo(Gizmo gizmo)
		{
			Command command = gizmo as Command;
			return command != null && (this.quickSearchFilter.Active && !this.Matches(command));
		}

		// Token: 0x06012874 RID: 75892 RVA: 0x00577920 File Offset: 0x00575B20
		private bool ShouldHighLightGizmo(Gizmo gizmo)
		{
			Command command = gizmo as Command;
			return command != null && (this.quickSearchFilter.Active && this.Matches(command));
		}

		// Token: 0x06012875 RID: 75893 RVA: 0x00577952 File Offset: 0x00575B52
		private bool Matches(Command c)
		{
			return this.quickSearchFilter.Matches(c.Label);
		}

		// Token: 0x06012876 RID: 75894 RVA: 0x00577968 File Offset: 0x00575B68
		protected void DoInfoBox(Rect infoRect, Designator designator)
		{
			Find.WindowStack.ImmediateWindow(32520, infoRect, WindowLayer.GameUI, delegate
			{
				if (designator != null)
				{
					Rect rect = infoRect.AtZero().ContractedBy(7f);
					Widgets.BeginGroup(rect);
					Rect rect2 = new Rect(0f, 0f, rect.width - designator.PanelReadoutTitleExtraRightMargin, 999f);
					Text.Font = GameFont.Small;
					Widgets.Label(rect2, designator.LabelCap);
					float num = Mathf.Max(24f, Text.CalcHeight(designator.LabelCap, rect2.width));
					designator.DrawPanelReadout(ref num, rect.width);
					Rect rect3 = new Rect(0f, num, rect.width, rect.height - num);
					string text = designator.Desc;
					GenText.SetTextSizeToFit(text, rect3);
					text = text.TruncateHeight(rect3.width, rect3.height, null);
					Widgets.Label(rect3, text);
					Widgets.EndGroup();
				}
			}, true, false, 1f, null, false);
		}

		// Token: 0x06012877 RID: 75895 RVA: 0x005779B4 File Offset: 0x00575BB4
		public void CacheSearchState()
		{
			this.anySearchMatches = true;
			this.uniqueSearchMatch = null;
			if (!this.quickSearchFilter.Active)
			{
				return;
			}
			int num = 0;
			Designator designator = null;
			foreach (Designator designator2 in this.def.ResolvedAllowedDesignators)
			{
				if (this.Matches(designator2))
				{
					num++;
					designator = designator2;
					if (num > 1)
					{
						return;
					}
				}
			}
			if (num == 0)
			{
				this.anySearchMatches = false;
				return;
			}
			if (num == 1)
			{
				this.uniqueSearchMatch = designator;
			}
		}

		// Token: 0x0400C079 RID: 49273
		public readonly DesignationCategoryDef def;

		// Token: 0x0400C07A RID: 49274
		private readonly QuickSearchFilter quickSearchFilter;

		// Token: 0x0400C07B RID: 49275
		private bool anySearchMatches;

		// Token: 0x0400C07C RID: 49276
		private Designator uniqueSearchMatch;

		// Token: 0x0400C07D RID: 49277
		private readonly Func<Gizmo, bool> shouldLowLightGizmoFunc;

		// Token: 0x0400C07E RID: 49278
		private readonly Func<Gizmo, bool> shouldHighLightGizmoFunc;

		// Token: 0x0400C07F RID: 49279
		public const float InfoRectHeight = 270f;
	}
}
