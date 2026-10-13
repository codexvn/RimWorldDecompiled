using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E4F RID: 7759
	public class StorytellerComp_MonolithMigration : StorytellerComp
	{
		// Token: 0x17001A52 RID: 6738
		// (get) Token: 0x0600AC8B RID: 44171 RVA: 0x003149DF File Offset: 0x00312BDF
		protected StorytellerCompProperties_MonolithMigration Props
		{
			get
			{
				return (StorytellerCompProperties_MonolithMigration)this.props;
			}
		}

		// Token: 0x0600AC8C RID: 44172 RVA: 0x003149EC File Offset: 0x00312BEC
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (!ModsConfig.AnomalyActive)
			{
				yield break;
			}
			if (Find.Anomaly.monolith != null)
			{
				yield break;
			}
			if (Find.Anomaly.LevelDef == MonolithLevelDefOf.Disrupted)
			{
				yield break;
			}
			if (Find.QuestManager.QuestsListForReading.Any((Quest q) => q.root == QuestScriptDefOf.MonolithMigration && (q.State == QuestState.Ongoing || q.State == QuestState.NotYetAccepted)))
			{
				yield break;
			}
			if (Rand.MTBEventOccurs(DebugSettings.fastMonolithRespawn ? 0.1f : this.Props.mtbDays, 60000f, 1000f))
			{
				yield return new FiringIncident(this.Props.incident, this, this.GenerateParms(IncidentCategoryDefOf.Misc, target));
			}
			yield break;
		}
	}
}
