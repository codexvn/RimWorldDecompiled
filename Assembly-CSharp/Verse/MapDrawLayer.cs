using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200046C RID: 1132
	public abstract class MapDrawLayer
	{
		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06001DBA RID: 7610 RVA: 0x000979E8 File Offset: 0x00095BE8
		protected Map Map
		{
			get
			{
				return this.map;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06001DBB RID: 7611 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06001DBC RID: 7612 RVA: 0x000979F0 File Offset: 0x00095BF0
		// (set) Token: 0x06001DBD RID: 7613 RVA: 0x000979F8 File Offset: 0x00095BF8
		public bool Dirty { get; set; }

		// Token: 0x06001DBE RID: 7614 RVA: 0x00097A01 File Offset: 0x00095C01
		public MapDrawLayer(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x00097A1C File Offset: 0x00095C1C
		public static LayerSubMesh CreateFreeSubMesh(Material material, Map map = null)
		{
			if (material == null)
			{
				return null;
			}
			Mesh mesh = new Mesh();
			if (UnityData.isEditor)
			{
				mesh.name = "SectionLayerSubMesh_Free" + ((map == null) ? "" : ("_" + map.Tile.ToString()));
			}
			return new LayerSubMesh(mesh, material, null);
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x00097A8C File Offset: 0x00095C8C
		public LayerSubMesh GetSubMesh(Material material)
		{
			if (material == null)
			{
				return null;
			}
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].material == material)
				{
					return this.subMeshes[i];
				}
			}
			Mesh mesh = new Mesh();
			if (UnityData.isEditor)
			{
				mesh.name = "SectionLayerSubMesh_" + base.GetType().Name + "_" + this.Map.Tile.ToString();
			}
			LayerSubMesh layerSubMesh = new LayerSubMesh(mesh, material, null);
			this.subMeshes.Add(layerSubMesh);
			return layerSubMesh;
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x00097B45 File Offset: 0x00095D45
		public virtual CellRect GetBoundaryRect()
		{
			return this.map.BoundsRect(0);
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x00097B54 File Offset: 0x00095D54
		public virtual void RefreshSubMeshBounds()
		{
			Bounds bounds = this.GetBoundaryRect().ExpandedBy(2).ToBounds();
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				this.subMeshes[i].mesh.bounds = bounds;
			}
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x00097BA8 File Offset: 0x00095DA8
		protected void FinalizeMesh(MeshParts tags)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].verts.Count > 0)
				{
					this.subMeshes[i].FinalizeMesh(tags);
				}
			}
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x00097BF8 File Offset: 0x00095DF8
		public virtual void DrawLayer()
		{
			if (!this.Visible)
			{
				return;
			}
			int count = this.subMeshes.Count;
			for (int i = 0; i < count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.finalized && !layerSubMesh.disabled)
				{
					Graphics.DrawMesh(layerSubMesh.mesh, Matrix4x4.identity, layerSubMesh.material, layerSubMesh.renderLayer);
				}
			}
		}

		// Token: 0x06001DC5 RID: 7621
		public abstract void Regenerate();

		// Token: 0x06001DC6 RID: 7622 RVA: 0x00097C60 File Offset: 0x00095E60
		protected void ClearSubMeshes(MeshParts parts)
		{
			foreach (LayerSubMesh layerSubMesh in this.subMeshes)
			{
				layerSubMesh.Clear(parts);
			}
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x00097CB4 File Offset: 0x00095EB4
		public void Dispose()
		{
			foreach (LayerSubMesh layerSubMesh in this.subMeshes)
			{
				UnityEngine.Object.Destroy(layerSubMesh.mesh);
			}
			this.subMeshes.Clear();
		}

		// Token: 0x040019D4 RID: 6612
		private Map map;

		// Token: 0x040019D5 RID: 6613
		public ulong relevantChangeTypes;

		// Token: 0x040019D6 RID: 6614
		public List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();
	}
}
