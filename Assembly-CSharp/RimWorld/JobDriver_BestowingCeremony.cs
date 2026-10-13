using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001224 RID: 4644
	public class JobDriver_BestowingCeremony : JobDriver
	{
		// Token: 0x1700119F RID: 4511
		// (get) Token: 0x06007170 RID: 29040 RVA: 0x00231140 File Offset: 0x0022F340
		private Pawn Bestower
		{
			get
			{
				return base.TargetA.Pawn;
			}
		}

		// Token: 0x170011A0 RID: 4512
		// (get) Token: 0x06007171 RID: 29041 RVA: 0x0023115B File Offset: 0x0022F35B
		private LordJob_BestowingCeremony CeremonyJob
		{
			get
			{
				return (LordJob_BestowingCeremony)this.Bestower.GetLord().LordJob;
			}
		}

		// Token: 0x170011A1 RID: 4513
		// (get) Token: 0x06007172 RID: 29042 RVA: 0x001E5D15 File Offset: 0x001E3F15
		private LocalTargetInfo BestowSpot
		{
			get
			{
				return this.job.targetB;
			}
		}

		// Token: 0x06007173 RID: 29043 RVA: 0x00231174 File Offset: 0x0022F374
		public static bool AnalyzeThroneRoom(Pawn bestower, Pawn target)
		{
			RoyalTitleDef titleAwardedWhenUpdating = target.royalty.GetTitleAwardedWhenUpdating(bestower.Faction, target.royalty.GetFavor(bestower.Faction));
			if (titleAwardedWhenUpdating != null && titleAwardedWhenUpdating.throneRoomRequirements != null)
			{
				using (List<RoomRequirement>.Enumerator enumerator = titleAwardedWhenUpdating.throneRoomRequirements.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.MetOrDisabled(bestower.GetRoom(RegionType.Set_All), target))
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06007174 RID: 29044 RVA: 0x00231204 File Offset: 0x0022F404
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.Bestower.ClearAllReservations(true);
			return this.pawn.Reserve(this.BestowSpot, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.Bestower, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007175 RID: 29045 RVA: 0x0023125E File Offset: 0x0022F45E
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckRoyalty("Bestowing ceremony"))
			{
				yield break;
			}
			base.AddFailCondition(() => this.Bestower.GetLord() == null || this.Bestower.GetLord().CurLordToil == null || !(this.Bestower.GetLord().CurLordToil is LordToil_BestowingCeremony_Perform));
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil waitToil = Toils_General.Wait(5000, TargetIndex.None);
			waitToil.AddPreInitAction(delegate
			{
				Messages.Message("MessageBestowingCeremonyStarted".Translate(this.pawn.Named("PAWN")), this.Bestower, MessageTypeDefOf.PositiveEvent, true);
			});
			waitToil.AddPreInitAction(delegate
			{
				if (!JobDriver_BestowingCeremony.AnalyzeThroneRoom(this.Bestower, this.pawn))
				{
					Messages.Message("BestowingCeremonyThroneroomRequirementsNotSatisfied".Translate(this.pawn.Named("PAWN"), this.pawn.royalty.GetTitleAwardedWhenUpdating(this.Bestower.Faction, this.pawn.royalty.GetFavor(this.Bestower.Faction)).label.Named("TITLE")), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					((LordJob_BestowingCeremony)this.Bestower.GetLord().LordJob).MakeCeremonyFail();
				}
			});
			waitToil.AddPreInitAction(delegate
			{
				SoundDefOf.Bestowing_Start.PlayOneShot(this.pawn);
			});
			waitToil.tickAction = delegate()
			{
				this.pawn.rotationTracker.FaceTarget(this.Bestower);
				if (this.mote == null || this.mote.Destroyed)
				{
					Vector3 loc = (this.pawn.TrueCenter() + this.Bestower.TrueCenter()) / 2f;
					this.mote = MoteMaker.MakeStaticMote(loc, this.pawn.Map, ThingDefOf.Mote_Bestow, 1f, false, 0f);
				}
				Mote mote = this.mote;
				if (mote != null)
				{
					mote.Maintain();
				}
				if ((this.sound == null || this.sound.Ended) && waitToil.actor.jobs.curDriver.ticksLeftThisToil <= 307)
				{
					this.sound = SoundDefOf.Bestowing_Warmup.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(this.pawn.Position, this.pawn.Map, false), MaintenanceType.PerTick));
				}
				if (this.sound != null)
				{
					this.sound.Maintain();
				}
			};
			waitToil.handlingFacing = false;
			waitToil.socialMode = RandomSocialMode.Off;
			waitToil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return waitToil;
			yield return Toils_General.Do(delegate
			{
				this.CeremonyJob.FinishCeremony(this.pawn);
				FleckMaker.Static((this.pawn.TrueCenter() + this.Bestower.TrueCenter()) / 2f, this.pawn.Map, FleckDefOf.PsycastAreaEffect, 2f);
				SoundDefOf.Bestowing_Finished.PlayOneShot(this.pawn);
			});
			yield break;
		}

		// Token: 0x04004E98 RID: 20120
		public const float WarmupSoundLength = 5.125f;

		// Token: 0x04004E99 RID: 20121
		public const int BestowTimeTicks = 5000;

		// Token: 0x04004E9A RID: 20122
		public const int PlayWarmupSoundAfterTicks = 307;

		// Token: 0x04004E9B RID: 20123
		protected const TargetIndex BestowerInd = TargetIndex.A;

		// Token: 0x04004E9C RID: 20124
		protected const TargetIndex BestowSpotInd = TargetIndex.B;

		// Token: 0x04004E9D RID: 20125
		private Mote mote;

		// Token: 0x04004E9E RID: 20126
		private Sustainer sound;
	}
}
