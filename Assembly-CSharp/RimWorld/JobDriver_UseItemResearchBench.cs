using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001347 RID: 4935
	public class JobDriver_UseItemResearchBench : JobDriver_UseItem
	{
		// Token: 0x06007938 RID: 31032 RVA: 0x0024A28B File Offset: 0x0024848B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return base.TryMakePreToilReservations(errorOnFailed) && this.pawn.Reserve(this.job.targetB, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007939 RID: 31033 RVA: 0x0024A2B9 File Offset: 0x002484B9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnIncapable(PawnCapacityDefOf.Manipulation);
			this.FailOn(() => !base.TargetThingA.TryGetComp<CompUsable>().CanBeUsedBy(this.pawn, false, false));
			this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			this.FailOnBurningImmobile(TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell, false);
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, null, false, false);
			yield return base.PrepareToUse();
			yield return base.Use();
			yield break;
		}

		// Token: 0x04005242 RID: 21058
		private const TargetIndex Item = TargetIndex.A;

		// Token: 0x04005243 RID: 21059
		private const TargetIndex ResearchBenchInd = TargetIndex.B;
	}
}
