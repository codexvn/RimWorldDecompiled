using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002504 RID: 9476
	public class DrugTakeRecord : IExposable
	{
		// Token: 0x17001FC2 RID: 8130
		// (get) Token: 0x0600CF05 RID: 52997 RVA: 0x003CC66A File Offset: 0x003CA86A
		public int LastTakenDays
		{
			get
			{
				return GenDate.DaysPassedAt(this.lastTakenTicks);
			}
		}

		// Token: 0x17001FC3 RID: 8131
		// (get) Token: 0x0600CF06 RID: 52998 RVA: 0x003CC677 File Offset: 0x003CA877
		// (set) Token: 0x0600CF07 RID: 52999 RVA: 0x003CC68E File Offset: 0x003CA88E
		public int TimesTakenThisDay
		{
			get
			{
				if (this.thisDay != GenDate.DaysPassed)
				{
					return 0;
				}
				return this.timesTakenThisDayInt;
			}
			set
			{
				this.timesTakenThisDayInt = value;
				this.thisDay = GenDate.DaysPassed;
			}
		}

		// Token: 0x0600CF08 RID: 53000 RVA: 0x003CC6A4 File Offset: 0x003CA8A4
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<int>(ref this.lastTakenTicks, "lastTakenTicks", 0, false);
			Scribe_Values.Look<int>(ref this.timesTakenThisDayInt, "timesTakenThisDay", 0, false);
			Scribe_Values.Look<int>(ref this.thisDay, "thisDay", 0, false);
		}

		// Token: 0x0600CF09 RID: 53001 RVA: 0x003CC6F7 File Offset: 0x003CA8F7
		public void Notify_LeftSuspension(int suspendedTicks)
		{
			this.lastTakenTicks += suspendedTicks;
			if (GenDate.DaysPassedAt(Find.TickManager.TicksGame - suspendedTicks) == this.thisDay)
			{
				this.thisDay = GenDate.DaysPassed;
			}
		}

		// Token: 0x04008CE3 RID: 36067
		public ThingDef drug;

		// Token: 0x04008CE4 RID: 36068
		public int lastTakenTicks;

		// Token: 0x04008CE5 RID: 36069
		private int timesTakenThisDayInt;

		// Token: 0x04008CE6 RID: 36070
		private int thisDay;
	}
}
