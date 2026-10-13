using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012EE RID: 4846
	public class JobDriver_ReleaseMechs : JobDriver
	{
		// Token: 0x060076E0 RID: 30432 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060076E1 RID: 30433 RVA: 0x00242DB7 File Offset: 0x00240FB7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.pawn.TryGetComp<CompMechCarrier>().TrySpawnPawns();
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			yield break;
		}
	}
}
