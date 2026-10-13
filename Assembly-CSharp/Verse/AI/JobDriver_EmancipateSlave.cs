using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E50 RID: 3664
	public class JobDriver_EmancipateSlave : JobDriver
	{
		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06005C4D RID: 23629 RVA: 0x001D7DF4 File Offset: 0x001D5FF4
		private Pawn Slave
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06005C4E RID: 23630 RVA: 0x001D7E1A File Offset: 0x001D601A
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Slave, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005C4F RID: 23631 RVA: 0x001D7E3D File Offset: 0x001D603D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Emancipate slave"))
			{
				yield break;
			}
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOn(() => this.Slave.guest.slaveInteractionMode != SlaveInteractionModeDefOf.Emancipate);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOnForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOn(() => !this.Slave.IsSlaveOfColony || !this.Slave.guest.SlaveIsSecure).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				GenGuest.EmancipateSlave(this.pawn, this.Slave);
			};
			yield return toil;
			yield break;
		}

		// Token: 0x040042D2 RID: 17106
		private const TargetIndex SlaveInd = TargetIndex.A;

		// Token: 0x040042D3 RID: 17107
		private const TargetIndex BedInd = TargetIndex.B;
	}
}
