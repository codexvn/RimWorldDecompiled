using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001299 RID: 4761
	public class JobDriver_InstallImplant : JobDriver
	{
		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x06007494 RID: 29844 RVA: 0x0023B3F0 File Offset: 0x002395F0
		private Pawn TargetPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x06007495 RID: 29845 RVA: 0x0023B418 File Offset: 0x00239618
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06007496 RID: 29846 RVA: 0x0023B43C File Offset: 0x0023963C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.TargetPawn, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007497 RID: 29847 RVA: 0x0023B48F File Offset: 0x0023968F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch, false).FailOnDespawnedOrNull(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false, true, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = Toils_General.WaitWith(TargetIndex.A, 600, false, true, false, TargetIndex.A, PathEndMode.Touch);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.FailOnDespawnedOrNull(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return toil;
			yield return Toils_General.Do(new Action(this.Install));
			yield break;
		}

		// Token: 0x06007498 RID: 29848 RVA: 0x0023B4A0 File Offset: 0x002396A0
		private void Install()
		{
			CompTargetEffect_InstallImplantInOtherPawn compTargetEffect_InstallImplantInOtherPawn = this.Item.TryGetComp<CompTargetEffect_InstallImplantInOtherPawn>();
			if (compTargetEffect_InstallImplantInOtherPawn == null)
			{
				return;
			}
			BodyPartRecord bodyPartRecord = this.TargetPawn.RaceProps.body.GetPartsWithDef(compTargetEffect_InstallImplantInOtherPawn.Props.bodyPart).FirstOrFallback(null);
			if (bodyPartRecord == null)
			{
				return;
			}
			Hediff firstHediffOfDef = this.TargetPawn.health.hediffSet.GetFirstHediffOfDef(compTargetEffect_InstallImplantInOtherPawn.Props.hediffDef, false);
			if (firstHediffOfDef == null && !compTargetEffect_InstallImplantInOtherPawn.Props.requiresExistingHediff)
			{
				this.TargetPawn.health.AddHediff(compTargetEffect_InstallImplantInOtherPawn.Props.hediffDef, bodyPartRecord, null, null);
			}
			else if (compTargetEffect_InstallImplantInOtherPawn.Props.canUpgrade)
			{
				((Hediff_Level)firstHediffOfDef).ChangeLevel(1);
			}
			if (this.TargetPawn.Map == Find.CurrentMap && compTargetEffect_InstallImplantInOtherPawn.Props.soundOnUsed != null)
			{
				compTargetEffect_InstallImplantInOtherPawn.Props.soundOnUsed.PlayOneShot(SoundInfo.InMap(this.TargetPawn, MaintenanceType.None));
			}
			this.Item.SplitOff(1).Destroy(DestroyMode.Vanish);
		}

		// Token: 0x04005014 RID: 20500
		private const TargetIndex TargetPawnInd = TargetIndex.A;

		// Token: 0x04005015 RID: 20501
		private const TargetIndex ItemInd = TargetIndex.B;

		// Token: 0x04005016 RID: 20502
		private const int DurationTicks = 600;
	}
}
