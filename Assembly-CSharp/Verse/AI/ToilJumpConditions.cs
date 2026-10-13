using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F0A RID: 3850
	public static class ToilJumpConditions
	{
		// Token: 0x06006037 RID: 24631 RVA: 0x001E87C0 File Offset: 0x001E69C0
		public static Toil JumpIf(this Toil toil, Func<bool> jumpCondition, Toil jumpToil)
		{
			toil.AddPreTickAction(delegate
			{
				if (jumpCondition())
				{
					toil.actor.jobs.curDriver.JumpToToil(jumpToil);
				}
			});
			return toil;
		}

		// Token: 0x06006038 RID: 24632 RVA: 0x001E8808 File Offset: 0x001E6A08
		public static Toil JumpIfDespawnedOrNull(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned;
			}, jumpToil);
		}

		// Token: 0x06006039 RID: 24633 RVA: 0x001E8844 File Offset: 0x001E6A44
		public static Toil JumpIfDespawnedOrNullOrForbidden(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return thing == null || !thing.Spawned || thing.IsForbidden(toil.actor);
			}, jumpToil);
		}

		// Token: 0x0600603A RID: 24634 RVA: 0x001E8880 File Offset: 0x001E6A80
		public static Toil JumpIfOutsideHomeArea(this Toil toil, TargetIndex ind, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Thing thing = toil.actor.jobs.curJob.GetTarget(ind).Thing;
				return !toil.actor.Map.areaManager.Home[thing.Position];
			}, jumpToil);
		}

		// Token: 0x0600603B RID: 24635 RVA: 0x001E88BC File Offset: 0x001E6ABC
		public static Toil JumpIfThingMissingDesignation(this Toil toil, TargetIndex ind, DesignationDef desDef, Toil jumpToil)
		{
			return toil.JumpIf(delegate
			{
				Pawn actor = toil.GetActor();
				Job curJob = actor.jobs.curJob;
				if (curJob.ignoreDesignations)
				{
					return false;
				}
				Thing thing = curJob.GetTarget(ind).Thing;
				return thing == null || actor.Map.designationManager.DesignationOn(thing, desDef) == null;
			}, jumpToil);
		}
	}
}
