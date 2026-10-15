using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;
using Verse.Sound;

namespace Verse;

public class Building : ThingWithComps
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__33 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		private IEnumerator<Command> _003C_003E7__wrap2;

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
		public _003CGetGizmos_003Ed__33(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 6:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 7:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building building = _003C_003E4__this;
				Lord lord;
				ColorInt? glowerColorOverride;
				CompGlower comp;
				Gizmo selectMonumentMarkerGizmo;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)building).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0088;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0088;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0112;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0138;
				case 4:
					_003C_003E1__state = -1;
					goto IL_018a;
				case 5:
					_003C_003E1__state = -1;
					goto IL_020f;
				case 6:
					_003C_003E1__state = -4;
					goto IL_0279;
				case 7:
					{
						_003C_003E1__state = -5;
						goto IL_02f2;
					}
					IL_0088:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (((building.def.BuildableByPlayer && building.def.passability != Traversability.Impassable && !building.def.IsDoor) || building.def.building.forceShowRoomStats) && Gizmo_RoomStats.GetRoomToShowStatsFor(building) != null && Find.Selector.SingleSelectedObject == building)
					{
						_003C_003E2__current = new Gizmo_RoomStats(building);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0112;
					IL_0138:
					if (building.def.Minifiable && (building.Faction == Faction.OfPlayer || building.def.building.alwaysUninstallable))
					{
						_003C_003E2__current = InstallationDesignatorDatabase.DesignatorFor(building.def);
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_018a;
					IL_0293:
					lord = building.GetLord();
					if (lord == null || lord.CurLordToil == null)
					{
						break;
					}
					_003C_003E7__wrap1 = lord.CurLordToil.GetBuildingGizmos(building).GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_02f2;
					IL_018a:
					glowerColorOverride = null;
					comp = building.GetComp<CompGlower>();
					if (comp != null && comp.HasGlowColorOverride)
					{
						glowerColorOverride = comp.GlowColor;
					}
					if (!building.def.building.neverBuildable)
					{
						Command command = BuildCopyCommandUtility.BuildCopyCommand(building.def, building.Stuff, building.StyleSourcePrecept as Precept_Building, building.StyleDef, styleOverridden: true, glowerColorOverride);
						if (command != null)
						{
							_003C_003E2__current = command;
							_003C_003E1__state = 5;
							return true;
						}
					}
					goto IL_020f;
					IL_020f:
					if (building.Faction == Faction.OfPlayer || building.def.building.alwaysShowRelatedBuildCommands)
					{
						_003C_003E7__wrap2 = BuildRelatedCommandUtility.RelatedBuildCommands(building.def).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0279;
					}
					goto IL_0293;
					IL_02f2:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 7;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					break;
					IL_0279:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Command current3 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 6;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					goto IL_0293;
					IL_0112:
					selectMonumentMarkerGizmo = QuestUtility.GetSelectMonumentMarkerGizmo(building);
					if (selectMonumentMarkerGizmo != null)
					{
						_003C_003E2__current = selectMonumentMarkerGizmo;
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0138;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
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
			_003CGetGizmos_003Ed__33 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__33(0)
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
	private sealed class _003CSpecialDisplayStats_003Ed__34 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building _003C_003E4__this;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__34(int _003C_003E1__state)
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
				Building building = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)building).SpecialDisplayStats().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (building.PaintColorDef != null && !building.PaintColorDef.label.NullOrEmpty())
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Building, "Stat_Building_PaintColor".Translate(), building.PaintColorDef.LabelCap, "Stat_Building_PaintColorDesc".Translate(), 6000);
						_003C_003E1__state = 2;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__34 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__34(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private ColorDef paintColorDef;

	public bool canChangeTerrainOnDestroyed = true;

	private static readonly SimpleCurve ShakeAmountPerAreaCurve = new SimpleCurve
	{
		new CurvePoint(1f, 0.07f),
		new CurvePoint(2f, 0.07f),
		new CurvePoint(4f, 0.1f),
		new CurvePoint(9f, 0.2f),
		new CurvePoint(16f, 0.5f)
	};

	private const float ChanceToGeneratePaintedFromTrader = 0.1f;

	private const int BuildingMinTickRate = 6;

	public CompPower PowerComp => GetComp<CompPower>();

	public ColorDef PaintColorDef => paintColorDef;

	public override Color DrawColor
	{
		get
		{
			if (paintColorDef != null)
			{
				return paintColorDef.color;
			}
			return base.DrawColor;
		}
	}

	public virtual bool TransmitsPowerNow => PowerComp?.Props.transmitsPower ?? false;

	public override int HitPoints
	{
		set
		{
			int hitPoints = HitPoints;
			base.HitPoints = value;
			BuildingsDamageSectionLayerUtility.Notify_BuildingHitPointsChanged(this, hitPoints);
			if (base.Spawned)
			{
				base.Map.events.Notify_BuildingHitPointsChanged(this);
			}
		}
	}

	public virtual int MaxItemsInCell => def.building.maxItemsInCell;

	public bool IsClearableFreeBuilding
	{
		get
		{
			if (!def.useHitPoints && !def.scatterableOnMapGen && def.passability == Traversability.Standable)
			{
				return this.GetStatValue(StatDefOf.WorkToBuild) == 0f;
			}
			return false;
		}
	}

	public virtual bool IsAirtight
	{
		get
		{
			if (def.building.isAirtight)
			{
				return true;
			}
			if (def.building.isStuffableAirtight && base.Stuff.stuffProps.isAirtight)
			{
				return true;
			}
			return false;
		}
	}

	public virtual bool ExchangeVacuum
	{
		get
		{
			if (IsAirtight)
			{
				return def.building.alwaysExchangeVacuum;
			}
			return true;
		}
	}

	protected override int MinTickIntervalRate => 6;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref canChangeTerrainOnDestroyed, "canChangeTerrainOnDestroyed", defaultValue: true);
		Scribe_Defs.Look(ref paintColorDef, "paintColorDef");
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		if (def.IsEdifice())
		{
			map.edificeGrid.Register(this);
			if (def.Fillage == FillCategory.Full)
			{
				map.terrainGrid.Drawer.SetDirty();
			}
			if (def.AffectsFertility)
			{
				map.fertilityGrid.Drawer.SetDirty();
			}
		}
		base.SpawnSetup(map, respawningAfterLoad);
		base.Map.listerBuildings.Add(this);
		if (def.coversFloor)
		{
			base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Terrain, regenAdjacentCells: true, regenAdjacentSections: false);
		}
		CellRect cellRect = this.OccupiedRect();
		for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
		{
			for (int j = cellRect.minX; j <= cellRect.maxX; j++)
			{
				IntVec3 intVec = new IntVec3(j, 0, i);
				base.Map.mapDrawer.MapMeshDirty(intVec, MapMeshFlagDefOf.Buildings);
				if (def.blockLight)
				{
					base.Map.glowGrid.LightBlockerAdded(intVec);
				}
				if (!SnowGrid.CanCoexistWithSnow(def))
				{
					base.Map.snowGrid.SetDepth(intVec, 0f);
				}
				if (ModsConfig.OdysseyActive && !SandGrid.CanCoexistWithSand(def))
				{
					base.Map.sandGrid.SetDepth(intVec, 0f);
				}
			}
		}
		if (base.Faction == Faction.OfPlayer && def.building != null && def.building.spawnedConceptLearnOpportunity != null)
		{
			LessonAutoActivator.TeachOpportunity(def.building.spawnedConceptLearnOpportunity, OpportunityType.GoodToKnow);
		}
		AutoHomeAreaMaker.Notify_BuildingSpawned(this);
		base.Map.listerBuildingsRepairable.Notify_BuildingSpawned(this);
		base.Map.listerArtificialBuildingsForMeditation.Notify_BuildingSpawned(this);
		base.Map.listerBuldingOfDefInProximity.Notify_BuildingSpawned(this);
		base.Map.listerBuildingWithTagInProximity.Notify_BuildingSpawned(this);
		if (!this.CanBeSeenOver())
		{
			base.Map.exitMapGrid.Notify_LOSBlockerSpawned();
		}
		SmoothSurfaceDesignatorUtility.Notify_BuildingSpawned(this);
		map.events.Notify_BuildingSpawned(this);
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		Map map = base.Map;
		List<Thing> attachedBuildings = GenConstruct.GetAttachedBuildings(this);
		base.DeSpawn(mode);
		if ((!def.IsFrame || mode != 0) && (!def.building.isNaturalRock || mode != DestroyMode.WillReplace) && mode != DestroyMode.WillReplace)
		{
			foreach (Thing item in attachedBuildings)
			{
				if (item.def.Minifiable)
				{
					GenSpawn.Spawn(item.MakeMinified(), item.Position, map);
				}
				else
				{
					item.Destroy(mode);
				}
			}
		}
		if (def.IsEdifice())
		{
			map.edificeGrid.DeRegister(this);
			if (def.Fillage == FillCategory.Full)
			{
				map.terrainGrid.Drawer.SetDirty();
				if (ModsConfig.BiotechActive)
				{
					map.pollutionGrid.Drawer.SetDirty();
				}
			}
			if (def.AffectsFertility)
			{
				map.fertilityGrid.Drawer.SetDirty();
			}
		}
		if (mode != DestroyMode.WillReplace)
		{
			if (def.MakeFog)
			{
				map.fogGrid.Notify_FogBlockerRemoved(this);
			}
			if (def.holdsRoof)
			{
				RoofCollapseCellsFinder.Notify_RoofHolderDespawned(this, map);
			}
			if (def.IsSmoothable)
			{
				SmoothSurfaceDesignatorUtility.Notify_BuildingDespawned(this, map);
			}
		}
		CellRect cellRect = this.OccupiedRect();
		for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
		{
			for (int j = cellRect.minX; j <= cellRect.maxX; j++)
			{
				IntVec3 intVec = new IntVec3(j, 0, i);
				ulong num = MapMeshFlagDefOf.Buildings;
				if (def.coversFloor)
				{
					num |= (ulong)MapMeshFlagDefOf.Terrain;
				}
				if (def.Fillage == FillCategory.Full)
				{
					num |= (ulong)MapMeshFlagDefOf.Roofs;
					num |= (ulong)MapMeshFlagDefOf.Snow;
					if (ModsConfig.OdysseyActive)
					{
						num |= (ulong)MapMeshFlagDefOf.Sand;
					}
				}
				map.mapDrawer.MapMeshDirty(intVec, num);
				if (def.blockLight)
				{
					map.glowGrid.LightBlockerRemoved(intVec);
				}
			}
		}
		map.listerBuildings.Remove(this);
		map.listerBuildingsRepairable.Notify_BuildingDeSpawned(this);
		map.listerArtificialBuildingsForMeditation.Notify_BuildingDeSpawned(this);
		map.listerBuldingOfDefInProximity.Notify_BuildingDeSpawned(this);
		map.listerBuildingWithTagInProximity.Notify_BuildingDeSpawned(this);
		if (def.building.leaveTerrain != null && Current.ProgramState == ProgramState.Playing && canChangeTerrainOnDestroyed)
		{
			foreach (IntVec3 item2 in this.OccupiedRect())
			{
				map.terrainGrid.SetTerrain(item2, def.building.leaveTerrain);
			}
		}
		map.designationManager.Notify_BuildingDespawned(this);
		if (!this.CanBeSeenOver())
		{
			map.exitMapGrid.Notify_LOSBlockerDespawned();
		}
		if (def.building.hasFuelingPort)
		{
			CompLaunchable compLaunchable = FuelingPortUtility.LaunchableAt(FuelingPortUtility.GetFuelingPortCell(base.Position, base.Rotation), map);
			if (compLaunchable != null && !base.BeingTransportedOnGravship)
			{
				compLaunchable.Notify_FuelingPortSourceDeSpawned();
			}
		}
		map.events.Notify_BuildingDespawned(this);
		if (MaxItemsInCell < 2)
		{
			return;
		}
		foreach (IntVec3 item3 in this.OccupiedRect())
		{
			int itemCount = item3.GetItemCount(map);
			if (itemCount <= 1 || itemCount <= item3.GetMaxItemsAllowedInCell(map))
			{
				continue;
			}
			for (int k = 0; k < itemCount - 1; k++)
			{
				Thing firstItem = item3.GetFirstItem(map);
				if (firstItem == null)
				{
					break;
				}
				firstItem.DeSpawn();
				GenPlace.TryPlaceThing(firstItem, item3, map, ThingPlaceMode.Near, null, null, null);
				if (item3.GetItemCount(map) <= item3.GetMaxItemsAllowedInCell(map))
				{
					break;
				}
			}
		}
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		bool spawned = base.Spawned;
		Map map = base.Map;
		SmoothableWallUtility.Notify_BuildingDestroying(this, mode);
		this.GetLord()?.Notify_BuildingLost(this, null);
		base.Destroy(mode);
		InstallBlueprintUtility.CancelBlueprintsFor(this);
		if (spawned)
		{
			switch (mode)
			{
			case DestroyMode.Deconstruct:
				SoundDefOf.Building_Deconstructed.PlayOneShot(new TargetInfo(base.Position, map));
				break;
			case DestroyMode.KillFinalize:
				DoDestroyEffects(map);
				break;
			}
		}
		if (spawned && ThingUtility.CheckAutoRebuildOnDestroyed(this, mode, map, def) is Blueprint_Storage blueprint_Storage && this is Building_Storage building_Storage)
		{
			blueprint_Storage.SetStorageGroup(building_Storage.storageGroup);
			blueprint_Storage.settings = new StorageSettings();
			blueprint_Storage.settings.CopyFrom(building_Storage.settings);
		}
	}

	public override void SetFaction(Faction newFaction, Pawn recruiter = null)
	{
		if (base.Spawned)
		{
			base.Map.listerBuildingsRepairable.Notify_BuildingDeSpawned(this);
			base.Map.listerBuildingWithTagInProximity.Notify_BuildingDeSpawned(this);
			base.Map.listerBuildings.Remove(this);
		}
		base.SetFaction(newFaction, recruiter);
		if (base.Spawned)
		{
			base.Map.listerBuildingsRepairable.Notify_BuildingSpawned(this);
			base.Map.listerArtificialBuildingsForMeditation.Notify_BuildingSpawned(this);
			base.Map.listerBuildingWithTagInProximity.Notify_BuildingSpawned(this);
			base.Map.listerBuildings.Add(this);
			base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.PowerGrid, regenAdjacentCells: true, regenAdjacentSections: false);
			if (newFaction == Faction.OfPlayer)
			{
				AutoHomeAreaMaker.Notify_BuildingClaimed(this);
			}
		}
	}

	public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
	{
		if (def.building != null && !def.building.canBeDamagedByAttacks)
		{
			absorbed = true;
			return;
		}
		if (base.Faction != null && base.Spawned && base.Faction != Faction.OfPlayer)
		{
			for (int i = 0; i < base.Map.lordManager.lords.Count; i++)
			{
				Lord lord = base.Map.lordManager.lords[i];
				if (lord.faction == base.Faction)
				{
					lord.Notify_BuildingDamaged(this, dinfo);
				}
			}
		}
		base.PreApplyDamage(ref dinfo, out absorbed);
		if (!absorbed && base.Faction != null)
		{
			base.Faction.Notify_BuildingTookDamage(this, dinfo);
		}
		if (!absorbed)
		{
			GetComp<CompStunnable>()?.ApplyDamage(dinfo);
		}
	}

	public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
	{
		base.PostApplyDamage(dinfo, totalDamageDealt);
		if (base.Spawned)
		{
			base.Map.listerBuildingsRepairable.Notify_BuildingTookDamage(this);
		}
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(this);
		if (blueprint_Install != null)
		{
			GenDraw.DrawLineBetween(this.TrueCenter(), blueprint_Install.TrueCenter());
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__33))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__33(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__34))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__34(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override AcceptanceReport ClaimableBy(Faction by)
	{
		if (!def.Claimable)
		{
			return false;
		}
		if (base.Faction == by)
		{
			return false;
		}
		for (int i = 0; i < base.AllComps.Count; i++)
		{
			if (base.AllComps[i].CompPreventClaimingBy(by))
			{
				return false;
			}
		}
		if (FactionPreventsClaimingOrAdopting(base.Faction ?? base.Map?.ParentFaction, forClaim: true, out var reason))
		{
			return reason;
		}
		return true;
	}

	public virtual AcceptanceReport DeconstructibleBy(Faction faction)
	{
		for (int i = 0; i < base.AllComps.Count; i++)
		{
			if (base.AllComps[i].CompForceDeconstructable())
			{
				return true;
			}
		}
		if (!def.building.IsDeconstructible)
		{
			return false;
		}
		if (DebugSettings.godMode)
		{
			return true;
		}
		if (base.Faction == faction)
		{
			return true;
		}
		if (def.building.alwaysDeconstructible)
		{
			return true;
		}
		return ClaimableBy(faction);
	}

	public virtual ushort PathWalkCostFor(Pawn p)
	{
		return 0;
	}

	public virtual bool IsDangerousFor(Pawn p)
	{
		return false;
	}

	public virtual bool IsWorking()
	{
		return true;
	}

	private void DoDestroyEffects(Map map)
	{
		if (def.building.destroyEffecter != null && !base.Position.Fogged(map))
		{
			Effecter effecter = def.building.destroyEffecter.Spawn(base.Position, map);
			effecter.Trigger(new TargetInfo(base.Position, map), TargetInfo.Invalid);
			effecter.Cleanup();
		}
		else
		{
			if (!def.IsEdifice())
			{
				return;
			}
			GetDestroySound()?.PlayOneShot(new TargetInfo(base.Position, map));
			foreach (IntVec3 item in this.OccupiedRect())
			{
				int num = (def.building.isNaturalRock ? 1 : Rand.RangeInclusive(3, 5));
				for (int i = 0; i < num; i++)
				{
					FleckMaker.ThrowDustPuffThick(item.ToVector3Shifted(), map, Rand.Range(1.5f, 2f), Color.white);
				}
			}
			if (Find.CurrentMap == map)
			{
				float num2 = def.building.destroyShakeAmount;
				if (num2 < 0f)
				{
					num2 = ShakeAmountPerAreaCurve.Evaluate(def.Size.Area);
				}
				CompLifespan compLifespan = this.TryGetComp<CompLifespan>();
				if (compLifespan == null || compLifespan.age < compLifespan.Props.lifespanTicks)
				{
					Find.CameraDriver.shaker.DoShake(num2);
				}
			}
		}
	}

	private SoundDef GetDestroySound()
	{
		if (!def.building.destroySound.NullOrUndefined())
		{
			return def.building.destroySound;
		}
		StuffCategoryDef stuffCategoryDef;
		if (def.MadeFromStuff && base.Stuff != null && !base.Stuff.stuffProps.categories.NullOrEmpty())
		{
			stuffCategoryDef = base.Stuff.stuffProps.categories[0];
		}
		else
		{
			if (def.CostList.NullOrEmpty() || !def.CostList[0].thingDef.IsStuff || def.CostList[0].thingDef.stuffProps.categories.NullOrEmpty())
			{
				return null;
			}
			stuffCategoryDef = def.CostList[0].thingDef.stuffProps.categories[0];
		}
		switch (def.building.buildingSizeCategory)
		{
		case BuildingSizeCategory.Small:
			if (!stuffCategoryDef.destroySoundSmall.NullOrUndefined())
			{
				return stuffCategoryDef.destroySoundSmall;
			}
			break;
		case BuildingSizeCategory.Medium:
			if (!stuffCategoryDef.destroySoundMedium.NullOrUndefined())
			{
				return stuffCategoryDef.destroySoundMedium;
			}
			break;
		case BuildingSizeCategory.Large:
			if (!stuffCategoryDef.destroySoundLarge.NullOrUndefined())
			{
				return stuffCategoryDef.destroySoundLarge;
			}
			break;
		case BuildingSizeCategory.None:
		{
			int area = def.Size.Area;
			if (area <= 1 && !stuffCategoryDef.destroySoundSmall.NullOrUndefined())
			{
				return stuffCategoryDef.destroySoundSmall;
			}
			if (area <= 4 && !stuffCategoryDef.destroySoundMedium.NullOrUndefined())
			{
				return stuffCategoryDef.destroySoundMedium;
			}
			if (!stuffCategoryDef.destroySoundLarge.NullOrUndefined())
			{
				return stuffCategoryDef.destroySoundLarge;
			}
			break;
		}
		}
		return null;
	}

	public override void PostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
	{
		base.PostGeneratedForTrader(trader, forTile, forFaction);
		if (def.building.paintable && Rand.Value < 0.1f)
		{
			ChangePaint(DefDatabase<ColorDef>.AllDefs.Where((ColorDef x) => x.colorType == ColorType.Structure).RandomElement());
		}
		else if (def.colorGeneratorInTraderStock != null)
		{
			this.SetColor(def.colorGeneratorInTraderStock.NewRandomizedColor());
		}
	}

	public override string GetInspectStringLowPriority()
	{
		string text = base.GetInspectStringLowPriority();
		if (!DeconstructibleBy(Faction.OfPlayer) && (def.IsNonDeconstructibleAttackableBuilding || def.building.quickTargetable) && def.building.displayAttackToDestroyOnInspectPane)
		{
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "AttackToDestroy".Translate();
		}
		return text;
	}

	public void ChangePaint(ColorDef colorDef)
	{
		paintColorDef = colorDef;
		Notify_ColorChanged();
		bool flag = base.Spawned;
		if (flag)
		{
			DrawerType drawerType = def.drawerType;
			bool flag2 = drawerType - 2 <= DrawerType.RealtimeOnly;
			flag = flag2;
		}
		if (flag)
		{
			base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Buildings);
		}
	}

	public static Gizmo SelectContainedItemGizmo(Thing container, Thing item)
	{
		if (!container.Faction.IsPlayerSafe())
		{
			return null;
		}
		return ContainingSelectionUtility.SelectCarriedThingGizmo(container, item);
	}

	public virtual int HaulToContainerDuration(Thing thing)
	{
		return def.building.haulToContainerDuration;
	}
}
