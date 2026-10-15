using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using RimWorld;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse;

public class ModMetaData : WorkshopUploadable
{
	private class ModMetaDataInternal
	{
		public string packageId = "";

		public string name = "";

		public string shortName = "";

		public string author = "Anonymous";

		public List<string> authors;

		public string modIconPath = "";

		public string modVersion = "";

		public string url = "";

		public string description = "No description provided.";

		public uint steamAppId;

		public List<string> supportedVersions;

		[Unsaved(true)]
		private string targetVersion;

		public List<ModDependency> modDependencies = new List<ModDependency>();

		public List<string> loadBefore = new List<string>();

		public List<string> loadAfter = new List<string>();

		public List<string> incompatibleWith = new List<string>();

		public List<string> forceLoadBefore = new List<string>();

		public List<string> forceLoadAfter = new List<string>();

		private VersionedData<string> descriptionsByVersion;

		private VersionedData<List<ModDependency>> modDependenciesByVersion;

		private VersionedData<List<string>> loadBeforeByVersion;

		private VersionedData<List<string>> loadAfterByVersion;

		private VersionedData<List<string>> incompatibleWithByVersion;

		public static readonly Regex PackageIdFormatRegex = new Regex("(?=.{1,60}$)^(?!\\.)(?=.*?[.])(?!.*([.])\\1+)[a-zA-Z0-9.]{1,}[a-zA-Z0-9]{1}$");

		public List<System.Version> SupportedVersions { get; private set; }

		private bool TryParseVersion(string str, bool logIssues = true)
		{
			if (!VersionControl.TryParseVersionString(str, out var version))
			{
				if (logIssues)
				{
					Log.Error("Unable to parse version string on mod " + name + " from " + author + " \"" + str + "\"");
				}
				return false;
			}
			SupportedVersions.Add(version);
			if (!VersionControl.IsWellFormattedVersionString(str))
			{
				if (logIssues)
				{
					Log.Warning("Malformed (correct format is Major.Minor) version string on mod " + name + " from " + author + " \"" + str + "\" - parsed as \"" + version.Major + "." + version.Minor + "\"");
				}
				return false;
			}
			return true;
		}

		public bool TryParseSupportedVersions(bool logIssues = true)
		{
			if (targetVersion != null && logIssues)
			{
				Log.Warning("Mod " + name + ": targetVersion field is obsolete, use supportedVersions instead.");
			}
			bool flag = false;
			SupportedVersions = new List<System.Version>();
			if (packageId.ToLower() == "ludeon.rimworld")
			{
				SupportedVersions.Add(VersionControl.CurrentVersion);
			}
			else if (supportedVersions == null)
			{
				if (logIssues)
				{
					Log.Warning("Mod " + name + " is missing supported versions list in About.xml! (example: <supportedVersions><li>1.0</li></supportedVersions>)");
				}
				flag = true;
			}
			else if (supportedVersions.Count == 0)
			{
				if (logIssues)
				{
					Log.Error("Mod " + name + ": <supportedVersions> in mod About.xml must specify at least one version.");
				}
				flag = true;
			}
			else
			{
				for (int i = 0; i < supportedVersions.Count; i++)
				{
					flag |= !TryParseVersion(supportedVersions[i], logIssues);
				}
			}
			SupportedVersions = (from v in SupportedVersions
				orderby (!VersionControl.IsCompatible(v)) ? 100 : (-100), v.Major descending, v.Minor descending
				select v).Distinct().ToList();
			return !flag;
		}

		public bool TryParsePackageId(bool isOfficial, bool logIssues = true)
		{
			bool flag = false;
			if (packageId.NullOrEmpty())
			{
				string text = "none";
				if (!description.NullOrEmpty())
				{
					text = GenText.StableStringHash(description).ToString().Replace("-", "");
					text = text.Substring(0, Math.Min(3, text.Length));
				}
				packageId = ConvertToASCII(author + text) + "." + ConvertToASCII(name);
				if (logIssues)
				{
					Log.Warning("Mod " + name + " is missing packageId in About.xml! (example: <packageId>AuthorName.ModName.Specific</packageId>)");
				}
				flag = true;
			}
			if (!PackageIdFormatRegex.IsMatch(packageId))
			{
				if (logIssues)
				{
					Log.Warning("Mod " + name + " <packageId> (" + packageId + ") is not in valid format.");
				}
				flag = true;
			}
			if (!isOfficial && packageId.ToLower().Contains("ludeon"))
			{
				if (logIssues)
				{
					Log.Warning("Mod " + name + " <packageId> contains word \"Ludeon\", which is reserved for official content.");
				}
				flag = true;
			}
			return !flag;
		}

