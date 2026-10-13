using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BE6 RID: 7142
	public abstract class QuestPart_MTB : QuestPartActivable
	{
		// Token: 0x17001878 RID: 6264
		// (get) Token: 0x06009F1E RID: 40734
		protected abstract float MTBDays { get; }

		// Token: 0x06009F1F RID: 40735 RVA: 0x002DF634 File Offset: 0x002DD834
		public override void QuestPartTick()
		{
			float mtbdays = this.MTBDays;
			if (mtbdays > 0f && Find.TickManager.TicksGame % 10 == 0 && Rand.MTBEventOccurs(mtbdays, 60000f, 10f))
			{
				base.Complete();
			}
		}

		// Token: 0x06009F20 RID: 40736 RVA: 0x002DF678 File Offset: 0x002DD878
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "MTB occurs " + this.ToString(), true, true, true, null))
			{
				base.Complete();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x04006E08 RID: 28168
		private const int CheckIntervalTicks = 10;
	}
}
