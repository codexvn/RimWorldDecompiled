using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using RimWorld;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000577 RID: 1399
	public class ModMetaData : WorkshopUploadable
	{
		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x0600247A RID: 9338 RVA: 0x000BD370 File Offset: 0x000BB570
		public Texture2D PreviewImage
		{
			get
			{
				if (this.previewImageWasLoaded)
				{
					return this.previewImage;
				}
				if (File.Exists(this.PreviewImagePath))
				{
					this.previewImage = new Texture2D(0, 0);
					this.previewImage.LoadImage(File.ReadAllBytes(this.PreviewImagePath));
				}
				this.previewImageWasLoaded = true;
				return this.previewImage;
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x0600247B RID: 9339 RVA: 0x000BD3CC File Offset: 0x000BB5CC
		public Texture2D Icon
		{
			get
			{
				if (this.iconImageWasLoaded)
				{
					return this.iconImage;
				}
				if (!this.ModIconPath.NullOrEmpty())
				{
					this.iconImage = ContentFinder<Texture2D>.Get(this.ModIconPath, true);
				}
				else if (File.Exists(this.ModIconImagePath))
				{
					this.iconImage = new Texture2D(0, 0);
					this.iconImage.LoadImage(File.ReadAllBytes(this.ModIconImagePath));
				}
				else
				{
					this.iconImage = ModMetaData.ModIcon.Texture;
				}
				this.iconImageWasLoaded = true;
				return this.iconImage;
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x0600247C RID: 9340 RVA: 0x000BD459 File Offset: 0x000BB659
		public string FolderName
		{
			get
			{
				return this.RootDir.Name;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x0600247D RID: 9341 RVA: 0x000BD466 File Offset: 0x000BB666
		public DirectoryInfo RootDir
		{
			get
			{
				return this.rootDirInt;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x0600247E RID: 9342 RVA: 0x000BD46E File Offset: 0x000BB66E
		public bool IsCoreMod
		{
			get
			{
				return this.SamePackageId("ludeon.rimworld", false);
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x0600247F RID: 9343 RVA: 0x000BD47C File Offset: 0x000BB67C
		// (set) Token: 0x06002480 RID: 9344 RVA: 0x000BD484 File Offset: 0x000BB684
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

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06002481 RID: 9345 RVA: 0x000BD48D File Offset: 0x000BB68D
		public bool VersionCompatible
		{
			get
			{
				if (this.IsCoreMod)
				{
					return true;
				}
				return this.meta.SupportedVersions.Any((System.Version v) => VersionControl.IsCompatible(v));
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06002482 RID: 9346 RVA: 0x000BD4C8 File Offset: 0x000BB6C8
		public bool MadeForNewerVersion
		{
			get
			{
				if (this.VersionCompatible)
				{
					return false;
				}
				return this.meta.SupportedVersions.Any((System.Version v) => v.Major > VersionControl.CurrentMajor || (v.Major == VersionControl.CurrentMajor && v.Minor > VersionControl.CurrentMinor));
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06002483 RID: 9347 RVA: 0x000BD503 File Offset: 0x000BB703
		public ExpansionDef Expansion
		{
			get
			{
				return ModLister.GetExpansionWithIdentifier(this.PackageId);
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06002484 RID: 9348 RVA: 0x000BD510 File Offset: 0x000BB710
		public string Name
		{
			get
			{
				ExpansionDef expansion = this.Expansion;
				if (expansion == null)
				{
					return this.meta.name;
				}
				return expansion.label;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06002485 RID: 9349 RVA: 0x000BD539 File Offset: 0x000BB739
		public string ShortName
		{
			get
			{
				if (!this.meta.shortName.NullOrEmpty())
				{
					return this.meta.shortName;
				}
				return this.Name;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06002486 RID: 9350 RVA: 0x000BD560 File Offset: 0x000BB760
		public string Description
		{
			get
			{
				if (this.descriptionCached == null)
				{
					ExpansionDef expansionWithIdentifier = ModLister.GetExpansionWithIdentifier(this.PackageId);
					this.descriptionCached = ((expansionWithIdentifier != null) ? expansionWithIdentifier.description : this.meta.description);
				}
				return this.descriptionCached;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x06002487 RID: 9351 RVA: 0x000BD5A3 File Offset: 0x000BB7A3
		public string AuthorsString
		{
			get
			{
				return this.Authors.ToCommaList(true, false);
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06002488 RID: 9352 RVA: 0x000BD5B2 File Offset: 0x000BB7B2
		public IEnumerable<string> Authors
		{
			get
			{
				if (!this.meta.authors.NullOrEmpty<string>())
				{
					foreach (string text in this.meta.authors)
					{
						yield return text;
					}
					List<string>.Enumerator enumerator = default(List<string>.Enumerator);
					yield break;
				}
				if (!string.IsNullOrWhiteSpace(this.meta.author))
				{
					foreach (string text2 in this.meta.author.Split(ModMetaData.AndToken, StringSplitOptions.RemoveEmptyEntries).SelectMany((string x) => x.Split(',', StringSplitOptions.None)))
					{
						yield return text2;
					}
					IEnumerator<string> enumerator2 = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06002489 RID: 9353 RVA: 0x000BD5C2 File Offset: 0x000BB7C2
		public virtual string ModVersion
		{
			get
			{
				return this.meta.modVersion;
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x0600248A RID: 9354 RVA: 0x000BD5CF File Offset: 0x000BB7CF
		public string Url
		{
			get
			{
				return this.meta.url;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x0600248B RID: 9355 RVA: 0x000BD5DC File Offset: 0x000BB7DC
		public uint SteamAppId
		{
			get
			{
				return this.meta.steamAppId;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x0600248C RID: 9356 RVA: 0x000BD5E9 File Offset: 0x000BB7E9
		public List<System.Version> SupportedVersionsReadOnly
		{
			get
			{
				return this.meta.SupportedVersions;
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x0600248D RID: 9357 RVA: 0x000BD5F6 File Offset: 0x000BB7F6
		IEnumerable<System.Version> WorkshopUploadable.SupportedVersions
		{
			get
			{
				return this.SupportedVersionsReadOnly;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x0600248E RID: 9358 RVA: 0x000BD5FE File Offset: 0x000BB7FE
		public string PreviewImagePath
		{
			get
			{
				return Path.Combine(this.rootDirInt.FullName, "About", "Preview.png");
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x0600248F RID: 9359 RVA: 0x000BD61A File Offset: 0x000BB81A
		public string ModIconImagePath
		{
			get
			{
				return Path.Combine(this.rootDirInt.FullName, "About", "ModIcon.png");
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06002490 RID: 9360 RVA: 0x000BD636 File Offset: 0x000BB836
		public bool Official
		{
			get
			{
				return this.IsCoreMod || this.Source == ContentSource.OfficialModsFolder;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06002491 RID: 9361 RVA: 0x000BD64B File Offset: 0x000BB84B
		public ContentSource Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06002492 RID: 9362 RVA: 0x000BD653 File Offset: 0x000BB853
		public string PackageId
		{
			get
			{
				if (!this.appendPackageIdSteamPostfix)
				{
					return this.packageIdLowerCase;
				}
				return this.packageIdLowerCase + ModMetaData.SteamModPostfix;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002493 RID: 9363 RVA: 0x000BD674 File Offset: 0x000BB874
		public string PackageIdNonUnique
		{
			get
			{
				return this.packageIdLowerCase;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06002494 RID: 9364 RVA: 0x000BD67C File Offset: 0x000BB87C
		public string PackageIdPlayerFacing
		{
			get
			{
				return this.meta.packageId;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06002495 RID: 9365 RVA: 0x000BD689 File Offset: 0x000BB889
		public List<ModDependency> Dependencies
		{
			get
			{
				return this.meta.modDependencies;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06002496 RID: 9366 RVA: 0x000BD696 File Offset: 0x000BB896
		public List<string> LoadBefore
		{
			get
			{
				return this.meta.loadBefore;
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x06002497 RID: 9367 RVA: 0x000BD6A3 File Offset: 0x000BB8A3
		public List<string> LoadAfter
		{
			get
			{
				return this.meta.loadAfter;
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x06002498 RID: 9368 RVA: 0x000BD6B0 File Offset: 0x000BB8B0
		public List<string> ForceLoadBefore
		{
			get
			{
				return this.meta.forceLoadBefore;
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06002499 RID: 9369 RVA: 0x000BD6BD File Offset: 0x000BB8BD
		public List<string> ForceLoadAfter
		{
			get
			{
				return this.meta.forceLoadAfter;
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x0600249A RID: 9370 RVA: 0x000BD6CA File Offset: 0x000BB8CA
		public List<string> IncompatibleWith
		{
			get
			{
				return this.meta.incompatibleWith;
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x0600249B RID: 9371 RVA: 0x000BD6D7 File Offset: 0x000BB8D7
		public string ModIconPath
		{
			get
			{
				return this.meta.modIconPath;
			}
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x000BD6E4 File Offset: 0x000BB8E4
		public List<string> UnsatisfiedDependencies()
		{
			this.unsatisfiedDepsList.Clear();
			for (int i = 0; i < this.Dependencies.Count; i++)
			{
				ModDependency modDependency = this.Dependencies[i];
				if (!modDependency.IsSatisfied)
				{
					this.unsatisfiedDepsList.Add(modDependency.displayName);
				}
			}
			return this.unsatisfiedDepsList;
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x0600249D RID: 9373 RVA: 0x000BD73E File Offset: 0x000BB93E
		// (set) Token: 0x0600249E RID: 9374 RVA: 0x000BD746 File Offset: 0x000BB946
		public bool HadIncorrectlyFormattedVersionInMetadata { get; private set; }

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x0600249F RID: 9375 RVA: 0x000BD74F File Offset: 0x000BB94F
		// (set) Token: 0x060024A0 RID: 9376 RVA: 0x000BD757 File Offset: 0x000BB957
		public bool HadIncorrectlyFormattedPackageId { get; private set; }

		// Token: 0x060024A1 RID: 9377 RVA: 0x000BD760 File Offset: 0x000BB960
		public ModMetaData(string localAbsPath, bool official = false)
		{
			this.rootDirInt = new DirectoryInfo(localAbsPath);
			this.source = (official ? ContentSource.OfficialModsFolder : ContentSource.ModsFolder);
			this.Init();
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x000BD7BC File Offset: 0x000BB9BC
		public ModMetaData(WorkshopItem workshopItem)
		{
			this.rootDirInt = workshopItem.Directory;
			this.source = ContentSource.SteamWorkshop;
			this.Init();
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x000BD810 File Offset: 0x000BBA10
		public void UnsetPreviewImage()
		{
			this.previewImage = null;
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x000BD819 File Offset: 0x000BBA19
		public bool SamePackageId(string otherPackageId, bool ignorePostfix = false)
		{
			if (this.PackageId == null)
			{
				return false;
			}
			if (ignorePostfix)
			{
				return this.packageIdLowerCase.Equals(otherPackageId, StringComparison.CurrentCultureIgnoreCase);
			}
			return this.PackageId.Equals(otherPackageId, StringComparison.CurrentCultureIgnoreCase);
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x000BD843 File Offset: 0x000BBA43
		public List<LoadFolder> LoadFoldersForVersion(string version)
		{
			ModLoadFolders modLoadFolders = this.loadFolders;
			if (modLoadFolders == null)
			{
				return null;
			}
			return modLoadFolders.FoldersForVersion(version);
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x000BD858 File Offset: 0x000BBA58
		private void Init()
		{
			this.meta = DirectXmlLoader.ItemFromXmlFile<ModMetaData.ModMetaDataInternal>(GenFile.ResolveCaseInsensitiveFilePath(this.RootDir.FullName + Path.DirectorySeparatorChar.ToString() + "About", "About.xml"), false);
			this.loadFolders = DirectXmlLoader.ItemFromXmlFile<ModLoadFolders>(GenFile.ResolveCaseInsensitiveFilePath(this.RootDir.FullName, "LoadFolders.xml"), false);
			bool shouldLogIssues = ModLister.ShouldLogIssues;
			this.HadIncorrectlyFormattedVersionInMetadata = !this.meta.TryParseSupportedVersions(!this.OnSteamWorkshop && shouldLogIssues);
			if (this.meta.name.NullOrEmpty())
			{
				if (this.OnSteamWorkshop)
				{
					this.meta.name = "Workshop mod " + this.FolderName;
				}
				else
				{
					this.meta.name = this.FolderName;
				}
			}
			this.HadIncorrectlyFormattedPackageId = !this.meta.TryParsePackageId(this.Official, !this.OnSteamWorkshop && shouldLogIssues);
			this.packageIdLowerCase = this.meta.packageId.ToLower();
			this.meta.InitVersionedData();
			this.meta.ValidateDependencies(shouldLogIssues);
			string publishedFileIdPath = this.PublishedFileIdPath;
			ulong value;
			if (File.Exists(this.PublishedFileIdPath) && ulong.TryParse(File.ReadAllText(publishedFileIdPath), out value))
			{
				this.publishedFileIdInt = new PublishedFileId_t(value);
			}
		}

		// Token: 0x060024A7 RID: 9383 RVA: 0x000BD9AC File Offset: 0x000BBBAC
		internal void DeleteContent()
		{
			this.rootDirInt.Delete(true);
			ModLister.RebuildModList();
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x060024A8 RID: 9384 RVA: 0x000BD9BF File Offset: 0x000BBBBF
		public bool OnSteamWorkshop
		{
			get
			{
				return this.source == ContentSource.SteamWorkshop;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x060024A9 RID: 9385 RVA: 0x000BD9CC File Offset: 0x000BBBCC
		private string PublishedFileIdPath
		{
			get
			{
				return string.Concat(new string[]
				{
					this.rootDirInt.FullName,
					Path.DirectorySeparatorChar.ToString(),
					"About",
					Path.DirectorySeparatorChar.ToString(),
					"PublishedFileId.txt"
				});
			}
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x000026EA File Offset: 0x000008EA
		public void PrepareForWorkshopUpload()
		{
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x000BDA22 File Offset: 0x000BBC22
		public bool CanToUploadToWorkshop()
		{
			return !this.Official && this.Source == ContentSource.ModsFolder && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x000BDA49 File Offset: 0x000BBC49
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x000BDA51 File Offset: 0x000BBC51
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			if (this.publishedFileIdInt == newPfid)
			{
				return;
			}
			this.publishedFileIdInt = newPfid;
			File.WriteAllText(this.PublishedFileIdPath, newPfid.ToString());
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x000BDA81 File Offset: 0x000BBC81
		public string GetWorkshopName()
		{
			return this.Name;
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x000BDA89 File Offset: 0x000BBC89
		public string GetWorkshopDescription()
		{
			return this.Description;
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x000BDA91 File Offset: 0x000BBC91
		public string GetWorkshopPreviewImagePath()
		{
			return this.PreviewImagePath;
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x000BDA99 File Offset: 0x000BBC99
		public IList<string> GetWorkshopTags()
		{
			if (!this.translationMod)
			{
				return new List<string>
				{
					"Mod"
				};
			}
			return new List<string>
			{
				"Translation"
			};
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x000BDAC4 File Offset: 0x000BBCC4
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return this.RootDir;
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x000BDACC File Offset: 0x000BBCCC
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000BDAE8 File Offset: 0x000BBCE8
		public IEnumerable<ModRequirement> GetRequirements()
		{
			int num;
			for (int i = 0; i < this.Dependencies.Count; i = num + 1)
			{
				yield return this.Dependencies[i];
				num = i;
			}
			for (int i = 0; i < this.meta.incompatibleWith.Count; i = num + 1)
			{
				ModMetaData modWithIdentifier = ModLister.GetModWithIdentifier(this.meta.incompatibleWith[i], false);
				if (modWithIdentifier != null)
				{
					yield return new ModIncompatibility
					{
						packageId = modWithIdentifier.PackageIdPlayerFacing,
						displayName = modWithIdentifier.Name
					};
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x000BDAF8 File Offset: 0x000BBCF8
		public override int GetHashCode()
		{
			return this.PackageId.GetHashCode();
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x000BDB05 File Offset: 0x000BBD05
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[",
				this.PackageIdPlayerFacing,
				"|",
				this.Name,
				"]"
			});
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x000BDB3C File Offset: 0x000BBD3C
		public string ToStringLong()
		{
			return this.PackageIdPlayerFacing + "(" + this.RootDir.ToString() + ")";
		}

		// Token: 0x04001E1F RID: 7711
		private DirectoryInfo rootDirInt;

		// Token: 0x04001E20 RID: 7712
		private ContentSource source;

		// Token: 0x04001E21 RID: 7713
		private Texture2D previewImage;

		// Token: 0x04001E22 RID: 7714
		private bool previewImageWasLoaded;

		// Token: 0x04001E23 RID: 7715
		private Texture2D iconImage;

		// Token: 0x04001E24 RID: 7716
		private bool iconImageWasLoaded;

		// Token: 0x04001E25 RID: 7717
		public bool enabled = true;

		// Token: 0x04001E26 RID: 7718
		private ModMetaData.ModMetaDataInternal meta = new ModMetaData.ModMetaDataInternal();

		// Token: 0x04001E27 RID: 7719
		public ModLoadFolders loadFolders;

		// Token: 0x04001E28 RID: 7720
		private WorkshopItemHook workshopHookInt;

		// Token: 0x04001E29 RID: 7721
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		// Token: 0x04001E2A RID: 7722
		public bool appendPackageIdSteamPostfix;

		// Token: 0x04001E2B RID: 7723
		public bool translationMod;

		// Token: 0x04001E2C RID: 7724
		public string packageIdLowerCase;

		// Token: 0x04001E2D RID: 7725
		private string descriptionCached;

		// Token: 0x04001E2E RID: 7726
		private const string AboutFolderName = "About";

		// Token: 0x04001E2F RID: 7727
		public static readonly string SteamModPostfix = "_steam";

		// Token: 0x04001E30 RID: 7728
		private static readonly CachedTexture ModIcon = new CachedTexture("UI/Icons/Options/OptionsMod");

		// Token: 0x04001E31 RID: 7729
		private static readonly string[] AndToken = new string[]
		{
			" and "
		};

		// Token: 0x04001E32 RID: 7730
		private List<string> unsatisfiedDepsList = new List<string>();

		// Token: 0x02000578 RID: 1400
		private class ModMetaDataInternal
		{
			// Token: 0x1700069B RID: 1691
			// (get) Token: 0x060024B9 RID: 9401 RVA: 0x000BDB8C File Offset: 0x000BBD8C
			// (set) Token: 0x060024BA RID: 9402 RVA: 0x000BDB94 File Offset: 0x000BBD94
			public List<System.Version> SupportedVersions { get; private set; }

			// Token: 0x060024BB RID: 9403 RVA: 0x000BDBA0 File Offset: 0x000BBDA0
			private bool TryParseVersion(string str, bool logIssues = true)
			{
				System.Version version;
				if (!VersionControl.TryParseVersionString(str, out version))
				{
					if (logIssues)
					{
						Log.Error(string.Concat(new string[]
						{
							"Unable to parse version string on mod ",
							this.name,
							" from ",
							this.author,
							" \"",
							str,
							"\""
						}));
					}
					return false;
				}
				this.SupportedVersions.Add(version);
				if (!VersionControl.IsWellFormattedVersionString(str))
				{
					if (logIssues)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Malformed (correct format is Major.Minor) version string on mod ",
							this.name,
							" from ",
							this.author,
							" \"",
							str,
							"\" - parsed as \"",
							version.Major.ToString(),
							".",
							version.Minor.ToString(),
							"\""
						}));
					}
					return false;
				}
				return true;
			}

			// Token: 0x060024BC RID: 9404 RVA: 0x000BDC9C File Offset: 0x000BBE9C
			public bool TryParseSupportedVersions(bool logIssues = true)
			{
				if (this.targetVersion != null && logIssues)
				{
					Log.Warning("Mod " + this.name + ": targetVersion field is obsolete, use supportedVersions instead.");
				}
				bool flag = false;
				this.SupportedVersions = new List<System.Version>();
				if (this.packageId.ToLower() == "ludeon.rimworld")
				{
					this.SupportedVersions.Add(VersionControl.CurrentVersion);
				}
				else if (this.supportedVersions == null)
				{
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " is missing supported versions list in About.xml! (example: <supportedVersions><li>1.0</li></supportedVersions>)");
					}
					flag = true;
				}
				else if (this.supportedVersions.Count == 0)
				{
					if (logIssues)
					{
						Log.Error("Mod " + this.name + ": <supportedVersions> in mod About.xml must specify at least one version.");
					}
					flag = true;
				}
				else
				{
					for (int i = 0; i < this.supportedVersions.Count; i++)
					{
						flag |= !this.TryParseVersion(this.supportedVersions[i], logIssues);
					}
				}
				this.SupportedVersions = this.SupportedVersions.OrderBy(delegate(System.Version v)
				{
					if (!VersionControl.IsCompatible(v))
					{
						return 100;
					}
					return -100;
				}).ThenByDescending((System.Version v) => v.Major).ThenByDescending((System.Version v) => v.Minor).Distinct<System.Version>().ToList<System.Version>();
				return !flag;
			}

			// Token: 0x060024BD RID: 9405 RVA: 0x000BDE18 File Offset: 0x000BC018
			public bool TryParsePackageId(bool isOfficial, bool logIssues = true)
			{
				bool flag = false;
				if (this.packageId.NullOrEmpty())
				{
					string text = "none";
					if (!this.description.NullOrEmpty())
					{
						text = GenText.StableStringHash(this.description).ToString().Replace("-", "");
						text = text.Substring(0, Math.Min(3, text.Length));
					}
					this.packageId = this.ConvertToASCII(this.author + text) + "." + this.ConvertToASCII(this.name);
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " is missing packageId in About.xml! (example: <packageId>AuthorName.ModName.Specific</packageId>)");
					}
					flag = true;
				}
				if (!ModMetaData.ModMetaDataInternal.PackageIdFormatRegex.IsMatch(this.packageId))
				{
					if (logIssues)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Mod ",
							this.name,
							" <packageId> (",
							this.packageId,
							") is not in valid format."
						}));
					}
					flag = true;
				}
				if (!isOfficial && this.packageId.ToLower().Contains("ludeon"))
				{
					if (logIssues)
					{
						Log.Warning("Mod " + this.name + " <packageId> contains word \"Ludeon\", which is reserved for official content.");
					}
					flag = true;
				}
				return !flag;
			}

			// Token: 0x060024BE RID: 9406 RVA: 0x000BDF5C File Offset: 0x000BC15C
			private string ConvertToASCII(string part)
			{
				StringBuilder stringBuilder = new StringBuilder("");
				foreach (char c in part)
				{
					if (!char.IsLetterOrDigit(c) || c >= '\u0080')
					{
						c = c % '\u0019' + 'A';
					}
					stringBuilder.Append(c);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x060024BF RID: 9407 RVA: 0x000BDFB4 File Offset: 0x000BC1B4
			public void ValidateDependencies(bool logIssues = true)
			{
				for (int i = this.modDependencies.Count - 1; i >= 0; i--)
				{
					bool flag = false;
					ModDependency modDependency = this.modDependencies[i];
					if (modDependency.packageId.NullOrEmpty())
					{
						if (logIssues)
						{
							Log.Warning("Mod " + this.name + " has a dependency with no <packageId> specified.");
						}
						flag = true;
					}
					else if (!ModMetaData.ModMetaDataInternal.PackageIdFormatRegex.IsMatch(modDependency.packageId))
					{
						if (logIssues)
						{
							Log.Warning("Mod " + this.name + " has a dependency with invalid <packageId>: " + modDependency.packageId);
						}
						flag = true;
					}
					if (modDependency.displayName.NullOrEmpty())
					{
						if (logIssues)
						{
							Log.Warning(string.Concat(new string[]
							{
								"Mod ",
								this.name,
								" has a dependency (",
								modDependency.packageId,
								") with empty display name."
							}));
						}
						flag = true;
					}
					if (modDependency.downloadUrl.NullOrEmpty() && modDependency.steamWorkshopUrl.NullOrEmpty() && !modDependency.packageId.ToLower().Contains("ludeon"))
					{
						if (logIssues)
						{
							Log.Warning(string.Concat(new string[]
							{
								"Mod ",
								this.name,
								" dependency (",
								modDependency.packageId,
								") needs to have <downloadUrl> and/or <steamWorkshopUrl> specified."
							}));
						}
						flag = true;
					}
					if (flag)
					{
						this.modDependencies.Remove(modDependency);
					}
				}
			}

			// Token: 0x060024C0 RID: 9408 RVA: 0x000BE120 File Offset: 0x000BC320
			public void InitVersionedData()
			{
				string currentVersionStringWithoutBuild = VersionControl.CurrentVersionStringWithoutBuild;
				ModMetaData.VersionedData<string> versionedData = this.descriptionsByVersion;
				string text = (versionedData != null) ? versionedData.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (text != null)
				{
					this.description = text;
				}
				ModMetaData.VersionedData<List<ModDependency>> versionedData2 = this.modDependenciesByVersion;
				List<ModDependency> list = (versionedData2 != null) ? versionedData2.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list != null)
				{
					this.modDependencies = list;
				}
				ModMetaData.VersionedData<List<string>> versionedData3 = this.loadBeforeByVersion;
				List<string> list2 = (versionedData3 != null) ? versionedData3.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list2 != null)
				{
					this.loadBefore = list2;
				}
				ModMetaData.VersionedData<List<string>> versionedData4 = this.loadAfterByVersion;
				List<string> list3 = (versionedData4 != null) ? versionedData4.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list3 != null)
				{
					this.loadAfter = list3;
				}
				ModMetaData.VersionedData<List<string>> versionedData5 = this.incompatibleWithByVersion;
				List<string> list4 = (versionedData5 != null) ? versionedData5.GetItemForVersion(currentVersionStringWithoutBuild) : null;
				if (list4 != null)
				{
					this.incompatibleWith = list4;
				}
			}

			// Token: 0x04001E35 RID: 7733
			public string packageId = "";

			// Token: 0x04001E36 RID: 7734
			public string name = "";

			// Token: 0x04001E37 RID: 7735
			public string shortName = "";

			// Token: 0x04001E38 RID: 7736
			public string author = "Anonymous";

			// Token: 0x04001E39 RID: 7737
			public List<string> authors;

			// Token: 0x04001E3A RID: 7738
			public string modIconPath = "";

			// Token: 0x04001E3B RID: 7739
			public string modVersion = "";

			// Token: 0x04001E3C RID: 7740
			public string url = "";

			// Token: 0x04001E3D RID: 7741
			public string description = "No description provided.";

			// Token: 0x04001E3E RID: 7742
			public uint steamAppId;

			// Token: 0x04001E3F RID: 7743
			public List<string> supportedVersions;

			// Token: 0x04001E40 RID: 7744
			[Unsaved(true)]
			private string targetVersion;

			// Token: 0x04001E41 RID: 7745
			public List<ModDependency> modDependencies = new List<ModDependency>();

			// Token: 0x04001E42 RID: 7746
			public List<string> loadBefore = new List<string>();

			// Token: 0x04001E43 RID: 7747
			public List<string> loadAfter = new List<string>();

			// Token: 0x04001E44 RID: 7748
			public List<string> incompatibleWith = new List<string>();

			// Token: 0x04001E45 RID: 7749
			public List<string> forceLoadBefore = new List<string>();

			// Token: 0x04001E46 RID: 7750
			public List<string> forceLoadAfter = new List<string>();

			// Token: 0x04001E47 RID: 7751
			private ModMetaData.VersionedData<string> descriptionsByVersion;

			// Token: 0x04001E48 RID: 7752
			private ModMetaData.VersionedData<List<ModDependency>> modDependenciesByVersion;

			// Token: 0x04001E49 RID: 7753
			private ModMetaData.VersionedData<List<string>> loadBeforeByVersion;

			// Token: 0x04001E4A RID: 7754
			private ModMetaData.VersionedData<List<string>> loadAfterByVersion;

			// Token: 0x04001E4B RID: 7755
			private ModMetaData.VersionedData<List<string>> incompatibleWithByVersion;

			// Token: 0x04001E4C RID: 7756
			public static readonly Regex PackageIdFormatRegex = new Regex("(?=.{1,60}$)^(?!\\.)(?=.*?[.])(?!.*([.])\\1+)[a-zA-Z0-9.]{1,}[a-zA-Z0-9]{1}$");
		}

		// Token: 0x0200057A RID: 1402
		private class VersionedData<T> where T : class
		{
			// Token: 0x060024C8 RID: 9416 RVA: 0x000BE2BC File Offset: 0x000BC4BC
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				foreach (object obj in xmlRoot.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (!(xmlNode is XmlComment))
					{
						string text = xmlNode.Name.ToLower();
						if (text.StartsWith("v"))
						{
							text = text.Substring(1);
						}
						if (!this.itemForVersion.ContainsKey(text))
						{
							this.itemForVersion[text] = ((typeof(T) == typeof(string)) ? ((T)((object)xmlNode.FirstChild.Value)) : DirectXmlToObject.ObjectFromXml<T>(xmlNode, false));
						}
						else
						{
							Log.Warning("More than one value for a same version of " + typeof(T).Name + " named " + xmlRoot.Name);
						}
					}
				}
			}

			// Token: 0x060024C9 RID: 9417 RVA: 0x000BE3BC File Offset: 0x000BC5BC
			public T GetItemForVersion(string ver)
			{
				if (this.itemForVersion.ContainsKey(ver))
				{
					return this.itemForVersion[ver];
				}
				return default(T);
			}

			// Token: 0x04001E52 RID: 7762
			private Dictionary<string, T> itemForVersion = new Dictionary<string, T>();
		}
	}
}
