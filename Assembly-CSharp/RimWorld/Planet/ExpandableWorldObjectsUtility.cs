using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CA2 RID: 15522
	[StaticConstructorOnStartup]
	public static class ExpandableWorldObjectsUtility
	{
		// Token: 0x17003493 RID: 13459
		// (get) Token: 0x0601518A RID: 86410 RVA: 0x0065027A File Offset: 0x0064E47A
		public static float RawTransitionPct
		{
			get
			{
				if (!Find.PlaySettings.showImportantExpandingIcons)
				{
					return 0f;
				}
				return ExpandableWorldObjectsUtility.transitionPct;
			}
		}

		// Token: 0x0601518B RID: 86411 RVA: 0x00650294 File Offset: 0x0064E494
		public static float TransitionPct(WorldObject wo)
		{
			if (ExpandableWorldObjectsUtility.HiddenByRules(wo))
			{
				return 0f;
			}
			if (wo.def.fullyExpandedInSpace && wo.Tile.LayerDef.isSpace && wo.Tile.Layer != Find.WorldSelector.SelectedLayer)
			{
				return 0.3f;
			}
			if (wo.def.fullyExpandedInSpace && wo.Tile.LayerDef.isSpace)
			{
				return 1f;
			}
			return ExpandableWorldObjectsUtility.transitionPct;
		}

		// Token: 0x0601518C RID: 86412 RVA: 0x00650320 File Offset: 0x0064E520
		public static bool HiddenByRules(WorldObject wo)
		{
			if (wo.Tile.LayerDef.isSpace)
			{
				return false;
			}
			bool flag = ExpandableWorldObjectsUtility.IsNonPlayerSettlement(wo);
			return (!Find.PlaySettings.showBasesExpandingIcons && flag) || (!Find.PlaySettings.showImportantExpandingIcons && !flag);
		}

		// Token: 0x0601518D RID: 86413 RVA: 0x00650370 File Offset: 0x0064E570
		private static bool IsNonPlayerSettlement(WorldObject wo)
		{
			Settlement settlement = wo as Settlement;
			return settlement != null && !settlement.Faction.IsPlayer && !settlement.HasMap;
		}

		// Token: 0x17003494 RID: 13460
		// (get) Token: 0x0601518E RID: 86414 RVA: 0x0065039F File Offset: 0x0064E59F
		public static float ExpandMoreTransitionPct
		{
			get
			{
				if (!Find.PlaySettings.showImportantExpandingIcons)
				{
					return 0f;
				}
				return ExpandableWorldObjectsUtility.expandMoreTransitionPct;
			}
		}

		// Token: 0x0601518F RID: 86415 RVA: 0x006503B8 File Offset: 0x0064E5B8
		public static void ExpandableWorldObjectsUpdate()
		{
			float num = Time.deltaTime * 3f;
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.VeryClose)
			{
				ExpandableWorldObjectsUtility.transitionPct -= num;
			}
			else
			{
				ExpandableWorldObjectsUtility.transitionPct += num;
			}
			ExpandableWorldObjectsUtility.transitionPct = Mathf.Clamp01(ExpandableWorldObjectsUtility.transitionPct);
			float num2 = Time.deltaTime * 4f;
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.Far)
			{
				ExpandableWorldObjectsUtility.expandMoreTransitionPct -= num2;
			}
			else
			{
				ExpandableWorldObjectsUtility.expandMoreTransitionPct += num2;
			}
			ExpandableWorldObjectsUtility.expandMoreTransitionPct = Mathf.Clamp01(ExpandableWorldObjectsUtility.expandMoreTransitionPct);
		}

		// Token: 0x06015190 RID: 86416 RVA: 0x0065044C File Offset: 0x0064E64C
		public static void ExpandableWorldObjectsOnGUI()
		{
			if (!DebugViewSettings.drawWorldObjects)
			{
				return;
			}
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			ExpandableWorldObjectsUtility.tmpWorldObjects.Clear();
			ExpandableWorldObjectsUtility.tmpWorldObjects.AddRange(Find.WorldObjects.AllWorldObjects);
			ExpandableWorldObjectsUtility.SortByExpandingIconPriority(ExpandableWorldObjectsUtility.tmpWorldObjects);
			WorldTargeter worldTargeter = Find.WorldTargeter;
			List<WorldObject> worldObjectsUnderMouse = null;
			if (worldTargeter.IsTargeting)
			{
				worldObjectsUnderMouse = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			}
			for (int i = 0; i < ExpandableWorldObjectsUtility.tmpWorldObjects.Count; i++)
			{
				try
				{
					WorldObject worldObject = ExpandableWorldObjectsUtility.tmpWorldObjects[i];
					if (worldObject.def.expandingIcon)
					{
						float num = ExpandableWorldObjectsUtility.TransitionPct(worldObject);
						bool flag = ExpandableWorldObjectsUtility.IsHighlighted(worldObject);
						if (num != 0f || flag)
						{
							if (!worldObject.HiddenBehindTerrainNow())
							{
								Material material = worldObject.ExpandingMaterial;
								Color expandingIconColor = worldObject.ExpandingIconColor;
								expandingIconColor.a = (flag ? 1f : num);
								if (worldTargeter.IsTargetedNow(worldObject, worldObjectsUnderMouse))
								{
									float num2 = GenMath.LerpDouble(-1f, 1f, 0.7f, 1f, Mathf.Sin(Time.time * 8f));
									expandingIconColor.r *= num2;
									expandingIconColor.g *= num2;
									expandingIconColor.b *= num2;
								}
								GUI.color = expandingIconColor;
								Rect rect = ExpandableWorldObjectsUtility.ExpandedIconScreenRect(worldObject, 1f);
								if (worldObject.ExpandingIconFlipHorizontal)
								{
									rect.x = rect.xMax;
									rect.width *= -1f;
								}
								if (material == null && flag)
								{
									material = ExpandableWorldObjectsUtility.GetMaterial(expandingIconColor, ExpandableWorldObjectsUtility.HighlightedColor);
								}
								else if (material == null)
								{
									MapParent mapParent = worldObject as MapParent;
									if (mapParent != null && mapParent.HasMap)
									{
										material = ExpandableWorldObjectsUtility.GetMaterial(expandingIconColor, ExpandableWorldObjectsUtility.HasMapColor);
									}
								}
								Widgets.DrawTextureRotated(rect, worldObject.ExpandingIcon, worldObject.ExpandingIconRotation, material);
							}
						}
					}
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Error while drawing {0}: {1}", ExpandableWorldObjectsUtility.tmpWorldObjects[i].ToStringSafe<WorldObject>(), arg));
				}
			}
			ExpandableWorldObjectsUtility.tmpWorldObjects.Clear();
			GUI.color = Color.white;
		}

		// Token: 0x06015191 RID: 86417 RVA: 0x00650690 File Offset: 0x0064E890
		private static bool IsHighlighted(WorldObject wo)
		{
			Dialog_WorldSearch dialog_WorldSearch;
			return Find.WindowStack.TryGetWindow<Dialog_WorldSearch>(out dialog_WorldSearch) && dialog_WorldSearch.CommonSearchWidget.filter.Text.Length >= 1 && dialog_WorldSearch.IsListed(wo);
		}

		// Token: 0x06015192 RID: 86418 RVA: 0x006506D0 File Offset: 0x0064E8D0
		private static Material GetMaterial(Color color, Color highlight)
		{
			return MaterialPool.MatFrom(new MaterialRequest(null, ShaderDatabase.ExpandingIconUI, color)
			{
				needsMainTex = false,
				colorTwo = highlight
			});
		}

		// Token: 0x06015193 RID: 86419 RVA: 0x00650704 File Offset: 0x0064E904
		public static Rect ExpandedIconScreenRect(WorldObject o, float factor = 1f)
		{
			Vector2 vector = o.ScreenPos();
			float num = 30f * o.def.expandingIconDrawSize;
			float num2;
			if (o.ExpandMore)
			{
				num2 = Mathf.Lerp(num, num * 1.35f, ExpandableWorldObjectsUtility.ExpandMoreTransitionPct);
			}
			else
			{
				num2 = num;
			}
			num2 *= factor;
			return new Rect(vector.x - num2 / 2f, vector.y - num2 / 2f, num2, num2);
		}

		// Token: 0x06015194 RID: 86420 RVA: 0x00650770 File Offset: 0x0064E970
		public static bool IsExpanded(WorldObject o)
		{
			return ExpandableWorldObjectsUtility.TransitionPct(o) > 0.5f && o.def.expandingIcon;
		}

		// Token: 0x06015195 RID: 86421 RVA: 0x0065078C File Offset: 0x0064E98C
		public static void GetExpandedWorldObjectUnderMouse(Vector2 mousePos, List<WorldObject> outList)
		{
			outList.Clear();
			Vector2 vector = mousePos;
			vector.y = (float)UI.screenHeight - vector.y;
			List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
			for (int i = 0; i < allWorldObjects.Count; i++)
			{
				WorldObject worldObject = allWorldObjects[i];
				if (ExpandableWorldObjectsUtility.IsExpanded(worldObject) && ExpandableWorldObjectsUtility.ExpandedIconScreenRect(worldObject, 1f).Contains(vector) && !worldObject.HiddenBehindTerrainNow())
				{
					outList.Add(worldObject);
				}
			}
			ExpandableWorldObjectsUtility.SortByExpandingIconPriority(outList);
			outList.Reverse();
		}

		// Token: 0x06015196 RID: 86422 RVA: 0x00650814 File Offset: 0x0064EA14
		private static void SortByExpandingIconPriority(List<WorldObject> worldObjects)
		{
			Vector3 cameraPos = Find.WorldCameraDriver.CameraPosition;
			worldObjects.SortBy(delegate(WorldObject x)
			{
				float num = x.ExpandingIconPriority;
				if (x.Faction != null && x.Faction.IsPlayer)
				{
					num += 0.001f;
				}
				return num;
			}, (WorldObject x) => -(x.DrawPos - cameraPos).sqrMagnitude);
		}

		// Token: 0x0400EB4D RID: 60237
		private static float transitionPct;

		// Token: 0x0400EB4E RID: 60238
		private static float expandMoreTransitionPct;

		// Token: 0x0400EB4F RID: 60239
		private static readonly List<WorldObject> tmpWorldObjects = new List<WorldObject>();

		// Token: 0x0400EB50 RID: 60240
		private const float WorldObjectIconSize = 30f;

		// Token: 0x0400EB51 RID: 60241
		private const float ExpandMoreWorldObjectIconSizeFactor = 1.35f;

		// Token: 0x0400EB52 RID: 60242
		private const float TransitionSpeed = 3f;

		// Token: 0x0400EB53 RID: 60243
		private const float ExpandMoreTransitionSpeed = 4f;

		// Token: 0x0400EB54 RID: 60244
		private const float BackgroundSpaceOpacity = 0.3f;

		// Token: 0x0400EB55 RID: 60245
		private static readonly Color HasMapColor = new Color(0.1f, 0.9f, 0.1f, 1f);

		// Token: 0x0400EB56 RID: 60246
		public static readonly Color HighlightedColor = new Color(0.9f, 0.8f, 0.1f, 1f);
	}
}
