using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200055B RID: 1371
	public sealed class ZoneManager : IExposable
	{
		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x060023A0 RID: 9120 RVA: 0x000B8730 File Offset: 0x000B6930
		public List<Zone> AllZones
		{
			get
			{
				return this.allZones;
			}
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x000B8738 File Offset: 0x000B6938
		public ZoneManager(Map map)
		{
			this.map = map;
			this.zoneGrid = new Zone[map.cellIndices.NumGridCells];
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x000B8768 File Offset: 0x000B6968
		public void ExposeData()
		{
			Scribe_Collections.Look<Zone>(ref this.allZones, "allZones", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateZoneManagerLinks();
				this.RebuildZoneGrid();
			}
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x000B8794 File Offset: 0x000B6994
		private void UpdateZoneManagerLinks()
		{
			for (int i = 0; i < this.allZones.Count; i++)
			{
				this.allZones[i].zoneManager = this;
			}
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x000B87CC File Offset: 0x000B69CC
		private void RebuildZoneGrid()
		{
			CellIndices cellIndices = this.map.cellIndices;
			this.zoneGrid = new Zone[cellIndices.NumGridCells];
			foreach (Zone zone in this.allZones)
			{
				foreach (IntVec3 c in zone)
				{
					this.zoneGrid[cellIndices.CellToIndex(c)] = zone;
				}
			}
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x000B8878 File Offset: 0x000B6A78
		public void RegisterZone(Zone newZone)
		{
			this.allZones.Add(newZone);
			newZone.PostRegister();
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x000B888C File Offset: 0x000B6A8C
		public void DeregisterZone(Zone oldZone)
		{
			this.allZones.Remove(oldZone);
			oldZone.PostDeregister();
			if (Find.Selector.SelectedZone == oldZone)
			{
				Find.Selector.ClearSelection();
			}
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x000B88B8 File Offset: 0x000B6AB8
		internal void AddZoneGridCell(Zone zone, IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = zone;
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x000B88D3 File Offset: 0x000B6AD3
		internal void ClearZoneGridCell(IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = null;
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x000B88EE File Offset: 0x000B6AEE
		public Zone ZoneAt(IntVec3 c)
		{
			return this.zoneGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x000B8908 File Offset: 0x000B6B08
		public string NewZoneName(string nameBase)
		{
			for (int i = 1; i <= 1000; i++)
			{
				string cand = nameBase + " " + i.ToString();
				if (!this.allZones.Any((Zone z) => z.label == cand))
				{
					return cand;
				}
			}
			Log.Error("Ran out of zone names.");
			return "Zone X";
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x000B8974 File Offset: 0x000B6B74
		internal void Notify_NoZoneOverlapThingSpawned(Thing thing)
		{
			CellRect cellRect = thing.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					Zone zone = this.ZoneAt(c);
					if (zone != null)
					{
						zone.RemoveCell(c);
						zone.CheckContiguous();
					}
				}
			}
		}

		// Token: 0x04001D96 RID: 7574
		public readonly Map map;

		// Token: 0x04001D97 RID: 7575
		private List<Zone> allZones = new List<Zone>();

		// Token: 0x04001D98 RID: 7576
		private Zone[] zoneGrid;
	}
}
