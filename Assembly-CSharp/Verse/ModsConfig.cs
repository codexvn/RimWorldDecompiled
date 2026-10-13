using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000585 RID: 1413
	public static class ModsConfig
	{
		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x0600250B RID: 9483 RVA: 0x000BF314 File Offset: 0x000BD514
		public static IEnumerable<ModMetaData> ActiveModsInLoadOrder
		{
			get
			{
				ModLister.EnsureInit();
				if (ModsConfig.activeModsInLoadOrderCachedDirty)
				{
					ModsConfig.activeModsInLoadOrderCached.Clear();
					for (int i = 0; i < ModsConfig.data.activeMods.Count; i++)
					{
						ModsConfig.activeModsInLoadOrderCached.Add(ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false));
					}
					ModsConfig.activeModsInLoadOrderCachedDirty = false;
				}
				return ModsConfig.activeModsInLoadOrderCached;
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x0600250C RID: 9484 RVA: 0x000BF37C File Offset: 0x000BD57C
		public static ExpansionDef LastInstalledExpansion
		{
			get
			{
				for (int i = ModsConfig.data.knownExpansions.Count - 1; i >= 0; i--)
				{
					ExpansionDef expansionWithIdentifier = ModLister.GetExpansionWithIdentifier(ModsConfig.data.knownExpansions[i]);
					if (expansionWithIdentifier != null && !expansionWithIdentifier.isCore && expansionWithIdentifier.Status != ExpansionStatus.NotInstalled)
					{
						return expansionWithIdentifier;
					}
				}
				return null;
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x0600250D RID: 9485 RVA: 0x000BF3D2 File Offset: 0x000BD5D2
		public static bool RoyaltyActive
		{
			get
			{
				return ModsConfig.royaltyActive;
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x0600250E RID: 9486 RVA: 0x000BF3D9 File Offset: 0x000BD5D9
		public static bool IdeologyActive
		{
			get
			{
				return ModsConfig.ideologyActive;
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x0600250F RID: 9487 RVA: 0x000BF3E0 File Offset: 0x000BD5E0
		public static bool BiotechActive
		{
			get
			{
				return ModsConfig.biotechActive;
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06002510 RID: 9488 RVA: 0x000BF3E7 File Offset: 0x000BD5E7
		public static bool AnomalyActive
		{
			get
			{
				return ModsConfig.anomalyActive;
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06002511 RID: 9489 RVA: 0x000BF3EE File Offset: 0x000BD5EE
		public static bool OdysseyActive
		{
			get
			{
				return ModsConfig.odysseyActive;
			}
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x000BF3F8 File Offset: 0x000BD5F8
		static ModsConfig()
		{
			bool flag = false;
			bool flag2 = false;
			ModsConfig.data = DirectXmlLoader.ItemFromXmlFile<ModsConfig.ModsConfigData>(GenFilePaths.ModsConfigFilePath, true);
			if (ModsConfig.data.version != null)
			{
				bool flag3 = false;
				int num2;
				if (ModsConfig.data.version.Contains("."))
				{
					int num = VersionControl.MinorFromVersionString(ModsConfig.data.version);
					if (VersionControl.MajorFromVersionString(ModsConfig.data.version) != VersionControl.CurrentMajor || num != VersionControl.CurrentMinor)
					{
						flag3 = true;
					}
				}
				else if (ModsConfig.data.version.Length > 0 && ModsConfig.data.version.All(new Func<char, bool>(char.IsNumber)) && int.TryParse(ModsConfig.data.version, out num2) && num2 <= 2009)
				{
					flag3 = true;
				}
				if (flag3)
				{
					Log.Message(string.Concat(new string[]
					{
						"Mods config data is from version ",
						ModsConfig.data.version,
						" while we are running ",
						VersionControl.CurrentVersionStringWithRev,
						". Resetting."
					}));
					ModsConfig.data = new ModsConfig.ModsConfigData();
					flag = true;
				}
			}
			for (int i = 0; i < ModsConfig.data.activeMods.Count; i++)
			{
				string packageId = ModsConfig.data.activeMods[i];
				if (ModLister.GetModWithIdentifier(packageId, false) == null)
				{
					ModMetaData modMetaData = ModLister.AllInstalledMods.FirstOrDefault((ModMetaData m) => m.FolderName == packageId);
					if (modMetaData != null)
					{
						ModsConfig.data.activeMods[i] = modMetaData.PackageId;
						flag2 = true;
					}
					string text;
					if (ModsConfig.TryGetPackageIdWithoutExtraSteamPostfix(packageId, out text) && ModLister.GetModWithIdentifier(text, false) != null)
					{
						ModsConfig.data.activeMods[i] = text;
					}
				}
			}
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ModMetaData modMetaData2 in ModLister.AllInstalledMods)
			{
				if (modMetaData2.Active)
				{
					if (hashSet.Contains(modMetaData2.PackageIdNonUnique))
					{
						modMetaData2.Active = false;
						Log.Warning("There was more than one enabled instance of mod with PackageID: " + modMetaData2.PackageIdNonUnique + ". Disabling the duplicates.");
						continue;
					}
					hashSet.Add(modMetaData2.PackageIdNonUnique);
				}
				if (!modMetaData2.IsCoreMod && modMetaData2.Official && ModsConfig.IsExpansionNew(modMetaData2.PackageId))
				{
					ModsConfig.SetActive(modMetaData2.PackageId, true);
					ModsConfig.AddKnownExpansion(modMetaData2.PackageId);
					int num3 = ModsConfig.data.activeMods.IndexOf(modMetaData2.PackageId);
					if (!modMetaData2.ForceLoadAfter.NullOrEmpty<string>())
					{
						foreach (string identifier in modMetaData2.ForceLoadAfter)
						{
							ModMetaData activeModWithIdentifier = ModLister.GetActiveModWithIdentifier(identifier, false);
							if (activeModWithIdentifier != null)
							{
								int num4 = ModsConfig.data.activeMods.IndexOf(activeModWithIdentifier.PackageId);
								if (num4 != -1 && num4 > num3)
								{
									string text2;
									ModsConfig.TryReorder(num3, num4, out text2);
									Gen.Swap<int>(ref num4, ref num3);
								}
							}
						}
					}
					if (!modMetaData2.ForceLoadBefore.NullOrEmpty<string>())
					{
						foreach (string identifier2 in modMetaData2.ForceLoadBefore)
						{
							ModMetaData activeModWithIdentifier2 = ModLister.GetActiveModWithIdentifier(identifier2, false);
							if (activeModWithIdentifier2 != null)
							{
								int num5 = ModsConfig.data.activeMods.IndexOf(activeModWithIdentifier2.PackageId);
								if (num5 != -1 && num5 < num3)
								{
									string text2;
									ModsConfig.TryReorder(num3, num5, out text2);
									Gen.Swap<int>(ref num5, ref num3);
								}
							}
						}
					}
					Prefs.Notify_NewExpansion();
					flag2 = true;
				}
			}
			if (ModsConfig.newKnownExpansions.Any<string>())
			{
				ModsConfig.newKnownExpansions.SortBy((string x) => ModsConfig.ExpansionsInReleaseOrder.IndexOf(x));
				ModsConfig.data.knownExpansions.AddRange(ModsConfig.newKnownExpansions);
				ModsConfig.newKnownExpansions.Clear();
			}
			if (!File.Exists(GenFilePaths.ModsConfigFilePath) || flag)
			{
				ModsConfig.Reset();
			}
			else if (flag2)
			{
				ModsConfig.Save();
			}
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x000BF8D0 File Offset: 0x000BDAD0
		public static bool TryGetPackageIdWithoutExtraSteamPostfix(string packageId, out string nonSteamPackageId)
		{
			if (packageId.EndsWith(ModMetaData.SteamModPostfix))
			{
				nonSteamPackageId = packageId.Substring(0, packageId.Length - ModMetaData.SteamModPostfix.Length);
				return true;
			}
			nonSteamPackageId = null;
			return false;
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x000BF900 File Offset: 0x000BDB00
		public static void DeactivateNotInstalledMods(Action<string> logCallback = null)
		{
			for (int i = ModsConfig.data.activeMods.Count - 1; i >= 0; i--)
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false);
				string identifier;
				if (modWithIdentifier == null && ModsConfig.TryGetPackageIdWithoutExtraSteamPostfix(ModsConfig.data.activeMods[i], out identifier))
				{
					modWithIdentifier = ModLister.GetModWithIdentifier(identifier, false);
				}
				if (modWithIdentifier == null)
				{
					if (logCallback != null)
					{
						logCallback("Deactivating " + ModsConfig.data.activeMods[i]);
					}
					ModsConfig.data.activeMods.RemoveAt(i);
				}
			}
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x000BF9A0 File Offset: 0x000BDBA0
		public static void Reset()
		{
			ModsConfig.data.activeMods.Clear();
			ModsConfig.data.activeMods.Add("ludeon.rimworld");
			foreach (ModMetaData modMetaData in ModLister.AllInstalledMods)
			{
				if (modMetaData.Official && !modMetaData.IsCoreMod && modMetaData.VersionCompatible)
				{
					ModsConfig.data.activeMods.Add(modMetaData.PackageId);
				}
			}
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
			ModsConfig.TrySortMods();
			ModsConfig.Save();
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x000BFA4C File Offset: 0x000BDC4C
		public static bool TryReorder(int modIndex, int newIndex, out string errorMessage)
		{
			errorMessage = null;
			if (modIndex == newIndex)
			{
				return false;
			}
			if (ModsConfig.ReorderConflict(modIndex, newIndex, out errorMessage))
			{
				return false;
			}
			ModsConfig.data.activeMods.Insert(newIndex, ModsConfig.data.activeMods[modIndex]);
			ModsConfig.data.activeMods.RemoveAt((modIndex < newIndex) ? modIndex : (modIndex + 1));
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
			return true;
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x000BFAB0 File Offset: 0x000BDCB0
		private static bool ReorderConflict(int modIndex, int newIndex, out string errorMessage)
		{
			errorMessage = null;
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[modIndex], false);
			if (modWithIdentifier.IsCoreMod)
			{
				foreach (string text in ModsConfig.ExpansionsInReleaseOrder)
				{
					int num = ModsConfig.data.activeMods.IndexOf(text);
					if (num != -1 && num < newIndex)
					{
						errorMessage = "ModReorderConflict_MustLoadBefore".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(text, false).Name);
						return true;
					}
				}
			}
			if (modWithIdentifier.Source == ContentSource.OfficialModsFolder && ModsConfig.data.activeMods.IndexOf(ModsConfig.Core) >= newIndex)
			{
				errorMessage = "ModReorderConflict_MustLoadAfter".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(ModsConfig.Core, false).Name);
				return true;
			}
			if (!modWithIdentifier.ForceLoadBefore.NullOrEmpty<string>())
			{
				foreach (string identifier in modWithIdentifier.ForceLoadBefore)
				{
					ModMetaData modWithIdentifier2 = ModLister.GetModWithIdentifier(identifier, false);
					if (modWithIdentifier2 != null)
					{
						for (int i = newIndex - 1; i >= 0; i--)
						{
							if (modWithIdentifier2.SamePackageId(ModsConfig.data.activeMods[i], false))
							{
								errorMessage = "ModReorderConflict_MustLoadBefore".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[i], false).Name);
								return true;
							}
						}
					}
				}
			}
			if (!modWithIdentifier.ForceLoadAfter.NullOrEmpty<string>())
			{
				foreach (string identifier2 in modWithIdentifier.ForceLoadAfter)
				{
					ModMetaData modWithIdentifier3 = ModLister.GetModWithIdentifier(identifier2, false);
					if (modWithIdentifier3 != null)
					{
						for (int j = newIndex; j < ModsConfig.data.activeMods.Count; j++)
						{
							if (modWithIdentifier3.SamePackageId(ModsConfig.data.activeMods[j], false))
							{
								errorMessage = "ModReorderConflict_MustLoadAfter".Translate(modWithIdentifier.Name, ModLister.GetModWithIdentifier(ModsConfig.data.activeMods[j], false).Name);
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x000BFD68 File Offset: 0x000BDF68
		public static void Reorder(List<int> newIndices)
		{
			List<string> list = new List<string>();
			foreach (int index in newIndices)
			{
				list.Add(ModsConfig.data.activeMods[index]);
			}
			ModsConfig.data.activeMods = list;
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x000BFDDC File Offset: 0x000BDFDC
		public static bool IsActive(ModMetaData mod)
		{
			return ModsConfig.IsActive(mod.PackageId);
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x000BFDE9 File Offset: 0x000BDFE9
		public static bool AreAllActive(string mods)
		{
			if (mods != null && mods.Contains(','))
			{
				return ModsConfig.AreAllActive(mods.ToLower().Split(',', StringSplitOptions.None));
			}
			return ModsConfig.IsActive(mods);
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x000BFE14 File Offset: 0x000BE014
		public static bool AreAllActive(IEnumerable<string> mods)
		{
			using (IEnumerator<string> enumerator = mods.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!ModsConfig.IsActive(enumerator.Current.Trim()))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x000BFE68 File Offset: 0x000BE068
		public static bool IsAnyActiveOrEmpty(IEnumerable<string> mods, bool trimNames = false)
		{
			if (!mods.EnumerableNullOrEmpty<string>())
			{
				foreach (string text in mods)
				{
					if (ModsConfig.IsActive(trimNames ? text.Trim() : text))
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x000BFED0 File Offset: 0x000BE0D0
		public static bool IsActive(string id)
		{
			return ModsConfig.activeModsHashSet.Contains(id.ToLower());
		}

		// Token: 0x0600251E RID: 9502 RVA: 0x000BFEE2 File Offset: 0x000BE0E2
		public static void SetActive(ModMetaData mod, bool active)
		{
			ModsConfig.SetActive(mod.PackageId, active);
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x000BFEF0 File Offset: 0x000BE0F0
		public static void SetActive(string modIdentifier, bool active)
		{
			string item = modIdentifier.ToLower();
			if (active)
			{
				if (!ModsConfig.data.activeMods.Contains(item))
				{
					ModsConfig.data.activeMods.Add(item);
					ModsConfig.EnsureModAdheresToForcedLoadOrder(modIdentifier);
				}
			}
			else if (ModsConfig.data.activeMods.Contains(item))
			{
				ModsConfig.data.activeMods.Remove(item);
			}
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x000BFF5C File Offset: 0x000BE15C
		public static void EnsureModAdheresToForcedLoadOrder(string modIdentifier)
		{
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(modIdentifier, false);
			if (modWithIdentifier == null)
			{
				return;
			}
			string item = modIdentifier.ToLower();
			if (!ModsConfig.data.activeMods.Contains(item))
			{
				return;
			}
			int? num = null;
			int num2 = ModsConfig.data.activeMods.IndexOf(item);
			if (!modWithIdentifier.ForceLoadAfter.NullOrEmpty<string>())
			{
				foreach (string text in modWithIdentifier.ForceLoadAfter)
				{
					int num3 = ModsConfig.data.activeMods.IndexOf(text.ToLower());
					if (num3 != -1)
					{
						num = new int?(Mathf.Max(num ?? int.MinValue, num3 + 1));
					}
				}
			}
			if (!modWithIdentifier.ForceLoadBefore.NullOrEmpty<string>())
			{
				foreach (string text2 in modWithIdentifier.ForceLoadBefore)
				{
					int num4 = ModsConfig.data.activeMods.IndexOf(text2.ToLower());
					if (num4 != -1)
					{
						num = new int?(Mathf.Min(num ?? int.MaxValue, num4));
					}
				}
			}
			if (num != null && num.Value != num2)
			{
				ModsConfig.data.activeMods.Remove(item);
				ModsConfig.data.activeMods.Insert(num.Value, item);
			}
		}

		// Token: 0x06002521 RID: 9505 RVA: 0x000C0108 File Offset: 0x000BE308
		public static void SetActiveToList(List<string> mods)
		{
			ModsConfig.data.activeMods = (from mod in mods
			where ModLister.GetModWithIdentifier(mod, false) != null
			select mod).ToList<string>();
			ModsConfig.RecacheActiveMods();
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x000C0143 File Offset: 0x000BE343
		public static bool IsExpansionNew(string id)
		{
			return !ModsConfig.data.knownExpansions.Contains(id.ToLower());
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x000C015D File Offset: 0x000BE35D
		public static void AddKnownExpansion(string id)
		{
			if (!ModsConfig.IsExpansionNew(id))
			{
				Log.Error("Tried to add already known expansion: " + id);
				return;
			}
			ModsConfig.newKnownExpansions.Add(id.ToLower());
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x000C0188 File Offset: 0x000BE388
		public static void Save()
		{
			ModsConfig.data.version = VersionControl.CurrentVersionStringWithRev;
			DirectXmlSaver.SaveDataObject(ModsConfig.data, GenFilePaths.ModsConfigFilePath);
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x000C01A8 File Offset: 0x000BE3A8
		public static void SaveFromList(List<string> mods)
		{
			DirectXmlSaver.SaveDataObject(new ModsConfig.ModsConfigData
			{
				version = VersionControl.CurrentVersionStringWithRev,
				activeMods = mods,
				knownExpansions = ModsConfig.data.knownExpansions
			}, GenFilePaths.ModsConfigFilePath);
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x000C01DC File Offset: 0x000BE3DC
		public static void RestartFromChangedMods()
		{
			Find.WindowStack.Add(new Dialog_MessageBox("ModsChanged".Translate(), null, delegate()
			{
				GenCommandLine.Restart();
			}, null, null, null, false, null, null, WindowLayer.Dialog));
		}

		// Token: 0x06002527 RID: 9511 RVA: 0x000C022C File Offset: 0x000BE42C
		public static Dictionary<string, string> GetModWarnings()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			List<ModMetaData> mods = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			for (int i = 0; i < mods.Count; i++)
			{
				int index = i;
				ModMetaData modMetaData = mods[index];
				StringBuilder stringBuilder = new StringBuilder("");
				ModMetaData activeModWithIdentifier = ModLister.GetActiveModWithIdentifier(modMetaData.PackageId, false);
				if (activeModWithIdentifier != null && modMetaData != activeModWithIdentifier)
				{
					stringBuilder.AppendLine("ModWithSameIdAlreadyActive".Translate(activeModWithIdentifier.Name));
				}
				List<string> list = ModsConfig.FindConflicts(modMetaData.IncompatibleWith, null);
				if (list.Any<string>())
				{
					stringBuilder.AppendLine("ModIncompatibleWithTip".Translate(list.ToCommaList(true, false)));
				}
				List<string> list2 = ModsConfig.FindConflicts(modMetaData.LoadBefore, (ModMetaData beforeMod) => mods.IndexOf(beforeMod) < index);
				if (list2.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadBefore".Translate(list2.ToCommaList(true, false)));
				}
				List<string> list3 = ModsConfig.FindConflicts(modMetaData.ForceLoadBefore, (ModMetaData beforeMod) => mods.IndexOf(beforeMod) < index);
				if (list3.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadBefore".Translate(list3.ToCommaList(true, false)));
				}
				List<string> list4 = ModsConfig.FindConflicts(modMetaData.LoadAfter, (ModMetaData afterMod) => mods.IndexOf(afterMod) > index);
				if (list4.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadAfter".Translate(list4.ToCommaList(true, false)));
				}
				List<string> list5 = ModsConfig.FindConflicts(modMetaData.ForceLoadAfter, (ModMetaData afterMod) => mods.IndexOf(afterMod) > index);
				if (list5.Any<string>())
				{
					stringBuilder.AppendLine("ModMustLoadAfter".Translate(list5.ToCommaList(true, false)));
				}
				if (modMetaData.Dependencies.Any<ModDependency>())
				{
					List<string> list6 = modMetaData.UnsatisfiedDependencies();
					if (list6.Any<string>())
					{
						stringBuilder.AppendLine("ModUnsatisfiedDependency".Translate(list6.ToCommaList(true, false)));
					}
				}
				dictionary.Add(modMetaData.PackageId, stringBuilder.ToString().TrimEndNewlines());
			}
			return dictionary;
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x000C04A4 File Offset: 0x000BE6A4
		public static bool ModHasAnyOrderingIssues(ModMetaData mod)
		{
			List<ModMetaData> mods = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			int index = mods.IndexOf(mod);
			return index != -1 && (ModsConfig.FindConflicts(mod.LoadBefore, (ModMetaData beforeMod) => mods.IndexOf(beforeMod) < index).Count > 0 || ModsConfig.FindConflicts(mod.LoadAfter, (ModMetaData afterMod) => mods.IndexOf(afterMod) > index).Count > 0);
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x000C0528 File Offset: 0x000BE728
		private static List<string> FindConflicts(List<string> modsToCheck, Func<ModMetaData, bool> predicate)
		{
			List<string> list = new List<string>();
			foreach (string identifier in modsToCheck)
			{
				ModMetaData activeModWithIdentifier = ModLister.GetActiveModWithIdentifier(identifier, true);
				if (activeModWithIdentifier != null && (predicate == null || predicate(activeModWithIdentifier)))
				{
					list.Add(activeModWithIdentifier.Name);
				}
			}
			return list;
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x000C0598 File Offset: 0x000BE798
		public static void TrySortMods()
		{
			List<ModMetaData> list = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			DirectedAcyclicGraph directedAcyclicGraph = new DirectedAcyclicGraph(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				ModMetaData modMetaData = list[i];
				using (IEnumerator<string> enumerator = modMetaData.LoadBefore.Concat(modMetaData.ForceLoadBefore).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string before = enumerator.Current;
						ModMetaData modMetaData2 = list.FirstOrDefault((ModMetaData m) => m.SamePackageId(before, true));
						if (modMetaData2 != null)
						{
							directedAcyclicGraph.AddEdge(list.IndexOf(modMetaData2), i);
						}
					}
				}
				using (IEnumerator<string> enumerator = modMetaData.LoadAfter.Concat(modMetaData.ForceLoadAfter).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string after = enumerator.Current;
						ModMetaData modMetaData3 = list.FirstOrDefault((ModMetaData m) => m.SamePackageId(after, true));
						if (modMetaData3 != null)
						{
							directedAcyclicGraph.AddEdge(i, list.IndexOf(modMetaData3));
						}
					}
				}
			}
			int num = directedAcyclicGraph.FindCycle();
			if (num != -1)
			{
				Find.WindowStack.Add(new Dialog_MessageBox("ModCyclicDependency".Translate(list[num].Name), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				return;
			}
			ModsConfig.Reorder(directedAcyclicGraph.TopologicalSort());
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x000C0724 File Offset: 0x000BE924
		private static void RecacheActiveMods()
		{
			ModsConfig.activeModsHashSet.Clear();
			foreach (string item in ModsConfig.data.activeMods)
			{
				ModsConfig.activeModsHashSet.Add(item);
			}
			ModsConfig.royaltyActive = ModsConfig.IsActive("ludeon.rimworld.royalty");
			ModsConfig.ideologyActive = ModsConfig.IsActive("ludeon.rimworld.ideology");
			ModsConfig.biotechActive = ModsConfig.IsActive("ludeon.rimworld.biotech");
			ModsConfig.anomalyActive = ModsConfig.IsActive("ludeon.rimworld.anomaly");
			ModsConfig.odysseyActive = ModsConfig.IsActive("ludeon.rimworld.odyssey");
			ModsConfig.activeModsInLoadOrderCachedDirty = true;
		}

		// Token: 0x04001E7B RID: 7803
		private static ModsConfig.ModsConfigData data;

		// Token: 0x04001E7C RID: 7804
		private static bool royaltyActive;

		// Token: 0x04001E7D RID: 7805
		private static bool ideologyActive;

		// Token: 0x04001E7E RID: 7806
		private static bool biotechActive;

		// Token: 0x04001E7F RID: 7807
		private static bool anomalyActive;

		// Token: 0x04001E80 RID: 7808
		private static bool odysseyActive;

		// Token: 0x04001E81 RID: 7809
		private static HashSet<string> activeModsHashSet = new HashSet<string>();

		// Token: 0x04001E82 RID: 7810
		private static List<ModMetaData> activeModsInLoadOrderCached = new List<ModMetaData>();

		// Token: 0x04001E83 RID: 7811
		private static bool activeModsInLoadOrderCachedDirty;

		// Token: 0x04001E84 RID: 7812
		private static List<string> newKnownExpansions = new List<string>();

		// Token: 0x04001E85 RID: 7813
		private static readonly string Core = "ludeon.rimworld";

		// Token: 0x04001E86 RID: 7814
		private static readonly List<string> ExpansionsInReleaseOrder = new List<string>
		{
			"ludeon.rimworld.royalty",
			"ludeon.rimworld.ideology",
			"ludeon.rimworld.biotech",
			"ludeon.rimworld.anomaly",
			"ludeon.rimworld.odyssey"
		};

		// Token: 0x02000586 RID: 1414
		private class ModsConfigData
		{
			// Token: 0x04001E87 RID: 7815
			[LoadAlias("buildNumber")]
			public string version;

			// Token: 0x04001E88 RID: 7816
			public List<string> activeMods = new List<string>();

			// Token: 0x04001E89 RID: 7817
			public List<string> knownExpansions = new List<string>();
		}
	}
}
