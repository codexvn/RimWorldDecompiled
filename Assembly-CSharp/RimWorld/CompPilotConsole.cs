using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002EEF RID: 12015
	[StaticConstructorOnStartup]
	public class CompPilotConsole : CompGravshipFacility
	{
		// Token: 0x17002BD0 RID: 11216
		// (get) Token: 0x06010F6A RID: 69482 RVA: 0x004E1824 File Offset: 0x004DFA24
		private CompBreakdownable Breakdownable
		{
			get
			{
				CompBreakdownable result;
				if ((result = this.breakdownableComp) == null)
				{
					result = (this.breakdownableComp = this.parent.GetComp<CompBreakdownable>());
				}
				return result;
			}
		}

		// Token: 0x06010F6B RID: 69483 RVA: 0x004E1850 File Offset: 0x004DFA50
		private int GetMaxLaunchDistance(PlanetLayer layer)
		{
			Building_GravEngine engine = this.engine;
			int? num = (engine != null) ? new int?(engine.MaxLaunchDistance) : null;
			return Mathf.FloorToInt((((num != null) ? new float?((float)num.GetValueOrDefault()) : null) / layer.Def.rangeDistanceFactor).GetValueOrDefault());
		}

		// Token: 0x06010F6C RID: 69484 RVA: 0x004E18DD File Offset: 0x004DFADD
		public static Material GetThrusterRadiusMat(PlanetTile tile)
		{
			if (!tile.LayerDef.isSpace)
			{
				return CompPilotConsole.ThrusterRadiusMat;
			}
			return CompPilotConsole.ThrusterRadiusMatHighVis;
		}

		// Token: 0x06010F6D RID: 69485 RVA: 0x004E18F8 File Offset: 0x004DFAF8
		public static Material GetFuelRadiusMat(PlanetTile tile)
		{
			if (!tile.LayerDef.isSpace)
			{
				return CompPilotConsole.FuelRadiusMat;
			}
			return CompPilotConsole.FuelRadiusMatHighVis;
		}

		// Token: 0x06010F6E RID: 69486 RVA: 0x004E1914 File Offset: 0x004DFB14
		public void StartChoosingDestination()
		{
			if (this.engine == null)
			{
				return;
			}
			if (DebugSettings.skipGravshipTileSelection)
			{
				Find.GravshipController.InitiateTakeoff(this.engine, default(PlanetTile));
				return;
			}
			CameraJumper.TryJump(CameraJumper.GetWorldTarget(this.parent), CameraJumper.MovementMode.Pan);
			Find.WorldSelector.ClearSelection();
			PlanetTile curTile = this.parent.Map.Tile;
			PlanetLayer curLayer = this.parent.Map.Tile.Layer;
			PlanetTile cachedClosestLayerTile = PlanetTile.Invalid;
			Find.TilePicker.StartTargeting(delegate(PlanetTile tile)
			{
				CompPilotConsole.cannotPlaceTileReason.Clear();
				float num;
				int num2;
				if (!GravshipUtility.TryGetPathFuelCost(curTile, tile, out num, out num2, 10f, this.engine.FuelUseageFactor) && !DebugSettings.ignoreGravshipRange)
				{
					Messages.Message("CannotLaunchDestination".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				MapParent mapParent;
				if (!this.engine.HasSignalJammer && Find.WorldObjects.TryGetWorldObjectAt<MapParent>(tile, out mapParent) && mapParent.RequiresSignalJammerToReach)
				{
					Messages.Message("TransportPodDestinationRequiresSignalJammer".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				if (num > this.engine.TotalFuel && !DebugSettings.ignoreGravshipRange)
				{
					Messages.Message("CannotLaunchNotEnoughFuel".Translate().CapitalizeFirst(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				if (num2 > this.GetMaxLaunchDistance(tile.Layer) && !DebugSettings.ignoreGravshipRange)
				{
					Messages.Message("TransportPodDestinationBeyondMaximumRange".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				if (tile == this.parent.Tile && !this.parent.Map.listerThings.AnyThingWithDef(ThingDefOf.GravAnchor))
				{
					Messages.Message("CannotLandOnSameTile".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				MapParent mapParent2 = Find.World.worldObjects.MapParentAt(tile);
				if (mapParent2 != null && mapParent2.HasMap)
				{
					return true;
				}
				if (!TileFinder.IsValidTileForNewSettlement(tile, CompPilotConsole.cannotPlaceTileReason, true))
				{
					Messages.Message(CompPilotConsole.cannotPlaceTileReason.ToString(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				return true;
			}, delegate(PlanetTile tile)
			{
				SettlementProximityGoodwillUtility.CheckConfirmSettle(tile, delegate
				{
					WorldComponent_GravshipController.DestroyTreesAroundSubstructure(this.engine.Map, this.engine.ValidSubstructure, 2, null);
					Find.World.renderer.wantedMode = WorldRenderMode.None;
					this.engine.ConsumeFuel(tile);
					Find.GravshipController.InitiateTakeoff(this.engine, tile);
					SoundDefOf.Gravship_Launch.PlayOneShotOnCamera(null);
				}, new Action(this.StartChoosingDestination), this.engine);
			}, delegate
			{
				WorldObject singleSelectedObject = Find.WorldSelector.SingleSelectedObject;
				PlanetTile planetTile = GenWorld.MouseTile(false);
				PlanetTile planetTile2 = (!planetTile.Valid && singleSelectedObject != null) ? singleSelectedObject.Tile : planetTile;
				Vector2 mousePosition = Event.current.mousePosition;
				GUI.DrawTexture(new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f), CompPilotConsole.TargeterMouseAttachment);
				if (planetTile2.Valid)
				{
					bool flag = false;
					if (!TileFinder.IsValidTileForNewSettlement(planetTile2, null, true))
					{
						return;
					}
					float num;
					int num2;
					string text;
					if (GravshipUtility.TryGetPathFuelCost(curTile, planetTile2, out num, out num2, 10f, this.engine.FuelUseageFactor))
					{
						flag = (num <= this.engine.TotalFuel && num2 <= this.GetMaxLaunchDistance(PlanetLayer.Selected));
						text = string.Format("{0}: {1}", "Cost".Translate().CapitalizeFirst(), "FuelAmount".Translate(num, ThingDefOf.Chemfuel));
						if (num2 > this.GetMaxLaunchDistance(PlanetLayer.Selected))
						{
							text += string.Format(" ({0})", "TransportPodDestinationBeyondMaximumRange".Translate());
						}
						else if (!flag)
						{
							text += string.Format(" ({0})", "CannotLaunchNotEnoughFuel".Translate().CapitalizeFirst());
						}
						else if (!this.engine.HasSignalJammer && singleSelectedObject is MapParent && singleSelectedObject.RequiresSignalJammerToReach)
						{
							flag = false;
							text += string.Format(" ({0})", "TransportPodDestinationRequiresSignalJammer".Translate());
						}
					}
					else
					{
						text = "CannotLaunchDestination".Translate();
					}
					if (singleSelectedObject != null && !planetTile.Valid)
					{
						Widgets.WorldAttachedLabel(singleSelectedObject.DrawPos, text, 0f, 0f, new Color?(flag ? Color.white : ColorLibrary.RedReadable));
						return;
					}
					Widgets.MouseAttachedLabel(text, 0f, 0f, new Color?(flag ? Color.white : ColorLibrary.RedReadable));
				}
			}, delegate
			{
				int maxLaunchDistance = this.GetMaxLaunchDistance(PlanetLayer.Selected);
				int num = GravshipUtility.MaxDistForFuel(this.engine.TotalFuel, curLayer, PlanetLayer.Selected, 10f, this.engine.FuelUseageFactor);
				PlanetTile planetTile = curTile;
				if (curTile.Layer != Find.WorldSelector.SelectedLayer)
				{
					if (cachedClosestLayerTile.Layer != Find.WorldSelector.SelectedLayer || !cachedClosestLayerTile.Valid)
					{
						cachedClosestLayerTile = Find.WorldSelector.SelectedLayer.GetClosestTile(curTile);
					}
					planetTile = cachedClosestLayerTile;
				}
				GenDraw.DrawWorldRadiusRing(planetTile, maxLaunchDistance, CompPilotConsole.GetThrusterRadiusMat(planetTile));
				if (num < maxLaunchDistance)
				{
					GenDraw.DrawWorldRadiusRing(planetTile, num, CompPilotConsole.GetFuelRadiusMat(planetTile));
				}
			}, true, delegate
			{
				CameraJumper.TryJump(this.parent, CameraJumper.MovementMode.Cut);
			}, "ChooseWhereToLand".Translate(), false, true, true, true, "MessageNoLandingSiteSelected".Translate());
		}

		// Token: 0x06010F6F RID: 69487 RVA: 0x004E1A20 File Offset: 0x004DFC20
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!this.parent.Spawned)
			{
				yield break;
			}
			if (DebugSettings.ShowDevGizmos && this.engine != null)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Launch instantly",
					action = delegate()
					{
						this.engine.launchInfo = new LaunchInfo
						{
							quality = 1f,
							doNegativeOutcome = false
						};
						this.StartChoosingDestination();
					}
				};
				if (this.engine.cooldownCompleteTick > GenTicks.TicksGame)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Reset cooldown",
						action = delegate()
						{
							this.engine.cooldownCompleteTick = -1;
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x06010F70 RID: 69488 RVA: 0x004E1A30 File Offset: 0x004DFC30
		public AcceptanceReport CanUseNow()
		{
			if (this.engine == null)
			{
				return new AcceptanceReport("CannotLaunchNoEngine".Translate().CapitalizeFirst());
			}
			if (this.Breakdownable.BrokenDown)
			{
				return new AcceptanceReport("BrokenDown".Translate().CapitalizeFirst());
			}
			if (!this.engine.ValidSubstructureAt(this.parent.InteractionCell))
			{
				return new AcceptanceReport("PilotConsoleInaccessible".Translate().CapitalizeFirst());
			}
			AcceptanceReport result = this.engine.CanLaunch(this);
			if (!result.Accepted)
			{
				return result;
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06010F71 RID: 69489 RVA: 0x004E1AE0 File Offset: 0x004DFCE0
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompInspectStringExtra());
			if (this.engine != null)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("GravshipName".Translate().CapitalizeFirst()).Append(": ").Append(this.engine.RenamableLabel);
				stringBuilder.AppendInNewLine("GravshipRange".Translate().CapitalizeFirst()).Append(": ").Append(this.GetMaxLaunchDistance(this.parent.Map.Tile.Layer).ToString("F0"));
				foreach (CompGravshipFacility compGravshipFacility in this.engine.GravshipComponents)
				{
					CompGravshipThruster compGravshipThruster = compGravshipFacility as CompGravshipThruster;
					if (compGravshipThruster != null && compGravshipThruster.Blocked)
					{
						stringBuilder.Append(" (").Append("ThrusterBlocked".Translate()).Append(")");
						break;
					}
				}
				stringBuilder.AppendInNewLine("StoredChemfuel".Translate().CapitalizeFirst()).Append(": ").Append(this.engine.TotalFuel.ToString("F0")).Append(" / ").Append(this.engine.MaxFuel.ToString("F0"));
				stringBuilder.AppendInNewLine("FuelConsumption".Translate().CapitalizeFirst()).Append(": ").Append("FuelPerTile".Translate(this.engine.FuelPerTile.ToString("0.#")).CapitalizeFirst());
				if (GenTicks.TicksGame < this.engine.cooldownCompleteTick)
				{
					stringBuilder.AppendInNewLine("CannotLaunchOnCooldown".Translate((this.engine.cooldownCompleteTick - GenTicks.TicksGame).ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06010F72 RID: 69490 RVA: 0x004E1D58 File Offset: 0x004DFF58
		private AcceptanceReport ValidateNavigator(LocalTargetInfo target)
		{
			Pawn pawn = target.Thing as Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!pawn.IsColonistPlayerControlled)
			{
				return false;
			}
			if (pawn.Downed || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving))
			{
				return "Incapable".Translate();
			}
			if (pawn.skills == null || pawn.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled)
			{
				return "Incapable".Translate();
			}
			return true;
		}

		// Token: 0x06010F73 RID: 69491 RVA: 0x004E1DEF File Offset: 0x004DFFEF
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			AcceptanceReport acceptanceReport = this.CanUseNow();
			if (!acceptanceReport.Accepted)
			{
				yield return new FloatMenuOption("CannotChooseNavigator".Translate() + ": " + acceptanceReport.Reason.CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else if (selPawn.skills == null || selPawn.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled)
			{
				yield return new FloatMenuOption("CannotChooseNavigator".Translate() + ": " + "IncapableOfCapacity".Translate(SkillDefOf.Intellectual.label).CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else if (!selPawn.CanReach(this.parent, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				yield return new FloatMenuOption("CannotChooseNavigator".Translate() + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else if (this.ValidateNavigator(selPawn))
			{
				yield return new FloatMenuOption("PilotGravship".Translate(), delegate()
				{
					selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.PilotConsole, this.parent), new JobTag?(JobTag.Misc), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
		}

		// Token: 0x0400B070 RID: 45168
		[Unsaved(false)]
		private CompBreakdownable breakdownableComp;

		// Token: 0x0400B071 RID: 45169
		public static readonly Material ThrusterRadiusMat = MaterialPool.MatFrom(GenDraw.OneSidedLineTexPath, ShaderDatabase.WorldOverlayTransparent, ColorLibrary.White, 3590);

		// Token: 0x0400B072 RID: 45170
		public static readonly Material FuelRadiusMat = MaterialPool.MatFrom(GenDraw.OneSidedLineTexPath, ShaderDatabase.WorldOverlayTransparent, ColorLibrary.Yellow, 3590);

		// Token: 0x0400B073 RID: 45171
		public static readonly Material ThrusterRadiusMatHighVis = MaterialPool.MatFrom(GenDraw.OneSidedLineOpaqueTexPath, ShaderDatabase.WorldOverlayAdditiveTwoSided, ColorLibrary.White, 3590);

		// Token: 0x0400B074 RID: 45172
		public static readonly Material FuelRadiusMatHighVis = MaterialPool.MatFrom(GenDraw.OneSidedLineOpaqueTexPath, ShaderDatabase.WorldOverlayAdditiveTwoSided, ColorLibrary.Yellow, 3590);

		// Token: 0x0400B075 RID: 45173
		private static readonly Texture2D TargeterMouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/LaunchableMouseAttachment", true);

		// Token: 0x0400B076 RID: 45174
		private static readonly StringBuilder cannotPlaceTileReason = new StringBuilder();
	}
}
