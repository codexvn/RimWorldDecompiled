using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse;

public static class ContentFinder<T> where T : class
{
	[CompilerGenerated]
	private sealed class _003CGetAllInFolder_003Ed__6 : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string folderPath;

		public string _003C_003E3__folderPath;

		private List<ModContentPack> _003Cmods_003E5__2;

		private string _003CmodsDir_003E5__3;

		private List<ModContentPack>.Enumerator _003C_003E7__wrap3;

		private IEnumerator<T> _003C_003E7__wrap4;

		private T[] _003C_003E7__wrap5;

		private int _003C_003E7__wrap6;

		private string _003CdirForBundleWithFolderName_003E5__8;

		private string _003CdirForBundleWithPackageId_003E5__9;

		private bool _003CcanLoadViaPackageId_003E5__10;

		private string[] _003CvalidExtensions_003E5__11;

		private int _003Cj_003E5__12;

		private AssetBundle _003CassetBundle_003E5__13;

		private string _003CpathWithoutExtWithPackageId_003E5__14;

		private IEnumerator<string> _003C_003E7__wrap14;

		T IEnumerator<T>.Current
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
		public _003CGetAllInFolder_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -4:
			case -3:
			case 1:
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -6:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			}
			_003Cmods_003E5__2 = null;
			_003CmodsDir_003E5__3 = null;
			_003C_003E7__wrap3 = default(List<ModContentPack>.Enumerator);
			_003C_003E7__wrap4 = null;
			_003C_003E7__wrap5 = null;
			_003CdirForBundleWithFolderName_003E5__8 = null;
			_003CdirForBundleWithPackageId_003E5__9 = null;
			_003CvalidExtensions_003E5__11 = null;
			_003CassetBundle_003E5__13 = null;
			_003CpathWithoutExtWithPackageId_003E5__14 = null;
			_003C_003E7__wrap14 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				T[] array;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!UnityData.IsInMainThread)
					{
						Log.Error("Tried to get all resources in a folder \"" + folderPath + "\" from a different thread. All resources must be loaded in the main thread.");
						return false;
					}
					_003C_003E7__wrap3 = LoadedModManager.RunningModsListForReading.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00e8;
				case 1:
					_003C_003E1__state = -4;
					goto IL_00ce;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E7__wrap6++;
					goto IL_01c9;
				case 3:
					_003C_003E1__state = -5;
					goto IL_04ca;
				case 4:
					{
						_003C_003E1__state = -6;
						goto IL_0588;
					}
					IL_0588:
					while (_003C_003E7__wrap14.MoveNext())
					{
						string current = _003C_003E7__wrap14.Current;
						if (_003CvalidExtensions_003E5__11.Contains<string>(Path.GetExtension(current)))
						{
							_003C_003E2__current = (T)(object)_003CassetBundle_003E5__13.LoadAsset(current, typeof(T));
							_003C_003E1__state = 4;
							return true;
						}
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap14 = null;
					_003CassetBundle_003E5__13 = null;
					_003CpathWithoutExtWithPackageId_003E5__14 = null;
					goto IL_05b0;
					IL_04ca:
					while (_003C_003E7__wrap14.MoveNext())
					{
						string current2 = _003C_003E7__wrap14.Current;
						if (_003CvalidExtensions_003E5__11.Contains<string>(Path.GetExtension(current2)))
						{
							_003C_003E2__current = (T)(object)_003CassetBundle_003E5__13.LoadAsset(current2, typeof(T));
							_003C_003E1__state = 3;
							return true;
						}
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap14 = null;
					if (_003CcanLoadViaPackageId_003E5__10)
					{
						IEnumerable<string> byPrefix = _003Cmods_003E5__2[_003C_003E7__wrap6].AllAssetNamesInBundleTrie(_003Cj_003E5__12).GetByPrefix(_003CpathWithoutExtWithPackageId_003E5__14);
						_003C_003E7__wrap14 = byPrefix.GetEnumerator();
						_003C_003E1__state = -6;
						goto IL_0588;
					}
					goto IL_05b0;
					IL_00e8:
					if (_003C_003E7__wrap3.MoveNext())
					{
						ModContentPack current3 = _003C_003E7__wrap3.Current;
						_003C_003E7__wrap4 = current3.GetContentHolder<T>().GetAllUnderPath(folderPath).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00ce;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap3 = default(List<ModContentPack>.Enumerator);
					array = null;
					if (typeof(T) == typeof(Texture2D))
					{
						array = (T[])(object)Resources.LoadAll<Texture2D>(GenFilePaths.ContentPath<Texture2D>() + folderPath);
					}
					if (typeof(T) == typeof(AudioClip))
					{
						array = (T[])(object)Resources.LoadAll<AudioClip>(GenFilePaths.ContentPath<AudioClip>() + folderPath);
					}
					if (array != null)
					{
						_003C_003E7__wrap5 = array;
						_003C_003E7__wrap6 = 0;
						goto IL_01c9;
					}
					goto IL_01e0;
					IL_01e0:
					_003Cmods_003E5__2 = LoadedModManager.RunningModsListForReading;
					_003CmodsDir_003E5__3 = Path.Combine("Assets", "Data");
					if (!(typeof(T) == typeof(Texture2D)) && !(typeof(T) == typeof(AudioClip)))
					{
						return false;
					}
					_003C_003E7__wrap6 = _003Cmods_003E5__2.Count - 1;
					goto IL_0614;
					IL_05b0:
					_003Cj_003E5__12++;
					goto IL_05c2;
					IL_05c2:
					if (_003Cj_003E5__12 < _003Cmods_003E5__2[_003C_003E7__wrap6].assetBundles.loadedAssetBundles.Count)
					{
						_003CassetBundle_003E5__13 = _003Cmods_003E5__2[_003C_003E7__wrap6].assetBundles.loadedAssetBundles[_003Cj_003E5__12];
						string text = Path.Combine(Path.Combine(_003CdirForBundleWithFolderName_003E5__8, GenFilePaths.ContentPath<T>()).Replace('\\', '/'), folderPath).ToLower();
						_003CpathWithoutExtWithPackageId_003E5__14 = Path.Combine(Path.Combine(_003CdirForBundleWithPackageId_003E5__9, GenFilePaths.ContentPath<T>()).Replace('\\', '/'), folderPath).ToLower();
						string text2 = text;
						if (text2[text2.Length - 1] != '/')
						{
							text += "/";
						}
						string text3 = _003CpathWithoutExtWithPackageId_003E5__14;
						if (text3[text3.Length - 1] != '/')
						{
							_003CpathWithoutExtWithPackageId_003E5__14 += "/";
						}
						IEnumerable<string> byPrefix2 = _003Cmods_003E5__2[_003C_003E7__wrap6].AllAssetNamesInBundleTrie(_003Cj_003E5__12).GetByPrefix(text);
						_003C_003E7__wrap14 = byPrefix2.GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_04ca;
					}
					_003CdirForBundleWithFolderName_003E5__8 = null;
					_003CdirForBundleWithPackageId_003E5__9 = null;
					_003CvalidExtensions_003E5__11 = null;
					_003C_003E7__wrap6--;
					goto IL_0614;
					IL_0614:
					if (_003C_003E7__wrap6 >= 0)
					{
						_003CdirForBundleWithFolderName_003E5__8 = Path.Combine(_003CmodsDir_003E5__3, _003Cmods_003E5__2[_003C_003E7__wrap6].FolderName);
						_003CdirForBundleWithPackageId_003E5__9 = Path.Combine(_003CmodsDir_003E5__3, _003Cmods_003E5__2[_003C_003E7__wrap6].PackageIdPlayerFacing);
						_003CcanLoadViaPackageId_003E5__10 = !_003Cmods_003E5__2[_003C_003E7__wrap6].IsOfficialMod;
						if (typeof(T) == typeof(Texture2D))
						{
							_003CvalidExtensions_003E5__11 = ModAssetBundlesHandler.TextureExtensions;
						}
						else
						{
							if (!(typeof(T) == typeof(AudioClip)))
							{
								throw new NotSupportedException($"Unsupported type {typeof(T)} for asset bundle loading. Did you forget to add a new type?");
							}
							_003CvalidExtensions_003E5__11 = ModAssetBundlesHandler.AudioClipExtensions;
						}
						_003Cj_003E5__12 = 0;
						goto IL_05c2;
					}
					return false;
					IL_00ce:
					if (_003C_003E7__wrap4.MoveNext())
					{
						T current4 = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap4 = null;
					goto IL_00e8;
					IL_01c9:
					if (_003C_003E7__wrap6 < _003C_003E7__wrap5.Length)
					{
						T val = _003C_003E7__wrap5[_003C_003E7__wrap6];
						_003C_003E2__current = val;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E7__wrap5 = null;
					goto IL_01e0;
				}
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
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap14 != null)
			{
				_003C_003E7__wrap14.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap14 != null)
			{
				_003C_003E7__wrap14.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CGetAllInFolder_003Ed__6 _003CGetAllInFolder_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetAllInFolder_003Ed__ = this;
			}
			else
			{
				_003CGetAllInFolder_003Ed__ = new _003CGetAllInFolder_003Ed__6(0);
			}
			_003CGetAllInFolder_003Ed__.folderPath = _003C_003E3__folderPath;
			return _003CGetAllInFolder_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	private static readonly Dictionary<string, T> bundleAssetCache = new Dictionary<string, T>();

	private static int bundleAssetCacheStamp = -1;

	public static T Get(string itemPath, bool reportFailure = true)
	{
		if (!UnityData.IsInMainThread)
		{
			Log.Error("Tried to get a resource \"" + itemPath + "\" from a different thread. All resources must be loaded in the main thread.");
			return null;
		}
		T val = null;
		if (typeof(T) != typeof(Shader) && ModContentHolder<T>.anyReloaded)
		{
			List<ModContentPack> runningModsListForReading = LoadedModManager.RunningModsListForReading;
			for (int num = runningModsListForReading.Count - 1; num >= 0; num--)
			{
				ModContentHolder<T> contentHolder = runningModsListForReading[num].GetContentHolder<T>();
				if (contentHolder.contentList.Count != 0)
				{
					val = contentHolder.Get(itemPath);
					if (val != null)
					{
						return val;
					}
				}
			}
		}
		if (typeof(T) == typeof(Texture2D))
		{
			val = (T)(object)Resources.Load<Texture2D>(GenFilePaths.ContentPath<Texture2D>() + itemPath);
		}
		if (typeof(T) == typeof(AudioClip))
		{
			val = (T)(object)Resources.Load<AudioClip>(GenFilePaths.ContentPath<AudioClip>() + itemPath);
		}
		if (val != null)
		{
			return val;
		}
		if (typeof(T) == typeof(Texture2D) || typeof(T) == typeof(AudioClip) || typeof(T) == typeof(Shader))
		{
			T val2 = TryFindAssetInModBundles(itemPath);
			if (val2 != null)
			{
				return val2;
			}
		}
		if (reportFailure)
		{
			string value = ((ContentFinderRequester.requester != null) ? (" for def '" + ContentFinderRequester.requester.defName + "'") : "");
			Log.Error($"Could not load {typeof(T).Name} at '{itemPath}'{value} in any active mod or in base resources.");
		}
		return null;
	}

	public static T TryFindAssetInModBundles(string itemPath)
	{
		if (!UnityData.IsInMainThread)
		{
			return TryFindAssetInModBundlesUncached(itemPath);
		}
		int num = LoadedAssetBundlesStamp();
		if (num != bundleAssetCacheStamp)
		{
			bundleAssetCache.Clear();
			bundleAssetCacheStamp = num;
		}
		if (bundleAssetCache.TryGetValue(itemPath, out var value) && (value == null || (value is UnityEngine.Object @object && @object != null)))
		{
			return value;
		}
		T val = TryFindAssetInModBundlesUncached(itemPath);
		bundleAssetCache[itemPath] = val;
		return val;
	}

	private static int LoadedAssetBundlesStamp()
	{
		List<ModContentPack> runningModsListForReading = LoadedModManager.RunningModsListForReading;
		int num = runningModsListForReading.Count;
		for (int i = 0; i < runningModsListForReading.Count; i++)
		{
			num = num * 31 + runningModsListForReading[i].assetBundles.loadedAssetBundles.Count;
		}
		return num;
	}

	private static T TryFindAssetInModBundlesUncached(string itemPath)
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
				throw new NotSupportedException($"Unsupported type {typeof(T)} for asset bundle loading. Did you forget to add a new type?");
			}
			array = ModAssetBundlesHandler.ShaderExtensions;
		}
		List<ModContentPack> runningModsListForReading = LoadedModManager.RunningModsListForReading;
		string path = Path.Combine("Assets", "Data");
		for (int num = runningModsListForReading.Count - 1; num >= 0; num--)
		{
			string path2 = Path.Combine(path, runningModsListForReading[num].FolderName);
			string path3 = Path.Combine(path, runningModsListForReading[num].PackageIdPlayerFacing);
			bool flag = !runningModsListForReading[num].IsOfficialMod;
			foreach (AssetBundle loadedAssetBundle in runningModsListForReading[num].assetBundles.loadedAssetBundles)
			{
				string text = Path.Combine(Path.Combine(path2, GenFilePaths.ContentPath<T>()), itemPath);
				string text2 = Path.Combine(Path.Combine(path3, GenFilePaths.ContentPath<T>()), itemPath);
				string[] array2 = array;
				foreach (string text3 in array2)
				{
					if (loadedAssetBundle.LoadAsset(text + text3, typeof(T)) is T result)
					{
						return result;
					}
					if (flag && loadedAssetBundle.LoadAsset(text2 + text3, typeof(T)) is T result2)
					{
						return result2;
					}
				}
			}
		}
		return null;
	}

	[IteratorStateMachine(typeof(ContentFinder<>._003CGetAllInFolder_003Ed__6))]
	public static IEnumerable<T> GetAllInFolder(string folderPath)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetAllInFolder_003Ed__6(-2)
		{
			_003C_003E3__folderPath = folderPath
		};
	}
}
