using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class WorldFeature : IExposable, ILoadReferenceable
{
	[CompilerGenerated]
	private sealed class _003Cget_Tiles_003Ed__27 : IEnumerable<int>, IEnumerable, IEnumerator<int>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private int _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldFeature _003C_003E4__this;

		private WorldGrid _003CworldGrid_003E5__2;

		private int _003CtilesCount_003E5__3;

		private int _003Ci_003E5__4;

		int IEnumerator<int>.Current
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
		public _003Cget_Tiles_003Ed__27(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CworldGrid_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			WorldFeature worldFeature = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0078;
			}
			_003C_003E1__state = -1;
			_003CworldGrid_003E5__2 = Find.WorldGrid;
			_003CtilesCount_003E5__3 = _003CworldGrid_003E5__2.TilesCount;
			_003Ci_003E5__4 = 0;
			goto IL_0088;
			IL_0078:
			_003Ci_003E5__4++;
			goto IL_0088;
			IL_0088:
			if (_003Ci_003E5__4 < _003CtilesCount_003E5__3)
			{
				if (_003CworldGrid_003E5__2[_003Ci_003E5__4].feature == worldFeature)
				{
					_003C_003E2__current = _003Ci_003E5__4;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0078;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<int> IEnumerable<int>.GetEnumerator()
		{
			_003Cget_Tiles_003Ed__27 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Tiles_003Ed__27(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<int>)this).GetEnumerator();
		}
	}

	public int uniqueID;

	public FeatureDef def;

	public PlanetLayer layer;

	public string name;

	public Vector3 drawCenter;

	public float drawAngle;

	public float maxDrawSizeInTiles;

	public float alpha;

	protected static SimpleCurve EffectiveDrawSizeCurve = new SimpleCurve
	{
		new CurvePoint(10f, 15f),
		new CurvePoint(25f, 40f),
		new CurvePoint(50f, 90f),
		new CurvePoint(100f, 150f),
		new CurvePoint(200f, 200f)
	};

	[TweakValue("Interface.World", 0f, 40f)]
	protected static float FeatureSizePoint10 = 15f;

	[TweakValue("Interface.World", 0f, 100f)]
	protected static float FeatureSizePoint25 = 40f;

	[TweakValue("Interface.World", 0f, 200f)]
	protected static float FeatureSizePoint50 = 90f;

	[TweakValue("Interface.World", 0f, 400f)]
	protected static float FeatureSizePoint100 = 150f;

	[TweakValue("Interface.World", 0f, 800f)]
	protected static float FeatureSizePoint200 = 200f;

	public float EffectiveDrawSize => EffectiveDrawSizeCurve.Evaluate(maxDrawSizeInTiles);

	public IEnumerable<int> Tiles
	{
		[IteratorStateMachine(typeof(_003Cget_Tiles_003Ed__27))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Tiles_003Ed__27(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	protected static void FeatureSizePoint10_Changed()
	{
		TweakChanged();
	}

	protected static void FeatureSizePoint25_Changed()
	{
		TweakChanged();
	}

	protected static void FeatureSizePoint50_Changed()
	{
		TweakChanged();
	}

	protected static void FeatureSizePoint100_Changed()
	{
		TweakChanged();
	}

	protected static void FeatureSizePoint200_Changed()
	{
		TweakChanged();
	}

	private static void TweakChanged()
	{
		Find.WorldFeatures.textsCreated = false;
		EffectiveDrawSizeCurve[0] = new CurvePoint(EffectiveDrawSizeCurve[0].x, FeatureSizePoint10);
		EffectiveDrawSizeCurve[1] = new CurvePoint(EffectiveDrawSizeCurve[1].x, FeatureSizePoint25);
		EffectiveDrawSizeCurve[2] = new CurvePoint(EffectiveDrawSizeCurve[2].x, FeatureSizePoint50);
		EffectiveDrawSizeCurve[3] = new CurvePoint(EffectiveDrawSizeCurve[3].x, FeatureSizePoint100);
		EffectiveDrawSizeCurve[4] = new CurvePoint(EffectiveDrawSizeCurve[4].x, FeatureSizePoint200);
	}

	public WorldFeature()
	{
	}

	public WorldFeature(FeatureDef def, PlanetLayer layer)
	{
		this.def = def;
		this.layer = layer;
		uniqueID = Find.UniqueIDsManager.GetNextWorldFeatureID();
	}

	public void ExposeData()
	{
		Scribe_Defs.Look(ref def, "def");
		Scribe_Values.Look(ref uniqueID, "uniqueID", 0);
		Scribe_Values.Look(ref name, "name");
		Scribe_Values.Look(ref drawCenter, "drawCenter");
		Scribe_Values.Look(ref drawAngle, "drawAngle", 0f);
		Scribe_Values.Look(ref maxDrawSizeInTiles, "maxDrawSizeInTiles", 0f);
		Scribe_References.Look(ref layer, "layer");
		if (Scribe.mode == LoadSaveMode.PostLoadInit && layer == null)
		{
			layer = Find.WorldGrid.Surface;
		}
		BackCompatibility.PostExposeData(this);
	}

	public string GetUniqueLoadID()
	{
		return "WorldFeature_" + uniqueID;
	}
}
