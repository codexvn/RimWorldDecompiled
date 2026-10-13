using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000486 RID: 1158
	internal class SectionLayer_SunShadows : SectionLayer_Dynamic
	{
		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001E5D RID: 7773 RVA: 0x0009E3B8 File Offset: 0x0009C5B8
		public override bool Visible
		{
			get
			{
				if (DebugViewSettings.drawShadows)
				{
					Map map = base.Map;
					bool? flag;
					if (map == null)
					{
						flag = null;
					}
					else
					{
						BiomeDef biome = map.Biome;
						flag = ((biome != null) ? new bool?(biome.disableShadows) : null);
					}
					bool? flag2 = flag;
					return !flag2.GetValueOrDefault();
				}
				return false;
			}
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x0009ADF8 File Offset: 0x00098FF8
		public SectionLayer_SunShadows(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Buildings;
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x0009E40C File Offset: 0x0009C60C
		public override bool ShouldDrawDynamic(CellRect view)
		{
			return this.section.CellRect.Overlaps(SectionLayer_SunShadows.GetSunShadowsViewRect(this.section.map, view));
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x0009AE45 File Offset: 0x00099045
		public override CellRect GetBoundaryRect()
		{
			return new CellRect(0, 0, this.section.map.Size.x, this.section.map.Size.z);
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x0009E43D File Offset: 0x0009C63D
		public override void DrawLayer()
		{
			this.RefreshSubMeshBounds();
			base.DrawLayer();
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x0009E44C File Offset: 0x0009C64C
		public override void Regenerate()
		{
			if (!MatBases.SunShadow.shader.isSupported)
			{
				return;
			}
			Building[] innerArray = base.Map.edificeGrid.InnerArray;
			float y = AltitudeLayer.Shadows.AltitudeFor();
			CellRect cellRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
			cellRect.ClipInsideMap(base.Map);
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.SunShadow);
			subMesh.Clear(MeshParts.All);
			subMesh.verts.Capacity = cellRect.Area * 2;
			subMesh.tris.Capacity = cellRect.Area * 4;
			subMesh.colors.Capacity = cellRect.Area * 2;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					Building building = innerArray[cellIndices.CellToIndex(i, j)];
					if (building != null && building.def.staticSunShadowHeight > 0f)
					{
						float staticSunShadowHeight = building.def.staticSunShadowHeight;
						Color32 item = new Color32(0, 0, 0, (byte)(255f * staticSunShadowHeight));
						int count = subMesh.verts.Count;
						subMesh.verts.Add(new Vector3((float)i, y, (float)j));
						subMesh.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
						subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)j));
						subMesh.colors.Add(SectionLayer_SunShadows.LowVertexColor);
						subMesh.colors.Add(SectionLayer_SunShadows.LowVertexColor);
						subMesh.colors.Add(SectionLayer_SunShadows.LowVertexColor);
						subMesh.colors.Add(SectionLayer_SunShadows.LowVertexColor);
						int count2 = subMesh.verts.Count;
						subMesh.tris.Add(count2 - 4);
						subMesh.tris.Add(count2 - 3);
						subMesh.tris.Add(count2 - 2);
						subMesh.tris.Add(count2 - 4);
						subMesh.tris.Add(count2 - 2);
						subMesh.tris.Add(count2 - 1);
						if (i > 0)
						{
							building = innerArray[cellIndices.CellToIndex(i - 1, j)];
							if (building == null || building.def.staticSunShadowHeight < staticSunShadowHeight)
							{
								int count3 = subMesh.verts.Count;
								subMesh.verts.Add(new Vector3((float)i, y, (float)j));
								subMesh.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
								subMesh.colors.Add(item);
								subMesh.colors.Add(item);
								subMesh.tris.Add(count + 1);
								subMesh.tris.Add(count);
								subMesh.tris.Add(count3);
								subMesh.tris.Add(count3);
								subMesh.tris.Add(count3 + 1);
								subMesh.tris.Add(count + 1);
							}
						}
						if (i < base.Map.Size.x - 1)
						{
							building = innerArray[cellIndices.CellToIndex(i + 1, j)];
							if (building == null || building.def.staticSunShadowHeight < staticSunShadowHeight)
							{
								int count4 = subMesh.verts.Count;
								subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
								subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)j));
								subMesh.colors.Add(item);
								subMesh.colors.Add(item);
								subMesh.tris.Add(count + 2);
								subMesh.tris.Add(count4);
								subMesh.tris.Add(count4 + 1);
								subMesh.tris.Add(count4 + 1);
								subMesh.tris.Add(count + 3);
								subMesh.tris.Add(count + 2);
							}
						}
						if (j > 0)
						{
							building = innerArray[cellIndices.CellToIndex(i, j - 1)];
							if (building == null || building.def.staticSunShadowHeight < staticSunShadowHeight)
							{
								int count5 = subMesh.verts.Count;
								subMesh.verts.Add(new Vector3((float)i, y, (float)j));
								subMesh.verts.Add(new Vector3((float)(i + 1), y, (float)j));
								subMesh.colors.Add(item);
								subMesh.colors.Add(item);
								subMesh.tris.Add(count);
								subMesh.tris.Add(count + 3);
								subMesh.tris.Add(count5);
								subMesh.tris.Add(count + 3);
								subMesh.tris.Add(count5 + 1);
								subMesh.tris.Add(count5);
							}
						}
					}
				}
			}
			if (subMesh.verts.Count > 0)
			{
				subMesh.FinalizeMesh(MeshParts.Verts | MeshParts.Tris | MeshParts.Colors);
				float num = Mathf.Max(15f, 15f);
				Vector3 size = subMesh.mesh.bounds.size;
				size.x += 2f * num + 2f;
				size.z += 2f * num + 2f;
				subMesh.mesh.bounds = new Bounds(Vector3.zero, new Vector3(1000f, 1000f, 1000f));
			}
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x0009EA04 File Offset: 0x0009CC04
		public static CellRect GetSunShadowsViewRect(Map map, CellRect rect)
		{
			if (SectionLayer_SunShadows.frame == RealTime.frameCount)
			{
				return SectionLayer_SunShadows.cachedRect;
			}
			GenCelestial.LightInfo lightSourceInfo = GenCelestial.GetLightSourceInfo(map, GenCelestial.LightType.Shadow);
			if (lightSourceInfo.vector.x < 0f)
			{
				rect.maxX -= Mathf.FloorToInt(lightSourceInfo.vector.x);
			}
			else
			{
				rect.minX -= Mathf.CeilToInt(lightSourceInfo.vector.x);
			}
			if (lightSourceInfo.vector.y < 0f)
			{
				rect.maxZ -= Mathf.FloorToInt(lightSourceInfo.vector.y);
			}
			else
			{
				rect.minZ -= Mathf.CeilToInt(lightSourceInfo.vector.y);
			}
			SectionLayer_SunShadows.frame = RealTime.frameCount;
			return SectionLayer_SunShadows.cachedRect = rect.ClipInsideMap(map);
		}

		// Token: 0x04001A2E RID: 6702
		private static readonly Color32 LowVertexColor = new Color32(0, 0, 0, 0);

		// Token: 0x04001A2F RID: 6703
		private static int frame;

		// Token: 0x04001A30 RID: 6704
		private static CellRect cachedRect;
	}
}
