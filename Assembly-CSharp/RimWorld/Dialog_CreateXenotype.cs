using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200314B RID: 12619
	public class Dialog_CreateXenotype : GeneCreationDialogBase
	{
		// Token: 0x17002E27 RID: 11815
		// (get) Token: 0x06011BFE RID: 72702 RVA: 0x0051F75F File Offset: 0x0051D95F
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)Mathf.Min(UI.screenWidth, 1036), (float)(UI.screenHeight - 4));
			}
		}

		// Token: 0x17002E28 RID: 11816
		// (get) Token: 0x06011BFF RID: 72703 RVA: 0x0051F77E File Offset: 0x0051D97E
		protected override List<GeneDef> SelectedGenes
		{
			get
			{
				return this.selectedGenes;
			}
		}

		// Token: 0x17002E29 RID: 11817
		// (get) Token: 0x06011C00 RID: 72704 RVA: 0x0051F788 File Offset: 0x0051D988
		protected override string Header
		{
			get
			{
				return "CreateXenotype".Translate().CapitalizeFirst();
			}
		}

		// Token: 0x17002E2A RID: 11818
		// (get) Token: 0x06011C01 RID: 72705 RVA: 0x0051F7AC File Offset: 0x0051D9AC
		protected override string AcceptButtonLabel
		{
			get
			{
				return "SaveAndApply".Translate().CapitalizeFirst();
			}
		}

		// Token: 0x06011C02 RID: 72706 RVA: 0x0051F7D0 File Offset: 0x0051D9D0
		public Dialog_CreateXenotype(int index, Action callback)
		{
			this.generationRequestIndex = index;
			this.callback = callback;
			this.xenotypeName = string.Empty;
			this.closeOnAccept = false;
			this.absorbInputAroundWindow = true;
			this.alwaysUseFullBiostatsTableHeight = true;
			this.searchWidgetOffsetX = GeneCreationDialogBase.ButSize.x * 2f + 4f;
			foreach (GeneCategoryDef key in DefDatabase<GeneCategoryDef>.AllDefs)
			{
				this.collapsedCategories.Add(key, false);
			}
			this.OnGenesChanged();
		}

		// Token: 0x06011C03 RID: 72707 RVA: 0x0051F8A8 File Offset: 0x0051DAA8
		public override void PostOpen()
		{
			if (!ModLister.CheckBiotech("xenotype creation"))
			{
				this.Close(false);
				return;
			}
			base.PostOpen();
		}

		// Token: 0x06011C04 RID: 72708 RVA: 0x0051F8C4 File Offset: 0x0051DAC4
		protected override void DrawGenes(Rect rect)
		{
			this.hoveredAnyGene = false;
			GUI.BeginGroup(rect);
			float num = 0f;
			this.DrawSection(new Rect(0f, 0f, rect.width, this.selectedHeight), this.selectedGenes, "SelectedGenes".Translate(), ref num, ref this.selectedHeight, false, rect, ref this.selectedCollapsed);
			if (!this.selectedCollapsed.Value)
			{
				num += 10f;
			}
			float num2 = num;
			Widgets.Label(0f, ref num, rect.width, "Genes".Translate().CapitalizeFirst(), default(TipSignal));
			num += 10f;
			float height = num - num2 - 4f;
			if (Widgets.ButtonText(new Rect(rect.width - 150f - 16f, num2, 150f, height), "CollapseAllCategories".Translate(), true, true, true, null))
			{
				SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				foreach (GeneCategoryDef key in DefDatabase<GeneCategoryDef>.AllDefs)
				{
					this.collapsedCategories[key] = true;
				}
			}
			if (Widgets.ButtonText(new Rect(rect.width - 300f - 4f - 16f, num2, 150f, height), "ExpandAllCategories".Translate(), true, true, true, null))
			{
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				foreach (GeneCategoryDef key2 in DefDatabase<GeneCategoryDef>.AllDefs)
				{
					this.collapsedCategories[key2] = false;
				}
			}
			float num3 = num;
			Rect rect2 = new Rect(0f, num, rect.width - 16f, this.scrollHeight);
			Widgets.BeginScrollView(new Rect(0f, num, rect.width, rect.height - num), ref this.scrollPosition, rect2, true);
			Rect containingRect = rect2;
			containingRect.y = num + this.scrollPosition.y;
			containingRect.height = rect.height;
			bool? flag = null;
			this.DrawSection(rect, GeneUtility.GenesInOrder, null, ref num, ref this.unselectedHeight, true, containingRect, ref flag);
			if (Event.current.type == EventType.Layout)
			{
				this.scrollHeight = num - num3;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			if (!this.hoveredAnyGene)
			{
				this.hoveredGene = null;
			}
		}

		// Token: 0x06011C05 RID: 72709 RVA: 0x0051FB80 File Offset: 0x0051DD80
		private void DrawSection(Rect rect, List<GeneDef> genes, string label, ref float curY, ref float sectionHeight, bool adding, Rect containingRect, ref bool? collapsed)
		{
			float num = 4f;
			if (!label.NullOrEmpty())
			{
				Rect rect2 = new Rect(0f, curY, rect.width, Text.LineHeight);
				rect2.xMax -= (adding ? 16f : (Text.CalcSize("ClickToAddOrRemove".Translate()).x + 4f));
				if (collapsed != null)
				{
					Rect position = new Rect(rect2.x, rect2.y + (rect2.height - 18f) / 2f, 18f, 18f);
					GUI.DrawTexture(position, collapsed.Value ? TexButton.Reveal : TexButton.Collapse);
					if (Widgets.ButtonInvisible(rect2, true))
					{
						collapsed = !collapsed;
						if (collapsed.Value)
						{
							SoundDefOf.TabClose.PlayOneShotOnCamera(null);
						}
						else
						{
							SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
						}
					}
					if (Mouse.IsOver(rect2))
					{
						Widgets.DrawHighlight(rect2);
					}
					rect2.xMin += position.width;
				}
				Widgets.Label(rect2, label);
				if (!adding)
				{
					Text.Anchor = TextAnchor.UpperRight;
					GUI.color = ColoredText.SubtleGrayColor;
					Widgets.Label(new Rect(rect2.xMax - 18f, curY, rect.width - rect2.width, Text.LineHeight), "ClickToAddOrRemove".Translate());
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperLeft;
				}
				curY += Text.LineHeight + 3f;
			}
			bool? flag = collapsed;
			bool flag2 = true;
			if (flag.GetValueOrDefault() == flag2 & flag != null)
			{
				if (Event.current.type == EventType.Layout)
				{
					sectionHeight = 0f;
				}
				return;
			}
			float num2 = curY;
			bool flag3 = false;
			float num3 = 34f + GeneCreationDialogBase.GeneSize.x + 8f;
			float num4 = GeneCreationDialogBase.GeneSize.y + 8f + 4f;
			float num5 = rect.width - 16f;
			float num6 = num3 + 4f;
			float b = (num5 - num6 * Mathf.Floor(num5 / num6)) / 2f;
			Rect rect3 = new Rect(0f, curY, rect.width, sectionHeight);
			if (!adding)
			{
				Widgets.DrawRectFast(rect3, Widgets.MenuSectionBGFillColor, null);
			}
			curY += 4f;
			if (!genes.Any<GeneDef>())
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				GUI.color = ColoredText.SubtleGrayColor;
				Widgets.Label(rect3, "(" + "NoneLower".Translate() + ")");
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
				curY += num4;
			}
			else
			{
				float num7 = rect.width - 16f;
				int num8 = Mathf.FloorToInt(num7 / (num3 + 1f));
				int num9 = Mathf.CeilToInt((float)genes.Count / (float)num8);
				float num10 = Mathf.Max(new float[]
				{
					num4 * (float)Mathf.Min(num9, 3)
				});
				float num11 = num4 * (float)num9;
				bool flag4;
				if (!adding)
				{
					flag = collapsed;
					flag2 = false;
					if ((flag.GetValueOrDefault() == flag2 & flag != null) && genes.Count > 0)
					{
						flag4 = (num11 > num10);
						goto IL_373;
					}
				}
				flag4 = false;
				IL_373:
				bool flag5 = flag4;
				float num12 = curY;
				if (flag5)
				{
					num12 = 0f;
					Rect outRect = new Rect(0f, curY, rect.width, num10);
					Rect viewRect = new Rect(0f, 0f, num7, num11);
					Widgets.BeginScrollView(outRect, ref this.selectedScrollPosition, viewRect, true);
				}
				GeneCategoryDef geneCategoryDef = null;
				int num13 = 0;
				for (int i = 0; i < genes.Count; i++)
				{
					GeneDef geneDef = genes[i];
					if ((!adding || !this.quickSearchWidget.filter.Active || (this.matchingGenes.Contains(geneDef) && !this.selectedGenes.Contains(geneDef)) || this.matchingCategories.Contains(geneDef.displayCategory)) && (this.ignoreRestrictions || geneDef.biostatArc <= 0))
					{
						bool flag6 = false;
						if (num + num3 > num5)
						{
							num = 4f;
							num12 += GeneCreationDialogBase.GeneSize.y + 8f + 4f;
							flag6 = true;
						}
						bool flag7 = this.quickSearchWidget.filter.Active && (this.matchingGenes.Contains(geneDef) || this.matchingCategories.Contains(geneDef.displayCategory));
						bool flag8 = this.collapsedCategories[geneDef.displayCategory] && !flag7;
						if (adding && geneCategoryDef != geneDef.displayCategory)
						{
							if (!flag6 && flag3)
							{
								num = 4f;
								num12 += GeneCreationDialogBase.GeneSize.y + 8f + 4f;
							}
							geneCategoryDef = geneDef.displayCategory;
							Rect rect4 = new Rect(num, num12, rect.width - 8f, Text.LineHeight);
							if (!flag7)
							{
								Rect position2 = new Rect(rect4.x, rect4.y + (rect4.height - 18f) / 2f, 18f, 18f);
								GUI.DrawTexture(position2, flag8 ? TexButton.Reveal : TexButton.Collapse);
								if (Widgets.ButtonInvisible(rect4, true))
								{
									this.collapsedCategories[geneDef.displayCategory] = !this.collapsedCategories[geneDef.displayCategory];
									if (this.collapsedCategories[geneDef.displayCategory])
									{
										SoundDefOf.TabClose.PlayOneShotOnCamera(null);
									}
									else
									{
										SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
									}
								}
								if (num13 % 2 == 1)
								{
									Widgets.DrawLightHighlight(rect4);
								}
								if (Mouse.IsOver(rect4))
								{
									Widgets.DrawHighlight(rect4);
								}
								rect4.xMin += position2.width;
							}
							Widgets.Label(rect4, geneCategoryDef.LabelCap);
							num12 += rect4.height;
							if (!flag8)
							{
								GUI.color = Color.grey;
								Widgets.DrawLineHorizontal(num, num12, rect.width - 8f);
								GUI.color = Color.white;
								num12 += 10f;
							}
							num13++;
						}
						if (adding && flag8)
						{
							flag3 = false;
							if (Event.current.type == EventType.Layout)
							{
								sectionHeight = num12 - num2;
							}
						}
						else
						{
							num = Mathf.Max(num, b);
							flag3 = true;
							if (this.DrawGene(geneDef, !adding, ref num, num12, num3, containingRect, flag7))
							{
								if (this.selectedGenes.Contains(geneDef))
								{
									SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
									this.selectedGenes.Remove(geneDef);
								}
								else
								{
									SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
									this.selectedGenes.Add(geneDef);
								}
								if (!this.xenotypeNameLocked)
								{
									this.xenotypeName = GeneUtility.GenerateXenotypeNameFromGenes(this.SelectedGenes);
								}
								this.OnGenesChanged();
								break;
							}
						}
					}
				}
				num12 += num4;
				if (flag5)
				{
					Widgets.EndScrollView();
					curY += num10;
				}
				else
				{
					curY = num12;
				}
			}
			if (Event.current.type == EventType.Layout)
			{
				sectionHeight = curY - num2;
			}
		}

		// Token: 0x06011C06 RID: 72710 RVA: 0x005202DC File Offset: 0x0051E4DC
		private bool DrawGene(GeneDef geneDef, bool selectedSection, ref float curX, float curY, float packWidth, Rect containingRect, bool isMatch)
		{
			bool result = false;
			Rect rect = new Rect(curX, curY, packWidth, GeneCreationDialogBase.GeneSize.y + 8f);
			if (!containingRect.Overlaps(rect))
			{
				curX = rect.xMax + 4f;
				return false;
			}
			bool selected = !selectedSection && this.selectedGenes.Contains(geneDef);
			bool overridden = this.leftChosenGroups.Any((GeneLeftChosenGroup x) => x.overriddenGenes.Contains(geneDef));
			Widgets.DrawOptionBackground(rect, selected);
			curX += 4f;
			GeneUIUtility.DrawBiostats(geneDef.biostatCpx, geneDef.biostatMet, geneDef.biostatArc, ref curX, curY, 4f);
			Rect geneRect = new Rect(curX, curY + 4f, GeneCreationDialogBase.GeneSize.x, GeneCreationDialogBase.GeneSize.y);
			GeneUIUtility.DrawGeneDef(geneDef, geneRect, this.inheritable ? GeneType.Endogene : GeneType.Xenogene, () => this.GeneTip(geneDef, selectedSection), false, false, overridden);
			curX += GeneCreationDialogBase.GeneSize.x + 4f;
			if (Mouse.IsOver(rect))
			{
				this.hoveredGene = geneDef;
				this.hoveredAnyGene = true;
			}
			else if (this.hoveredGene != null && geneDef.ConflictsWith(this.hoveredGene))
			{
				Widgets.DrawLightHighlight(rect);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				result = true;
			}
			curX = Mathf.Max(curX, rect.xMax + 4f);
			return result;
		}

		// Token: 0x06011C07 RID: 72711 RVA: 0x00520478 File Offset: 0x0051E678
		private string GeneTip(GeneDef geneDef, bool selectedSection)
		{
			string text = null;
			if (selectedSection)
			{
				if (this.leftChosenGroups.Any((GeneLeftChosenGroup x) => x.leftChosen == geneDef))
				{
					text = Dialog_CreateXenotype.<GeneTip>g__GroupInfo|26_0(this.leftChosenGroups.FirstOrDefault((GeneLeftChosenGroup x) => x.leftChosen == geneDef));
				}
				else if (this.cachedOverriddenGenes.Contains(geneDef))
				{
					text = Dialog_CreateXenotype.<GeneTip>g__GroupInfo|26_0(this.leftChosenGroups.FirstOrDefault((GeneLeftChosenGroup x) => x.overriddenGenes.Contains(geneDef)));
				}
				else if (this.randomChosenGroups.ContainsKey(geneDef))
				{
					text = ("GeneWillBeRandomChosen".Translate() + ":\n" + (from x in this.randomChosenGroups[geneDef]
					select x.label).ToLineList("  - ", true)).Colorize(ColoredText.TipSectionTitleColor);
				}
			}
			if (this.selectedGenes.Contains(geneDef) && geneDef.prerequisite != null && !this.selectedGenes.Contains(geneDef.prerequisite))
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += ("MessageGeneMissingPrerequisite".Translate(geneDef.label).CapitalizeFirst() + ": " + geneDef.prerequisite.LabelCap).Colorize(ColorLibrary.RedReadable);
			}
			if (!text.NullOrEmpty())
			{
				text += "\n\n";
			}
			return text + (this.selectedGenes.Contains(geneDef) ? "ClickToRemove" : "ClickToAdd").Translate().Colorize(ColoredText.SubtleGrayColor);
		}

		// Token: 0x06011C08 RID: 72712 RVA: 0x0052066C File Offset: 0x0051E86C
		protected override void PostXenotypeOnGUI(float curX, float curY)
		{
			TaggedString taggedString = "GenesAreInheritable".Translate();
			TaggedString taggedString2 = "IgnoreRestrictions".Translate();
			float width = Mathf.Max(Text.CalcSize(taggedString).x, Text.CalcSize(taggedString2).x) + 4f + 24f;
			Rect rect = new Rect(curX, curY, width, Text.LineHeight);
			Widgets.CheckboxLabeled(rect, taggedString, ref this.inheritable, false, null, null, false, false);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, "GenesAreInheritableDesc".Translate());
			}
			rect.y += Text.LineHeight;
			bool ignoreRestrictions = this.ignoreRestrictions;
			Widgets.CheckboxLabeled(rect, taggedString2, ref this.ignoreRestrictions, false, null, null, false, false);
			if (ignoreRestrictions != this.ignoreRestrictions)
			{
				if (this.ignoreRestrictions)
				{
					if (!Dialog_CreateXenotype.ignoreRestrictionsConfirmationSent)
					{
						Dialog_CreateXenotype.ignoreRestrictionsConfirmationSent = true;
						Find.WindowStack.Add(new Dialog_MessageBox("IgnoreRestrictionsConfirmation".Translate(), "Yes".Translate(), delegate()
						{
						}, "No".Translate(), delegate()
						{
							this.ignoreRestrictions = false;
						}, null, false, null, null, WindowLayer.Dialog));
					}
				}
				else
				{
					this.selectedGenes.RemoveAll((GeneDef x) => x.biostatArc > 0);
					this.OnGenesChanged();
				}
			}
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, "IgnoreRestrictionsDesc".Translate());
			}
			this.postXenotypeHeight += rect.yMax - curY;
		}

		// Token: 0x06011C09 RID: 72713 RVA: 0x00520830 File Offset: 0x0051EA30
		protected override void OnGenesChanged()
		{
			this.selectedGenes.SortGeneDefs();
			base.OnGenesChanged();
		}

		// Token: 0x06011C0A RID: 72714 RVA: 0x00520844 File Offset: 0x0051EA44
		protected override void DrawSearchRect(Rect rect)
		{
			base.DrawSearchRect(rect);
			if (Widgets.ButtonText(new Rect(rect.xMax - GeneCreationDialogBase.ButSize.x, rect.y, GeneCreationDialogBase.ButSize.x, GeneCreationDialogBase.ButSize.y), "LoadCustom".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_XenotypeList_Load(delegate(CustomXenotype xenotype)
				{
					this.xenotypeName = xenotype.name;
					this.xenotypeNameLocked = true;
					this.selectedGenes.Clear();
					this.selectedGenes.AddRange(xenotype.genes);
					this.inheritable = xenotype.inheritable;
					this.iconDef = xenotype.IconDef;
					this.OnGenesChanged();
					this.ignoreRestrictions = (this.selectedGenes.Any((GeneDef x) => x.biostatArc > 0) || !this.WithinAcceptableBiostatLimits(false));
				}));
			}
			if (Widgets.ButtonText(new Rect(rect.xMax - GeneCreationDialogBase.ButSize.x * 2f - 4f, rect.y, GeneCreationDialogBase.ButSize.x, GeneCreationDialogBase.ButSize.y), "LoadPremade".Translate(), true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (XenotypeDef xenotype2 in from c in DefDatabase<XenotypeDef>.AllDefs
				orderby -c.displayPriority
				select c)
				{
					XenotypeDef xenotype = xenotype2;
					list.Add(new FloatMenuOption(xenotype.LabelCap, delegate()
					{
						this.xenotypeName = xenotype.label;
						this.selectedGenes.Clear();
						this.selectedGenes.AddRange(xenotype.genes);
						this.inheritable = xenotype.inheritable;
						this.OnGenesChanged();
						this.ignoreRestrictions = (this.selectedGenes.Any((GeneDef g) => g.biostatArc > 0) || !this.WithinAcceptableBiostatLimits(false));
					}, xenotype.Icon, XenotypeDef.IconColor, MenuOptionPriority.Default, delegate(Rect r)
					{
						TooltipHandler.TipRegion(r, xenotype.descriptionShort ?? xenotype.description);
					}, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06011C0B RID: 72715 RVA: 0x00520A08 File Offset: 0x0051EC08
		protected override void DoBottomButtons(Rect rect)
		{
			base.DoBottomButtons(rect);
			if (this.leftChosenGroups.Any<GeneLeftChosenGroup>())
			{
				int num = this.leftChosenGroups.Sum((GeneLeftChosenGroup x) => x.overriddenGenes.Count);
				GeneLeftChosenGroup geneLeftChosenGroup = this.leftChosenGroups[0];
				string text = "GenesConflict".Translate() + ": " + "GenesConflictDesc".Translate(geneLeftChosenGroup.leftChosen.Named("FIRST"), geneLeftChosenGroup.overriddenGenes[0].Named("SECOND")).CapitalizeFirst() + ((num > 1) ? (" +" + (num - 1).ToString()) : string.Empty);
				float x2 = Text.CalcSize(text).x;
				GUI.color = ColorLibrary.RedReadable;
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(new Rect(rect.xMax - GeneCreationDialogBase.ButSize.x - x2 - 4f, rect.y, x2, rect.height), text);
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
		}

		// Token: 0x06011C0C RID: 72716 RVA: 0x00520B44 File Offset: 0x0051ED44
		protected override bool CanAccept()
		{
			if (!base.CanAccept())
			{
				return false;
			}
			if (!this.selectedGenes.Any<GeneDef>())
			{
				Messages.Message("MessageNoSelectedGenes".Translate(), null, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (GenFilePaths.AllCustomXenotypeFiles.EnumerableCount() >= 200)
			{
				Messages.Message("MessageTooManyCustomXenotypes", null, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!this.ignoreRestrictions && this.leftChosenGroups.Any<GeneLeftChosenGroup>())
			{
				Messages.Message("MessageConflictingGenesPresent".Translate(), null, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (this.generationRequestIndex >= 0)
			{
				PawnGenerationRequest generationRequest = StartingPawnUtility.GetGenerationRequest(this.generationRequestIndex);
				if (TutorSystem.TutorialMode && generationRequest.MustBeCapableOfViolence)
				{
					if (this.selectedGenes.Any((GeneDef g) => g.disabledWorkTags.HasFlag(WorkTags.Violent)))
					{
						Messages.Message("MessageStartingPawnCapableOfViolence".Translate(), MessageTypeDefOf.RejectInput, false);
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06011C0D RID: 72717 RVA: 0x00520C4C File Offset: 0x0051EE4C
		protected override void Accept()
		{
			IEnumerable<string> warnings = this.GetWarnings();
			if (warnings.Any<string>())
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("XenotypeBreaksLimits".Translate() + ":\n" + warnings.ToLineList("  - ", true) + "\n\n" + "SaveAnyway".Translate(), new Action(this.AcceptInner), false, null, WindowLayer.Dialog));
				return;
			}
			this.AcceptInner();
		}

		// Token: 0x06011C0E RID: 72718 RVA: 0x00520CCC File Offset: 0x0051EECC
		private void AcceptInner()
		{
			CustomXenotype customXenotype = new CustomXenotype();
			CustomXenotype customXenotype2 = customXenotype;
			string xenotypeName = this.xenotypeName;
			customXenotype2.name = ((xenotypeName != null) ? xenotypeName.Trim() : null);
			customXenotype.genes.AddRange(this.selectedGenes);
			customXenotype.inheritable = this.inheritable;
			customXenotype.iconDef = this.iconDef;
			string xenotypeName2 = GenFile.SanitizedFileName(customXenotype.name);
			string absPath = GenFilePaths.AbsFilePathForXenotype(xenotypeName2);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveXenotype(customXenotype, absPath);
			}, "SavingLongEvent", false, null, true, false, null);
			if (this.generationRequestIndex >= 0)
			{
				PawnGenerationRequest generationRequest = StartingPawnUtility.GetGenerationRequest(this.generationRequestIndex);
				generationRequest.ForcedXenotype = null;
				generationRequest.ForcedCustomXenotype = customXenotype;
				StartingPawnUtility.SetGenerationRequest(this.generationRequestIndex, generationRequest);
			}
			Action action = this.callback;
			if (action != null)
			{
				action();
			}
			this.Close(true);
		}

		// Token: 0x06011C0F RID: 72719 RVA: 0x00520DC5 File Offset: 0x0051EFC5
		private IEnumerable<string> GetWarnings()
		{
			if (!this.ignoreRestrictions)
			{
				yield break;
			}
			if (this.arc > 0 && this.inheritable)
			{
				yield return "XenotypeBreaksLimits_Archites".Translate();
			}
			if (this.met > GeneTuning.BiostatRange.TrueMax)
			{
				yield return "XenotypeBreaksLimits_Exceeds".Translate("Metabolism".Translate().Named("STAT"), this.met.Named("VALUE"), GeneTuning.BiostatRange.TrueMax.Named("MAX")).CapitalizeFirst();
			}
			else if (this.met < GeneTuning.BiostatRange.TrueMin)
			{
				yield return "XenotypeBreaksLimits_Below".Translate("Metabolism".Translate().Named("STAT"), this.met.Named("VALUE"), GeneTuning.BiostatRange.TrueMin.Named("MIN")).CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x06011C10 RID: 72720 RVA: 0x00520DD8 File Offset: 0x0051EFD8
		protected override void UpdateSearchResults()
		{
			this.quickSearchWidget.noResultsMatched = false;
			this.matchingGenes.Clear();
			this.matchingCategories.Clear();
			if (!this.quickSearchWidget.filter.Active)
			{
				return;
			}
			foreach (GeneDef geneDef in GeneUtility.GenesInOrder)
			{
				if (!this.selectedGenes.Contains(geneDef))
				{
					if (this.quickSearchWidget.filter.Matches(geneDef.label))
					{
						this.matchingGenes.Add(geneDef);
					}
					if (this.quickSearchWidget.filter.Matches(geneDef.displayCategory.label))
					{
						this.matchingCategories.Add(geneDef.displayCategory);
					}
				}
			}
			this.quickSearchWidget.noResultsMatched = (!this.matchingGenes.Any<GeneDef>() && !this.matchingCategories.Any<GeneCategoryDef>());
		}

		// Token: 0x06011C12 RID: 72722 RVA: 0x00520F04 File Offset: 0x0051F104
		[CompilerGenerated]
		internal static string <GeneTip>g__GroupInfo|26_0(GeneLeftChosenGroup group)
		{
			if (group == null)
			{
				return null;
			}
			return ("GeneLeftmostActive".Translate() + ":\n  - " + group.leftChosen.LabelCap + " (" + "Active".Translate() + ")" + "\n" + (from x in @group.overriddenGenes
			select (x.label + " (" + "Suppressed".Translate() + ")").Colorize(ColorLibrary.RedReadable)).ToLineList("  - ", true)).Colorize(ColoredText.TipSectionTitleColor);
		}

		// Token: 0x0400B71F RID: 46879
		private int generationRequestIndex;

		// Token: 0x0400B720 RID: 46880
		private Action callback;

		// Token: 0x0400B721 RID: 46881
		private List<GeneDef> selectedGenes = new List<GeneDef>();

		// Token: 0x0400B722 RID: 46882
		private bool inheritable;

		// Token: 0x0400B723 RID: 46883
		private bool? selectedCollapsed = new bool?(false);

		// Token: 0x0400B724 RID: 46884
		private HashSet<GeneCategoryDef> matchingCategories = new HashSet<GeneCategoryDef>();

		// Token: 0x0400B725 RID: 46885
		private Dictionary<GeneCategoryDef, bool> collapsedCategories = new Dictionary<GeneCategoryDef, bool>();

		// Token: 0x0400B726 RID: 46886
		private bool hoveredAnyGene;

		// Token: 0x0400B727 RID: 46887
		private GeneDef hoveredGene;

		// Token: 0x0400B728 RID: 46888
		private static bool ignoreRestrictionsConfirmationSent;

		// Token: 0x0400B729 RID: 46889
		private Vector2 selectedScrollPosition;

		// Token: 0x0400B72A RID: 46890
		private const int MaxCustomXenotypes = 200;

		// Token: 0x0400B72B RID: 46891
		private static readonly Color OutlineColorSelected = new Color(1f, 1f, 0.7f, 1f);
	}
}
