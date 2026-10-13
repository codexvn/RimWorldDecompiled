using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020003A7 RID: 935
	public class AnimalPenConnectedDistrictsCalculator : AnimalPenEnclosureCalculator
	{
		// Token: 0x060017EC RID: 6124 RVA: 0x00079981 File Offset: 0x00077B81
		protected override void VisitDirectlyConnectedRegion(Region r)
		{
			this.AddDistrict(r);
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x00079981 File Offset: 0x00077B81
		protected override void VisitIndirectlyDirectlyConnectedRegion(Region r)
		{
			this.AddDistrict(r);
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x00079981 File Offset: 0x00077B81
		protected override void VisitPassableDoorway(Region r)
		{
			this.AddDistrict(r);
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x0007998C File Offset: 0x00077B8C
		private void AddDistrict(Region r)
		{
			District district = r.District;
			if (!this.districtsTmp.Contains(district))
			{
				this.districtsTmp.Add(district);
			}
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x000799BA File Offset: 0x00077BBA
		public static void InvalidateDistrictCache(District district)
		{
			AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCached.Remove(district);
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x000799C8 File Offset: 0x00077BC8
		public List<District> CalculateConnectedDistricts(IntVec3 position, Map map)
		{
			District district = position.GetDistrict(map, RegionType.Set_Passable);
			if (Find.TickManager.TicksGame == AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCachedTick)
			{
				if (AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCached.ContainsKey(district))
				{
					return AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCached[district];
				}
			}
			else
			{
				AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCached.Clear();
				AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCachedTick = Find.TickManager.TicksGame;
			}
			this.districtsTmp.Clear();
			if (!base.VisitPen(position, map))
			{
				this.districtsTmp.Clear();
			}
			AnimalPenConnectedDistrictsCalculator.connectedDistrictsForRootCached[district] = this.districtsTmp.ToList<District>();
			return this.districtsTmp;
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x00079A5E File Offset: 0x00077C5E
		public void Reset()
		{
			this.districtsTmp.Clear();
		}

		// Token: 0x04001575 RID: 5493
		private readonly List<District> districtsTmp = new List<District>();

		// Token: 0x04001576 RID: 5494
		private static readonly Dictionary<District, List<District>> connectedDistrictsForRootCached = new Dictionary<District, List<District>>();

		// Token: 0x04001577 RID: 5495
		private static int connectedDistrictsForRootCachedTick = -1;
	}
}
