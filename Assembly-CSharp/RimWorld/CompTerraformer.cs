using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompTerraformer : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__11 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompTerraformer _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CCompGetGizmosExtra_003Ed__11(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			CompTerraformer compTerraformer = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!DebugSettings.ShowDevGizmos)
				{
					return false;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Convert",
					action = compTerraformer.Convert,
					Disabled = !compTerraformer.Active
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__11(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private int convertTick;

	public CompProperties_Terraformer Props => (CompProperties_Terraformer)props;

	public int TicksTillConvert => Mathf.Max(convertTick - GenTicks.TicksGame, 0);

	private bool Active
	{
		get
		{
			if (!parent.Spawned)
			{
				return false;
			}
			if (!TryGetCellToCovert(out var _))
			{
				return false;
			}
			return true;
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		if (!respawningAfterLoad && !parent.BeingTransportedOnGravship)
		{
			convertTick = GenTicks.TicksGame + Props.secondsPerConvert.SecondsToTicks();
		}
	}

	private bool TryGetCellToCovert(out IntVec3 cell)
	{
		int num = GenRadial.NumCellsInRadius(Props.radius);
		Map map = parent.Map;
		for (int i = 0; i < num; i++)
		{
			cell = parent.Position + GenRadial.RadialPattern[i];
			if (CanEverConvertCell(cell, map, Props.convertTerrainDef))
			{
				return true;
			}
		}
		cell = IntVec3.Invalid;
		return false;
	}

	public static bool CanEverConvertCell(IntVec3 cell, Map map, TerrainDef skip = null)
	{
		if (!cell.InBounds(map))
		{
			return false;
		}
		if (cell.GetEdifice(map) != null)
		{
			return false;
		}
		TerrainDef terrain = cell.GetTerrain(map);
		if (!terrain.canEverTerraform)
		{
			return false;
		}
		if (skip != null && terrain == skip)
		{
			return false;
		}
		if (terrain.passability == Traversability.Impassable)
		{
			return false;
		}
		if (!terrain.affordances.Contains(TerrainAffordanceDefOf.Light))
		{
			return false;
		}
		if (terrain.isFoundation)
		{
			return false;
		}
		return true;
	}

	private void Convert()
	{
		if (TryGetCellToCovert(out var cell))
		{
			parent.Map.terrainGrid.SetTerrain(cell, Props.convertTerrainDef);
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__11))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__11(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void CompTickRare()
	{
		if (GenTicks.TicksGame >= convertTick && Active)
		{
			convertTick = GenTicks.TicksGame + Props.secondsPerConvert.SecondsToTicks();
			Convert();
		}
	}

	public override void PostDrawExtraSelectionOverlays()
	{
		GenDraw.DrawRadiusRing(parent.Position, Props.radius);
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref convertTick, "convertTick", 0);
	}
}
