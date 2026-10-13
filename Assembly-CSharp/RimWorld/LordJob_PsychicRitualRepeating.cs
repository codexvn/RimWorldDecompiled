using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001669 RID: 5737
	public class LordJob_PsychicRitualRepeating : LordJob_PsychicRitual
	{
		// Token: 0x06008743 RID: 34627 RVA: 0x002863F2 File Offset: 0x002845F2
		public LordJob_PsychicRitualRepeating(PsychicRitualDef def, PsychicRitualRoleAssignments assignments, int iterations, IntVec3 ritualCell, float points = -1f) : base(def, assignments, points)
		{
			this.iterations = iterations;
			this.ritualCell = ritualCell;
			this.invoker = assignments.FirstAssignedPawn(PsychicRitualRoleDefOf.Invoker);
		}

		// Token: 0x06008744 RID: 34628 RVA: 0x00286425 File Offset: 0x00284625
		protected LordJob_PsychicRitualRepeating()
		{
		}

		// Token: 0x06008745 RID: 34629 RVA: 0x00286434 File Offset: 0x00284634
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			List<LordToil_PsychicRitual> list = new List<LordToil_PsychicRitual>();
			List<LordToil> list2 = new List<LordToil>();
			LordToil_PsychicRitual lordToil_PsychicRitual = null;
			int i;
			for (i = 0; i < this.iterations; i++)
			{
				LordToil_PsychicRitual lordToil_PsychicRitual2 = new LordToil_PsychicRitual(this.def, this.assignments)
				{
					iteration = i,
					removeLordOnCancel = false
				};
				stateGraph.AddToil(lordToil_PsychicRitual2);
				list.Add(lordToil_PsychicRitual2);
				if (lordToil_PsychicRitual != null)
				{
					LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(this.ritualCell, new float?((float)60), null);
					stateGraph.AddToil(lordToil_DefendPoint);
					list2.Add(lordToil_DefendPoint);
					Transition transition = new Transition(lordToil_PsychicRitual, lordToil_DefendPoint, false, true);
					transition.AddTrigger(new Trigger_Memo("PsychicRitualCompleted" + (i - 1).ToString()));
					stateGraph.AddTransition(transition, false);
					Transition transition2 = new Transition(lordToil_DefendPoint, lordToil_PsychicRitual2, false, true);
					transition2.AddTrigger(new Trigger_TicksPassed((i + 1) * 2500));
					stateGraph.AddTransition(transition2, false);
				}
				else
				{
					stateGraph.StartingToil = lordToil_PsychicRitual2;
				}
				lordToil_PsychicRitual = lordToil_PsychicRitual2;
			}
			LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(false, false);
			stateGraph.AddToil(lordToil_AssaultColony);
			Transition transition3 = new Transition(null, lordToil_AssaultColony, false, true);
			transition3.AddSources(list);
			transition3.AddSources(list2);
			transition3.AddTrigger(new Trigger_Memo("PsychicRitualCompleted" + (i - 1).ToString()));
			transition3.AddTrigger(new Trigger_Memo("PsychicRitualCanceled"));
			transition3.AddTrigger(new Trigger_PawnKilled(this.lord.ownedPawns.Except(this.invoker).ToList<Pawn>()));
			transition3.AddPostAction(new TransitionAction_Custom(delegate()
			{
				if (this.lord.ownedPawns.Any((Pawn x) => !x.DeadOrDowned))
				{
					Messages.Message("MessagePsychicRitualAssault".Translate(this.lord.faction), this.lord.ownedPawns, MessageTypeDefOf.ThreatSmall, true);
				}
			}));
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x06008746 RID: 34630 RVA: 0x002865F4 File Offset: 0x002847F4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.iterations, "iterations", 0, false);
			Scribe_References.Look<Pawn>(ref this.invoker, "invoker", false);
			Scribe_Values.Look<IntVec3>(ref this.ritualCell, "ritualCell", default(IntVec3), false);
		}

		// Token: 0x04005855 RID: 22613
		private int iterations = 1;

		// Token: 0x04005856 RID: 22614
		private Pawn invoker;

		// Token: 0x04005857 RID: 22615
		private IntVec3 ritualCell;

		// Token: 0x04005858 RID: 22616
		public const string MemoPsychicRitualCompleted = "PsychicRitualCompleted";

		// Token: 0x04005859 RID: 22617
		public const string MemoPsychicRitualCanceled = "PsychicRitualCanceled";
	}
}
