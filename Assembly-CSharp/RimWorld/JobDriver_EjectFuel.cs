using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001255 RID: 4693
	public class JobDriver_EjectFuel : JobDriver
	{
		// Token: 0x170011E4 RID: 4580
		// (get) Token: 0x060072CD RID: 29389 RVA: 0x001E5D2F File Offset: 0x001E3F2F
		private Thing Target
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x170011E5 RID: 4581
		// (get) Token: 0x060072CE RID: 29390 RVA: 0x00235B6C File Offset: 0x00233D6C
		private CompRefuelable Refuelable
		{
			get
			{
				CompRefuelable result;
				if ((result = this.compRefuelable) == null)
				{
					result = (this.compRefuelable = this.job.targetA.Thing.TryGetComp<CompRefuelable>());
				}
				return result;
			}
		}

		// Token: 0x060072CF RID: 29391 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060072D0 RID: 29392 RVA: 0x00235BA1 File Offset: 0x00233DA1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				if (!this.Refuelable.CanEjectFuel())
				{
					Designation designation = base.Map.designationManager.DesignationOn(this.job.targetA.Thing, DesignationDefOf.EjectFuel);
					if (designation == null)
					{
						return;
					}
					designation.Delete();
				}
			};
			yield return toil.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false).FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.EjectFuel).FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil2 = Toils_General.Wait(120, TargetIndex.A).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			BuildingProperties building = this.Target.def.building;
			if (building != null && building.openingStartedSound != null)
			{
				toil2.PlaySoundAtStart(this.Target.def.building.openingStartedSound);
			}
			yield return toil2;
			yield return Toils_General.Do(delegate
			{
				Designation designation = base.Map.designationManager.DesignationOn(this.job.targetA.Thing, DesignationDefOf.EjectFuel);
				if (designation != null)
				{
					designation.Delete();
				}
				this.Refuelable.EjectFuel();
			});
			yield break;
		}

		// Token: 0x04004F43 RID: 20291
		public const int EjectTicks = 120;

		// Token: 0x04004F44 RID: 20292
		private CompRefuelable compRefuelable;
	}
}
