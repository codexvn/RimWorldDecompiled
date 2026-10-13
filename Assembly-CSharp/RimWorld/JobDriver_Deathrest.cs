using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200124B RID: 4683
	public class JobDriver_Deathrest : JobDriver
	{
		// Token: 0x170011D8 RID: 4568
		// (get) Token: 0x0600728E RID: 29326 RVA: 0x00234F20 File Offset: 0x00233120
		private Building_Bed Bed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Building_Bed;
			}
		}

		// Token: 0x170011D9 RID: 4569
		// (get) Token: 0x0600728F RID: 29327 RVA: 0x00234F46 File Offset: 0x00233146
		public override bool PlayerInterruptable
		{
			get
			{
				return !base.OnLastToil;
			}
		}

		// Token: 0x06007290 RID: 29328 RVA: 0x00234F51 File Offset: 0x00233151
		public override string GetReport()
		{
			return this.ReportStringProcessed(SanguophageUtility.DeathrestJobReport(this.pawn));
		}

		// Token: 0x06007291 RID: 29329 RVA: 0x00234F64 File Offset: 0x00233164
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.Bed == null || this.pawn.Reserve(this.Bed, this.job, this.Bed.SleepingSlotsCount, 0, null, errorOnFailed, false);
		}

		// Token: 0x06007292 RID: 29330 RVA: 0x00234F9B File Offset: 0x0023319B
		public override bool CanBeginNowWhileLyingDown()
		{
			return JobInBedUtility.InBedOrRestSpotNow(this.pawn, this.job.GetTarget(TargetIndex.A));
		}

		// Token: 0x06007293 RID: 29331 RVA: 0x00234FB4 File Offset: 0x002331B4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			bool hasBed = this.Bed != null;
			if (hasBed)
			{
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A, TargetIndex.None);
				Toil toil = Toils_Bed.GotoBed(TargetIndex.A);
				toil.AddFailCondition(delegate
				{
					if (SanguophageUtility.ShouldBeDeathrestingOrInComaInsteadOfDead(this.pawn))
					{
						return false;
					}
					using (CellRect.Enumerator enumerator = this.Bed.OccupiedRect().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.InSunlight(this.Bed.Map))
							{
								Messages.Message("MessageBedExposedToSunlight".Translate(this.pawn.Named("PAWN"), this.Bed.Named("BED")), this.Bed, MessageTypeDefOf.RejectInput, true);
								return true;
							}
						}
					}
					return false;
				});
				yield return toil;
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			}
			Toil toil2 = Toils_LayDown.LayDown(TargetIndex.A, hasBed, false, true, true, PawnPosture.LayingOnGroundNormal, true);
			Toil toil3 = toil2;
			toil3.initAction = (Action)Delegate.Combine(toil3.initAction, new Action(delegate()
			{
				if (this.pawn.Drafted)
				{
					this.pawn.drafter.Drafted = false;
				}
				if (!this.pawn.health.hediffSet.HasHediff(HediffDefOf.Deathrest, false))
				{
					this.pawn.health.AddHediff(HediffDefOf.Deathrest, null, null, null);
				}
			}));
			Toil toil4 = toil2;
			toil4.tickIntervalAction = (Action<int>)Delegate.Combine(toil4.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				if (this.pawn.IsHashIntervalTick(160, delta))
				{
					MoteMaker.MakeAttachedOverlay(this.pawn, ThingDefOf.Mote_Deathresting, new Vector3(0f, this.pawn.story.bodyType.bedOffset).RotatedBy(this.pawn.Rotation), 1f, -1f);
				}
			}));
			yield return toil2;
			yield break;
		}

		// Token: 0x04004F2A RID: 20266
		private const TargetIndex BedIndex = TargetIndex.A;

		// Token: 0x04004F2B RID: 20267
		private const int MoteIntervalTicks = 160;
	}
}
