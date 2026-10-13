using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002539 RID: 9529
	public class Thought_Dumb : Thought
	{
		// Token: 0x17002004 RID: 8196
		// (get) Token: 0x0600D07C RID: 53372 RVA: 0x003D534E File Offset: 0x003D354E
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x0600D07D RID: 53373 RVA: 0x003D5356 File Offset: 0x003D3556
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x0600D07E RID: 53374 RVA: 0x003D535F File Offset: 0x003D355F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}

		// Token: 0x04008DD0 RID: 36304
		private int forcedStage;
	}
}
