using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020029F7 RID: 10743
	public static class DoorsDebugDrawer
	{
		// Token: 0x0600ED52 RID: 60754 RVA: 0x0044E01C File Offset: 0x0044C21C
		public static void DrawDebug()
		{
			if (!DebugViewSettings.drawDoorsDebug)
			{
				return;
			}
			CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
			List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
			for (int i = 0; i < list.Count; i++)
			{
				if (currentViewRect.Contains(list[i].Position))
				{
					Building_Door building_Door = list[i] as Building_Door;
					if (building_Door != null)
					{
						Color col;
						if (building_Door.FreePassage)
						{
							col = new Color(0f, 1f, 0f, 0.5f);
						}
						else
						{
							col = new Color(1f, 0f, 0f, 0.5f);
						}
						foreach (IntVec3 c in building_Door.OccupiedRect())
						{
							CellRenderer.RenderCell(c, SolidColorMaterials.SimpleSolidColorMaterial(col, false));
						}
					}
				}
			}
		}
	}
}
