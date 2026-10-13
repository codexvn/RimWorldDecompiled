using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020032A9 RID: 12969
	[StaticConstructorOnStartup]
	public static class StorytellerUI
	{
		// Token: 0x060123F8 RID: 74744 RVA: 0x0055DADB File Offset: 0x0055BCDB
		public static void ResetStorytellerSelectionInterface()
		{
			StorytellerUI.scrollPosition = default(Vector2);
			StorytellerUI.explanationScrollPosition = default(Vector2);
			StorytellerUI.explanationScrollPositionAnimated = null;
		}

		// Token: 0x060123F9 RID: 74745 RVA: 0x0055DAFC File Offset: 0x0055BCFC
		public static void DrawStorytellerSelectionInterface(Rect rect, ref StorytellerDef chosenStoryteller, ref DifficultyDef difficulty, ref Difficulty difficultyValues, Listing_Standard infoListing)
		{
			Widgets.BeginGroup(rect);
			Rect outRect = new Rect(0f, 0f, Storyteller.PortraitSizeTiny.x + 16f, rect.height);
			Rect viewRect = new Rect(0f, 0f, Storyteller.PortraitSizeTiny.x, (float)DefDatabase<StorytellerDef>.AllDefs.Count<StorytellerDef>() * (Storyteller.PortraitSizeTiny.y + 10f));
			Widgets.BeginScrollView(outRect, ref StorytellerUI.scrollPosition, viewRect, true);
			Rect rect2 = new Rect(0f, 0f, Storyteller.PortraitSizeTiny.x, Storyteller.PortraitSizeTiny.y).ContractedBy(4f);
			foreach (StorytellerDef storytellerDef in from tel in DefDatabase<StorytellerDef>.AllDefs
			orderby tel.listOrder
			select tel)
			{
				if (storytellerDef.listVisible)
				{
					bool flag = chosenStoryteller == storytellerDef;
					Widgets.DrawOptionBackground(rect2, flag);
					if (Widgets.ButtonImage(rect2, storytellerDef.portraitTinyTex, Color.white, new Color(0.72f, 0.68f, 0.59f), true, null))
					{
						TutorSystem.Notify_Event("ChooseStoryteller");
						chosenStoryteller = storytellerDef;
					}
					if (flag)
					{
						GUI.DrawTexture(rect2, StorytellerUI.StorytellerHighlightTex);
					}
					rect2.y += rect2.height + 8f;
				}
			}
			Widgets.EndScrollView();
			Rect outRect2 = new Rect(outRect.xMax + 8f, 0f, rect.width - outRect.width - 8f, rect.height);
			StorytellerUI.explanationInnerRect.width = outRect2.width - 16f;
			Widgets.BeginScrollView(outRect2, ref StorytellerUI.explanationScrollPosition, StorytellerUI.explanationInnerRect, true);
			Text.Font = GameFont.Small;
			Widgets.Label(new Rect(0f, 0f, 300f, 999f), "HowStorytellersWork".Translate());
			Rect rect3 = new Rect(0f, 120f, 290f, 9999f);
			float num = 300f;
			if (chosenStoryteller != null && chosenStoryteller.listVisible)
			{
				Rect position = new Rect(390f - outRect2.x, rect.height - Storyteller.PortraitSizeLarge.y - 1f, Storyteller.PortraitSizeLarge.x, Storyteller.PortraitSizeLarge.y);
				GUI.DrawTexture(position, chosenStoryteller.portraitLargeTex);
				Text.Anchor = TextAnchor.UpperLeft;
				infoListing.Begin(rect3);
				Text.Font = GameFont.Medium;
				infoListing.Indent(15f);
				infoListing.Label(chosenStoryteller.label, -1f, null);
				infoListing.Outdent(15f);
				Text.Font = GameFont.Small;
				infoListing.Gap(8f);
				infoListing.Label(chosenStoryteller.description, 160f, null);
				infoListing.Gap(6f);
				foreach (DifficultyDef difficultyDef in DefDatabase<DifficultyDef>.AllDefs)
				{
					TaggedString taggedString = difficultyDef.LabelCap;
					if (difficultyDef.isCustom)
					{
						taggedString += "...";
					}
					if (infoListing.RadioButton(taggedString, difficulty == difficultyDef, 0f, difficultyDef.description.ResolveTags(), new float?(0f)))
					{
						if (!difficultyDef.isCustom)
						{
							difficultyValues.CopyFrom(difficultyDef);
						}
						else if (difficultyDef != difficulty)
						{
							difficultyValues.CopyFrom(DifficultyDefOf.Rough);
							float time = Time.time;
							float num2 = 0.6f;
							StorytellerUI.explanationScrollPositionAnimated = AnimationCurve.EaseInOut(time, StorytellerUI.explanationScrollPosition.y, time + num2, StorytellerUI.explanationInnerRect.height);
						}
						difficulty = difficultyDef;
					}
					infoListing.Gap(3f);
				}
				if (Current.ProgramState == ProgramState.Entry)
				{
					infoListing.Gap(15f);
					bool active = Find.GameInitData.permadeathChosen && Find.GameInitData.permadeath;
					bool active2 = Find.GameInitData.permadeathChosen && !Find.GameInitData.permadeath;
					if (infoListing.RadioButton("ReloadAnytimeMode".Translate(), active2, 0f, "ReloadAnytimeModeInfo".Translate(), null))
					{
						Find.GameInitData.permadeathChosen = true;
						Find.GameInitData.permadeath = false;
					}
					infoListing.Gap(3f);
					if (infoListing.RadioButton("CommitmentMode".TranslateWithBackup("PermadeathMode"), active, 0f, "PermadeathModeInfo".Translate(), null))
					{
						Find.GameInitData.permadeathChosen = true;
						Find.GameInitData.permadeath = true;
					}
					if (ModsConfig.AnomalyActive)
					{
						infoListing.Gap(15f);
						if (infoListing.ButtonText("AnomalySettings".Translate() + "...", null, 1f))
						{
							if (difficulty == null)
							{
								Messages.Message("MustChooseDifficulty".Translate(), MessageTypeDefOf.RejectInput, false);
							}
							else
							{
								Find.WindowStack.Add(new Dialog_AnomalySettings(difficultyValues));
							}
						}
					}
				}
				num = rect3.y + infoListing.CurHeight;
				infoListing.End();
				if (difficulty != null && difficulty.isCustom)
				{
					if (StorytellerUI.explanationScrollPositionAnimated != null)
					{
						float time2 = Time.time;
						if (time2 < StorytellerUI.explanationScrollPositionAnimated.keys.Last<UnityEngine.Keyframe>().time)
						{
							StorytellerUI.explanationScrollPosition.y = StorytellerUI.explanationScrollPositionAnimated.Evaluate(time2);
						}
						else
						{
							StorytellerUI.explanationScrollPositionAnimated = null;
						}
					}
					Listing_Standard listing_Standard = new Listing_Standard();
					float num3 = position.xMax - StorytellerUI.explanationInnerRect.x;
					listing_Standard.ColumnWidth = num3 / 2f - 17f;
					Rect rect4 = new Rect(0f, Math.Max(position.yMax, num), num3, 9999f);
					listing_Standard.Begin(rect4);
					Text.Font = GameFont.Medium;
					listing_Standard.Indent(15f);
					listing_Standard.Label("DifficultyCustomSectionLabel".Translate(), -1f, null);
					listing_Standard.Outdent(15f);
					Text.Font = GameFont.Small;
					listing_Standard.Gap(12f);
					if (listing_Standard.ButtonText("DifficultyReset".Translate(), null, 1f))
					{
						StorytellerUI.MakeResetDifficultyFloatMenu(difficultyValues);
					}
					float curHeight = listing_Standard.CurHeight;
					float gapHeight = outRect2.height / 2f;
					StorytellerUI.DrawCustomLeft(listing_Standard, difficultyValues);
					listing_Standard.Gap(gapHeight);
					listing_Standard.NewColumn();
					listing_Standard.Gap(curHeight);
					StorytellerUI.DrawCustomRight(listing_Standard, difficultyValues);
					listing_Standard.Gap(gapHeight);
					num = rect4.y + listing_Standard.MaxColumnHeightSeen;
					listing_Standard.End();
				}
			}
			StorytellerUI.explanationInnerRect.height = num;
			Widgets.EndScrollView();
			Widgets.EndGroup();
		}

		// Token: 0x060123FA RID: 74746 RVA: 0x0055E248 File Offset: 0x0055C448
		private static void DrawCustomLeft(Listing_Standard listing, Difficulty difficulty)
		{
			Listing_Standard listing_Standard = StorytellerUI.DrawCustomSectionStart(listing, StorytellerUI.sectionHeightThreats, "DifficultyThreatSection".Translate(), null);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "threatScale", ref difficulty.threatScale, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowBigThreats", ref difficulty.allowBigThreats, false, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowViolentQuests", ref difficulty.allowViolentQuests, false, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowIntroThreats", ref difficulty.allowIntroThreats, false, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "predatorsHuntHumanlikes", ref difficulty.predatorsHuntHumanlikes, false, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowExtremeWeatherIncidents", ref difficulty.allowExtremeWeatherIncidents, false, true);
			if (ModsConfig.BiotechActive)
			{
				StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "wastepackInfestationChanceFactor", ref difficulty.wastepackInfestationChanceFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			}
			StorytellerUI.DrawCustomSectionEnd(listing, listing_Standard, out StorytellerUI.sectionHeightThreats);
			listing_Standard = StorytellerUI.DrawCustomSectionStart(listing, StorytellerUI.sectionHeightEconomy, "DifficultyEconomySection".Translate(), null);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "cropYieldFactor", ref difficulty.cropYieldFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "mineYieldFactor", ref difficulty.mineYieldFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "butcherYieldFactor", ref difficulty.butcherYieldFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			if (ModsConfig.OdysseyActive)
			{
				StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "fishingYieldFactor", ref difficulty.fishingYieldFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			}
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "researchSpeedFactor", ref difficulty.researchSpeedFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "questRewardValueFactor", ref difficulty.questRewardValueFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "raidLootPointsFactor", ref difficulty.raidLootPointsFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "tradePriceFactorLoss", ref difficulty.tradePriceFactorLoss, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 0.5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "maintenanceCostFactor", ref difficulty.maintenanceCostFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0.01f, 1f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "scariaRotChance", ref difficulty.scariaRotChance, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 1f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "enemyDeathOnDownedChanceFactor", ref difficulty.enemyDeathOnDownedChanceFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 1f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "nomadicMineableResourcesFactor", ref difficulty.nomadicMineableResourcesFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 2f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomSectionEnd(listing, listing_Standard, out StorytellerUI.sectionHeightEconomy);
			if (ModsConfig.IdeologyActive)
			{
				listing_Standard = StorytellerUI.DrawCustomSectionStart(listing, StorytellerUI.sectionHeightIdeology, "DifficultyIdeologySection".Translate(), null);
				StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "lowPopConversionBoost", ref difficulty.lowPopConversionBoost, ToStringStyle.Integer, ToStringNumberSense.Factor, 1f, 5f, 1f, false, 1000f);
				StorytellerUI.DrawCustomSectionEnd(listing, listing_Standard, out StorytellerUI.sectionHeightIdeology);
			}
			if (ModsConfig.AnomalyActive && difficulty.AnomalyPlaystyleDef.enableAnomalyContent)
			{
				listing_Standard = StorytellerUI.DrawCustomSectionStart(listing, StorytellerUI.sectionHeightAnomaly, "DifficultyAnomalySection".Translate(), null);
				if (difficulty.AnomalyPlaystyleDef.overrideThreatFraction)
				{
					float num = difficulty.overrideAnomalyThreatsFraction ?? 0.15f;
					StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "Difficulty_AnomalyThreats_Label".Translate(), Dialog_AnomalySettings.GetFrequencyLabel(num), "Difficulty_AnomalyThreats_Info".Translate(), ref num, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 1f, 0.01f, false, 1000f);
					difficulty.overrideAnomalyThreatsFraction = new float?(num);
				}
				else
				{
					StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "Difficulty_AnomalyThreatsInactive_Label".Translate(), Dialog_AnomalySettings.GetFrequencyLabel(difficulty.anomalyThreatsInactiveFraction), "Difficulty_AnomalyThreatsInactive_Info".Translate(), ref difficulty.anomalyThreatsInactiveFraction, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 1f, 0.01f, false, 1000f);
					float anomalyThreatsActiveFraction = difficulty.anomalyThreatsActiveFraction;
					StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "Difficulty_AnomalyThreatsActive_Label".Translate(), Dialog_AnomalySettings.GetFrequencyLabel(anomalyThreatsActiveFraction), "Difficulty_AnomalyThreatsActive_Info".Translate(Mathf.Clamp01(anomalyThreatsActiveFraction).ToStringPercent(), Mathf.Clamp01(anomalyThreatsActiveFraction * 1.5f).ToStringPercent()), ref difficulty.anomalyThreatsActiveFraction, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0.1f, 1f, 0.01f, false, 1000f);
				}
				StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "Difficulty_StudyEfficiency_Label".Translate(), "Difficulty_StudyEfficiency_Info".Translate(), ref difficulty.studyEfficiencyFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
				StorytellerUI.DrawCustomSectionEnd(listing, listing_Standard, out StorytellerUI.sectionHeightAnomaly);
			}
			if (ModsConfig.BiotechActive)
			{
				listing_Standard = StorytellerUI.DrawCustomSectionStart(listing, StorytellerUI.sectionHeightChildren, "DifficultyChildrenSection".Translate(), null);
				StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "noBabiesOrChildren", ref difficulty.noBabiesOrChildren, false, true);
				StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "babiesAreHealthy", ref difficulty.babiesAreHealthy, false, true);
				if (!difficulty.noBabiesOrChildren)
				{
					StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "childRaidersAllowed", ref difficulty.childRaidersAllowed, false, true);
					if (ModsConfig.AnomalyActive)
					{
						StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "childShamblersAllowed", ref difficulty.childShamblersAllowed, false, true);
					}
				}
				else
				{
					StorytellerUI.DrawDisabledCustomDifficultySetting(listing_Standard, "childRaidersAllowed", "BabiesAreHealthyDisableReason".Translate());
					if (ModsConfig.AnomalyActive)
					{
						StorytellerUI.DrawDisabledCustomDifficultySetting(listing_Standard, "childShamblersAllowed", "BabiesAreHealthyDisableReason".Translate());
					}
				}
				StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "childAgingRate", ref difficulty.childAgingRate, ToStringStyle.Integer, ToStringNumberSense.Factor, 1f, 6f, 1f, false, 1000f);
				StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "adultAgingRate", ref difficulty.adultAgingRate, ToStringStyle.Integer, ToStringNumberSense.Factor, 1f, 6f, 1f, false, 1000f);
				StorytellerUI.DrawCustomSectionEnd(listing, listing_Standard, out StorytellerUI.sectionHeightChildren);
			}
		}

		// Token: 0x060123FB RID: 74747 RVA: 0x0055E86C File Offset: 0x0055CA6C
		private static void DrawCustomRight(Listing_Standard listing, Difficulty difficulty)
		{
			Listing_Standard listing_Standard = StorytellerUI.DrawCustomSectionStart(listing, StorytellerUI.sectionHeightGeneral, "DifficultyGeneralSection".Translate(), null);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "colonistMoodOffset", ref difficulty.colonistMoodOffset, ToStringStyle.Integer, ToStringNumberSense.Offset, -20f, 20f, 1f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "foodPoisonChanceFactor", ref difficulty.foodPoisonChanceFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "manhunterChanceOnDamageFactor", ref difficulty.manhunterChanceOnDamageFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "playerPawnInfectionChanceFactor", ref difficulty.playerPawnInfectionChanceFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "diseaseIntervalFactor", ref difficulty.diseaseIntervalFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, true, 100f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "enemyReproductionRateFactor", ref difficulty.enemyReproductionRateFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "deepDrillInfestationChanceFactor", ref difficulty.deepDrillInfestationChanceFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 5f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "friendlyFireChanceFactor", ref difficulty.friendlyFireChanceFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 1f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "allowInstantKillChance", ref difficulty.allowInstantKillChance, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 1f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "peacefulTemples", ref difficulty.peacefulTemples, true, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowCaveHives", ref difficulty.allowCaveHives, false, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "unwaveringPrisoners", ref difficulty.unwaveringPrisoners, false, true);
			StorytellerUI.DrawCustomSectionEnd(listing, listing_Standard, out StorytellerUI.sectionHeightGeneral);
			listing_Standard = StorytellerUI.DrawCustomSectionStart(listing, StorytellerUI.sectionHeightPlayerTools, "DifficultyPlayerToolsSection".Translate(), null);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowTraps", ref difficulty.allowTraps, false, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowTurrets", ref difficulty.allowTurrets, false, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "allowMortars", ref difficulty.allowMortars, false, true);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "classicMortars", ref difficulty.classicMortars, false, true);
			StorytellerUI.DrawCustomSectionEnd(listing, listing_Standard, out StorytellerUI.sectionHeightPlayerTools);
			listing_Standard = StorytellerUI.DrawCustomSectionStart(listing, StorytellerUI.sectionHeightAdaptation, "DifficultyAdaptationSection".Translate(), null);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "adaptationGrowthRateFactorOverZero", ref difficulty.adaptationGrowthRateFactorOverZero, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 1f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "adaptationEffectFactor", ref difficulty.adaptationEffectFactor, ToStringStyle.PercentZero, ToStringNumberSense.Absolute, 0f, 1f, 0.01f, false, 1000f);
			StorytellerUI.DrawCustomDifficultyCheckbox(listing_Standard, "fixedWealthMode", ref difficulty.fixedWealthMode, false, true);
			GUI.enabled = difficulty.fixedWealthMode;
			float num = Mathf.Round(12f / difficulty.fixedWealthTimeFactor);
			StorytellerUI.DrawCustomDifficultySlider(listing_Standard, "fixedWealthTimeFactor", ref num, ToStringStyle.Integer, ToStringNumberSense.Absolute, 1f, 20f, 1f, false, 1000f);
			difficulty.fixedWealthTimeFactor = 12f / num;
			GUI.enabled = true;
			StorytellerUI.DrawCustomSectionEnd(listing, listing_Standard, out StorytellerUI.sectionHeightAdaptation);
		}

		// Token: 0x060123FC RID: 74748 RVA: 0x0055EB95 File Offset: 0x0055CD95
		private static Listing_Standard DrawCustomSectionStart(Listing_Standard listing, float height, string label, string tooltip = null)
		{
			listing.Gap(12f);
			listing.Label(label, -1f, tooltip);
			Listing_Standard listing_Standard = listing.BeginSection(height, 8f, 6f);
			listing_Standard.maxOneColumn = true;
			return listing_Standard;
		}

		// Token: 0x060123FD RID: 74749 RVA: 0x0055EBCD File Offset: 0x0055CDCD
		private static void DrawCustomSectionEnd(Listing_Standard listing, Listing_Standard section, out float height)
		{
			listing.EndSection(section);
			height = section.CurHeight;
		}

		// Token: 0x060123FE RID: 74750 RVA: 0x0055EBE0 File Offset: 0x0055CDE0
		private static void MakeResetDifficultyFloatMenu(Difficulty difficultyValues)
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
							difficultyValues.CopyFrom(d);
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x060123FF RID: 74751 RVA: 0x0055EC9C File Offset: 0x0055CE9C
		private static void DrawCustomDifficultySlider(Listing_Standard listing, string optionName, ref float value, ToStringStyle style, ToStringNumberSense numberSense, float min, float max, float precision = 0.01f, bool reciprocate = false, float reciprocalCutoff = 1000f)
		{
			string str = reciprocate ? "_Inverted" : "";
			string str2 = optionName.CapitalizeFirst();
			string key = "Difficulty_" + str2 + str + "_Label";
			string key2 = "Difficulty_" + str2 + str + "_Info";
			float num = value;
			if (reciprocate)
			{
				num = StorytellerUI.Reciprocal(num, reciprocalCutoff);
			}
			TaggedString label = key.Translate() + ": " + num.ToStringByStyle(style, numberSense);
			listing.Label(label, -1f, key2.Translate());
			float num2 = listing.Slider(num, min, max);
			if (num2 != num)
			{
				num = GenMath.RoundTo(num2, precision);
			}
			if (reciprocate)
			{
				num = StorytellerUI.Reciprocal(num, reciprocalCutoff);
			}
			value = num;
		}

		// Token: 0x06012400 RID: 74752 RVA: 0x0055ED58 File Offset: 0x0055CF58
		private static void DrawCustomDifficultySlider(Listing_Standard listing, string label, string tooltip, ref float value, ToStringStyle style, ToStringNumberSense numberSense, float min, float max, float precision = 0.01f, bool reciprocate = false, float reciprocalCutoff = 1000f)
		{
			StorytellerUI.DrawCustomDifficultySlider(listing, label, null, tooltip, ref value, style, numberSense, min, max, precision, reciprocate, reciprocalCutoff);
		}

		// Token: 0x06012401 RID: 74753 RVA: 0x0055ED80 File Offset: 0x0055CF80
		private static void DrawCustomDifficultySlider(Listing_Standard listing, string label, string labelSuffix, string tooltip, ref float value, ToStringStyle style, ToStringNumberSense numberSense, float min, float max, float precision = 0.01f, bool reciprocate = false, float reciprocalCutoff = 1000f)
		{
			float num = value;
			if (reciprocate)
			{
				num = StorytellerUI.Reciprocal(num, reciprocalCutoff);
			}
			label = label.CapitalizeFirst() + ": " + num.ToStringByStyle(style, numberSense);
			if (!labelSuffix.NullOrEmpty())
			{
				label = label + " - " + labelSuffix;
			}
			listing.Label(label, -1f, tooltip.CapitalizeFirst());
			float num2 = listing.Slider(num, min, max);
			if (num2 != num)
			{
				num = GenMath.RoundTo(num2, precision);
			}
			if (reciprocate)
			{
				num = StorytellerUI.Reciprocal(num, reciprocalCutoff);
			}
			value = num;
		}

		// Token: 0x06012402 RID: 74754 RVA: 0x0055EE14 File Offset: 0x0055D014
		private static void DrawCustomDifficultyCheckbox(Listing_Standard listing, string optionName, ref bool value, bool invert = false, bool showTooltip = true)
		{
			string str = invert ? "_Inverted" : "";
			string str2 = optionName.CapitalizeFirst();
			string key = "Difficulty_" + str2 + str + "_Label";
			string key2 = "Difficulty_" + str2 + str + "_Info";
			bool flag = invert ? (!value) : value;
			listing.CheckboxLabeled(key.Translate(), ref flag, showTooltip ? key2.Translate() : null, 0f, 1f);
			value = (invert ? (!flag) : flag);
		}

		// Token: 0x06012403 RID: 74755 RVA: 0x0055EEAC File Offset: 0x0055D0AC
		private static void DrawDisabledCustomDifficultySetting(Listing_Standard listing, string optionName, TaggedString disableReason)
		{
			string str = optionName.CapitalizeFirst();
			string key = "Difficulty_" + str + "_Label";
			string key2 = "Difficulty_" + str + "_Info";
			Color color = GUI.color;
			GUI.color = ColoredText.SubtleGrayColor;
			listing.Label(key.Translate(), -1f, (key2.Translate() + "\n\n" + disableReason.Colorize(ColoredText.WarningColor)).ToString());
			GUI.color = color;
		}

		// Token: 0x06012404 RID: 74756 RVA: 0x0055EF36 File Offset: 0x0055D136
		private static float Reciprocal(float f, float cutOff)
		{
			cutOff *= 10f;
			if (Mathf.Abs(f) < 0.01f)
			{
				return cutOff;
			}
			if (f >= 0.99f * cutOff)
			{
				return 0f;
			}
			return 1f / f;
		}

		// Token: 0x0400BDDF RID: 48607
		private static Vector2 scrollPosition = default(Vector2);

		// Token: 0x0400BDE0 RID: 48608
		private static Vector2 explanationScrollPosition = default(Vector2);

		// Token: 0x0400BDE1 RID: 48609
		private static AnimationCurve explanationScrollPositionAnimated;

		// Token: 0x0400BDE2 RID: 48610
		private static Rect explanationInnerRect = default(Rect);

		// Token: 0x0400BDE3 RID: 48611
		private static float sectionHeightThreats = 0f;

		// Token: 0x0400BDE4 RID: 48612
		private static float sectionHeightGeneral = 0f;

		// Token: 0x0400BDE5 RID: 48613
		private static float sectionHeightPlayerTools = 0f;

		// Token: 0x0400BDE6 RID: 48614
		private static float sectionHeightEconomy = 0f;

		// Token: 0x0400BDE7 RID: 48615
		private static float sectionHeightAdaptation = 0f;

		// Token: 0x0400BDE8 RID: 48616
		private static float sectionHeightIdeology = 0f;

		// Token: 0x0400BDE9 RID: 48617
		private static float sectionHeightChildren = 0f;

		// Token: 0x0400BDEA RID: 48618
		private static float sectionHeightAnomaly = 0f;

		// Token: 0x0400BDEB RID: 48619
		private static readonly Texture2D StorytellerHighlightTex = ContentFinder<Texture2D>.Get("UI/HeroArt/Storytellers/Highlight", true);

		// Token: 0x0400BDEC RID: 48620
		private const float CustomSettingsPrecision = 0.01f;
	}
}
