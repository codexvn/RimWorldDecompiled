using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C86 RID: 7302
	public class QuestPart_ReplaceLostLeaderReferences : QuestPart
	{
		// Token: 0x0600A2B7 RID: 41655 RVA: 0x002EC75C File Offset: 0x002EA95C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Pawn arg = signal.args.GetArg<Pawn>("SUBJECT");
				Pawn arg2 = signal.args.GetArg<Pawn>("NEWFACTIONLEADER");
				if (arg != null && arg2 != null)
				{
					List<QuestPart> partsListForReading = this.quest.PartsListForReading;
					for (int i = 0; i < partsListForReading.Count; i++)
					{
						partsListForReading[i].ReplacePawnReferences(arg, arg2);
					}
					if (arg.questTags != null)
					{
						if (arg2.questTags == null)
						{
							arg2.questTags = new List<string>();
						}
						arg2.questTags.AddRange(arg.questTags);
					}
				}
			}
		}

		// Token: 0x0600A2B8 RID: 41656 RVA: 0x002EC807 File Offset: 0x002EAA07
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x0600A2B9 RID: 41657 RVA: 0x002EC824 File Offset: 0x002EAA24
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
		}

		// Token: 0x0400708E RID: 28814
		public string inSignal;
	}
}
