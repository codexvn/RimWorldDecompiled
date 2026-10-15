using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld.Planet;

public static class TransportersArrivalActionUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_0<T> where T : TransportersArrivalAction
	{
		public Func<FloatMenuAcceptanceReport> acceptanceReportGetter;

		public Action<Action> uiConfirmationCallback;

		public Action<PlanetTile, TransportersArrivalAction> launchAction;

		public PlanetTile destinationTile;

		public Func<T> arrivalActionGetter;

		public Action _003C_003E9__1;

		internal void _003CGetFloatMenuOptions_003Eb__0()
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = acceptanceReportGetter();
			if (floatMenuAcceptanceReport.Accepted)
			{
				if (uiConfirmationCallback == null)
				{
					launchAction(destinationTile, arrivalActionGetter());
					return;
				}
				uiConfirmationCallback(delegate
				{
					launchAction(destinationTile, arrivalActionGetter());
				});
			}
			else if (!floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
			{
				Messages.Message(floatMenuAcceptanceReport.FailMessage, new GlobalTargetInfo(destinationTile), MessageTypeDefOf.RejectInput, historical: false);
			}
		}

		internal void _003CGetFloatMenuOptions_003Eb__1()
		{
			launchAction(destinationTile, arrivalActionGetter());
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__0<T> : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable where T : TransportersArrivalAction
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Func<FloatMenuAcceptanceReport> acceptanceReportGetter;

		public Func<FloatMenuAcceptanceReport> _003C_003E3__acceptanceReportGetter;

		private Action<Action> uiConfirmationCallback;

		public Action<Action> _003C_003E3__uiConfirmationCallback;

		private Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Action<PlanetTile, TransportersArrivalAction> _003C_003E3__launchAction;

		private PlanetTile destinationTile;

		public PlanetTile _003C_003E3__destinationTile;

		private Func<T> arrivalActionGetter;

		public Func<T> _003C_003E3__arrivalActionGetter;

		private string label;

		public string _003C_003E3__label;

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
		public _003CGetFloatMenuOptions_003Ed__0(int _003C_003E1__state)
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
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass0_0<T> CS_0024_003C_003E8__locals11 = new _003C_003Ec__DisplayClass0_0<T>
				{
					acceptanceReportGetter = acceptanceReportGetter,
					uiConfirmationCallback = uiConfirmationCallback,
					launchAction = launchAction,
					destinationTile = destinationTile,
					arrivalActionGetter = arrivalActionGetter
				};
				FloatMenuAcceptanceReport floatMenuAcceptanceReport = CS_0024_003C_003E8__locals11.acceptanceReportGetter();
				if (!floatMenuAcceptanceReport.Accepted && floatMenuAcceptanceReport.FailReason.NullOrEmpty() && floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
				{
					break;
				}
				if (!floatMenuAcceptanceReport.Accepted && !floatMenuAcceptanceReport.FailReason.NullOrEmpty())
				{
					label = label + " (" + floatMenuAcceptanceReport.FailReason + ")";
				}
				_003C_003E2__current = new FloatMenuOption(label, delegate
				{
					FloatMenuAcceptanceReport floatMenuAcceptanceReport2 = CS_0024_003C_003E8__locals11.acceptanceReportGetter();
					if (floatMenuAcceptanceReport2.Accepted)
					{
						if (CS_0024_003C_003E8__locals11.uiConfirmationCallback == null)
						{
							CS_0024_003C_003E8__locals11.launchAction(CS_0024_003C_003E8__locals11.destinationTile, CS_0024_003C_003E8__locals11.arrivalActionGetter());
						}
						else
						{
							CS_0024_003C_003E8__locals11.uiConfirmationCallback(delegate
							{
								CS_0024_003C_003E8__locals11.launchAction(CS_0024_003C_003E8__locals11.destinationTile, CS_0024_003C_003E8__locals11.arrivalActionGetter());
							});
						}
					}
					else if (!floatMenuAcceptanceReport2.FailMessage.NullOrEmpty())
					{
						Messages.Message(floatMenuAcceptanceReport2.FailMessage, new GlobalTargetInfo(CS_0024_003C_003E8__locals11.destinationTile), MessageTypeDefOf.RejectInput, historical: false);
					}
				});
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__0<T> _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__0<T>(0);
			}
			_003CGetFloatMenuOptions_003Ed__.acceptanceReportGetter = _003C_003E3__acceptanceReportGetter;
			_003CGetFloatMenuOptions_003Ed__.arrivalActionGetter = _003C_003E3__arrivalActionGetter;
			_003CGetFloatMenuOptions_003Ed__.label = _003C_003E3__label;
			_003CGetFloatMenuOptions_003Ed__.launchAction = _003C_003E3__launchAction;
			_003CGetFloatMenuOptions_003Ed__.destinationTile = _003C_003E3__destinationTile;
			_003CGetFloatMenuOptions_003Ed__.uiConfirmationCallback = _003C_003E3__uiConfirmationCallback;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__0<>))]
	public static IEnumerable<FloatMenuOption> GetFloatMenuOptions<T>(Func<FloatMenuAcceptanceReport> acceptanceReportGetter, Func<T> arrivalActionGetter, string label, Action<PlanetTile, TransportersArrivalAction> launchAction, PlanetTile destinationTile, Action<Action> uiConfirmationCallback = null) where T : TransportersArrivalAction
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__0<T>(-2)
		{
			_003C_003E3__acceptanceReportGetter = acceptanceReportGetter,
			_003C_003E3__arrivalActionGetter = arrivalActionGetter,
			_003C_003E3__label = label,
			_003C_003E3__launchAction = launchAction,
			_003C_003E3__destinationTile = destinationTile,
			_003C_003E3__uiConfirmationCallback = uiConfirmationCallback
		};
	}

	public static bool AnyNonDownedColonist(IEnumerable<IThingHolder> pods)
	{
		if (CaravanShuttleUtility.IsCaravanShuttle(pods.First() as CompTransporter))
		{
			return true;
		}
		foreach (IThingHolder pod in pods)
		{
			ThingOwner directlyHeldThings = pod.GetDirectlyHeldThings();
			for (int i = 0; i < directlyHeldThings.Count; i++)
			{
				if (directlyHeldThings[i] is Pawn { IsColonist: not false, Downed: false })
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool AnyPotentialCaravanOwner(IEnumerable<IThingHolder> pods, Faction faction)
	{
		foreach (IThingHolder pod in pods)
		{
			if (pod is CompTransporter transporter && CaravanShuttleUtility.IsCaravanShuttle(transporter))
			{
				return true;
			}
			ThingOwner directlyHeldThings = pod.GetDirectlyHeldThings();
			for (int i = 0; i < directlyHeldThings.Count; i++)
			{
				if (directlyHeldThings[i] is Pawn pawn && CaravanUtility.IsOwner(pawn, faction))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static Thing GetLookTarget(List<ActiveTransporterInfo> pods)
	{
		for (int i = 0; i < pods.Count; i++)
		{
			ThingOwner directlyHeldThings = pods[i].GetDirectlyHeldThings();
			for (int j = 0; j < directlyHeldThings.Count; j++)
			{
				if (directlyHeldThings[j] is Pawn { IsColonist: not false } pawn)
				{
					return pawn;
				}
			}
		}
		for (int k = 0; k < pods.Count; k++)
		{
			Thing thing = pods[k].GetDirectlyHeldThings().FirstOrDefault();
			if (thing != null)
			{
				return thing;
			}
		}
		return null;
	}

	public static void DropTravellingDropPods(List<ActiveTransporterInfo> transporters, IntVec3 near, Map map)
	{
		RemovePawnsFromWorldPawns(transporters);
		for (int i = 0; i < transporters.Count; i++)
		{
			DropCellFinder.TryFindDropSpotNear(near, map, out var result, allowFogged: false, canRoofPunch: true, allowIndoors: true, null);
			DropPodUtility.MakeDropPodAt(result, map, transporters[i]);
		}
	}

	public static Thing DropShuttle(ActiveTransporterInfo transporter, Map map, IntVec3 near, Rot4? rotation = null, Faction faction = null)
	{
		RemovePawnsFromWorldPawns(Gen.YieldSingle(transporter));
		Thing thing = transporter.RemoveShuttle();
		if (thing == null)
		{
			thing = QuestGen_Shuttle.GenerateShuttle(faction, null, null, acceptColonists: false, onlyAcceptColonists: false, onlyAcceptHealthy: false, 0, dropEverythingIfUnsatisfied: false, leaveImmediatelyWhenSatisfied: false, dropEverythingOnArrival: true);
		}
		TransportShipDef shipDef = thing.TryGetComp<CompShuttle>()?.Props.shipDef ?? TransportShipDefOf.Ship_Shuttle;
		Rot4 valueOrDefault = rotation.GetValueOrDefault();
		if (!rotation.HasValue)
		{
			valueOrDefault = shipDef.shipThing.defaultPlacingRot;
			rotation = valueOrDefault;
		}
		thing.Rotation = rotation.Value;
		IntVec3 result;
		if ((bool)RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(near, map, shipDef.shipThing, rotation.Value))
		{
			result = near;
		}
		else if (!CellFinder.TryFindRandomCellNear(near, map, 20, (IntVec3 c) => RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(c, map, shipDef.shipThing, rotation.Value), out result) && !CellFinder.TryFindRandomCell(map, (IntVec3 c) => RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(c, map, shipDef.shipThing, rotation.Value), out result))
		{
			Log.Warning("Could not find a suitable cell for shuttle landing.");
			result = near;
		}
		CompTransporter compTransporter = thing.TryGetComp<CompTransporter>();
		compTransporter.innerContainer.TryAddRangeOrTransfer(transporter.innerContainer);
		TransportShip transportShip = compTransporter.Shuttle.shipParent;
		if (transportShip == null)
		{
			transportShip = TransportShipMaker.MakeTransportShip(shipDef, null, thing);
		}
		if (!result.IsValid)
		{
			result = DropCellFinder.GetBestShuttleLandingSpot(map, Faction.OfPlayer);
		}
		transportShip.ArriveAt(result, map.Parent);
		TransportShipDef shipDef2 = thing.TryGetComp<CompShuttle>().Props.shipDef;
		if (shipDef2 == null || !shipDef2.playerShuttle)
		{
			transportShip.AddJobs(ShipJobDefOf.Unload, ShipJobDefOf.FlyAway);
		}
		else
		{
			ShipJob_Unload shipJob_Unload = (ShipJob_Unload)ShipJobMaker.MakeShipJob(ShipJobDefOf.Unload);
			shipJob_Unload.dropMode = TransportShipDropMode.PawnsOnly;
			transportShip.AddJob(shipJob_Unload);
		}
		return thing;
	}

	public static void RemovePawnsFromWorldPawns(IEnumerable<ActiveTransporterInfo> transporters)
	{
		foreach (ActiveTransporterInfo transporter in transporters)
		{
			ThingOwner innerContainer = transporter.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (innerContainer[i] is Pawn p && p.IsWorldPawn())
				{
					Find.WorldPawns.RemovePawn(p);
				}
			}
		}
	}
}
