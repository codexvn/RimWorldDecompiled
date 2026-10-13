using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200117D RID: 4477
	public class JobDriver_Repair : JobDriver
	{
		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x06006D8A RID: 28042 RVA: 0x00224760 File Offset: 0x00222960
		private Building Building
		{
			get
			{
				return (Building)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x06006D8B RID: 28043 RVA: 0x00224788 File Offset: 0x00222988
		private bool IsBuildingAttachment
		{
			get
			{
				ThingDef thingDef = GenConstruct.BuiltDefOf(this.Building.def) as ThingDef;
				return ((thingDef != null) ? thingDef.building : null) != null && thingDef.building.isAttachment;
			}
		}

		// Token: 0x06006D8C RID: 28044 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006D8D RID: 28045 RVA: 0x002247C6 File Offset: 0x002229C6
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			PathEndMode pathEndMode = this.IsBuildingAttachment ? PathEndMode.OnCell : PathEndMode.Touch;
			yield return Toils_Goto.GotoThing(TargetIndex.A, pathEndMode, false);
			Toil repair = ToilMaker.MakeToil("MakeNewToils");
			repair.initAction = delegate()
			{
				this.ticksToNextRepair = 80f;
			};
			repair.tickAction = delegate()
			{
				Pawn actor = repair.actor;
				Pawn_SkillTracker skills = actor.skills;
				if (skills != null)
				{
					skills.Learn(SkillDefOf.Construction, 0.05f, false, false);
				}
				if (this.IsBuildingAttachment)
				{
					actor.rotationTracker.FaceTarget(GenConstruct.GetWallAttachedTo(this.Building));
				}
				else
				{
					actor.rotationTracker.FaceTarget(actor.CurJob.GetTarget(TargetIndex.A));
				}
				float num = actor.GetStatValue(StatDefOf.ConstructionSpeed, true, -1) * 1.7f;
				this.ticksToNextRepair -= num;
				if (this.ticksToNextRepair <= 0f)
				{
					this.ticksToNextRepair += 20f;
					this.TargetThingA.HitPoints++;
					this.TargetThingA.HitPoints = Mathf.Min(this.TargetThingA.HitPoints, this.TargetThingA.MaxHitPoints);
					this.Map.listerBuildingsRepairable.Notify_BuildingRepaired((Building)this.TargetThingA);
					if (this.TargetThingA.HitPoints == this.TargetThingA.MaxHitPoints)
					{
						actor.records.Increment(RecordDefOf.ThingsRepaired);
						actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					}
				}
			};
			repair.FailOnCannotTouch(TargetIndex.A, pathEndMode);
			repair.WithEffect(base.TargetThingA.def.repairEffect, TargetIndex.A, null);
			repair.defaultCompleteMode = ToilCompleteMode.Never;
			repair.activeSkill = (() => SkillDefOf.Construction);
			repair.handlingFacing = true;
			yield return repair;
			yield break;
		}

		// Token: 0x04004CA8 RID: 19624
		protected float ticksToNextRepair;

		// Token: 0x04004CA9 RID: 19625
		private const float WarmupTicks = 80f;

		// Token: 0x04004CAA RID: 19626
		private const float TicksBetweenRepairs = 20f;
	}
}
