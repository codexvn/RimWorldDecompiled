using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E7F RID: 15999
	public static class WorldObjectSelectionUtility
	{
		// Token: 0x06015E36 RID: 89654 RVA: 0x0068C301 File Offset: 0x0068A501
		public static IEnumerable<WorldObject> MultiSelectableWorldObjectsInScreenRectDistinct(Rect rect)
		{
			List<WorldObject> allObjects = Find.WorldObjects.AllWorldObjects;
			int num;
			for (int i = 0; i < allObjects.Count; i = num + 1)
			{
				if (!allObjects[i].NeverMultiSelect && !allObjects[i].HiddenBehindTerrainNow())
				{
					if (ExpandableWorldObjectsUtility.IsExpanded(allObjects[i]))
					{
						if (rect.Overlaps(ExpandableWorldObjectsUtility.ExpandedIconScreenRect(allObjects[i], 1f)))
						{
							yield return allObjects[i];
						}
					}
					else if (rect.Contains(allObjects[i].ScreenPos()))
					{
						yield return allObjects[i];
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06015E37 RID: 89655 RVA: 0x0068C314 File Offset: 0x0068A514
		public static bool HiddenBehindTerrainNow(this WorldObject o)
		{
			return WorldRendererUtility.HiddenBehindTerrainNow(o.DrawPos) || (!o.VisibleInBackground && o.Tile.Layer != PlanetLayer.Selected);
		}

		// Token: 0x06015E38 RID: 89656 RVA: 0x0068C352 File Offset: 0x0068A552
		public static Vector2 ScreenPos(this WorldObject o)
		{
			return GenWorldUI.WorldToUIPosition(o.DrawPos);
		}

		// Token: 0x06015E39 RID: 89657 RVA: 0x0068C360 File Offset: 0x0068A560
		public static bool VisibleToCameraNow(this WorldObject o)
		{
			if (!WorldRendererUtility.WorldSelected)
			{
				return false;
			}
			if (o.HiddenBehindTerrainNow())
			{
				return false;
			}
			Vector2 point = o.ScreenPos();
			return new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Contains(point);
		}

		// Token: 0x06015E3A RID: 89658 RVA: 0x0068C3AC File Offset: 0x0068A5AC
		public static float DistanceToMouse(this WorldObject o, Vector2 mousePos)
		{
			Ray ray = Find.WorldCamera.ScreenPointToRay(mousePos * Prefs.UIScale);
			int worldLayerMask = WorldCameraManager.WorldLayerMask;
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit, 1500f, worldLayerMask))
			{
				return Vector3.Distance(raycastHit.point, o.DrawPos);
			}
			return Vector3.Cross(ray.direction, o.DrawPos - ray.origin).magnitude;
		}
	}
}
