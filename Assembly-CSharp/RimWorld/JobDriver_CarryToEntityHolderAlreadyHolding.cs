using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001235 RID: 4661
	public class JobDriver_CarryToEntityHolderAlreadyHolding : JobDriver
	{
		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x060071FD RID: 29181 RVA: 0x00233404 File Offset: 0x00231604
		private Thing Takee
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x060071FE RID: 29182 RVA: 0x00233428 File Offset: 0x00231628
		private CompEntityHolder DestHolder
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompEntityHolder>();
			}
		}

		// Token: 0x060071FF RID: 29183 RVA: 0x0023344E File Offset: 0x0023164E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.DestHolder.parent, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007200 RID: 29184 RVA: 0x00233476 File Offset: 0x00231676
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => !this.DestHolder.Available);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false, true, false);
			foreach (Toil toil in JobDriver_CarryToEntityHolder.ChainTakeeToPlatformToils(this.pawn, this.Takee, this.DestHolder, TargetIndex.A))
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04004EE5 RID: 20197
		private const TargetIndex DestIndex = TargetIndex.A;

		// Token: 0x04004EE6 RID: 20198
		private const TargetIndex TakeeIndex = TargetIndex.B;
	}
}
