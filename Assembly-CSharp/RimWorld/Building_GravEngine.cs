using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002A13 RID: 10771
	[StaticConstructorOnStartup]
	public class Building_GravEngine : Building, IRenameable
	{
		// Token: 0x170024EF RID: 9455
		// (get) Token: 0x0600EE5B RID: 61019 RVA: 0x004528DB File Offset: 0x00450ADB
		private static Graphic OnCooldownGraphic
		{
			get
			{
				Graphic result;
				if ((result = Building_GravEngine.onCooldownGraphic) == null)
				{
					result = (Building_GravEngine.onCooldownGraphic = GraphicDatabase.Get<Graphic_Single>("Things/Building/GravEngine/GravEngine_Cooldown", ShaderDatabase.Cutout, Vector2.one * 3f, Color.white));
				}
				return result;
			}
		}

		// Token: 0x170024F0 RID: 9456
		// (get) Token: 0x0600EE5C RID: 61020 RVA: 0x0045290F File Offset: 0x00450B0F
		// (set) Token: 0x0600EE5D RID: 61021 RVA: 0x00452921 File Offset: 0x00450B21
		public string RenamableLabel
		{
			get
			{
				return this.gravshipName ?? this.BaseLabel;
			}
			set
			{
				this.gravshipName = value;
				this.nameHidden = false;
			}
		}

		// Token: 0x170024F1 RID: 9457
		// (get) Token: 0x0600EE5E RID: 61022 RVA: 0x00439709 File Offset: 0x00437909
		public string BaseLabel
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x170024F2 RID: 9458
		// (get) Token: 0x0600EE5F RID: 61023 RVA: 0x00452931 File Offset: 0x00450B31
		public string InspectLabel
		{
			get
			{
				return this.BaseLabel;
			}
		}

		// Token: 0x170024F3 RID: 9459
		// (get) Token: 0x0600EE60 RID: 61024 RVA: 0x00452939 File Offset: 0x00450B39
		public override Graphic Graphic
		{
			get
			{
				if (Find.TickManager.TicksGame >= this.cooldownCompleteTick)
				{
					return base.Graphic;
				}
				return Building_GravEngine.OnCooldownGraphic;
			}
		}

		// Token: 0x170024F4 RID: 9460
		// (get) Token: 0x0600EE61 RID: 61025 RVA: 0x0045295C File Offset: 0x00450B5C
		public CompAffectedByFacilities AffectedByFacilities
		{
			get
			{
				CompAffectedByFacilities result;
				if ((result = this.affectedByFacilities) == null)
				{
					result = (this.affectedByFacilities = base.GetComp<CompAffectedByFacilities>());
				}
				return result;
			}
		}

		// Token: 0x170024F5 RID: 9461
		// (get) Token: 0x0600EE62 RID: 61026 RVA: 0x00452982 File Offset: 0x00450B82
		public int MaxLaunchDistance
		{
			get
			{
				return (int)this.GetStatValue(StatDefOf.GravshipRange, true, -1);
			}
		}

		// Token: 0x170024F6 RID: 9462
		// (get) Token: 0x0600EE63 RID: 61027 RVA: 0x00452992 File Offset: 0x00450B92
		public HashSet<IntVec3> ValidSubstructure
		{
			get
			{
				this.UpdateSubstructureIfNeeded(true);
				return this.validSubstructure;
			}
		}

		// Token: 0x170024F7 RID: 9463
		// (get) Token: 0x0600EE64 RID: 61028 RVA: 0x004529A1 File Offset: 0x00450BA1
		public HashSet<IntVec3> AllConnectedSubstructure
		{
			get
			{
				this.UpdateSubstructureIfNeeded(true);
				return this.allConnectedSubstructure;
			}
		}

		// Token: 0x170024F8 RID: 9464
		// (get) Token: 0x0600EE65 RID: 61029 RVA: 0x004529B0 File Offset: 0x00450BB0
		public HashSet<IntVec3> ValidSubstructureNoRegen
		{
			get
			{
				this.UpdateSubstructureIfNeeded(false);
				return this.validSubstructure;
			}
		}

		// Token: 0x170024F9 RID: 9465
		// (get) Token: 0x0600EE66 RID: 61030 RVA: 0x004529BF File Offset: 0x00450BBF
		public HashSet<IntVec3> AllConnectedSubstructureNoRegen
		{
			get
			{
				this.UpdateSubstructureIfNeeded(false);
				return this.allConnectedSubstructure;
			}
		}

		// Token: 0x170024FA RID: 9466
		// (get) Token: 0x0600EE67 RID: 61031 RVA: 0x004529D0 File Offset: 0x00450BD0
		public List<CompGravshipFacility> GravshipComponents
		{
			get
			{
				if (this.gravshipComponentsDirty)
				{
					this.gravshipComponentsDirty = false;
					this.gravshipComponents.Clear();
					using (List<Thing>.Enumerator enumerator = this.AffectedByFacilities.LinkedFacilitiesListForReading.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							CompGravshipFacility item;
							if (enumerator.Current.TryGetComp(out item))
							{
								this.gravshipComponents.Add(item);
							}
						}
					}
				}
				return this.gravshipComponents;
			}
		}

		// Token: 0x170024FB RID: 9467
		// (get) Token: 0x0600EE68 RID: 61032 RVA: 0x00452A58 File Offset: 0x00450C58
		public float TotalFuel
		{
			get
			{
				float num = 0f;
				foreach (CompGravshipFacility compGravshipFacility in this.GravshipComponents)
				{
					CompRefuelable compRefuelable;
					if (compGravshipFacility.parent.Spawned && compGravshipFacility.Props.providesFuel && compGravshipFacility.CanBeActive && compGravshipFacility.parent.TryGetComp(out compRefuelable))
					{
						num += compRefuelable.Fuel;
					}
				}
				return num;
			}
		}

		// Token: 0x170024FC RID: 9468
		// (get) Token: 0x0600EE69 RID: 61033 RVA: 0x00452AE8 File Offset: 0x00450CE8
		public float MaxFuel
		{
			get
			{
				float num = 0f;
				foreach (CompGravshipFacility compGravshipFacility in this.GravshipComponents)
				{
					CompRefuelable compRefuelable;
					if (compGravshipFacility.parent.Spawned && compGravshipFacility.Props.providesFuel && compGravshipFacility.CanBeActive && compGravshipFacility.parent.TryGetComp(out compRefuelable))
					{
						num += compRefuelable.Props.fuelCapacity;
					}
				}
				return num;
			}
		}

		// Token: 0x170024FD RID: 9469
		// (get) Token: 0x0600EE6A RID: 61034 RVA: 0x00452B7C File Offset: 0x00450D7C
		public float FuelSavingsPercent
		{
			get
			{
				float num = 0f;
				foreach (CompGravshipFacility compGravshipFacility in this.GravshipComponents)
				{
					if (compGravshipFacility.parent.Spawned && compGravshipFacility.Props.fuelSavingsPercent > 0f && compGravshipFacility.CanBeActive)
					{
						num += compGravshipFacility.Props.fuelSavingsPercent;
					}
				}
				return Mathf.Clamp01(num);
			}
		}

		// Token: 0x170024FE RID: 9470
		// (get) Token: 0x0600EE6B RID: 61035 RVA: 0x00452C0C File Offset: 0x00450E0C
		public float FuelPerTile
		{
			get
			{
				return 10f * this.FuelUseageFactor;
			}
		}

		// Token: 0x170024FF RID: 9471
		// (get) Token: 0x0600EE6C RID: 61036 RVA: 0x00452C1A File Offset: 0x00450E1A
		public float FuelUseageFactor
		{
			get
			{
				return 1f - this.FuelSavingsPercent;
			}
		}

		// Token: 0x17002500 RID: 9472
		// (get) Token: 0x0600EE6D RID: 61037 RVA: 0x00452C28 File Offset: 0x00450E28
		public List<GravshipComponentTypeDef> MissingComponents
		{
			get
			{
				if (this.missingComponentsDirty)
				{
					this.missingComponentsDirty = false;
					this.missingComponents.Clear();
					using (List<GravshipComponentTypeDef>.Enumerator enumerator = DefDatabase<GravshipComponentTypeDef>.AllDefsListForReading.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GravshipComponentTypeDef def = enumerator.Current;
							if (def.requiredForLaunch && this.GravshipComponents.Find((CompGravshipFacility c) => c.Props.componentTypeDef == def) == null)
							{
								this.missingComponents.Add(def);
							}
						}
					}
				}
				return this.missingComponents;
			}
		}

		// Token: 0x17002501 RID: 9473
		// (get) Token: 0x0600EE6E RID: 61038 RVA: 0x00452CD8 File Offset: 0x00450ED8
		public bool HasSignalJammer
		{
			get
			{
				return this.GravshipComponents.Any((CompGravshipFacility c) => c.Props.componentTypeDef == GravshipComponentTypeDefOf.SignalJammer);
			}
		}

		// Token: 0x0600EE6F RID: 61039 RVA: 0x00452D04 File Offset: 0x00450F04
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			this.ForceSubstructureDirty();
		}

		// Token: 0x0600EE70 RID: 61040 RVA: 0x00452D1C File Offset: 0x00450F1C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.missingComponents.Clear();
			this.ForceSubstructureDirty();
		}

		// Token: 0x0600EE71 RID: 61041 RVA: 0x00452D36 File Offset: 0x00450F36
		public override void PostSwapMap()
		{
			base.PostSwapMap();
			this.ForceSubstructureDirty();
		}

		// Token: 0x0600EE72 RID: 61042 RVA: 0x00452D44 File Offset: 0x00450F44
		protected override void Tick()
		{
			base.Tick();
			this.hasTickedThisSession = true;
			if (this.silentlyActivate)
			{
				this.Inspect(true);
				this.silentlyActivate = false;
			}
			if (Find.TickManager.TicksGame == this.cooldownCompleteTick)
			{
				Messages.Message("GravshipReadyToLaunch".Translate(this.RenamableLabel), this, MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x0600EE73 RID: 61043 RVA: 0x00452DB4 File Offset: 0x00450FB4
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (base.Spawned)
			{
				if (Find.TickManager.TicksGame >= this.cooldownCompleteTick)
				{
					drawLoc.z += 0.5f * (1f + Mathf.Sin(6.2831855f * (float)GenTicks.TicksGame / 500f)) * 0.3f;
				}
				drawLoc.y += 0.03658537f;
				Vector3 s = new Vector3(this.def.graphicData.drawSize.x, 1f, this.def.graphicData.drawSize.y);
				Graphics.DrawMesh(MeshPool.plane10Back, Matrix4x4.TRS(drawLoc, base.Rotation.AsQuat, s), Building_GravEngine.OrbMat.Material, 0, null, 0);
			}
		}

		// Token: 0x0600EE74 RID: 61044 RVA: 0x00452E8C File Offset: 0x0045108C
		public void AddComponent(CompGravshipFacility comp)
		{
			this.ForceSubstructureDirty();
		}

		// Token: 0x0600EE75 RID: 61045 RVA: 0x00452E8C File Offset: 0x0045108C
		public void RemoveComponent(CompGravshipFacility comp)
		{
			this.ForceSubstructureDirty();
		}

		// Token: 0x0600EE76 RID: 61046 RVA: 0x00452E94 File Offset: 0x00451094
		public AcceptanceReport CanLaunch(CompPilotConsole console)
		{
			Map map = base.Map;
			bool flag;
			if (map == null)
			{
				flag = false;
			}
			else
			{
				MapGeneratorDef generatorDef = map.generatorDef;
				bool? flag2;
				if (generatorDef == null)
				{
					flag2 = null;
				}
				else
				{
					PocketMapProperties pocketMapProperties = generatorDef.pocketMapProperties;
					flag2 = ((pocketMapProperties != null) ? new bool?(pocketMapProperties.canLaunchGravship) : null);
				}
				bool? flag3 = flag2;
				bool flag4 = false;
				flag = (flag3.GetValueOrDefault() == flag4 & flag3 != null);
			}
			if (flag)
			{
				return new AcceptanceReport(base.Map.generatorDef.label);
			}
			if (this.ValidSubstructure.Count == 0)
			{
				return new AcceptanceReport("CannotLaunchNoSubstructure".Translate().CapitalizeFirst());
			}
			if (!console.CanBeActive)
			{
				return new AcceptanceReport("CannotLaunchDisconnected".Translate().CapitalizeFirst());
			}
			if (this.TotalFuel < 10f)
			{
				return new AcceptanceReport("CannotLaunchNotEnoughFuel".Translate().CapitalizeFirst());
			}
			if (this.MaxLaunchDistance <= 0)
			{
				return new AcceptanceReport("CannotLaunchNoThrusters".Translate().CapitalizeFirst());
			}
			if (GenTicks.TicksGame < this.cooldownCompleteTick)
			{
				return new AcceptanceReport("CannotLaunchOnCooldown".Translate((this.cooldownCompleteTick - GenTicks.TicksGame).ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst());
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600EE77 RID: 61047 RVA: 0x00452FF8 File Offset: 0x004511F8
		public void ConsumeFuel(PlanetTile tile)
		{
			float num;
			int num2;
			if (!GravshipUtility.TryGetPathFuelCost(base.Map.Tile, tile, out num, out num2, 10f, 1f))
			{
				Log.Error(string.Format("Failed to get the fuel cost from tile ({0}) to {1}.", base.Map.Tile, tile));
				return;
			}
			float num3 = num / this.TotalFuel;
			foreach (CompGravshipFacility compGravshipFacility in this.GravshipComponents)
			{
				if (compGravshipFacility.CanBeActive && compGravshipFacility.Props.providesFuel)
				{
					CompRefuelable comp = compGravshipFacility.parent.GetComp<CompRefuelable>();
					if (comp != null)
					{
						comp.ConsumeFuel(comp.Fuel * num3);
					}
				}
			}
			int ticksGame = GenTicks.TicksGame;
			LaunchInfo launchInfo = this.launchInfo;
			this.cooldownCompleteTick = ticksGame + (int)GravshipUtility.LaunchCooldownFromQuality((launchInfo != null) ? launchInfo.quality : 1f);
		}

		// Token: 0x0600EE78 RID: 61048 RVA: 0x004530F8 File Offset: 0x004512F8
		public bool ValidSubstructureAt(IntVec3 cell)
		{
			return this.ValidSubstructure.Contains(cell);
		}

		// Token: 0x0600EE79 RID: 61049 RVA: 0x00453108 File Offset: 0x00451308
		public bool OnValidSubstructure(Thing thing)
		{
			if (!thing.Spawned)
			{
				return false;
			}
			BuildingProperties building = thing.def.building;
			if (building != null && building.isAttachment)
			{
				Thing wallAttachedTo = GenConstruct.GetWallAttachedTo(thing);
				return this.ValidSubstructureAt(wallAttachedTo.Position);
			}
			foreach (IntVec3 cell in thing.OccupiedRect())
			{
				if (!this.ValidSubstructureAt(cell))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600EE7A RID: 61050 RVA: 0x004531A0 File Offset: 0x004513A0
		public bool LooselyConnectedToGravEngine(Thing thing)
		{
			if (!thing.Spawned)
			{
				return false;
			}
			foreach (IntVec3 item in thing.OccupiedRect())
			{
				if (!this.AllConnectedSubstructure.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600EE7B RID: 61051 RVA: 0x00453210 File Offset: 0x00451410
		public void Inspect(bool silent = false)
		{
			if (Find.ResearchManager.gravEngineInspected)
			{
				return;
			}
			Find.ResearchManager.gravEngineInspected = true;
			QuestUtility.SendQuestTargetSignals(this.questTags, "Inspected", this.Named("SUBJECT"));
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Gravship, OpportunityType.Important);
			if (base.Faction != Faction.OfPlayer)
			{
				this.SetFaction(Faction.OfPlayer, null);
			}
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.GravEngine, StorytellerUtility.DefaultThreatPointsNow(base.Map));
			if (!silent)
			{
				DiaNode diaNode = new DiaNode("GravEngineInspectedLetterContents".Translate());
				DiaOption diaOption = new DiaOption("ViewQuest".Translate());
				diaOption.resolveTree = true;
				diaOption.action = delegate()
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
					((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(quest);
				};
				diaNode.options.Add(diaOption);
				diaNode.options.Add(new DiaOption("Close".Translate())
				{
					resolveTree = true
				});
				Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(diaNode, false, false, null);
				dialog_NodeTree.forcePause = true;
				Find.WindowStack.Add(dialog_NodeTree);
			}
		}

		// Token: 0x0600EE7C RID: 61052 RVA: 0x00453325 File Offset: 0x00451525
		public void ForceSubstructureDirty()
		{
			this.substructureDirty = true;
			this.missingComponentsDirty = true;
			this.gravshipComponentsDirty = true;
		}

		// Token: 0x0600EE7D RID: 61053 RVA: 0x0045333C File Offset: 0x0045153C
		private void UpdateSubstructureIfNeeded(bool regenerateSectionLayers)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			if (base.Destroyed || !base.Spawned)
			{
				return;
			}
			if (this.substructureDirty)
			{
				this.substructureDirty = false;
				GravshipUtility.GetConnectedSubstructure(this, this.allConnectedSubstructure, int.MaxValue, false);
				GravshipUtility.GetConnectedSubstructure(this, this.validSubstructure, (int)this.GetStatValue(StatDefOf.SubstructureSupport, true, -1), true);
				Map map = base.Map;
				if (map != null)
				{
					SubstructureGrid substructureGrid = map.substructureGrid;
					if (substructureGrid != null)
					{
						substructureGrid.Drawer.SetDirty();
					}
				}
				if (regenerateSectionLayers && base.Map != null && base.Map.mapDrawer != null)
				{
					base.Map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_GravshipMask));
					base.Map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_GravshipHull));
					base.Map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_SubstructureProps));
				}
			}
			if (this.hasTickedThisSession && this.nameHidden && !this.haveShownNameDialog && Find.ResearchManager.gravEngineInspected && base.Faction == Faction.OfPlayer && this.validSubstructure.Count > 90)
			{
				Find.WindowStack.Add(new Dialog_NamePlayerGravship(this));
				this.haveShownNameDialog = true;
			}
		}

		// Token: 0x0600EE7E RID: 61054 RVA: 0x00453480 File Offset: 0x00451680
		public override AcceptanceReport ClaimableBy(Faction by)
		{
			AcceptanceReport result = base.ClaimableBy(by);
			if (!result.Accepted)
			{
				return result;
			}
			if (!Find.ResearchManager.gravEngineInspected)
			{
				return "GravEngineNotInspected".Translate();
			}
			return true;
		}

		// Token: 0x0600EE7F RID: 61055 RVA: 0x004534C4 File Offset: 0x004516C4
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!base.Spawned)
			{
				return text;
			}
			if (text.Length > 0)
			{
				text += "\n";
			}
			if (!Find.ResearchManager.gravEngineInspected)
			{
				text += "GravEngineNotInspected".Translate() + "\n";
			}
			text = string.Concat(new string[]
			{
				text,
				"ConnectedSubstructure".Translate() + ": ",
				this.AllConnectedSubstructure.Count.ToString(),
				" / ",
				StatDefOf.SubstructureSupport.ValueToString(this.GetStatValue(StatDefOf.SubstructureSupport, true, -1), ToStringNumberSense.Absolute, true)
			});
			int num = base.Map.listerThings.ThingsOfDef(ThingDefOf.Blueprint_Substructure).Count;
			num += base.Map.listerThings.ThingsOfDef(ThingDefOf.Frame_Substructure).Count;
			if (num > 0)
			{
				text = text + ("\n" + "SubstructureBlueprintsCount".Translate() + ": ") + num.ToString();
			}
			if (this.MissingComponents.Count > 0)
			{
				string[] array = new string[5];
				array[0] = text;
				array[1] = "\n";
				array[2] = "Requires".Translate().ToString();
				array[3] = ": ";
				array[4] = (from d in this.MissingComponents
				select d.label).ToCommaList(false, false).Colorize(ColorLibrary.RedReadable).CapitalizeFirst();
				text = string.Concat(array);
			}
			return text;
		}

		// Token: 0x0600EE80 RID: 61056 RVA: 0x00453683 File Offset: 0x00451883
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (!base.Spawned)
			{
				yield break;
			}
			if (!Find.ResearchManager.gravEngineInspected)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandInspectGravEngine".Translate(this),
					defaultDesc = "CommandInspectGravEngineDesc".Translate(),
					icon = Building_GravEngine.InspectCommandTex,
					action = delegate()
					{
						Find.Targeter.BeginTargeting(TargetingParameters.ForColonist(), delegate(LocalTargetInfo targ)
						{
							Pawn pawn = targ.Thing as Pawn;
							if (pawn != null)
							{
								pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.InspectGravEngine, this), new JobTag?(JobTag.Misc), false);
							}
						}, delegate(LocalTargetInfo targ)
						{
							if (Building_GravEngine.<GetGizmos>g__ValidateInspectionTarget|77_0(targ))
							{
								GenDraw.DrawTargetHighlight(targ);
							}
						}, new Func<LocalTargetInfo, bool>(Building_GravEngine.<GetGizmos>g__ValidateInspectionTarget|77_0), null, null, Building_GravEngine.InspectCommandTex, true, delegate(LocalTargetInfo _)
						{
							Widgets.MouseAttachedLabel("ChooseWhoShouldInspect".Translate(this), 0f, 0f, null);
						}, null);
					}
				};
				if (DebugSettings.ShowDevGizmos)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Inspect now",
						action = delegate()
						{
							this.Inspect(false);
						}
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EE81 RID: 61057 RVA: 0x00453693 File Offset: 0x00451893
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (!Find.ResearchManager.gravEngineInspected)
			{
				yield return new FloatMenuOption("CommandInspectGravEngine".Translate(this), delegate()
				{
					selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.InspectGravEngine, this), new JobTag?(JobTag.Misc), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EE82 RID: 61058 RVA: 0x004536AA File Offset: 0x004518AA
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.FuelSavingsPercent > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_GravEngineFuelConsumption".Translate(), (-this.FuelSavingsPercent).ToStringPercentSigned("F0"), "StatsReport_GravEngineFuelConsumption_Desc".Translate(), 520, null, null, false, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EE83 RID: 61059 RVA: 0x004536BA File Offset: 0x004518BA
		public IEnumerable<TaggedString> GetOrbitalWarnings()
		{
			bool flag = true;
			int heatPusherCount = 0;
			int oxygenPusherCount = 0;
			this.tmpGravshipRooms.Clear();
			foreach (IntVec3 intVec in this.validSubstructure)
			{
				foreach (Thing thing in intVec.GetThingList(base.Map))
				{
					if (thing.HasComp<CompOxygenPusher>())
					{
						int num = oxygenPusherCount;
						oxygenPusherCount = num + 1;
					}
					CompHeatPusher compHeatPusher;
					if (thing is Building_Heater || (thing.TryGetComp(out compHeatPusher) && compHeatPusher.Props.heatPerSecond > 20f))
					{
						int num = heatPusherCount;
						heatPusherCount = num + 1;
					}
				}
				Room room = intVec.GetRoom(base.Map);
				if (room != null && !this.tmpGravshipRooms.Contains(room) && !room.UsesOutdoorTemperature && !room.IsDoorway)
				{
					this.tmpGravshipRooms.Add(room);
					if (!VacuumUtility.IsRoomAirtight(room))
					{
						flag = false;
					}
				}
			}
			string baseWarning = ("OrbitalWarning_Warning".Translate() + ": ").Colorize(ColorLibrary.RedReadable);
			if (!flag)
			{
				yield return baseWarning + "OrbitalWarning_Vacuum".Translate();
			}
			if (heatPusherCount == 0)
			{
				yield return baseWarning + "OrbitalWarning_NoHeatSources".Translate();
			}
			else if ((float)heatPusherCount < (float)this.ValidSubstructure.Count / 250f)
			{
				yield return baseWarning + "OrbitalWarning_FewHeatSources".Translate() + ": " + heatPusherCount.ToString();
			}
			if (oxygenPusherCount == 0)
			{
				yield return baseWarning + "OrbitalWarning_NoOxygenPumps".Translate();
			}
			else if ((float)oxygenPusherCount < (float)this.ValidSubstructure.Count / 400f)
			{
				yield return baseWarning + "OrbitalWarning_FewOxygenPumps".Translate() + ": " + oxygenPusherCount.ToString();
			}
			yield break;
		}

		// Token: 0x0600EE84 RID: 61060 RVA: 0x004536CC File Offset: 0x004518CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.cooldownCompleteTick, "cooldownCompleteTick", -1, false);
			Scribe_Values.Look<bool>(ref this.silentlyActivate, "silentlyActivate", false, false);
			Scribe_Values.Look<bool>(ref this.nameHidden, "nameHidden", true, false);
			Scribe_Values.Look<string>(ref this.gravshipName, "gravshipName", null, false);
			Scribe_Deep.Look<LaunchInfo>(ref this.launchInfo, "launchInfo", Array.Empty<object>());
		}

		// Token: 0x0600EE8B RID: 61067 RVA: 0x004538CC File Offset: 0x00451ACC
		[CompilerGenerated]
		internal static bool <GetGizmos>g__ValidateInspectionTarget|77_0(LocalTargetInfo targ)
		{
			Pawn pawn = targ.Thing as Pawn;
			return pawn != null && pawn.IsColonistPlayerControlled && pawn.CanReach(pawn.Position, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && !pawn.Downed && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving);
		}

		// Token: 0x04009DA4 RID: 40356
		public int cooldownCompleteTick = -1;

		// Token: 0x04009DA5 RID: 40357
		public bool silentlyActivate;

		// Token: 0x04009DA6 RID: 40358
		public bool nameHidden = true;

		// Token: 0x04009DA7 RID: 40359
		private string gravshipName = "Gravship".Translate();

		// Token: 0x04009DA8 RID: 40360
		public LaunchInfo launchInfo;

		// Token: 0x04009DA9 RID: 40361
		[Unsaved(false)]
		private CompAffectedByFacilities affectedByFacilities;

		// Token: 0x04009DAA RID: 40362
		[Unsaved(false)]
		private List<CompGravshipFacility> gravshipComponents = new List<CompGravshipFacility>();

		// Token: 0x04009DAB RID: 40363
		[Unsaved(false)]
		private HashSet<IntVec3> validSubstructure = new HashSet<IntVec3>();

		// Token: 0x04009DAC RID: 40364
		[Unsaved(false)]
		private HashSet<IntVec3> allConnectedSubstructure = new HashSet<IntVec3>();

		// Token: 0x04009DAD RID: 40365
		[Unsaved(false)]
		private List<GravshipComponentTypeDef> missingComponents = new List<GravshipComponentTypeDef>();

		// Token: 0x04009DAE RID: 40366
		[Unsaved(false)]
		public bool substructureDirty = true;

		// Token: 0x04009DAF RID: 40367
		[Unsaved(false)]
		private bool missingComponentsDirty = true;

		// Token: 0x04009DB0 RID: 40368
		[Unsaved(false)]
		private bool gravshipComponentsDirty = true;

		// Token: 0x04009DB1 RID: 40369
		[Unsaved(false)]
		private bool haveShownNameDialog;

		// Token: 0x04009DB2 RID: 40370
		[Unsaved(false)]
		private bool hasTickedThisSession;

		// Token: 0x04009DB3 RID: 40371
		private static readonly Texture2D InspectCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Inspect", true);

		// Token: 0x04009DB4 RID: 40372
		private static readonly CachedMaterial OrbMat = new CachedMaterial("Things/Building/GravEngine/GravEngine_Orb", ShaderDatabase.Cutout);

		// Token: 0x04009DB5 RID: 40373
		private static Graphic onCooldownGraphic;

		// Token: 0x04009DB6 RID: 40374
		private const float BaseFuelPerTile = 10f;

		// Token: 0x04009DB7 RID: 40375
		private const float BobHeight = 0.3f;

		// Token: 0x04009DB8 RID: 40376
		private const int NameDialogSubstructureThreshold = 90;

		// Token: 0x04009DB9 RID: 40377
		private HashSet<Room> tmpGravshipRooms = new HashSet<Room>();
	}
}
