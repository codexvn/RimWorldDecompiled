using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E4C RID: 7756
	public class StorytellerComp_MechanitorComplexQuest : StorytellerComp
	{
		// Token: 0x17001A4E RID: 6734
		// (get) Token: 0x0600AC7E RID: 44158 RVA: 0x0031473A File Offset: 0x0031293A
		private StorytellerCompProperties_MechanitorComplexQuest Props
		{
			get
			{
				return (StorytellerCompProperties_MechanitorComplexQuest)this.props;
			}
		}

		// Token: 0x17001A4F RID: 6735
		// (get) Token: 0x0600AC7F RID: 44159 RVA: 0x00314747 File Offset: 0x00312947
		private bool ExistingMechanitorOrMechlink
		{
			get
			{
				return MechanitorUtility.AnyMechanitorInPlayerFaction() || MechanitorUtility.AnyMechlinkInMap();
			}
		}

		// Token: 0x0600AC80 RID: 44160 RVA: 0x00314757 File Offset: 0x00312957
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (Find.History.mechanoidDatacoreOpportunityAvailable && !Find.History.mechanoidDatacoreReadOrLost)
			{
				yield break;
			}
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			if (!this.Props.blockedByQueuedOrActiveQuests.NullOrEmpty<QuestScriptDef>())
			{
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					if (this.Props.blockedByQueuedOrActiveQuests.Contains(questsListForReading[i].root) && (questsListForReading[i].State == QuestState.NotYetAccepted || questsListForReading[i].State == QuestState.Ongoing))
					{
						yield break;
					}
				}
			}
			int num = -1;
			for (int j = 0; j < questsListForReading.Count; j++)
			{
				if (questsListForReading[j].root == this.Props.incident.questScriptDef && questsListForReading[j].cleanupTick > num)
				{
					num = questsListForReading[j].cleanupTick;
				}
			}
			if (num > 0 && (Find.TickManager.TicksGame - num).TicksToDays() < this.Props.minSpacingDays)
			{
				yield break;
			}
			float num2 = (float)this.Props.mtbDays;
			if (this.Props.existingMechanitorOrMechlinkMTBFactor != 1f && this.ExistingMechanitorOrMechlink)
			{
				num2 *= this.Props.existingMechanitorOrMechlinkMTBFactor;
			}
			if (Rand.MTBEventOccurs(num2, 60000f, 1000f))
			{
				IncidentParms parms = this.GenerateParms(this.Props.incident.category, target);
				if (this.Props.incident.Worker.CanFireNow(parms))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
			}
			yield break;
		}
	}
}
