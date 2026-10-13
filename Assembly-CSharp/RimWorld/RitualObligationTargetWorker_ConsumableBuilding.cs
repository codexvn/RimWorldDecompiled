using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02002712 RID: 10002
	public class RitualObligationTargetWorker_ConsumableBuilding : RitualObligationTargetWorker_ThingDef
	{
		// Token: 0x0600DB83 RID: 56195 RVA: 0x0040B609 File Offset: 0x00409809
		public RitualObligationTargetWorker_ConsumableBuilding()
		{
		}

		// Token: 0x0600DB84 RID: 56196 RVA: 0x0040B611 File Offset: 0x00409811
		public RitualObligationTargetWorker_ConsumableBuilding(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DB85 RID: 56197 RVA: 0x0040C184 File Offset: 0x0040A384
		public override List<string> MissingTargetBuilding(Ideo ideo)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.def.thingDefs.Count; i++)
			{
				if (ideo.HasPreceptForBuilding(this.def.thingDefs[i]))
				{
					return base.MissingTargetBuilding(ideo);
				}
				list.Add(this.def.thingDefs[i].label);
			}
			return list;
		}
	}
}
