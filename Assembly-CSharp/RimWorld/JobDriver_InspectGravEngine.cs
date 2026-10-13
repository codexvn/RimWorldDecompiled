using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001297 RID: 4759
	public class JobDriver_InspectGravEngine : JobDriver
	{
		// Token: 0x06007488 RID: 29832 RVA: 0x0022F6AD File Offset: 0x0022D8AD
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(base.TargetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007489 RID: 29833 RVA: 0x0023B287 File Offset: 0x00239487
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_General.WaitWith(TargetIndex.A, 240, true, false, false, TargetIndex.A, PathEndMode.Touch);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				Building_GravEngine building_GravEngine = base.TargetA.Thing as Building_GravEngine;
				if (building_GravEngine != null)
				{
					building_GravEngine.Inspect(false);
				}
			};
			yield return toil;
			yield break;
		}

		// Token: 0x0400500F RID: 20495
		private const TargetIndex GravEngineIndex = TargetIndex.A;
	}
}
