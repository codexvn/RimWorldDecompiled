using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000C49 RID: 3145
	public static class BackCompatibility
	{
		// Token: 0x06005095 RID: 20629 RVA: 0x0019ECA0 File Offset: 0x0019CEA0
		public static bool IsSaveCompatibleWith(string version)
		{
			if (VersionControl.MajorFromVersionString(version) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(version) == VersionControl.CurrentMinor)
			{
				return true;
			}
			if (VersionControl.MajorFromVersionString(version) >= 1 && VersionControl.MajorFromVersionString(version) == VersionControl.CurrentMajor && VersionControl.MinorFromVersionString(version) <= VersionControl.CurrentMinor)
			{
				return true;
			}
			if (VersionControl.MajorFromVersionString(version) == 0 && VersionControl.CurrentMajor == 0)
			{
				int num = VersionControl.MinorFromVersionString(version);
				int currentMinor = VersionControl.CurrentMinor;
				for (int i = 0; i < BackCompatibility.SaveCompatibleMinorVersions.Length; i++)
				{
					if (BackCompatibility.SaveCompatibleMinorVersions[i].First == num && BackCompatibility.SaveCompatibleMinorVersions[i].Second == currentMinor)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005096 RID: 20630 RVA: 0x0019ED48 File Offset: 0x0019CF48
		public static void PreLoadSavegame(string loadingVersion)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(true))
				{
					try
					{
						BackCompatibility.conversionChain[i].PreLoadSavegame(loadingVersion);
					}
					catch (Exception ex)
					{
						string str = "Error in PreLoadSavegame of ";
						Type type = BackCompatibility.conversionChain[i].GetType();
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}

		// Token: 0x06005097 RID: 20631 RVA: 0x0019EDE4 File Offset: 0x0019CFE4
		public static void PostLoadSavegame(string loadingVersion)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(true))
				{
					try
					{
						BackCompatibility.conversionChain[i].PostLoadSavegame(loadingVersion);
					}
					catch (Exception ex)
					{
						string str = "Error in PostLoadSavegame of ";
						Type type = BackCompatibility.conversionChain[i].GetType();
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}

		// Token: 0x06005098 RID: 20632 RVA: 0x0019EE80 File Offset: 0x0019D080
		public static string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (Scribe.mode != LoadSaveMode.Inactive && VersionControl.CurrentBuild == ScribeMetaHeaderUtility.loadedGameVersionBuild && !Application.isEditor)
			{
				return defName;
			}
			if (GenDefDatabase.GetDefSilentFail(defType, defName, false) != null)
			{
				return defName;
			}
			string text = defName;
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (Scribe.mode == LoadSaveMode.Inactive || BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						string text2 = BackCompatibility.conversionChain[i].BackCompatibleDefName(defType, text, forDefInjections, node);
						if (text2 != null)
						{
							text = text2;
						}
					}
					catch (Exception ex)
					{
						string str = "Error in BackCompatibleDefName of ";
						Type type = BackCompatibility.conversionChain[i].GetType();
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			return text;
		}

		// Token: 0x06005099 RID: 20633 RVA: 0x0019EF58 File Offset: 0x0019D158
		public static object BackCompatibleEnum(Type enumType, string enumName)
		{
			if (enumType == typeof(QualityCategory))
			{
				if (enumName == "Shoddy")
				{
					return QualityCategory.Poor;
				}
				if (enumName == "Superior")
				{
					return QualityCategory.Excellent;
				}
			}
			return null;
		}

		// Token: 0x0600509A RID: 20634 RVA: 0x0019EF98 File Offset: 0x0019D198
		public static Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			if (VersionControl.CurrentBuild == ScribeMetaHeaderUtility.loadedGameVersionBuild && !Application.isEditor)
			{
				return GenTypes.GetTypeInAnyAssembly(providedClassName, null);
			}
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						Type backCompatibleType = BackCompatibility.conversionChain[i].GetBackCompatibleType(baseType, providedClassName, node);
						if (backCompatibleType != null)
						{
							return backCompatibleType;
						}
					}
					catch (Exception ex)
					{
						string str = "Error in GetBackCompatibleType of ";
						Type type = BackCompatibility.conversionChain[i].GetType();
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			return GenTypes.GetTypeInAnyAssembly(providedClassName, null);
		}

		// Token: 0x0600509B RID: 20635 RVA: 0x0019F068 File Offset: 0x0019D268
		public static Type GetBackCompatibleTypeDirect(Type baseType, string providedClassName)
		{
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToVersion(VersionControl.CurrentMajor, VersionControl.CurrentMinor))
				{
					try
					{
						Type backCompatibleType = BackCompatibility.conversionChain[i].GetBackCompatibleType(baseType, providedClassName, null);
						if (backCompatibleType != null)
						{
							return backCompatibleType;
						}
					}
					catch (Exception ex)
					{
						string str = "Error in GetBackCompatibleType of ";
						Type type = BackCompatibility.conversionChain[i].GetType();
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			return GenTypes.GetTypeInAnyAssembly(providedClassName, null);
		}

		// Token: 0x0600509C RID: 20636 RVA: 0x0019F12C File Offset: 0x0019D32C
		public static int GetBackCompatibleBodyPartIndex(BodyDef body, int index)
		{
			if (VersionControl.CurrentBuild == ScribeMetaHeaderUtility.loadedGameVersionBuild && !Application.isEditor)
			{
				return index;
			}
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						index = BackCompatibility.conversionChain[i].GetBackCompatibleBodyPartIndex(body, index);
					}
					catch (Exception ex)
					{
						string str = "Error in GetBackCompatibleBodyPartIndex of ";
						string str2 = (body != null) ? body.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			return index;
		}

		// Token: 0x0600509D RID: 20637 RVA: 0x0019F1D0 File Offset: 0x0019D3D0
		public static bool WasDefRemoved(string defName, Type type)
		{
			foreach (Tuple<string, Type> tuple in BackCompatibility.RemovedDefs)
			{
				if (tuple.Item1 == defName && tuple.Item2 == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600509E RID: 20638 RVA: 0x0019F240 File Offset: 0x0019D440
		public static void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				return;
			}
			if (VersionControl.CurrentBuild == ScribeMetaHeaderUtility.loadedGameVersionBuild && !Application.isEditor)
			{
				return;
			}
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						BackCompatibility.conversionChain[i].PostExposeData(obj);
					}
					catch (Exception ex)
					{
						string str = "Error in PostExposeData of ";
						Type type = BackCompatibility.conversionChain[i].GetType();
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}

		// Token: 0x0600509F RID: 20639 RVA: 0x0019F2F8 File Offset: 0x0019D4F8
		public static void PostCouldntLoadDef(string defName)
		{
			if (VersionControl.CurrentBuild == ScribeMetaHeaderUtility.loadedGameVersionBuild && !Application.isEditor)
			{
				return;
			}
			for (int i = 0; i < BackCompatibility.conversionChain.Count; i++)
			{
				if (BackCompatibility.conversionChain[i].AppliesToLoadedGameVersion(false))
				{
					try
					{
						BackCompatibility.conversionChain[i].PostCouldntLoadDef(defName);
					}
					catch (Exception ex)
					{
						string str = "Error in PostCouldntLoadDef of ";
						Type type = BackCompatibility.conversionChain[i].GetType();
						string str2 = (type != null) ? type.ToString() : null;
						string str3 = "\n";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}

		// Token: 0x060050A0 RID: 20640 RVA: 0x0019F3A8 File Offset: 0x0019D5A8
		public static void PawnTrainingTrackerPostLoadInit(Pawn_TrainingTracker tracker, ref DefMap<TrainableDef, bool> wantedTrainables, ref DefMap<TrainableDef, int> steps, ref DefMap<TrainableDef, bool> learned)
		{
			if (wantedTrainables == null)
			{
				wantedTrainables = new DefMap<TrainableDef, bool>();
			}
			if (steps == null)
			{
				steps = new DefMap<TrainableDef, int>();
			}
			if (learned == null)
			{
				learned = new DefMap<TrainableDef, bool>();
			}
			if (tracker.GetSteps(TrainableDefOf.Tameness) == 0 && DefDatabase<TrainableDef>.AllDefsListForReading.Any((TrainableDef td) => tracker.GetSteps(td) != 0))
			{
				tracker.Train(TrainableDefOf.Tameness, null, true);
			}
			foreach (TrainableDef trainableDef in DefDatabase<TrainableDef>.AllDefsListForReading)
			{
				if (!tracker.CanAssignToTrain(trainableDef).Accepted)
				{
					wantedTrainables[trainableDef] = false;
					learned[trainableDef] = false;
					steps[trainableDef] = 0;
					if (trainableDef == TrainableDefOf.Obedience && tracker.pawn.playerSettings != null)
					{
						tracker.pawn.playerSettings.Master = null;
						tracker.pawn.playerSettings.followDrafted = false;
						tracker.pawn.playerSettings.followFieldwork = false;
					}
				}
				if (tracker.GetSteps(trainableDef) == trainableDef.steps)
				{
					tracker.Train(trainableDef, null, true);
				}
			}
		}

		// Token: 0x060050A1 RID: 20641 RVA: 0x0019F514 File Offset: 0x0019D714
		public static void TriggerDataFractionColonyDamageTakenNull(Trigger_FractionColonyDamageTaken trigger, Map map)
		{
			trigger.data = new TriggerData_FractionColonyDamageTaken();
			((TriggerData_FractionColonyDamageTaken)trigger.data).startColonyDamage = map.damageWatcher.DamageTakenEver;
		}

		// Token: 0x060050A2 RID: 20642 RVA: 0x0019F53C File Offset: 0x0019D73C
		public static void TriggerDataPawnCycleIndNull(Trigger_KidnapVictimPresent trigger)
		{
			trigger.data = new TriggerData_PawnCycleInd();
		}

		// Token: 0x060050A3 RID: 20643 RVA: 0x0019F549 File Offset: 0x0019D749
		public static void TriggerDataTicksPassedNull(Trigger_TicksPassed trigger)
		{
			trigger.data = new TriggerData_TicksPassed();
		}

		// Token: 0x060050A4 RID: 20644 RVA: 0x0019F556 File Offset: 0x0019D756
		public static TerrainDef BackCompatibleTerrainWithShortHash(ushort hash)
		{
			if (hash == 16442)
			{
				return TerrainDefOf.WaterMovingChestDeep;
			}
			return null;
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x0019F567 File Offset: 0x0019D767
		public static ThingDef BackCompatibleThingDefWithShortHash(ushort hash)
		{
			if (hash == 62520)
			{
				return ThingDefOf.MineableComponentsIndustrial;
			}
			return null;
		}

		// Token: 0x060050A6 RID: 20646 RVA: 0x0019F578 File Offset: 0x0019D778
		public static ThingDef BackCompatibleThingDefWithShortHash_Force(ushort hash, int major, int minor)
		{
			if (major == 0 && minor <= 18 && hash == 27292)
			{
				return ThingDefOf.MineableSteel;
			}
			return null;
		}

		// Token: 0x060050A7 RID: 20647 RVA: 0x0019F594 File Offset: 0x0019D794
		public static bool CheckSpawnBackCompatibleThingAfterLoading(Thing thing, Map map)
		{
			if (ScribeMetaHeaderUtility.loadedGameVersionMajor == 0 && ScribeMetaHeaderUtility.loadedGameVersionMinor <= 18 && thing.stackCount > thing.def.stackLimit && thing.stackCount != 1 && thing.def.stackLimit != 1)
			{
				BackCompatibility.tmpThingsToSpawnLater.Add(thing);
				return true;
			}
			return false;
		}

		// Token: 0x060050A8 RID: 20648 RVA: 0x0019F5E9 File Offset: 0x0019D7E9
		public static void PreCheckSpawnBackCompatibleThingAfterLoading(Map map)
		{
			BackCompatibility.tmpThingsToSpawnLater.Clear();
		}

		// Token: 0x060050A9 RID: 20649 RVA: 0x0019F5F8 File Offset: 0x0019D7F8
		public static void PostCheckSpawnBackCompatibleThingAfterLoading(Map map)
		{
			for (int i = 0; i < BackCompatibility.tmpThingsToSpawnLater.Count; i++)
			{
				GenPlace.TryPlaceThing(BackCompatibility.tmpThingsToSpawnLater[i], BackCompatibility.tmpThingsToSpawnLater[i].Position, map, ThingPlaceMode.Near, null, null, null, 1);
			}
			BackCompatibility.tmpThingsToSpawnLater.Clear();
		}

		// Token: 0x060050AA RID: 20650 RVA: 0x0019F654 File Offset: 0x0019D854
		public static void FactionManagerPostLoadInit()
		{
			if (ModsConfig.RoyaltyActive && Find.FactionManager.FirstFactionOfDef(FactionDefOf.Empire) == null && Find.World.info.factions == null)
			{
				FactionGenerator.CreateFactionAndAddToManager(FactionDefOf.Empire);
			}
			if (ModsConfig.AnomalyActive)
			{
				if (Find.FactionManager.FirstFactionOfDef(FactionDefOf.HoraxCult) == null)
				{
					FactionGenerator.CreateFactionAndAddToManager(FactionDefOf.HoraxCult);
				}
				if (Find.FactionManager.FirstFactionOfDef(FactionDefOf.Entities) == null)
				{
					FactionGenerator.CreateFactionAndAddToManager(FactionDefOf.Entities);
				}
			}
			if (ModsConfig.OdysseyActive)
			{
				if (Find.FactionManager.FirstFactionOfDef(FactionDefOf.TradersGuild) == null)
				{
					FactionGenerator.CreateFactionAndAddToManager(Find.WorldGrid.Orbit, FactionDefOf.TradersGuild);
				}
				if (Find.FactionManager.FirstFactionOfDef(FactionDefOf.Salvagers) == null)
				{
					FactionGenerator.CreateFactionAndAddToManager(Find.WorldGrid.Orbit, FactionDefOf.Salvagers);
				}
			}
		}

		// Token: 0x060050AB RID: 20651 RVA: 0x0019F724 File Offset: 0x0019D924
		public static void IdeoManagerPostloadInit()
		{
			if (ModsConfig.AnomalyActive && ModsConfig.IdeologyActive && !Find.IdeoManager.classicMode)
			{
				Faction cult = Find.FactionManager.OfHoraxCult;
				Faction cult2 = cult;
				if (((cult2 != null) ? cult2.ideos : null) != null && cult.def.fixedIdeo && !cult.def.forcedMemes.NullOrEmpty<MemeDef>() && !cult.def.forcedMemes.All((MemeDef x) => cult.ideos.PrimaryIdeo.memes.Contains(x)))
				{
					FactionDef def = cult.def;
					bool forceNoExpansionIdeo = false;
					List<PreceptDef> disallowedPrecepts = null;
					List<MemeDef> disallowedMemes = null;
					string ideoName = cult.def.ideoName;
					List<StyleCategoryDef> styles = cult.def.styles;
					List<DeityPreset> deityPresets = cult.def.deityPresets;
					bool hiddenIdeo = cult.def.hiddenIdeo;
					string ideoDescription = cult.def.ideoDescription;
					Ideo ideo = IdeoGenerator.MakeFixedIdeo(new IdeoGenerationParms(def, forceNoExpansionIdeo, disallowedPrecepts, disallowedMemes, cult.def.forcedMemes, false, false, false, true, ideoName, styles, deityPresets, hiddenIdeo, ideoDescription, cult.def.requiredPreceptsOnly));
					ideo.primaryFactionColor = new Color?(cult.Color);
					cult.ideos.SetPrimary(ideo);
					Find.IdeoManager.Add(ideo);
				}
			}
		}

		// Token: 0x060050AC RID: 20652 RVA: 0x0019F8A8 File Offset: 0x0019DAA8
		public static void ResearchManagerPostLoadInit()
		{
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if ((allDefsListForReading[i].IsFinished || allDefsListForReading[i].ProgressReal > 0f) && allDefsListForReading[i].TechprintsApplied < allDefsListForReading[i].TechprintCount)
				{
					Find.ResearchManager.AddTechprints(allDefsListForReading[i], allDefsListForReading[i].TechprintCount - allDefsListForReading[i].TechprintsApplied);
				}
			}
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x0019F931 File Offset: 0x0019DB31
		public static void PrefsDataPostLoad(PrefsData prefsData)
		{
			if (prefsData.pauseOnUrgentLetter != null)
			{
				if (prefsData.pauseOnUrgentLetter.Value)
				{
					prefsData.automaticPauseMode = AutomaticPauseMode.MajorThreat;
				}
				else
				{
					prefsData.automaticPauseMode = AutomaticPauseMode.Never;
				}
			}
			if (prefsData.debugActionPalette == null)
			{
				prefsData.debugActionPalette = new List<string>();
			}
		}

		// Token: 0x04003A14 RID: 14868
		public static readonly Pair<int, int>[] SaveCompatibleMinorVersions = new Pair<int, int>[]
		{
			new Pair<int, int>(17, 18)
		};

		// Token: 0x04003A15 RID: 14869
		private static List<BackCompatibilityConverter> conversionChain = new List<BackCompatibilityConverter>
		{
			new BackCompatibilityConverter_0_17_AndLower(),
			new BackCompatibilityConverter_0_18(),
			new BackCompatibilityConverter_0_19(),
			new BackCompatibilityConverter_1_0(),
			new BackCompatibilityConverter_1_2(),
			new BackCompatibilityConverter_1_3(),
			new BackCompatibilityConverter_1_4(),
			new BackCompatibilityConverter_1_5(),
			new BackCompatibilityConverter_Universal()
		};

		// Token: 0x04003A16 RID: 14870
		private static readonly List<Tuple<string, Type>> RemovedDefs = new List<Tuple<string, Type>>
		{
			new Tuple<string, Type>("PsychicSilencer", typeof(HediffDef)),
			new Tuple<string, Type>("PsychicSilencer", typeof(ThingDef)),
			new Tuple<string, Type>("Gun_Slugthrower", typeof(ThingDef)),
			new Tuple<string, Type>("LazyWorker78", typeof(BackstoryDef)),
			new Tuple<string, Type>("ExMilitary9", typeof(BackstoryDef)),
			new Tuple<string, Type>("InsuranceBroker9", typeof(BackstoryDef)),
			new Tuple<string, Type>("StreetUrchin22", typeof(BackstoryDef)),
			new Tuple<string, Type>("CardCounter25", typeof(BackstoryDef)),
			new Tuple<string, Type>("CaesicMarshal72", typeof(BackstoryDef)),
			new Tuple<string, Type>("GrownMate56", typeof(BackstoryDef)),
			new Tuple<string, Type>("DepartmentHead61", typeof(BackstoryDef)),
			new Tuple<string, Type>("ArmyCommander14", typeof(BackstoryDef)),
			new Tuple<string, Type>("DivorceKid95", typeof(BackstoryDef)),
			new Tuple<string, Type>("PetTorturer60", typeof(BackstoryDef)),
			new Tuple<string, Type>("PoliticalClimber28", typeof(BackstoryDef)),
			new Tuple<string, Type>("NoInteraction", typeof(PrisonerInteractionModeDef)),
			new Tuple<string, Type>("Study", typeof(DesignationDef)),
			new Tuple<string, Type>("Mote_HateChant", typeof(ThingDef)),
			new Tuple<string, Type>("GeneticChemicalDependency", typeof(ThoughtDef))
		};

		// Token: 0x04003A17 RID: 14871
		private static List<Thing> tmpThingsToSpawnLater = new List<Thing>();
	}
}
