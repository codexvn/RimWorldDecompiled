using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B12 RID: 15122
	public class QuestPart_MechRelay : QuestPart
	{
		// Token: 0x06014936 RID: 84278 RVA: 0x006238AB File Offset: 0x00621AAB
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				Thing thing = this.relay;
				if (thing == null)
				{
					return;
				}
				CompMechRelay compMechRelay = thing.TryGetComp<CompMechRelay>();
				if (compMechRelay == null)
				{
					return;
				}
				compMechRelay.Deactivate();
			}
		}

		// Token: 0x06014937 RID: 84279 RVA: 0x006238DA File Offset: 0x00621ADA
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Thing>(ref this.relay, "relay", false);
		}

		// Token: 0x0400E47D RID: 58493
		public string inSignal;

		// Token: 0x0400E47E RID: 58494
		public Thing relay;
	}
}
