using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C76 RID: 15478
	public abstract class WorldDrawLayer : WorldDrawLayerBase
	{
		// Token: 0x17003441 RID: 13377
		// (get) Token: 0x06015063 RID: 86115 RVA: 0x0064B577 File Offset: 0x00649777
		public override bool Visible
		{
			get
			{
				return base.Visible && (PlanetLayer.Selected == this.planetLayer || this.VisibleWhenLayerNotSelected);
			}
		}

		// Token: 0x17003442 RID: 13378
		// (get) Token: 0x06015064 RID: 86116 RVA: 0x0064B598 File Offset: 0x00649798
		public override Vector3 Position
		{
			get
			{
				if (this.CameraIsOrigin)
				{
					return Find.WorldCameraDriver.CameraPosition;
				}
				return this.planetLayer.Origin;
			}
		}

		// Token: 0x17003443 RID: 13379
		// (get) Token: 0x06015065 RID: 86117 RVA: 0x0064B5B8 File Offset: 0x006497B8
		public bool Raycastable
		{
			get
			{
				return !this.meshCollidersInOrder.NullOrEmpty<MeshCollider>() && PlanetLayer.Selected == this.planetLayer;
			}
		}

		// Token: 0x06015066 RID: 86118 RVA: 0x0064B5D6 File Offset: 0x006497D6
		public void Initialize(PlanetLayer layer)
		{
			this.planetLayer = layer;
		}

		// Token: 0x06015067 RID: 86119 RVA: 0x0064B5DF File Offset: 0x006497DF
		public override IEnumerable Regenerate()
		{
			List<List<int>> list = this.triangleIndexToTileID;
			if (list != null)
			{
				list.Clear();
			}
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06015068 RID: 86120 RVA: 0x0064B5F0 File Offset: 0x006497F0
		protected void AppendRaycastableTriangle(int subMeshIndex, int tileIndex)
		{
			if (this.triangleIndexToTileID == null)
			{
				this.triangleIndexToTileID = new List<List<int>>();
			}
			while (this.triangleIndexToTileID.Count < this.subMeshes.Count)
			{
				this.triangleIndexToTileID.Add(new List<int>());
			}
			this.triangleIndexToTileID[subMeshIndex].Add(tileIndex);
		}

		// Token: 0x06015069 RID: 86121 RVA: 0x0064B64C File Offset: 0x0064984C
		public bool TryGetTileFromRayHit(RaycastHit hit, out PlanetTile id)
		{
			id = PlanetTile.Invalid;
			for (int i = 0; i < this.meshCollidersInOrder.Count; i++)
			{
				if (this.meshCollidersInOrder[i] == hit.collider)
				{
					id = new PlanetTile(this.triangleIndexToTileID[i][hit.triangleIndex], this.planetLayer);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0601506A RID: 86122 RVA: 0x0064B6C0 File Offset: 0x006498C0
		protected IEnumerable RegenerateWorldMeshColliders()
		{
			if (this.meshCollidersInOrder == null)
			{
				this.meshCollidersInOrder = new List<MeshCollider>();
			}
			else
			{
				this.meshCollidersInOrder.Clear();
			}
			GameObject gameObject = WorldTerrainColliderManager.Get(this.planetLayer, WorldCameraManager.WorldLayer);
			gameObject.transform.position = this.Position;
			MeshCollider[] components = gameObject.GetComponents<MeshCollider>();
			int j;
			for (j = 0; j < components.Length; j++)
			{
				UnityEngine.Object.Destroy(components[j]);
			}
			for (int i = 0; i < this.subMeshes.Count; i = j + 1)
			{
				MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
				meshCollider.sharedMesh = this.subMeshes[i].mesh;
				this.meshCollidersInOrder.Add(meshCollider);
				yield return null;
				j = i;
			}
			yield break;
		}

		// Token: 0x0400EA94 RID: 60052
		public PlanetLayer planetLayer;

		// Token: 0x0400EA95 RID: 60053
		private List<MeshCollider> meshCollidersInOrder;

		// Token: 0x0400EA96 RID: 60054
		private List<List<int>> triangleIndexToTileID;
	}
}
