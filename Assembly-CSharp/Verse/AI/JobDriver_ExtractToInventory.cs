using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E54 RID: 3668
	public class JobDriver_ExtractToInventory : JobDriver
	{
		// Token: 0x06005C69 RID: 23657 RVA: 0x001D81A7 File Offset: 0x001D63A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06005C6A RID: 23658 RVA: 0x001D81CB File Offset: 0x001D63CB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnDestroyedOrNull(TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, this.job.GetTarget(TargetIndex.A).Thing.def.hasInteractionCell ? PathEndMode.InteractionCell : PathEndMode.Touch, false).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_General.WaitWhileExtractingContents(TargetIndex.A, TargetIndex.B, 300);
			yield return Toils_General.Do(delegate
			{
				this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompThingContainer>().innerContainer.TryDropAll(this.pawn.Position, this.pawn.Map, ThingPlaceMode.Near, null, null, true);
			});
			yield return Toils_Haul.TakeToInventory(TargetIndex.B, this.job.count);
			yield break;
		}

		// Token: 0x040042DD RID: 17117
		private const TargetIndex ContainerInd = TargetIndex.A;

		// Token: 0x040042DE RID: 17118
		private const TargetIndex ContentsInd = TargetIndex.B;

		// Token: 0x040042DF RID: 17119
		public const int OpenTicks = 300;
	}
}
