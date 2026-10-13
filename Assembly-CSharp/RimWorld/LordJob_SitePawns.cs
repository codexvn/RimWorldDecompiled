using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001676 RID: 5750
	public class LordJob_SitePawns : LordJob
	{
		// Token: 0x0600877C RID: 34684 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		public LordJob_SitePawns()
		{
		}

		// Token: 0x0600877D RID: 34685 RVA: 0x002874D0 File Offset: 0x002856D0
		public LordJob_SitePawns(Faction faction, IntVec3 baseCenter, int delayBeforeAssault)
		{
			this.faction = faction;
			this.baseCenter = baseCenter;
			this.delayBeforeAssault = delayBeforeAssault;
		}

		// Token: 0x0600877E RID: 34686 RVA: 0x002874F0 File Offset: 0x002856F0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendBase lordToil_DefendBase = new LordToil_DefendBase(this.baseCenter);
			stateGraph.StartingToil = lordToil_DefendBase;
			LordToil_HuntDownColonists lordToil_HuntDownColonists = new LordToil_HuntDownColonists();
			stateGraph.AddToil(lordToil_HuntDownColonists);
			Transition transition = new Transition(lordToil_DefendBase, lordToil_HuntDownColonists, false, true);
			transition.AddTrigger(new Trigger_FractionPawnsLost(0.2f));
			transition.AddTrigger(new Trigger_PawnHarmed(0.4f, false, null, null, null));
			transition.AddTrigger(new Trigger_ChanceOnTickInterval(2500, 0.03f));
			transition.AddTrigger(new Trigger_TicksPassed(this.delayBeforeAssault));
			transition.AddTrigger(new Trigger_UrgentlyHungry());
			transition.AddTrigger(new Trigger_ChanceOnPlayerHarmNPCBuilding(0.4f));
			transition.AddTrigger(new Trigger_OnClamor(ClamorDefOf.Ability));
			transition.AddPostAction(new TransitionAction_WakeAll());
			TaggedString taggedString = this.faction.def.messageDefendersAttacking.Formatted(this.faction.def.pawnsPlural, this.faction.Name, Faction.OfPlayer.def.pawnsPlural).CapitalizeFirst();
			transition.AddPreAction(new TransitionAction_Message(taggedString, MessageTypeDefOf.ThreatBig, null, 1f, null));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x0600877F RID: 34687 RVA: 0x00287634 File Offset: 0x00285834
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.baseCenter, "baseCenter", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.delayBeforeAssault, "delayBeforeAssault", 25000, false);
		}

		// Token: 0x04005875 RID: 22645
		private Faction faction;

		// Token: 0x04005876 RID: 22646
		private IntVec3 baseCenter;

		// Token: 0x04005877 RID: 22647
		private int delayBeforeAssault;

		// Token: 0x04005878 RID: 22648
		private List<Pawn> tmpPawns;

		// Token: 0x04005879 RID: 22649
		private List<IntVec3> tmpCells;
	}
}
