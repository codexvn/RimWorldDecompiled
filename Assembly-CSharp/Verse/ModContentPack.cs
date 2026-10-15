using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using KTrie;
using RimWorld;
using UnityEngine;
using UnityStubs;

namespace Verse;

public class ModContentPack
{
	[CompilerGenerated]
	private sealed class _003CLoadDefs_003Ed__57 : IEnumerable<LoadableXmlAsset>, IEnumerable, IEnumerator<LoadableXmlAsset>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private LoadableXmlAsset _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		public ModContentPack _003C_003E4__this;

		private List<LoadableXmlAsset>.Enumerator _003C_003E7__wrap1;

		LoadableXmlAsset IEnumerator<LoadableXmlAsset>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CLoadDefs_003Ed__57(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<LoadableXmlAsset>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ModContentPack modContentPack = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					if (!hotReload && modContentPack.defs.Count != 0)
					{
						Log.ErrorOnce("LoadDefs called with already existing def packages", 39029405);
					}
					DeepProfiler.Start("Load defs via DirectXmlLoader");
					List<LoadableXmlAsset> list = DirectXmlLoader.XmlAssetsInModFolder(modContentPack, "Defs/").ToList();
					DeepProfiler.End();
					DeepProfiler.Start("Parse loaded defs");
					_003C_003E7__wrap1 = list.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					LoadableXmlAsset current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<LoadableXmlAsset>.Enumerator);
				DeepProfiler.End();
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<LoadableXmlAsset> IEnumerable<LoadableXmlAsset>.GetEnumerator()
		{
			_003CLoadDefs_003Ed__57 _003CLoadDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CLoadDefs_003Ed__ = this;
			}
			else
			{
				_003CLoadDefs_003Ed__ = new _003CLoadDefs_003Ed__57(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CLoadDefs_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CLoadDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<LoadableXmlAsset>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CVersionsInRange_003Ed__62<T> : IEnumerable<(Version version, T item)>, IEnumerable, IEnumerator<(Version version, T item)>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private (Version version, T item) _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<(Version version, T item)> candidates;

		public List<(Version version, T item)> _003C_003E3__candidates;

		private Version _003Cfloor_003E5__2;

		private bool _003CatFloor_003E5__3;

		private (Version version, T item) _003ChighestBelow_003E5__4;

		private List<(Version version, T item)>.Enumerator _003C_003E7__wrap4;

		(Version version, T item) IEnumerator<(Version, T)>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CVersionsInRange_003Ed__62(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Cfloor_003E5__2 = null;
			_003ChighestBelow_003E5__4 = default((Version, T));
			_003C_003E7__wrap4 = default(List<(Version, T)>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003Cfloor_003E5__2 = FirstAssetPackVersion;
					_003CatFloor_003E5__3 = false;
					_003ChighestBelow_003E5__4 = default((Version, T));
					_003C_003E7__wrap4 = candidates.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00f1;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00f1;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00f1:
					while (_003C_003E7__wrap4.MoveNext())
					{
						(Version, T) current = _003C_003E7__wrap4.Current;
						if (current.Item1 >= _003Cfloor_003E5__2)
						{
							_003CatFloor_003E5__3 |= current.Item1 == _003Cfloor_003E5__2;
							_003C_003E2__current = current;
							_003C_003E1__state = 1;
							return true;
						}
						if (_003ChighestBelow_003E5__4.version == null || current.Item1 > _003ChighestBelow_003E5__4.version)
						{
							_003ChighestBelow_003E5__4 = current;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap4 = default(List<(Version, T)>.Enumerator);
					if (!_003CatFloor_003E5__3 && _003ChighestBelow_003E5__4.version != null)
					{
						_003C_003E2__current = _003ChighestBelow_003E5__4;
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap4/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<(Version version, T item)> IEnumerable<(Version, T)>.GetEnumerator()
		{
			_003CVersionsInRange_003Ed__62<T> _003CVersionsInRange_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CVersionsInRange_003Ed__ = this;
			}
			else
			{
				_003CVersionsInRange_003Ed__ = new _003CVersionsInRange_003Ed__62<T>(0);
			}
			_003CVersionsInRange_003Ed__.candidates = _003C_003E3__candidates;
			return _003CVersionsInRange_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<(Version, T)>)this).GetEnumerator();
		}
	}

	private DirectoryInfo rootDirInt;

	public int loadOrder;

	private string nameInt;

	private string packageIdInt;

	private string packageIdPlayerFacingInt;

	private bool official;

	private ModContentHolder<AudioClip> audioClips;

	private ModContentHolder<Texture2D> textures;

	private ModContentHolder<string> strings;

	public ModAssetBundlesHandler assetBundles;

	public ModAssemblyHandler assemblies;

	private List<PatchOperation> patches;

	private List<Def> defs = new List<Def>();

	private List<List<string>> allAssetNamesInBundleCached;

	private List<StringTrieSet> allAssetNamesInBundleCachedTrie;

	public List<string> foldersToLoadDescendingOrder;

	private bool loadedAnyPatches;

	public const string LudeonPackageIdAuthor = "ludeon";

	public const string CoreModPackageId = "ludeon.rimworld";

	public const string RoyaltyModPackageId = "ludeon.rimworld.royalty";

	public const string IdeologyModPackageId = "ludeon.rimworld.ideology";

	public const string BiotechModPackageId = "ludeon.rimworld.biotech";

	public const string AnomalyModPackageId = "ludeon.rimworld.anomaly";

	public const string OdysseyModPackageId = "ludeon.rimworld.odyssey";

	public static readonly string[] ProductPackageIDs = new string[6] { "ludeon.rimworld", "ludeon.rimworld.royalty", "ludeon.rimworld.ideology", "ludeon.rimworld.biotech", "ludeon.rimworld.anomaly", "ludeon.rimworld.odyssey" };

	public static readonly string CommonFolderName = "Common";

	public static readonly Version FirstAssetPackVersion = new Version(1, 7);

	public string RootDir => rootDirInt.FullName;

	public string PackageId => packageIdInt;

	public string PackageIdPlayerFacing => packageIdPlayerFacingInt;

	public uint SteamAppId => ModMetaData?.SteamAppId ?? 0;

	public ModMetaData ModMetaData => ModLister.GetModWithIdentifier(PackageId);

	public string FolderName => rootDirInt.Name;

	public string Name => nameInt;

	public int OverwritePriority => (!IsCoreMod) ? 1 : 0;

	public bool IsCoreMod => PackageId == "ludeon.rimworld";

	public bool IsOfficialMod => official;

	public IEnumerable<Def> AllDefs => defs;

	public IEnumerable<PatchOperation> Patches
	{
		get
		{
			if (patches == null)
			{
				LoadPatches();
			}
			return patches;
		}
	}

	public List<string> AllAssetNamesInBundle(int index)
	{
		if (allAssetNamesInBundleCached == null)
		{
			allAssetNamesInBundleCached = new List<List<string>>();
			foreach (AssetBundle loadedAssetBundle in assetBundles.loadedAssetBundles)
			{
				allAssetNamesInBundleCached.Add(new List<string>(loadedAssetBundle.GetAllAssetNames()));
			}
		}
		return allAssetNamesInBundleCached[index];
	}

	public StringTrieSet AllAssetNamesInBundleTrie(int index)
	{
		if (allAssetNamesInBundleCachedTrie == null)
		{
			allAssetNamesInBundleCachedTrie = new List<StringTrieSet>();
			foreach (AssetBundle loadedAssetBundle in assetBundles.loadedAssetBundles)
			{
				StringTrieSet stringTrieSet = new StringTrieSet();
				string[] allAssetNames = loadedAssetBundle.GetAllAssetNames();
				foreach (string item in allAssetNames)
				{
					stringTrieSet.Add(item);
				}
				allAssetNamesInBundleCachedTrie.Add(stringTrieSet);
			}
		}
		return allAssetNamesInBundleCachedTrie[index];
	}

	public ModContentPack(DirectoryInfo directory, string packageId, string packageIdPlayerFacing, int loadOrder, string name, bool official)
	{
		rootDirInt = directory;
		this.loadOrder = loadOrder;
		nameInt = name;
		this.official = official;
		packageIdInt = packageId.ToLower();
		packageIdPlayerFacingInt = packageIdPlayerFacing;
		audioClips = new ModContentHolder<AudioClip>(this);
		textures = new ModContentHolder<Texture2D>(this);
		strings = new ModContentHolder<string>(this);
		assetBundles = new ModAssetBundlesHandler(this);
		assemblies = new ModAssemblyHandler(this);
		InitLoadFolders();
		foreach (string item in foldersToLoadDescendingOrder)
		{
			ContentPacks.MountLoadFolder(item);
		}
	}

	public void ClearDestroy()
	{
		audioClips.ClearDestroy();
		textures.ClearDestroy();
		assetBundles.ClearDestroy();
		allAssetNamesInBundleCached = null;
		allAssetNamesInBundleCachedTrie = null;
	}

	public ModContentHolder<T> GetContentHolder<T>() where T : class
	{
		if (typeof(T) == typeof(Texture2D))
		{
			return (ModContentHolder<T>)(object)textures;
		}
		if (typeof(T) == typeof(AudioClip))
		{
			return (ModContentHolder<T>)(object)audioClips;
		}
		if (typeof(T) == typeof(string))
		{
			return (ModContentHolder<T>)(object)strings;
		}
		Log.Error("Mod lacks manager for asset type " + strings);
		return null;
	}

	private void ReloadContentInt(bool hotReload = false)
	{
		if (hotReload)
		{
			return;
		}
		DeepProfiler.Start("Reload audio clips");
		try
		{
			audioClips.ReloadAll(hotReload);
		}
		finally
		{
			DeepProfiler.End();
		}
		DeepProfiler.Start("Reload textures");
		try
		{
			textures.ReloadAll(hotReload);
		}
		finally
		{
			DeepProfiler.End();
		}
		DeepProfiler.Start("Reload strings");
		try
		{
			strings.ReloadAll(hotReload);
		}
		finally
		{
			DeepProfiler.End();
		}
		DeepProfiler.Start("Reload asset bundles");
		try
		{
			assetBundles.ReloadAll(hotReload);
			allAssetNamesInBundleCached = null;
			allAssetNamesInBundleCachedTrie = null;
		}
		finally
		{
			DeepProfiler.End();
		}
	}

	public void ReloadContent(bool hotReload = false)
	{
		LongEventHandler.ExecuteWhenFinished(delegate
		{
			ReloadContentInt(hotReload);
		});
		if (!hotReload)
		{
			assemblies.ReloadAll();
		}
	}

	[IteratorStateMachine(typeof(_003CLoadDefs_003Ed__57))]
	public IEnumerable<LoadableXmlAsset> LoadDefs(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CLoadDefs_003Ed__57(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__hotReload = hotReload
		};
	}

	private void InitLoadFolders()
	{
		foldersToLoadDescendingOrder = new List<string>();
		ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(PackageId);
		if (modWithIdentifier?.loadFolders != null && modWithIdentifier.loadFolders.DefinedVersions().Count > 0)
		{
			List<LoadFolder> list = modWithIdentifier.LoadFoldersForVersion(VersionControl.CurrentVersionString);
			if (list != null && list.Count > 0)
			{
				AddFolders(list);
				return;
			}
			string text = (from x in modWithIdentifier.loadFolders.DefinedVersions().Where(delegate(string x)
				{
					if (x == "default" || x.NullOrEmpty() || !x.Contains("."))
					{
						return false;
					}
					try
					{
						return VersionControl.VersionFromString(x) <= VersionControl.CurrentVersion;
					}
					catch
					{
						return false;
					}
				})
				orderby x descending
				select x).FirstOrDefault();
			if (text != null)
			{
				List<LoadFolder> list2 = modWithIdentifier.LoadFoldersForVersion(text);
				if (list2 != null)
				{
					AddFolders(list2);
					return;
				}
			}
			List<LoadFolder> list3 = modWithIdentifier.LoadFoldersForVersion("default");
			if (list3 != null)
			{
				AddFolders(list3);
				return;
			}
		}
		if (foldersToLoadDescendingOrder.Count != 0)
		{
			return;
		}
		string text2 = Path.Combine(RootDir, VersionControl.CurrentVersionStringWithoutBuild);
		if (Directory.Exists(text2))
		{
			foldersToLoadDescendingOrder.Add(text2);
		}
		else
		{
			Version version = new Version(0, 0);
			List<Version> list4 = new List<Version>();
			DirectoryInfo[] directories = rootDirInt.GetDirectories();
			for (int i = 0; i < directories.Length; i++)
			{
				if (VersionControl.TryParseVersionString(directories[i].Name, out var version2))
				{
					list4.Add(version2);
				}
			}
			list4.Sort();
			foreach (Version item in list4)
			{
				if ((item > version || version > VersionControl.CurrentVersion) && (item <= VersionControl.CurrentVersion || version.Major == 0))
				{
					version = item;
				}
			}
			if (version.Major > 0)
			{
				foldersToLoadDescendingOrder.Add(Path.Combine(RootDir, version.ToString()));
			}
		}
		string text3 = Path.Combine(RootDir, CommonFolderName);
		if (Directory.Exists(text3))
		{
			foldersToLoadDescendingOrder.Add(text3);
		}
		foldersToLoadDescendingOrder.Add(RootDir);
		void AddFolders(List<LoadFolder> folders)
		{
			for (int num = folders.Count - 1; num >= 0; num--)
			{
				if (folders[num].ShouldLoad)
				{
					foldersToLoadDescendingOrder.Add(Path.Combine(RootDir, folders[num].folderName));
				}
			}
		}
	}

	public static List<string> AllLoadFolders(ModMetaData mod)
	{
		return AllLoadFolders(mod.RootDir.FullName, mod.loadFolders);
	}

	public static List<string> AllLoadFolders(string rootDir, ModLoadFolders loadFolders)
	{
		List<string> folders = new List<string>();
		List<string> list = loadFolders?.DefinedVersions();
		if (list != null && list.Count > 0)
		{
			List<(Version, string)> list2 = new List<(Version, string)>();
			foreach (string item2 in list)
			{
				if (VersionControl.TryParseVersionString(item2, out var version))
				{
					list2.Add((version, item2));
				}
			}
			foreach (var item3 in VersionsInRange(list2))
			{
				string item = item3.item;
				foreach (LoadFolder item4 in loadFolders.FoldersForVersion(item))
				{
					Add(Path.Combine(rootDir, item4.folderName));
				}
			}
			List<LoadFolder> list3 = loadFolders.FoldersForVersion("default");
			if (list3 != null)
			{
				foreach (LoadFolder item5 in list3)
				{
					Add(Path.Combine(rootDir, item5.folderName));
				}
			}
			return folders;
		}
		List<(Version, string)> list4 = new List<(Version, string)>();
		DirectoryInfo[] directories = new DirectoryInfo(rootDir).GetDirectories();
		foreach (DirectoryInfo directoryInfo in directories)
		{
			if (VersionControl.TryParseVersionString(directoryInfo.Name, out var version2))
			{
				list4.Add((version2, directoryInfo.FullName));
			}
		}
		foreach (var item6 in VersionsInRange(list4))
		{
			Add(item6.item);
		}
		string text = Path.Combine(rootDir, CommonFolderName);
		if (Directory.Exists(text))
		{
			Add(text);
		}
		Add(rootDir);
		return folders;
		void Add(string folder)
		{
			folder = Path.TrimEndingDirectorySeparator(Path.GetFullPath(folder));
			if (!folders.Contains<string>(folder, StringComparer.OrdinalIgnoreCase))
			{
				folders.Add(folder);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CVersionsInRange_003Ed__62<>))]
	private static IEnumerable<(Version version, T item)> VersionsInRange<T>(List<(Version version, T item)> candidates)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CVersionsInRange_003Ed__62<T>(-2)
		{
			_003C_003E3__candidates = candidates
		};
	}

	private void LoadPatches()
	{
		DeepProfiler.Start("Loading all patches");
		patches = new List<PatchOperation>();
		loadedAnyPatches = false;
		List<LoadableXmlAsset> list = DirectXmlLoader.XmlAssetsInModFolder(this, "Patches/").ToList();
		for (int i = 0; i < list.Count; i++)
		{
			XmlElement documentElement = list[i].xmlDoc.DocumentElement;
			if (documentElement.Name != "Patch")
			{
				Log.Error($"Unexpected document element in patch XML; got {documentElement.Name}, expected 'Patch'");
				continue;
			}
			foreach (XmlNode childNode in documentElement.ChildNodes)
			{
				if (childNode.NodeType == XmlNodeType.Element)
				{
					if (childNode.Name != "Operation")
					{
						Log.Error($"Unexpected element in patch XML; got {childNode.Name}, expected 'Operation'");
						continue;
					}
					PatchOperation patchOperation = DirectXmlToObject.ObjectFromXml<PatchOperation>(childNode, doPostLoad: false);
					patchOperation.sourceFile = list[i].FullFilePath;
					patches.Add(patchOperation);
					loadedAnyPatches = true;
				}
			}
		}
		DeepProfiler.End();
	}

	public static Dictionary<string, FileInfo> GetAllFilesForMod(ModContentPack mod, string contentPath, Func<string, bool> validateExtension = null, List<string> foldersToLoadDebug = null)
	{
		List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
		Dictionary<string, FileInfo> dictionary = new Dictionary<string, FileInfo>();
		for (int i = 0; i < list.Count; i++)
		{
			string text = list[i];
			string text2 = Path.Combine(text, contentPath);
			if (!ContentPacks.DirectoryExists(text2))
			{
				continue;
			}
			List<string> list2 = ContentPacks.EnumerateFiles(text2, recursive: true).ToList();
			list2.Sort(StringComparer.Ordinal);
			for (int j = 0; j < list2.Count; j++)
			{
				FileInfo fileInfo = new FileInfo(list2[j]);
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
		return dictionary;
	}

	public static List<Tuple<string, FileInfo>> GetAllFilesForModPreserveOrder(ModContentPack mod, string contentPath, Func<string, bool> validateExtension = null, List<string> foldersToLoadDebug = null)
	{
		List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
		List<Tuple<string, FileInfo>> list2 = new List<Tuple<string, FileInfo>>();
		for (int num = list.Count - 1; num >= 0; num--)
		{
			string text = list[num];
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, contentPath));
			if (directoryInfo.Exists)
			{
				FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
				Array.Sort(files, (FileInfo a, FileInfo b) => a.Name.CompareTo(b.Name));
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
		for (int num2 = list2.Count - 1; num2 >= 0; num2--)
		{
			Tuple<string, FileInfo> tuple = list2[num2];
			if (!hashSet.Contains(tuple.Item1))
			{
				hashSet.Add(tuple.Item1);
			}
			else
			{
				list2.RemoveAt(num2);
			}
		}
		return list2;
	}

	public bool AnyContentLoaded()
	{
		if (AnyNonTranslationContentLoaded())
		{
			return true;
		}
		if (AnyTranslationsLoaded())
		{
			return true;
		}
		return false;
	}

	public bool AnyNonTranslationContentLoaded()
	{
		if (textures.contentList != null && textures.contentList.Count != 0)
		{
			return true;
		}
		if (audioClips.contentList != null && audioClips.contentList.Count != 0)
		{
			return true;
		}
		if (strings.contentList != null && strings.contentList.Count != 0)
		{
			return true;
		}
		if (!assemblies.loadedAssemblies.NullOrEmpty())
		{
			return true;
		}
		if (!assetBundles.loadedAssetBundles.NullOrEmpty())
		{
			return true;
		}
		if (loadedAnyPatches)
		{
			return true;
		}
		if (AllDefs.Any())
		{
			return true;
		}
		return false;
	}

	public bool AnyTranslationsLoaded()
	{
		foreach (string item in foldersToLoadDescendingOrder)
		{
			string path = Path.Combine(item, "Languages");
			if (Directory.Exists(path) && Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).Any())
			{
				return true;
			}
		}
		return false;
	}

	public void ClearPatchesCache()
	{
		patches = null;
	}

	public void ClearDefs()
	{
		defs.Clear();
	}

	public void AddDef(Def def, string source = "Unknown")
	{
		def.modContentPack = this;
		def.fileName = source;
		defs.Add(def);
	}

	public override string ToString()
	{
		return PackageIdPlayerFacing;
	}
}
