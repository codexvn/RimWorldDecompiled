using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020011A9 RID: 4521
	[StaticConstructorOnStartup]
	public class JobDriver_GiveSpeech : JobDriver
	{
		// Token: 0x17001124 RID: 4388
		// (get) Token: 0x06006E8B RID: 28299 RVA: 0x00227D5B File Offset: 0x00225F5B
		private Building_Throne Throne
		{
			get
			{
				return (Building_Throne)base.TargetThingA;
			}
		}

		// Token: 0x06006E8C RID: 28300 RVA: 0x00227D68 File Offset: 0x00225F68
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.Throne != null)
			{
				return this.pawn.Reserve(this.Throne, this.job, 1, -1, null, errorOnFailed, false);
			}
			return this.pawn.Reserve(base.TargetLocA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006E8D RID: 28301 RVA: 0x00227DC0 File Offset: 0x00225FC0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return Toils_General.Do(delegate
			{
				LocalTargetInfo pack = (this.Throne != null) ? (this.Throne.InteractionCell + this.Throne.Rotation.FacingCell) : base.TargetB;
				this.job.SetTarget(TargetIndex.B, pack);
			});
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			if (this.Throne != null)
			{
				toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
				toil.FailOn(() => this.Throne.AssignedPawn != this.pawn);
				toil.FailOn(() => RoomRoleWorker_ThroneRoom.Validate(this.Throne.GetRoom(RegionType.Set_All)) != null);
			}
			else
			{
				toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.OnCell);
			}
			toil.tickIntervalAction = delegate(int delta)
			{
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				this.pawn.skills.Learn(SkillDefOf.Social, 0.3f * (float)delta, false, false);
				if (this.ticksTillSocialInteraction <= 0)
				{
					if (this.job.showSpeechBubbles)
					{
						MoteMaker.MakeSpeechBubble(this.pawn, JobDriver_GiveSpeech.moteIcon);
					}
					if (this.job.interaction != null)
					{
						Lord lord = this.pawn.GetLord();
						LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
						if (lordJob_Ritual != null)
						{
							SocialInteractionUtility.ImitateSocialInteractionWithManyPawns(this.pawn, lordJob_Ritual.lord.ownedPawns, this.job.interaction);
						}
					}
					this.ticksTillSocialInteraction = JobDriver_GiveSpeech.SocialInteractionInterval;
				}
				if (this.ticksTillFacingUpdate == 0)
				{
					IntVec3 intVec = RitualUtility.RitualCrowdCenterFor(this.pawn);
					if (intVec.IsValid)
					{
						this.facingCellCached = intVec;
					}
					this.ticksTillFacingUpdate = JobDriver_GiveSpeech.FacingUpdateInterval;
				}
				if (this.facingCellCached.IsValid)
				{
					this.pawn.rotationTracker.FaceTarget(this.facingCellCached);
				}
				else
				{
					this.pawn.rotationTracker.FaceTarget(this.job.GetTarget(TargetIndex.B));
				}
				this.rotateToFace = TargetIndex.B;
				this.ticksTillSocialInteraction -= delta;
				this.ticksTillFacingUpdate -= delta;
			};
			if (ModsConfig.IdeologyActive)
			{
				toil.PlaySustainerOrSound(delegate()
				{
					if (this.pawn.gender != Gender.Female)
					{
						return this.job.speechSoundMale;
					}
					return this.job.speechSoundFemale;
				}, this.pawn.story.VoicePitchFactor);
			}
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.handlingFacing = true;
			yield return toil;
			yield break;
		}

		// Token: 0x06006E8E RID: 28302 RVA: 0x00227DD0 File Offset: 0x00225FD0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksTillSocialInteraction, "ticksTillSocialInteraction", 0, false);
			Scribe_Values.Look<int>(ref this.ticksTillFacingUpdate, "ticksTillFacingUpdate", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.facingCellCached, "facingCellCached", default(IntVec3), false);
		}

		// Token: 0x04004D30 RID: 19760
		private int ticksTillSocialInteraction = 60;

		// Token: 0x04004D31 RID: 19761
		private int ticksTillFacingUpdate;

		// Token: 0x04004D32 RID: 19762
		private IntVec3 facingCellCached = IntVec3.Invalid;

		// Token: 0x04004D33 RID: 19763
		private const TargetIndex StandIndex = TargetIndex.A;

		// Token: 0x04004D34 RID: 19764
		private const TargetIndex FacingIndex = TargetIndex.B;

		// Token: 0x04004D35 RID: 19765
		private static readonly int SocialInteractionInterval = 480;

		// Token: 0x04004D36 RID: 19766
		private static readonly int FacingUpdateInterval = 20;

		// Token: 0x04004D37 RID: 19767
		public static readonly Texture2D moteIcon = ContentFinder<Texture2D>.Get("Things/Mote/SpeechSymbols/Speech", true);
	}
}
