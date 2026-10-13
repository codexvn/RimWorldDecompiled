using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020023DC RID: 9180
	[StaticConstructorOnStartup]
	public class RoyalTitlePermitWorker_CallShuttle : RoyalTitlePermitWorker_Targeted
	{
		// Token: 0x0600C708 RID: 50952 RVA: 0x0039FF4C File Offset: 0x0039E14C
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!base.CanHitTarget(target))
			{
				if (target.IsValid && showMessages)
				{
					Messages.Message(this.def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			AcceptanceReport acceptanceReport = RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(target, this.map, null, null);
			if (!acceptanceReport.Accepted)
			{
				Messages.Message(acceptanceReport.Reason, new LookTargets(target.Cell, this.map), MessageTypeDefOf.RejectInput, false);
			}
			return acceptanceReport.Accepted;
		}

		// Token: 0x0600C709 RID: 50953 RVA: 0x0039FA56 File Offset: 0x0039DC56
		public override void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(this.caller.Position, base.RangeClamped, Color.white, null);
			RoyalTitlePermitWorker_CallShuttle.DrawShuttleGhost(target, this.map, ThingDefOf.Shuttle, ThingDefOf.Shuttle.defaultPlacingRot);
		}

		// Token: 0x0600C70A RID: 50954 RVA: 0x0039FFEE File Offset: 0x0039E1EE
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallShuttle(target.Cell);
		}

		// Token: 0x0600C70B RID: 50955 RVA: 0x003A0000 File Offset: 0x0039E200
		public override void OnGUI(LocalTargetInfo target)
		{
			if (!target.IsValid || !RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(target, this.map, null, null).Accepted)
			{
				GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
			}
		}

		// Token: 0x0600C70C RID: 50956 RVA: 0x003A0040 File Offset: 0x0039E240
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			MapGeneratorDef generatorDef = map.generatorDef;
			if (generatorDef != null && generatorDef.isUnderground)
			{
				yield return new FloatMenuOption(this.def.LabelCap + ": " + "CommandCallRoyalAidMapUnreachable".Translate(faction.Named("FACTION")), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			if (faction.HostileTo(Faction.OfPlayer))
			{
				yield return new FloatMenuOption(this.def.LabelCap + ": " + "CommandCallRoyalAidFactionHostile".Translate(faction.Named("FACTION")), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			string label = this.def.LabelCap + ": ";
			Action action = null;
			bool free;
			if (base.FillAidOption(pawn, faction, ref label, out free))
			{
				action = delegate()
				{
					this.BeginCallShuttle(pawn, pawn.MapHeld, faction, free);
				};
			}
			yield return new FloatMenuOption(label, action, faction.def.FactionIcon, faction.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
			yield break;
		}

		// Token: 0x0600C70D RID: 50957 RVA: 0x003A0065 File Offset: 0x0039E265
		public override IEnumerable<Gizmo> GetCaravanGizmos(Pawn pawn, Faction faction)
		{
			string defaultDesc;
			bool flag;
			if (!base.FillCaravanAidOption(pawn, faction, out defaultDesc, out this.free, out flag))
			{
				yield break;
			}
			Command_Action command_Action = new Command_Action
			{
				defaultLabel = this.def.LabelCap + " (" + pawn.LabelShort + ")",
				defaultDesc = defaultDesc,
				icon = RoyalTitlePermitWorker_CallShuttle.CommandTex,
				action = delegate()
				{
					this.CallShuttleToCaravan(pawn, faction, this.free);
				}
			};
			if (pawn.MapHeld != null && pawn.MapHeld.generatorDef.isUnderground)
			{
				command_Action.Disable("CommandCallRoyalAidMapUnreachable".Translate(faction.Named("FACTION")));
			}
			if (faction.HostileTo(Faction.OfPlayer))
			{
				command_Action.Disable("CommandCallRoyalAidFactionHostile".Translate(faction.Named("FACTION")));
			}
			if (flag)
			{
				command_Action.Disable("CommandCallRoyalAidNotEnoughFavor".Translate());
			}
			yield return command_Action;
			yield break;
		}

		// Token: 0x0600C70E RID: 50958 RVA: 0x003A0084 File Offset: 0x0039E284
		private void BeginCallShuttle(Pawn caller, Map map, Faction faction, bool free)
		{
			this.targetingParameters = new TargetingParameters();
			this.targetingParameters.canTargetLocations = true;
			this.targetingParameters.canTargetSelf = false;
			this.targetingParameters.canTargetPawns = false;
			this.targetingParameters.canTargetFires = false;
			this.targetingParameters.canTargetBuildings = true;
			this.targetingParameters.canTargetItems = true;
			this.caller = caller;
			this.map = map;
			this.calledFaction = faction;
			this.free = free;
			float rangeActual = base.RangeClamped;
			this.targetingParameters.validator = ((TargetInfo target) => rangeActual <= 0f || target.Cell.DistanceTo(caller.Position) <= rangeActual);
			Find.Targeter.BeginTargeting(this, null, false, null, null, true);
		}

		// Token: 0x0600C70F RID: 50959 RVA: 0x003A0148 File Offset: 0x0039E348
		private void CallShuttle(IntVec3 landingCell)
		{
			if (this.caller.Spawned)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Shuttle, null);
				CompShuttle compShuttle = thing.TryGetComp<CompShuttle>();
				compShuttle.permitShuttle = true;
				compShuttle.acceptChildren = true;
				TransportShip transportShip = TransportShipMaker.MakeTransportShip(TransportShipDefOf.Ship_Shuttle, null, thing);
				transportShip.ArriveAt(landingCell, this.map.Parent);
				transportShip.AddJobs(new ShipJobDef[]
				{
					ShipJobDefOf.WaitForever,
					ShipJobDefOf.Unload_Destination,
					ShipJobDefOf.FlyAway
				});
				this.caller.royalty.GetPermit(this.def, this.calledFaction).Notify_Used();
				if (!this.free)
				{
					this.caller.royalty.TryRemoveFavor(this.calledFaction, this.def.royalAid.favorCost);
				}
			}
		}

		// Token: 0x0600C710 RID: 50960 RVA: 0x003A0218 File Offset: 0x0039E418
		private void CallShuttleToCaravan(Pawn caller, Faction faction, bool free)
		{
			RoyalTitlePermitWorker_CallShuttle.<>c__DisplayClass10_0 CS$<>8__locals1 = new RoyalTitlePermitWorker_CallShuttle.<>c__DisplayClass10_0();
			CS$<>8__locals1.caller = caller;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.faction = faction;
			CS$<>8__locals1.free = free;
			CS$<>8__locals1.caravan = CS$<>8__locals1.caller.GetCaravan();
			CS$<>8__locals1.maxLaunchDistance = TransportShipDefOf.Ship_Shuttle.maxLaunchDistance;
			CameraJumper.TryJump(CameraJumper.GetWorldTarget(CS$<>8__locals1.caravan), CameraJumper.MovementMode.Pan);
			Find.WorldSelector.ClearSelection();
			CS$<>8__locals1.caravanTile = CS$<>8__locals1.caravan.Tile;
			Find.WorldTargeter.BeginTargeting(new Func<GlobalTargetInfo, bool>(CS$<>8__locals1.<CallShuttleToCaravan>g__ChoseWorldTarget|1), true, CompLaunchable.TargeterMouseAttachment, false, delegate
			{
				if (CS$<>8__locals1.maxLaunchDistance > 0)
				{
					PlanetTile center = CS$<>8__locals1.caravanTile;
					if (CS$<>8__locals1.caravanTile.Layer != Find.WorldSelector.SelectedLayer)
					{
						center = Find.WorldSelector.SelectedLayer.GetClosestTile(CS$<>8__locals1.caravanTile);
					}
					GenDraw.DrawWorldRadiusRing(center, CS$<>8__locals1.maxLaunchDistance, null);
				}
			}, (GlobalTargetInfo target) => CompLaunchable.TargetingLabelGetter(target, CS$<>8__locals1.caravanTile, CS$<>8__locals1.maxLaunchDistance, Gen.YieldSingle<Caravan>(CS$<>8__locals1.caravan), new Action<PlanetTile, TransportersArrivalAction>(base.<CallShuttleToCaravan>g__Launch|0), null, null), null, null, false);
		}

		// Token: 0x0600C711 RID: 50961 RVA: 0x003A02DC File Offset: 0x0039E4DC
		public static void DrawShuttleGhost(LocalTargetInfo target, Map map, ThingDef shuttleDef, Rot4 rot)
		{
			Color ghostCol = RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(target, map, shuttleDef, new Rot4?(rot)).Accepted ? Designator_Place.CanPlaceColor : Designator_Place.CannotPlaceColor;
			GhostDrawer.DrawGhostThing(target.Cell, rot, shuttleDef, shuttleDef.graphic, ghostCol, AltitudeLayer.Blueprint, null, true, null);
			Vector3 position = ThingUtility.InteractionCellWhenAt(shuttleDef, target.Cell, rot, map).ToVector3ShiftedWithAltitude(AltitudeLayer.Blueprint);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenDraw.InteractionCellMaterial, 0);
		}

		// Token: 0x0600C712 RID: 50962 RVA: 0x003A0358 File Offset: 0x0039E558
		public static AcceptanceReport ShuttleCanLandHere(LocalTargetInfo target, Map map, ThingDef shuttleDef = null, Rot4? rot = null)
		{
			TaggedString t = "CannotCallShuttle".Translate() + ": ";
			if (!target.IsValid)
			{
				return new AcceptanceReport(t + "MessageTransportPodsDestinationIsInvalid".Translate().CapitalizeFirst());
			}
			if (shuttleDef == null)
			{
				shuttleDef = ThingDefOf.Shuttle;
			}
			Rot4 value = rot.GetValueOrDefault();
			if (rot == null)
			{
				value = shuttleDef.defaultPlacingRot;
				rot = new Rot4?(value);
			}
			foreach (IntVec3 cell in GenAdj.OccupiedRect(target.Cell, rot.Value, shuttleDef.size))
			{
				string reportFromCell = RoyalTitlePermitWorker_CallShuttle.GetReportFromCell(cell, map, false, shuttleDef);
				if (reportFromCell != null)
				{
					return new AcceptanceReport(t + reportFromCell);
				}
			}
			string reportFromCell2 = RoyalTitlePermitWorker_CallShuttle.GetReportFromCell(ThingUtility.InteractionCellWhenAt(shuttleDef, target.Cell, rot.Value, map), map, true, shuttleDef);
			if (reportFromCell2 != null)
			{
				return new AcceptanceReport(t + reportFromCell2);
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600C713 RID: 50963 RVA: 0x003A0488 File Offset: 0x0039E688
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
				if (!interactionSpot && thing.def.category == ThingCategory.Item)
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

		// Token: 0x040087A5 RID: 34725
		private Faction calledFaction;

		// Token: 0x040087A6 RID: 34726
		private static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/CallShuttle", true);
	}
}
