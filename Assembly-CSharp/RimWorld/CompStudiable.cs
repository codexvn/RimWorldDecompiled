using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E5D RID: 11869
	public class CompStudiable : ThingComp
	{
		// Token: 0x17002AD1 RID: 10961
		// (get) Token: 0x06010B4D RID: 68429 RVA: 0x004D1087 File Offset: 0x004CF287
		public CompProperties_Studiable Props
		{
			get
			{
				return (CompProperties_Studiable)this.props;
			}
		}

		// Token: 0x17002AD2 RID: 10962
		// (get) Token: 0x06010B4E RID: 68430 RVA: 0x0048AE48 File Offset: 0x00489048
		public Pawn Pawn
		{
			get
			{
				return this.parent as Pawn;
			}
		}

		// Token: 0x17002AD3 RID: 10963
		// (get) Token: 0x06010B4F RID: 68431 RVA: 0x004D1094 File Offset: 0x004CF294
		public float ProgressPercent
		{
			get
			{
				if (!this.Props.Completable)
				{
					return 0f;
				}
				return this.studyPoints / this.Props.studyAmountToComplete;
			}
		}

		// Token: 0x17002AD4 RID: 10964
		// (get) Token: 0x06010B50 RID: 68432 RVA: 0x004D10BB File Offset: 0x004CF2BB
		public float AdjustedAnomalyKnowledgePerStudy
		{
			get
			{
				return this.AdjustedAnomalyKnowledge(null) / 5f;
			}
		}

		// Token: 0x17002AD5 RID: 10965
		// (get) Token: 0x06010B51 RID: 68433 RVA: 0x004D10CA File Offset: 0x004CF2CA
		public bool Completed
		{
			get
			{
				return this.Props.Completable && this.ProgressPercent >= 1f;
			}
		}

		// Token: 0x17002AD6 RID: 10966
		// (get) Token: 0x06010B52 RID: 68434 RVA: 0x004D10EB File Offset: 0x004CF2EB
		public int TicksTilNextStudy
		{
			get
			{
				return this.lastStudiedTick + this.Props.frequencyTicks - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17002AD7 RID: 10967
		// (get) Token: 0x06010B53 RID: 68435 RVA: 0x004D110A File Offset: 0x004CF30A
		private bool IsMutant
		{
			get
			{
				return this.Pawn != null && this.Pawn.IsMutant;
			}
		}

		// Token: 0x17002AD8 RID: 10968
		// (get) Token: 0x06010B54 RID: 68436 RVA: 0x004D1124 File Offset: 0x004CF324
		public virtual float AnomalyKnowledge
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return 0f;
				}
				if (this.KnowledgeCategory == null)
				{
					return 0f;
				}
				float num = 0f;
				CompStudyUnlocks compStudyUnlocks = this.compStudyUnlocks;
				if (compStudyUnlocks != null && compStudyUnlocks.StudyKnowledgeAmount != null)
				{
					num = this.compStudyUnlocks.StudyKnowledgeAmount.Value;
				}
				else if (this.Props.anomalyKnowledge != null)
				{
					num = this.Props.anomalyKnowledge.Value;
				}
				else if (this.Pawn != null)
				{
					num = (float)this.Pawn.RaceProps.anomalyKnowledge;
				}
				else
				{
					Debug.LogError(string.Format("Thing {0} has studiable comp but has no overriden anomaly knowledge amount, ensure this is on a pawn with setup race props vars, or provides a knowledge amount value in this comp.", this.parent));
				}
				CompStudyUnlocks compStudyUnlocks2 = this.compStudyUnlocks;
				if (compStudyUnlocks2 != null && compStudyUnlocks2.StudyKnowledgeAmount != null)
				{
					num = this.compStudyUnlocks.StudyKnowledgeAmount.Value;
				}
				if (this.IsMutant)
				{
					num += this.Pawn.mutant.Def.anomalyKnowledgeOffset;
				}
				return num;
			}
		}

		// Token: 0x17002AD9 RID: 10969
		// (get) Token: 0x06010B55 RID: 68437 RVA: 0x004D122C File Offset: 0x004CF42C
		public virtual KnowledgeCategoryDef KnowledgeCategory
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return null;
				}
				if (this.IsMutant)
				{
					return this.Pawn.mutant.Def.knowledgeCategory;
				}
				if (this.Props.knowledgeCategory != null)
				{
					return this.Props.knowledgeCategory;
				}
				Pawn pawn = this.Pawn;
				if (pawn == null)
				{
					return null;
				}
				return pawn.RaceProps.knowledgeCategory;
			}
		}

		// Token: 0x17002ADA RID: 10970
		// (get) Token: 0x06010B56 RID: 68438 RVA: 0x004D128F File Offset: 0x004CF48F
		public bool RequiresHoldingPlatform
		{
			get
			{
				return this.IsMutant || this.Props.requiresHoldingPlatform;
			}
		}

		// Token: 0x17002ADB RID: 10971
		// (get) Token: 0x06010B57 RID: 68439 RVA: 0x004D12A6 File Offset: 0x004CF4A6
		public bool RequiresImprisonment
		{
			get
			{
				return !this.IsMutant && this.Props.requiresImprisonment;
			}
		}

		// Token: 0x17002ADC RID: 10972
		// (get) Token: 0x06010B58 RID: 68440 RVA: 0x004D12BD File Offset: 0x004CF4BD
		public bool Deactivated
		{
			get
			{
				return this.Props.canBeActivityDeactivated && this.CompActivity.Deactivated;
			}
		}

		// Token: 0x17002ADD RID: 10973
		// (get) Token: 0x06010B59 RID: 68441 RVA: 0x004D12D9 File Offset: 0x004CF4D9
		private bool DisplayStudyInfoOnInspectPane
		{
			get
			{
				return !this.parent.Destroyed && !this.Completed && (!ModsConfig.AnomalyActive || Find.Anomaly.HighestLevelReached >= this.Props.minMonolithLevelForStudy);
			}
		}

		// Token: 0x17002ADE RID: 10974
		// (get) Token: 0x06010B5A RID: 68442 RVA: 0x004D1318 File Offset: 0x004CF518
		private CompActivity CompActivity
		{
			get
			{
				CompActivity result;
				if ((result = this.compActivity) == null)
				{
					result = (this.compActivity = this.parent.GetComp<CompActivity>());
				}
				return result;
			}
		}

		// Token: 0x06010B5B RID: 68443 RVA: 0x004D1343 File Offset: 0x004CF543
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.studyEnabled = this.Props.studyEnabledByDefault;
		}

		// Token: 0x06010B5C RID: 68444 RVA: 0x004D135C File Offset: 0x004CF55C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.compStudyUnlocks = this.parent.TryGetComp<CompStudyUnlocks>();
			StudyManager studyManager = Find.StudyManager;
			Thing parent = this.parent;
			Map map;
			if ((map = this.parent.Map) == null)
			{
				Thing thing = this.parent.ParentHolder as Thing;
				map = ((thing != null) ? thing.Map : null);
			}
			studyManager.UpdateStudiableCache(parent, map);
		}

		// Token: 0x06010B5D RID: 68445 RVA: 0x004D13B5 File Offset: 0x004CF5B5
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			Find.StudyManager.UpdateStudiableCache(this.parent, map);
			base.PostDeSpawn(map, mode);
		}

		// Token: 0x06010B5E RID: 68446 RVA: 0x004D13D0 File Offset: 0x004CF5D0
		public void SetStudyEnabled(bool enabled)
		{
			this.studyEnabled = enabled;
		}

		// Token: 0x06010B5F RID: 68447 RVA: 0x004D13DC File Offset: 0x004CF5DC
		public bool CurrentlyStudiable()
		{
			string text;
			if (!this.EverStudiable(out text))
			{
				return false;
			}
			if (!this.studyEnabled)
			{
				return false;
			}
			if (this.Props.frequencyTicks > 0 && this.TicksTilNextStudy > 0)
			{
				return false;
			}
			Pawn pawn = this.parent as Pawn;
			if (pawn != null)
			{
				if (this.RequiresHoldingPlatform)
				{
					CompHoldingPlatformTarget compHoldingPlatformTarget = pawn.TryGetComp<CompHoldingPlatformTarget>();
					if (compHoldingPlatformTarget == null || !compHoldingPlatformTarget.CanStudy)
					{
						return false;
					}
				}
				if (this.RequiresImprisonment)
				{
					if (pawn.Downed || !pawn.Awake())
					{
						return false;
					}
					if (pawn.guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.Study))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06010B60 RID: 68448 RVA: 0x004D1474 File Offset: 0x004CF674
		public bool EverStudiable()
		{
			string text;
			return this.EverStudiable(out text);
		}

		// Token: 0x06010B61 RID: 68449 RVA: 0x004D1489 File Offset: 0x004CF689
		public bool EverStudiable(out string reason)
		{
			reason = null;
			return this.StudyUnlocked() && this.EverStudiableCached(out reason);
		}

		// Token: 0x06010B62 RID: 68450 RVA: 0x004D14A4 File Offset: 0x004CF6A4
		public bool StudyUnlocked()
		{
			return !this.parent.Destroyed && !this.Deactivated && (!ModsConfig.AnomalyActive || Find.Anomaly.HighestLevelReached >= this.Props.minMonolithLevelForStudy || !Find.Anomaly.GenerateMonolith);
		}

		// Token: 0x06010B63 RID: 68451 RVA: 0x004D14F8 File Offset: 0x004CF6F8
		public bool EverStudiableCached()
		{
			string text;
			return this.EverStudiableCached(out text);
		}

		// Token: 0x06010B64 RID: 68452 RVA: 0x004D1510 File Offset: 0x004CF710
		public bool EverStudiableCached(out string reason)
		{
			reason = null;
			if (this.parent == null)
			{
				return false;
			}
			Pawn pawn = this.parent as Pawn;
			if (pawn != null)
			{
				if (this.RequiresHoldingPlatform && !(this.parent.ParentHolder is Building_HoldingPlatform))
				{
					reason = "RequiresHoldingPlatform".Translate();
					return false;
				}
				if (this.RequiresImprisonment && ((!pawn.Inhumanized() && !pawn.kindDef.studiableAsPrisoner) || !pawn.IsPrisonerOfColony))
				{
					reason = "RequiresImprisonment".Translate();
					return false;
				}
			}
			return true;
		}

		// Token: 0x06010B65 RID: 68453 RVA: 0x004D15A0 File Offset: 0x004CF7A0
		public virtual void Study(Pawn studier, float studyAmount, float anomalyKnowledgeAmount = 0f)
		{
			bool completed = this.Completed;
			studyAmount *= Find.Storyteller.difficulty.researchSpeedFactor;
			studyAmount *= studier.GetStatValue(StatDefOf.ResearchSpeed, true, -1);
			this.anomalyKnowledgeGained += anomalyKnowledgeAmount;
			Find.StudyManager.Study(this.parent, studier, studyAmount);
			if (ModsConfig.AnomalyActive && anomalyKnowledgeAmount > 0f)
			{
				Find.StudyManager.StudyAnomaly(this.parent, studier, anomalyKnowledgeAmount, this.KnowledgeCategory);
			}
			if (studier != null)
			{
				studier.skills.Learn(SkillDefOf.Intellectual, 0.1f, false, false);
			}
			if (!completed && this.Completed)
			{
				QuestUtility.SendQuestTargetSignals(this.parent.questTags, "Researched", this.parent.Named("SUBJECT"), studier.Named("STUDIER"));
				if (!this.Props.completedMessage.NullOrEmpty())
				{
					Messages.Message(this.Props.completedMessage, this.parent, MessageTypeDefOf.NeutralEvent, true);
				}
				if (studier != null && !this.Props.completedLetterText.NullOrEmpty() && !this.Props.completedLetterTitle.NullOrEmpty())
				{
					Find.LetterStack.ReceiveLetter(this.Props.completedLetterTitle.Formatted(studier.Named("STUDIER"), this.parent.Named("PARENT")), this.Props.completedLetterText.Formatted(studier.Named("STUDIER"), this.parent.Named("PARENT")), this.Props.completedLetterDef ?? LetterDefOf.NeutralEvent, new List<Thing>
					{
						this.parent,
						studier
					}, null, null, null, null, 0, true);
				}
			}
		}

		// Token: 0x06010B66 RID: 68454 RVA: 0x004D1772 File Offset: 0x004CF972
		public void Notify_ActivityDeactivated()
		{
			if (!this.Props.canBeActivityDeactivated)
			{
				return;
			}
			this.studyEnabled = false;
		}

		// Token: 0x06010B67 RID: 68455 RVA: 0x004D1789 File Offset: 0x004CF989
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			string text;
			bool flag = this.EverStudiable(out text);
			if (this.Props.showToggleGizmo && (flag || text != null))
			{
				Command_Toggle command_Toggle = new Command_Toggle
				{
					defaultLabel = "CommandToggleStudy".Translate(),
					defaultDesc = "CommandToggleStudyDesc".Translate(),
					icon = CompStudiable.StudyToggleIcon.Texture,
					isActive = (() => this.studyEnabled),
					toggleAction = delegate()
					{
						this.SetStudyEnabled(!this.studyEnabled);
					},
					hideIconIfDisabled = true
				};
				command_Toggle.tutorTag = "ToggleStudy";
				if (text != null)
				{
					command_Toggle.Disable(text);
				}
				yield return command_Toggle;
			}
			if (DebugSettings.ShowDevGizmos)
			{
				if (this.EverStudiable() && this.TicksTilNextStudy > 0)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: End study cooldown",
						action = delegate()
						{
							this.lastStudiedTick = Find.TickManager.TicksGame - this.Props.frequencyTicks;
						}
					};
				}
				if (this.Props.Completable && !this.Completed)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Complete study",
						action = delegate()
						{
							int num = 100;
							while (!this.Completed && num > 0)
							{
								Map map = this.parent.Map;
								Pawn studier;
								if (map == null)
								{
									studier = null;
								}
								else
								{
									MapPawns mapPawns = map.mapPawns;
									if (mapPawns == null)
									{
										studier = null;
									}
									else
									{
										List<Pawn> freeColonists = mapPawns.FreeColonists;
										studier = ((freeColonists != null) ? freeColonists.RandomElement<Pawn>() : null);
									}
								}
								this.Study(studier, float.MaxValue, 0f);
								num--;
							}
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x06010B68 RID: 68456 RVA: 0x004D1799 File Offset: 0x004CF999
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetRelatedQuestHyperlinks()
		{
			List<Quest> quests = Find.QuestManager.QuestsListForReading;
			int num;
			for (int i = 0; i < quests.Count; i = num + 1)
			{
				if (!quests[i].hidden && (quests[i].State == QuestState.Ongoing || quests[i].State == QuestState.NotYetAccepted))
				{
					List<QuestPart> partsListForReading = quests[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_RequirementsToAcceptThingStudied questPart_RequirementsToAcceptThingStudied = partsListForReading[j] as QuestPart_RequirementsToAcceptThingStudied;
						if (questPart_RequirementsToAcceptThingStudied != null && questPart_RequirementsToAcceptThingStudied.thing == this.parent)
						{
							yield return new Dialog_InfoCard.Hyperlink(quests[i], -1);
							break;
						}
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06010B69 RID: 68457 RVA: 0x004D17AC File Offset: 0x004CF9AC
		private float AdjustedAnomalyKnowledge(StringBuilder sb = null)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return 0f;
			}
			float num = this.AnomalyKnowledge;
			if (sb != null)
			{
				sb.AppendLine("StatsReport_BaseValue".Translate() + ": " + num.ToStringDecimalIfSmall());
			}
			bool flag = false;
			CompHoldingPlatformTarget compHoldingPlatformTarget;
			if (this.parent.TryGetComp(out compHoldingPlatformTarget) && compHoldingPlatformTarget.CurrentlyHeldOnPlatform)
			{
				CompEntityHolder comp = compHoldingPlatformTarget.HeldPlatform.GetComp<CompEntityHolder>();
				float studyKnowledgeAmountMultiplier = ContainmentUtility.GetStudyKnowledgeAmountMultiplier(this.parent, comp);
				if (!Mathf.Approximately(studyKnowledgeAmountMultiplier, 1f))
				{
					if (!flag)
					{
						if (sb != null)
						{
							sb.AppendLine();
						}
						flag = true;
					}
					num *= studyKnowledgeAmountMultiplier;
					if (sb != null)
					{
						sb.AppendLine("FactorContainmentStrength".Translate() + " (" + StatDefOf.ContainmentStrength.Worker.ValueToString(comp.ContainmentStrength, true, ToStringNumberSense.Absolute) + ")" + ": x" + studyKnowledgeAmountMultiplier.ToStringPercent());
					}
				}
				if (compHoldingPlatformTarget.HeldPlatform.HasAttachedElectroharvester)
				{
					if (!flag)
					{
						if (sb != null)
						{
							sb.AppendLine();
						}
						flag = true;
					}
					num *= 0.5f;
					if (sb != null)
					{
						sb.AppendLine(ThingDefOf.Electroharvester.LabelCap + ": x" + 0.5f.ToStringPercent());
					}
				}
			}
			CompActivity compActivity;
			if (this.parent.TryGetComp(out compActivity))
			{
				if (!flag)
				{
					if (sb != null)
					{
						sb.AppendLine();
					}
					flag = true;
				}
				num *= compActivity.ActivityResearchFactor;
				if (sb != null)
				{
					sb.AppendLine("FactorActivity".Translate() + ": x" + compActivity.ActivityResearchFactor.ToStringPercent());
				}
			}
			if (!Mathf.Approximately(this.Props.knowledgeFactorOutdoors, 1f) && this.parent.IsOutside())
			{
				if (!flag)
				{
					if (sb != null)
					{
						sb.AppendLine();
					}
					flag = true;
				}
				num *= this.Props.knowledgeFactorOutdoors;
				if (sb != null)
				{
					sb.AppendLine("Outdoors".Translate().CapitalizeFirst() + ": x" + this.Props.knowledgeFactorOutdoors.ToStringPercent());
				}
			}
			if (!Mathf.Approximately(Find.Storyteller.difficulty.studyEfficiencyFactor, 1f))
			{
				if (!flag)
				{
					if (sb != null)
					{
						sb.AppendLine();
					}
				}
				num *= Find.Storyteller.difficulty.studyEfficiencyFactor;
				if (sb != null)
				{
					sb.AppendLine("DifficultyLevel".Translate() + ": x" + Find.Storyteller.difficulty.studyEfficiencyFactor.ToStringPercent());
				}
			}
			if (sb != null)
			{
				sb.AppendLine();
			}
			if (sb != null)
			{
				sb.AppendLine("StatsReport_FinalValue".Translate() + ": " + num.ToStringDecimalIfSmall());
			}
			return num;
		}

		// Token: 0x06010B6A RID: 68458 RVA: 0x004D1A9B File Offset: 0x004CFC9B
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			IEnumerable<StatDrawEntry> enumerable = base.SpecialDisplayStats();
			if (enumerable != null)
			{
				foreach (StatDrawEntry statDrawEntry in enumerable)
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			if (this.Props.Completable)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "Study".Translate(), this.studyPoints.ToString() + " / " + this.Props.studyAmountToComplete.ToString(), "Stat_Studiable_Desc".Translate(), 3000, null, this.GetRelatedQuestHyperlinks(), false, true);
			}
			if (ModsConfig.AnomalyActive && this.AnomalyKnowledge > 0f && this.KnowledgeCategory != null)
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn == null || this.IsMutant || this.RequiresHoldingPlatform || (this.RequiresImprisonment && (pawn.Inhumanized() || pawn.kindDef.studiableAsPrisoner)))
				{
					CompStudiable.knowledgeSb.Clear();
					float f = this.AdjustedAnomalyKnowledge(CompStudiable.knowledgeSb);
					yield return new StatDrawEntry(StatCategoryDefOf.Containment, "KnowledgeFromStudy".Translate(), "Stat_Knowledge".Translate(f.ToStringDecimalIfSmall(), this.KnowledgeCategory.label), "KnowledgeFromStudyDesc".Translate() + "\n\n" + CompStudiable.knowledgeSb.ToString(), 2550, null, null, false, true);
					if (this.Props.frequencyTicks > 0)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Containment, "StudyFrequency".Translate(), this.Props.frequencyTicks.ToStringTicksToPeriod(true, false, true, true, false), "StudyFrequencyDesc".Translate(), 2545, null, null, false, true);
					}
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06010B6B RID: 68459 RVA: 0x004D1AAC File Offset: 0x004CFCAC
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastStudiedTick, "lastStudiedTick", 0, false);
			Scribe_Values.Look<bool>(ref this.studyEnabled, "studyEnabled", true, false);
			Scribe_Values.Look<float>(ref this.studyPoints, "studiedAmount", 0f, false);
			Scribe_Values.Look<float>(ref this.anomalyKnowledgeGained, "anomalyKnowledgeGained", 0f, false);
			Scribe_Values.Look<int>(ref this.studyInteractions, "studyInteractions", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				float num = 0f;
				Scribe_Values.Look<float>(ref num, "progress", 0f, false);
				if (num > 0f)
				{
					float num2 = num / this.Props.studyAmountToComplete;
					this.studyPoints = this.Props.studyAmountToComplete * num2;
				}
			}
			float num3;
			if (Scribe.mode == LoadSaveMode.PostLoadInit && Find.StudyManager.backCompatStudyProgress.TryGetValue(this.parent.def, out num3))
			{
				this.studyPoints = this.Props.studyAmountToComplete * num3;
			}
		}

		// Token: 0x06010B6C RID: 68460 RVA: 0x004D1BA0 File Offset: 0x004CFDA0
		public override string CompInspectStringExtra()
		{
			string text = string.Empty;
			if (!this.EverStudiable())
			{
				return text;
			}
			if (this.Props.Completable)
			{
				text = text + ("StudyProgress".Translate() + ": " + this.studyPoints.ToString("F1") + " / ") + this.Props.studyAmountToComplete.ToString();
				if (this.Completed)
				{
					text += " (" + "StudyCompleted".Translate() + ")";
				}
			}
			else if (!this.RequiresHoldingPlatform && !(this.parent is Pawn))
			{
				string inspectStringExtraFor = CompStudiable.GetInspectStringExtraFor(this.parent);
				if (!inspectStringExtraFor.NullOrEmpty())
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += inspectStringExtraFor;
				}
			}
			return text;
		}

		// Token: 0x06010B6D RID: 68461 RVA: 0x004D1C98 File Offset: 0x004CFE98
		public static string GetInspectStringExtraFor(Thing thing)
		{
			CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
			if (compStudiable != null && compStudiable.DisplayStudyInfoOnInspectPane && compStudiable.EverStudiable())
			{
				TaggedString t = TaggedString.Empty;
				if (ModsConfig.AnomalyActive && compStudiable.AnomalyKnowledge > 0f && compStudiable.KnowledgeCategory != null)
				{
					t += "KnowledgeCategory".Translate() + ": " + compStudiable.KnowledgeCategory.LabelCap;
					if (compStudiable.TicksTilNextStudy <= 0)
					{
						t += string.Format(" ({0})", "ReadyToStudy".Translate());
					}
					else if (compStudiable.TicksTilNextStudy > 0)
					{
						t += string.Format(" ({0})", "CanBeStudiedInDuration".Translate(compStudiable.TicksTilNextStudy.ToStringTicksToPeriod(true, false, true, true, false)));
					}
					if (compStudiable.Props.knowledgeFactorOutdoors != 1f && compStudiable.parent.IsOutside())
					{
						t += string.Format("\n{0} ({1})", "KnowledgeFactorOutdoors".Translate() + ": x" + compStudiable.Props.knowledgeFactorOutdoors.ToStringPercent(), "Outdoors".Translate());
					}
				}
				else if (compStudiable.TicksTilNextStudy <= 0)
				{
					t += "ReadyToStudy".Translate().CapitalizeFirst();
				}
				else
				{
					t += "CanBeStudiedInDuration".Translate(compStudiable.TicksTilNextStudy.ToStringTicksToPeriod(true, false, true, true, false));
				}
				return t.Resolve();
			}
			return null;
		}

		// Token: 0x0400AE36 RID: 44598
		private static readonly CachedTexture StudyToggleIcon = new CachedTexture("UI/Icons/Study");

		// Token: 0x0400AE37 RID: 44599
		public int lastStudiedTick = -9999999;

		// Token: 0x0400AE38 RID: 44600
		public bool studyEnabled = true;

		// Token: 0x0400AE39 RID: 44601
		public float studyPoints;

		// Token: 0x0400AE3A RID: 44602
		public float anomalyKnowledgeGained;

		// Token: 0x0400AE3B RID: 44603
		public int studyInteractions;

		// Token: 0x0400AE3C RID: 44604
		private CompStudyUnlocks compStudyUnlocks;

		// Token: 0x0400AE3D RID: 44605
		private CompActivity compActivity;

		// Token: 0x0400AE3E RID: 44606
		private static StringBuilder knowledgeSb = new StringBuilder();
	}
}
