using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CAA RID: 7338
	public class QuestPart_SetFactionHidden : QuestPart
	{
		// Token: 0x17001922 RID: 6434
		// (get) Token: 0x0600A373 RID: 41843 RVA: 0x002EE60B File Offset: 0x002EC80B
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction != null)
				{
					yield return this.faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A374 RID: 41844 RVA: 0x002EE61C File Offset: 0x002EC81C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null && this.faction.Hidden != this.hidden)
			{
				this.faction.hidden = new bool?(this.hidden);
			}
		}

		// Token: 0x0600A375 RID: 41845 RVA: 0x002EE674 File Offset: 0x002EC874
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x0600A376 RID: 41846 RVA: 0x002EE686 File Offset: 0x002EC886
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x040070EA RID: 28906
		public string inSignal;

		// Token: 0x040070EB RID: 28907
		public Faction faction;

		// Token: 0x040070EC RID: 28908
		public bool hidden;
	}
}
