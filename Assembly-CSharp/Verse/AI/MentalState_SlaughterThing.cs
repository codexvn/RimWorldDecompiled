using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F8F RID: 3983
	public abstract class MentalState_SlaughterThing : MentalState
	{
		// Token: 0x17000F7E RID: 3966
		// (get) Token: 0x06006242 RID: 25154 RVA: 0x001F0975 File Offset: 0x001EEB75
		protected virtual int MinTicksBetweenSlaughter
		{
			get
			{
				return 3750;
			}
		}

		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x06006243 RID: 25155 RVA: 0x000E9647 File Offset: 0x000E7847
		protected virtual int MaxThingsSlaughtered
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x06006244 RID: 25156 RVA: 0x001F097C File Offset: 0x001EEB7C
		public bool SlaughteredRecently
		{
			get
			{
				return this.lastSlaughterTicks >= 0 && Find.TickManager.TicksGame - this.lastSlaughterTicks < this.MinTicksBetweenSlaughter;
			}
		}

		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x06006245 RID: 25157 RVA: 0x001F09A2 File Offset: 0x001EEBA2
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.lastSlaughterTicks >= 0;
			}
		}

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x06006246 RID: 25158
		protected abstract bool SlaughterTargetAvailable { get; }

		// Token: 0x06006247 RID: 25159 RVA: 0x001F09B0 File Offset: 0x001EEBB0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastSlaughterTicks, "lastSlaughterTicks", 0, false);
			Scribe_Values.Look<int>(ref this.thingsSlaughtered, "thingsSlaughtered", 0, false);
		}

		// Token: 0x06006248 RID: 25160 RVA: 0x001F09DC File Offset: 0x001EEBDC
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			if (this.pawn.IsHashIntervalTick(600, delta) && (this.pawn.CurJob == null || this.pawn.CurJob.def != JobDefOf.Slaughter) && !this.SlaughterTargetAvailable)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x06006249 RID: 25161 RVA: 0x001F0A35 File Offset: 0x001EEC35
		public override void Notify_SlaughteredTarget()
		{
			this.lastSlaughterTicks = Find.TickManager.TicksGame;
			this.thingsSlaughtered++;
			if (this.thingsSlaughtered >= this.MaxThingsSlaughtered)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x0400466C RID: 18028
		protected int lastSlaughterTicks = -1;

		// Token: 0x0400466D RID: 18029
		[LoadAlias("animalsSlaughtered")]
		protected int thingsSlaughtered;

		// Token: 0x0400466E RID: 18030
		protected const int NoThingToSlaughterCheckInterval = 600;
	}
}
