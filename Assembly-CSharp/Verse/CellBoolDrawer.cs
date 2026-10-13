using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003CA RID: 970
	public class CellBoolDrawer
	{
		// Token: 0x060018E9 RID: 6377 RVA: 0x0007D6FC File Offset: 0x0007B8FC
		private CellBoolDrawer(int mapSizeX, int mapSizeZ, float opacity = 0.33f)
		{
			this.mapSizeX = mapSizeX;
			this.mapSizeZ = mapSizeZ;
			this.opacity = opacity;
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x0007D74C File Offset: 0x0007B94C
		public CellBoolDrawer(ICellBoolGiver giver, int mapSizeX, int mapSizeZ, float opacity = 0.33f) : this(mapSizeX, mapSizeZ, opacity)
		{
			this.colorGetter = (() => giver.Color);
			this.extraColorGetter = new Func<int, Color>(giver.GetCellExtraColor);
			this.cellBoolGetter = new Func<int, bool>(giver.GetCellBool);
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x0007D7B2 File Offset: 0x0007B9B2
		public CellBoolDrawer(ICellBoolGiver giver, int mapSizeX, int mapSizeZ, int renderQueue, float opacity = 0.33f) : this(giver, mapSizeX, mapSizeZ, opacity)
		{
			this.renderQueue = renderQueue;
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x0007D7C7 File Offset: 0x0007B9C7
		public CellBoolDrawer(Func<int, bool> cellBoolGetter, Func<Color> colorGetter, Func<int, Color> extraColorGetter, int mapSizeX, int mapSizeZ, float opacity = 0.33f) : this(mapSizeX, mapSizeZ, opacity)
		{
			this.colorGetter = colorGetter;
			this.extraColorGetter = extraColorGetter;
			this.cellBoolGetter = cellBoolGetter;
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x0007D7EA File Offset: 0x0007B9EA
		public CellBoolDrawer(Func<int, bool> cellBoolGetter, Func<Color> colorGetter, Func<int, Color> extraColorGetter, int mapSizeX, int mapSizeZ, int renderQueue, float opacity = 0.33f) : this(cellBoolGetter, colorGetter, extraColorGetter, mapSizeX, mapSizeZ, opacity)
		{
			this.renderQueue = renderQueue;
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x0007D803 File Offset: 0x0007BA03
		public void MarkForDraw()
		{
			this.wantDraw = true;
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x0007D80C File Offset: 0x0007BA0C
		public void CellBoolDrawerUpdate()
		{
			if (!this.wantDraw)
			{
				return;
			}
			this.ActuallyDraw();
			this.wantDraw = false;
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x0007D824 File Offset: 0x0007BA24
		private void ActuallyDraw()
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

		// Token: 0x060018F1 RID: 6385 RVA: 0x0007D877 File Offset: 0x0007BA77
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x0007D880 File Offset: 0x0007BA80
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
					name = "CellBoolDrawer"
				};
				this.meshes.Add(item);
			}
			Mesh mesh = this.meshes[num];
			CellRect cellRect = new CellRect(0, 0, this.mapSizeX, this.mapSizeZ);
			float y = AltitudeLayer.MapDataOverlay.AltitudeFor();
			bool careAboutVertexColors = false;
			for (int j = cellRect.minX; j <= cellRect.maxX; j++)
			{
				for (int k = cellRect.minZ; k <= cellRect.maxZ; k++)
				{
					int arg = CellIndicesUtility.CellToIndex(j, k, this.mapSizeX);
					if (this.cellBoolGetter(arg))
					{
						CellBoolDrawer.verts.Add(new Vector3((float)j, y, (float)k));
						CellBoolDrawer.verts.Add(new Vector3((float)j, y, (float)(k + 1)));
						CellBoolDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)(k + 1)));
						CellBoolDrawer.verts.Add(new Vector3((float)(j + 1), y, (float)k));
						Color color = this.extraColorGetter(arg);
						CellBoolDrawer.colors.Add(color);
						CellBoolDrawer.colors.Add(color);
						CellBoolDrawer.colors.Add(color);
						CellBoolDrawer.colors.Add(color);
						if (color != Color.white)
						{
							careAboutVertexColors = true;
						}
						int count = CellBoolDrawer.verts.Count;
						CellBoolDrawer.tris.Add(count - 4);
						CellBoolDrawer.tris.Add(count - 3);
						CellBoolDrawer.tris.Add(count - 2);
						CellBoolDrawer.tris.Add(count - 4);
						CellBoolDrawer.tris.Add(count - 2);
						CellBoolDrawer.tris.Add(count - 1);
						num2++;
						if (num2 >= 16383)
						{
							this.FinalizeWorkingDataIntoMesh(mesh);
							num++;
							if (this.meshes.Count < num + 1)
							{
								Mesh item2 = new Mesh
								{
									name = "CellBoolDrawer"
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
			this.CreateMaterialIfNeeded(careAboutVertexColors);
			this.dirty = false;
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x0007DAFC File Offset: 0x0007BCFC
		private void FinalizeWorkingDataIntoMesh(Mesh mesh)
		{
			if (CellBoolDrawer.verts.Count > 0)
			{
				mesh.SetVertices(CellBoolDrawer.verts);
				CellBoolDrawer.verts.Clear();
				mesh.SetTriangles(CellBoolDrawer.tris, 0);
				CellBoolDrawer.tris.Clear();
				mesh.SetColors(CellBoolDrawer.colors);
				CellBoolDrawer.colors.Clear();
			}
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x0007DB58 File Offset: 0x0007BD58
		private void CreateMaterialIfNeeded(bool careAboutVertexColors)
		{
			if (this.material == null || this.materialCaresAboutVertexColors != careAboutVertexColors)
			{
				Color color = this.colorGetter();
				this.material = SolidColorMaterials.SimpleSolidColorMaterial(new Color(color.r, color.g, color.b, this.opacity * color.a), careAboutVertexColors);
				this.materialCaresAboutVertexColors = careAboutVertexColors;
				this.material.renderQueue = this.renderQueue;
			}
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x0007DBD0 File Offset: 0x0007BDD0
		public void Notify_ColorChanged()
		{
			this.material = null;
			this.SetDirty();
		}

		// Token: 0x040015F3 RID: 5619
		private bool wantDraw;

		// Token: 0x040015F4 RID: 5620
		private Material material;

		// Token: 0x040015F5 RID: 5621
		private bool materialCaresAboutVertexColors;

		// Token: 0x040015F6 RID: 5622
		private bool dirty = true;

		// Token: 0x040015F7 RID: 5623
		private List<Mesh> meshes = new List<Mesh>();

		// Token: 0x040015F8 RID: 5624
		private int mapSizeX;

		// Token: 0x040015F9 RID: 5625
		private int mapSizeZ;

		// Token: 0x040015FA RID: 5626
		private float opacity = 0.33f;

		// Token: 0x040015FB RID: 5627
		private int renderQueue = 3600;

		// Token: 0x040015FC RID: 5628
		private Func<Color> colorGetter;

		// Token: 0x040015FD RID: 5629
		private Func<int, Color> extraColorGetter;

		// Token: 0x040015FE RID: 5630
		private Func<int, bool> cellBoolGetter;

		// Token: 0x040015FF RID: 5631
		private static List<Vector3> verts = new List<Vector3>();

		// Token: 0x04001600 RID: 5632
		private static List<int> tris = new List<int>();

		// Token: 0x04001601 RID: 5633
		private static List<Color> colors = new List<Color>();

		// Token: 0x04001602 RID: 5634
		private const float DefaultOpacity = 0.33f;

		// Token: 0x04001603 RID: 5635
		private const int MaxCellsPerMesh = 16383;
	}
}
