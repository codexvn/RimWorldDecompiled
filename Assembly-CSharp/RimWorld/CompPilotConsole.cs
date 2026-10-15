using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public class CompPilotConsole : CompGravshipFacility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass19_0
	{
		public Pawn selPawn;

		public CompPilotConsole _003C_003E4__this;

		internal void _003CCompFloatMenuOptions_003Eb__0()
		{
			selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.PilotConsole, _003C_003E4__this.parent), JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompFloatMenuOptions_003Ed__19 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		public CompPilotConsole _003C_003E4__this;

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
		public _003CCompFloatMenuOptions_003Ed__19(int _003C_003E1__state)
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
			CompPilotConsole compPilotConsole = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass19_0 CS_0024_003C_003E8__locals6 = new _003C_003Ec__DisplayClass19_0
				{
					selPawn = selPawn,
					_003C_003E4__this = _003C_003E4__this
				};
				AcceptanceReport acceptanceReport = compPilotConsole.CanUseNow();
				if (!acceptanceReport.Accepted)
				{
					_003C_003E2__current = new FloatMenuOption("CannotChooseNavigator".Translate() + ": " + acceptanceReport.Reason.CapitalizeFirst(), null);
					_003C_003E1__state = 1;
					return true;
				}
				if (CS_0024_003C_003E8__locals6.selPawn.skills == null || CS_0024_003C_003E8__locals6.selPawn.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled)
				{
					_003C_003E2__current = new FloatMenuOption("CannotChooseNavigator".Translate() + ": " + "IncapableOfCapacity".Translate(SkillDefOf.Intellectual.label).CapitalizeFirst(), null);
					_003C_003E1__state = 2;
					return true;
				}
				if (!CS_0024_003C_003E8__locals6.selPawn.CanReach(compPilotConsole.parent, PathEndMode.InteractionCell, Danger.Deadly))
				{
					_003C_003E2__current = new FloatMenuOption("CannotChooseNavigator".Translate() + ": " + "NoPath".Translate(), null);
					_003C_003E1__state = 3;
					return true;
				}
				if ((bool)compPilotConsole.ValidateNavigator(CS_0024_003C_003E8__locals6.selPawn))
				{
					_003C_003E2__current = new FloatMenuOption("PilotGravship".Translate(), delegate
					{
						CS_0024_003C_003E8__locals6.selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.PilotConsole, CS_0024_003C_003E8__locals6._003C_003E4__this.parent), JobTag.Misc);
					});
					_003C_003E1__state = 4;
					return true;
				}
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
			case 3:
				_003C_003E1__state = -1;
				break;
			case 4:
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
			_003CCompFloatMenuOptions_003Ed__19 _003CCompFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompFloatMenuOptions_003Ed__ = new _003CCompFloatMenuOptions_003Ed__19(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCompFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CCompFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__15 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompPilotConsole _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__15(int _003C_003E1__state)
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
			CompPilotConsole CS_0024_003C_003E8__locals7 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!CS_0024_003C_003E8__locals7.parent.Spawned || CS_0024_003C_003E8__locals7.engine == null)
				{
					return false;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "ViewRange".Translate(),
					defaultDesc = "ViewRangeDesc".Translate(),
					icon = ViewRangeTex.Texture,
					action = delegate
					{
						CS_0024_003C_003E8__locals7.StartChoosingDestination_NewTemp(launching: false);
					}
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (DebugSettings.ShowDevGizmos)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Launch instantly",
						action = delegate
						{
							CS_0024_003C_003E8__locals7.engine.launchInfo = new LaunchInfo
							{
								quality = 1f,
								doNegativeOutcome = false
							};
							CS_0024_003C_003E8__locals7.StartChoosingDestination_NewTemp();
						}
					};
					_003C_003E1__state = 2;
					return true;
				}
				break;
			case 2:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals7.engine.cooldownCompleteTick > GenTicks.TicksGame)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Reset cooldown",
						action = delegate
						{
							CS_0024_003C_003E8__locals7.engine.cooldownCompleteTick = -1;
						}
					};
					_003C_003E1__state = 3;
					return true;
				}
				break;
			case 3:
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__15(0)
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

	[Unsaved(false)]
	private CompBreakdownable breakdownableComp;

	public static readonly Material ThrusterRadiusMat = MaterialPool.MatFrom(GenDraw.OneSidedLineTexPath, ShaderDatabase.WorldOverlayTransparent, ColorLibrary.White, 3590);

	public static readonly Material FuelRadiusMat = MaterialPool.MatFrom(GenDraw.OneSidedLineTexPath, ShaderDatabase.WorldOverlayTransparent, ColorLibrary.Yellow, 3590);

	public static readonly Material ThrusterRadiusMatHighVis = MaterialPool.MatFrom(GenDraw.OneSidedLineOpaqueTexPath, ShaderDatabase.WorldOverlayAdditiveTwoSided, ColorLibrary.White, 3590);

	public static readonly Material FuelRadiusMatHighVis = MaterialPool.MatFrom(GenDraw.OneSidedLineOpaqueTexPath, ShaderDatabase.WorldOverlayAdditiveTwoSided, ColorLibrary.Yellow, 3590);

	private static readonly Texture2D TargeterMouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/LaunchableMouseAttachment");

	private static readonly StringBuilder cannotPlaceTileReason = new StringBuilder();

	private static readonly CachedTexture ViewRangeTex = new CachedTexture("UI/Commands/ViewRange");

	private CompBreakdownable Breakdownable => breakdownableComp ?? (breakdownableComp = parent.GetComp<CompBreakdownable>());

	private int GetMaxLaunchDistance(PlanetLayer layer)
	{
		return Mathf.FloorToInt(((float?)engine?.MaxLaunchDistance / layer.Def.rangeDistanceFactor).GetValueOrDefault());
	}

	public static Material GetThrusterRadiusMat(PlanetTile tile)
	{
		if (!tile.LayerDef.isSpace)
		{
			return ThrusterRadiusMat;
		}
		return ThrusterRadiusMatHighVis;
	}

	public static Material GetFuelRadiusMat(PlanetTile tile)
	{
		if (!tile.LayerDef.isSpace)
		{
			return FuelRadiusMat;
		}
		return FuelRadiusMatHighVis;
	}

	public void StartChoosingDestination()
	{
		StartChoosingDestination_NewTemp();
	}

	public void StartChoosingDestination_NewTemp(bool launching = true)
	{
		if (engine == null)
		{
			return;
		}
		if (launching && DebugSettings.skipGravshipTileSelection)
		{
			Find.GravshipController.InitiateTakeoff(engine, default(PlanetTile));
			return;
		}
		CameraJumper.TryJump(CameraJumper.GetWorldTarget(parent));
		Find.WorldSelector.ClearSelection();
		PlanetTile curTile = parent.Map.Tile;
		PlanetLayer curLayer = parent.Map.Tile.Layer;
		PlanetTile cachedClosestLayerTile = PlanetTile.Invalid;
		Find.TilePicker.StartTargeting_NewTemp(delegate(PlanetTile tile)
		{
			cannotPlaceTileReason.Clear();
			if (!GravshipUtility.TryGetPathFuelCost(curTile, tile, out var cost, out var distance, 10f, engine.FuelUseageFactor) && !DebugSettings.ignoreGravshipRange)
			{
				Messages.Message("CannotLaunchDestination".Translate(), MessageTypeDefOf.RejectInput, historical: false);
				return false;
			}
			if (!engine.HasSignalJammer && Find.WorldObjects.TryGetWorldObjectAt<MapParent>(tile, out var wo) && wo.RequiresSignalJammerToReach)
			{
				Messages.Message("TransportPodDestinationRequiresSignalJammer".Translate(), MessageTypeDefOf.RejectInput, historical: false);
				return false;
			}
			if (cost > engine.TotalFuel && !DebugSettings.ignoreGravshipRange)
			{
				Messages.Message("CannotLaunchNotEnoughFuel".Translate().CapitalizeFirst(), MessageTypeDefOf.RejectInput, historical: false);
				return false;
			}
			if (distance > GetMaxLaunchDistance(tile.Layer) && !DebugSettings.ignoreGravshipRange)
			{
				Messages.Message("TransportPodDestinationBeyondMaximumRange".Translate(), MessageTypeDefOf.RejectInput, historical: false);
				return false;
			}
			if (tile == parent.Tile && !parent.Map.listerThings.AnyThingWithDef(ThingDefOf.GravAnchor))
			{
				Messages.Message("CannotLandOnSameTile".Translate(), MessageTypeDefOf.RejectInput, historical: false);
				return false;
			}
			MapParent mapParent = Find.World.worldObjects.MapParentAt(tile);
			if (mapParent != null && mapParent.HasMap)
			{
				return true;
			}
			if (!TileFinder.IsValidTileForNewSettlement(tile, cannotPlaceTileReason, forGravship: true))
			{
				Messages.Message(cannotPlaceTileReason.ToString(), MessageTypeDefOf.RejectInput, historical: false);
				return false;
			}
			return true;
		}, delegate(PlanetTile tile)
		{
			if (launching)
			{
				SettlementProximityGoodwillUtility.CheckConfirmSettle(tile, delegate
				{
					WorldComponent_GravshipController.DestroyTreesAroundSubstructure(engine.Map, engine.ValidSubstructure);
					Find.World.renderer.wantedMode = WorldRenderMode.None;
					engine.ConsumeFuel(tile);
					Find.GravshipController.InitiateTakeoff(engine, tile);
					SoundDefOf.Gravship_Launch.PlayOneShotOnCamera();
				}, delegate
				{
					StartChoosingDestination_NewTemp();
				}, engine);
			}
		}, delegate
		{
			WorldObject singleSelectedObject = Find.WorldSelector.SingleSelectedObject;
			PlanetTile planetTile = GenWorld.MouseTile();
			PlanetTile planetTile2 = ((!planetTile.Valid && singleSelectedObject != null) ? singleSelectedObject.Tile : planetTile);
			Vector2 mousePosition = Event.current.mousePosition;
			GUI.DrawTexture(new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f), TargeterMouseAttachment);
			if (planetTile2.Valid)
			{
				bool flag = false;
				int maxLaunchDistance = GetMaxLaunchDistance(PlanetLayer.Selected);
				PlanetTile tileA = ((curTile.Layer == planetTile2.Layer) ? curTile : planetTile2.Layer.GetClosestTile_NewTemp(curTile));
				string text;
				float cost2;
				int distance2;
				if (Find.WorldGrid.ApproxDistanceInTiles(tileA, planetTile2) > (float)(maxLaunchDistance + 10))
				{
					text = string.Format(" ({0})", "TransportPodDestinationBeyondMaximumRange".Translate());
				}
				else if (GravshipUtility.TryGetPathFuelCost(curTile, planetTile2, out cost2, out distance2, 10f, engine.FuelUseageFactor))
				{
					flag = cost2 <= engine.TotalFuel && distance2 <= maxLaunchDistance;
					text = string.Format("{0}: {1}", "Cost".Translate().CapitalizeFirst(), "FuelAmount".Translate(cost2, ThingDefOf.Chemfuel));
					if (distance2 > maxLaunchDistance)
					{
						text += string.Format(" ({0})", "TransportPodDestinationBeyondMaximumRange".Translate());
					}
					else if (!flag)
					{
						text += string.Format(" ({0})", "CannotLaunchNotEnoughFuel".Translate().CapitalizeFirst());
					}
					else if (!engine.HasSignalJammer && singleSelectedObject is MapParent && singleSelectedObject.RequiresSignalJammerToReach)
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
					Widgets.WorldAttachedLabel(singleSelectedObject.DrawPos, text, 0f, 0f, flag ? Color.white : ColorLibrary.RedReadable);
				}
				else
				{
					Widgets.MouseAttachedLabel(text, 0f, 0f, flag ? Color.white : ColorLibrary.RedReadable);
				}
			}
		}, delegate
		{
			int maxLaunchDistance2 = GetMaxLaunchDistance(PlanetLayer.Selected);
			int num = GravshipUtility.MaxDistForFuel(engine.TotalFuel, curLayer, PlanetLayer.Selected, 10f, engine.FuelUseageFactor);
			PlanetTile planetTile3 = curTile;
			if (curTile.Layer != Find.WorldSelector.SelectedLayer)
			{
				if (cachedClosestLayerTile.Layer != Find.WorldSelector.SelectedLayer || !cachedClosestLayerTile.Valid)
				{
					cachedClosestLayerTile = Find.WorldSelector.SelectedLayer.GetClosestTile_NewTemp(curTile);
				}
				planetTile3 = cachedClosestLayerTile;
			}
			GenDraw.DrawWorldRadiusRing(planetTile3, maxLaunchDistance2, GetThrusterRadiusMat(planetTile3));
			if (num < maxLaunchDistance2)
			{
				GenDraw.DrawWorldRadiusRing(planetTile3, num, GetFuelRadiusMat(planetTile3));
			}
		}, allowEscape: true, delegate
		{
			CameraJumper.TryJump(parent, CameraJumper.MovementMode.Cut);
		}, "ChooseWhereToLand".Translate(), showRandomButton: false, selectTileBehindObject: true, hideFormCaravanGizmo: true, noTileChosenMessage: "MessageNoLandingSiteSelected".Translate(), showNextButton: launching, canCancel: true);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__15))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}

	public AcceptanceReport CanUseNow()
	{
		if (engine == null)
		{
			return new AcceptanceReport("CannotLaunchNoEngine".Translate().CapitalizeFirst());
		}
		if (Breakdownable.BrokenDown)
		{
			return new AcceptanceReport("BrokenDown".Translate().CapitalizeFirst());
		}
		if (!engine.ValidSubstructureAt(parent.InteractionCell))
		{
			return new AcceptanceReport("PilotConsoleInaccessible".Translate().CapitalizeFirst());
		}
		AcceptanceReport result = engine.CanLaunch(this);
		if (!result.Accepted)
		{
			return result;
		}
		return AcceptanceReport.WasAccepted;
	}

	public override string CompInspectStringExtra()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.CompInspectStringExtra());
		if (engine != null)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append("GravshipName".Translate().CapitalizeFirst()).Append(": ").Append(engine.RenamableLabel);
			stringBuilder.AppendInNewLine("GravshipRange".Translate().CapitalizeFirst()).Append(": ").Append(GetMaxLaunchDistance(parent.Map.Tile.Layer).ToString("F0"));
			foreach (CompGravshipFacility gravshipComponent in engine.GravshipComponents)
			{
				if (gravshipComponent is CompGravshipThruster { Blocked: not false })
				{
					stringBuilder.Append(" (").Append("ThrusterBlocked".Translate()).Append(")");
					break;
				}
			}
			stringBuilder.AppendInNewLine("StoredChemfuel".Translate().CapitalizeFirst()).Append(": ").Append(engine.TotalFuel.ToString("F0"))
				.Append(" / ")
				.Append(engine.MaxFuel.ToString("F0"));
			stringBuilder.AppendInNewLine("FuelConsumption".Translate().CapitalizeFirst()).Append(": ").Append("FuelPerTile".Translate(engine.FuelPerTile.ToString("0.#")).CapitalizeFirst());
			if (GenTicks.TicksGame < engine.cooldownCompleteTick)
			{
				stringBuilder.AppendInNewLine("CannotLaunchOnCooldown".Translate((engine.cooldownCompleteTick - GenTicks.TicksGame).ToStringTicksToPeriod()).CapitalizeFirst());
			}
		}
		return stringBuilder.ToString();
	}

	private AcceptanceReport ValidateNavigator(LocalTargetInfo target)
	{
		if (!(target.Thing is Pawn pawn))
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

	[IteratorStateMachine(typeof(_003CCompFloatMenuOptions_003Ed__19))]
	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompFloatMenuOptions_003Ed__19(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}
}
