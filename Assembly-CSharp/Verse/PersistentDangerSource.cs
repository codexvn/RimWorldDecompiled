using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020005D6 RID: 1494
	public class PersistentDangerSource : SimpleBoolPathFinderDataSource
	{
		// Token: 0x06002658 RID: 9816 RVA: 0x000CA662 File Offset: 0x000C8862
		public PersistentDangerSource(Map map) : base(map)
		{
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x000CAA28 File Offset: 0x000C8C28
		public override void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.data.Clear();
			for (int i = 0; i < this.cellCount; i++)
			{
				IntVec3 c = this.map.cellIndices.IndexToCell(i);
				TerrainDef terrain = c.GetTerrain(this.map);
				List<Thing> thingList = c.GetThingList(this.map);
				bool flag = false;
				for (int j = 0; j < thingList.Count; j++)
				{
					if (thingList[j].def.pathfinderDangerous && !(thingList[j] is Pawn))
					{
						flag = true;
						break;
					}
				}
				this.data.Set(i, terrain.dangerous || flag);
			}
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x000CAAD4 File Offset: 0x000C8CD4
		public override bool UpdateIncrementally(IEnumerable<PathRequest> requests, List<IntVec3> cellDeltas)
		{
			foreach (IntVec3 c in cellDeltas)
			{
				int pos = this.map.cellIndices.CellToIndex(c);
				List<Thing> thingList = c.GetThingList(this.map);
				bool flag = false;
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i].def.pathfinderDangerous && !(thingList[i] is Pawn))
					{
						flag = true;
						break;
					}
				}
				this.data.Set(pos, flag || c.GetTerrain(this.map).dangerous);
			}
			return false;
		}
	}
}
