using System;
using System.Xml;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000C5B RID: 3163
	public class BackCompatibilityConverter_1_4 : BackCompatibilityConverter
	{
		// Token: 0x060050F7 RID: 20727 RVA: 0x001A403C File Offset: 0x001A223C
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer == 0 || (majorVer == 1 && minorVer <= 4);
		}

		// Token: 0x060050F8 RID: 20728 RVA: 0x001A4050 File Offset: 0x001A2250
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (defType == typeof(ThingDef) && defName == "AncientMechDetritus")
			{
				return "ChunkMechanoidSlag";
			}
			return null;
		}

		// Token: 0x060050F9 RID: 20729 RVA: 0x001A4078 File Offset: 0x001A2278
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			if (providedClassName == "QuestPart_Filter_ThingStudied")
			{
				return typeof(QuestPart_Filter_ThingAnalyzed);
			}
			if (providedClassName == "Building" && node["def"].InnerText == "AncientMechDetritus")
			{
				return typeof(Thing);
			}
			return null;
		}

		// Token: 0x060050FA RID: 20730 RVA: 0x001A40D4 File Offset: 0x001A22D4
		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Game game = obj as Game;
				if (game != null)
				{
					game.relationshipRecords = new RelationshipRecords();
					if (game.readingPolicyDatabase == null)
					{
						game.readingPolicyDatabase = new ReadingPolicyDatabase();
					}
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Thing thing = obj as Thing;
				if (thing != null)
				{
					if (thing.def == ThingDefOf.ChunkMechanoidSlag)
					{
						thing.HitPoints = thing.MaxHitPoints;
					}
					if (thing.def == ThingDefOf.VitalsMonitor && thing.Rotation.IsHorizontal)
					{
						thing.Rotation = thing.Rotation.Opposite;
					}
				}
				Pawn pawn = obj as Pawn;
				if (pawn != null)
				{
					if (pawn.reading == null && pawn.RaceProps.Humanlike)
					{
						pawn.reading = new Pawn_ReadingTracker(pawn);
					}
					if (pawn.workSettings != null && pawn.workSettings.Initialized && ModsConfig.AnomalyActive && !pawn.WorkTypeIsDisabled(WorkTypeDefOf.DarkStudy) && pawn.skills.AverageOfRelevantSkillsFor(WorkTypeDefOf.DarkStudy) >= 6f)
					{
						pawn.workSettings.SetPriority(WorkTypeDefOf.DarkStudy, 3);
					}
				}
				Map map = obj as Map;
				if (map != null)
				{
					map.enrouteManager = new EnrouteManager(map);
					if (map.generatorDef == null)
					{
						map.generatorDef = MapGeneratorDefOf.Base_Player;
					}
				}
				PlaySettings playSettings = obj as PlaySettings;
				if (playSettings != null)
				{
					playSettings.defaultCareForColonist = MedicalCareCategory.Best;
					playSettings.defaultCareForPrisoner = MedicalCareCategory.HerbalOrWorse;
					playSettings.defaultCareForSlave = MedicalCareCategory.HerbalOrWorse;
					playSettings.defaultCareForTamedAnimal = MedicalCareCategory.HerbalOrWorse;
					playSettings.defaultCareForFriendlyFaction = MedicalCareCategory.HerbalOrWorse;
					playSettings.defaultCareForNeutralFaction = MedicalCareCategory.HerbalOrWorse;
					playSettings.defaultCareForHostileFaction = MedicalCareCategory.HerbalOrWorse;
					playSettings.defaultCareForNoFaction = MedicalCareCategory.HerbalOrWorse;
					playSettings.defaultCareForWildlife = MedicalCareCategory.HerbalOrWorse;
					playSettings.defaultCareForEntities = MedicalCareCategory.NoMeds;
					playSettings.defaultCareForGhouls = MedicalCareCategory.NoMeds;
				}
			}
		}
	}
}
