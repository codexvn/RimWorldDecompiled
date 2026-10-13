using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F18 RID: 3864
	public static class Toils_General
	{
		// Token: 0x0600605C RID: 24668 RVA: 0x001E9138 File Offset: 0x001E7338
		public static Toil StopDead()
		{
			Toil toil = ToilMaker.MakeToil("StopDead");
			toil.initAction = delegate()
			{
				toil.actor.pather.StopDead();
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x0600605D RID: 24669 RVA: 0x001E9184 File Offset: 0x001E7384
		public static Toil Wait(int ticks, TargetIndex face = TargetIndex.None)
		{
			Toil toil = ToilMaker.MakeToil("Wait");
			toil.initAction = delegate()
			{
				toil.actor.pather.StopDead();
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = ticks;
			if (face != TargetIndex.None)
			{
				toil.handlingFacing = true;
				toil.tickIntervalAction = delegate(int delta)
				{
					toil.actor.rotationTracker.FaceTarget(toil.actor.CurJob.GetTarget(face));
				};
			}
			return toil;
		}

		// Token: 0x0600605E RID: 24670 RVA: 0x001E9210 File Offset: 0x001E7410
		public static Toil WaitWith(TargetIndex targetInd, int ticks, bool useProgressBar = false, bool maintainPosture = false, bool maintainSleep = false, TargetIndex face = TargetIndex.None, PathEndMode pathEndMode = PathEndMode.Touch)
		{
			Toil toil = ToilMaker.MakeToil("WaitWith");
			toil.initAction = delegate()
			{
				toil.actor.pather.StopDead();
				Pawn pawn = toil.actor.CurJob.GetTarget(targetInd).Thing as Pawn;
				if (pawn != null)
				{
					if (pawn == toil.actor)
					{
						Log.Warning("Executing WaitWith toil but otherPawn is the same as toil.actor");
						return;
					}
					PawnUtility.ForceWait(pawn, ticks, null, maintainPosture, maintainSleep);
				}
			};
			toil.FailOnDespawnedOrNull(targetInd);
			toil.FailOnCannotTouch(targetInd, pathEndMode);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = ticks;
			if (face != TargetIndex.None)
			{
				toil.handlingFacing = true;
				toil.tickIntervalAction = delegate(int delta)
				{
					toil.actor.rotationTracker.FaceTarget(toil.actor.CurJob.GetTarget(face));
				};
			}
			if (useProgressBar)
			{
				toil.WithProgressBarToilDelay(targetInd, false, -0.5f);
			}
			return toil;
		}

		// Token: 0x0600605F RID: 24671 RVA: 0x001E9300 File Offset: 0x001E7500
		public static Toil RemoveDesignationsOnThing(TargetIndex ind, DesignationDef def)
		{
			Toil toil = ToilMaker.MakeToil("RemoveDesignationsOnThing");
			toil.initAction = delegate()
			{
				toil.actor.Map.designationManager.RemoveAllDesignationsOn(toil.actor.jobs.curJob.GetTarget(ind).Thing, false);
			};
			return toil;
		}

		// Token: 0x06006060 RID: 24672 RVA: 0x001E9348 File Offset: 0x001E7548
		public static Toil ClearTarget(TargetIndex ind)
		{
			Toil toil = ToilMaker.MakeToil("ClearTarget");
			toil.initAction = delegate()
			{
				toil.GetActor().CurJob.SetTarget(ind, null);
			};
			return toil;
		}

		// Token: 0x06006061 RID: 24673 RVA: 0x001E9390 File Offset: 0x001E7590
		public static Toil PutCarriedThingInInventory()
		{
			Toil toil = ToilMaker.MakeToil("PutCarriedThingInInventory");
			toil.initAction = delegate()
			{
				Pawn actor = toil.GetActor();
				if (actor.carryTracker.CarriedThing != null && !actor.carryTracker.innerContainer.TryTransferToContainer(actor.carryTracker.CarriedThing, actor.inventory.innerContainer, true))
				{
					Thing thing;
					actor.carryTracker.TryDropCarriedThing(actor.Position, actor.carryTracker.CarriedThing.stackCount, ThingPlaceMode.Near, out thing, null);
				}
			};
			return toil;
		}

		// Token: 0x06006062 RID: 24674 RVA: 0x001E93D0 File Offset: 0x001E75D0
		public static Toil Do(Action action)
		{
			Toil toil = ToilMaker.MakeToil("Do");
			toil.initAction = action;
			return toil;
		}

		// Token: 0x06006063 RID: 24675 RVA: 0x001E93E3 File Offset: 0x001E75E3
		public static Toil DoAtomic(Action action)
		{
			Toil toil = ToilMaker.MakeToil("DoAtomic");
			toil.initAction = action;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06006064 RID: 24676 RVA: 0x001E9400 File Offset: 0x001E7600
		public static Toil Open(TargetIndex openableInd)
		{
			Toil open = ToilMaker.MakeToil("Open");
			open.initAction = delegate()
			{
				Pawn actor = open.actor;
				Thing thing = actor.CurJob.GetTarget(openableInd).Thing;
				Designation designation = actor.Map.designationManager.DesignationOn(thing, DesignationDefOf.Open);
				if (designation != null)
				{
					designation.Delete();
				}
				IOpenable openable = (IOpenable)thing;
				if (openable.CanOpen)
				{
					openable.Open();
					actor.records.Increment(RecordDefOf.ContainersOpened);
				}
			};
			open.defaultCompleteMode = ToilCompleteMode.Instant;
			return open;
		}

		// Token: 0x06006065 RID: 24677 RVA: 0x001E9453 File Offset: 0x001E7653
		public static Toil Label()
		{
			Toil toil = ToilMaker.MakeToil("Label");
			toil.atomicWithPrevious = true;
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06006066 RID: 24678 RVA: 0x001E9470 File Offset: 0x001E7670
		public static Toil WaitWhileExtractingContents(TargetIndex containerInd, TargetIndex contentsInd, int openTicks)
		{
			Toil extract = Toils_General.Wait(openTicks, containerInd).WithProgressBarToilDelay(containerInd, false, -0.5f).FailOnDespawnedOrNull(containerInd);
			extract.handlingFacing = true;
			extract.AddPreInitAction(delegate
			{
				Thing thing = extract.actor.CurJob.GetTarget(contentsInd).Thing;
				QuestUtility.SendQuestTargetSignals(thing.questTags, "StartedExtractingFromContainer", thing.Named("SUBJECT"));
			});
			return extract;
		}
	}
}
