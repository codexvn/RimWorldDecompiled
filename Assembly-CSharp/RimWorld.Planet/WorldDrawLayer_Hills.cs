using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class WorldDrawLayer_Hills : WorldDrawLayer
{
	[CompilerGenerated]
	private sealed class _003CRegenerate_003Ed__6 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldDrawLayer_Hills _003C_003E4__this;

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
		public _003CRegenerate_003Ed__6(int _003C_003E1__state)
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
				WorldDrawLayer_Hills worldDrawLayer_Hills = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((WorldDrawLayer)worldDrawLayer_Hills).Regenerate().GetEnumerator();
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
				Rand.PushState();
				Rand.Seed = Find.World.info.Seed;
				for (int i = 0; i < worldDrawLayer_Hills.planetLayer.TilesCount; i++)
				{
					Tile tile = worldDrawLayer_Hills.planetLayer[i];
					if (tile.Landmark == null)
					{
						Material material;
						FloatRange floatRange;
						switch (tile.hilliness)
						{
						case Hilliness.SmallHills:
							material = WorldMaterials.SmallHills;
							floatRange = BasePosOffsetRange_SmallHills;
							break;
						case Hilliness.LargeHills:
							material = WorldMaterials.LargeHills;
							floatRange = BasePosOffsetRange_LargeHills;
							break;
						case Hilliness.Mountainous:
							material = WorldMaterials.Mountains;
							floatRange = BasePosOffsetRange_Mountains;
							break;
						case Hilliness.Impassable:
							material = WorldMaterials.ImpassableMountains;
							floatRange = BasePosOffsetRange_ImpassableMountains;
							break;
						default:
							continue;
						}
						LayerSubMesh subMesh = worldDrawLayer_Hills.GetSubMesh(material);
						Vector3 tileCenter = worldDrawLayer_Hills.planetLayer.GetTileCenter(new PlanetTile(i, worldDrawLayer_Hills.planetLayer));
						Vector3 posForTangents = tileCenter;
						float magnitude = tileCenter.magnitude;
						tileCenter = (tileCenter + Rand.UnitVector3 * (floatRange.RandomInRange * worldDrawLayer_Hills.planetLayer.AverageTileSize)).normalized * magnitude;
						WorldRendererUtility.PrintQuadTangentialToPlanet(tileCenter, posForTangents, BaseSizeRange.RandomInRange * worldDrawLayer_Hills.planetLayer.GetTileSize(tile.tile), 0.005f, subMesh, counterClockwise: false, Rand.Range(0f, 360f), printUVs: false);
						WorldRendererUtility.PrintTextureAtlasUVs(Rand.Range(0, TexturesInAtlas.x), Rand.Range(0, TexturesInAtlas.z), TexturesInAtlas.x, TexturesInAtlas.z, subMesh);
					}
				}
				Rand.PopState();
				worldDrawLayer_Hills.FinalizeMesh(MeshParts.All);
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
			_003CRegenerate_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CRegenerate_003Ed__6(0)
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

	private static readonly FloatRange BaseSizeRange = new FloatRange(0.9f, 1.1f);

	private static readonly IntVec2 TexturesInAtlas = new IntVec2(2, 2);

	private static readonly FloatRange BasePosOffsetRange_SmallHills = new FloatRange(0f, 0.37f);

	private static readonly FloatRange BasePosOffsetRange_LargeHills = new FloatRange(0f, 0.2f);

	private static readonly FloatRange BasePosOffsetRange_Mountains = new FloatRange(0f, 0.08f);

	private static readonly FloatRange BasePosOffsetRange_ImpassableMountains = new FloatRange(0f, 0.08f);

	[IteratorStateMachine(typeof(_003CRegenerate_003Ed__6))]
	public override IEnumerable Regenerate()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRegenerate_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}
}
