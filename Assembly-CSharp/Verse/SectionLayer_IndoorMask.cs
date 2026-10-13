using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047F RID: 1151
	public class SectionLayer_IndoorMask : SectionLayer
	{
		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001E2D RID: 7725 RVA: 0x0009C473 File Offset: 0x0009A673
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawShadows;
			}
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x0009C47A File Offset: 0x0009A67A
		public SectionLayer_IndoorMask(Section section) : base(section)
		{
			this.relevantChangeTypes = (MapMeshFlagDefOf.Buildings | MapMeshFlagDefOf.Roofs | MapMeshFlagDefOf.FogOfWar);
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x0009C4AC File Offset: 0x0009A6AC
		public override void Regenerate()
		{
			CellRect cellRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
			cellRect.ClipInsideMap(base.Map);
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.IndoorMask);
			LayerSubMesh subMesh2 = base.GetSubMesh(MatBases.RoofedOutdoorMask);
			LayerSubMesh subMesh3 = base.GetSubMesh(MatBases.FilledMask);
			LayerSubMesh subMesh4 = base.GetSubMesh(MatBases.DebugOverlay);
			Map map = base.Map;
			IEnumerable<IntVec3> cells = cellRect.Cells;
			int area = cellRect.Area;
			LayerSubMesh indoorMesh = subMesh;
			LayerSubMesh outdoorMesh = subMesh2;
			LayerSubMesh filledMesh = subMesh3;
			LayerSubMesh debugMesh = subMesh4;
			bool drawIndoorMask = DebugViewSettings.drawIndoorMask;
			bool drawOutdoorMask = DebugViewSettings.drawOutdoorMask;
			SectionLayer_IndoorMask.GenerateSectionLayer(map, cells, area, indoorMesh, outdoorMesh, filledMesh, debugMesh, default(Vector3), drawIndoorMask, drawOutdoorMask);
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x0009C560 File Offset: 0x0009A760
		public static LayerSubMesh BakeGravshipIndoorMesh(Map map, IEnumerable<IntVec3> cells, int cellCount, Material mat, Vector3 center = default(Vector3))
		{
			LayerSubMesh layerSubMesh = MapDrawLayer.CreateFreeSubMesh(mat, null);
			SectionLayer_IndoorMask.GenerateSectionLayer(map, cells, cellCount, layerSubMesh, layerSubMesh, null, null, -center, false, false);
			return layerSubMesh;
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x0009C58C File Offset: 0x0009A78C
		private static void GenerateSectionLayer(Map map, IEnumerable<IntVec3> cells, int cellCount, LayerSubMesh indoorMesh, LayerSubMesh outdoorMesh, LayerSubMesh filledMesh, LayerSubMesh debugMesh, Vector3 meshOffset = default(Vector3), bool debugIndoor = false, bool debugOutdoor = false)
		{
			SectionLayer_IndoorMask.ClearSubMesh(indoorMesh, cellCount);
			SectionLayer_IndoorMask.ClearSubMesh(outdoorMesh, cellCount);
			SectionLayer_IndoorMask.ClearSubMesh(filledMesh, cellCount);
			SectionLayer_IndoorMask.ClearSubMesh(debugMesh, cellCount);
			CellIndices cellIndices = map.cellIndices;
			foreach (IntVec3 intVec in cells)
			{
				float x = (float)intVec.x + meshOffset.x;
				float z = (float)intVec.z + meshOffset.z;
				IntVec3 intVec2 = new IntVec3(intVec.x, 0, intVec.z);
				if (intVec2.InBounds(map))
				{
					bool flag = SectionLayer_IndoorMask.HideCommon(map, intVec2);
					bool flag2 = SectionLayer_IndoorMask.HideRainFogOverlay(map, intVec2);
					if (flag || flag2)
					{
						Building building = map.edificeGrid.InnerArray[cellIndices.CellToIndex(intVec2.x, intVec2.z)];
						float overage;
						if (building != null && (building.def.passability == Traversability.Impassable || building.def.IsDoor))
						{
							overage = 0f;
						}
						else
						{
							overage = 0.16f;
						}
						if (flag)
						{
							Room room = intVec2.GetRoom(map);
							if (room == null || !room.ProperRoom)
							{
								Building edifice = intVec2.GetEdifice(map);
								RoofDef roof = intVec2.GetRoof(map);
								if ((edifice == null || edifice.def.Fillage != FillCategory.Full) && (roof == null || !roof.isThickRoof))
								{
									SectionLayer_IndoorMask.AppendQuadToMesh(outdoorMesh, x, z, 0f);
									if (!debugIndoor && debugOutdoor)
									{
										SectionLayer_IndoorMask.AppendQuadToMesh(debugMesh, x, z, 0f);
										continue;
									}
									continue;
								}
							}
						}
						SectionLayer_IndoorMask.AppendQuadToMesh(indoorMesh, x, z, overage);
						if (debugIndoor)
						{
							SectionLayer_IndoorMask.AppendQuadToMesh(debugMesh, x, z, overage);
						}
					}
				}
			}
			if (indoorMesh != null && !indoorMesh.finalized && indoorMesh.verts.Count > 0)
			{
				indoorMesh.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
			}
			if (outdoorMesh != null && !outdoorMesh.finalized && outdoorMesh.verts.Count > 0)
			{
				outdoorMesh.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
			}
			if (filledMesh != null && !filledMesh.finalized && filledMesh.verts.Count > 0)
			{
				filledMesh.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
			}
			if (debugMesh != null && !debugMesh.finalized && debugMesh.verts.Count > 0)
			{
				debugMesh.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
			}
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x0009C7DC File Offset: 0x0009A9DC
		private static bool HideRainFogOverlay(Map map, IntVec3 c)
		{
			Building edifice = c.GetEdifice(map);
			return ((edifice != null) ? edifice.def.building : null) != null && edifice.def.building.isNaturalRock;
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x0009C81C File Offset: 0x0009AA1C
		private static bool HideCommon(Map map, IntVec3 c)
		{
			if (map.fogGrid.IsFogged(c))
			{
				return true;
			}
			if (c.Roofed(map))
			{
				Building edifice = c.GetEdifice(map);
				if (edifice == null)
				{
					return true;
				}
				if (edifice.def.Fillage != FillCategory.Full)
				{
					return true;
				}
				if (edifice.def.size.x > 1 || edifice.def.size.z > 1)
				{
					return true;
				}
				if (edifice.def.holdsRoof)
				{
					return true;
				}
				if (edifice.def.blockWeather)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x0009C8A6 File Offset: 0x0009AAA6
		private static void ClearSubMesh(LayerSubMesh mesh, int cellCount)
		{
			if (mesh == null)
			{
				return;
			}
			mesh.Clear(MeshParts.All);
			if (!DebugViewSettings.drawIndoorMask && !DebugViewSettings.drawOutdoorMask)
			{
				return;
			}
			mesh.verts.Capacity = cellCount * 2;
			mesh.tris.Capacity = cellCount * 4;
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x0009C8E0 File Offset: 0x0009AAE0
		public static void AppendQuadToMesh(LayerSubMesh mesh, float x, float z, float overage)
		{
			if (mesh == null)
			{
				return;
			}
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			mesh.verts.Add(new Vector3(x - overage, y, z - overage));
			mesh.verts.Add(new Vector3(x - overage, y, z + 1f + overage));
			mesh.verts.Add(new Vector3(x + 1f + overage, y, z + 1f + overage));
			mesh.verts.Add(new Vector3(x + 1f + overage, y, z - overage));
			int count = mesh.verts.Count;
			mesh.tris.Add(count - 4);
			mesh.tris.Add(count - 3);
			mesh.tris.Add(count - 2);
			mesh.tris.Add(count - 4);
			mesh.tris.Add(count - 2);
			mesh.tris.Add(count - 1);
		}
	}
}
