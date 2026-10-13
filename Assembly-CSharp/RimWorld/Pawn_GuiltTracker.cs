using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002518 RID: 9496
	public class Pawn_GuiltTracker : IExposable
	{
		// Token: 0x17001FDD RID: 8157
		// (get) Token: 0x0600CF82 RID: 53122 RVA: 0x003CE7A1 File Offset: 0x003CC9A1
		public bool IsGuilty
		{
			get
			{
				return this.guiltyTicksLeft > 0 || (this.pawn.InAggroMentalState && this.pawn.MentalStateDef.allowGuilty);
			}
		}

		// Token: 0x17001FDE RID: 8158
		// (get) Token: 0x0600CF83 RID: 53123 RVA: 0x003CE7CD File Offset: 0x003CC9CD
		public int TicksUntilInnocent
		{
			get
			{
				return this.guiltyTicksLeft;
			}
		}

		// Token: 0x17001FDF RID: 8159
		// (get) Token: 0x0600CF84 RID: 53124 RVA: 0x003CE7D5 File Offset: 0x003CC9D5
		public string Tip
		{
			get
			{
				return "GuiltyDesc".Translate() + ": " + this.TicksUntilInnocent.ToStringTicksToPeriod(true, false, true, true, false);
			}
		}

		// Token: 0x0600CF85 RID: 53125 RVA: 0x003CE805 File Offset: 0x003CCA05
		public Pawn_GuiltTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CF86 RID: 53126 RVA: 0x003CE814 File Offset: 0x003CCA14
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.guiltyTicksLeft, "guiltyTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.awaitingExecution, "awaitingExecution", false, false);
		}

		// Token: 0x0600CF87 RID: 53127 RVA: 0x003CE83A File Offset: 0x003CCA3A
		public void Notify_Guilty(int durationTicks = 60000)
		{
			this.guiltyTicksLeft = durationTicks;
		}

		// Token: 0x0600CF88 RID: 53128 RVA: 0x003CE843 File Offset: 0x003CCA43
		public void GuiltTrackerTickInterval(int delta)
		{
			if (this.guiltyTicksLeft > 0)
			{
				this.guiltyTicksLeft -= delta;
				return;
			}
			if (!this.IsGuilty)
			{
				this.awaitingExecution = false;
			}
		}

		// Token: 0x04008D36 RID: 36150
		private Pawn pawn;

		// Token: 0x04008D37 RID: 36151
		public bool awaitingExecution;

		// Token: 0x04008D38 RID: 36152
		private int guiltyTicksLeft;

		// Token: 0x04008D39 RID: 36153
		private const int DefaultGuiltyDuration = 60000;
	}
}
