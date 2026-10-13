using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200055E RID: 1374
	public static class ContentFinder<T> where T : class
	{
		// Token: 0x060023AE RID: 9134 RVA: 0x000B89F0 File Offset: 0x000B6BF0
		public static T Get(string itemPath, bool reportFailure = true)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get a resource \"" + itemPath + "\" from a different thread. All resources must be loaded in the main thread.");
				return default(T);
			}
			T t = default(T);
			if (typeof(T) != typeof(Shader))
			{
				List<ModContentPack> runningModsListForReading = LoadedModManager.RunningModsListForReading;
				for (int i = runningModsListForReading.Count - 1; i >= 0; i--)
				{
					t = runningModsListForReading[i].GetContentHolder<T>().Get(itemPath);
					if (t != null)
					{
						return t;
					}
				}
			}
			if (typeof(T) == typeof(Texture2D))
			{
				t = (T)((object)Resources.Load<Texture2D>(GenFilePaths.ContentPath<Texture2D>() + itemPath));
			}
			if (typeof(T) == typeof(AudioClip))
			{
				t = (T)((object)Resources.Load<AudioClip>(GenFilePaths.ContentPath<AudioClip>() + itemPath));
			}
			if (t != null)
			{
				return t;
			}
			if (typeof(T) == typeof(Texture2D) || typeof(T) == typeof(AudioClip) || typeof(T) == typeof(Shader))
			{
				T t2 = ContentFinder<T>.TryFindAssetInModBundles(itemPath);
				if (t2 != null)
				{
					return t2;
				}
			}
			if (reportFailure)
			{
				string text = (ContentFinderRequester.requester != null) ? (" for def '" + ContentFinderRequester.requester.defName + "'") : "";
				Log.Error(string.Concat(new string[]
				{
					"Could not load ",
					typeof(T).Name,
					" at '",
					itemPath,
					"'",
					text,
					" in any active mod or in base resources."
				}));
			}
			return default(T);
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x000B8BD0 File Offset: 0x000B6DD0
		public static T TryFindAssetInModBundles(string itemPath)
		{
			string[] array;
			if (typeof(T) == typeof(Texture2D))
			{
				array = ModAssetBundlesHandler.TextureExtensions;
			}
			else if (typeof(T) == typeof(AudioClip))
			{
				array = ModAssetBundlesHandler.AudioClipExtensions;
			}
			else
			{
				if (!(typeof(T) == typeof(Shader)))
				{
					throw new NotSupportedException(string.Format("Unsupported type {0} for asset bundle loading. Did you forget to add a new type?", typeof(T)));
				}
				array = ModAssetBundlesHandler.ShaderExtensions;
			}
			List<ModContentPack> runningModsListForReading = LoadedModManager.RunningModsListForReading;
			string path = Path.Combine("Assets", "Data");
			for (int i = runningModsListForReading.Count - 1; i >= 0; i--)
			{
				string path2 = Path.Combine(path, runningModsListForReading[i].FolderName);
				string path3 = Path.Combine(path, runningModsListForReading[i].PackageIdPlayerFacing);
				bool flag = !runningModsListForReading[i].IsOfficialMod;
				foreach (AssetBundle assetBundle in runningModsListForReading[i].assetBundles.loadedAssetBundles)
				{
					string str = Path.Combine(Path.Combine(path2, GenFilePaths.ContentPath<T>()), itemPath);
					string str2 = Path.Combine(Path.Combine(path3, GenFilePaths.ContentPath<T>()), itemPath);
					foreach (string str3 in array)
					{
						T t = assetBundle.LoadAsset(str + str3, typeof(T)) as T;
						if (t != null)
						{
							return t;
						}
						if (flag)
						{
							T t2 = assetBundle.LoadAsset(str2 + str3, typeof(T)) as T;
							if (t2 != null)
							{
								return t2;
							}
						}
					}
				}
			}
			return default(T);
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x000B8DE0 File Offset: 0x000B6FE0
		public static IEnumerable<T> GetAllInFolder(string folderPath)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to get all resources in a folder \"" + folderPath + "\" from a different thread. All resources must be loaded in the main thread.");
				yield break;
			}
			foreach (ModContentPack modContentPack in LoadedModManager.RunningModsListForReading)
			{
				foreach (T t in modContentPack.GetContentHolder<T>().GetAllUnderPath(folderPath))
				{
					yield return t;
				}
				IEnumerator<T> enumerator2 = null;
			}
			List<ModContentPack>.Enumerator enumerator = default(List<ModContentPack>.Enumerator);
			T[] array = null;
			if (typeof(T) == typeof(Texture2D))
			{
				array = (T[])Resources.LoadAll<Texture2D>(GenFilePaths.ContentPath<Texture2D>() + folderPath);
			}
			if (typeof(T) == typeof(AudioClip))
			{
				array = (T[])Resources.LoadAll<AudioClip>(GenFilePaths.ContentPath<AudioClip>() + folderPath);
			}
			if (array != null)
			{
				foreach (T t2 in array)
				{
					yield return t2;
				}
				T[] array2 = null;
			}
			List<ModContentPack> mods = LoadedModManager.RunningModsListForReading;
			string modsDir = Path.Combine("Assets", "Data");
			if (!(typeof(T) == typeof(Texture2D)) && !(typeof(T) == typeof(AudioClip)))
			{
				yield break;
			}
			int num;
			for (int j = mods.Count - 1; j >= 0; j = num - 1)
			{
				string dirForBundleWithFolderName = Path.Combine(modsDir, mods[j].FolderName);
				string dirForBundleWithPackageId = Path.Combine(modsDir, mods[j].PackageIdPlayerFacing);
				bool canLoadViaPackageId = !mods[j].IsOfficialMod;
				string[] validExtensions;
				if (typeof(T) == typeof(Texture2D))
				{
					validExtensions = ModAssetBundlesHandler.TextureExtensions;
				}
				else
				{
					if (!(typeof(T) == typeof(AudioClip)))
					{
						throw new NotSupportedException(string.Format("Unsupported type {0} for asset bundle loading. Did you forget to add a new type?", typeof(T)));
					}
					validExtensions = ModAssetBundlesHandler.AudioClipExtensions;
				}
				for (int i = 0; i < mods[j].assetBundles.loadedAssetBundles.Count; i = num + 1)
				{
					AssetBundle assetBundle = mods[j].assetBundles.loadedAssetBundles[i];
					string text = Path.Combine(Path.Combine(dirForBundleWithFolderName, GenFilePaths.ContentPath<T>()).Replace('\\', '/'), folderPath).ToLower();
					string pathWithoutExtWithPackageId = Path.Combine(Path.Combine(dirForBundleWithPackageId, GenFilePaths.ContentPath<T>()).Replace('\\', '/'), folderPath).ToLower();
					string text2 = text;
					if (text2[text2.Length - 1] != '/')
					{
						text += "/";
					}
					string text3 = pathWithoutExtWithPackageId;
					if (text3[text3.Length - 1] != '/')
					{
						pathWithoutExtWithPackageId += "/";
					}
					IEnumerable<string> byPrefix = mods[j].AllAssetNamesInBundleTrie(i).GetByPrefix(text);
					foreach (string text4 in byPrefix)
					{
						if (validExtensions.Contains(Path.GetExtension(text4)))
						{
							yield return (T)((object)assetBundle.LoadAsset(text4, typeof(T)));
						}
					}
					IEnumerator<string> enumerator3 = null;
					if (canLoadViaPackageId)
					{
						IEnumerable<string> byPrefix2 = mods[j].AllAssetNamesInBundleTrie(i).GetByPrefix(pathWithoutExtWithPackageId);
						foreach (string text5 in byPrefix2)
						{
							if (validExtensions.Contains(Path.GetExtension(text5)))
							{
								yield return (T)((object)assetBundle.LoadAsset(text5, typeof(T)));
							}
						}
						enumerator3 = null;
						assetBundle = null;
						pathWithoutExtWithPackageId = null;
					}
					num = i;
				}
				dirForBundleWithFolderName = null;
				dirForBundleWithPackageId = null;
				validExtensions = null;
				num = j;
			}
			yield break;
			yield break;
		}
	}
}
