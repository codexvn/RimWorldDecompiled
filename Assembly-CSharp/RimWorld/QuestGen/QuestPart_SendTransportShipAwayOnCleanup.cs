using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A24 RID: 14884
	public class QuestPart_SendTransportShipAwayOnCleanup : QuestPart
	{
		// Token: 0x06014623 RID: 83491 RVA: 0x00617436 File Offset: 0x00615636
		public override bool QuestPartReserves(TransportShip ship)
		{
			return ship == this.transportShip;
		}

		// Token: 0x06014624 RID: 83492 RVA: 0x00617441 File Offset: 0x00615641
		public override void Cleanup()
		{
			SendTransportShipAwayUtility.SendTransportShipAway(this.transportShip, this.unloadContents, this.unsatisfiedDropMode);
			this.transportShip = null;
		}

		// Token: 0x06014625 RID: 83493 RVA: 0x00617461 File Offset: 0x00615661
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.unloadContents, "unloadContents", false, false);
			Scribe_Values.Look<TransportShipDropMode>(ref this.unsatisfiedDropMode, "unsatisfiedDropMode", TransportShipDropMode.None, false);
			Scribe_References.Look<TransportShip>(ref this.transportShip, "transportShip", false);
		}

		// Token: 0x0400E123 RID: 57635
		public TransportShip transportShip;

		// Token: 0x0400E124 RID: 57636
		public TransportShipDropMode unsatisfiedDropMode;

		// Token: 0x0400E125 RID: 57637
		public bool unloadContents;
	}
}
