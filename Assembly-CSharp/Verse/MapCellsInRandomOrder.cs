using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D34 RID: 3380
	public class MapCellsInRandomOrder
	{
		// Token: 0x06005617 RID: 22039 RVA: 0x001BCAD8 File Offset: 0x001BACD8
		public MapCellsInRandomOrder(Map map)
		{
			this.map = map;
		}

		// Token: 0x06005618 RID: 22040 RVA: 0x001BCAE7 File Offset: 0x001BACE7
		public List<IntVec3> GetAll()
		{
			this.CreateListIfShould();
			return this.randomizedCells;
		}

		// Token: 0x06005619 RID: 22041 RVA: 0x001BCAF5 File Offset: 0x001BACF5
		public IntVec3 Get(int index)
		{
			this.CreateListIfShould();
			return this.randomizedCells[index];
		}

		// Token: 0x0600561A RID: 22042 RVA: 0x001BCB0C File Offset: 0x001BAD0C
		private void CreateListIfShould()
		{
			if (this.randomizedCells != null)
			{
				return;
			}
			this.randomizedCells = new List<IntVec3>(this.map.Area);
			foreach (IntVec3 item in this.map.AllCells)
			{
				this.randomizedCells.Add(item);
			}
			Rand.PushState();
			Rand.Seed = (Find.World.info.Seed ^ this.map.Tile.GetHashCode());
			this.randomizedCells.Shuffle<IntVec3>();
			Rand.PopState();
		}

		// Token: 0x04003E3B RID: 15931
		private Map map;

		// Token: 0x04003E3C RID: 15932
		private List<IntVec3> randomizedCells;
	}
}
