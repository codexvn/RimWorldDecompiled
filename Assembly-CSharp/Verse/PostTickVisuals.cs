using System;

namespace Verse
{
	// Token: 0x020004DC RID: 1244
	public class PostTickVisuals
	{
		// Token: 0x0600201A RID: 8218 RVA: 0x000A7E64 File Offset: 0x000A6064
		public PostTickVisuals(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x000A7E74 File Offset: 0x000A6074
		public void ProcessPostTickVisuals()
		{
			int ticksThisFrame = Find.TickManager.TicksThisFrame;
			if (ticksThisFrame > 0)
			{
				CellRect viewRect = Find.CameraDriver.CurrentViewRect.ExpandedBy(3);
				foreach (Pawn pawn in this.map.mapPawns.AllPawnsSpawned)
				{
					pawn.ProcessPostTickVisuals(ticksThisFrame, viewRect);
				}
			}
		}

		// Token: 0x04001B76 RID: 7030
		private Map map;
	}
}
