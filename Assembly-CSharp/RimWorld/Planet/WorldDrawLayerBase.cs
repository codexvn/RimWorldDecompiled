using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C73 RID: 15475
	public abstract class WorldDrawLayerBase
	{
		// Token: 0x17003434 RID: 13364
		// (get) Token: 0x06015042 RID: 86082 RVA: 0x0064AF57 File Offset: 0x00649157
		public virtual bool ShouldRegenerate
		{
			get
			{
				return this.dirty && this.Visible;
			}
		}

		// Token: 0x17003435 RID: 13365
		// (get) Token: 0x06015043 RID: 86083 RVA: 0x0064AF69 File Offset: 0x00649169
		protected virtual int RenderLayer
		{
			get
			{
				return WorldCameraManager.WorldLayer;
			}
		}

		// Token: 0x17003436 RID: 13366
		// (get) Token: 0x06015044 RID: 86084 RVA: 0x0064AF70 File Offset: 0x00649170
		protected virtual Quaternion Rotation
		{
			get
			{
				return Quaternion.identity;
			}
		}

		// Token: 0x17003437 RID: 13367
		// (get) Token: 0x06015045 RID: 86085 RVA: 0x0004FC72 File Offset: 0x0004DE72
		protected virtual float Alpha
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17003438 RID: 13368
		// (get) Token: 0x06015046 RID: 86086 RVA: 0x0064AF77 File Offset: 0x00649177
		public bool Dirty
		{
			get
			{
				return this.dirty;
			}
		}

		// Token: 0x17003439 RID: 13369
		// (get) Token: 0x06015047 RID: 86087 RVA: 0x0064AF7F File Offset: 0x0064917F
		public virtual bool Visible
		{
			get
			{
				return !WorldRendererUtility.WorldBackgroundNow || this.VisibleInBackground;
			}
		}

		// Token: 0x1700343A RID: 13370
		// (get) Token: 0x06015048 RID: 86088 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool VisibleWhenLayerNotSelected
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700343B RID: 13371
		// (get) Token: 0x06015049 RID: 86089 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool VisibleInBackground
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700343C RID: 13372
		// (get) Token: 0x0601504A RID: 86090 RVA: 0x0064AF90 File Offset: 0x00649190
		public virtual Vector3 Position
		{
			get
			{
				if (this.CameraIsOrigin)
				{
					return Find.WorldCameraDriver.CameraPosition;
				}
				return Vector3.zero;
			}
		}

		// Token: 0x1700343D RID: 13373
		// (get) Token: 0x0601504B RID: 86091 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool CameraIsOrigin
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700343E RID: 13374
		// (get) Token: 0x0601504C RID: 86092 RVA: 0x0064AFAC File Offset: 0x006491AC
		public MaterialPropertyBlock MatPropBlock
		{
			get
			{
				MaterialPropertyBlock result;
				if ((result = this.propertyBlock) == null)
				{
					result = (this.propertyBlock = new MaterialPropertyBlock());
				}
				return result;
			}
		}

		// Token: 0x0601504D RID: 86093 RVA: 0x0064AFD4 File Offset: 0x006491D4
		protected LayerSubMesh GetSubMesh(Material material)
		{
			int num;
			return this.GetSubMesh(material, out num);
		}

		// Token: 0x0601504E RID: 86094 RVA: 0x0064AFEC File Offset: 0x006491EC
		protected LayerSubMesh GetSubMesh(Material material, out int subMeshIndex)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.material == material && layerSubMesh.verts.Count < 40000)
				{
					subMeshIndex = i;
					return layerSubMesh;
				}
			}
			Mesh mesh = new Mesh();
			if (UnityData.isEditor)
			{
				mesh.name = "WorldLayerSubMesh_" + base.GetType().Name + "_" + Find.World.info.seedString;
			}
			LayerSubMesh layerSubMesh2 = new LayerSubMesh(mesh, material, null);
			subMeshIndex = this.subMeshes.Count;
			this.subMeshes.Add(layerSubMesh2);
			return layerSubMesh2;
		}

		// Token: 0x0601504F RID: 86095 RVA: 0x0064B0AC File Offset: 0x006492AC
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

		// Token: 0x06015050 RID: 86096 RVA: 0x0064B0FA File Offset: 0x006492FA
		public void RegenerateNow()
		{
			this.dirty = false;
			this.Regenerate().ExecuteEnumerable();
		}

		// Token: 0x06015051 RID: 86097 RVA: 0x0064B110 File Offset: 0x00649310
		public virtual void Render()
		{
			if (!this.Visible)
			{
				return;
			}
			if (this.ShouldRegenerate)
			{
				this.RegenerateNow();
			}
			int renderLayer = this.RenderLayer;
			Vector3 position = this.Position;
			Quaternion rotation = this.Rotation;
			float alpha = this.Alpha;
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				if (this.subMeshes[i].finalized)
				{
					if (!Mathf.Approximately(alpha, 1f))
					{
						Color color = this.subMeshes[i].material.color;
						this.MatPropBlock.SetColor(ShaderPropertyIDs.Color, new Color(color.r, color.g, color.b, color.a * alpha));
						Graphics.DrawMesh(this.subMeshes[i].mesh, position, rotation, this.subMeshes[i].material, renderLayer, null, 0, this.MatPropBlock);
					}
					else
					{
						Graphics.DrawMesh(this.subMeshes[i].mesh, position, rotation, this.subMeshes[i].material, renderLayer);
					}
				}
			}
		}

		// Token: 0x06015052 RID: 86098 RVA: 0x0064B245 File Offset: 0x00649445
		public virtual IEnumerable Regenerate()
		{
			this.dirty = false;
			this.ClearSubMeshes(MeshParts.All);
			yield break;
		}

		// Token: 0x06015053 RID: 86099 RVA: 0x0064B255 File Offset: 0x00649455
		public void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06015054 RID: 86100 RVA: 0x0064B260 File Offset: 0x00649460
		private void ClearSubMeshes(MeshParts parts)
		{
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				this.subMeshes[i].Clear(parts);
			}
		}

		// Token: 0x06015055 RID: 86101 RVA: 0x0064B298 File Offset: 0x00649498
		public void Dispose()
		{
			foreach (LayerSubMesh layerSubMesh in this.subMeshes)
			{
				UnityEngine.Object.Destroy(layerSubMesh.mesh);
			}
			this.subMeshes.Clear();
		}

		// Token: 0x0400EA8A RID: 60042
		protected bool dirty = true;

		// Token: 0x0400EA8B RID: 60043
		private MaterialPropertyBlock propertyBlock;

		// Token: 0x0400EA8C RID: 60044
		protected readonly List<LayerSubMesh> subMeshes = new List<LayerSubMesh>();

		// Token: 0x0400EA8D RID: 60045
		private const int MaxVerticesPerMesh = 40000;
	}
}
