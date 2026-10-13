using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001919 RID: 6425
	public class ThingDefGenerator_Neurotrainer
	{
		// Token: 0x06009205 RID: 37381 RVA: 0x002B0EDF File Offset: 0x002AF0DF
		public static IEnumerable<ThingDef> ImpliedThingDefs(bool hotReload = false)
		{
			foreach (AbilityDef abilityDef in DefDatabase<AbilityDef>.AllDefs)
			{
				if (abilityDef.IsPsycast)
				{
					ThingDef thingDef = ThingDefGenerator_Neurotrainer.BaseNeurotrainer(ThingDefGenerator_Neurotrainer.PsytrainerDefPrefix + "_" + abilityDef.defName, hotReload);
					thingDef.label = "PsycastNeurotrainerLabel".Translate(abilityDef.label);
					thingDef.description = "PsycastNeurotrainerDescription".Translate(abilityDef.Named("PSYCAST"), abilityDef.description.Named("PSYCASTDESCRIPTION"));
					thingDef.comps.Add(new CompProperties_Usable
					{
						useJob = JobDefOf.UseNeurotrainer,
						useLabel = "PsycastNeurotrainerUseLabel".Translate(abilityDef.label),
						showUseGizmo = true
					});
					thingDef.comps.Add(new CompProperties_UseEffect_GainAbility
					{
						ability = abilityDef
					});
					thingDef.statBases.Add(new StatModifier
					{
						stat = StatDefOf.MarketValue,
						value = Mathf.Round(Mathf.Lerp(100f, 1000f, (float)abilityDef.level / 6f))
					});
					thingDef.thingCategories = new List<ThingCategoryDef>
					{
						ThingCategoryDefOf.NeurotrainersPsycast
					};
					thingDef.thingSetMakerTags = new List<string>
					{
						"RewardStandardLowFreq"
					};
					thingDef.modContentPack = abilityDef.modContentPack;
					thingDef.descriptionHyperlinks = new List<DefHyperlink>
					{
						new DefHyperlink(abilityDef)
					};
					yield return thingDef;
				}
			}
			IEnumerator<AbilityDef> enumerator = null;
			foreach (SkillDef skillDef in DefDatabase<SkillDef>.AllDefs)
			{
				ThingDef thingDef2 = ThingDefGenerator_Neurotrainer.BaseNeurotrainer(ThingDefGenerator_Neurotrainer.NeurotrainerDefPrefix + "_" + skillDef.defName, hotReload);
				thingDef2.label = "SkillNeurotrainerLabel".Translate(skillDef.label);
				thingDef2.description = "SkillNeurotrainerDescription".Translate();
				thingDef2.comps.Add(new CompProperties_Usable
				{
					useJob = JobDefOf.UseNeurotrainer,
					useLabel = "SkillNeurotrainerUseLabel".Translate(skillDef.label),
					showUseGizmo = true
				});
				thingDef2.comps.Add(new CompProperties_UseEffect_LearnSkill
				{
					skill = skillDef
				});
				thingDef2.statBases.Add(new StatModifier
				{
					stat = StatDefOf.MarketValue,
					value = 750f
				});
				thingDef2.thingCategories = new List<ThingCategoryDef>
				{
					ThingCategoryDefOf.NeurotrainersSkill
				};
				thingDef2.thingSetMakerTags = new List<string>
				{
					"RewardStandardMidFreq",
					"SkillNeurotrainer"
				};
				thingDef2.modContentPack = skillDef.modContentPack;
				yield return thingDef2;
			}
			IEnumerator<SkillDef> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x06009206 RID: 37382 RVA: 0x002B0EF0 File Offset: 0x002AF0F0
		private static ThingDef BaseNeurotrainer(string defName, bool hotReload = false)
		{
			ThingDef thingDef = hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, false) ?? new ThingDef()) : new ThingDef();
			thingDef.defName = defName;
			thingDef.category = ThingCategory.Item;
			thingDef.selectable = true;
			thingDef.thingClass = typeof(ThingWithComps);
			thingDef.comps = new List<CompProperties>
			{
				new CompProperties_UseEffectPlaySound
				{
					soundOnUsed = SoundDefOf.MechSerumUsed
				},
				new CompProperties_UseEffectDestroySelf
				{
					compClass = typeof(CompUseEffect_DestroySelf)
				},
				new CompProperties_Forbiddable()
			};
			thingDef.graphicData = new GraphicData
			{
				texPath = "Things/Item/Special/MechSerumNeurotrainer",
				graphicClass = typeof(Graphic_Single)
			};
			thingDef.drawGUIOverlay = false;
			thingDef.statBases = new List<StatModifier>
			{
				new StatModifier
				{
					stat = StatDefOf.MaxHitPoints,
					value = 80f
				},
				new StatModifier
				{
					stat = StatDefOf.Mass,
					value = 0.2f
				},
				new StatModifier
				{
					stat = StatDefOf.DeteriorationRate,
					value = 2f
				},
				new StatModifier
				{
					stat = StatDefOf.Flammability,
					value = 0.2f
				}
			};
			thingDef.techLevel = TechLevel.Ultra;
			thingDef.altitudeLayer = AltitudeLayer.Item;
			thingDef.alwaysHaulable = true;
			thingDef.rotatable = false;
			thingDef.pathCost = 14;
			thingDef.tradeTags = new List<string>
			{
				"ExoticMisc"
			};
			thingDef.stackLimit = 1;
			thingDef.tradeNeverStack = true;
			thingDef.forceDebugSpawnable = true;
			thingDef.drawerType = DrawerType.MapMeshOnly;
			return thingDef;
		}

		// Token: 0x04005D1D RID: 23837
		public static string NeurotrainerDefPrefix = "Neurotrainer";

		// Token: 0x04005D1E RID: 23838
		public static string PsytrainerDefPrefix = "Psytrainer";

		// Token: 0x04005D1F RID: 23839
		private const int MaxAbilityLevel = 6;
	}
}
