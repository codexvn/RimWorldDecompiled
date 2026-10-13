using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012C8 RID: 4808
	public class JobDriver_Open : JobDriver
	{
		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x060075C6 RID: 30150 RVA: 0x0023F2FF File Offset: 0x0023D4FF
		private IOpenable Openable
		{
			get
			{
				return (IOpenable)this.job.targetA.Thing;
			}
		}

		// Token: 0x17001279 RID: 4729
		// (get) Token: 0x060075C7 RID: 30151 RVA: 0x001E5D2F File Offset: 0x001E3F2F
		private Thing Target
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x060075C8 RID: 30152 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060075C9 RID: 30153 RVA: 0x0023F316 File Offset: 0x0023D516
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				if (!this.Openable.CanOpen)
				{
					Designation designation = base.Map.designationManager.DesignationOn(this.job.targetA.Thing, DesignationDefOf.Open);
					if (designation != null)
					{
						designation.Delete();
					}
				}
			};
			yield return toil.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false).FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.Open).FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil2 = Toils_General.Wait(this.Openable.OpenTicks, TargetIndex.A).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			if (this.Target.def.building != null && this.Target.def.building.openingStartedSound != null)
			{
				toil2.PlaySoundAtStart(this.Target.def.building.openingStartedSound);
			}
			yield return toil2;
			yield return Toils_General.Open(TargetIndex.A);
			yield break;
		}
	}
}
