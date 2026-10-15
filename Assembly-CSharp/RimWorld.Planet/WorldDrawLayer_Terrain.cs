using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class WorldDrawLayer_Terrain : WorldDrawLayer
{
	[CompilerGenerated]
	private sealed class _003CCalculateInterpolatedVerticesParams_003Ed__7 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldDrawLayer_Terrain _003C_003E4__this;

		private int _003CtilesCount_003E5__2;

		private NativeArray<Vector3> _003Cverts_003E5__3;

		private NativeArray<int> _003CtileIDToVerts_offsets_003E5__4;

		private NativeArray<int> _003CtileIDToNeighbors_offsets_003E5__5;

		private NativeArray<PlanetTile> _003CtileIDToNeighbors_values_003E5__6;

		private int _003Ci_003E5__7;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CCalculateInterpolatedVerticesParams_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			WorldDrawLayer_Terrain worldDrawLayer_Terrain = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_030c;
			}
			_003C_003E1__state = -1;
			worldDrawLayer_Terrain.elevationValues.Clear();
			_003CtilesCount_003E5__2 = worldDrawLayer_Terrain.planetLayer.TilesCount;
			_003Cverts_003E5__3 = worldDrawLayer_Terrain.planetLayer.UnsafeVerts;
			_003CtileIDToVerts_offsets_003E5__4 = worldDrawLayer_Terrain.planetLayer.UnsafeTileIDToVerts_offsets;
			_003CtileIDToNeighbors_offsets_003E5__5 = worldDrawLayer_Terrain.planetLayer.UnsafeTileIDToNeighbors_offsets;
			_003CtileIDToNeighbors_values_003E5__6 = worldDrawLayer_Terrain.planetLayer.UnsafeTileIDToNeighbors_values;
			_003Ci_003E5__7 = 0;
			goto IL_031e;
			IL_030c:
			_003Ci_003E5__7++;
			goto IL_031e;
			IL_031e:
			if (_003Ci_003E5__7 < worldDrawLayer_Terrain.planetLayer.TilesCount)
			{
				Tile tile = worldDrawLayer_Terrain.planetLayer[_003Ci_003E5__7];
				float elevation = tile.elevation;
				int num2 = ((_003Ci_003E5__7 + 1 < _003CtileIDToNeighbors_offsets_003E5__5.Length) ? _003CtileIDToNeighbors_offsets_003E5__5[_003Ci_003E5__7 + 1] : _003CtileIDToNeighbors_values_003E5__6.Length);
				int num3 = ((_003Ci_003E5__7 + 1 < _003CtilesCount_003E5__2) ? _003CtileIDToVerts_offsets_003E5__4[_003Ci_003E5__7 + 1] : _003Cverts_003E5__3.Length);
				for (int i = _003CtileIDToVerts_offsets_003E5__4[_003Ci_003E5__7]; i < num3; i++)
				{
					Vector3 vector = default(Vector3);
					vector.x = elevation;
					Vector3 item = vector;
					bool flag = false;
					for (int j = _003CtileIDToNeighbors_offsets_003E5__5[_003Ci_003E5__7]; j < num2; j++)
					{
						int num4 = ((_003CtileIDToNeighbors_values_003E5__6[j].tileId + 1 < _003CtileIDToVerts_offsets_003E5__4.Length) ? _003CtileIDToVerts_offsets_003E5__4[_003CtileIDToNeighbors_values_003E5__6[j].tileId + 1] : _003Cverts_003E5__3.Length);
						for (int k = _003CtileIDToVerts_offsets_003E5__4[_003CtileIDToNeighbors_values_003E5__6[j].tileId]; k < num4; k++)
						{
							if (!(_003Cverts_003E5__3[k] == _003Cverts_003E5__3[i]))
							{
								continue;
							}
							Tile tile2 = worldDrawLayer_Terrain.planetLayer[_003CtileIDToNeighbors_values_003E5__6[j]];
							if (!flag)
							{
								if ((tile2.elevation >= 0f && elevation <= 0f) || (tile2.elevation <= 0f && elevation >= 0f))
								{
									flag = true;
								}
								else if (tile2.elevation > item.x)
								{
									item.x = tile2.elevation;
								}
							}
							break;
						}
					}
					item.y = item.x;
					if (flag)
					{
						item.x = 0f;
					}
					if (tile.PrimaryBiome.DrawMaterial.shader != ShaderDatabase.WorldOcean && item.x < 0f)
					{
						item.x = 0f;
					}
					worldDrawLayer_Terrain.elevationValues.Add(item);
				}
				if (_003Ci_003E5__7 % 1000 == 0)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_030c;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			_003CCalculateInterpolatedVerticesParams_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCalculateInterpolatedVerticesParams_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<object>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CRegenerate_003Ed__4 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldDrawLayer_Terrain _003C_003E4__this;

		private NativeArray<int> _003CtileIDToVerts_offsets_003E5__2;

		private NativeArray<Vector3> _003Cverts_003E5__3;

		private IEnumerator _003C_003E7__wrap3;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CRegenerate_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WorldDrawLayer_Terrain worldDrawLayer_Terrain = _003C_003E4__this;
				int num2;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap3 = ((WorldDrawLayer)worldDrawLayer_Terrain).Regenerate().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					_003C_003E1__state = -4;
					goto IL_011c;
				case 3:
					{
						_003C_003E1__state = -5;
						break;
					}
					IL_0078:
					if (_003C_003E7__wrap3.MoveNext())
					{
						object current = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap3 = null;
					if (!setGlobalMacro)
					{
						setGlobalMacro = true;
						Shader.SetGlobalTexture(ShaderPropertyIDs.MacroTex, ContentFinder<Texture2D>.Get("World/WorldMacro"));
					}
					_003CtileIDToVerts_offsets_003E5__2 = worldDrawLayer_Terrain.planetLayer.UnsafeTileIDToVerts_offsets;
					_003Cverts_003E5__3 = worldDrawLayer_Terrain.planetLayer.UnsafeVerts;
					_003C_003E7__wrap3 = worldDrawLayer_Terrain.CalculateInterpolatedVerticesParams().GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_011c;
					IL_011c:
					if (_003C_003E7__wrap3.MoveNext())
					{
						object current2 = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = null;
					num2 = 0;
					for (int i = 0; i < worldDrawLayer_Terrain.planetLayer.TilesCount; i++)
					{
						Tile tile = worldDrawLayer_Terrain.planetLayer[i];
						BiomeDef primaryBiome = tile.PrimaryBiome;
						Landmark landmark = tile.Landmark;
						bool flag = landmark != null && landmark.def.drawType == LandmarkDef.LandmarkDrawType.TerrainMask;
						Material material = primaryBiome.DrawMaterial;
						if (flag)
						{
							material = MaterialPool.MatFrom(new MaterialRequest(material.mainTexture, material.shader)
							{
								maskTex = landmark.def.Texture,
								secondaryTex = worldDrawLayer_Terrain.planetLayer.Def.backgroundBiome.DrawMaterial.mainTexture,
								renderQueue = material.renderQueue,
								shaderParameters = landmark.def.terrainParameters
							});
						}
						int subMeshIndex;
						LayerSubMesh subMesh = worldDrawLayer_Terrain.GetSubMesh(material, out subMeshIndex);
						int count = subMesh.verts.Count;
						int num3 = 0;
						int oneAfterLastVertIndex = GetOneAfterLastVertIndex(i, _003CtileIDToVerts_offsets_003E5__2, _003Cverts_003E5__3);
						int num4 = oneAfterLastVertIndex - _003CtileIDToVerts_offsets_003E5__2[i];
						int num5 = _003CtileIDToVerts_offsets_003E5__2[i];
						int num6 = 0;
						int num7 = 0;
						if (flag)
						{
							num6 = Rand.RangeSeeded(0, landmark.def.atlasSize.x, i);
							num7 = Rand.RangeSeeded(0, landmark.def.atlasSize.z, i ^ 0x1BD3E0);
						}
						int num8 = 0;
						if (flag && landmark.def.coastRotateMode != 0)
						{
							num8 = GetLandmarkRotationIncrements(tile, worldDrawLayer_Terrain.planetLayer, landmark, _003CtileIDToVerts_offsets_003E5__2, _003Cverts_003E5__3);
						}
						for (int j = num5; j < oneAfterLastVertIndex; j++)
						{
							Vector3 item = _003Cverts_003E5__3[j];
							subMesh.verts.Add(item);
							subMesh.uvs.Add(worldDrawLayer_Terrain.elevationValues[num2]);
							if (flag)
							{
								float angleOffset = 360f / (float)num4 * (float)(-num8);
								Vector2 vector = (GenGeo.RegularPolygonVertexPosition(num4, num3, angleOffset) + Vector2.one) / 2f;
								float num9 = 1f / (float)landmark.def.atlasSize.x;
								float num10 = 1f / (float)landmark.def.atlasSize.z;
								float num11 = (float)num6 * num9;
								float num12 = (float)num7 * num10;
								vector.x = Mathf.Lerp(num11, num11 + num9, vector.x);
								vector.y = Mathf.Lerp(num12, num12 + num10, vector.y);
								subMesh.uvsChannelTwo.Add(vector);
							}
							num2++;
							if (j < oneAfterLastVertIndex - 2)
							{
								subMesh.tris.Add(count + num3 + 2);
								subMesh.tris.Add(count + num3 + 1);
								subMesh.tris.Add(count);
								worldDrawLayer_Terrain.AppendRaycastableTriangle(subMeshIndex, i);
							}
							num3++;
						}
					}
					worldDrawLayer_Terrain.FinalizeMesh(MeshParts.All);
					_003C_003E7__wrap3 = worldDrawLayer_Terrain.RegenerateWorldMeshColliders().GetEnumerator();
					_003C_003E1__state = -5;
					break;
				}
				if (_003C_003E7__wrap3.MoveNext())
				{
					object current3 = _003C_003E7__wrap3.Current;
					_003C_003E2__current = current3;
					_003C_003E1__state = 3;
					return true;
				}
				_003C_003Em__Finally3();
				_003C_003E7__wrap3 = null;
				worldDrawLayer_Terrain.elevationValues.Clear();
				worldDrawLayer_Terrain.elevationValues.TrimExcess();
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap3 is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap3 is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap3 is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			_003CRegenerate_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CRegenerate_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<object>)this).GetEnumerator();
		}
	}

	private readonly List<Vector3> elevationValues = new List<Vector3>();

	private static bool setGlobalMacro;

	private static readonly List<PlanetTile> neighbours = new List<PlanetTile>(8);

	private const string MacroTexturePath = "World/WorldMacro";

	[IteratorStateMachine(typeof(_003CRegenerate_003Ed__4))]
	public override IEnumerable Regenerate()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRegenerate_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}

	private static int GetLandmarkRotationIncrements(Tile tile, PlanetLayer layer, Landmark landmark, NativeArray<int> tileIDToVerts_offsets, NativeArray<Vector3> verts)
	{
		int oneAfterLastVertIndex = GetOneAfterLastVertIndex(tile.tile.tileId, tileIDToVerts_offsets, verts);
		int num = tileIDToVerts_offsets[tile.tile.tileId];
		int num2 = 0;
		layer.GetTileNeighbors(tile.tile, neighbours);
		PlanetTile planetTile = PlanetTile.Invalid;
		for (int i = 0; i < neighbours.Count; i++)
		{
			if (LandmarkDef.IsValidRotatableNeighbour(layer[neighbours[i]].PrimaryBiome, landmark.def.coastRotateMode))
			{
				planetTile = neighbours[i];
				break;
			}
		}
		if (planetTile.Valid)
		{
			int num3 = tileIDToVerts_offsets[planetTile.tileId];
			int oneAfterLastVertIndex2 = GetOneAfterLastVertIndex(planetTile.tileId, tileIDToVerts_offsets, verts);
			bool flag = false;
			for (int j = num; j < oneAfterLastVertIndex; j++)
			{
				int index2 = WrapIndex(num, oneAfterLastVertIndex, j + 1);
				int index3 = WrapIndex(num, oneAfterLastVertIndex, j + 2);
				for (int k = num3; k < oneAfterLastVertIndex2; k++)
				{
					int index4 = WrapIndex(num3, oneAfterLastVertIndex2, k);
					int index5 = WrapIndex(num3, oneAfterLastVertIndex2, k + 1);
					if ((!(verts[index2] != verts[index4]) || !(verts[index3] != verts[index4])) && (!(verts[index2] != verts[index5]) || !(verts[index3] != verts[index5])))
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
				Log.Error($"Failed to get increments for rotation of landmark with a valid neighbouring tile. Tile: {tile}");
			}
		}
		else
		{
			Log.Error($"Failed to get non background biome near tile with a landmark which has to rotate away from the background. Tile: {tile}");
		}
		return num2;
		static int WrapIndex(int start, int oneAfterLast, int index)
		{
			if (index >= oneAfterLast)
			{
				return start + index % oneAfterLast;
			}
			return index;
		}
	}

	private static int GetOneAfterLastVertIndex(int index, NativeArray<int> tileIDToVerts_offsets, NativeArray<Vector3> verts)
	{
		if (index + 1 >= tileIDToVerts_offsets.Length)
		{
			return verts.Length;
		}
		return tileIDToVerts_offsets[index + 1];
	}

	[IteratorStateMachine(typeof(_003CCalculateInterpolatedVerticesParams_003Ed__7))]
	private IEnumerable CalculateInterpolatedVerticesParams()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCalculateInterpolatedVerticesParams_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}
}
