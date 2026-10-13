using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200267B RID: 9851
	public class PreceptRequirement_Altar : PreceptRequirement
	{
		// Token: 0x0600D84A RID: 55370 RVA: 0x004010AC File Offset: 0x003FF2AC
		public override bool Met(List<Precept> precepts)
		{
			for (int i = 0; i < precepts.Count; i++)
			{
				Precept_Building precept_Building = precepts[i] as Precept_Building;
				if (precept_Building != null && precept_Building.ThingDef.isAltar)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D84B RID: 55371 RVA: 0x004010EC File Offset: 0x003FF2EC
		public override Precept MakePrecept(Ideo ideo)
		{
			PreceptDef ideoBuilding = PreceptDefOf.IdeoBuilding;
			Precept_Building precept_Building = (Precept_Building)PreceptMaker.MakePrecept(ideoBuilding);
			precept_Building.ideo = ideo;
			precept_Building.ThingDef = (from b in ideoBuilding.Worker.ThingDefsForIdeo(ideo, null)
			where b.def.isAltar
			select b).RandomElement<PreceptThingChance>().def;
			return precept_Building;
		}
	}
}
