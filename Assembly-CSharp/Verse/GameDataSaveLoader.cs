using System;
using System.IO;
using RimWorld;

namespace Verse
{
	// Token: 0x0200083F RID: 2111
	public static class GameDataSaveLoader
	{
		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x0600358A RID: 13706 RVA: 0x0011D206 File Offset: 0x0011B406
		public static bool CurrentGameStateIsValuable
		{
			get
			{
				return Find.TickManager.TicksGame > GameDataSaveLoader.lastSaveTick + 60;
			}
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x0600358B RID: 13707 RVA: 0x0011D21C File Offset: 0x0011B41C
		public static bool SavingIsTemporarilyDisabled
		{
			get
			{
				return Find.TilePicker.Active || Find.WindowStack.WindowsPreventSave || WorldComponent_GravshipController.CutsceneInProgress;
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x0600358C RID: 13708 RVA: 0x0011D23D File Offset: 0x0011B43D
		public static bool IsSavingOrLoadingExternalIdeo
		{
			get
			{
				return GameDataSaveLoader.isSavingOrLoadingExternalIdeo;
			}
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x0011D244 File Offset: 0x0011B444
		public static void SaveScenario(Scenario scen, string absFilePath)
		{
			try
			{
				scen.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedscenario", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<Scenario>(ref scen, "scenario", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving world: " + ex.ToString());
			}
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x0011D2B4 File Offset: 0x0011B4B4
		public static bool TryLoadScenario(string absPath, ScenarioCategory category, out Scenario scen)
		{
			scen = null;
			bool result;
			using (Log.LockMessages())
			{
				try
				{
					Scribe.loader.InitLoading(absPath);
					try
					{
						ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Scenario, true);
						Scribe_Deep.Look<Scenario>(ref scen, "scenario", Array.Empty<object>());
						Scribe.loader.FinalizeLoading();
					}
					catch
					{
						Scribe.ForceStop();
						throw;
					}
					scen.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
					scen.Category = category;
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Exception loading scenario: {0}", arg));
					scen = null;
					Scribe.ForceStop();
				}
				if (scen == null)
				{
					result = false;
				}
				else
				{
					for (int i = scen.parts.Count - 1; i >= 0; i--)
					{
						ScenPart scenPart = scen.parts[i];
						if (((scenPart != null) ? scenPart.def : null) == null || scen.parts[i].HasNullDefs())
						{
							scen.parts.RemoveAt(i);
							scen.valid = false;
						}
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x0011D3D8 File Offset: 0x0011B5D8
		public static void SaveGame(string fileName)
		{
			try
			{
				SafeSaver.Save(GenFilePaths.FilePathForSavedGame(fileName), "savegame", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Game game = Current.Game;
					Scribe_Deep.Look<Game>(ref game, "game", Array.Empty<object>());
				}, Find.GameInfo.permadeathMode);
				GameDataSaveLoader.lastSaveTick = Find.TickManager.TicksGame;
			}
			catch (Exception ex)
			{
				string str = "Exception while saving game: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x06003590 RID: 13712 RVA: 0x0011D460 File Offset: 0x0011B660
		public static void CheckVersionAndLoadGame(string saveFileName)
		{
			PreLoadUtility.CheckVersionAndLoad(GenFilePaths.FilePathForSavedGame(saveFileName), ScribeMetaHeaderUtility.ScribeHeaderMode.Map, delegate
			{
				GameDataSaveLoader.LoadGame(saveFileName);
			}, false);
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x0011D498 File Offset: 0x0011B698
		public static void LoadGame(string saveFileName)
		{
			GameDataSaveLoader.<>c__DisplayClass30_0 CS$<>8__locals1 = new GameDataSaveLoader.<>c__DisplayClass30_0();
			CS$<>8__locals1.saveFileName = saveFileName;
			LongEventHandler.QueueLongEvent(new Action(CS$<>8__locals1.<LoadGame>g__PreLoadAct|0), "Play", "LoadingLongEvent", true, null, true, false);
			Game game = Current.Game;
			if (game == null)
			{
				return;
			}
			game.Dispose();
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x0011D4D3 File Offset: 0x0011B6D3
		public static void LoadGame(FileInfo saveFile)
		{
			GameDataSaveLoader.LoadGame(Path.GetFileNameWithoutExtension(saveFile.Name));
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x0011D4E8 File Offset: 0x0011B6E8
		public static void SaveIdeo(Ideo ideo, string absFilePath)
		{
			try
			{
				GameDataSaveLoader.isSavingOrLoadingExternalIdeo = true;
				ideo.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedideo", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<Ideo>(ref ideo, "ideo", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving ideo: " + ex.ToString());
			}
			finally
			{
				GameDataSaveLoader.isSavingOrLoadingExternalIdeo = false;
			}
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x0011D570 File Offset: 0x0011B770
		public static bool TryLoadIdeo(string absPath, out Ideo ideo)
		{
			ideo = null;
			try
			{
				GameDataSaveLoader.isSavingOrLoadingExternalIdeo = true;
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Ideo, true);
					Scribe_Deep.Look<Ideo>(ref ideo, "ideo", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				ideo.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading ideo: " + ex.ToString());
				ideo = null;
				Scribe.ForceStop();
			}
			finally
			{
				GameDataSaveLoader.isSavingOrLoadingExternalIdeo = false;
			}
			return ideo != null;
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x0011D62C File Offset: 0x0011B82C
		public static void SaveXenotype(CustomXenotype xenotype, string absFilePath)
		{
			try
			{
				xenotype.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedXenotype", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<CustomXenotype>(ref xenotype, "xenotype", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving xenotype: " + ex.ToString());
			}
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x0011D69C File Offset: 0x0011B89C
		public static bool TryLoadXenotype(string absPath, out CustomXenotype xenotype)
		{
			xenotype = null;
			try
			{
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Xenotype, true);
					Scribe_Deep.Look<CustomXenotype>(ref xenotype, "xenotype", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				xenotype.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading xenotype: " + ex.ToString());
				xenotype = null;
				Scribe.ForceStop();
			}
			return xenotype != null;
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x0011D73C File Offset: 0x0011B93C
		public static void SaveModList(ModList modList, string absFilePath)
		{
			try
			{
				modList.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedModList", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<ModList>(ref modList, "modList", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				string str = "Exception while saving mod list: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x0011D7B0 File Offset: 0x0011B9B0
		public static bool TryLoadModList(string absPath, out ModList modList)
		{
			modList = null;
			try
			{
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.ModList, true);
					Scribe_Deep.Look<ModList>(ref modList, "modList", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				modList.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading mod list: " + ex.ToString());
				modList = null;
				Scribe.ForceStop();
			}
			return modList != null;
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x0011D850 File Offset: 0x0011BA50
		public static void SaveCameraConfig(CameraMapConfig config, string absFilePath)
		{
			try
			{
				config.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "cameraConfig", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<CameraMapConfig>(ref config, "camConfig", Array.Empty<object>());
				}, false);
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving camera config: " + ex.ToString());
			}
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x0011D8C0 File Offset: 0x0011BAC0
		public static bool TryLoadCameraConfig(string absPath, out CameraMapConfig config)
		{
			config = null;
			try
			{
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.CameraConfig, true);
					Scribe_Deep.Look<CameraMapConfig>(ref config, "camConfig", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				config.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading camera config: " + ex.ToString());
				config = null;
				Scribe.ForceStop();
			}
			return config != null;
		}

		// Token: 0x0400285B RID: 10331
		private static int lastSaveTick = -9999;

		// Token: 0x0400285C RID: 10332
		private static bool isSavingOrLoadingExternalIdeo;

		// Token: 0x0400285D RID: 10333
		public const string SavedScenarioParentNodeName = "savedscenario";

		// Token: 0x0400285E RID: 10334
		public const string SavedWorldParentNodeName = "savedworld";

		// Token: 0x0400285F RID: 10335
		public const string SavedGameParentNodeName = "savegame";

		// Token: 0x04002860 RID: 10336
		public const string SavedIdeoParentNodeName = "savedideo";

		// Token: 0x04002861 RID: 10337
		public const string SavedXenotypeParentNodeName = "savedXenotype";

		// Token: 0x04002862 RID: 10338
		public const string SavedXenogermParentNode = "savedXenogerm";

		// Token: 0x04002863 RID: 10339
		public const string SavedModListParentNodeName = "savedModList";

		// Token: 0x04002864 RID: 10340
		public const string SavedCameraConfigParentNodeName = "cameraConfig";

		// Token: 0x04002865 RID: 10341
		public const string GameNodeName = "game";

		// Token: 0x04002866 RID: 10342
		public const string WorldNodeName = "world";

		// Token: 0x04002867 RID: 10343
		public const string ScenarioNodeName = "scenario";

		// Token: 0x04002868 RID: 10344
		public const string IdeoNodeName = "ideo";

		// Token: 0x04002869 RID: 10345
		public const string XenotypeNodeName = "xenotype";

		// Token: 0x0400286A RID: 10346
		public const string XenogermNodeName = "xenogerm";

		// Token: 0x0400286B RID: 10347
		public const string ModListNodeName = "modList";

		// Token: 0x0400286C RID: 10348
		public const string CameraConfigNodeName = "camConfig";

		// Token: 0x0400286D RID: 10349
		public const string AutosavePrefix = "Autosave";

		// Token: 0x0400286E RID: 10350
		public const string AutostartSaveName = "autostart";
	}
}
