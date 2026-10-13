using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200164D RID: 5709
	public class LordJob_DefendBase : LordJob
	{
		// Token: 0x0600869D RID: 34461 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_DefendBase()
		{
		}

		// Token: 0x0600869E RID: 34462 RVA: 0x002831FF File Offset: 0x002813FF
		public LordJob_DefendBase(Faction faction, IntVec3 baseCenter, int delayBeforeAssault, bool attackWhenPlayerBecameEnemy = false)
		{
			this.faction = faction;
			this.baseCenter = baseCenter;
			this.attackWhenPlayerBecameEnemy = attackWhenPlayerBecameEnemy;
			this.delayBeforeAssault = delayBeforeAssault;
		}

		// Token: 0x0600869F RID: 34463 RVA: 0x00283224 File Offset: 0x00281424
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendBase lordToil_DefendBase = new LordToil_DefendBase(this.baseCenter);
			stateGraph.StartingToil = lordToil_DefendBase;
			LordToil_DefendBase lordToil_DefendBase2 = new LordToil_DefendBase(this.baseCenter);
			stateGraph.AddToil(lordToil_DefendBase2);
			LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(true, false);
			lordToil_AssaultColony.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_AssaultColony);
			Transition transition = new Transition(lordToil_DefendBase, lordToil_DefendBase2, false, true);
			transition.AddSource(lordToil_AssaultColony);
			transition.AddTrigger(new Trigger_BecameNonHostileToPlayer());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_DefendBase2, this.attackWhenPlayerBecameEnemy ? lordToil_AssaultColony : lordToil_DefendBase, false, true);
			if (this.attackWhenPlayerBecameEnemy)
			{
				transition2.AddSource(lordToil_DefendBase);
			}
			transition2.AddTrigger(new Trigger_BecamePlayerEnemy());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_DefendBase, lordToil_AssaultColony, false, true);
			transition3.AddTrigger(new Trigger_FractionPawnsLost(0.2f));
			transition3.AddTrigger(new Trigger_PawnHarmed(0.4f, false, null, null, null));
			transition3.AddTrigger(new Trigger_ChanceOnTickInterval(2500, 0.03f));
			transition3.AddTrigger(new Trigger_TicksPassed(this.delayBeforeAssault));
			transition3.AddTrigger(new Trigger_UrgentlyHungry());
			transition3.AddTrigger(new Trigger_ChanceOnPlayerHarmNPCBuilding(0.4f));
			transition3.AddTrigger(new Trigger_OnClamor(ClamorDefOf.Ability));
			transition3.AddPostAction(new TransitionAction_WakeAll());
			TaggedString taggedString = this.faction.def.messageDefendersAttacking.Formatted(this.faction.def.pawnsPlural, this.faction.Name, Faction.OfPlayer.def.pawnsPlural).CapitalizeFirst();
			transition3.AddPreAction(new TransitionAction_Message(taggedString, MessageTypeDefOf.ThreatBig, null, 1f, null));
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x060086A0 RID: 34464 RVA: 0x002833F0 File Offset: 0x002815F0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.baseCenter, "baseCenter", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.attackWhenPlayerBecameEnemy, "attackWhenPlayerBecameEnemy", false, false);
			Scribe_Values.Look<int>(ref this.delayBeforeAssault, "delayBeforeAssault", 25000, false);
		}

		// Token: 0x040057ED RID: 22509
		private Faction faction;

		// Token: 0x040057EE RID: 22510
		private IntVec3 baseCenter;

		// Token: 0x040057EF RID: 22511
		private bool attackWhenPlayerBecameEnemy;

		// Token: 0x040057F0 RID: 22512
		private int delayBeforeAssault;

		// Token: 0x040057F1 RID: 22513
		private List<Pawn> tmpPawns;

		// Token: 0x040057F2 RID: 22514
		private List<IntVec3> tmpCells;
	}
}
