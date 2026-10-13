using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003111 RID: 12561
	public class Dialog_AnomalySettings : Window
	{
		// Token: 0x17002DE5 RID: 11749
		// (get) Token: 0x06011A66 RID: 72294 RVA: 0x00511137 File Offset: 0x0050F337
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 475f);
			}
		}

		// Token: 0x06011A67 RID: 72295 RVA: 0x00511148 File Offset: 0x0050F348
		public Dialog_AnomalySettings(Difficulty difficulty) : base(null)
		{
			this.doCloseX = true;
			this.absorbInputAroundWindow = true;
			this.listing = new Listing_Standard();
			this.listing.maxOneColumn = true;
			this.difficulty = difficulty;
			this.anomalyThreatsInactiveFraction = difficulty.anomalyThreatsInactiveFraction;
			this.anomalyThreatsActiveFraction = difficulty.anomalyThreatsActiveFraction;
			this.overrideAnomalyThreatsFraction = difficulty.overrideAnomalyThreatsFraction.GetValueOrDefault();
			this.studyEfficiencyFactor = difficulty.studyEfficiencyFactor;
			this.anomalyPlaystyleDef = difficulty.AnomalyPlaystyleDef;
		}

		// Token: 0x06011A68 RID: 72296 RVA: 0x005111CC File Offset: 0x0050F3CC
		public override void DoWindowContents(Rect inRect)
		{
			using (new TextBlock(GameFont.Medium))
			{
				Widgets.Label(inRect, "AnomalySettings".Translate());
				inRect.yMin += Text.LineHeight + 10f;
			}
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(new Rect((inRect.x + inRect.width - Dialog_AnomalySettings.AcceptButtonSize.x) / 2f, inRect.yMax - Dialog_AnomalySettings.AcceptButtonSize.y, Dialog_AnomalySettings.AcceptButtonSize.x, Dialog_AnomalySettings.AcceptButtonSize.y), "Accept".Translate(), true, true, true, null))
			{
				if (this.anomalyPlaystyleDef.overrideThreatFraction)
				{
					this.difficulty.overrideAnomalyThreatsFraction = new float?(this.overrideAnomalyThreatsFraction);
				}
				else
				{
					this.difficulty.overrideAnomalyThreatsFraction = null;
				}
				this.difficulty.anomalyThreatsInactiveFraction = this.anomalyThreatsInactiveFraction;
				this.difficulty.anomalyThreatsActiveFraction = this.anomalyThreatsActiveFraction;
				this.difficulty.studyEfficiencyFactor = this.studyEfficiencyFactor;
				this.difficulty.AnomalyPlaystyleDef = this.anomalyPlaystyleDef;
				this.Close(true);
			}
			inRect.yMax -= Dialog_AnomalySettings.AcceptButtonSize.y + 17f;
			Rect rect = new Rect(inRect.x, inRect.yMax - Text.LineHeight, inRect.width, Text.LineHeight);
			rect.xMin += 10f;
			rect.xMax -= 10f;
			rect.height = Text.LineHeight + 5f;
			if (Widgets.ButtonText(rect, "SetToStandardPlaystyle".Translate() + "...", true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				using (IEnumerator<DifficultyDef> enumerator = DefDatabase<DifficultyDef>.AllDefs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DifficultyDef d = enumerator.Current;
						if (!d.isCustom)
						{
							list.Add(new FloatMenuOption(d.LabelCap, delegate()
							{
								this.anomalyThreatsInactiveFraction = d.anomalyThreatsInactiveFraction;
								this.anomalyThreatsActiveFraction = d.anomalyThreatsActiveFraction;
								this.studyEfficiencyFactor = d.studyEfficiencyFactor;
								this.anomalyPlaystyleDef = AnomalyPlaystyleDefOf.Standard;
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			inRect.yMax -= rect.height + 4f;
			Rect rect2 = new Rect(0f, 0f, inRect.width - 16f, this.scrollHeight);
			Widgets.BeginScrollView(inRect, ref this.scrollPosition, rect2, true);
			Rect rect3 = rect2;
			rect3.height = 99999f;
			this.listing.Begin(rect3);
			this.DrawPlaystyles();
			this.DrawExtraSettings();
			this.scrollHeight = this.listing.CurHeight;
			this.listing.End();
			Widgets.EndScrollView();
		}

		// Token: 0x06011A69 RID: 72297 RVA: 0x00511510 File Offset: 0x0050F710
		private void DrawPlaystyles()
		{
			this.listing.Label("ChooseAnomalyPlaystyle".Translate(), -1f, null);
			this.listing.Gap(12f);
			foreach (AnomalyPlaystyleDef anomalyPlaystyleDef in DefDatabase<AnomalyPlaystyleDef>.AllDefs)
			{
				bool flag = !Find.Scenario.standardAnomalyPlaystyleOnly || anomalyPlaystyleDef == AnomalyPlaystyleDefOf.Standard;
				string text = anomalyPlaystyleDef.LabelCap.AsTipTitle() + "\n" + anomalyPlaystyleDef.description;
				if (!flag)
				{
					text = text + "\n\n" + ("DisabledByScenario".Translate() + ": " + Find.Scenario.name).Colorize(ColorLibrary.RedReadable);
				}
				if (this.listing.RadioButton(anomalyPlaystyleDef.LabelCap, this.anomalyPlaystyleDef == anomalyPlaystyleDef, 30f, 80f, text, new float?(0f), !flag))
				{
					if (flag)
					{
						if (!this.anomalyPlaystyleDef.overrideThreatFraction && anomalyPlaystyleDef.overrideThreatFraction)
						{
							this.overrideAnomalyThreatsFraction = 0.15f;
						}
						this.anomalyPlaystyleDef = anomalyPlaystyleDef;
					}
					else
					{
						Messages.Message("DisabledByScenario".Translate() + ": " + Find.Scenario.name, MessageTypeDefOf.RejectInput, false);
					}
				}
				this.listing.Gap(3f);
			}
			this.listing.Gap(12f);
		}

		// Token: 0x06011A6A RID: 72298 RVA: 0x005116BC File Offset: 0x0050F8BC
		private void DrawExtraSettings()
		{
			if (this.anomalyPlaystyleDef.displayThreatFractionSliders || this.anomalyPlaystyleDef.overrideThreatFraction || this.anomalyPlaystyleDef.displayThreatFractionSliders)
			{
				this.listing.Label("CanBeEditedInStorytellerSettings".Translate() + ":", -1f, null);
			}
			if (this.anomalyPlaystyleDef.displayThreatFractionSliders)
			{
				TaggedString taggedString = "Difficulty_AnomalyThreatsInactive_Info".Translate();
				this.listing.Label("Difficulty_AnomalyThreatsInactive_Label".Translate() + ": " + this.anomalyThreatsInactiveFraction.ToStringPercent() + " - " + Dialog_AnomalySettings.GetFrequencyLabel(this.anomalyThreatsInactiveFraction), -1f, taggedString);
				this.anomalyThreatsInactiveFraction = this.listing.Slider(this.anomalyThreatsInactiveFraction, 0f, 1f);
				TaggedString taggedString2 = "Difficulty_AnomalyThreatsActive_Info".Translate(Mathf.Clamp01(this.anomalyThreatsActiveFraction).ToStringPercent(), Mathf.Clamp01(this.anomalyThreatsActiveFraction * 1.5f).ToStringPercent());
				this.listing.Label("Difficulty_AnomalyThreatsActive_Label".Translate() + ": " + this.anomalyThreatsActiveFraction.ToStringPercent() + " - " + Dialog_AnomalySettings.GetFrequencyLabel(this.anomalyThreatsActiveFraction), -1f, taggedString2);
				this.anomalyThreatsActiveFraction = this.listing.Slider(this.anomalyThreatsActiveFraction, 0f, 1f);
			}
			else if (this.anomalyPlaystyleDef.overrideThreatFraction)
			{
				TaggedString taggedString3 = "Difficulty_AnomalyThreats_Info".Translate();
				this.listing.Label("Difficulty_AnomalyThreats_Label".Translate() + ": " + this.overrideAnomalyThreatsFraction.ToStringPercent() + " - " + Dialog_AnomalySettings.GetFrequencyLabel(this.overrideAnomalyThreatsFraction), -1f, taggedString3);
				this.overrideAnomalyThreatsFraction = this.listing.Slider(this.overrideAnomalyThreatsFraction, 0f, 1f);
			}
			if (this.anomalyPlaystyleDef.displayStudyFactorSlider)
			{
				this.listing.Label("Difficulty_StudyEfficiency_Label".Translate() + ": " + this.studyEfficiencyFactor.ToStringPercent(), -1f, "Difficulty_StudyEfficiency_Info".Translate());
				this.studyEfficiencyFactor = this.listing.Slider(this.studyEfficiencyFactor, 0f, 5f);
			}
		}

		// Token: 0x06011A6B RID: 72299 RVA: 0x00511960 File Offset: 0x0050FB60
		public static string GetFrequencyLabel(float freq)
		{
			for (int i = 0; i < Dialog_AnomalySettings.FrequencyLabels.Length; i++)
			{
				if (freq <= Dialog_AnomalySettings.FrequencyLabels[i].Item1)
				{
					return Dialog_AnomalySettings.FrequencyLabels[i].Item2.Translate();
				}
			}
			return Dialog_AnomalySettings.FrequencyLabels[Dialog_AnomalySettings.FrequencyLabels.Length - 1].Item2.Translate();
		}

		// Token: 0x0400B5B9 RID: 46521
		private Difficulty difficulty;

		// Token: 0x0400B5BA RID: 46522
		private float anomalyThreatsInactiveFraction;

		// Token: 0x0400B5BB RID: 46523
		private float anomalyThreatsActiveFraction;

		// Token: 0x0400B5BC RID: 46524
		private float studyEfficiencyFactor;

		// Token: 0x0400B5BD RID: 46525
		private float overrideAnomalyThreatsFraction;

		// Token: 0x0400B5BE RID: 46526
		private AnomalyPlaystyleDef anomalyPlaystyleDef;

		// Token: 0x0400B5BF RID: 46527
		private Vector2 scrollPosition;

		// Token: 0x0400B5C0 RID: 46528
		private float scrollHeight;

		// Token: 0x0400B5C1 RID: 46529
		private Listing_Standard listing;

		// Token: 0x0400B5C2 RID: 46530
		private static readonly Vector2 AcceptButtonSize = new Vector2(100f, 35f);

		// Token: 0x0400B5C3 RID: 46531
		private const float DefaultOverrideThreatFraction = 0.15f;

		// Token: 0x0400B5C4 RID: 46532
		private const float MaxStudyEfficiencyFactor = 5f;

		// Token: 0x0400B5C5 RID: 46533
		private static ValueTuple<float, string>[] FrequencyLabels = new ValueTuple<float, string>[]
		{
			new ValueTuple<float, string>(0f, "AnomalyFrequency_None"),
			new ValueTuple<float, string>(0.05f, "AnomalyFrequency_VeryRare"),
			new ValueTuple<float, string>(0.2f, "AnomalyFrequency_Rare"),
			new ValueTuple<float, string>(0.5f, "AnomalyFrequency_Balanced"),
			new ValueTuple<float, string>(0.75f, "AnomalyFrequency_Intense"),
			new ValueTuple<float, string>(1f, "AnomalyFrequency_Overwhelming")
		};
	}
}
