using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200166D RID: 5741
	public class LordJob_SanguophageMeeting : LordJob
	{
		// Token: 0x170014BB RID: 5307
		// (get) Token: 0x06008753 RID: 34643 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008754 RID: 34644 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_SanguophageMeeting()
		{
		}

		// Token: 0x06008755 RID: 34645 RVA: 0x002868F8 File Offset: 0x00284AF8
		public LordJob_SanguophageMeeting(IntVec3 targetCell, List<Thing> enemyTargets, int meetingDurationTicks, string outSignalMeetingStarted, string outSignalMeetingEnded, string outSignalAllSanguophagesGone, string inSignalDefend, string inSignalQuestSuccess, string inSignalQuestFail)
		{
			this.targetCell = targetCell;
			this.enemyTargets = enemyTargets;
			this.meetingDurationTicks = meetingDurationTicks;
			this.outSignalMeetingStarted = outSignalMeetingStarted;
			this.outSignalMeetingEnded = outSignalMeetingEnded;
			this.outSignalAllSanguophagesGone = outSignalAllSanguophagesGone;
			this.inSignalDefend = inSignalDefend;
			this.inSignalQuestSuccess = inSignalQuestSuccess;
			this.inSignalQuestFail = inSignalQuestFail;
		}

		// Token: 0x06008756 RID: 34646 RVA: 0x00286950 File Offset: 0x00284B50
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckBiotech("Sanguophage meeting"))
			{
				return stateGraph;
			}
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.targetCell);
			stateGraph.AddToil(lordToil_Travel);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_SanguophageMeeting lordToil_SanguophageMeeting = new LordToil_SanguophageMeeting(this.targetCell, this.meetingDurationTicks);
			stateGraph.AddToil(lordToil_SanguophageMeeting);
			LordToil_ExitMapRandom lordToil_ExitMapRandom = new LordToil_ExitMapRandom();
			stateGraph.AddToil(lordToil_ExitMapRandom);
			LordToil_ExitMapAndDefendSelf lordToil_ExitMapAndDefendSelf = new LordToil_ExitMapAndDefendSelf();
			stateGraph.AddToil(lordToil_ExitMapAndDefendSelf);
			LordToil_AssaultThings lordToil_AssaultThings = new LordToil_AssaultThings(this.enemyTargets);
			stateGraph.AddToil(lordToil_AssaultThings);
			Transition transition = new Transition(lordToil_Travel, lordToil_SanguophageMeeting, false, true);
			transition.AddTrigger(new Trigger_Memo("TravelArrived"));
			transition.AddPostAction(new TransitionAction_Custom(delegate()
			{
				if (!this.outSignalMeetingStarted.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalMeetingStarted, false));
				}
				Building building = (Building)ThingMaker.MakeThing(ThingDefOf.SanguphageMeetingTorch, null);
				building.SetFaction(this.lord.faction, null);
				if (GenPlace.TryPlaceThing(building, this.targetCell, base.Map, ThingPlaceMode.Near, null, null, null, 1))
				{
					this.lord.AddBuilding(building);
				}
			}));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_SanguophageMeeting, lordToil_ExitMapRandom, false, true);
			transition2.AddTrigger(new Trigger_TicksPassed(this.meetingDurationTicks));
			transition2.AddTrigger(new Trigger_Signal(this.inSignalQuestSuccess));
			transition2.AddTrigger(new Trigger_Signal(this.inSignalQuestFail));
			transition2.AddPostAction(new TransitionAction_Custom(delegate()
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignalMeetingEnded, false));
			}));
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_SanguophageMeeting, lordToil_ExitMapAndDefendSelf, false, true);
			transition3.AddSource(lordToil_Travel);
			transition3.AddSource(lordToil_ExitMapRandom);
			transition3.AddSource(lordToil_AssaultThings);
			transition3.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition3.AddTrigger(new Trigger_PawnKilled());
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_Travel, lordToil_ExitMapAndDefendSelf, false, true);
			transition4.AddSource(lordToil_SanguophageMeeting);
			transition4.AddTrigger(new Trigger_PawnHarmed(1f, true, Faction.OfPlayer, null, null));
			transition4.AddPostAction(new TransitionAction_Custom(delegate()
			{
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "BeingAttacked", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_SanguophageMeeting, lordToil_AssaultThings, false, true);
			transition5.AddTrigger(new Trigger_Signal(this.inSignalDefend));
			transition5.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_AssaultThings, lordToil_ExitMapRandom, false, true);
			transition6.AddTrigger(new Trigger_Signal(this.inSignalQuestSuccess));
			stateGraph.AddTransition(transition6, false);
			Transition transition7 = new Transition(lordToil_SanguophageMeeting, lordToil_ExitMapAndDefendSelf, false, true);
			transition7.AddTrigger(new Trigger_Custom(delegate(TriggerSignal signal)
			{
				if (signal.type == TriggerSignalType.Tick)
				{
					for (int i = 0; i < this.lord.ownedPawns.Count; i++)
					{
						Pawn pawn = this.lord.ownedPawns[i];
						if (!pawn.Dead)
						{
							Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
							if (firstHediffOfDef != null && firstHediffOfDef.Severity >= 0.35f)
							{
								return true;
							}
							Hediff firstHediffOfDef2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke, false);
							if (firstHediffOfDef2 != null && firstHediffOfDef2.Severity >= 0.35f)
							{
								return true;
							}
						}
					}
				}
				return false;
			}));
			transition7.AddPostAction(new TransitionAction_Custom(delegate()
			{
				Messages.Message("SanguophagesLeavingTemperature".Translate(), this.lord.ownedPawns, MessageTypeDefOf.NegativeEvent, true);
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "BeingAttacked", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition7, false);
			return stateGraph;
		}

		// Token: 0x06008757 RID: 34647 RVA: 0x00286BC0 File Offset: 0x00284DC0
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			base.Notify_PawnLost(p, condition);
			if ((condition == PawnLostCondition.ExitedMap || condition == PawnLostCondition.Incapped || condition == PawnLostCondition.Killed || condition == PawnLostCondition.ChangedFaction || condition == PawnLostCondition.Vanished) && this.lord.ownedPawns.Count == 0)
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignalAllSanguophagesGone, false));
			}
		}

		// Token: 0x06008758 RID: 34648 RVA: 0x00286C14 File Offset: 0x00284E14
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.targetCell, "targetCell", default(IntVec3), false);
			Scribe_Collections.Look<Thing>(ref this.enemyTargets, "enemyTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.meetingDurationTicks, "meetingDurationTicks", 0, false);
			Scribe_Values.Look<string>(ref this.outSignalMeetingStarted, "outSignalMeetingStarted", null, false);
			Scribe_Values.Look<string>(ref this.outSignalMeetingEnded, "outSignalMeetingEnded", null, false);
			Scribe_Values.Look<string>(ref this.outSignalAllSanguophagesGone, "outSignalAllSanguophagesGone", null, false);
			Scribe_Values.Look<string>(ref this.inSignalDefend, "inSignalDefend", null, false);
			Scribe_Values.Look<string>(ref this.inSignalQuestSuccess, "inSignalQuestSuccess", null, false);
			Scribe_Values.Look<string>(ref this.inSignalQuestFail, "inSignalQuestFail", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.enemyTargets.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04005860 RID: 22624
		public IntVec3 targetCell;

		// Token: 0x04005861 RID: 22625
		public List<Thing> enemyTargets;

		// Token: 0x04005862 RID: 22626
		public int meetingDurationTicks;

		// Token: 0x04005863 RID: 22627
		public string outSignalMeetingStarted;

		// Token: 0x04005864 RID: 22628
		public string outSignalMeetingEnded;

		// Token: 0x04005865 RID: 22629
		public string outSignalAllSanguophagesGone;

		// Token: 0x04005866 RID: 22630
		public string inSignalDefend;

		// Token: 0x04005867 RID: 22631
		public string inSignalQuestSuccess;

		// Token: 0x04005868 RID: 22632
		public string inSignalQuestFail;

		// Token: 0x04005869 RID: 22633
		private const float HeatstrokeHypothermiaMinSeverityForLeaving = 0.35f;
	}
}
