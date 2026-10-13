using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200046A RID: 1130
	public class LayerSubMesh
	{
		// Token: 0x06001DA5 RID: 7589 RVA: 0x00096EE8 File Offset: 0x000950E8
		public LayerSubMesh(Mesh mesh, Material material, Bounds? bounds = null)
		{
			this.mesh = mesh;
			this.material = material;
		}

		// Token: 0x06001DA6 RID: 7590 RVA: 0x00096F58 File Offset: 0x00095158
		public void Clear(MeshParts parts)
		{
			if ((parts & MeshParts.Verts) != MeshParts.None)
			{
				this.verts.Clear();
			}
			if ((parts & MeshParts.Tris) != MeshParts.None)
			{
				this.tris.Clear();
			}
			if ((parts & MeshParts.Colors) != MeshParts.None)
			{
				this.colors.Clear();
			}
			if ((parts & MeshParts.UVs1) != MeshParts.None)
			{
				this.uvs.Clear();
			}
			if ((parts & MeshParts.UVs2) != MeshParts.None)
			{
				this.uvsChannelTwo.Clear();
			}
			if ((parts & MeshParts.Normals) != MeshParts.None)
			{
				this.normals.Clear();
			}
			this.finalized = false;
		}

		// Token: 0x06001DA7 RID: 7591 RVA: 0x00096FD0 File Offset: 0x000951D0
		public void FinalizeMesh(MeshParts parts)
		{
			if (this.finalized)
			{
				Log.Warning("Finalizing mesh which is already finalized. Did you forget to call Clear()?");
			}
			if ((parts & MeshParts.Verts) != MeshParts.None || (parts & MeshParts.Tris) != MeshParts.None)
			{
				this.mesh.Clear();
			}
			if ((parts & MeshParts.Verts) != MeshParts.None)
			{
				if (this.verts.Count > 0)
				{
					this.mesh.SetVertices(this.verts);
				}
				else
				{
					Log.Error(string.Format("Cannot cook Verts for {0}: no ingredients data. If you want to not render this submesh, disable it.", this.material));
				}
			}
			if ((parts & MeshParts.Tris) != MeshParts.None)
			{
				if (this.tris.Count > 0)
				{
					this.mesh.SetTriangles(this.tris, 0);
				}
				else
				{
					Log.Error(string.Format("Cannot cook Tris for {0}: no ingredients data.", this.material));
				}
			}
			if ((parts & MeshParts.Colors) != MeshParts.None && this.colors.Count > 0)
			{
				this.mesh.SetColors(this.colors);
			}
			if ((parts & MeshParts.UVs1) != MeshParts.None && this.uvs.Count > 0)
			{
				this.mesh.SetUVs(0, this.uvs);
			}
			if ((parts & MeshParts.UVs2) != MeshParts.None && this.uvsChannelTwo.Count > 0)
			{
				this.mesh.SetUVs(1, this.uvsChannelTwo);
			}
			if ((parts & MeshParts.Normals) != MeshParts.None && this.normals.Count > 0)
			{
				this.mesh.SetNormals(this.normals);
			}
			this.finalized = true;
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x00097116 File Offset: 0x00095316
		public override string ToString()
		{
			return string.Format("LayerSubMesh({0})", this.material);
		}

		// Token: 0x040019C4 RID: 6596
		public bool finalized;

		// Token: 0x040019C5 RID: 6597
		public bool disabled;

		// Token: 0x040019C6 RID: 6598
		public Material material;

		// Token: 0x040019C7 RID: 6599
		public int renderLayer;

		// Token: 0x040019C8 RID: 6600
		public Mesh mesh;

		// Token: 0x040019C9 RID: 6601
		public List<Vector3> verts = new List<Vector3>();

		// Token: 0x040019CA RID: 6602
		public List<int> tris = new List<int>();

		// Token: 0x040019CB RID: 6603
		public List<Color32> colors = new List<Color32>();

		// Token: 0x040019CC RID: 6604
		public List<Vector3> pollution = new List<Vector3>();

		// Token: 0x040019CD RID: 6605
		public List<Vector3> uvs = new List<Vector3>();

		// Token: 0x040019CE RID: 6606
		public List<Vector3> uvsChannelTwo = new List<Vector3>();

		// Token: 0x040019CF RID: 6607
		public List<Vector3> normals = new List<Vector3>();
	}
}
