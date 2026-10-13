using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011F8 RID: 4600
	public class JobDriver_WatchBuilding : JobDriver
	{
		// Token: 0x06007065 RID: 28773 RVA: 0x0022E090 File Offset: 0x0022C290
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, this.job.def.joyMaxParticipants, 0, null, errorOnFailed, false) && this.pawn.ReserveSittableOrSpot(this.job.targetB.Cell, this.job, errorOnFailed) && (!base.TargetC.HasThing || !(base.TargetC.Thing is Building_Bed) || this.pawn.Reserve(this.job.targetC, this.job, ((Building_Bed)base.TargetC.Thing).SleepingSlotsCount, 0, null, errorOnFailed, false));
		}

		// Token: 0x06007066 RID: 28774 RVA: 0x0022E15C File Offset: 0x0022C35C
		public override bool CanBeginNowWhileLyingDown()
		{
			return base.TargetC.HasThing && base.TargetC.Thing is Building_Bed && JobInBedUtility.InBedOrRestSpotNow(this.pawn, base.TargetC);
		}

		// Token: 0x06007067 RID: 28775 RVA: 0x0022E1A1 File Offset: 0x0022C3A1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_WatchBuilding.<>c__DisplayClass2_0 CS$<>8__locals1 = new JobDriver_WatchBuilding.<>c__DisplayClass2_0();
			CS$<>8__locals1.<>4__this = this;
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			if (base.TargetC.HasThing && base.TargetC.Thing is Building_Bed)
			{
				this.KeepLyingDown(TargetIndex.C);
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.C, TargetIndex.None);
				yield return Toils_Bed.GotoBed(TargetIndex.C);
				CS$<>8__locals1.watch = Toils_LayDown.LayDown(TargetIndex.C, true, false, true, true, PawnPosture.LayingOnGroundNormal, false);
				CS$<>8__locals1.watch.AddFailCondition(() => !CS$<>8__locals1.watch.actor.Awake());
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				CS$<>8__locals1.watch = ToilMaker.MakeToil("MakeNewToils");
			}
			CS$<>8__locals1.watch.AddPreTickIntervalAction(new Action<int>(this.WatchTickAction));
			CS$<>8__locals1.watch.AddFinishAction(delegate
			{
				JoyUtility.TryGainRecRoomThought(CS$<>8__locals1.<>4__this.pawn);
			});
			CS$<>8__locals1.watch.defaultCompleteMode = ToilCompleteMode.Delay;
			CS$<>8__locals1.watch.defaultDuration = this.job.def.joyDuration;
			CS$<>8__locals1.watch.handlingFacing = true;
			if (base.TargetA.Thing.def.building != null && base.TargetA.Thing.def.building.effectWatching != null)
			{
				CS$<>8__locals1.watch.WithEffect(() => CS$<>8__locals1.<>4__this.TargetA.Thing.def.building.effectWatching, new Func<LocalTargetInfo>(CS$<>8__locals1.<MakeNewToils>g__EffectTargetGetter|2), null);
			}
			yield return CS$<>8__locals1.watch;
			yield break;
		}

		// Token: 0x06007068 RID: 28776 RVA: 0x0022E1B4 File Offset: 0x0022C3B4
		protected virtual void WatchTickAction(int delta)
		{
			this.pawn.rotationTracker.FaceCell(base.TargetA.Cell);
			this.pawn.GainComfortFromCellIfPossible(delta, false);
			JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.EndJob, 1f, (Building)base.TargetThingA);
		}

		// Token: 0x06007069 RID: 28777 RVA: 0x0022E20C File Offset: 0x0022C40C
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				base.TargetA.Thing.def
			};
		}
	}
}
