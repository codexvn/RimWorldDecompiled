using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004F4 RID: 1268
	public class RegionDirtyer
	{
		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x060020E2 RID: 8418 RVA: 0x000AC57F File Offset: 0x000AA77F
		public bool AnyDirty
		{
			get
			{
				return this.dirtyCells.Count > 0;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x060020E3 RID: 8419 RVA: 0x000AC58F File Offset: 0x000AA78F
		public HashSet<IntVec3> DirtyCells
		{
			get
			{
				return this.dirtyCells;
			}
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x000AC597 File Offset: 0x000AA797
		public RegionDirtyer(Map map)
		{
			this.map = map;
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x000AC5BC File Offset: 0x000AA7BC
		internal void Notify_WalkabilityChanged(IntVec3 c, bool newWalkability)
		{
			this.regionsToDirty.Clear();
			for (int i = 0; i < 9; i++)
			{
				IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
				if (c2.InBounds(this.map))
				{
					Region regionAt_NoRebuild_InvalidAllowed = this.map.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(c2);
					if (regionAt_NoRebuild_InvalidAllowed != null && regionAt_NoRebuild_InvalidAllowed.valid)
					{
						this.map.TemperatureVacuumCache.TryCacheRegionTempInfo(c2, regionAt_NoRebuild_InvalidAllowed);
						this.regionsToDirty.Add(regionAt_NoRebuild_InvalidAllowed);
					}
				}
			}
			for (int j = 0; j < this.regionsToDirty.Count; j++)
			{
				this.SetRegionDirty(this.regionsToDirty[j], true);
			}
			this.regionsToDirty.Clear();
			if (newWalkability)
			{
				this.dirtyCells.Add(c);
			}
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x000AC680 File Offset: 0x000AA880
		internal void Notify_ThingAffectingRegionsSpawned(Thing b)
		{
			this.DirtyRegionForThing(b);
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x000AC680 File Offset: 0x000AA880
		internal void Notify_ThingAffectingRegionsDespawned(Thing b)
		{
			this.DirtyRegionForThing(b);
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x000AC68C File Offset: 0x000AA88C
		internal void DirtyRegionForThing(Thing b)
		{
			this.regionsToDirty.Clear();
			foreach (IntVec3 c in b.OccupiedRect().ExpandedBy(1).ClipInsideMap(b.Map))
			{
				Region validRegionAt_NoRebuild = b.Map.regionGrid.GetValidRegionAt_NoRebuild(c);
				if (validRegionAt_NoRebuild != null)
				{
					b.Map.TemperatureVacuumCache.TryCacheRegionTempInfo(c, validRegionAt_NoRebuild);
					this.regionsToDirty.Add(validRegionAt_NoRebuild);
				}
			}
			for (int i = 0; i < this.regionsToDirty.Count; i++)
			{
				this.SetRegionDirty(this.regionsToDirty[i], true);
			}
			this.regionsToDirty.Clear();
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x000AC770 File Offset: 0x000AA970
		internal void SetAllClean()
		{
			foreach (IntVec3 c in this.dirtyCells)
			{
				this.map.TemperatureVacuumCache.ResetCachedCellInfo(c);
			}
			this.dirtyCells.Clear();
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x000AC7D8 File Offset: 0x000AA9D8
		private void SetRegionDirty(Region reg, bool addCellsToDirtyCells = true)
		{
			if (!reg.valid)
			{
				return;
			}
			reg.valid = false;
			reg.District = null;
			for (int i = 0; i < reg.links.Count; i++)
			{
				reg.links[i].Deregister(reg);
			}
			reg.links.Clear();
			reg.ListerThings.Clear();
			this.map.GetComponent<VacuumComponent>().SetDrawerDirty();
			if (addCellsToDirtyCells)
			{
				foreach (IntVec3 intVec in reg.Cells)
				{
					this.dirtyCells.Add(intVec);
					if (DebugViewSettings.drawRegionDirties)
					{
						this.map.debugDrawer.FlashCell(intVec, 0f, null, 50);
					}
				}
			}
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000AC8B4 File Offset: 0x000AAAB4
		internal void SetAllDirty()
		{
			this.dirtyCells.Clear();
			foreach (IntVec3 item in this.map)
			{
				this.dirtyCells.Add(item);
			}
			foreach (Region reg in this.map.regionGrid.AllRegions_NoRebuild_InvalidAllowed)
			{
				this.SetRegionDirty(reg, false);
			}
		}

		// Token: 0x04001BFD RID: 7165
		private Map map;

		// Token: 0x04001BFE RID: 7166
		private HashSet<IntVec3> dirtyCells = new HashSet<IntVec3>();

		// Token: 0x04001BFF RID: 7167
		private List<Region> regionsToDirty = new List<Region>();
	}
}
