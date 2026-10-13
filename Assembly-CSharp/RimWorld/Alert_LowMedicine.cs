using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FF4 RID: 12276
	public class Alert_LowMedicine : Alert
	{
		// Token: 0x06011496 RID: 70806 RVA: 0x004F40C3 File Offset: 0x004F22C3
		public Alert_LowMedicine()
		{
			this.defaultLabel = "LowMedicine".Translate();
			this.defaultExplanation = "NoMedicineDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06011497 RID: 70807 RVA: 0x004F40FC File Offset: 0x004F22FC
		public override TaggedString GetExplanation()
		{
			if (this.mapWithNoMedicine == null)
			{
				return string.Empty;
			}
			if (this.medicineCount == 0)
			{
				return this.defaultExplanation;
			}
			return "LowMedicineDesc".Translate(this.medicineCount);
		}

		// Token: 0x06011498 RID: 70808 RVA: 0x004F413A File Offset: 0x004F233A
		public override AlertReport GetReport()
		{
			if ((float)Find.TickManager.TicksGame < 150000f)
			{
				return false;
			}
			return this.MapWithLowMedicine() != null;
		}

		// Token: 0x06011499 RID: 70809 RVA: 0x004F4164 File Offset: 0x004F2364
		private Map MapWithLowMedicine()
		{
			this.mapWithNoMedicine = null;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome && map.mapPawns.AnyColonistSpawned && (float)this.MedicineCount(map) < 2f * (float)map.mapPawns.FreeColonistsSpawnedCount)
				{
					this.mapWithNoMedicine = map;
					break;
				}
			}
			return this.mapWithNoMedicine;
		}

		// Token: 0x0601149A RID: 70810 RVA: 0x004F41D7 File Offset: 0x004F23D7
		private int MedicineCount(Map map)
		{
			this.medicineCount = map.resourceCounter.GetCountIn(ThingRequestGroup.Medicine);
			return this.medicineCount;
		}

		// Token: 0x0400B338 RID: 45880
		private Map mapWithNoMedicine;

		// Token: 0x0400B339 RID: 45881
		private int medicineCount;

		// Token: 0x0400B33A RID: 45882
		private const float MedicinePerColonistThreshold = 2f;
	}
}
