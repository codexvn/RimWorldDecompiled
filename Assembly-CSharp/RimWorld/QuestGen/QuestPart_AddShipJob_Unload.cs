using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A1D RID: 14877
	public class QuestPart_AddShipJob_Unload : QuestPart_AddShipJob
	{
		// Token: 0x0601460F RID: 83471 RVA: 0x00617104 File Offset: 0x00615304
		public override ShipJob GetShipJob()
		{
			ShipJob_Unload shipJob_Unload = (ShipJob_Unload)ShipJobMaker.MakeShipJob(this.shipJobDef);
			shipJob_Unload.unforbidAll = this.unforbidAll;
			return shipJob_Unload;
		}

		// Token: 0x06014610 RID: 83472 RVA: 0x00617122 File Offset: 0x00615322
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.unforbidAll, "unforbidAll", false, false);
		}

		// Token: 0x0400E115 RID: 57621
		public bool unforbidAll = true;
	}
}
