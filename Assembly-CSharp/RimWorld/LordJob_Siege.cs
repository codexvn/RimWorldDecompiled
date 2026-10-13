using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001673 RID: 5747
	public class LordJob_Siege : LordJob
	{
		// Token: 0x170014BD RID: 5309
		// (get) Token: 0x06008772 RID: 34674 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008773 RID: 34675 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_Siege()
		{
		}

		// Token: 0x06008774 RID: 34676 RVA: 0x0028722F File Offset: 0x0028542F
		public LordJob_Siege(Faction faction, IntVec3 siegeSpot, float blueprintPoints)
		{
			this.faction = faction;
			this.siegeSpot = siegeSpot;
			this.blueprintPoints = blueprintPoints;
		}

		// Token: 0x06008775 RID: 34677 RVA: 0x0028724C File Offset: 0x0028544C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Travel(this.siegeSpot).CreateGraph()).StartingToil;
			LordToil_Siege lordToil_Siege = new LordToil_Siege(this.siegeSpot, this.blueprintPoints);
			stateGraph.AddToil(lordToil_Siege);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, false, true);
			lordToil_ExitMap.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil startingToil2 = stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true, false, false).CreateGraph()).StartingToil;
			Transition transition = new Transition(startingToil, lordToil_Siege, false, true);
			transition.AddTrigger(new Trigger_Memo("TravelArrived"));
			transition.AddTrigger(new Trigger_TicksPassed(5000));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Siege, startingToil2, false, true);
			transition2.AddTrigger(new Trigger_Memo("NoBuilders"));
			transition2.AddTrigger(new Trigger_Memo("NoArtillery"));
			transition2.AddTrigger(new Trigger_PawnHarmed(0.08f, false, null, null, null));
			transition2.AddTrigger(new Trigger_FractionPawnsLost(0.3f));
			transition2.AddTrigger(new Trigger_TicksPassed((int)(60000f * Rand.Range(1.5f, 3f))));
			transition2.AddPreAction(new TransitionAction_Message("MessageSiegersAssaulting".Translate(this.faction.def.pawnsPlural, this.faction), MessageTypeDefOf.ThreatBig, null, 1f, null));
			transition2.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_Siege, lordToil_ExitMap, false, true);
			transition3.AddSource(startingToil2);
			transition3.AddSource(startingToil);
			transition3.AddTrigger(new Trigger_BecameNonHostileToPlayer());
			transition3.AddPreAction(new TransitionAction_Message("MessageRaidersLeaving".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x06008776 RID: 34678 RVA: 0x00287458 File Offset: 0x00285658
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.siegeSpot, "siegeSpot", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.blueprintPoints, "blueprintPoints", 0f, false);
		}

		// Token: 0x04005872 RID: 22642
		private Faction faction;

		// Token: 0x04005873 RID: 22643
		private IntVec3 siegeSpot;

		// Token: 0x04005874 RID: 22644
		private float blueprintPoints;
	}
}
