using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001667 RID: 5735
	public class LordJob_Metalhorror : LordJob
	{
		// Token: 0x170014B8 RID: 5304
		// (get) Token: 0x06008732 RID: 34610 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanAutoAddPawns
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008733 RID: 34611 RVA: 0x00285FA0 File Offset: 0x002841A0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_MetalhorrorActive lordToil_MetalhorrorActive = new LordToil_MetalhorrorActive();
			stateGraph.AddToil(lordToil_MetalhorrorActive);
			stateGraph.StartingToil = lordToil_MetalhorrorActive;
			LordToil_Sleep lordToil_Sleep = new LordToil_Sleep();
			stateGraph.AddToil(lordToil_Sleep);
			Transition transition = new Transition(lordToil_MetalhorrorActive, lordToil_Sleep, false, true);
			transition.AddTrigger(new Trigger_Custom((TriggerSignal _) => this.lord.ownedPawns.Count != 0 && GenTicks.IsTickInterval(120) && this.ShouldBeHibernating()));
			transition.AddPostAction(new TransitionAction_Message("MessageMetalHorrorDormant".Translate(), MessageTypeDefOf.NeutralEvent, null, 1f, null));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Sleep, lordToil_MetalhorrorActive, false, true);
			transition2.AddTrigger(new Trigger_DormancyWakeup());
			transition2.AddTrigger(new Trigger_Custom((TriggerSignal _) => this.lord.ownedPawns.Count != 0 && GenTicks.IsTickInterval(120) && !this.ShouldBeHibernating()));
			transition2.AddPostAction(new TransitionAction_WakeAll());
			transition2.AddPostAction(new TransitionAction_Letter("LetterMetalhorrorReawakeningLabel".Translate(), "LetterMetalhorrorReawakening".Translate(), LetterDefOf.ThreatBig));
			stateGraph.AddTransition(transition2, false);
			return stateGraph;
		}

		// Token: 0x06008734 RID: 34612 RVA: 0x00286090 File Offset: 0x00284290
		private bool ShouldBeHibernating()
		{
			bool result = false;
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				if (pawn.SpawnedOrAnyParentSpawned)
				{
					result = true;
					if (GenTicks.TicksGame < pawn.TickSpawned + 15000 || pawn.mindState.WasRecentlyCombatantTicks(15000) || GenTicks.TicksGame < pawn.mindState.hibernationEndedTick + 7500 || pawn.IsOnHoldingPlatform)
					{
						return false;
					}
				}
			}
			return result;
		}

		// Token: 0x06008735 RID: 34613 RVA: 0x0028613C File Offset: 0x0028433C
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.biosignature, "biosignature", 0, false);
		}

		// Token: 0x0400584E RID: 22606
		public int biosignature;

		// Token: 0x0400584F RID: 22607
		private const int TicksToDormant = 15000;

		// Token: 0x04005850 RID: 22608
		private const int TicksToDormantHibernation = 7500;

		// Token: 0x04005851 RID: 22609
		private const int TransitionCheckRateTicks = 120;
	}
}
