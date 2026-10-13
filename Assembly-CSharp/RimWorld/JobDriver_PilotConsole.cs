using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012DE RID: 4830
	public class JobDriver_PilotConsole : JobDriver
	{
		// Token: 0x06007656 RID: 30294 RVA: 0x001D81A7 File Offset: 0x001D63A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007657 RID: 30295 RVA: 0x00240DBB File Offset: 0x0023EFBB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				if (thing == null)
				{
					return;
				}
				((Precept_Ritual)this.pawn.Ideo.GetPrecept(PreceptDefOf.GravshipLaunch)).ShowRitualBeginWindow(thing, null, this.pawn, null);
			};
			yield return toil;
			yield break;
		}

		// Token: 0x040050E2 RID: 20706
		private const TargetIndex ConsoleInd = TargetIndex.A;
	}
}
