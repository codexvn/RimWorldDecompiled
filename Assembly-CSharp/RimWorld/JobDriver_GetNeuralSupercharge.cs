using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001390 RID: 5008
	public class JobDriver_GetNeuralSupercharge : JobDriver
	{
		// Token: 0x06007AB0 RID: 31408 RVA: 0x0024F558 File Offset: 0x0024D758
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A).Thing, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007AB1 RID: 31409 RVA: 0x0024F594 File Offset: 0x0024D794
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			yield return Toils_General.Wait(0.55f.SecondsToTicks(), TargetIndex.None);
			Toil toil = Toils_General.Wait(1.5f.SecondsToTicks(), TargetIndex.None);
			toil.WithEffect(() => EffecterDefOf.NeuralSuperchargerUse, this.job.GetTarget(TargetIndex.A).Thing, null);
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				Thing thing = this.job.GetTarget(TargetIndex.A).Thing;
				CompRechargeable compRechargeable = (thing != null) ? thing.TryGetComp<CompRechargeable>() : null;
				if (compRechargeable != null)
				{
					compRechargeable.Discharge();
				}
				this.pawn.health.AddHediff(HediffDefOf.NeuralSupercharge, this.pawn.health.hediffSet.GetBrain(), null, null);
			});
			yield return Toils_General.Wait(0.35f.SecondsToTicks(), TargetIndex.None);
			yield break;
		}

		// Token: 0x04005305 RID: 21253
		private const TargetIndex ChargerInd = TargetIndex.A;
	}
}
