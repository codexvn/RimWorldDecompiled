using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001665 RID: 5733
	public class LordJob_MechanoidsDefend : LordJob_MechanoidDefendBase
	{
		// Token: 0x170014B6 RID: 5302
		// (get) Token: 0x06008727 RID: 34599 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanBlockHostileVisitors
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170014B7 RID: 5303
		// (get) Token: 0x06008728 RID: 34600 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008729 RID: 34601 RVA: 0x00285C04 File Offset: 0x00283E04
		public LordJob_MechanoidsDefend()
		{
		}

		// Token: 0x0600872A RID: 34602 RVA: 0x00285C0C File Offset: 0x00283E0C
		public LordJob_MechanoidsDefend(List<Thing> things, Faction faction, float defendRadius, IntVec3 defSpot, bool canAssaultColony, bool isMechCluster)
		{
			this.things.AddRange(things);
			this.faction = faction;
			this.defendRadius = defendRadius;
			this.defSpot = defSpot;
			this.canAssaultColony = canAssaultColony;
			this.isMechCluster = isMechCluster;
		}

		// Token: 0x0600872B RID: 34603 RVA: 0x00285C48 File Offset: 0x00283E48
		public LordJob_MechanoidsDefend(SpawnedPawnParams parms)
		{
			this.things.Add(parms.spawnerThing);
			this.faction = parms.spawnerThing.Faction;
			this.defendRadius = parms.defendRadius;
			this.defSpot = parms.defSpot;
			this.canAssaultColony = false;
		}

		// Token: 0x0600872C RID: 34604 RVA: 0x00285C9C File Offset: 0x00283E9C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!this.defSpot.IsValid)
			{
				Log.Warning("LordJob_MechanoidsDefendShip defSpot is invalid. Returning graph for LordJob_AssaultColony.");
				stateGraph.AttachSubgraph(new LordJob_AssaultColony(this.faction, true, true, false, false, true, false, false).CreateGraph());
				return stateGraph;
			}
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(this.defSpot, new float?(this.defendRadius), null);
			stateGraph.StartingToil = lordToil_DefendPoint;
			LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(false, false);
			stateGraph.AddToil(lordToil_AssaultColony);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, false, true);
			lordToil_ExitMap.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap);
			if (this.canAssaultColony)
			{
				LordToil_AssaultColony lordToil_AssaultColony2 = new LordToil_AssaultColony(false, false);
				stateGraph.AddToil(lordToil_AssaultColony2);
				Transition transition = new Transition(lordToil_DefendPoint, lordToil_AssaultColony, false, true);
				transition.AddSource(lordToil_AssaultColony2);
				transition.AddTrigger(new Trigger_PawnCannotReachMapEdge());
				stateGraph.AddTransition(transition, false);
				Transition transition2 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony2, false, true);
				transition2.AddTrigger(new Trigger_PawnHarmed(0.5f, true, null, null, null));
				transition2.AddTrigger(new Trigger_PawnLostViolently(true));
				transition2.AddTrigger(new Trigger_Memo(LordJob_MechanoidsDefend.MemoDamaged));
				transition2.AddPostAction(new TransitionAction_EndAllJobs());
				stateGraph.AddTransition(transition2, false);
				Transition transition3 = new Transition(lordToil_AssaultColony2, lordToil_DefendPoint, false, true);
				transition3.AddTrigger(new Trigger_TicksPassedWithoutHarmOrMemos(1380, new string[]
				{
					LordJob_MechanoidsDefend.MemoDamaged
				}));
				transition3.AddPostAction(new TransitionAction_EndAttackBuildingJobs());
				stateGraph.AddTransition(transition3, false);
				Transition transition4 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony, false, true);
				transition4.AddSource(lordToil_AssaultColony2);
				transition4.AddTrigger(new Trigger_AnyThingDamageTaken(this.things, 0.5f));
				transition4.AddTrigger(new Trigger_Memo(HediffGiver_Heat.MemoPawnBurnedByAir));
				stateGraph.AddTransition(transition4, false);
			}
			Transition transition5 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony, false, true);
			transition5.AddTrigger(new Trigger_ChanceOnSignal(TriggerSignalType.MechClusterDefeated, 1f));
			stateGraph.AddTransition(transition5, false);
			if (!this.isMechCluster)
			{
				Transition transition6 = new Transition(lordToil_DefendPoint, lordToil_AssaultColony, false, true);
				transition6.AddTrigger(new Trigger_AnyThingDamageTaken(this.things, 1f));
				stateGraph.AddTransition(transition6, false);
			}
			Transition transition7 = new Transition(lordToil_AssaultColony, lordToil_ExitMap, false, true);
			transition7.AddTrigger(new Trigger_GameEnding(2500));
			transition7.AddPreAction(new TransitionAction_Message("MechanoidsMovingOn".Translate(), null, 1f));
			stateGraph.AddTransition(transition7, false);
			return stateGraph;
		}

		// Token: 0x0600872D RID: 34605 RVA: 0x00285EFC File Offset: 0x002840FC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shipPart, "shipPart", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x.DestroyedOrNull());
				if (this.shipPart != null)
				{
					if (this.things == null)
					{
						this.things = new List<Thing>();
					}
					this.things.Add(this.shipPart);
					this.shipPart = null;
				}
			}
		}

		// Token: 0x0400584A RID: 22602
		private Thing shipPart;

		// Token: 0x0400584B RID: 22603
		public static readonly string MemoDamaged = "ShipPartDamaged";
	}
}
