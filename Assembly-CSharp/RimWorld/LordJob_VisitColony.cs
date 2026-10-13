using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001683 RID: 5763
	public class LordJob_VisitColony : LordJob
	{
		// Token: 0x060087C8 RID: 34760 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_VisitColony()
		{
		}

		// Token: 0x060087C9 RID: 34761 RVA: 0x00288F6C File Offset: 0x0028716C
		public LordJob_VisitColony(Faction faction, IntVec3 chillSpot, int? durationTicks = null)
		{
			this.faction = faction;
			this.chillSpot = chillSpot;
			this.durationTicks = durationTicks;
		}

		// Token: 0x060087CA RID: 34762 RVA: 0x00288F8C File Offset: 0x0028718C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Travel(this.chillSpot).CreateGraph()).StartingToil;
			stateGraph.StartingToil = startingToil;
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(this.chillSpot, null, null);
			stateGraph.AddToil(lordToil_DefendPoint);
			LordToil_TakeWoundedGuest lordToil_TakeWoundedGuest = new LordToil_TakeWoundedGuest();
			stateGraph.AddToil(lordToil_TakeWoundedGuest);
			this.exitSubgraph = new LordJob_TravelAndExit(IntVec3.Invalid).CreateGraph();
			LordToil startingToil2 = stateGraph.AttachSubgraph(this.exitSubgraph).StartingToil;
			LordToil target = this.exitSubgraph.lordToils[1];
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Walk, true, false);
			stateGraph.AddToil(lordToil_ExitMap);
			Transition transition = new Transition(startingToil, startingToil2, false, true);
			transition.AddSources(new LordToil[]
			{
				lordToil_DefendPoint
			});
			transition.AddTrigger(new Trigger_PawnExperiencingDangerousTemperatures());
			if (this.faction != null)
			{
				transition.AddPreAction(new TransitionAction_Message("MessageVisitorsDangerousTemperature".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			}
			transition.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(startingToil, startingToil2, false, true);
			transition2.AddSources(new LordToil[]
			{
				lordToil_DefendPoint
			});
			transition2.AddTrigger(new Trigger_PawnExperiencingAnomalousWeather());
			if (this.faction != null)
			{
				transition2.AddPreAction(new TransitionAction_Message("MessageVisitorsAnomalousWeather".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			}
			transition2.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(startingToil, lordToil_ExitMap, false, true);
			transition3.AddSources(new LordToil[]
			{
				lordToil_DefendPoint,
				lordToil_TakeWoundedGuest
			});
			transition3.AddSources(this.exitSubgraph.lordToils);
			transition3.AddTrigger(new Trigger_PawnCannotReachMapEdge());
			if (this.faction != null)
			{
				transition3.AddPreAction(new TransitionAction_Message("MessageVisitorsTrappedLeaving".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			}
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_ExitMap, startingToil2, false, true);
			transition4.AddTrigger(new Trigger_PawnCanReachMapEdge());
			transition4.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			transition4.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(startingToil, lordToil_DefendPoint, false, true);
			transition5.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition5, false);
			if (this.faction != null)
			{
				Transition transition6 = new Transition(lordToil_DefendPoint, lordToil_TakeWoundedGuest, false, true);
				transition6.AddTrigger(new Trigger_WoundedGuestPresent());
				transition6.AddPreAction(new TransitionAction_Message("MessageVisitorsTakingWounded".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
				stateGraph.AddTransition(transition6, false);
			}
			Transition transition7 = new Transition(lordToil_DefendPoint, target, false, true);
			transition7.AddSources(new LordToil[]
			{
				lordToil_TakeWoundedGuest,
				startingToil
			});
			transition7.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition7.AddPreAction(new TransitionAction_SetDefendLocalGroup());
			transition7.AddPostAction(new TransitionAction_WakeAll());
			transition7.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition7, false);
			Transition transition8 = new Transition(lordToil_DefendPoint, startingToil2, false, true);
			int tickLimit;
			if (DebugSettings.instantVisitorsGift && this.faction != null)
			{
				tickLimit = 0;
			}
			else if (this.durationTicks != null)
			{
				tickLimit = this.durationTicks.Value;
			}
			else
			{
				tickLimit = Rand.Range(8000, 22000);
			}
			transition8.AddTrigger(new Trigger_TicksPassed(tickLimit));
			if (this.faction != null)
			{
				transition8.AddPreAction(new TransitionAction_Message("VisitorsLeaving".Translate(this.faction.Name), null, 1f));
			}
			if (this.gifts != null)
			{
				transition8.AddPreAction(new TransitionAction_GiveGift
				{
					gifts = this.gifts
				});
			}
			else
			{
				transition8.AddPreAction(new TransitionAction_CheckGiveGift());
			}
			transition8.AddPostAction(new TransitionAction_WakeAll());
			transition8.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			stateGraph.AddTransition(transition8, false);
			return stateGraph;
		}

		// Token: 0x060087CB RID: 34763 RVA: 0x00289428 File Offset: 0x00287628
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.chillSpot, "chillSpot", default(IntVec3), false);
			Scribe_Values.Look<int?>(ref this.durationTicks, "durationTicks", null, false);
			Scribe_Collections.Look<Thing>(ref this.gifts, "gifts", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<Thing> list = this.gifts;
				if (list == null)
				{
					return;
				}
				list.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x0400589E RID: 22686
		private Faction faction;

		// Token: 0x0400589F RID: 22687
		private IntVec3 chillSpot;

		// Token: 0x040058A0 RID: 22688
		private int? durationTicks;

		// Token: 0x040058A1 RID: 22689
		public List<Thing> gifts;

		// Token: 0x040058A2 RID: 22690
		public StateGraph exitSubgraph;
	}
}
