using System;

namespace Verse
{
	// Token: 0x020007F0 RID: 2032
	public abstract class Stance_Busy : Stance
	{
		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x060033EA RID: 13290 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool StanceBusy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060033EB RID: 13291 RVA: 0x0011058C File Offset: 0x0010E78C
		public Stance_Busy()
		{
			this.SetPieSizeFactor();
			this.startedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x001105B5 File Offset: 0x0010E7B5
		public Stance_Busy(int ticks, LocalTargetInfo focusTarg, Verb verb)
		{
			this.ticksLeft = ticks;
			this.startedTick = Find.TickManager.TicksGame;
			this.focusTarg = focusTarg;
			this.verb = verb;
		}

		// Token: 0x060033ED RID: 13293 RVA: 0x001105ED File Offset: 0x0010E7ED
		public Stance_Busy(int ticks) : this(ticks, null, null)
		{
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x001105FD File Offset: 0x0010E7FD
		private void SetPieSizeFactor()
		{
			if (this.ticksLeft < 300)
			{
				this.pieSizeFactor = 1f;
				return;
			}
			if (this.ticksLeft < 450)
			{
				this.pieSizeFactor = 0.75f;
				return;
			}
			this.pieSizeFactor = 0.5f;
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x0011063C File Offset: 0x0010E83C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.startedTick, "startedTick", 0, false);
			Scribe_TargetInfo.Look(ref this.focusTarg, "focusTarg");
			Scribe_Values.Look<bool>(ref this.neverAimWeapon, "neverAimWeapon", false, false);
			Scribe_References.Look<Verb>(ref this.verb, "verb", false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.SetPieSizeFactor();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.verb != null && this.verb.BuggedAfterLoading)
			{
				this.verb = null;
				Type type = base.GetType();
				Log.Warning(((type != null) ? type.ToString() : null) + " had a bugged verb after loading.");
			}
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x001106F9 File Offset: 0x0010E8F9
		public override void StanceTick()
		{
			if (this.stanceTracker.stunner.Stunned)
			{
				return;
			}
			this.ticksLeft--;
			if (this.ticksLeft <= 0)
			{
				this.Expire();
			}
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x0011072B File Offset: 0x0010E92B
		protected virtual void Expire()
		{
			if (this.stanceTracker.curStance == this)
			{
				this.stanceTracker.SetStance(new Stance_Mobile());
			}
		}

		// Token: 0x0400272D RID: 10029
		public int ticksLeft;

		// Token: 0x0400272E RID: 10030
		public int startedTick;

		// Token: 0x0400272F RID: 10031
		public Verb verb;

		// Token: 0x04002730 RID: 10032
		public LocalTargetInfo focusTarg;

		// Token: 0x04002731 RID: 10033
		public bool neverAimWeapon;

		// Token: 0x04002732 RID: 10034
		protected float pieSizeFactor = 1f;
	}
}
