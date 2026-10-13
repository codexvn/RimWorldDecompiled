using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012BA RID: 4794
	public class JobDriver_ManTurret : JobDriver
	{
		// Token: 0x1700126C RID: 4716
		// (get) Token: 0x06007578 RID: 30072 RVA: 0x0023DE44 File Offset: 0x0023C044
		private Building_TurretGun Turret
		{
			get
			{
				return (Building_TurretGun)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x06007579 RID: 30073 RVA: 0x0023DE6C File Offset: 0x0023C06C
		private Thing Hauling
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x0600757A RID: 30074 RVA: 0x0023DE90 File Offset: 0x0023C090
		private static bool GunNeedsLoading(Building b)
		{
			Building_TurretGun building_TurretGun = b as Building_TurretGun;
			if (building_TurretGun == null)
			{
				return false;
			}
			CompChangeableProjectile compChangeableProjectile = building_TurretGun.gun.TryGetComp<CompChangeableProjectile>();
			return compChangeableProjectile != null && !compChangeableProjectile.Loaded;
		}

		// Token: 0x0600757B RID: 30075 RVA: 0x0023DEC4 File Offset: 0x0023C0C4
		private static bool GunNeedsRefueling(Building b)
		{
			Building_TurretGun building_TurretGun = b as Building_TurretGun;
			if (building_TurretGun == null)
			{
				return false;
			}
			CompRefuelable compRefuelable = building_TurretGun.TryGetComp<CompRefuelable>();
			return compRefuelable != null && !compRefuelable.HasFuel && compRefuelable.Props.fuelIsMortarBarrel && !Find.Storyteller.difficulty.classicMortars;
		}

		// Token: 0x0600757C RID: 30076 RVA: 0x0023DF10 File Offset: 0x0023C110
		public static Thing FindAmmoForTurret(Pawn pawn, Building_TurretGun gun)
		{
			JobDriver_ManTurret.<>c__DisplayClass10_0 CS$<>8__locals1 = new JobDriver_ManTurret.<>c__DisplayClass10_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.allowedShellsSettings = ((CS$<>8__locals1.pawn.IsColonist || CS$<>8__locals1.pawn.IsColonyMech) ? gun.gun.TryGetComp<CompChangeableProjectile>().allowedShellsSettings : null);
			return GenClosest.ClosestThingReachable(gun.Position, gun.Map, ThingRequest.ForGroup(ThingRequestGroup.Shell), PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 40f, new Predicate<Thing>(CS$<>8__locals1.<FindAmmoForTurret>g__ShellValidator|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x0600757D RID: 30077 RVA: 0x0023DFA0 File Offset: 0x0023C1A0
		public static Thing FindFuelForTurret(Pawn pawn, Building_TurretGun gun)
		{
			JobDriver_ManTurret.<>c__DisplayClass11_0 CS$<>8__locals1 = new JobDriver_ManTurret.<>c__DisplayClass11_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.refuelableComp = gun.TryGetComp<CompRefuelable>();
			if (CS$<>8__locals1.refuelableComp == null)
			{
				return null;
			}
			return GenClosest.ClosestThingReachable(gun.Position, gun.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 40f, new Predicate<Thing>(CS$<>8__locals1.<FindFuelForTurret>g__FuelValidator|0), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x0600757E RID: 30078 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600757F RID: 30079 RVA: 0x0023E012 File Offset: 0x0023C212
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil gotoTurret = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			Toil refuelIfNeeded = ToilMaker.MakeToil("MakeNewToils");
			refuelIfNeeded.initAction = delegate()
			{
				Pawn actor = refuelIfNeeded.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				Building_TurretGun building_TurretGun = building as Building_TurretGun;
				if (!JobDriver_ManTurret.GunNeedsRefueling(building))
				{
					this.JumpToToil(gotoTurret);
					return;
				}
				Thing thing = JobDriver_ManTurret.FindFuelForTurret(this.pawn, building_TurretGun);
				if (thing == null)
				{
					CompRefuelable compRefuelable = building.TryGetComp<CompRefuelable>();
					if (actor.Faction == Faction.OfPlayer && compRefuelable != null)
					{
						Messages.Message("MessageOutOfNearbyFuelFor".Translate(actor.LabelShort, building_TurretGun.Label, actor.Named("PAWN"), building_TurretGun.Named("GUN"), compRefuelable.Props.fuelFilter.Summary.Named("FUEL")).CapitalizeFirst(), building_TurretGun, MessageTypeDefOf.NegativeEvent, true);
					}
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
				actor.CurJob.targetB = thing;
				actor.CurJob.count = 1;
			};
			yield return refuelIfNeeded;
			yield return Toils_Reserve.Reserve(TargetIndex.B, 10, 1, null, false);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell, false).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false, true, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_General.Wait(240, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_Refuel.FinalizeRefueling(TargetIndex.A, TargetIndex.B);
			Toil loadIfNeeded = ToilMaker.MakeToil("MakeNewToils");
			loadIfNeeded.initAction = delegate()
			{
				Pawn actor = loadIfNeeded.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				Building_TurretGun building_TurretGun = building as Building_TurretGun;
				if (!JobDriver_ManTurret.GunNeedsLoading(building))
				{
					this.JumpToToil(gotoTurret);
					return;
				}
				Thing thing = JobDriver_ManTurret.FindAmmoForTurret(this.pawn, building_TurretGun);
				if (thing == null)
				{
					if (actor.Faction == Faction.OfPlayer)
					{
						Messages.Message("MessageOutOfNearbyShellsFor".Translate(actor.LabelShort, building_TurretGun.Label, actor.Named("PAWN"), building_TurretGun.Named("GUN")).CapitalizeFirst(), building_TurretGun, MessageTypeDefOf.NegativeEvent, true);
					}
					actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
				actor.CurJob.targetB = thing;
				actor.CurJob.count = 1;
			};
			yield return loadIfNeeded;
			yield return Toils_Reserve.Reserve(TargetIndex.B, 10, 1, null, false);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell, false).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false, true, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			Toil loadShell = ToilMaker.MakeToil("MakeNewToils");
			loadShell.initAction = delegate()
			{
				Pawn actor = loadShell.actor;
				SoundDefOf.Artillery_ShellLoaded.PlayOneShot(new TargetInfo(this.Turret.Position, this.Turret.Map, false));
				this.Turret.gun.TryGetComp<CompChangeableProjectile>().LoadShell(this.Hauling.def, 1);
				actor.carryTracker.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			};
			yield return loadShell;
			yield return gotoTurret;
			Toil man = ToilMaker.MakeToil("MakeNewToils");
			man.tickAction = delegate()
			{
				Pawn actor = man.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				if (JobDriver_ManTurret.GunNeedsLoading(building))
				{
					this.JumpToToil(loadIfNeeded);
					return;
				}
				if (JobDriver_ManTurret.GunNeedsRefueling(building))
				{
					this.JumpToToil(refuelIfNeeded);
					return;
				}
				building.GetComp<CompMannable>().ManForATick(actor);
				man.actor.rotationTracker.FaceCell(building.Position);
			};
			man.handlingFacing = true;
			man.defaultCompleteMode = ToilCompleteMode.Never;
			man.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return man;
			yield break;
		}

		// Token: 0x0400506B RID: 20587
		private const float SearchRadius = 40f;

		// Token: 0x0400506C RID: 20588
		private const int MaxPawnReservations = 10;

		// Token: 0x0400506D RID: 20589
		private const TargetIndex TurretInd = TargetIndex.A;

		// Token: 0x0400506E RID: 20590
		private const TargetIndex HaulingInd = TargetIndex.B;
	}
}
