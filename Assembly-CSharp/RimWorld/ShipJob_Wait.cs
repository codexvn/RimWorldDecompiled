using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200177D RID: 6013
	[StaticConstructorOnStartup]
	public abstract class ShipJob_Wait : ShipJob
	{
		// Token: 0x170015AD RID: 5549
		// (get) Token: 0x06008D17 RID: 36119 RVA: 0x0029F885 File Offset: 0x0029DA85
		public override bool ShowGizmos
		{
			get
			{
				return this.showGizmos;
			}
		}

		// Token: 0x170015AE RID: 5550
		// (get) Token: 0x06008D18 RID: 36120 RVA: 0x0029F88D File Offset: 0x0029DA8D
		private int MaxLaunchDistance
		{
			get
			{
				return this.transportShip.def.maxLaunchDistance;
			}
		}

		// Token: 0x06008D19 RID: 36121 RVA: 0x0029F89F File Offset: 0x0029DA9F
		public override IEnumerable<Gizmo> GetJobGizmos()
		{
			if (this.transportShip.ShuttleComp.permitShuttle)
			{
				if (this.transportShip.TransporterComp.LoadingInProgressOrReadyToLaunch && this.transportShip.TransporterComp.innerContainer.Any<Thing>())
				{
					yield return this.LaunchAction();
				}
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "CommandShuttleDismiss".Translate(),
					defaultDesc = "CommandShuttleDismissDesc".Translate(),
					icon = ShipJob_Wait.DismissTex,
					alsoClickIfOtherInGroupClicked = false,
					action = delegate()
					{
						this.transportShip.ForceJob(ShipJobDefOf.Unload);
						this.transportShip.AddJob(ShipJobDefOf.FlyAway);
					}
				};
				yield return command_Action;
			}
			else if (!this.leaveImmediatelyWhenSatisfied)
			{
				Command_Action command_Action2 = new Command_Action();
				command_Action2.defaultLabel = "CommandSendShuttle".Translate();
				command_Action2.defaultDesc = "CommandSendShuttleDesc".Translate();
				command_Action2.icon = ShipJob_Wait.SendCommandTex;
				command_Action2.alsoClickIfOtherInGroupClicked = false;
				command_Action2.action = delegate()
				{
					this.transportShip.ForceJob(ShipJobDefOf.FlyAway);
				};
				if (!this.transportShip.ShuttleComp.AllRequiredThingsLoaded)
				{
					command_Action2.Disable("CommandSendShuttleFailMissingRequiredThing".Translate());
				}
				yield return command_Action2;
			}
			yield break;
		}

		// Token: 0x06008D1A RID: 36122 RVA: 0x0029F8B0 File Offset: 0x0029DAB0
		private Command_Action LaunchAction()
		{
			return new Command_Action
			{
				defaultLabel = "CommandLaunchGroup".Translate(),
				defaultDesc = "CommandLaunchGroupDesc".Translate(),
				icon = CompLaunchable.LaunchCommandTex,
				alsoClickIfOtherInGroupClicked = false,
				action = delegate()
				{
					CameraJumper.TryJump(CameraJumper.GetWorldTarget(this.transportShip.shipThing), CameraJumper.MovementMode.Pan);
					Find.WorldSelector.ClearSelection();
					PlanetTile tile = this.transportShip.shipThing.Map.Tile;
					Find.WorldTargeter.BeginTargeting(new Func<GlobalTargetInfo, bool>(this.ChoseWorldTarget), true, CompLaunchable.TargeterMouseAttachment, true, delegate
					{
						GenDraw.DrawWorldRadiusRing(tile, this.MaxLaunchDistance, null);
					}, (GlobalTargetInfo target) => CompLaunchable.TargetingLabelGetter(target, tile, this.MaxLaunchDistance, Gen.YieldSingle<IThingHolder>(this.transportShip.TransporterComp), new Action<PlanetTile, TransportersArrivalAction>(this.Launch), null, null), null, null, false);
				}
			};
		}

		// Token: 0x06008D1B RID: 36123 RVA: 0x0029F910 File Offset: 0x0029DB10
		private bool ChoseWorldTarget(GlobalTargetInfo target)
		{
			PlanetTile tile = this.transportShip.shipThing.Map.Tile;
			return CompLaunchable.ChoseWorldTarget(target, tile, Gen.YieldSingle<IThingHolder>(this.transportShip.TransporterComp), this.MaxLaunchDistance, new Action<PlanetTile, TransportersArrivalAction>(this.Launch), null, null);
		}

		// Token: 0x06008D1C RID: 36124 RVA: 0x0029F968 File Offset: 0x0029DB68
		private void Launch(PlanetTile destinationTile, TransportersArrivalAction arrivalAction)
		{
			ShipJob_FlyAway shipJob_FlyAway = (ShipJob_FlyAway)ShipJobMaker.MakeShipJob(ShipJobDefOf.FlyAway);
			shipJob_FlyAway.destinationTile = destinationTile;
			shipJob_FlyAway.arrivalAction = arrivalAction;
			this.transportShip.SetNextJob(shipJob_FlyAway);
			this.transportShip.TryGetNextJob();
			CameraJumper.TryHideWorld();
		}

		// Token: 0x06008D1D RID: 36125 RVA: 0x0029F9B0 File Offset: 0x0029DBB0
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.transportShip.ShipExistsAndIsSpawned)
			{
				if (this.leaveImmediatelyWhenSatisfied && this.transportShip.ShuttleComp.AllRequiredThingsLoaded)
				{
					this.SendAway();
					return;
				}
				if (this.transportShip.shipThing.IsHashIntervalTick(60, delta))
				{
					bool flag = false;
					if (!this.sendAwayIfAnyDespawnedDownedOrDead.NullOrEmpty<Thing>())
					{
						foreach (Thing thing in this.sendAwayIfAnyDespawnedDownedOrDead)
						{
							Pawn pawn = thing as Pawn;
							if ((!thing.Spawned || (pawn != null && (pawn.Dead || pawn.Downed))) && !this.transportShip.TransporterComp.innerContainer.Contains(thing))
							{
								flag = true;
								this.SendAway();
								break;
							}
						}
					}
					if (!flag && !this.sendAwayIfAllDespawned.NullOrEmpty<Thing>())
					{
						bool flag2 = false;
						foreach (Thing thing2 in this.sendAwayIfAllDespawned)
						{
							if (thing2.MapHeld == this.transportShip.shipThing.Map || thing2.MapHeld.PocketMapParent.sourceMap == this.transportShip.shipThing.Map || this.transportShip.TransporterComp.innerContainer.Contains(thing2))
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							this.SendAway();
						}
					}
				}
			}
		}

		// Token: 0x06008D1E RID: 36126 RVA: 0x0029FB58 File Offset: 0x0029DD58
		protected virtual void SendAway()
		{
			this.transportShip.SetNextJob(ShipJobMaker.MakeShipJob(ShipJobDefOf.FlyAway));
			this.End();
		}

		// Token: 0x06008D1F RID: 36127 RVA: 0x0029FB78 File Offset: 0x0029DD78
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.leaveImmediatelyWhenSatisfied, "leaveImmediatelyWhenSatisfied", false, false);
			Scribe_Values.Look<bool>(ref this.showGizmos, "showGizmos", true, false);
			Scribe_Values.Look<bool>(ref this.targetPlayerSettlement, "targetPlayerSettlement", false, false);
			Scribe_Collections.Look<Thing>(ref this.sendAwayIfAllDespawned, "sendAwayIfAllDespawned", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.sendAwayIfAnyDespawnedDownedOrDead, "sendAwayIfAnyDespawned", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<Thing> list = this.sendAwayIfAllDespawned;
				if (list != null)
				{
					list.RemoveAll((Thing x) => x == null);
				}
				List<Thing> list2 = this.sendAwayIfAnyDespawnedDownedOrDead;
				if (list2 == null)
				{
					return;
				}
				list2.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04005BB2 RID: 23474
		public bool leaveImmediatelyWhenSatisfied;

		// Token: 0x04005BB3 RID: 23475
		public bool showGizmos = true;

		// Token: 0x04005BB4 RID: 23476
		public bool targetPlayerSettlement;

		// Token: 0x04005BB5 RID: 23477
		public List<Thing> sendAwayIfAllDespawned;

		// Token: 0x04005BB6 RID: 23478
		public List<Thing> sendAwayIfAnyDespawnedDownedOrDead;

		// Token: 0x04005BB7 RID: 23479
		private static readonly Texture2D DismissTex = ContentFinder<Texture2D>.Get("UI/Commands/DismissShuttle", true);

		// Token: 0x04005BB8 RID: 23480
		protected static readonly Texture2D SendCommandTex = CompLaunchable.LaunchCommandTex;

		// Token: 0x04005BB9 RID: 23481
		private const int CheckAllDespawnedInterval = 60;
	}
}
