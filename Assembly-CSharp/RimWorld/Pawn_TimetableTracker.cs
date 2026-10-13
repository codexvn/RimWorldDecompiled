using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020024E4 RID: 9444
	public class Pawn_TimetableTracker : IExposable
	{
		// Token: 0x17001FA3 RID: 8099
		// (get) Token: 0x0600CE40 RID: 52800 RVA: 0x003C8E7F File Offset: 0x003C707F
		public TimeAssignmentDef CurrentAssignment
		{
			get
			{
				if (!this.pawn.IsColonist || this.pawn.IsPrisonerOfColony)
				{
					return TimeAssignmentDefOf.Anything;
				}
				return this.times[GenLocalDate.HourOfDay(this.pawn)];
			}
		}

		// Token: 0x0600CE41 RID: 52801 RVA: 0x003C8EB8 File Offset: 0x003C70B8
		public Pawn_TimetableTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.times = new List<TimeAssignmentDef>(24);
			for (int i = 0; i < 24; i++)
			{
				TimeAssignmentDef item;
				if (i <= 5 || i > 21)
				{
					item = TimeAssignmentDefOf.Sleep;
				}
				else
				{
					item = TimeAssignmentDefOf.Anything;
				}
				this.times.Add(item);
			}
		}

		// Token: 0x0600CE42 RID: 52802 RVA: 0x003C8F10 File Offset: 0x003C7110
		public void ExposeData()
		{
			Scribe_Collections.Look<TimeAssignmentDef>(ref this.times, "times", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !ModsConfig.RoyaltyActive)
			{
				for (int i = 0; i < this.times.Count; i++)
				{
					if (this.times[i] == TimeAssignmentDefOf.Meditate)
					{
						this.times[i] = TimeAssignmentDefOf.Anything;
					}
				}
			}
		}

		// Token: 0x0600CE43 RID: 52803 RVA: 0x003C8F7C File Offset: 0x003C717C
		public TimeAssignmentDef GetAssignment(int hour)
		{
			return this.times[hour];
		}

		// Token: 0x0600CE44 RID: 52804 RVA: 0x003C8F8A File Offset: 0x003C718A
		public void SetAssignment(int hour, TimeAssignmentDef ta)
		{
			this.times[hour] = ta;
		}

		// Token: 0x04008C5B RID: 35931
		private Pawn pawn;

		// Token: 0x04008C5C RID: 35932
		public List<TimeAssignmentDef> times;
	}
}
