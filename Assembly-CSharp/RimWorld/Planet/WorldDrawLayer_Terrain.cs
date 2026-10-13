using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C9A RID: 15514
	public class WorldDrawLayer_Terrain : WorldDrawLayer
	{
		// Token: 0x06015150 RID: 86352 RVA: 0x0064F14F File Offset: 0x0064D34F
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			if (!WorldDrawLayer_Terrain.setGlobalMacro)
			{
				WorldDrawLayer_Terrain.setGlobalMacro = true;
				Shader.SetGlobalTexture(ShaderPropertyIDs.MacroTex, ContentFinder<Texture2D>.Get("World/WorldMacro", true));
			}
			NativeArray<int> tileIDToVerts_offsets = this.planetLayer.UnsafeTileIDToVerts_offsets;
			NativeArray<Vector3> verts = this.planetLayer.UnsafeVerts;
			foreach (object obj2 in this.CalculateInterpolatedVerticesParams())
			{
				yield return obj2;
			}
			enumerator = null;
			int num = 0;
			for (int i = 0; i < this.planetLayer.TilesCount; i++)
			{
				Tile tile = this.planetLayer[i];
				BiomeDef primaryBiome = tile.PrimaryBiome;
				Landmark landmark = tile.Landmark;
				bool flag = landmark != null && landmark.def.drawType == LandmarkDef.LandmarkDrawType.TerrainMask;
				Material material = primaryBiome.DrawMaterial;
				if (flag)
				{
					material = MaterialPool.MatFrom(new MaterialRequest(material.mainTexture, material.shader)
					{
						maskTex = landmark.def.Texture,
						secondaryTex = this.planetLayer.Def.backgroundBiome.DrawMaterial.mainTexture,
						renderQueue = material.renderQueue,
						shaderParameters = landmark.def.terrainParameters
					});
				}
				int subMeshIndex;
				LayerSubMesh subMesh = base.GetSubMesh(material, out subMeshIndex);
				int count = subMesh.verts.Count;
				int num2 = 0;
				int oneAfterLastVertIndex = WorldDrawLayer_Terrain.GetOneAfterLastVertIndex(i, tileIDToVerts_offsets, verts);
				int num3 = oneAfterLastVertIndex - tileIDToVerts_offsets[i];
				int num4 = tileIDToVerts_offsets[i];
				int num5 = 0;
				int num6 = 0;
				if (flag)
				{
					num5 = Rand.RangeSeeded(0, landmark.def.atlasSize.x, i);
					num6 = Rand.RangeSeeded(0, landmark.def.atlasSize.z, i ^ 1823712);
				}
				int num7 = 0;
				if (flag && landmark.def.coastRotateMode != LandmarkDef.CoastRotateMode.None)
				{
					num7 = WorldDrawLayer_Terrain.GetLandmarkRotationIncrements(tile, this.planetLayer, landmark, tileIDToVerts_offsets, verts);
				}
				for (int j = num4; j < oneAfterLastVertIndex; j++)
				{
					Vector3 item = verts[j];
					subMesh.verts.Add(item);
					subMesh.uvs.Add(this.elevationValues[num]);
					if (flag)
					{
						float angleOffset = 360f / (float)num3 * (float)(-(float)num7);
						Vector2 vector = (GenGeo.RegularPolygonVertexPosition(num3, num2, angleOffset) + Vector2.one) / 2f;
						float num8 = 1f / (float)landmark.def.atlasSize.x;
						float num9 = 1f / (float)landmark.def.atlasSize.z;
						float num10 = (float)num5 * num8;
						float num11 = (float)num6 * num9;
						vector.x = Mathf.Lerp(num10, num10 + num8, vector.x);
						vector.y = Mathf.Lerp(num11, num11 + num9, vector.y);
						subMesh.uvsChannelTwo.Add(vector);
					}
					num++;
					if (j < oneAfterLastVertIndex - 2)
					{
						subMesh.tris.Add(count + num2 + 2);
						subMesh.tris.Add(count + num2 + 1);
						subMesh.tris.Add(count);
						base.AppendRaycastableTriangle(subMeshIndex, i);
					}
					num2++;
				}
			}
			base.FinalizeMesh(MeshParts.All);
			foreach (object obj3 in base.RegenerateWorldMeshColliders())
			{
				yield return obj3;
			}
			enumerator = null;
			this.elevationValues.Clear();
			this.elevationValues.TrimExcess();
			yield break;
			yield break;
		}

		// Token: 0x06015151 RID: 86353 RVA: 0x0064F160 File Offset: 0x0064D360
		private static int GetLandmarkRotationIncrements(Tile tile, PlanetLayer layer, Landmark landmark, NativeArray<int> tileIDToVerts_offsets, NativeArray<Vector3> verts)
		{
			int oneAfterLastVertIndex = WorldDrawLayer_Terrain.GetOneAfterLastVertIndex(tile.tile.tileId, tileIDToVerts_offsets, verts);
			int num = tileIDToVerts_offsets[tile.tile.tileId];
			int num2 = 0;
			layer.GetTileNeighbors(tile.tile, WorldDrawLayer_Terrain.neighbours);
			PlanetTile planetTile = PlanetTile.Invalid;
			for (int i = 0; i < WorldDrawLayer_Terrain.neighbours.Count; i++)
			{
				if (LandmarkDef.IsValidRotatableNeighbour(layer[WorldDrawLayer_Terrain.neighbours[i]].PrimaryBiome, landmark.def.coastRotateMode))
				{
					planetTile = WorldDrawLayer_Terrain.neighbours[i];
					break;
				}
			}
			if (planetTile.Valid)
			{
				int num3 = tileIDToVerts_offsets[planetTile.tileId];
				int oneAfterLastVertIndex2 = WorldDrawLayer_Terrain.GetOneAfterLastVertIndex(planetTile.tileId, tileIDToVerts_offsets, verts);
				bool flag = false;
				for (int j = num; j < oneAfterLastVertIndex; j++)
				{
					int index = WorldDrawLayer_Terrain.<GetLandmarkRotationIncrements>g__WrapIndex|5_0(num, oneAfterLastVertIndex, j + 1);
					int index2 = WorldDrawLayer_Terrain.<GetLandmarkRotationIncrements>g__WrapIndex|5_0(num, oneAfterLastVertIndex, j + 2);
					for (int k = num3; k < oneAfterLastVertIndex2; k++)
					{
						int index3 = WorldDrawLayer_Terrain.<GetLandmarkRotationIncrements>g__WrapIndex|5_0(num3, oneAfterLastVertIndex2, k);
						int index4 = WorldDrawLayer_Terrain.<GetLandmarkRotationIncrements>g__WrapIndex|5_0(num3, oneAfterLastVertIndex2, k + 1);
						if ((!(verts[index] != verts[index3]) || !(verts[index2] != verts[index3])) && (!(verts[index] != verts[index4]) || !(verts[index2] != verts[index4])))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
					num2++;
				}
				if (!flag)
				{
					Log.Error(string.Format("Failed to get increments for rotation of landmark with a valid neighbouring tile. Tile: {0}", tile));
				}
			}
			else
			{
				Log.Error(string.Format("Failed to get non background biome near tile with a landmark which has to rotate away from the background. Tile: {0}", tile));
			}
			return num2;
		}

		// Token: 0x06015152 RID: 86354 RVA: 0x0064F328 File Offset: 0x0064D528
		private static int GetOneAfterLastVertIndex(int index, NativeArray<int> tileIDToVerts_offsets, NativeArray<Vector3> verts)
		{
			if (index + 1 >= tileIDToVerts_offsets.Length)
			{
				return verts.Length;
			}
			return tileIDToVerts_offsets[index + 1];
		}

		// Token: 0x06015153 RID: 86355 RVA: 0x0064F348 File Offset: 0x0064D548
		private IEnumerable CalculateInterpolatedVerticesParams()
		{
			this.elevationValues.Clear();
			int tilesCount = this.planetLayer.TilesCount;
			NativeArray<Vector3> verts = this.planetLayer.UnsafeVerts;
			NativeArray<int> tileIDToVerts_offsets = this.planetLayer.UnsafeTileIDToVerts_offsets;
			NativeArray<int> tileIDToNeighbors_offsets = this.planetLayer.UnsafeTileIDToNeighbors_offsets;
			NativeArray<PlanetTile> tileIDToNeighbors_values = this.planetLayer.UnsafeTileIDToNeighbors_values;
			int num4;
			for (int i = 0; i < this.planetLayer.TilesCount; i = num4 + 1)
			{
				Tile tile = this.planetLayer[i];
				float elevation = tile.elevation;
				int num = (i + 1 < tileIDToNeighbors_offsets.Length) ? tileIDToNeighbors_offsets[i + 1] : tileIDToNeighbors_values.Length;
				int num2 = (i + 1 < tilesCount) ? tileIDToVerts_offsets[i + 1] : verts.Length;
				for (int j = tileIDToVerts_offsets[i]; j < num2; j++)
				{
					Vector3 vector = new Vector3
					{
						x = elevation
					};
					bool flag = false;
					for (int k = tileIDToNeighbors_offsets[i]; k < num; k++)
					{
						int num3 = (tileIDToNeighbors_values[k].tileId + 1 < tileIDToVerts_offsets.Length) ? tileIDToVerts_offsets[tileIDToNeighbors_values[k].tileId + 1] : verts.Length;
						int l = tileIDToVerts_offsets[tileIDToNeighbors_values[k].tileId];
						while (l < num3)
						{
							if (verts[l] == verts[j])
							{
								Tile tile2 = this.planetLayer[tileIDToNeighbors_values[k]];
								if (flag)
								{
									break;
								}
								if ((tile2.elevation >= 0f && elevation <= 0f) || (tile2.elevation <= 0f && elevation >= 0f))
								{
									flag = true;
									break;
								}
								if (tile2.elevation > vector.x)
								{
									vector.x = tile2.elevation;
									break;
								}
								break;
							}
							else
							{
								l++;
							}
						}
					}
					vector.y = vector.x;
					if (flag)
					{
						vector.x = 0f;
					}
					if (tile.PrimaryBiome.DrawMaterial.shader != ShaderDatabase.WorldOcean && vector.x < 0f)
					{
						vector.x = 0f;
					}
					this.elevationValues.Add(vector);
				}
				if (i % 1000 == 0)
				{
					yield return null;
				}
				num4 = i;
			}
			yield break;
		}

		// Token: 0x06015157 RID: 86359 RVA: 0x0064F378 File Offset: 0x0064D578
		[CompilerGenerated]
		internal static int <GetLandmarkRotationIncrements>g__WrapIndex|5_0(int start, int oneAfterLast, int index)
		{
			if (index >= oneAfterLast)
			{
				return start + index % oneAfterLast;
			}
			return index;
		}

		// Token: 0x0400EB2C RID: 60204
		private readonly List<Vector3> elevationValues = new List<Vector3>();

		// Token: 0x0400EB2D RID: 60205
		private static bool setGlobalMacro;

		// Token: 0x0400EB2E RID: 60206
		private static readonly List<PlanetTile> neighbours = new List<PlanetTile>(8);

		// Token: 0x0400EB2F RID: 60207
		private const string MacroTexturePath = "World/WorldMacro";
	}
}
