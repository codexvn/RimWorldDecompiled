using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011E6 RID: 4582
	public class JobDriver_PlayMusicalInstrument : JobDriver_SitFacingBuilding
	{
		// Token: 0x1700115A RID: 4442
		// (get) Token: 0x06006FF0 RID: 28656 RVA: 0x0022CBC4 File Offset: 0x0022ADC4
		public Building_MusicalInstrument MusicalInstrument
		{
			get
			{
				return (Building_MusicalInstrument)((Thing)this.job.GetTarget(TargetIndex.A));
			}
		}

		// Token: 0x06006FF1 RID: 28657 RVA: 0x0022CBDC File Offset: 0x0022ADDC
		protected override void ModifyPlayToil(Toil toil)
		{
			ModLister.CheckRoyaltyOrIdeology("Instrument");
			base.ModifyPlayToil(toil);
			Action<int> <>9__2;
			toil.AddPreInitAction(delegate
			{
				Building_MusicalInstrument musicalInstrument = this.MusicalInstrument;
				if (musicalInstrument != null)
				{
					musicalInstrument.StartPlaying(this.pawn);
				}
				Toil toil2 = toil;
				Action<int> tickIntervalAction;
				if ((tickIntervalAction = <>9__2) == null)
				{
					tickIntervalAction = (<>9__2 = delegate(int delta)
					{
						toil.actor.rotationTracker.FaceTarget(toil.actor.CurJob.GetTarget(this.TargetC.IsValid ? TargetIndex.C : TargetIndex.A));
						this.pawn.GainComfortFromCellIfPossible(delta, false);
						JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.EndJob, 1f, this.MusicalInstrument);
					});
				}
				toil2.tickIntervalAction = tickIntervalAction;
			});
			toil.handlingFacing = true;
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			toil.AddFinishAction(delegate
			{
				this.MusicalInstrument.StopPlaying();
			});
		}
	}
}
