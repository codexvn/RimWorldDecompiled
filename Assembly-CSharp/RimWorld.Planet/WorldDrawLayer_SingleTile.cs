using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public abstract class WorldDrawLayer_SingleTile : WorldDrawLayer
{
	[CompilerGenerated]
	private sealed class _003CRegenerate_003Ed__8 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldDrawLayer_SingleTile _003C_003E4__this;

		private IEnumerator _003C_003E7__wrap1;

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
		public _003CRegenerate_003Ed__8(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WorldDrawLayer_SingleTile worldDrawLayer_SingleTile = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((WorldDrawLayer)worldDrawLayer_SingleTile).Regenerate().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					object current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				PlanetTile tile = worldDrawLayer_SingleTile.Tile;
				if (tile.Valid && tile.Layer == worldDrawLayer_SingleTile.planetLayer)
				{
					LayerSubMesh subMesh = worldDrawLayer_SingleTile.GetSubMesh(worldDrawLayer_SingleTile.Material);
					Find.WorldGrid.GetTileVertices(tile, worldDrawLayer_SingleTile.verts);
					int count = subMesh.verts.Count;
					int i = 0;
					for (int count2 = worldDrawLayer_SingleTile.verts.Count; i < count2; i++)
					{
						subMesh.verts.Add(worldDrawLayer_SingleTile.verts[i] + worldDrawLayer_SingleTile.verts[i].normalized * 0.02f);
						subMesh.uvs.Add((GenGeo.RegularPolygonVertexPosition(count2, i) + Vector2.one) / 2f);
						if (i < count2 - 2)
						{
							subMesh.tris.Add(count + i + 2);
							subMesh.tris.Add(count + i + 1);
							subMesh.tris.Add(count);
						}
					}
					worldDrawLayer_SingleTile.FinalizeMesh(MeshParts.All);
				}
				worldDrawLayer_SingleTile.lastDrawnPlanetTile = tile;
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
			if (_003C_003E7__wrap1 is IDisposable disposable)
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
			_003CRegenerate_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CRegenerate_003Ed__8(0)
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

	private PlanetTile lastDrawnPlanetTile = new PlanetTile(-1);

	private List<Vector3> verts = new List<Vector3>();

	protected abstract PlanetTile Tile { get; }

	protected abstract Material Material { get; }

	public override bool ShouldRegenerate
	{
		get
		{
			if (!base.ShouldRegenerate)
			{
				return Tile != lastDrawnPlanetTile;
			}
			return true;
		}
	}

	[IteratorStateMachine(typeof(_003CRegenerate_003Ed__8))]
	public override IEnumerable Regenerate()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRegenerate_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}
}
