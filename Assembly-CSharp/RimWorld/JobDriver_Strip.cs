using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001319 RID: 4889
	public class JobDriver_Strip : JobDriver
	{
		// Token: 0x060077FD RID: 30717 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060077FE RID: 30718 RVA: 0x0024633B File Offset: 0x0024453B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn(() => !StrippableUtility.CanBeStrippedByColony(base.TargetThingA));
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.pawn.pather.StartPath(base.TargetThingA, PathEndMode.ClosestTouch);
			};
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return toil;
			yield return Toils_General.Wait(60, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.initAction = delegate()
			{
				Thing thing = this.job.targetA.Thing;
				Designation designation = base.Map.designationManager.DesignationOn(thing, DesignationDefOf.Strip);
				if (designation != null)
				{
					designation.Delete();
				}
				IStrippable strippable = thing as IStrippable;
				if (strippable != null)
				{
					strippable.Strip(true);
				}
				this.pawn.records.Increment(RecordDefOf.BodiesStripped);
			};
			toil2.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil2;
			yield break;
		}

		// Token: 0x060077FF RID: 30719 RVA: 0x0024634C File Offset: 0x0024454C
		public override object[] TaleParameters()
		{
			Corpse corpse = base.TargetA.Thing as Corpse;
			return new object[]
			{
				this.pawn,
				(corpse != null) ? corpse.InnerPawn : base.TargetA.Thing
			};
		}

		// Token: 0x040051A6 RID: 20902
		private const int StripTicks = 60;
	}
}
