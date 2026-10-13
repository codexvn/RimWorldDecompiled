using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011D7 RID: 4567
	public class JobDriver_DevourerDigest : JobDriver
	{
		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x06006F99 RID: 28569 RVA: 0x0022B994 File Offset: 0x00229B94
		private CompDevourer Comp
		{
			get
			{
				CompDevourer result;
				if ((result = this.comp) == null)
				{
					result = (this.comp = this.pawn.GetComp<CompDevourer>());
				}
				return result;
			}
		}

		// Token: 0x06006F9A RID: 28570 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06006F9B RID: 28571 RVA: 0x00002C42 File Offset: 0x00000E42
		public override string GetReport()
		{
			return null;
		}

		// Token: 0x06006F9C RID: 28572 RVA: 0x0022B9BF File Offset: 0x00229BBF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			int digestionTicks = this.Comp.GetDigestionTicks();
			Toil toil = Toils_General.Wait(digestionTicks, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.None, digestionTicks, false, -0.5f);
			toil.FailOn(() => !this.Comp.Digesting);
			toil.PlaySustainerOrSound(SoundDefOf.Pawn_Devourer_Digesting, 1f);
			toil.AddFinishAction(new Action(this.Comp.DigestJobFinished));
			yield return toil;
			yield break;
		}

		// Token: 0x04004DCC RID: 19916
		private CompDevourer comp;
	}
}
