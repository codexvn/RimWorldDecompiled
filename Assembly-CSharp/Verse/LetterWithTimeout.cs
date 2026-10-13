using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BA9 RID: 2985
	public abstract class LetterWithTimeout : Letter
	{
		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x06004C4A RID: 19530 RVA: 0x00183EC1 File Offset: 0x001820C1
		public bool TimeoutActive
		{
			get
			{
				return this.disappearAtTick >= 0;
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06004C4B RID: 19531 RVA: 0x00183ECF File Offset: 0x001820CF
		public bool TimeoutPassed
		{
			get
			{
				return this.TimeoutActive && Find.TickManager.TicksGame >= this.disappearAtTick;
			}
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06004C4C RID: 19532 RVA: 0x00183EF0 File Offset: 0x001820F0
		public virtual bool LastTickBeforeTimeout
		{
			get
			{
				return this.TimeoutActive && this.disappearAtTick <= Find.TickManager.TicksGame + 1;
			}
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06004C4D RID: 19533 RVA: 0x00183F13 File Offset: 0x00182113
		public override bool ShouldAutomaticallyOpenLetter
		{
			get
			{
				return this.LastTickBeforeTimeout;
			}
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x06004C4E RID: 19534 RVA: 0x00183F1B File Offset: 0x0018211B
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && !this.TimeoutPassed;
			}
		}

		// Token: 0x06004C4F RID: 19535 RVA: 0x00183F32 File Offset: 0x00182132
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.disappearAtTick, "disappearAtTick", -1, false);
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x00183F4C File Offset: 0x0018214C
		public void StartTimeout(int duration)
		{
			this.disappearAtTick = Find.TickManager.TicksGame + duration;
		}

		// Token: 0x06004C51 RID: 19537 RVA: 0x00183F60 File Offset: 0x00182160
		protected override string PostProcessedLabel()
		{
			string text = base.PostProcessedLabel();
			if (this.TimeoutActive)
			{
				text += " (" + Mathf.RoundToInt((float)(this.disappearAtTick - Find.TickManager.TicksGame) / 2500f).ToString() + "LetterHour".Translate() + ")";
			}
			return text;
		}

		// Token: 0x0400366B RID: 13931
		public int disappearAtTick = -1;
	}
}
