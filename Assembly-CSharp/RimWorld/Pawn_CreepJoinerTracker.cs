using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020024B4 RID: 9396
	public class Pawn_CreepJoinerTracker : IExposable
	{
		// Token: 0x17001F1B RID: 7963
		// (get) Token: 0x0600CC13 RID: 52243 RVA: 0x003BC923 File Offset: 0x003BAB23
		private BaseCreepJoinerWorker DownsideWorker
		{
			get
			{
				return this.GetWorker<BaseCreepJoinerWorker>(this.downside.workerType, ref this.downsideWorkerInt);
			}
		}

		// Token: 0x17001F1C RID: 7964
		// (get) Token: 0x0600CC14 RID: 52244 RVA: 0x003BC93C File Offset: 0x003BAB3C
		private BaseCreepJoinerWorker RejectionWorker
		{
			get
			{
				return this.GetWorker<BaseCreepJoinerWorker>(this.rejection.workerType, ref this.rejectionWorkerInt);
			}
		}

		// Token: 0x17001F1D RID: 7965
		// (get) Token: 0x0600CC15 RID: 52245 RVA: 0x003BC955 File Offset: 0x003BAB55
		private BaseCreepJoinerWorker AggressiveWorker
		{
			get
			{
				return this.GetWorker<BaseCreepJoinerWorker>(this.aggressive.workerType, ref this.aggressiveWorkerInt);
			}
		}

		// Token: 0x17001F1E RID: 7966
		// (get) Token: 0x0600CC16 RID: 52246 RVA: 0x003BC970 File Offset: 0x003BAB70
		public bool IsOnEntryLord
		{
			get
			{
				if (this.entryLordEnded)
				{
					return false;
				}
				Lord lord = this.Pawn.GetLord();
				return lord != null && lord.LordJob is LordJob_CreepJoiner;
			}
		}

		// Token: 0x17001F1F RID: 7967
		// (get) Token: 0x0600CC17 RID: 52247 RVA: 0x003BC9A6 File Offset: 0x003BABA6
		public bool Disabled
		{
			get
			{
				return this.duplicated || this.Pawn.everLostEgo || !this.Pawn.SpawnedOrAnyParentSpawned;
			}
		}

		// Token: 0x17001F20 RID: 7968
		// (get) Token: 0x0600CC18 RID: 52248 RVA: 0x003BC9CD File Offset: 0x003BABCD
		public Pawn Pawn { get; }

		// Token: 0x0600CC19 RID: 52249 RVA: 0x000024DF File Offset: 0x000006DF
		public Pawn_CreepJoinerTracker()
		{
		}

		// Token: 0x0600CC1A RID: 52250 RVA: 0x003BC9D5 File Offset: 0x003BABD5
		public Pawn_CreepJoinerTracker(Pawn pawn)
		{
			this.Pawn = pawn;
		}

		// Token: 0x0600CC1B RID: 52251 RVA: 0x003BC9E4 File Offset: 0x003BABE4
		public void Notify_Created()
		{
			this.ResolveGraphics();
			BaseCreepJoinerWorker downsideWorker = this.DownsideWorker;
			if (downsideWorker != null)
			{
				downsideWorker.OnCreated();
			}
			BaseCreepJoinerWorker aggressiveWorker = this.AggressiveWorker;
			if (aggressiveWorker != null)
			{
				aggressiveWorker.OnCreated();
			}
			BaseCreepJoinerWorker rejectionWorker = this.RejectionWorker;
			if (rejectionWorker == null)
			{
				return;
			}
			rejectionWorker.OnCreated();
		}

		// Token: 0x0600CC1C RID: 52252 RVA: 0x003BCA1E File Offset: 0x003BAC1E
		public void TickInterval(int delta)
		{
			if (this.Disabled)
			{
				return;
			}
			if (this.Pawn.IsColonist)
			{
				this.ColonistTickInterval(delta);
				return;
			}
			this.CheckTriggersTickInterval(delta);
		}

		// Token: 0x0600CC1D RID: 52253 RVA: 0x003BCA48 File Offset: 0x003BAC48
		private void CheckTriggersTickInterval(int delta)
		{
			if (this.triggerPrisonerAggressiveAt != 0 && GenTicks.TicksGame >= this.triggerPrisonerAggressiveAt && !this.triggeredAggressive)
			{
				this.DoAggressive();
			}
			else if (this.triggerPrisonerRejectionAt != 0 && GenTicks.TicksGame >= this.triggerPrisonerRejectionAt && !this.triggeredRejection)
			{
				this.DoRejection();
			}
			if (this.triggerPrisonerAggressiveAt == 0 && this.triggerPrisonerRejectionAt == 0 && this.downside.canOccurWhenImprisoned && this.Pawn.IsPrisonerOfColony && this.Pawn.IsHashIntervalTick(15000, delta))
			{
				this.CheckDownsideOccurs();
			}
		}

		// Token: 0x0600CC1E RID: 52254 RVA: 0x003BCAE0 File Offset: 0x003BACE0
		private void ColonistTickInterval(int delta)
		{
			if (this.Pawn.IsHashIntervalTick(15000, delta))
			{
				this.CheckDownsideOccurs();
			}
		}

		// Token: 0x0600CC1F RID: 52255 RVA: 0x003BCAFC File Offset: 0x003BACFC
		private void CheckDownsideOccurs()
		{
			if (this.downside == null)
			{
				return;
			}
			if (!this.downside.repeats && this.triggeredDownside)
			{
				return;
			}
			if (!this.downside.canOccurWhenImprisoned && (this.Pawn.IsPrisoner || this.Pawn.IsSlave))
			{
				return;
			}
			if (this.downside.triggerMinDays != FloatRange.Zero && GenTicks.TicksGame < this.canTriggerDownsideAfter)
			{
				return;
			}
			if (!this.downside.canOccurWhileDowned && this.Pawn.Downed)
			{
				return;
			}
			if (!this.Pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			if (this.downside.mustBeConscious && !this.Pawn.health.capacities.CanBeAwake)
			{
				return;
			}
			bool flag = this.downside.triggerMtbDays != 0f && Rand.MTBEventOccurs(this.downside.triggerMtbDays, 60000f, 15000f);
			bool flag2 = this.downsideTriggersAt != 0 && GenTicks.TicksGame >= this.downsideTriggersAt;
			if (flag || flag2)
			{
				this.DoDownside();
			}
		}

		// Token: 0x0600CC20 RID: 52256 RVA: 0x003BCC18 File Offset: 0x003BAE18
		public void Notify_ChangedFaction()
		{
			if (this.Pawn.IsColonist)
			{
				this.ClearLord();
				this.joinedTick = GenTicks.TicksGame;
				if (this.downside.triggersAfterDays != FloatRange.Zero)
				{
					this.downsideTriggersAt = GenTicks.TicksGame + (int)(this.downside.triggersAfterDays.RandomInRange * 60000f);
				}
				if (this.downside.triggerMinDays != FloatRange.Zero)
				{
					this.canTriggerDownsideAfter = GenTicks.TicksGame + (int)(this.downside.triggerMinDays.RandomInRange * 60000f);
				}
			}
		}

		// Token: 0x0600CC21 RID: 52257 RVA: 0x003BCCBA File Offset: 0x003BAEBA
		public void Notify_DuplicatedFrom(Pawn _)
		{
			this.duplicated = true;
		}

		// Token: 0x0600CC22 RID: 52258 RVA: 0x003BCCC3 File Offset: 0x003BAEC3
		public void Notify_Arrested(bool succeeded)
		{
			if (this.duplicated)
			{
				return;
			}
			if (!succeeded)
			{
				this.DoAggressive();
				return;
			}
			this.ClearLord();
		}

		// Token: 0x0600CC23 RID: 52259 RVA: 0x003BCCE0 File Offset: 0x003BAEE0
		public void Notify_Released()
		{
			if (this.duplicated)
			{
				return;
			}
			this.triggerPrisonerRejectionAt = GenTicks.TicksGame + Pawn_CreepJoinerTracker.ReleaseRejectionDelayTicks.RandomInRange;
		}

		// Token: 0x0600CC24 RID: 52260 RVA: 0x003BCD10 File Offset: 0x003BAF10
		public void Notify_PrisonBreakout()
		{
			if (this.duplicated)
			{
				return;
			}
			this.triggerPrisonerAggressiveAt = GenTicks.TicksGame + Pawn_CreepJoinerTracker.ReleaseRejectionDelayTicks.RandomInRange;
		}

		// Token: 0x0600CC25 RID: 52261 RVA: 0x003BCD3F File Offset: 0x003BAF3F
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (!DebugSettings.ShowDevGizmos || !this.Pawn.IsColonist)
			{
				yield break;
			}
			if (this.canTriggerDownsideAfter != 0 && !this.triggeredDownside && GenTicks.TicksGame < this.canTriggerDownsideAfter && (this.downside.canOccurWhenImprisoned || !this.Pawn.IsPrisoner))
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Unlock downside trigger",
					action = delegate()
					{
						this.canTriggerDownsideAfter = GenTicks.TicksGame;
					}
				};
			}
			else if ((this.downsideTriggersAt != 0 || this.downside.triggerMtbDays != 0f) && (!this.triggeredDownside || this.downside.repeats) && (this.downside.canOccurWhenImprisoned || !this.Pawn.IsPrisoner))
			{
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "DEV: Trigger timed downside",
					action = new Action(this.DoDownside)
				};
				BaseCreepJoinerWorker downsideWorker = this.DownsideWorker;
				if (downsideWorker != null && !downsideWorker.CanDoResponse())
				{
					command_Action.Disable("Worker is blocking this trigger from occuring.");
				}
				yield return command_Action;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Do aggressive",
				action = new Action(this.DoAggressive)
			};
			yield return new Command_Action
			{
				defaultLabel = "DEV: Do rejection",
				action = new Action(this.DoRejection)
			};
			yield break;
		}

		// Token: 0x0600CC26 RID: 52262 RVA: 0x003BCD50 File Offset: 0x003BAF50
		public string GetInspectString()
		{
			if (!DebugSettings.godMode || this.Disabled)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			string text = "DEV Downside: " + this.downside.label;
			if (this.canTriggerDownsideAfter != 0 && !this.triggeredDownside && GenTicks.TicksGame < this.canTriggerDownsideAfter && (this.downside.canOccurWhenImprisoned || !this.Pawn.IsPrisoner))
			{
				text = text + " (can after: " + (this.canTriggerDownsideAfter - GenTicks.TicksGame).ToStringTicksToPeriod(true, false, true, true, false) + ")";
			}
			else if (this.downsideTriggersAt != 0 && !this.triggeredDownside && (this.downside.canOccurWhenImprisoned || !this.Pawn.IsPrisoner))
			{
				text = text + " (triggers: " + (this.downsideTriggersAt - GenTicks.TicksGame).ToStringTicksToPeriod(true, false, true, true, false) + ")";
			}
			string str = "DEV Benefit: ";
			CreepJoinerBenefitDef creepJoinerBenefitDef = this.benefit;
			stringBuilder.AppendLine(str + ((creepJoinerBenefitDef != null) ? creepJoinerBenefitDef.label : null));
			stringBuilder.AppendLine(text);
			string str2 = "DEV Rejection: ";
			CreepJoinerRejectionDef creepJoinerRejectionDef = this.rejection;
			stringBuilder.AppendLine(str2 + ((creepJoinerRejectionDef != null) ? creepJoinerRejectionDef.label : null));
			string str3 = "DEV Aggressive: ";
			CreepJoinerAggressiveDef creepJoinerAggressiveDef = this.aggressive;
			stringBuilder.Append(str3 + ((creepJoinerAggressiveDef != null) ? creepJoinerAggressiveDef.label : null));
			return stringBuilder.ToString();
		}

		// Token: 0x0600CC27 RID: 52263 RVA: 0x003BCEB0 File Offset: 0x003BB0B0
		public void Notify_CreepJoinerSpokenTo(Pawn speaker)
		{
			if (this.Disabled)
			{
				return;
			}
			this.speaker = speaker;
			Find.SignalManager.SendSignal(new Signal(this.spokeToSignal, false));
		}

		// Token: 0x0600CC28 RID: 52264 RVA: 0x003BCED8 File Offset: 0x003BB0D8
		public void Notify_CreepJoinerAttacked(Pawn instigatorPawn)
		{
			if (this.Disabled)
			{
				return;
			}
			if (instigatorPawn == null)
			{
				return;
			}
			if (instigatorPawn.CurJobDef == JobDefOf.SocialFight)
			{
				return;
			}
			this.DoAggressive();
		}

		// Token: 0x0600CC29 RID: 52265 RVA: 0x003BCEFB File Offset: 0x003BB0FB
		public void Notify_CreepJoinerKilled()
		{
			if (this.Disabled)
			{
				return;
			}
			if (this.AggressiveWorker.CanOccurOnDeath)
			{
				this.DoAggressive();
			}
		}

		// Token: 0x0600CC2A RID: 52266 RVA: 0x003BCF19 File Offset: 0x003BB119
		public void Notify_CreepJoinerRejected()
		{
			this.DoRejection();
		}

		// Token: 0x0600CC2B RID: 52267 RVA: 0x003BCF24 File Offset: 0x003BB124
		public void DoDownside()
		{
			if (this.Disabled)
			{
				return;
			}
			if (this.DownsideWorker != null && !this.DownsideWorker.CanDoResponse())
			{
				return;
			}
			this.ClearLord();
			this.triggeredDownside = true;
			if (this.downside.triggersAfterDays != FloatRange.Zero)
			{
				this.downsideTriggersAt = GenTicks.TicksGame + (int)(this.downside.triggersAfterDays.RandomInRange * 60000f);
			}
			if (this.downside.triggerMinDays != FloatRange.Zero)
			{
				this.canTriggerDownsideAfter = GenTicks.TicksGame + (int)(this.downside.triggerMinDays.RandomInRange * 60000f);
			}
			List<TargetInfo> list = new List<TargetInfo>
			{
				this.Pawn
			};
			List<NamedArgument> list2 = new List<NamedArgument>
			{
				this.Pawn.Named("PAWN")
			};
			foreach (HediffDef def in this.downside.hediffs)
			{
				Hediff hd;
				HediffComp_ReplaceHediff hediffComp_ReplaceHediff;
				if (this.Pawn.health.hediffSet.TryGetHediff(def, out hd) && hd.TryGetComp(out hediffComp_ReplaceHediff))
				{
					hediffComp_ReplaceHediff.Trigger();
				}
			}
			BaseCreepJoinerWorker downsideWorker = this.DownsideWorker;
			if (downsideWorker != null)
			{
				downsideWorker.DoResponse(list, list2);
			}
			if (this.downside.hasLetter)
			{
				TaggedString label = this.downside.letterLabel.Formatted(list2);
				TaggedString text = this.downside.letterDesc.Formatted(list2);
				Find.LetterStack.ReceiveLetter(label, text, this.downside.letterDef, list, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600CC2C RID: 52268 RVA: 0x003BD0E0 File Offset: 0x003BB2E0
		public void DoRejection()
		{
			if (this.Disabled || this.triggeredRejection || this.triggeredAggressive || this.hasLeft)
			{
				return;
			}
			if (this.RejectionWorker != null && !this.RejectionWorker.CanDoResponse())
			{
				return;
			}
			this.triggeredRejection = true;
			this.ClearLord();
			List<TargetInfo> list = new List<TargetInfo>
			{
				this.Pawn
			};
			List<NamedArgument> list2 = new List<NamedArgument>
			{
				this.Pawn.Named("PAWN")
			};
			BaseCreepJoinerWorker rejectionWorker = this.RejectionWorker;
			if (rejectionWorker != null)
			{
				rejectionWorker.DoResponse(list, list2);
			}
			if (this.rejection.hasLetter)
			{
				TaggedString label = this.rejection.letterLabel.Formatted(list2);
				TaggedString text = this.rejection.letterDesc.Formatted(list2);
				Find.LetterStack.ReceiveLetter(label, text, this.rejection.letterDef, list, null, null, null, null, 0, true);
			}
		}

		// Token: 0x17001F21 RID: 7969
		// (get) Token: 0x0600CC2D RID: 52269 RVA: 0x003BD1CB File Offset: 0x003BB3CB
		public bool CanTriggerAggressive
		{
			get
			{
				return !this.Disabled && !this.triggeredAggressive && (this.AggressiveWorker == null || this.AggressiveWorker.CanDoResponse());
			}
		}

		// Token: 0x0600CC2E RID: 52270 RVA: 0x003BD1F4 File Offset: 0x003BB3F4
		public void DoAggressive()
		{
			if (!this.CanTriggerAggressive)
			{
				return;
			}
			this.triggeredAggressive = true;
			this.ClearLord();
			List<TargetInfo> list = new List<TargetInfo>
			{
				this.Pawn
			};
			List<NamedArgument> list2 = new List<NamedArgument>
			{
				this.Pawn.Named("PAWN")
			};
			BaseCreepJoinerWorker aggressiveWorker = this.AggressiveWorker;
			if (aggressiveWorker != null)
			{
				aggressiveWorker.DoResponse(list, list2);
			}
			if (this.aggressive.hasMessage)
			{
				Messages.Message(this.aggressive.message.Formatted(list2), list, MessageTypeDefOf.NegativeEvent, true);
			}
			if (this.aggressive.hasLetter)
			{
				TaggedString label = this.aggressive.letterLabel.Formatted(list2);
				TaggedString text = this.aggressive.letterDesc.Formatted(list2);
				Find.LetterStack.ReceiveLetter(label, text, this.aggressive.letterDef, list, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600CC2F RID: 52271 RVA: 0x003BD2E8 File Offset: 0x003BB4E8
		public void DoLeave()
		{
			if (this.Disabled || this.hasLeft)
			{
				return;
			}
			this.ClearLord();
			if (this.Pawn.Faction != null && this.Pawn.Faction.IsPlayer)
			{
				this.Pawn.SetFaction(null, null);
			}
			LordMaker.MakeNewLord(this.Pawn.Faction, new LordJob_ExitMapBest(LocomotionUrgency.Jog, false, false), this.Pawn.Map, null).AddPawn(this.Pawn);
			this.hasLeft = true;
		}

		// Token: 0x0600CC30 RID: 52272 RVA: 0x003BD370 File Offset: 0x003BB570
		public bool DoSurgicalInspection(Pawn surgeon, StringBuilder sb)
		{
			bool result = false;
			if (!this.benefit.surgicalInspectionLetterExtra.NullOrEmpty())
			{
				sb.Append("\n\n" + this.benefit.surgicalInspectionLetterExtra.Formatted(this.Pawn.Named("PAWN"), surgeon.Named("SURGEON")));
				result = true;
			}
			if (!this.downside.surgicalInspectionLetterExtra.NullOrEmpty())
			{
				sb.Append("\n\n" + this.downside.surgicalInspectionLetterExtra.Formatted(this.Pawn.Named("PAWN"), surgeon.Named("SURGEON")));
				result = true;
			}
			if (!this.aggressive.surgicalInspectionLetterExtra.NullOrEmpty())
			{
				sb.Append("\n\n" + this.aggressive.surgicalInspectionLetterExtra.Formatted(this.Pawn.Named("PAWN"), surgeon.Named("SURGEON")));
				result = true;
			}
			if (!this.rejection.surgicalInspectionLetterExtra.NullOrEmpty())
			{
				sb.Append("\n\n" + this.rejection.surgicalInspectionLetterExtra.Formatted(this.Pawn.Named("PAWN"), surgeon.Named("SURGEON")));
				result = true;
			}
			return result;
		}

		// Token: 0x0600CC31 RID: 52273 RVA: 0x003BD4D4 File Offset: 0x003BB6D4
		public IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			if (this.Disabled || !this.IsOnEntryLord)
			{
				yield break;
			}
			FloatMenuOption floatMenuOption;
			if (!selPawn.CanReach(this.Pawn, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				floatMenuOption = new FloatMenuOption("CannotTalkTo".Translate(this.Pawn) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else if (!selPawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking))
			{
				floatMenuOption = new FloatMenuOption("CannotTalkTo".Translate(this.Pawn) + ": " + "Incapable".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else
			{
				floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TalkTo".Translate(this.Pawn), delegate()
				{
					Job job = JobMaker.MakeJob(JobDefOf.TalkCreepJoiner, this.Pawn);
					job.playerForced = true;
					selPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this.Pawn, "ReservedBy", null);
			}
			yield return floatMenuOption;
			yield break;
		}

		// Token: 0x0600CC32 RID: 52274 RVA: 0x003BD4EC File Offset: 0x003BB6EC
		private void ResolveGraphics()
		{
			if (!this.form.forcedHeadTypes.NullOrEmpty<HeadTypeDef>())
			{
				this.Pawn.story.TryGetRandomHeadFromSet(this.form.forcedHeadTypes);
			}
			if (this.form.hairTagFilter != null)
			{
				this.Pawn.story.hairDef = PawnStyleItemChooser.RandomHairFor(this.Pawn);
			}
			if (this.form.beardTagFilter != null)
			{
				this.Pawn.style.beardDef = PawnStyleItemChooser.RandomBeardFor(this.Pawn);
			}
			if (this.form.hairColorOverride != null)
			{
				this.Pawn.story.HairColor = this.form.hairColorOverride.Value;
			}
			this.Pawn.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x0600CC33 RID: 52275 RVA: 0x003BD5C0 File Offset: 0x003BB7C0
		private void ClearLord()
		{
			if (this.IsOnEntryLord)
			{
				this.entryLordEnded = true;
			}
			Lord lord = this.Pawn.GetLord();
			if (lord == null)
			{
				return;
			}
			lord.Notify_PawnLost(this.Pawn, PawnLostCondition.Undefined, null);
		}

		// Token: 0x0600CC34 RID: 52276 RVA: 0x003BD604 File Offset: 0x003BB804
		private T GetWorker<T>(Type type, ref T worker) where T : BaseCreepJoinerWorker
		{
			if (worker != null)
			{
				return worker;
			}
			if (type == null)
			{
				return default(T);
			}
			worker = (T)((object)Activator.CreateInstance(type));
			worker.Tracker = this;
			return worker;
		}

		// Token: 0x0600CC35 RID: 52277 RVA: 0x003BD65C File Offset: 0x003BB85C
		public void ExposeData()
		{
			Scribe_Defs.Look<CreepJoinerFormKindDef>(ref this.form, "form");
			Scribe_Defs.Look<CreepJoinerBenefitDef>(ref this.benefit, "benefit");
			Scribe_Defs.Look<CreepJoinerDownsideDef>(ref this.downside, "downside");
			Scribe_Defs.Look<CreepJoinerRejectionDef>(ref this.rejection, "rejection");
			Scribe_Defs.Look<CreepJoinerAggressiveDef>(ref this.aggressive, "aggressive");
			Scribe_Values.Look<int>(ref this.timeoutAt, "timeoutAt", 0, false);
			Scribe_Values.Look<bool>(ref this.duplicated, "duplicated", false, false);
			Scribe_Values.Look<int>(ref this.joinedTick, "joinedTick", 0, false);
			Scribe_Values.Look<string>(ref this.spokeToSignal, "spokeToSignal", null, false);
			Scribe_Values.Look<bool>(ref this.triggeredDownside, "triggeredDownside", false, false);
			Scribe_Values.Look<bool>(ref this.triggeredAggressive, "triggeredAggressive", false, false);
			Scribe_Values.Look<bool>(ref this.triggeredRejection, "triggeredRejection", false, false);
			Scribe_Values.Look<bool>(ref this.hasLeft, "hasLeft", false, false);
			Scribe_Values.Look<int>(ref this.canTriggerDownsideAfter, "canTriggerDownsideAfter", 0, false);
			Scribe_Values.Look<int>(ref this.downsideTriggersAt, "downsideTriggersAt", 0, false);
			Scribe_Values.Look<bool>(ref this.entryLordEnded, "entryLordEnded", false, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_References.Look<Pawn>(ref this.speaker, "speaker", false);
		}

		// Token: 0x04008B1B RID: 35611
		private static readonly IntRange ReleaseRejectionDelayTicks = new IntRange(60, 180);

		// Token: 0x04008B1C RID: 35612
		private const int CheckDownsideInterval = 15000;

		// Token: 0x04008B1D RID: 35613
		public CreepJoinerFormKindDef form;

		// Token: 0x04008B1E RID: 35614
		public CreepJoinerBenefitDef benefit;

		// Token: 0x04008B1F RID: 35615
		public CreepJoinerDownsideDef downside;

		// Token: 0x04008B20 RID: 35616
		public CreepJoinerAggressiveDef aggressive;

		// Token: 0x04008B21 RID: 35617
		public CreepJoinerRejectionDef rejection;

		// Token: 0x04008B22 RID: 35618
		public Pawn speaker;

		// Token: 0x04008B23 RID: 35619
		public Quest quest;

		// Token: 0x04008B24 RID: 35620
		public string spokeToSignal;

		// Token: 0x04008B25 RID: 35621
		public int timeoutAt;

		// Token: 0x04008B26 RID: 35622
		public int joinedTick;

		// Token: 0x04008B27 RID: 35623
		private int downsideTriggersAt;

		// Token: 0x04008B28 RID: 35624
		private int canTriggerDownsideAfter;

		// Token: 0x04008B29 RID: 35625
		private bool triggeredDownside;

		// Token: 0x04008B2A RID: 35626
		private bool triggeredAggressive;

		// Token: 0x04008B2B RID: 35627
		private bool triggeredRejection;

		// Token: 0x04008B2C RID: 35628
		private bool hasLeft;

		// Token: 0x04008B2D RID: 35629
		private bool entryLordEnded;

		// Token: 0x04008B2E RID: 35630
		private int triggerPrisonerRejectionAt;

		// Token: 0x04008B2F RID: 35631
		private int triggerPrisonerAggressiveAt;

		// Token: 0x04008B30 RID: 35632
		private bool duplicated;

		// Token: 0x04008B31 RID: 35633
		private BaseCreepJoinerWorker downsideWorkerInt;

		// Token: 0x04008B32 RID: 35634
		private BaseCreepJoinerWorker rejectionWorkerInt;

		// Token: 0x04008B33 RID: 35635
		private BaseCreepJoinerWorker aggressiveWorkerInt;
	}
}
