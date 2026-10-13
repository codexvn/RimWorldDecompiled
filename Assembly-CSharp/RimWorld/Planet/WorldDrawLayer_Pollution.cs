using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C89 RID: 15497
	public class WorldDrawLayer_Pollution : WorldDrawLayer
	{
		// Token: 0x17003466 RID: 13414
		// (get) Token: 0x060150DF RID: 86239 RVA: 0x0064D356 File Offset: 0x0064B556
		private Material LightPollution
		{
			get
			{
				if (this.lightPollution == null)
				{
					this.lightPollution = MaterialPool.MatFrom("World/Pollution/Light", ShaderDatabase.WorldOverlayTransparentLitPollution, 3510);
				}
				return this.lightPollution;
			}
		}

		// Token: 0x17003467 RID: 13415
		// (get) Token: 0x060150E0 RID: 86240 RVA: 0x0064D386 File Offset: 0x0064B586
		private Material ModeratePollution
		{
			get
			{
				if (this.moderatePollution == null)
				{
					this.moderatePollution = MaterialPool.MatFrom("World/Pollution/Moderate", ShaderDatabase.WorldOverlayTransparentLitPollution, 3510);
				}
				return this.moderatePollution;
			}
		}

		// Token: 0x17003468 RID: 13416
		// (get) Token: 0x060150E1 RID: 86241 RVA: 0x0064D3B6 File Offset: 0x0064B5B6
		private Material ExtremePollution
		{
			get
			{
				if (this.extemePollution == null)
				{
					this.extemePollution = MaterialPool.MatFrom("World/Pollution/Extreme", ShaderDatabase.WorldOverlayTransparentLitPollution, 3510);
				}
				return this.extemePollution;
			}
		}

		// Token: 0x060150E2 RID: 86242 RVA: 0x0064D3E6 File Offset: 0x0064B5E6
		private int GetRegionIdForTile(PlanetTile tile)
		{
			return Mathf.FloorToInt((float)tile.tileId / 500f);
		}

		// Token: 0x060150E3 RID: 86243 RVA: 0x0064D3FA File Offset: 0x0064B5FA
		public List<LayerSubMesh> GetSubMeshesForRegion(int regionId)
		{
			if (!this.subMeshesByRegion.ContainsKey(regionId))
			{
				this.subMeshesByRegion[regionId] = new List<LayerSubMesh>();
			}
			return this.subMeshesByRegion[regionId];
		}

		// Token: 0x060150E4 RID: 86244 RVA: 0x0064D428 File Offset: 0x0064B628
		public LayerSubMesh GetSubMeshForMaterialAndRegion(Material material, int regionId)
		{
			List<LayerSubMesh> subMeshesForRegion = this.GetSubMeshesForRegion(regionId);
			for (int i = 0; i < subMeshesForRegion.Count; i++)
			{
				if (subMeshesForRegion[i].material == material)
				{
					return subMeshesForRegion[i];
				}
			}
			Mesh mesh = new Mesh();
			if (UnityData.isEditor)
			{
				mesh.name = "WorldLayerSubMesh_" + base.GetType().Name + "_" + Find.World.info.seedString;
			}
			LayerSubMesh layerSubMesh = new LayerSubMesh(mesh, material, null);
			subMeshesForRegion.Add(layerSubMesh);
			this.subMeshes.Add(layerSubMesh);
			return layerSubMesh;
		}

		// Token: 0x060150E5 RID: 86245 RVA: 0x0064D4CC File Offset: 0x0064B6CC
		private void RegenerateRegion(int regionId)
		{
			List<LayerSubMesh> subMeshesForRegion = this.GetSubMeshesForRegion(regionId);
			for (int i = 0; i < subMeshesForRegion.Count; i++)
			{
				subMeshesForRegion[i].Clear(MeshParts.All);
			}
			int num = regionId * 500;
			int num2 = num + 500;
			for (int j = num; j < num2; j++)
			{
				PlanetTile tile = new PlanetTile(j, this.planetLayer);
				if (!Find.World.grid.InBounds(tile))
				{
					break;
				}
				this.TryAddMeshForTile(tile);
			}
			for (int k = 0; k < subMeshesForRegion.Count; k++)
			{
				if (subMeshesForRegion[k].verts.Count > 0)
				{
					subMeshesForRegion[k].FinalizeMesh(MeshParts.All);
				}
			}
		}

		// Token: 0x060150E6 RID: 86246 RVA: 0x0064D57D File Offset: 0x0064B77D
		public override IEnumerable Regenerate()
		{
			if (!ModsConfig.BiotechActive)
			{
				yield break;
			}
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			int pollutedMeshesPrinted = 0;
			this.verts.Clear();
			this.subMeshesByRegion.Clear();
			this.regionsToRegenerate.Clear();
			int num;
			for (int i = 0; i < this.planetLayer.TilesCount; i = num + 1)
			{
				if (this.TryAddMeshForTile(this.planetLayer.PlanetTileForID(i)))
				{
					num = pollutedMeshesPrinted;
					pollutedMeshesPrinted = num + 1;
					if (pollutedMeshesPrinted % 1000 == 0)
					{
						yield return null;
					}
				}
				num = i;
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x060150E7 RID: 86247 RVA: 0x0064D590 File Offset: 0x0064B790
		private bool TryAddMeshForTile(PlanetTile tile)
		{
			PollutionLevel pollution = tile.Tile.PollutionLevel();
			Material materialForTilePollution = this.GetMaterialForTilePollution(pollution);
			if (materialForTilePollution == null)
			{
				return false;
			}
			int regionIdForTile = this.GetRegionIdForTile(tile);
			LayerSubMesh subMeshForMaterialAndRegion = this.GetSubMeshForMaterialAndRegion(materialForTilePollution, regionIdForTile);
			Find.WorldGrid.GetTileVertices(tile, this.verts);
			Find.WorldGrid.GetTileNeighbors(tile, this.tmpNeighbors);
			int count = subMeshForMaterialAndRegion.verts.Count;
			this.tmpBordersUnpollutedVerts.Clear();
			this.tmpVerts.Clear();
			for (int i = 0; i < this.tmpNeighbors.Count; i++)
			{
				if (this.planetLayer[this.tmpNeighbors[i]].PollutionLevel() < PollutionLevel.Moderate)
				{
					Vector3 center = Find.WorldGrid.GetTileCenter(this.tmpNeighbors[i]);
					this.tmpVerts.AddRange(this.verts);
					this.tmpVerts.SortBy((Vector3 v) => Vector2.Distance(center, v));
					for (int j = 0; j < 2; j++)
					{
						this.tmpBordersUnpollutedVerts.Add(this.tmpVerts[j]);
					}
				}
			}
			int k = 0;
			int count2 = this.verts.Count;
			while (k < count2)
			{
				Vector3 vector = this.verts[k] + this.verts[k].normalized * 0.02f;
				subMeshForMaterialAndRegion.verts.Add(vector);
				subMeshForMaterialAndRegion.uvs.Add(vector * 0.1f);
				Color c = this.tmpBordersUnpollutedVerts.Contains(this.verts[k]) ? WorldDrawLayer_Pollution.BordersUnpollutedTileColor : WorldDrawLayer_Pollution.DefaultTileColor;
				subMeshForMaterialAndRegion.colors.Add(c);
				if (k < count2 - 2)
				{
					subMeshForMaterialAndRegion.tris.Add(count + k + 2);
					subMeshForMaterialAndRegion.tris.Add(count + k + 1);
					subMeshForMaterialAndRegion.tris.Add(count);
				}
				k++;
			}
			this.tmpBordersUnpollutedVerts.Clear();
			this.tmpVerts.Clear();
			return true;
		}

		// Token: 0x060150E8 RID: 86248 RVA: 0x0064D7D0 File Offset: 0x0064B9D0
		private Material GetMaterialForTilePollution(PollutionLevel pollution)
		{
			switch (pollution)
			{
			case PollutionLevel.Light:
				return this.LightPollution;
			case PollutionLevel.Moderate:
				return this.ModeratePollution;
			case PollutionLevel.Extreme:
				return this.ExtremePollution;
			default:
				return null;
			}
		}

		// Token: 0x060150E9 RID: 86249 RVA: 0x0064D800 File Offset: 0x0064BA00
		public void Notify_TilePollutionChanged(PlanetTile tileId)
		{
			int regionIdForTile = this.GetRegionIdForTile(tileId);
			if (!this.regionsToRegenerate.Contains(regionIdForTile))
			{
				this.regionsToRegenerate.Enqueue(regionIdForTile);
			}
			Find.WorldGrid.GetTileNeighbors(tileId, WorldDrawLayer_Pollution.TmpChangedNeighbours);
			for (int i = 0; i < WorldDrawLayer_Pollution.TmpChangedNeighbours.Count; i++)
			{
				int regionIdForTile2 = this.GetRegionIdForTile(WorldDrawLayer_Pollution.TmpChangedNeighbours[i]);
				if (!this.regionsToRegenerate.Contains(regionIdForTile2))
				{
					this.regionsToRegenerate.Enqueue(regionIdForTile2);
				}
			}
			WorldDrawLayer_Pollution.TmpChangedNeighbours.Clear();
		}

		// Token: 0x060150EA RID: 86250 RVA: 0x0064D88C File Offset: 0x0064BA8C
		public override void Render()
		{
			if (this.regionsToRegenerate.Count > 0)
			{
				int regionId = this.regionsToRegenerate.Dequeue();
				this.RegenerateRegion(regionId);
			}
			base.Render();
		}

		// Token: 0x0400EADE RID: 60126
		private const int TilesPerSubMesh = 500;

		// Token: 0x0400EADF RID: 60127
		private const float ScaleUVFactor = 0.1f;

		// Token: 0x0400EAE0 RID: 60128
		private static readonly Color DefaultTileColor = Color.white;

		// Token: 0x0400EAE1 RID: 60129
		private static readonly Color BordersUnpollutedTileColor = new Color(1f, 1f, 1f, 0.4f);

		// Token: 0x0400EAE2 RID: 60130
		private readonly List<Vector3> verts = new List<Vector3>();

		// Token: 0x0400EAE3 RID: 60131
		private readonly Dictionary<int, List<LayerSubMesh>> subMeshesByRegion = new Dictionary<int, List<LayerSubMesh>>();

		// Token: 0x0400EAE4 RID: 60132
		private readonly Queue<int> regionsToRegenerate = new Queue<int>();

		// Token: 0x0400EAE5 RID: 60133
		private Material lightPollution;

		// Token: 0x0400EAE6 RID: 60134
		private Material moderatePollution;

		// Token: 0x0400EAE7 RID: 60135
		private Material extemePollution;

		// Token: 0x0400EAE8 RID: 60136
		private readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();

		// Token: 0x0400EAE9 RID: 60137
		private readonly HashSet<Vector3> tmpBordersUnpollutedVerts = new HashSet<Vector3>();

		// Token: 0x0400EAEA RID: 60138
		private readonly List<Vector3> tmpVerts = new List<Vector3>();

		// Token: 0x0400EAEB RID: 60139
		private static readonly List<PlanetTile> TmpChangedNeighbours = new List<PlanetTile>();
	}
}
