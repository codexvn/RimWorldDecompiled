using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B8E RID: 7054
	public class QuestPart_PassOutInterval : QuestPartActivable
	{
		// Token: 0x06009D7B RID: 40315 RVA: 0x002DA4D8 File Offset: 0x002D86D8
		public override void QuestPartTick()
		{
			if (this.currentInterval < 0)
			{
				foreach (string tag in this.outSignals)
				{
					Find.SignalManager.SendSignal(new Signal(tag, false));
				}
				this.currentInterval = this.ticksInterval.RandomInRange;
			}
			this.currentInterval--;
		}

		// Token: 0x06009D7C RID: 40316 RVA: 0x002DA560 File Offset: 0x002D8760
		protected override void ProcessQuestSignal(Signal signal)
		{
			if (this.inSignalsDisable.Contains(signal.tag))
			{
				this.Disable();
			}
		}

		// Token: 0x06009D7D RID: 40317 RVA: 0x002DA57C File Offset: 0x002D877C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.inSignalsDisable, "inSignalsDisable", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.currentInterval, "currentInterval", 0, false);
			Scribe_Values.Look<IntRange>(ref this.ticksInterval, "ticksInterval", default(IntRange), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.outSignals = (this.outSignals ?? new List<string>());
				this.inSignalsDisable = (this.inSignalsDisable ?? new List<string>());
			}
		}

		// Token: 0x06009D7E RID: 40318 RVA: 0x002DA61C File Offset: 0x002D881C
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "Reset Interval " + this.ToString(), true, true, true, null))
			{
				this.currentInterval = 0;
			}
			curY += rect.height + 4f;
		}

		// Token: 0x04006D24 RID: 27940
		public IntRange ticksInterval;

		// Token: 0x04006D25 RID: 27941
		public List<string> outSignals = new List<string>();

		// Token: 0x04006D26 RID: 27942
		public List<string> inSignalsDisable = new List<string>();

		// Token: 0x04006D27 RID: 27943
		private int currentInterval;
	}
}
