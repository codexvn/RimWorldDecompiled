using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E62 RID: 11874
	public class CompStudiableMonolith : CompStudiable
	{
		// Token: 0x17002AE5 RID: 10981
		// (get) Token: 0x06010B8F RID: 68495 RVA: 0x004D268F File Offset: 0x004D088F
		public override float AnomalyKnowledge
		{
			get
			{
				if (!Find.Anomaly.QuestlineEnded)
				{
					return base.AnomalyKnowledge;
				}
				return 4f;
			}
		}

		// Token: 0x17002AE6 RID: 10982
		// (get) Token: 0x06010B90 RID: 68496 RVA: 0x004D26A9 File Offset: 0x004D08A9
		public override KnowledgeCategoryDef KnowledgeCategory
		{
			get
			{
				return Find.Anomaly.LevelDef.monolithStudyCategory;
			}
		}

		// Token: 0x06010B91 RID: 68497 RVA: 0x004D26BA File Offset: 0x004D08BA
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.anomalyKnowledgeGained = Find.Anomaly.monolithAnomalyKnowledge;
		}

		// Token: 0x06010B92 RID: 68498 RVA: 0x004D26D3 File Offset: 0x004D08D3
		public override void Study(Pawn studier, float studyAmount, float anomalyKnowledgeAmount = 0f)
		{
			base.Study(studier, studyAmount, anomalyKnowledgeAmount);
			Find.Anomaly.monolithAnomalyKnowledge = this.anomalyKnowledgeGained;
		}

		// Token: 0x0400AE4E RID: 44622
		private const float FinishedStudyAmount = 4f;
	}
}
