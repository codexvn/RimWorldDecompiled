using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CFA RID: 3322
	public static class GenFilePaths
	{
		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x060054CD RID: 21709 RVA: 0x001B5B38 File Offset: 0x001B3D38
		public static string SaveDataFolderPath
		{
			get
			{
				if (GenFilePaths.saveDataPath == null)
				{
					string text;
					if (GenCommandLine.TryGetCommandLineArg("savedatafolder", out text))
					{
						text.TrimEnd(new char[]
						{
							'\\',
							'/'
						});
						if (text == "")
						{
							text = (Path.DirectorySeparatorChar.ToString() ?? "");
						}
						GenFilePaths.saveDataPath = text;
						Log.Message("Save data folder overridden to " + GenFilePaths.saveDataPath);
					}
					else
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(UnityData.dataPath);
						if (UnityData.isEditor)
						{
							GenFilePaths.saveDataPath = Path.Combine(directoryInfo.Parent.ToString(), "SaveData");
						}
						else if (UnityData.platform == RuntimePlatform.OSXPlayer || UnityData.platform == RuntimePlatform.OSXEditor)
						{
							string path = Path.Combine(Directory.GetParent(UnityData.persistentDataPath).ToString(), "RimWorld");
							if (!Directory.Exists(path))
							{
								Directory.CreateDirectory(path);
							}
							GenFilePaths.saveDataPath = path;
						}
						else
						{
							GenFilePaths.saveDataPath = Application.persistentDataPath;
						}
					}
					DirectoryInfo directoryInfo2 = new DirectoryInfo(GenFilePaths.saveDataPath);
					if (!directoryInfo2.Exists)
					{
						directoryInfo2.Create();
					}
				}
				return GenFilePaths.saveDataPath;
			}
		}

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x060054CE RID: 21710 RVA: 0x001B5C50 File Offset: 0x001B3E50
		public static string ScenarioPreviewImagePath
		{
			get
			{
				if (!UnityData.isEditor)
				{
					return Path.Combine(GenFilePaths.ExecutableDir.FullName, "ScenarioPreview.jpg");
				}
				return Path.Combine(Path.Combine(Path.Combine(GenFilePaths.ExecutableDir.FullName, "PlatformSpecific"), "All"), "ScenarioPreview.jpg");
			}
		}

		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x060054CF RID: 21711 RVA: 0x001B5CA1 File Offset: 0x001B3EA1
		private static DirectoryInfo ExecutableDir
		{
			get
			{
				return new DirectoryInfo(UnityData.dataPath).Parent;
			}
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x060054D0 RID: 21712 RVA: 0x001B5CB2 File Offset: 0x001B3EB2
		public static string ModsFolderPath
		{
			get
			{
				if (GenFilePaths.modsFolderPath == null)
				{
					GenFilePaths.modsFolderPath = GenFilePaths.GetOrCreateModsFolder("Mods");
				}
				return GenFilePaths.modsFolderPath;
			}
		}

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x060054D1 RID: 21713 RVA: 0x001B5CCF File Offset: 0x001B3ECF
		public static string OfficialModsFolderPath
		{
			get
			{
				if (GenFilePaths.officialModsFolderPath == null)
				{
					GenFilePaths.officialModsFolderPath = GenFilePaths.GetOrCreateModsFolder("Data");
				}
				return GenFilePaths.officialModsFolderPath;
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x060054D2 RID: 21714 RVA: 0x001B5CEC File Offset: 0x001B3EEC
		public static string ConfigFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Config");
			}
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x060054D3 RID: 21715 RVA: 0x001B5CF8 File Offset: 0x001B3EF8
		public static string SavedGamesFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Saves");
			}
		}

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x060054D4 RID: 21716 RVA: 0x001B5D04 File Offset: 0x001B3F04
		private static string ScenariosFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Scenarios");
			}
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x060054D5 RID: 21717 RVA: 0x001B5D10 File Offset: 0x001B3F10
		private static string ModListsFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("ModLists");
			}
		}

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x060054D6 RID: 21718 RVA: 0x001B5D1C File Offset: 0x001B3F1C
		private static string IdeosFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Ideos");
			}
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x060054D7 RID: 21719 RVA: 0x001B5D28 File Offset: 0x001B3F28
		private static string XenotypesFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Xenotypes");
			}
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x060054D8 RID: 21720 RVA: 0x001B5D34 File Offset: 0x001B3F34
		private static string XenogermsFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Xenogerms");
			}
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x060054D9 RID: 21721 RVA: 0x001B5D40 File Offset: 0x001B3F40
		private static string CameraConfigsFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("CameraConfigs");
			}
		}

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x060054DA RID: 21722 RVA: 0x001B5D4C File Offset: 0x001B3F4C
		private static string ExternalHistoryFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("External");
			}
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x060054DB RID: 21723 RVA: 0x001B5D58 File Offset: 0x001B3F58
		public static string ScreenshotFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("Screenshots");
			}
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x060054DC RID: 21724 RVA: 0x001B5D64 File Offset: 0x001B3F64
		public static string DevOutputFolderPath
		{
			get
			{
				return GenFilePaths.FolderUnderSaveData("DevOutput");
			}
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x060054DD RID: 21725 RVA: 0x001B5D70 File Offset: 0x001B3F70
		public static string ModsConfigFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "ModsConfig.xml");
			}
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x060054DE RID: 21726 RVA: 0x001B5D81 File Offset: 0x001B3F81
		public static string ConceptKnowledgeFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Knowledge.xml");
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x060054DF RID: 21727 RVA: 0x001B5D92 File Offset: 0x001B3F92
		public static string PrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "Prefs.xml");
			}
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x060054E0 RID: 21728 RVA: 0x001B5DA3 File Offset: 0x001B3FA3
		public static string KeyPrefsFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "KeyPrefs.xml");
			}
		}

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x060054E1 RID: 21729 RVA: 0x001B5DB4 File Offset: 0x001B3FB4
		public static string LastPlayedVersionFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "LastPlayedVersion.txt");
			}
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x060054E2 RID: 21730 RVA: 0x001B5DC5 File Offset: 0x001B3FC5
		public static string DevModePermanentlyDisabledFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.ConfigFolderPath, "DevModeDisabled");
			}
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x060054E3 RID: 21731 RVA: 0x001B5DD6 File Offset: 0x001B3FD6
		public static string BackstoryOutputFilePath
		{
			get
			{
				return Path.Combine(GenFilePaths.DevOutputFolderPath, "Fresh_Backstories.xml");
			}
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x060054E4 RID: 21732 RVA: 0x001B5DE7 File Offset: 0x001B3FE7
		public static string TempFolderPath
		{
			get
			{
				return Application.temporaryCachePath;
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x060054E5 RID: 21733 RVA: 0x001B5DF0 File Offset: 0x001B3FF0
		public static IEnumerable<FileInfo> AllSavedGameFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.SavedGamesFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rws"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x060054E6 RID: 21734 RVA: 0x001B5E64 File Offset: 0x001B4064
		public static IEnumerable<FileInfo> AllCustomScenarioFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ScenariosFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rsc"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x060054E7 RID: 21735 RVA: 0x001B5ED8 File Offset: 0x001B40D8
		public static IEnumerable<FileInfo> AllModListFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ModListsFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rml"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x060054E8 RID: 21736 RVA: 0x001B5F4C File Offset: 0x001B414C
		public static IEnumerable<FileInfo> AllCustomIdeoFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.IdeosFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rid"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x060054E9 RID: 21737 RVA: 0x001B5FC0 File Offset: 0x001B41C0
		public static IEnumerable<FileInfo> AllCustomXenotypeFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.XenotypesFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".xtp"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x060054EA RID: 21738 RVA: 0x001B6034 File Offset: 0x001B4234
		public static IEnumerable<FileInfo> AllCustomXenogermFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.XenogermsFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".xgm"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x060054EB RID: 21739 RVA: 0x001B60A8 File Offset: 0x001B42A8
		public static IEnumerable<FileInfo> AllCameraConfigFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.CameraConfigsFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".ccg"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x060054EC RID: 21740 RVA: 0x001B611C File Offset: 0x001B431C
		public static IEnumerable<FileInfo> AllExternalHistoryFiles
		{
			get
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.ExternalHistoryFolderPath);
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return from f in directoryInfo.GetFiles()
				where f.Extension == ".rwh"
				orderby f.LastWriteTime descending
				select f;
			}
		}

		// Token: 0x060054ED RID: 21741 RVA: 0x001B6190 File Offset: 0x001B4390
		private static string FolderUnderSaveData(string folderName)
		{
			string text = Path.Combine(GenFilePaths.SaveDataFolderPath, folderName);
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			return text;
		}

		// Token: 0x060054EE RID: 21742 RVA: 0x001B61BD File Offset: 0x001B43BD
		public static string FilePathForSavedGame(string gameName)
		{
			return Path.Combine(GenFilePaths.SavedGamesFolderPath, gameName + ".rws");
		}

		// Token: 0x060054EF RID: 21743 RVA: 0x001B61D4 File Offset: 0x001B43D4
		public static string AbsPathForScenario(string scenarioName)
		{
			return Path.Combine(GenFilePaths.ScenariosFolderPath, scenarioName + ".rsc");
		}

		// Token: 0x060054F0 RID: 21744 RVA: 0x001B61EB File Offset: 0x001B43EB
		public static string AbsPathForIdeo(string ideoName)
		{
			return Path.Combine(GenFilePaths.IdeosFolderPath, ideoName + ".rid");
		}

		// Token: 0x060054F1 RID: 21745 RVA: 0x001B6202 File Offset: 0x001B4402
		public static string AbsFilePathForXenotype(string xenotypeName)
		{
			return Path.Combine(GenFilePaths.XenotypesFolderPath, xenotypeName + ".xtp");
		}

		// Token: 0x060054F2 RID: 21746 RVA: 0x001B6219 File Offset: 0x001B4419
		public static string AbsFilePathForModList(string modListName)
		{
			return Path.Combine(GenFilePaths.ModListsFolderPath, modListName + ".rml");
		}

		// Token: 0x060054F3 RID: 21747 RVA: 0x001B6230 File Offset: 0x001B4430
		public static string AbsFilePathForXenogerm(string xenogermName)
		{
			return Path.Combine(GenFilePaths.XenogermsFolderPath, xenogermName + ".xgm");
		}

		// Token: 0x060054F4 RID: 21748 RVA: 0x001B6247 File Offset: 0x001B4447
		public static string AbsFilePathForCameraConfig(string configName)
		{
			return Path.Combine(GenFilePaths.CameraConfigsFolderPath, configName + ".ccg");
		}

		// Token: 0x060054F5 RID: 21749 RVA: 0x001B6260 File Offset: 0x001B4460
		public static string ContentPath<T>()
		{
			if (typeof(T) == typeof(AudioClip))
			{
				return "Sounds/";
			}
			if (typeof(T) == typeof(Texture2D))
			{
				return "Textures/";
			}
			if (typeof(T) == typeof(string))
			{
				return "Strings/";
			}
			if (typeof(T) == typeof(AssetBundle))
			{
				return "AssetBundles/";
			}
			if (typeof(T) == typeof(Shader))
			{
				return "Materials/";
			}
			throw new ArgumentException();
		}

		// Token: 0x060054F6 RID: 21750 RVA: 0x001B6318 File Offset: 0x001B4518
		private static string GetOrCreateModsFolder(string folderName)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(UnityData.dataPath);
			DirectoryInfo directoryInfo2;
			if (UnityData.isEditor)
			{
				directoryInfo2 = directoryInfo;
			}
			else
			{
				directoryInfo2 = directoryInfo.Parent;
			}
			string text = Path.Combine(directoryInfo2.ToString(), folderName);
			DirectoryInfo directoryInfo3 = new DirectoryInfo(text);
			if (!directoryInfo3.Exists)
			{
				directoryInfo3.Create();
			}
			return text;
		}

		// Token: 0x060054F7 RID: 21751 RVA: 0x001B6368 File Offset: 0x001B4568
		public static string SafeURIForUnityWWWFromPath(string rawPath)
		{
			string text = rawPath;
			for (int i = 0; i < GenFilePaths.FilePathRaw.Length; i++)
			{
				text = text.Replace(GenFilePaths.FilePathRaw[i], GenFilePaths.FilePathSafe[i]);
			}
			return "file:///" + text;
		}

		// Token: 0x04003D45 RID: 15685
		private static string saveDataPath = null;

		// Token: 0x04003D46 RID: 15686
		private static string modsFolderPath = null;

		// Token: 0x04003D47 RID: 15687
		private static string officialModsFolderPath = null;

		// Token: 0x04003D48 RID: 15688
		public const string SoundsFolder = "Sounds/";

		// Token: 0x04003D49 RID: 15689
		public const string SoundsFolderName = "Sounds";

		// Token: 0x04003D4A RID: 15690
		public const string TexturesFolder = "Textures/";

		// Token: 0x04003D4B RID: 15691
		public const string TexturesFolderName = "Textures";

		// Token: 0x04003D4C RID: 15692
		public const string StringsFolder = "Strings/";

		// Token: 0x04003D4D RID: 15693
		public const string DefsFolder = "Defs/";

		// Token: 0x04003D4E RID: 15694
		public const string PatchesFolder = "Patches/";

		// Token: 0x04003D4F RID: 15695
		public const string AssetBundlesFolderName = "AssetBundles";

		// Token: 0x04003D50 RID: 15696
		public const string AssetBundlesFolder = "AssetBundles/";

		// Token: 0x04003D51 RID: 15697
		public const string MaterialsFolder = "Materials/";

		// Token: 0x04003D52 RID: 15698
		public const string AssetsFolderName = "Assets";

		// Token: 0x04003D53 RID: 15699
		public const string ResourcesFolderName = "Resources";

		// Token: 0x04003D54 RID: 15700
		public const string ModsFolderName = "Mods";

		// Token: 0x04003D55 RID: 15701
		public const string AssembliesFolder = "Assemblies/";

		// Token: 0x04003D56 RID: 15702
		public const string OfficialModsFolderName = "Data";

		// Token: 0x04003D57 RID: 15703
		public const string CoreFolderName = "Core";

		// Token: 0x04003D58 RID: 15704
		public const string BackstoriesPath = "Backstories";

		// Token: 0x04003D59 RID: 15705
		public const string SavedGameExtension = ".rws";

		// Token: 0x04003D5A RID: 15706
		public const string ScenarioExtension = ".rsc";

		// Token: 0x04003D5B RID: 15707
		public const string ModListExtension = ".rml";

		// Token: 0x04003D5C RID: 15708
		public const string IdeoExtension = ".rid";

		// Token: 0x04003D5D RID: 15709
		public const string XenotypeExtension = ".xtp";

		// Token: 0x04003D5E RID: 15710
		public const string XenogermExtension = ".xgm";

		// Token: 0x04003D5F RID: 15711
		public const string CameraConfigExtension = ".ccg";

		// Token: 0x04003D60 RID: 15712
		public const string ExternalHistoryFileExtension = ".rwh";

		// Token: 0x04003D61 RID: 15713
		private const string SaveDataFolderCommand = "savedatafolder";

		// Token: 0x04003D62 RID: 15714
		private static readonly string[] FilePathRaw = new string[]
		{
			"Ž",
			"ž",
			"Ÿ",
			"¡",
			"¢",
			"£",
			"¤",
			"¥",
			"¦",
			"§",
			"¨",
			"©",
			"ª",
			"À",
			"Á",
			"Â",
			"Ã",
			"Ä",
			"Å",
			"Æ",
			"Ç",
			"È",
			"É",
			"Ê",
			"Ë",
			"Ì",
			"Í",
			"Î",
			"Ï",
			"Ð",
			"Ñ",
			"Ò",
			"Ó",
			"Ô",
			"Õ",
			"Ö",
			"Ù",
			"Ú",
			"Û",
			"Ü",
			"Ý",
			"Þ",
			"ß",
			"à",
			"á",
			"â",
			"ã",
			"ä",
			"å",
			"æ",
			"ç",
			"è",
			"é",
			"ê",
			"ë",
			"ì",
			"í",
			"î",
			"ï",
			"ð",
			"ñ",
			"ò",
			"ó",
			"ô",
			"õ",
			"ö",
			"ù",
			"ú",
			"û",
			"ü",
			"ý",
			"þ",
			"ÿ"
		};

		// Token: 0x04003D63 RID: 15715
		private static readonly string[] FilePathSafe = new string[]
		{
			"%8E",
			"%9E",
			"%9F",
			"%A1",
			"%A2",
			"%A3",
			"%A4",
			"%A5",
			"%A6",
			"%A7",
			"%A8",
			"%A9",
			"%AA",
			"%C0",
			"%C1",
			"%C2",
			"%C3",
			"%C4",
			"%C5",
			"%C6",
			"%C7",
			"%C8",
			"%C9",
			"%CA",
			"%CB",
			"%CC",
			"%CD",
			"%CE",
			"%CF",
			"%D0",
			"%D1",
			"%D2",
			"%D3",
			"%D4",
			"%D5",
			"%D6",
			"%D9",
			"%DA",
			"%DB",
			"%DC",
			"%DD",
			"%DE",
			"%DF",
			"%E0",
			"%E1",
			"%E2",
			"%E3",
			"%E4",
			"%E5",
			"%E6",
			"%E7",
			"%E8",
			"%E9",
			"%EA",
			"%EB",
			"%EC",
			"%ED",
			"%EE",
			"%EF",
			"%F0",
			"%F1",
			"%F2",
			"%F3",
			"%F4",
			"%F5",
			"%F6",
			"%F9",
			"%FA",
			"%FB",
			"%FC",
			"%FD",
			"%FE",
			"%FF"
		};
	}
}
