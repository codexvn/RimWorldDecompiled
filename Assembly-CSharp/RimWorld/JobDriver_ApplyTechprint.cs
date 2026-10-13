using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001222 RID: 4642
	public class JobDriver_ApplyTechprint : JobDriver
	{
		// Token: 0x1700119A RID: 4506
		// (get) Token: 0x06007160 RID: 29024 RVA: 0x00230E1C File Offset: 0x0022F01C
		protected Building_ResearchBench ResearchBench
		{
			get
			{
				return (Building_ResearchBench)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700119B RID: 4507
		// (get) Token: 0x06007161 RID: 29025 RVA: 0x00230E44 File Offset: 0x0022F044
		protected Thing Techprint
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x1700119C RID: 4508
		// (get) Token: 0x06007162 RID: 29026 RVA: 0x00230E65 File Offset: 0x0022F065
		protected CompTechprint TechprintComp
		{
			get
			{
				return this.Techprint.TryGetComp<CompTechprint>();
			}
		}

		// Token: 0x06007163 RID: 29027 RVA: 0x00230E74 File Offset: 0x0022F074
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ResearchBench, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Techprint, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007164 RID: 29028 RVA: 0x00230EC7 File Offset: 0x0022F0C7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_General.DoAtomic(delegate
			{
				this.job.count = 1;
			});
			Toil toil = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null, false);
			yield return toil;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false);
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, null, false, false);
			yield return Toils_General.Wait(600, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.initAction = delegate()
			{
				Find.ResearchManager.ApplyTechprint(this.TechprintComp.Props.project, this.pawn);
				this.Techprint.Destroy(DestroyMode.Vanish);
				SoundDefOf.TechprintApplied.PlayOneShotOnCamera(null);
			};
			toil2.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil2;
			yield break;
		}

		// Token: 0x04004E90 RID: 20112
		private const TargetIndex ResearchBenchInd = TargetIndex.A;

		// Token: 0x04004E91 RID: 20113
		private const TargetIndex TechprintInd = TargetIndex.B;

		// Token: 0x04004E92 RID: 20114
		private const TargetIndex HaulCell = TargetIndex.C;

		// Token: 0x04004E93 RID: 20115
		private const int Duration = 600;
	}
}
