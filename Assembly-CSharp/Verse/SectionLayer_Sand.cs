using System;
using System.Collections.Generic;
using RimWorld;
using Unity.Collections;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000484 RID: 1156
	[StaticConstructorOnStartup]
	public class SectionLayer_Sand : SectionLayer
	{
		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001E53 RID: 7763 RVA: 0x0009DA73 File Offset: 0x0009BC73
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawSand && ModsConfig.OdysseyActive;
			}
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x0009DA83 File Offset: 0x0009BC83
		public SectionLayer_Sand(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Sand;
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x0009DAAC File Offset: 0x0009BCAC
		private bool Filled(int index)
		{
			Building building = base.Map.edificeGrid[index];
			return building != null && building.def.Fillage == FillCategory.Full;
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x0009DAE0 File Offset: 0x0009BCE0
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.Sand);
			if (ModsConfig.BiotechActive)
			{
				subMesh.material.SetTexture(ShaderPropertyIDs.PollutedTex, SectionLayer_Sand.PollutedSandTex.Texture);
			}
			if (subMesh.mesh.vertexCount == 0)
			{
				SectionLayerGeometryMaker_Solid.MakeBaseGeometry(this.section, subMesh, AltitudeLayer.Terrain);
			}
			SectionLayer_Sand.opacityListTmp.Clear();
			subMesh.Clear(MeshParts.Colors);
			if (!ModsConfig.OdysseyActive)
			{
				subMesh.disabled = true;
				return;
			}
			NativeArray<float> depthGrid_Unsafe = base.Map.sandGrid.DepthGrid_Unsafe;
			CellRect cellRect = this.section.CellRect;
			bool flag = false;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					SectionLayer_Sand.opacityListTmp.Clear();
					float num = depthGrid_Unsafe[cellIndices.CellToIndex(i, j)];
					for (int k = 0; k < 9; k++)
					{
						IntVec3 c = new IntVec3(i, 0, j) + GenAdj.AdjacentCellsAndInsideForUV[k];
						this.adjValuesTmp[k] = (c.InBounds(base.Map) ? depthGrid_Unsafe[cellIndices.CellToIndex(c)] : num);
					}
					for (int l = 0; l < 9; l++)
					{
						float num2 = 0f;
						for (int m = 0; m < SectionLayer_Sand.vertexWeights[l].Count; m++)
						{
							num2 += this.adjValuesTmp[SectionLayer_Sand.vertexWeights[l][m]];
						}
						num2 /= (float)SectionLayer_Sand.vertexWeights[l].Count;
						if (num2 > 0.01f)
						{
							flag = true;
						}
						SectionLayer_Sand.opacityListTmp.Add(num2);
					}
					for (int n = 0; n < 9; n++)
					{
						this.adjValuesTmp[n] = (base.Map.pollutionGrid.IsPolluted(new IntVec3(i, 0, j) + GenAdj.AdjacentCellsAndInsideForUV[n]) ? 1f : 0f);
					}
					for (int num3 = 0; num3 < 9; num3++)
					{
						float num4 = 0f;
						for (int num5 = 0; num5 < SectionLayer_Sand.vertexWeights[num3].Count; num5++)
						{
							num4 += this.adjValuesTmp[SectionLayer_Sand.vertexWeights[num3][num5]];
						}
						num4 /= (float)SectionLayer_Sand.vertexWeights[num3].Count;
						float num6 = SectionLayer_Sand.opacityListTmp[num3];
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

		// Token: 0x04001A26 RID: 6694
		private float[] adjValuesTmp = new float[9];

		// Token: 0x04001A27 RID: 6695
		private static readonly List<float> opacityListTmp = new List<float>();

		// Token: 0x04001A28 RID: 6696
		private static readonly CachedTexture PollutedSandTex = new CachedTexture("Other/WindsweptSandPolluted");

		// Token: 0x04001A29 RID: 6697
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
