using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033D5 RID: 13269
	public class ITab_StudyNotes : ITab
	{
		// Token: 0x170030BF RID: 12479
		// (get) Token: 0x06012A42 RID: 76354 RVA: 0x005841FF File Offset: 0x005823FF
		protected Thing StudiableThing
		{
			get
			{
				Building_HoldingPlatform building_HoldingPlatform = base.SelThing as Building_HoldingPlatform;
				return ((building_HoldingPlatform != null) ? building_HoldingPlatform.HeldPawn : null) ?? base.SelThing;
			}
		}

		// Token: 0x170030C0 RID: 12480
		// (get) Token: 0x06012A43 RID: 76355 RVA: 0x00584222 File Offset: 0x00582422
		protected bool Studiable
		{
			get
			{
				CompStudiable compStudiable = this.StudiableThing.TryGetComp<CompStudiable>();
				return compStudiable != null && compStudiable.StudyUnlocked();
			}
		}

		// Token: 0x170030C1 RID: 12481
		// (get) Token: 0x06012A44 RID: 76356 RVA: 0x0058423A File Offset: 0x0058243A
		public override bool IsVisible
		{
			get
			{
				return this.Studiable && this.StudyUnlocks != null;
			}
		}

		// Token: 0x170030C2 RID: 12482
		// (get) Token: 0x06012A45 RID: 76357 RVA: 0x0058424F File Offset: 0x0058244F
		private CompStudyUnlocks StudyUnlocks
		{
			get
			{
				return this.StudiableThing.TryGetComp<CompStudyUnlocks>();
			}
		}

		// Token: 0x170030C3 RID: 12483
		// (get) Token: 0x06012A46 RID: 76358 RVA: 0x0058425C File Offset: 0x0058245C
		protected virtual IReadOnlyList<ChoiceLetter> Letters
		{
			get
			{
				return this.StudyUnlocks.Letters;
			}
		}

		// Token: 0x170030C4 RID: 12484
		// (get) Token: 0x06012A47 RID: 76359 RVA: 0x00584269 File Offset: 0x00582469
		protected virtual bool StudyCompleted
		{
			get
			{
				return this.StudyUnlocks.Completed;
			}
		}

		// Token: 0x170030C5 RID: 12485
		// (get) Token: 0x06012A48 RID: 76360 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool DrawThingIcon
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06012A49 RID: 76361 RVA: 0x00584276 File Offset: 0x00582476
		public ITab_StudyNotes()
		{
			this.size = new Vector2(Mathf.Min(610f, (float)UI.screenWidth), 350f);
			this.labelKey = "TabStudyNotesContents";
		}

		// Token: 0x06012A4A RID: 76362 RVA: 0x005842A9 File Offset: 0x005824A9
		public override void OnOpen()
		{
			this.selectedLetter = (this.Letters.EnumerableNullOrEmpty<ChoiceLetter>() ? null : this.Letters.Last<ChoiceLetter>());
		}

		// Token: 0x06012A4B RID: 76363 RVA: 0x005842CC File Offset: 0x005824CC
		protected override void FillTab()
		{
			if (this.previous != this.StudiableThing)
			{
				this.selectedLetter = (this.Letters.EnumerableNullOrEmpty<ChoiceLetter>() ? null : this.Letters.Last<ChoiceLetter>());
				this.previous = this.StudiableThing;
			}
			Rect rect = new Rect(0f, 20f, this.size.x, this.size.y - 20f);
			rect = rect.ContractedBy(10f);
			Rect rect2 = rect;
			rect2.y = 10f;
			rect2.height = 30f;
			Rect rect3 = rect;
			rect3.yMin = rect2.yMax + 17f;
			Rect rect4;
			Rect rect5;
			rect3.SplitVerticallyWithMargin(out rect4, out rect5, 17f);
			rect5.yMin += 17f;
			Rect rect6 = rect5;
			rect6.xMin -= 17f;
			rect6.yMin = rect.yMin;
			this.DrawTitle(rect2);
			this.DrawLetters(rect4);
			if (this.selectedLetter != null)
			{
				Widgets.LabelScrollable(rect5, this.selectedLetter.Text, ref this.rightScroll, false, true, false);
				return;
			}
			using (new TextBlock(new GameFont?(GameFont.Small), new TextAnchor?(TextAnchor.MiddleCenter), new Color?(Color.gray)))
			{
				Widgets.Label(rect6, "StudyNotesTab_NoDiscoveries".Translate());
			}
		}

		// Token: 0x06012A4C RID: 76364 RVA: 0x0058444C File Offset: 0x0058264C
		private void DrawTitle(Rect rect)
		{
			float num = 0f;
			if (this.DrawThingIcon)
			{
				Rect position = rect;
				position.width = position.height;
				num = rect.height + 10f;
				GUI.DrawTexture(position, this.StudiableThing.def.uiIcon);
			}
			Rect rect2 = rect;
			rect2.xMin += num;
			rect2.xMax = rect.x + rect.width / 2f;
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax;
			Rect rect4 = rect3;
			rect4.y = rect2.yMax - 4f;
			using (new TextBlock(GameFont.Medium, TextAnchor.MiddleLeft))
			{
				Widgets.LabelFit(rect2, this.StudiableThing.LabelCap);
			}
			CompStudiable compStudiable = this.StudiableThing.TryGetComp<CompStudiable>();
			if (compStudiable != null)
			{
				Widgets.CheckboxLabeled(rect3, "StudyNotesTab_ToggleStudy".Translate(), ref compStudiable.studyEnabled, false, null, null, true, false);
				string text;
				if (!compStudiable.EverStudiable(out text) && !text.NullOrEmpty())
				{
					using (new TextBlock(ColorLibrary.RedReadable))
					{
						Widgets.Label(rect4, text);
					}
				}
			}
		}

		// Token: 0x06012A4D RID: 76365 RVA: 0x005845A4 File Offset: 0x005827A4
		private void DrawLetters(Rect rect)
		{
			Rect rect2 = rect;
			rect2.height = Text.LineHeight;
			TaggedString taggedString = this.StudyCompleted ? "StudyNotesTab_StudyProgressCompleted".Translate() : "StudyNotesTab_StudyProgressOngoing".Translate();
			TaggedString taggedString2 = "StudyNotesTab_StudyProgress".Translate();
			using (new TextBlock(TextAnchor.MiddleLeft))
			{
				Widgets.Label(rect2, string.Format("{0}: {1}", taggedString2, taggedString));
			}
			Widgets.DrawLineHorizontal(rect.x, rect2.yMax + 4f, rect.width, Color.gray);
			int num = this.Letters.EnumerableNullOrEmpty<ChoiceLetter>() ? 0 : this.Letters.Count;
			Rect outRect = rect;
			outRect.yMin = rect2.yMax + 10f;
			Rect rect3 = new Rect(0f, 0f, rect.width, 30f * (float)num);
			float num2 = 0f;
			Widgets.BeginScrollView(outRect, ref this.leftScroll, rect3, true);
			for (int i = num - 1; i >= 0; i--)
			{
				this.DoLetterRow(rect3, ref num2, this.Letters[i], i);
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06012A4E RID: 76366 RVA: 0x005846EC File Offset: 0x005828EC
		private void DoLetterRow(Rect rect, ref float y, ChoiceLetter letter, int index)
		{
			rect.y = y;
			rect.height = 30f;
			y += 30f;
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlightSelected(rect);
				this.selectedLetter = letter;
			}
			else if (this.selectedLetter == letter)
			{
				Widgets.DrawHighlightSelected(rect);
			}
			else if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Rect rect2 = rect;
			rect2.width = 90f;
			Vector2 location = (Find.CurrentMap != null) ? Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile) : default(Vector2);
			string str = GenDate.DateShortStringAt((long)GenDate.TickGameToAbs(letter.arrivalTick), location);
			Rect rect3 = rect;
			rect3.xMin = rect2.xMax + 4f;
			using (new TextBlock(new GameFont?(GameFont.Small), new TextAnchor?(TextAnchor.MiddleLeft), new bool?(false)))
			{
				Widgets.Label(rect2, str.Truncate(rect2.width, null));
				using (new TextBlock(new Color(0.75f, 0.75f, 0.75f)))
				{
					Widgets.LabelEllipses(rect3, letter.Label);
				}
			}
		}

		// Token: 0x0400C196 RID: 49558
		private Vector2 leftScroll;

		// Token: 0x0400C197 RID: 49559
		private Vector2 rightScroll;

		// Token: 0x0400C198 RID: 49560
		private ChoiceLetter selectedLetter;

		// Token: 0x0400C199 RID: 49561
		private Thing previous;

		// Token: 0x0400C19A RID: 49562
		private const float TopPadding = 20f;

		// Token: 0x0400C19B RID: 49563
		private const float InitialHeight = 350f;

		// Token: 0x0400C19C RID: 49564
		private const float TitleHeight = 30f;

		// Token: 0x0400C19D RID: 49565
		private const float InitialWidth = 610f;

		// Token: 0x0400C19E RID: 49566
		private const float DateSize = 90f;

		// Token: 0x0400C19F RID: 49567
		private const float RowHeight = 30f;
	}
}
