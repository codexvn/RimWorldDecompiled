using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02001077 RID: 4215
	public class PsychicRitualToil_Multiplex : PsychicRitualToil
	{
		// Token: 0x0600672C RID: 26412 RVA: 0x0020AAA4 File Offset: 0x00208CA4
		protected PsychicRitualToil_Multiplex()
		{
		}

		// Token: 0x0600672D RID: 26413 RVA: 0x0020AAC4 File Offset: 0x00208CC4
		public PsychicRitualToil_Multiplex(Dictionary<PsychicRitualRoleDef, PsychicRitualToil> roleToils, PsychicRitualToil fallback = null)
		{
			this.roleToils = new Dictionary<PsychicRitualRoleDef, PsychicRitualToil>(roleToils);
			this.fallback = fallback;
			this.childToils = new List<PsychicRitualToil>(roleToils.Values);
			this.childToils.Add(fallback);
			this.childToils.RemoveDuplicates(null);
		}

		// Token: 0x0600672E RID: 26414 RVA: 0x0020AB29 File Offset: 0x00208D29
		public PsychicRitualToil ToilForPawn(PsychicRitual psychicRitual, Pawn pawn)
		{
			return this.roleToils.GetWithFallback(psychicRitual.assignments.RoleForPawn(pawn, true), this.fallback);
		}

		// Token: 0x0600672F RID: 26415 RVA: 0x0020AB4C File Offset: 0x00208D4C
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				psychicRitualToil.Start(psychicRitual, parent);
			}
		}

		// Token: 0x06006730 RID: 26416 RVA: 0x0020ABA0 File Offset: 0x00208DA0
		public override void End(PsychicRitual psychicRitual, PsychicRitualGraph parent, bool success)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				psychicRitualToil.End(psychicRitual, parent, success);
			}
		}

		// Token: 0x06006731 RID: 26417 RVA: 0x0020ABF4 File Offset: 0x00208DF4
		public override bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			bool flag = true;
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				flag = (psychicRitualToil.Tick(psychicRitual, parent) && flag);
			}
			return flag;
		}

		// Token: 0x06006732 RID: 26418 RVA: 0x0020AC4C File Offset: 0x00208E4C
		public override AcceptanceReport AllowsFloatMenu(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.AllowsFloatMenu(psychicRitual, parent, pawn);
			}
			return base.AllowsFloatMenu(psychicRitual, parent, pawn);
		}

		// Token: 0x06006733 RID: 26419 RVA: 0x0020AC78 File Offset: 0x00208E78
		public override bool BlocksSocialInteraction(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.BlocksSocialInteraction(psychicRitual, parent, pawn);
			}
			return base.BlocksSocialInteraction(psychicRitual, parent, pawn);
		}

		// Token: 0x06006734 RID: 26420 RVA: 0x0020ACA4 File Offset: 0x00208EA4
		public override void SetPawnDuty(Pawn pawn, PsychicRitual psychicRitual, PsychicRitualGraph parent, DutyDef def, LocalTargetInfo? focus = null, LocalTargetInfo? focusSecond = null, LocalTargetInfo? focusThird = null, string tag = null, float wanderRadius = 0f, Rot4? overrideFacing = null)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				psychicRitualToil.SetPawnDuty(pawn, psychicRitual, parent, def, focus, focusSecond, focusThird, tag, wanderRadius, null);
				return;
			}
			base.SetPawnDuty(pawn, psychicRitual, parent, def, focus, focusSecond, focusThird, tag, wanderRadius, null);
		}

		// Token: 0x06006735 RID: 26421 RVA: 0x0020ACFC File Offset: 0x00208EFC
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				psychicRitualToil.UpdateAllDuties(psychicRitual, parent);
			}
		}

		// Token: 0x06006736 RID: 26422 RVA: 0x0020AD50 File Offset: 0x00208F50
		public override bool DutyActiveWhenDown(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.DutyActiveWhenDown(psychicRitual, parent, pawn);
			}
			return base.DutyActiveWhenDown(psychicRitual, parent, pawn);
		}

		// Token: 0x06006737 RID: 26423 RVA: 0x0020AD7C File Offset: 0x00208F7C
		public override bool ClearJobOnStart(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.ClearJobOnStart(psychicRitual, parent, pawn);
			}
			return base.ClearJobOnStart(psychicRitual, parent, pawn);
		}

		// Token: 0x06006738 RID: 26424 RVA: 0x0020ADA8 File Offset: 0x00208FA8
		public override string GetJobReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.GetJobReport(psychicRitual, parent, pawn);
			}
			return base.GetJobReport(psychicRitual, parent, pawn);
		}

		// Token: 0x06006739 RID: 26425 RVA: 0x0020ADD4 File Offset: 0x00208FD4
		public override IEnumerable<Gizmo> GetPawnGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.GetPawnGizmos(psychicRitual, parent, pawn);
			}
			return base.GetPawnGizmos(psychicRitual, parent, pawn);
		}

		// Token: 0x0600673A RID: 26426 RVA: 0x0020AE00 File Offset: 0x00209000
		public override IEnumerable<Gizmo> GetBuildingGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			return this.childToils.SelectMany((PsychicRitualToil toil) => toil.GetBuildingGizmos(psychicRitual, parent, building));
		}

		// Token: 0x0600673B RID: 26427 RVA: 0x0020AE40 File Offset: 0x00209040
		public override string GetPawnReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.GetPawnReport(psychicRitual, parent, pawn);
			}
			return base.GetPawnReport(psychicRitual, parent, pawn);
		}

		// Token: 0x0600673C RID: 26428 RVA: 0x00002C42 File Offset: 0x00000E42
		public override string GetReport(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			return null;
		}

		// Token: 0x0600673D RID: 26429 RVA: 0x0020AE6C File Offset: 0x0020906C
		public override bool ShouldRemovePawn(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, PawnLostCondition condition)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.ShouldRemovePawn(psychicRitual, parent, pawn, condition);
			}
			return base.ShouldRemovePawn(psychicRitual, parent, pawn, condition);
		}

		// Token: 0x0600673E RID: 26430 RVA: 0x0020AE9C File Offset: 0x0020909C
		public override AcceptanceReport AbilityAllowed(PsychicRitual psychicRitual, PsychicRitualGraph parent, Ability ability)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				AcceptanceReport result = psychicRitualToil.AbilityAllowed(psychicRitual, parent, ability);
				if (!result.Accepted)
				{
					return result;
				}
			}
			return base.AbilityAllowed(psychicRitual, parent, ability);
		}

		// Token: 0x0600673F RID: 26431 RVA: 0x0020AF08 File Offset: 0x00209108
		public override void Notify_BuildingDespawnedOnMap(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				psychicRitualToil.Notify_BuildingDespawnedOnMap(psychicRitual, parent, building);
			}
		}

		// Token: 0x06006740 RID: 26432 RVA: 0x0020AF5C File Offset: 0x0020915C
		public override void Notify_BuildingLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				psychicRitualToil.Notify_BuildingLost(psychicRitual, parent, building);
			}
		}

		// Token: 0x06006741 RID: 26433 RVA: 0x0020AFB0 File Offset: 0x002091B0
		public override void Notify_BuildingSpawnedOnMap(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				psychicRitualToil.Notify_BuildingSpawnedOnMap(psychicRitual, parent, building);
			}
		}

		// Token: 0x06006742 RID: 26434 RVA: 0x0020B004 File Offset: 0x00209204
		public override void Notify_CorpseLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Corpse corpse)
		{
			foreach (PsychicRitualToil psychicRitualToil in this.childToils)
			{
				psychicRitualToil.Notify_CorpseLost(psychicRitual, parent, corpse);
			}
		}

		// Token: 0x06006743 RID: 26435 RVA: 0x0020B058 File Offset: 0x00209258
		public override ThinkResult Notify_DutyConstantResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.Notify_DutyConstantResult(psychicRitual, parent, result, pawn, issueParams);
			}
			return base.Notify_DutyConstantResult(psychicRitual, parent, result, pawn, issueParams);
		}

		// Token: 0x06006744 RID: 26436 RVA: 0x0020B090 File Offset: 0x00209290
		public override ThinkResult Notify_DutyResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				return psychicRitualToil.Notify_DutyResult(psychicRitual, parent, result, pawn, issueParams);
			}
			return base.Notify_DutyResult(psychicRitual, parent, result, pawn, issueParams);
		}

		// Token: 0x06006745 RID: 26437 RVA: 0x0020B0C8 File Offset: 0x002092C8
		public override void Notify_PawnJobDone(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Job job, JobCondition condition)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				psychicRitualToil.Notify_PawnJobDone(psychicRitual, parent, pawn, job, condition);
				return;
			}
			base.Notify_PawnJobDone(psychicRitual, parent, pawn, job, condition);
		}

		// Token: 0x06006746 RID: 26438 RVA: 0x0020B0FC File Offset: 0x002092FC
		public override void Notify_ReachedDutyLocation(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				psychicRitualToil.Notify_ReachedDutyLocation(psychicRitual, parent, pawn);
				return;
			}
			base.Notify_ReachedDutyLocation(psychicRitual, parent, pawn);
		}

		// Token: 0x06006747 RID: 26439 RVA: 0x0020B128 File Offset: 0x00209328
		public override void Notify_PawnLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, PawnLostCondition cond)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				psychicRitualToil.Notify_PawnLost(psychicRitual, parent, pawn, cond);
				return;
			}
			base.Notify_PawnLost(psychicRitual, parent, pawn, cond);
		}

		// Token: 0x06006748 RID: 26440 RVA: 0x0020B158 File Offset: 0x00209358
		public override void Notify_ConstructionFailed(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
			PsychicRitualToil psychicRitualToil = this.ToilForPawn(psychicRitual, pawn);
			if (psychicRitualToil != null)
			{
				psychicRitualToil.Notify_ConstructionFailed(psychicRitual, parent, pawn, frame, newBlueprint);
				return;
			}
			base.Notify_ConstructionFailed(psychicRitual, parent, pawn, frame, newBlueprint);
		}

		// Token: 0x06006749 RID: 26441 RVA: 0x0020B18C File Offset: 0x0020938C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<PsychicRitualRoleDef, PsychicRitualToil>(ref this.roleToils, "roleToils", LookMode.Def, LookMode.Reference, ref this.tmpRolesLoading, ref this.tmpToilsLoading, true, false, false);
			Scribe_Collections.Look<PsychicRitualToil>(ref this.childToils, "childToils", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<PsychicRitualToil>(ref this.fallback, "fallback", false);
		}

		// Token: 0x0400499D RID: 18845
		private Dictionary<PsychicRitualRoleDef, PsychicRitualToil> roleToils;

		// Token: 0x0400499E RID: 18846
		private List<PsychicRitualToil> childToils;

		// Token: 0x0400499F RID: 18847
		private PsychicRitualToil fallback;

		// Token: 0x040049A0 RID: 18848
		private List<PsychicRitualRoleDef> tmpRolesLoading = new List<PsychicRitualRoleDef>();

		// Token: 0x040049A1 RID: 18849
		private List<PsychicRitualToil> tmpToilsLoading = new List<PsychicRitualToil>();
	}
}
