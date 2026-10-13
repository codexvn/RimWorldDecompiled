using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020007ED RID: 2029
	public class Pawn_StanceTracker : IExposable
	{
		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x060033D8 RID: 13272 RVA: 0x0011031F File Offset: 0x0010E51F
		public bool FullBodyBusy
		{
			get
			{
				return this.stunner.Stunned || this.curStance.StanceBusy;
			}
		}

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x060033D9 RID: 13273 RVA: 0x0011033B File Offset: 0x0010E53B
		public bool FullBodyBusyRecently
		{
			get
			{
				return GenTicks.TicksGame > this.lastTickFullBodyBusy + 2;
			}
		}

		// Token: 0x060033DA RID: 13274 RVA: 0x0011034C File Offset: 0x0010E54C
		public Pawn_StanceTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.stunner = new StunHandler(this.pawn);
			this.stagger = new StaggerHandler(this.pawn);
		}

		// Token: 0x060033DB RID: 13275 RVA: 0x00110388 File Offset: 0x0010E588
		public void StanceTrackerTick()
		{
			this.stunner.StunHandlerTick();
			this.stagger.StaggerHandlerTick();
			this.curStance.StanceTick();
			if (this.FullBodyBusy)
			{
				this.lastTickFullBodyBusy = GenTicks.TicksGame;
			}
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x001103BE File Offset: 0x0010E5BE
		public void StanceTrackerDraw()
		{
			this.curStance.StanceDraw();
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x001103CC File Offset: 0x0010E5CC
		public void ExposeData()
		{
			Scribe_Deep.Look<StunHandler>(ref this.stunner, "stunner", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<StaggerHandler>(ref this.stagger, "stagger", new object[]
			{
				this.pawn
			});
			Scribe_Deep.Look<Stance>(ref this.curStance, "curStance", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastTickFullBodyBusy, "lastTickFullBodyBusy", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.curStance != null)
			{
				this.curStance.stanceTracker = this;
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.stagger == null)
			{
				this.stagger = new StaggerHandler(this.pawn);
				int num = 0;
				Scribe_Values.Look<int>(ref num, "staggerUntilTick", 0, false);
				if (num > Find.TickManager.TicksGame)
				{
					this.stagger.StaggerFor(num - Find.TickManager.TicksGame, 0.17f);
				}
			}
		}

		// Token: 0x060033DE RID: 13278 RVA: 0x001104B5 File Offset: 0x0010E6B5
		[Obsolete("Use pawn.stances.stagger.StaggerFor instead")]
		public void StaggerFor(int ticks)
		{
			this.stagger.StaggerFor(ticks, 0.17f);
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x001104C9 File Offset: 0x0010E6C9
		public void CancelBusyStanceSoft()
		{
			if (this.curStance is Stance_Warmup)
			{
				this.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x001104E3 File Offset: 0x0010E6E3
		public void CancelBusyStanceHard()
		{
			this.SetStance(new Stance_Mobile());
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x001104F0 File Offset: 0x0010E6F0
		public void SetStance(Stance newStance)
		{
			if (this.debugLog)
			{
				Log.Message(string.Format("{0} {1} SetStance {2} -> {3}", new object[]
				{
					Find.TickManager.TicksGame,
					this.pawn,
					this.curStance,
					newStance
				}));
			}
			newStance.stanceTracker = this;
			this.curStance = newStance;
			if (this.pawn.jobs.curDriver != null)
			{
				this.pawn.jobs.curDriver.Notify_StanceChanged();
			}
		}

		// Token: 0x060033E2 RID: 13282 RVA: 0x000026EA File Offset: 0x000008EA
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
		}

		// Token: 0x04002722 RID: 10018
		public Pawn pawn;

		// Token: 0x04002723 RID: 10019
		public Stance curStance = new Stance_Mobile();

		// Token: 0x04002724 RID: 10020
		public int lastTickFullBodyBusy;

		// Token: 0x04002725 RID: 10021
		public StunHandler stunner;

		// Token: 0x04002726 RID: 10022
		public StaggerHandler stagger;

		// Token: 0x04002727 RID: 10023
		public const int StaggerMeleeAttackTicks = 95;

		// Token: 0x04002728 RID: 10024
		public const int StaggerBulletImpactTicks = 95;

		// Token: 0x04002729 RID: 10025
		public const int StaggerExplosionImpactTicks = 95;

		// Token: 0x0400272A RID: 10026
		public const int RecentlyDurationTicks = 2;

		// Token: 0x0400272B RID: 10027
		public bool debugLog;
	}
}
