using System;
using System.Collections.Generic;
using System.IO;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CF4 RID: 3316
	[StaticConstructorOnStartup]
	public static class GenDraw
	{
		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x0600548E RID: 21646 RVA: 0x001B3A0E File Offset: 0x001B1C0E
		public static Material CurTargetingMat
		{
			get
			{
				GenDraw.TargetSquareMatSingle.color = GenDraw.CurTargetingColor;
				return GenDraw.TargetSquareMatSingle;
			}
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x0600548F RID: 21647 RVA: 0x001B3A24 File Offset: 0x001B1C24
		public static Color CurTargetingColor
		{
			get
			{
				float num = (float)Math.Sin((double)(Time.time * 8f));
				num *= 0.2f;
				num += 0.8f;
				return new Color(1f, num, num);
			}
		}

		// Token: 0x06005490 RID: 21648 RVA: 0x001B3A60 File Offset: 0x001B1C60
		public static void DrawNoBuildEdgeLines()
		{
			GenDraw.DrawMapEdgeLines(10);
		}

		// Token: 0x06005491 RID: 21649 RVA: 0x001B3A69 File Offset: 0x001B1C69
		public static void DrawNoZoneEdgeLines()
		{
			GenDraw.DrawMapEdgeLines(5);
		}

		// Token: 0x06005492 RID: 21650 RVA: 0x001B3A74 File Offset: 0x001B1C74
		private static void DrawMapEdgeLines(int edgeDist)
		{
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			IntVec3 size = Find.CurrentMap.Size;
			Vector3 vector = new Vector3((float)edgeDist, y, (float)edgeDist);
			Vector3 vector2 = new Vector3((float)edgeDist, y, (float)(size.z - edgeDist));
			Vector3 vector3 = new Vector3((float)(size.x - edgeDist), y, (float)(size.z - edgeDist));
			Vector3 vector4 = new Vector3((float)(size.x - edgeDist), y, (float)edgeDist);
			GenDraw.DrawLineBetween(vector, vector2, GenDraw.LineMatMetaOverlay, 0.2f);
			GenDraw.DrawLineBetween(vector2, vector3, GenDraw.LineMatMetaOverlay, 0.2f);
			GenDraw.DrawLineBetween(vector3, vector4, GenDraw.LineMatMetaOverlay, 0.2f);
			GenDraw.DrawLineBetween(vector4, vector, GenDraw.LineMatMetaOverlay, 0.2f);
		}

		// Token: 0x06005493 RID: 21651 RVA: 0x001B3B28 File Offset: 0x001B1D28
		public static void DrawLineBetween(Vector3 A, Vector3 B)
		{
			GenDraw.DrawLineBetween(A, B, GenDraw.LineMatWhite, 0.2f);
		}

		// Token: 0x06005494 RID: 21652 RVA: 0x001B3B3B File Offset: 0x001B1D3B
		public static void DrawLineBetween(Vector3 A, Vector3 B, float layer)
		{
			GenDraw.DrawLineBetween(A, B, layer, GenDraw.LineMatWhite, 0.2f);
		}

		// Token: 0x06005495 RID: 21653 RVA: 0x001B3B4F File Offset: 0x001B1D4F
		public static void DrawLineBetween(Vector3 A, Vector3 B, float layer, Material mat, float lineWidth = 0.2f)
		{
			GenDraw.DrawLineBetween(A + Vector3.up * layer, B + Vector3.up * layer, mat, lineWidth);
		}

		// Token: 0x06005496 RID: 21654 RVA: 0x001B3B7B File Offset: 0x001B1D7B
		public static void DrawLineBetween(Vector3 A, Vector3 B, SimpleColor color, float lineWidth = 0.2f)
		{
			GenDraw.DrawLineBetween(A, B, GenDraw.GetLineMat(color), lineWidth);
		}

		// Token: 0x06005497 RID: 21655 RVA: 0x001B3B8C File Offset: 0x001B1D8C
		public static void DrawLineBetween(Vector3 A, Vector3 B, Material mat, float lineWidth = 0.2f)
		{
			if (Mathf.Abs(A.x - B.x) < 0.01f && Mathf.Abs(A.z - B.z) < 0.01f)
			{
				return;
			}
			Vector3 pos = (A + B) / 2f;
			if (A == B)
			{
				return;
			}
			A.y = B.y;
			float z = (A - B).MagnitudeHorizontal();
			Quaternion q = Quaternion.LookRotation(A - B);
			Vector3 s = new Vector3(lineWidth, 1f, z);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, q, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, mat, 0);
		}

		// Token: 0x06005498 RID: 21656 RVA: 0x001B3C3D File Offset: 0x001B1E3D
		public static void DrawCircleOutline(Vector3 center, float radius)
		{
			GenDraw.DrawCircleOutline(center, radius, GenDraw.LineMatWhite);
		}

		// Token: 0x06005499 RID: 21657 RVA: 0x001B3C4B File Offset: 0x001B1E4B
		public static void DrawCircleOutline(Vector3 center, float radius, SimpleColor color)
		{
			GenDraw.DrawCircleOutline(center, radius, GenDraw.GetLineMat(color));
		}

		// Token: 0x0600549A RID: 21658 RVA: 0x001B3C5C File Offset: 0x001B1E5C
		public static void DrawCircleOutline(Vector3 center, float radius, Material material)
		{
			int num = Mathf.Clamp(Mathf.RoundToInt(24f * radius), 12, 48);
			float num2 = 0f;
			float num3 = 6.2831855f / (float)num;
			Vector3 vector = center;
			Vector3 a = center;
			for (int i = 0; i < num + 2; i++)
			{
				if (i >= 2)
				{
					GenDraw.DrawLineBetween(a, vector, material, 0.2f);
				}
				a = vector;
				vector = center;
				vector.x += Mathf.Cos(num2) * radius;
				vector.z += Mathf.Sin(num2) * radius;
				num2 += num3;
			}
		}

		// Token: 0x0600549B RID: 21659 RVA: 0x001B3CE8 File Offset: 0x001B1EE8
		private static Material GetLineMat(SimpleColor color)
		{
			switch (color)
			{
			case SimpleColor.White:
				return GenDraw.LineMatWhite;
			case SimpleColor.Red:
				return GenDraw.LineMatRed;
			case SimpleColor.Green:
				return GenDraw.LineMatGreen;
			case SimpleColor.Blue:
				return GenDraw.LineMatBlue;
			case SimpleColor.Magenta:
				return GenDraw.LineMatMagenta;
			case SimpleColor.Yellow:
				return GenDraw.LineMatYellow;
			case SimpleColor.Cyan:
				return GenDraw.LineMatCyan;
			case SimpleColor.Orange:
				return GenDraw.LineMatOrange;
			default:
				return GenDraw.LineMatWhite;
			}
		}

		// Token: 0x0600549C RID: 21660 RVA: 0x001B3D52 File Offset: 0x001B1F52
		public static void DrawWorldLineBetween(Vector3 A, Vector3 B, float widthFactor = 1f)
		{
			GenDraw.DrawWorldLineBetween(A, B, GenDraw.WorldLineMatWhite, widthFactor);
		}

		// Token: 0x0600549D RID: 21661 RVA: 0x001B3D64 File Offset: 0x001B1F64
		public static void DrawWorldLineBetween(Vector3 A, Vector3 B, Material material, float widthFactor = 1f)
		{
			if (Mathf.Abs(A.x - B.x) < 0.005f && Mathf.Abs(A.y - B.y) < 0.005f && Mathf.Abs(A.z - B.z) < 0.005f)
			{
				return;
			}
			Vector3 pos = (A + B) / 2f;
			float magnitude = (A - B).magnitude;
			Quaternion q = Quaternion.LookRotation(A - B, pos.normalized);
			Vector3 s = new Vector3(0.2f * Find.WorldGrid.AverageTileSize * widthFactor, 1f, magnitude);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, q, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, WorldCameraManager.WorldLayer);
		}

		// Token: 0x0600549E RID: 21662 RVA: 0x001B3E38 File Offset: 0x001B2038
		public static void DrawWorldRadiusRing(PlanetTile center, int radius, Material overrideMat = null)
		{
			if (radius < 0)
			{
				return;
			}
			if (GenDraw.cachedEdgeTilesForCenter != center || GenDraw.cachedEdgeTilesForRadius != radius || GenDraw.cachedEdgeTilesForWorldSeed != Find.World.info.Seed)
			{
				GenDraw.cachedEdgeTilesForCenter = center;
				GenDraw.cachedEdgeTilesForRadius = radius;
				GenDraw.cachedEdgeTilesForWorldSeed = Find.World.info.Seed;
				GenDraw.cachedEdgeTiles.Clear();
				GenDraw.cachedEdgeTilesSorted.Clear();
				WorldGrid grid = Find.WorldGrid;
				center.Layer.Filler.FloodFill(center, (PlanetTile tile) => true, delegate(PlanetTile tile, int dist)
				{
					if (dist > radius + 1)
					{
						return true;
					}
					if (dist == radius + 1 || grid.GetTileNeighborCount(tile) < 5)
					{
						GenDraw.cachedEdgeTiles.Add(tile);
					}
					return false;
				}, int.MaxValue, null);
				GenDraw.cachedEdgeTilesSorted.AddRange(GenDraw.cachedEdgeTiles);
				Vector3 c = Vector3.zero;
				foreach (PlanetTile tile2 in GenDraw.cachedEdgeTilesSorted)
				{
					c += grid.GetTileCenter(tile2);
				}
				c /= (float)GenDraw.cachedEdgeTilesSorted.Count;
				Vector3 n = c.normalized;
				Vector3 refDir = Vector3.ProjectOnPlane(grid.GetTileCenter(GenDraw.cachedEdgeTilesSorted[0]) - c, n).normalized;
				GenDraw.cachedEdgeTilesSorted.Sort(delegate(PlanetTile a, PlanetTile b)
				{
					Vector3 normalized = Vector3.ProjectOnPlane(grid.GetTileCenter(a) - c, n).normalized;
					float num = Vector3.SignedAngle(refDir, normalized, n);
					num = ((num < 0f) ? (num + 360f) : num);
					Vector3 normalized2 = Vector3.ProjectOnPlane(grid.GetTileCenter(b) - c, n).normalized;
					float num2 = Vector3.SignedAngle(refDir, normalized2, n);
					num2 = ((num2 < 0f) ? (num2 + 360f) : num2);
					if (Mathf.Approximately((float)a, (float)b))
					{
						return 0;
					}
					if (num > num2)
					{
						return -1;
					}
					return 1;
				});
				for (int i = 0; i < GenDraw.cachedEdgeTilesSorted.Count; i++)
				{
					PlanetTile tileA = GenDraw.cachedEdgeTilesSorted[i];
					PlanetTile planetTile = GenDraw.cachedEdgeTilesSorted[(i + 1) % GenDraw.cachedEdgeTilesSorted.Count];
					PlanetTile tileB = GenDraw.cachedEdgeTilesSorted[(i + 2) % GenDraw.cachedEdgeTilesSorted.Count];
					if (!grid.IsNeighbor(tileA, planetTile) && grid.IsNeighbor(planetTile, tileB) && grid.IsNeighbor(tileA, tileB))
					{
						GenDraw.cachedEdgeTilesSorted.Swap((i + 1) % GenDraw.cachedEdgeTilesSorted.Count, (i + 2) % GenDraw.cachedEdgeTilesSorted.Count);
					}
				}
			}
			Material material = overrideMat ?? (center.LayerDef.isSpace ? center.LayerDef.WorldLineMaterialHighVis : center.LayerDef.WorldLineMaterial);
			GenDraw.DrawWorldLineStrip(GenDraw.cachedEdgeTilesSorted, material, 5f * center.LayerDef.lineWidthFactor);
		}

		// Token: 0x0600549F RID: 21663 RVA: 0x001B4114 File Offset: 0x001B2314
		public static void DrawWorldLineStrip(List<PlanetTile> edgeTiles, Material material, float widthFactor)
		{
			if (edgeTiles.Count < 3)
			{
				return;
			}
			for (int i = 0; i < edgeTiles.Count; i++)
			{
				int index = (i == 0) ? (edgeTiles.Count - 1) : (i - 1);
				PlanetTile b = edgeTiles[i];
				GenDraw.DrawLineBetween(edgeTiles[index], b, material, widthFactor);
			}
		}

		// Token: 0x060054A0 RID: 21664 RVA: 0x001B4164 File Offset: 0x001B2364
		private static void DrawLineBetween(PlanetTile a, PlanetTile b, Material material, float widthFactor)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			float d = 0.08f;
			Vector3 a2 = worldGrid.GetTileCenter(a);
			Vector3 vector = worldGrid.GetTileCenter(b);
			a2 += a2.normalized * d;
			vector += vector.normalized * d;
			GenDraw.DrawWorldLineBetween(a2, vector, material, widthFactor);
		}

		// Token: 0x060054A1 RID: 21665 RVA: 0x001B41BC File Offset: 0x001B23BC
		public static void DrawTargetHighlight(LocalTargetInfo targ)
		{
			if (targ.Thing != null)
			{
				GenDraw.DrawTargetingHighlight_Thing(targ.Thing);
				return;
			}
			GenDraw.DrawTargetingHighlight_Cell(targ.Cell);
		}

		// Token: 0x060054A2 RID: 21666 RVA: 0x001B41E0 File Offset: 0x001B23E0
		private static void DrawTargetingHighlight_Cell(IntVec3 c)
		{
			GenDraw.DrawTargetHighlightWithLayer(c, AltitudeLayer.Building, null);
		}

		// Token: 0x060054A3 RID: 21667 RVA: 0x001B41EC File Offset: 0x001B23EC
		public static void DrawTargetHighlightWithLayer(IntVec3 c, AltitudeLayer layer, Material material = null)
		{
			Vector3 position = c.ToVector3ShiftedWithAltitude(layer);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, material ?? GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x060054A4 RID: 21668 RVA: 0x001B4220 File Offset: 0x001B2420
		public static void DrawTargetHighlightWithLayer(Vector3 c, AltitudeLayer layer)
		{
			Vector3 position = new Vector3(c.x, layer.AltitudeFor(), c.z);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.CurTargetingMat, 0);
		}

		// Token: 0x060054A5 RID: 21669 RVA: 0x001B425C File Offset: 0x001B245C
		private static void DrawTargetingHighlight_Thing(Thing t)
		{
			Vector3 vector = t.TrueCenter();
			Graphics.DrawMesh(MeshPool.plane10, new Vector3(vector.x, AltitudeLayer.MapDataOverlay.AltitudeFor(), vector.z), t.Rotation.AsQuat, GenDraw.CurTargetingMat, 0);
			if (t is Pawn || t is Corpse)
			{
				TargetHighlighter.Highlight(t, false, true, false);
			}
		}

		// Token: 0x060054A6 RID: 21670 RVA: 0x001B42C4 File Offset: 0x001B24C4
		public static void DrawStencilCell(Vector3 c, Material material, float width = 1f, float height = 1f)
		{
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(new Vector3(c.x, -1f, c.z), Quaternion.identity, new Vector3(width, 1f, height));
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
		}

		// Token: 0x060054A7 RID: 21671 RVA: 0x001B4314 File Offset: 0x001B2514
		public static void DrawTargetingHightlight_Explosion(IntVec3 c, float Radius)
		{
			GenDraw.DrawRadiusRing(c, Radius);
		}

		// Token: 0x060054A8 RID: 21672 RVA: 0x001B4320 File Offset: 0x001B2520
		public static void DrawInteractionCells(ThingDef tDef, IntVec3 center, Rot4 placingRot)
		{
			if (!tDef.multipleInteractionCellOffsets.NullOrEmpty<IntVec3>())
			{
				using (List<IntVec3>.Enumerator enumerator = tDef.multipleInteractionCellOffsets.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 interactionOffset = enumerator.Current;
						GenDraw.DrawInteractionCell(tDef, interactionOffset, center, placingRot);
					}
					return;
				}
			}
			if (tDef.hasInteractionCell)
			{
				GenDraw.DrawInteractionCell(tDef, tDef.interactionCellOffset, center, placingRot);
			}
		}

		// Token: 0x060054A9 RID: 21673 RVA: 0x001B4398 File Offset: 0x001B2598
		private static void DrawInteractionCell(ThingDef tDef, IntVec3 interactionOffset, IntVec3 center, Rot4 placingRot)
		{
			IntVec3 c = ThingUtility.InteractionCell(interactionOffset, center, placingRot);
			Vector3 vector = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			if (c.InBounds(Find.CurrentMap))
			{
				Building edifice = c.GetEdifice(Find.CurrentMap);
				if (edifice != null && edifice.def.building != null && edifice.def.building.isSittable)
				{
					return;
				}
			}
			if (tDef.interactionCellGraphic == null && tDef.interactionCellIcon != null)
			{
				ThingDef thingDef = tDef.interactionCellIcon;
				if (thingDef.blueprintDef != null)
				{
					thingDef = thingDef.blueprintDef;
				}
				tDef.interactionCellGraphic = thingDef.graphic.GetColoredVersion(ShaderTypeDefOf.EdgeDetect.Shader, GenDraw.InteractionCellIntensity, Color.white);
			}
			if (tDef.interactionCellGraphic != null)
			{
				Rot4 rot = tDef.interactionCellIconReverse ? placingRot.Opposite : placingRot;
				tDef.interactionCellGraphic.DrawFromDef(vector, rot, tDef.interactionCellIcon, 0f);
				return;
			}
			Graphics.DrawMesh(MeshPool.plane10, vector, Quaternion.identity, GenDraw.InteractionCellMaterial, 0);
		}

		// Token: 0x060054AA RID: 21674 RVA: 0x001B448C File Offset: 0x001B268C
		public static void DrawRadiusRing(IntVec3 center, float radius, Color color, Func<IntVec3, bool> predicate = null)
		{
			if (radius > GenRadial.MaxRadialPatternRadius)
			{
				if (!GenDraw.maxRadiusMessaged)
				{
					Log.Error("Cannot draw radius ring of radius " + radius.ToString() + ": not enough squares in the precalculated list.");
					GenDraw.maxRadiusMessaged = true;
				}
				return;
			}
			GenDraw.ringDrawCells.Clear();
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[i];
				if (predicate == null || predicate(intVec))
				{
					GenDraw.ringDrawCells.Add(intVec);
				}
			}
			GenDraw.DrawFieldEdges(GenDraw.ringDrawCells, color, null, null, 2900);
		}

		// Token: 0x060054AB RID: 21675 RVA: 0x001B452A File Offset: 0x001B272A
		public static void DrawRadiusRing(IntVec3 center, float radius)
		{
			GenDraw.DrawRadiusRing(center, radius, Color.white, null);
		}

		// Token: 0x060054AC RID: 21676 RVA: 0x001B453C File Offset: 0x001B273C
		public static void DrawFieldEdges(List<IntVec3> cells, int renderQueue = 2900)
		{
			GenDraw.DrawFieldEdges(cells, Color.white, null, null, renderQueue);
		}

		// Token: 0x060054AD RID: 21677 RVA: 0x001B4564 File Offset: 0x001B2764
		public static void DrawFieldEdges(List<IntVec3> cells, Color color, float? altOffset = null, HashSet<IntVec3> ignoreBorderCells = null, int renderQueue = 2900)
		{
			Map currentMap = Find.CurrentMap;
			Material material = MaterialPool.MatFrom((Texture2D)GenDraw.TargetSquareMatSide.mainTexture, ShaderDatabase.Transparent, color, renderQueue);
			material.GetTexture(GenDraw.MainTex).wrapMode = TextureWrapMode.Clamp;
			material.enableInstancing = true;
			if (GenDraw.fieldGrid == null)
			{
				GenDraw.fieldGrid = new BoolGrid(currentMap);
			}
			else
			{
				GenDraw.fieldGrid.ClearAndResizeTo(currentMap);
			}
			int x = currentMap.Size.x;
			int z = currentMap.Size.z;
			int count = cells.Count;
			float y = altOffset ?? (Rand.ValueSeeded(color.ToOpaque().GetHashCode()) * 0.03658537f / 10f);
			for (int i = 0; i < count; i++)
			{
				if (cells[i].InBounds(currentMap))
				{
					GenDraw.fieldGrid[cells[i].x, cells[i].z] = true;
				}
			}
			GenDraw.instancingMatrices.Clear();
			for (int j = 0; j < count; j++)
			{
				IntVec3 intVec = cells[j];
				if (intVec.InBounds(currentMap))
				{
					GenDraw.rotNeeded[0] = (intVec.z < z - 1 && !GenDraw.fieldGrid[intVec.x, intVec.z + 1] && (ignoreBorderCells == null || !ignoreBorderCells.Contains(intVec + IntVec3.North)));
					GenDraw.rotNeeded[1] = (intVec.x < x - 1 && !GenDraw.fieldGrid[intVec.x + 1, intVec.z] && (ignoreBorderCells == null || !ignoreBorderCells.Contains(intVec + IntVec3.East)));
					GenDraw.rotNeeded[2] = (intVec.z > 0 && !GenDraw.fieldGrid[intVec.x, intVec.z - 1] && (ignoreBorderCells == null || !ignoreBorderCells.Contains(intVec + IntVec3.South)));
					GenDraw.rotNeeded[3] = (intVec.x > 0 && !GenDraw.fieldGrid[intVec.x - 1, intVec.z] && (ignoreBorderCells == null || !ignoreBorderCells.Contains(intVec + IntVec3.West)));
					for (int k = 0; k < 4; k++)
					{
						if (GenDraw.rotNeeded[k])
						{
							GenDraw.instancingMatrices.Add(Matrix4x4.TRS(intVec.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays) + new Vector3(0f, y, 0f), new Rot4(k).AsQuat, Vector3.one));
						}
					}
				}
			}
			if (GenDraw.instancingMatrices.Count > 0)
			{
				Graphics.DrawMeshInstanced(MeshPool.plane10, 0, material, GenDraw.instancingMatrices);
			}
		}

		// Token: 0x060054AE RID: 21678 RVA: 0x001B4854 File Offset: 0x001B2A54
		public static void DrawDiagonalStripes(List<IntVec3> cells, Color? color = null, float? altOffset = null, int renderQueue = 2900)
		{
			Color value = color.GetValueOrDefault();
			if (color == null)
			{
				value = Color.white;
				color = new Color?(value);
			}
			Map currentMap = Find.CurrentMap;
			Material material = MaterialPool.MatFrom((Texture2D)GenDraw.DiagonalStripesMat.mainTexture, ShaderDatabase.Transparent, color.Value, renderQueue);
			material.GetTexture(GenDraw.MainTex).wrapMode = TextureWrapMode.Repeat;
			material.enableInstancing = true;
			if (GenDraw.stripeGrid == null)
			{
				GenDraw.stripeGrid = new BoolGrid(currentMap);
			}
			else
			{
				GenDraw.stripeGrid.ClearAndResizeTo(currentMap);
			}
			int count = cells.Count;
			float y = altOffset ?? (Rand.ValueSeeded(color.Value.ToOpaque().GetHashCode()) * 0.03658537f / 10f);
			for (int i = 0; i < count; i++)
			{
				if (cells[i].InBounds(currentMap))
				{
					GenDraw.stripeGrid[cells[i].x, cells[i].z] = true;
				}
			}
			GenDraw.instancingMatrices.Clear();
			for (int j = 0; j < count; j++)
			{
				IntVec3 c = cells[j];
				if (c.InBounds(currentMap))
				{
					GenDraw.instancingMatrices.Add(Matrix4x4.TRS(c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays) + new Vector3(0f, y, 0f), Quaternion.identity, Vector3.one));
				}
			}
			if (GenDraw.instancingMatrices.Count > 0)
			{
				Graphics.DrawMeshInstanced(MeshPool.plane10, 0, material, GenDraw.instancingMatrices);
			}
		}

		// Token: 0x060054AF RID: 21679 RVA: 0x001B49F4 File Offset: 0x001B2BF4
		public static void DrawAimPie(Thing shooter, LocalTargetInfo target, int degreesWide, float offsetDist)
		{
			float facing = 0f;
			if (target.Cell != shooter.Position)
			{
				if (target.Thing != null)
				{
					facing = (target.Thing.DrawPos - shooter.Position.ToVector3Shifted()).AngleFlat();
				}
				else
				{
					facing = (target.Cell - shooter.Position).AngleFlat;
				}
			}
			GenDraw.DrawAimPieRaw(shooter.DrawPos + new Vector3(0f, offsetDist, 0f), facing, degreesWide);
		}

		// Token: 0x060054B0 RID: 21680 RVA: 0x001B4A88 File Offset: 0x001B2C88
		public static void DrawAimPieRaw(Vector3 center, float facing, int degreesWide)
		{
			if (degreesWide <= 0)
			{
				return;
			}
			if (degreesWide > 360)
			{
				degreesWide = 360;
			}
			center += Quaternion.AngleAxis(facing, Vector3.up) * Vector3.forward * 0.8f;
			Graphics.DrawMesh(MeshPool.pies[degreesWide], center, Quaternion.AngleAxis(facing + (float)(degreesWide / 2) - 90f, Vector3.up), GenDraw.AimPieMaterial, 0);
		}

		// Token: 0x060054B1 RID: 21681 RVA: 0x001B4AFC File Offset: 0x001B2CFC
		public static void DrawCooldownCircle(Vector3 center, float radius)
		{
			Vector3 s = new Vector3(radius, 1f, radius);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(center, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.circle, matrix, GenDraw.AimPieMaterial, 0);
		}

		// Token: 0x060054B2 RID: 21682 RVA: 0x001B4B40 File Offset: 0x001B2D40
		public static void DrawFillableBar(GenDraw.FillableBarRequest r)
		{
			Vector2 vector = r.preRotationOffset.RotatedBy(r.rotation.AsAngle);
			r.center += new Vector3(vector.x, 0f, vector.y);
			if (r.rotation == Rot4.South)
			{
				r.rotation = Rot4.North;
			}
			if (r.rotation == Rot4.West)
			{
				r.rotation = Rot4.East;
			}
			Vector3 s = new Vector3(r.size.x + r.margin, 1f, r.size.y + r.margin);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(r.center, r.rotation.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, r.unfilledMat, 0);
			if (r.fillPercent > 0.001f)
			{
				s = new Vector3(r.size.x * r.fillPercent, 1f, r.size.y);
				matrix = default(Matrix4x4);
				Vector3 pos = r.center + Vector3.up * 0.01f;
				if (!r.rotation.IsHorizontal)
				{
					pos.x -= r.size.x * 0.5f;
					pos.x += 0.5f * r.size.x * r.fillPercent;
				}
				else
				{
					pos.z -= r.size.x * 0.5f;
					pos.z += 0.5f * r.size.x * r.fillPercent;
				}
				matrix.SetTRS(pos, r.rotation.AsQuat, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, r.filledMat, 0);
			}
		}

		// Token: 0x060054B3 RID: 21683 RVA: 0x001B4D44 File Offset: 0x001B2F44
		public static void DrawMeshNowOrLater(Mesh mesh, Vector3 loc, Quaternion quat, Material mat, bool drawNow)
		{
			if (drawNow)
			{
				if (mat == null || !mat.SetPass(0))
				{
					string str = "SetPass(0) call failed on material ";
					string str2 = (mat != null) ? mat.name : null;
					string str3 = " with shader ";
					string str4;
					if (mat == null)
					{
						str4 = null;
					}
					else
					{
						Shader shader = mat.shader;
						str4 = ((shader != null) ? shader.name : null);
					}
					Log.Error(str + str2 + str3 + str4);
				}
				Graphics.DrawMeshNow(mesh, loc, quat);
				return;
			}
			Graphics.DrawMesh(mesh, loc, quat, mat, 0);
		}

		// Token: 0x060054B4 RID: 21684 RVA: 0x001B4DB2 File Offset: 0x001B2FB2
		public static void DrawMeshNowOrLater(Mesh mesh, Matrix4x4 matrix, Material mat, bool drawNow, MaterialPropertyBlock properties = null)
		{
			if (drawNow)
			{
				mat.SetPass(0);
				Graphics.DrawMeshNow(mesh, matrix);
				return;
			}
			Graphics.DrawMesh(mesh, matrix, mat, 0, null, 0, properties);
		}

		// Token: 0x060054B5 RID: 21685 RVA: 0x001B4DD4 File Offset: 0x001B2FD4
		public static void DrawArrowPointingAt(Vector3 mapTarget, bool offscreenOnly = false)
		{
			Vector3 vector = UI.UIToMapPosition((float)(UI.screenWidth / 2), (float)(UI.screenHeight / 2));
			if ((vector - mapTarget).MagnitudeHorizontalSquared() < 81f)
			{
				if (!offscreenOnly)
				{
					Vector3 position = mapTarget;
					position.y = AltitudeLayer.MetaOverlays.AltitudeFor();
					position.z -= 1.5f;
					Graphics.DrawMesh(MeshPool.plane20, position, Quaternion.identity, GenDraw.ArrowMatWhite, 0);
					return;
				}
			}
			else
			{
				Vector3 normalized = (mapTarget - vector).Yto0().normalized;
				Vector3 position2 = vector + normalized * 7f;
				position2.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				Quaternion rotation = Quaternion.LookRotation(normalized);
				Graphics.DrawMesh(MeshPool.plane20, position2, rotation, GenDraw.ArrowMatWhite, 0);
			}
		}

		// Token: 0x060054B6 RID: 21686 RVA: 0x001B4E98 File Offset: 0x001B3098
		public static void DrawArrowRotated(Vector3 pos, float rotationAngle, bool ghost)
		{
			Quaternion rotation = Quaternion.AngleAxis(rotationAngle, new Vector3(0f, 1f, 0f));
			Vector3 position = pos;
			position.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Graphics.DrawMesh(MeshPool.plane10, position, rotation, ghost ? GenDraw.ArrowMatGhost : GenDraw.ArrowMatWhite, 0);
		}

		// Token: 0x060054B7 RID: 21687 RVA: 0x001B4EEC File Offset: 0x001B30EC
		public static void DrawArrowPointingAt(PlanetTile tile, bool offscreenOnly = false)
		{
			if (PlanetLayer.Selected != tile.Layer)
			{
				return;
			}
			WorldGrid worldGrid = Find.WorldGrid;
			Vector2 vector = GenWorldUI.WorldToUIPosition(worldGrid.GetTileCenter(tile));
			Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
			rect = rect.ContractedBy(0.1f * rect.width, 0.1f * rect.height);
			bool flag = rect.Contains(vector);
			Vector2 center = rect.center;
			bool flag2 = Find.WorldCameraDriver.AltitudePercent >= 0.3f;
			if (flag)
			{
				if (!offscreenOnly)
				{
					PlanetTile tileNeighbor = tile.Layer.GetTileNeighbor(tile, 0);
					if (flag2)
					{
						tileNeighbor = tile.Layer.GetTileNeighbor(tileNeighbor, 0);
					}
					Vector3 tileCenter = worldGrid.GetTileCenter(tileNeighbor);
					float num = tile.Layer.GetHeadingFromTo(tileNeighbor, tile);
					num -= 90f;
					WorldRendererUtility.DrawQuadTangentialToPlanet(tileCenter, flag2 ? 2.4f : 1.2f, 0.05f, GenDraw.ArrowMatWhite, num, false, false, null);
					return;
				}
			}
			else
			{
				Vector2 normalized = (vector - center).normalized;
				Vector2 a = center + normalized * 7f;
				Ray ray = Find.WorldCamera.ScreenPointToRay(a * Prefs.UIScale);
				int worldLayerMask = WorldCameraManager.WorldLayerMask;
				WorldTerrainColliderManager.EnsureRaycastCollidersUpdated();
				RaycastHit hit;
				if (!Physics.Raycast(ray, out hit, 1500f, worldLayerMask))
				{
					return;
				}
				PlanetTile tileFromRayHit = Find.World.renderer.GetTileFromRayHit(hit);
				float num2 = tile.Layer.GetHeadingFromTo(tileFromRayHit, tile);
				num2 -= 90f;
				WorldRendererUtility.DrawQuadTangentialToPlanet(worldGrid.GetTileCenter(tileFromRayHit), flag2 ? 4f : 2f, 0.05f, GenDraw.ArrowMatWhite, num2, false, false, null);
			}
		}

		// Token: 0x060054B8 RID: 21688 RVA: 0x001B50B0 File Offset: 0x001B32B0
		public static void DrawCellRect(CellRect rect, Vector3 offset, Material mat, MaterialPropertyBlock properties = null, int layer = 0)
		{
			Matrix4x4 matrix = Matrix4x4.TRS(rect.CenterVector3 + offset, Quaternion.identity, new Vector3((float)rect.Width, 1f, (float)rect.Height));
			Graphics.DrawMesh(MeshPool.plane10, matrix, mat, layer, null, 0, properties);
		}

		// Token: 0x060054B9 RID: 21689 RVA: 0x001B5100 File Offset: 0x001B3300
		public static void DrawQuad(Material mat, Vector3 position, Quaternion rotation, float scale, MaterialPropertyBlock props = null)
		{
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(position, rotation, new Vector3(scale, 1f, scale)), mat, 0, null, 0, props);
		}

		// Token: 0x060054BA RID: 21690 RVA: 0x001B5125 File Offset: 0x001B3325
		public static void DrawQuad(Material mat, Vector3 position, Quaternion rotation, Vector3 scale, MaterialPropertyBlock props = null)
		{
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(position, rotation, scale), mat, 0, null, 0, props);
		}

		// Token: 0x060054BB RID: 21691 RVA: 0x001B5140 File Offset: 0x001B3340
		public static Texture2D CreateTexture2D(this RenderTexture renderTexture, TextureFormat format = TextureFormat.ARGB32, bool mipChain = false)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = renderTexture;
			Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, format, mipChain);
			texture2D.ReadPixels(new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height), 0, 0);
			texture2D.Apply();
			RenderTexture.active = active;
			return texture2D;
		}

		// Token: 0x060054BC RID: 21692 RVA: 0x001B51A0 File Offset: 0x001B33A0
		public static Texture2D CreateTexture2D(this RenderTexture renderTexture, Rect cropRect, TextureFormat format = TextureFormat.ARGB32, bool mipChain = false)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = renderTexture;
			Texture2D texture2D = new Texture2D((int)cropRect.width, (int)cropRect.height, format, mipChain);
			texture2D.ReadPixels(cropRect, 0, 0);
			texture2D.Apply();
			RenderTexture.active = active;
			return texture2D;
		}

		// Token: 0x060054BD RID: 21693 RVA: 0x001B51E5 File Offset: 0x001B33E5
		public static void SaveAsPNG(this Texture2D texture, string path)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			File.WriteAllBytes(path, texture.EncodeToPNG());
		}

		// Token: 0x060054BE RID: 21694 RVA: 0x001B51FF File Offset: 0x001B33FF
		public static void SaveAsPNG(this RenderTexture texture, string path)
		{
			Texture2D texture2 = texture.CreateTexture2D(TextureFormat.ARGB32, false);
			texture2.SaveAsPNG(path);
			texture2.Release();
		}

		// Token: 0x060054BF RID: 21695 RVA: 0x001B5215 File Offset: 0x001B3415
		public static void Release(this Texture2D texture)
		{
			UnityEngine.Object.Destroy(texture);
		}

		// Token: 0x04003D0B RID: 15627
		private static List<Matrix4x4> instancingMatrices = new List<Matrix4x4>();

		// Token: 0x04003D0C RID: 15628
		private static readonly Material TargetSquareMatSingle = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Square", ShaderDatabase.Transparent);

		// Token: 0x04003D0D RID: 15629
		private static readonly Material InvalidTargetSquareMatSingle = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Square", ShaderDatabase.Transparent, Color.gray);

		// Token: 0x04003D0E RID: 15630
		private const float TargetPulseFrequency = 8f;

		// Token: 0x04003D0F RID: 15631
		public static readonly string LineTexPath = "UI/Overlays/ThingLine";

		// Token: 0x04003D10 RID: 15632
		public static readonly string OneSidedLineTexPath = "UI/Overlays/OneSidedLine";

		// Token: 0x04003D11 RID: 15633
		public static readonly string OneSidedLineOpaqueTexPath = "UI/Overlays/OneSidedLineOpaque";

		// Token: 0x04003D12 RID: 15634
		private static readonly Material LineMatWhite = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.white);

		// Token: 0x04003D13 RID: 15635
		private static readonly Material LineMatRed = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.red);

		// Token: 0x04003D14 RID: 15636
		private static readonly Material LineMatGreen = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.green);

		// Token: 0x04003D15 RID: 15637
		private static readonly Material LineMatBlue = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.blue);

		// Token: 0x04003D16 RID: 15638
		private static readonly Material LineMatMagenta = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.magenta);

		// Token: 0x04003D17 RID: 15639
		private static readonly Material LineMatYellow = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.yellow);

		// Token: 0x04003D18 RID: 15640
		private static readonly Material LineMatCyan = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, Color.cyan);

		// Token: 0x04003D19 RID: 15641
		private static readonly Material LineMatOrange = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, ColorLibrary.Orange);

		// Token: 0x04003D1A RID: 15642
		private static readonly Material LineMatMetaOverlay = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.MetaOverlay);

		// Token: 0x04003D1B RID: 15643
		private static readonly Material WorldLineMatWhite = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.WorldOverlayTransparent, Color.white, 3590);

		// Token: 0x04003D1C RID: 15644
		private static readonly Material TargetSquareMatSide = MatLoader.LoadMat("Misc/FieldEdge", -1);

		// Token: 0x04003D1D RID: 15645
		private static readonly Material DiagonalStripesMat = MatLoader.LoadMat("Misc/DiagonalStripes", -1);

		// Token: 0x04003D1E RID: 15646
		public static readonly Material RitualStencilMat = MaterialPool.MatFrom(ShaderDatabase.RitualStencil);

		// Token: 0x04003D1F RID: 15647
		private const float LineWidth = 0.2f;

		// Token: 0x04003D20 RID: 15648
		private const float BaseWorldLineWidth = 0.2f;

		// Token: 0x04003D21 RID: 15649
		public static readonly Material InteractionCellMaterial = MaterialPool.MatFrom("UI/Overlays/InteractionCell", ShaderDatabase.Transparent);

		// Token: 0x04003D22 RID: 15650
		private static readonly Color InteractionCellIntensity = new Color(1f, 1f, 1f, 0.3f);

		// Token: 0x04003D23 RID: 15651
		public const float MultiItemsPerCellDrawSizeFactor = 0.8f;

		// Token: 0x04003D24 RID: 15652
		private static readonly List<PlanetTile> cachedEdgeTilesSorted = new List<PlanetTile>();

		// Token: 0x04003D25 RID: 15653
		private static readonly HashSet<PlanetTile> cachedEdgeTiles = new HashSet<PlanetTile>();

		// Token: 0x04003D26 RID: 15654
		private static PlanetTile cachedEdgeTilesForCenter = PlanetTile.Invalid;

		// Token: 0x04003D27 RID: 15655
		private static int cachedEdgeTilesForRadius = -1;

		// Token: 0x04003D28 RID: 15656
		private static int cachedEdgeTilesForWorldSeed = -1;

		// Token: 0x04003D29 RID: 15657
		private static List<IntVec3> ringDrawCells = new List<IntVec3>();

		// Token: 0x04003D2A RID: 15658
		private static bool maxRadiusMessaged = false;

		// Token: 0x04003D2B RID: 15659
		private static BoolGrid fieldGrid;

		// Token: 0x04003D2C RID: 15660
		private static readonly bool[] rotNeeded = new bool[4];

		// Token: 0x04003D2D RID: 15661
		private static BoolGrid stripeGrid;

		// Token: 0x04003D2E RID: 15662
		private static readonly Material AimPieMaterial = SolidColorMaterials.SimpleSolidColorMaterial(new Color(1f, 1f, 1f, 0.3f), false);

		// Token: 0x04003D2F RID: 15663
		public static readonly Material ArrowMatWhite = MaterialPool.MatFrom("UI/Overlays/Arrow", ShaderDatabase.CutoutFlying01, Color.white);

		// Token: 0x04003D30 RID: 15664
		private static readonly Material ArrowMatGhost = MaterialPool.MatFrom("UI/Overlays/ArrowGhost", ShaderDatabase.Transparent, Color.white);

		// Token: 0x04003D31 RID: 15665
		private static readonly int MainTex = Shader.PropertyToID("_MainTex");

		// Token: 0x02000CF5 RID: 3317
		public struct FillableBarRequest
		{
			// Token: 0x04003D32 RID: 15666
			public Vector3 center;

			// Token: 0x04003D33 RID: 15667
			public Vector2 size;

			// Token: 0x04003D34 RID: 15668
			public float fillPercent;

			// Token: 0x04003D35 RID: 15669
			public Material filledMat;

			// Token: 0x04003D36 RID: 15670
			public Material unfilledMat;

			// Token: 0x04003D37 RID: 15671
			public float margin;

			// Token: 0x04003D38 RID: 15672
			public Rot4 rotation;

			// Token: 0x04003D39 RID: 15673
			public Vector2 preRotationOffset;
		}
	}
}
