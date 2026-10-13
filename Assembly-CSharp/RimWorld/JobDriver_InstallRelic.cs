using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200129B RID: 4763
	public class JobDriver_InstallRelic : JobDriver
	{
		// Token: 0x060074A2 RID: 29858 RVA: 0x0023B73C File Offset: 0x0023993C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060074A3 RID: 29859 RVA: 0x0023B791 File Offset: 0x00239991
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Relic"))
			{
				yield break;
			}
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell, false).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOn((Toil to) => this.<MakeNewToils>g__ReliquaryFull|5_0());
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false, true, false).FailOn((Toil to) => this.<MakeNewToils>g__ReliquaryFull|5_0());
			yield return Toils_Haul.CarryHauledThingToCell(TargetIndex.C, PathEndMode.ClosestTouch).FailOn((Toil to) => this.<MakeNewToils>g__ReliquaryFull|5_0());
			Toil toil = Toils_General.Wait(300, TargetIndex.B).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.B).FailOn((Toil to) => this.<MakeNewToils>g__ReliquaryFull|5_0());
			toil.handlingFacing = true;
			yield return toil;
			yield return Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.A, delegate
			{
				this.job.GetTarget(TargetIndex.A).Thing.def.soundDrop.PlayOneShot(new TargetInfo(this.job.GetTarget(TargetIndex.B).Cell, this.pawn.Map, false));
				SoundDefOf.Relic_Installed.PlayOneShot(new TargetInfo(this.job.GetTarget(TargetIndex.B).Cell, this.pawn.Map, false));
			});
			yield break;
		}

		// Token: 0x060074A5 RID: 29861 RVA: 0x0023B7A4 File Offset: 0x002399A4
		[CompilerGenerated]
		private bool <MakeNewToils>g__ReliquaryFull|5_0()
		{
			CompRelicContainer compRelicContainer = this.pawn.jobs.curJob.GetTarget(TargetIndex.B).Thing.TryGetComp<CompRelicContainer>();
			return compRelicContainer == null || compRelicContainer.Full;
		}

		// Token: 0x0400501B RID: 20507
		private const TargetIndex RelicInd = TargetIndex.A;

		// Token: 0x0400501C RID: 20508
		private const TargetIndex ContainerInd = TargetIndex.B;

		// Token: 0x0400501D RID: 20509
		private const TargetIndex ContainerInteractionCellInd = TargetIndex.C;

		// Token: 0x0400501E RID: 20510
		private const int InstallTicks = 300;
	}
}
