using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200190B RID: 6411
	public static class TerrainDefGenerator_Carpet
	{
		// Token: 0x060091BB RID: 37307 RVA: 0x002AE347 File Offset: 0x002AC547
		public static IEnumerable<TerrainDef> ImpliedTerrainDefs(bool hotReload = false)
		{
			IEnumerable<ColorDef> enumerable = from x in DefDatabase<ColorDef>.AllDefs
			where x.colorType == ColorType.Structure
			select x;
			foreach (ColorDef c in enumerable)
			{
				int index = 0;
				foreach (TerrainTemplateDef tp in DefDatabase<TerrainTemplateDef>.AllDefs)
				{
					yield return TerrainDefGenerator_Carpet.CarpetFromBlueprint(tp, c, index, hotReload);
					int num = index;
					index = num + 1;
				}
				IEnumerator<TerrainTemplateDef> enumerator2 = null;
				c = null;
			}
			IEnumerator<ColorDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060091BC RID: 37308 RVA: 0x002AE358 File Offset: 0x002AC558
		public static TerrainDef CarpetFromBlueprint(TerrainTemplateDef tp, ColorDef colorDef, int index, bool hotReload = false)
		{
			string defName = tp.defName + colorDef.defName.Replace("Structure_", "");
			TerrainDef terrainDef = hotReload ? (DefDatabase<TerrainDef>.GetNamed(defName, false) ?? new TerrainDef()) : new TerrainDef();
			terrainDef.defName = defName;
			terrainDef.label = tp.label.Formatted(colorDef.label);
			terrainDef.texturePath = tp.texturePath;
			terrainDef.researchPrerequisites = tp.researchPrerequisites;
			terrainDef.burnedDef = tp.burnedDef;
			terrainDef.costList = tp.costList;
			terrainDef.description = tp.description;
			terrainDef.colorDef = colorDef;
			terrainDef.designatorDropdown = tp.designatorDropdown;
			terrainDef.uiOrder = (float)tp.uiOrder;
			terrainDef.statBases = tp.statBases;
			terrainDef.renderPrecedence = tp.renderPrecedenceStart - index;
			terrainDef.constructionSkillPrerequisite = tp.constructionSkillPrerequisite;
			terrainDef.canGenerateDefaultDesignator = tp.canGenerateDefaultDesignator;
			terrainDef.tags = tp.tags;
			terrainDef.dominantStyleCategory = tp.dominantStyleCategory;
			terrainDef.layerable = true;
			terrainDef.affordances = new List<TerrainAffordanceDef>
			{
				TerrainAffordanceDefOf.Walkable,
				TerrainAffordanceDefOf.Light,
				TerrainAffordanceDefOf.Medium,
				TerrainAffordanceDefOf.Heavy
			};
			terrainDef.designationCategory = DesignationCategoryDefOf.Floors;
			terrainDef.fertility = 0f;
			terrainDef.constructEffect = EffecterDefOf.ConstructDirt;
			terrainDef.pollutionColor = new Color(1f, 1f, 1f, 0.8f);
			terrainDef.pollutionOverlayScale = new Vector2(0.75f, 0.75f);
			terrainDef.pollutionOverlayTexturePath = "Terrain/Surfaces/PollutionFloorSmooth";
			terrainDef.terrainAffordanceNeeded = TerrainAffordanceDefOf.Heavy;
			if (ModsConfig.BiotechActive)
			{
				terrainDef.pollutionShaderType = ShaderTypeDefOf.TerrainFadeRoughLinearBurn;
			}
			return terrainDef;
		}
	}
}
