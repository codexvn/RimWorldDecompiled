using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020005D3 RID: 1491
	public class FenceSource : SimpleBoolPathFinderDataSource
	{
		// Token: 0x0600264C RID: 9804 RVA: 0x000CA662 File Offset: 0x000C8862
		public FenceSource(Map map) : base(map)
		{
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x000CA66C File Offset: 0x000C886C
		public override void ComputeAll(IEnumerable<PathRequest> _)
		{
			this.data.Clear();
			CellIndices cellIndices = this.map.cellIndices;
			foreach (Thing thing in ((IEnumerable<Thing>)this.map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial)))
			{
				if (thing.def.IsFence)
				{
					int pos = cellIndices.CellToIndex(thing.Position);
					this.data.Set(pos, true);
				}
			}
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x000CA700 File Offset: 0x000C8900
		public override bool UpdateIncrementally(IEnumerable<PathRequest> _, List<IntVec3> cellDeltas)
		{
			CellIndices cellIndices = this.map.cellIndices;
			Building[] innerArray = this.map.edificeGrid.InnerArray;
			foreach (IntVec3 c in cellDeltas)
			{
				int num = cellIndices.CellToIndex(c);
				int pos = num;
				Building building = innerArray[num];
				this.data.Set(pos, building != null && building.def.IsFence);
			}
			return false;
		}
	}
}
