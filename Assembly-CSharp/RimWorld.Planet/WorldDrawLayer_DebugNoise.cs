using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using LudeonTK;
using Unity.Collections;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet;

public class WorldDrawLayer_DebugNoise : WorldDrawLayer
{
	[CompilerGenerated]
	private sealed class _003CRegenerate_003Ed__9 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldDrawLayer_DebugNoise _003C_003E4__this;

		private Dialog_DevNoiseWorld _003Cwindow_003E5__2;

		private Perlin _003Cperlin_003E5__3;

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
		public _003CRegenerate_003Ed__9(int _003C_003E1__state)
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
			_003Cwindow_003E5__2 = null;
			_003Cperlin_003E5__3 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WorldDrawLayer_DebugNoise worldDrawLayer_DebugNoise = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (worldDrawLayer_DebugNoise.material == null)
					{
						worldDrawLayer_DebugNoise.material = new Material(WorldMaterials.VertexColorTransparent);
					}
					worldDrawLayer_DebugNoise.TryGetWindow(out _003Cwindow_003E5__2);
					_003Cperlin_003E5__3 = _003Cwindow_003E5__2.noise;
					if (_003Cperlin_003E5__3 == null)
					{
						_003Cperlin_003E5__3 = new Perlin(0.017000000923871994, 2.0, 0.5, 6, normalized: true, invert: false, Find.World.info.Seed, QualityMode.Medium);
					}
					_003C_003E7__wrap3 = ((WorldDrawLayer)worldDrawLayer_DebugNoise).Regenerate().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap3.MoveNext())
				{
					object current = _003C_003E7__wrap3.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap3 = null;
				NativeArray<int> unsafeTileIDToVerts_offsets = worldDrawLayer_DebugNoise.planetLayer.UnsafeTileIDToVerts_offsets;
				NativeArray<Vector3> unsafeVerts = worldDrawLayer_DebugNoise.planetLayer.UnsafeVerts;
				byte a = (byte)Mathf.Clamp(Mathf.RoundToInt(_003Cwindow_003E5__2.alpha * 255.999f), 0, 255);
				worldDrawLayer_DebugNoise.triangleIndexToTileID.Clear();
				for (int i = 0; i < worldDrawLayer_DebugNoise.planetLayer.TilesCount; i++)
				{
					Vector3 tileCenter = Find.WorldGrid.GetTileCenter(new PlanetTile(i, worldDrawLayer_DebugNoise.planetLayer));
					float num2 = (float)_003Cperlin_003E5__3.GetValue(tileCenter.x, tileCenter.y, tileCenter.z);
					byte b = (byte)Mathf.Clamp(Mathf.RoundToInt(num2 * 255.999f), 0, 255);
					if (_003Cwindow_003E5__2.cutoff != 0f && num2 < _003Cwindow_003E5__2.cutoff && _003Cwindow_003E5__2.cutoffEnabled)
					{
						continue;
					}
					int subMeshIndex;
					LayerSubMesh subMesh = worldDrawLayer_DebugNoise.GetSubMesh(worldDrawLayer_DebugNoise.material, out subMeshIndex);
					while (subMeshIndex >= worldDrawLayer_DebugNoise.triangleIndexToTileID.Count)
					{
						worldDrawLayer_DebugNoise.triangleIndexToTileID.Add(new List<int>());
					}
					int count = subMesh.verts.Count;
					int num3 = 0;
					int num4 = ((i + 1 < unsafeTileIDToVerts_offsets.Length) ? unsafeTileIDToVerts_offsets[i + 1] : unsafeVerts.Length);
					for (int j = unsafeTileIDToVerts_offsets[i]; j < num4; j++)
					{
						subMesh.verts.Add(unsafeVerts[j]);
						subMesh.colors.Add(new Color32(b, b, b, a));
						if (j < num4 - 2)
						{
							subMesh.tris.Add(count + num3 + 2);
							subMesh.tris.Add(count + num3 + 1);
							subMesh.tris.Add(count);
							worldDrawLayer_DebugNoise.triangleIndexToTileID[subMeshIndex].Add(i);
						}
						num3++;
					}
				}
				worldDrawLayer_DebugNoise.FinalizeMesh(MeshParts.All);
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			_003CRegenerate_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CRegenerate_003Ed__9(0)
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

	private readonly List<List<int>> triangleIndexToTileID = new List<List<int>>();

	private Material material;

	public override bool Visible
	{
		get
		{
			if (base.Visible && TryGetWindow(out var window))
			{
				return window.alpha > 0f;
			}
			return false;
		}
	}

	public override bool VisibleWhenLayerNotSelected => false;

	public override bool VisibleInBackground => false;

	private bool TryGetWindow(out Dialog_DevNoiseWorld window)
	{
		return Find.WindowStack.TryGetWindow<Dialog_DevNoiseWorld>(out window);
	}

	[IteratorStateMachine(typeof(_003CRegenerate_003Ed__9))]
	public override IEnumerable Regenerate()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRegenerate_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}
}
