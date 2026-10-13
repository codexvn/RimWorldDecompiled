using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003117 RID: 12567
	public class Dialog_AutoSlaughter : Window
	{
		// Token: 0x17002DE8 RID: 11752
		// (get) Token: 0x06011A7F RID: 72319 RVA: 0x005123BC File Offset: 0x005105BC
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1050f, 600f);
			}
		}

		// Token: 0x06011A80 RID: 72320 RVA: 0x005123D0 File Offset: 0x005105D0
		public Dialog_AutoSlaughter(Map map) : base(null)
		{
			this.map = map;
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06011A81 RID: 72321 RVA: 0x0051243A File Offset: 0x0051063A
		public override void PostOpen()
		{
			base.PostOpen();
			this.RecalculateAnimals();
		}

		// Token: 0x06011A82 RID: 72322 RVA: 0x00512448 File Offset: 0x00510648
		private void RecalculateAnimals()
		{
			this.animalCounts.Clear();
			foreach (AutoSlaughterConfig autoSlaughterConfig in this.map.autoSlaughterManager.configs)
			{
				Dialog_AutoSlaughter.AnimalCountRecord value = default(Dialog_AutoSlaughter.AnimalCountRecord);
				this.CountPlayerAnimals(this.map, autoSlaughterConfig, autoSlaughterConfig.animal, out value.male, out value.maleYoung, out value.female, out value.femaleYoung, out value.total, out value.pregnant, out value.bonded);
				this.animalCounts.Add(autoSlaughterConfig.animal, value);
			}
			this.configsOrdered = (from c in this.map.autoSlaughterManager.configs
			orderby this.animalCounts[c.animal].total descending, c.animal.label
			select c).ToList<AutoSlaughterConfig>();
		}

		// Token: 0x06011A83 RID: 72323 RVA: 0x00512558 File Offset: 0x00510758
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(inRect);
			rect.yMax -= Window.CloseButSize.y;
			rect.yMin += 8f;
			Listing_Standard listing_Standard = new Listing_Standard(rect, () => this.scrollPos);
			listing_Standard.ColumnWidth = inRect.width - 16f - 4f;
			this.viewRect = new Rect(0f, 0f, rect.width - 16f, 30f * (float)(this.configsOrdered.Count + 1));
			Rect other = rect;
			other.x = this.scrollPos.x;
			other.y = this.scrollPos.y;
			Widgets.BeginScrollView(rect, ref this.scrollPos, this.viewRect, true);
			listing_Standard.Begin(this.viewRect);
			this.DoAnimalHeader(listing_Standard.GetRect(24f, 1f), listing_Standard.GetRect(24f, 1f));
			listing_Standard.Gap(6f);
			int num = 0;
			foreach (AutoSlaughterConfig config in this.configsOrdered)
			{
				Rect rect2 = listing_Standard.GetRect(24f, 1f);
				if (rect2.Overlaps(other))
				{
					this.DoAnimalRow(rect2, config, num);
				}
				listing_Standard.Gap(6f);
				num++;
			}
			listing_Standard.End();
			Widgets.EndScrollView();
			Rect rect3 = new Rect(inRect.x + inRect.width / 2f + Window.CloseButSize.x / 2f + 10f, inRect.y + inRect.height - 35f, 395f, 50f);
			rect3.yMax = inRect.yMax;
			rect3.xMax = inRect.xMax;
			Color color = GUI.color;
			GameFont font = Text.Font;
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = Color.gray;
			Text.Font = GameFont.Tiny;
			if (Text.TinyFontSupported)
			{
				Widgets.Label(rect3, "AutoSlaugtherTip".Translate());
			}
			else
			{
				Widgets.Label(rect3, "AutoSlaugtherTip".Translate().Truncate(rect3.width, null));
				TooltipHandler.TipRegion(rect3, "AutoSlaugtherTip".Translate());
			}
			Text.Font = font;
			Text.Anchor = anchor;
			GUI.color = color;
		}

		// Token: 0x06011A84 RID: 72324 RVA: 0x005127EC File Offset: 0x005109EC
		private void CountPlayerAnimals(Map map, AutoSlaughterConfig config, ThingDef animal, out int currentMales, out int currentMalesYoung, out int currentFemales, out int currentFemalesYoung, out int currentTotal, out int currentPregnant, out int currentBonded)
		{
			currentMales = (currentMalesYoung = (currentFemales = (currentFemalesYoung = (currentTotal = (currentPregnant = (currentBonded = 0))))));
			foreach (Pawn pawn in map.mapPawns.SpawnedColonyAnimals)
			{
				if (pawn.def == animal && AutoSlaughterManager.CanEverAutoSlaughter(pawn))
				{
					if (pawn.relations.GetDirectRelationsCount(PawnRelationDefOf.Bond, null) > 0)
					{
						currentBonded++;
						if (!config.allowSlaughterBonded)
						{
							continue;
						}
					}
					if (pawn.gender == Gender.Male)
					{
						if (pawn.ageTracker.CurLifeStage.reproductive)
						{
							currentMales++;
						}
						else
						{
							currentMalesYoung++;
						}
					}
					else if (pawn.gender == Gender.Female)
					{
						if (pawn.ageTracker.CurLifeStage.reproductive)
						{
							Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant, false);
							if (firstHediffOfDef != null && firstHediffOfDef.Visible)
							{
								currentPregnant++;
								if (!config.allowSlaughterPregnant)
								{
									continue;
								}
								currentFemales++;
							}
							else
							{
								currentFemales++;
							}
						}
						else
						{
							currentFemalesYoung++;
						}
					}
					currentTotal++;
				}
			}
		}

		// Token: 0x06011A85 RID: 72325 RVA: 0x0051294C File Offset: 0x00510B4C
		private float CalculateLabelWidth(Rect rect)
		{
			float num = 64f;
			return rect.width - 24f - 4f - 4f - num * 7f - 420f - 32f;
		}

		// Token: 0x06011A86 RID: 72326 RVA: 0x0051298C File Offset: 0x00510B8C
		private void DoMaxColumn(WidgetRow row, ref int val, ref string buffer, int current)
		{
			int num = val;
			if (val == -1)
			{
				float num2 = 68f;
				float width = (60f - num2) / 2f;
				row.Gap(width);
				if (row.ButtonIconWithBG(TexButton.Infinity, 48f, "AutoSlaughterTooltipSetLimit".Translate(), true))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					val = current;
				}
				row.Gap(width);
			}
			else
			{
				row.CellGap = 0f;
				row.Gap(-4f);
				row.TextFieldNumeric<int>(ref val, ref buffer, 40f);
				val = Mathf.Max(0, val);
				if (row.ButtonIcon(TexButton.CloseXSmall, null, new Color?(Color.white), null, null, true, 16f))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					val = -1;
					buffer = null;
				}
				row.CellGap = 4f;
				row.Gap(4f);
			}
			if (num != val)
			{
				this.map.autoSlaughterManager.Notify_ConfigChanged();
			}
		}

		// Token: 0x06011A87 RID: 72327 RVA: 0x00512A94 File Offset: 0x00510C94
		private void DoAnimalHeader(Rect rect1, Rect rect2)
		{
			Dialog_AutoSlaughter.<>c__DisplayClass24_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.rect1 = rect1;
			CS$<>8__locals1.rect2 = rect2;
			float width = this.CalculateLabelWidth(CS$<>8__locals1.rect1);
			Widgets.BeginGroup(new Rect(CS$<>8__locals1.rect1.x, CS$<>8__locals1.rect1.y, CS$<>8__locals1.rect1.width, CS$<>8__locals1.rect1.height + CS$<>8__locals1.rect2.height + 1f));
			int num = 0;
			foreach (Rect rect3 in this.tmpGroupRects)
			{
				if (num % 2 == 1)
				{
					Widgets.DrawLightHighlight(rect3);
					Widgets.DrawLightHighlight(rect3);
				}
				else
				{
					Widgets.DrawLightHighlight(rect3);
				}
				GUI.color = Color.gray;
				if (num > 0)
				{
					Widgets.DrawLineVertical(rect3.xMin, 0f, CS$<>8__locals1.rect1.height + CS$<>8__locals1.rect2.height + 1f);
				}
				if (num < this.tmpGroupRects.Count - 1)
				{
					Widgets.DrawLineVertical(rect3.xMax, 0f, CS$<>8__locals1.rect1.height + CS$<>8__locals1.rect2.height + 1f);
				}
				GUI.color = Color.white;
				num++;
			}
			foreach (Rect rect4 in this.tmpMouseoverHighlightRects)
			{
				Widgets.DrawHighlightIfMouseover(rect4);
			}
			Widgets.EndGroup();
			this.tmpMouseoverHighlightRects.Clear();
			this.tmpGroupRects.Clear();
			Widgets.BeginGroup(CS$<>8__locals1.rect1);
			Dialog_AutoSlaughter.<>c__DisplayClass24_1 CS$<>8__locals2;
			CS$<>8__locals2.row = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleCenter;
			CS$<>8__locals2.row.Label(string.Empty, 24f, null, -1f);
			CS$<>8__locals2.startX = CS$<>8__locals2.row.FinalX;
			CS$<>8__locals2.row.Label(string.Empty, width, "AutoSlaugtherHeaderTooltipLabel".Translate(), -1f);
			Rect item = new Rect(CS$<>8__locals2.startX, CS$<>8__locals1.rect1.height, CS$<>8__locals2.row.FinalX - CS$<>8__locals2.startX, CS$<>8__locals1.rect2.height);
			this.tmpMouseoverHighlightRects.Add(item);
			this.tmpGroupRects.Add(item);
			this.<DoAnimalHeader>g__AddCurrentAndMaxEntries|24_0("AutoSlaugtherHeaderColTotal", 0f, 0f, ref CS$<>8__locals1, ref CS$<>8__locals2);
			this.<DoAnimalHeader>g__AddCurrentAndMaxEntries|24_0("AnimalMaleAdult", 0f, 0f, ref CS$<>8__locals1, ref CS$<>8__locals2);
			this.<DoAnimalHeader>g__AddCurrentAndMaxEntries|24_0("AnimalMaleYoung", 0f, 0f, ref CS$<>8__locals1, ref CS$<>8__locals2);
			this.<DoAnimalHeader>g__AddCurrentAndMaxEntries|24_0("AnimalFemaleAdult", 0f, 0f, ref CS$<>8__locals1, ref CS$<>8__locals2);
			this.<DoAnimalHeader>g__AddCurrentAndMaxEntries|24_0("AnimalFemaleYoung", 0f, 0f, ref CS$<>8__locals1, ref CS$<>8__locals2);
			this.<DoAnimalHeader>g__AddCurrentAndMaxEntries|24_0("AnimalPregnant", 0f, 16f, ref CS$<>8__locals1, ref CS$<>8__locals2);
			this.<DoAnimalHeader>g__AddCurrentAndMaxEntries|24_0("AnimalBonded", 0f, 16f, ref CS$<>8__locals1, ref CS$<>8__locals2);
			Text.Anchor = anchor;
			Widgets.EndGroup();
			Widgets.BeginGroup(CS$<>8__locals1.rect2);
			WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			TextAnchor anchor2 = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleCenter;
			widgetRow.Label(string.Empty, 24f, null, -1f);
			widgetRow.Label("AutoSlaugtherHeaderColLabel".Translate(), width, "AutoSlaugtherHeaderTooltipLabel".Translate(), -1f);
			widgetRow.Label("AutoSlaugtherHeaderColCurrent".Translate(), 60f, "AutoSlaugtherHeaderTooltipCurrentTotal".Translate(), -1f);
			widgetRow.Label("AutoSlaugtherHeaderColMax".Translate(), 56f, "AutoSlaugtherHeaderTooltipMaxTotal".Translate(), -1f);
			widgetRow.Label("AutoSlaugtherHeaderColCurrent".Translate(), 60f, "AutoSlaugtherHeaderTooltipCurrentMales".Translate(), -1f);
			widgetRow.Label("AutoSlaugtherHeaderColMax".Translate(), 56f, "AutoSlaugtherHeaderTooltipMaxMales".Translate(), -1f);
			widgetRow.Label("AutoSlaugtherHeaderColCurrent".Translate(), 60f, "AutoSlaughterHeaderTooltipCurrentMalesYoung".Translate(), -1f);
			widgetRow.Label("AutoSlaugtherHeaderColMax".Translate(), 56f, "AutoSlaughterHeaderTooltipMaxMalesYoung".Translate(), -1f);
			widgetRow.Label("AutoSlaugtherHeaderColCurrent".Translate(), 60f, "AutoSlaugtherHeaderTooltipCurrentFemales".Translate(), -1f);
			widgetRow.Label("AutoSlaugtherHeaderColMax".Translate(), 56f, "AutoSlaugtherHeaderTooltipMaxFemales".Translate(), -1f);
			widgetRow.Label("AutoSlaugtherHeaderColCurrent".Translate(), 60f, "AutoSlaugtherHeaderTooltipCurrentFemalesYoung".Translate(), -1f);
			widgetRow.Label("AutoSlaugtherHeaderColMax".Translate(), 56f, "AutoSlaughterHeaderTooltipMaxFemalesYoung".Translate(), -1f);
			widgetRow.Label("AutoSlaugtherHeaderColCurrent".Translate(), 60f, "AutoSlaughterHeaderTooltipCurrentPregnant".Translate(), -1f);
			widgetRow.Label("AllowSlaughter".Translate(), 72f, "AutoSlaughterHeaderTooltipAllowSlaughterPregnant".Translate(), -1f);
			widgetRow.Label("AutoSlaugtherHeaderColCurrent".Translate(), 60f, "AutoSlaughterHeaderTooltipCurrentBonded".Translate(), -1f);
			widgetRow.Label("AllowSlaughter".Translate(), 72f, "AutoSlaughterHeaderTooltipAllowSlaughterBonded".Translate(), -1f);
			Text.Anchor = anchor2;
			Widgets.EndGroup();
			GUI.color = Color.gray;
			Widgets.DrawLineHorizontal(CS$<>8__locals1.rect2.x, CS$<>8__locals1.rect2.y + CS$<>8__locals1.rect2.height + 1f, CS$<>8__locals1.rect2.width);
			GUI.color = Color.white;
		}

		// Token: 0x06011A88 RID: 72328 RVA: 0x0051315C File Offset: 0x0051135C
		private void DoAnimalRow(Rect rect, AutoSlaughterConfig config, int index)
		{
			Dialog_AutoSlaughter.<>c__DisplayClass25_0 CS$<>8__locals1 = new Dialog_AutoSlaughter.<>c__DisplayClass25_0();
			CS$<>8__locals1.config = config;
			CS$<>8__locals1.<>4__this = this;
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Color color = GUI.color;
			Dialog_AutoSlaughter.AnimalCountRecord animalCountRecord = this.animalCounts[CS$<>8__locals1.config.animal];
			float width = this.CalculateLabelWidth(rect);
			Widgets.BeginGroup(rect);
			Dialog_AutoSlaughter.<>c__DisplayClass25_1 CS$<>8__locals2;
			CS$<>8__locals2.row = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			CS$<>8__locals2.row.DefIcon(CS$<>8__locals1.config.animal, null);
			CS$<>8__locals2.row.Gap(4f);
			GUI.color = ((animalCountRecord.total == 0) ? Color.gray : color);
			CS$<>8__locals2.row.Label(CS$<>8__locals1.config.animal.LabelCap.Truncate(width, null), width, CS$<>8__locals1.<DoAnimalRow>g__GetTipForAnimal|1(), -1f);
			GUI.color = color;
			Dialog_AutoSlaughter.<DoAnimalRow>g__DrawCurrentCol|25_0(animalCountRecord.total, new int?(CS$<>8__locals1.config.maxTotal), ref CS$<>8__locals2);
			this.DoMaxColumn(CS$<>8__locals2.row, ref CS$<>8__locals1.config.maxTotal, ref CS$<>8__locals1.config.uiMaxTotalBuffer, animalCountRecord.total);
			Dialog_AutoSlaughter.<DoAnimalRow>g__DrawCurrentCol|25_0(animalCountRecord.male, new int?(CS$<>8__locals1.config.maxMales), ref CS$<>8__locals2);
			this.DoMaxColumn(CS$<>8__locals2.row, ref CS$<>8__locals1.config.maxMales, ref CS$<>8__locals1.config.uiMaxMalesBuffer, animalCountRecord.male);
			Dialog_AutoSlaughter.<DoAnimalRow>g__DrawCurrentCol|25_0(animalCountRecord.maleYoung, new int?(CS$<>8__locals1.config.maxMalesYoung), ref CS$<>8__locals2);
			this.DoMaxColumn(CS$<>8__locals2.row, ref CS$<>8__locals1.config.maxMalesYoung, ref CS$<>8__locals1.config.uiMaxMalesYoungBuffer, animalCountRecord.maleYoung);
			Dialog_AutoSlaughter.<DoAnimalRow>g__DrawCurrentCol|25_0(animalCountRecord.female, new int?(CS$<>8__locals1.config.maxFemales), ref CS$<>8__locals2);
			this.DoMaxColumn(CS$<>8__locals2.row, ref CS$<>8__locals1.config.maxFemales, ref CS$<>8__locals1.config.uiMaxFemalesBuffer, animalCountRecord.female);
			Dialog_AutoSlaughter.<DoAnimalRow>g__DrawCurrentCol|25_0(animalCountRecord.femaleYoung, new int?(CS$<>8__locals1.config.maxFemalesYoung), ref CS$<>8__locals2);
			this.DoMaxColumn(CS$<>8__locals2.row, ref CS$<>8__locals1.config.maxFemalesYoung, ref CS$<>8__locals1.config.uiMaxFemalesYoungBuffer, animalCountRecord.femaleYoung);
			Text.Anchor = TextAnchor.MiddleCenter;
			CS$<>8__locals2.row.Label(animalCountRecord.pregnant.ToString(), 60f, null, -1f);
			Text.Anchor = TextAnchor.UpperLeft;
			bool allowSlaughterPregnant = CS$<>8__locals1.config.allowSlaughterPregnant;
			CS$<>8__locals2.row.Gap(26f);
			Widgets.Checkbox(CS$<>8__locals2.row.FinalX, 0f, ref CS$<>8__locals1.config.allowSlaughterPregnant, 24f, false, true, null, null);
			if (allowSlaughterPregnant != CS$<>8__locals1.config.allowSlaughterPregnant)
			{
				this.RecalculateAnimals();
			}
			CS$<>8__locals2.row.Gap(52f);
			Text.Anchor = TextAnchor.MiddleCenter;
			CS$<>8__locals2.row.Label(animalCountRecord.bonded.ToString(), 60f, null, -1f);
			Text.Anchor = TextAnchor.UpperLeft;
			CS$<>8__locals2.row.Gap(24f);
			bool allowSlaughterBonded = CS$<>8__locals1.config.allowSlaughterBonded;
			Widgets.Checkbox(CS$<>8__locals2.row.FinalX, 0f, ref CS$<>8__locals1.config.allowSlaughterBonded, 24f, false, true, null, null);
			if (allowSlaughterBonded != CS$<>8__locals1.config.allowSlaughterBonded)
			{
				this.RecalculateAnimals();
			}
			Widgets.EndGroup();
		}

		// Token: 0x06011A8B RID: 72331 RVA: 0x005134F8 File Offset: 0x005116F8
		[CompilerGenerated]
		private void <DoAnimalHeader>g__AddCurrentAndMaxEntries|24_0(string headerKey, float extraWidthFirst = 0f, float extraWidthSecond = 0f, ref Dialog_AutoSlaughter.<>c__DisplayClass24_0 A_4, ref Dialog_AutoSlaughter.<>c__DisplayClass24_1 A_5)
		{
			A_5.startX = A_5.row.FinalX;
			A_5.row.Label(string.Empty, 60f + extraWidthFirst, null, -1f);
			this.tmpMouseoverHighlightRects.Add(new Rect(A_5.startX, A_4.rect1.height, A_5.row.FinalX - A_5.startX, A_4.rect2.height));
			float finalX = A_5.row.FinalX;
			A_5.row.Label(string.Empty, 56f + extraWidthSecond, null, -1f);
			this.tmpMouseoverHighlightRects.Add(new Rect(finalX, A_4.rect1.height, A_5.row.FinalX - finalX, A_4.rect2.height));
			Rect rect = new Rect(A_5.startX, 0f, A_5.row.FinalX - A_5.startX, A_4.rect2.height);
			Widgets.Label(rect, headerKey.Translate());
			this.tmpGroupRects.Add(rect);
		}

		// Token: 0x06011A8C RID: 72332 RVA: 0x00513628 File Offset: 0x00511828
		[CompilerGenerated]
		internal static void <DoAnimalRow>g__DrawCurrentCol|25_0(int val, int? limit = null, ref Dialog_AutoSlaughter.<>c__DisplayClass25_1 A_2)
		{
			Color? color = null;
			if (val == 0)
			{
				color = new Color?(Color.gray);
			}
			else if (limit != null)
			{
				int? num = limit;
				int num2 = -1;
				if (!(num.GetValueOrDefault() == num2 & num != null))
				{
					int num3 = val;
					num = limit;
					if (num3 > num.GetValueOrDefault() & num != null)
					{
						color = new Color?(ColorLibrary.RedReadable);
					}
				}
			}
			Color color2 = GUI.color;
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = (color ?? Color.white);
			A_2.row.Label(val.ToString(), 60f, null, -1f);
			Text.Anchor = anchor;
			GUI.color = color2;
		}

		// Token: 0x06011A8D RID: 72333 RVA: 0x005136E8 File Offset: 0x005118E8
		[CompilerGenerated]
		internal static string <DoAnimalRow>g__DevTipPartForPawn|25_2(Pawn pawn)
		{
			string text = string.Concat(new string[]
			{
				pawn.LabelShortCap,
				" ",
				pawn.gender.GetLabel(false),
				" (",
				pawn.ageTracker.AgeBiologicalYears.ToString(),
				"y)"
			});
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Pregnant, false);
			if (firstHediffOfDef != null)
			{
				text = text + ", pregnant (" + firstHediffOfDef.Severity.ToStringPercent() + ")";
			}
			return text;
		}

		// Token: 0x0400B5D8 RID: 46552
		private Map map;

		// Token: 0x0400B5D9 RID: 46553
		private Vector2 scrollPos;

		// Token: 0x0400B5DA RID: 46554
		private Rect viewRect;

		// Token: 0x0400B5DB RID: 46555
		private Dictionary<ThingDef, Dialog_AutoSlaughter.AnimalCountRecord> animalCounts = new Dictionary<ThingDef, Dialog_AutoSlaughter.AnimalCountRecord>();

		// Token: 0x0400B5DC RID: 46556
		private List<AutoSlaughterConfig> configsOrdered = new List<AutoSlaughterConfig>();

		// Token: 0x0400B5DD RID: 46557
		private List<Rect> tmpMouseoverHighlightRects = new List<Rect>();

		// Token: 0x0400B5DE RID: 46558
		private List<Rect> tmpGroupRects = new List<Rect>();

		// Token: 0x0400B5DF RID: 46559
		private const float ColumnWidthCurrent = 60f;

		// Token: 0x0400B5E0 RID: 46560
		private const float ColumnWidthMaxNoLabelSpacing = 56f;

		// Token: 0x0400B5E1 RID: 46561
		private const float ColumnWidthMax = 60f;

		// Token: 0x0400B5E2 RID: 46562
		private const float SizeControlInfinityButton = 48f;

		// Token: 0x0400B5E3 RID: 46563
		private const float SizeControlTextArea = 40f;

		// Token: 0x0400B5E4 RID: 46564
		private const float ExtraSpacingPregnant = 16f;

		// Token: 0x0400B5E5 RID: 46565
		private const int NumColumns = 7;

		// Token: 0x02003118 RID: 12568
		private struct AnimalCountRecord
		{
			// Token: 0x06011A8E RID: 72334 RVA: 0x0051377E File Offset: 0x0051197E
			public AnimalCountRecord(int total, int male, int maleYoung, int female, int femaleYoung, int pregnant, int bonded)
			{
				this.total = total;
				this.male = male;
				this.maleYoung = maleYoung;
				this.female = female;
				this.femaleYoung = femaleYoung;
				this.pregnant = pregnant;
				this.bonded = bonded;
			}

			// Token: 0x0400B5E6 RID: 46566
			public int total;

			// Token: 0x0400B5E7 RID: 46567
			public int male;

			// Token: 0x0400B5E8 RID: 46568
			public int maleYoung;

			// Token: 0x0400B5E9 RID: 46569
			public int female;

			// Token: 0x0400B5EA RID: 46570
			public int femaleYoung;

			// Token: 0x0400B5EB RID: 46571
			public int pregnant;

			// Token: 0x0400B5EC RID: 46572
			public int bonded;
		}
	}
}
