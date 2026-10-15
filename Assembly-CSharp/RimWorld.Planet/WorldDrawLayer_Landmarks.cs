using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class WorldDrawLayer_Landmarks : WorldDrawLayer
{
	[CompilerGenerated]
	private sealed class _003CRegenerate_003Ed__7 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldDrawLayer_Landmarks _003C_003E4__this;

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
		public _003CRegenerate_003Ed__7(int _003C_003E1__state)
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
				WorldDrawLayer_Landmarks worldDrawLayer_Landmarks = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((WorldDrawLayer)worldDrawLayer_Landmarks).Regenerate().GetEnumerator();
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
				if (Find.World.landmarks != null)
				{
					foreach (var (tile, landmark2) in Find.World.landmarks.landmarks)
					{
						if (worldDrawLayer_Landmarks.planetLayer == tile.Layer && landmark2.def.drawType == LandmarkDef.LandmarkDrawType.Standard)
						{
							worldDrawLayer_Landmarks.DrawStandard(landmark2, tile);
						}
					}
				}
				worldDrawLayer_Landmarks.FinalizeMesh(MeshParts.All);
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
			_003CRegenerate_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CRegenerate_003Ed__7(0)
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

	private static readonly List<PlanetTile> neighbours = new List<PlanetTile>(8);

	public override bool VisibleWhenLayerNotSelected => false;

	public override bool VisibleInBackground => false;

	public override bool Visible => ModsConfig.OdysseyActive;

	[IteratorStateMachine(typeof(_003CRegenerate_003Ed__7))]
	public override IEnumerable Regenerate()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRegenerate_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void DrawStandard(Landmark landmark, PlanetTile tile)
	{
		LayerSubMesh subMesh = GetSubMesh(landmark.def.Material);
		Vector3 tileCenter = planetLayer.GetTileCenter(tile);
		float rotation = Rand.Range(0f, 360f);
		if (landmark.def.coastRotateMode != 0)
		{
			planetLayer.GetTileNeighbors(tile, neighbours);
			PlanetTile toTile = PlanetTile.Invalid;
			for (int i = 0; i < neighbours.Count; i++)
			{
				if (LandmarkDef.IsValidRotatableNeighbour(planetLayer[neighbours[i]].PrimaryBiome, landmark.def.coastRotateMode))
				{
					toTile = neighbours[i];
					break;
				}
			}
			if (toTile.Valid)
			{
				rotation = planetLayer.GetHeadingFromTo(tile, toTile) - 90f;
			}
		}
		Rand.PushState(tile.tileId);
		WorldRendererUtility.PrintQuadTangentialToPlanet(tileCenter, planetLayer.GetTileSize(tile) * landmark.def.drawScale, landmark.def.drawAboveRoads ? 0.025f : 0.0075f, subMesh, counterClockwise: false, rotation, printUVs: false);
		WorldRendererUtility.PrintTextureAtlasUVs(Rand.Range(0, landmark.def.atlasSize.x), Rand.Range(0, landmark.def.atlasSize.z), landmark.def.atlasSize.x, landmark.def.atlasSize.z, subMesh);
		Rand.PopState();
	}
}
