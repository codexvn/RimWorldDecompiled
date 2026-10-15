using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public abstract class ShipJob_Wait : ShipJob
{
	[CompilerGenerated]
	private sealed class _003CGetJobGizmos_003Ed__12 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ShipJob_Wait _003C_003E4__this;

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
		public _003CGetJobGizmos_003Ed__12(int _003C_003E1__state)
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
			ShipJob_Wait CS_0024_003C_003E8__locals9 = _003C_003E4__this;
			Command_Action command_Action2;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals9.transportShip.ShuttleComp.permitShuttle)
				{
					if (CS_0024_003C_003E8__locals9.transportShip.TransporterComp.LoadingInProgressOrReadyToLaunch && CS_0024_003C_003E8__locals9.transportShip.TransporterComp.innerContainer.Any())
					{
						_003C_003E2__current = CS_0024_003C_003E8__locals9.LaunchAction();
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0087;
				}
				if (!CS_0024_003C_003E8__locals9.leaveImmediatelyWhenSatisfied)
				{
					Command_Action command_Action = new Command_Action
					{
						defaultLabel = "CommandSendShuttle".Translate(),
						defaultDesc = "CommandSendShuttleDesc".Translate(),
						icon = SendCommandTex,
						alsoClickIfOtherInGroupClicked = false,
						action = delegate
						{
							CS_0024_003C_003E8__locals9.transportShip.ForceJob(ShipJobDefOf.FlyAway);
						}
					};
					if (!CS_0024_003C_003E8__locals9.transportShip.ShuttleComp.AllRequiredThingsLoaded)
					{
						command_Action.Disable("CommandSendShuttleFailMissingRequiredThing".Translate());
					}
					_003C_003E2__current = command_Action;
					_003C_003E1__state = 3;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0087;
			case 2:
				_003C_003E1__state = -1;
				break;
			case 3:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0087:
				command_Action2 = new Command_Action
				{
					defaultLabel = "CommandShuttleDismiss".Translate(),
					defaultDesc = "CommandShuttleDismissDesc".Translate(),
					icon = DismissTex,
					alsoClickIfOtherInGroupClicked = false,
					action = delegate
					{
						CS_0024_003C_003E8__locals9.transportShip.ForceJob(ShipJobDefOf.Unload);
						CS_0024_003C_003E8__locals9.transportShip.AddJob(ShipJobDefOf.FlyAway);
					}
				};
				_003C_003E2__current = command_Action2;
				_003C_003E1__state = 2;
				return true;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetJobGizmos_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetJobGizmos_003Ed__12(0)
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

	public bool leaveImmediatelyWhenSatisfied;

	public bool showGizmos = true;

	public bool targetPlayerSettlement;

	public List<Thing> sendAwayIfAllDespawned;

	public List<Thing> sendAwayIfAnyDespawnedDownedOrDead;

	private static readonly Texture2D DismissTex = ContentFinder<Texture2D>.Get("UI/Commands/DismissShuttle");

	protected static readonly Texture2D SendCommandTex = CompLaunchable.LaunchCommandTex;

	private const int CheckAllDespawnedInterval = 60;

	public override bool ShowGizmos => showGizmos;

	private int MaxLaunchDistance => transportShip.def.maxLaunchDistance;

	[IteratorStateMachine(typeof(_003CGetJobGizmos_003Ed__12))]
	public override IEnumerable<Gizmo> GetJobGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetJobGizmos_003Ed__12(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Command_Action LaunchAction()
	{
		return new Command_Action
		{
			defaultLabel = "CommandLaunchGroup".Translate(),
			defaultDesc = "CommandLaunchGroupDesc".Translate(),
			icon = CompLaunchable.LaunchCommandTex,
			alsoClickIfOtherInGroupClicked = false,
			action = delegate
			{
				CameraJumper.TryJump(CameraJumper.GetWorldTarget(transportShip.shipThing));
				Find.WorldSelector.ClearSelection();
				PlanetTile tile = transportShip.shipThing.Map.Tile;
				Find.WorldTargeter.BeginTargeting(ChoseWorldTarget, canTargetTiles: true, CompLaunchable.TargeterMouseAttachment, closeWorldTabWhenFinished: true, delegate
				{
					PlanetTile planetTile = tile;
					if (planetTile.Layer != Find.WorldSelector.SelectedLayer)
					{
						planetTile = Find.WorldSelector.SelectedLayer.GetClosestTile_NewTemp(planetTile);
					}
					GenDraw.DrawWorldRadiusRing(planetTile, MaxLaunchDistance);
				}, (GlobalTargetInfo target) => CompLaunchable.TargetingLabelGetter(target, tile, MaxLaunchDistance, Gen.YieldSingle((IThingHolder)transportShip.TransporterComp), Launch, null, null), null, null);
			}
		};
	}

	private bool ChoseWorldTarget(GlobalTargetInfo target)
	{
		PlanetTile tile = transportShip.shipThing.Map.Tile;
		return CompLaunchable.ChoseWorldTarget(target, tile, Gen.YieldSingle((IThingHolder)transportShip.TransporterComp), MaxLaunchDistance, Launch, null, null);
	}

	private void Launch(PlanetTile destinationTile, TransportersArrivalAction arrivalAction)
	{
		ShipJob_FlyAway shipJob_FlyAway = (ShipJob_FlyAway)ShipJobMaker.MakeShipJob(ShipJobDefOf.FlyAway);
		shipJob_FlyAway.destinationTile = destinationTile;
		shipJob_FlyAway.arrivalAction = arrivalAction;
		transportShip.SetNextJob(shipJob_FlyAway);
		transportShip.TryGetNextJob();
		CameraJumper.TryHideWorld();
	}

	public override void TickInterval(int delta)
	{
		base.TickInterval(delta);
		if (!transportShip.ShipExistsAndIsSpawned)
		{
			return;
		}
		if (leaveImmediatelyWhenSatisfied && transportShip.ShuttleComp.AllRequiredThingsLoaded)
		{
			SendAway();
		}
		else
		{
			if (!transportShip.shipThing.IsHashIntervalTick(60, delta))
			{
				return;
			}
			bool flag = false;
			if (!sendAwayIfAnyDespawnedDownedOrDead.NullOrEmpty())
			{
				foreach (Thing item in sendAwayIfAnyDespawnedDownedOrDead)
				{
					Pawn pawn = item as Pawn;
					if ((!item.Spawned || (pawn != null && (pawn.Dead || pawn.Downed))) && !transportShip.TransporterComp.innerContainer.Contains(item))
					{
						flag = true;
						SendAway();
						break;
					}
				}
			}
			if (flag || sendAwayIfAllDespawned.NullOrEmpty())
			{
				return;
			}
			bool flag2 = false;
			foreach (Thing item2 in sendAwayIfAllDespawned)
			{
				if (item2.MapHeld == transportShip.shipThing.Map || item2.MapHeld.PocketMapParent.sourceMap == transportShip.shipThing.Map || transportShip.TransporterComp.innerContainer.Contains(item2))
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				SendAway();
			}
		}
	}

	protected virtual void SendAway()
	{
		transportShip.SetNextJob(ShipJobMaker.MakeShipJob(ShipJobDefOf.FlyAway));
		End();
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref leaveImmediatelyWhenSatisfied, "leaveImmediatelyWhenSatisfied", defaultValue: false);
		Scribe_Values.Look(ref showGizmos, "showGizmos", defaultValue: true);
		Scribe_Values.Look(ref targetPlayerSettlement, "targetPlayerSettlement", defaultValue: false);
		Scribe_Collections.Look(ref sendAwayIfAllDespawned, "sendAwayIfAllDespawned", LookMode.Reference);
		Scribe_Collections.Look(ref sendAwayIfAnyDespawnedDownedOrDead, "sendAwayIfAnyDespawned", LookMode.Reference);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			sendAwayIfAllDespawned?.RemoveAll((Thing x) => x == null);
			sendAwayIfAnyDespawnedDownedOrDead?.RemoveAll((Thing x) => x == null);
		}
	}
}
