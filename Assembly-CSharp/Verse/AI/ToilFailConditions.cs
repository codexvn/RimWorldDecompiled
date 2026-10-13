using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000EEF RID: 3823
	public static class ToilFailConditions
	{
		// Token: 0x06005FE5 RID: 24549 RVA: 0x001E7698 File Offset: 0x001E5898
		public static Toil FailOn(this Toil toil, Func<Toil, bool> condition)
		{
			toil.AddEndCondition(delegate
			{
				if (condition(toil))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return toil;
		}

		// Token: 0x06005FE6 RID: 24550 RVA: 0x001E76D8 File Offset: 0x001E58D8
		public static T FailOn<T>(this T f, Func<bool> condition) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (condition())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FE7 RID: 24551 RVA: 0x001E770C File Offset: 0x001E590C
		public static T FailOnDestroyedOrNull<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (f.GetActor().jobs.curJob.GetTarget(ind).Thing.DestroyedOrNull())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FE8 RID: 24552 RVA: 0x001E7750 File Offset: 0x001E5950
		public static T FailOnInvalidOrDestroyed<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				LocalTargetInfo target = f.GetActor().jobs.curJob.GetTarget(ind);
				if (!target.IsValid || target.ThingDestroyed)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FE9 RID: 24553 RVA: 0x001E7794 File Offset: 0x001E5994
		public static bool DespawnedOrNull(LocalTargetInfo target, Pawn actor)
		{
			Thing thing = target.Thing;
			return (thing != null || !target.IsValid) && (thing == null || !thing.Spawned || thing.Map != actor.Map);
		}

		// Token: 0x06005FEA RID: 24554 RVA: 0x001E77D4 File Offset: 0x001E59D4
		public static T FailOnDespawnedOrNull<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (ToilFailConditions.DespawnedOrNull(f.GetActor().jobs.curJob.GetTarget(ind), f.GetActor()))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FEB RID: 24555 RVA: 0x001E7818 File Offset: 0x001E5A18
		public static bool SelfAndParentsDespawnedOrNull(LocalTargetInfo target, Pawn actor)
		{
			Thing thing = target.Thing;
			return (thing != null || !target.IsValid) && (thing == null || !thing.SpawnedOrAnyParentSpawned || thing.MapHeld != actor.Map);
		}

		// Token: 0x06005FEC RID: 24556 RVA: 0x001E7858 File Offset: 0x001E5A58
		public static T FailOnSelfAndParentsDespawnedOrNull<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (ToilFailConditions.SelfAndParentsDespawnedOrNull(f.GetActor().jobs.curJob.GetTarget(ind), f.GetActor()))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FED RID: 24557 RVA: 0x001E789C File Offset: 0x001E5A9C
		public static T EndOnDespawnedOrNull<T>(this T f, TargetIndex ind, JobCondition endCondition = JobCondition.Incompletable) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (ToilFailConditions.DespawnedOrNull(f.GetActor().jobs.curJob.GetTarget(ind), f.GetActor()))
				{
					return endCondition;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FEE RID: 24558 RVA: 0x001E78E8 File Offset: 0x001E5AE8
		public static T EndOnNoTargetInQueue<T>(this T f, TargetIndex ind, JobCondition endCondition = JobCondition.Incompletable) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (f.GetActor().jobs.curJob.GetTargetQueue(ind).NullOrEmpty<LocalTargetInfo>())
				{
					return endCondition;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FEF RID: 24559 RVA: 0x001E7934 File Offset: 0x001E5B34
		public static T FailOnDowned<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (((Pawn)f.GetActor().jobs.curJob.GetTarget(ind).Thing).Downed)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FF0 RID: 24560 RVA: 0x001E7978 File Offset: 0x001E5B78
		public static T FailOnMobile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (((Pawn)f.GetActor().jobs.curJob.GetTarget(ind).Thing).health.State == PawnHealthState.Mobile)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FF1 RID: 24561 RVA: 0x001E79BC File Offset: 0x001E5BBC
		public static T FailOnNotDowned<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (!((Pawn)f.GetActor().jobs.curJob.GetTarget(ind).Thing).Downed)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FF2 RID: 24562 RVA: 0x001E7A00 File Offset: 0x001E5C00
		public static T FailOnNotAwake<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (!((Pawn)f.GetActor().jobs.curJob.GetTarget(ind).Thing).Awake())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FF3 RID: 24563 RVA: 0x001E7A44 File Offset: 0x001E5C44
		public static T FailOnNotCasualInterruptible<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (!((Pawn)f.GetActor().jobs.curJob.GetTarget(ind).Thing).CanCasuallyInteractNow(false, false, false, false))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FF4 RID: 24564 RVA: 0x001E7A88 File Offset: 0x001E5C88
		public static T FailOnMentalState<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn pawn = f.GetActor().jobs.curJob.GetTarget(ind).Thing as Pawn;
				if (pawn != null && pawn.InMentalState && !pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FF5 RID: 24565 RVA: 0x001E7ACC File Offset: 0x001E5CCC
		public static T FailOnAggroMentalState<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn pawn = f.GetActor().jobs.curJob.GetTarget(ind).Thing as Pawn;
				if (pawn != null && pawn.InAggroMentalState && !pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FF6 RID: 24566 RVA: 0x001E7B10 File Offset: 0x001E5D10
		public static T FailOnAggroMentalStateAndHostile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn pawn = f.GetActor().jobs.curJob.GetTarget(ind).Thing as Pawn;
				if (pawn != null && pawn.InAggroMentalState && !pawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false) && pawn.HostileTo(f.GetActor()))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FF7 RID: 24567 RVA: 0x001E7B54 File Offset: 0x001E5D54
		public static T FailOnSomeonePhysicallyInteracting<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
				if (thing != null && actor.Map.physicalInteractionReservationManager.IsReserved(thing) && !actor.Map.physicalInteractionReservationManager.IsReservedBy(actor, thing))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FF8 RID: 24568 RVA: 0x001E7B98 File Offset: 0x001E5D98
		public static T FailOnForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				if (actor.Faction != Faction.OfPlayer)
				{
					return JobCondition.Ongoing;
				}
				if (actor.jobs.curJob.ignoreForbidden)
				{
					return JobCondition.Ongoing;
				}
				IntVec3 cell = actor.jobs.curJob.GetTarget(ind).Cell;
				Thing thing = actor.jobs.curJob.GetTarget(ind).Thing;
				IThingHolder thingHolder = (thing != null) ? thing.ParentHolder : null;
				if (!(thingHolder is Pawn_CarryTracker) && !(thingHolder is Pawn_InventoryTracker) && cell.IsValid && cell.IsForbidden(actor))
				{
					return JobCondition.Incompletable;
				}
				if (thing == null)
				{
					return JobCondition.Ongoing;
				}
				if (thing.IsForbidden(actor))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FF9 RID: 24569 RVA: 0x001E7BDC File Offset: 0x001E5DDC
		public static T FailOnDespawnedNullOrForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.FailOnDespawnedOrNull(ind);
			f.FailOnForbidden(ind);
			return f;
		}

		// Token: 0x06005FFA RID: 24570 RVA: 0x001E7BEF File Offset: 0x001E5DEF
		public static T FailOnDestroyedNullOrForbidden<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.FailOnDestroyedOrNull(ind);
			f.FailOnForbidden(ind);
			return f;
		}

		// Token: 0x06005FFB RID: 24571 RVA: 0x001E7C04 File Offset: 0x001E5E04
		public static T FailOnThingMissingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Job curJob = actor.jobs.curJob;
				if (curJob.ignoreDesignations)
				{
					return JobCondition.Ongoing;
				}
				Thing thing = curJob.GetTarget(ind).Thing;
				if (thing == null || actor.Map.designationManager.DesignationOn(thing, desDef) == null)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FFC RID: 24572 RVA: 0x001E7C50 File Offset: 0x001E5E50
		public static T FailOnThingHavingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Job curJob = actor.jobs.curJob;
				if (curJob.ignoreDesignations)
				{
					return JobCondition.Ongoing;
				}
				Thing thing = curJob.GetTarget(ind).Thing;
				if (thing == null || actor.Map.designationManager.DesignationOn(thing, desDef) != null)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FFD RID: 24573 RVA: 0x001E7C9C File Offset: 0x001E5E9C
		public static T FailOnCellMissingDesignation<T>(this T f, TargetIndex ind, DesignationDef desDef) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				Job curJob = actor.jobs.curJob;
				if (curJob.ignoreDesignations)
				{
					return JobCondition.Ongoing;
				}
				if (actor.Map.designationManager.DesignationAt(curJob.GetTarget(ind).Cell, desDef) == null)
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FFE RID: 24574 RVA: 0x001E7CE8 File Offset: 0x001E5EE8
		public static T FailOnBurningImmobile<T>(this T f, TargetIndex ind) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				LocalTargetInfo target = actor.jobs.curJob.GetTarget(ind);
				if (target.IsValid && target.ToTargetInfo(actor.Map).IsBurning())
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06005FFF RID: 24575 RVA: 0x001E7D2C File Offset: 0x001E5F2C
		public static T FailOnCannotTouch<T>(this T f, TargetIndex ind, PathEndMode peMode) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (!f.GetActor().CanReachImmediate(f.GetActor().jobs.curJob.GetTarget(ind), peMode))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06006000 RID: 24576 RVA: 0x001E7D78 File Offset: 0x001E5F78
		public static T FailOnIncapable<T>(this T f, PawnCapacityDef pawnCapacity) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				if (!f.GetActor().health.capacities.CapableOf(pawnCapacity))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06006001 RID: 24577 RVA: 0x001E7DBC File Offset: 0x001E5FBC
		public static T FailOnChildLearningConditions<T>(this T f) where T : IJobEndable
		{
			f.AddEndCondition(delegate
			{
				Pawn actor = f.GetActor();
				if (!actor.DevelopmentalStage.Juvenile() || PawnUtility.WillSoonHaveBasicNeed(actor, -0.05f))
				{
					return JobCondition.Incompletable;
				}
				return JobCondition.Ongoing;
			});
			return f;
		}

		// Token: 0x06006002 RID: 24578 RVA: 0x001E7DFC File Offset: 0x001E5FFC
		public static Toil FailOnDespawnedNullOrForbiddenPlacedThings(this Toil toil, TargetIndex containerIndex = TargetIndex.None)
		{
			toil.AddFailCondition(delegate
			{
				if (toil.actor.jobs.curJob.placedThings == null)
				{
					return false;
				}
				for (int i = 0; i < toil.actor.jobs.curJob.placedThings.Count; i++)
				{
					ThingCountClass thingCountClass = toil.actor.jobs.curJob.placedThings[i];
					Thing thing = toil.actor.jobs.curJob.GetTarget(containerIndex).Thing;
					ThingOwner thingOwner = (thing != null) ? thing.TryGetInnerInteractableThingOwner() : null;
					if (thingCountClass.thing == null || (!thingCountClass.thing.Spawned && (thingOwner == null || !thingOwner.Contains(thingCountClass.thing))) || thingCountClass.thing.MapHeld != toil.actor.Map || (!toil.actor.CurJob.ignoreForbidden && thingCountClass.thing.IsForbidden(toil.actor)))
					{
						return true;
					}
				}
				return false;
			});
			return toil;
		}
	}
}
