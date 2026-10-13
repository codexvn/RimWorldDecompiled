using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002DA1 RID: 11681
	[StaticConstructorOnStartup]
	public class CompPlantable : ThingComp
	{
		// Token: 0x170029DB RID: 10715
		// (get) Token: 0x060106C0 RID: 67264 RVA: 0x004BD0E7 File Offset: 0x004BB2E7
		private static TargetingParameters TargetingParams
		{
			get
			{
				return new TargetingParameters
				{
					canTargetPawns = false,
					canTargetLocations = true
				};
			}
		}

		// Token: 0x170029DC RID: 10716
		// (get) Token: 0x060106C1 RID: 67265 RVA: 0x004BD0FC File Offset: 0x004BB2FC
		public CompProperties_Plantable Props
		{
			get
			{
				return (CompProperties_Plantable)this.props;
			}
		}

		// Token: 0x170029DD RID: 10717
		// (get) Token: 0x060106C2 RID: 67266 RVA: 0x004BD109 File Offset: 0x004BB309
		public List<IntVec3> PlantCells
		{
			get
			{
				return this.plantCells;
			}
		}

		// Token: 0x060106C3 RID: 67267 RVA: 0x004BD111 File Offset: 0x004BB311
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (ModsConfig.IdeologyActive || ModsConfig.BiotechActive || ModsConfig.AnomalyActive || ModsConfig.OdysseyActive)
			{
				if (this.PlantCells.Count > 0)
				{
					yield return new Command_Action
					{
						defaultLabel = "CancelPlantThing".Translate(this.Props.plantDefToSpawn),
						defaultDesc = "CancelPlantThingDesc".Translate(this.Props.plantDefToSpawn),
						icon = CompPlantable.CancelCommandTex,
						hotKey = KeyBindingDefOf.Designator_Cancel,
						action = delegate()
						{
							this.plantCells.Clear();
						}
					};
				}
				if (this.PlantCells.Count < this.parent.stackCount)
				{
					yield return new Command_Action
					{
						defaultLabel = "PlantThing".Translate(this.Props.plantDefToSpawn),
						defaultDesc = "PlantThingDesc".Translate(this.Props.plantDefToSpawn),
						icon = this.Props.plantDefToSpawn.uiIcon,
						action = new Action(this.BeginTargeting)
					};
				}
			}
			yield break;
		}

		// Token: 0x060106C4 RID: 67268 RVA: 0x004BD124 File Offset: 0x004BB324
		private void BeginTargeting()
		{
			Find.Targeter.BeginTargeting(CompPlantable.TargetingParams, delegate(LocalTargetInfo t)
			{
				if (!this.ValidateTarget(t))
				{
					this.BeginTargeting();
					return;
				}
				List<Thing> list;
				if (this.ConnectionStrengthReducedByNearbyBuilding(t.Cell, out list))
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("TreeBlockedByNearbyBuildingsDesc".Translate(this.Props.plantDefToSpawn.Named("PLANT")), delegate()
					{
						this.plantCells.Add(t.Cell);
					}, false, null, WindowLayer.Dialog));
					return;
				}
				this.plantCells.Add(t.Cell);
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}, delegate(LocalTargetInfo t)
			{
				if (this.CanPlantAt(t.Cell, this.parent.MapHeld).Accepted)
				{
					GenDraw.DrawTargetHighlight(t);
					this.DrawSurroundingsInfo(t.Cell);
				}
			}, (LocalTargetInfo t) => true, null, null, this.Props.plantDefToSpawn.uiIcon, false, null, null);
		}

		// Token: 0x060106C5 RID: 67269 RVA: 0x004BD18C File Offset: 0x004BB38C
		private bool ConnectionStrengthReducedByNearbyBuilding(IntVec3 cell, out List<Thing> buildings)
		{
			CompProperties_TreeConnection compProperties = this.Props.plantDefToSpawn.GetCompProperties<CompProperties_TreeConnection>();
			if (compProperties != null)
			{
				buildings = this.parent.MapHeld.listerArtificialBuildingsForMeditation.GetForCell(cell, compProperties.radiusToBuildingForConnectionStrengthLoss);
				if (buildings.Any<Thing>())
				{
					return true;
				}
			}
			buildings = null;
			return false;
		}

		// Token: 0x060106C6 RID: 67270 RVA: 0x004BD1DC File Offset: 0x004BB3DC
		private void DrawSurroundingsInfo(IntVec3 cell)
		{
			CompProperties_TreeConnection compProperties = this.Props.plantDefToSpawn.GetCompProperties<CompProperties_TreeConnection>();
			if (compProperties != null)
			{
				Color color = Color.white;
				List<Thing> list;
				if (this.ConnectionStrengthReducedByNearbyBuilding(cell, out list))
				{
					color = Color.red;
					foreach (Thing t in list)
					{
						GenDraw.DrawLineBetween(cell.ToVector3ShiftedWithAltitude(AltitudeLayer.Blueprint), t.TrueCenter(), SimpleColor.Red, 0.2f);
					}
				}
				color.a = 0.5f;
				GenDraw.DrawRadiusRing(cell, compProperties.radiusToBuildingForConnectionStrengthLoss, color, null);
			}
			for (int i = 0; i < this.Props.plantDefToSpawn.comps.Count; i++)
			{
				IPlantEffectRadius plantEffectRadius = this.Props.plantDefToSpawn.comps[i] as IPlantEffectRadius;
				if (plantEffectRadius != null)
				{
					Color white = Color.white;
					white.a = 0.5f;
					GenDraw.DrawRadiusRing(cell, plantEffectRadius.PlantEffectRadius, white, null);
					return;
				}
			}
		}

		// Token: 0x060106C7 RID: 67271 RVA: 0x004BD2F0 File Offset: 0x004BB4F0
		public override void PostDrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.plantCells.Count; i++)
			{
				if (this.plantCells[i].IsValid)
				{
					IntVec3 intVec = this.plantCells[i];
					GenDraw.DrawLineBetween(this.parent.PositionHeld.ToVector3Shifted(), intVec.ToVector3Shifted(), AltitudeLayer.Blueprint.AltitudeFor());
					GhostDrawer.DrawGhostThing(intVec, Rot4.North, this.Props.plantDefToSpawn, this.Props.plantDefToSpawn.graphic, Color.white, AltitudeLayer.Blueprint, null, true, null);
					this.DrawSurroundingsInfo(intVec);
				}
			}
		}

		// Token: 0x060106C8 RID: 67272 RVA: 0x004BD39C File Offset: 0x004BB59C
		public void DoPlant(Pawn planter, IntVec3 cell, Map map)
		{
			Plant plant = (Plant)ThingMaker.MakeThing(this.Props.plantDefToSpawn, null);
			if (GenPlace.TryPlaceThing(plant, cell, map, ThingPlaceMode.Direct, null, null, null, 1))
			{
				planter.records.Increment(RecordDefOf.PlantsSown);
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedPlant, planter.Named(HistoryEventArgsNames.Doer)), true);
				if (this.Props.plantDefToSpawn.plant.humanFoodPlant)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.SowedHumanFoodPlant, planter.Named(HistoryEventArgsNames.Doer)), true);
				}
				if (ModsConfig.IdeologyActive && plant.def == ThingDefOf.Plant_TreeGauranlen)
				{
					EffecterDefOf.GauranlenLeavesBatch.Spawn(cell, map, 1f).Cleanup();
				}
				plant.Growth = 0.0001f;
				plant.sown = true;
				map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.Things);
				this.parent.Destroy(DestroyMode.Vanish);
				if (ModsConfig.IdeologyActive && plant.def == ThingDefOf.Plant_TreeGauranlen)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.PlantedGauranlenTree, planter.Named(HistoryEventArgsNames.Doer)), true);
				}
			}
		}

		// Token: 0x060106C9 RID: 67273 RVA: 0x004BD4D3 File Offset: 0x004BB6D3
		public void Notify_SeedRemovedFromStackForPlantingAt(IntVec3 cell)
		{
			if (this.plantCells.Contains(cell))
			{
				this.plantCells.Remove(cell);
			}
		}

		// Token: 0x060106CA RID: 67274 RVA: 0x004BD4F0 File Offset: 0x004BB6F0
		public void Notify_RemovedFromStackForPlantingAt(IntVec3 newPlantCell)
		{
			this.plantCells.Clear();
			if (newPlantCell.IsValid)
			{
				this.plantCells.Add(newPlantCell);
			}
		}

		// Token: 0x060106CB RID: 67275 RVA: 0x004BD514 File Offset: 0x004BB714
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			CompPlantable compPlantable = otherStack.TryGetComp<CompPlantable>();
			if (compPlantable == null)
			{
				return;
			}
			List<IntVec3> list = compPlantable.PlantCells;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsValid && !this.plantCells.Contains(list[i]))
				{
					this.plantCells.Add(list[i]);
				}
			}
		}

		// Token: 0x060106CC RID: 67276 RVA: 0x004BD57C File Offset: 0x004BB77C
		public override void PostSplitOff(Thing piece)
		{
			CompPlantable compPlantable = piece.TryGetComp<CompPlantable>();
			if (compPlantable == null)
			{
				return;
			}
			List<IntVec3> list = this.plantCells;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsValid && !compPlantable.plantCells.Contains(list[i]))
				{
					compPlantable.plantCells.Add(list[i]);
				}
			}
		}

		// Token: 0x060106CD RID: 67277 RVA: 0x004BD5E4 File Offset: 0x004BB7E4
		public AcceptanceReport CanPlantAt(IntVec3 cell, Map map)
		{
			if (!cell.IsValid || cell.Fogged(map))
			{
				return false;
			}
			Thing thing;
			AcceptanceReport acceptanceReport = this.Props.plantDefToSpawn.CanEverPlantAt(cell, map, out thing, true, false, false);
			if (!acceptanceReport.Accepted)
			{
				return "CannotPlantThing".Translate(this.parent) + ": " + acceptanceReport.Reason.CapitalizeFirst();
			}
			TerrainDef terrain = cell.GetTerrain(map);
			if (terrain.IsFloor && terrain.fertility <= 0f)
			{
				return "CannotPlantMissingTerrainTag".Translate();
			}
			if (this.Props.plantDefToSpawn.plant.interferesWithRoof && cell.Roofed(map))
			{
				return "CannotPlantThing".Translate(this.parent) + ": " + "BlockedByRoof".Translate().CapitalizeFirst();
			}
			Thing thing2 = PlantUtility.AdjacentSowBlocker(this.Props.plantDefToSpawn, cell, map);
			if (thing2 != null)
			{
				return "CannotPlantThing".Translate(this.parent) + ": " + "AdjacentSowBlocker".Translate(thing2);
			}
			if (this.Props.plantDefToSpawn.plant.minSpacingBetweenSamePlant > 0f)
			{
				foreach (Thing thing3 in map.listerThings.ThingsOfDef(this.Props.plantDefToSpawn))
				{
					if (thing3.Position.InHorDistOf(cell, this.Props.plantDefToSpawn.plant.minSpacingBetweenSamePlant))
					{
						return "CannotPlantThing".Translate(this.parent) + ": " + "TooCloseToOtherPlant".Translate(thing3);
					}
				}
				foreach (Thing thing4 in map.listerThings.ThingsOfDef(this.parent.def))
				{
					CompPlantable compPlantable = thing4.TryGetComp<CompPlantable>();
					if (compPlantable != null && !compPlantable.PlantCells.NullOrEmpty<IntVec3>())
					{
						for (int i = 0; i < compPlantable.PlantCells.Count; i++)
						{
							if (compPlantable.PlantCells[i].InHorDistOf(cell, this.Props.plantDefToSpawn.plant.minSpacingBetweenSamePlant))
							{
								return "CannotPlantThing".Translate(this.parent) + ": " + "TooCloseToOtherSeedPlantCell".Translate(thing4.GetCustomLabelNoCount(false));
							}
						}
					}
				}
			}
			List<Thing> list = map.thingGrid.ThingsListAt(cell);
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j] is Building_PlantGrower)
				{
					return "CannotPlantThing".Translate(this.parent) + ": " + "BlockedBy".Translate(list[j]).CapitalizeFirst();
				}
			}
			return true;
		}

		// Token: 0x060106CE RID: 67278 RVA: 0x004BD9A0 File Offset: 0x004BBBA0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.plantCells, "plantCells", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.plantCells == null)
			{
				this.plantCells = new List<IntVec3>();
			}
		}

		// Token: 0x060106CF RID: 67279 RVA: 0x004BD9DC File Offset: 0x004BBBDC
		public bool ValidateTarget(LocalTargetInfo target)
		{
			AcceptanceReport acceptanceReport = this.CanPlantAt(target.Cell, this.parent.MapHeld);
			if (!acceptanceReport.Accepted)
			{
				if (!acceptanceReport.Reason.NullOrEmpty())
				{
					Messages.Message(acceptanceReport.Reason, this.parent, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0400AB5B RID: 43867
		private List<IntVec3> plantCells = new List<IntVec3>();

		// Token: 0x0400AB5C RID: 43868
		private static readonly Texture2D CancelCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);
	}
}
