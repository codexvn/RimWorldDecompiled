using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001A3B RID: 6715
	public class GatheringDef : Def
	{
		// Token: 0x1700171E RID: 5918
		// (get) Token: 0x06009740 RID: 38720 RVA: 0x002C30A2 File Offset: 0x002C12A2
		public bool IsRandomSelectable
		{
			get
			{
				return this.randomSelectionWeight > 0f;
			}
		}

		// Token: 0x1700171F RID: 5919
		// (get) Token: 0x06009741 RID: 38721 RVA: 0x002C30B1 File Offset: 0x002C12B1
		public GatheringWorker Worker
		{
			get
			{
				if (this.worker == null)
				{
					this.worker = (GatheringWorker)Activator.CreateInstance(this.workerClass);
					this.worker.def = this;
				}
				return this.worker;
			}
		}

		// Token: 0x06009742 RID: 38722 RVA: 0x002C30E3 File Offset: 0x002C12E3
		public bool CanExecute(Map map, Pawn organizer = null, bool ignoreGameConditions = false)
		{
			return (ignoreGameConditions || GatheringsUtility.AcceptableGameConditionsToStartGathering(map, this)) && this.Worker.CanExecute(map, organizer);
		}

		// Token: 0x04006448 RID: 25672
		public Type workerClass = typeof(GatheringWorker);

		// Token: 0x04006449 RID: 25673
		public DutyDef duty;

		// Token: 0x0400644A RID: 25674
		public float randomSelectionWeight;

		// Token: 0x0400644B RID: 25675
		public bool respectTimetable = true;

		// Token: 0x0400644C RID: 25676
		public List<ThingDef> gatherSpotDefs;

		// Token: 0x0400644D RID: 25677
		[MustTranslate]
		public string letterTitle;

		// Token: 0x0400644E RID: 25678
		[MustTranslate]
		public string letterText;

		// Token: 0x0400644F RID: 25679
		[MustTranslate]
		public string calledOffMessage;

		// Token: 0x04006450 RID: 25680
		[MustTranslate]
		public string finishedMessage;

		// Token: 0x04006451 RID: 25681
		public List<RoyalTitleDef> requiredTitleAny = new List<RoyalTitleDef>();

		// Token: 0x04006452 RID: 25682
		private GatheringWorker worker;
	}
}
