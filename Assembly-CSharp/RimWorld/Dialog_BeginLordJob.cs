using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200311F RID: 12575
	[StaticConstructorOnStartup]
	public abstract class Dialog_BeginLordJob : Window
	{
		// Token: 0x17002DEB RID: 11755
		// (get) Token: 0x06011A98 RID: 72344 RVA: 0x00513863 File Offset: 0x00511A63
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(845f, 740f);
			}
		}

		// Token: 0x17002DEC RID: 11756
		// (get) Token: 0x06011A99 RID: 72345 RVA: 0x00513874 File Offset: 0x00511A74
		protected virtual Vector2 ButtonSize
		{
			get
			{
				return new Vector2(200f, 40f);
			}
		}

		// Token: 0x06011A9A RID: 72346 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual IEnumerable<string> BlockingIssues()
		{
			return null;
		}

		// Token: 0x17002DED RID: 11757
		// (get) Token: 0x06011A9B RID: 72347 RVA: 0x00513888 File Offset: 0x00511A88
		protected string WarningText
		{
			get
			{
				if (this.BlockingIssues() == null)
				{
					return "";
				}
				string result = "";
				using (IEnumerator<string> enumerator = this.BlockingIssues().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						result = enumerator.Current;
					}
				}
				return result;
			}
		}

		// Token: 0x17002DEE RID: 11758
		// (get) Token: 0x06011A9C RID: 72348 RVA: 0x005138E8 File Offset: 0x00511AE8
		public virtual bool CanBegin
		{
			get
			{
				IEnumerable<string> enumerable = this.BlockingIssues();
				return enumerable == null || !enumerable.Any<string>();
			}
		}

		// Token: 0x17002DEF RID: 11759
		// (get) Token: 0x06011A9D RID: 72349 RVA: 0x0051390A File Offset: 0x00511B0A
		public virtual TaggedString HeaderLabel
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17002DF0 RID: 11760
		// (get) Token: 0x06011A9E RID: 72350 RVA: 0x0051390A File Offset: 0x00511B0A
		public virtual TaggedString DescriptionLabel
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17002DF1 RID: 11761
		// (get) Token: 0x06011A9F RID: 72351 RVA: 0x0051390A File Offset: 0x00511B0A
		public virtual TaggedString ExtraExplanationLabel
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17002DF2 RID: 11762
		// (get) Token: 0x06011AA0 RID: 72352 RVA: 0x00513916 File Offset: 0x00511B16
		public virtual TaggedString ExpectedQualityLabel
		{
			get
			{
				return "ExpectedLordJobQuality".Translate();
			}
		}

		// Token: 0x17002DF3 RID: 11763
		// (get) Token: 0x06011AA1 RID: 72353 RVA: 0x00513922 File Offset: 0x00511B22
		public virtual TaggedString OkButtonLabel
		{
			get
			{
				return "OK".Translate();
			}
		}

		// Token: 0x17002DF4 RID: 11764
		// (get) Token: 0x06011AA2 RID: 72354 RVA: 0x0051392E File Offset: 0x00511B2E
		public virtual TaggedString CancelButtonLabel
		{
			get
			{
				return "CancelButton".Translate();
			}
		}

		// Token: 0x17002DF5 RID: 11765
		// (get) Token: 0x06011AA3 RID: 72355 RVA: 0x0051393A File Offset: 0x00511B3A
		public virtual TaggedString QualityFactorsLabel
		{
			get
			{
				return "QualityFactors".Translate();
			}
		}

		// Token: 0x17002DF6 RID: 11766
		// (get) Token: 0x06011AA4 RID: 72356 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual Texture2D Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06011AA5 RID: 72357 RVA: 0x0028A2AB File Offset: 0x002884AB
		public virtual TaggedString ExpectedDurationLabel(FloatRange qualityRange)
		{
			return TaggedString.Empty;
		}

		// Token: 0x06011AA6 RID: 72358 RVA: 0x00513946 File Offset: 0x00511B46
		public virtual string OutcomeChancesLabel(string qualityNumber)
		{
			return "LordJobOutcomeChances".Translate(qualityNumber);
		}

		// Token: 0x06011AA7 RID: 72359 RVA: 0x0051395D File Offset: 0x00511B5D
		public virtual string OutcomeToolTip(ILordJobOutcomePossibility possibility)
		{
			return possibility.ToolTip;
		}

		// Token: 0x06011AA8 RID: 72360 RVA: 0x000440FD File Offset: 0x000422FD
		public virtual Color QualitySummaryColor(FloatRange qualityRange)
		{
			return Color.white;
		}

		// Token: 0x06011AA9 RID: 72361 RVA: 0x0051396A File Offset: 0x00511B6A
		protected virtual void Start()
		{
			this.Close(true);
		}

		// Token: 0x06011AAA RID: 72362 RVA: 0x0051396A File Offset: 0x00511B6A
		protected virtual void Cancel()
		{
			this.Close(true);
		}

		// Token: 0x06011AAB RID: 72363 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DoExtraHeaderInfo(ref RectDivider layout, ref RectDivider headerLabelRow)
		{
		}

		// Token: 0x06011AAC RID: 72364 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DrawExtraOutcomeDescriptions(Rect viewRect, FloatRange qualityRange, string qualityNumber, ref float curY, ref float totalInfoHeight)
		{
		}

		// Token: 0x06011AAD RID: 72365 RVA: 0x00513973 File Offset: 0x00511B73
		public Dialog_BeginLordJob(IPawnRoleSelectionWidget participantsDrawer) : base(null)
		{
			if (!ModLister.CheckAnyExpansion("Ritual"))
			{
				return;
			}
			this.participantsDrawer = participantsDrawer;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			this.forcePause = true;
		}

		// Token: 0x06011AAE RID: 72366 RVA: 0x005139B0 File Offset: 0x00511BB0
		public override void DoWindowContents(Rect inRect)
		{
			using (TextBlock.Default())
			{
				RectDivider rectDivider = new RectDivider(inRect, 798775645, null);
				rectDivider.NewRow(0f, VerticalJustification.Bottom, new float?((float)1));
				this.DoHeader(ref rectDivider);
				rectDivider.NewRow(0f, VerticalJustification.Top, null);
				this.DoDescription(ref rectDivider);
				this.DoButtonRow(ref rectDivider, this.CanBegin);
				rectDivider.NewRow(0f, VerticalJustification.Top, new float?(20f));
				rectDivider.NewCol(20f, HorizontalJustification.Left, new float?(0f));
				RectDivider rectDivider2 = rectDivider.NewCol(320f, HorizontalJustification.Left, new float?(24f));
				RectDivider rectDivider3 = rectDivider;
				this.DoLeftColumn(ref rectDivider2);
				this.DoRightColumn(ref rectDivider3);
			}
		}

		// Token: 0x06011AAF RID: 72367 RVA: 0x00513AA0 File Offset: 0x00511CA0
		public virtual void DoHeader(ref RectDivider layout)
		{
			RectDivider rectDivider;
			using (new TextBlock(GameFont.Medium))
			{
				Vector2 vector = Text.CalcSize(this.HeaderLabel);
				rectDivider = layout.NewRow(vector.y, VerticalJustification.Top, null);
				if (this.Icon != null)
				{
					Widgets.DrawTextureFitted(rectDivider.NewCol(vector.y, HorizontalJustification.Left, new float?(5f)), this.Icon, 1f, 1f);
				}
				Widgets.Label(rectDivider.NewCol(vector.x, HorizontalJustification.Left, null), this.HeaderLabel);
			}
			this.DoExtraHeaderInfo(ref layout, ref rectDivider);
		}

		// Token: 0x06011AB0 RID: 72368 RVA: 0x00513B70 File Offset: 0x00511D70
		public virtual void DoDescription(ref RectDivider layout)
		{
			using (new ProfilerBlock("DoDescription"))
			{
				string text = this.DescriptionLabel;
				if (!text.NullOrEmpty())
				{
					float num = Text.CalcHeight(text, layout.Rect.width - 30f);
					RectDivider rect = layout.NewRow(num + 17f, VerticalJustification.Top, new float?(10f));
					rect.NewRow(0f, VerticalJustification.Top, new float?(10f));
					rect.NewCol(10f, HorizontalJustification.Left, new float?(0f));
					rect.NewCol(20f, HorizontalJustification.Right, new float?(0f));
					Widgets.Label(rect, text);
				}
				string text2 = this.ExtraExplanationLabel;
				if (!text2.NullOrEmpty() || !text2.NullOrEmpty())
				{
					float num2 = Text.CalcHeight(text2, layout.Rect.width - 30f);
					RectDivider rect2 = layout.NewRow(num2 + 17f, VerticalJustification.Top, new float?(0f));
					rect2.NewRow(0f, VerticalJustification.Top, new float?(10f));
					rect2.NewCol(10f, HorizontalJustification.Left, new float?(0f));
					rect2.NewCol(20f, HorizontalJustification.Right, new float?(0f));
					Widgets.Label(rect2, text2);
				}
			}
		}

		// Token: 0x06011AB1 RID: 72369 RVA: 0x00513D04 File Offset: 0x00511F04
		public virtual void DoButtonRow(ref RectDivider layout, bool canBegin)
		{
			RectDivider rectDivider = layout.NewRow(this.ButtonSize.y, VerticalJustification.Bottom, new float?(0f));
			RectDivider rect = rectDivider.NewCol(this.ButtonSize.x, HorizontalJustification.Right, new float?(10f));
			RectDivider rect2 = rectDivider.NewCol(this.ButtonSize.x, HorizontalJustification.Left, null);
			RectDivider rect3 = rectDivider.NewCol(rectDivider.Rect.width, HorizontalJustification.Right, null);
			using (new TextBlock(canBegin ? Color.white : Color.gray))
			{
				if (Widgets.ButtonText(rect, this.OkButtonLabel, true, true, canBegin, null))
				{
					this.Start();
				}
			}
			if (Widgets.ButtonText(rect2, this.CancelButtonLabel, true, true, true, null))
			{
				this.Cancel();
			}
			using (new TextBlock(TextAnchor.MiddleRight, ColorLibrary.RedReadable))
			{
				Widgets.Label(rect3, this.WarningText);
			}
		}

		// Token: 0x06011AB2 RID: 72370 RVA: 0x00513E54 File Offset: 0x00512054
		public virtual void DoLeftColumn(ref RectDivider layout)
		{
			layout.NewRow(10f, VerticalJustification.Bottom, new float?(0f));
			using (new ProfilerBlock("DrawPawnList"))
			{
				this.participantsDrawer.DrawPawnList(layout);
			}
		}

		// Token: 0x06011AB3 RID: 72371 RVA: 0x00513EBC File Offset: 0x005120BC
		public virtual void DoRightColumn(ref RectDivider layout)
		{
			this.DrawQualityFactors(layout.NewCol(402f, HorizontalJustification.Left, null));
		}

		// Token: 0x06011AB4 RID: 72372 RVA: 0x00513EEC File Offset: 0x005120EC
		private float DrawQualityFactor(Rect viewRect, bool even, QualityFactor qualityFactor, float y)
		{
			if (qualityFactor == null)
			{
				return 0f;
			}
			Rect rect = new Rect(viewRect.x, y, viewRect.width, 25f);
			Rect rect2 = new Rect
			{
				x = viewRect.x,
				width = viewRect.width + 10f,
				y = y - 3f,
				height = 28f
			};
			if (even)
			{
				Widgets.DrawLightHighlight(rect2);
			}
			GUI.color = (qualityFactor.uncertainOutcome ? ColorLibrary.Yellow : (qualityFactor.positive ? ColorLibrary.Green : ColorLibrary.RedReadable));
			Rect rect3 = rect;
			rect3.width = 205f;
			Widgets.LabelEllipses(rect3, "  " + qualityFactor.label);
			using (new TextBlock(TextAnchor.UpperRight))
			{
				Widgets.Label(rect, qualityFactor.qualityChange);
			}
			if (!qualityFactor.noMiddleColumnInfo)
			{
				if (!qualityFactor.count.NullOrEmpty())
				{
					float x = Text.CalcSize(qualityFactor.count).x;
					Rect rect4 = new Rect(rect);
					rect4.xMin += 220f - x / 2f;
					rect4.width = x;
					Widgets.Label(rect4, qualityFactor.count);
				}
				else
				{
					GUI.color = Color.white;
					Texture2D image;
					if (qualityFactor.uncertainOutcome)
					{
						image = Dialog_BeginLordJob.questionMark;
					}
					else if (qualityFactor.present)
					{
						image = Dialog_BeginLordJob.QualityOffsetCheckOn;
					}
					else
					{
						image = Dialog_BeginLordJob.QualityOffsetCheckOff;
					}
					Rect rect5 = new Rect(rect);
					rect5.x += 208f;
					rect5.y -= 1f;
					rect5.width = 24f;
					rect5.height = 24f;
					if (!qualityFactor.present)
					{
						if (qualityFactor.uncertainOutcome)
						{
							TooltipHandler.TipRegion(rect5, () => "QualityFactorTooltipUncertain".Translate(), 238934347);
						}
						else
						{
							TooltipHandler.TipRegion(rect5, () => "QualityFactorTooltipNotFulfilled".Translate(), 238934347);
						}
					}
					GUI.DrawTexture(rect5, image);
				}
			}
			GUI.color = Color.white;
			if (qualityFactor.toolTip != null && Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect2);
				TooltipHandler.TipRegion(rect, () => qualityFactor.toolTip, 976091152);
			}
			return 28f;
		}

		// Token: 0x06011AB5 RID: 72373 RVA: 0x005141D4 File Offset: 0x005123D4
		public static string QualityNumberToString(FloatRange qualityRange)
		{
			if (qualityRange.min == qualityRange.max)
			{
				return qualityRange.min.ToStringPercent("F0");
			}
			return qualityRange.min.ToStringPercent("F0") + "-" + qualityRange.max.ToStringPercent("F0");
		}

		// Token: 0x06011AB6 RID: 72374 RVA: 0x0051422A File Offset: 0x0051242A
		protected virtual List<QualityFactor> PopulateQualityFactors(out FloatRange qualityRange)
		{
			this.tmpExpectedOutcomeEffects.Clear();
			qualityRange = new FloatRange(0f, 0f);
			return this.tmpExpectedOutcomeEffects;
		}

		// Token: 0x06011AB7 RID: 72375 RVA: 0x00514252 File Offset: 0x00512452
		protected virtual List<ILordJobOutcomePossibility> PopulateOutcomePossibilities()
		{
			Dialog_BeginLordJob.tmpOutcomes.Clear();
			return Dialog_BeginLordJob.tmpOutcomes;
		}

		// Token: 0x06011AB8 RID: 72376 RVA: 0x00514264 File Offset: 0x00512464
		public virtual void DrawOutcomeChances(Rect viewRect, FloatRange qualityRange, string qualityNumber, ref float curY, ref float totalInfoHeight)
		{
			List<ILordJobOutcomePossibility> list = this.PopulateOutcomePossibilities();
			if (list.NullOrEmpty<ILordJobOutcomePossibility>())
			{
				return;
			}
			Widgets.Label(new Rect(viewRect.x, curY, viewRect.width, 32f), this.OutcomeChancesLabel(qualityNumber) + ": ");
			curY += 28f;
			totalInfoHeight += 28f;
			float num = 0f;
			foreach (ILordJobOutcomePossibility lordJobOutcomePossibility in list)
			{
				num += lordJobOutcomePossibility.Weight(qualityRange);
			}
			foreach (ILordJobOutcomePossibility lordJobOutcomePossibility2 in list)
			{
				float f = lordJobOutcomePossibility2.Weight(qualityRange) / num;
				TaggedString taggedString = "  - " + lordJobOutcomePossibility2.Label + ": " + f.ToStringPercent();
				Rect rect = new Rect(viewRect.x, curY, Text.CalcSize(taggedString).x + 4f, 32f);
				Rect rect2 = new Rect(rect)
				{
					width = rect.width + 8f,
					height = 22f
				};
				if (Mouse.IsOver(rect2))
				{
					string desc = this.OutcomeToolTip(lordJobOutcomePossibility2);
					Widgets.DrawLightHighlight(rect2);
					if (!desc.NullOrEmpty())
					{
						TooltipHandler.TipRegion(rect2, () => desc, 231134347);
					}
				}
				Widgets.Label(rect, taggedString);
				curY += Text.LineHeight;
				totalInfoHeight += Text.LineHeight;
			}
		}

		// Token: 0x06011AB9 RID: 72377 RVA: 0x00514468 File Offset: 0x00512668
		protected virtual void DrawQualityDescription(Rect outRectQualityDesc, FloatRange qualityRange, string qualityNumber, float totalInfoHeight)
		{
			Rect viewRect = new Rect(0f, 0f, outRectQualityDesc.width, this.qualityDescHeight);
			bool flag = this.qualityDescHeight > outRectQualityDesc.height;
			if (flag)
			{
				viewRect.width -= 16f;
			}
			float num = 0f;
			Widgets.BeginScrollView(outRectQualityDesc, ref this.scrollPositionQualityDesc, viewRect, flag);
			TaggedString taggedString = this.ExpectedDurationLabel(qualityRange);
			if (!taggedString.NullOrEmpty())
			{
				float num2 = Text.CalcHeight(taggedString, viewRect.width);
				Widgets.Label(new Rect(viewRect.x, num - 4f, viewRect.width, num2), taggedString);
				num += 17f + (num2 - 22f);
				totalInfoHeight += 17f + (num2 - 22f);
			}
			this.DrawOutcomeChances(viewRect, qualityRange, qualityNumber, ref num, ref totalInfoHeight);
			this.DrawExtraOutcomeDescriptions(viewRect, qualityRange, qualityNumber, ref num, ref totalInfoHeight);
			GUI.color = Color.white;
			this.qualityDescHeight = num;
			Widgets.EndScrollView();
		}

		// Token: 0x06011ABA RID: 72378 RVA: 0x0051456C File Offset: 0x0051276C
		protected virtual void DrawQualityFactors(Rect viewRect)
		{
			float num = viewRect.y;
			float num2 = 0f;
			bool flag = true;
			FloatRange qualityRange;
			List<QualityFactor> list = this.PopulateQualityFactors(out qualityRange);
			if (list.NullOrEmpty<QualityFactor>())
			{
				return;
			}
			Widgets.Label(new Rect(viewRect.x, num + 3f, viewRect.width, 32f), this.QualityFactorsLabel);
			num += 32f;
			num2 += 32f;
			foreach (QualityFactor qualityFactor in from e in list
			orderby e.priority descending
			select e)
			{
				float num3 = this.DrawQualityFactor(viewRect, flag, qualityFactor, num);
				num += num3;
				num2 += num3;
				flag = !flag;
			}
			num += 2f;
			string text = Dialog_BeginLordJob.QualityNumberToString(qualityRange);
			using (new TextBlock(this.QualitySummaryColor(qualityRange)))
			{
				Widgets.Label(new Rect(viewRect.x, num + 4f, viewRect.width, 25f), this.ExpectedQualityLabel + ":");
				using (new TextBlock(GameFont.Medium))
				{
					float x = Text.CalcSize(text).x;
					Widgets.Label(new Rect(viewRect.xMax - x, num - 2f, viewRect.width, 32f), text);
				}
				num += 28f;
				num2 += 28f;
			}
			Rect rect = viewRect;
			rect.width += 10f;
			rect.height = num2;
			rect = rect.ExpandedBy(9f);
			using (new TextBlock(new Color(0.25f, 0.25f, 0.25f)))
			{
				Widgets.DrawBox(rect, 2, null);
			}
			num += 10f;
			num2 += 10f;
			Rect outRectQualityDesc = new Rect(viewRect.x, num, viewRect.width, viewRect.height - num2);
			this.DrawQualityDescription(outRectQualityDesc, qualityRange, text, num2);
		}

		// Token: 0x06011ABB RID: 72379 RVA: 0x005147D4 File Offset: 0x005129D4
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			this.participantsDrawer.WindowUpdate();
		}

		// Token: 0x06011ABC RID: 72380 RVA: 0x005147E7 File Offset: 0x005129E7
		public override void OnAcceptKeyPressed()
		{
			this.Start();
		}

		// Token: 0x0400B5F7 RID: 46583
		protected IPawnRoleSelectionWidget participantsDrawer;

		// Token: 0x0400B5F8 RID: 46584
		private Vector2 scrollPositionQualityDesc;

		// Token: 0x0400B5F9 RID: 46585
		private float qualityDescHeight;

		// Token: 0x0400B5FA RID: 46586
		private static readonly Texture2D questionMark = ContentFinder<Texture2D>.Get("UI/Overlays/QuestionMark", true);

		// Token: 0x0400B5FB RID: 46587
		private static readonly Texture2D QualityOffsetCheckOn = Resources.Load<Texture2D>("Textures/UI/Widgets/RitualQualityCheck_On");

		// Token: 0x0400B5FC RID: 46588
		private static readonly Texture2D QualityOffsetCheckOff = Resources.Load<Texture2D>("Textures/UI/Widgets/RitualQualityCheck_Off");

		// Token: 0x0400B5FD RID: 46589
		protected const float CategoryCaptionHeight = 32f;

		// Token: 0x0400B5FE RID: 46590
		protected const float EntryHeight = 28f;

		// Token: 0x0400B5FF RID: 46591
		protected const float ListWidth = 320f;

		// Token: 0x0400B600 RID: 46592
		protected const float QualityOffsetListWidth = 402f;

		// Token: 0x0400B601 RID: 46593
		private const int ContextHash = 798775645;

		// Token: 0x0400B602 RID: 46594
		private List<QualityFactor> tmpExpectedOutcomeEffects = new List<QualityFactor>();

		// Token: 0x0400B603 RID: 46595
		private static List<ILordJobOutcomePossibility> tmpOutcomes = new List<ILordJobOutcomePossibility>();
	}
}
