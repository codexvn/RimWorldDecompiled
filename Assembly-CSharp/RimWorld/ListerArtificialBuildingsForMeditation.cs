using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F68 RID: 8040
	public class ListerArtificialBuildingsForMeditation
	{
		// Token: 0x0600B1A6 RID: 45478 RVA: 0x0032C6BA File Offset: 0x0032A8BA
		public ListerArtificialBuildingsForMeditation(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600B1A7 RID: 45479 RVA: 0x0032C6D4 File Offset: 0x0032A8D4
		public void Notify_BuildingSpawned(Building b)
		{
			if (MeditationUtility.CountsAsArtificialBuilding(b))
			{
				this.artificialBuildingsPerCell.Clear();
			}
		}

		// Token: 0x0600B1A8 RID: 45480 RVA: 0x0032C6D4 File Offset: 0x0032A8D4
		public void Notify_BuildingDeSpawned(Building b)
		{
			if (MeditationUtility.CountsAsArtificialBuilding(b))
			{
				this.artificialBuildingsPerCell.Clear();
			}
		}

		// Token: 0x0600B1A9 RID: 45481 RVA: 0x0032C6EC File Offset: 0x0032A8EC
		public List<Thing> GetForCell(IntVec3 cell, float radius)
		{
			CellWithRadius key = new CellWithRadius(cell, radius);
			List<Thing> list;
			if (!this.artificialBuildingsPerCell.TryGetValue(key, out list))
			{
				list = new List<Thing>();
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(cell, this.map, radius, false))
				{
					if (MeditationUtility.CountsAsArtificialBuilding(thing))
					{
						list.Add(thing);
					}
				}
				this.artificialBuildingsPerCell[key] = list;
			}
			return list;
		}

		// Token: 0x04007988 RID: 31112
		private Map map;

		// Token: 0x04007989 RID: 31113
		private Dictionary<CellWithRadius, List<Thing>> artificialBuildingsPerCell = new Dictionary<CellWithRadius, List<Thing>>();
	}
}
