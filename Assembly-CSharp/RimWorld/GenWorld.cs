using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003658 RID: 13912
	public static class GenWorld
	{
		// Token: 0x06013510 RID: 79120 RVA: 0x005BDA78 File Offset: 0x005BBC78
		public static PlanetTile MouseTile(bool snapToExpandableWorldObjects = false)
		{
			PlanetLayer selected = PlanetLayer.Selected;
			if (snapToExpandableWorldObjects)
			{
				if (GenWorld.cachedFrame_snap == Time.frameCount && GenWorld.cachedLayer == selected)
				{
					return GenWorld.cachedTile_snap;
				}
				GenWorld.cachedTile_snap = GenWorld.TileAt(UI.MousePositionOnUI, true);
				GenWorld.cachedFrame_snap = Time.frameCount;
				GenWorld.cachedLayer = selected;
				return GenWorld.cachedTile_snap;
			}
			else
			{
				if (GenWorld.cachedFrame_noSnap == Time.frameCount && GenWorld.cachedLayer == selected)
				{
					return GenWorld.cachedTile_noSnap;
				}
				GenWorld.cachedTile_noSnap = GenWorld.TileAt(UI.MousePositionOnUI, false);
				GenWorld.cachedFrame_noSnap = Time.frameCount;
				GenWorld.cachedLayer = selected;
				return GenWorld.cachedTile_noSnap;
			}
		}

		// Token: 0x06013511 RID: 79121 RVA: 0x005BDB10 File Offset: 0x005BBD10
		public static PlanetTile TileAt(Vector2 clickPos, bool snapToExpandableWorldObjects = false)
		{
			Camera worldCamera = Find.WorldCamera;
			if (!worldCamera.gameObject.activeInHierarchy)
			{
				return PlanetTile.Invalid;
			}
			if (snapToExpandableWorldObjects)
			{
				ExpandableWorldObjectsUtility.GetExpandedWorldObjectUnderMouse(UI.MousePositionOnUI, GenWorld.tmpWorldObjectsUnderMouse);
				if (GenWorld.tmpWorldObjectsUnderMouse.Any<WorldObject>())
				{
					PlanetTile tile = GenWorld.tmpWorldObjectsUnderMouse[0].Tile;
					GenWorld.tmpWorldObjectsUnderMouse.Clear();
					return tile;
				}
			}
			Ray ray = worldCamera.ScreenPointToRay(clickPos * Prefs.UIScale);
			int worldLayerMask = WorldCameraManager.WorldLayerMask;
			WorldTerrainColliderManager.EnsureRaycastCollidersUpdated();
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 1500f, worldLayerMask))
			{
				return Find.World.renderer.GetTileFromRayHit(hit);
			}
			return PlanetTile.Invalid;
		}

		// Token: 0x06013512 RID: 79122 RVA: 0x005BDBB8 File Offset: 0x005BBDB8
		public static string GetPollutionDescription(float pollution)
		{
			if (pollution <= 0f)
			{
				return "TilePollutionNone".Translate();
			}
			if (pollution <= 0.333f)
			{
				return "TilePollutionLight".Translate();
			}
			if (pollution <= 0.666f)
			{
				return "TilePollutionModerate".Translate();
			}
			return "TilePollutionExtreme".Translate();
		}

		// Token: 0x0400D65F RID: 54879
		private static PlanetTile cachedTile_noSnap = PlanetTile.Invalid;

		// Token: 0x0400D660 RID: 54880
		private static int cachedFrame_noSnap = -1;

		// Token: 0x0400D661 RID: 54881
		private static PlanetTile cachedTile_snap = PlanetTile.Invalid;

		// Token: 0x0400D662 RID: 54882
		private static int cachedFrame_snap = -1;

		// Token: 0x0400D663 RID: 54883
		private static PlanetLayer cachedLayer;

		// Token: 0x0400D664 RID: 54884
		public const float MaxRayLength = 1500f;

		// Token: 0x0400D665 RID: 54885
		private static readonly List<WorldObject> tmpWorldObjectsUnderMouse = new List<WorldObject>();
	}
}
