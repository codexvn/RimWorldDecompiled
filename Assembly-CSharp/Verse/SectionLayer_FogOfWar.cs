using System;
using RimWorld;
using Unity.Collections;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047B RID: 1147
	public class SectionLayer_FogOfWar : SectionLayer
	{
		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001E13 RID: 7699 RVA: 0x0009B956 File Offset: 0x00099B56
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawFog;
			}
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x0009B95D File Offset: 0x00099B5D
		public SectionLayer_FogOfWar(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.FogOfWar;
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x0009B984 File Offset: 0x00099B84
		public override void Regenerate()
		{
			LayerSubMesh subMesh = base.GetSubMesh(MatBases.FogOfWar);
			if (subMesh.mesh.vertexCount == 0)
			{
				SectionLayerGeometryMaker_Solid.MakeBaseGeometry(this.section, subMesh, AltitudeLayer.FogOfWar);
			}
			subMesh.Clear(MeshParts.Colors);
			NativeBitArray fogGrid_Unsafe = base.Map.fogGrid.FogGrid_Unsafe;
			CellRect cellRect = this.section.CellRect;
			int num = base.Map.Size.z - 1;
			int num2 = base.Map.Size.x - 1;
			bool flag = false;
			CellIndices cellIndices = base.Map.cellIndices;
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					if (fogGrid_Unsafe.IsSet(cellIndices.CellToIndex(i, j)))
					{
						for (int k = 0; k < 9; k++)
						{
							this.vertsCovered[k] = true;
						}
					}
					else
					{
						for (int l = 0; l < 9; l++)
						{
							this.vertsCovered[l] = false;
						}
						if (j < num && fogGrid_Unsafe.IsSet(cellIndices.CellToIndex(i, j + 1)))
						{
							this.vertsCovered[2] = true;
							this.vertsCovered[3] = true;
							this.vertsCovered[4] = true;
						}
						if (j > 0 && fogGrid_Unsafe.IsSet(cellIndices.CellToIndex(i, j - 1)))
						{
							this.vertsCovered[6] = true;
							this.vertsCovered[7] = true;
							this.vertsCovered[0] = true;
						}
						if (i < num2 && fogGrid_Unsafe.IsSet(cellIndices.CellToIndex(i + 1, j)))
						{
							this.vertsCovered[4] = true;
							this.vertsCovered[5] = true;
							this.vertsCovered[6] = true;
						}
						if (i > 0 && fogGrid_Unsafe.IsSet(cellIndices.CellToIndex(i - 1, j)))
						{
							this.vertsCovered[0] = true;
							this.vertsCovered[1] = true;
							this.vertsCovered[2] = true;
						}
						if (j > 0 && i > 0 && fogGrid_Unsafe.IsSet(cellIndices.CellToIndex(i - 1, j - 1)))
						{
							this.vertsCovered[0] = true;
						}
						if (j < num && i > 0 && fogGrid_Unsafe.IsSet(cellIndices.CellToIndex(i - 1, j + 1)))
						{
							this.vertsCovered[2] = true;
						}
						if (j < num && i < num2 && fogGrid_Unsafe.IsSet(cellIndices.CellToIndex(i + 1, j + 1)))
						{
							this.vertsCovered[4] = true;
						}
						if (j > 0 && i < num2 && fogGrid_Unsafe.IsSet(cellIndices.CellToIndex(i + 1, j - 1)))
						{
							this.vertsCovered[6] = true;
						}
					}
					for (int m = 0; m < 9; m++)
					{
						byte a;
						if (this.vertsCovered[m])
						{
							a = byte.MaxValue;
							flag = true;
						}
						else
						{
							a = 0;
						}
						subMesh.colors.Add(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, a));
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

		// Token: 0x04001A05 RID: 6661
		private readonly bool[] vertsCovered = new bool[9];
	}
}
