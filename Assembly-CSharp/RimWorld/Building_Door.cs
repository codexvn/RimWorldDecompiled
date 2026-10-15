using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public class Building_Door : Building
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__81 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_Door _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__81(int _003C_003E1__state)
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
				Building_Door CS_0024_003C_003E8__locals7 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)CS_0024_003C_003E8__locals7).GetGizmos().GetEnumerator();
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
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if ((CS_0024_003C_003E8__locals7.Faction == Faction.OfPlayer || CS_0024_003C_003E8__locals7.Faction == null) && !CS_0024_003C_003E8__locals7.AlwaysOpen)
					{
						_003C_003E2__current = new Command_Toggle
						{
							defaultLabel = "CommandToggleDoorHoldOpen".Translate(),
							defaultDesc = "CommandToggleDoorHoldOpenDesc".Translate(),
							hotKey = KeyBindingDefOf.Misc3,
							icon = TexCommand.HoldOpen,
							isActive = () => CS_0024_003C_003E8__locals7.holdOpenInt,
							toggleAction = delegate
							{
								CS_0024_003C_003E8__locals7.holdOpenInt = !CS_0024_003C_003E8__locals7.holdOpenInt;
							}
						};
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__81 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__81(0)
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

	public CompPowerTrader powerComp;

	protected bool openInt;

	protected bool holdOpenInt;

	private int lastFriendlyTouchTick = -9999;

	protected int ticksUntilClose;

	private Pawn approachingPawn;

	private int ticksOpen;

	protected int ticksSinceOpen;

	private bool freePassageWhenClearedReachabilityCache;

	private bool willCloseSoonForJobs;

	private const float OpenTicks = 45f;

	private const int CloseDelayTicks = 110;

	private const int ApproachCloseDelayTicks = 300;

	private const int MaxTicksSinceFriendlyTouchToAutoClose = 120;

	private const float PowerOffDoorOpenSpeedFactor = 0.25f;

	private const float VisualDoorOffsetStart = 0f;

	private const float VisualDoorOffsetEnd = 0.45f;

	private const float NotifyFogGridDoorOpenPct = 0.4f;

	private const int TicksOpenToBreach = 600;

	private int CloseDelayAdjusted => Mathf.FloorToInt(110f * (DoorPowerOn ? def.building.poweredDoorCloseSpeedFactor : def.building.unpoweredDoorCloseSpeedFactor));

	private int WillCloseSoonThreshold => CloseDelayAdjusted + 1;

	public bool Open => openInt;

	public bool HoldOpen => holdOpenInt;

	protected virtual bool CheckFaction => true;

	protected virtual bool CanDrawMovers => true;

	public override bool ExchangeVacuum
	{
		get
		{
			if (!base.ExchangeVacuum)
			{
				return Open;
			}
			return true;
		}
	}

	protected virtual float TempEqualizeRate => def.building.doorTempEqualizeRate;

	public virtual bool FreePassage
	{
		get
		{
			if (!openInt)
			{
				return false;
			}
			if (!holdOpenInt)
			{
				return !WillCloseSoon;
			}
			return true;
		}
	}

	public int TicksTillFullyOpened
	{
		get
		{
			int num = TicksToOpenNow - ticksSinceOpen;
			if (num < 0)
			{
				num = 0;
			}
			return num;
		}
	}

	public bool WillCloseSoon
	{
		get
		{
			if (!UnityData.IsInMainThread)
			{
				return willCloseSoonForJobs;
			}
			if (!base.Spawned)
			{
				return true;
			}
			if (!openInt)
			{
				return true;
			}
			if (holdOpenInt)
			{
				return false;
			}
			if (ticksUntilClose > 0 && ticksUntilClose <= WillCloseSoonThreshold && !BlockedOpenMomentary)
			{
				return true;
			}
			if (CanTryCloseAutomatically && !BlockedOpenMomentary)
			{
				return true;
			}
			foreach (IntVec3 item in this.OccupiedRect())
			{
				for (int i = 0; i < 5; i++)
				{
					IntVec3 c = item + GenAdj.CardinalDirectionsAndInside[i];
					if (!c.InBounds(base.Map))
					{
						continue;
					}
					List<Thing> thingList = c.GetThingList(base.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j] is Pawn pawn && !pawn.HostileTo(this) && !pawn.Downed && PawnCanOpen(pawn) && (pawn.Position == item || (pawn.pather.Moving && pawn.pather.nextCell == item)))
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}

	public bool ContainmentBreached
	{
		get
		{
			if (openInt)
			{
				return ticksOpen >= 600;
			}
			return false;
		}
	}

	public bool BlockedOpenMomentary
	{
		get
		{
			if (StuckOpen)
			{
				return true;
			}
			foreach (IntVec3 item in this.OccupiedRect())
			{
				List<Thing> thingList = item.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Pawn || thing is Corpse)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	protected virtual bool AlwaysOpen => false;

	protected bool StuckOpen
	{
		get
		{
			if (AlwaysOpen)
			{
				return true;
			}
			if (!base.Spawned || def.size == IntVec2.One)
			{
				return false;
			}
			foreach (IntVec3 item in DoorUtility.WallRequirementCells(def, base.Position, base.Rotation))
			{
				if (!DoorUtility.EncapsulatingWallAt(item, base.Map))
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool DoorPowerOn
	{
		get
		{
			if (powerComp != null)
			{
				return powerComp.PowerOn;
			}
			return false;
		}
	}

	public bool SlowsPawns
	{
		get
		{
			if (DoorPowerOn)
			{
				return TicksToOpenNow > 20;
			}
			return true;
		}
	}

	public int TicksToOpenNow
	{
		get
		{
			float num = 45f / this.GetStatValue(StatDefOf.DoorOpenSpeed);
			num = ((!DoorPowerOn) ? (num * def.building.unpoweredDoorOpenSpeedFactor) : (num * (0.25f * def.building.poweredDoorOpenSpeedFactor)));
			return Mathf.RoundToInt(num);
		}
	}

	private bool CanTryCloseAutomatically
	{
		get
		{
			if (!AlwaysOpen && FriendlyTouchedRecently)
			{
				return !HoldOpen;
			}
			return false;
		}
	}

	private bool FriendlyTouchedRecently => Find.TickManager.TicksGame < lastFriendlyTouchTick + 120;

	public override bool FireBulwark
	{
		get
		{
			if (!Open)
			{
				return base.FireBulwark;
			}
			return false;
		}
	}

	protected virtual float OpenPct => Mathf.Clamp01((float)ticksSinceOpen / (float)TicksToOpenNow);

	public void SnapshotWillCloseSoonForJobs()
	{
		willCloseSoonForJobs = WillCloseSoon;
	}

	public override void PostMake()
	{
		base.PostMake();
		powerComp = GetComp<CompPowerTrader>();
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		powerComp = GetComp<CompPowerTrader>();
		ClearReachabilityCache(map);
		if (BlockedOpenMomentary)
		{
			DoorOpen();
		}
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		Map map = base.Map;
		base.DeSpawn(mode);
		ClearReachabilityCache(map);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref openInt, "open", defaultValue: false);
		Scribe_Values.Look(ref holdOpenInt, "holdOpen", defaultValue: false);
		Scribe_Values.Look(ref lastFriendlyTouchTick, "lastFriendlyTouchTick", 0);
		Scribe_Values.Look(ref ticksUntilClose, "ticksUntilClose", 0);
		Scribe_References.Look(ref approachingPawn, "approachingPawn");
		Scribe_Values.Look(ref ticksOpen, "ticksOpen", 0);
		if (Scribe.mode == LoadSaveMode.LoadingVars && openInt)
		{
			ticksSinceOpen = TicksToOpenNow;
		}
	}

	public override void SetFaction(Faction newFaction, Pawn recruiter = null)
	{
		base.SetFaction(newFaction, recruiter);
		if (base.Spawned)
		{
			ClearReachabilityCache(base.Map);
		}
	}

	protected override void Tick()
	{
		base.Tick();
		if (FreePassage != freePassageWhenClearedReachabilityCache)
		{
			ClearReachabilityCache(base.Map);
		}
		if (!openInt)
		{
			if (ticksSinceOpen > 0)
			{
				ticksSinceOpen--;
			}
			ticksOpen = 0;
			if (this.IsHashIntervalTick(def.building.doorTempEqualizeIntervalClosed))
			{
				GenTemperature.EqualizeTemperaturesThroughBuilding(this, TempEqualizeRate, twoWay: false);
			}
		}
		else
		{
			if (!openInt)
			{
				return;
			}
			if (ticksSinceOpen < TicksToOpenNow)
			{
				ticksSinceOpen++;
			}
			ticksOpen++;
			foreach (IntVec3 item in this.OccupiedRect())
			{
				List<Thing> thingList = item.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (thingList[i] is Pawn p)
					{
						CheckFriendlyTouched(p);
					}
				}
			}
			if (ticksUntilClose > 0 && !AlwaysOpen)
			{
				foreach (IntVec3 item2 in this.OccupiedRect())
				{
					if (base.Map.thingGrid.CellContains(item2, ThingCategory.Pawn))
					{
						ticksUntilClose = CloseDelayAdjusted;
						break;
					}
				}
				ticksUntilClose--;
				if (ticksUntilClose <= 0 && !holdOpenInt && !DoorTryClose())
				{
					ticksUntilClose = 1;
				}
			}
			else if (CanTryCloseAutomatically)
			{
				ticksUntilClose = CloseDelayAdjusted;
			}
			if ((Find.TickManager.TicksGame + thingIDNumber.HashOffset()) % def.building.doorTempEqualizeIntervalOpen == 0)
			{
				GenTemperature.EqualizeTemperaturesThroughBuilding(this, TempEqualizeRate, twoWay: false);
			}
			if (OpenPct >= 0.4f && approachingPawn != null)
			{
				base.Map.fogGrid.Notify_PawnEnteringDoor(this, approachingPawn);
				approachingPawn = null;
			}
		}
	}

	public void CheckFriendlyTouched(Pawn p)
	{
		if (!p.HostileTo(this) && PawnCanOpen(p))
		{
			lastFriendlyTouchTick = Find.TickManager.TicksGame;
		}
	}

	public void Notify_PawnApproaching(Pawn p, float moveCost)
	{
		CheckFriendlyTouched(p);
		bool num = PawnCanOpen(p);
		if (num || Open)
		{
			approachingPawn = p;
		}
		if (num && !SlowsPawns)
		{
			DoorOpen(Mathf.Max(300, Mathf.CeilToInt(moveCost) + 1));
		}
	}

	public bool CanPhysicallyPass(Pawn p)
	{
		if (!FreePassage && !PawnCanOpen(p))
		{
			if (Open)
			{
				return p.HostileTo(this);
			}
			return false;
		}
		return true;
	}

	public virtual bool PawnCanOpen(Pawn p)
	{
		if (!p.CanOpenDoors)
		{
			return false;
		}
		if (base.Map?.Parent != null && base.Map.Parent.doorsAlwaysOpenForPlayerPawns && p.Faction == Faction.OfPlayer)
		{
			return true;
		}
		if (p.CanOpenAnyDoor)
		{
			return true;
		}
		if (p.FenceBlocked && !def.building.roamerCanOpen && (!p.roping.IsRopedByPawn || !PawnCanOpen(p.roping.RopedByPawn)))
		{
			return false;
		}
		if (base.Faction == null)
		{
			return p.RaceProps.canOpenFactionlessDoors;
		}
		if (p.guest != null && p.guest.Released)
		{
			return true;
		}
		if (CheckFaction)
		{
			return GenAI.MachinesLike(base.Faction, p);
		}
		return true;
	}

	public override bool BlocksPawn(Pawn p)
	{
		if (openInt)
		{
			return false;
		}
		return !PawnCanOpen(p);
	}

	protected virtual void DoorOpen(int ticksToClose = 110)
	{
		if (openInt)
		{
			ticksUntilClose = ticksToClose;
		}
		else
		{
			ticksUntilClose = TicksToOpenNow + ticksToClose;
		}
		if (!openInt)
		{
			openInt = true;
			CheckClearReachabilityCacheBecauseOpenedOrClosed();
			if (DoorPowerOn)
			{
				def.building.soundDoorOpenPowered.PlayOneShot(this);
			}
			else
			{
				def.building.soundDoorOpenManual.PlayOneShot(this);
			}
			base.Map.events.Notify_DoorOpened(this);
		}
	}

	protected bool DoorTryClose()
	{
		if (holdOpenInt || BlockedOpenMomentary)
		{
			return false;
		}
		openInt = false;
		CheckClearReachabilityCacheBecauseOpenedOrClosed();
		if (DoorPowerOn)
		{
			def.building.soundDoorClosePowered.PlayOneShot(this);
		}
		else
		{
			def.building.soundDoorCloseManual.PlayOneShot(this);
		}
		base.Map.events.Notify_DoorClosed(this);
		return true;
	}

	public void StartManualOpenBy(Pawn opener)
	{
		DoorOpen();
	}

	public void StartManualCloseBy(Pawn closer)
	{
		ticksUntilClose = CloseDelayAdjusted;
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		DoorPreDraw();
		if (CanDrawMovers)
		{
			float offsetDist = 0f + 0.45f * OpenPct;
			DrawMovers(drawLoc, offsetDist, Graphic, AltitudeLayer.DoorMoveable.AltitudeFor(), Vector3.one, Graphic.ShadowGraphic);
		}
		Comps_PostDraw();
	}

	protected void DrawMovers(Vector3 drawPos, float offsetDist, Graphic graphic, float altitude, Vector3 drawScaleFactor, Graphic_Shadow shadowGraphic)
	{
		for (int i = 0; i < 2; i++)
		{
			Mesh mesh;
			Vector3 vector;
			if (i == 0)
			{
				vector = new Vector3(0f, 0f, -def.size.x);
				mesh = MeshPool.plane10;
			}
			else
			{
				vector = new Vector3(0f, 0f, def.size.x);
				mesh = MeshPool.plane10Flip;
			}
			Rot4 rotation = base.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			vector = rotation.AsQuat * vector;
			Vector3 vector2 = drawPos;
			vector2.y = altitude;
			vector2 += vector * offsetDist;
			Graphics.DrawMesh(mesh, Matrix4x4.TRS(vector2, base.Rotation.AsQuat, new Vector3((float)def.size.x * drawScaleFactor.x, drawScaleFactor.y, (float)def.size.z * drawScaleFactor.z)), graphic.MatAt(base.Rotation, this), 0);
			shadowGraphic?.DrawWorker(vector2, base.Rotation, def, this, 0f);
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__81))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__81(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void ClearReachabilityCache(Map map)
	{
		map.reachability.ClearCache();
		freePassageWhenClearedReachabilityCache = FreePassage;
	}

	private void CheckClearReachabilityCacheBecauseOpenedOrClosed()
	{
		if (base.Spawned)
		{
			base.Map.reachability.ClearCacheForHostile(this);
		}
	}

	protected void DoorPreDraw()
	{
		if (def.size == IntVec2.One)
		{
			base.Rotation = DoorUtility.DoorRotationAt(base.Position, base.Map, def.building.preferConnectingToFences);
		}
	}

	public override string GetInspectString()
	{
		string text = base.GetInspectString();
		if (StuckOpen && !AlwaysOpen)
		{
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "DoorMustBeEnclosedByWalls".Translate().Colorize(ColorLibrary.RedReadable);
		}
		return text;
	}
}
