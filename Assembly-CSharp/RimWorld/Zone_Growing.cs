using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Zone_Growing : Zone, IPlantToGrowSettable
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__19 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Zone_Growing _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

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
		public _003CGetGizmos_003Ed__19(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
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
				Zone_Growing CS_0024_003C_003E8__locals9 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Hide_ZoneGrow(CS_0024_003C_003E8__locals9);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Zone)CS_0024_003C_003E8__locals9).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_009f;
				case 2:
					_003C_003E1__state = -3;
					goto IL_009f;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Toggle
					{
						defaultLabel = "CommandAllowSow".Translate(),
						defaultDesc = "CommandAllowSowDesc".Translate(),
						hotKey = KeyBindingDefOf.Command_ItemForbid,
						icon = TexCommand.ForbidOff,
						isActive = () => CS_0024_003C_003E8__locals9.allowSow,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals9.allowSow = !CS_0024_003C_003E8__locals9.allowSow;
						}
					};
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Toggle
					{
						defaultLabel = "CommandAllowCut".Translate(),
						defaultDesc = "CommandAllowCutDesc".Translate(),
						icon = Designator_PlantsCut.IconTex,
						isActive = () => CS_0024_003C_003E8__locals9.allowCut,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals9.allowCut = !CS_0024_003C_003E8__locals9.allowCut;
						}
					};
					_003C_003E1__state = 5;
					return true;
				case 5:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_009f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = PlantToGrowSettableUtility.SetPlantToGrowCommand(CS_0024_003C_003E8__locals9);
					_003C_003E1__state = 3;
					return true;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__19 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__19(0)
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

	[CompilerGenerated]
	private sealed class _003CGetZoneAddGizmos_003Ed__20 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

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
		public _003CGetZoneAddGizmos_003Ed__20(int _003C_003E1__state)
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
				_003C_003E2__current = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing_Expand>();
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
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGetZoneAddGizmos_003Ed__20(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private ThingDef plantDefToGrow;

	public bool allowSow = true;

	public bool allowCut = true;

	public override bool IsMultiselectable => true;

	protected override Color NextZoneColor => ZoneColorUtility.NextGrowingZoneColor();

	IEnumerable<IntVec3> IPlantToGrowSettable.Cells => base.Cells;

	public ThingDef PlantDefToGrow
	{
		get
		{
			if (plantDefToGrow != null)
			{
				return plantDefToGrow;
			}
			if (PollutionUtility.SettableEntirelyPolluted(this))
			{
				plantDefToGrow = ThingDefOf.Plant_Toxipotato;
			}
			else
			{
				plantDefToGrow = ThingDefOf.Plant_Potato;
			}
			return plantDefToGrow;
		}
	}

	public Zone_Growing()
	{
	}

	public Zone_Growing(ZoneManager zoneManager)
		: base("GrowingZone".Translate(), zoneManager)
	{
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref plantDefToGrow, "plantDefToGrow");
		Scribe_Values.Look(ref allowSow, "allowSow", defaultValue: true);
		Scribe_Values.Look(ref allowCut, "allowCut", defaultValue: true);
	}

	public void ContentsStatistics(out int totalPlants, out float averagePlantAgeTicks, out int oldestPlantAgeTicks, out float averagePlantGrowth, out float maxPlantGrowth)
	{
		averagePlantAgeTicks = 0f;
		totalPlants = 0;
		oldestPlantAgeTicks = 0;
		averagePlantGrowth = 0f;
		maxPlantGrowth = 0f;
		foreach (IntVec3 cell in base.Cells)
		{
			foreach (Thing thing in cell.GetThingList(base.Map))
			{
				if (thing.def == plantDefToGrow && thing is Plant plant)
				{
					totalPlants++;
					averagePlantAgeTicks += plant.Age;
					oldestPlantAgeTicks = Mathf.Max(oldestPlantAgeTicks, plant.Age);
					averagePlantGrowth += plant.Growth;
					maxPlantGrowth = Mathf.Max(maxPlantGrowth, plant.Growth);
				}
			}
		}
		averagePlantGrowth /= totalPlants;
		averagePlantAgeTicks /= totalPlants;
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
		stringBuilder.AppendLine();
		if (!base.Cells.NullOrEmpty())
		{
			ContentsStatistics(out var totalPlants, out var averagePlantAgeTicks, out var oldestPlantAgeTicks, out var averagePlantGrowth, out var maxPlantGrowth);
			if (totalPlants > 0)
			{
				string value = Mathf.RoundToInt(averagePlantAgeTicks).ToStringTicksToPeriodVerbose();
				string value2 = oldestPlantAgeTicks.ToStringTicksToPeriodVerbose();
				stringBuilder.AppendLine($"{"Contains".Translate().CapitalizeFirst()}: {totalPlants} {Find.ActiveLanguageWorker.Pluralize(plantDefToGrow.label, totalPlants)}");
				stringBuilder.AppendLine($"{"AveragePlantAge".Translate().CapitalizeFirst()}: {value} ({"PercentGrowth".Translate(averagePlantGrowth.ToStringPercent())})");
				stringBuilder.AppendLine($"{"OldestPlantAge".Translate().CapitalizeFirst()}: {value2} ({"PercentGrowth".Translate(maxPlantGrowth.ToStringPercent())})");
			}
			IntVec3 c = base.Cells.First();
			if (c.UsesOutdoorTemperature(base.Map))
			{
				stringBuilder.AppendLine($"{"OutdoorGrowingPeriod".Translate()}: {GrowingQuadrumsDescription(base.Map, plantDefToGrow)}");
			}
			if (PlantUtility.GrowthSeasonNow(c, base.Map, plantDefToGrow))
			{
				stringBuilder.Append("GrowSeasonHereNow".Translate());
			}
			else
			{
				stringBuilder.Append("CannotGrowBadSeasonTemperature".Translate());
			}
		}
		return stringBuilder.ToString();
	}

	public static string GrowingQuadrumsDescription(PlanetTile tile, ThingDef plantDef = null)
	{
		if (!tile.Valid)
		{
			return "NoGrowingPeriod".Translate();
		}
		float minTemp = plantDef?.plant.minOptimalGrowthTemperature ?? 6f;
		float maxTemp = plantDef?.plant.maxOptimalGrowthTemperature ?? 42f;
		List<Twelfth> list = GenTemperature.TwelfthsInAverageTemperatureRange(tile, minTemp, maxTemp);
		if (list.NullOrEmpty())
		{
			return "NoGrowingPeriod".Translate();
		}
		if (list.Count == 12)
		{
			return "GrowYearRound".Translate();
		}
		return "PeriodDays".Translate($"{list.Count * 5}/{60}") + (" (" + QuadrumUtility.QuadrumsRangeLabel(list) + ")");
	}

	public static string GrowingQuadrumsDescription(Map map, ThingDef plantDef = null)
	{
		if (map == null)
		{
			return "NoGrowingPeriod".Translate();
		}
		if (!map.IsPocketMap)
		{
			return GrowingQuadrumsDescription(map.Tile, plantDef);
		}
		float num = plantDef?.plant.minOptimalGrowthTemperature ?? 6f;
		float num2 = plantDef?.plant.maxOptimalGrowthTemperature ?? 42f;
		float outdoorTemp = map.mapTemperature.OutdoorTemp;
		if (outdoorTemp >= num && outdoorTemp <= num2)
		{
			return "GrowYearRound".Translate();
		}
		return "NoGrowingPeriod".Translate();
	}

	public override void AddCell(IntVec3 c)
	{
		base.AddCell(c);
		foreach (Thing item in base.Map.thingGrid.ThingsListAt(c))
		{
			if (!item.TryGetComp(out CompPlantPreventCutting comp) || !comp.PreventCutting)
			{
				Designator_PlantsHarvestWood.PossiblyWarnPlayerImportantPlantDesignateCut(item);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__19))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__19(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetZoneAddGizmos_003Ed__20))]
	public override IEnumerable<Gizmo> GetZoneAddGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetZoneAddGizmos_003Ed__20(-2);
	}

	public ThingDef GetPlantDefToGrow()
	{
		return PlantDefToGrow;
	}

	public void SetPlantDefToGrow(ThingDef plantDef)
	{
		plantDefToGrow = plantDef;
	}

	public bool CanAcceptSowNow()
	{
		return true;
	}
}
