using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D15 RID: 15637
	[StaticConstructorOnStartup]
	public static class ExpandableLandmarksUtility
	{
		// Token: 0x1700350C RID: 13580
		// (get) Token: 0x06015486 RID: 87174 RVA: 0x0065DEC4 File Offset: 0x0065C0C4
		private static Texture2D LandmarkTex
		{
			get
			{
				Texture2D result;
				if ((result = ExpandableLandmarksUtility.landmarkTex) == null)
				{
					result = (ExpandableLandmarksUtility.landmarkTex = ContentFinder<Texture2D>.Get("World/WorldObjects/Expanding/LandmarkExpanding", true));
				}
				return result;
			}
		}

		// Token: 0x1700350D RID: 13581
		// (get) Token: 0x06015487 RID: 87175 RVA: 0x0065DEE0 File Offset: 0x0065C0E0
		private static float TransitionPct
		{
			get
			{
				if (!Find.PlaySettings.showExpandingLandmarks)
				{
					return 0f;
				}
				return ExpandableLandmarksUtility.transitionPct;
			}
		}

		// Token: 0x1700350E RID: 13582
		// (get) Token: 0x06015488 RID: 87176 RVA: 0x0065DEFC File Offset: 0x0065C0FC
		private static List<PlanetTile> LandmarksToShow
		{
			get
			{
				if (!ExpandableLandmarksUtility.cachedLandmarksToShow.Empty<PlanetTile>())
				{
					return ExpandableLandmarksUtility.cachedLandmarksToShow;
				}
				foreach (PlanetTile planetTile in Find.World.landmarks.landmarks.Keys)
				{
					if (!Find.WorldObjects.AnyWorldObjectAt(planetTile))
					{
						ExpandableLandmarksUtility.cachedLandmarksToShow.Add(planetTile);
					}
				}
				return ExpandableLandmarksUtility.cachedLandmarksToShow;
			}
		}

		// Token: 0x06015489 RID: 87177 RVA: 0x0065DF88 File Offset: 0x0065C188
		public static void ResetStaticData()
		{
			ExpandableLandmarksUtility.landmarkTex = null;
			ExpandableLandmarksUtility.cachedLandmarksToShow.Clear();
		}

		// Token: 0x0601548A RID: 87178 RVA: 0x0065DF9C File Offset: 0x0065C19C
		public static void ExpandableLandmarksUpdate()
		{
			float num = Time.deltaTime * 3f;
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.VeryClose)
			{
				ExpandableLandmarksUtility.transitionPct -= num;
			}
			else
			{
				ExpandableLandmarksUtility.transitionPct += num;
			}
			ExpandableLandmarksUtility.transitionPct = Mathf.Clamp01(ExpandableLandmarksUtility.transitionPct);
		}

		// Token: 0x0601548B RID: 87179 RVA: 0x0065DFEC File Offset: 0x0065C1EC
		public static void ExpandableLandmarksOnGUI()
		{
			foreach (PlanetTile planetTile in ExpandableLandmarksUtility.LandmarksToShow)
			{
				try
				{
					bool flag = ExpandableLandmarksUtility.IsHighlighted(planetTile);
					if (ExpandableLandmarksUtility.TransitionPct != 0f || flag)
					{
						if (!ExpandableLandmarksUtility.HiddenBehindTerrainNow(planetTile))
						{
							Color white = Color.white;
							white.a = (flag ? 1f : ExpandableLandmarksUtility.transitionPct);
							GUI.color = white;
							Rect rect = ExpandableLandmarksUtility.ExpandedIconScreenRect(planetTile);
							Material material = null;
							if (flag)
							{
								material = ExpandableLandmarksUtility.GetMaterial(white);
							}
							Widgets.DrawTextureRotated(rect, ExpandableLandmarksUtility.LandmarkTex, 0f, material);
						}
					}
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Error while drawing landmark at {0}: {1}", planetTile.ToStringSafe<PlanetTile>(), arg));
				}
			}
			GUI.color = Color.white;
		}

		// Token: 0x0601548C RID: 87180 RVA: 0x0065E0DC File Offset: 0x0065C2DC
		private static bool IsHighlighted(PlanetTile tile)
		{
			Dialog_WorldSearch dialog_WorldSearch;
			return Find.WindowStack.TryGetWindow<Dialog_WorldSearch>(out dialog_WorldSearch) && ((dialog_WorldSearch.Highlighted != null && dialog_WorldSearch.Highlighted.tile == tile) || (dialog_WorldSearch.CommonSearchWidget.filter.Text.Length >= 1 && dialog_WorldSearch.IsListed(tile)));
		}

		// Token: 0x0601548D RID: 87181 RVA: 0x0065E138 File Offset: 0x0065C338
		private static Material GetMaterial(Color color)
		{
			return MaterialPool.MatFrom(new MaterialRequest(null, ShaderDatabase.ExpandingIconUI, color)
			{
				needsMainTex = false,
				colorTwo = ExpandableLandmarksUtility.HighlightedColor
			});
		}

		// Token: 0x0601548E RID: 87182 RVA: 0x0065E170 File Offset: 0x0065C370
		private static Rect ExpandedIconScreenRect(PlanetTile tile)
		{
			Vector2 vector = GenWorldUI.WorldToUIPosition(ExpandableLandmarksUtility.DrawPos(tile));
			bool flag = tile.Layer != Find.WorldSelector.SelectedLayer;
			float num = 30f;
			if (flag)
			{
				num /= 2f;
			}
			return new Rect(vector.x - num / 2f, vector.y - num / 2f, num, num);
		}

		// Token: 0x0601548F RID: 87183 RVA: 0x0065E1D2 File Offset: 0x0065C3D2
		private static bool HiddenBehindTerrainNow(PlanetTile tile)
		{
			return WorldRendererUtility.HiddenBehindTerrainNow(ExpandableLandmarksUtility.DrawPos(tile)) || tile.Layer != PlanetLayer.Selected;
		}

		// Token: 0x06015490 RID: 87184 RVA: 0x0065E1F4 File Offset: 0x0065C3F4
		private static Vector3 DrawPos(PlanetTile tile)
		{
			return tile.Layer.Origin + Find.WorldGrid.GetTileCenter(tile);
		}

		// Token: 0x06015491 RID: 87185 RVA: 0x0065E212 File Offset: 0x0065C412
		public static void Notify_WorldObjectsChanged()
		{
			ExpandableLandmarksUtility.cachedLandmarksToShow.Clear();
		}

		// Token: 0x0400ED50 RID: 60752
		private static float transitionPct;

		// Token: 0x0400ED51 RID: 60753
		private static List<PlanetTile> cachedLandmarksToShow = new List<PlanetTile>();

		// Token: 0x0400ED52 RID: 60754
		private const float LandmarkIconSize = 30f;

		// Token: 0x0400ED53 RID: 60755
		private const float TransitionSpeed = 3f;

		// Token: 0x0400ED54 RID: 60756
		private static readonly Color HighlightedColor = ExpandableWorldObjectsUtility.HighlightedColor;

		// Token: 0x0400ED55 RID: 60757
		private static Texture2D landmarkTex;
	}
}
