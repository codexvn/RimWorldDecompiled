using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003125 RID: 12581
	[StaticConstructorOnStartup]
	public class Dialog_BeginRitual : Dialog_BeginLordJob
	{
		// Token: 0x17002DFC RID: 11772
		// (get) Token: 0x06011AD9 RID: 72409 RVA: 0x00514E97 File Offset: 0x00513097
		public override TaggedString HeaderLabel
		{
			get
			{
				return this.ritualLabel;
			}
		}

		// Token: 0x17002DFD RID: 11773
		// (get) Token: 0x06011ADA RID: 72410 RVA: 0x00514EA4 File Offset: 0x005130A4
		public override TaggedString OkButtonLabel
		{
			get
			{
				return this.okButtonText;
			}
		}

		// Token: 0x17002DFE RID: 11774
		// (get) Token: 0x06011ADB RID: 72411 RVA: 0x00514EB1 File Offset: 0x005130B1
		public override Texture2D Icon
		{
			get
			{
				Precept_Ritual precept_Ritual = this.ritual;
				if (precept_Ritual == null)
				{
					return null;
				}
				RitualPatternDef sourcePattern = precept_Ritual.sourcePattern;
				if (sourcePattern == null)
				{
					return null;
				}
				return sourcePattern.Icon;
			}
		}

		// Token: 0x17002DFF RID: 11775
		// (get) Token: 0x06011ADC RID: 72412 RVA: 0x00514ED0 File Offset: 0x005130D0
		public string SleepingWarning
		{
			get
			{
				if (this.sleepingMessage.NullOrEmpty())
				{
					this.sleepingMessage = "RitualBeginSleepingWarning".Translate();
				}
				if (this.assignments.Participants.Any((Pawn p) => !p.Awake()))
				{
					return this.sleepingMessage;
				}
				return null;
			}
		}

		// Token: 0x06011ADD RID: 72413 RVA: 0x00514F38 File Offset: 0x00513138
		public override Color QualitySummaryColor(FloatRange qualityRange)
		{
			if (qualityRange.min >= 0.25f)
			{
				return Color.white;
			}
			return ColorLibrary.RedReadable;
		}

		// Token: 0x17002E00 RID: 11776
		// (get) Token: 0x06011ADE RID: 72414 RVA: 0x00514F54 File Offset: 0x00513154
		public override TaggedString DescriptionLabel
		{
			get
			{
				Precept_Ritual precept_Ritual = this.ritual;
				string text;
				if (precept_Ritual == null)
				{
					text = null;
				}
				else
				{
					RitualBehaviorWorker behavior = precept_Ritual.behavior;
					text = ((behavior != null) ? behavior.descriptionOverride : null);
				}
				string text2;
				if ((text2 = text) == null)
				{
					Precept_Ritual precept_Ritual2 = this.ritual;
					text2 = ((precept_Ritual2 != null) ? precept_Ritual2.Description : null);
				}
				string text3 = text2;
				if (text3 == null)
				{
					return TaggedString.Empty;
				}
				return text3.Formatted(this.organizer.Named("ORGANIZER"));
			}
		}

		// Token: 0x17002E01 RID: 11777
		// (get) Token: 0x06011ADF RID: 72415 RVA: 0x00514FB4 File Offset: 0x005131B4
		public override TaggedString ExtraExplanationLabel
		{
			get
			{
				Precept_Ritual precept_Ritual = this.ritual;
				string text;
				if (precept_Ritual == null)
				{
					text = null;
				}
				else
				{
					RitualBehaviorWorker behavior = precept_Ritual.behavior;
					text = ((behavior != null) ? behavior.GetExplanation(this.ritual, this.assignments, this.PredictedQuality(null).min) : null);
				}
				string text2 = text;
				if (!this.ritualExplanation.NullOrEmpty() || !text2.NullOrEmpty())
				{
					string text3 = this.ritualExplanation;
					if (!text2.NullOrEmpty())
					{
						if (!text3.NullOrEmpty())
						{
							text3 += "\n\n";
						}
						text3 += text2;
					}
					return text3;
				}
				return "";
			}
		}

		// Token: 0x17002E02 RID: 11778
		// (get) Token: 0x06011AE0 RID: 72416 RVA: 0x00515049 File Offset: 0x00513249
		public override TaggedString ExpectedQualityLabel
		{
			get
			{
				Precept_Ritual precept_Ritual = this.ritual;
				bool flag;
				if (precept_Ritual == null)
				{
					flag = (null != null);
				}
				else
				{
					RitualOutcomeEffectWorker outcomeEffect = precept_Ritual.outcomeEffect;
					flag = (((outcomeEffect != null) ? outcomeEffect.ExpectedQualityLabel() : null) != null);
				}
				if (flag)
				{
					return this.ritual.outcomeEffect.ExpectedQualityLabel();
				}
				return base.ExpectedQualityLabel;
			}
		}

		// Token: 0x06011AE1 RID: 72417 RVA: 0x00515087 File Offset: 0x00513287
		protected override IEnumerable<string> BlockingIssues()
		{
			if (!this.assignments.Participants.Any<Pawn>())
			{
				yield return "MessageRitualNeedsAtLeastOnePerson".Translate();
			}
			foreach (Pawn pawn in this.assignments.Participants)
			{
				if (!pawn.IsPrisoner && !pawn.SafeTemperatureRange().IncludesEpsilon(this.target.Cell.GetTemperature(this.target.Map)))
				{
					Precept_Ritual precept_Ritual = this.ritual;
					if (precept_Ritual == null || !precept_Ritual.ignoreExtremeTemperatures)
					{
						yield return "CantJoinRitualInExtremeWeather".Translate();
						break;
					}
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			if (this.ritual != null)
			{
				if (this.ritual.behavior.SpectatorsRequired() && this.assignments.SpectatorsForReading.Count == 0)
				{
					yield return "MessageRitualNeedsAtLeastOneSpectator".Translate();
				}
				if (this.ritual.outcomeEffect != null)
				{
					foreach (string text in this.ritual.outcomeEffect.BlockingIssues(this.ritual, this.target, this.assignments))
					{
						yield return text;
					}
					IEnumerator<string> enumerator2 = null;
				}
				if (this.ritual.obligationTargetFilter != null)
				{
					foreach (string text2 in this.ritual.obligationTargetFilter.GetBlockingIssues(this.target, this.assignments))
					{
						yield return text2;
					}
					IEnumerator<string> enumerator2 = null;
				}
				if (!this.ritual.behavior.def.roles.NullOrEmpty<RitualRole>())
				{
					foreach (IGrouping<string, RitualRole> source in from r in this.ritual.behavior.def.roles
					group r by r.mergeId ?? r.id)
					{
						RitualRole firstRole = source.First<RitualRole>();
						int requiredPawnCount = source.Count((RitualRole r) => r.required);
						if (requiredPawnCount > 0)
						{
							IEnumerable<Pawn> selectedPawns = source.SelectMany((RitualRole r) => this.assignments.AssignedPawns(r));
							foreach (Pawn p2 in selectedPawns)
							{
								bool flag;
								string text3 = this.assignments.PawnNotAssignableReason(p2, firstRole, out flag);
								if (text3 != null)
								{
									yield return text3;
								}
							}
							IEnumerator<Pawn> enumerator4 = null;
							if (requiredPawnCount == 1 && !selectedPawns.Any<Pawn>())
							{
								yield return "MessageLordJobNeedsAtLeastOneRolePawn".Translate(firstRole.Label.Resolve());
							}
							else if (requiredPawnCount > 1 && selectedPawns.Count<Pawn>() < requiredPawnCount)
							{
								yield return "MessageLordJobNeedsAtLeastNumRolePawn".Translate(Find.ActiveLanguageWorker.Pluralize(firstRole.Label, -1), requiredPawnCount);
							}
							selectedPawns = null;
						}
						firstRole = null;
					}
					IEnumerator<IGrouping<string, RitualRole>> enumerator3 = null;
					if (!this.assignments.ExtraRequiredPawnsForReading.NullOrEmpty<Pawn>())
					{
						foreach (Pawn pawn2 in this.assignments.ExtraRequiredPawnsForReading)
						{
							bool flag;
							string text4 = this.assignments.PawnNotAssignableReason(pawn2, this.assignments.RoleForPawn(pawn2, true), out flag);
							if (text4 != null)
							{
								yield return text4;
							}
						}
						enumerator = default(List<Pawn>.Enumerator);
					}
				}
				if (this.ritual.ritualOnlyForIdeoMembers && !this.assignments.Participants.Any((Pawn p) => p.Ideo == this.ritual.ideo))
				{
					yield return "MessageNeedAtLeastOneParticipantOfIdeo".Translate(this.ritual.ideo.memberName);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06011AE2 RID: 72418 RVA: 0x00515098 File Offset: 0x00513298
		public override TaggedString ExpectedDurationLabel(FloatRange qualityRange)
		{
			if (this.ritual == null)
			{
				return TaggedString.Empty;
			}
			string text = this.ritual.behavior.ExpectedDuration(this.ritual, this.assignments, qualityRange.min);
			if (text.NullOrEmpty())
			{
				return null;
			}
			return "{0}: {1}".Formatted("ExpectedLordJobDuration".Translate(), text);
		}

		// Token: 0x06011AE3 RID: 72419 RVA: 0x00515104 File Offset: 0x00513304
		public override string OutcomeToolTip(ILordJobOutcomePossibility outcomeChance)
		{
			string text = this.outcome.OutcomeMoodBreakdown(outcomeChance as RitualOutcomePossibility);
			string text2 = base.OutcomeToolTip(outcomeChance);
			if (text2 != null && !text2.NullOrEmpty())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += text2;
			}
			return text;
		}

		// Token: 0x06011AE4 RID: 72420 RVA: 0x00515153 File Offset: 0x00513353
		private Dialog_BeginRitual(RitualRoleAssignments assignments, Precept_Ritual ritual, TargetInfo target, RitualOutcomeEffectDef outcome) : base(new PawnRitualRoleSelectionWidget(assignments, ritual, target, outcome))
		{
			this.assignments = assignments;
			this.ritual = ritual;
			this.target = target;
			this.outcome = outcome;
		}

		// Token: 0x06011AE5 RID: 72421 RVA: 0x00515184 File Offset: 0x00513384
		public Dialog_BeginRitual(string ritualLabel, Precept_Ritual ritual, TargetInfo target, Map map, Dialog_BeginRitual.ActionCallback action, Pawn organizer, RitualObligation obligation, Dialog_BeginRitual.PawnFilter filter = null, string okButtonText = null, List<Pawn> requiredPawns = null, Dictionary<string, Pawn> forcedForRole = null, RitualOutcomeEffectDef outcome = null, List<string> extraInfoText = null, Pawn selectedPawn = null)
		{
			RitualRoleAssignments ritualRoleAssignments = Dialog_BeginRitual.CreateRitualRoleAssignments(ritual, target, map, filter, requiredPawns, forcedForRole, selectedPawn);
			RitualOutcomeEffectDef ritualOutcomeEffectDef;
			if (ritual == null)
			{
				ritualOutcomeEffectDef = null;
			}
			else
			{
				RitualOutcomeEffectWorker outcomeEffect = ritual.outcomeEffect;
				ritualOutcomeEffectDef = ((outcomeEffect != null) ? outcomeEffect.def : null);
			}
			this..ctor(ritualRoleAssignments, ritual, target, ritualOutcomeEffectDef ?? outcome);
			this.obligation = obligation;
			this.filter = filter;
			this.organizer = organizer;
			this.map = map;
			this.action = action;
			this.ritualExplanation = ((ritual != null) ? ritual.ritualExplanation : null);
			this.ritualLabel = ritualLabel;
			this.okButtonText = (okButtonText ?? base.OkButtonLabel);
			this.extraInfos = extraInfoText;
			Dialog_BeginRitual.cachedRoles.Clear();
			if (((ritual != null) ? ritual.ideo : null) != null)
			{
				Dialog_BeginRitual.cachedRoles.AddRange(from r in ritual.ideo.RolesListForReading
				where !r.def.leaderRole
				select r);
				Precept_Role precept_Role = Faction.OfPlayer.ideos.PrimaryIdeo.RolesListForReading.FirstOrDefault((Precept_Role p) => p.def.leaderRole);
				if (precept_Role != null)
				{
					Dialog_BeginRitual.cachedRoles.Add(precept_Role);
				}
				Dialog_BeginRitual.cachedRoles.SortBy((Precept_Role x) => x.def.displayOrderInImpact);
			}
		}

		// Token: 0x06011AE6 RID: 72422 RVA: 0x005152EC File Offset: 0x005134EC
		protected override void Start()
		{
			if (!this.confirmText.NullOrEmpty())
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(this.confirmText, delegate()
				{
					if (this.PredictedQuality(null).min < 0.25f && this.outcome.warnOnLowQuality)
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("RitualQualityLowWarning".Translate(this.HeaderLabel, 0.25f.ToStringPercent()), new Action(this.<Start>g__InnerStart|40_0), true, null, WindowLayer.Dialog));
						return;
					}
					this.<Start>g__InnerStart|40_0();
				}, true, null, WindowLayer.Dialog));
				return;
			}
			this.<Start>g__InnerStart|40_0();
		}

		// Token: 0x06011AE7 RID: 72423 RVA: 0x0051532C File Offset: 0x0051352C
		private FloatRange PredictedQuality(List<QualityFactor> expectedOutcomeEffects = null)
		{
			float num = this.outcome.startingQuality;
			float num2 = 0f;
			foreach (RitualOutcomeComp ritualOutcomeComp in this.outcome.comps)
			{
				RitualOutcomeComp ritualOutcomeComp2 = ritualOutcomeComp;
				Precept_Ritual precept_Ritual = this.ritual;
				TargetInfo ritualTarget = this.target;
				RitualObligation ritualObligation = this.obligation;
				RitualRoleAssignments ritualRoleAssignments = this.assignments;
				Precept_Ritual precept_Ritual2 = this.ritual;
				RitualOutcomeComp_Data data;
				if (precept_Ritual2 == null)
				{
					data = null;
				}
				else
				{
					RitualOutcomeEffectWorker outcomeEffect = precept_Ritual2.outcomeEffect;
					data = ((outcomeEffect != null) ? outcomeEffect.DataForComp(ritualOutcomeComp) : null);
				}
				QualityFactor qualityFactor = ritualOutcomeComp2.GetQualityFactor(precept_Ritual, ritualTarget, ritualObligation, ritualRoleAssignments, data);
				if (qualityFactor != null)
				{
					if (!qualityFactor.label.NullOrEmpty() && expectedOutcomeEffects != null)
					{
						expectedOutcomeEffects.Add(qualityFactor);
					}
					if (qualityFactor.uncertainOutcome)
					{
						num2 += qualityFactor.quality;
					}
					else
					{
						num += qualityFactor.quality;
					}
				}
			}
			if (this.ritual != null && this.ritual.RepeatPenaltyActive)
			{
				num += this.ritual.RepeatQualityPenalty;
			}
			Map map = this.map;
			Precept_Ritual precept_Ritual3 = this.ritual;
			Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(map, (precept_Ritual3 != null) ? precept_Ritual3.def : null);
			if (expectationsOffset != null)
			{
				num += expectationsOffset.Item2;
			}
			num = Mathf.Clamp(num, this.outcome.minQuality, this.outcome.maxQuality);
			num2 += num;
			num2 = Mathf.Clamp(num2, this.outcome.minQuality, this.outcome.maxQuality);
			return new FloatRange(num, num2);
		}

		// Token: 0x06011AE8 RID: 72424 RVA: 0x005154A8 File Offset: 0x005136A8
		protected override List<QualityFactor> PopulateQualityFactors(out FloatRange qualityRange)
		{
			List<QualityFactor> list = base.PopulateQualityFactors(out qualityRange);
			if (this.outcome == null)
			{
				return list;
			}
			float startingQuality = this.outcome.startingQuality;
			qualityRange = this.PredictedQuality(list);
			if (startingQuality > 0f)
			{
				list.Add(new QualityFactor
				{
					label = "StartingQuality".Translate(),
					qualityChange = "+" + startingQuality.ToStringPercent("F0"),
					quality = startingQuality,
					noMiddleColumnInfo = true,
					positive = true,
					priority = 5f
				});
			}
			if (this.ritual != null && this.ritual.RepeatPenaltyActive)
			{
				float repeatQualityPenalty = this.ritual.RepeatQualityPenalty;
				list.Add(new QualityFactor
				{
					label = "RitualOutcomePerformedRecently".Translate(),
					qualityChange = repeatQualityPenalty.ToStringPercent(),
					quality = repeatQualityPenalty,
					noMiddleColumnInfo = true,
					positive = false,
					priority = 5f
				});
			}
			Map map = this.map;
			Precept_Ritual precept_Ritual = this.ritual;
			Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(map, (precept_Ritual != null) ? precept_Ritual.def : null);
			if (expectationsOffset != null)
			{
				list.Add(new QualityFactor
				{
					label = "RitualQualityExpectations".Translate(expectationsOffset.Item1.LabelCap),
					qualityChange = "+" + expectationsOffset.Item2.ToStringPercent(),
					quality = expectationsOffset.Item2,
					noMiddleColumnInfo = true,
					positive = true,
					priority = 5f
				});
			}
			return list;
		}

		// Token: 0x06011AE9 RID: 72425 RVA: 0x00515644 File Offset: 0x00513844
		protected override List<ILordJobOutcomePossibility> PopulateOutcomePossibilities()
		{
			List<ILordJobOutcomePossibility> list = base.PopulateOutcomePossibilities();
			RitualOutcomeEffectDef ritualOutcomeEffectDef = this.outcome;
			bool flag;
			if (ritualOutcomeEffectDef == null)
			{
				flag = false;
			}
			else
			{
				List<RitualOutcomePossibility> outcomeChances = ritualOutcomeEffectDef.outcomeChances;
				bool? flag2 = (outcomeChances != null) ? new bool?(outcomeChances.NullOrEmpty<RitualOutcomePossibility>()) : null;
				bool flag3 = false;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag)
			{
				list.AddRange(this.outcome.outcomeChances);
				if (this.ritual != null && this.ritual.outcomeEffect != null)
				{
					RitualOutcomePossibility forcedOutcome = this.ritual.outcomeEffect.GetForcedOutcome(this.ritual, this.target, this.obligation, this.assignments);
					if (forcedOutcome != null)
					{
						list.Clear();
						list.Add(forcedOutcome);
					}
				}
			}
			return list;
		}

		// Token: 0x06011AEA RID: 72426 RVA: 0x00515700 File Offset: 0x00513900
		public override void DrawExtraOutcomeDescriptions(Rect viewRect, FloatRange qualityRange, string qualityNumber, ref float curY, ref float totalInfoHeight)
		{
			this.DrawExtraRitualOutcomeDescriptions(viewRect, qualityRange.min, ref curY, ref totalInfoHeight);
			curY += 10f;
			totalInfoHeight += 10f;
			this.DrawExtraInfo(viewRect, ref curY, ref totalInfoHeight);
			this.DrawOutcomeWarnings(viewRect, ref curY, ref totalInfoHeight);
			this.DrawFinalOutcomeInfo(viewRect, ref curY, ref totalInfoHeight);
		}

		// Token: 0x06011AEB RID: 72427 RVA: 0x00515758 File Offset: 0x00513958
		public void DrawExtraRitualOutcomeDescriptions(Rect viewRect, float totalQuality, ref float curY, ref float totalInfoHeight)
		{
			if (this.outcome.extraOutcomeDescriptions == null)
			{
				return;
			}
			curY += Text.LineHeight;
			totalInfoHeight += Text.LineHeight;
			int num = 0;
			for (;;)
			{
				int num2 = num;
				List<RitualOutcomeEffectDef.ExtraOutcomeChanceDescription> extraOutcomeDescriptions = this.outcome.extraOutcomeDescriptions;
				int? num3 = (extraOutcomeDescriptions != null) ? new int?(extraOutcomeDescriptions.Count) : null;
				if (!(num2 < num3.GetValueOrDefault() & num3 != null))
				{
					break;
				}
				RitualOutcomeEffectDef.ExtraOutcomeChanceDescription extraOutcomeChanceDescription = this.outcome.extraOutcomeDescriptions[num];
				float value = extraOutcomeChanceDescription.qualityToValue(totalQuality);
				TaggedString taggedString = extraOutcomeChanceDescription.description.Formatted(value);
				Vector2 vector = Text.CalcSize(taggedString);
				if (vector.x > viewRect.width)
				{
					vector = new Vector2(viewRect.width, Text.CalcHeight(taggedString, viewRect.width));
				}
				Widgets.Label(new Rect(viewRect.x, curY, vector.x, Mathf.Max(32f, vector.y)), taggedString);
				curY += vector.y;
				totalInfoHeight += vector.y;
				num++;
			}
		}

		// Token: 0x06011AEC RID: 72428 RVA: 0x00515880 File Offset: 0x00513A80
		public void DrawExtraInfo(Rect viewRect, ref float curY, ref float totalInfoHeight)
		{
			if (this.extraInfos == null)
			{
				return;
			}
			foreach (string text in this.extraInfos)
			{
				float num = Math.Max(Text.CalcHeight(text, viewRect.width) + 3f, 28f);
				Widgets.Label(new Rect(viewRect.x, curY + 4f, viewRect.width, num), text);
				curY += num;
				totalInfoHeight += num;
			}
		}

		// Token: 0x06011AED RID: 72429 RVA: 0x00515924 File Offset: 0x00513B24
		public void DrawOutcomeWarnings(Rect viewRect, ref float curY, ref float totalInfoHeight)
		{
			string sleepingWarning = this.SleepingWarning;
			if (sleepingWarning.NullOrEmpty())
			{
				return;
			}
			float num = Math.Max(Text.CalcHeight(sleepingWarning, viewRect.width) + 3f, 28f);
			Widgets.Label(new Rect(viewRect.x, curY + 4f, viewRect.width, num), sleepingWarning);
			curY += num;
		}

		// Token: 0x06011AEE RID: 72430 RVA: 0x00515988 File Offset: 0x00513B88
		public void DrawFinalOutcomeInfo(Rect viewRect, ref float curY, ref float totalInfoHeight)
		{
			if (this.outcome.outcomeChances.NullOrEmpty<RitualOutcomePossibility>())
			{
				return;
			}
			Precept_Ritual precept_Ritual = this.ritual;
			bool flag;
			if (precept_Ritual == null)
			{
				flag = true;
			}
			else
			{
				Ideo ideo = precept_Ritual.ideo;
				bool? flag2 = (ideo != null) ? new bool?(ideo.Fluid) : null;
				bool flag3 = true;
				flag = !(flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag)
			{
				return;
			}
			SimpleCurve developmentPointsOverOutcomeIndexCurveForRitual = IdeoDevelopmentUtility.GetDevelopmentPointsOverOutcomeIndexCurveForRitual(this.ritual.ideo, this.ritual);
			if (developmentPointsOverOutcomeIndexCurveForRitual == null)
			{
				return;
			}
			curY += 10f;
			totalInfoHeight += 10f;
			TaggedString taggedString = "RitualDevelopmentPointRewards".Translate() + ":\n";
			float num = Text.CalcHeight(taggedString, viewRect.width);
			Widgets.Label(new Rect(viewRect.x, curY, viewRect.width, num), taggedString);
			curY += num;
			for (int i = 0; i < this.outcome.outcomeChances.Count; i++)
			{
				RitualOutcomePossibility ritualOutcomePossibility = this.outcome.outcomeChances[i];
				string label = "  - " + ritualOutcomePossibility.label + ": " + developmentPointsOverOutcomeIndexCurveForRitual.Evaluate((float)i).ToStringWithSign("0.##");
				Widgets.Label(new Rect(viewRect.x, curY, viewRect.width, 32f), label);
				curY += Text.LineHeight;
				totalInfoHeight += Text.LineHeight;
			}
		}

		// Token: 0x06011AEF RID: 72431 RVA: 0x00515B00 File Offset: 0x00513D00
		public override void DoExtraHeaderInfo(ref RectDivider layout, ref RectDivider headerLabelRow)
		{
			if (this.ritual == null)
			{
				return;
			}
			if (ModsConfig.IdeologyActive && this.ritual.ideo != null && !Find.IdeoManager.classicMode)
			{
				Ideo ideo = this.ritual.ideo;
				Vector2 vector = Text.CalcSize(ideo.name.CapitalizeFirst());
				IdeoUIUtility.DrawIdeoPlate(headerLabelRow.NewRow(vector.y, VerticalJustification.Top, null).NewCol(vector.x + 30f, HorizontalJustification.Right, null), ideo, null);
			}
			using (new TextBlock(new GameFont?(GameFont.Small), new TextAnchor?(TextAnchor.LowerRight), new bool?(false), new Color?(Color.gray)))
			{
				if (!this.ritual.Label.EqualsIgnoreCase(this.ritual.UIInfoFirstLine))
				{
					Widgets.Label(new Rect(headerLabelRow)
					{
						height = Text.CalcHeight(this.ritual.UIInfoFirstLine, headerLabelRow.Rect.width)
					}, this.ritual.UIInfoFirstLine);
				}
			}
		}

		// Token: 0x06011AF0 RID: 72432 RVA: 0x00515C40 File Offset: 0x00513E40
		public override void PostOpen()
		{
			base.PostOpen();
			this.assignments.FillPawns(this.filter, this.target);
			if (this.outcome != null && this.ritual != null && this.ritual.outcomeEffect != null)
			{
				foreach (RitualOutcomeComp ritualOutcomeComp in this.outcome.comps)
				{
					ritualOutcomeComp.Notify_AssignmentsChanged(this.assignments, this.ritual.outcomeEffect.DataForComp(ritualOutcomeComp));
				}
			}
		}

		// Token: 0x06011AF1 RID: 72433 RVA: 0x00515CE8 File Offset: 0x00513EE8
		protected override void DrawQualityFactors(Rect viewRect)
		{
			Precept_Ritual precept_Ritual = this.ritual;
			bool flag;
			if (precept_Ritual == null)
			{
				flag = false;
			}
			else
			{
				RitualOutcomeEffectWorker outcomeEffect = precept_Ritual.outcomeEffect;
				bool? flag2 = (outcomeEffect != null) ? new bool?(outcomeEffect.ShowQuality) : null;
				bool flag3 = false;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag)
			{
				Rect rect = viewRect;
				rect.y += 17f;
				string t = this.ritual.outcomeEffect.def.outcomeChances.MaxBy((RitualOutcomePossibility c) => c.positivityIndex).label.CapitalizeFirst();
				TaggedString taggedString = "RitualOutcomeNoQuality".Translate() + ":\n\n  - " + t + " " + this.ritual.Label;
				Rect rect2 = rect;
				rect2.width += 10f;
				rect2.height = Text.CalcHeight(taggedString, rect.width);
				rect2 = rect2.ExpandedBy(9f);
				GUI.color = new Color(0.25f, 0.25f, 0.25f);
				Widgets.DrawBox(rect2, 2, null);
				GUI.color = Color.white;
				Widgets.Label(rect, taggedString);
				return;
			}
			base.DrawQualityFactors(viewRect);
		}

		// Token: 0x06011AF2 RID: 72434 RVA: 0x00515E48 File Offset: 0x00514048
		private void UpdateRoleChangeTargetRole(Pawn p)
		{
			Precept_Role roleToChangeTo = null;
			Ideo ideo = p.Ideo;
			if (((ideo != null) ? ideo.GetRole(p) : null) == null)
			{
				roleToChangeTo = RitualUtility.AllRolesForPawn(p).FirstOrDefault((Precept_Role r) => r.Active && r.RequirementsMet(p));
			}
			this.SetRoleToChangeTo(roleToChangeTo);
		}

		// Token: 0x06011AF3 RID: 72435 RVA: 0x00515EA7 File Offset: 0x005140A7
		public void SetRoleToChangeTo(Precept_Role role)
		{
			this.assignments.SetRoleChangeSelection(role);
		}

		// Token: 0x06011AF4 RID: 72436 RVA: 0x00515EB8 File Offset: 0x005140B8
		private void DrawRoleSelection(Pawn pawn, Rect rect)
		{
			Dialog_BeginRitual.<>c__DisplayClass54_0 CS$<>8__locals1 = new Dialog_BeginRitual.<>c__DisplayClass54_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pawn = pawn;
			Precept_Role roleChangeSelection = this.assignments.RoleChangeSelection;
			Dialog_BeginRitual.<>c__DisplayClass54_0 CS$<>8__locals2 = CS$<>8__locals1;
			Pawn pawn2 = CS$<>8__locals1.pawn;
			Precept_Role currentRole;
			if (pawn2 == null)
			{
				currentRole = null;
			}
			else
			{
				Ideo ideo = pawn2.Ideo;
				currentRole = ((ideo != null) ? ideo.GetRole(CS$<>8__locals1.pawn) : null);
			}
			CS$<>8__locals2.currentRole = currentRole;
			if (roleChangeSelection == null && CS$<>8__locals1.currentRole == null)
			{
				this.UpdateRoleChangeTargetRole(CS$<>8__locals1.pawn);
				roleChangeSelection = this.assignments.RoleChangeSelection;
			}
			if (roleChangeSelection != null || CS$<>8__locals1.currentRole != null)
			{
				SocialCardUtility.DrawPawnRole(CS$<>8__locals1.pawn, roleChangeSelection, (roleChangeSelection != null) ? roleChangeSelection.LabelCap : "RemoveRole".Translate(CS$<>8__locals1.currentRole.Label).Resolve(), rect, false);
			}
			Rect rect2 = new Rect(rect.x + 220f, rect.y + 2f, 140f, 32f);
			Pawn pawn3 = CS$<>8__locals1.pawn;
			bool flag = ((pawn3 != null) ? pawn3.Ideo : null) != null;
			if (!flag)
			{
				GUI.color = Color.gray;
			}
			if (Dialog_BeginRitual.cachedRoles.Count > 1 && Widgets.ButtonText(rect2, "ChooseNewRole".Translate() + "...", true, true, flag, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				if (CS$<>8__locals1.currentRole != null)
				{
					list.Add(new FloatMenuOption("None".Translate(), delegate()
					{
						CS$<>8__locals1.<>4__this.confirmText = "ChooseRoleConfirmUnassign".Translate(CS$<>8__locals1.currentRole.Named("ROLE"), CS$<>8__locals1.pawn.Named("PAWN")) + "\n\n" + "ChooseRoleConfirmAssignPostfix".Translate();
						CS$<>8__locals1.<>4__this.SetRoleToChangeTo(null);
					}, Widgets.PlaceholderIconTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
				}
				using (List<Precept_Role>.Enumerator enumerator = Dialog_BeginRitual.cachedRoles.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Dialog_BeginRitual.<>c__DisplayClass54_1 CS$<>8__locals3 = new Dialog_BeginRitual.<>c__DisplayClass54_1();
						CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals1;
						CS$<>8__locals3.newRole = enumerator.Current;
						if (CS$<>8__locals3.newRole != roleChangeSelection && CS$<>8__locals3.newRole != CS$<>8__locals3.CS$<>8__locals1.currentRole && CS$<>8__locals3.newRole.Active && CS$<>8__locals3.newRole.RequirementsMet(CS$<>8__locals3.CS$<>8__locals1.pawn) && (!CS$<>8__locals3.newRole.def.leaderRole || CS$<>8__locals3.CS$<>8__locals1.pawn.Ideo == Faction.OfPlayer.ideos.PrimaryIdeo))
						{
							string text = CS$<>8__locals3.newRole.LabelForPawn(CS$<>8__locals3.CS$<>8__locals1.pawn) + " (" + CS$<>8__locals3.newRole.def.label + ")";
							TaggedString confirmTextLocal = "ChooseRoleConfirmAssign".Translate(CS$<>8__locals3.newRole.Named("ROLE"), CS$<>8__locals3.CS$<>8__locals1.pawn.Named("PAWN"));
							string extraConfirmText = RitualUtility.RoleChangeConfirmation(CS$<>8__locals3.CS$<>8__locals1.pawn, CS$<>8__locals3.CS$<>8__locals1.currentRole, CS$<>8__locals3.newRole);
							Pawn pawn4 = CS$<>8__locals3.newRole.ChosenPawns().FirstOrDefault<Pawn>();
							if (pawn4 != null && CS$<>8__locals3.newRole is Precept_RoleSingle)
							{
								text = text + ": " + pawn4.LabelShort;
							}
							if (!extraConfirmText.NullOrEmpty())
							{
								list.Add(new FloatMenuOption(text, delegate()
								{
									CS$<>8__locals3.CS$<>8__locals1.<>4__this.confirmText = confirmTextLocal + "\n\n" + extraConfirmText + "\n\n" + "ChooseRoleConfirmAssignPostfix".Translate();
									CS$<>8__locals3.CS$<>8__locals1.<>4__this.SetRoleToChangeTo(CS$<>8__locals3.newRole);
								}, CS$<>8__locals3.newRole.Icon, CS$<>8__locals3.newRole.ideo.Color, MenuOptionPriority.Default, new Action<Rect>(CS$<>8__locals3.<DrawRoleSelection>g__DrawTooltip|1), null, 0f, null, null, true, 0, HorizontalJustification.Left, false)
								{
									orderInPriority = CS$<>8__locals3.newRole.def.displayOrderInImpact
								});
							}
							else
							{
								list.Add(new FloatMenuOption(text, delegate()
								{
									CS$<>8__locals3.newRole.Assign(CS$<>8__locals3.CS$<>8__locals1.pawn, true);
								}, CS$<>8__locals3.newRole.Icon, CS$<>8__locals3.newRole.ideo.Color, MenuOptionPriority.Default, new Action<Rect>(CS$<>8__locals3.<DrawRoleSelection>g__DrawTooltip|1), null, 0f, null, null, true, 0, HorizontalJustification.Left, false)
								{
									orderInPriority = CS$<>8__locals3.newRole.def.displayOrderInImpact
								});
							}
						}
					}
				}
				foreach (Precept_Role precept_Role in Dialog_BeginRitual.cachedRoles)
				{
					if ((precept_Role != roleChangeSelection && !precept_Role.RequirementsMet(CS$<>8__locals1.pawn)) || !precept_Role.Active)
					{
						string text2 = precept_Role.LabelForPawn(CS$<>8__locals1.pawn) + " (" + precept_Role.def.label + ")";
						if (precept_Role.ChosenPawnSingle() != null)
						{
							text2 = text2 + ": " + precept_Role.ChosenPawnSingle().LabelShort;
						}
						else if (!precept_Role.RequirementsMet(CS$<>8__locals1.pawn))
						{
							text2 = text2 + ": " + precept_Role.GetFirstUnmetRequirement(CS$<>8__locals1.pawn).GetLabel(precept_Role).CapitalizeFirst();
						}
						else if (!precept_Role.Active && precept_Role.def.activationBelieverCount > precept_Role.ideo.ColonistBelieverCountCached)
						{
							text2 += ": " + "InactiveRoleRequiresMoreBelievers".Translate(precept_Role.def.activationBelieverCount, precept_Role.ideo.memberName, precept_Role.ideo.ColonistBelieverCountCached).CapitalizeFirst();
						}
						list.Add(new FloatMenuOption(text2, null, precept_Role.Icon, precept_Role.ideo.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false)
						{
							orderInPriority = precept_Role.def.displayOrderInImpact
						});
					}
				}
				if (list.Any<FloatMenuOption>())
				{
					Find.WindowStack.Add(new FloatMenu(list));
				}
			}
			GUI.color = Color.white;
		}

		// Token: 0x06011AF5 RID: 72437 RVA: 0x00516558 File Offset: 0x00514758
		public void DoRoleSelection(ref RectDivider layout)
		{
			RitualRoleIdeoRoleChanger ritualRoleIdeoRoleChanger = this.assignments.AllRolesForReading.OfType<RitualRoleIdeoRoleChanger>().FirstOrDefault<RitualRoleIdeoRoleChanger>();
			if (ritualRoleIdeoRoleChanger != null && Dialog_BeginRitual.cachedRoles.Any<Precept_Role>())
			{
				Pawn pawn = this.assignments.FirstAssignedPawn(ritualRoleIdeoRoleChanger);
				if (pawn != null)
				{
					RectDivider rect = layout.NewRow(40f, VerticalJustification.Top, new float?(10f)).NewCol(320f, HorizontalJustification.Left, null);
					this.DrawRoleSelection(pawn, rect);
				}
			}
		}

		// Token: 0x06011AF6 RID: 72438 RVA: 0x005165D8 File Offset: 0x005147D8
		public override void DoRightColumn(ref RectDivider layout)
		{
			if (this.target.Thing != null)
			{
				Rect targetRow = layout.NewRow(32f, VerticalJustification.Bottom, new float?(28f));
				this.DrawTargetLocation(targetRow);
			}
			this.DoRoleSelection(ref layout);
			base.DoRightColumn(ref layout);
		}

		// Token: 0x06011AF7 RID: 72439 RVA: 0x00516624 File Offset: 0x00514824
		public void DrawTargetLocation(Rect targetRow)
		{
			TaggedString taggedString = "RitualTakesPlaceAt".Translate() + ": ";
			TaggedString taggedString2 = taggedString + this.target.Thing.LabelShortCap;
			float x = Text.CalcSize(taggedString).x;
			float x2 = Text.CalcSize(this.target.Thing.LabelShortCap).x;
			float num = Text.CalcSize(taggedString2).x + 4f + 32f;
			Rect rect = new Rect(targetRow.xMax - (x2 + 4f + 32f), targetRow.y - 6f, 32f, 32f);
			Rect rect2 = new Rect(targetRow.xMax - num, targetRow.y, x, 32f);
			Rect rect3 = new Rect(targetRow.xMax - (x2 + 4f + 32f) + 32f + 4f, targetRow.y, x2, 32f);
			Widgets.Label(rect2, taggedString);
			Widgets.Label(rect3, this.target.Thing.LabelShortCap);
			Widgets.ThingIcon(rect, this.target.Thing, 1f, null, false, 1f, false);
			if (Mouse.IsOver(rect2) || Mouse.IsOver(rect3) || Mouse.IsOver(rect))
			{
				Find.WindowStack.ImmediateWindow(738453, new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight), WindowLayer.Super, delegate
				{
					GenUI.DrawArrowPointingAtWorldspace(this.target.Cell.ToVector3(), Find.Camera);
				}, false, false, 0f, null, false);
			}
		}

		// Token: 0x06011AF8 RID: 72440 RVA: 0x005167CC File Offset: 0x005149CC
		public static RitualRoleAssignments CreateRitualRoleAssignments(Precept_Ritual ritual, TargetInfo target, Map map, Dialog_BeginRitual.PawnFilter filter, List<Pawn> requiredPawns, Dictionary<string, Pawn> forcedForRole, Pawn selectedPawn)
		{
			RitualRoleAssignments ritualRoleAssignments = new RitualRoleAssignments(ritual, target);
			List<Pawn> list = new List<Pawn>(map.mapPawns.FreeColonistsAndPrisonersSpawned);
			List<Pawn> list2 = new List<Pawn>(list.Count);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				Pawn pawn = list[i];
				if (filter != null && !filter(pawn, true, true))
				{
					list.RemoveAt(i);
				}
				else
				{
					bool flag2;
					bool flag = ritualRoleAssignments.PawnNotAssignableReason(pawn, null, out flag2) == null || flag2;
					if (!flag && ritual != null)
					{
						if (pawn.DevelopmentalStage != DevelopmentalStage.Baby && pawn.DevelopmentalStage != DevelopmentalStage.Newborn)
						{
							list2.AddUnique(pawn);
						}
						foreach (RitualRole ritualRole in ritual.behavior.def.roles)
						{
							if ((ritualRoleAssignments.PawnNotAssignableReason(pawn, ritualRole, out flag2) == null || flag2) && (filter == null || filter(pawn, !(ritualRole is RitualRoleForced), ritualRole.allowOtherIdeos)) && (ritualRole.maxCount > 1 || forcedForRole == null || !forcedForRole.ContainsKey(ritualRole.id)))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						list.RemoveAt(i);
					}
				}
			}
			if (requiredPawns != null)
			{
				foreach (Pawn obj in requiredPawns)
				{
					list.AddUnique(obj);
				}
			}
			if (forcedForRole != null)
			{
				foreach (KeyValuePair<string, Pawn> keyValuePair in forcedForRole)
				{
					list.AddUnique(keyValuePair.Value);
				}
			}
			if (ritual != null)
			{
				using (List<RitualRole>.Enumerator enumerator = ritual.behavior.def.roles.GetEnumerator())
				{
					Func<Pawn, bool> <>9__0;
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Animal)
						{
							List<Pawn> list3 = list;
							IEnumerable<Pawn> spawnedColonyAnimals = map.mapPawns.SpawnedColonyAnimals;
							Func<Pawn, bool> predicate;
							if ((predicate = <>9__0) == null)
							{
								predicate = (<>9__0 = ((Pawn p) => filter == null || filter(p, true, true)));
							}
							list3.AddRange(spawnedColonyAnimals.Where(predicate));
							break;
						}
					}
				}
			}
			ritualRoleAssignments.Setup(list, list2, forcedForRole, requiredPawns, selectedPawn);
			return ritualRoleAssignments;
		}

		// Token: 0x06011AFC RID: 72444 RVA: 0x00516AA3 File Offset: 0x00514CA3
		[CompilerGenerated]
		private void <Start>g__InnerStart|40_0()
		{
			Dialog_BeginRitual.ActionCallback actionCallback = this.action;
			if (actionCallback != null && actionCallback(this.assignments))
			{
				this.Close(true);
			}
		}

		// Token: 0x0400B612 RID: 46610
		protected RitualRoleAssignments assignments;

		// Token: 0x0400B613 RID: 46611
		protected Precept_Ritual ritual;

		// Token: 0x0400B614 RID: 46612
		protected TargetInfo target;

		// Token: 0x0400B615 RID: 46613
		protected RitualObligation obligation;

		// Token: 0x0400B616 RID: 46614
		protected RitualOutcomeEffectDef outcome;

		// Token: 0x0400B617 RID: 46615
		protected Dialog_BeginRitual.PawnFilter filter;

		// Token: 0x0400B618 RID: 46616
		protected Pawn organizer;

		// Token: 0x0400B619 RID: 46617
		protected Map map;

		// Token: 0x0400B61A RID: 46618
		protected Dialog_BeginRitual.ActionCallback action;

		// Token: 0x0400B61B RID: 46619
		protected List<string> extraInfos;

		// Token: 0x0400B61C RID: 46620
		private string sleepingMessage;

		// Token: 0x0400B61D RID: 46621
		protected string ritualExplanation;

		// Token: 0x0400B61E RID: 46622
		protected string ritualLabel;

		// Token: 0x0400B61F RID: 46623
		protected string okButtonText;

		// Token: 0x0400B620 RID: 46624
		protected string confirmText;

		// Token: 0x0400B621 RID: 46625
		private static List<Precept_Role> cachedRoles = new List<Precept_Role>();

		// Token: 0x0400B622 RID: 46626
		protected const float RoleHeight = 40f;

		// Token: 0x0400B623 RID: 46627
		protected const float TargetIconSize = 32f;

		// Token: 0x02003126 RID: 12582
		// (Invoke) Token: 0x06011B00 RID: 72448
		public delegate bool ActionCallback(RitualRoleAssignments assignments);

		// Token: 0x02003127 RID: 12583
		// (Invoke) Token: 0x06011B04 RID: 72452
		public delegate bool PawnFilter(Pawn pawn, bool voluntary, bool allowOtherIdeos);
	}
}
