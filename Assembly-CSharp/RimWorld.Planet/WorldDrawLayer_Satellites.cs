using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class WorldDrawLayer_Satellites : WorldDrawLayer
{
	private struct Orbit
	{
		public float inclination;

		public float angleOfAscendingNode;

		public float altitude;

		public float rotationAngle;

		public float angleDeltaSecond;
	}

	[CompilerGenerated]
	private sealed class _003CRegenerate_003Ed__9 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldDrawLayer_Satellites _003C_003E4__this;

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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WorldDrawLayer_Satellites worldDrawLayer_Satellites = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (worldDrawLayer_Satellites.material == null)
					{
						worldDrawLayer_Satellites.material = MaterialPool.MatFrom("PlaceholderImage", ShaderDatabase.WorldOverlayTransparentLit, 3600);
					}
					if (worldDrawLayer_Satellites.orbits == null)
					{
						worldDrawLayer_Satellites.orbits = new List<Orbit>
						{
							new Orbit
							{
								inclination = 15f,
								angleOfAscendingNode = 0f,
								altitude = 25f,
								rotationAngle = 0f,
								angleDeltaSecond = -4f
							}
						};
					}
					_003C_003E7__wrap1 = ((WorldDrawLayer)worldDrawLayer_Satellites).Regenerate().GetEnumerator();
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
				float num2 = (GenTicks.TicksGame - worldDrawLayer_Satellites.lastUpdate).TicksToSeconds();
				for (int i = 0; i < worldDrawLayer_Satellites.orbits.Count; i++)
				{
					Orbit value = worldDrawLayer_Satellites.orbits[i];
					WorldRendererUtility.PrintQuadTangentialToPlanet(Quaternion.AngleAxis(value.angleOfAscendingNode, Vector3.up) * Quaternion.Euler(0f, 0f, value.inclination) * Quaternion.Euler(0f, value.rotationAngle, 0f) * (-Vector3.forward * (100f + value.altitude)), subMesh: worldDrawLayer_Satellites.GetSubMesh(worldDrawLayer_Satellites.material), size: worldDrawLayer_Satellites.planetLayer.AverageTileSize * 6f, altOffset: 0f);
					value.rotationAngle = (value.rotationAngle + value.angleDeltaSecond * num2) % 360f;
					worldDrawLayer_Satellites.orbits[i] = value;
				}
				Rand.PopState();
				worldDrawLayer_Satellites.FinalizeMesh(MeshParts.All);
				worldDrawLayer_Satellites.lastUpdate = GenTicks.TicksGame;
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

	private const float DistanceToOrbit = 25f;

	private Material material;

	private int lastUpdate;

	private List<Orbit> orbits;

	protected override Quaternion Rotation => Quaternion.identity;

	public override bool ShouldRegenerate => lastUpdate != GenTicks.TicksGame;

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
