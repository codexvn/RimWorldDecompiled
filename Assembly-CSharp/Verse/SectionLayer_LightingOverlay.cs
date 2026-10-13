using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000480 RID: 1152
	[StaticConstructorOnStartup]
	public class SectionLayer_LightingOverlay : SectionLayer
	{
		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001E36 RID: 7734 RVA: 0x0009C9CD File Offset: 0x0009ABCD
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawLightingOverlay;
			}
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x0009C9D4 File Offset: 0x0009ABD4
		public SectionLayer_LightingOverlay(Section section) : base(section)
		{
			this.relevantChangeTypes = (MapMeshFlagDefOf.Roofs | MapMeshFlagDefOf.GroundGlow);
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x0009C9F8 File Offset: 0x0009ABF8
		public string GlowReportAt(IntVec3 c)
		{
			Color32[] colors = base.GetSubMesh(MatBases.LightOverlay).mesh.colors32;
			int num;
			int num2;
			int num3;
			int num4;
			int num5;
			SectionLayer_LightingOverlay.CalculateVertexIndices(this.sectRect, c.x, c.z, this.firstCenterInd, out num, out num2, out num3, out num4, out num5);
			StringBuilder stringBuilder = new StringBuilder();
			string str = "BL=";
			Color32 color = colors[num];
			stringBuilder.Append(str + color.ToString());
			string str2 = "\nTL=";
			color = colors[num2];
			stringBuilder.Append(str2 + color.ToString());
			string str3 = "\nTR=";
			color = colors[num3];
			stringBuilder.Append(str3 + color.ToString());
			string str4 = "\nBR=";
			color = colors[num4];
			stringBuilder.Append(str4 + color.ToString());
			string str5 = "\nCenter=";
			color = colors[num5];
			stringBuilder.Append(str5 + color.ToString());
			return stringBuilder.ToString();
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x0009CB14 File Offset: 0x0009AD14
		public static LayerSubMesh Bake(Map map, CellRect sectRect, Material mat, Predicate<int> filter)
		{
			LayerSubMesh layerSubMesh = MapDrawLayer.CreateFreeSubMesh(mat ?? MatBases.LightOverlay, map);
			int num = 0;
			SectionLayer_LightingOverlay.GenerateLightingOverlay(map, layerSubMesh, sectRect, ref num, true, filter);
			return layerSubMesh;
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x0009CB44 File Offset: 0x0009AD44
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.LightOverlay);
			if (subMesh.verts.Count == 0)
			{
				this.sectRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
				this.sectRect.ClipInsideMap(base.Map);
			}
			SectionLayer_LightingOverlay.GenerateLightingOverlay(base.Map, subMesh, this.sectRect, ref this.firstCenterInd, false, null);
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x0009CBC8 File Offset: 0x0009ADC8
		private static void GenerateLightingOverlay(Map map, LayerSubMesh subMesh, CellRect rect, ref int firstCenterInd, bool centered = false, Predicate<int> filter = null)
		{
			rect.ClipInsideMap(map);
			if (subMesh.verts.Count == 0)
			{
				SectionLayer_LightingOverlay.MakeBaseGeometry(map, subMesh, rect, out firstCenterInd, centered);
			}
			Color32[] array = new Color32[subMesh.verts.Count];
			int maxX = rect.maxX;
			int maxZ = rect.maxZ;
			int width = rect.Width;
			int x = map.Size.x;
			Thing[] innerArray = map.edificeGrid.InnerArray;
			Thing[] array2 = innerArray;
			int num = array2.Length;
			RoofGrid roofGrid = map.roofGrid;
			CellIndices cellIndices = map.cellIndices;
			int num2;
			int num3;
			int num4;
			int num5;
			int num6;
			SectionLayer_LightingOverlay.CalculateVertexIndices(rect, firstCenterInd, rect.minX, rect.minZ, out num2, out num3, out num4, out num5, out num6);
			int num7 = cellIndices.CellToIndex(new IntVec3(rect.minX, 0, rect.minZ));
			int[] array3 = new int[4];
			array3[0] = -map.Size.x - 1;
			array3[1] = -map.Size.x;
			array3[2] = -1;
			int[] array4 = array3;
			int[] array5 = new int[4];
			array5[0] = -1;
			array5[1] = -1;
			int[] array6 = array5;
			for (int i = rect.minZ; i <= maxZ + 1; i++)
			{
				int num8 = num7 / x;
				int j = rect.minX;
				while (j <= maxX + 1)
				{
					if (filter != null && !filter(num7))
					{
						array[num2] = new Color32(0, 0, 0, 0);
					}
					else
					{
						ColorInt colorInt = new ColorInt(0, 0, 0, 0);
						int num9 = 0;
						bool flag = false;
						for (int k = 0; k < 4; k++)
						{
							int num10 = num7 + array4[k];
							if (num10 >= 0 && num10 < num && num10 / x == num8 + array6[k])
							{
								Thing thing = array2[num10];
								RoofDef roofDef = roofGrid.RoofAt(num10);
								if (roofDef != null && (roofDef.isThickRoof || thing == null || !thing.def.holdsRoof || thing.def.altitudeLayer == AltitudeLayer.DoorMoveable))
								{
									flag = true;
								}
								if (thing == null || !thing.def.blockLight)
								{
									colorInt += map.glowGrid.VisualGlowAt(num10);
									num9++;
								}
							}
						}
						if (num9 > 0)
						{
							array[num2] = (colorInt / num9).ProjectToColor32();
						}
						else
						{
							array[num2] = new Color32(0, 0, 0, 0);
						}
						if (flag && array[num2].a < 100)
						{
							array[num2].a = 100;
						}
					}
					j++;
					num2++;
					num7++;
				}
				int num11 = maxX + 2 - rect.minX;
				num2 -= num11;
				num7 -= num11;
				num2 += width + 1;
				num7 += map.Size.x;
			}
			int num12;
			int num13;
			SectionLayer_LightingOverlay.CalculateVertexIndices(rect, firstCenterInd, rect.minX, rect.minZ, out num12, out num6, out num5, out num4, out num13);
			int num14 = cellIndices.CellToIndex(rect.minX, rect.minZ);
			for (int l = rect.minZ; l <= maxZ; l++)
			{
				int m = rect.minX;
				while (m <= maxX)
				{
					if (filter != null && !filter(num14))
					{
						array[num13] = new Color32(0, 0, 0, 0);
					}
					else
					{
						ColorInt colorInt2 = default(ColorInt) + array[num12];
						colorInt2 += array[num12 + 1];
						colorInt2 += array[num12 + width + 1];
						colorInt2 += array[num12 + width + 2];
						array[num13] = new Color32((byte)(colorInt2.r / 4), (byte)(colorInt2.g / 4), (byte)(colorInt2.b / 4), (byte)(colorInt2.a / 4));
						if (array[num13].a < 100 && roofGrid.Roofed(num14))
						{
							Thing thing2 = array2[num14];
							if (thing2 == null || !thing2.def.holdsRoof)
							{
								array[num13].a = 100;
							}
						}
					}
					m++;
					num12++;
					num13++;
					num14++;
				}
				num12++;
				num14 -= width;
				num14 += map.Size.x;
			}
			subMesh.mesh.colors32 = array;
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x0009D01C File Offset: 0x0009B21C
		private static void MakeBaseGeometry(Map map, LayerSubMesh sm, CellRect sectRect, out int firstCenterInd, bool centered = false)
		{
			sectRect.ClipInsideMap(map);
			float num = centered ? ((float)(-(float)sectRect.minX) - (float)sectRect.Width / 2f) : 0f;
			float num2 = centered ? ((float)(-(float)sectRect.minZ) - (float)sectRect.Height / 2f) : 0f;
			int capacity = (sectRect.Width + 1) * (sectRect.Height + 1) + sectRect.Area;
			float y = AltitudeLayer.LightingOverlay.AltitudeFor();
			sm.verts.Capacity = capacity;
			for (int i = sectRect.minZ; i <= sectRect.maxZ + 1; i++)
			{
				for (int j = sectRect.minX; j <= sectRect.maxX + 1; j++)
				{
					sm.verts.Add(new Vector3((float)j + num, y, (float)i + num2));
				}
			}
			firstCenterInd = sm.verts.Count;
			for (int k = sectRect.minZ; k <= sectRect.maxZ; k++)
			{
				for (int l = sectRect.minX; l <= sectRect.maxX; l++)
				{
					sm.verts.Add(new Vector3((float)l + num + 0.5f, y, (float)k + num2 + 0.5f));
				}
			}
			sm.tris.Capacity = sectRect.Area * 4 * 3;
			for (int m = sectRect.minZ; m <= sectRect.maxZ; m++)
			{
				for (int n = sectRect.minX; n <= sectRect.maxX; n++)
				{
					int item;
					int item2;
					int item3;
					int item4;
					int item5;
					SectionLayer_LightingOverlay.CalculateVertexIndices(sectRect, firstCenterInd, n, m, out item, out item2, out item3, out item4, out item5);
					sm.tris.Add(item);
					sm.tris.Add(item5);
					sm.tris.Add(item4);
					sm.tris.Add(item);
					sm.tris.Add(item2);
					sm.tris.Add(item5);
					sm.tris.Add(item2);
					sm.tris.Add(item3);
					sm.tris.Add(item5);
					sm.tris.Add(item3);
					sm.tris.Add(item4);
					sm.tris.Add(item5);
				}
			}
			sm.FinalizeMesh(MeshParts.Verts | MeshParts.Tris);
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x0009D274 File Offset: 0x0009B474
		private static void CalculateVertexIndices(CellRect sectRect, int firstCenterInd, int worldX, int worldZ, out int botLeft, out int topLeft, out int topRight, out int botRight, out int center)
		{
			int num = worldX - sectRect.minX;
			int num2 = worldZ - sectRect.minZ;
			botLeft = num2 * (sectRect.Width + 1) + num;
			topLeft = (num2 + 1) * (sectRect.Width + 1) + num;
			topRight = (num2 + 1) * (sectRect.Width + 1) + (num + 1);
			botRight = num2 * (sectRect.Width + 1) + (num + 1);
			center = firstCenterInd + (num2 * sectRect.Width + num);
		}

		// Token: 0x04001A0F RID: 6671
		private int firstCenterInd;

		// Token: 0x04001A10 RID: 6672
		private CellRect sectRect;

		// Token: 0x04001A11 RID: 6673
		private const byte RoofedAreaMinSkyCover = 100;
	}
}
