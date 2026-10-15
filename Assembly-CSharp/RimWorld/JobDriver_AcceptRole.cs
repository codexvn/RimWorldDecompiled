using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

[StaticConstructorOnStartup]
public class JobDriver_AcceptRole : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__9 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_AcceptRole _003C_003E4__this;

		Toil IEnumerator<Toil>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CMakeNewToils_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_AcceptRole CS_0024_003C_003E8__locals24 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals24.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.OnCell);
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals24.pawn.GainComfortFromCellIfPossible(delta);
					CS_0024_003C_003E8__locals24.pawn.skills.Learn(SkillDefOf.Social, 0.3f * (float)delta);
					if (CS_0024_003C_003E8__locals24.ticksTillSocialInteraction <= 0)
					{
						if (CS_0024_003C_003E8__locals24.job.showSpeechBubbles)
						{
							MoteMaker.MakeSpeechBubble(CS_0024_003C_003E8__locals24.pawn, moteIcon);
						}
						if (CS_0024_003C_003E8__locals24.pawn.GetLord()?.LordJob is LordJob_Ritual lordJob_Ritual)
						{
							SocialInteractionUtility.ImitateSocialInteractionWithManyPawns(CS_0024_003C_003E8__locals24.pawn, lordJob_Ritual.lord.ownedPawns, (lordJob_Ritual.assignments.RoleChangeSelection != null) ? InteractionDefOf.Speech_AcceptRole : InteractionDefOf.Speech_RemoveRole);
						}
						CS_0024_003C_003E8__locals24.ticksTillSocialInteraction = SocialInteractionInterval;
					}
					if (CS_0024_003C_003E8__locals24.ticksTillFacingUpdate == 0)
					{
						IntVec3 facingCellCached = RitualUtility.RitualCrowdCenterFor(CS_0024_003C_003E8__locals24.pawn);
						if (facingCellCached.IsValid)
						{
							CS_0024_003C_003E8__locals24.facingCellCached = facingCellCached;
						}
						CS_0024_003C_003E8__locals24.ticksTillFacingUpdate = FacingUpdateInterval;
					}
					if (CS_0024_003C_003E8__locals24.facingCellCached.IsValid)
					{
						CS_0024_003C_003E8__locals24.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals24.facingCellCached);
					}
					else
					{
						CS_0024_003C_003E8__locals24.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals24.job.GetTarget(TargetIndex.B));
					}
					CS_0024_003C_003E8__locals24.rotateToFace = TargetIndex.B;
					CS_0024_003C_003E8__locals24.ticksTillSocialInteraction -= delta;
				};
				toil.PlaySustainerOrSound(() => (CS_0024_003C_003E8__locals24.pawn.gender != Gender.Female) ? CS_0024_003C_003E8__locals24.job.speechSoundMale : CS_0024_003C_003E8__locals24.job.speechSoundFemale, CS_0024_003C_003E8__locals24.pawn.story.VoicePitchFactor);
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				toil.handlingFacing = true;
				_003C_003E2__current = toil;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__9(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	private int ticksTillSocialInteraction = 60;

	private int ticksTillFacingUpdate;

	private IntVec3 facingCellCached = IntVec3.Invalid;

	private const TargetIndex StandIndex = TargetIndex.A;

	private const TargetIndex FacingIndex = TargetIndex.B;

	private static readonly int SocialInteractionInterval = 480;

	private static readonly int FacingUpdateInterval = 20;

	public static readonly Texture2D moteIcon = ContentFinder<Texture2D>.Get("Things/Mote/SpeechSymbols/Speech");

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(base.TargetLocA, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__9))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref ticksTillSocialInteraction, "ticksTillSocialInteraction", 0);
		Scribe_Values.Look(ref ticksTillFacingUpdate, "ticksTillFacingUpdate", 0);
		Scribe_Values.Look(ref facingCellCached, "facingCellCached");
	}
}
