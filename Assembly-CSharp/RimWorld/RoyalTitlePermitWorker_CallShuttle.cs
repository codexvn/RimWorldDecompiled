using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public class RoyalTitlePermitWorker_CallShuttle : RoyalTitlePermitWorker_Targeted
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public RoyalTitlePermitWorker_CallShuttle _003C_003E4__this;

		public Pawn pawn;

		public Faction faction;

		public bool free;

		internal void _003CGetRoyalAidOptions_003Eb__0()
		{
			_003C_003E4__this.BeginCallShuttle(pawn, pawn.MapHeld, faction, free);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public RoyalTitlePermitWorker_CallShuttle _003C_003E4__this;

		public Pawn pawn;

		public Faction faction;

		internal void _003CGetCaravanGizmos_003Eb__0()
		{
			_003C_003E4__this.CallShuttleToCaravan(pawn, faction, _003C_003E4__this.free);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetCaravanGizmos_003Ed__7 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RoyalTitlePermitWorker_CallShuttle _003C_003E4__this;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private Faction faction;

		public Faction _003C_003E3__faction;

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
		public _003CGetCaravanGizmos_003Ed__7(int _003C_003E1__state)
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
			RoyalTitlePermitWorker_CallShuttle royalTitlePermitWorker_CallShuttle = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass7_0 CS_0024_003C_003E8__locals12 = new _003C_003Ec__DisplayClass7_0
				{
					_003C_003E4__this = _003C_003E4__this,
					pawn = pawn,
					faction = faction
				};
				if (!royalTitlePermitWorker_CallShuttle.FillCaravanAidOption(CS_0024_003C_003E8__locals12.pawn, CS_0024_003C_003E8__locals12.faction, out var description, out royalTitlePermitWorker_CallShuttle.free, out var disableNotEnoughFavor))
				{
					return false;
				}
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = royalTitlePermitWorker_CallShuttle.def.LabelCap + " (" + CS_0024_003C_003E8__locals12.pawn.LabelShort + ")",
					defaultDesc = description,
					icon = CommandTex,
					action = delegate
					{
						CS_0024_003C_003E8__locals12._003C_003E4__this.CallShuttleToCaravan(CS_0024_003C_003E8__locals12.pawn, CS_0024_003C_003E8__locals12.faction, CS_0024_003C_003E8__locals12._003C_003E4__this.free);
					}
				};
				if (CS_0024_003C_003E8__locals12.pawn.MapHeld != null && CS_0024_003C_003E8__locals12.pawn.MapHeld.generatorDef.isUnderground)
				{
					command_Action.Disable("CommandCallRoyalAidMapUnreachable".Translate(CS_0024_003C_003E8__locals12.faction.Named("FACTION")));
				}
				if (CS_0024_003C_003E8__locals12.faction.HostileTo(Faction.OfPlayer))
				{
					command_Action.Disable("CommandCallRoyalAidFactionHostile".Translate(CS_0024_003C_003E8__locals12.faction.Named("FACTION")));
				}
				if (disableNotEnoughFavor)
				{
					command_Action.Disable("CommandCallRoyalAidNotEnoughFavor".Translate());
				}
				_003C_003E2__current = command_Action;
				_003C_003E1__state = 1;
				return true;
			}
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
			_003CGetCaravanGizmos_003Ed__7 _003CGetCaravanGizmos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetCaravanGizmos_003Ed__ = this;
			}
			else
			{
				_003CGetCaravanGizmos_003Ed__ = new _003CGetCaravanGizmos_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetCaravanGizmos_003Ed__.pawn = _003C_003E3__pawn;
			_003CGetCaravanGizmos_003Ed__.faction = _003C_003E3__faction;
			return _003CGetCaravanGizmos_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetRoyalAidOptions_003Ed__6 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RoyalTitlePermitWorker_CallShuttle _003C_003E4__this;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private Faction faction;

		public Faction _003C_003E3__faction;

		private Map map;

		public Map _003C_003E3__map;

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
		public _003CGetRoyalAidOptions_003Ed__6(int _003C_003E1__state)
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
			RoyalTitlePermitWorker_CallShuttle royalTitlePermitWorker_CallShuttle = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass6_0 CS_0024_003C_003E8__locals13 = new _003C_003Ec__DisplayClass6_0
				{
					_003C_003E4__this = _003C_003E4__this,
					pawn = pawn,
					faction = faction
				};
				MapGeneratorDef generatorDef = map.generatorDef;
				if (generatorDef != null && generatorDef.isUnderground)
				{
					_003C_003E2__current = new FloatMenuOption(royalTitlePermitWorker_CallShuttle.def.LabelCap + ": " + "CommandCallRoyalAidMapUnreachable".Translate(CS_0024_003C_003E8__locals13.faction.Named("FACTION")), null);
					_003C_003E1__state = 1;
					return true;
				}
				if (CS_0024_003C_003E8__locals13.faction.HostileTo(Faction.OfPlayer))
				{
					_003C_003E2__current = new FloatMenuOption(royalTitlePermitWorker_CallShuttle.def.LabelCap + ": " + "CommandCallRoyalAidFactionHostile".Translate(CS_0024_003C_003E8__locals13.faction.Named("FACTION")), null);
					_003C_003E1__state = 2;
					return true;
				}
				string description = royalTitlePermitWorker_CallShuttle.def.LabelCap + ": ";
				Action action = null;
				if (royalTitlePermitWorker_CallShuttle.FillAidOption(CS_0024_003C_003E8__locals13.pawn, CS_0024_003C_003E8__locals13.faction, ref description, out CS_0024_003C_003E8__locals13.free))
				{
					action = delegate
					{
						CS_0024_003C_003E8__locals13._003C_003E4__this.BeginCallShuttle(CS_0024_003C_003E8__locals13.pawn, CS_0024_003C_003E8__locals13.pawn.MapHeld, CS_0024_003C_003E8__locals13.faction, CS_0024_003C_003E8__locals13.free);
					};
				}
				_003C_003E2__current = new FloatMenuOption(description, action, CS_0024_003C_003E8__locals13.faction.def.FactionIcon, CS_0024_003C_003E8__locals13.faction.Color);
				_003C_003E1__state = 3;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			case 2:
				_003C_003E1__state = -1;
				return false;
			case 3:
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetRoyalAidOptions_003Ed__6 _003CGetRoyalAidOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetRoyalAidOptions_003Ed__ = this;
			}
			else
			{
				_003CGetRoyalAidOptions_003Ed__ = new _003CGetRoyalAidOptions_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetRoyalAidOptions_003Ed__.map = _003C_003E3__map;
			_003CGetRoyalAidOptions_003Ed__.pawn = _003C_003E3__pawn;
			_003CGetRoyalAidOptions_003Ed__.faction = _003C_003E3__faction;
			return _003CGetRoyalAidOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private Faction calledFaction;

	private static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/CallShuttle");

	public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
	{
		if (!CanHitTarget(target))
		{
			if (target.IsValid && showMessages)
			{
				Messages.Message(def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput);
			}
			return false;
		}
		AcceptanceReport acceptanceReport = ShuttleCanLandHere(target, map, null, null);
		if (!acceptanceReport.Accepted)
		{
			Messages.Message(acceptanceReport.Reason, new LookTargets(target.Cell, map), MessageTypeDefOf.RejectInput, historical: false);
		}
		return acceptanceReport.Accepted;
	}

	public override void DrawHighlight(LocalTargetInfo target)
	{
		GenDraw.DrawRadiusRing(caller.Position, base.RangeClamped, Color.white);
		DrawShuttleGhost(target, map, ThingDefOf.Shuttle, ThingDefOf.Shuttle.defaultPlacingRot);
	}

	public override void OrderForceTarget(LocalTargetInfo target)
	{
		CallShuttle(target.Cell);
	}

	public override void OnGUI(LocalTargetInfo target)
	{
		if (!target.IsValid || !ShuttleCanLandHere(target, map, null, null).Accepted)
		{
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}
	}

	[IteratorStateMachine(typeof(_003CGetRoyalAidOptions_003Ed__6))]
	public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetRoyalAidOptions_003Ed__6(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__map = map,
			_003C_003E3__pawn = pawn,
			_003C_003E3__faction = faction
		};
	}

	[IteratorStateMachine(typeof(_003CGetCaravanGizmos_003Ed__7))]
	public override IEnumerable<Gizmo> GetCaravanGizmos(Pawn pawn, Faction faction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetCaravanGizmos_003Ed__7(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pawn = pawn,
			_003C_003E3__faction = faction
		};
	}

	private void BeginCallShuttle(Pawn caller, Map map, Faction faction, bool free)
	{
		targetingParameters = new TargetingParameters();
		targetingParameters.canTargetLocations = true;
		targetingParameters.canTargetSelf = false;
		targetingParameters.canTargetPawns = false;
		targetingParameters.canTargetFires = false;
		targetingParameters.canTargetBuildings = true;
		targetingParameters.canTargetItems = true;
		base.caller = caller;
		base.map = map;
		calledFaction = faction;
		base.free = free;
		float rangeActual = base.RangeClamped;
		targetingParameters.validator = (TargetInfo target) => (!(rangeActual > 0f) || !(target.Cell.DistanceTo(caller.Position) > rangeActual)) ? true : false;
		Find.Targeter.BeginTargeting(this);
	}

	private void CallShuttle(IntVec3 landingCell)
	{
		if (caller.Spawned)
		{
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Shuttle);
			CompShuttle compShuttle = thing.TryGetComp<CompShuttle>();
			compShuttle.permitShuttle = true;
			compShuttle.acceptChildren = true;
			TransportShip transportShip = TransportShipMaker.MakeTransportShip(TransportShipDefOf.Ship_Shuttle, null, thing);
			transportShip.ArriveAt(landingCell, map.Parent);
			transportShip.AddJobs(ShipJobDefOf.WaitForever, ShipJobDefOf.Unload_Destination, ShipJobDefOf.FlyAway);
			caller.royalty.GetPermit(def, calledFaction).Notify_Used();
			if (!free)
			{
				caller.royalty.TryRemoveFavor(calledFaction, def.royalAid.favorCost);
			}
		}
	}

	private void CallShuttleToCaravan(Pawn caller, Faction faction, bool free)
	{
		Caravan caravan = caller.GetCaravan();
		int maxLaunchDistance = TransportShipDefOf.Ship_Shuttle.maxLaunchDistance;
		CameraJumper.TryJump(CameraJumper.GetWorldTarget(caravan));
		Find.WorldSelector.ClearSelection();
		PlanetTile caravanTile = caravan.Tile;
		Find.WorldTargeter.BeginTargeting(ChoseWorldTarget, canTargetTiles: true, CompLaunchable.TargeterMouseAttachment, closeWorldTabWhenFinished: false, delegate
		{
			if (maxLaunchDistance > 0)
			{
				PlanetTile center = caravanTile;
				if (caravanTile.Layer != Find.WorldSelector.SelectedLayer)
				{
					center = Find.WorldSelector.SelectedLayer.GetClosestTile_NewTemp(caravanTile);
				}
				GenDraw.DrawWorldRadiusRing(center, maxLaunchDistance);
			}
		}, (GlobalTargetInfo target) => CompLaunchable.TargetingLabelGetter(target, caravanTile, maxLaunchDistance, Gen.YieldSingle(caravan), Launch, null, null), null, null);
		bool ChoseWorldTarget(GlobalTargetInfo target)
		{
			return CompLaunchable.ChoseWorldTarget(target, caravan.Tile, Gen.YieldSingle(caravan), maxLaunchDistance, Launch, null, null);
		}
		void Launch(PlanetTile tile, TransportersArrivalAction arrivalAction)
		{
			ActiveTransporterInfo activeTransporterInfo = new ActiveTransporterInfo();
			activeTransporterInfo.innerContainer.TryAddRangeOrTransfer(CaravanInventoryUtility.AllInventoryItems(caravan));
			activeTransporterInfo.innerContainer.TryAddRangeOrTransfer(caravan.GetDirectlyHeldThings());
			caravan.Destroy();
			TravellingTransporters travellingTransporters = (TravellingTransporters)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.TravelingShuttle);
			travellingTransporters.Tile = caravan.Tile;
			travellingTransporters.SetFaction(Faction.OfPlayer);
			travellingTransporters.destinationTile = tile;
			travellingTransporters.arrivalAction = arrivalAction;
			travellingTransporters.AddTransporter(activeTransporterInfo, justLeftTheMap: true);
			Find.WorldObjects.Add(travellingTransporters);
			Find.WorldTargeter.StopTargeting();
			caller.royalty.GetPermit(def, faction).Notify_Used();
			if (!free)
			{
				caller.royalty.TryRemoveFavor(faction, def.royalAid.favorCost);
			}
		}
	}

	public static void DrawShuttleGhost(LocalTargetInfo target, Map map, ThingDef shuttleDef, Rot4 rot)
	{
		Color ghostCol = (ShuttleCanLandHere(target, map, shuttleDef, rot).Accepted ? Designator_Place.CanPlaceColor : Designator_Place.CannotPlaceColor);
		GhostDrawer.DrawGhostThing(target.Cell, rot, shuttleDef, shuttleDef.graphic, ghostCol, AltitudeLayer.Blueprint);
		Vector3 position = ThingUtility.InteractionCellWhenAt(shuttleDef, target.Cell, rot, map).ToVector3ShiftedWithAltitude(AltitudeLayer.Blueprint);
		Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.InteractionCellMaterial, 0);
	}

	public static AcceptanceReport ShuttleCanLandHere(LocalTargetInfo target, Map map, ThingDef shuttleDef = null, Rot4? rot = null)
	{
		TaggedString taggedString = "ShuttleCannotLand".Translate() + ": ";
		if (!target.IsValid)
		{
			return new AcceptanceReport(taggedString + "MessageTransportPodsDestinationIsInvalid".Translate().CapitalizeFirst());
		}
		if (shuttleDef == null)
		{
			shuttleDef = ThingDefOf.Shuttle;
		}
		Rot4 valueOrDefault = rot.GetValueOrDefault();
		if (!rot.HasValue)
		{
			valueOrDefault = shuttleDef.defaultPlacingRot;
			rot = valueOrDefault;
		}
		foreach (IntVec3 item in GenAdj.OccupiedRect(target.Cell, rot.Value, shuttleDef.size))
		{
			string reportFromCell = GetReportFromCell(item, map, interactionSpot: false, shuttleDef);
			if (reportFromCell != null)
			{
				return new AcceptanceReport(taggedString + reportFromCell);
			}
		}
		string reportFromCell2 = GetReportFromCell(ThingUtility.InteractionCellWhenAt(shuttleDef, target.Cell, rot.Value, map), map, interactionSpot: true, shuttleDef);
		if (reportFromCell2 != null)
		{
			return new AcceptanceReport(taggedString + reportFromCell2);
		}
		return AcceptanceReport.WasAccepted;
	}

	private static string GetReportFromCell(IntVec3 cell, Map map, bool interactionSpot, ThingDef shuttleDef)
	{
		if (!cell.InBounds(map))
		{
			return "OutOfBounds".Translate().CapitalizeFirst();
		}
		if (cell.Fogged(map))
		{
			return "ShuttleCannotLand_Fogged".Translate().CapitalizeFirst();
		}
		if (!cell.Walkable(map))
		{
			return "ShuttleCannotLand_Unwalkable".Translate().CapitalizeFirst();
		}
		if (!cell.GetAffordances(map).Contains(ThingDefOf.Shuttle.terrainAffordanceNeeded))
		{
			return "ShuttleCannotLand_Unaffordable".Translate(shuttleDef.label).CapitalizeFirst();
		}
		RoofDef roof = cell.GetRoof(map);
		if (roof != null && (roof.isNatural || roof.isThickRoof))
		{
			return "MessageTransportPodsDestinationIsInvalid".Translate().CapitalizeFirst();
		}
		List<Thing> thingList = cell.GetThingList(map);
		for (int i = 0; i < thingList.Count; i++)
		{
			Thing thing = thingList[i];
			if (thing is IActiveTransporter || thing is Skyfaller || (thing.def.category == ThingCategory.Building && !thing.def.building.isPowerConduit))
			{
				return "BlockedBy".Translate(thing).CapitalizeFirst();
			}
			PlantProperties plant = thing.def.plant;
			if (plant != null && plant.IsTree)
			{
				return "BlockedBy".Translate(thing).CapitalizeFirst();
			}
		}
		return null;
	}
}