		private string ConvertToASCII(string part)
		{
			StringBuilder stringBuilder = new StringBuilder("");
			for (int i = 0; i < part.Length; i++)
			{
				char c = part[i];
				if (!char.IsLetterOrDigit(c) || c >= '\u0080')
				{
					c = (char)(c % 25 + 65);
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		public void ValidateDependencies(bool logIssues = true)
		{
			for (int num = modDependencies.Count - 1; num >= 0; num--)
			{
				bool flag = false;
				ModDependency modDependency = modDependencies[num];
				if (modDependency.packageId.NullOrEmpty())
				{
					if (logIssues)
					{
						Log.Warning("Mod " + name + " has a dependency with no <packageId> specified.");
					}
					flag = true;
				}
				else if (!PackageIdFormatRegex.IsMatch(modDependency.packageId))
				{
					if (logIssues)
					{
						Log.Warning("Mod " + name + " has a dependency with invalid <packageId>: " + modDependency.packageId);
					}
					flag = true;
				}
				if (modDependency.displayName.NullOrEmpty())
				{
					if (logIssues)
					{
						Log.Warning("Mod " + name + " has a dependency (" + modDependency.packageId + ") with empty display name.");
					}
					flag = true;
				}
				if (modDependency.downloadUrl.NullOrEmpty() && modDependency.steamWorkshopUrl.NullOrEmpty() && !modDependency.packageId.ToLower().Contains("ludeon"))
				{
					if (logIssues)
					{
						Log.Warning("Mod " + name + " dependency (" + modDependency.packageId + ") needs to have <downloadUrl> and/or <steamWorkshopUrl> specified.");
					}
					flag = true;
				}
				if (flag)
				{
					modDependencies.Remove(modDependency);
				}
			}
		}

		public void InitVersionedData()
		{
			string currentVersionStringWithoutBuild = VersionControl.CurrentVersionStringWithoutBuild;
			string text = descriptionsByVersion?.GetItemForVersion(currentVersionStringWithoutBuild);
			if (text != null)
			{
				description = text;
			}
			List<ModDependency> list = modDependenciesByVersion?.GetItemForVersion(currentVersionStringWithoutBuild);
			if (list != null)
			{
				modDependencies = list;
			}
			List<string> list2 = loadBeforeByVersion?.GetItemForVersion(currentVersionStringWithoutBuild);
			if (list2 != null)
			{
				loadBefore = list2;
			}
			List<string> list3 = loadAfterByVersion?.GetItemForVersion(currentVersionStringWithoutBuild);
			if (list3 != null)
			{
				loadAfter = list3;
			}
			List<string> list4 = incompatibleWithByVersion?.GetItemForVersion(currentVersionStringWithoutBuild);
			if (list4 != null)
			{
				incompatibleWith = list4;
			}
		}
	}

	private class VersionedData<T> where T : class
	{
		private Dictionary<string, T> itemForVersion = new Dictionary<string, T>();

		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			foreach (XmlNode childNode in xmlRoot.ChildNodes)
			{
				if (!(childNode is XmlComment))
				{
					string text = childNode.Name.ToLower();
					if (text.StartsWith("v"))
					{
						text = text.Substring(1);
					}
					if (!itemForVersion.ContainsKey(text))
					{
						itemForVersion[text] = ((typeof(T) == typeof(string)) ? ((T)(object)childNode.FirstChild.Value) : DirectXmlToObject.ObjectFromXml<T>(childNode, doPostLoad: false));
					}
					else
					{
						Log.Warning("More than one value for a same version of " + typeof(T).Name + " named " + xmlRoot.Name);
					}
				}
			}
		}

		public T GetItemForVersion(string ver)
		{
			if (itemForVersion.ContainsKey(ver))
			{
				return itemForVersion[ver];
			}
			return null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetRequirements_003Ed__122 : IEnumerable<ModRequirement>, IEnumerable, IEnumerator<ModRequirement>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ModRequirement _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ModMetaData _003C_003E4__this;

		private int _003Ci_003E5__2;

		ModRequirement IEnumerator<ModRequirement>.Current
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
		public _003CGetRequirements_003Ed__122(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			ModMetaData modMetaData = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				goto IL_0069;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				goto IL_0069;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_00d8;
				}
				IL_0069:
				if (_003Ci_003E5__2 < modMetaData.Dependencies.Count)
				{
					_003C_003E2__current = modMetaData.Dependencies[_003Ci_003E5__2];
					_003C_003E1__state = 1;
					return true;
				}
				_003Ci_003E5__2 = 0;
				goto IL_00e8;
				IL_00e8:
				if (_003Ci_003E5__2 < modMetaData.meta.incompatibleWith.Count)
				{
					ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(modMetaData.meta.incompatibleWith[_003Ci_003E5__2]);
					if (modWithIdentifier != null)
					{
						_003C_003E2__current = new ModIncompatibility
						{
							packageId = modWithIdentifier.PackageIdPlayerFacing,
							displayName = modWithIdentifier.Name
						};
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00d8;
				}
				return false;
				IL_00d8:
				_003Ci_003E5__2++;
				goto IL_00e8;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ModRequirement> IEnumerable<ModRequirement>.GetEnumerator()
		{
			_003CGetRequirements_003Ed__122 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetRequirements_003Ed__122(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ModRequirement>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_Authors_003Ed__51 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ModMetaData _003C_003E4__this;

		private List<string>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<string> _003C_003E7__wrap2;

		string IEnumerator<string>.Current
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
		public _003Cget_Authors_003Ed__51(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<string>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ModMetaData modMetaData = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!modMetaData.meta.authors.NullOrEmpty())
					{
						_003C_003E7__wrap1 = modMetaData.meta.authors.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0089;
					}
					if (string.IsNullOrWhiteSpace(modMetaData.meta.author))
					{
						break;
					}
					_003C_003E7__wrap2 = modMetaData.meta.author.Split(AndToken, StringSplitOptions.RemoveEmptyEntries).SelectMany((string x) => x.Split(',')).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_013b;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0089;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_013b;
					}
					IL_0089:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<string>.Enumerator);
					return false;
					IL_013b:
					if (_003C_003E7__wrap2.MoveNext())
					{
						string current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003Cget_Authors_003Ed__51 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Authors_003Ed__51(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	private DirectoryInfo rootDirInt;

	private ContentSource source;

	private Texture2D previewImage;

	private bool previewImageWasLoaded;

	private Texture2D iconImage;

	private bool iconImageWasLoaded;

	public bool enabled = true;

	private ModMetaDataInternal meta = new ModMetaDataInternal();

	public ModLoadFolders loadFolders;

	private WorkshopItemHook workshopHookInt;

	private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

	public bool appendPackageIdSteamPostfix;

	public bool translationMod;

	public bool uploadBuildAssetPack = true;

	public bool uploadSkipSourceTextures;

	public DirectoryInfo workshopUploadDirectoryOverride;

	public string packageIdLowerCase;

	private string descriptionCached;

	private const string AboutFolderName = "About";

	public static readonly string SteamModPostfix = "_steam";

	private static readonly CachedTexture ModIcon = new CachedTexture("UI/Icons/Options/OptionsMod");

	private static readonly string[] AndToken = new string[1] { " and " };

	private List<string> unsatisfiedDepsList = new List<string>();

	public Texture2D PreviewImage
	{
		get
		{
			if (previewImageWasLoaded)
			{
				return previewImage;
			}
			if (File.Exists(PreviewImagePath))
			{
				previewImage = new Texture2D(0, 0);
				previewImage.LoadImage(File.ReadAllBytes(PreviewImagePath));
			}
			previewImageWasLoaded = true;
			return previewImage;
		}
	}

	public Texture2D Icon
	{
		get
		{
			if (iconImageWasLoaded)
			{
				return iconImage;
			}
			if (!ModIconPath.NullOrEmpty())
			{
				iconImage = ContentFinder<Texture2D>.Get(ModIconPath);
			}
			else if (File.Exists(ModIconImagePath))
			{
				iconImage = new Texture2D(0, 0);
				iconImage.LoadImage(File.ReadAllBytes(ModIconImagePath));
			}
			else
			{
				iconImage = ModIcon.Texture;
			}
			iconImageWasLoaded = true;
			return iconImage;
		}
	}

	public string FolderName => RootDir.Name;

	public DirectoryInfo RootDir => rootDirInt;

	public bool IsCoreMod => SamePackageId("ludeon.rimworld");

	public bool Active
	{
		get
		{
			return ModsConfig.IsActive(this);
		}
		set
		{
			ModsConfig.SetActive(this, value);
		}
	}

	public bool VersionCompatible
	{
		get
		{
			if (IsCoreMod)
			{
				return true;
			}
			return meta.SupportedVersions.Any((System.Version v) => VersionControl.IsCompatible(v));
		}
	}

	public bool MadeForNewerVersion
	{
		get
		{
			if (VersionCompatible)
			{
				return false;
			}
			return meta.SupportedVersions.Any((System.Version v) => v.Major > VersionControl.CurrentMajor || (v.Major == VersionControl.CurrentMajor && v.Minor > VersionControl.CurrentMinor));
		}
	}

	public ExpansionDef Expansion => ModLister.GetExpansionWithIdentifier(PackageId);

	public string Name
	{
		get
		{
			ExpansionDef expansion = Expansion;
			if (expansion == null)
			{
				return meta.name;
			}
			return expansion.label;
		}
	}

	public string ShortName
	{
		get
		{
			if (!meta.shortName.NullOrEmpty())
			{
				return meta.shortName;
			}
			return Name;
		}
	}

	public string Description
	{
		get
		{
			if (descriptionCached == null)
			{
				ExpansionDef expansionWithIdentifier = ModLister.GetExpansionWithIdentifier(PackageId);
				descriptionCached = ((expansionWithIdentifier != null) ? expansionWithIdentifier.description : meta.description);
			}
			return descriptionCached;
		}
	}

	public string AuthorsString => Authors.ToCommaList(useAnd: true);

	public IEnumerable<string> Authors
	{
		[IteratorStateMachine(typeof(_003Cget_Authors_003Ed__51))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Authors_003Ed__51(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public virtual string ModVersion => meta.modVersion;

	public string Url => meta.url;

	public uint SteamAppId => meta.steamAppId;

	public List<System.Version> SupportedVersionsReadOnly => meta.SupportedVersions;

	IEnumerable<System.Version> WorkshopUploadable.SupportedVersions => SupportedVersionsReadOnly;

	public string PreviewImagePath => GenFile.ResolveCaseInsensitiveFilePath(Path.Combine(rootDirInt.FullName, "About"), "Preview.png");

	public string ModIconImagePath => GenFile.ResolveCaseInsensitiveFilePath(Path.Combine(rootDirInt.FullName, "About"), "ModIcon.png");

	public bool Official
	{
		get
		{
			if (!IsCoreMod)
			{
				return Source == ContentSource.OfficialModsFolder;
			}
			return true;
		}
	}

	public ContentSource Source => source;

	public string PackageId
	{
		get
		{
			if (!appendPackageIdSteamPostfix)
			{
				return packageIdLowerCase;
			}
			return packageIdLowerCase + SteamModPostfix;
		}
	}

	public string PackageIdNonUnique => packageIdLowerCase;

	public string PackageIdPlayerFacing => meta.packageId;

	public List<ModDependency> Dependencies => meta.modDependencies;

	public List<string> LoadBefore => meta.loadBefore;

	public List<string> LoadAfter => meta.loadAfter;

	public List<string> ForceLoadBefore => meta.forceLoadBefore;

	public List<string> ForceLoadAfter => meta.forceLoadAfter;

	public List<string> IncompatibleWith => meta.incompatibleWith;

	public string ModIconPath => meta.modIconPath;

	public bool HadIncorrectlyFormattedVersionInMetadata { get; private set; }

	public bool HadIncorrectlyFormattedPackageId { get; private set; }

	public bool OnSteamWorkshop => source == ContentSource.SteamWorkshop;

	private string PublishedFileIdPath => rootDirInt.FullName + Path.DirectorySeparatorChar + "About" + Path.DirectorySeparatorChar + "PublishedFileId.txt";

	public List<string> UnsatisfiedDependencies()
	{
		unsatisfiedDepsList.Clear();
		for (int i = 0; i < Dependencies.Count; i++)
		{
			ModDependency modDependency = Dependencies[i];
			if (!modDependency.IsSatisfied)
			{
				unsatisfiedDepsList.Add(modDependency.displayName);
			}
		}
		return unsatisfiedDepsList;
	}

	public ModMetaData(string localAbsPath, bool official = false)
	{
		rootDirInt = new DirectoryInfo(localAbsPath);
		source = (official ? ContentSource.OfficialModsFolder : ContentSource.ModsFolder);
		Init();
	}

	public ModMetaData(WorkshopItem workshopItem)
	{
		rootDirInt = workshopItem.Directory;
		source = ContentSource.SteamWorkshop;
		Init();
	}

	public void UnsetPreviewImage()
	{
		previewImage = null;
	}

	public bool SamePackageId(string otherPackageId, bool ignorePostfix = false)
	{
		if (PackageId == null)
		{
			return false;
		}
		if (ignorePostfix)
		{
			return packageIdLowerCase.Equals(otherPackageId, StringComparison.CurrentCultureIgnoreCase);
		}
		return PackageId.Equals(otherPackageId, StringComparison.CurrentCultureIgnoreCase);
	}

	public List<LoadFolder> LoadFoldersForVersion(string version)
	{
		return loadFolders?.FoldersForVersion(version);
	}

	private void Init()
	{
		meta = DirectXmlLoader.ItemFromXmlFile<ModMetaDataInternal>(GenFile.ResolveCaseInsensitiveFilePath(RootDir.FullName + Path.DirectorySeparatorChar + "About", "About.xml"), resolveCrossRefs: false);
		loadFolders = DirectXmlLoader.ItemFromXmlFile<ModLoadFolders>(GenFile.ResolveCaseInsensitiveFilePath(RootDir.FullName, "LoadFolders.xml"), resolveCrossRefs: false);
		bool shouldLogIssues = ModLister.ShouldLogIssues;
		HadIncorrectlyFormattedVersionInMetadata = !meta.TryParseSupportedVersions(!OnSteamWorkshop && shouldLogIssues);
		if (meta.name.NullOrEmpty())
		{
			if (OnSteamWorkshop)
			{
				meta.name = "Workshop mod " + FolderName;
			}
			else
			{
				meta.name = FolderName;
			}
		}
		HadIncorrectlyFormattedPackageId = !meta.TryParsePackageId(Official, !OnSteamWorkshop && shouldLogIssues);
		packageIdLowerCase = meta.packageId.ToLower();
		meta.InitVersionedData();
		meta.ValidateDependencies(shouldLogIssues);
		string publishedFileIdPath = PublishedFileIdPath;
		if (File.Exists(PublishedFileIdPath) && ulong.TryParse(File.ReadAllText(publishedFileIdPath), out var result))
		{
			publishedFileIdInt = new PublishedFileId_t(result);
		}
	}

	internal void DeleteContent()
	{
		rootDirInt.Delete(recursive: true);
		ModLister.RebuildModList();
	}

	public void PrepareForWorkshopUpload()
	{
	}

	public bool CanToUploadToWorkshop()
	{
		if (Official)
		{
			return false;
		}
		if (Source != ContentSource.ModsFolder)
		{
			return false;
		}
		if (GetWorkshopItemHook().MayHaveAuthorNotCurrentUser)
		{
			return false;
		}
		return true;
	}

	public PublishedFileId_t GetPublishedFileId()
	{
		return publishedFileIdInt;
	}

	public void SetPublishedFileId(PublishedFileId_t newPfid)
	{
		if (!(publishedFileIdInt == newPfid))
		{
			publishedFileIdInt = newPfid;
			File.WriteAllText(PublishedFileIdPath, newPfid.ToString());
		}
	}

	public string GetWorkshopName()
	{
		return Name;
	}

	public string GetWorkshopDescription()
	{
		return Description;
	}

	public string GetWorkshopPreviewImagePath()
	{
		return PreviewImagePath;
	}

	public IList<string> GetWorkshopTags()
	{
		if (!translationMod)
		{
			return new List<string> { "Mod" };
		}
		return new List<string> { "Translation" };
	}

	public DirectoryInfo GetWorkshopUploadDirectory()
	{
		return workshopUploadDirectoryOverride ?? RootDir;
	}

	public WorkshopItemHook GetWorkshopItemHook()
	{
		if (workshopHookInt == null)
		{
			workshopHookInt = new WorkshopItemHook(this);
		}
		return workshopHookInt;
	}

	[IteratorStateMachine(typeof(_003CGetRequirements_003Ed__122))]
	public IEnumerable<ModRequirement> GetRequirements()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetRequirements_003Ed__122(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override int GetHashCode()
	{
		return PackageId.GetHashCode();
	}

	public override string ToString()
	{
		return "[" + PackageIdPlayerFacing + "|" + Name + "]";
	}

	public string ToStringLong()
	{
		return PackageIdPlayerFacing + "(" + RootDir.ToString() + ")";
	}
}
