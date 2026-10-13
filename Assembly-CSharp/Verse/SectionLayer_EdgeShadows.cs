using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000479 RID: 1145
	public class SectionLayer_EdgeShadows : SectionLayer_Dynamic
	{
		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001E0A RID: 7690 RVA: 0x0009ADA4 File Offset: 0x00098FA4
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

		// Token: 0x06001E0B RID: 7691 RVA: 0x0009ADF8 File Offset: 0x00098FF8
		public SectionLayer_EdgeShadows(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Buildings;
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x0009AE14 File Offset: 0x00099014
		public override bool ShouldDrawDynamic(CellRect view)
		{
			return this.section.CellRect.Overlaps(SectionLayer_SunShadows.GetSunShadowsViewRect(this.section.map, view));
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x0009AE45 File Offset: 0x00099045
		public override CellRect GetBoundaryRect()
		{
			return new CellRect(0, 0, this.section.map.Size.x, this.section.map.Size.z);
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x0009AE78 File Offset: 0x00099078
		public override void Regenerate()
		{
			Building[] innerArray = base.Map.edificeGrid.InnerArray;
			float y = AltitudeLayer.Shadows.AltitudeFor();
			CellRect cellRect = new CellRect(this.section.botLeft.x, this.section.botLeft.z, 17, 17);
			cellRect.ClipInsideMap(base.Map);
			LayerSubMesh sm = base.GetSubMesh(MatBases.EdgeShadow);
			sm.Clear(MeshParts.All);
			sm.verts.Capacity = cellRect.Area * 4;
			sm.colors.Capacity = cellRect.Area * 4;
			sm.tris.Capacity = cellRect.Area * 8;
			bool[] array = new bool[4];
			bool[] array2 = new bool[4];
			bool[] array3 = new bool[4];
			CellIndices cellIndices = base.Map.cellIndices;
			Action<int> <>9__0;
			Action <>9__1;
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					Thing thing = innerArray[cellIndices.CellToIndex(i, j)];
					if (thing != null && thing.def.castEdgeShadows)
					{
						sm.verts.Add(new Vector3((float)i, y, (float)j));
						sm.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
						sm.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
						sm.verts.Add(new Vector3((float)(i + 1), y, (float)j));
						sm.colors.Add(SectionLayer_EdgeShadows.Shadowed);
						sm.colors.Add(SectionLayer_EdgeShadows.Shadowed);
						sm.colors.Add(SectionLayer_EdgeShadows.Shadowed);
						sm.colors.Add(SectionLayer_EdgeShadows.Shadowed);
						int count = sm.verts.Count;
						sm.tris.Add(count - 4);
						sm.tris.Add(count - 3);
						sm.tris.Add(count - 2);
						sm.tris.Add(count - 4);
						sm.tris.Add(count - 2);
						sm.tris.Add(count - 1);
					}
					else
					{
						array[0] = false;
						array[1] = false;
						array[2] = false;
						array[3] = false;
						array2[0] = false;
						array2[1] = false;
						array2[2] = false;
						array2[3] = false;
						array3[0] = false;
						array3[1] = false;
						array3[2] = false;
						array3[3] = false;
						IntVec3 a = new IntVec3(i, 0, j);
						IntVec3[] cardinalDirectionsAround = GenAdj.CardinalDirectionsAround;
						for (int k = 0; k < 4; k++)
						{
							IntVec3 c = a + cardinalDirectionsAround[k];
							if (c.InBounds(base.Map))
							{
								thing = innerArray[cellIndices.CellToIndex(c)];
								if (thing != null && thing.def.castEdgeShadows)
								{
									array2[k] = true;
									array[(k + 3) % 4] = true;
									array[k] = true;
								}
							}
						}
						IntVec3[] diagonalDirectionsAround = GenAdj.DiagonalDirectionsAround;
						for (int l = 0; l < 4; l++)
						{
							if (!array[l])
							{
								IntVec3 c = a + diagonalDirectionsAround[l];
								if (c.InBounds(base.Map))
								{
									thing = innerArray[cellIndices.CellToIndex(c)];
									if (thing != null && thing.def.castEdgeShadows)
									{
										array[l] = true;
										array3[l] = true;
									}
								}
							}
						}
						Action<int> action;
						if ((action = <>9__0) == null)
						{
							action = (<>9__0 = delegate(int idx)
							{
								sm.tris.Add(sm.verts.Count - 2);
								sm.tris.Add(idx);
								sm.tris.Add(sm.verts.Count - 1);
								sm.tris.Add(sm.verts.Count - 1);
								sm.tris.Add(idx);
								sm.tris.Add(idx + 1);
							});
						}
						Action<int> action2 = action;
						Action action3;
						if ((action3 = <>9__1) == null)
						{
							action3 = (<>9__1 = delegate()
							{
								sm.colors.Add(SectionLayer_EdgeShadows.Shadowed);
								sm.colors.Add(SectionLayer_EdgeShadows.Lit);
								sm.colors.Add(SectionLayer_EdgeShadows.Lit);
								sm.tris.Add(sm.verts.Count - 3);
								sm.tris.Add(sm.verts.Count - 2);
								sm.tris.Add(sm.verts.Count - 1);
							});
						}
						Action action4 = action3;
						int count2 = sm.verts.Count;
						if (array[0])
						{
							if (array2[0] || array2[1])
							{
								float num2;
								float num = num2 = 0f;
								if (array2[0])
								{
									num = 0.45f;
								}
								if (array2[1])
								{
									num2 = 0.45f;
								}
								sm.verts.Add(new Vector3((float)i, y, (float)j));
								sm.colors.Add(SectionLayer_EdgeShadows.Shadowed);
								sm.verts.Add(new Vector3((float)i + num2, y, (float)j + num));
								sm.colors.Add(SectionLayer_EdgeShadows.Lit);
								if (array[1] && !array3[1])
								{
									action2(sm.verts.Count);
								}
							}
							else
							{
								sm.verts.Add(new Vector3((float)i, y, (float)j));
								sm.verts.Add(new Vector3((float)i, y, (float)j + 0.45f));
								sm.verts.Add(new Vector3((float)i + 0.45f, y, (float)j));
								action4();
							}
						}
						if (array[1])
						{
							if (array2[1] || array2[2])
							{
								float num2;
								float num = num2 = 0f;
								if (array2[1])
								{
									num2 = 0.45f;
								}
								if (array2[2])
								{
									num = -0.45f;
								}
								sm.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
								sm.colors.Add(SectionLayer_EdgeShadows.Shadowed);
								sm.verts.Add(new Vector3((float)i + num2, y, (float)(j + 1) + num));
								sm.colors.Add(SectionLayer_EdgeShadows.Lit);
								if (array[2] && !array3[2])
								{
									action2(sm.verts.Count);
								}
							}
							else
							{
								sm.verts.Add(new Vector3((float)i, y, (float)(j + 1)));
								sm.verts.Add(new Vector3((float)i + 0.45f, y, (float)(j + 1)));
								sm.verts.Add(new Vector3((float)i, y, (float)(j + 1) - 0.45f));
								action4();
							}
						}
						if (array[2])
						{
							if (array2[2] || array2[3])
							{
								float num2;
								float num = num2 = 0f;
								if (array2[2])
								{
									num = -0.45f;
								}
								if (array2[3])
								{
									num2 = -0.45f;
								}
								sm.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
								sm.colors.Add(SectionLayer_EdgeShadows.Shadowed);
								sm.verts.Add(new Vector3((float)(i + 1) + num2, y, (float)(j + 1) + num));
								sm.colors.Add(SectionLayer_EdgeShadows.Lit);
								if (array[3] && !array3[3])
								{
									action2(sm.verts.Count);
								}
							}
							else
							{
								sm.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1)));
								sm.verts.Add(new Vector3((float)(i + 1), y, (float)(j + 1) - 0.45f));
								sm.verts.Add(new Vector3((float)(i + 1) - 0.45f, y, (float)(j + 1)));
								action4();
							}
						}
						if (array[3])
						{
							if (array2[3] || array2[0])
							{
								float num2;
								float num = num2 = 0f;
								if (array2[3])
								{
									num2 = -0.45f;
								}
								if (array2[0])
								{
									num = 0.45f;
								}
								sm.verts.Add(new Vector3((float)(i + 1), y, (float)j));
								sm.colors.Add(SectionLayer_EdgeShadows.Shadowed);
								sm.verts.Add(new Vector3((float)(i + 1) + num2, y, (float)j + num));
								sm.colors.Add(SectionLayer_EdgeShadows.Lit);
								if (array[0] && !array3[0])
								{
									action2(count2);
								}
							}
							else
							{
								sm.verts.Add(new Vector3((float)(i + 1), y, (float)j));
								sm.verts.Add(new Vector3((float)(i + 1) - 0.45f, y, (float)j));
								sm.verts.Add(new Vector3((float)(i + 1), y, (float)j + 0.45f));
								action4();
							}
						}
					}
				}
			}
			if (sm.verts.Count > 0)
			{
				sm.FinalizeMesh(MeshParts.Verts | MeshParts.Tris | MeshParts.Colors);
			}
		}

		// Token: 0x040019FE RID: 6654
		private const float InDist = 0.45f;

		// Token: 0x040019FF RID: 6655
		private const byte ShadowBrightness = 195;

		// Token: 0x04001A00 RID: 6656
		private static readonly Color32 Shadowed = new Color32(195, 195, 195, byte.MaxValue);

		// Token: 0x04001A01 RID: 6657
		private static readonly Color32 Lit = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
	}
}
