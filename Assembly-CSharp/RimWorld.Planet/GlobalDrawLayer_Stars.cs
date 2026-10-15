using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class GlobalDrawLayer_Stars : WorldDrawLayerBase
{
	[CompilerGenerated]
	private sealed class _003CRegenerate_003Ed__14 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public GlobalDrawLayer_Stars _003C_003E4__this;

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
		public _003CRegenerate_003Ed__14(int _003C_003E1__state)
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
				GlobalDrawLayer_Stars globalDrawLayer_Stars = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((WorldDrawLayerBase)globalDrawLayer_Stars).Regenerate().GetEnumerator();
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
				for (int i = 0; i < 1500; i++)
				{
					Vector3 unitVector = Rand.UnitVector3;
					Vector3 pos = unitVector * 20f;
					LayerSubMesh subMesh = globalDrawLayer_Stars.GetSubMesh(WorldMaterials.Stars);
					float num2 = StarsDrawSize.RandomInRange;
					Vector3 rhs = (globalDrawLayer_Stars.UseStaticRotation ? GenCelestial.CurSunPositionInWorldSpace().normalized : Vector3.forward);
					float num3 = Vector3.Dot(unitVector, rhs);
					if (num3 > 0.8f)
					{
						num2 *= GenMath.LerpDouble(0.8f, 1f, 1f, 0.35f, num3);
					}
					WorldRendererUtility.PrintQuadTangentialToPlanet(pos, num2, 0f, subMesh, counterClockwise: true, Rand.Range(0f, 360f));
				}
				globalDrawLayer_Stars.calculatedForStartingTile = ((Find.GameInitData != null) ? Find.GameInitData.startingTile : PlanetTile.Invalid);
				globalDrawLayer_Stars.calculatedForStaticRotation = globalDrawLayer_Stars.UseStaticRotation;
				Rand.PopState();
				globalDrawLayer_Stars.FinalizeMesh(MeshParts.All);
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
			_003CRegenerate_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CRegenerate_003Ed__14(0)
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

	private bool calculatedForStaticRotation;

	private PlanetTile calculatedForStartingTile = PlanetTile.Invalid;

	public const float DistanceToStars = 20f;

	private static readonly FloatRange StarsDrawSize = new FloatRange(1f, 3.8f);

	private const int StarsCount = 1500;

	private const float DistToSunToReduceStarSize = 0.8f;

	protected override int RenderLayer => WorldCameraManager.WorldSkyboxLayer;

	public override bool ShouldRegenerate
	{
		get
		{
			if (!base.ShouldRegenerate && (Find.GameInitData == null || !(Find.GameInitData.startingTile != calculatedForStartingTile)))
			{
				return UseStaticRotation != calculatedForStaticRotation;
			}
			return true;
		}
	}

	private bool UseStaticRotation => Current.ProgramState == ProgramState.Entry;

	protected override Quaternion Rotation
	{
		get
		{
			if (UseStaticRotation)
			{
				return Quaternion.identity;
			}
			return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
		}
	}

	[IteratorStateMachine(typeof(_003CRegenerate_003Ed__14))]
	public override IEnumerable Regenerate()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRegenerate_003Ed__14(-2)
		{
			_003C_003E4__this = this
		};
	}
}
