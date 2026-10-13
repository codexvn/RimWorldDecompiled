using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B92 RID: 7058
	public class QuestPart_PassWithFactionArg : QuestPart_Pass
	{
		// Token: 0x06009D8B RID: 40331 RVA: 0x002DA8FC File Offset: 0x002D8AFC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				SignalArgs args = new SignalArgs(signal.args);
				if (this.outSignalOutcomeArg != null)
				{
					args.Add(this.outSignalOutcomeArg.Value.Named("OUTCOME"));
				}
				args.Add(this.faction.Named("FACTION"));
				Find.SignalManager.SendSignal(new Signal(this.outSignal, args, false));
			}
		}

		// Token: 0x06009D8C RID: 40332 RVA: 0x002DA985 File Offset: 0x002D8B85
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06009D8D RID: 40333 RVA: 0x002DA997 File Offset: 0x002D8B97
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x04006D2E RID: 27950
		public Faction faction;
	}
}
