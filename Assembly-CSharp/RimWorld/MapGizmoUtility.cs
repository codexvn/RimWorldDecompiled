using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200212A RID: 8490
	[StaticConstructorOnStartup]
	public static class MapGizmoUtility
	{
		// Token: 0x17001C1A RID: 7194
		// (get) Token: 0x0600B91F RID: 47391 RVA: 0x00356FA0 File Offset: 0x003551A0
		public static Gizmo LastMouseOverGizmo
		{
			get
			{
				return MapGizmoUtility.lastMouseOverGizmo;
			}
		}

		// Token: 0x0600B920 RID: 47392 RVA: 0x00356FA8 File Offset: 0x003551A8
		public static void MapUIOnGUI()
		{
			if (Find.MainTabsRoot.OpenTab != null && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Inspect)
			{
				return;
			}
			MapGizmoUtility.tmpObjectsList.Clear();
			MapGizmoUtility.tmpObjectsList.AddRange(Find.Selector.SelectedObjects);
			GizmoGridDrawer.DrawGizmoGridFor(MapGizmoUtility.tmpObjectsList, out MapGizmoUtility.mouseoverGizmo);
		}

		// Token: 0x0600B921 RID: 47393 RVA: 0x00357000 File Offset: 0x00355200
		public static void MapUIUpdate()
		{
			MapGizmoUtility.lastMouseOverGizmo = MapGizmoUtility.mouseoverGizmo;
			if (MapGizmoUtility.mouseoverGizmo != null)
			{
				MapGizmoUtility.mouseoverGizmo.GizmoUpdateOnMouseover();
			}
		}

		// Token: 0x04007FBB RID: 32699
		private static Gizmo mouseoverGizmo;

		// Token: 0x04007FBC RID: 32700
		private static Gizmo lastMouseOverGizmo;

		// Token: 0x04007FBD RID: 32701
		private static int cacheFrame;

		// Token: 0x04007FBE RID: 32702
		private static readonly List<object> tmpObjectsList = new List<object>();
	}
}
