using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001229 RID: 4649
	public class JobDriver_CarryGenepackToContainer : JobDriver
	{
		// Token: 0x170011A7 RID: 4519
		// (get) Token: 0x06007194 RID: 29076 RVA: 0x00231A60 File Offset: 0x0022FC60
		private Thing Container
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x170011A8 RID: 4520
		// (get) Token: 0x06007195 RID: 29077 RVA: 0x00231A81 File Offset: 0x0022FC81
		private CompGenepackContainer ContainerComp
		{
			get
			{
				return this.Container.TryGetComp<CompGenepackContainer>();
			}
		}

		// Token: 0x170011A9 RID: 4521
		// (get) Token: 0x06007196 RID: 29078 RVA: 0x00231A90 File Offset: 0x0022FC90
		private Genepack Genepack
		{
			get
			{
				return (Genepack)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06007197 RID: 29079 RVA: 0x001D81A7 File Offset: 0x001D63A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007198 RID: 29080 RVA: 0x00231AB6 File Offset: 0x0022FCB6
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckBiotech("Genepack"))
			{
				yield break;
			}
			this.FailOn(delegate()
			{
				CompGenepackContainer containerComp = this.ContainerComp;
				return containerComp == null || containerComp.Full || (!containerComp.autoLoad && (!containerComp.leftToLoad.Contains(this.Genepack) || this.Genepack.targetContainer != this.Container));
			});
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell, false).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false, true, false);
			yield return Toils_Goto.Goto(TargetIndex.B, PathEndMode.Touch);
			Toil toil = Toils_General.Wait(30, TargetIndex.B).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.B);
			toil.handlingFacing = true;
			yield return toil;
			yield return Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.A, delegate
			{
				this.Genepack.def.soundDrop.PlayOneShot(SoundInfo.InMap(this.Container, MaintenanceType.None));
				this.Genepack.targetContainer = null;
				CompGenepackContainer containerComp = this.ContainerComp;
				containerComp.leftToLoad.Remove(this.Genepack);
				MoteMaker.ThrowText(this.Container.DrawPos, this.pawn.Map, "InsertedThing".Translate(string.Format("{0} / {1}", containerComp.innerContainer.Count, containerComp.Props.maxCapacity)), -1f);
			});
			yield break;
		}

		// Token: 0x04004EAB RID: 20139
		private const TargetIndex GenepackInd = TargetIndex.A;

		// Token: 0x04004EAC RID: 20140
		private const TargetIndex ContainerInd = TargetIndex.B;

		// Token: 0x04004EAD RID: 20141
		private const int InsertTicks = 30;
	}
}
