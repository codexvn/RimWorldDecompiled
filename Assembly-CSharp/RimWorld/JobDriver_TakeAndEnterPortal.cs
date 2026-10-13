using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001324 RID: 4900
	public class JobDriver_TakeAndEnterPortal : JobDriver_EnterPortal
	{
		// Token: 0x170012F3 RID: 4851
		// (get) Token: 0x0600784A RID: 30794 RVA: 0x002472C4 File Offset: 0x002454C4
		private Thing ThingToTake
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x170012F4 RID: 4852
		// (get) Token: 0x0600784B RID: 30795 RVA: 0x002472E5 File Offset: 0x002454E5
		private Pawn PawnToTake
		{
			get
			{
				return this.ThingToTake as Pawn;
			}
		}

		// Token: 0x0600784C RID: 30796 RVA: 0x002472F2 File Offset: 0x002454F2
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ThingToTake, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600784D RID: 30797 RVA: 0x00247315 File Offset: 0x00245515
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOn(() => this.PawnToTake != null && !this.PawnToTake.Downed && this.PawnToTake.Awake());
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, false).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Construct.UninstallIfMinifiable(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false, true, false);
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040051D5 RID: 20949
		private const TargetIndex ThingInd = TargetIndex.B;
	}
}
