using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D0C RID: 11532
	[StaticConstructorOnStartup]
	public class CompLaunchable : ThingComp
	{
		// Token: 0x17002918 RID: 10520
		// (get) Token: 0x06010339 RID: 66361 RVA: 0x004AF1B0 File Offset: 0x004AD3B0
		public CompProperties_Launchable Props
		{
			get
			{
				return (CompProperties_Launchable)this.props;
			}
		}

		// Token: 0x17002919 RID: 10521
		// (get) Token: 0x0601033A RID: 66362 RVA: 0x004AF1C0 File Offset: 0x004AD3C0
		protected CompTransporter Transporter
		{
			get
			{
				CompTransporter result;
				if ((result = this.cachedCompTransporter) == null)
				{
					result = (this.cachedCompTransporter = this.parent.GetComp<CompTransporter>());
				}
				return result;
			}
		}

		// Token: 0x1700291A RID: 10522
		// (get) Token: 0x0601033B RID: 66363 RVA: 0x004AF1EC File Offset: 0x004AD3EC
		public virtual CompRefuelable Refuelable
		{
			get
			{
				CompRefuelable result;
				if ((result = this.cachedCompRefuelable) == null)
				{
					result = (this.cachedCompRefuelable = this.parent.GetComp<CompRefuelable>());
				}
				return result;
			}
		}

		// Token: 0x1700291B RID: 10523
		// (get) Token: 0x0601033C RID: 66364 RVA: 0x004AF218 File Offset: 0x004AD418
		private CompShuttle Shuttle
		{
			get
			{
				CompShuttle result;
				if ((result = this.cachedCompShuttle) == null)
				{
					result = (this.cachedCompShuttle = this.parent.GetComp<CompShuttle>());
				}
				return result;
			}
		}

		// Token: 0x1700291C RID: 10524
		// (get) Token: 0x0601033D RID: 66365 RVA: 0x004AF243 File Offset: 0x004AD443
		protected List<CompTransporter> TransportersInGroup
		{
			get
			{
				return this.Transporter.TransportersInGroup(this.parent.Map);
			}
		}

		// Token: 0x1700291D RID: 10525
		// (get) Token: 0x0601033E RID: 66366 RVA: 0x004AF25B File Offset: 0x004AD45B
		private bool AnyInGroupHasAnythingLeftToLoad
		{
			get
			{
				return this.Transporter.AnyInGroupHasAnythingLeftToLoad;
			}
		}

		// Token: 0x1700291E RID: 10526
		// (get) Token: 0x0601033F RID: 66367 RVA: 0x004AF268 File Offset: 0x004AD468
		private Thing FirstThingLeftToLoadInGroup
		{
			get
			{
				return this.Transporter.FirstThingLeftToLoadInGroup;
			}
		}

		// Token: 0x1700291F RID: 10527
		// (get) Token: 0x06010340 RID: 66368 RVA: 0x004AF275 File Offset: 0x004AD475
		public virtual float FuelLevel
		{
			get
			{
				return this.Refuelable.Fuel;
			}
		}

		// Token: 0x17002920 RID: 10528
		// (get) Token: 0x06010341 RID: 66369 RVA: 0x004AF282 File Offset: 0x004AD482
		public virtual float MaxFuelLevel
		{
			get
			{
				return this.Refuelable.Props.fuelCapacity;
			}
		}

		// Token: 0x17002921 RID: 10529
		// (get) Token: 0x06010342 RID: 66370 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool RequiresFuelingPort
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002922 RID: 10530
		// (get) Token: 0x06010343 RID: 66371 RVA: 0x004AF294 File Offset: 0x004AD494
		private bool AnyInGroupIsUnderRoof
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				for (int i = 0; i < transportersInGroup.Count; i++)
				{
					if (transportersInGroup[i].parent.Position.Roofed(this.parent.Map))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17002923 RID: 10531
		// (get) Token: 0x06010344 RID: 66372 RVA: 0x004AF2E0 File Offset: 0x004AD4E0
		private bool AllLaunchablesInGroupHaveFuelForLaunch
		{
			get
			{
				foreach (CompTransporter compTransporter in this.TransportersInGroup)
				{
					if (compTransporter.Launchable.FuelLevel < compTransporter.Launchable.Props.minFuelCost)
					{
						return false;
					}
					if (compTransporter.Launchable.RequiresFuelingPort && !compTransporter.Launchable.Refuelable.HasFuel)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17002924 RID: 10532
		// (get) Token: 0x06010345 RID: 66373 RVA: 0x004AF374 File Offset: 0x004AD574
		private float MinFuelLevelInGroup
		{
			get
			{
				List<CompTransporter> transportersInGroup = this.TransportersInGroup;
				float num = 0f;
				bool flag = false;
				foreach (CompTransporter compTransporter in transportersInGroup)
				{
					float fuelLevel = compTransporter.Launchable.FuelLevel;
					if (!flag || fuelLevel < num)
					{
						num = fuelLevel;
						flag = true;
					}
				}
				if (flag)
				{
					return num;
				}
				return 0f;
			}
		}

		// Token: 0x06010346 RID: 66374 RVA: 0x004AF3E8 File Offset: 0x004AD5E8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.lastLaunchTick, "lastLaunchTick", 0, false);
		}

		// Token: 0x06010347 RID: 66375 RVA: 0x004AF404 File Offset: 0x004AD604
		public override void CompTick()
		{
			if (!this.Props.cooldownEndedMessage.NullOrEmpty() && this.lastLaunchTick > 0 && this.lastLaunchTick + this.Props.cooldownTicks == Find.TickManager.TicksGame)
			{
				Messages.Message(this.Props.cooldownEndedMessage.Formatted(this.parent.LabelCap), this.parent, MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x06010348 RID: 66376 RVA: 0x004AF488 File Offset: 0x004AD688
		public virtual AcceptanceReport CanLaunch(float? overrideFuelLevel = null)
		{
			if (!this.Transporter.LoadingInProgressOrReadyToLaunch)
			{
				return "CommandLaunchGroupFailNotLoaded".Translate();
			}
			if (this.parent.Spawned && this.AnyInGroupIsUnderRoof)
			{
				return "CommandLaunchGroupFailUnderRoof".Translate();
			}
			if (overrideFuelLevel != null)
			{
				float? num = overrideFuelLevel;
				float minFuelCost = this.Props.minFuelCost;
				if (num.GetValueOrDefault() < minFuelCost & num != null)
				{
					return "CommandLaunchGroupFailNoFuel".Translate();
				}
			}
			else if (!this.AllLaunchablesInGroupHaveFuelForLaunch)
			{
				return "CommandLaunchGroupFailNoFuel".Translate();
			}
			if (this.Transporter.OverMassCapacity)
			{
				return "CommandLaunchGroupFailOverMassCapacity".Translate() + ": " + "MassUsageString".Translate(this.Transporter.MassUsage.ToString("F0"), this.Transporter.MassCapacity.ToString("F0"));
			}
			int num2 = this.Props.cooldownTicks - Find.TickManager.TicksGame + this.lastLaunchTick;
			if (this.Props.cooldownTicks > 0 && this.lastLaunchTick > 0 && num2 > 0)
			{
				return "CommandLaunchGroupCooldown".Translate() + " (" + num2.ToStringTicksToPeriod(true, false, true, true, false) + ")";
			}
			if (this.Shuttle != null)
			{
				return this.Shuttle.CanLaunch;
			}
			return true;
		}

		// Token: 0x06010349 RID: 66377 RVA: 0x004AF624 File Offset: 0x004AD824
		public int MaxLaunchDistanceEver(PlanetLayer layer)
		{
			if (!this.Transporter.LoadingInProgressOrReadyToLaunch)
			{
				return 0;
			}
			int num = this.MaxLaunchDistanceAtFuelLevel(this.MaxFuelLevel, layer);
			if (this.Props.fixedLaunchDistanceMax >= 0)
			{
				num = Mathf.Min(num, Mathf.RoundToInt((float)this.Props.fixedLaunchDistanceMax / layer.Def.rangeDistanceFactor));
			}
			return num;
		}

		// Token: 0x0601034A RID: 66378 RVA: 0x004AF681 File Offset: 0x004AD881
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			CompTransporter transporter = this.Transporter;
			if (transporter != null && !transporter.Groupable)
			{
				int num = 0;
				foreach (object obj in Find.Selector.SelectedObjects)
				{
					ThingWithComps thingWithComps = obj as ThingWithComps;
					if (thingWithComps != null && thingWithComps.HasComp<CompTransporter>())
					{
						num++;
					}
				}
				if (num > 1)
				{
					yield break;
				}
			}
			Command_Action command_Action = new Command_Action
			{
				defaultLabel = "CommandLaunchGroup".Translate(),
				defaultDesc = "CommandLaunchGroupDesc".Translate(),
				icon = CompLaunchable.LaunchCommandTex,
				alsoClickIfOtherInGroupClicked = false,
				action = delegate()
				{
					if (this.AnyInGroupHasAnythingLeftToLoad)
					{
						TaggedString taggedString = "ConfirmSendNotCompletelyLoadedLaunchable".Translate() + ":\n";
						taggedString += (from x in this.Transporter.leftToLoad
						select x.AnyThing.LabelCap).ToLineList(" -", false);
						taggedString += "\n\n" + "ConfirmSendLaunchAnyway".Translate();
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(taggedString, delegate()
						{
							this.StartChoosingDestination(new Action<PlanetTile, TransportersArrivalAction>(this.TryLaunch), null);
						}, false, null, WindowLayer.Dialog));
						return;
					}
					this.StartChoosingDestination(new Action<PlanetTile, TransportersArrivalAction>(this.TryLaunch), null);
				}
			};
			AcceptanceReport acceptanceReport = this.CanLaunch(null);
			if (!acceptanceReport.Accepted)
			{
				command_Action.Disable(acceptanceReport.Reason);
			}
			yield return command_Action;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: End cooldown",
					action = delegate()
					{
						this.lastLaunchTick = Find.TickManager.TicksGame - this.Props.cooldownTicks;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0601034B RID: 66379 RVA: 0x004AF694 File Offset: 0x004AD894
		public override string CompInspectStringExtra()
		{
			if (!this.Transporter.LoadingInProgressOrReadyToLaunch)
			{
				return null;
			}
			if (this.Refuelable != null && !this.Refuelable.HasFuel)
			{
				return "NotReadyForLaunch".Translate() + ": " + "NotAllLaunchablesInGroupHaveAnyFuel".Translate().CapitalizeFirst() + ".";
			}
			if (this.AnyInGroupHasAnythingLeftToLoad)
			{
				return "NotReadyForLaunch".Translate() + ": " + "LaunchableInGroupHasSomethingLeftToLoad".Translate().CapitalizeFirst() + ".";
			}
			if (!this.AllLaunchablesInGroupHaveFuelForLaunch)
			{
				return "NotReadyForLaunch".Translate() + ": " + "NotAllLaunchablesInGroupHaveAnyFuel".Translate().CapitalizeFirst() + ".";
			}
			int num = this.Props.cooldownTicks - Find.TickManager.TicksGame + this.lastLaunchTick;
			if (this.Props.cooldownTicks > 0 && this.lastLaunchTick > 0 && num > 0)
			{
				return "NotReadyForLaunch".Translate() + ": " + "CommandLaunchGroupCooldown".Translate() + " (" + num.ToStringTicksToPeriod(true, false, true, true, false) + ")";
			}
			return "ReadyForLaunch".Translate();
		}

		// Token: 0x0601034C RID: 66380 RVA: 0x004AF820 File Offset: 0x004ADA20
		public void StartChoosingDestination(Action<PlanetTile, TransportersArrivalAction> launchAction, float? overrideFuelLevel = null)
		{
			PlanetTile tile = this.parent.Tile;
			CameraJumper.TryJump(CameraJumper.GetWorldTarget(new GlobalTargetInfo(tile)), CameraJumper.MovementMode.Pan);
			Find.WorldSelector.ClearSelection();
			Find.WorldTargeter.BeginTargeting((GlobalTargetInfo t) => this.ChoseWorldTarget(t, launchAction, overrideFuelLevel), true, CompLaunchable.TargeterMouseAttachment, !CaravanShuttleUtility.IsCaravanShuttle(this.Transporter), delegate
			{
				PlanetTile planetTile;
				if (this.cachedLayer != Find.WorldSelector.SelectedLayer || this.cachedOrigin != tile)
				{
					this.cachedLayer = Find.WorldSelector.SelectedLayer;
					this.cachedOrigin = tile;
					planetTile = (this.cachedClosest = Find.WorldSelector.SelectedLayer.GetClosestTile(tile));
				}
				else
				{
					planetTile = this.cachedClosest;
				}
				int num = this.MaxLaunchDistanceEver(planetTile.Layer);
				GenDraw.DrawWorldRadiusRing(planetTile, num, CompPilotConsole.GetThrusterRadiusMat(planetTile));
				if (this.Refuelable != null)
				{
					int num2 = GravshipUtility.MaxDistForFuel(this.FuelLevel, planetTile.Layer, PlanetLayer.Selected, this.Props.fuelPerTile, 1f);
					if (num2 < num)
					{
						GenDraw.DrawWorldRadiusRing(planetTile, num2, CompPilotConsole.GetFuelRadiusMat(planetTile));
					}
				}
			}, (GlobalTargetInfo target) => CompLaunchable.TargetingLabelGetter(target, tile, this.MaxLaunchDistanceEver(this.cachedClosest.Layer), this.TransportersInGroup, launchAction, this, overrideFuelLevel), null, new PlanetTile?(tile), true);
		}

		// Token: 0x0601034D RID: 66381 RVA: 0x004AF8C8 File Offset: 0x004ADAC8
		private bool ChoseWorldTarget(GlobalTargetInfo target, Action<PlanetTile, TransportersArrivalAction> launchAction, float? overrideFuelLevel = null)
		{
			if (!this.Transporter.LoadingInProgressOrReadyToLaunch)
			{
				return true;
			}
			this.cachedClosest = (this.cachedOrigin = PlanetTile.Invalid);
			this.cachedLayer = null;
			PlanetTile tile = this.parent.Tile;
			return CompLaunchable.ChoseWorldTarget(target, tile, this.TransportersInGroup, this.MaxLaunchDistanceEver(target.Tile.Layer), launchAction, this, overrideFuelLevel);
		}

		// Token: 0x0601034E RID: 66382 RVA: 0x004AF930 File Offset: 0x004ADB30
		public void TryLaunch(PlanetTile destinationTile, TransportersArrivalAction arrivalAction)
		{
			if (!this.parent.Spawned)
			{
				Log.Error(string.Format("Tried to launch {0}, but it's unspawned.", this.parent));
				return;
			}
			List<CompTransporter> transportersInGroup = this.TransportersInGroup;
			if (transportersInGroup == null)
			{
				Log.Error(string.Format("Tried to launch {0}, but it's not in any group.", this.parent));
				return;
			}
			if (!this.CanLaunch(null))
			{
				return;
			}
			Map map = this.parent.Map;
			int num = Find.WorldGrid.TraversalDistanceBetween(map.Tile, destinationTile, true, int.MaxValue, true);
			Current.Game.CurrentMap = map;
			if (num > this.MaxLaunchDistanceAtFuelLevel(this.MinFuelLevelInGroup, destinationTile.Layer))
			{
				return;
			}
			this.Transporter.TryRemoveLord(map);
			int groupID = this.Transporter.groupID;
			float amount = Mathf.Max(this.FuelNeededToLaunchAtDist((float)num, destinationTile.Layer), 1f);
			this.lastLaunchTick = Find.TickManager.TicksGame;
			for (int i = 0; i < transportersInGroup.Count; i++)
			{
				CompTransporter compTransporter = transportersInGroup[i];
				CompRefuelable refuelable = compTransporter.Launchable.Refuelable;
				if (refuelable != null)
				{
					refuelable.ConsumeFuel(amount);
				}
				ThingOwner directlyHeldThings = compTransporter.GetDirectlyHeldThings();
				ActiveTransporter activeTransporter = (ActiveTransporter)ThingMaker.MakeThing(this.Props.activeTransporterDef ?? ThingDefOf.ActiveDropPod, null);
				activeTransporter.Contents = new ActiveTransporterInfo();
				activeTransporter.Contents.innerContainer.TryAddRangeOrTransfer(directlyHeldThings, true, true);
				activeTransporter.Contents.sentTransporterDef = this.parent.def;
				activeTransporter.Rotation = this.parent.Rotation;
				FlyShipLeaving flyShipLeaving = (FlyShipLeaving)SkyfallerMaker.MakeSkyfaller(this.Props.skyfallerLeaving ?? ThingDefOf.DropPodLeaving, activeTransporter);
				flyShipLeaving.groupID = groupID;
				flyShipLeaving.destinationTile = destinationTile;
				flyShipLeaving.arrivalAction = arrivalAction;
				flyShipLeaving.worldObjectDef = (this.Props.worldObjectDef ?? WorldObjectDefOf.TravellingTransporters);
				if (compTransporter.parent.HasComp<CompShuttle>())
				{
					activeTransporter.Contents.SetShuttle(compTransporter.parent);
				}
				else
				{
					compTransporter.CleanUpLoadingVars(map);
					compTransporter.parent.Destroy(DestroyMode.Vanish);
				}
				GenSpawn.Spawn(flyShipLeaving, compTransporter.parent.Position, map, WipeMode.Vanish);
				INotifyLaunchableLaunch notifyLaunchableLaunch = ((refuelable != null) ? refuelable.parent : null) as INotifyLaunchableLaunch;
				if (notifyLaunchableLaunch != null)
				{
					notifyLaunchableLaunch.Notify_LaunchableLaunched(compTransporter.Launchable);
				}
			}
			CameraJumper.TryHideWorld();
		}

		// Token: 0x0601034F RID: 66383 RVA: 0x004AFB9B File Offset: 0x004ADD9B
		public void Notify_FuelingPortSourceDeSpawned()
		{
			if (this.Transporter.CancelLoad())
			{
				Messages.Message("MessageTransportersLoadCanceled_FuelingPortGiverDeSpawned".Translate(), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06010350 RID: 66384 RVA: 0x004AFBD0 File Offset: 0x004ADDD0
		public int MaxLaunchDistanceAtFuelLevel(float fuelLevel)
		{
			if (this.Props.fixedLaunchDistanceMax >= 0)
			{
				return this.Props.fixedLaunchDistanceMax;
			}
			return this.MaxLaunchDistanceAtFuelLevel(fuelLevel, this.parent.Tile.Layer);
		}

		// Token: 0x06010351 RID: 66385 RVA: 0x004AFC14 File Offset: 0x004ADE14
		public int MaxLaunchDistanceAtFuelLevel(float fuelLevel, PlanetLayer layer)
		{
			if (this.Props.fixedLaunchDistanceMax >= 0)
			{
				return this.Props.fixedLaunchDistanceMax;
			}
			if (fuelLevel < this.Props.minFuelCost)
			{
				return 0;
			}
			return Mathf.FloorToInt(fuelLevel / (this.Props.fuelPerTile * layer.Def.rangeDistanceFactor));
		}

		// Token: 0x06010352 RID: 66386 RVA: 0x004AFC6C File Offset: 0x004ADE6C
		public float FuelNeededToLaunchAtDist(float dist, PlanetLayer layer)
		{
			float num = dist * (this.Props.fuelPerTile * layer.Def.rangeDistanceFactor);
			if (this.Props.minFuelCost > 0f && num < this.Props.minFuelCost)
			{
				return this.Props.minFuelCost;
			}
			return num;
		}

		// Token: 0x06010353 RID: 66387 RVA: 0x004AFCC0 File Offset: 0x004ADEC0
		private IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptionsAt(PlanetTile tile, Action<PlanetTile, TransportersArrivalAction> launchAction, bool isShuttle)
		{
			bool anything = false;
			if (TransportersArrivalAction_FormCaravan.CanFormCaravanAt(this.TransportersInGroup, tile) && !Find.WorldObjects.AnySettlementBaseAt(tile) && !Find.WorldObjects.AnySiteAt(tile))
			{
				anything = true;
				yield return new FloatMenuOption("FormCaravanHere".Translate(), delegate()
				{
					launchAction(tile, new TransportersArrivalAction_FormCaravan(isShuttle ? "MessageShuttleArrived" : "MessageTransportPodsArrived"));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			List<WorldObject> worldObjects = Find.WorldObjects.AllWorldObjects;
			int num;
			for (int i = 0; i < worldObjects.Count; i = num + 1)
			{
				if (!(worldObjects[i].Tile != tile))
				{
					if (isShuttle)
					{
						foreach (FloatMenuOption floatMenuOption in worldObjects[i].GetShuttleFloatMenuOptions(this.TransportersInGroup, launchAction))
						{
							anything = true;
							yield return floatMenuOption;
						}
						IEnumerator<FloatMenuOption> enumerator = null;
					}
					else
					{
						foreach (FloatMenuOption floatMenuOption2 in worldObjects[i].GetTransportersFloatMenuOptions(this.TransportersInGroup, launchAction))
						{
							anything = true;
							yield return floatMenuOption2;
						}
						IEnumerator<FloatMenuOption> enumerator = null;
					}
				}
				num = i;
			}
			if (!anything && !Find.World.Impassable(tile))
			{
				yield return new FloatMenuOption("TransportPodsContentsWillBeLost".Translate(), delegate()
				{
					launchAction(tile, null);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
			yield break;
		}

		// Token: 0x06010354 RID: 66388 RVA: 0x004AFCE8 File Offset: 0x004ADEE8
		public static bool ChoseWorldTarget(GlobalTargetInfo target, PlanetTile tile, IEnumerable<IThingHolder> pods, int maxLaunchDistance, Action<PlanetTile, TransportersArrivalAction> launchAction, CompLaunchable launchable, float? overrideFuelLevel = null)
		{
			if (!target.IsValid)
			{
				Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (target.HasWorldObject && !target.WorldObject.def.validLaunchTarget)
			{
				Messages.Message("MessageWorldObjectIsInvalid".Translate(target.WorldObject.Named("OBJECT")), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (ModsConfig.OdysseyActive && target.HasWorldObject && target.WorldObject.RequiresSignalJammerToReach)
			{
				Messages.Message("TransportPodDestinationRequiresSignalJammer".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			float fuelLevel = overrideFuelLevel ?? ((launchable != null) ? launchable.FuelLevel : -1f);
			int num = Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile, true, int.MaxValue, true);
			int? num2 = (launchable != null) ? new int?(launchable.MaxLaunchDistanceAtFuelLevel(fuelLevel, target.Tile.Layer)) : null;
			float num3 = (num2 != null) ? ((float)num2.GetValueOrDefault()) : -1f;
			float fuelRequired = (launchable != null) ? launchable.FuelNeededToLaunchAtDist((float)num, target.Tile.Layer) : -1f;
			if (num3 >= 0f && (float)num > num3)
			{
				Messages.Message("TransportPodNotEnoughFuel".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (maxLaunchDistance >= 0 && num > maxLaunchDistance)
			{
				Messages.Message("TransportPodDestinationBeyondMaximumRange".Translate(), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			bool flag = ((launchable != null) ? launchable.Shuttle : null) != null;
			List<FloatMenuOption> list = ((launchable != null) ? launchable.GetTransportersFloatMenuOptionsAt(target.Tile, launchAction, flag) : CompLaunchable.GetOptionsForTile(target.Tile, pods, launchAction)).ToList<FloatMenuOption>();
			if (!list.Any<FloatMenuOption>())
			{
				if (Find.World.Impassable(target.Tile))
				{
					Messages.Message("MessageTransportPodsDestinationIsInvalid".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				if (flag && fuelLevel < fuelRequired * 2f)
				{
					WorldObject worldObject = target.WorldObject;
					bool flag2;
					if (worldObject == null)
					{
						flag2 = true;
					}
					else
					{
						Faction faction = worldObject.Faction;
						bool? flag3 = (faction != null) ? new bool?(faction.IsPlayer) : null;
						bool flag4 = true;
						flag2 = !(flag3.GetValueOrDefault() == flag4 & flag3 != null);
					}
					if (flag2)
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmShuttleNotEnoughFuelToReturn".Translate(fuelLevel, fuelRequired * 2f), delegate()
						{
							launchAction(target.Tile, null);
						}, false, null, WindowLayer.Dialog));
						return true;
					}
				}
				launchAction(target.Tile, null);
				return true;
			}
			else
			{
				if (flag && fuelLevel < fuelRequired * 2f)
				{
					WorldObject worldObject2 = target.WorldObject;
					bool flag5;
					if (worldObject2 == null)
					{
						flag5 = true;
					}
					else
					{
						Faction faction2 = worldObject2.Faction;
						bool? flag3 = (faction2 != null) ? new bool?(faction2.IsPlayer) : null;
						bool flag4 = true;
						flag5 = !(flag3.GetValueOrDefault() == flag4 & flag3 != null);
					}
					if (flag5)
					{
						for (int i = 0; i < list.Count; i++)
						{
							Action oldAction = list[i].action;
							list[i].action = delegate()
							{
								Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmShuttleNotEnoughFuelToReturn".Translate(fuelLevel, fuelRequired * 2f), oldAction, false, null, WindowLayer.Dialog));
							};
						}
					}
				}
				if (list.Count != 1)
				{
					Find.WindowStack.Add(new FloatMenu(list));
					return false;
				}
				if (!list.First<FloatMenuOption>().Disabled)
				{
					list.First<FloatMenuOption>().action();
					return true;
				}
				return false;
			}
		}

		// Token: 0x06010355 RID: 66389 RVA: 0x004B0139 File Offset: 0x004AE339
		public static IEnumerable<FloatMenuOption> GetOptionsForTile(PlanetTile tile, IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			bool anything = false;
			if (TransportersArrivalAction_FormCaravan.CanFormCaravanAt(pods, tile) && !Find.WorldObjects.AnySettlementBaseAt(tile) && !Find.WorldObjects.AnySiteAt(tile))
			{
				anything = true;
				yield return new FloatMenuOption("FormCaravanHere".Translate(), delegate()
				{
					launchAction(tile, new TransportersArrivalAction_FormCaravan("MessageShuttleArrived"));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			List<WorldObject> worldObjects = Find.WorldObjects.AllWorldObjects;
			int num;
			for (int i = 0; i < worldObjects.Count; i = num + 1)
			{
				if (!(worldObjects[i].Tile != tile))
				{
					foreach (FloatMenuOption floatMenuOption in worldObjects[i].GetShuttleFloatMenuOptions(pods, launchAction))
					{
						anything = true;
						yield return floatMenuOption;
					}
					IEnumerator<FloatMenuOption> enumerator = null;
				}
				num = i;
			}
			if (!anything && !Find.World.Impassable(tile))
			{
				yield return new FloatMenuOption("TransportPodsContentsWillBeLost".Translate(), delegate()
				{
					launchAction(tile, null);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
			yield break;
		}

		// Token: 0x06010356 RID: 66390 RVA: 0x004B0158 File Offset: 0x004AE358
		public static TaggedString TargetingLabelGetter(GlobalTargetInfo target, PlanetTile tile, int maxLaunchDistance, IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction, CompLaunchable launchable, float? overrideFuelLevel = null)
		{
			if (!target.IsValid)
			{
				return null;
			}
			if (target.Tile.Layer != tile.Layer && !tile.Layer.HasConnectionPathTo(target.Tile.Layer))
			{
				GUI.color = ColorLibrary.RedReadable;
				return "TransportPodDestinationNoPath".Translate(target.Tile.Layer.Def.Named("LAYER"));
			}
			if (ModsConfig.OdysseyActive)
			{
				WorldObject worldObject = Find.World.worldObjects.WorldObjectAt<WorldObject>(target.Tile);
				if (worldObject != null && worldObject.RequiresSignalJammerToReach)
				{
					GUI.color = ColorLibrary.RedReadable;
					return "TransportPodDestinationRequiresSignalJammer".Translate();
				}
			}
			int num = Find.WorldGrid.TraversalDistanceBetween(tile, target.Tile, true, maxLaunchDistance, true);
			if (maxLaunchDistance > 0 && num > maxLaunchDistance)
			{
				GUI.color = ColorLibrary.RedReadable;
				return "TransportPodDestinationBeyondMaximumRange".Translate();
			}
			bool flag = ((launchable != null) ? launchable.Shuttle : null) != null;
			string text = "";
			if (flag)
			{
				float num2 = overrideFuelLevel ?? launchable.FuelLevel;
				float num3 = launchable.FuelNeededToLaunchAtDist((float)num, target.Tile.Layer);
				text = string.Format("{0}: {1}", "Cost".Translate().CapitalizeFirst(), "FuelAmount".Translate(num3, ThingDefOf.Chemfuel));
				if (num3 > num2)
				{
					text = (text + string.Format(" ({0})", "TransportPodNotEnoughFuel".Translate())).Colorize(ColorLibrary.RedReadable);
				}
			}
			if (target.HasWorldObject && !target.WorldObject.def.validLaunchTarget)
			{
				return string.Empty;
			}
			IEnumerable<FloatMenuOption> source = (launchable != null) ? launchable.GetTransportersFloatMenuOptionsAt(target.Tile, launchAction, flag) : CompLaunchable.GetOptionsForTile(target.Tile, pods, launchAction);
			if (!source.Any<FloatMenuOption>())
			{
				return string.Empty;
			}
			if (source.Count<FloatMenuOption>() == 1)
			{
				if (source.First<FloatMenuOption>().Disabled)
				{
					GUI.color = ColorLibrary.RedReadable;
				}
				return source.First<FloatMenuOption>().Label + "\n" + text;
			}
			MapParent mapParent = target.WorldObject as MapParent;
			if (mapParent != null)
			{
				return "ClickToSeeAvailableOrders_WorldObject".Translate(mapParent.LabelCap) + "\n" + text;
			}
			return "ClickToSeeAvailableOrders_Empty".Translate() + "\n" + text;
		}

		// Token: 0x06010357 RID: 66391 RVA: 0x004B040F File Offset: 0x004AE60F
		public void Notify_Arrived()
		{
			this.lastLaunchTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0400A933 RID: 43315
		public int lastLaunchTick = -1;

		// Token: 0x0400A934 RID: 43316
		private CompTransporter cachedCompTransporter;

		// Token: 0x0400A935 RID: 43317
		private CompRefuelable cachedCompRefuelable;

		// Token: 0x0400A936 RID: 43318
		private CompShuttle cachedCompShuttle;

		// Token: 0x0400A937 RID: 43319
		public static readonly Texture2D TargeterMouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/LaunchableMouseAttachment", true);

		// Token: 0x0400A938 RID: 43320
		public static readonly Texture2D LaunchCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true);

		// Token: 0x0400A939 RID: 43321
		private PlanetTile cachedClosest;

		// Token: 0x0400A93A RID: 43322
		private PlanetTile cachedOrigin;

		// Token: 0x0400A93B RID: 43323
		private PlanetLayer cachedLayer;
	}
}
