using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000569 RID: 1385
	public class ModAssetBundlesHandler
	{
		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x060023EF RID: 9199 RVA: 0x000BAB54 File Offset: 0x000B8D54
		private string BundleSuffixForCurrentOs
		{
			get
			{
				RuntimePlatform platform = Application.platform;
				if (platform <= RuntimePlatform.WindowsPlayer)
				{
					if (platform <= RuntimePlatform.OSXPlayer)
					{
						return "_mac";
					}
					if (platform != RuntimePlatform.WindowsPlayer)
					{
						goto IL_3A;
					}
				}
				else if (platform != RuntimePlatform.WindowsEditor)
				{
					if (platform == RuntimePlatform.LinuxPlayer || platform == RuntimePlatform.LinuxEditor)
					{
						return "_linux";
					}
					goto IL_3A;
				}
				return "_win";
				IL_3A:
				throw new NotSupportedException(string.Format("Unsupported platform for asset bundle loading: {0}", Application.platform));
			}
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x000BABB6 File Offset: 0x000B8DB6
		public ModAssetBundlesHandler(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x000BABD0 File Offset: 0x000B8DD0
		public void ReloadAll(bool hotReload = false)
		{
			foreach (KeyValuePair<string, FileInfo> keyValuePair in ModContentPack.GetAllFilesForMod(this.mod, GenFilePaths.ContentPath<AssetBundle>(), new Func<string, bool>(this.IsAcceptableExtension), null))
			{
				string text;
				FileInfo fileInfo;
				keyValuePair.Deconstruct(out text, out fileInfo);
				FileInfo fileInfo2 = fileInfo;
				string bundleNameWithoutOsSpecifier = this.GetBundleNameWithoutOsSpecifier(fileInfo2);
				bool flag = bundleNameWithoutOsSpecifier == null || fileInfo2.Name.Replace(bundleNameWithoutOsSpecifier, "") == this.BundleSuffixForCurrentOs;
				if (flag)
				{
					AssetBundle assetBundle = AssetBundle.LoadFromFile(fileInfo2.FullName);
					if (assetBundle != null)
					{
						if (!this.loadedAssetBundles.Contains(assetBundle))
						{
							this.loadedAssetBundles.Add(assetBundle);
						}
					}
					else
					{
						Log.Error("Could not load asset bundle at " + fileInfo2.FullName);
					}
				}
			}
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x000BACC8 File Offset: 0x000B8EC8
		private bool IsAcceptableExtension(string extension)
		{
			return extension.NullOrEmpty();
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x000BACD8 File Offset: 0x000B8ED8
		private string GetBundleNameWithoutOsSpecifier(FileInfo file)
		{
			string name = file.Name;
			if (name.EndsWith("_linux"))
			{
				string text = name;
				int length = "_linux".Length;
				return text.Substring(0, text.Length - length);
			}
			if (name.EndsWith("_mac"))
			{
				string text = name;
				int length = "_mac".Length;
				return text.Substring(0, text.Length - length);
			}
			if (name.EndsWith("_win"))
			{
				string text = name;
				int length = "_win".Length;
				return text.Substring(0, text.Length - length);
			}
			return null;
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x000BAD6B File Offset: 0x000B8F6B
		public void ClearDestroy()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				for (int i = 0; i < this.loadedAssetBundles.Count; i++)
				{
					this.loadedAssetBundles[i].Unload(true);
				}
				this.loadedAssetBundles.Clear();
			});
		}

		// Token: 0x04001DC2 RID: 7618
		private ModContentPack mod;

		// Token: 0x04001DC3 RID: 7619
		public List<AssetBundle> loadedAssetBundles = new List<AssetBundle>();

		// Token: 0x04001DC4 RID: 7620
		public static readonly string[] TextureExtensions = new string[]
		{
			".png",
			".psd",
			".jpg",
			".jpeg"
		};

		// Token: 0x04001DC5 RID: 7621
		public static readonly string[] AudioClipExtensions = new string[]
		{
			".wav",
			".mp3",
			".ogg"
		};

		// Token: 0x04001DC6 RID: 7622
		public static readonly string[] ShaderExtensions = new string[]
		{
			".shader"
		};

		// Token: 0x04001DC7 RID: 7623
		public const string LinuxBundleSuffix = "_linux";

		// Token: 0x04001DC8 RID: 7624
		public const string MacBundleSuffix = "_mac";

		// Token: 0x04001DC9 RID: 7625
		public const string WindowsBundleSuffix = "_win";
	}
}
