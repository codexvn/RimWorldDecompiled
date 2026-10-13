using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200157F RID: 5503
	public class JobDriver_ReturnToGauranlenTree : JobDriver
	{
		// Token: 0x170013FA RID: 5114
		// (get) Token: 0x06008279 RID: 33401 RVA: 0x00272D86 File Offset: 0x00270F86
		private CompTreeConnection TreeComp
		{
			get
			{
				return this.job.targetA.Thing.TryGetComp<CompTreeConnection>();
			}
		}

		// Token: 0x0600827A RID: 33402 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600827B RID: 33403 RVA: 0x00273338 File Offset: 0x00271538
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => !this.TreeComp.ShouldReturnToTree(this.pawn));
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_General.WaitWith(TargetIndex.A, 180, true, false, false, TargetIndex.None, PathEndMode.Touch).WithEffect(EffecterDefOf.GauranlenLeaves, TargetIndex.A, null).PlaySustainerOrSound(SoundDefOf.Interact_Sow, 1f);
			yield return Toils_General.Do(delegate
			{
				this.TreeComp.RemoveDryad(this.pawn);
				this.pawn.DeSpawn(DestroyMode.Vanish);
				Find.WorldPawns.PassToWorld(this.pawn, PawnDiscardDecideMode.Discard);
			});
			yield break;
		}

		// Token: 0x04005697 RID: 22167
		private const int WaitTicks = 180;
	}
}
