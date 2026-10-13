using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001233 RID: 4659
	public class JobDriver_TransferBetweenEntityHolders : JobDriver
	{
		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x060071EC RID: 29164 RVA: 0x00233030 File Offset: 0x00231230
		private Thing Takee
		{
			get
			{
				return this.job.GetTarget(TargetIndex.C).Thing;
			}
		}

		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x060071ED RID: 29165 RVA: 0x00233054 File Offset: 0x00231254
		private CompEntityHolder SourceHolder
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompEntityHolder>();
			}
		}

		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x060071EE RID: 29166 RVA: 0x0023307C File Offset: 0x0023127C
		private CompEntityHolder DestHolder
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing.TryGetComp<CompEntityHolder>();
			}
		}

		// Token: 0x060071EF RID: 29167 RVA: 0x002330A4 File Offset: 0x002312A4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.SourceHolder.parent, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.DestHolder.parent, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060071F0 RID: 29168 RVA: 0x00233124 File Offset: 0x00231324
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.C);
			this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			this.FailOn(() => !this.DestHolder.Available);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_General.Do(delegate
			{
				CompHoldingPlatformTarget compHoldingPlatformTarget = this.Takee.TryGetComp<CompHoldingPlatformTarget>();
				if (compHoldingPlatformTarget != null)
				{
					compHoldingPlatformTarget.Notify_ReleasedFromPlatform();
				}
				this.SourceHolder.EjectContents();
				this.pawn.Map.reservationManager.Release(this.job.GetTarget(TargetIndex.A), this.pawn, this.job);
			}).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.C, false, false, false, true, false);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, false);
			foreach (Toil toil in JobDriver_CarryToEntityHolder.ChainTakeeToPlatformToils(this.pawn, this.Takee, this.DestHolder, TargetIndex.B))
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04004EDD RID: 20189
		private const TargetIndex SourceHolderIndex = TargetIndex.A;

		// Token: 0x04004EDE RID: 20190
		private const TargetIndex DestHolderIndex = TargetIndex.B;

		// Token: 0x04004EDF RID: 20191
		private const TargetIndex TakeeIndex = TargetIndex.C;
	}
}
