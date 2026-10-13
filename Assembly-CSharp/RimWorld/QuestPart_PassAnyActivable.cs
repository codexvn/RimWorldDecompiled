using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B8C RID: 7052
	public class QuestPart_PassAnyActivable : QuestPartActivable
	{
		// Token: 0x06009D73 RID: 40307 RVA: 0x002DA30C File Offset: 0x002D850C
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				base.Complete();
			}
		}

		// Token: 0x06009D74 RID: 40308 RVA: 0x002DA32E File Offset: 0x002D852E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06009D75 RID: 40309 RVA: 0x002DA34C File Offset: 0x002D854C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int.ToString());
			}
		}

		// Token: 0x04006D21 RID: 27937
		public List<string> inSignals = new List<string>();
	}
}
