using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class Zone_Fishing : Zone
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass51_0
	{
		public Pawn selPawn;

		internal bool _003CGetFloatMenuOptions_003Eb__0(IntVec3 c)
		{
			return c.IsForbidden(selPawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass51_1
	{
		public IntVec3 cell;

		public IntVec3 spot;

		public _003C_003Ec__DisplayClass51_0 CS_0024_003C_003E8__locals1;

		internal void _003CGetFloatMenuOptions_003Eb__1()
		{
			CS_0024_003C_003E8__locals1.selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Fish, cell, spot), JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__51 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		public Zone_Fishing _003C_003E4__this;

		private _003C_003Ec__DisplayClass51_0 _003C_003E8__1;

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
		public _003CGetFloatMenuOptions_003Ed__51(int _003C_003E1__state)
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
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Zone_Fishing zone_Fishing = _003C_003E4__this;
				int num2;
				bool flag;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass51_0();
					_003C_003E8__1.selPawn = selPawn;
					_003C_003E7__wrap1 = ((Zone)zone_Fishing).GetFloatMenuOptions(_003C_003E8__1.selPawn).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00c3;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00c3;
				case 2:
					_003C_003E1__state = -1;
					return false;
				case 3:
					_003C_003E1__state = -1;
					return false;
				case 4:
					_003C_003E1__state = -1;
					return false;
				case 5:
					_003C_003E1__state = -1;
					return false;
				case 6:
					_003C_003E1__state = -1;
					return false;
				case 7:
					_003C_003E1__state = -1;
					return false;
				case 8:
					_003C_003E1__state = -1;
					goto IL_0511;
				case 9:
					_003C_003E1__state = -1;
					goto IL_0511;
				case 10:
					_003C_003E1__state = -1;
					return false;
				case 11:
					_003C_003E1__state = -1;
					return false;
				case 12:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0511:
					return false;
					IL_00c3:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!zone_Fishing.ShouldFishNow)
					{
						if (!zone_Fishing.OverTargetPopulation)
						{
							_003C_003E2__current = new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "FishingSpotUnderTargetPopulation".Translate().CapitalizeFirst(), null);
							_003C_003E1__state = 2;
							return true;
						}
						if (!zone_Fishing.Allowed)
						{
							_003C_003E2__current = new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "ForbiddenLower".Translate().CapitalizeFirst(), null);
							_003C_003E1__state = 3;
							return true;
						}
						if (!zone_Fishing.UnderTargetResourceCount)
						{
							_003C_003E2__current = new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "FishingSpotOverTargetResourceCount".Translate().CapitalizeFirst(), null);
							_003C_003E1__state = 4;
							return true;
						}
						if (zone_Fishing.PausedDueToResourceCount)
						{
							_003C_003E2__current = new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "Paused".Translate().CapitalizeFirst(), null);
							_003C_003E1__state = 5;
							return true;
						}
						return false;
					}
					if (_003C_003E8__1.selPawn.Ideo != null && !new HistoryEvent(HistoryEventDefOf.SlaughteredFish, _003C_003E8__1.selPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job())
					{
						_003C_003E2__current = new FloatMenuOption($"{"CannotFish".Translate().CapitalizeFirst()}: {"IdeoligionForbids".Translate()} {"Fishing".Translate()}", null);
						_003C_003E1__state = 6;
						return true;
					}
					if (_003C_003E8__1.selPawn.WorkTypeIsDisabled(WorkTypeDefOf.Fishing))
					{
						_003C_003E2__current = new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "CannotPrioritizeWorkTypeDisabled".Translate(WorkTypeDefOf.Fishing.gerundLabel), null);
						_003C_003E1__state = 7;
						return true;
					}
					if (!zone_Fishing.FishbleCells.Any())
					{
						if (zone_Fishing.AllFishableCellsFrozenNow)
						{
							_003C_003E2__current = new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "FishingSpotFrozen".Translate().CapitalizeFirst(), null);
							_003C_003E1__state = 8;
							return true;
						}
						_003C_003E2__current = new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "FishingSpotBlocked".Translate().CapitalizeFirst(), null);
						_003C_003E1__state = 9;
						return true;
					}
					if (!_003C_003E8__1.selPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
					{
						_003C_003E2__current = new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "IncapableOfCapacity".Translate(PawnCapacityDefOf.Manipulation.label).CapitalizeFirst(), null);
						_003C_003E1__state = 10;
						return true;
					}
					num2 = 0;
					flag = false;
					while (num2 < 60)
					{
						_003C_003Ec__DisplayClass51_1 CS_0024_003C_003E8__locals14 = new _003C_003Ec__DisplayClass51_1
						{
							CS_0024_003C_003E8__locals1 = _003C_003E8__1,
							cell = zone_Fishing.RandomFishableCell
						};
						if (CS_0024_003C_003E8__locals14.cell.IsValid && CS_0024_003C_003E8__locals14.cell.Standable(zone_Fishing.Map) && !CS_0024_003C_003E8__locals14.cell.IsForbidden(CS_0024_003C_003E8__locals14.CS_0024_003C_003E8__locals1.selPawn) && CS_0024_003C_003E8__locals14.CS_0024_003C_003E8__locals1.selPawn.CanReserve(CS_0024_003C_003E8__locals14.cell))
						{
							CS_0024_003C_003E8__locals14.spot = WorkGiver_Fish.BestStandSpotFor(CS_0024_003C_003E8__locals14.CS_0024_003C_003E8__locals1.selPawn, CS_0024_003C_003E8__locals14.cell);
							if (CS_0024_003C_003E8__locals14.spot.IsValid && (flag || !CS_0024_003C_003E8__locals14.spot.GetTerrain(zone_Fishing.Map).IsWater))
							{
								_003C_003E2__current = new FloatMenuOption("PrioritizeGenericSimple".Translate("Fishing".Translate()), delegate
								{
									CS_0024_003C_003E8__locals14.CS_0024_003C_003E8__locals1.selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Fish, CS_0024_003C_003E8__locals14.cell, CS_0024_003C_003E8__locals14.spot), JobTag.Misc);
								});
								_003C_003E1__state = 11;
								return true;
							}
						}
						num2++;
						if (num2 >= 60 && !flag)
						{
							flag = true;
							num2 = 0;
						}
					}
					if (zone_Fishing.cells.All((IntVec3 c) => c.IsForbidden(_003C_003E8__1.selPawn)))
					{
						_003C_003E2__current = new FloatMenuOption("CannotFish".Translate().CapitalizeFirst() + ": " + "CannotPrioritizeForbiddenOutsideAllowedArea".Translate().CapitalizeFirst(), null);
						_003C_003E1__state = 12;
						return true;
					}
					break;
				}
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
			_003CGetFloatMenuOptions_003Ed__51 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__51(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__52 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Zone_Fishing _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__52(int _003C_003E1__state)
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
				Zone_Fishing CS_0024_003C_003E8__locals5 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Hide_ZoneFishing(CS_0024_003C_003E8__locals5);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Zone)CS_0024_003C_003E8__locals5).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0097;
				case 2:
					_003C_003E1__state = -3;
					goto IL_0097;
				case 3:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0097:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = new Command_Toggle
					{
						defaultLabel = "CommandAllowFish".Translate(),
						defaultDesc = "CommandAllowFishDesc".Translate(),
						icon = TexCommand.ForbidOff,
						hotKey = KeyBindingDefOf.Command_ItemForbid,
						isActive = () => CS_0024_003C_003E8__locals5.allowed,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals5.allowed = !CS_0024_003C_003E8__locals5.allowed;
						}
					};
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
			_003CGetGizmos_003Ed__52 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__52(0)
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
	private sealed class _003CGetZoneAddGizmos_003Ed__47 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
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
		public _003CGetZoneAddGizmos_003Ed__47(int _003C_003E1__state)
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
				_003C_003E2__current = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Fishing_Expand>();
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
			return new _003CGetZoneAddGizmos_003Ed__47(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private bool allowed = true;

	public int repeatCount = 1;

	public int targetCount = 100;

	public float targetPopulationPct = 0.5f;

	public FishRepeatMode repeatMode = FishRepeatMode.TargetCount;

	public bool pauseWhenSatisfied;

	public int unpauseAtCount;

	private bool pausedDueToResourceCount;

	private bool cachedIsFrozen;

	private int cachedFrozenTicks = int.MinValue;

	private int lastPauseCheckTicks = int.MinValue;

	private readonly ITab[] ITabs = new ITab[1]
	{
		new ITab_Fishing()
	};

	private const string FishTexPath = "UI/Designators/ZoneCreate_Fishing";

	private const int FrozenCheckInterval = 60;

	private List<IntVec3> tmpFishableCells = new List<IntVec3>();

	public override bool IsMultiselectable => true;

	protected override Color NextZoneColor => ZoneColorUtility.NextFishingZoneColor();

	public bool Allowed => allowed;

	public bool ShouldFishNow
	{
		get
		{
			if (Allowed && OverTargetPopulation && !PausedDueToResourceCount)
			{
				return UnderTargetResourceCount;
			}
			return false;
		}
	}

	private bool AllFishableCellsFrozenNow => cells.FindIndex(delegate(IntVec3 c)
	{
		TerrainDef terrain = c.GetTerrain(base.Map);
		return terrain == null || !terrain.temporary || terrain != TerrainDefOf.ThinIce;
	}) < 0;

	private bool UnderTargetResourceCount => repeatMode switch
	{
		FishRepeatMode.DoForever => true, 
		FishRepeatMode.TargetCount => OwnedFishCount < targetCount, 
		FishRepeatMode.RepeatCount => repeatCount > 0, 
		_ => OwnedFishCount < repeatCount, 
	};

	public int OwnedFishCount => base.Map.listerThings.ThingsInGroup(ThingRequestGroup.Fish).Sum((Thing t) => t.stackCount) + CarriedFishCount();

	private bool OverTargetPopulation
	{
		get
		{
			if (base.Map.TileInfo.MaxFishPopulation <= 0f || cells.Count == 0 || FishType == WaterBodyType.None)
			{
				return false;
			}
			return base.Map.waterBodyTracker.PopulationPercentAt(base.Cells[0]) > targetPopulationPct;
		}
	}

	public WaterBodyType FishType
	{
		get
		{
			if (cells.Count == 0)
			{
				return WaterBodyType.None;
			}
			return base.Cells[0].GetWaterBodyType(base.Map);
		}
	}

	public bool AllFishableCellsFrozen
	{
		get
		{
			if (Find.TickManager.TicksGame < cachedFrozenTicks + 60)
			{
				return cachedIsFrozen;
			}
			cachedFrozenTicks = Find.TickManager.TicksGame;
			cachedIsFrozen = AllFishableCellsFrozenNow;
			return cachedIsFrozen;
		}
	}

	public bool PausedDueToResourceCount
	{
		get
		{
			if (Find.TickManager.TicksGame < lastPauseCheckTicks + 60)
			{
				return pausedDueToResourceCount;
			}
			RecheckPausedDueToResourceCount();
			return pausedDueToResourceCount;
		}
	}

	public bool HasAnyFishableCells
	{
		get
		{
			for (int i = 0; i < cells.Count; i++)
			{
				if (IsFishable(cells[i]))
				{
					return true;
				}
			}
			return false;
		}
	}

	public List<IntVec3> FishbleCells
	{
		get
		{
			tmpFishableCells.Clear();
			for (int i = 0; i < cells.Count; i++)
			{
				IntVec3 intVec = cells[i];
				if (IsFishable(intVec))
				{
					tmpFishableCells.Add(intVec);
				}
			}
			return tmpFishableCells;
		}
	}

	public IntVec3 RandomFishableCell
	{
		get
		{
			if (FishbleCells.TryRandomElement(out var result))
			{
				return result;
			}
			return IntVec3.Invalid;
		}
	}

	public bool IsFishable(IntVec3 cell)
	{
		if (cell.GetTerrain(base.Map).IsWater)
		{
			return cell.GetWaterBodyType(base.Map) == FishType;
		}
		return false;
	}

	public Zone_Fishing()
	{
	}

	public Zone_Fishing(ZoneManager zoneManager)
		: base("Zone_Fishing".Translate(), zoneManager)
	{
	}

	public override IEnumerable<InspectTabBase> GetInspectTabs()
	{
		return ITabs;
	}

	[IteratorStateMachine(typeof(_003CGetZoneAddGizmos_003Ed__47))]
	public override IEnumerable<Gizmo> GetZoneAddGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetZoneAddGizmos_003Ed__47(-2);
	}

	private int CarriedFishCount()
	{
		int num = 0;
		foreach (Pawn item in base.Map.mapPawns.FreeColonistsSpawned)
		{
			Thing carriedThing = item.carryTracker.CarriedThing;
			if (carriedThing != null && ThingRequestGroup.Fish.Includes(carriedThing.def) && !carriedThing.Spawned)
			{
				num += carriedThing.stackCount;
			}
		}
		return num;
	}

	public void Notify_Fished()
	{
		if (repeatMode == FishRepeatMode.RepeatCount)
		{
			repeatCount--;
		}
		RecheckPausedDueToResourceCount();
	}

	public void RecheckPausedDueToResourceCount()
	{
		lastPauseCheckTicks = Find.TickManager.TicksGame;
		if (repeatMode != FishRepeatMode.TargetCount || !pauseWhenSatisfied)
		{
			pausedDueToResourceCount = false;
		}
		else if (pausedDueToResourceCount)
		{
			pausedDueToResourceCount = OwnedFishCount > unpauseAtCount;
		}
		else
		{
			pausedDueToResourceCount = OwnedFishCount >= targetCount;
		}
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__51))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__51(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__52))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__52(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetInspectString()
	{
		string inspectString = base.GetInspectString();
		float num = base.Map.waterBodyTracker.FishPopulationAt(base.Cells[0]);
		inspectString += $"\n{"FishPopulation".Translate().CapitalizeFirst()}: {num:F0} ({FishingUtility.FishPopulationLabel(num)})";
		if (base.Map.gameConditionManager.FishPopulationOffsetFactorPerDay(base.Map, out var culprit) < 0f && culprit != null)
		{
			inspectString += $"\n{"FishPopulationDecline".Translate().CapitalizeFirst()}: {culprit.LabelCap}";
		}
		if (!ShouldFishNow)
		{
			if (!UnderTargetResourceCount)
			{
				inspectString += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "FishingSpotOverTargetResourceCount".Translate() + ")";
			}
			else if (PausedDueToResourceCount)
			{
				inspectString += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "Paused".Translate() + ")";
			}
			else if (!OverTargetPopulation)
			{
				inspectString += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "FishingSpotUnderTargetPopulation".Translate() + ")";
			}
			else if (!Allowed)
			{
				inspectString += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "ForbiddenLower".Translate() + ")";
			}
		}
		else if (AllFishableCellsFrozen)
		{
			inspectString += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "FishingSpotFrozen".Translate() + ")";
		}
		else if (!FishbleCells.Any())
		{
			inspectString += "\n" + "CannotFish".Translate().CapitalizeFirst() + " (" + "FishingSpotBlocked".Translate() + ")";
		}
		return inspectString;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref allowed, "allowed", defaultValue: true);
		Scribe_Values.Look(ref repeatMode, "repeatMode", FishRepeatMode.TargetCount);
		Scribe_Values.Look(ref repeatCount, "repeatCount", 1);
		Scribe_Values.Look(ref targetCount, "targetCount", 100);
		Scribe_Values.Look(ref targetPopulationPct, "targetPopulationPct", 0.5f);
		Scribe_Values.Look(ref pauseWhenSatisfied, "pauseWhenSatisfied", defaultValue: false);
		Scribe_Values.Look(ref unpauseAtCount, "unpauseAtCount", 50);
		Scribe_Values.Look(ref pausedDueToResourceCount, "pausedDueToResourceCount", defaultValue: false);
	}
}
