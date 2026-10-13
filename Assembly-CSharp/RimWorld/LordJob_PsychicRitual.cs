using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001689 RID: 5769
	public class LordJob_PsychicRitual : LordJob
	{
		// Token: 0x060087DD RID: 34781 RVA: 0x002896B8 File Offset: 0x002878B8
		public override AcceptanceReport AllowsDrafting(Pawn pawn)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.AllowsDrafting(pawn);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitual.def.AllowsDrafting(pawn);
		}

		// Token: 0x060087DE RID: 34782 RVA: 0x002896F7 File Offset: 0x002878F7
		public LordJob_PsychicRitual(PsychicRitualDef def, PsychicRitualRoleAssignments assignments, float points = -1f)
		{
			this.def = def;
			this.assignments = assignments;
			this.points = points;
		}

		// Token: 0x060087DF RID: 34783 RVA: 0x0020FCAA File Offset: 0x0020DEAA
		protected LordJob_PsychicRitual()
		{
		}

		// Token: 0x060087E0 RID: 34784 RVA: 0x00289714 File Offset: 0x00287914
		public override StateGraph CreateGraph()
		{
			if (!ModLister.CheckAnomaly("Psychic ritual"))
			{
				return null;
			}
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_PsychicRitual(this.def, this.assignments));
			return stateGraph;
		}

		// Token: 0x060087E1 RID: 34785 RVA: 0x00289740 File Offset: 0x00287940
		public override string GetReport(Pawn pawn)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.GetJobReport(pawn);
			}
			string pawnReport = lordToil_PsychicRitual.RitualData.psychicRitualToil.GetPawnReport(lordToil_PsychicRitual.RitualData.psychicRitual, null, pawn);
			if (pawnReport == null)
			{
				return null;
			}
			return pawnReport.CapitalizeFirst();
		}

		// Token: 0x060087E2 RID: 34786 RVA: 0x00289794 File Offset: 0x00287994
		public override string GetJobReport(Pawn pawn)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.GetJobReport(pawn);
			}
			string jobReport = lordToil_PsychicRitual.RitualData.psychicRitualToil.GetJobReport(lordToil_PsychicRitual.RitualData.psychicRitual, null, pawn);
			if (jobReport == null)
			{
				return null;
			}
			return jobReport.CapitalizeFirst();
		}

		// Token: 0x060087E3 RID: 34787 RVA: 0x002897E8 File Offset: 0x002879E8
		public override AcceptanceReport AllowsFloatMenu(Pawn pawn)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.AllowsFloatMenu(pawn);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitualToil.AllowsFloatMenu(lordToil_PsychicRitual.RitualData.psychicRitual, null, pawn);
		}

		// Token: 0x060087E4 RID: 34788 RVA: 0x00289830 File Offset: 0x00287A30
		public override bool BlocksSocialInteraction(Pawn pawn)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.BlocksSocialInteraction(pawn);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitualToil.BlocksSocialInteraction(lordToil_PsychicRitual.RitualData.psychicRitual, null, pawn);
		}

		// Token: 0x060087E5 RID: 34789 RVA: 0x00289878 File Offset: 0x00287A78
		public override bool DutyActiveWhenDown(Pawn pawn)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.BlocksSocialInteraction(pawn);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitualToil.DutyActiveWhenDown(lordToil_PsychicRitual.RitualData.psychicRitual, null, pawn);
		}

		// Token: 0x060087E6 RID: 34790 RVA: 0x002898C0 File Offset: 0x00287AC0
		public override AcceptanceReport AbilityAllowed(Ability ability)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.AbilityAllowed(ability);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitualToil.AbilityAllowed(lordToil_PsychicRitual.RitualData.psychicRitual, null, ability);
		}

		// Token: 0x060087E7 RID: 34791 RVA: 0x00289908 File Offset: 0x00287B08
		public override bool ShouldRemovePawn(Pawn pawn, PawnLostCondition reason)
		{
			if (reason == PawnLostCondition.LordRejected)
			{
				return true;
			}
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.ShouldRemovePawn(pawn, reason);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitualToil.ShouldRemovePawn(lordToil_PsychicRitual.RitualData.psychicRitual, null, pawn, reason);
		}

		// Token: 0x060087E8 RID: 34792 RVA: 0x00289958 File Offset: 0x00287B58
		public override ThinkResult Notify_DutyConstantResult(ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.Notify_DutyConstantResult(result, pawn, issueParams);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitualToil.Notify_DutyConstantResult(lordToil_PsychicRitual.RitualData.psychicRitual, null, result, pawn, issueParams);
		}

		// Token: 0x060087E9 RID: 34793 RVA: 0x002899A4 File Offset: 0x00287BA4
		public override ThinkResult Notify_DutyResult(ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return base.Notify_DutyResult(result, pawn, issueParams);
			}
			return lordToil_PsychicRitual.RitualData.psychicRitualToil.Notify_DutyResult(lordToil_PsychicRitual.RitualData.psychicRitual, null, result, pawn, issueParams);
		}

		// Token: 0x060087EA RID: 34794 RVA: 0x002899F0 File Offset: 0x00287BF0
		public override void Notify_MapRemoved()
		{
			LordToil_PsychicRitual lordToil_PsychicRitual = this.lord.CurLordToil as LordToil_PsychicRitual;
			if (lordToil_PsychicRitual == null)
			{
				return;
			}
			lordToil_PsychicRitual.RitualData.psychicRitual.CancelPsychicRitual(null);
		}

		// Token: 0x060087EB RID: 34795 RVA: 0x00289A28 File Offset: 0x00287C28
		public override void ExposeData()
		{
			Scribe_Defs.Look<PsychicRitualDef>(ref this.def, "def");
			Scribe_Deep.Look<PsychicRitualRoleAssignments>(ref this.assignments, "assignments", Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
		}

		// Token: 0x060087EC RID: 34796 RVA: 0x00289A65 File Offset: 0x00287C65
		public override bool EndPawnJobOnCleanup(Pawn pawn)
		{
			Job curJob = pawn.CurJob;
			return ((curJob != null) ? curJob.lord : null) == this.lord;
		}

		// Token: 0x060087ED RID: 34797 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool PrisonerSecure(Pawn pawn)
		{
			return true;
		}

		// Token: 0x040058A9 RID: 22697
		public PsychicRitualDef def;

		// Token: 0x040058AA RID: 22698
		public PsychicRitualRoleAssignments assignments;

		// Token: 0x040058AB RID: 22699
		public float points;
	}
}
