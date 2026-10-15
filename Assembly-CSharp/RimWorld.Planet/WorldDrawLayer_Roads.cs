using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet;

public class WorldDrawLayer_Roads : WorldDrawLayer_Paths
{
	[CompilerGenerated]
	private sealed class _003CRegenerate_003Ed__7 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldDrawLayer_Roads _003C_003E4__this;

		private LayerSubMesh _003CsubMesh_003E5__2;

		private List<RoadWorldLayerDef> _003CroadLayerDefs_003E5__3;

		private IEnumerator _003C_003E7__wrap3;

		private int _003Ci_003E5__5;

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
			_003CsubMesh_003E5__2 = null;
			_003CroadLayerDefs_003E5__3 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WorldDrawLayer_Roads worldDrawLayer_Roads = _003C_003E4__this;
				SurfaceTile surfaceTile;
				int num2;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap3 = ((WorldDrawLayer)worldDrawLayer_Roads).Regenerate().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						goto IL_0107;
					}
					IL_0072:
					if (_003C_003E7__wrap3.MoveNext())
					{
						object current = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap3 = null;
					_003CsubMesh_003E5__2 = worldDrawLayer_Roads.GetSubMesh(WorldMaterials.Roads);
					_003CroadLayerDefs_003E5__3 = DefDatabase<RoadWorldLayerDef>.AllDefs.OrderBy((RoadWorldLayerDef def) => def.order).ToList();
					_003Ci_003E5__5 = 0;
					goto IL_02e2;
					IL_02e2:
					if (_003Ci_003E5__5 < worldDrawLayer_Roads.planetLayer.TilesCount)
					{
						if (_003Ci_003E5__5 % 1000 == 0)
						{
							_003C_003E2__current = null;
							_003C_003E1__state = 2;
							return true;
						}
						goto IL_0107;
					}
					worldDrawLayer_Roads.FinalizeMesh(MeshParts.All);
					return false;
					IL_0107:
					if (_003CsubMesh_003E5__2.verts.Count > 60000)
					{
						_003CsubMesh_003E5__2 = worldDrawLayer_Roads.GetSubMesh(WorldMaterials.Roads);
					}
					surfaceTile = (SurfaceTile)worldDrawLayer_Roads.planetLayer[_003Ci_003E5__5];
					if (!surfaceTile.WaterCovered)
					{
						List<OutputDirection> list = new List<OutputDirection>();
						if (surfaceTile.potentialRoads != null)
						{
							bool allowSmoothTransition = true;
							for (int i = 0; i < surfaceTile.potentialRoads.Count - 1; i++)
							{
								if (surfaceTile.potentialRoads[i].road.worldTransitionGroup != surfaceTile.potentialRoads[i + 1].road.worldTransitionGroup)
								{
									allowSmoothTransition = false;
								}
							}
							for (int j = 0; j < _003CroadLayerDefs_003E5__3.Count; j++)
							{
								bool flag = false;
								list.Clear();
								for (int k = 0; k < surfaceTile.potentialRoads.Count; k++)
								{
									RoadDef road = surfaceTile.potentialRoads[k].road;
									float layerWidth = road.GetLayerWidth(_003CroadLayerDefs_003E5__3[j]);
									if (layerWidth > 0f)
									{
										flag = true;
									}
									list.Add(new OutputDirection
									{
										neighbor = surfaceTile.potentialRoads[k].neighbor,
										width = layerWidth,
										distortionFrequency = road.distortionFrequency,
										distortionIntensity = road.distortionIntensity
									});
								}
								if (flag)
								{
									worldDrawLayer_Roads.GeneratePaths(_003CsubMesh_003E5__2, new PlanetTile(_003Ci_003E5__5, worldDrawLayer_Roads.planetLayer), list, _003CroadLayerDefs_003E5__3[j].color, allowSmoothTransition);
								}
							}
						}
					}
					num2 = _003Ci_003E5__5 + 1;
					_003Ci_003E5__5 = num2;
					goto IL_02e2;
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

	private readonly ModuleBase roadDisplacementX = new Perlin(1.0, 2.0, 0.5, 3, 74173887, QualityMode.Medium);

	private readonly ModuleBase roadDisplacementY = new Perlin(1.0, 2.0, 0.5, 3, 67515931, QualityMode.Medium);

	private readonly ModuleBase roadDisplacementZ = new Perlin(1.0, 2.0, 0.5, 3, 87116801, QualityMode.Medium);

	public override bool VisibleWhenLayerNotSelected => false;

	public override bool VisibleInBackground => false;

	[IteratorStateMachine(typeof(_003CRegenerate_003Ed__7))]
	public override IEnumerable Regenerate()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRegenerate_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
	{
		Vector3 coordinate = inp * distortionFrequency;
		float magnitude = inp.magnitude;
		Vector3 vector = new Vector3(roadDisplacementX.GetValue(coordinate), roadDisplacementY.GetValue(coordinate), roadDisplacementZ.GetValue(coordinate));
		if ((double)vector.magnitude > 0.0001)
		{
			float num = (1f / (1f + Mathf.Exp((0f - vector.magnitude) / 1f * 2f)) * 2f - 1f) * 1f;
			vector = vector.normalized * num;
		}
		inp = (inp + vector * distortionIntensity).normalized * magnitude;
		return inp + inp.normalized * 0.02f;
	}
}
