using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002146 RID: 8518
	public class CompSchedule : ThingComp
	{
		// Token: 0x17001C4F RID: 7247
		// (get) Token: 0x0600BA16 RID: 47638 RVA: 0x0035B307 File Offset: 0x00359507
		public CompProperties_Schedule Props
		{
			get
			{
				return (CompProperties_Schedule)this.props;
			}
		}

		// Token: 0x17001C50 RID: 7248
		// (get) Token: 0x0600BA17 RID: 47639 RVA: 0x0035B314 File Offset: 0x00359514
		// (set) Token: 0x0600BA18 RID: 47640 RVA: 0x0035B31C File Offset: 0x0035951C
		public bool Allowed
		{
			get
			{
				return this.intAllowed;
			}
			set
			{
				if (this.intAllowed == value)
				{
					return;
				}
				this.intAllowed = value;
				this.parent.BroadcastCompSignal(this.intAllowed ? "ScheduledOn" : "ScheduledOff");
			}
		}

		// Token: 0x0600BA19 RID: 47641 RVA: 0x0035B34E File Offset: 0x0035954E
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.RecalculateAllowed();
		}

		// Token: 0x0600BA1A RID: 47642 RVA: 0x0035B35D File Offset: 0x0035955D
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.RecalculateAllowed();
		}

		// Token: 0x0600BA1B RID: 47643 RVA: 0x0035B36C File Offset: 0x0035956C
		public void RecalculateAllowed()
		{
			float num = GenLocalDate.DayPercent(this.parent);
			if (this.Props.startTime <= this.Props.endTime)
			{
				this.Allowed = (num > this.Props.startTime && num < this.Props.endTime);
				return;
			}
			this.Allowed = (num < this.Props.endTime || num > this.Props.startTime);
		}

		// Token: 0x0600BA1C RID: 47644 RVA: 0x0035B3E8 File Offset: 0x003595E8
		public override string CompInspectStringExtra()
		{
			if (!this.Allowed)
			{
				return this.Props.offMessage;
			}
			return null;
		}

		// Token: 0x0400804F RID: 32847
		public const string ScheduledOnSignal = "ScheduledOn";

		// Token: 0x04008050 RID: 32848
		public const string ScheduledOffSignal = "ScheduledOff";

		// Token: 0x04008051 RID: 32849
		private bool intAllowed;
	}
}
