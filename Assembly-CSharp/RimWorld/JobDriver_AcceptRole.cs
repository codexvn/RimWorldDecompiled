using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001199 RID: 4505
	[StaticConstructorOnStartup]
	public class JobDriver_AcceptRole : JobDriver
	{
		// Token: 0x06006E29 RID: 28201 RVA: 0x00226893 File Offset: 0x00224A93
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(base.TargetLocA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006E2A RID: 28202 RVA: 0x002268B6 File Offset: 0x00224AB6
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.OnCell);
			toil.tickIntervalAction = delegate(int delta)
			{
				this.pawn.GainComfortFromCellIfPossible(delta, false);
				this.pawn.skills.Learn(SkillDefOf.Social, 0.3f * (float)delta, false, false);
				if (this.ticksTillSocialInteraction <= 0)
				{
					if (this.job.showSpeechBubbles)
					{
						MoteMaker.MakeSpeechBubble(this.pawn, JobDriver_AcceptRole.moteIcon);
					}
					Lord lord = this.pawn.GetLord();
					LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
					if (lordJob_Ritual != null)
					{
						SocialInteractionUtility.ImitateSocialInteractionWithManyPawns(this.pawn, lordJob_Ritual.lord.ownedPawns, (lordJob_Ritual.assignments.RoleChangeSelection != null) ? InteractionDefOf.Speech_AcceptRole : InteractionDefOf.Speech_RemoveRole);
					}
					this.ticksTillSocialInteraction = JobDriver_AcceptRole.SocialInteractionInterval;
				}
				if (this.ticksTillFacingUpdate == 0)
				{
					IntVec3 intVec = RitualUtility.RitualCrowdCenterFor(this.pawn);
					if (intVec.IsValid)
					{
						this.facingCellCached = intVec;
					}
					this.ticksTillFacingUpdate = JobDriver_AcceptRole.FacingUpdateInterval;
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
			};
			toil.PlaySustainerOrSound(delegate()
			{
				if (this.pawn.gender != Gender.Female)
				{
					return this.job.speechSoundMale;
				}
				return this.job.speechSoundFemale;
			}, this.pawn.story.VoicePitchFactor);
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.handlingFacing = true;
			yield return toil;
			yield break;
		}

		// Token: 0x06006E2B RID: 28203 RVA: 0x002268C8 File Offset: 0x00224AC8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksTillSocialInteraction, "ticksTillSocialInteraction", 0, false);
			Scribe_Values.Look<int>(ref this.ticksTillFacingUpdate, "ticksTillFacingUpdate", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.facingCellCached, "facingCellCached", default(IntVec3), false);
		}

		// Token: 0x04004CF4 RID: 19700
		private int ticksTillSocialInteraction = 60;

		// Token: 0x04004CF5 RID: 19701
		private int ticksTillFacingUpdate;

		// Token: 0x04004CF6 RID: 19702
		private IntVec3 facingCellCached = IntVec3.Invalid;

		// Token: 0x04004CF7 RID: 19703
		private const TargetIndex StandIndex = TargetIndex.A;

		// Token: 0x04004CF8 RID: 19704
		private const TargetIndex FacingIndex = TargetIndex.B;

		// Token: 0x04004CF9 RID: 19705
		private static readonly int SocialInteractionInterval = 480;

		// Token: 0x04004CFA RID: 19706
		private static readonly int FacingUpdateInterval = 20;

		// Token: 0x04004CFB RID: 19707
		public static readonly Texture2D moteIcon = ContentFinder<Texture2D>.Get("Things/Mote/SpeechSymbols/Speech", true);
	}
}
