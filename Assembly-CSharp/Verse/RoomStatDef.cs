using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002DA RID: 730
	public class RoomStatDef : Def
	{
		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06001203 RID: 4611 RVA: 0x00054167 File Offset: 0x00052367
		public RoomStatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RoomStatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x00054199 File Offset: 0x00052399
		public RoomStatScoreStage GetScoreStage(float score)
		{
			if (this.scoreStages.NullOrEmpty<RoomStatScoreStage>())
			{
				return null;
			}
			return this.scoreStages[this.GetScoreStageIndex(score)];
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x000541BC File Offset: 0x000523BC
		public int GetScoreStageIndex(float score)
		{
			if (this.scoreStages.NullOrEmpty<RoomStatScoreStage>())
			{
				throw new InvalidOperationException("No score stages available.");
			}
			int result = 0;
			int num = 0;
			while (num < this.scoreStages.Count && score >= this.scoreStages[num].minScore)
			{
				result = num;
				num++;
			}
			return result;
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x00054210 File Offset: 0x00052410
		public string ScoreToString(float score)
		{
			if (this.displayRounded)
			{
				return Mathf.RoundToInt(score).ToString();
			}
			return score.ToString("F2");
		}

		// Token: 0x0400103B RID: 4155
		public Type workerClass;

		// Token: 0x0400103C RID: 4156
		public float updatePriority;

		// Token: 0x0400103D RID: 4157
		public bool displayRounded;

		// Token: 0x0400103E RID: 4158
		public bool isHidden;

		// Token: 0x0400103F RID: 4159
		public float roomlessScore;

		// Token: 0x04001040 RID: 4160
		public List<RoomStatScoreStage> scoreStages;

		// Token: 0x04001041 RID: 4161
		public RoomStatDef inputStat;

		// Token: 0x04001042 RID: 4162
		public SimpleCurve curve;

		// Token: 0x04001043 RID: 4163
		[Unsaved(false)]
		private RoomStatWorker workerInt;
	}
}
