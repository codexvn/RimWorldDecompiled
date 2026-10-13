using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020016BD RID: 5821
	public class LordJob_Ritual : LordJob_Joinable_Gathering
	{
		// Token: 0x170014F9 RID: 5369
		// (get) Token: 0x06008927 RID: 35111 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170014FA RID: 5370
		// (get) Token: 0x06008928 RID: 35112 RVA: 0x0028E469 File Offset: 0x0028C669
		public Precept_Ritual Ritual
		{
			get
			{
				return this.ritual;
			}
		}

		// Token: 0x170014FB RID: 5371
		// (get) Token: 0x06008929 RID: 35113 RVA: 0x0028E474 File Offset: 0x0028C674
		public virtual string RitualLabel
		{
			get
			{
				if (this.ritual == null)
				{
					return "Ritual".Translate().Resolve();
				}
				return this.ritual.LabelCap;
			}
		}

		// Token: 0x170014FC RID: 5372
		// (get) Token: 0x0600892A RID: 35114 RVA: 0x0028E4A7 File Offset: 0x0028C6A7
		protected string CallOffSignal
		{
			get
			{
				return this.lord.GetUniqueLoadID() + ".callOffRitual";
			}
		}

		// Token: 0x170014FD RID: 5373
		// (get) Token: 0x0600892B RID: 35115 RVA: 0x0028E4BE File Offset: 0x0028C6BE
		protected string CancelSignal
		{
			get
			{
				return this.lord.GetUniqueLoadID() + ".cancelRitual";
			}
		}

		// Token: 0x170014FE RID: 5374
		// (get) Token: 0x0600892C RID: 35116 RVA: 0x0028E4D5 File Offset: 0x0028C6D5
		public override int TicksLeft
		{
			get
			{
				return (int)((float)this.durationTicks - this.ticksPassedWithProgress);
			}
		}

		// Token: 0x170014FF RID: 5375
		// (get) Token: 0x0600892D RID: 35117 RVA: 0x0028E4E6 File Offset: 0x0028C6E6
		public float Progress
		{
			get
			{
				return this.ticksPassedWithProgress / (float)this.durationTicks;
			}
		}

		// Token: 0x17001500 RID: 5376
		// (get) Token: 0x0600892E RID: 35118 RVA: 0x0028E4F6 File Offset: 0x0028C6F6
		public float TicksPassedWithProgress
		{
			get
			{
				return this.ticksPassedWithProgress;
			}
		}

		// Token: 0x17001501 RID: 5377
		// (get) Token: 0x0600892F RID: 35119 RVA: 0x0028E4FE File Offset: 0x0028C6FE
		public int StageIndex
		{
			get
			{
				return this.stageIndex;
			}
		}

		// Token: 0x17001502 RID: 5378
		// (get) Token: 0x06008930 RID: 35120 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DontInterruptLayingPawnsOnCleanup
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001503 RID: 5379
		// (get) Token: 0x06008931 RID: 35121 RVA: 0x0028E506 File Offset: 0x0028C706
		public RitualStage CurrentStage
		{
			get
			{
				if (this.StageIndex < 0 || this.stages.NullOrEmpty<RitualStage>() || this.StageIndex >= this.stages.Count)
				{
					return null;
				}
				return this.stages[this.StageIndex];
			}
		}

		// Token: 0x17001504 RID: 5380
		// (get) Token: 0x06008932 RID: 35122 RVA: 0x0028E544 File Offset: 0x0028C744
		protected string TimeLeftPostfix
		{
			get
			{
				if (this.durationTicks <= 0)
				{
					return "";
				}
				return " (" + "RitualEndsIn".Translate(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false)).Resolve() + ")";
			}
		}

		// Token: 0x17001505 RID: 5381
		// (get) Token: 0x06008933 RID: 35123 RVA: 0x0028E596 File Offset: 0x0028C796
		protected virtual int MinTicksToFinish
		{
			get
			{
				if (this.ignoreDurationToFinish)
				{
					return -1;
				}
				if (this.lastEssentialStageIndex != -1)
				{
					return this.lastEssentialStageEndedTick + 1;
				}
				return this.durationTicks;
			}
		}

		// Token: 0x17001506 RID: 5382
		// (get) Token: 0x06008934 RID: 35124 RVA: 0x0028E5BA File Offset: 0x0028C7BA
		public Room GetRoom
		{
			get
			{
				return this.spot.GetRoom(base.Map);
			}
		}

		// Token: 0x17001507 RID: 5383
		// (get) Token: 0x06008935 RID: 35125 RVA: 0x0028E5D0 File Offset: 0x0028C7D0
		public IntVec2 RoomBoundsCached
		{
			get
			{
				if (this.roomBoundsCached.IsInvalid)
				{
					new IntVec2(int.MaxValue, int.MaxValue);
					new IntVec2(int.MinValue, int.MinValue);
					Room getRoom = this.GetRoom;
					if (getRoom == null || getRoom.PsychologicallyOutdoors || !getRoom.ProperRoom)
					{
						this.roomBoundsCached = IntVec2.Invalid;
					}
					else
					{
						this.roomBoundsCached = new IntVec2(getRoom.ExtentsClose.Width, getRoom.ExtentsClose.Height);
					}
				}
				return this.roomBoundsCached;
			}
		}

		// Token: 0x17001508 RID: 5384
		// (get) Token: 0x06008936 RID: 35126 RVA: 0x0028E65E File Offset: 0x0028C85E
		public Sustainer AmbiencePlaying
		{
			get
			{
				Sustainer soundPlaying;
				if ((soundPlaying = this.ambiencePlaying) == null)
				{
					Precept_Ritual precept_Ritual = this.ritual;
					if (precept_Ritual == null)
					{
						return null;
					}
					RitualBehaviorWorker behavior = precept_Ritual.behavior;
					if (behavior == null)
					{
						return null;
					}
					soundPlaying = behavior.SoundPlaying;
				}
				return soundPlaying;
			}
		}

		// Token: 0x06008937 RID: 35127 RVA: 0x0028E686 File Offset: 0x0028C886
		public override AcceptanceReport AllowsDrafting(Pawn pawn)
		{
			return new AcceptanceReport("ParticipatingInRitual".Translate(pawn, this.RitualLabel));
		}

		// Token: 0x06008938 RID: 35128 RVA: 0x0028E6B0 File Offset: 0x0028C8B0
		public LordJob_Ritual()
		{
		}

		// Token: 0x06008939 RID: 35129 RVA: 0x0028E74C File Offset: 0x0028C94C
		public LordJob_Ritual(TargetInfo selectedTarget, Precept_Ritual ritual, RitualObligation obligation, List<RitualStage> allStages, RitualRoleAssignments assignments, Pawn organizer = null, IntVec3? spotOverride = null)
		{
			this.spot = (spotOverride ?? selectedTarget.CenterCell);
			this.selectedTarget = selectedTarget;
			this.ritual = ritual;
			this.obligation = obligation;
			this.stages = allStages;
			this.organizer = organizer;
			this.assignments = assignments;
			this.durationTicks = ritual.behavior.def.durationTicks.RandomInRange;
			this.repeatPenalty = ritual.RepeatPenaltyActive;
		}

		// Token: 0x0600893A RID: 35130 RVA: 0x0028E85C File Offset: 0x0028CA5C
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			if (reason == PawnLostCondition.Incapped)
			{
				RitualRoleAssignments ritualRoleAssignments = this.assignments;
				bool flag;
				if (ritualRoleAssignments == null)
				{
					flag = false;
				}
				else
				{
					RitualRole ritualRole = ritualRoleAssignments.RoleForPawn(p, true);
					bool? flag2 = (ritualRole != null) ? new bool?(ritualRole.allowDowned) : null;
					bool flag3 = true;
					flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
				}
				if (flag)
				{
					return false;
				}
			}
			return base.ShouldRemovePawn(p, reason);
		}

		// Token: 0x0600893B RID: 35131 RVA: 0x0028E8BC File Offset: 0x0028CABC
		public override bool EndPawnJobOnCleanup(Pawn p)
		{
			RitualRoleAssignments ritualRoleAssignments = this.assignments;
			bool? flag;
			if (ritualRoleAssignments == null)
			{
				flag = null;
			}
			else
			{
				RitualRole ritualRole = ritualRoleAssignments.RoleForPawn(p, true);
				flag = ((ritualRole != null) ? new bool?(ritualRole.endJobOnRitualCleanup) : null);
			}
			bool? flag2 = flag;
			if (flag2 == null)
			{
				return base.EndPawnJobOnCleanup(p);
			}
			return flag2.GetValueOrDefault();
		}

		// Token: 0x0600893C RID: 35132 RVA: 0x0028E918 File Offset: 0x0028CB18
		public void PreparePawns()
		{
			LordJob_Ritual.tmpParticipants.Clear();
			LordJob_Ritual.tmpParticipants.AddRange(this.assignments.Participants);
			foreach (Pawn pawn in LordJob_Ritual.tmpParticipants)
			{
				if (pawn.drafter != null)
				{
					pawn.drafter.Drafted = false;
				}
				if (!pawn.Awake())
				{
					RestUtility.WakeUp(pawn, true);
				}
			}
		}

		// Token: 0x0600893D RID: 35133 RVA: 0x0028E686 File Offset: 0x0028C886
		public override AcceptanceReport AllowsFloatMenu(Pawn pawn)
		{
			return new AcceptanceReport("ParticipatingInRitual".Translate(pawn, this.RitualLabel));
		}

		// Token: 0x0600893E RID: 35134 RVA: 0x0028E9A8 File Offset: 0x0028CBA8
		public override bool BlocksSocialInteraction(Pawn pawn)
		{
			RitualRole ritualRole = this.RoleFor(pawn, true);
			return ritualRole != null && ritualRole.blocksSocialInteraction;
		}

		// Token: 0x0600893F RID: 35135 RVA: 0x0028E9C0 File Offset: 0x0028CBC0
		public override bool DutyActiveWhenDown(Pawn pawn)
		{
			RitualRoleAssignments ritualRoleAssignments = this.assignments;
			RitualRole ritualRole = (ritualRoleAssignments != null) ? ritualRoleAssignments.RoleForPawn(pawn, true) : null;
			return (ritualRole != null && ritualRole.allowDowned) || base.DutyActiveWhenDown(pawn);
		}

		// Token: 0x06008940 RID: 35136 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool PrisonerSecure(Pawn pawn)
		{
			return true;
		}

		// Token: 0x06008941 RID: 35137 RVA: 0x0028E9F6 File Offset: 0x0028CBF6
		public override AcceptanceReport AbilityAllowed(Ability ability)
		{
			return new AcceptanceReport("AbilityDisabledInRitual".Translate(ability.pawn, this.RitualLabel));
		}

		// Token: 0x06008942 RID: 35138 RVA: 0x0028EA24 File Offset: 0x0028CC24
		public override void Notify_AddedToLord()
		{
			this.stageSecondFocus = new List<TargetInfo>();
			this.stagePawnSecondFocus = new List<RitualStagePawnSecondFocus>();
			this.ritualStagePositions = new List<RitualStagePositions>();
			this.ritualStageOnTickActions = new List<RitualStageOnTickActions>();
			if (this.stages != null)
			{
				for (int i = 0; i < this.stages.Count; i++)
				{
					RitualStage ritualStage = this.stages[i];
					if (ritualStage.essential && ritualStage != this.stages.Last<RitualStage>())
					{
						this.lastEssentialStageIndex = i;
					}
					this.stageSecondFocus.Add(ritualStage.GetSecondFocus(this));
					IEnumerable<RitualStagePawnSecondFocus> pawnSecondFoci = ritualStage.GetPawnSecondFoci(this);
					if (pawnSecondFoci != null)
					{
						foreach (RitualStagePawnSecondFocus ritualStagePawnSecondFocus in pawnSecondFoci)
						{
							ritualStagePawnSecondFocus.stageIndex = i;
							this.stagePawnSecondFocus.Add(ritualStagePawnSecondFocus);
						}
					}
					this.ritualStagePositions.Add(new RitualStagePositions());
					foreach (Pawn pawn in this.assignments.Participants)
					{
						PawnStagePosition pawnPosition = ritualStage.GetPawnPosition(this.spot, pawn, this, null);
						this.ritualStagePositions[i].referencePositions.Add(new PawnRitualReference(pawn), pawnPosition);
					}
					List<ActionOnTick> list = new List<ActionOnTick>();
					if (ritualStage.tickActionMaker != null)
					{
						list.AddRange(ritualStage.tickActionMaker.GenerateTimedActions(this, ritualStage));
					}
					this.ritualStageOnTickActions.Add(new RitualStageOnTickActions
					{
						actions = list
					});
				}
			}
			if (this.ritual != null)
			{
				Ideo ideo = this.ritual.ideo;
				if (this.ritual.behavior != null && this.assignments != null && !this.ritual.behavior.def.useVisualEffectsFromRoleIdIdeo.NullOrEmpty())
				{
					Pawn pawn2 = this.assignments.FirstAssignedPawn(this.ritual.behavior.def.useVisualEffectsFromRoleIdIdeo);
					if (pawn2 != null)
					{
						ideo = pawn2.Ideo;
					}
				}
				if (ideo != null && this.ritual.playsIdeoMusic)
				{
					this.ambienceDef = ideo.SoundOngoingRitual;
				}
				if (this.ritual.def.usesIdeoVisualEffects && ideo != null && ideo.RitualEffect != null)
				{
					RitualVisualEffect instance = ideo.RitualEffect.GetInstance();
					instance.Setup(this, false);
					this.effectWorkers.Add(instance);
				}
			}
			bool flag = this.GetRoom != null && !this.GetRoom.PsychologicallyOutdoors && this.GetRoom.ProperRoom;
			this.lord.lordManager.stencilDrawers.Add(new StencilDrawerForCells
			{
				sourceLord = this.lord,
				cells = (flag ? this.GetRoom.Cells.ToList<IntVec3>() : null),
				center = this.selectedTarget.CenterVector3,
				dimensionsIfNoCells = LordJob_Ritual.DefaultRitualVfxScale,
				ticksLeftWithoutLord = 60
			});
			this.initedVisualEffects = true;
		}

		// Token: 0x06008943 RID: 35139 RVA: 0x0028ED44 File Offset: 0x0028CF44
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			this.toils.Clear();
			foreach (RitualStage stage2 in this.stages)
			{
				LordToil_Ritual lordToil_Ritual = this.MakeToil(stage2);
				stateGraph.AddToil(lordToil_Ritual);
				this.toils.Add(lordToil_Ritual);
			}
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			for (int i = 0; i < this.stages.Count; i++)
			{
				LordToil_Ritual lordToil_Ritual2 = this.toils[i];
				RitualStage stage = this.stages[i];
				int iCapture = i;
				lordToil_Ritual2.startAction = delegate()
				{
					this.ignoreDurationToFinish |= stage.ignoreDurationToFinishAfterStage;
					this.stageTicks = 0;
					Find.SignalManager.SendSignal(new Signal("RitualStarted", false));
					LordJob_Ritual.<>c__DisplayClass82_1 CS$<>8__locals3;
					CS$<>8__locals3.existingPosHighlights = new Dictionary<IntVec3, Mote>();
					CS$<>8__locals3.existingPawnHighlights = new Dictionary<Pawn, Mote>();
					foreach (KeyValuePair<IntVec3, Mote> keyValuePair in this.highlightedPositions)
					{
						CS$<>8__locals3.existingPosHighlights.Add(keyValuePair.Key, keyValuePair.Value);
					}
					foreach (KeyValuePair<Pawn, Mote> keyValuePair2 in this.highlightedPawns)
					{
						CS$<>8__locals3.existingPawnHighlights.Add(keyValuePair2.Key, keyValuePair2.Value);
					}
					this.highlightedPositions.Clear();
					this.highlightedPawns.Clear();
					foreach (Pawn pawn in this.assignments.Participants)
					{
						RitualRole ritualRole = this.RoleFor(pawn, true);
						if (ritualRole != null && stage.highlightRolePositions.Contains(ritualRole.id))
						{
							this.<CreateGraph>g__TryAddPosHighlight|82_11(pawn.Position, ref CS$<>8__locals3);
						}
						if (ritualRole != null && stage.highlightRolePawns.Contains(ritualRole.id))
						{
							this.<CreateGraph>g__TryAddPawnHighlight|82_12(pawn, ref CS$<>8__locals3);
						}
					}
					foreach (KeyValuePair<PawnRitualReference, PawnStagePosition> keyValuePair3 in this.ritualStagePositions[iCapture].referencePositions)
					{
						if (keyValuePair3.Value.highlight)
						{
							this.<CreateGraph>g__TryAddPosHighlight|82_11(keyValuePair3.Value.cell, ref CS$<>8__locals3);
						}
					}
					foreach (RitualVisualEffect ritualVisualEffect in this.effectWorkersCurrentStage)
					{
						ritualVisualEffect.Cleanup();
					}
					this.effectWorkersCurrentStage.Clear();
					if (stage.visualEffectDef != null)
					{
						RitualVisualEffect instance = stage.visualEffectDef.GetInstance();
						instance.Setup(this, false);
						this.effectWorkersCurrentStage.Add(instance);
					}
				};
				if (stage.endTriggers.Any((StageEndTrigger e) => e.CountsTowardsProgress))
				{
					lordToil_Ritual2.tickAction = delegate()
					{
						this.ticksPassedWithProgress += stage.ProgressPerTick(this);
					};
				}
				Transition transition = new Transition(lordToil_Ritual2, lordToil_End, false, true);
				foreach (Trigger trigger in this.CallOffTriggers())
				{
					transition.AddTrigger(trigger);
				}
				if (this.organizer != null)
				{
					transition.AddTrigger(new Trigger_PawnLost(PawnLostCondition.LeftVoluntarily, this.organizer));
				}
				transition.AddPreAction(new TransitionAction_Custom(delegate()
				{
					this.ApplyOutcome(this.<CreateGraph>g__Progress|82_3(), true, true, false);
				}));
				transition.AddPreAction(new TransitionAction_Custom(delegate()
				{
					RitualStageAction interruptedAction = stage.interruptedAction;
					if (interruptedAction == null)
					{
						return;
					}
					interruptedAction.Apply(this);
				}));
				stateGraph.AddTransition(transition, false);
				Transition transition2 = new Transition(lordToil_Ritual2, lordToil_End, false, true);
				transition2.AddTrigger(new Trigger_Signal(this.CancelSignal));
				transition2.AddPreAction(new TransitionAction_Custom(delegate()
				{
					this.ApplyOutcome(0f, false, true, true);
				}));
				transition2.AddPreAction(new TransitionAction_Custom(delegate()
				{
					RitualStageAction interruptedAction = stage.interruptedAction;
					if (interruptedAction == null)
					{
						return;
					}
					interruptedAction.Apply(this);
				}));
				stateGraph.AddTransition(transition2, false);
				if (!stage.failTriggers.NullOrEmpty<StageFailTrigger>())
				{
					using (List<StageFailTrigger>.Enumerator enumerator3 = stage.failTriggers.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							StageFailTrigger f = enumerator3.Current;
							int i1 = i;
							Transition transition3 = new Transition(lordToil_Ritual2, lordToil_End, false, true);
							transition3.AddTrigger(new Trigger_TickCondition(() => this.ticksPassed > f.allowanceTicks && f.Failed(this, this.selectedTarget, this.SecondFocusForStage(i1, null)), 1));
							transition3.AddPreAction(new TransitionAction_Message("RitualCalledOff".Translate(this.ritual.Label) + " " + "Reason".Translate() + ": " + f.Reason(this, this.selectedTarget).CapitalizeFirst(), MessageTypeDefOf.NegativeEvent, this.selectedTarget, null, 1f));
							transition3.AddPreAction(new TransitionAction_Custom(delegate()
							{
								this.ApplyOutcome(this.<CreateGraph>g__Progress|82_3(), false, false, true);
							}));
							stateGraph.AddTransition(transition3, false);
						}
					}
				}
				List<Trigger> list = new List<Trigger>();
				foreach (StageEndTrigger stageEndTrigger in stage.endTriggers)
				{
					list.Add(stageEndTrigger.MakeTrigger(this, this.selectedTarget, this.AllSecondFoci(i), stage));
				}
				bool flag = i == this.stages.Count - 1;
				Transition transition4 = new Transition(lordToil_Ritual2, flag ? lordToil_End : this.toils[i + 1], false, true);
				foreach (Trigger trigger2 in list)
				{
					transition4.AddTrigger(trigger2);
				}
				transition4.AddPreAction(new TransitionAction_Custom(delegate()
				{
					RitualStageAction preAction = stage.preAction;
					if (preAction == null)
					{
						return;
					}
					preAction.Apply(this);
				}));
				int i2 = i;
				transition4.AddPostAction(new TransitionAction_Custom(delegate()
				{
					RitualStageAction postAction = stage.postAction;
					if (postAction != null)
					{
						postAction.Apply(this);
					}
					if (i2 == this.lastEssentialStageIndex)
					{
						this.lastEssentialStageEndedTick = Mathf.FloorToInt(this.ticksPassedWithProgress);
					}
					this.stageIndex++;
				}));
				if (flag)
				{
					transition4.AddPreAction(new TransitionAction_Custom(delegate()
					{
						this.ApplyOutcome(1f, true, true, false);
					}));
				}
				stateGraph.AddTransition(transition4, false);
			}
			return stateGraph;
		}

		// Token: 0x06008944 RID: 35140 RVA: 0x0028F204 File Offset: 0x0028D404
		protected virtual IEnumerable<Trigger> CallOffTriggers()
		{
			yield return new Trigger_TickCondition(new Func<bool>(this.ShouldBeCalledOff), 1);
			yield return new Trigger_PawnKilled(this.pawnsDeathIgnored);
			yield return new Trigger_Signal(this.CallOffSignal);
			if (!this.ritual.behavior.def.cancellationTriggers.NullOrEmpty<RitualCancellationTrigger>())
			{
				foreach (RitualCancellationTrigger ritualCancellationTrigger in this.ritual.behavior.def.cancellationTriggers)
				{
					foreach (Trigger trigger in ritualCancellationTrigger.CancellationTriggers(this.assignments))
					{
						yield return trigger;
					}
					IEnumerator<Trigger> enumerator2 = null;
				}
				List<RitualCancellationTrigger>.Enumerator enumerator = default(List<RitualCancellationTrigger>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06008945 RID: 35141 RVA: 0x0028F214 File Offset: 0x0028D414
		public override string GetJobReport(Pawn pawn)
		{
			RitualRoleBehavior ritualRoleBehavior = this.BehaviorFor(pawn, true);
			if (ritualRoleBehavior == null)
			{
				return null;
			}
			return ritualRoleBehavior.GetJobReportOverride(pawn);
		}

		// Token: 0x06008946 RID: 35142 RVA: 0x0028F22C File Offset: 0x0028D42C
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttending".Translate((this.ritual != null) ? this.ritual.Label : this.gatheringDef.label) + this.TimeLeftPostfix;
		}

		// Token: 0x06008947 RID: 35143 RVA: 0x0028F278 File Offset: 0x0028D478
		protected virtual LordToil_Ritual MakeToil(RitualStage stage)
		{
			return new LordToil_Ritual(this.spot, this, stage, this.organizer);
		}

		// Token: 0x17001509 RID: 5385
		// (get) Token: 0x06008948 RID: 35144 RVA: 0x0028F28D File Offset: 0x0028D48D
		public virtual IEnumerable<Pawn> PawnsToCountTowardsPresence
		{
			get
			{
				return this.lord.ownedPawns;
			}
		}

		// Token: 0x06008949 RID: 35145 RVA: 0x0028F29A File Offset: 0x0028D49A
		protected virtual bool RitualFinished(float progress, bool cancelled)
		{
			return this.ticksPassedWithProgress >= (float)this.MinTicksToFinish && (this.lastEssentialStageIndex == -1 || this.lastEssentialStageEndedTick != -1) && !cancelled;
		}

		// Token: 0x0600894A RID: 35146 RVA: 0x0028F2C4 File Offset: 0x0028D4C4
		public virtual void ApplyOutcome(float progress, bool showFinishedMessage = true, bool showFailedMessage = true, bool cancelled = false)
		{
			if (this.ended)
			{
				return;
			}
			this.ended = true;
			this.cancelled = cancelled;
			if (this.RitualFinished(progress, cancelled))
			{
				this.totalPresenceTmp.Clear();
				foreach (LordToil_Ritual lordToil_Ritual in this.toils)
				{
					foreach (KeyValuePair<Pawn, int> keyValuePair in lordToil_Ritual.Data.presentForTicks)
					{
						if (keyValuePair.Key != null && !keyValuePair.Key.Dead)
						{
							if (!this.totalPresenceTmp.ContainsKey(keyValuePair.Key))
							{
								this.totalPresenceTmp.Add(keyValuePair.Key, keyValuePair.Value);
							}
							else
							{
								Dictionary<Pawn, int> dictionary = this.totalPresenceTmp;
								Pawn key = keyValuePair.Key;
								dictionary[key] += keyValuePair.Value;
							}
						}
					}
				}
				float tickScale = this.ticksPassedWithProgress / (float)this.ticksPassed;
				float targetDuration = (this.durationTicks > 0) ? ((float)this.durationTicks) : this.ticksPassedWithProgress;
				this.totalPresenceTmp.RemoveAll((KeyValuePair<Pawn, int> tp) => targetDuration * (float)tp.Value < tickScale / 2f);
				if (this.totalPresenceTmp.Count > 0 || this.ritual.outcomeEffect.def.allowOutcomeWithNoAttendance)
				{
					this.AddParticipantThoughts();
					try
					{
						this.ritual.outcomeEffect.Apply(progress, this.totalPresenceTmp, this);
					}
					catch (Exception ex)
					{
						string str = "Error while applying ritual outcome effect: ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
					}
					if (this.obligation != null)
					{
						this.ritual.RemoveObligation(this.obligation, true);
					}
					if (showFinishedMessage && this.ritual.behavior.def.displayCompletedMessage)
					{
						Messages.Message("RitualFinished".Translate(this.ritual.Label), new TargetInfo(this.spot, base.Map, false), MessageTypeDefOf.SilentInput, true);
					}
				}
				else
				{
					Messages.Message("RitualNobodyAttended".Translate(this.ritual.Label), new TargetInfo(this.spot, base.Map, false), MessageTypeDefOf.NegativeEvent, true);
				}
				this.totalPresenceTmp.Clear();
				if (this.Ritual != null && this.Ritual.ideo != null)
				{
					foreach (Precept precept in this.Ritual.ideo.PreceptsListForReading)
					{
						Precept_Ritual precept_Ritual = precept as Precept_Ritual;
						if (precept_Ritual != null && !precept_Ritual.obligationTriggers.NullOrEmpty<RitualObligationTrigger>())
						{
							foreach (RitualObligationTrigger ritualObligationTrigger in precept_Ritual.obligationTriggers)
							{
								ritualObligationTrigger.Notify_RitualExecuted(this);
							}
						}
					}
				}
				this.ritual.lastFinishedTick = GenTicks.TicksGame;
			}
			else
			{
				if (showFailedMessage)
				{
					Messages.Message("RitualCalledOff".Translate(this.ritual.Label).CapitalizeFirst(), new TargetInfo(this.spot, base.Map, false), MessageTypeDefOf.NegativeEvent, true);
				}
				try
				{
					if (this.ritual.outcomeEffect.ApplyOnFailure)
					{
						this.ritual.outcomeEffect.Apply(progress, this.totalPresenceTmp, this);
					}
				}
				catch (Exception ex3)
				{
					string str2 = "Error while applying ritual outcome effect: ";
					Exception ex4 = ex3;
					Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
				}
			}
			RitualOutcomeEffectWorker outcomeEffect = this.ritual.outcomeEffect;
			if (outcomeEffect != null)
			{
				outcomeEffect.ResetCompDatas();
			}
			base.Map.lordManager.RemoveLord(this.lord);
		}

		// Token: 0x0600894B RID: 35147 RVA: 0x0028F72C File Offset: 0x0028D92C
		private void AddRelicInRoomThought()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			Precept_Ritual precept_Ritual = this.Ritual;
			if (((precept_Ritual != null) ? precept_Ritual.ideo : null) == null)
			{
				return;
			}
			if (this.selectedTarget.Map == null)
			{
				return;
			}
			Room room = this.selectedTarget.Cell.GetRoom(this.selectedTarget.Map);
			if (room != null && !room.TouchesMapEdge)
			{
				int num = 0;
				string str = string.Empty;
				foreach (Thing thing in room.ContainedThings(ThingDefOf.Reliquary))
				{
					CompRelicContainer compRelicContainer = thing.TryGetComp<CompRelicContainer>();
					if (compRelicContainer != null)
					{
						ThingWithComps thingWithComps = compRelicContainer.ContainedThing as ThingWithComps;
						Precept_ThingStyle precept_ThingStyle;
						if (thingWithComps == null)
						{
							precept_ThingStyle = null;
						}
						else
						{
							CompStyleable compStyleable = thingWithComps.compStyleable;
							precept_ThingStyle = ((compStyleable != null) ? compStyleable.SourcePrecept : null);
						}
						Precept_ThingStyle precept_ThingStyle2 = precept_ThingStyle;
						if (precept_ThingStyle2 != null && precept_ThingStyle2.ideo == this.Ritual.ideo)
						{
							if (num == 0)
							{
								str = compRelicContainer.ContainedThing.Label;
							}
							num++;
						}
					}
				}
				if (num > 0)
				{
					foreach (KeyValuePair<Pawn, int> keyValuePair in this.totalPresenceTmp)
					{
						if (keyValuePair.Key.Ideo == this.Ritual.ideo)
						{
							Thought_RelicAtRitual thought_RelicAtRitual = (Thought_RelicAtRitual)ThoughtMaker.MakeThought(ThoughtDefOf.RelicAtRitual, Mathf.Min(num, ThoughtDefOf.RelicAtRitual.stages.Count) - 1);
							thought_RelicAtRitual.relicName = Find.ActiveLanguageWorker.WithDefiniteArticle(str, Gender.None, false, false);
							keyValuePair.Key.needs.mood.thoughts.memories.TryGainMemory(thought_RelicAtRitual, null);
						}
					}
				}
			}
		}

		// Token: 0x0600894C RID: 35148 RVA: 0x0028F8F8 File Offset: 0x0028DAF8
		private void AddParticipantThoughts()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			Precept_Ritual precept_Ritual = this.Ritual;
			if (((precept_Ritual != null) ? precept_Ritual.ideo : null) == null || this.Ritual.def.mergeRitualGizmosFromAllIdeos)
			{
				return;
			}
			foreach (KeyValuePair<Pawn, int> keyValuePair in this.totalPresenceTmp)
			{
				if (keyValuePair.Key.Ideo != this.Ritual.ideo)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ParticipatedInOthersRitual, keyValuePair.Key.Named(HistoryEventArgsNames.Doer)), true);
				}
			}
			this.AddRelicInRoomThought();
		}

		// Token: 0x0600894D RID: 35149 RVA: 0x0028F9B8 File Offset: 0x0028DBB8
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return this.MakeToil((!this.stages.NullOrEmpty<RitualStage>()) ? this.stages[0] : null);
		}

		// Token: 0x0600894E RID: 35150 RVA: 0x0028F9DC File Offset: 0x0028DBDC
		public bool TargetStillAllowed()
		{
			if (this.selectedTarget.ThingDestroyed)
			{
				return false;
			}
			if (this.ritual.behavior.ChecksReservations)
			{
				LordJob_Ritual.reservers.Clear();
				this.selectedTarget.Map.reservationManager.ReserversOf((LocalTargetInfo)this.selectedTarget, LordJob_Ritual.reservers);
				LordJob_Ritual.reservers.ExceptWith(this.assignments.Participants);
				if (LordJob_Ritual.reservers.Count > 0)
				{
					return false;
				}
			}
			return this.Ritual.behavior.TargetStillAllowed(this.selectedTarget, this);
		}

		// Token: 0x0600894F RID: 35151 RVA: 0x0028FA7C File Offset: 0x0028DC7C
		protected override bool ShouldBeCalledOff()
		{
			if (this.lord.ownedPawns.Count == 0)
			{
				return true;
			}
			if (!this.TargetStillAllowed())
			{
				return true;
			}
			if (this.organizer != null && this.organizer.Downed)
			{
				return true;
			}
			foreach (RitualRole ritualRole in this.assignments.AllRolesForReading)
			{
				if (ritualRole.required)
				{
					Pawn pawn = this.assignments.FirstAssignedPawn(ritualRole);
					if (pawn != null && !this.lord.ownedPawns.Contains(pawn) && this.ShouldCallOffBecausePawnNoLongerOwned(pawn))
					{
						return true;
					}
				}
			}
			foreach (Pawn pawn2 in this.assignments.ExtraRequiredPawnsForReading)
			{
				if (!this.lord.ownedPawns.Contains(pawn2) && this.ShouldCallOffBecausePawnNoLongerOwned(pawn2))
				{
					return true;
				}
				IThingHolder thingHolder;
				if (pawn2.Corpse == null || pawn2.Corpse.ParentHolder == null)
				{
					IThingHolder carriedBy = pawn2.CarriedBy;
					thingHolder = carriedBy;
				}
				else
				{
					thingHolder = pawn2.Corpse.ParentHolder.ParentHolder;
				}
				Pawn pawn3 = thingHolder as Pawn;
				if (pawn3 != null && pawn3.GetLord() != this.lord)
				{
					return true;
				}
			}
			foreach (Pawn pawn4 in this.assignments.Participants)
			{
				if (pawn4 != null && pawn4.Map != null)
				{
					RitualRole ritualRole2 = this.assignments.RoleForPawn(pawn4, true);
					if (((ritualRole2 != null && ritualRole2.required) || this.assignments.Required(pawn4) || this.assignments.ExtraRequiredPawnsForReading.Contains(pawn4)) && SelfDefenseUtility.ShouldStartFleeing(pawn4))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06008950 RID: 35152 RVA: 0x0028FCA0 File Offset: 0x0028DEA0
		protected virtual bool ShouldCallOffBecausePawnNoLongerOwned(Pawn p)
		{
			return !this.pawnsDeathIgnored.Contains(p);
		}

		// Token: 0x06008951 RID: 35153 RVA: 0x0028FCB4 File Offset: 0x0028DEB4
		public bool RoleFilled(string roleId)
		{
			return !this.ritual.behavior.def.roles.NullOrEmpty<RitualRole>() && this.ritual.behavior.def.roles.FirstOrDefault((RitualRole r) => r.id == roleId) != null && this.assignments.FirstAssignedPawn(roleId) != null;
		}

		// Token: 0x06008952 RID: 35154 RVA: 0x0028FD2A File Offset: 0x0028DF2A
		public RitualRole RoleFor(Pawn p, bool includeForced = false)
		{
			RitualRoleAssignments ritualRoleAssignments = this.assignments;
			if (ritualRoleAssignments == null)
			{
				return null;
			}
			return ritualRoleAssignments.RoleForPawn(p, includeForced);
		}

		// Token: 0x06008953 RID: 35155 RVA: 0x0028FD3F File Offset: 0x0028DF3F
		public bool TryGetRoleFor(Pawn p, out RitualRole role, bool includeForced = false)
		{
			RitualRoleAssignments ritualRoleAssignments = this.assignments;
			role = ((ritualRoleAssignments != null) ? ritualRoleAssignments.RoleForPawn(p, includeForced) : null);
			return role != null;
		}

		// Token: 0x06008954 RID: 35156 RVA: 0x0028FD5C File Offset: 0x0028DF5C
		public Pawn PawnWithRole(string roleId)
		{
			return this.assignments.FirstAssignedPawn(roleId);
		}

		// Token: 0x06008955 RID: 35157 RVA: 0x0028FD6C File Offset: 0x0028DF6C
		public RitualRole GetRole(string id)
		{
			if (this.ritual.behavior.def.roles.NullOrEmpty<RitualRole>())
			{
				return null;
			}
			return this.ritual.behavior.def.roles.FirstOrDefault((RitualRole r) => r.id == id);
		}

		// Token: 0x06008956 RID: 35158 RVA: 0x0028FDCA File Offset: 0x0028DFCA
		public RitualRoleBehavior BehaviorFor(Pawn pawn, bool includeForced)
		{
			RitualStage currentStage = this.CurrentStage;
			if (currentStage == null)
			{
				return null;
			}
			RitualRole ritualRole = this.RoleFor(pawn, includeForced);
			return currentStage.BehaviorForRole((ritualRole != null) ? ritualRole.id : null);
		}

		// Token: 0x06008957 RID: 35159 RVA: 0x0028FDF1 File Offset: 0x0028DFF1
		public override IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
		{
			RitualRole ritualRole;
			if (p != this.organizer && !this.assignments.ExtraRequiredPawnsForReading.Contains(p) && (!this.TryGetRoleFor(p, out ritualRole, false) || !ritualRole.required))
			{
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "CommandLeaveRitual".Translate(this.ritual.Named("RITUAL")),
					defaultDesc = "CommandLeaveRitualDesc".Translate(this.ritual.Named("RITUAL")),
					icon = this.ritual.CancelIcon
				};
				if (!this.ritual.def.mergeRitualGizmosFromAllIdeos)
				{
					command_Action.defaultIconColor = this.ritual.ideo.Color;
				}
				command_Action.action = delegate()
				{
					this.pawnsForcedToLeave.Add(p);
					this.lord.Notify_PawnLost(p, PawnLostCondition.ForcedByPlayerAction, null);
					Pawn_JobTracker jobs = p.jobs;
					if (jobs != null)
					{
						jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				};
				if (this.lord.ownedPawns.Count < 2)
				{
					command_Action.Disable("CommandLeaveLastParticipant".Translate(this.ritual.Named("RITUAL")));
				}
				command_Action.hotKey = KeyBindingDefOf.Misc5;
				yield return command_Action;
			}
			yield return this.GetCancelGizmo();
			yield break;
		}

		// Token: 0x06008958 RID: 35160 RVA: 0x0028FE08 File Offset: 0x0028E008
		public Gizmo GetCancelGizmo()
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandCancelRitual".Translate(this.ritual.Named("RITUAL"));
			command_Action.defaultDesc = "CommandCancelRitualDesc".Translate(this.ritual.Named("RITUAL"));
			command_Action.icon = this.ritual.CancelIcon;
			if (!this.ritual.def.mergeRitualGizmosFromAllIdeos)
			{
				command_Action.defaultIconColor = this.ritual.ideo.Color;
			}
			command_Action.action = delegate()
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CommandCancelRitualConfirm".Translate(this.ritual.Named("RITUAL")), new Action(this.Cancel), false, null, WindowLayer.Dialog));
				SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
			};
			command_Action.hotKey = KeyBindingDefOf.Misc6;
			return command_Action;
		}

		// Token: 0x06008959 RID: 35161 RVA: 0x0028FEBC File Offset: 0x0028E0BC
		public void Cancel()
		{
			Find.SignalManager.SendSignal(new Signal(this.CancelSignal, false));
		}

		// Token: 0x0600895A RID: 35162 RVA: 0x0028FED4 File Offset: 0x0028E0D4
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			if (!this.IsInvited(p))
			{
				return 0f;
			}
			RitualRole ritualRole = this.RoleFor(p, true);
			bool flag;
			if (ritualRole == null)
			{
				Precept_Ritual precept_Ritual = this.ritual;
				flag = (precept_Ritual != null && precept_Ritual.behavior.def.spectatorsIgnoreBleeding);
			}
			else
			{
				flag = ritualRole.ignoreBleeding;
			}
			bool ignoreBleeding = flag;
			if (!GatheringsUtility.ShouldPawnKeepAttendingRitual(p, this.ritual, ignoreBleeding))
			{
				return 0f;
			}
			if (this.spot.IsForbidden(p))
			{
				return 0f;
			}
			if (!this.lord.ownedPawns.Contains(p) && base.IsGatheringAboutToEnd())
			{
				return 0f;
			}
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.blocksSocialInteraction)
				{
					return 0f;
				}
			}
			return VoluntarilyJoinableLordJobJoinPriorities.SocialGathering;
		}

		// Token: 0x0600895B RID: 35163 RVA: 0x0028FFAD File Offset: 0x0028E1AD
		protected override bool IsInvited(Pawn p)
		{
			return !this.pawnsForcedToLeave.Contains(p) && (this.assignments == null || this.assignments.PawnParticipating(p));
		}

		// Token: 0x0600895C RID: 35164 RVA: 0x0028FFD8 File Offset: 0x0028E1D8
		public override void LordJobTick()
		{
			base.LordJobTick();
			Building edifice = this.spot.GetEdifice(base.Map);
			TargetInfo targetInfo = (edifice != null) ? edifice : new TargetInfo(this.spot, base.Map, false);
			if (this.CurrentStage == null || this.CurrentStage.showProgressBar)
			{
				if (this.progressBar == null)
				{
					this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
				}
				this.progressBar.EffectTick(targetInfo, TargetInfo.Invalid);
				MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBar.children[0]).mote;
				if (mote != null)
				{
					mote.progress = (this.progressBarOverride ?? (1f - Mathf.Clamp01((float)this.TicksLeft / (float)base.DurationTicks)));
					mote.offsetZ = -0.5f;
				}
			}
			else if (this.progressBar != null)
			{
				this.progressBar.Cleanup();
				this.progressBar = null;
			}
			if (this.ritual != null)
			{
				LordToil_Ritual lordToil_Ritual = this.lord.CurLordToil as LordToil_Ritual;
				if (lordToil_Ritual != null && lordToil_Ritual.stage != null)
				{
					this.ritual.outcomeEffect.Tick(this, lordToil_Ritual.stage.ProgressPerTick(this));
				}
				else
				{
					this.ritual.outcomeEffect.Tick(this, 1f);
				}
			}
			if (this.ritual != null && this.ritual.behavior != null)
			{
				this.ritual.behavior.Tick(this);
			}
			if (DebugSettings.playRitualAmbience && this.ambienceDef != null && (this.ambiencePlaying == null || this.ambiencePlaying.Ended))
			{
				this.ambiencePlaying = this.ambienceDef.TrySpawnSustainer(SoundInfo.InMap(targetInfo, MaintenanceType.PerTick));
			}
			Sustainer sustainer = this.ambiencePlaying;
			if (sustainer != null)
			{
				sustainer.Maintain();
			}
			LordToil_Ritual lordToil_Ritual2 = this.lord.CurLordToil as LordToil_Ritual;
			if (lordToil_Ritual2 != null)
			{
				RitualStage stage = lordToil_Ritual2.stage;
				if (stage != null)
				{
					foreach (ActionOnTick actionOnTick in this.ritualStageOnTickActions[this.stages.IndexOf(stage)].actions)
					{
						if (actionOnTick.tick == this.stageTicks)
						{
							try
							{
								actionOnTick.Apply(this);
							}
							catch (Exception ex)
							{
								string str = "Error while applying ritual on-tick action: ";
								Exception ex2 = ex;
								Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
							}
						}
					}
				}
			}
			if (!this.initedVisualEffects)
			{
				foreach (RitualVisualEffect ritualVisualEffect in this.effectWorkers)
				{
					ritualVisualEffect.ritual = this;
					ritualVisualEffect.Setup(this, true);
				}
				foreach (RitualVisualEffect ritualVisualEffect2 in this.effectWorkersCurrentStage)
				{
					ritualVisualEffect2.ritual = this;
					ritualVisualEffect2.Setup(this, true);
				}
				this.initedVisualEffects = true;
			}
			foreach (RitualVisualEffect ritualVisualEffect3 in this.effectWorkers)
			{
				ritualVisualEffect3.Tick();
			}
			foreach (RitualVisualEffect ritualVisualEffect4 in this.effectWorkersCurrentStage)
			{
				ritualVisualEffect4.Tick();
			}
			foreach (KeyValuePair<IntVec3, Mote> keyValuePair in this.highlightedPositions)
			{
				keyValuePair.Value.Maintain();
			}
			foreach (KeyValuePair<Pawn, Mote> keyValuePair2 in this.highlightedPawns)
			{
				keyValuePair2.Value.Maintain();
			}
			this.ticksPassed++;
			this.stageTicks++;
			if (this.lastEssentialStageEndedTick != -1 && this.ticksPassed > this.lastEssentialStageEndedTick)
			{
				this.ticksSinceLastEssentialStage++;
			}
		}

		// Token: 0x0600895D RID: 35165 RVA: 0x00290468 File Offset: 0x0028E668
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.progressBar != null)
			{
				this.progressBar.Cleanup();
				this.progressBar = null;
			}
			Precept_Ritual precept_Ritual = this.ritual;
			if (precept_Ritual != null)
			{
				RitualBehaviorWorker behavior = precept_Ritual.behavior;
				if (behavior != null)
				{
					behavior.Cleanup(this);
				}
			}
			this.highlightedPositions.Clear();
			this.highlightedPawns.Clear();
			foreach (RitualVisualEffect ritualVisualEffect in this.effectWorkers)
			{
				ritualVisualEffect.Cleanup();
			}
			foreach (RitualVisualEffect ritualVisualEffect2 in this.effectWorkersCurrentStage)
			{
				ritualVisualEffect2.Cleanup();
			}
		}

		// Token: 0x0600895E RID: 35166 RVA: 0x0029054C File Offset: 0x0028E74C
		public override void PostCleanup()
		{
			Precept_Ritual precept_Ritual = this.ritual;
			if (precept_Ritual == null)
			{
				return;
			}
			RitualBehaviorWorker behavior = precept_Ritual.behavior;
			if (behavior == null)
			{
				return;
			}
			behavior.PostCleanup(this);
		}

		// Token: 0x0600895F RID: 35167 RVA: 0x0029056C File Offset: 0x0028E76C
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			base.Notify_PawnLost(p, condition);
			if (this.progressBar != null)
			{
				this.progressBar.Cleanup();
				this.progressBar = null;
			}
			LordToil_Ritual lordToil_Ritual = this.lord.CurLordToil as LordToil_Ritual;
			if (lordToil_Ritual != null)
			{
				RitualStage stage = lordToil_Ritual.stage;
				if (stage != null)
				{
					RitualStageAction pawnLeaveAction = stage.pawnLeaveAction;
					if (pawnLeaveAction != null)
					{
						pawnLeaveAction.ApplyToPawn(this, p);
					}
				}
			}
			if (condition == PawnLostCondition.Killed)
			{
				RitualRole ritualRole = this.assignments.RoleForPawn(p, true);
				if (ritualRole != null && !ritualRole.removeFromAssignmentsOnDeath)
				{
					return;
				}
			}
			this.assignments.RemoveParticipant(p);
		}

		// Token: 0x06008960 RID: 35168 RVA: 0x002905F8 File Offset: 0x0028E7F8
		public override void Notify_InMentalState(Pawn pawn, MentalStateDef stateDef)
		{
			base.Notify_InMentalState(pawn, stateDef);
			this.lord.Notify_PawnLost(pawn, PawnLostCondition.InMentalState, null);
		}

		// Token: 0x06008961 RID: 35169 RVA: 0x00290624 File Offset: 0x0028E824
		public void AddTagForPawn(Pawn p, string tag)
		{
			if (this.perPawnTags.ContainsKey(p))
			{
				if (!this.perPawnTags[p].tags.Contains(tag))
				{
					this.perPawnTags[p].tags.Add(tag);
					return;
				}
			}
			else
			{
				this.perPawnTags[p] = new PawnTags
				{
					tags = new List<string>
					{
						tag
					}
				};
			}
		}

		// Token: 0x06008962 RID: 35170 RVA: 0x00290693 File Offset: 0x0028E893
		public bool PawnTagSet(Pawn p, string tag)
		{
			return this.perPawnTags.ContainsKey(p) && this.perPawnTags[p].tags.Contains(tag);
		}

		// Token: 0x06008963 RID: 35171 RVA: 0x002906BC File Offset: 0x0028E8BC
		public TargetInfo SecondFocusForStage(int index, Pawn forPawn = null)
		{
			if (forPawn != null)
			{
				foreach (RitualStagePawnSecondFocus ritualStagePawnSecondFocus in this.stagePawnSecondFocus)
				{
					if (ritualStagePawnSecondFocus.stageIndex == index && ritualStagePawnSecondFocus.pawn == forPawn)
					{
						return ritualStagePawnSecondFocus.target;
					}
				}
			}
			return this.stageSecondFocus[index];
		}

		// Token: 0x06008964 RID: 35172 RVA: 0x00290734 File Offset: 0x0028E934
		public IEnumerable<TargetInfo> AllSecondFoci(int index)
		{
			yield return this.stageSecondFocus[index];
			foreach (RitualStagePawnSecondFocus ritualStagePawnSecondFocus in this.stagePawnSecondFocus)
			{
				if (ritualStagePawnSecondFocus.stageIndex == index)
				{
					yield return ritualStagePawnSecondFocus.target;
				}
			}
			List<RitualStagePawnSecondFocus>.Enumerator enumerator = default(List<RitualStagePawnSecondFocus>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06008965 RID: 35173 RVA: 0x0029074B File Offset: 0x0028E94B
		public TargetInfo SecondFocusForStage(RitualStage stage, Pawn forPawn = null)
		{
			return this.SecondFocusForStage(this.stages.IndexOf(stage), forPawn);
		}

		// Token: 0x06008966 RID: 35174 RVA: 0x00290760 File Offset: 0x0028E960
		public PawnStagePosition PawnPositionForStage(Pawn pawn, RitualStage stage)
		{
			int num = this.stages.IndexOf(stage);
			if (num == -1 || this.ritualStagePositions.Count <= num)
			{
				Log.Error("Invalid stage id for ritual stage position: " + num.ToString());
				return null;
			}
			foreach (KeyValuePair<PawnRitualReference, PawnStagePosition> keyValuePair in this.ritualStagePositions[num].referencePositions)
			{
				if (keyValuePair.Key.pawn == pawn)
				{
					return keyValuePair.Value;
				}
			}
			return null;
		}

		// Token: 0x06008967 RID: 35175 RVA: 0x0029080C File Offset: 0x0028EA0C
		public IntVec3 CurrentSpectatorCrowdCenter()
		{
			int num = 0;
			Vector3 vector = Vector3.zero;
			foreach (Pawn pawn in this.assignments.SpectatorsForReading)
			{
				Job curJob = pawn.CurJob;
				if (curJob != null && curJob.def == JobDefOf.SpectateCeremony)
				{
					LocalTargetInfo target = curJob.GetTarget(TargetIndex.A);
					if (target.IsValid)
					{
						vector += target.Cell.ToVector3();
						num++;
					}
				}
			}
			if (num == 0)
			{
				return IntVec3.Invalid;
			}
			vector /= (float)num;
			return new IntVec3((int)vector.x, (int)vector.y, (int)vector.z);
		}

		// Token: 0x06008968 RID: 35176 RVA: 0x002908D4 File Offset: 0x0028EAD4
		private void AddPositionHighlight(IntVec3 pos)
		{
			Mote mote = MoteMaker.MakeStaticMote(pos.ToVector3Shifted(), base.Map, ThingDefOf.Mote_RolePositionHighlight, 1f, false, 0f);
			this.highlightedPositions.Add(pos, mote);
			mote.Maintain();
		}

		// Token: 0x06008969 RID: 35177 RVA: 0x00290918 File Offset: 0x0028EB18
		private void AddPawnHighlight(Pawn pawn)
		{
			Mote mote = MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_RolePawnHighlight, new Vector3(0f, 0f, -0.4f), 1f, -1f);
			this.highlightedPawns.Add(pawn, mote);
			mote.Maintain();
		}

		// Token: 0x0600896A RID: 35178 RVA: 0x00290962 File Offset: 0x0028EB62
		public bool IsParticipating(Pawn p)
		{
			return this.lord.ownedPawns.Contains(p) && GatheringsUtility.InGatheringArea(p.Position, this.Spot, p.MapHeld);
		}

		// Token: 0x0600896B RID: 35179 RVA: 0x00290990 File Offset: 0x0028EB90
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Precept_Ritual>(ref this.ritual, "ritual", false);
			Scribe_References.Look<RitualObligation>(ref this.obligation, "obligation", false);
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
			Scribe_Values.Look<float>(ref this.ticksPassedWithProgress, "ticksPassedWithProgress", 0f, false);
			Scribe_Values.Look<float?>(ref this.progressBarOverride, "progressBarOverride", null, false);
			Scribe_Values.Look<int>(ref this.stageTicks, "stageTicks", 0, false);
			Scribe_Values.Look<int>(ref this.lastEssentialStageIndex, "lastEssentialStageIndex", 0, false);
			Scribe_Values.Look<int>(ref this.lastEssentialStageEndedTick, "lastEssentialStageEndedTick", 0, false);
			Scribe_Values.Look<int>(ref this.ticksSinceLastEssentialStage, "ticksSinceLastEssentialStage", 0, false);
			Scribe_Values.Look<bool>(ref this.repeatPenalty, "repeatPenalty", false, false);
			Scribe_Values.Look<int>(ref this.stageIndex, "stageIndex", 0, false);
			Scribe_Defs.Look<SoundDef>(ref this.ambienceDef, "ambienceDef");
			Scribe_TargetInfo.Look(ref this.selectedTarget, "selectedTarget");
			Scribe_Collections.Look<Pawn>(ref this.pawnsForcedToLeave, "pawnsForcedToLeave", LookMode.Reference);
			Scribe_Collections.Look<Thing>(ref this.usedThings, "usedThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.pawnsDeathIgnored, "pawnsDeathIgnored", true, LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn, PawnTags>(ref this.perPawnTags, "perPawnTags", LookMode.Reference, LookMode.Deep, ref this.tmpTagPawns, ref this.tmpTags, true, true, false);
			Scribe_Collections.Look<RitualStage>(ref this.stages, "stages", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<TargetInfo>(ref this.stageSecondFocus, "stageSecondFocus", LookMode.TargetInfo, Array.Empty<object>());
			Scribe_Collections.Look<RitualStagePawnSecondFocus>(ref this.stagePawnSecondFocus, "stagePawnSecondFocus", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<RitualStagePositions>(ref this.ritualStagePositions, "ritualStagePositions", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<RitualStageOnTickActions>(ref this.ritualStageOnTickActions, "ritualStageOnTickActions", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<RitualRoleAssignments>(ref this.assignments, "assignments", Array.Empty<object>());
			Scribe_Collections.Look<RitualVisualEffect>(ref this.effectWorkers, "effectWorkers", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<RitualVisualEffect>(ref this.effectWorkersCurrentStage, "effectWorkersCurrentStage", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.ignoreDurationToFinish, "ignoreDurationToFinish", false, false);
			Scribe_Values.Look<bool>(ref this.ended, "ended", false, false);
		}

		// Token: 0x0600896D RID: 35181 RVA: 0x00290BEA File Offset: 0x0028EDEA
		[CompilerGenerated]
		private void <CreateGraph>g__TryAddPosHighlight|82_11(IntVec3 cell, ref LordJob_Ritual.<>c__DisplayClass82_1 A_2)
		{
			if (!A_2.existingPosHighlights.ContainsKey(cell))
			{
				this.AddPositionHighlight(cell);
				return;
			}
			this.highlightedPositions.Add(cell, A_2.existingPosHighlights[cell]);
		}

		// Token: 0x0600896E RID: 35182 RVA: 0x00290C1A File Offset: 0x0028EE1A
		[CompilerGenerated]
		private void <CreateGraph>g__TryAddPawnHighlight|82_12(Pawn pawn, ref LordJob_Ritual.<>c__DisplayClass82_1 A_2)
		{
			if (!A_2.existingPawnHighlights.ContainsKey(pawn))
			{
				this.AddPawnHighlight(pawn);
				return;
			}
			this.highlightedPawns.Add(pawn, A_2.existingPawnHighlights[pawn]);
		}

		// Token: 0x0600896F RID: 35183 RVA: 0x00290C4C File Offset: 0x0028EE4C
		[CompilerGenerated]
		private float <CreateGraph>g__Progress|82_3()
		{
			float value = (float)this.ticksPassed / (float)this.durationTicks;
			if (this.lastEssentialStageIndex != -1)
			{
				int num = base.DurationTicks - this.lastEssentialStageEndedTick;
				value = (float)this.ticksSinceLastEssentialStage / (float)num;
			}
			return Mathf.Clamp01(value);
		}

		// Token: 0x040059A4 RID: 22948
		protected Precept_Ritual ritual;

		// Token: 0x040059A5 RID: 22949
		public RitualObligation obligation;

		// Token: 0x040059A6 RID: 22950
		public TargetInfo selectedTarget;

		// Token: 0x040059A7 RID: 22951
		public RitualRoleAssignments assignments;

		// Token: 0x040059A8 RID: 22952
		private List<RitualStage> stages;

		// Token: 0x040059A9 RID: 22953
		public List<Pawn> pawnsDeathIgnored = new List<Pawn>();

		// Token: 0x040059AA RID: 22954
		protected HashSet<Pawn> pawnsForcedToLeave = new HashSet<Pawn>();

		// Token: 0x040059AB RID: 22955
		public List<Thing> usedThings = new List<Thing>();

		// Token: 0x040059AC RID: 22956
		public Dictionary<Pawn, PawnTags> perPawnTags = new Dictionary<Pawn, PawnTags>();

		// Token: 0x040059AD RID: 22957
		protected SoundDef ambienceDef;

		// Token: 0x040059AE RID: 22958
		protected int stageTicks;

		// Token: 0x040059AF RID: 22959
		protected int ticksPassed;

		// Token: 0x040059B0 RID: 22960
		protected int stageIndex;

		// Token: 0x040059B1 RID: 22961
		protected float ticksPassedWithProgress;

		// Token: 0x040059B2 RID: 22962
		public float? progressBarOverride;

		// Token: 0x040059B3 RID: 22963
		protected bool ended;

		// Token: 0x040059B4 RID: 22964
		public bool repeatPenalty;

		// Token: 0x040059B5 RID: 22965
		private List<TargetInfo> stageSecondFocus;

		// Token: 0x040059B6 RID: 22966
		private List<RitualStagePawnSecondFocus> stagePawnSecondFocus;

		// Token: 0x040059B7 RID: 22967
		private List<RitualStagePositions> ritualStagePositions;

		// Token: 0x040059B8 RID: 22968
		private List<RitualStageOnTickActions> ritualStageOnTickActions;

		// Token: 0x040059B9 RID: 22969
		private List<RitualVisualEffect> effectWorkers = new List<RitualVisualEffect>();

		// Token: 0x040059BA RID: 22970
		private List<RitualVisualEffect> effectWorkersCurrentStage = new List<RitualVisualEffect>();

		// Token: 0x040059BB RID: 22971
		private int lastEssentialStageIndex = -1;

		// Token: 0x040059BC RID: 22972
		private int lastEssentialStageEndedTick = -1;

		// Token: 0x040059BD RID: 22973
		private int ticksSinceLastEssentialStage;

		// Token: 0x040059BE RID: 22974
		private bool ignoreDurationToFinish;

		// Token: 0x040059BF RID: 22975
		protected Effecter progressBar;

		// Token: 0x040059C0 RID: 22976
		protected Sustainer ambiencePlaying;

		// Token: 0x040059C1 RID: 22977
		protected List<LordToil_Ritual> toils = new List<LordToil_Ritual>();

		// Token: 0x040059C2 RID: 22978
		public bool cancelled;

		// Token: 0x040059C3 RID: 22979
		private bool initedVisualEffects;

		// Token: 0x040059C4 RID: 22980
		private IntVec2 roomBoundsCached = IntVec2.Invalid;

		// Token: 0x040059C5 RID: 22981
		private Dictionary<IntVec3, Mote> highlightedPositions = new Dictionary<IntVec3, Mote>();

		// Token: 0x040059C6 RID: 22982
		private Dictionary<Pawn, Mote> highlightedPawns = new Dictionary<Pawn, Mote>();

		// Token: 0x040059C7 RID: 22983
		public const string RitualStartedSignal = "RitualStarted";

		// Token: 0x040059C8 RID: 22984
		public static readonly IntVec2 DefaultRitualVfxScale = new IntVec2(28, 28);

		// Token: 0x040059C9 RID: 22985
		private static readonly List<Pawn> tmpParticipants = new List<Pawn>(8);

		// Token: 0x040059CA RID: 22986
		private Dictionary<Pawn, int> totalPresenceTmp = new Dictionary<Pawn, int>();

		// Token: 0x040059CB RID: 22987
		private static HashSet<Pawn> reservers = new HashSet<Pawn>();

		// Token: 0x040059CC RID: 22988
		private List<Pawn> tmpTagPawns;

		// Token: 0x040059CD RID: 22989
		private List<Pawn> tmpSubRolePawns;

		// Token: 0x040059CE RID: 22990
		private List<Pawn> tmpForcedRolePawns;

		// Token: 0x040059CF RID: 22991
		private List<PawnTags> tmpTags;

		// Token: 0x040059D0 RID: 22992
		private List<string> tmpForcedRoleIds;

		// Token: 0x040059D1 RID: 22993
		private List<string> tmpSubRoleIds;
	}
}
