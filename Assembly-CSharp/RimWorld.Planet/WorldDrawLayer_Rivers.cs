using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet;

public class WorldDrawLayer_Rivers : WorldDrawLayer_Paths
{
	[CompilerGenerated]
	private sealed class _003CRegenerate_003Ed__11 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldDrawLayer_Rivers _003C_003E4__this;

		private LayerSubMesh _003CsubMesh_003E5__2;

		private LayerSubMesh _003CsubMeshBorder_003E5__3;

		private List<OutputDirection> _003Coutputs_003E5__4;

		private List<OutputDirection> _003CoutputsBorder_003E5__5;

		private IEnumerator _003C_003E7__wrap5;

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
		public _003CRegenerate_003Ed__11(int _003C_003E1__state)
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
			_003CsubMesh_003E5__2 = null;
			_003CsubMeshBorder_003E5__3 = null;
			_003Coutputs_003E5__4 = null;
			_003CoutputsBorder_003E5__5 = null;
			_003C_003E7__wrap5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WorldDrawLayer_Rivers worldDrawLayer_Rivers = _003C_003E4__this;
				SurfaceTile surfaceTile;
				int num2;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap5 = ((WorldDrawLayer)worldDrawLayer_Rivers).Regenerate().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						goto IL_00fa;
					}
					IL_0072:
					if (_003C_003E7__wrap5.MoveNext())
					{
						object current = _003C_003E7__wrap5.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap5 = null;
					_003CsubMesh_003E5__2 = worldDrawLayer_Rivers.GetSubMesh(WorldMaterials.Rivers);
					_003CsubMeshBorder_003E5__3 = worldDrawLayer_Rivers.GetSubMesh(WorldMaterials.RiversBorder);
					_003Coutputs_003E5__4 = new List<OutputDirection>();
					_003CoutputsBorder_003E5__5 = new List<OutputDirection>();
					_003Ci_003E5__7 = 0;
					goto IL_0282;
					IL_0282:
					if (_003Ci_003E5__7 < worldDrawLayer_Rivers.planetLayer.TilesCount)
					{
						if (_003Ci_003E5__7 % 1000 == 0)
						{
							_003C_003E2__current = null;
							_003C_003E1__state = 2;
							return true;
						}
						goto IL_00fa;
					}
					worldDrawLayer_Rivers.FinalizeMesh(MeshParts.All);
					return false;
					IL_00fa:
					if (_003CsubMesh_003E5__2.verts.Count > 60000)
					{
						_003CsubMesh_003E5__2 = worldDrawLayer_Rivers.GetSubMesh(WorldMaterials.Rivers);
						_003CsubMeshBorder_003E5__3 = worldDrawLayer_Rivers.GetSubMesh(WorldMaterials.RiversBorder);
					}
					surfaceTile = (SurfaceTile)worldDrawLayer_Rivers.planetLayer[_003Ci_003E5__7];
					if (surfaceTile.potentialRivers != null)
					{
						_003Coutputs_003E5__4.Clear();
						_003CoutputsBorder_003E5__5.Clear();
						for (int i = 0; i < surfaceTile.potentialRivers.Count; i++)
						{
							_003Coutputs_003E5__4.Add(new OutputDirection
							{
								neighbor = surfaceTile.potentialRivers[i].neighbor,
								width = surfaceTile.potentialRivers[i].river.widthOnWorld - 0.2f
							});
							_003CoutputsBorder_003E5__5.Add(new OutputDirection
							{
								neighbor = surfaceTile.potentialRivers[i].neighbor,
								width = surfaceTile.potentialRivers[i].river.widthOnWorld
							});
						}
						worldDrawLayer_Rivers.GeneratePaths(_003CsubMesh_003E5__2, surfaceTile.tile, _003Coutputs_003E5__4, worldDrawLayer_Rivers.riverColor, allowSmoothTransition: true);
						worldDrawLayer_Rivers.GeneratePaths(_003CsubMeshBorder_003E5__3, surfaceTile.tile, _003CoutputsBorder_003E5__5, worldDrawLayer_Rivers.riverColor, allowSmoothTransition: true);
					}
					num2 = _003Ci_003E5__7 + 1;
					_003Ci_003E5__7 = num2;
					goto IL_0282;
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
			if (_003C_003E7__wrap5 is IDisposable disposable)
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
			_003CRegenerate_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CRegenerate_003Ed__11(0)
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

	private readonly Color32 riverColor = new Color32(73, 82, 100, byte.MaxValue);

	private const float PerlinFrequency = 0.6f;

	private const float PerlinMagnitude = 0.1f;

	private readonly ModuleBase riverDisplacementX = new Perlin(0.6000000238418579, 2.0, 0.5, 3, 84905524, QualityMode.Medium);

	private readonly ModuleBase riverDisplacementY = new Perlin(0.6000000238418579, 2.0, 0.5, 3, 37971116, QualityMode.Medium);

	private readonly ModuleBase riverDisplacementZ = new Perlin(0.6000000238418579, 2.0, 0.5, 3, 91572032, QualityMode.Medium);

	public override bool VisibleWhenLayerNotSelected => false;

	public override bool VisibleInBackground => false;

	public WorldDrawLayer_Rivers()
	{
		pointyEnds = true;
	}

	[IteratorStateMachine(typeof(_003CRegenerate_003Ed__11))]
	public override IEnumerable Regenerate()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRegenerate_003Ed__11(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
	{
		float magnitude = inp.magnitude;
		inp = (inp + new Vector3(riverDisplacementX.GetValue(inp), riverDisplacementY.GetValue(inp), riverDisplacementZ.GetValue(inp)) * 0.1f).normalized * magnitude;
		return inp + inp.normalized * 0.01f;
	}
}
