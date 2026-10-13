using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C1A RID: 15386
	public class WorldPathPool
	{
		// Token: 0x170033A2 RID: 13218
		// (get) Token: 0x06014DE8 RID: 85480 RVA: 0x0064010A File Offset: 0x0063E30A
		public static WorldPath NotFoundPath { get; } = WorldPath.NewNotFound();

		// Token: 0x06014DEA RID: 85482 RVA: 0x00640120 File Offset: 0x0063E320
		public WorldPath GetEmptyWorldPath()
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				if (!this.paths[i].inUse)
				{
					this.paths[i].inUse = true;
					return this.paths[i];
				}
			}
			if (this.paths.Count > Find.WorldObjects.CaravansCount + 2 + (Find.WorldObjects.RoutePlannerWaypointsCount - 1))
			{
				Log.ErrorOnce("WorldPathPool leak: more paths than caravans. Force-recovering.", 664788);
				this.paths.Clear();
			}
			WorldPath worldPath = new WorldPath();
			this.paths.Add(worldPath);
			worldPath.inUse = true;
			return worldPath;
		}

		// Token: 0x0400E89C RID: 59548
		private readonly List<WorldPath> paths = new List<WorldPath>(64);
	}
}
