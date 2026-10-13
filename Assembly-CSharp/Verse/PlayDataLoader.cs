using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudeonTK;
using RimWorld;
using RimWorld.BaseGen;
using RimWorld.IO;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001F0 RID: 496
	public static class PlayDataLoader
	{
		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000D58 RID: 3416 RVA: 0x0003F2F3 File Offset: 0x0003D4F3
		public static bool Loaded
		{
			get
			{
				return PlayDataLoader.loadedInt;
			}
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0003F2FC File Offset: 0x0003D4FC
		public static void LoadAllPlayData(bool recovering = false)
		{
			if (PlayDataLoader.loadedInt)
			{
				Log.Error("Loading play data when already loaded. Call ClearAllPlayData first.");
				return;
			}
			DeepProfiler.Start("LoadAllPlayData");
			try
			{
				PlayDataLoader.DoPlayLoad();
			}
			catch (Exception ex)
			{
				if (!Prefs.ResetModsConfigOnCrash)
				{
					throw;
				}
				if (recovering)
				{
					Log.Warning("Could not recover from errors loading play data. Giving up.");
					throw;
				}
				IEnumerable<ModMetaData> activeModsInLoadOrder = ModsConfig.ActiveModsInLoadOrder;
				if (!activeModsInLoadOrder.Any((ModMetaData x) => !x.Official))
				{
					if (activeModsInLoadOrder.Any((ModMetaData x) => x.IsCoreMod))
					{
						throw;
					}
				}
				string str = "Caught exception while loading play data but there are active mods other than Core. Resetting mods config and trying again.\nThe exception was: ";
				Exception ex2 = ex;
				Log.Warning(str + ((ex2 != null) ? ex2.ToString() : null));
				try
				{
					PlayDataLoader.ClearAllPlayData();
				}
				catch
				{
					string str2 = "Caught exception while recovering from errors and trying to clear all play data. Ignoring it.\nThe exception was: ";
					Exception ex3 = ex;
					Log.Warning(str2 + ((ex3 != null) ? ex3.ToString() : null));
				}
				ModsConfig.Reset();
				DirectXmlCrossRefLoader.Clear();
				PlayDataLoader.LoadAllPlayData(true);
				return;
			}
			finally
			{
				DeepProfiler.End();
			}
			PlayDataLoader.loadedInt = true;
			if (recovering)
			{
				Log.Message("Successfully recovered from errors and loaded play data.");
				DelayedErrorWindowRequest.Add("RecoveredFromErrorsText".Translate(), "RecoveredFromErrorsDialogTitle".Translate());
			}
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x0003F458 File Offset: 0x0003D658
		private static void DoPlayLoad()
		{
			GlobalTextureAtlasManager.ClearStaticAtlasBuildQueue();
			DeepProfiler.Start("GraphicDatabase.Clear()");
			try
			{
				GraphicDatabase.Clear();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Load all active mods.");
			try
			{
				LoadedModManager.LoadAllActiveMods(false);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Load language metadata.");
			try
			{
				LanguageDatabase.InitAllMetadata();
			}
			finally
			{
				DeepProfiler.End();
			}
			LongEventHandler.SetCurrentEventText("LoadingDefs".Translate());
			DeepProfiler.Start("Copy all Defs from mods to global databases.");
			try
			{
				Parallel.ForEach<Type>(typeof(Def).AllSubclasses(), delegate(Type defType)
				{
					GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), defType, "AddAllInMods");
				});
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Resolve cross-references between non-implied Defs.");
			try
			{
				DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.Silent);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Rebind DefOfs (early).");
			try
			{
				DefOfHelper.RebindAllDefOfs(true);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("TKeySystem.BuildMappings()");
			try
			{
				TKeySystem.BuildMappings();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Legacy backstory translations.");
			try
			{
				BackstoryTranslationUtility.LoadAndInjectBackstoryData(LanguageDatabase.activeLanguage.AllDirectories, null);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Inject selected language data into game data (early pass).");
			try
			{
				LanguageDatabase.activeLanguage.InjectIntoData_BeforeImpliedDefs();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Global operations (early pass).");
			try
			{
				ColoredText.ResetStaticData();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Generate implied Defs (pre-resolve).");
			try
			{
				DefGenerator.GenerateImpliedDefs_PreResolve(false);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Resolve cross-references between Defs made by the implied defs.");
			try
			{
				DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.LogErrors);
			}
			finally
			{
				DirectXmlCrossRefLoader.Clear();
				DeepProfiler.End();
			}
			DeepProfiler.Start("Rebind DefOfs (final).");
			try
			{
				DefOfHelper.RebindAllDefOfs(false);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Other def binding, resetting and global operations (pre-resolve).");
			try
			{
				PlayDataLoader.ResetStaticDataPre();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Resolve references.");
			try
			{
				DeepProfiler.Start("ThingCategoryDef resolver");
				try
				{
					DeepProfiler.enabled = false;
					DefDatabase<ThingCategoryDef>.ResolveAllReferences(true, true);
					DeepProfiler.enabled = true;
				}
				finally
				{
					DeepProfiler.End();
				}
				DeepProfiler.Start("RecipeDef resolver");
				try
				{
					DeepProfiler.enabled = false;
					DefDatabase<RecipeDef>.ResolveAllReferences(true, true);
					DeepProfiler.enabled = true;
				}
				finally
				{
					DeepProfiler.End();
				}
				DeepProfiler.Start("Static resolver calls");
				try
				{
					foreach (Type type in typeof(Def).AllSubclasses())
					{
						if (!(type == typeof(ThingDef)) && !(type == typeof(ThingCategoryDef)) && !(type == typeof(RecipeDef)))
						{
							GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), type, "ResolveAllReferences", new object[]
							{
								true,
								false
							});
						}
					}
				}
				finally
				{
					DeepProfiler.End();
				}
				DeepProfiler.Start("ThingDef resolver");
				try
				{
					DefDatabase<ThingDef>.ResolveAllReferences(true, false);
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Generate implied Defs (post-resolve).");
			try
			{
				DefGenerator.GenerateImpliedDefs_PostResolve(false);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Other def binding, resetting and global operations (post-resolve).");
			try
			{
				PlayDataLoader.ResetStaticDataPost();
			}
			finally
			{
				DeepProfiler.End();
			}
			if (Prefs.DevMode)
			{
				DeepProfiler.Start("Error check all defs.");
				try
				{
					Parallel.ForEach<Type>(typeof(Def).AllSubclasses(), delegate(Type defType)
					{
						GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), defType, "ErrorCheckAllDefs");
					});
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			LongEventHandler.SetCurrentEventText("Initializing".Translate());
			DeepProfiler.Start("Load keyboard preferences.");
			try
			{
				KeyPrefs.Init();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Short hash giving.");
			try
			{
				ShortHashGiver.GiveAllShortHashes();
			}
			finally
			{
				DeepProfiler.End();
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				DeepProfiler.Start("Load all bios");
				try
				{
					SolidBioDatabase.LoadAllBios();
				}
				finally
				{
					DeepProfiler.End();
				}
			});
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				DeepProfiler.Start("Inject selected language data into game data.");
				try
				{
					LanguageDatabase.activeLanguage.InjectIntoData_AfterImpliedDefs();
					GenLabel.ClearCache();
				}
				finally
				{
					DeepProfiler.End();
				}
			});
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				DeepProfiler.Start("Static constructor calls");
				try
				{
					StaticConstructorOnStartupUtility.CallAll();
					if (Prefs.DevMode)
					{
						StaticConstructorOnStartupUtility.ReportProbablyMissingAttributes();
					}
				}
				finally
				{
					DeepProfiler.End();
				}
				FloatMenuMakerMap.Init();
				DeepProfiler.Start("Atlas baking.");
				try
				{
					GlobalTextureAtlasManager.BakeStaticAtlases();
				}
				finally
				{
					DeepProfiler.End();
				}
				DeepProfiler.Start("Garbage Collection");
				try
				{
					AbstractFilesystem.ClearAllCache();
					GC.Collect(int.MaxValue, GCCollectionMode.Forced);
					Resources.UnloadUnusedAssets();
				}
				finally
				{
					DeepProfiler.End();
				}
			});
			LongEventHandler.ExecuteWhenFinished(new Action(Log.ResetMessageCount));
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x0003FAC8 File Offset: 0x0003DCC8
		public static void ClearAllPlayData()
		{
			LanguageDatabase.Clear();
			LoadedModManager.ClearDestroy();
			foreach (Type genericParam in typeof(Def).AllSubclasses())
			{
				GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), genericParam, "Clear");
			}
			ThingCategoryNodeDatabase.Clear();
			SolidBioDatabase.Clear();
			Current.Game = null;
			PlayDataLoader.loadedInt = false;
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x0003FB54 File Offset: 0x0003DD54
		public static void HotReloadDefs()
		{
			Dictionary<BodyPartRecord, int> prevBodyPartIndices = DefDatabase<BodyDef>.AllDefsListForReading.SelectMany((BodyDef x) => x.AllParts).ToDictionary((BodyPartRecord x) => x, (BodyPartRecord x) => x.Index);
			Action <>9__4;
			LongEventHandler.QueueLongEvent(delegate()
			{
				DeepProfiler.Start("GraphicDatabase.Clear()");
				try
				{
					GraphicDatabase.Clear();
				}
				finally
				{
					DeepProfiler.End();
				}
				DeepProfiler.Start("Load all active mods.");
				try
				{
					LoadedModManager.LoadAllActiveMods(true);
				}
				finally
				{
					DeepProfiler.End();
				}
				DeepProfiler.Start("Resolve cross-references between non-implied Defs.");
				try
				{
					DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.Silent);
				}
				finally
				{
					DeepProfiler.End();
				}
				DeepProfiler.Start("Global operations (early pass).");
				try
				{
					ColoredText.ResetStaticData();
				}
				finally
				{
					DeepProfiler.End();
				}
				LongEventHandler.ForceExecuteToExecuteWhenFinished();
				DeepProfiler.Start("Generate implied Defs (pre-resolve).");
				try
				{
					DefGenerator.GenerateImpliedDefs_PreResolve(true);
				}
				finally
				{
					DeepProfiler.End();
				}
				DeepProfiler.Start("Resolve cross-references.");
				try
				{
					DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.LogErrors);
				}
				finally
				{
					DirectXmlCrossRefLoader.Clear();
					DeepProfiler.End();
				}
				DeepProfiler.Start("Other def binding, resetting and global operations (pre-resolve).");
				try
				{
					PlayDataLoader.ResetStaticDataPre();
				}
				finally
				{
					DeepProfiler.End();
				}
				DeepProfiler.Start("Resolve references.");
				try
				{
					DeepProfiler.Start("ThingCategoryDef resolver");
					try
					{
						DeepProfiler.enabled = false;
						DefDatabase<ThingCategoryDef>.ResolveAllReferences(true, true);
						DeepProfiler.enabled = true;
					}
					finally
					{
						DeepProfiler.End();
					}
					DeepProfiler.Start("RecipeDef resolver");
					try
					{
						DeepProfiler.enabled = false;
						DefDatabase<RecipeDef>.ResolveAllReferences(true, true);
						DeepProfiler.enabled = true;
					}
					finally
					{
						DeepProfiler.End();
					}
					DeepProfiler.Start("Static resolver calls");
					try
					{
						foreach (Type type in typeof(Def).AllSubclasses())
						{
							if (!(type == typeof(ThingDef)) && !(type == typeof(ThingCategoryDef)) && !(type == typeof(RecipeDef)))
							{
								GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), type, "ResolveAllReferences", new object[]
								{
									true,
									false
								});
							}
						}
					}
					finally
					{
						DeepProfiler.End();
					}
					DeepProfiler.Start("ThingDef resolver");
					try
					{
						DefDatabase<ThingDef>.ResolveAllReferences(true, false);
					}
					finally
					{
						DeepProfiler.End();
					}
				}
				finally
				{
					DeepProfiler.End();
				}
				DeepProfiler.Start("Generate implied Defs (post-resolve).");
				try
				{
					DefGenerator.GenerateImpliedDefs_PostResolve(true);
				}
				finally
				{
					DeepProfiler.End();
				}
				DeepProfiler.Start("Other def binding, resetting and global operations (post-resolve).");
				try
				{
					PlayDataLoader.ResetStaticDataPost();
				}
				finally
				{
					DeepProfiler.End();
				}
				Action action;
				if ((action = <>9__4) == null)
				{
					action = (<>9__4 = delegate()
					{
						DeepProfiler.Start("Match new CompProperties");
						foreach (Map map in Find.Maps)
						{
							List<CompProperties> list = new List<CompProperties>();
							foreach (Thing thing in ((IEnumerable<Thing>)map.spawnedThings))
							{
								ThingWithComps thingWithComps = thing as ThingWithComps;
								if (thingWithComps != null && thingWithComps.AllComps != null)
								{
									list.Clear();
									list.AddRange(thingWithComps.def.comps);
									List<ThingComp> allComps = thingWithComps.AllComps;
									for (int i = 0; i < allComps.Count; i++)
									{
										CompProperties compProperties = null;
										for (int j = 0; j < list.Count; j++)
										{
											if (list[j].compClass == allComps[i].GetType())
											{
												compProperties = list[j];
												list.RemoveAt(j);
												break;
											}
										}
										if (compProperties != null)
										{
											allComps[i].props = compProperties;
										}
									}
									for (int k = 0; k < list.Count; k++)
									{
										try
										{
											ThingComp thingComp = (ThingComp)Activator.CreateInstance(list[k].compClass);
											thingComp.parent = thingWithComps;
											allComps.Add(thingComp);
											thingComp.Initialize(list[k]);
										}
										catch (Exception ex)
										{
											string str = "Could not instantiate or initialize a ThingComp: ";
											Exception ex2 = ex;
											Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
										}
									}
								}
							}
						}
						DeepProfiler.End();
						DeepProfiler.Start("Match new body parts");
						foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
						{
							foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
							{
								if (hediff.Part != null)
								{
									hediff.Part = pawn.RaceProps.body.AllParts[prevBodyPartIndices[hediff.Part]];
								}
							}
						}
						DeepProfiler.End();
						foreach (Map map2 in Find.Maps)
						{
							DeepProfiler.Start("Notify things");
							foreach (Thing thing2 in ((IEnumerable<Thing>)map2.spawnedThings))
							{
								thing2.Notify_DefsHotReloaded();
							}
							DeepProfiler.End();
							DeepProfiler.Start("Rebuild MapDrawer");
							map2.mapDrawer.RegenerateEverythingNow();
							DeepProfiler.End();
						}
						foreach (Window window in Find.WindowStack.Windows.ToList<Window>())
						{
							window.Close(false);
						}
					});
				}
				LongEventHandler.ExecuteWhenFinished(action);
				LongEventHandler.ExecuteWhenFinished(new Action(Log.ResetMessageCount));
			}, "LoadingDefs", false, null, true, false, null);
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0003FBF4 File Offset: 0x0003DDF4
		private static void ResetStaticDataPre()
		{
			PlayerKnowledgeDatabase.ReloadAndRebind();
			LessonAutoActivator.Reset();
			CostListCalculator.Reset();
			Pawn.ResetStaticData();
			PawnApparelGenerator.Reset();
			RestUtility.Reset();
			ThoughtUtility.Reset();
			ThinkTreeKeyAssigner.Reset();
			ThingCategoryNodeDatabase.FinalizeInit();
			TrainableUtility.Reset();
			HaulAIUtility.Reset();
			GenConstruct.Reset();
			MedicalCareUtility.Reset();
			InspectPaneUtility.Reset();
			DateReadout.Reset();
			ResearchProjectDef.GenerateNonOverlappingCoordinates();
			BaseGen.Reset();
			ResourceCounter.ResetDefs();
			ApparelProperties.ResetStaticData();
			WildPlantSpawner.ResetStaticData();
			PawnGenerator.Reset();
			GroundSpawner.ResetStaticData();
			Hive.ResetStaticData();
			ExpectationsUtility.Reset();
			SkillUI.Reset();
			QuestNode_GetThingPlayerCanProduce.ResetStaticData();
			Pawn_PsychicEntropyTracker.ResetStaticData();
			QuestNode_GetRandomNegativeGameCondition.ResetStaticData();
			RoyalTitleUtility.ResetStaticData();
			RewardsGenerator.ResetStaticData();
			ThoughtWorker_Precept_HasAutomatedTurrets.ResetStaticData();
			AnimalPenUtility.ResetStaticData();
			StorageSettings.ResetStaticData();
			Listing_TreeThingFilter.ResetStaticData();
			PawnSkinColors.ResetStaticData();
			PawnHairColors.ResetStaticData();
			GenStuff.ResetStaticData();
			StatDef.ResetStaticData();
			MouseoverUtility.Reset();
			Dialog_Debug.ResetStaticData();
			JobGiver_GetHemogen.ResetStaticData();
			CachedTexture.ResetStaticData();
			CachedMaterial.ResetStaticData();
			Storyteller.ResetStaticData();
			PawnRenderUtility.ResetStaticData();
			QuickSearchUtility.ResetStaticData();
			ExpandableLandmarksUtility.ResetStaticData();
			WorkGiver_FillFermentingBarrel.ResetStaticData();
			WorkGiver_InteractAnimal.ResetStaticData();
			WorkGiver_Warden_DoExecution.ResetStaticData();
			WorkGiver_GrowerSow.ResetStaticData();
			MineAIUtility.ResetStaticData();
			WorkGiver_FixBrokenDownBuilding.ResetStaticData();
			WorkGiver_ConstructDeliverResources.ResetStaticData();
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x0003FD0F File Offset: 0x0003DF0F
		private static void ResetStaticDataPost()
		{
			PawnWeaponGenerator.Reset();
			BuildingProperties.FinalizeInit();
			ThingSetMakerUtility.Reset();
			StatDef.SetImmutability();
			WealthWatcher.ResetStaticData();
			GenLabel.ClearCache();
		}

		// Token: 0x040007B8 RID: 1976
		private static bool loadedInt;
	}
}
