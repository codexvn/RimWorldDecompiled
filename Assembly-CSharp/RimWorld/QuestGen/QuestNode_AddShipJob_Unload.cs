using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A16 RID: 14870
	public class QuestNode_AddShipJob_Unload : QuestNode_AddShipJob
	{
		// Token: 0x17003346 RID: 13126
		// (get) Token: 0x060145F0 RID: 83440 RVA: 0x00616B0F File Offset: 0x00614D0F
		protected override ShipJobDef DefaultShipJobDef
		{
			get
			{
				return ShipJobDefOf.Unload;
			}
		}

		// Token: 0x060145F1 RID: 83441 RVA: 0x00616B18 File Offset: 0x00614D18
		protected override void AddJobVars(ShipJob shipJob, Slate slate)
		{
			ShipJob_Unload shipJob_Unload = shipJob as ShipJob_Unload;
			if (shipJob_Unload != null)
			{
				shipJob_Unload.dropMode = (this.dropMode.GetValue(slate) ?? TransportShipDropMode.All);
			}
		}

		// Token: 0x0400E0FF RID: 57599
		public SlateRef<TransportShipDropMode?> dropMode;
	}
}
