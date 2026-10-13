using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020008B7 RID: 2231
	public class LinkGrid
	{
		// Token: 0x0600386A RID: 14442 RVA: 0x0012CAD5 File Offset: 0x0012ACD5
		public LinkGrid(Map map)
		{
			this.map = map;
			this.linkGrid = new LinkFlags[map.cellIndices.NumGridCells];
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x0012CAFA File Offset: 0x0012ACFA
		public LinkFlags LinkFlagsAt(IntVec3 c)
		{
			return this.linkGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x0012CB14 File Offset: 0x0012AD14
		public void Notify_LinkerCreatedOrDestroyed(Thing linker)
		{
			CellIndices cellIndices = this.map.cellIndices;
			foreach (IntVec3 c in linker.OccupiedRect())
			{
				LinkFlags linkFlags = LinkFlags.None;
				List<Thing> list = this.map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def.graphicData != null)
					{
						linkFlags |= list[i].def.graphicData.linkFlags;
					}
				}
				this.linkGrid[cellIndices.CellToIndex(c)] = linkFlags;
			}
		}

		// Token: 0x040029C5 RID: 10693
		private Map map;

		// Token: 0x040029C6 RID: 10694
		private LinkFlags[] linkGrid;
	}
}
