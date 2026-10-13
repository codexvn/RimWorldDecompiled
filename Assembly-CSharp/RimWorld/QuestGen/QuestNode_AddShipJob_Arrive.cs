using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A14 RID: 14868
	public class QuestNode_AddShipJob_Arrive : QuestNode_AddShipJob
	{
		// Token: 0x17003345 RID: 13125
		// (get) Token: 0x060145EB RID: 83435 RVA: 0x00616A1E File Offset: 0x00614C1E
		protected override ShipJobDef DefaultShipJobDef
		{
			get
			{
				return ShipJobDefOf.Arrive;
			}
		}

		// Token: 0x060145EC RID: 83436 RVA: 0x00616A28 File Offset: 0x00614C28
		protected override void AddJobVars(ShipJob shipJob, Slate slate)
		{
			ShipJob_Arrive shipJob_Arrive = shipJob as ShipJob_Arrive;
			if (shipJob_Arrive != null)
			{
				Map map = this.map.GetValue(slate) ?? slate.Get<Map>("map", null, false);
				shipJob_Arrive.mapParent = map.Parent;
				IntVec3? value = this.landingCell.GetValue(slate);
				IntVec3? intVec = (value != null) ? value : slate.Get<IntVec3?>("landingCell", null, false);
				if (intVec != null)
				{
					shipJob_Arrive.cell = intVec.Value;
				}
			}
		}

		// Token: 0x0400E0FA RID: 57594
		public SlateRef<Map> map;

		// Token: 0x0400E0FB RID: 57595
		public SlateRef<IntVec3?> landingCell;
	}
}
