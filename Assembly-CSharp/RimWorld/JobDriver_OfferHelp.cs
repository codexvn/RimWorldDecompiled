using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012C6 RID: 4806
	public class JobDriver_OfferHelp : JobDriver
	{
		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x060075B8 RID: 30136 RVA: 0x0023F190 File Offset: 0x0023D390
		public Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.pawn.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060075B9 RID: 30137 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060075BA RID: 30138 RVA: 0x0023F1BB File Offset: 0x0023D3BB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => !this.OtherPawn.mindState.WillJoinColonyIfRescued);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_General.DoAtomic(delegate
			{
				this.OtherPawn.mindState.JoinColonyBecauseRescuedBy(this.pawn);
			});
			yield break;
		}

		// Token: 0x04005093 RID: 20627
		private const TargetIndex OtherPawnInd = TargetIndex.A;
	}
}
