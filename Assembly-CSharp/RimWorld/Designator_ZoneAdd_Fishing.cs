using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Designator_ZoneAdd_Fishing : Designator_ZoneAdd
{
	[CompilerGenerated]
	private sealed class _003Cget_RightClickFloatMenuOptions_003Ed__10 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Designator_ZoneAdd_Fishing _003C_003E4__this;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003Cget_RightClickFloatMenuOptions_003Ed__10(int _003C_003E1__state)
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
				Designator_ZoneAdd_Fishing designator_ZoneAdd_Fishing = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!designator_ZoneAdd_Fishing.ShowRightClickHideOptions)
					{
						return false;
					}
					_003C_003E7__wrap1 = Command_Hide_ZoneFishing.GetHideOptions().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003Cget_RightClickFloatMenuOptions_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_RightClickFloatMenuOptions_003Ed__10(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private readonly List<IntVec3> freshWaterCells = new List<IntVec3>();

	private readonly List<IntVec3> saltWaterCells = new List<IntVec3>();

	private readonly List<Zone> selectedZones = new List<Zone>();

	protected override string NewZoneLabel => "Zone_Fishing".Translate();

	public override bool Visible
	{
		get
		{
			if (ModsConfig.OdysseyActive)
			{
				if (!ResearchProjectDefOf.Fishing.IsFinished)
				{
					return DebugSettings.godMode;
				}
				return true;
			}
			return false;
		}
	}

	protected virtual bool ShowRightClickHideOptions => true;

	public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
	{
		[IteratorStateMachine(typeof(_003Cget_RightClickFloatMenuOptions_003Ed__10))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_RightClickFloatMenuOptions_003Ed__10(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public Designator_ZoneAdd_Fishing()
	{
		zoneTypeToPlace = typeof(Zone_Fishing);
		defaultLabel = "Zone_Fishing".Translate();
		defaultDesc = "DesignatorFishingZoneDesc".Translate();
		tutorTag = "ZoneAdd_Fishing";
		icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Fishing");
	}

	public override AcceptanceReport CanDesignateCell(IntVec3 c)
	{
		if (!c.InBounds(base.Map))
		{
			return false;
		}
		if (c.GetWaterBodyType(base.Map) == WaterBodyType.None || c.GetTerrain(base.Map).passability == Traversability.Impassable || !base.Map.waterBodyTracker.AnyFishPopulationAt(c))
		{
			return "CannotPlaceFishingZone".Translate();
		}
		AcceptanceReport result = base.CanDesignateCell(c);
		if (!result.Accepted)
		{
			return result;
		}
		return true;
	}

	protected override Zone MakeNewZone()
	{
		if (!ModLister.CheckOdyssey("Fishing"))
		{
			return null;
		}
		return new Zone_Fishing(Find.CurrentMap.zoneManager);
	}

	public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
	{
		if (!ModLister.CheckOdyssey("Fishing"))
		{
			return;
		}
		try
		{
			bool flag = false;
			foreach (IntVec3 cell in cells)
			{
				if (!flag && (bool)CanDesignateCell(cell))
				{
					flag = true;
				}
				switch (cell.GetWaterBodyType(base.Map))
				{
				case WaterBodyType.Saltwater:
					saltWaterCells.Add(cell);
					break;
				case WaterBodyType.Freshwater:
					freshWaterCells.Add(cell);
					break;
				}
			}
			if (!flag)
			{
				Finalize(somethingSucceeded: false);
				return;
			}
			Zone_Fishing zone_Fishing = Find.Selector.SelectedZone as Zone_Fishing;
			WaterBodyType? waterBodyType = zone_Fishing?.Cells[0].GetWaterBodyType(base.Map);
			if (zone_Fishing != null)
			{
				selectedZones.Add(zone_Fishing);
			}
			if (waterBodyType != WaterBodyType.Saltwater)
			{
				Find.Selector.ClearSelection();
			}
			if (saltWaterCells.Count > 0)
			{
				base.DesignateMultiCell((IEnumerable<IntVec3>)saltWaterCells);
			}
			if (Find.Selector.SelectedZone is Zone_Fishing zone_Fishing2 && zone_Fishing2 != zone_Fishing)
			{
				selectedZones.Add(zone_Fishing2);
			}
			if (waterBodyType == WaterBodyType.Freshwater)
			{
				Find.Selector.Select(zone_Fishing, playSound: false, forceDesignatorDeselect: false);
			}
			else
			{
				Find.Selector.ClearSelection();
			}
			if (freshWaterCells.Count > 0)
			{
				base.DesignateMultiCell((IEnumerable<IntVec3>)freshWaterCells);
			}
			if (Find.Selector.SelectedZone is Zone_Fishing zone_Fishing3 && zone_Fishing3 != zone_Fishing)
			{
				selectedZones.Add(zone_Fishing3);
			}
			Find.Selector.ClearSelection();
			foreach (Zone selectedZone in selectedZones)
			{
				Find.Selector.Select(selectedZone, playSound: false, forceDesignatorDeselect: false);
			}
		}
		finally
		{
			saltWaterCells.Clear();
			freshWaterCells.Clear();
			selectedZones.Clear();
		}
	}

	protected override void FinalizeDesignationFailed()
	{
		base.FinalizeDesignationFailed();
		Messages.Message("CannotPlaceFishingZone".Translate(), MessageTypeDefOf.RejectInput, historical: false);
	}
}
