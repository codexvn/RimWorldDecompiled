using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200267D RID: 9853
	[StaticConstructorOnStartup]
	public class Precept_Ritual : Precept
	{
		// Token: 0x1700212C RID: 8492
		// (get) Token: 0x0600D850 RID: 55376 RVA: 0x00401173 File Offset: 0x003FF373
		public float RepeatQualityPenalty
		{
			get
			{
				return Mathf.Lerp(-0.95f, 0f, this.RepeatPenaltyProgress);
			}
		}

		// Token: 0x1700212D RID: 8493
		// (get) Token: 0x0600D851 RID: 55377 RVA: 0x0040118C File Offset: 0x003FF38C
		public override string UIInfoFirstLine
		{
			get
			{
				if (!this.ShortDescOverrideCap.NullOrEmpty())
				{
					return this.ShortDescOverrideCap;
				}
				return this.def.LabelCap.Resolve();
			}
		}

		// Token: 0x1700212E RID: 8494
		// (get) Token: 0x0600D852 RID: 55378 RVA: 0x004008D2 File Offset: 0x003FEAD2
		public override string UIInfoSecondLine
		{
			get
			{
				return base.LabelCap;
			}
		}

		// Token: 0x1700212F RID: 8495
		// (get) Token: 0x0600D853 RID: 55379 RVA: 0x004011C0 File Offset: 0x003FF3C0
		public bool RepeatPenaltyActive
		{
			get
			{
				return this.isAnytime && this.lastFinishedTick != -1 && this.def.useRepeatPenalty && this.TicksSinceLastPerformed < 1200000;
			}
		}

		// Token: 0x17002130 RID: 8496
		// (get) Token: 0x0600D854 RID: 55380 RVA: 0x004011EF File Offset: 0x003FF3EF
		public int TicksSinceLastPerformed
		{
			get
			{
				return GenTicks.TicksGame - this.lastFinishedTick;
			}
		}

		// Token: 0x17002131 RID: 8497
		// (get) Token: 0x0600D855 RID: 55381 RVA: 0x004011FD File Offset: 0x003FF3FD
		public float RepeatPenaltyProgress
		{
			get
			{
				return (float)this.TicksSinceLastPerformed / 1200000f;
			}
		}

		// Token: 0x17002132 RID: 8498
		// (get) Token: 0x0600D856 RID: 55382 RVA: 0x0040120C File Offset: 0x003FF40C
		public string RepeatPenaltyTimeLeft
		{
			get
			{
				return (1200000 - this.TicksSinceLastPerformed).ToStringTicksToPeriod(true, false, true, true, false);
			}
		}

		// Token: 0x17002133 RID: 8499
		// (get) Token: 0x0600D857 RID: 55383 RVA: 0x00401224 File Offset: 0x003FF424
		public bool SupportsAttachableOutcomeEffect
		{
			get
			{
				RitualOutcomeEffectWorker ritualOutcomeEffectWorker = this.outcomeEffect;
				return ritualOutcomeEffectWorker != null && ritualOutcomeEffectWorker.SupportsAttachableOutcomeEffect;
			}
		}

		// Token: 0x17002134 RID: 8500
		// (get) Token: 0x0600D858 RID: 55384 RVA: 0x00401237 File Offset: 0x003FF437
		public bool IsDateTriggered
		{
			get
			{
				if (this.obligationTriggers != null)
				{
					return this.obligationTriggers.Any((RitualObligationTrigger o) => o is RitualObligationTrigger_Date);
				}
				return false;
			}
		}

		// Token: 0x17002135 RID: 8501
		// (get) Token: 0x0600D859 RID: 55385 RVA: 0x0040126D File Offset: 0x003FF46D
		public string ShortDescOverrideCap
		{
			get
			{
				if (this.shortDescOverrideCap.NullOrEmpty() && this.shortDescOverride != null)
				{
					this.shortDescOverrideCap = this.shortDescOverride.CapitalizeFirst();
				}
				return this.shortDescOverrideCap;
			}
		}

		// Token: 0x17002136 RID: 8502
		// (get) Token: 0x0600D85A RID: 55386 RVA: 0x0040129C File Offset: 0x003FF49C
		public override Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					string text = this.iconPathOverride ?? this.def.iconPath;
					if (text != null)
					{
						this.icon = ContentFinder<Texture2D>.Get(text, true);
					}
					else
					{
						this.icon = this.ideo.Icon;
					}
				}
				return this.icon;
			}
		}

		// Token: 0x17002137 RID: 8503
		// (get) Token: 0x0600D85B RID: 55387 RVA: 0x004012F8 File Offset: 0x003FF4F8
		public virtual Texture2D CancelIcon
		{
			get
			{
				if (this.cancelIcon == null)
				{
					string text = this.cancelIconPathOverride ?? this.def.iconPath;
					if (text != null)
					{
						this.cancelIcon = ContentFinder<Texture2D>.Get(text, true);
					}
					else
					{
						this.cancelIcon = this.ideo.Icon;
					}
				}
				return this.cancelIcon;
			}
		}

		// Token: 0x17002138 RID: 8504
		// (get) Token: 0x0600D85C RID: 55388 RVA: 0x00401354 File Offset: 0x003FF554
		public override string TipLabel
		{
			get
			{
				if (this.tipLabelCached == null)
				{
					this.tipLabelCached = ((!this.def.tipLabelOverride.NullOrEmpty()) ? this.def.tipLabelOverride : base.LabelCap);
					if (!this.ShortDescOverrideCap.NullOrEmpty() && this.ShortDescOverrideCap != this.tipLabelCached)
					{
						this.tipLabelCached = this.tipLabelCached + "\n" + this.ShortDescOverrideCap;
					}
					if (this.isAnytime)
					{
						this.tipLabelCached += "\n" + "RitualStartAnyTime".Translate();
					}
					else
					{
						RitualObligationTrigger_Date ritualObligationTrigger_Date = this.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>();
						if (ritualObligationTrigger_Date != null)
						{
							this.tipLabelCached = this.tipLabelCached + "\n" + ritualObligationTrigger_Date.DateString;
						}
						else
						{
							this.tipLabelCached += "\n" + "RitualStartEvent".Translate();
						}
					}
				}
				return this.tipLabelCached;
			}
		}

		// Token: 0x17002139 RID: 8505
		// (get) Token: 0x0600D85D RID: 55389 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool UsesGeneratedName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700213A RID: 8506
		// (get) Token: 0x0600D85E RID: 55390 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanRegenerate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700213B RID: 8507
		// (get) Token: 0x0600D85F RID: 55391 RVA: 0x00401469 File Offset: 0x003FF669
		public static Texture2D DateRitualTex
		{
			get
			{
				if (Precept_Ritual.dayRitualTex == null)
				{
					Precept_Ritual.dayRitualTex = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/DateRitual", true);
				}
				return Precept_Ritual.dayRitualTex;
			}
		}

		// Token: 0x1700213C RID: 8508
		// (get) Token: 0x0600D860 RID: 55392 RVA: 0x0040148D File Offset: 0x003FF68D
		public static Texture2D AnytimeRitualTex
		{
			get
			{
				if (Precept_Ritual.anytimeRitualTex == null)
				{
					Precept_Ritual.anytimeRitualTex = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/AnytimeRitual", true);
				}
				return Precept_Ritual.anytimeRitualTex;
			}
		}

		// Token: 0x1700213D RID: 8509
		// (get) Token: 0x0600D861 RID: 55393 RVA: 0x004014B1 File Offset: 0x003FF6B1
		public static Texture2D EventRitualTex
		{
			get
			{
				if (Precept_Ritual.eventRitualTex == null)
				{
					Precept_Ritual.eventRitualTex = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/EventRitual", true);
				}
				return Precept_Ritual.eventRitualTex;
			}
		}

		// Token: 0x0600D862 RID: 55394 RVA: 0x004014D8 File Offset: 0x003FF6D8
		public TaggedString GetBeginRitualText(RitualObligation obligation = null)
		{
			RitualObligationTargetFilter ritualObligationTargetFilter = this.obligationTargetFilter;
			string text = ((ritualObligationTargetFilter != null) ? ritualObligationTargetFilter.LabelExtraPart(obligation) : null) ?? "";
			if (!string.IsNullOrEmpty(this.def.ritualPatternBase.beginRitualOverride))
			{
				return new TaggedString(this.def.ritualPatternBase.beginRitualOverride).Formatted(this.Label, text);
			}
			if (text.NullOrEmpty() || this.mergeGizmosForObligations)
			{
				return "BeginRitual".Translate(this.Label);
			}
			return "BeginRitualFor".Translate(this.Label, text);
		}

		// Token: 0x0600D863 RID: 55395 RVA: 0x00401586 File Offset: 0x003FF786
		public override void ClearTipCache()
		{
			base.ClearTipCache();
			this.tipLabelCached = null;
		}

		// Token: 0x0600D864 RID: 55396 RVA: 0x00401595 File Offset: 0x003FF795
		public override string GetTip()
		{
			return this.TipMainPart() + this.TipExtraPart();
		}

		// Token: 0x0600D865 RID: 55397 RVA: 0x004015A8 File Offset: 0x003FF7A8
		public string TipMainPart()
		{
			Precept.tmpCompsDesc.Clear();
			if (this.RepeatPenaltyActive)
			{
				float value = (float)Mathf.RoundToInt(this.RepeatPenaltyProgress * 20f * 10f) / 10f;
				float value2 = (float)Mathf.RoundToInt((1f - this.RepeatPenaltyProgress) * 20f * 10f) / 10f;
				Precept.tmpCompsDesc.AppendLine(base.ColorizeWarning("RitualRepeatPenaltyTip".Translate(20, value, this.RepeatQualityPenalty.ToStringPercent(), value2)));
				Precept.tmpCompsDesc.AppendLine();
			}
			if (!this.DescriptionForTip.NullOrEmpty())
			{
				string value3 = this.DescriptionForTip;
				RitualBehaviorWorker ritualBehaviorWorker = this.behavior;
				if (((ritualBehaviorWorker != null) ? ritualBehaviorWorker.descriptionOverride : null) != null)
				{
					RitualBehaviorWorker ritualBehaviorWorker2 = this.behavior;
					value3 = ((ritualBehaviorWorker2 != null) ? ritualBehaviorWorker2.descriptionOverride : null);
				}
				Precept.tmpCompsDesc.Append(value3);
			}
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				if (ritualObligationTrigger.TriggerExtraDesc != null)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(ritualObligationTrigger.TriggerExtraDesc);
				}
			}
			if (this.outcomeEffect != null)
			{
				StringBuilder stringBuilder = new StringBuilder(this.outcomeEffect.Description);
				if (!this.outcomeEffect.def.extraPredictedOutcomeDescriptions.NullOrEmpty<string>())
				{
					foreach (string str in this.outcomeEffect.def.extraPredictedOutcomeDescriptions)
					{
						stringBuilder.Append(" " + str.Formatted(this.shortDescOverride ?? this.def.label));
					}
				}
				if (this.attachableOutcomeEffect != null)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendInNewLine(this.attachableOutcomeEffect.DescriptionForRitualValidated(this));
				}
				if (stringBuilder.Length > 0)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(stringBuilder.ToString());
				}
			}
			return Precept.tmpCompsDesc.ToString();
		}

		// Token: 0x0600D866 RID: 55398 RVA: 0x00401818 File Offset: 0x003FFA18
		public string TipExtraPart()
		{
			this.tmpTipExtraSb.Clear();
			if (this.obligationTargetFilter != null && this.obligationTargetFilter.GetTargetInfos(null).Any<string>())
			{
				this.tmpTipExtraSb.AppendLine();
				this.tmpTipExtraSb.AppendInNewLine(base.ColorizeDescTitle("RitualFocusObjects".Translate() + ":"));
				this.tmpTipExtraSb.AppendInNewLine(this.obligationTargetFilter.GetTargetInfos(null).ToLineList("  - ", true));
			}
			string text = this.RolesDescription();
			if (text != null)
			{
				this.tmpTipExtraSb.Append(base.ColorizeDescTitle("\n\n" + "RitualParticipatingRoles".Translate() + ":" + "\n"));
				this.tmpTipExtraSb.Append(text);
			}
			if (this.ideo.Fluid)
			{
				SimpleCurve developmentPointsOverOutcomeIndexCurveForRitual = IdeoDevelopmentUtility.GetDevelopmentPointsOverOutcomeIndexCurveForRitual(this.ideo, this);
				if (developmentPointsOverOutcomeIndexCurveForRitual != null)
				{
					float y = developmentPointsOverOutcomeIndexCurveForRitual.MinBy((CurvePoint p) => p.y).y;
					float y2 = developmentPointsOverOutcomeIndexCurveForRitual.MaxBy((CurvePoint p) => p.y).y;
					this.tmpTipExtraSb.Append(base.ColorizeDescTitle("\n\n" + "RitualDevelopmentPoints".Translate() + ":" + "\n"));
					this.tmpTipExtraSb.Append("RitualDevelopmentPointsTip".Translate(y, y2));
				}
			}
			return this.tmpTipExtraSb.ToString();
		}

		// Token: 0x0600D867 RID: 55399 RVA: 0x004019E0 File Offset: 0x003FFBE0
		public string RolesDescription()
		{
			if (this.behavior.def.roles.NullOrEmpty<RitualRole>())
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (RitualRole ritualRole in this.behavior.def.roles)
			{
				Precept_Role precept_Role = ritualRole.FindInstance(this.ideo);
				TaggedString t = ritualRole.LabelCap;
				if (precept_Role != null)
				{
					t = precept_Role.LabelCap;
					if (precept_Role != null)
					{
						PreceptDef def = precept_Role.def;
						bool? flag = (def != null) ? new bool?(def.leaderRole) : null;
						bool flag2 = true;
						if (flag.GetValueOrDefault() == flag2 & flag != null)
						{
							Pawn pawn = precept_Role.ChosenPawnSingle();
							if (pawn != null)
							{
								t = precept_Role.LabelForPawn(pawn).CapitalizeFirst();
							}
						}
					}
				}
				stringBuilder.AppendInNewLine("  - " + t + " (" + ((ritualRole.required && !ritualRole.substitutable) ? "Required".Translate().ToLower() : "Optional".Translate()) + ")");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600D868 RID: 55400 RVA: 0x00401B44 File Offset: 0x003FFD44
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, null);
			base.RegenerateName();
		}

		// Token: 0x0600D869 RID: 55401 RVA: 0x00401B54 File Offset: 0x003FFD54
		public override void PostMake()
		{
			this.sourcePattern = this.def.ritualPatternBase;
		}

		// Token: 0x0600D86A RID: 55402 RVA: 0x00401B68 File Offset: 0x003FFD68
		public override string GenerateNameRaw()
		{
			if (this.nameMaker == null)
			{
				return this.def.label;
			}
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(this.nameMaker);
			base.AddIdeoRulesTo(ref request);
			if (!request.Rules.Any<Rule>())
			{
				return this.def.label;
			}
			if (this.ideo.culture.festivalNameMaker != null)
			{
				request.Includes.Add(this.ideo.culture.festivalNameMaker);
			}
			List<string> list = new List<string>();
			string text = GrammarResolver.Resolve("r_ritualName", request, null, false, null, null, list, false);
			this.usesDefiniteArticle = !list.Contains("noArticle");
			if (this.def.capitalizeAsTitle)
			{
				text = GenText.CapitalizeAsTitle(text);
			}
			return text;
		}

		// Token: 0x0600D86B RID: 55403 RVA: 0x00401C34 File Offset: 0x003FFE34
		public void AddObligation(RitualObligation obligation)
		{
			if (!this.ideo.ObligationsActive && !this.def.allowOptionalRitualObligations)
			{
				return;
			}
			if (!ModLister.CheckIdeology("Ritual obligations"))
			{
				return;
			}
			bool flag = this.def.notifyPlayerOnOpportunity && (float)Find.TickManager.TicksGame >= 60000f * this.def.skipOpportunityLettersBeforeDay;
			foreach (RitualRole ritualRole in this.behavior.def.RequiredRoles())
			{
				Precept_Role precept_Role = ritualRole.FindInstance(this.ideo);
				if (precept_Role != null && !precept_Role.Active)
				{
					if (flag)
					{
						RitualObligationTrigger_Date ritualObligationTrigger_Date = this.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>();
						if (ritualObligationTrigger_Date != null)
						{
							Find.LetterStack.ReceiveLetter("LetterObligationRoleInactive".Translate(base.LabelCap), "LetterObligationRoleInactiveDateDesc".Translate(ritualObligationTrigger_Date.DateString, base.LabelCap, this.ideo.memberName, precept_Role.Named("ROLE")), LetterDefOf.NeutralEvent, null, 0, true);
						}
					}
					return;
				}
			}
			if (this.activeObligations == null)
			{
				this.activeObligations = new List<RitualObligation>();
			}
			if (this.completedObligations == null)
			{
				this.completedObligations = new List<RitualObligation>();
			}
			if (obligation.onlyForPawns == null)
			{
				obligation.onlyForPawns = new List<Pawn>();
			}
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				if (pawn.Ideo == this.ideo)
				{
					obligation.onlyForPawns.Add(pawn);
				}
			}
			this.activeObligations.Add(obligation);
			if (obligation.sendLetter && flag && this.def.notifyPlayerOnOpportunity)
			{
				LookTargets lookTargets = LookTargets.Invalid;
				TargetInfo firstValidTarget = obligation.FirstValidTarget;
				Map map = Find.AnyPlayerHomeMap;
				if (firstValidTarget.IsValid)
				{
					map = firstValidTarget.Map;
				}
				if (map != null)
				{
					IEnumerable<TargetInfo> targets = this.obligationTargetFilter.GetTargets(obligation, map);
					if (targets.Any<TargetInfo>())
					{
						lookTargets = targets.First<TargetInfo>();
					}
					else
					{
						lookTargets = firstValidTarget;
					}
				}
				Find.LetterStack.ReceiveLetter(obligation.LetterLabel, obligation.LetterText.Resolve(), LetterDefOf.NeutralEvent, lookTargets, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600D86C RID: 55404 RVA: 0x00401ECC File Offset: 0x004000CC
		public void RemoveObligation(RitualObligation obligation, bool completed = false)
		{
			this.activeObligations.Remove(obligation);
			if (completed)
			{
				this.completedObligations.Add(obligation);
			}
		}

		// Token: 0x0600D86D RID: 55405 RVA: 0x00401EEC File Offset: 0x004000EC
		public bool ShouldShowGizmo(TargetInfo target)
		{
			if (this.activeObligations != null)
			{
				for (int i = 0; i < this.activeObligations.Count; i++)
				{
					if (this.obligationTargetFilter.CanUseTarget(target, this.activeObligations[i]).ShouldShowGizmo)
					{
						return true;
					}
				}
			}
			return (this.obligationTriggers.FirstOrDefault((RitualObligationTrigger o) => o is RitualObligationTrigger_Date) != null || this.isAnytime) && this.obligationTargetFilter.CanUseTarget(target, null).ShouldShowGizmo;
		}

		// Token: 0x0600D86E RID: 55406 RVA: 0x00401F86 File Offset: 0x00400186
		public RitualTargetUseReport CanUseTarget(TargetInfo target, RitualObligation obligation)
		{
			return this.obligationTargetFilter.CanUseTarget(target, obligation);
		}

		// Token: 0x0600D86F RID: 55407 RVA: 0x00401F98 File Offset: 0x00400198
		public override void Tick()
		{
			base.Tick();
			if (!this.activeObligations.NullOrEmpty<RitualObligation>())
			{
				for (int i = this.activeObligations.Count - 1; i >= 0; i--)
				{
					if (!this.activeObligations[i].StillValid || !this.obligationTargetFilter.ObligationTargetsValid(this.activeObligations[i]))
					{
						this.RemoveObligation(this.activeObligations[i], false);
					}
				}
			}
			for (int j = 0; j < this.obligationTriggers.Count; j++)
			{
				try
				{
					this.obligationTriggers[j].Tick();
				}
				catch (Exception ex)
				{
					string str = "Error while ticking a ritual obligation trigger: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x0600D870 RID: 55408 RVA: 0x00402068 File Offset: 0x00400268
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			yield return base.EditFloatMenuOption();
			yield break;
		}

		// Token: 0x0600D871 RID: 55409 RVA: 0x00402078 File Offset: 0x00400278
		public override void Notify_MemberDied(Pawn p)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberDied(p);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberDied(p);
			}
		}

		// Token: 0x0600D872 RID: 55410 RVA: 0x004020DC File Offset: 0x004002DC
		public override void Notify_MemberCorpseDestroyed(Pawn p)
		{
			if (!ModsConfig.IdeologyActive || Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberCorpseDestroyed(p);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberCorpseDestroyed(p);
			}
		}

		// Token: 0x0600D873 RID: 55411 RVA: 0x00402144 File Offset: 0x00400344
		public override void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberChangedFaction(p, oldFaction, newFaction);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberChangedFaction(p, oldFaction, newFaction);
			}
		}

		// Token: 0x0600D874 RID: 55412 RVA: 0x004021AC File Offset: 0x004003AC
		public override void Notify_MemberLost(Pawn pawn)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberLost(pawn);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberLost(pawn);
			}
		}

		// Token: 0x0600D875 RID: 55413 RVA: 0x00402210 File Offset: 0x00400410
		public override void Notify_MemberGained(Pawn pawn)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberGained(pawn);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberGained(pawn);
			}
		}

		// Token: 0x0600D876 RID: 55414 RVA: 0x00402274 File Offset: 0x00400474
		public override void Notify_MemberGenerated(Pawn pawn, bool newborn, bool ignoreApparel = false)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberGenerated(pawn, newborn, ignoreApparel);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberGenerated(pawn);
			}
		}

		// Token: 0x0600D877 RID: 55415 RVA: 0x004022D8 File Offset: 0x004004D8
		public override void Notify_MemberGuestStatusChanged(Pawn pawn)
		{
			base.Notify_MemberGuestStatusChanged(pawn);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberGuestStatusChanged(pawn);
			}
		}

		// Token: 0x0600D878 RID: 55416 RVA: 0x00402330 File Offset: 0x00400530
		public override void Notify_GameStarted()
		{
			base.Notify_GameStarted();
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_GameStarted();
			}
		}

		// Token: 0x0600D879 RID: 55417 RVA: 0x00402388 File Offset: 0x00400588
		public override void Notify_IdeoReformed()
		{
			base.Notify_IdeoReformed();
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_IdeoReformed();
			}
		}

		// Token: 0x0600D87A RID: 55418 RVA: 0x004023E0 File Offset: 0x004005E0
		public IEnumerable<Gizmo> GetGizmoFor(TargetInfo t)
		{
			Precept_Ritual.<>c__DisplayClass105_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.t = t;
			if (!this.allowOtherInstances)
			{
				using (List<LordJob_Ritual>.Enumerator enumerator = Find.IdeoManager.GetActiveRituals(CS$<>8__locals1.t.Map).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Ritual == this)
						{
							yield break;
						}
					}
				}
			}
			if (!this.activeObligations.NullOrEmpty<RitualObligation>())
			{
				foreach (RitualObligation obligation in this.activeObligations)
				{
					Command_Ritual command_Ritual = this.<GetGizmoFor>g__CommandForObligation|105_0(obligation, ref CS$<>8__locals1);
					if (command_Ritual != null)
					{
						yield return command_Ritual;
						if (this.mergeGizmosForObligations)
						{
							break;
						}
					}
				}
				List<RitualObligation>.Enumerator enumerator2 = default(List<RitualObligation>.Enumerator);
			}
			else if (this.isAnytime)
			{
				RitualTargetUseReport ritualTargetUseReport = this.CanUseTarget(CS$<>8__locals1.t, null);
				Command_Ritual command_Ritual2 = new Command_Ritual(this, CS$<>8__locals1.t, null, null);
				if (!ritualTargetUseReport.failReason.NullOrEmpty())
				{
					command_Ritual2.disabledReason = ritualTargetUseReport.failReason;
					command_Ritual2.Disabled = true;
				}
				yield return command_Ritual2;
			}
			else
			{
				List<RitualObligationTrigger> list = this.obligationTriggers;
				RitualObligationTrigger ritualObligationTrigger;
				if (list == null)
				{
					ritualObligationTrigger = null;
				}
				else
				{
					ritualObligationTrigger = list.FirstOrDefault((RitualObligationTrigger o) => o is RitualObligationTrigger_Date);
				}
				RitualObligationTrigger ritualObligationTrigger2 = ritualObligationTrigger;
				if (ritualObligationTrigger2 != null)
				{
					RitualObligationTrigger_Date ritualObligationTrigger_Date = (RitualObligationTrigger_Date)ritualObligationTrigger2;
					int num = ritualObligationTrigger_Date.OccursOnTick();
					int num2 = ritualObligationTrigger_Date.CurrentTickRelative();
					if (num2 > num)
					{
						num += 3600000;
					}
					yield return new Command_Ritual(this, CS$<>8__locals1.t, null, null)
					{
						disabledReason = "DateRitualNoObligation".Translate(base.LabelCap, (num - num2).ToStringTicksToPeriod(true, false, true, true, false), ritualObligationTrigger_Date.DateString).Resolve(),
						Disabled = true
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600D87B RID: 55419 RVA: 0x004023F8 File Offset: 0x004005F8
		public void ShowRitualBeginWindow(TargetInfo targetInfo, RitualObligation forObligation = null, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			RitualObligation ritualObligation = forObligation;
			if (ritualObligation == null)
			{
				ritualObligation = ((this.activeObligations != null) ? this.activeObligations.FirstOrDefault((RitualObligation o) => this.obligationTargetFilter.CanUseTarget(targetInfo, o).canUse) : null);
			}
			Window ritualBeginWindow = this.GetRitualBeginWindow(targetInfo, ritualObligation, null, null, forcedForRole, selectedPawn);
			if (ritualBeginWindow != null)
			{
				Find.WindowStack.Add(ritualBeginWindow);
			}
		}

		// Token: 0x0600D87C RID: 55420 RVA: 0x00402464 File Offset: 0x00400664
		public Window GetRitualBeginWindow(TargetInfo targetInfo, RitualObligation obligation = null, Action onConfirm = null, Pawn organizer = null, Dictionary<string, Pawn> forcedForRole = null, Pawn selectedPawn = null)
		{
			string text = this.behavior.CanStartRitualNow(targetInfo, this, selectedPawn, forcedForRole);
			if (!string.IsNullOrEmpty(text))
			{
				Messages.Message(text, targetInfo, MessageTypeDefOf.RejectInput, false);
			}
			List<string> list = new List<string>();
			if (this.outcomeEffect != null)
			{
				if (!this.outcomeEffect.def.extraInfoLines.NullOrEmpty<string>())
				{
					foreach (string item in this.outcomeEffect.def.extraInfoLines)
					{
						list.Add(item);
					}
				}
				if (!this.outcomeEffect.def.extraPredictedOutcomeDescriptions.NullOrEmpty<string>())
				{
					foreach (string str in this.outcomeEffect.def.extraPredictedOutcomeDescriptions)
					{
						list.Add(str.Formatted(this.shortDescOverride ?? this.def.label));
					}
				}
				if (this.attachableOutcomeEffect != null)
				{
					list.Add(this.attachableOutcomeEffect.DescriptionForRitualValidated(this, targetInfo.Map));
				}
			}
			string ritualLabel = this.Label.CapitalizeFirst();
			TargetInfo targetInfo2 = targetInfo;
			Map map = targetInfo.Map;
			Dialog_BeginRitual.ActionCallback action = delegate(RitualRoleAssignments assignments)
			{
				this.behavior.TryExecuteOn(targetInfo, organizer, this, obligation, assignments, true);
				Action onConfirm2 = onConfirm;
				if (onConfirm2 != null)
				{
					onConfirm2();
				}
				return true;
			};
			Pawn organizer2 = organizer;
			RitualObligation obligation2 = obligation;
			Dialog_BeginRitual.PawnFilter filter = (Pawn pawn, bool voluntary, bool allowOtherIdeos) => pawn.GetLord() == null && (!pawn.RaceProps.Animal || this.behavior.def.roles.Any(delegate(RitualRole r)
			{
				string text2;
				return r.AppliesToPawn(pawn, out text2, targetInfo, null, null, null, true);
			})) && !pawn.IsSubhuman && (!this.ritualOnlyForIdeoMembers || this.def.allowSpectatorsFromOtherIdeos || (pawn.Ideo == this.ideo || !voluntary || allowOtherIdeos) || pawn.IsPrisonerOfColony || pawn.RaceProps.Animal || (!forcedForRole.NullOrEmpty<string, Pawn>() && forcedForRole.ContainsValue(pawn)));
			string okButtonText = "Begin".Translate();
			object requiredPawns;
			if (organizer == null)
			{
				requiredPawns = null;
			}
			else
			{
				(requiredPawns = new List<Pawn>()).Add(organizer);
			}
			List<string> extraInfoText = list;
			return new Dialog_BeginRitual(ritualLabel, this, targetInfo2, map, action, organizer2, obligation2, filter, okButtonText, requiredPawns, forcedForRole, null, extraInfoText, selectedPawn);
		}

		// Token: 0x0600D87D RID: 55421 RVA: 0x00402684 File Offset: 0x00400884
		public void Notify_CooldownFromAbilityStarted(int cooldown)
		{
			this.abilityOnCooldownUntilTick = Find.TickManager.TicksGame + cooldown;
		}

		// Token: 0x0600D87E RID: 55422 RVA: 0x00402698 File Offset: 0x00400898
		public override void DrawIcon(Rect rect)
		{
			GUI.color = this.ideo.Color;
			GUI.DrawTexture(rect, this.Icon);
			GUI.color = Color.white;
		}

		// Token: 0x0600D87F RID: 55423 RVA: 0x004026C0 File Offset: 0x004008C0
		public override void DrawPreceptBox(Rect preceptBox, IdeoEditMode editMode, bool forceHighlight = false)
		{
			base.DrawPreceptBox(preceptBox, editMode, forceHighlight);
			Rect position = new Rect(preceptBox.xMax - (float)this.DateIconSize.x, preceptBox.yMin, (float)this.DateIconSize.x, (float)this.DateIconSize.z);
			if (this.isAnytime)
			{
				GUI.DrawTexture(position, Precept_Ritual.AnytimeRitualTex);
				return;
			}
			if (this.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>() != null)
			{
				GUI.DrawTexture(position, Precept_Ritual.DateRitualTex);
				return;
			}
			GUI.DrawTexture(position, Precept_Ritual.EventRitualTex);
		}

		// Token: 0x0600D880 RID: 55424 RVA: 0x003FC7B1 File Offset: 0x003FA9B1
		public override void Notify_RecachedPrecepts()
		{
			this.tipCached = null;
		}

		// Token: 0x0600D881 RID: 55425 RVA: 0x00402750 File Offset: 0x00400950
		public override bool TryGetLostByReformingWarning(out string warning)
		{
			warning = null;
			IEnumerable<LordJob_Ritual> activeRitualsForPrecept = RitualUtility.GetActiveRitualsForPrecept(this);
			if (activeRitualsForPrecept.Any<LordJob_Ritual>())
			{
				foreach (LordJob_Ritual lordJob_Ritual in activeRitualsForPrecept)
				{
					if (!warning.NullOrEmpty())
					{
						warning += "\n\n";
					}
					warning += "ReformIdeoCancelRituals".Translate(lordJob_Ritual.RitualLabel);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600D882 RID: 55426 RVA: 0x004027E0 File Offset: 0x004009E0
		public override void Notify_RemovedByReforming()
		{
			this.tmpActiveRituals.Clear();
			this.tmpActiveRituals.AddRange(RitualUtility.GetActiveRitualsForPrecept(this));
			for (int i = 0; i < this.tmpActiveRituals.Count; i++)
			{
				this.tmpActiveRituals[i].Cancel();
			}
			this.tmpActiveRituals.Clear();
		}

		// Token: 0x0600D883 RID: 55427 RVA: 0x0040283C File Offset: 0x00400A3C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.nameMaker, "nameMaker");
			Scribe_Values.Look<bool>(ref this.ritualOnlyForIdeoMembers, "ritualOnlyForIdeoMembers", false, false);
			Scribe_Values.Look<string>(ref this.ritualExpectedDesc, "ritualExpectedDesc", null, false);
			Scribe_Values.Look<string>(ref this.ritualExpectedDescNoAdjective, "ritualExpectedDescNoAdjective", null, false);
			Scribe_Values.Look<string>(ref this.shortDescOverride, "shortDescOverride", null, false);
			Scribe_Values.Look<string>(ref this.iconPathOverride, "iconPathOverride", null, false);
			Scribe_Values.Look<string>(ref this.patternGroupTag, "patternGroupTag", null, false);
			Scribe_Values.Look<TechLevel>(ref this.minTechLevel, "minTechLevel", TechLevel.Undefined, false);
			Scribe_Values.Look<TechLevel>(ref this.maxTechLevel, "maxTechLevel", TechLevel.Undefined, false);
			Scribe_Values.Look<bool>(ref this.isAnytime, "isAnytime", false, false);
			Scribe_Values.Look<bool>(ref this.canBeAnytime, "canBeAnytime", false, false);
			Scribe_Values.Look<bool>(ref this.allowOtherInstances, "allowOtherInstances", false, false);
			Scribe_Values.Look<bool>(ref this.playsIdeoMusic, "playsIdeoMusic", false, false);
			Scribe_Values.Look<string>(ref this.ritualExplanation, "ritualExplanation", null, false);
			Scribe_Values.Look<bool>(ref this.mergeGizmosForObligations, "mergeGizmosForObligations", false, false);
			Scribe_Values.Look<bool>(ref this.canMergeGizmosFromDifferentIdeos, "canMergeGizmosFromDifferentIdeos", true, false);
			Scribe_Values.Look<bool>(ref this.generatedAttachedReward, "generatedAttachedReward", false, false);
			Scribe_Defs.Look<RitualPatternDef>(ref this.sourcePattern, "sourcePattern");
			Scribe_Deep.Look<RitualObligationTargetFilter>(ref this.obligationTargetFilter, "obligationTargetFilter", Array.Empty<object>());
			Scribe_Deep.Look<RitualTargetFilter>(ref this.targetFilter, "targetFilter", Array.Empty<object>());
			Scribe_Deep.Look<RitualBehaviorWorker>(ref this.behavior, "behavior", Array.Empty<object>());
			Scribe_Collections.Look<RitualObligationTrigger>(ref this.obligationTriggers, "triggers", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<RitualAttachableOutcomeEffectDef>(ref this.attachableOutcomeEffect, "attachableOutcomeEffect");
			Scribe_Deep.Look<RitualOutcomeEffectWorker>(ref this.outcomeEffect, "outcomeEffect", Array.Empty<object>());
			Scribe_Collections.Look<PlanetLayerDef>(ref this.layerWhitelist, "layerWhitelist", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<PlanetLayerDef>(ref this.layerBlacklist, "layerBlacklist", LookMode.Undefined, Array.Empty<object>());
			if (!GameDataSaveLoader.IsSavingOrLoadingExternalIdeo)
			{
				Scribe_Collections.Look<RitualObligation>(ref this.activeObligations, "activeObligations", LookMode.Deep, Array.Empty<object>());
				Scribe_Collections.Look<RitualObligation>(ref this.completedObligations, "completedObligations", LookMode.Deep, Array.Empty<object>());
				Scribe_Values.Look<int>(ref this.lastFinishedTick, "lastFinishedTick", 0, false);
				Scribe_Values.Look<int>(ref this.abilityOnCooldownUntilTick, "abilityOnCooldownUntilTick", -1, false);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (!this.activeObligations.NullOrEmpty<RitualObligation>())
				{
					if (this.activeObligations.RemoveAll((RitualObligation t) => t == null) != 0)
					{
						Log.Warning("Some activeObligations were null.");
					}
					foreach (RitualObligation ritualObligation in this.activeObligations)
					{
						ritualObligation.precept = this;
					}
				}
				if (!this.obligationTriggers.NullOrEmpty<RitualObligationTrigger>())
				{
					if (this.obligationTriggers.RemoveAll((RitualObligationTrigger t) => t == null) != 0)
					{
						Log.Warning("Some obligationTriggers were null.");
					}
					foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
					{
						ritualObligationTrigger.ritual = this;
					}
				}
				if (this.obligationTargetFilter != null)
				{
					this.obligationTargetFilter.parent = this;
				}
				if (this.attachableOutcomeEffect == null && !this.generatedAttachedReward && this.SupportsAttachableOutcomeEffect)
				{
					this.attachableOutcomeEffect = (from d in DefDatabase<RitualAttachableOutcomeEffectDef>.AllDefs
					where d.CanAttachToRitual(this)
					select d).RandomElementWithFallback(null);
					this.generatedAttachedReward = true;
				}
				this.cancelIconPathOverride = this.sourcePattern.cancelIconPathOverride;
			}
		}

		// Token: 0x0600D884 RID: 55428 RVA: 0x00402C08 File Offset: 0x00400E08
		public override void CopyTo(Precept other)
		{
			base.CopyTo(other);
			Precept_Ritual precept_Ritual = (Precept_Ritual)other;
			if (this.activeObligations != null)
			{
				precept_Ritual.activeObligations = new List<RitualObligation>();
				foreach (RitualObligation ritualObligation in this.activeObligations)
				{
					RitualObligation ritualObligation2 = new RitualObligation();
					ritualObligation.CopyTo(ritualObligation2);
					precept_Ritual.activeObligations.Add(ritualObligation2);
				}
			}
			precept_Ritual.nameMaker = this.nameMaker;
			precept_Ritual.obligationTriggers.Clear();
			for (int i = 0; i < this.obligationTriggers.Count; i++)
			{
				RitualObligationTrigger ritualObligationTrigger = (RitualObligationTrigger)Activator.CreateInstance(this.obligationTriggers[i].GetType());
				this.obligationTriggers[i].CopyTo(ritualObligationTrigger);
				ritualObligationTrigger.ritual = precept_Ritual;
				precept_Ritual.obligationTriggers.Add(ritualObligationTrigger);
			}
			if (this.obligationTargetFilter != null)
			{
				precept_Ritual.obligationTargetFilter = this.obligationTargetFilter;
				precept_Ritual.obligationTargetFilter.parent = precept_Ritual;
			}
			precept_Ritual.outcomeEffect = this.outcomeEffect;
			precept_Ritual.attachableOutcomeEffect = this.attachableOutcomeEffect;
			precept_Ritual.targetFilter = this.targetFilter;
			precept_Ritual.behavior = this.behavior;
			precept_Ritual.ritualOnlyForIdeoMembers = this.ritualOnlyForIdeoMembers;
			precept_Ritual.isAnytime = this.isAnytime;
			precept_Ritual.canBeAnytime = this.canBeAnytime;
			precept_Ritual.playsIdeoMusic = this.playsIdeoMusic;
			precept_Ritual.mergeGizmosForObligations = this.mergeGizmosForObligations;
			precept_Ritual.canMergeGizmosFromDifferentIdeos = this.canMergeGizmosFromDifferentIdeos;
			precept_Ritual.lastFinishedTick = this.lastFinishedTick;
			precept_Ritual.abilityOnCooldownUntilTick = this.abilityOnCooldownUntilTick;
			precept_Ritual.ritualExpectedDesc = this.ritualExpectedDesc;
			precept_Ritual.ritualExpectedDescNoAdjective = this.ritualExpectedDescNoAdjective;
			precept_Ritual.shortDescOverride = this.shortDescOverride;
			precept_Ritual.ritualExplanation = this.ritualExplanation;
			precept_Ritual.iconPathOverride = this.iconPathOverride;
			precept_Ritual.patternGroupTag = this.patternGroupTag;
			precept_Ritual.minTechLevel = this.minTechLevel;
			precept_Ritual.maxTechLevel = this.maxTechLevel;
			precept_Ritual.generatedAttachedReward = this.generatedAttachedReward;
			precept_Ritual.sourcePattern = this.sourcePattern;
		}

		// Token: 0x0600D886 RID: 55430 RVA: 0x00402E88 File Offset: 0x00401088
		[CompilerGenerated]
		private Command_Ritual <GetGizmoFor>g__CommandForObligation|105_0(RitualObligation obligation, ref Precept_Ritual.<>c__DisplayClass105_0 A_2)
		{
			RitualTargetUseReport ritualTargetUseReport = this.CanUseTarget(A_2.t, obligation);
			if (ritualTargetUseReport.canUse)
			{
				return new Command_Ritual(this, A_2.t, obligation, null);
			}
			if (!ritualTargetUseReport.failReason.NullOrEmpty())
			{
				return new Command_Ritual(this, A_2.t, obligation, null)
				{
					disabledReason = ritualTargetUseReport.failReason,
					Disabled = true
				};
			}
			return null;
		}

		// Token: 0x04009292 RID: 37522
		public List<RitualObligation> activeObligations;

		// Token: 0x04009293 RID: 37523
		public List<RitualObligation> completedObligations;

		// Token: 0x04009294 RID: 37524
		public RulePackDef nameMaker;

		// Token: 0x04009295 RID: 37525
		public List<RitualObligationTrigger> obligationTriggers = new List<RitualObligationTrigger>();

		// Token: 0x04009296 RID: 37526
		public RitualOutcomeEffectWorker outcomeEffect;

		// Token: 0x04009297 RID: 37527
		public RitualAttachableOutcomeEffectDef attachableOutcomeEffect;

		// Token: 0x04009298 RID: 37528
		public RitualObligationTargetFilter obligationTargetFilter;

		// Token: 0x04009299 RID: 37529
		public RitualTargetFilter targetFilter;

		// Token: 0x0400929A RID: 37530
		public RitualBehaviorWorker behavior;

		// Token: 0x0400929B RID: 37531
		public bool ritualOnlyForIdeoMembers;

		// Token: 0x0400929C RID: 37532
		public bool isAnytime;

		// Token: 0x0400929D RID: 37533
		public bool canBeAnytime;

		// Token: 0x0400929E RID: 37534
		public bool allowOtherInstances;

		// Token: 0x0400929F RID: 37535
		public bool playsIdeoMusic;

		// Token: 0x040092A0 RID: 37536
		public bool mergeGizmosForObligations;

		// Token: 0x040092A1 RID: 37537
		public bool canMergeGizmosFromDifferentIdeos = true;

		// Token: 0x040092A2 RID: 37538
		public int lastFinishedTick = -1;

		// Token: 0x040092A3 RID: 37539
		public int abilityOnCooldownUntilTick = -1;

		// Token: 0x040092A4 RID: 37540
		public bool ignoreExtremeTemperatures;

		// Token: 0x040092A5 RID: 37541
		public string ritualExpectedDesc;

		// Token: 0x040092A6 RID: 37542
		public string ritualExpectedDescNoAdjective;

		// Token: 0x040092A7 RID: 37543
		public string shortDescOverride;

		// Token: 0x040092A8 RID: 37544
		public string ritualExplanation;

		// Token: 0x040092A9 RID: 37545
		public string iconPathOverride;

		// Token: 0x040092AA RID: 37546
		public string cancelIconPathOverride;

		// Token: 0x040092AB RID: 37547
		public string patternGroupTag;

		// Token: 0x040092AC RID: 37548
		public TechLevel minTechLevel;

		// Token: 0x040092AD RID: 37549
		public TechLevel maxTechLevel;

		// Token: 0x040092AE RID: 37550
		public bool generatedAttachedReward;

		// Token: 0x040092AF RID: 37551
		public bool showIdeoIconsInDialog = true;

		// Token: 0x040092B0 RID: 37552
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x040092B1 RID: 37553
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x040092B2 RID: 37554
		public RitualPatternDef sourcePattern;

		// Token: 0x040092B3 RID: 37555
		private string shortDescOverrideCap;

		// Token: 0x040092B4 RID: 37556
		public const float RepeatQualityPenaltyMax = -0.95f;

		// Token: 0x040092B5 RID: 37557
		public const int RepeatPenaltyDurationDays = 20;

		// Token: 0x040092B6 RID: 37558
		public const float LowQualityWarningThreshold = 0.25f;

		// Token: 0x040092B7 RID: 37559
		private readonly IntVec2 DateIconSize = new IntVec2(28, 28);

		// Token: 0x040092B8 RID: 37560
		private Texture2D icon;

		// Token: 0x040092B9 RID: 37561
		private Texture2D cancelIcon;

		// Token: 0x040092BA RID: 37562
		private string tipLabelCached;

		// Token: 0x040092BB RID: 37563
		private static Texture2D dayRitualTex;

		// Token: 0x040092BC RID: 37564
		private static Texture2D anytimeRitualTex;

		// Token: 0x040092BD RID: 37565
		private static Texture2D eventRitualTex;

		// Token: 0x040092BE RID: 37566
		private StringBuilder tmpTipExtraSb = new StringBuilder();

		// Token: 0x040092BF RID: 37567
		private List<LordJob_Ritual> tmpActiveRituals = new List<LordJob_Ritual>();
	}
}
