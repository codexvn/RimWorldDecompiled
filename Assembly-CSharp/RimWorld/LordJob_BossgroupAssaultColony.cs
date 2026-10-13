using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001646 RID: 5702
	public class LordJob_BossgroupAssaultColony : LordJob
	{
		// Token: 0x06008674 RID: 34420 RVA: 0x00282753 File Offset: 0x00280953
		public LordJob_BossgroupAssaultColony()
		{
		}

		// Token: 0x06008675 RID: 34421 RVA: 0x00282766 File Offset: 0x00280966
		public LordJob_BossgroupAssaultColony(Faction faction, IntVec3 stageLoc, IEnumerable<Pawn> bosses)
		{
			this.faction = faction;
			this.stageLoc = stageLoc;
			this.bosses.AddRange(bosses);
		}

		// Token: 0x06008676 RID: 34422 RVA: 0x00282794 File Offset: 0x00280994
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Stage lordToil_Stage = new LordToil_Stage(this.stageLoc);
			stateGraph.StartingToil = lordToil_Stage;
			LordToil_MoveInBossgroup lordToil_MoveInBossgroup = new LordToil_MoveInBossgroup(this.bosses);
			stateGraph.AddToil(lordToil_MoveInBossgroup);
			LordToil_AssaultColonyBossgroup lordToil_AssaultColonyBossgroup = new LordToil_AssaultColonyBossgroup();
			stateGraph.AddToil(lordToil_AssaultColonyBossgroup);
			Transition transition = new Transition(lordToil_Stage, lordToil_MoveInBossgroup, false, true);
			transition.AddTrigger(new Trigger_TicksPassed(LordJob_BossgroupAssaultColony.PrepareTicksRange.RandomInRange));
			if (this.faction != null)
			{
				transition.AddPreAction(new TransitionAction_Message("MessageRaidersBeginningAssault".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), MessageTypeDefOf.ThreatBig, null, 1f, null));
			}
			transition.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Stage, lordToil_AssaultColonyBossgroup, false, true);
			transition2.AddTrigger(new Trigger_PawnHarmed(1f, false, null, null, null));
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_MoveInBossgroup, lordToil_AssaultColonyBossgroup, false, true);
			transition3.AddTrigger(new Trigger_PawnHarmed(1f, false, null, null, null));
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x06008677 RID: 34423 RVA: 0x002828CC File Offset: 0x00280ACC
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.stageLoc, "stageLoc", default(IntVec3), false);
			Scribe_Collections.Look<Pawn>(ref this.bosses, "bosses", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.bosses.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x040057DB RID: 22491
		private static readonly IntRange PrepareTicksRange = new IntRange(5000, 10000);

		// Token: 0x040057DC RID: 22492
		private Faction faction;

		// Token: 0x040057DD RID: 22493
		private IntVec3 stageLoc;

		// Token: 0x040057DE RID: 22494
		private List<Pawn> bosses = new List<Pawn>();
	}
}
