using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008B9 RID: 2233
	public static class GenView
	{
		// Token: 0x06003873 RID: 14451 RVA: 0x0012CD65 File Offset: 0x0012AF65
		public static bool ShouldSpawnMotesAt(this Vector3 loc, Map map, bool drawOffscreen = true)
		{
			return loc.ToIntVec3().ShouldSpawnMotesAt(map, drawOffscreen);
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x0012CD74 File Offset: 0x0012AF74
		public static bool ShouldSpawnMotesAt(this IntVec3 loc, Map map, bool drawOffscreen = true)
		{
			if (map != Find.CurrentMap)
			{
				return false;
			}
			if (!loc.InBounds(map))
			{
				return false;
			}
			if (drawOffscreen)
			{
				return true;
			}
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			return GenView.viewRect.Contains(loc);
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x0012CDC5 File Offset: 0x0012AFC5
		public static Vector3 RandomPositionOnOrNearScreen()
		{
			GenView.viewRect = Find.CameraDriver.CurrentViewRect;
			GenView.viewRect = GenView.viewRect.ExpandedBy(5);
			GenView.viewRect.ClipInsideMap(Find.CurrentMap);
			return GenView.viewRect.RandomVector3;
		}

		// Token: 0x040029CC RID: 10700
		private static CellRect viewRect;

		// Token: 0x040029CD RID: 10701
		private const int ViewRectMargin = 5;
	}
}
