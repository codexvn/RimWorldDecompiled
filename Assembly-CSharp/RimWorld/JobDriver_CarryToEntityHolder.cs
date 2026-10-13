using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200122F RID: 4655
	public class JobDriver_CarryToEntityHolder : JobDriver
	{
		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x060071CF RID: 29135 RVA: 0x002329D4 File Offset: 0x00230BD4
		private Thing Takee
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x060071D0 RID: 29136 RVA: 0x002329F8 File Offset: 0x00230BF8
		private CompEntityHolder DestHolder
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompEntityHolder>();
			}
		}

		// Token: 0x060071D1 RID: 29137 RVA: 0x00232A20 File Offset: 0x00230C20
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.DestHolder.parent, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060071D2 RID: 29138 RVA: 0x00232A78 File Offset: 0x00230C78
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.B);
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => !this.DestHolder.Available);
			this.FailOn(delegate()
			{
				Pawn pawn = this.Takee as Pawn;
				if (pawn != null)
				{
					CompActivity comp = pawn.GetComp<CompActivity>();
					return comp != null && !comp.IsDormant;
				}
				return false;
			});
			this.FailOn(() => this.Takee.TryGetComp<CompHoldingPlatformTarget>().EntityHolder != this.DestHolder);
			if (this.pawn.carryTracker.CarriedThing != this.Takee)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell, false);
			}
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false, true, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false);
			foreach (Toil toil in JobDriver_CarryToEntityHolder.ChainTakeeToPlatformToils(this.pawn, this.Takee, this.DestHolder, TargetIndex.A))
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return Toils_General.Do(delegate
			{
				Pawn pawn = this.Takee as Pawn;
				if (pawn != null && (!pawn.RaceProps.Humanlike || pawn.IsMutant))
				{
					TaleRecorder.RecordTale(TaleDefOf.Captured, new object[]
					{
						this.pawn,
						pawn
					});
				}
			});
			yield break;
			yield break;
		}

		// Token: 0x060071D3 RID: 29139 RVA: 0x00232A88 File Offset: 0x00230C88
		public static IEnumerable<Toil> ChainTakeeToPlatformToils(Pawn taker, Thing takee, CompEntityHolder platform, TargetIndex platformIndex)
		{
			yield return Toils_Goto.GotoThing(platformIndex, PathEndMode.ClosestTouch, false);
			Toil toil = Toils_General.WaitWith(platformIndex, 300, true, false, false, TargetIndex.None, PathEndMode.Touch);
			toil.PlaySustainerOrSound(SoundDefOf.ChainToPlatform, 1f);
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				Thing takee2 = takee;
				platform.Container.TryAddOrTransfer(takee2, 1, true);
				CompHoldingPlatformTarget compHoldingPlatformTarget = takee2.TryGetComp<CompHoldingPlatformTarget>();
				if (compHoldingPlatformTarget != null)
				{
					compHoldingPlatformTarget.Notify_HeldOnPlatform(platform.Container);
					if (compHoldingPlatformTarget.Props.capturedLetterLabel != null)
					{
						Find.LetterStack.ReceiveLetter(compHoldingPlatformTarget.Props.capturedLetterLabel, compHoldingPlatformTarget.Props.capturedLetterText.Formatted(taker.Named("PAWN")), LetterDefOf.NeutralEvent, platform.parent, null, null, null, null, 0, true);
					}
				}
			});
			yield break;
		}

		// Token: 0x04004EC6 RID: 20166
		private const TargetIndex DestHolderIndex = TargetIndex.A;

		// Token: 0x04004EC7 RID: 20167
		private const TargetIndex TakeeIndex = TargetIndex.B;

		// Token: 0x04004EC8 RID: 20168
		private const int EnterDelayTicks = 300;
	}
}
