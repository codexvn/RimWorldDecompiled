using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030D0 RID: 12496
	public class Designator_Build : Designator_Place
	{
		// Token: 0x17002D74 RID: 11636
		// (get) Token: 0x060118B6 RID: 71862 RVA: 0x00509936 File Offset: 0x00507B36
		public override BuildableDef PlacingDef
		{
			get
			{
				return this.entDef;
			}
		}

		// Token: 0x17002D75 RID: 11637
		// (get) Token: 0x060118B7 RID: 71863 RVA: 0x00509940 File Offset: 0x00507B40
		public override ThingDef StuffDef
		{
			get
			{
				if (this.stuffDef != null)
				{
					return this.stuffDef;
				}
				ThingDef thingDef = null;
				ThingDef thingDef2 = this.entDef as ThingDef;
				if (thingDef2 != null && thingDef2.MadeFromStuff)
				{
					thingDef = GenStuff.DefaultStuffFor(thingDef2);
					if (Find.CurrentMap != null && Find.CurrentMap.resourceCounter.GetCount(thingDef) < thingDef2.CostStuffCount)
					{
						ThingDef thingDef3 = null;
						foreach (KeyValuePair<ThingDef, int> keyValuePair in Find.CurrentMap.resourceCounter.AllCountedAmounts)
						{
							if (keyValuePair.Key.IsStuff && keyValuePair.Key.stuffProps.canSuggestUseDefaultStuff && keyValuePair.Key.stuffProps.CanMake(thingDef2) && keyValuePair.Value >= thingDef2.CostStuffCount)
							{
								thingDef3 = keyValuePair.Key;
								break;
							}
						}
						if (thingDef3 != null)
						{
							thingDef = thingDef3;
						}
					}
				}
				return thingDef;
			}
		}

		// Token: 0x17002D76 RID: 11638
		// (get) Token: 0x060118B8 RID: 71864 RVA: 0x00509A48 File Offset: 0x00507C48
		public ThingDef StuffDefRaw
		{
			get
			{
				return this.stuffDef;
			}
		}

		// Token: 0x17002D77 RID: 11639
		// (get) Token: 0x060118B9 RID: 71865 RVA: 0x00509A50 File Offset: 0x00507C50
		public override string Label
		{
			get
			{
				ThingDef thingDef = this.entDef as ThingDef;
				string text;
				if (thingDef != null && this.writeStuff)
				{
					text = GenLabel.ThingLabel(thingDef, this.StuffDef, 1);
				}
				else
				{
					text = this.entDef.label;
				}
				if (this.sourcePrecept != null)
				{
					text = this.sourcePrecept.TransformThingLabel(text);
				}
				if (thingDef != null && !this.writeStuff && thingDef.MadeFromStuff)
				{
					text += "...";
				}
				return text;
			}
		}

		// Token: 0x17002D78 RID: 11640
		// (get) Token: 0x060118BA RID: 71866 RVA: 0x00509AC5 File Offset: 0x00507CC5
		public override string Desc
		{
			get
			{
				return this.entDef.description;
			}
		}

		// Token: 0x17002D79 RID: 11641
		// (get) Token: 0x060118BB RID: 71867 RVA: 0x00509AD4 File Offset: 0x00507CD4
		public override Color IconDrawColor
		{
			get
			{
				ThingDef thingDef = this.entDef as ThingDef;
				if (thingDef != null)
				{
					Color? ideoColorForBuilding = IdeoUtility.GetIdeoColorForBuilding(thingDef, Faction.OfPlayer);
					if (ideoColorForBuilding != null)
					{
						return ideoColorForBuilding.Value;
					}
				}
				if (this.StuffDef != null)
				{
					return this.entDef.GetColorForStuff(this.StuffDef);
				}
				return this.entDef.uiIconColor;
			}
		}

		// Token: 0x17002D7A RID: 11642
		// (get) Token: 0x060118BC RID: 71868 RVA: 0x00509B34 File Offset: 0x00507D34
		public override bool Visible
		{
			get
			{
				if (DebugSettings.godMode)
				{
					return true;
				}
				if (this.entDef.minTechLevelToBuild != TechLevel.Undefined && Faction.OfPlayer.def.techLevel < this.entDef.minTechLevelToBuild)
				{
					return false;
				}
				if (this.entDef.maxTechLevelToBuild != TechLevel.Undefined && Faction.OfPlayer.def.techLevel > this.entDef.maxTechLevelToBuild)
				{
					return false;
				}
				if (!this.entDef.IsResearchFinished)
				{
					return false;
				}
				if (ModsConfig.AnomalyActive && this.entDef.minMonolithLevel > Find.Anomaly.HighestLevelReached && Find.Anomaly.GenerateMonolith)
				{
					return false;
				}
				if (!Find.Storyteller.difficulty.AllowedToBuild(this.entDef))
				{
					return false;
				}
				if (this.entDef.PlaceWorkers != null)
				{
					using (List<PlaceWorker>.Enumerator enumerator = this.entDef.PlaceWorkers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!enumerator.Current.IsBuildDesignatorVisible(this.entDef))
							{
								return false;
							}
						}
					}
				}
				if (this.entDef.buildingPrerequisites != null)
				{
					for (int i = 0; i < this.entDef.buildingPrerequisites.Count; i++)
					{
						if (!base.Map.listerBuildings.ColonistsHaveBuilding(this.entDef.buildingPrerequisites[i]))
						{
							return false;
						}
					}
				}
				if (this.entDef.discoveryPrerequisites != null)
				{
					for (int j = 0; j < this.entDef.discoveryPrerequisites.Count; j++)
					{
						if (Find.HiddenItemsManager.Hidden(this.entDef.discoveryPrerequisites[j]))
						{
							return false;
						}
					}
				}
				return !ModsConfig.OdysseyActive || !this.entDef.requireInspectedGravEngine || Find.ResearchManager.gravEngineInspected;
			}
		}

		// Token: 0x17002D7B RID: 11643
		// (get) Token: 0x060118BD RID: 71869 RVA: 0x00509D14 File Offset: 0x00507F14
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				if (!this.eyedropMode)
				{
					return this.entDef.drawStyleCategory;
				}
				return this.eyedropper.DrawStyleCategory;
			}
		}

		// Token: 0x17002D7C RID: 11644
		// (get) Token: 0x060118BE RID: 71870 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D7D RID: 11645
		// (get) Token: 0x060118BF RID: 71871 RVA: 0x00356058 File Offset: 0x00354258
		public override float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x17002D7E RID: 11646
		// (get) Token: 0x060118C0 RID: 71872 RVA: 0x00509D35 File Offset: 0x00507F35
		public override string HighlightTag
		{
			get
			{
				if (this.cachedHighlightTag == null && this.tutorTag != null)
				{
					this.cachedHighlightTag = "Designator-Build-" + this.tutorTag;
				}
				return this.cachedHighlightTag;
			}
		}

		// Token: 0x17002D7F RID: 11647
		// (get) Token: 0x060118C1 RID: 71873 RVA: 0x00509D64 File Offset: 0x00507F64
		public override ThingStyleDef ThingStyleDefForPreview
		{
			get
			{
				if (this.sourcePrecept == null)
				{
					return this.ThingStyleDefNonPreceptSource;
				}
				ThingDef thingDef = this.entDef as ThingDef;
				if (thingDef != null)
				{
					return this.sourcePrecept.ideo.GetStyleFor(thingDef);
				}
				return null;
			}
		}

		// Token: 0x17002D80 RID: 11648
		// (get) Token: 0x060118C2 RID: 71874 RVA: 0x00509DA4 File Offset: 0x00507FA4
		public ThingStyleDef ThingStyleDefNonPreceptSource
		{
			get
			{
				ThingDef thingDef = this.PlacingDef as ThingDef;
				if (thingDef == null)
				{
					return null;
				}
				if (Find.IdeoManager.classicMode && this.styleOverridden)
				{
					return this.styleDef;
				}
				FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
				if (ideos == null)
				{
					return null;
				}
				Ideo primaryIdeo = ideos.PrimaryIdeo;
				if (primaryIdeo == null)
				{
					return null;
				}
				return primaryIdeo.GetStyleFor(thingDef);
			}
		}

		// Token: 0x17002D81 RID: 11649
		// (get) Token: 0x060118C3 RID: 71875 RVA: 0x00509DFE File Offset: 0x00507FFE
		public override float Order
		{
			get
			{
				return this.entDef.uiOrder;
			}
		}

		// Token: 0x060118C4 RID: 71876 RVA: 0x0013CBB3 File Offset: 0x0013ADB3
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}

		// Token: 0x060118C5 RID: 71877 RVA: 0x00509E0C File Offset: 0x0050800C
		private void UpdateIcon()
		{
			this.icon = this.entDef.GetUIIconForStuff(this.StuffDef);
			TerrainDef terrainDef = this.entDef as TerrainDef;
			if (terrainDef != null && terrainDef.cropIcon)
			{
				this.iconTexCoords = Widgets.CroppedTerrainTextureRect((Texture2D)this.icon);
			}
		}

		// Token: 0x060118C6 RID: 71878 RVA: 0x00509E5D File Offset: 0x0050805D
		public override void Deselected()
		{
			if (this.resetStuffAfterDeselect != null)
			{
				this.stuffDef = this.resetStuffAfterDeselect;
			}
			this.styleOverridden = false;
			this.glowerColorOverride = null;
			this.resetStuffAfterDeselect = null;
			this.UpdateIcon();
		}

		// Token: 0x060118C7 RID: 71879 RVA: 0x00509E94 File Offset: 0x00508094
		public Designator_Build(BuildableDef entDef)
		{
			this.entDef = entDef;
			this.iconAngle = entDef.uiIconAngle;
			this.iconOffset = entDef.uiIconOffset;
			this.hotKey = entDef.designationHotKey;
			this.tutorTag = entDef.defName;
			this.Order = 2000f;
			ThingDef thingDef = entDef as ThingDef;
			if (thingDef != null)
			{
				this.iconProportions = (thingDef.defaultPlacingRot.IsHorizontal ? thingDef.graphicData.drawSize.Rotated() : thingDef.graphicData.drawSize);
				this.iconDrawScale = GenUI.IconDrawScale(thingDef);
			}
			else
			{
				this.iconProportions = new Vector2(1f, 1f);
				this.iconDrawScale = 1f;
			}
			this.ResetStuffToDefault();
			BuildableDef placingDef = this.PlacingDef;
			TerrainDef terrain = placingDef as TerrainDef;
			if (terrain != null && terrain.colorDef != null && terrain.designatorDropdown != null && terrain.designatorDropdown.includeEyeDropperTool)
			{
				Func<TerrainDef, bool> <>9__1;
				this.eyedropper = new Designator_Eyedropper(delegate(ColorDef newCol)
				{
					IEnumerable<TerrainDef> allDefs = DefDatabase<TerrainDef>.AllDefs;
					Func<TerrainDef, bool> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((TerrainDef x) => x.designatorDropdown == terrain.designatorDropdown));
					}
					foreach (TerrainDef terrainDef in allDefs.Where(predicate))
					{
						if (terrainDef.colorDef == newCol)
						{
							Designator_Dropdown designator_Dropdown = BuildCopyCommandUtility.FindAllowedDesignatorRoot(terrainDef, true) as Designator_Dropdown;
							if (designator_Dropdown != null)
							{
								Log.Message("Selecting dropdown");
								Designator_Build des = BuildCopyCommandUtility.FindAllowedDesignator(terrainDef, true);
								designator_Dropdown.SetActiveDesignator(des, true);
								Find.DesignatorManager.Select(des);
								break;
							}
						}
					}
				}, "SelectColoredFloor".Translate(), "DesignatorEyeDropperDesc_Carpet".Translate());
			}
		}

		// Token: 0x060118C8 RID: 71880 RVA: 0x00509FDC File Offset: 0x005081DC
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null && thingDef.MadeFromStuff)
			{
				Designator_Dropdown.DrawExtraOptionsIcon(topLeft, this.GetWidth(maxWidth));
			}
			return result;
		}

		// Token: 0x060118C9 RID: 71881 RVA: 0x0050A018 File Offset: 0x00508218
		public override void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			ThingStyleDef thingStyleDef = (this.PlacingDef is ThingDef) ? this.ThingStyleDefForPreview : null;
			Color? color = parms.lowLight ? new Color?(Command.LowLightIconColor) : null;
			Color value = color.GetValueOrDefault();
			if (color == null)
			{
				value = this.IconDrawColor;
				color = new Color?(value);
			}
			Widgets.DefIcon(rect, this.PlacingDef, this.StuffDef, 0.85f, thingStyleDef, false, color, this.overrideMaterial, null, 1f);
		}

		// Token: 0x060118CA RID: 71882 RVA: 0x0050A0AC File Offset: 0x005082AC
		public Texture ResolvedIcon(ThingStyleDef styleDef = null)
		{
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null)
			{
				ThingStyleDef thingStyleDef = styleDef ?? this.ThingStyleDefNonPreceptSource;
				return Widgets.GetIconFor(thingDef, this.StuffDef, thingStyleDef, null);
			}
			if (this.StuffDef != null)
			{
				Graphic_Appearances graphic_Appearances = this.entDef.graphic as Graphic_Appearances;
				if (graphic_Appearances != null)
				{
					return (Texture2D)graphic_Appearances.SubGraphicFor(this.StuffDef).MatAt(this.entDef.defaultPlacingRot, null).mainTexture;
				}
			}
			return this.icon;
		}

		// Token: 0x060118CB RID: 71883 RVA: 0x0050A135 File Offset: 0x00508335
		public override void DrawMouseAttachments()
		{
			this.eyedropMode = (this.eyedropper != null && KeyBindingDefOf.ShowEyedropper.IsDown);
			if (this.eyedropMode)
			{
				this.eyedropper.DrawMouseAttachments();
				return;
			}
			base.DrawMouseAttachments();
		}

		// Token: 0x060118CC RID: 71884 RVA: 0x0050A16C File Offset: 0x0050836C
		public void ResetStuffToDefault()
		{
			this.stuffDef = null;
			this.UpdateIcon();
		}

		// Token: 0x060118CD RID: 71885 RVA: 0x0050A17C File Offset: 0x0050837C
		protected override void DrawPlaceMouseAttachments(float curX, ref float curY)
		{
			base.DrawPlaceMouseAttachments(curX, ref curY);
			if (!ArchitectCategoryTab.InfoRect.Contains(UI.MousePositionOnUIInverted))
			{
				if (this.eyedropper != null)
				{
					Rect rect = new Rect(curX, curY, 9999f, Text.LineHeight * 2f);
					Widgets.Label(rect, "Color".Translate() + ": " + ((TerrainDef)this.PlacingDef).colorDef.LabelCap + "\n" + KeyBindingDefOf.ShowEyedropper.MainKeyLabel + ": " + "GrabExistingColor".Translate());
					curY += rect.height;
				}
				DesignationDragger dragger = Find.DesignatorManager.Dragger;
				int num;
				if (dragger.Dragging)
				{
					num = dragger.DragCells.Count<IntVec3>();
				}
				else
				{
					num = 1;
				}
				List<ThingDefCountClass> list = this.entDef.CostListAdjusted(this.StuffDef, true);
				for (int i = 0; i < list.Count; i++)
				{
					ThingDefCountClass thingDefCountClass = list[i];
					float y = curY;
					Widgets.ThingIcon(new Rect(curX, y, 27f, 27f), thingDefCountClass.thingDef, null, null, 1f, null, null, 1f);
					Rect rect2 = new Rect(curX + 29f, y, 999f, 29f);
					int num2 = num * thingDefCountClass.count;
					string text = num2.ToString();
					if (base.Map.resourceCounter.GetCount(thingDefCountClass.thingDef) < num2)
					{
						GUI.color = Color.red;
						text += " (" + "NotEnoughStoredLower".Translate() + ")";
					}
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.Label(rect2, text);
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
					curY += 29f;
				}
			}
		}

		// Token: 0x060118CE RID: 71886 RVA: 0x0050A388 File Offset: 0x00508588
		public override void ProcessInput(Event ev)
		{
			if (!base.CheckCanInteract())
			{
				return;
			}
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef == null || !thingDef.MadeFromStuff)
			{
				base.ProcessInput(ev);
				return;
			}
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (ThingDef thingDef2 in base.Map.resourceCounter.AllCountedAmounts.Keys.OrderByDescending(delegate(ThingDef d)
			{
				StuffProperties stuffProps = d.stuffProps;
				if (stuffProps == null)
				{
					return float.PositiveInfinity;
				}
				return stuffProps.commonality;
			}).ThenBy((ThingDef d) => d.BaseMarketValue))
			{
				if (thingDef2.IsStuff && thingDef2.stuffProps.CanMake(thingDef) && (DebugSettings.godMode || base.Map.listerThings.ThingsOfDef(thingDef2).Count > 0))
				{
					ThingDef localStuffDef = thingDef2;
					string text;
					if (this.sourcePrecept != null)
					{
						text = "ThingMadeOfStuffLabel".Translate(localStuffDef.LabelAsStuff, this.sourcePrecept.Label);
					}
					else
					{
						text = GenLabel.ThingLabel(this.entDef, localStuffDef, 1);
					}
					text = text.CapitalizeFirst();
					list.Add(new FloatMenuOption(text, delegate()
					{
						this.<>n__0(ev);
						Find.DesignatorManager.Select(this);
						this.stuffDef = localStuffDef;
						this.writeStuff = true;
					}, thingDef2, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null)
					{
						tutorTag = "SelectStuff-" + thingDef.defName + "-" + localStuffDef.defName
					});
				}
			}
			if (list.Count == 0)
			{
				Messages.Message("NoStuffsToBuildWith".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			FloatMenu floatMenu = new FloatMenu(list);
			floatMenu.onCloseCallback = delegate()
			{
				this.writeStuff = true;
			};
			Find.WindowStack.Add(floatMenu);
			Find.DesignatorManager.Select(this);
		}

		// Token: 0x060118CF RID: 71887 RVA: 0x0050A5E8 File Offset: 0x005087E8
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (this.eyedropMode)
			{
				return this.eyedropper.CanDesignateCell(c);
			}
			return GenConstruct.CanPlaceBlueprintAt(this.entDef, c, this.placingRot, base.Map, DebugSettings.godMode, null, null, this.StuffDef, false, false, false);
		}

		// Token: 0x060118D0 RID: 71888 RVA: 0x0050A634 File Offset: 0x00508834
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(base.TutorTagDesignate, c)))
			{
				return;
			}
			if (this.eyedropMode)
			{
				this.eyedropper.DesignateSingleCell(c);
				return;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = thingList.Count - 1; i >= 0; i--)
			{
				Frame frame = thingList[i] as Frame;
				if (frame != null && this.entDef.blueprintDef.replaceTags.NotNullAndContainsAnyElement(frame.def.replaceTags))
				{
					frame.Destroy(DestroyMode.Cancel);
				}
			}
			if (DebugSettings.godMode || this.entDef.GetStatValueAbstract(StatDefOf.WorkToBuild, this.StuffDef) == 0f)
			{
				TerrainDef terrainDef = this.entDef as TerrainDef;
				if (terrainDef != null)
				{
					base.Map.terrainGrid.RemoveTempTerrain(c, false, false);
					if (terrainDef.isFoundation)
					{
						if (base.Map.terrainGrid.CanRemoveTopLayerAt(c))
						{
							base.Map.terrainGrid.RemoveTopLayer(c, !DebugSettings.godMode);
						}
						base.Map.terrainGrid.SetFoundation(c, terrainDef);
					}
					else if (terrainDef.temporary)
					{
						base.Map.terrainGrid.SetTempTerrain(c, terrainDef);
					}
					else
					{
						base.Map.terrainGrid.SetTerrain(c, terrainDef);
					}
				}
				else
				{
					Thing thing = ThingMaker.MakeThing((ThingDef)this.entDef, this.StuffDef);
					thing.SetFactionDirect(Faction.OfPlayer);
					Thing thing2 = GenSpawn.Spawn(thing, c, base.Map, this.placingRot, WipeMode.Vanish, false, false);
					if (this.sourcePrecept != null)
					{
						Building building = thing2 as Building;
						if (building != null)
						{
							building.StyleSourcePrecept = this.sourcePrecept;
							goto IL_1CE;
						}
					}
					if (this.sourcePrecept == null)
					{
						thing2.StyleDef = this.ThingStyleDefNonPreceptSource;
					}
					IL_1CE:
					if (this.glowerColorOverride != null)
					{
						CompGlower compGlower = thing2.TryGetComp<CompGlower>();
						if (compGlower != null)
						{
							compGlower.GlowColor = this.glowerColorOverride.Value;
						}
					}
				}
			}
			else
			{
				GenSpawn.WipeExistingThings(c, this.placingRot, this.entDef.blueprintDef, base.Map, DestroyMode.Deconstruct);
				Blueprint_Build blueprint_Build = GenConstruct.PlaceBlueprintForBuild(this.entDef, c, base.Map, this.placingRot, Faction.OfPlayer, this.StuffDef, null, null, true);
				blueprint_Build.StyleSourcePrecept = this.sourcePrecept;
				if (blueprint_Build.StyleSourcePrecept == null)
				{
					blueprint_Build.StyleDef = this.ThingStyleDefNonPreceptSource;
				}
				blueprint_Build.glowerColorOverride = this.glowerColorOverride;
			}
			FleckMaker.ThrowMetaPuffs(GenAdj.OccupiedRect(c, this.placingRot, this.entDef.Size), base.Map);
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null && thingDef.IsOrbitalTradeBeacon)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.BuildOrbitalTradeBeacon, KnowledgeAmount.Total);
			}
			if (TutorSystem.TutorialMode)
			{
				TutorSystem.Notify_Event(new EventPack(base.TutorTagDesignate, c));
			}
			if (this.entDef.PlaceWorkers != null)
			{
				for (int j = 0; j < this.entDef.PlaceWorkers.Count; j++)
				{
					this.entDef.PlaceWorkers[j].PostPlace(base.Map, this.entDef, c, this.placingRot);
				}
			}
		}

		// Token: 0x060118D1 RID: 71889 RVA: 0x0050A96B File Offset: 0x00508B6B
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.entDef, this.placingRot);
		}

		// Token: 0x060118D2 RID: 71890 RVA: 0x0050A984 File Offset: 0x00508B84
		public override void DrawPanelReadout(ref float curY, float width)
		{
			if (this.entDef.CostStuffCount <= 0 && this.stuffDef != null)
			{
				this.stuffDef = null;
			}
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null)
			{
				Widgets.InfoCardButton(width - 24f - 2f, 6f, thingDef, this.StuffDef);
			}
			else
			{
				Widgets.InfoCardButton(width - 24f - 2f, 6f, this.entDef);
			}
			Text.Font = GameFont.Small;
			Designator_Build.tmpcostList.Clear();
			Designator_Build.tmpcostList.AddRange(this.entDef.CostListAdjusted(this.StuffDef, false));
			Designator_Build.tmpcostList.Sort((ThingDefCountClass a, ThingDefCountClass b) => b.count - a.count);
			for (int i = 0; i < Designator_Build.tmpcostList.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = Designator_Build.tmpcostList[i];
				Color color = GUI.color;
				Widgets.ThingIcon(new Rect(0f, curY, 20f, 20f), thingDefCountClass.thingDef, null, null, 1f, null, null, 1f);
				GUI.color = color;
				if (thingDefCountClass.thingDef != null && thingDefCountClass.thingDef.resourceReadoutPriority != ResourceCountPriority.Uncounted && base.Map.resourceCounter.GetCount(thingDefCountClass.thingDef) < thingDefCountClass.count)
				{
					GUI.color = Color.red;
				}
				Widgets.Label(new Rect(26f, curY + 2f, 50f, 100f), thingDefCountClass.count.ToString());
				GUI.color = Color.white;
				string text;
				if (thingDefCountClass.thingDef == null)
				{
					text = "(" + "UnchosenStuff".Translate() + ")";
				}
				else
				{
					text = thingDefCountClass.thingDef.LabelCap;
				}
				float width2 = width - 60f;
				float num = Text.CalcHeight(text, width2) - 5f;
				Widgets.Label(new Rect(60f, curY + 2f, width2, num + 5f), text);
				curY += num;
			}
			StyleCategoryDef styleCategoryDef = this.entDef.dominantStyleCategory;
			if (styleCategoryDef == null && thingDef != null)
			{
				FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
				StyleCategoryDef styleCategoryDef2;
				if (ideos == null)
				{
					styleCategoryDef2 = null;
				}
				else
				{
					Ideo primaryIdeo = ideos.PrimaryIdeo;
					styleCategoryDef2 = ((primaryIdeo != null) ? primaryIdeo.GetStyleCategoryFor(thingDef) : null);
				}
				styleCategoryDef = styleCategoryDef2;
			}
			if (styleCategoryDef != null)
			{
				TaggedString taggedString = "DominantStyle".Translate().CapitalizeFirst() + ": " + styleCategoryDef.LabelCap;
				Rect rect = new Rect(0f, curY + 2f, width, Text.CalcHeight(taggedString, width));
				Widgets.Label(rect, taggedString);
				curY += rect.height;
			}
			if (this.entDef.constructionSkillPrerequisite > 0)
			{
				this.DrawSkillRequirement(SkillDefOf.Construction, this.entDef.constructionSkillPrerequisite, width, ref curY);
			}
			if (this.entDef.artisticSkillPrerequisite > 0)
			{
				this.DrawSkillRequirement(SkillDefOf.Artistic, this.entDef.artisticSkillPrerequisite, width, ref curY);
			}
			bool flag = false;
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.FreeColonists)
			{
				if (pawn.skills.GetSkill(SkillDefOf.Construction).Level >= this.entDef.constructionSkillPrerequisite && pawn.skills.GetSkill(SkillDefOf.Artistic).Level >= this.entDef.artisticSkillPrerequisite)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				TaggedString taggedString2 = "NoColonistWithAllSkillsForConstructing".Translate(Faction.OfPlayer.def.pawnsPlural);
				GUI.color = Color.red;
				Pawn pawn2;
				if (this.AnyMechWithSkillsRequired(this.entDef, out pawn2))
				{
					taggedString2 += " " + "MechCanBuildThis".Translate(pawn2.kindDef.label);
					GUI.color = Color.white;
				}
				Rect rect2 = new Rect(0f, curY + 2f, width, Text.CalcHeight(taggedString2, width));
				Widgets.Label(rect2, taggedString2);
				GUI.color = Color.white;
				curY += rect2.height;
			}
			curY += 4f;
		}

		// Token: 0x060118D3 RID: 71891 RVA: 0x0050AE04 File Offset: 0x00509004
		private bool AnyColonistWithSkill(int skill, SkillDef skillDef, bool careIfDisabled)
		{
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.FreeColonists)
			{
				if (pawn.skills.GetSkill(skillDef).Level >= skill && (!careIfDisabled || pawn.workSettings.WorkIsActive(WorkTypeDefOf.Construction)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060118D4 RID: 71892 RVA: 0x0050AE8C File Offset: 0x0050908C
		public bool AnyMechWithSkillsRequired(BuildableDef def, out Pawn pawn)
		{
			return MechanitorUtility.AnyPlayerMechCanDoWork(WorkTypeDefOf.Construction, def.constructionSkillPrerequisite, out pawn);
		}

		// Token: 0x060118D5 RID: 71893 RVA: 0x0050AEA0 File Offset: 0x005090A0
		private void DrawSkillRequirement(SkillDef skillDef, int requirement, float width, ref float curY)
		{
			Rect rect = new Rect(0f, curY + 2f, width, 24f);
			if (!this.AnyColonistWithSkill(requirement, skillDef, false))
			{
				GUI.color = Color.red;
				TooltipHandler.TipRegionByKey(rect, "NoColonistWithSkillTip", Faction.OfPlayer.def.pawnsPlural);
			}
			else if (!this.AnyColonistWithSkill(requirement, skillDef, true))
			{
				GUI.color = Color.yellow;
				TooltipHandler.TipRegionByKey(rect, "AllColonistsWithSkillHaveDisabledConstructingTip", Faction.OfPlayer.def.pawnsPlural, WorkTypeDefOf.Construction.gerundLabel);
			}
			else
			{
				GUI.color = new Color(0.72f, 0.87f, 0.72f);
			}
			Widgets.Label(rect, string.Format("{0}: {1}", "SkillNeededForConstructing".Translate(skillDef.LabelCap), requirement));
			GUI.color = Color.white;
			curY += 18f;
		}

		// Token: 0x060118D6 RID: 71894 RVA: 0x0050AFA2 File Offset: 0x005091A2
		public void SetStuffDef(ThingDef stuffDef)
		{
			this.stuffDef = stuffDef;
			this.UpdateIcon();
		}

		// Token: 0x060118D7 RID: 71895 RVA: 0x0050AFB1 File Offset: 0x005091B1
		public void SetTemporaryVars(ThingDef stuffDef, bool styleOverridden)
		{
			this.resetStuffAfterDeselect = this.stuffDef;
			this.stuffDef = stuffDef;
			this.styleOverridden = styleOverridden;
			this.UpdateIcon();
		}

		// Token: 0x0400B510 RID: 46352
		protected BuildableDef entDef;

		// Token: 0x0400B511 RID: 46353
		public Precept_Building sourcePrecept;

		// Token: 0x0400B512 RID: 46354
		public ThingStyleDef styleDef;

		// Token: 0x0400B513 RID: 46355
		public bool styleOverridden;

		// Token: 0x0400B514 RID: 46356
		public ColorInt? glowerColorOverride;

		// Token: 0x0400B515 RID: 46357
		private ThingDef stuffDef;

		// Token: 0x0400B516 RID: 46358
		private ThingDef resetStuffAfterDeselect;

		// Token: 0x0400B517 RID: 46359
		private bool writeStuff;

		// Token: 0x0400B518 RID: 46360
		public const float DragPriceDrawNumberX = 29f;

		// Token: 0x0400B519 RID: 46361
		public const float ResLineSpacing = 29f;

		// Token: 0x0400B51A RID: 46362
		private static List<ThingDefCountClass> tmpcostList = new List<ThingDefCountClass>();
	}
}
