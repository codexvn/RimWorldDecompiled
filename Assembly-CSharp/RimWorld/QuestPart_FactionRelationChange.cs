using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C47 RID: 7239
	public class QuestPart_FactionRelationChange : QuestPart
	{
		// Token: 0x0600A124 RID: 41252 RVA: 0x002E5BD8 File Offset: 0x002E3DD8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null && this.faction != Faction.OfPlayer)
			{
				this.faction.SetRelationDirect(Faction.OfPlayer, this.relationKind, this.canSendHostilityLetter, null, null);
			}
		}

		// Token: 0x0600A125 RID: 41253 RVA: 0x002E5C3A File Offset: 0x002E3E3A
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x0600A126 RID: 41254 RVA: 0x002E5C4C File Offset: 0x002E3E4C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<FactionRelationKind>(ref this.relationKind, "relationKind", FactionRelationKind.Hostile, false);
			Scribe_Values.Look<bool>(ref this.canSendHostilityLetter, "canSendHostilityLetter", false, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x04006F62 RID: 28514
		public string inSignal;

		// Token: 0x04006F63 RID: 28515
		public Faction faction;

		// Token: 0x04006F64 RID: 28516
		public FactionRelationKind relationKind;

		// Token: 0x04006F65 RID: 28517
		public bool canSendHostilityLetter;
	}
}
