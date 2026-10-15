using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class WorldDrawLayer_Pollution : WorldDrawLayer
{
	[CompilerGenerated]
	private sealed class _003CRegenerate_003Ed__20 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldDrawLayer_Pollution _003C_003E4__this;

		private int _003CpollutedMeshesPrinted_003E5__2;

		private IEnumerator _003C_003E7__wrap2;

		private int _003Ci_003E5__4;

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
		public _003CRegenerate_003Ed__20(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WorldDrawLayer_Pollution worldDrawLayer_Pollution = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!ModsConfig.BiotechActive)
					{
						return false;
					}
					_003C_003E7__wrap2 = ((WorldDrawLayer)worldDrawLayer_Pollution).Regenerate().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0080;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0080;
				case 2:
					{
						_003C_003E1__state = -1;
						goto IL_011d;
					}
					IL_012f:
					if (_003Ci_003E5__4 < worldDrawLayer_Pollution.planetLayer.TilesCount)
					{
						if (worldDrawLayer_Pollution.TryAddMeshForTile(worldDrawLayer_Pollution.planetLayer.PlanetTileForID(_003Ci_003E5__4)))
						{
							_003CpollutedMeshesPrinted_003E5__2++;
							if (_003CpollutedMeshesPrinted_003E5__2 % 1000 == 0)
							{
								_003C_003E2__current = null;
								_003C_003E1__state = 2;
								return true;
							}
						}
						goto IL_011d;
					}
					worldDrawLayer_Pollution.FinalizeMesh(MeshParts.All);
					return false;
					IL_0080:
					if (_003C_003E7__wrap2.MoveNext())
					{
						object current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					_003CpollutedMeshesPrinted_003E5__2 = 0;
					worldDrawLayer_Pollution.verts.Clear();
					worldDrawLayer_Pollution.subMeshesByRegion.Clear();
					worldDrawLayer_Pollution.regionsToRegenerate.Clear();
					_003Ci_003E5__4 = 0;
					goto IL_012f;
					IL_011d:
					_003Ci_003E5__4++;
					goto IL_012f;
				}
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
			if (_003C_003E7__wrap2 is IDisposable disposable)
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
			_003CRegenerate_003Ed__20 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CRegenerate_003Ed__20(0)
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

	private const int TilesPerSubMesh = 500;

	private const float ScaleUVFactor = 0.1f;

	private static readonly Color DefaultTileColor = Color.white;

	private static readonly Color BordersUnpollutedTileColor = new Color(1f, 1f, 1f, 0.4f);

	private readonly List<Vector3> verts = new List<Vector3>();

	private readonly Dictionary<int, List<LayerSubMesh>> subMeshesByRegion = new Dictionary<int, List<LayerSubMesh>>();

	private readonly Queue<int> regionsToRegenerate = new Queue<int>();

	private Material lightPollution;

	private Material moderatePollution;

	private Material extemePollution;

	private readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();

	private readonly HashSet<Vector3> tmpBordersUnpollutedVerts = new HashSet<Vector3>();

	private readonly List<Vector3> tmpVerts = new List<Vector3>();

	private static readonly List<PlanetTile> TmpChangedNeighbours = new List<PlanetTile>();

	private Material LightPollution
	{
		get
		{
			if (lightPollution == null)
			{
				lightPollution = MaterialPool.MatFrom("World/Pollution/Light", ShaderDatabase.WorldOverlayTransparentLitPollution, 3510);
			}
			return lightPollution;
		}
	}

	private Material ModeratePollution
	{
		get
		{
			if (moderatePollution == null)
			{
				moderatePollution = MaterialPool.MatFrom("World/Pollution/Moderate", ShaderDatabase.WorldOverlayTransparentLitPollution, 3510);
			}
			return moderatePollution;
		}
	}

	private Material ExtremePollution
	{
		get
		{
			if (extemePollution == null)
			{
				extemePollution = MaterialPool.MatFrom("World/Pollution/Extreme", ShaderDatabase.WorldOverlayTransparentLitPollution, 3510);
			}
			return extemePollution;
		}
	}

	private int GetRegionIdForTile(PlanetTile tile)
	{
		return Mathf.FloorToInt((float)tile.tileId / 500f);
	}

	public List<LayerSubMesh> GetSubMeshesForRegion(int regionId)
	{
		if (!subMeshesByRegion.ContainsKey(regionId))
		{
			subMeshesByRegion[regionId] = new List<LayerSubMesh>();
		}
		return subMeshesByRegion[regionId];
	}

	public LayerSubMesh GetSubMeshForMaterialAndRegion(Material material, int regionId)
	{
		List<LayerSubMesh> subMeshesForRegion = GetSubMeshesForRegion(regionId);
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
			mesh.name = "WorldLayerSubMesh_" + GetType().Name + "_" + Find.World.info.seedString;
		}
		LayerSubMesh layerSubMesh = new LayerSubMesh(mesh, material, null);
		subMeshesForRegion.Add(layerSubMesh);
		subMeshes.Add(layerSubMesh);
		return layerSubMesh;
	}

	private void RegenerateRegion(int regionId)
	{
		List<LayerSubMesh> subMeshesForRegion = GetSubMeshesForRegion(regionId);
		for (int i = 0; i < subMeshesForRegion.Count; i++)
		{
			subMeshesForRegion[i].Clear(MeshParts.All);
		}
		int num = regionId * 500;
		int num2 = num + 500;
		for (int j = num; j < num2; j++)
		{
			PlanetTile tile = new PlanetTile(j, planetLayer);
			if (!Find.World.grid.InBounds(tile))
			{
				break;
			}
			TryAddMeshForTile(tile);
		}
		for (int k = 0; k < subMeshesForRegion.Count; k++)
		{
			if (subMeshesForRegion[k].verts.Count > 0)
			{
				subMeshesForRegion[k].FinalizeMesh(MeshParts.All);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CRegenerate_003Ed__20))]
	public override IEnumerable Regenerate()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRegenerate_003Ed__20(-2)
		{
			_003C_003E4__this = this
		};
	}

	private bool TryAddMeshForTile(PlanetTile tile)
	{
		PollutionLevel pollution = tile.Tile.PollutionLevel();
		Material materialForTilePollution = GetMaterialForTilePollution(pollution);
		if (materialForTilePollution == null)
		{
			return false;
		}
		int regionIdForTile = GetRegionIdForTile(tile);
		LayerSubMesh subMeshForMaterialAndRegion = GetSubMeshForMaterialAndRegion(materialForTilePollution, regionIdForTile);
		Find.WorldGrid.GetTileVertices(tile, verts);
		Find.WorldGrid.GetTileNeighbors(tile, tmpNeighbors);
		int count = subMeshForMaterialAndRegion.verts.Count;
		tmpBordersUnpollutedVerts.Clear();
		tmpVerts.Clear();
		for (int i = 0; i < tmpNeighbors.Count; i++)
		{
			if (planetLayer[tmpNeighbors[i]].PollutionLevel() < PollutionLevel.Moderate)
			{
				Vector3 center = Find.WorldGrid.GetTileCenter(tmpNeighbors[i]);
				tmpVerts.AddRange(verts);
				tmpVerts.SortBy((Vector3 v) => Vector2.Distance(center, v));
				for (int j = 0; j < 2; j++)
				{
					tmpBordersUnpollutedVerts.Add(tmpVerts[j]);
				}
			}
		}
		int k = 0;
		for (int count2 = verts.Count; k < count2; k++)
		{
			Vector3 vector = verts[k] + verts[k].normalized * 0.02f;
			subMeshForMaterialAndRegion.verts.Add(vector);
			subMeshForMaterialAndRegion.uvs.Add(vector * 0.1f);
			Color color = (tmpBordersUnpollutedVerts.Contains(verts[k]) ? BordersUnpollutedTileColor : DefaultTileColor);
			subMeshForMaterialAndRegion.colors.Add(color);
			if (k < count2 - 2)
			{
				subMeshForMaterialAndRegion.tris.Add(count + k + 2);
				subMeshForMaterialAndRegion.tris.Add(count + k + 1);
				subMeshForMaterialAndRegion.tris.Add(count);
			}
		}
		tmpBordersUnpollutedVerts.Clear();
		tmpVerts.Clear();
		return true;
	}

	private Material GetMaterialForTilePollution(PollutionLevel pollution)
	{
		return pollution switch
		{
			PollutionLevel.Light => LightPollution, 
			PollutionLevel.Moderate => ModeratePollution, 
			PollutionLevel.Extreme => ExtremePollution, 
			_ => null, 
		};
	}

	public void Notify_TilePollutionChanged(PlanetTile tileId)
	{
		int regionIdForTile = GetRegionIdForTile(tileId);
		if (!regionsToRegenerate.Contains(regionIdForTile))
		{
			regionsToRegenerate.Enqueue(regionIdForTile);
		}
		Find.WorldGrid.GetTileNeighbors(tileId, TmpChangedNeighbours);
		for (int i = 0; i < TmpChangedNeighbours.Count; i++)
		{
			int regionIdForTile2 = GetRegionIdForTile(TmpChangedNeighbours[i]);
			if (!regionsToRegenerate.Contains(regionIdForTile2))
			{
				regionsToRegenerate.Enqueue(regionIdForTile2);
			}
		}
		TmpChangedNeighbours.Clear();
	}

	public override void Render()
	{
		if (regionsToRegenerate.Count > 0)
		{
			int regionId = regionsToRegenerate.Dequeue();
			RegenerateRegion(regionId);
		}
		base.Render();
	}
}
