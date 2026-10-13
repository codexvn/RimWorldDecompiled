using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001652 RID: 5714
	public class LordJob_EscortPawn : LordJob
	{
		// Token: 0x170014A1 RID: 5281
		// (get) Token: 0x060086B2 RID: 34482 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool AlwaysShowWeapon
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060086B3 RID: 34483 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_EscortPawn()
		{
		}

		// Token: 0x060086B4 RID: 34484 RVA: 0x002835CD File Offset: 0x002817CD
		public LordJob_EscortPawn(Pawn escortee, Thing shuttle = null)
		{
			this.escortee = escortee;
			this.shuttle = shuttle;
			this.escorteeFaction = escortee.Faction;
		}

		// Token: 0x060086B5 RID: 34485 RVA: 0x002835F0 File Offset: 0x002817F0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
			LordToil_EscortPawn lordToil_EscortPawn = new LordToil_EscortPawn(this.escortee, 7f);
			stateGraph.AddToil(lordToil_EscortPawn);
			LordToil lordToil = (this.shuttle == null) ? new LordToil_ExitMap(LocomotionUrgency.None, false, false) : new LordToil_EnterShuttleOrLeave(this.shuttle, LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil);
			LordToil lordToil2 = (this.shuttle == null) ? new LordToil_ExitMapFighting(LocomotionUrgency.Jog, true, false) : new LordToil_EnterShuttleOrLeave(this.shuttle, LocomotionUrgency.None, false, false);
			lordToil2.useAvoidGrid = true;
			stateGraph.AddToil(lordToil2);
			Transition transition = new Transition(lordToil_EscortPawn, lordToil, false, true);
			Trigger_Custom trigger = new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.escortee.Dead);
			transition.AddTrigger(trigger);
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_EscortPawn, lordToil, false, true);
			Trigger_Custom trigger2 = new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && (this.escortee.MapHeld != this.lord.Map || (this.shuttle != null && this.escortee.ParentHolder == this.shuttle.TryGetComp<CompTransporter>() && this.shuttle.TryGetComp<CompShuttle>().shipParent.Waiting)));
			transition2.AddTrigger(trigger2);
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_EscortPawn, lordToil, false, true);
			transition3.AddTrigger(new Trigger_BecamePlayerEnemy());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_EscortPawn, lordToil, false, true);
			transition4.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.escortee.Faction != this.escorteeFaction));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_EscortPawn, lordToil2, false, true);
			transition5.AddTrigger(new Trigger_PawnHarmed(1f, true, base.Map.ParentFaction, null, null));
			transition5.AddTrigger(new Trigger_PawnLostViolently(true, this.escortee.Faction));
			Func<Pawn, bool> <>9__4;
			transition5.AddPostAction(new TransitionAction_Custom(delegate()
			{
				if (!this.leavingDangerMessage.NullOrEmpty())
				{
					Messages.Message(this.leavingDangerMessage, this.escortee, MessageTypeDefOf.NegativeEvent, true);
				}
				CompShuttle compShuttle = compShuttle;
				if (compShuttle != null)
				{
					Func<Pawn, bool> pawnToLeaveBehind;
					if ((pawnToLeaveBehind = <>9__4) == null)
					{
						pawnToLeaveBehind = (<>9__4 = ((Pawn p) => p != this.escortee));
					}
					compShuttle.SetPawnToLeaveBehind(pawnToLeaveBehind);
				}
				QuestUtility.SendQuestTargetSignals(this.lord.questTags, "BeingAttacked", this.lord.Named("SUBJECT"));
			}));
			stateGraph.AddTransition(transition5, false);
			return stateGraph;
		}

		// Token: 0x060086B6 RID: 34486 RVA: 0x002837A8 File Offset: 0x002819A8
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.escortee, "escortee", false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_References.Look<Faction>(ref this.escorteeFaction, "escorteeFaction", false);
			Scribe_Values.Look<string>(ref this.leavingDangerMessage, "leavingDangerMessage", null, false);
		}

		// Token: 0x040057F7 RID: 22519
		public Pawn escortee;

		// Token: 0x040057F8 RID: 22520
		public Thing shuttle;

		// Token: 0x040057F9 RID: 22521
		private Faction escorteeFaction;

		// Token: 0x040057FA RID: 22522
		public string leavingDangerMessage;
	}
}
