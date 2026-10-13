using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02001065 RID: 4197
	public abstract class PsychicRitualToil : IExposable, ILoadReferenceable
	{
		// Token: 0x0600668C RID: 26252 RVA: 0x002071D7 File Offset: 0x002053D7
		public PsychicRitualToil()
		{
			this.uniqueId = string.Format("PsychicRitualToil_{0}", Find.UniqueIDsManager.GetNextPsychicRitualToilID());
		}

		// Token: 0x0600668D RID: 26253 RVA: 0x002071FE File Offset: 0x002053FE
		public string GetUniqueLoadID()
		{
			return this.uniqueId;
		}

		// Token: 0x0600668E RID: 26254 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
		}

		// Token: 0x0600668F RID: 26255 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			return true;
		}

		// Token: 0x06006690 RID: 26256 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
		}

		// Token: 0x06006691 RID: 26257 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void End(PsychicRitual psychicRitual, PsychicRitualGraph parent, bool success)
		{
		}

		// Token: 0x06006692 RID: 26258 RVA: 0x00207206 File Offset: 0x00205406
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.uniqueId, "uniqueId", null, false);
		}

		// Token: 0x06006693 RID: 26259 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string GetJobReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			return null;
		}

		// Token: 0x06006694 RID: 26260 RVA: 0x0020721A File Offset: 0x0020541A
		public virtual string GetPawnReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			return psychicRitual.def.GetPawnReport(psychicRitual, pawn);
		}

		// Token: 0x06006695 RID: 26261 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string GetReport(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			return null;
		}

		// Token: 0x06006696 RID: 26262 RVA: 0x00207229 File Offset: 0x00205429
		public virtual AcceptanceReport AllowsFloatMenu(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			return psychicRitual.def.AllowsFloatMenu(pawn);
		}

		// Token: 0x06006697 RID: 26263 RVA: 0x00207237 File Offset: 0x00205437
		public virtual bool BlocksSocialInteraction(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			return psychicRitual.def.BlocksSocialInteraction(pawn);
		}

		// Token: 0x06006698 RID: 26264 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool ClearJobOnStart(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			return true;
		}

		// Token: 0x06006699 RID: 26265 RVA: 0x00207245 File Offset: 0x00205445
		public virtual AcceptanceReport AbilityAllowed(PsychicRitual psychicRitual, PsychicRitualGraph parent, Ability ability)
		{
			return psychicRitual.def.AbilityAllowed(ability);
		}

		// Token: 0x0600669A RID: 26266 RVA: 0x00207254 File Offset: 0x00205454
		public virtual IEnumerable<Gizmo> GetPawnGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualRoleDef psychicRitualRoleDef = psychicRitual.assignments.RoleForPawn(pawn, true);
			if (psychicRitualRoleDef == null)
			{
				return Enumerable.Empty<Gizmo>();
			}
			return psychicRitualRoleDef.GetPawnGizmos(psychicRitual, pawn);
		}

		// Token: 0x0600669B RID: 26267 RVA: 0x000B7861 File Offset: 0x000B5A61
		public virtual IEnumerable<Gizmo> GetBuildingGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x0600669C RID: 26268 RVA: 0x00207280 File Offset: 0x00205480
		public virtual void SetPawnDuty(Pawn pawn, PsychicRitual psychicRitual, PsychicRitualGraph parent, DutyDef def, LocalTargetInfo? focus = null, LocalTargetInfo? focusSecond = null, LocalTargetInfo? focusThird = null, string tag = null, float wanderRadius = 0f, Rot4? overrideFacing = null)
		{
			if (((pawn != null) ? pawn.mindState : null) == null)
			{
				return;
			}
			pawn.mindState.duty = new PawnDuty(def, focus ?? LocalTargetInfo.Invalid, focusSecond ?? LocalTargetInfo.Invalid, focusThird ?? LocalTargetInfo.Invalid, -1f)
			{
				tag = tag,
				socialModeMaxOverride = new RandomSocialMode?(RandomSocialMode.Off),
				source = this,
				locomotion = LocomotionUrgency.Sprint,
				wanderRadius = new float?(wanderRadius),
				overrideFacing = (overrideFacing ?? Rot4.Invalid)
			};
		}

		// Token: 0x0600669D RID: 26269 RVA: 0x00207350 File Offset: 0x00205550
		public virtual bool ShouldRemovePawn(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, PawnLostCondition condition)
		{
			PsychicRitualRoleDef psychicRitualRoleDef = psychicRitual.assignments.RoleForPawn(pawn, true);
			return psychicRitualRoleDef == null || !psychicRitualRoleDef.ConditionAllowed(condition);
		}

		// Token: 0x0600669E RID: 26270 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, PawnLostCondition cond)
		{
		}

		// Token: 0x0600669F RID: 26271 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnJobDone(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Job job, JobCondition condition)
		{
		}

		// Token: 0x060066A0 RID: 26272 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_BuildingLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
		}

		// Token: 0x060066A1 RID: 26273 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_CorpseLost(PsychicRitual psychicRitual, PsychicRitualGraph parent, Corpse corpse)
		{
		}

		// Token: 0x060066A2 RID: 26274 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_ReachedDutyLocation(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
		}

		// Token: 0x060066A3 RID: 26275 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_ConstructionFailed(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
		}

		// Token: 0x060066A4 RID: 26276 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_BuildingSpawnedOnMap(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
		}

		// Token: 0x060066A5 RID: 26277 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_BuildingDespawnedOnMap(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
		}

		// Token: 0x060066A6 RID: 26278 RVA: 0x001CA655 File Offset: 0x001C8855
		public virtual ThinkResult Notify_DutyConstantResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			return result;
		}

		// Token: 0x060066A7 RID: 26279 RVA: 0x001CA655 File Offset: 0x001C8855
		public virtual ThinkResult Notify_DutyResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			return result;
		}

		// Token: 0x060066A8 RID: 26280 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool DutyActiveWhenDown(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			return false;
		}

		// Token: 0x04004944 RID: 18756
		private string uniqueId;
	}
}
