using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E69 RID: 11881
	public class CompStudyUnlocksMonolith : CompStudyUnlocks
	{
		// Token: 0x17002AF1 RID: 10993
		// (get) Token: 0x06010BB9 RID: 68537 RVA: 0x004D2E7F File Offset: 0x004D107F
		protected new CompProperties_StudyUnlocksMonolith Props
		{
			get
			{
				return (CompProperties_StudyUnlocksMonolith)this.props;
			}
		}

		// Token: 0x06010BBA RID: 68538 RVA: 0x004D2E8C File Offset: 0x004D108C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.nextIndex = Find.Anomaly.MonolithNextIndex;
			this.studyProgress = Find.Anomaly.MonolithStudyProgress;
		}

		// Token: 0x06010BBB RID: 68539 RVA: 0x004D2EAE File Offset: 0x004D10AE
		protected override void Notify_StudyLevelChanged(ChoiceLetter keptLetter)
		{
			Find.Anomaly.Notify_MonolithStudyIncreased(keptLetter, this.nextIndex, this.studyProgress);
			this.letters.Clear();
		}
	}
}
