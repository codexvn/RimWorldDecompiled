using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001388 RID: 5000
	public class JobDriver_Feed : JobDriver
	{
		// Token: 0x1700134C RID: 4940
		// (get) Token: 0x06007A7A RID: 31354 RVA: 0x001E5D2F File Offset: 0x001E3F2F
		protected Thing Food
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x1700134D RID: 4941
		// (get) Token: 0x06007A7B RID: 31355 RVA: 0x0024E845 File Offset: 0x0024CA45
		protected Pawn Deliveree
		{
			get
			{
				return (Pawn)this.job.targetB.Thing;
			}
		}

		// Token: 0x06007A7C RID: 31356 RVA: 0x0024E85C File Offset: 0x0024CA5C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007A7D RID: 31357 RVA: 0x0024E884 File Offset: 0x0024CA84
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			if (this.pawn.inventory != null && this.pawn.inventory.Contains(base.TargetThingA))
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
			}
			else if (base.TargetThingA is Building_NutrientPasteDispenser)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.PickupIngestible(TargetIndex.A, this.Deliveree);
			}
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch, false);
			yield return Toils_Ingest.ChewIngestible(this.Deliveree, 1.5f, TargetIndex.A, TargetIndex.None).FailOnCannotTouch(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_Ingest.FinalizeIngest(this.Deliveree, TargetIndex.A);
			yield break;
		}

		// Token: 0x040052E5 RID: 21221
		private const TargetIndex IngestableInd = TargetIndex.A;

		// Token: 0x040052E6 RID: 21222
		private const TargetIndex DelivereeInd = TargetIndex.B;

		// Token: 0x040052E7 RID: 21223
		private const float FeedDurationMultiplier = 1.5f;
	}
}
