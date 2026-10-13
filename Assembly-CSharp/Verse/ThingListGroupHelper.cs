using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000451 RID: 1105
	public static class ThingListGroupHelper
	{
		// Token: 0x06001C98 RID: 7320 RVA: 0x0008F6C8 File Offset: 0x0008D8C8
		static ThingListGroupHelper()
		{
			int num = 0;
			foreach (object obj in Enum.GetValues(typeof(ThingRequestGroup)))
			{
				ThingListGroupHelper.AllGroups[num] = (ThingRequestGroup)obj;
				num++;
			}
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x0008F750 File Offset: 0x0008D950
		public static bool Includes(this ThingRequestGroup group, ThingDef def)
		{
			switch (group)
			{
			case ThingRequestGroup.Undefined:
				return false;
			case ThingRequestGroup.Nothing:
				return false;
			case ThingRequestGroup.Everything:
				return true;
			case ThingRequestGroup.HaulableEver:
				return def.EverHaulable;
			case ThingRequestGroup.HaulableAlways:
				return def.alwaysHaulable;
			case ThingRequestGroup.FoodSource:
				return def.IsNutritionGivingIngestible || def.thingClass == typeof(Building_NutrientPasteDispenser);
			case ThingRequestGroup.MechCharger:
				return def.thingClass == typeof(Building_MechCharger);
			case ThingRequestGroup.FoodSourceNotPlantOrTree:
				return (def.IsNutritionGivingIngestible && (def.ingestible.foodType & ~FoodTypeFlags.Plant & ~FoodTypeFlags.Tree) != FoodTypeFlags.None) || def.thingClass == typeof(Building_NutrientPasteDispenser);
			case ThingRequestGroup.Corpse:
				return typeof(Corpse).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Blueprint:
				return def.IsBlueprint;
			case ThingRequestGroup.BuildingArtificial:
				return def.IsBuildingArtificial;
			case ThingRequestGroup.BuildingFrame:
				return def.IsFrame;
			case ThingRequestGroup.Pawn:
				return def.category == ThingCategory.Pawn;
			case ThingRequestGroup.PotentialBillGiver:
				return !def.AllRecipes.NullOrEmpty<RecipeDef>();
			case ThingRequestGroup.Medicine:
				return def.IsMedicine;
			case ThingRequestGroup.Filth:
				return def.filth != null;
			case ThingRequestGroup.AttackTarget:
				return typeof(IAttackTarget).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Weapon:
				return def.IsWeapon;
			case ThingRequestGroup.Apparel:
				return def.IsApparel;
			case ThingRequestGroup.Refuelable:
				return def.HasAssignableCompFrom(typeof(CompRefuelable));
			case ThingRequestGroup.Styleable:
				return def.HasAssignableCompFrom(typeof(CompStyleable));
			case ThingRequestGroup.HaulableEverOrMinifiable:
				return def.EverHaulable || def.Minifiable;
			case ThingRequestGroup.Drug:
				return def.IsDrug;
			case ThingRequestGroup.Shell:
				return def.IsShell;
			case ThingRequestGroup.HarvestablePlant:
				return def.category == ThingCategory.Plant && def.plant.Harvestable;
			case ThingRequestGroup.Fire:
				return typeof(Fire).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Bed:
				return def.IsBed;
			case ThingRequestGroup.Plant:
				return def.category == ThingCategory.Plant;
			case ThingRequestGroup.NonStumpPlant:
				return def.category == ThingCategory.Plant && !def.plant.isStump;
			case ThingRequestGroup.WasteProducer:
				return def.HasAssignableCompFrom(typeof(CompWasteProducer));
			case ThingRequestGroup.Book:
				return def.thingClass.SameOrSubclassOf<Book>();
			case ThingRequestGroup.HoldingPlatformTarget:
				return def.HasAssignableCompFrom(typeof(CompHoldingPlatformTarget));
			case ThingRequestGroup.PsychicRitualSpot:
				return def.HasAssignableCompFrom(typeof(CompPsychicRitualSpot));
			case ThingRequestGroup.EntityHolder:
				return def.HasAssignableCompFrom(typeof(CompEntityHolder));
			case ThingRequestGroup.SubstructureFootprint:
				return ModsConfig.OdysseyActive && def.HasAssignableCompFrom(typeof(CompSubstructureFootprint));
			case ThingRequestGroup.PassengerShuttle:
				return typeof(Building_PassengerShuttle).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Fish:
				return ModsConfig.OdysseyActive && def.thingCategories.NotNullAndContains(ThingCategoryDefOf.Fish);
			case ThingRequestGroup.Construction:
				return def.IsBlueprint || def.IsFrame;
			case ThingRequestGroup.HasGUIOverlay:
				return def.drawGUIOverlay;
			case ThingRequestGroup.MinifiedThing:
				return typeof(MinifiedThing).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Grave:
				return typeof(Building_Grave).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Art:
				return def.HasAssignableCompFrom(typeof(CompArt));
			case ThingRequestGroup.ThingHolder:
				return def.ThisOrAnyCompIsThingHolder();
			case ThingRequestGroup.ActiveTransporter:
				return typeof(IActiveTransporter).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Transporter:
				return def.HasAssignableCompFrom(typeof(CompTransporter));
			case ThingRequestGroup.LongRangeMineralScanner:
				return def.HasAssignableCompFrom(typeof(CompLongRangeMineralScanner));
			case ThingRequestGroup.AffectsSky:
				return def.HasAssignableCompFrom(typeof(CompAffectsSky));
			case ThingRequestGroup.WindSource:
				return def.HasAssignableCompFrom(typeof(CompWindSource));
			case ThingRequestGroup.AlwaysFlee:
				return def.alwaysFlee;
			case ThingRequestGroup.ResearchBench:
				return typeof(Building_ResearchBench).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Facility:
				return def.HasAssignableCompFrom(typeof(CompFacility));
			case ThingRequestGroup.AffectedByFacilities:
				return def.HasAssignableCompFrom(typeof(CompAffectedByFacilities));
			case ThingRequestGroup.CreatesInfestations:
				return def.HasAssignableCompFrom(typeof(CompCreatesInfestations));
			case ThingRequestGroup.WithCustomRectForSelector:
				return def.hasCustomRectForSelector;
			case ThingRequestGroup.ProjectileInterceptor:
				return def.HasAssignableCompFrom(typeof(CompProjectileInterceptor));
			case ThingRequestGroup.ConditionCauser:
				return def.GetCompProperties<CompProperties_CausesGameCondition>() != null;
			case ThingRequestGroup.MusicalInstrument:
				return typeof(Building_MusicalInstrument).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.MusicSource:
				return def.HasAssignableCompFrom(typeof(CompPlaysMusic));
			case ThingRequestGroup.Throne:
				return typeof(Building_Throne).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.FoodDispenser:
				return def.IsFoodDispenser;
			case ThingRequestGroup.Projectile:
				return def.projectile != null;
			case ThingRequestGroup.Chunk:
				return !def.thingCategories.NullOrEmpty<ThingCategoryDef>() && (def.thingCategories.Contains(ThingCategoryDefOf.Chunks) || def.thingCategories.Contains(ThingCategoryDefOf.StoneChunks));
			case ThingRequestGroup.MeditationFocus:
				return def.HasAssignableCompFrom(typeof(CompMeditationFocus));
			case ThingRequestGroup.Seed:
				return def.GetCompProperties<CompProperties_Plantable>() != null;
			case ThingRequestGroup.DryadSpawner:
				return def.GetCompProperties<CompProperties_TreeConnection>() != null;
			case ThingRequestGroup.Studiable:
				return def.HasAssignableCompFrom(typeof(CompStudiable)) || ThingRequestGroup.EntityHolder.Includes(def);
			case ThingRequestGroup.Suppressable:
				return def.HasAssignableCompFrom(typeof(CompActivity)) || ThingRequestGroup.EntityHolder.Includes(def);
			case ThingRequestGroup.ActionDelay:
				return typeof(SignalAction_Delay).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.MechGestator:
				return typeof(Building_MechGestator).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.GenepackHolder:
				return def.GetCompProperties<CompProperties_GenepackContainer>() != null;
			case ThingRequestGroup.Dissolving:
				return def.HasAssignableCompFrom(typeof(CompDissolution));
			case ThingRequestGroup.SubcoreScanner:
				return typeof(Building_SubcoreScanner).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.BossgroupCaller:
				return def.GetCompProperties<CompProperties_Useable_CallBossgroup>() != null;
			case ThingRequestGroup.WakeUpDormant:
				return def.HasAssignableCompFrom(typeof(CompWakeUpDormant));
			case ThingRequestGroup.Atomizer:
				return def.HasAssignableCompFrom(typeof(CompAtomizer));
			case ThingRequestGroup.PowerTrader:
				return def.HasAssignableCompFrom(typeof(CompPowerTrader));
			case ThingRequestGroup.MapPortal:
				return typeof(MapPortal).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.BuildingGroundSpawner:
				return typeof(BuildingGroundSpawner).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.Hackable:
				return def.HasAssignableCompFrom(typeof(CompHackable));
			case ThingRequestGroup.Door:
				return def.IsDoor;
			case ThingRequestGroup.CostProvider:
				return typeof(IPathFindCostProvider).IsAssignableFrom(def.thingClass);
			case ThingRequestGroup.ApparelSource:
				return typeof(IHaulSource).IsAssignableFrom(def.thingClass);
			default:
				throw new ArgumentException("group");
			}
		}

		// Token: 0x040018C5 RID: 6341
		public static readonly ThingRequestGroup[] AllGroups = new ThingRequestGroup[Enum.GetValues(typeof(ThingRequestGroup)).Length];
	}
}
