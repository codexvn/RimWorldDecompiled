using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class WorldDrawLayer_Clouds : WorldDrawLayer
{
	[CompilerGenerated]
	private sealed class _003CRegenerate_003Ed__9 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldDrawLayer_Clouds _003C_003E4__this;

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
				WorldDrawLayer_Clouds worldDrawLayer_Clouds = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((WorldDrawLayer)worldDrawLayer_Clouds).Regenerate().GetEnumerator();
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
				if (worldDrawLayer_Clouds.material == null)
				{
					worldDrawLayer_Clouds.material = new Material(WorldMaterials.Clouds);
					worldDrawLayer_Clouds.material.SetVector(ShaderPropertyIDs.Seed, new Vector2(UnityEngine.Random.value, UnityEngine.Random.value));
					worldDrawLayer_Clouds.material.SetFloat(ShaderPropertyIDs.PlanetRadius, worldDrawLayer_Clouds.planetLayer.Radius);
					worldDrawLayer_Clouds.material.SetVector(ShaderPropertyIDs.PlanetOrigin, worldDrawLayer_Clouds.planetLayer.Origin);
					worldDrawLayer_Clouds.noiseTexture = ContentFinder<Texture2D>.Get("Other/Perlin");
					worldDrawLayer_Clouds.sunsetColorRamp = ContentFinder<Texture2D>.Get("World/SunsetGradient");
					worldDrawLayer_Clouds.cloudTexture = ContentFinder<Texture2D>.Get("World/CloudMap");
					worldDrawLayer_Clouds.material.SetTexture(ShaderPropertyIDs.NoiseTex, worldDrawLayer_Clouds.noiseTexture);
					worldDrawLayer_Clouds.material.SetTexture(ShaderPropertyIDs.SunsetColorRamp, worldDrawLayer_Clouds.sunsetColorRamp);
					worldDrawLayer_Clouds.material.SetTexture(ShaderPropertyIDs.CloudMap, worldDrawLayer_Clouds.cloudTexture);
					worldDrawLayer_Clouds.set = false;
				}
				SphereGenerator.Generate(4, worldDrawLayer_Clouds.planetLayer.Radius + 0.2f, Vector3.forward, 180f, out var outVerts, out var outIndices);
				LayerSubMesh subMesh = worldDrawLayer_Clouds.GetSubMesh(worldDrawLayer_Clouds.material);
				subMesh.verts.AddRange(outVerts);
				subMesh.tris.AddRange(outIndices);
				worldDrawLayer_Clouds.FinalizeMesh(MeshParts.All);
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

	private Material material;

	private Texture2D noiseTexture;

	private Texture2D sunsetColorRamp;

	private Texture2D cloudTexture;

	private float opacity = 1f;

	private float opacityVel;

	private bool set;

	private const int SubdivisionsCount = 4;

	private const float OpacityChangeTime = 0.15f;

	[IteratorStateMachine(typeof(_003CRegenerate_003Ed__9))]
	public override IEnumerable Regenerate()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRegenerate_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}

	private float GetTargetOpacity()
	{
		if (PlanetLayer.Selected == planetLayer && !WorldRendererUtility.WorldBackgroundNow)
		{
			return 0f;
		}
		return 1f;
	}

	public override void Render()
	{
		opacity = ((!set) ? GetTargetOpacity() : Mathf.SmoothDamp(opacity, GetTargetOpacity(), ref opacityVel, 0.15f));
		material.SetFloat(ShaderPropertyIDs.CloudShaderOpacity, opacity);
		set = true;
		base.Render();
	}
}
