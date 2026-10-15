using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public class CompPowerPlantWater : CompPowerPlant
{
	[CompilerGenerated]
	private sealed class _003CGroundCells_003Ed__26 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Rot4 rot;

		public Rot4 _003C_003E3__rot;

		private IntVec3 loc;

		public IntVec3 _003C_003E3__loc;

		private IntVec3 _003CperpOffset_003E5__2;

		IntVec3 IEnumerator<IntVec3>.Current
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
		public _003CGroundCells_003Ed__26(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003CperpOffset_003E5__2 = rot.Rotated(RotationDirection.Counterclockwise).FacingCell;
				_003C_003E2__current = loc - rot.FacingCell;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = loc - rot.FacingCell - _003CperpOffset_003E5__2;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = loc - rot.FacingCell + _003CperpOffset_003E5__2;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = loc;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = loc - _003CperpOffset_003E5__2;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = loc + _003CperpOffset_003E5__2;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CGroundCells_003Ed__26 _003CGroundCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGroundCells_003Ed__ = this;
			}
			else
			{
				_003CGroundCells_003Ed__ = new _003CGroundCells_003Ed__26(0);
			}
			_003CGroundCells_003Ed__.loc = _003C_003E3__loc;
			_003CGroundCells_003Ed__.rot = _003C_003E3__rot;
			return _003CGroundCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CWaterCells_003Ed__20 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Rot4 rot;

		public Rot4 _003C_003E3__rot;

		private IntVec3 loc;

		public IntVec3 _003C_003E3__loc;

		private IntVec3 _003CperpOffset_003E5__2;

		IntVec3 IEnumerator<IntVec3>.Current
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
		public _003CWaterCells_003Ed__20(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003CperpOffset_003E5__2 = rot.Rotated(RotationDirection.Counterclockwise).FacingCell;
				_003C_003E2__current = loc + rot.FacingCell * 3;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = loc + rot.FacingCell * 3 - _003CperpOffset_003E5__2;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = loc + rot.FacingCell * 3 - _003CperpOffset_003E5__2 * 2;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = loc + rot.FacingCell * 3 + _003CperpOffset_003E5__2;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = loc + rot.FacingCell * 3 + _003CperpOffset_003E5__2 * 2;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CWaterCells_003Ed__20 _003CWaterCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CWaterCells_003Ed__ = this;
			}
			else
			{
				_003CWaterCells_003Ed__ = new _003CWaterCells_003Ed__20(0);
			}
			_003CWaterCells_003Ed__.loc = _003C_003E3__loc;
			_003CWaterCells_003Ed__.rot = _003C_003E3__rot;
			return _003CWaterCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CWaterUseCells_003Ed__24 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec3 loc;

		public IntVec3 _003C_003E3__loc;

		private Rot4 rot;

		public Rot4 _003C_003E3__rot;

		private CellRect.Enumerator _003C_003E7__wrap1;

		IntVec3 IEnumerator<IntVec3>.Current
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
		public _003CWaterUseCells_003Ed__24(int _003C_003E1__state)
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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = WaterUseRect(loc, rot).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					IntVec3 current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CWaterUseCells_003Ed__24 _003CWaterUseCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CWaterUseCells_003Ed__ = this;
			}
			else
			{
				_003CWaterUseCells_003Ed__ = new _003CWaterUseCells_003Ed__24(0);
			}
			_003CWaterUseCells_003Ed__.loc = _003C_003E3__loc;
			_003CWaterUseCells_003Ed__.rot = _003C_003E3__rot;
			return _003CWaterUseCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	private float spinPosition;

	private bool cacheDirty = true;

	private bool waterUsable;

	private bool waterDoubleUsed;

	private float spinRate = 1f;

	private const float PowerFactorIfWaterDoubleUsed = 0.3f;

	private const float SpinRateFactor = 1f / 150f;

	private const float BladeOffset = 2.36f;

	private const int BladeCount = 9;

	private const int RebuildCacheIntervalTicks = 1200;

	public static readonly Material BladesMat = MaterialPool.MatFrom("Things/Building/Power/WatermillGenerator/WatermillGeneratorBlades");

	protected override float DesiredPowerOutput
	{
		get
		{
			if (cacheDirty)
			{
				RebuildCache();
			}
			if (!waterUsable)
			{
				return 0f;
			}
			if (waterDoubleUsed)
			{
				return base.DesiredPowerOutput * 0.3f;
			}
			return base.DesiredPowerOutput;
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		spinPosition = Rand.Range(0f, 15f);
		RebuildCache();
		ForceOthersToRebuildCache(parent.Map);
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		base.PostDeSpawn(map, mode);
		ForceOthersToRebuildCache(map);
	}

	private void ClearCache()
	{
		cacheDirty = true;
	}

	private void RebuildCache()
	{
		waterUsable = true;
		foreach (IntVec3 item in WaterCells())
		{
			if (item.InBounds(parent.Map) && !item.GetAffordances(parent.Map).Contains(TerrainAffordanceDefOf.MovingFluid))
			{
				waterUsable = false;
				break;
			}
		}
		waterDoubleUsed = false;
		List<Building> list = parent.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.WatermillGenerator);
		foreach (IntVec3 item2 in WaterUseCells())
		{
			if (!item2.InBounds(parent.Map))
			{
				continue;
			}
			foreach (Building item3 in list)
			{
				if (item3 != parent && item3.GetComp<CompPowerPlantWater>().WaterUseRect().Contains(item2))
				{
					waterDoubleUsed = true;
					break;
				}
			}
		}
		if (!waterUsable)
		{
			spinRate = 0f;
			return;
		}
		Vector3 zero = Vector3.zero;
		foreach (IntVec3 item4 in WaterCells())
		{
			zero += parent.Map.waterInfo.GetWaterMovement(item4.ToVector3Shifted());
		}
		spinRate = Mathf.Sign(Vector3.Dot(zero, parent.Rotation.Rotated(RotationDirection.Clockwise).FacingCell.ToVector3()));
		spinRate *= Rand.RangeSeeded(0.9f, 1.1f, parent.thingIDNumber * 60509 + 33151);
		if (waterDoubleUsed)
		{
			spinRate *= 0.5f;
		}
		cacheDirty = false;
	}

	private void ForceOthersToRebuildCache(Map map)
	{
		foreach (Building item in map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.WatermillGenerator))
		{
			item.GetComp<CompPowerPlantWater>().ClearCache();
		}
	}

	public override void CompTick()
	{
		base.CompTick();
		if (base.PowerOutput > 0.01f)
		{
			spinPosition = (spinPosition + 1f / 150f * spinRate + (float)Math.PI * 2f) % ((float)Math.PI * 2f);
		}
		if (parent.IsHashIntervalTick(1200))
		{
			RebuildCache();
		}
	}

	public IEnumerable<IntVec3> WaterCells()
	{
		return WaterCells(parent.Position, parent.Rotation);
	}

	[IteratorStateMachine(typeof(_003CWaterCells_003Ed__20))]
	public static IEnumerable<IntVec3> WaterCells(IntVec3 loc, Rot4 rot)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CWaterCells_003Ed__20(-2)
		{
			_003C_003E3__loc = loc,
			_003C_003E3__rot = rot
		};
	}

	public CellRect WaterUseRect()
	{
		return WaterUseRect(parent.Position, parent.Rotation);
	}

	public static CellRect WaterUseRect(IntVec3 loc, Rot4 rot)
	{
		int width = (rot.IsHorizontal ? 7 : 13);
		int height = (rot.IsHorizontal ? 13 : 7);
		return CellRect.CenteredOn(loc + rot.FacingCell * 4, width, height);
	}

	public IEnumerable<IntVec3> WaterUseCells()
	{
		return WaterUseCells(parent.Position, parent.Rotation);
	}

	[IteratorStateMachine(typeof(_003CWaterUseCells_003Ed__24))]
	public static IEnumerable<IntVec3> WaterUseCells(IntVec3 loc, Rot4 rot)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CWaterUseCells_003Ed__24(-2)
		{
			_003C_003E3__loc = loc,
			_003C_003E3__rot = rot
		};
	}

	public IEnumerable<IntVec3> GroundCells()
	{
		return GroundCells(parent.Position, parent.Rotation);
	}

	[IteratorStateMachine(typeof(_003CGroundCells_003Ed__26))]
	public static IEnumerable<IntVec3> GroundCells(IntVec3 loc, Rot4 rot)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGroundCells_003Ed__26(-2)
		{
			_003C_003E3__loc = loc,
			_003C_003E3__rot = rot
		};
	}

	public override void PostDraw()
	{
		base.PostDraw();
		Vector3 vector = parent.TrueCenter();
		vector += parent.Rotation.FacingCell.ToVector3() * 2.36f;
		for (int i = 0; i < 9; i++)
		{
			float num = spinPosition + (float)Math.PI * 2f * (float)i / 9f;
			float x = Mathf.Abs(4f * Mathf.Sin(num));
			bool num2 = num % ((float)Math.PI * 2f) < (float)Math.PI;
			Vector2 vector2 = new Vector2(x, 1f);
			Vector3 s = new Vector3(vector2.x, 1f, vector2.y);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(vector + Vector3.up * 0.03658537f * Mathf.Cos(num), parent.Rotation.AsQuat, s);
			Graphics.DrawMesh(num2 ? MeshPool.plane10 : MeshPool.plane10Flip, matrix, BladesMat, 0);
		}
	}

	public override string CompInspectStringExtra()
	{
		string text = base.CompInspectStringExtra();
		if (waterUsable && waterDoubleUsed)
		{
			text += "\n" + "Watermill_WaterUsedTwice".Translate();
		}
		return text;
	}
}
