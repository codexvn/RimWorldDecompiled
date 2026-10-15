using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class PlaceWorker_WatermillGenerator : PlaceWorker
{
	[CompilerGenerated]
	private sealed class _003CDisplayAffordances_003Ed__4 : IEnumerable<TerrainAffordanceDef>, IEnumerable, IEnumerator<TerrainAffordanceDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TerrainAffordanceDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		TerrainAffordanceDef IEnumerator<TerrainAffordanceDef>.Current
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
		public _003CDisplayAffordances_003Ed__4(int _003C_003E1__state)
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
				_003C_003E2__current = TerrainAffordanceDefOf.Heavy;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = TerrainAffordanceDefOf.MovingFluid;
				_003C_003E1__state = 2;
				return true;
			case 2:
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
		IEnumerator<TerrainAffordanceDef> IEnumerable<TerrainAffordanceDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CDisplayAffordances_003Ed__4(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TerrainAffordanceDef>)this).GetEnumerator();
		}
	}

	private static List<Thing> waterMills = new List<Thing>();

	public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
	{
		foreach (IntVec3 item in CompPowerPlantWater.GroundCells(loc, rot))
		{
			if (!item.GetAffordances(map).Contains(TerrainAffordanceDefOf.Heavy))
			{
				return new AcceptanceReport("TerrainCannotSupport_TerrainAffordance".Translate(checkingDef, TerrainAffordanceDefOf.Heavy));
			}
		}
		if (!WaterCellsPresent(loc, rot, map))
		{
			return new AcceptanceReport("MustBeOnMovingWater".Translate());
		}
		return true;
	}

	private bool WaterCellsPresent(IntVec3 loc, Rot4 rot, Map map)
	{
		foreach (IntVec3 item in CompPowerPlantWater.WaterCells(loc, rot))
		{
			if (!item.InBounds(map) || !item.GetAffordances(map).Contains(TerrainAffordanceDefOf.MovingFluid))
			{
				return false;
			}
		}
		return true;
	}

	public override void DrawGhost(ThingDef def, IntVec3 loc, Rot4 rot, Color ghostCol, Thing thing = null)
	{
		GenDraw.DrawFieldEdges(CompPowerPlantWater.GroundCells(loc, rot).ToList(), Color.white, null);
		Color color = (WaterCellsPresent(loc, rot, Find.CurrentMap) ? Designator_Place.CanPlaceColor.ToOpaque() : Designator_Place.CannotPlaceColor.ToOpaque());
		GenDraw.DrawFieldEdges(CompPowerPlantWater.WaterCells(loc, rot).ToList(), color, null);
		bool flag = false;
		CellRect cellRect = CompPowerPlantWater.WaterUseRect(loc, rot);
		waterMills.AddRange(Find.CurrentMap.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.WatermillGenerator).Cast<Thing>());
		waterMills.AddRange(from t in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint)
			where t.def.entityDefToBuild == ThingDefOf.WatermillGenerator
			select t);
		waterMills.AddRange(from t in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame)
			where t.def.entityDefToBuild == ThingDefOf.WatermillGenerator
			select t);
		foreach (Thing waterMill in waterMills)
		{
			GenDraw.DrawFieldEdges(CompPowerPlantWater.WaterUseCells(waterMill.Position, waterMill.Rotation).ToList(), new Color(0.2f, 0.2f, 1f), null);
			if (cellRect.Overlaps(CompPowerPlantWater.WaterUseRect(waterMill.Position, waterMill.Rotation)))
			{
				flag = true;
			}
		}
		waterMills.Clear();
		Color color2 = (flag ? new Color(1f, 0.6f, 0f) : Designator_Place.CanPlaceColor.ToOpaque());
		if (!flag || Time.realtimeSinceStartup % 0.4f < 0.2f)
		{
			GenDraw.DrawFieldEdges(CompPowerPlantWater.WaterUseCells(loc, rot).ToList(), color2, null);
		}
	}

	[IteratorStateMachine(typeof(_003CDisplayAffordances_003Ed__4))]
	public override IEnumerable<TerrainAffordanceDef> DisplayAffordances()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CDisplayAffordances_003Ed__4(-2);
	}
}
