using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x0200085E RID: 2142
	public class ScribeMetaHeaderUtility
	{
		// Token: 0x06003617 RID: 13847 RVA: 0x00120C90 File Offset: 0x0011EE90
		public static void WriteMetaHeader()
		{
			if (Scribe.EnterNode("meta"))
			{
				try
				{
					string currentVersionStringWithRev = VersionControl.CurrentVersionStringWithRev;
					Scribe_Values.Look<string>(ref currentVersionStringWithRev, "gameVersion", null, false);
					List<string> list = (from mod in LoadedModManager.RunningMods
					select mod.PackageId).ToList<string>();
					Scribe_Collections.Look<string>(ref list, "modIds", LookMode.Undefined, Array.Empty<object>());
					List<uint> list2 = (from mod in LoadedModManager.RunningMods
					select mod.SteamAppId).ToList<uint>();
					Scribe_Collections.Look<uint>(ref list2, "modSteamIds", LookMode.Undefined, Array.Empty<object>());
					List<string> list3 = (from mod in LoadedModManager.RunningMods
					select mod.Name).ToList<string>();
					Scribe_Collections.Look<string>(ref list3, "modNames", LookMode.Undefined, Array.Empty<object>());
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x00120D9C File Offset: 0x0011EF9C
		public static void LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode mode, bool logVersionConflictWarning)
		{
			ScribeMetaHeaderUtility.loadedGameVersion = "Unknown";
			ScribeMetaHeaderUtility.loadedGameVersionMajor = 0;
			ScribeMetaHeaderUtility.loadedGameVersionMinor = 0;
			ScribeMetaHeaderUtility.loadedGameVersionBuild = 0;
			ScribeMetaHeaderUtility.loadedModIdsList = null;
			ScribeMetaHeaderUtility.loadedModNamesList = null;
			ScribeMetaHeaderUtility.lastMode = mode;
			if (Scribe.mode != LoadSaveMode.Inactive && Scribe.EnterNode("meta"))
			{
				try
				{
					Scribe_Values.Look<string>(ref ScribeMetaHeaderUtility.loadedGameVersion, "gameVersion", null, false);
					Scribe_Collections.Look<string>(ref ScribeMetaHeaderUtility.loadedModIdsList, "modIds", LookMode.Undefined, Array.Empty<object>());
					Scribe_Collections.Look<string>(ref ScribeMetaHeaderUtility.loadedModNamesList, "modNames", LookMode.Undefined, Array.Empty<object>());
					if (Scribe.mode == LoadSaveMode.LoadingVars && !ScribeMetaHeaderUtility.loadedGameVersion.NullOrEmpty())
					{
						try
						{
							ScribeMetaHeaderUtility.loadedGameVersionMajor = VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
							ScribeMetaHeaderUtility.loadedGameVersionMinor = VersionControl.MinorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
							ScribeMetaHeaderUtility.loadedGameVersionBuild = VersionControl.BuildFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion);
						}
						catch (Exception ex)
						{
							string str = "Error parsing loaded version. ";
							Exception ex2 = ex;
							Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
						}
					}
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			if (logVersionConflictWarning && (mode == ScribeMetaHeaderUtility.ScribeHeaderMode.Map || !UnityData.isEditor) && !ScribeMetaHeaderUtility.VersionsMatch())
			{
				Log.Warning(string.Concat(new string[]
				{
					"Loaded file (",
					mode.ToString(),
					") is from version ",
					ScribeMetaHeaderUtility.loadedGameVersion,
					", we are running version ",
					VersionControl.CurrentVersionStringWithRev,
					"."
				}));
			}
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x00120F18 File Offset: 0x0011F118
		private static bool VersionsMatch()
		{
			return ScribeMetaHeaderUtility.loadedGameVersionBuild == VersionControl.BuildFromVersionString(VersionControl.CurrentVersionStringWithRev);
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x00120F2C File Offset: 0x0011F12C
		public static bool TryCreateDialogsForVersionMismatchWarnings(Action confirmedAction)
		{
			string text = null;
			string title = null;
			if (!BackCompatibility.IsSaveCompatibleWith(ScribeMetaHeaderUtility.loadedGameVersion) && !ScribeMetaHeaderUtility.VersionsMatch())
			{
				title = "VersionMismatch".Translate();
				string value = ScribeMetaHeaderUtility.loadedGameVersion.NullOrEmpty() ? ("(" + "UnknownLower".TranslateSimple() + ")") : ScribeMetaHeaderUtility.loadedGameVersion;
				if (ScribeMetaHeaderUtility.lastMode == ScribeMetaHeaderUtility.ScribeHeaderMode.Map)
				{
					text = "SaveGameIncompatibleWarningText".Translate(value, VersionControl.CurrentVersionString);
				}
				else if (ScribeMetaHeaderUtility.lastMode == ScribeMetaHeaderUtility.ScribeHeaderMode.World)
				{
					text = "WorldFileVersionMismatch".Translate(value, VersionControl.CurrentVersionString);
				}
				else
				{
					text = "FileIncompatibleWarning".Translate(value, VersionControl.CurrentVersionString);
				}
			}
			string text2;
			string text3;
			if (!ScribeMetaHeaderUtility.LoadedModsMatchesActiveMods(out text2, out text3))
			{
				Find.WindowStack.Add(new Dialog_ModMismatch(confirmedAction, ScribeMetaHeaderUtility.loadedModIdsList, ScribeMetaHeaderUtility.loadedModNamesList));
				return true;
			}
			if (text != null)
			{
				Dialog_MessageBox dialog_MessageBox = Dialog_MessageBox.CreateConfirmation(text, confirmedAction, false, title, WindowLayer.Dialog);
				dialog_MessageBox.buttonAText = "LoadAnyway".Translate();
				Find.WindowStack.Add(dialog_MessageBox);
				return true;
			}
			return false;
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x0012106C File Offset: 0x0011F26C
		public static bool LoadedModsMatchesActiveMods(out string loadedModsSummary, out string runningModsSummary)
		{
			loadedModsSummary = null;
			runningModsSummary = null;
			List<string> list = (from mod in LoadedModManager.RunningMods
			select mod.PackageId).ToList<string>();
			List<string> b = (from mod in LoadedModManager.RunningMods
			select mod.FolderName).ToList<string>();
			if (ScribeMetaHeaderUtility.ModListsMatch(ScribeMetaHeaderUtility.loadedModIdsList, list) || ScribeMetaHeaderUtility.ModListsMatch(ScribeMetaHeaderUtility.loadedModIdsList, b))
			{
				return true;
			}
			if (ScribeMetaHeaderUtility.loadedModNamesList == null)
			{
				loadedModsSummary = "None".Translate();
			}
			else
			{
				loadedModsSummary = ScribeMetaHeaderUtility.loadedModNamesList.ToCommaList(false, false);
			}
			runningModsSummary = (from id in list
			select ModLister.GetModWithIdentifier(id, false).Name).ToCommaList(false, false);
			return false;
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x00121150 File Offset: 0x0011F350
		private static bool ModListsMatch(List<string> a, List<string> b)
		{
			if (a == null || b == null)
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (a[i].ToLower() != b[i].ToLower())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x001211A8 File Offset: 0x0011F3A8
		public static string GameVersionOf(FileInfo file)
		{
			if (!file.Exists)
			{
				throw new ArgumentException();
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(file.FullName))
				{
					using (XmlTextReader xmlTextReader = new XmlTextReader(streamReader))
					{
						if (ScribeMetaHeaderUtility.ReadToMetaElement(xmlTextReader) && xmlTextReader.ReadToDescendant("gameVersion"))
						{
							return VersionControl.VersionStringWithoutRev(xmlTextReader.ReadString());
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Exception getting game version of " + file.Name + ": " + ex.ToString());
			}
			return null;
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x00121264 File Offset: 0x0011F464
		public static bool ReadToMetaElement(XmlTextReader textReader)
		{
			return ScribeMetaHeaderUtility.ReadToNextElement(textReader) && ScribeMetaHeaderUtility.ReadToNextElement(textReader) && !(textReader.Name != "meta");
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x0012128F File Offset: 0x0011F48F
		private static bool ReadToNextElement(XmlTextReader textReader)
		{
			while (textReader.Read())
			{
				if (textReader.NodeType == XmlNodeType.Element)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040028C4 RID: 10436
		private static ScribeMetaHeaderUtility.ScribeHeaderMode lastMode;

		// Token: 0x040028C5 RID: 10437
		public static string loadedGameVersion;

		// Token: 0x040028C6 RID: 10438
		public static int loadedGameVersionMajor;

		// Token: 0x040028C7 RID: 10439
		public static int loadedGameVersionMinor;

		// Token: 0x040028C8 RID: 10440
		public static int loadedGameVersionBuild;

		// Token: 0x040028C9 RID: 10441
		public static List<string> loadedModIdsList;

		// Token: 0x040028CA RID: 10442
		public static List<string> loadedModNamesList;

		// Token: 0x040028CB RID: 10443
		public static List<int> loadedModSteamIdsList;

		// Token: 0x040028CC RID: 10444
		public const string MetaNodeName = "meta";

		// Token: 0x040028CD RID: 10445
		public const string GameVersionNodeName = "gameVersion";

		// Token: 0x040028CE RID: 10446
		public const string ModIdsNodeName = "modIds";

		// Token: 0x040028CF RID: 10447
		public const string ModNamesNodeName = "modNames";

		// Token: 0x040028D0 RID: 10448
		public const string ModSteamIdsNodeName = "modSteamIds";

		// Token: 0x0200085F RID: 2143
		public enum ScribeHeaderMode
		{
			// Token: 0x040028D2 RID: 10450
			None,
			// Token: 0x040028D3 RID: 10451
			Map,
			// Token: 0x040028D4 RID: 10452
			World,
			// Token: 0x040028D5 RID: 10453
			Scenario,
			// Token: 0x040028D6 RID: 10454
			Ideo,
			// Token: 0x040028D7 RID: 10455
			Xenotype,
			// Token: 0x040028D8 RID: 10456
			Xenogerm,
			// Token: 0x040028D9 RID: 10457
			ModList,
			// Token: 0x040028DA RID: 10458
			CameraConfig
		}
	}
}
