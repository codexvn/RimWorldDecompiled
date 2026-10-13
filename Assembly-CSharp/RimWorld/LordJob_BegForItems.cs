using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001636 RID: 5686
	public class LordJob_BegForItems : LordJob
	{
		// Token: 0x06008604 RID: 34308 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_BegForItems()
		{
		}

		// Token: 0x06008605 RID: 34309 RVA: 0x00280510 File Offset: 0x0027E710
		public LordJob_BegForItems(Faction faction, IntVec3 idleSpot, Pawn target, ThingDef thingDef, int amount, string outSignalItemsReceived = null)
		{
			this.idleSpot = idleSpot;
			this.faction = faction;
			this.target = target;
			this.thingDef = thingDef;
			this.amount = amount;
			this.outSignalItemsReceived = outSignalItemsReceived;
		}

		// Token: 0x06008606 RID: 34310 RVA: 0x00280548 File Offset: 0x0027E748
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckIdeology("Beg for items"))
			{
				return stateGraph;
			}
			LordToil_TravelAndWaitForItems lordToil_TravelAndWaitForItems = new LordToil_TravelAndWaitForItems(this.idleSpot, this.target, this.thingDef, this.amount);
			stateGraph.AddToil(lordToil_TravelAndWaitForItems);
			stateGraph.StartingToil = lordToil_TravelAndWaitForItems;
			LordToil_WaitForItems waitForItems = new LordToil_WaitForItems(this.target, this.thingDef, this.amount, this.idleSpot);
			stateGraph.AddToil(waitForItems);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil_ExitMapAndDefendSelf toil = new LordToil_ExitMapAndDefendSelf();
			stateGraph.AddToil(toil);
			Transition transition = new Transition(lordToil_TravelAndWaitForItems, waitForItems, false, true);
			transition.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(waitForItems, lordToil_ExitMap, false, true);
			transition2.AddSource(lordToil_TravelAndWaitForItems);
			transition2.AddTrigger(new Trigger_Custom((TriggerSignal s) => waitForItems.HasAllRequestedItems));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			if (!this.outSignalItemsReceived.NullOrEmpty())
			{
				transition2.AddPostAction(new TransitionAction_Custom(delegate()
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalItemsReceived, false));
				}));
			}
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(waitForItems, toil, false, true);
			transition3.AddSource(lordToil_TravelAndWaitForItems);
			transition3.AddSource(lordToil_ExitMap);
			transition3.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition3.AddTrigger(new Trigger_PawnKilled());
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_TravelAndWaitForItems, lordToil_ExitMap, false, true);
			transition4.AddSource(waitForItems);
			transition4.AddPreAction(new TransitionAction_Message("MessageVisitorsDangerousTemperature".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			transition4.AddPostAction(new TransitionAction_EndAllJobs());
			transition4.AddTrigger(new Trigger_PawnExperiencingDangerousTemperatures());
			stateGraph.AddTransition(transition4, false);
			return stateGraph;
		}

		// Token: 0x06008607 RID: 34311 RVA: 0x00280758 File Offset: 0x0027E958
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.idleSpot, "idleSpot", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
			Scribe_Values.Look<string>(ref this.outSignalItemsReceived, "outSignalItemsReceived", null, false);
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
		}

		// Token: 0x04005790 RID: 22416
		private IntVec3 idleSpot;

		// Token: 0x04005791 RID: 22417
		private Faction faction;

		// Token: 0x04005792 RID: 22418
		private Pawn target;

		// Token: 0x04005793 RID: 22419
		private ThingDef thingDef;

		// Token: 0x04005794 RID: 22420
		private int amount;

		// Token: 0x04005795 RID: 22421
		private string outSignalItemsReceived;
	}
}
