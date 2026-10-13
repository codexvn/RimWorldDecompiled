using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E48 RID: 7752
	public class StorytellerComp_ImportantQuest : StorytellerComp
	{
		// Token: 0x17001A49 RID: 6729
		// (get) Token: 0x0600AC6E RID: 44142 RVA: 0x0031376A File Offset: 0x0031196A
		private static int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x17001A4A RID: 6730
		// (get) Token: 0x0600AC6F RID: 44143 RVA: 0x0031458F File Offset: 0x0031278F
		private StorytellerCompProperties_ImportantQuest Props
		{
			get
			{
				return (StorytellerCompProperties_ImportantQuest)this.props;
			}
		}

		// Token: 0x17001A4B RID: 6731
		// (get) Token: 0x0600AC70 RID: 44144 RVA: 0x0031459C File Offset: 0x0031279C
		private bool BeenGivenQuest
		{
			get
			{
				return Find.QuestManager.QuestsListForReading.Any((Quest q) => q.root == this.Props.questDef);
			}
		}

		// Token: 0x0600AC71 RID: 44145 RVA: 0x003145B9 File Offset: 0x003127B9
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (StorytellerComp_ImportantQuest.IntervalsPassed <= this.Props.fireAfterDaysPassed * 60 || this.BeenGivenQuest)
			{
				yield break;
			}
			IncidentDef questIncident = this.Props.questIncident;
			if (!questIncident.TargetAllowed(target))
			{
				yield break;
			}
			FiringIncident firingIncident = new FiringIncident(questIncident, this, this.GenerateParms(questIncident.category, target));
			yield return firingIncident;
			yield break;
		}
	}
}
