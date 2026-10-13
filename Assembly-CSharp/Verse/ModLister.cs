using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RimWorld;
using Steamworks;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000574 RID: 1396
	public static class ModLister
	{
		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x0600244A RID: 9290 RVA: 0x000BC843 File Offset: 0x000BAA43
		public static IEnumerable<ModMetaData> AllInstalledMods
		{
			get
			{
				return ModLister.mods;
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x0600244B RID: 9291 RVA: 0x000BC84C File Offset: 0x000BAA4C
		public static IEnumerable<DirectoryInfo> AllActiveModDirs
		{
			get
			{
				return from mod in ModLister.mods
				where mod.Active
				select mod.RootDir;
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x0600244C RID: 9292 RVA: 0x000BC8A8 File Offset: 0x000BAAA8
		public static List<ExpansionDef> AllExpansions
		{
			get
			{
				if (ModLister.AllExpansionsCached.NullOrEmpty<ExpansionDef>())
				{
					ModLister.AllExpansionsCached = DefDatabase<ExpansionDef>.AllDefsListForReading.Where(delegate(ExpansionDef e)
					{
						ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(e.linkedMod, false);
						return modWithIdentifier == null || modWithIdentifier.Official;
					}).ToList<ExpansionDef>();
				}
				return ModLister.AllExpansionsCached;
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x0600244D RID: 9293 RVA: 0x000BC8F9 File Offset: 0x000BAAF9
		public static bool RoyaltyInstalled
		{
			get
			{
				return ModLister.royaltyInstalled;
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x0600244E RID: 9294 RVA: 0x000BC900 File Offset: 0x000BAB00
		public static bool IdeologyInstalled
		{
			get
			{
				return ModLister.ideologyInstalled;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x0600244F RID: 9295 RVA: 0x000BC907 File Offset: 0x000BAB07
		public static bool BiotechInstalled
		{
			get
			{
				return ModLister.biotechInstalled;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06002450 RID: 9296 RVA: 0x000BC90E File Offset: 0x000BAB0E
		public static bool AnomalyInstalled
		{
			get
			{
				return ModLister.anomalyInstalled;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06002451 RID: 9297 RVA: 0x000BC915 File Offset: 0x000BAB15
		public static bool OdysseyInstalled
		{
			get
			{
				return ModLister.odysseyInstalled;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06002452 RID: 9298 RVA: 0x000BC91C File Offset: 0x000BAB1C
		public static bool ShouldLogIssues
		{
			get
			{
				return !ModLister.modListBuilt && !ModLister.nestedRebuildInProgress;
			}
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x000BC92F File Offset: 0x000BAB2F
		static ModLister()
		{
			ModLister.RebuildModList();
			ModLister.modListBuilt = true;
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x000026EA File Offset: 0x000008EA
		public static void EnsureInit()
		{
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x000BC964 File Offset: 0x000BAB64
		public static void RebuildModList()
		{
			ModLister.nestedRebuildInProgress = ModLister.rebuildingModList;
			ModLister.rebuildingModList = true;
			string s = "Rebuilding mods list";
			ModLister.mods.Clear();
			ModLister.modsByPackageId.Clear();
			ModLister.modsByPackageIdIgnorePostfix.Clear();
			WorkshopItems.EnsureInit();
			DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.LogErrors);
			s += "\nAdding official mods from content folder:";
			foreach (string localAbsPath in from d in new DirectoryInfo(GenFilePaths.OfficialModsFolderPath).GetDirectories()
			select d.FullName)
			{
				ModMetaData modMetaData = new ModMetaData(localAbsPath, true);
				if (ModLister.TryAddMod(modMetaData))
				{
					s = s + "\n  Adding " + modMetaData.ToStringLong();
				}
			}
			s += "\nAdding mods from mods folder:";
			foreach (string localAbsPath2 in from d in new DirectoryInfo(GenFilePaths.ModsFolderPath).GetDirectories()
			select d.FullName)
			{
				ModMetaData modMetaData2 = new ModMetaData(localAbsPath2, false);
				if (ModLister.TryAddMod(modMetaData2))
				{
					s = s + "\n  Adding " + modMetaData2.ToStringLong();
				}
			}
			s += "\nAdding mods from Steam:";
			foreach (WorkshopItem workshopItem in from it in WorkshopItems.AllSubscribedItems
			where it is WorkshopItem_Mod
			select it)
			{
				ModMetaData modMetaData3 = new ModMetaData(workshopItem);
				if (ModLister.TryAddMod(modMetaData3))
				{
					s = s + "\n  Adding " + modMetaData3.ToStringLong();
				}
			}
			s += "\nDeactivating not-installed mods:";
			ModsConfig.DeactivateNotInstalledMods(delegate(string log)
			{
				s = s + "\n   " + log;
			});
			if (ModLister.mods.Count((ModMetaData m) => m.Active) == 0)
			{
				s += "\nThere are no active mods. Activating Core mod.";
				ModLister.mods.First((ModMetaData m) => m.IsCoreMod).Active = true;
			}
			ModLister.RecacheExpansionsInstalled();
			if (Prefs.LogVerbose)
			{
				Log.Message(s);
			}
			ModLister.rebuildingModList = false;
			ModLister.nestedRebuildInProgress = false;
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x000BCC5C File Offset: 0x000BAE5C
		public static int InstalledModsListHash(bool activeOnly)
		{
			int num = 17;
			int num2 = 0;
			foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder)
			{
				if (!activeOnly || ModsConfig.IsActive(modMetaData.PackageId))
				{
					num = num * 31 + modMetaData.GetHashCode();
					num = num * 31 + num2 * 2654241;
					num2++;
				}
			}
			return num;
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x000BCCD4 File Offset: 0x000BAED4
		public static ModMetaData GetModWithIdentifier(string identifier, bool ignorePostfix = false)
		{
			if (ignorePostfix)
			{
				if (!ModLister.modsByPackageIdIgnorePostfix.ContainsKey(identifier))
				{
					return null;
				}
				return ModLister.modsByPackageIdIgnorePostfix[identifier].ElementAtOrDefault(0);
			}
			else
			{
				if (!ModLister.modsByPackageId.ContainsKey(identifier))
				{
					return null;
				}
				return ModLister.modsByPackageId[identifier].ElementAtOrDefault(0);
			}
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x000BCD28 File Offset: 0x000BAF28
		public static ModMetaData GetActiveModWithIdentifier(string identifier, bool ignorePostfix = false)
		{
			Dictionary<string, List<ModMetaData>> dictionary = ignorePostfix ? ModLister.modsByPackageIdIgnorePostfix : ModLister.modsByPackageId;
			if (!dictionary.ContainsKey(identifier))
			{
				return null;
			}
			foreach (ModMetaData modMetaData in dictionary[identifier])
			{
				if (modMetaData.Active)
				{
					return modMetaData;
				}
			}
			return null;
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x000BCDA0 File Offset: 0x000BAFA0
		public static ExpansionDef GetExpansionWithIdentifier(string packageId)
		{
			for (int i = 0; i < ModLister.AllExpansions.Count; i++)
			{
				if (ModLister.AllExpansions[i].linkedMod == packageId)
				{
					return ModLister.AllExpansions[i];
				}
			}
			return null;
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000BCDE8 File Offset: 0x000BAFE8
		public static bool HasActiveModWithName(string name)
		{
			foreach (ModMetaData modMetaData in ModLister.mods)
			{
				if (modMetaData.Active && modMetaData.Name == name)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x000BCE50 File Offset: 0x000BB050
		public static bool AnyFromListActive(List<string> mods)
		{
			using (List<string>.Enumerator enumerator = mods.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (ModLister.GetActiveModWithIdentifier(enumerator.Current, false) != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x000BCEA8 File Offset: 0x000BB0A8
		private static void RecacheExpansionsInstalled()
		{
			ModLister.royaltyInstalled = ModLister.modsByPackageId.ContainsKey("ludeon.rimworld.royalty");
			ModLister.ideologyInstalled = ModLister.modsByPackageId.ContainsKey("ludeon.rimworld.ideology");
			ModLister.biotechInstalled = ModLister.modsByPackageId.ContainsKey("ludeon.rimworld.biotech");
			ModLister.anomalyInstalled = ModLister.modsByPackageId.ContainsKey("ludeon.rimworld.anomaly");
			ModLister.odysseyInstalled = ModLister.modsByPackageId.ContainsKey("ludeon.rimworld.odyssey");
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x000BCF1C File Offset: 0x000BB11C
		private static bool TryAddMod(ModMetaData mod)
		{
			if (mod.Official && !mod.IsCoreMod && SteamManager.Initialized && mod.SteamAppId != 0U)
			{
				bool flag = true;
				try
				{
					flag = SteamApps.BIsDlcInstalled(new AppId_t(mod.SteamAppId));
				}
				catch (Exception ex)
				{
					string str = "Could not determine if a DLC is installed: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				if (!flag)
				{
					return false;
				}
			}
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(mod.PackageId, false);
			if (modWithIdentifier == null)
			{
				ModLister.mods.Add(mod);
				if (ModLister.modsByPackageId.ContainsKey(mod.PackageId))
				{
					ModLister.modsByPackageId[mod.PackageId].Add(mod);
				}
				else
				{
					ModLister.modsByPackageId.Add(mod.PackageId, new List<ModMetaData>
					{
						mod
					});
				}
				if (ModLister.modsByPackageIdIgnorePostfix.ContainsKey(mod.packageIdLowerCase))
				{
					ModLister.modsByPackageIdIgnorePostfix[mod.packageIdLowerCase].Add(mod);
				}
				else
				{
					ModLister.modsByPackageIdIgnorePostfix.Add(mod.packageIdLowerCase, new List<ModMetaData>
					{
						mod
					});
				}
				return true;
			}
			if (mod.RootDir.FullName != modWithIdentifier.RootDir.FullName)
			{
				if (mod.OnSteamWorkshop != modWithIdentifier.OnSteamWorkshop)
				{
					ModMetaData modMetaData = mod.OnSteamWorkshop ? mod : modWithIdentifier;
					if (!modMetaData.appendPackageIdSteamPostfix)
					{
						modMetaData.appendPackageIdSteamPostfix = true;
						return ModLister.TryAddMod(mod);
					}
				}
				Log.Error(string.Concat(new string[]
				{
					"Tried loading mod with the same packageId multiple times: ",
					mod.PackageIdPlayerFacing,
					". Ignoring the duplicates.\n",
					mod.RootDir.FullName,
					"\n",
					modWithIdentifier.RootDir.FullName
				}));
				return false;
			}
			return false;
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x000BD0E0 File Offset: 0x000BB2E0
		private static bool CheckDLC(bool dlc, string featureName, string dlcNameIndef, string installedPropertyName)
		{
			if (!dlc)
			{
				Log.ErrorOnce(string.Concat(new string[]
				{
					featureName,
					" is ",
					dlcNameIndef,
					"-specific game system. If you want to use this code please check ModLister.",
					installedPropertyName,
					" before calling it."
				}), featureName.GetHashCode());
			}
			return dlc;
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x000BD120 File Offset: 0x000BB320
		public static bool CheckRoyalty(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled, featureNameSingular, "a Royalty", "RoyaltyInstalled");
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x000BD137 File Offset: 0x000BB337
		public static bool CheckIdeology(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.IdeologyInstalled, featureNameSingular, "an Ideology", "IdeologyInstalled");
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x000BD14E File Offset: 0x000BB34E
		public static bool CheckBiotech(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.BiotechInstalled, featureNameSingular, "a Biotech", "BiotechInstalled");
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x000BD165 File Offset: 0x000BB365
		public static bool CheckAnomaly(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.AnomalyInstalled, featureNameSingular, "an Anomaly", "AnomalyInstalled");
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x000BD17C File Offset: 0x000BB37C
		public static bool CheckOdyssey(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.OdysseyInstalled, featureNameSingular, "an Odyssey", "OdysseyInstalled");
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x000BD193 File Offset: 0x000BB393
		public static bool CheckIdeologyOrBiotech(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.IdeologyInstalled || ModLister.BiotechInstalled, featureNameSingular, "a Ideology or Biotech", "IdeologyInstalled or BiotechInstalled");
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x000BD1B4 File Offset: 0x000BB3B4
		public static bool CheckRoyaltyAndIdeology(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled && ModLister.IdeologyInstalled, featureNameSingular, "a Royalty and Ideology", "RoyaltyInstalled and IdeologyInstalled");
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x000BD1D5 File Offset: 0x000BB3D5
		public static bool CheckRoyaltyOrIdeology(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.IdeologyInstalled, featureNameSingular, "a Royalty or Ideology", "RoyaltyInstalled or IdeologyInstalled");
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x000BD1F6 File Offset: 0x000BB3F6
		public static bool CheckRoyaltyOrBiotech(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.BiotechInstalled, featureNameSingular, "a Royalty or Biotech", "RoyaltyInstalled or BiotechInstalled");
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x000BD217 File Offset: 0x000BB417
		public static bool CheckRoyaltyOrAnomaly(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.AnomalyInstalled, featureNameSingular, "a Royalty or Anomaly", "RoyaltyInstalled or AnomalyInstalled");
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x000BD238 File Offset: 0x000BB438
		public static bool CheckBiotechOrAnomaly(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.BiotechInstalled || ModLister.AnomalyInstalled, featureNameSingular, "a Biotech or Anomaly", "BiotechInstalled or AnomalyInstalled");
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x000BD259 File Offset: 0x000BB459
		public static bool CheckRoyaltyOrOdyssey(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.OdysseyInstalled, featureNameSingular, "a Royalty or Odyssey", "RoyaltyInstalled or OdysseyInstalled");
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x000BD27A File Offset: 0x000BB47A
		public static bool CheckRoyaltyOrIdeologyOrBiotech(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.BiotechInstalled || ModLister.IdeologyInstalled, featureNameSingular, "a Royalty or Ideology or Biotech", "RoyaltyInstalled or IdeologyInstalled or BiotechInstalled");
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x000BD2A2 File Offset: 0x000BB4A2
		public static bool CheckBiotechOrAnomalyOrOdyssey(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.BiotechInstalled || ModLister.AnomalyInstalled || ModLister.OdysseyInstalled, featureNameSingular, "a Biotech or Anomaly or Odyssey", "BiotechInstalled or AnomalyInstalled or OdysseyInstalled");
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x000BD2CA File Offset: 0x000BB4CA
		public static bool CheckAnyExpansion(string featureNameSingular)
		{
			return ModLister.CheckDLC(ModLister.RoyaltyInstalled || ModLister.IdeologyInstalled || ModLister.BiotechInstalled || ModLister.AnomalyInstalled || ModLister.OdysseyInstalled, featureNameSingular, "a Royalty or Ideology or Biotech or Anomaly or Odyssey", "RoyaltyInstalled or IdeologyInstalled or BiotechInstalled or AnomalyInstalled or OdysseyInstalled");
		}

		// Token: 0x04001E09 RID: 7689
		private static List<ModMetaData> mods = new List<ModMetaData>();

		// Token: 0x04001E0A RID: 7690
		private static Dictionary<string, List<ModMetaData>> modsByPackageId = new Dictionary<string, List<ModMetaData>>(StringComparer.CurrentCultureIgnoreCase);

		// Token: 0x04001E0B RID: 7691
		private static Dictionary<string, List<ModMetaData>> modsByPackageIdIgnorePostfix = new Dictionary<string, List<ModMetaData>>(StringComparer.CurrentCultureIgnoreCase);

		// Token: 0x04001E0C RID: 7692
		private static bool modListBuilt;

		// Token: 0x04001E0D RID: 7693
		private static bool rebuildingModList;

		// Token: 0x04001E0E RID: 7694
		private static bool nestedRebuildInProgress;

		// Token: 0x04001E0F RID: 7695
		private static List<ExpansionDef> AllExpansionsCached;

		// Token: 0x04001E10 RID: 7696
		private static bool royaltyInstalled;

		// Token: 0x04001E11 RID: 7697
		private static bool ideologyInstalled;

		// Token: 0x04001E12 RID: 7698
		private static bool biotechInstalled;

		// Token: 0x04001E13 RID: 7699
		private static bool anomalyInstalled;

		// Token: 0x04001E14 RID: 7700
		private static bool odysseyInstalled;
	}
}
