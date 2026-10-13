using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A1B RID: 14875
	public class QuestPart_AddShipJob : QuestPart
	{
		// Token: 0x06014604 RID: 83460 RVA: 0x00616E92 File Offset: 0x00615092
		public virtual ShipJob GetShipJob()
		{
			return this.shipJob ?? ShipJobMaker.MakeShipJob(this.shipJobDef);
		}

		// Token: 0x06014605 RID: 83461 RVA: 0x00616EA9 File Offset: 0x006150A9
		public override bool QuestPartReserves(TransportShip ship)
		{
			return ship == this.transportShip;
		}

		// Token: 0x06014606 RID: 83462 RVA: 0x00616EB4 File Offset: 0x006150B4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				switch (this.shipJobStartMode)
				{
				case ShipJobStartMode.Queue:
					this.transportShip.AddJob(this.GetShipJob());
					break;
				case ShipJobStartMode.Instant:
					this.transportShip.AddJob(this.GetShipJob());
					this.transportShip.TryGetNextJob();
					break;
				case ShipJobStartMode.Force:
					this.transportShip.ForceJob(this.GetShipJob());
					break;
				case ShipJobStartMode.Force_DelayCurrent:
					this.transportShip.ForceJob_DelayCurrent(this.GetShipJob());
					break;
				}
				this.shipJob = null;
			}
		}

		// Token: 0x06014607 RID: 83463 RVA: 0x00616F4F File Offset: 0x0061514F
		public override void Cleanup()
		{
			base.Cleanup();
			this.transportShip = null;
		}

		// Token: 0x06014608 RID: 83464 RVA: 0x00616F60 File Offset: 0x00615160
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<ShipJobStartMode>(ref this.shipJobStartMode, "shipJobStartMode", ShipJobStartMode.Queue, false);
			Scribe_Deep.Look<ShipJob>(ref this.shipJob, "shipJob", Array.Empty<object>());
			Scribe_Defs.Look<ShipJobDef>(ref this.shipJobDef, "shipJobDef");
			Scribe_References.Look<TransportShip>(ref this.transportShip, "transportShip", false);
		}

		// Token: 0x0400E10C RID: 57612
		public string inSignal;

		// Token: 0x0400E10D RID: 57613
		public ShipJobDef shipJobDef;

		// Token: 0x0400E10E RID: 57614
		public TransportShip transportShip;

		// Token: 0x0400E10F RID: 57615
		public ShipJobStartMode shipJobStartMode;

		// Token: 0x0400E110 RID: 57616
		public ShipJob shipJob;
	}
}
