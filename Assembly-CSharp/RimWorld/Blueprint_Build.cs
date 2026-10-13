using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200294C RID: 10572
	public class Blueprint_Build : Blueprint, IHaulEnroute, ILoadReferenceable
	{
		// Token: 0x17002332 RID: 9010
		// (get) Token: 0x0600E7AB RID: 59307 RVA: 0x00437E44 File Offset: 0x00436044
		public override string Label
		{
			get
			{
				string text = this.def.entityDefToBuild.label;
				if (base.StyleSourcePrecept != null)
				{
					text = base.StyleSourcePrecept.TransformThingLabel(text);
				}
				if (this.stuffToUse != null)
				{
					return "ThingMadeOfStuffLabel".Translate(this.stuffToUse.LabelAsStuff, text) + "BlueprintLabelExtra".Translate();
				}
				return text + "BlueprintLabelExtra".Translate();
			}
		}

		// Token: 0x17002333 RID: 9011
		// (get) Token: 0x0600E7AC RID: 59308 RVA: 0x00437EC9 File Offset: 0x004360C9
		protected override float WorkTotal
		{
			get
			{
				return this.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, this.stuffToUse);
			}
		}

		// Token: 0x17002334 RID: 9012
		// (get) Token: 0x0600E7AD RID: 59309 RVA: 0x00437EE6 File Offset: 0x004360E6
		public ThingDef BuildDef
		{
			get
			{
				return this.def.entityDefToBuild as ThingDef;
			}
		}

		// Token: 0x0600E7AE RID: 59310 RVA: 0x00437EF8 File Offset: 0x004360F8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.stuffToUse, "stuffToUse");
			Scribe_Defs.Look<ThingStyleDef>(ref this.selectedStyleDef, "selectedStyleDef");
			Scribe_Values.Look<bool>(ref this.styleOverridden, "styleOverridden", false, false);
			Scribe_Values.Look<ColorInt?>(ref this.glowerColorOverride, "glowerColorOverride", null, false);
		}

		// Token: 0x0600E7AF RID: 59311 RVA: 0x00437F57 File Offset: 0x00436157
		public override ThingStyleDef EntityToBuildStyle()
		{
			return this.selectedStyleDef ?? this.StyleDef;
		}

		// Token: 0x0600E7B0 RID: 59312 RVA: 0x00437F69 File Offset: 0x00436169
		public override BuildableDef EntityToBuild()
		{
			return this.def.entityDefToBuild;
		}

		// Token: 0x0600E7B1 RID: 59313 RVA: 0x00437F76 File Offset: 0x00436176
		public override ThingDef EntityToBuildStuff()
		{
			return this.stuffToUse;
		}

		// Token: 0x0600E7B2 RID: 59314 RVA: 0x00437F7E File Offset: 0x0043617E
		public override List<ThingDefCountClass> TotalMaterialCost()
		{
			return this.def.entityDefToBuild.CostListAdjusted(this.stuffToUse, true);
		}

		// Token: 0x0600E7B3 RID: 59315 RVA: 0x00437F98 File Offset: 0x00436198
		protected override Thing MakeSolidThing(out bool shouldSelect)
		{
			Frame frame = (Frame)ThingMaker.MakeThing(this.def.entityDefToBuild.frameDef, this.stuffToUse);
			frame.StyleSourcePrecept = base.StyleSourcePrecept;
			frame.StyleDef = this.StyleDef;
			frame.glowerColorOverride = this.glowerColorOverride;
			shouldSelect = false;
			base.Map.enrouteManager.SendReservations(this, frame);
			return frame;
		}

		// Token: 0x0600E7B4 RID: 59316 RVA: 0x00438000 File Offset: 0x00436200
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Command command = BuildCopyCommandUtility.BuildCopyCommand(this.def.entityDefToBuild, this.stuffToUse, base.StyleSourcePrecept as Precept_Building, this.selectedStyleDef ?? this.StyleDef, this.styleOverridden, this.glowerColorOverride);
			if (command != null)
			{
				yield return command;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				if (ModsConfig.IdeologyActive && Find.IdeoManager.classicMode)
				{
					BuildableDef entityDefToBuild = this.def.entityDefToBuild;
					ThingDef thingDef = entityDefToBuild as ThingDef;
					if (thingDef != null && thingDef.CanBeStyled())
					{
						Color stuffColor = (this.stuffToUse == null) ? Color.white : this.def.entityDefToBuild.GetColorForStuff(this.stuffToUse);
						Command_Action command_Action = new Command_Action
						{
							defaultLabel = "ChangeStyle".Translate().CapitalizeFirst(),
							defaultDesc = "ChangeStyleDesc".Translate(),
							icon = Blueprint_Build.ChangeStyleTex.Texture,
							Order = 15f,
							action = delegate()
							{
								List<FloatMenuOption> list = new List<FloatMenuOption>();
								list.Add(new FloatMenuOption("Basic".Translate().CapitalizeFirst(), delegate()
								{
									this.ChangeStyleOfAllSelected(this.def.entityDefToBuild, null);
								}, Widgets.GetIconFor(thingDef, this.Stuff, null, null), stuffColor, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
								foreach (StyleCategoryDef styleCategoryDef in thingDef.RelevantStyleCategories)
								{
									foreach (ThingDefStyle thingDefStyle in styleCategoryDef.thingDefStyles)
									{
										if (thingDefStyle.ThingDef == thingDef)
										{
											ThingDefStyle tdStyle = thingDefStyle;
											list.Add(new FloatMenuOption(styleCategoryDef.LabelCap, delegate()
											{
												this.ChangeStyleOfAllSelected(this.def.entityDefToBuild, tdStyle.StyleDef);
											}, Widgets.GetIconFor(thingDef, this.Stuff, tdStyle.StyleDef, null), stuffColor, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
											break;
										}
									}
								}
								if (list.Any<FloatMenuOption>())
								{
									Find.WindowStack.Add(new FloatMenu(list));
								}
							}
						};
						if (!thingDef.RelevantStyleCategories.Any<StyleCategoryDef>())
						{
							command_Action.Disable("ChangeStyleDisabledNoCategories".Translate());
						}
						yield return command_Action;
					}
				}
				foreach (Command command2 in BuildRelatedCommandUtility.RelatedBuildCommands(this.def.entityDefToBuild))
				{
					yield return command2;
				}
				IEnumerator<Command> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600E7B5 RID: 59317 RVA: 0x00438010 File Offset: 0x00436210
		private void ChangeStyleOfAllSelected(BuildableDef buildable, ThingStyleDef styleDef)
		{
			foreach (object obj in Find.Selector.SelectedObjects)
			{
				Blueprint_Build blueprint_Build = obj as Blueprint_Build;
				if (blueprint_Build != null && blueprint_Build.def.entityDefToBuild == buildable)
				{
					blueprint_Build.StyleDef = styleDef;
					blueprint_Build.styleOverridden = true;
					blueprint_Build.DirtyMapMesh(blueprint_Build.Map);
				}
			}
		}

		// Token: 0x0600E7B6 RID: 59318 RVA: 0x00438090 File Offset: 0x00436290
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.Vanish && !this.def.IsBlueprint)
			{
				foreach (Thing thing in GenConstruct.GetAttachedBuildings(this))
				{
					thing.DeSpawn(mode);
				}
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600E7B7 RID: 59319 RVA: 0x004380F8 File Offset: 0x004362F8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine("ContainedResources".Translate() + ":");
			bool flag = true;
			foreach (ThingDefCountClass thingDefCountClass in this.TotalMaterialCost())
			{
				if (!flag)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(thingDefCountClass.thingDef.LabelCap + ": 0 / " + thingDefCountClass.count.ToString());
				flag = false;
			}
			ThingStyleDef styleDef = this.StyleDef;
			if (((styleDef != null) ? styleDef.Category : null) != null && base.StyleSourcePrecept == null)
			{
				stringBuilder.AppendInNewLine("Style".Translate() + ": " + this.StyleDef.Category.LabelCap);
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x04009AA3 RID: 39587
		public ThingDef stuffToUse;

		// Token: 0x04009AA4 RID: 39588
		public ThingStyleDef selectedStyleDef;

		// Token: 0x04009AA5 RID: 39589
		public bool styleOverridden;

		// Token: 0x04009AA6 RID: 39590
		public ColorInt? glowerColorOverride;

		// Token: 0x04009AA7 RID: 39591
		private static readonly CachedTexture ChangeStyleTex = new CachedTexture("UI/Gizmos/ChangeStyle");
	}
}
