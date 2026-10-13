using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using KTrie;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000570 RID: 1392
	public class ModContentPack
	{
		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x000BB9EF File Offset: 0x000B9BEF
		public string RootDir
		{
			get
			{
				return this.rootDirInt.FullName;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x000BB9FC File Offset: 0x000B9BFC
		public string PackageId
		{
			get
			{
				return this.packageIdInt;
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x0600241B RID: 9243 RVA: 0x000BBA04 File Offset: 0x000B9C04
		public string PackageIdPlayerFacing
		{
			get
			{
				return this.packageIdPlayerFacingInt;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x0600241C RID: 9244 RVA: 0x000BBA0C File Offset: 0x000B9C0C
		public uint SteamAppId
		{
			get
			{
				ModMetaData modMetaData = this.ModMetaData;
				if (modMetaData == null)
				{
					return 0U;
				}
				return modMetaData.SteamAppId;
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x0600241D RID: 9245 RVA: 0x000BBA1F File Offset: 0x000B9C1F
		public ModMetaData ModMetaData
		{
			get
			{
				return ModLister.GetModWithIdentifier(this.PackageId, false);
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x000BBA2D File Offset: 0x000B9C2D
		public string FolderName
		{
			get
			{
				return this.rootDirInt.Name;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x0600241F RID: 9247 RVA: 0x000BBA3A File Offset: 0x000B9C3A
		public string Name
		{
			get
			{
				return this.nameInt;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x000BBA42 File Offset: 0x000B9C42
		public int OverwritePriority
		{
			get
			{
				if (!this.IsCoreMod)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06002421 RID: 9249 RVA: 0x000BBA4F File Offset: 0x000B9C4F
		public bool IsCoreMod
		{
			get
			{
				return this.PackageId == "ludeon.rimworld";
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06002422 RID: 9250 RVA: 0x000BBA61 File Offset: 0x000B9C61
		public bool IsOfficialMod
		{
			get
			{
				return this.official;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06002423 RID: 9251 RVA: 0x000BBA69 File Offset: 0x000B9C69
		public IEnumerable<Def> AllDefs
		{
			get
			{
				return this.defs;
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x000BBA71 File Offset: 0x000B9C71
		public IEnumerable<PatchOperation> Patches
		{
			get
			{
				if (this.patches == null)
				{
					this.LoadPatches();
				}
				return this.patches;
			}
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x000BBA88 File Offset: 0x000B9C88
		public List<string> AllAssetNamesInBundle(int index)
		{
			if (this.allAssetNamesInBundleCached == null)
			{
				this.allAssetNamesInBundleCached = new List<List<string>>();
				foreach (AssetBundle assetBundle in this.assetBundles.loadedAssetBundles)
				{
					this.allAssetNamesInBundleCached.Add(new List<string>(assetBundle.GetAllAssetNames()));
				}
			}
			return this.allAssetNamesInBundleCached[index];
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x000BBB10 File Offset: 0x000B9D10
		public StringTrieSet AllAssetNamesInBundleTrie(int index)
		{
			if (this.allAssetNamesInBundleCachedTrie == null)
			{
				this.allAssetNamesInBundleCachedTrie = new List<StringTrieSet>();
				foreach (AssetBundle assetBundle in this.assetBundles.loadedAssetBundles)
				{
					StringTrieSet stringTrieSet = new StringTrieSet();
					foreach (string item in assetBundle.GetAllAssetNames())
					{
						stringTrieSet.Add(item);
					}
					this.allAssetNamesInBundleCachedTrie.Add(stringTrieSet);
				}
			}
			return this.allAssetNamesInBundleCachedTrie[index];
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x000BBBB4 File Offset: 0x000B9DB4
		public ModContentPack(DirectoryInfo directory, string packageId, string packageIdPlayerFacing, int loadOrder, string name, bool official)
		{
			this.rootDirInt = directory;
			this.loadOrder = loadOrder;
			this.nameInt = name;
			this.official = official;
			this.packageIdInt = packageId.ToLower();
			this.packageIdPlayerFacingInt = packageIdPlayerFacing;
			this.audioClips = new ModContentHolder<AudioClip>(this);
			this.textures = new ModContentHolder<Texture2D>(this);
			this.strings = new ModContentHolder<string>(this);
			this.assetBundles = new ModAssetBundlesHandler(this);
			this.assemblies = new ModAssemblyHandler(this);
			this.InitLoadFolders();
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x000BBC46 File Offset: 0x000B9E46
		public void ClearDestroy()
		{
			this.audioClips.ClearDestroy();
			this.textures.ClearDestroy();
			this.assetBundles.ClearDestroy();
			this.allAssetNamesInBundleCached = null;
			this.allAssetNamesInBundleCachedTrie = null;
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x000BBC78 File Offset: 0x000B9E78
		public ModContentHolder<T> GetContentHolder<T>() where T : class
		{
			if (typeof(T) == typeof(Texture2D))
			{
				return (ModContentHolder<T>)this.textures;
			}
			if (typeof(T) == typeof(AudioClip))
			{
				return (ModContentHolder<T>)this.audioClips;
			}
			if (typeof(T) == typeof(string))
			{
				return (ModContentHolder<T>)this.strings;
			}
			string str = "Mod lacks manager for asset type ";
			ModContentHolder<string> modContentHolder = this.strings;
			Log.Error(str + ((modContentHolder != null) ? modContentHolder.ToString() : null));
			return null;
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x000BBD1C File Offset: 0x000B9F1C
		private void ReloadContentInt(bool hotReload = false)
		{
			if (hotReload)
			{
				return;
			}
			DeepProfiler.Start("Reload audio clips");
			try
			{
				this.audioClips.ReloadAll(hotReload);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Reload textures");
			try
			{
				this.textures.ReloadAll(hotReload);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Reload strings");
			try
			{
				this.strings.ReloadAll(hotReload);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("Reload asset bundles");
			try
			{
				this.assetBundles.ReloadAll(hotReload);
				this.allAssetNamesInBundleCached = null;
				this.allAssetNamesInBundleCachedTrie = null;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x000BBDE8 File Offset: 0x000B9FE8
		public void ReloadContent(bool hotReload = false)
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.ReloadContentInt(hotReload);
			});
			if (!hotReload)
			{
				this.assemblies.ReloadAll();
			}
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x000BBE20 File Offset: 0x000BA020
		public IEnumerable<LoadableXmlAsset> LoadDefs(bool hotReload = false)
		{
			if (!hotReload && this.defs.Count != 0)
			{
				Log.ErrorOnce("LoadDefs called with already existing def packages", 39029405);
			}
			DeepProfiler.Start("Load defs via DirectXmlLoader");
			List<LoadableXmlAsset> list = DirectXmlLoader.XmlAssetsInModFolder(this, "Defs/", null).ToList<LoadableXmlAsset>();
			DeepProfiler.End();
			DeepProfiler.Start("Parse loaded defs");
			foreach (LoadableXmlAsset loadableXmlAsset in list)
			{
				yield return loadableXmlAsset;
			}
			List<LoadableXmlAsset>.Enumerator enumerator = default(List<LoadableXmlAsset>.Enumerator);
			DeepProfiler.End();
			yield break;
			yield break;
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x000BBE38 File Offset: 0x000BA038
		private void InitLoadFolders()
		{
			this.foldersToLoadDescendingOrder = new List<string>();
			ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.PackageId, false);
			if (((modWithIdentifier != null) ? modWithIdentifier.loadFolders : null) != null && modWithIdentifier.loadFolders.DefinedVersions().Count > 0)
			{
				List<LoadFolder> list = modWithIdentifier.LoadFoldersForVersion(VersionControl.CurrentVersionString);
				if (list != null && list.Count > 0)
				{
					this.<InitLoadFolders>g__AddFolders|58_0(list);
					return;
				}
				string text = (from x in modWithIdentifier.loadFolders.DefinedVersions().Where(delegate(string x)
				{
					if (x == "default" || x.NullOrEmpty() || !x.Contains("."))
					{
						return false;
					}
					bool result;
					try
					{
						result = (VersionControl.VersionFromString(x) <= VersionControl.CurrentVersion);
					}
					catch
					{
						result = false;
					}
					return result;
				})
				orderby x descending
				select x).FirstOrDefault<string>();
				if (text != null)
				{
					List<LoadFolder> list2 = modWithIdentifier.LoadFoldersForVersion(text);
					if (list2 != null)
					{
						this.<InitLoadFolders>g__AddFolders|58_0(list2);
						return;
					}
				}
				List<LoadFolder> list3 = modWithIdentifier.LoadFoldersForVersion("default");
				if (list3 != null)
				{
					this.<InitLoadFolders>g__AddFolders|58_0(list3);
					return;
				}
			}
			if (this.foldersToLoadDescendingOrder.Count == 0)
			{
				string text2 = Path.Combine(this.RootDir, VersionControl.CurrentVersionStringWithoutBuild);
				if (Directory.Exists(text2))
				{
					this.foldersToLoadDescendingOrder.Add(text2);
				}
				else
				{
					Version version = new Version(0, 0);
					List<Version> list4 = new List<Version>();
					DirectoryInfo[] directories = this.rootDirInt.GetDirectories();
					for (int i = 0; i < directories.Length; i++)
					{
						Version item;
						if (VersionControl.TryParseVersionString(directories[i].Name, out item))
						{
							list4.Add(item);
						}
					}
					list4.Sort();
					foreach (Version version2 in list4)
					{
						if ((version2 > version || version > VersionControl.CurrentVersion) && (version2 <= VersionControl.CurrentVersion || version.Major == 0))
						{
							version = version2;
						}
					}
					if (version.Major > 0)
					{
						this.foldersToLoadDescendingOrder.Add(Path.Combine(this.RootDir, version.ToString()));
					}
				}
				string text3 = Path.Combine(this.RootDir, ModContentPack.CommonFolderName);
				if (Directory.Exists(text3))
				{
					this.foldersToLoadDescendingOrder.Add(text3);
				}
				this.foldersToLoadDescendingOrder.Add(this.RootDir);
			}
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x000BC090 File Offset: 0x000BA290
		private void LoadPatches()
		{
			DeepProfiler.Start("Loading all patches");
			this.patches = new List<PatchOperation>();
			this.loadedAnyPatches = false;
			List<LoadableXmlAsset> list = DirectXmlLoader.XmlAssetsInModFolder(this, "Patches/", null).ToList<LoadableXmlAsset>();
			for (int i = 0; i < list.Count; i++)
			{
				XmlElement documentElement = list[i].xmlDoc.DocumentElement;
				if (documentElement.Name != "Patch")
				{
					Log.Error(string.Format("Unexpected document element in patch XML; got {0}, expected 'Patch'", documentElement.Name));
				}
				else
				{
					foreach (object obj in documentElement.ChildNodes)
					{
						XmlNode xmlNode = (XmlNode)obj;
						if (xmlNode.NodeType == XmlNodeType.Element)
						{
							if (xmlNode.Name != "Operation")
							{
								Log.Error(string.Format("Unexpected element in patch XML; got {0}, expected 'Operation'", xmlNode.Name));
							}
							else
							{
								PatchOperation patchOperation = DirectXmlToObject.ObjectFromXml<PatchOperation>(xmlNode, false);
								patchOperation.sourceFile = list[i].FullFilePath;
								this.patches.Add(patchOperation);
								this.loadedAnyPatches = true;
							}
						}
					}
				}
			}
			DeepProfiler.End();
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x000BC1D4 File Offset: 0x000BA3D4
		public static Dictionary<string, FileInfo> GetAllFilesForMod(ModContentPack mod, string contentPath, Func<string, bool> validateExtension = null, List<string> foldersToLoadDebug = null)
		{
			List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
			Dictionary<string, FileInfo> dictionary = new Dictionary<string, FileInfo>();
			for (int i = 0; i < list.Count; i++)
			{
				string text = list[i];
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, contentPath));
				if (directoryInfo.Exists)
				{
					foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
					{
						if (validateExtension == null || validateExtension(fileInfo.Extension))
						{
							string key = fileInfo.FullName.Substring(text.Length + 1);
							if (!dictionary.ContainsKey(key))
							{
								dictionary.Add(key, fileInfo);
							}
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x000BC290 File Offset: 0x000BA490
		public static List<Tuple<string, FileInfo>> GetAllFilesForModPreserveOrder(ModContentPack mod, string contentPath, Func<string, bool> validateExtension = null, List<string> foldersToLoadDebug = null)
		{
			List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
			List<Tuple<string, FileInfo>> list2 = new List<Tuple<string, FileInfo>>();
			for (int i = list.Count - 1; i >= 0; i--)
			{
				string text = list[i];
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, contentPath));
				if (directoryInfo.Exists)
				{
					FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
					Array.Sort<FileInfo>(files, (FileInfo a, FileInfo b) => a.Name.CompareTo(b.Name));
					foreach (FileInfo fileInfo in files)
					{
						if (validateExtension == null || validateExtension(fileInfo.Extension))
						{
							string item = fileInfo.FullName.Substring(text.Length + 1);
							list2.Add(new Tuple<string, FileInfo>(item, fileInfo));
						}
					}
				}
			}
			HashSet<string> hashSet = new HashSet<string>();
			for (int k = list2.Count - 1; k >= 0; k--)
			{
				Tuple<string, FileInfo> tuple = list2[k];
				if (!hashSet.Contains(tuple.Item1))
				{
					hashSet.Add(tuple.Item1);
				}
				else
				{
					list2.RemoveAt(k);
				}
			}
			return list2;
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x000BC3C2 File Offset: 0x000BA5C2
		public bool AnyContentLoaded()
		{
			return this.AnyNonTranslationContentLoaded() || this.AnyTranslationsLoaded();
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x000BC3DC File Offset: 0x000BA5DC
		public bool AnyNonTranslationContentLoaded()
		{
			return (this.textures.contentList != null && this.textures.contentList.Count != 0) || (this.audioClips.contentList != null && this.audioClips.contentList.Count != 0) || (this.strings.contentList != null && this.strings.contentList.Count != 0) || !this.assemblies.loadedAssemblies.NullOrEmpty<Assembly>() || !this.assetBundles.loadedAssetBundles.NullOrEmpty<AssetBundle>() || this.loadedAnyPatches || this.AllDefs.Any<Def>();
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x000BC490 File Offset: 0x000BA690
		public bool AnyTranslationsLoaded()
		{
			foreach (string path in this.foldersToLoadDescendingOrder)
			{
				string path2 = Path.Combine(path, "Languages");
				if (Directory.Exists(path2) && Directory.EnumerateFiles(path2, "*", SearchOption.AllDirectories).Any<string>())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x000BC508 File Offset: 0x000BA708
		public void ClearPatchesCache()
		{
			this.patches = null;
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x000BC511 File Offset: 0x000BA711
		public void ClearDefs()
		{
			this.defs.Clear();
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x000BC51E File Offset: 0x000BA71E
		public void AddDef(Def def, string source = "Unknown")
		{
			def.modContentPack = this;
			def.fileName = source;
			this.defs.Add(def);
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x000BC53A File Offset: 0x000BA73A
		public override string ToString()
		{
			return this.PackageIdPlayerFacing;
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x000BC598 File Offset: 0x000BA798
		[CompilerGenerated]
		private void <InitLoadFolders>g__AddFolders|58_0(List<LoadFolder> folders)
		{
			for (int i = folders.Count - 1; i >= 0; i--)
			{
				if (folders[i].ShouldLoad)
				{
					this.foldersToLoadDescendingOrder.Add(Path.Combine(this.RootDir, folders[i].folderName));
				}
			}
		}

		// Token: 0x04001DE2 RID: 7650
		private DirectoryInfo rootDirInt;

		// Token: 0x04001DE3 RID: 7651
		public int loadOrder;

		// Token: 0x04001DE4 RID: 7652
		private string nameInt;

		// Token: 0x04001DE5 RID: 7653
		private string packageIdInt;

		// Token: 0x04001DE6 RID: 7654
		private string packageIdPlayerFacingInt;

		// Token: 0x04001DE7 RID: 7655
		private bool official;

		// Token: 0x04001DE8 RID: 7656
		private ModContentHolder<AudioClip> audioClips;

		// Token: 0x04001DE9 RID: 7657
		private ModContentHolder<Texture2D> textures;

		// Token: 0x04001DEA RID: 7658
		private ModContentHolder<string> strings;

		// Token: 0x04001DEB RID: 7659
		public ModAssetBundlesHandler assetBundles;

		// Token: 0x04001DEC RID: 7660
		public ModAssemblyHandler assemblies;

		// Token: 0x04001DED RID: 7661
		private List<PatchOperation> patches;

		// Token: 0x04001DEE RID: 7662
		private List<Def> defs = new List<Def>();

		// Token: 0x04001DEF RID: 7663
		private List<List<string>> allAssetNamesInBundleCached;

		// Token: 0x04001DF0 RID: 7664
		private List<StringTrieSet> allAssetNamesInBundleCachedTrie;

		// Token: 0x04001DF1 RID: 7665
		public List<string> foldersToLoadDescendingOrder;

		// Token: 0x04001DF2 RID: 7666
		private bool loadedAnyPatches;

		// Token: 0x04001DF3 RID: 7667
		public const string LudeonPackageIdAuthor = "ludeon";

		// Token: 0x04001DF4 RID: 7668
		public const string CoreModPackageId = "ludeon.rimworld";

		// Token: 0x04001DF5 RID: 7669
		public const string RoyaltyModPackageId = "ludeon.rimworld.royalty";

		// Token: 0x04001DF6 RID: 7670
		public const string IdeologyModPackageId = "ludeon.rimworld.ideology";

		// Token: 0x04001DF7 RID: 7671
		public const string BiotechModPackageId = "ludeon.rimworld.biotech";

		// Token: 0x04001DF8 RID: 7672
		public const string AnomalyModPackageId = "ludeon.rimworld.anomaly";

		// Token: 0x04001DF9 RID: 7673
		public const string OdysseyModPackageId = "ludeon.rimworld.odyssey";

		// Token: 0x04001DFA RID: 7674
		public static readonly string[] ProductPackageIDs = new string[]
		{
			"ludeon.rimworld",
			"ludeon.rimworld.royalty",
			"ludeon.rimworld.ideology",
			"ludeon.rimworld.biotech",
			"ludeon.rimworld.anomaly",
			"ludeon.rimworld.odyssey"
		};

		// Token: 0x04001DFB RID: 7675
		public static readonly string CommonFolderName = "Common";
	}
}
