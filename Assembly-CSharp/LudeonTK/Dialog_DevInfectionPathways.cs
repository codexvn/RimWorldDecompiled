using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200008C RID: 140
	[StaticConstructorOnStartup]
	public class Dialog_DevInfectionPathways : Window_Dev
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002FA RID: 762 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0000FF5A File Offset: 0x0000E15A
		protected override float Margin
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00010197 File Offset: 0x0000E397
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(230f, 330f);
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000101A8 File Offset: 0x0000E3A8
		public Dialog_DevInfectionPathways()
		{
			this.draggable = true;
			this.focusWhenOpened = false;
			this.drawShadow = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.preventCameraMotion = false;
			this.drawInScreenshotMode = false;
			this.windowPosition = Prefs.DevPalettePosition;
			this.onlyDrawInDevMode = true;
			this.doCloseX = true;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00010208 File Offset: 0x0000E408
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect rect = new Rect(inRect.x, inRect.y, inRect.width, 24f);
			Rect rect2 = new Rect(inRect.x, inRect.height / 2f - 12f, inRect.width, 24f);
			DevGUI.Label(rect, "Infection Pathway Debugger");
			Text.Font = GameFont.Tiny;
			List<Pawn> selectedPawns = Find.Selector.SelectedPawns;
			if (selectedPawns.Count == 0 || !selectedPawns[0].RaceProps.Humanlike)
			{
				DevGUI.Label(rect2, "No valid humanlike selected");
				return;
			}
			Pawn pawn = Find.Selector.SelectedPawns[0];
			if (pawn.infectionVectors.PathwaysCount == 0)
			{
				DevGUI.Label(rect2, "No vectors");
				return;
			}
			Rect outRect = inRect;
			outRect.yMin = rect.height + 6f;
			Rect rect3 = inRect;
			rect3.y = 0f;
			rect3.height = this.lastHeight;
			Widgets.BeginScrollView(outRect, ref this.scroll, rect3, true);
			float num = 0f;
			int num2 = 0;
			this.PrintLabel("Def name", "Age", 1, rect3, ref num);
			foreach (InfectionPathway pathway in pawn.infectionVectors.Pathways)
			{
				this.PrintLabel(pathway, num2++, rect3, ref num);
			}
			this.lastHeight = num;
			Widgets.EndScrollView();
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0001039C File Offset: 0x0000E59C
		private void PrintLabel(InfectionPathway pathway, int row, Rect container, ref float y)
		{
			this.PrintLabel(pathway.Def.label, pathway.AgeTicks.ToStringSecondsFromTicks(), row, container, ref y);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x000103C0 File Offset: 0x0000E5C0
		private void PrintLabel(string key, string value, int row, Rect container, ref float y)
		{
			Rect rect = new Rect(container.x, y, container.width, 20f);
			float num = container.width * 0.6f;
			Rect rect2 = new Rect(container.x, y, num, 20f);
			Rect rect3 = new Rect(container.x + num, y, num, 20f);
			if (row % 2 == 0)
			{
				DevGUI.DrawLightHighlight(rect);
			}
			DevGUI.Label(rect2, key);
			DevGUI.Label(rect3, value);
			y += 20f;
		}

		// Token: 0x040001B9 RID: 441
		private Vector2 windowPosition;

		// Token: 0x040001BA RID: 442
		private Vector2 scroll;

		// Token: 0x040001BB RID: 443
		private float lastHeight;

		// Token: 0x040001BC RID: 444
		private const string Title = "Infection Pathway Debugger";

		// Token: 0x040001BD RID: 445
		private const float ButtonHeight = 30f;
	}
}
