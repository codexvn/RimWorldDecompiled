using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200043C RID: 1084
	public sealed class ThingGrid
	{
		// Token: 0x06001BFE RID: 7166 RVA: 0x0008D0A8 File Offset: 0x0008B2A8
		public ThingGrid(Map map)
		{
			this.map = map;
			CellIndices cellIndices = map.cellIndices;
			this.thingGrid = new List<Thing>[cellIndices.NumGridCells];
			for (int i = 0; i < cellIndices.NumGridCells; i++)
			{
				this.thingGrid[i] = new List<Thing>(4);
			}
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x0008D0FC File Offset: 0x0008B2FC
		public void Register(Thing t)
		{
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				this.RegisterInCell(t, t.Position);
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.RegisterInCell(t, new IntVec3(j, 0, i));
				}
			}
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x0008D180 File Offset: 0x0008B380
		private void RegisterInCell(Thing t, IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				string str = (t != null) ? t.ToString() : null;
				string str2 = " tried to register out of bounds at ";
				IntVec3 intVec = c;
				Log.Warning(str + str2 + intVec.ToString() + ". Destroying.");
				t.Destroy(DestroyMode.Vanish);
				return;
			}
			this.thingGrid[this.map.cellIndices.CellToIndex(c)].Add(t);
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x0008D1F4 File Offset: 0x0008B3F4
		public void Deregister(Thing t, bool doEvenIfDespawned = false)
		{
			if (!t.Spawned && !doEvenIfDespawned)
			{
				return;
			}
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				this.DeregisterInCell(t, t.Position);
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.DeregisterInCell(t, new IntVec3(j, 0, i));
				}
			}
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x0008D284 File Offset: 0x0008B484
		private void DeregisterInCell(Thing t, IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				string str = (t != null) ? t.ToString() : null;
				string str2 = " tried to de-register out of bounds at ";
				IntVec3 intVec = c;
				Log.Error(str + str2 + intVec.ToString());
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (this.thingGrid[num].Contains(t))
			{
				this.thingGrid[num].Remove(t);
			}
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x0008D2FC File Offset: 0x0008B4FC
		public IEnumerable<Thing> ThingsAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				yield break;
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			int num;
			for (int i = 0; i < list.Count; i = num + 1)
			{
				yield return list[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x0008D314 File Offset: 0x0008B514
		public List<Thing> ThingsListAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				string str = "Got ThingsListAt out of bounds: ";
				IntVec3 intVec = c;
				Log.ErrorOnce(str + intVec.ToString(), 495287);
				return ThingGrid.EmptyThingList;
			}
			return this.thingGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x0008D370 File Offset: 0x0008B570
		public List<Thing> ThingsListAtFast(IntVec3 c)
		{
			return this.thingGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x0008D38A File Offset: 0x0008B58A
		public List<Thing> ThingsListAtFast(int index)
		{
			return this.thingGrid[index];
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x0008D394 File Offset: 0x0008B594
		public bool CellContains(IntVec3 c, ThingCategory cat)
		{
			return this.ThingAt(c, cat) != null;
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x0008D3A4 File Offset: 0x0008B5A4
		public Thing ThingAt(IntVec3 c, ThingCategory cat)
		{
			if (!c.InBounds(this.map))
			{
				return null;
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == cat)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x0008D408 File Offset: 0x0008B608
		public bool CellContains(IntVec3 c, ThingDef def)
		{
			return this.ThingAt(c, def) != null;
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x0008D418 File Offset: 0x0008B618
		public Thing ThingAt(IntVec3 c, ThingDef def)
		{
			if (!c.InBounds(this.map))
			{
				return null;
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def == def)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x0008D478 File Offset: 0x0008B678
		public T ThingAt<T>(IntVec3 c) where T : Thing
		{
			if (!c.InBounds(this.map))
			{
				return default(T);
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				T t = list[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x04001817 RID: 6167
		private Map map;

		// Token: 0x04001818 RID: 6168
		private List<Thing>[] thingGrid;

		// Token: 0x04001819 RID: 6169
		private static readonly List<Thing> EmptyThingList = new List<Thing>();
	}
}
