using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200312F RID: 12591
	public class Dialog_ChangeDryadCaste : Window
	{
		// Token: 0x17002E05 RID: 11781
		// (get) Token: 0x06011B2A RID: 72490 RVA: 0x0051752F File Offset: 0x0051572F
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)Mathf.Min(900, UI.screenWidth), 650f);
			}
		}

		// Token: 0x17002E06 RID: 11782
		// (get) Token: 0x06011B2B RID: 72491 RVA: 0x0051754B File Offset: 0x0051574B
		private PawnKindDef SelectedKind
		{
			get
			{
				return this.selectedMode.pawnKindDef;
			}
		}

		// Token: 0x06011B2C RID: 72492 RVA: 0x00517558 File Offset: 0x00515758
		public Dialog_ChangeDryadCaste(Thing tree) : base(null)
		{
			this.treeConnection = tree.TryGetComp<CompTreeConnection>();
			this.currentMode = this.treeConnection.desiredMode;
			this.selectedMode = this.currentMode;
			this.connectedPawn = this.treeConnection.ConnectedPawn;
			this.forcePause = true;
			this.closeOnAccept = false;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.allDryadModes = DefDatabase<GauranlenTreeModeDef>.AllDefs.ToList<GauranlenTreeModeDef>();
		}

		// Token: 0x06011B2D RID: 72493 RVA: 0x005175D2 File Offset: 0x005157D2
		public override void PreOpen()
		{
			if (!ModLister.CheckIdeology("Dryad upgrades"))
			{
				this.Close(true);
			}
			base.PreOpen();
			this.SetupView();
		}

		// Token: 0x06011B2E RID: 72494 RVA: 0x005175F4 File Offset: 0x005157F4
		private void SetupView()
		{
			foreach (GauranlenTreeModeDef stage in this.allDryadModes)
			{
				this.rightViewWidth = Mathf.Max(this.rightViewWidth, this.GetPosition(stage, this.InitialSize.y).x + Dialog_ChangeDryadCaste.OptionSize.x);
			}
			this.rightViewWidth += 20f;
		}

		// Token: 0x06011B2F RID: 72495 RVA: 0x00517688 File Offset: 0x00515888
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			string label = (this.selectedMode != null) ? this.selectedMode.LabelCap : "ChangeMode".Translate();
			Widgets.Label(new Rect(inRect.x, inRect.y, inRect.width, 35f), label);
			Text.Font = GameFont.Small;
			float num = inRect.y + 35f + 10f;
			float num2 = num;
			float num3 = inRect.height - num;
			num3 -= Dialog_ChangeDryadCaste.ButSize.y + 10f;
			this.DrawLeftRect(new Rect(inRect.xMin, num, 400f, num3), ref num2);
			this.DrawRightRect(new Rect(inRect.x + 400f + 17f, num, inRect.width - 400f - 17f, num3));
		}

		// Token: 0x06011B30 RID: 72496 RVA: 0x0051776C File Offset: 0x0051596C
		private void DrawLeftRect(Rect rect, ref float curY)
		{
			Rect rect2 = new Rect(rect.x, curY, rect.width, rect.height)
			{
				yMax = rect.yMax
			}.ContractedBy(4f);
			if (this.selectedMode == null)
			{
				Widgets.Label(rect2, "ChooseProductionModeInitialDesc".Translate(this.connectedPawn.Named("PAWN"), this.treeConnection.parent.Named("TREE"), ThingDefOf.DryadCocoon.GetCompProperties<CompProperties_DryadCocoon>().daysToComplete.Named("UPGRADEDURATION")));
				return;
			}
			Widgets.Label(rect2.x, ref curY, rect2.width, this.selectedMode.Description, default(TipSignal));
			curY += 10f;
			if (!Find.IdeoManager.classicMode && !this.selectedMode.requiredMemes.NullOrEmpty<MemeDef>())
			{
				Widgets.Label(rect2.x, ref curY, rect2.width, "RequiredMemes".Translate() + ":", default(TipSignal));
				string text = "";
				for (int i = 0; i < this.selectedMode.requiredMemes.Count; i++)
				{
					MemeDef memeDef = this.selectedMode.requiredMemes[i];
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text = text + "  - " + memeDef.LabelCap.ToString().Colorize(this.connectedPawn.Ideo.HasMeme(memeDef) ? Color.white : ColorLibrary.RedReadable);
				}
				Widgets.Label(rect2.x, ref curY, rect2.width, text, default(TipSignal));
				curY += 10f;
			}
			if (this.selectedMode.previousStage != null)
			{
				Widgets.Label(rect2.x, ref curY, rect2.width, "RequiredStage".Translate() + ": " + this.selectedMode.previousStage.pawnKindDef.LabelCap.ToString().Colorize(Color.white), default(TipSignal));
				curY += 10f;
			}
			if (this.selectedMode.displayedStats != null)
			{
				for (int j = 0; j < this.selectedMode.displayedStats.Count; j++)
				{
					StatDef statDef = this.selectedMode.displayedStats[j];
					Widgets.Label(rect2.x, ref curY, rect2.width, statDef.LabelCap + ": " + statDef.ValueToString(this.SelectedKind.race.GetStatValueAbstract(statDef, null), statDef.toStringNumberSense, true), default(TipSignal));
				}
				curY += 10f;
			}
			if (this.selectedMode.hyperlinks != null)
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in Dialog_InfoCard.DefsToHyperlinks(this.selectedMode.hyperlinks))
				{
					Widgets.HyperlinkWithIcon(new Rect(rect2.x, curY, rect2.width, Text.LineHeight), hyperlink, null, 2f, 6f, null, false, null);
					curY += Text.LineHeight;
				}
				curY += 10f;
			}
			Rect rect3 = new Rect(rect2.x, rect2.yMax - 55f, rect2.width, 55f);
			if (this.MeetsRequirements(this.selectedMode) && this.selectedMode != this.currentMode)
			{
				if (Widgets.ButtonText(rect3, "Accept".Translate(), true, true, true, null))
				{
					Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("GauranlenModeChangeDescFull".Translate(this.treeConnection.parent.Named("TREE"), this.connectedPawn.Named("CONNECTEDPAWN"), ThingDefOf.DryadCocoon.GetCompProperties<CompProperties_DryadCocoon>().daysToComplete.Named("DURATION")), delegate()
					{
						this.StartChange();
					}, false, null, WindowLayer.Dialog);
					Find.WindowStack.Add(window);
					return;
				}
			}
			else
			{
				string label;
				if (this.selectedMode == this.currentMode)
				{
					label = "AlreadySelected".Translate();
				}
				else if (!this.MeetsMemeRequirements(this.selectedMode))
				{
					label = "MissingRequiredMemes".Translate();
				}
				else if (this.selectedMode.previousStage != null && this.currentMode != this.selectedMode.previousStage)
				{
					label = "Locked".Translate() + ": " + "MissingRequiredCaste".Translate();
				}
				else
				{
					label = "Locked".Translate();
				}
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.DrawHighlight(rect3);
				Widgets.Label(rect3.ContractedBy(5f), label);
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		// Token: 0x06011B31 RID: 72497 RVA: 0x00517CCC File Offset: 0x00515ECC
		private void StartChange()
		{
			this.treeConnection.desiredMode = this.selectedMode;
			SoundDefOf.GauranlenProductionModeSet.PlayOneShotOnCamera(null);
			this.Close(false);
		}

		// Token: 0x06011B32 RID: 72498 RVA: 0x00517CF4 File Offset: 0x00515EF4
		private void DrawRightRect(Rect rect)
		{
			Widgets.DrawMenuSection(rect);
			Rect rect2 = new Rect(0f, 0f, this.rightViewWidth, rect.height - 16f);
			Rect rect3 = rect2.ContractedBy(10f);
			Widgets.ScrollHorizontal(rect, ref this.scrollPosition, rect2, 20f);
			Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, true);
			Widgets.BeginGroup(rect3);
			this.DrawDependencyLines(rect3);
			foreach (GauranlenTreeModeDef stage in this.allDryadModes)
			{
				this.DrawDryadStage(rect3, stage);
			}
			Widgets.EndGroup();
			Widgets.EndScrollView();
		}

		// Token: 0x06011B33 RID: 72499 RVA: 0x00517DB8 File Offset: 0x00515FB8
		private bool MeetsMemeRequirements(GauranlenTreeModeDef stage)
		{
			if (!Find.IdeoManager.classicMode && !stage.requiredMemes.NullOrEmpty<MemeDef>())
			{
				foreach (MemeDef meme in stage.requiredMemes)
				{
					if (!this.connectedPawn.Ideo.HasMeme(meme))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06011B34 RID: 72500 RVA: 0x00517E38 File Offset: 0x00516038
		private bool MeetsRequirements(GauranlenTreeModeDef mode)
		{
			return (mode.previousStage == null || this.currentMode == mode.previousStage) && this.MeetsMemeRequirements(mode);
		}

		// Token: 0x06011B35 RID: 72501 RVA: 0x00517E5C File Offset: 0x0051605C
		private Color GetBoxColor(GauranlenTreeModeDef mode)
		{
			Color color = TexUI.AvailResearchColor;
			if (mode == this.currentMode)
			{
				color = TexUI.OldActiveResearchColor;
			}
			else if (!this.MeetsRequirements(mode))
			{
				color = TexUI.LockedResearchColor;
			}
			if (this.selectedMode == mode)
			{
				color += TexUI.HighlightBgResearchColor;
			}
			return color;
		}

		// Token: 0x06011B36 RID: 72502 RVA: 0x00517EA5 File Offset: 0x005160A5
		private Color GetBoxOutlineColor(GauranlenTreeModeDef mode)
		{
			if (this.selectedMode != null && this.selectedMode == mode)
			{
				return TexUI.HighlightBorderResearchColor;
			}
			return TexUI.DefaultBorderResearchColor;
		}

		// Token: 0x06011B37 RID: 72503 RVA: 0x00517EC3 File Offset: 0x005160C3
		private Color GetTextColor(GauranlenTreeModeDef mode)
		{
			if (!this.MeetsRequirements(mode))
			{
				return ColorLibrary.RedReadable;
			}
			return Color.white;
		}

		// Token: 0x06011B38 RID: 72504 RVA: 0x00517EDC File Offset: 0x005160DC
		private void DrawDependencyLines(Rect fullRect)
		{
			foreach (GauranlenTreeModeDef gauranlenTreeModeDef in this.allDryadModes)
			{
				if (gauranlenTreeModeDef.previousStage != null)
				{
					this.DrawLineBetween(gauranlenTreeModeDef, gauranlenTreeModeDef.previousStage, fullRect.height, TexUI.DefaultLineResearchColor, 2f);
				}
			}
			foreach (GauranlenTreeModeDef gauranlenTreeModeDef2 in this.allDryadModes)
			{
				if (gauranlenTreeModeDef2.previousStage != null && (gauranlenTreeModeDef2.previousStage == this.selectedMode || this.selectedMode == gauranlenTreeModeDef2))
				{
					this.DrawLineBetween(gauranlenTreeModeDef2, gauranlenTreeModeDef2.previousStage, fullRect.height, TexUI.HighlightLineResearchColor, 3f);
				}
			}
		}

		// Token: 0x06011B39 RID: 72505 RVA: 0x00517FC8 File Offset: 0x005161C8
		private void DrawDryadStage(Rect rect, GauranlenTreeModeDef stage)
		{
			Vector2 position = this.GetPosition(stage, rect.height);
			Rect rect2 = new Rect(position.x, position.y, Dialog_ChangeDryadCaste.OptionSize.x, Dialog_ChangeDryadCaste.OptionSize.y);
			Widgets.DrawBoxSolidWithOutline(rect2, this.GetBoxColor(stage), this.GetBoxOutlineColor(stage), 1);
			Rect rect3 = new Rect(rect2.x, rect2.y, rect2.height, rect2.height);
			Widgets.DefIcon(rect3.ContractedBy(4f), stage.pawnKindDef, null, 1f, null, false, null, null, null, 1f);
			GUI.color = this.GetTextColor(stage);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(new Rect(rect3.xMax, rect2.y, rect2.width - rect3.width, rect2.height).ContractedBy(4f), stage.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			if (Widgets.ButtonInvisible(rect2, true))
			{
				this.selectedMode = stage;
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06011B3A RID: 72506 RVA: 0x005180F8 File Offset: 0x005162F8
		private void DrawLineBetween(GauranlenTreeModeDef left, GauranlenTreeModeDef right, float height, Color color, float width = 2f)
		{
			Vector2 start = this.GetPosition(left, height) + new Vector2(5f, Dialog_ChangeDryadCaste.OptionSize.y / 2f);
			Vector2 end = this.GetPosition(right, height) + Dialog_ChangeDryadCaste.OptionSize / 2f;
			Widgets.DrawLine(start, end, color, width);
		}

		// Token: 0x06011B3B RID: 72507 RVA: 0x00518154 File Offset: 0x00516354
		private Vector2 GetPosition(GauranlenTreeModeDef stage, float height)
		{
			return new Vector2(stage.drawPosition.x * Dialog_ChangeDryadCaste.OptionSize.x + stage.drawPosition.x * 52f, (height - Dialog_ChangeDryadCaste.OptionSize.y) * stage.drawPosition.y);
		}

		// Token: 0x0400B642 RID: 46658
		private CompTreeConnection treeConnection;

		// Token: 0x0400B643 RID: 46659
		private Pawn connectedPawn;

		// Token: 0x0400B644 RID: 46660
		private Vector2 scrollPosition;

		// Token: 0x0400B645 RID: 46661
		private GauranlenTreeModeDef selectedMode;

		// Token: 0x0400B646 RID: 46662
		private GauranlenTreeModeDef currentMode;

		// Token: 0x0400B647 RID: 46663
		private float rightViewWidth;

		// Token: 0x0400B648 RID: 46664
		private List<GauranlenTreeModeDef> allDryadModes;

		// Token: 0x0400B649 RID: 46665
		private const float HeaderHeight = 35f;

		// Token: 0x0400B64A RID: 46666
		private const float LeftRectWidth = 400f;

		// Token: 0x0400B64B RID: 46667
		private const float OptionSpacing = 52f;

		// Token: 0x0400B64C RID: 46668
		private const float ChangeFormButtonHeight = 55f;

		// Token: 0x0400B64D RID: 46669
		private static readonly Vector2 OptionSize = new Vector2(190f, 46f);

		// Token: 0x0400B64E RID: 46670
		private static readonly Vector2 ButSize = new Vector2(200f, 40f);
	}
}
