using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020007CD RID: 1997
	public class Pawn_CallTracker
	{
		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x060032A7 RID: 12967 RVA: 0x00109158 File Offset: 0x00107358
		private bool PawnAggressive
		{
			get
			{
				return this.pawn.InAggroMentalState || (this.pawn.mindState.enemyTarget != null && this.pawn.mindState.enemyTarget.Spawned && Find.TickManager.TicksGame - this.pawn.mindState.lastEngageTargetTick <= 360) || (this.pawn.CurJob != null && this.pawn.CurJob.def == JobDefOf.AttackMelee);
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x060032A8 RID: 12968 RVA: 0x001091E8 File Offset: 0x001073E8
		private float IdleCallVolumeFactor
		{
			get
			{
				switch (Find.TickManager.CurTimeSpeed)
				{
				case TimeSpeed.Paused:
					return 1f;
				case TimeSpeed.Normal:
					return 1f;
				case TimeSpeed.Fast:
					return 1f;
				case TimeSpeed.Superfast:
					return 0.25f;
				case TimeSpeed.Ultrafast:
					return 0.25f;
				default:
					throw new NotImplementedException();
				}
			}
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x0010923F File Offset: 0x0010743F
		public Pawn_CallTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x00109255 File Offset: 0x00107455
		public void CallTrackerTickInterval(int delta)
		{
			if (this.ticksToNextCall < 0)
			{
				this.ResetTicksToNextCall();
			}
			this.ticksToNextCall -= delta;
			if (this.ticksToNextCall <= 0)
			{
				this.TryDoCall();
				this.ResetTicksToNextCall();
			}
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x0010928C File Offset: 0x0010748C
		private void ResetTicksToNextCall()
		{
			if (this.pawn.IsMutant)
			{
				this.ticksToNextCall = MutantUtility.CallIntervalRange.RandomInRange;
				return;
			}
			this.ticksToNextCall = this.pawn.def.race.soundCallIntervalRange.RandomInRange;
			if (this.PawnAggressive)
			{
				this.ticksToNextCall = (int)((float)this.ticksToNextCall * this.pawn.def.race.soundCallIntervalAggressiveFactor);
				return;
			}
			if (this.pawn.Faction != null && this.pawn.Faction.IsPlayer)
			{
				this.ticksToNextCall = (int)((float)this.ticksToNextCall * this.pawn.def.race.soundCallIntervalFriendlyFactor);
			}
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x00109348 File Offset: 0x00107548
		private void TryDoCall()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			if (!Find.CameraDriver.CurrentViewRect.ExpandedBy(10).Contains(this.pawn.Position))
			{
				return;
			}
			if (this.pawn.Downed || !this.pawn.Awake())
			{
				return;
			}
			if (this.pawn.IsPsychologicallyInvisible())
			{
				return;
			}
			if (this.pawn.Position.Fogged(this.pawn.MapHeld))
			{
				return;
			}
			if (this.pawn.IsColonyMech && this.pawn.IsCharging())
			{
				return;
			}
			this.DoCall(false);
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x001093F8 File Offset: 0x001075F8
		public void DoCall(bool forceAggressive = false)
		{
			if (this.PawnAggressive || forceAggressive)
			{
				LifeStageUtility.PlayNearestLifestageSound(this.pawn, (LifeStageAge lifeStage) => lifeStage.soundAngry, null, (MutantDef mutant) => mutant.soundAngry, 1f);
				return;
			}
			LifeStageUtility.PlayNearestLifestageSound(this.pawn, (LifeStageAge lifeStage) => lifeStage.soundCall, (GeneDef gene) => gene.soundCall, (MutantDef mutant) => mutant.soundCall, this.IdleCallVolumeFactor);
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x001094D0 File Offset: 0x001076D0
		public void Notify_InAggroMentalState()
		{
			this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x001094F0 File Offset: 0x001076F0
		public void Notify_DidMeleeAttack()
		{
			if (Rand.Value < 0.5f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnMeleeDelayRange.RandomInRange;
			}
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x0010951C File Offset: 0x0010771C
		public void Notify_Released()
		{
			if (Rand.Value < 0.75f)
			{
				this.ticksToNextCall = Pawn_CallTracker.CallOnAggroDelayRange.RandomInRange;
			}
		}

		// Token: 0x04002683 RID: 9859
		public Pawn pawn;

		// Token: 0x04002684 RID: 9860
		private int ticksToNextCall = -1;

		// Token: 0x04002685 RID: 9861
		private static readonly IntRange CallOnAggroDelayRange = new IntRange(0, 120);

		// Token: 0x04002686 RID: 9862
		private static readonly IntRange CallOnMeleeDelayRange = new IntRange(0, 20);

		// Token: 0x04002687 RID: 9863
		private const float AngryCallOnMeleeChance = 0.5f;

		// Token: 0x04002688 RID: 9864
		private const int AggressiveDurationAfterEngagingTarget = 360;
	}
}
