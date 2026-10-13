using System;
using System.Collections.Generic;
using RimWorld;
using Unity.Collections;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000485 RID: 1157
	[StaticConstructorOnStartup]
	public class SectionLayer_Snow : SectionLayer
	{
		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001E58 RID: 7768 RVA: 0x0009DF22 File Offset: 0x0009C122
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawSnow;
			}
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x0009DF29 File Offset: 0x0009C129
		public SectionLayer_Snow(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Snow;
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x0009DF50 File Offset: 0x0009C150
		private bool Filled(int index)
		{
			Building building = base.Map.edificeGrid[index];
			return building != null && building.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x0009DF84 File Offset: 0x0009C184
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.Snow);
			if (ModsConfig.BiotechActive)
			{
				subMesh.material.SetTexture(ShaderPropertyIDs.PollutedTex, SectionLayer_Snow.PollutedSnowTex.Texture);
			}
			if (subMesh.mesh.vertexCount == 0)
			{
				SectionLayerGeometryMaker_Solid.MakeBaseGeometry(this.section, subMesh, AltitudeLayer.Terrain);
			}
			SectionLayer_Snow.opacityListTmp.Clear();
			subMesh.Clear(MeshParts.Colors);
			NativeArray<float> depthGrid_Unsafe = base.Map.snowGrid.DepthGrid_Unsafe;
			CellRect cellRect = this.section.CellRect;
			bool flag = false;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					SectionLayer_Snow.opacityListTmp.Clear();
					float num = depthGrid_Unsafe[cellIndices.CellToIndex(i, j)];
					for (int k = 0; k < 9; k++)
					{
						IntVec3 c = new IntVec3(i, 0, j) + GenAdj.AdjacentCellsAndInsideForUV[k];
						this.adjValuesTmp[k] = (c.InBounds(base.Map) ? depthGrid_Unsafe[cellIndices.CellToIndex(c)] : num);
					}
					for (int l = 0; l < 9; l++)
					{
						float num2 = 0f;
						for (int m = 0; m < SectionLayer_Snow.vertexWeights[l].Count; m++)
						{
							num2 += this.adjValuesTmp[SectionLayer_Snow.vertexWeights[l][m]];
						}
						num2 /= (float)SectionLayer_Snow.vertexWeights[l].Count;
						if (num2 > 0.01f)
						{
							flag = true;
						}
						SectionLayer_Snow.opacityListTmp.Add(num2);
					}
					for (int n = 0; n < 9; n++)
					{
						this.adjValuesTmp[n] = (base.Map.pollutionGrid.IsPolluted(new IntVec3(i, 0, j) + GenAdj.AdjacentCellsAndInsideForUV[n]) ? 1f : 0f);
					}
					for (int num3 = 0; num3 < 9; num3++)
					{
						float num4 = 0f;
						for (int num5 = 0; num5 < SectionLayer_Snow.vertexWeights[num3].Count; num5++)
						{
							num4 += this.adjValuesTmp[SectionLayer_Snow.vertexWeights[num3][num5]];
						}
						num4 /= (float)SectionLayer_Snow.vertexWeights[num3].Count;
						float num6 = SectionLayer_Snow.opacityListTmp[num3];
						subMesh.colors.Add(new Color32(Convert.ToByte(num4 * 255f), byte.MaxValue, byte.MaxValue, Convert.ToByte(num6 * 255f)));
					}
				}
			}
			if (flag)
			{
				subMesh.disabled = false;
				subMesh.FinalizeMesh(MeshParts.Colors);
				return;
			}
			subMesh.disabled = true;
		}

		// Token: 0x04001A2A RID: 6698
		private float[] adjValuesTmp = new float[9];

		// Token: 0x04001A2B RID: 6699
		private static readonly List<float> opacityListTmp = new List<float>();

		// Token: 0x04001A2C RID: 6700
		private static readonly CachedTexture PollutedSnowTex = new CachedTexture("Other/SnowPolluted");

		// Token: 0x04001A2D RID: 6701
		public static readonly List<List<int>> vertexWeights = new List<List<int>>
		{
			new List<int>
			{
				0,
				1,
				2,
				8
			},
			new List<int>
			{
				2,
				8
			},
			new List<int>
			{
				2,
				3,
				4,
				8
			},
			new List<int>
			{
				4,
				8
			},
			new List<int>
			{
				4,
				5,
				6,
				8
			},
			new List<int>
			{
				6,
				8
			},
			new List<int>
			{
				6,
				7,
				0,
				8
			},
			new List<int>
			{
				0,
				8
			},
			new List<int>
			{
				8
			}
		};
	}
}
