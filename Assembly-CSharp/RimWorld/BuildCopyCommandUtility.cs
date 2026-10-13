using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200351A RID: 13594
	public static class BuildCopyCommandUtility
	{
		// Token: 0x06013159 RID: 78169 RVA: 0x005A9208 File Offset: 0x005A7408
		public static Command BuildCopyCommand(BuildableDef buildable, ThingDef stuff, Precept_Building sourcePrecept, ThingStyleDef style, bool styleOverridden, ColorInt? glowerColorOverride = null)
		{
			return BuildCopyCommandUtility.BuildCommand(buildable, stuff, sourcePrecept, style, styleOverridden, "CommandBuildCopy".Translate(), "CommandBuildCopyDesc".Translate(), true, glowerColorOverride);
		}

		// Token: 0x0601315A RID: 78170 RVA: 0x005A9244 File Offset: 0x005A7444
		public static Command BuildCommand(BuildableDef buildable, ThingDef stuff = null, Precept_Building sourcePrecept = null, ThingStyleDef style = null, bool styleOverridden = false, string label = null, string description = null, bool allowHotKey = false, ColorInt? glowerColorOverride = null)
		{
			if (label == null)
			{
				label = buildable.label;
			}
			if (description == null)
			{
				description = buildable.description;
			}
			Designator_Build des = BuildCopyCommandUtility.FindAllowedDesignator(buildable, true);
			if (des == null)
			{
				return null;
			}
			if (buildable.MadeFromStuff && stuff == null)
			{
				return des;
			}
			Command_Action command_Action = new Command_Action();
			command_Action.action = delegate()
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				Find.DesignatorManager.Select(des);
				des.glowerColorOverride = glowerColorOverride;
				des.SetTemporaryVars(stuff, styleOverridden);
			};
			command_Action.defaultLabel = label;
			command_Action.defaultDesc = description;
			ThingDef stuffDefRaw = des.StuffDefRaw;
			des.SetStuffDef(stuff);
			des.styleDef = style;
			command_Action.icon = des.ResolvedIcon(style);
			command_Action.iconProportions = des.iconProportions;
			command_Action.iconDrawScale = des.iconDrawScale;
			command_Action.iconTexCoords = des.iconTexCoords;
			command_Action.iconAngle = des.iconAngle;
			command_Action.iconOffset = des.iconOffset;
			command_Action.Order = 10f;
			command_Action.SetColorOverride(des.IconDrawColor);
			des.sourcePrecept = sourcePrecept;
			des.SetStuffDef(stuffDefRaw);
			if (buildable.uiIconMaterial != null)
			{
				command_Action.overrideMaterial = (des.overrideMaterial = buildable.uiIconMaterial);
			}
			command_Action.defaultIconColor = ((stuff != null) ? buildable.GetColorForStuff(stuff) : buildable.uiIconColor);
			if (allowHotKey)
			{
				command_Action.hotKey = KeyBindingDefOf.Misc11;
			}
			return command_Action;
		}

		// Token: 0x0601315B RID: 78171 RVA: 0x005A93FC File Offset: 0x005A75FC
		public static Designator_Build FindAllowedDesignator(BuildableDef buildable, bool mustBeVisible = true)
		{
			if (Current.Game != null)
			{
				if (BuildCopyCommandUtility.cache.ContainsKey(buildable))
				{
					if (mustBeVisible)
					{
						Designator_Build designator_Build = BuildCopyCommandUtility.cache[buildable];
						if (designator_Build == null || !designator_Build.Visible)
						{
							return null;
						}
					}
					return BuildCopyCommandUtility.cache[buildable];
				}
			}
			else
			{
				BuildCopyCommandUtility.cache.Clear();
			}
			List<DesignationCategoryDef> allDefsListForReading = DefDatabase<DesignationCategoryDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				foreach (Designator designator in allDefsListForReading[i].AllResolvedAndIdeoDesignators)
				{
					Designator_Build designator_Build2 = BuildCopyCommandUtility.FindAllowedDesignatorRecursive(designator, buildable, false);
					if (designator_Build2 != null)
					{
						BuildCopyCommandUtility.cache.TryAdd(buildable, designator_Build2);
						Designator_Build result;
						if (mustBeVisible)
						{
							Designator_Build designator_Build3 = BuildCopyCommandUtility.cache[buildable];
							if (designator_Build3 == null || !designator_Build3.Visible)
							{
								result = null;
								goto IL_AA;
							}
						}
						result = designator_Build2;
						IL_AA:
						return result;
					}
				}
			}
			BuildCopyCommandUtility.cache.TryAdd(buildable, null);
			return null;
		}

		// Token: 0x0601315C RID: 78172 RVA: 0x005A94FC File Offset: 0x005A76FC
		public static void ClearCache()
		{
			BuildCopyCommandUtility.cache.Clear();
		}

		// Token: 0x0601315D RID: 78173 RVA: 0x005A9508 File Offset: 0x005A7708
		public static Designator FindAllowedDesignatorRoot(BuildableDef buildable, bool mustBeVisible = true)
		{
			List<Designator> allResolvedDesignators = buildable.designationCategory.AllResolvedDesignators;
			for (int i = 0; i < allResolvedDesignators.Count; i++)
			{
				if (BuildCopyCommandUtility.FindAllowedDesignatorRecursive(allResolvedDesignators[i], buildable, mustBeVisible) != null)
				{
					return allResolvedDesignators[i];
				}
			}
			return null;
		}

		// Token: 0x0601315E RID: 78174 RVA: 0x005A954C File Offset: 0x005A774C
		private static Designator_Build FindAllowedDesignatorRecursive(Designator designator, BuildableDef buildable, bool mustBeVisible)
		{
			if (!Current.Game.Rules.DesignatorAllowed(designator))
			{
				return null;
			}
			if (mustBeVisible && !designator.Visible)
			{
				return null;
			}
			Designator_Build designator_Build = designator as Designator_Build;
			if (designator_Build != null && designator_Build.PlacingDef == buildable)
			{
				return designator_Build;
			}
			Designator_Dropdown designator_Dropdown = designator as Designator_Dropdown;
			if (designator_Dropdown != null)
			{
				for (int i = 0; i < designator_Dropdown.Elements.Count; i++)
				{
					Designator_Build designator_Build2 = BuildCopyCommandUtility.FindAllowedDesignatorRecursive(designator_Dropdown.Elements[i], buildable, mustBeVisible);
					if (designator_Build2 != null)
					{
						return designator_Build2;
					}
				}
			}
			return null;
		}

		// Token: 0x0400C5E2 RID: 50658
		private static readonly Dictionary<BuildableDef, Designator_Build> cache = new Dictionary<BuildableDef, Designator_Build>();
	}
}
