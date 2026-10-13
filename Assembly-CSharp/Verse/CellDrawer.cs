using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003CC RID: 972
	public class CellDrawer
	{
		// Token: 0x060018F9 RID: 6393 RVA: 0x0007DC0C File Offset: 0x0007BE0C
		public CellDrawer(Func<int, Color?> cellIndexToColor, Material material, int mapSizeX, int mapSizeZ)
		{
			this.colorSource = cellIndexToColor;
			this.material = material;
			this.mapSizeX = mapSizeX;
			this.mapSizeZ = mapSizeZ;
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x0007DC43 File Offset: 0x0007BE43
		public void MarkForDraw()
		{
			this.markedForDraw = true;
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x0007DC4C File Offset: 0x0007BE4C
		public void Update()
		{
			if (!this.markedForDraw)
			{
				return;
			}
			this.Draw();
			this.markedForDraw = false;
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x0007DC64 File Offset: 0x0007BE64
		private void Draw()
		{
			if (this.dirty)
			{
				this.RegenerateMesh();
			}
			for (int i = 0; i < this.meshes.Count; i++)
			{
				Graphics.DrawMesh(this.meshes[i], Vector3.zero, Quaternion.identity, this.material, 0);
			}
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x0007DCB7 File Offset: 0x0007BEB7
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x060018FE RID: 6398 RVA: 0x0007DCC0 File Offset: 0x0007BEC0
		public void RegenerateMesh()
		{
			for (int i = 0; i < this.meshes.Count; i++)
			{
				this.meshes[i].Clear();
			}
			int num = 0;
			int num2 = 0;
			if (this.meshes.Count < 1)
			{
				Mesh item = new Mesh
				{
					name = "CellDrawer"
				};
				this.meshes.Add(item);
			}
			Mesh mesh = this.meshes[num];
			CellRect cellRect = new CellRect(0, 0, this.mapSizeX, this.mapSizeZ);
			float y = AltitudeLayer.MapDataOverlay.AltitudeFor();
			for (int j = cellRect.minX; j <= cellRect.maxX; j++)
			{
				for (int k = cellRect.minZ; k <= cellRect.maxZ; k++)
				{
					int arg = CellIndicesUtility.CellToIndex(j, k, this.mapSizeX);
					Color? color = this.colorSource(arg);
					if (color != null)
					{
						CellDrawer.verts.Add(new Vector3((float)j, y, (float)k));
						CellDrawer.verts.Add(new Vector3((float)j, y, (float)(k + 1)));
						CellDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)(k + 1)));
						CellDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)k));
						Color value = color.Value;
						CellDrawer.colors.Add(value);
						CellDrawer.colors.Add(value);
						CellDrawer.colors.Add(value);
						CellDrawer.colors.Add(value);
						int count = CellDrawer.verts.Count;
						CellDrawer.tris.Add(count - 4);
						CellDrawer.tris.Add(count - 3);
						CellDrawer.tris.Add(count - 2);
						CellDrawer.tris.Add(count - 4);
						CellDrawer.tris.Add(count - 2);
						CellDrawer.tris.Add(count - 1);
						num2++;
						if (num2 >= 16383)
						{
							this.FinalizeWorkingDataIntoMesh(mesh);
							num++;
							if (this.meshes.Count < num + 1)
							{
								Mesh item2 = new Mesh
								{
									name = "CellDrawer"
								};
								this.meshes.Add(item2);
							}
							mesh = this.meshes[num];
							num2 = 0;
						}
					}
				}
			}
			this.FinalizeWorkingDataIntoMesh(mesh);
			this.dirty = false;
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x0007DF24 File Offset: 0x0007C124
		private void FinalizeWorkingDataIntoMesh(Mesh mesh)
		{
			if (CellDrawer.verts.Count > 0)
			{
				mesh.SetVertices(CellDrawer.verts);
				CellDrawer.verts.Clear();
				mesh.SetTriangles(CellDrawer.tris, 0);
				CellDrawer.tris.Clear();
				mesh.SetColors(CellDrawer.colors);
				CellDrawer.colors.Clear();
			}
		}

		// Token: 0x04001605 RID: 5637
		private bool markedForDraw;

		// Token: 0x04001606 RID: 5638
		private Material material;

		// Token: 0x04001607 RID: 5639
		private bool dirty = true;

		// Token: 0x04001608 RID: 5640
		private List<Mesh> meshes = new List<Mesh>();

		// Token: 0x04001609 RID: 5641
		private int mapSizeX;

		// Token: 0x0400160A RID: 5642
		private int mapSizeZ;

		// Token: 0x0400160B RID: 5643
		private Func<int, Color?> colorSource;

		// Token: 0x0400160C RID: 5644
		private static List<Vector3> verts = new List<Vector3>();

		// Token: 0x0400160D RID: 5645
		private static List<int> tris = new List<int>();

		// Token: 0x0400160E RID: 5646
		private static List<Color> colors = new List<Color>();

		// Token: 0x0400160F RID: 5647
		private const int MaxCellsPerMesh = 16383;
	}
}
