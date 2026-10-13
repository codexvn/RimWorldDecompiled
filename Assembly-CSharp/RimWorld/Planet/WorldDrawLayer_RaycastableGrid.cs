using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C8C RID: 15500
	public class WorldDrawLayer_RaycastableGrid : WorldDrawLayer
	{
		// Token: 0x1700346B RID: 13419
		// (get) Token: 0x060150F9 RID: 86265 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700346C RID: 13420
		// (get) Token: 0x060150FA RID: 86266 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060150FB RID: 86267 RVA: 0x0064DBB3 File Offset: 0x0064BDB3
		public override IEnumerable Regenerate()
		{
			if (this.material == null)
			{
				this.material = new Material(WorldMaterials.VertexColorTransparent);
			}
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			NativeArray<int> unsafeTileIDToVerts_offsets = this.planetLayer.UnsafeTileIDToVerts_offsets;
			NativeArray<Vector3> unsafeVerts = this.planetLayer.UnsafeVerts;
			for (int i = 0; i < this.planetLayer.TilesCount; i++)
			{
				int subMeshIndex;
				LayerSubMesh subMesh = base.GetSubMesh(this.material, out subMeshIndex);
				int count = subMesh.verts.Count;
				int num = 0;
				int num2 = (i + 1 < unsafeTileIDToVerts_offsets.Length) ? unsafeTileIDToVerts_offsets[i + 1] : unsafeVerts.Length;
				int num3 = unsafeTileIDToVerts_offsets[i];
				for (int j = unsafeTileIDToVerts_offsets[i]; j < num2; j++)
				{
					subMesh.verts.Add(unsafeVerts[j]);
					subMesh.colors.Add(new Color32(200, 200, 200, 10));
					if (num == 0)
					{
						subMesh.uvs.Add(new Vector3(1f, 0f, 0f));
					}
					else if (num == 1)
					{
						subMesh.uvs.Add(new Vector3(0f, 1f, 0f));
					}
					else
					{
						subMesh.uvs.Add(new Vector3(0f, 0f, 1f));
					}
					if (j < num2 - 2)
					{
						subMesh.tris.Add(count + num + 2);
						subMesh.tris.Add(count + num + 1);
						subMesh.tris.Add(count);
						base.AppendRaycastableTriangle(subMeshIndex, i);
					}
					num++;
				}
			}
			base.FinalizeMesh(MeshParts.All);
			foreach (object obj2 in base.RegenerateWorldMeshColliders())
			{
				yield return obj2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060150FC RID: 86268 RVA: 0x0064DBC3 File Offset: 0x0064BDC3
		public override void Render()
		{
			if (this.ShouldRegenerate)
			{
				base.RegenerateNow();
			}
		}

		// Token: 0x0400EAF4 RID: 60148
		private Material material;
	}
}
