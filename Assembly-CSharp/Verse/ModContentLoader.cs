using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.IO;
using UnityEngine;
using UnityStubs;

namespace Verse;

public static class ModContentLoader<T> where T : class
{
	[CompilerGenerated]
	private sealed class _003CLoadAllForMod_003Ed__4 : IEnumerable<Pair<string, LoadedContentItem<T>>>, IEnumerable, IEnumerator<Pair<string, LoadedContentItem<T>>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pair<string, LoadedContentItem<T>> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ModContentPack mod;

		public ModContentPack _003C_003E3__mod;

		private HashSet<string> _003CddsFiles_003E5__2;

		private Dictionary<string, FileInfo>.Enumerator _003C_003E7__wrap2;

		Pair<string, LoadedContentItem<T>> IEnumerator<Pair<string, LoadedContentItem<T>>>.Current
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
		public _003CLoadAllForMod_003Ed__4(int _003C_003E1__state)
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
			_003CddsFiles_003E5__2 = null;
			_003C_003E7__wrap2 = default(Dictionary<string, FileInfo>.Enumerator);
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
				{
					_003C_003E1__state = -1;
					DeepProfiler.Start("Loading assets of type " + typeof(T)?.ToString() + " for mod " + mod);
					if (!ModContentLoader<T>.prefetchScans.TryRemove(mod, out var value))
					{
						value = ModContentPack.GetAllFilesForMod(mod, GenFilePaths.ContentPath<T>(), ModContentLoader<T>.IsAcceptableExtension);
					}
					_003CddsFiles_003E5__2 = (from k in value.Keys
						select k.ToLowerInvariant() into k
						where k.EndsWith(".dds")
						select k).ToHashSet();
					_003C_003E7__wrap2 = value.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					var (text2, fileInfo2) = (KeyValuePair<string, FileInfo>)(ref _003C_003E7__wrap2.Current);
					if (!ModContentLoader<T>.ShadowedByDds(text2, _003CddsFiles_003E5__2))
					{
						LoadedContentItem<T> loadedContentItem = ModContentLoader<T>.LoadItem((VirtualFile)(FilesystemFile)fileInfo2);
						if (loadedContentItem != null)
						{
							_003C_003E2__current = new Pair<string, LoadedContentItem<T>>(text2, loadedContentItem);
							_003C_003E1__state = 1;
							return true;
						}
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = default(Dictionary<string, FileInfo>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pair<string, LoadedContentItem<T>>> IEnumerable<Pair<string, LoadedContentItem<T>>>.GetEnumerator()
		{
			_003CLoadAllForMod_003Ed__4 _003CLoadAllForMod_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CLoadAllForMod_003Ed__ = this;
			}
			else
			{
				_003CLoadAllForMod_003Ed__ = new _003CLoadAllForMod_003Ed__4(0);
			}
			_003CLoadAllForMod_003Ed__.mod = _003C_003E3__mod;
			return _003CLoadAllForMod_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pair<string, LoadedContentItem<T>>>)this).GetEnumerator();
		}
	}

	private static string[] AcceptableExtensionsAudio = new string[7] { ".wav", ".mp3", ".ogg", ".xm", ".it", ".mod", ".s3m" };

	private static string[] AcceptableExtensionsTexture = new string[5] { ".png", ".jpg", ".jpeg", ".psd", ".dds" };

	private static string[] AcceptableExtensionsString = new string[1] { ".txt" };

	private static readonly ConcurrentDictionary<ModContentPack, Dictionary<string, FileInfo>> prefetchScans = new ConcurrentDictionary<ModContentPack, Dictionary<string, FileInfo>>();

	public static bool IsAcceptableExtension(string extension)
	{
		string[] array;
		if (typeof(T) == typeof(AudioClip))
		{
			array = AcceptableExtensionsAudio;
		}
		else if (typeof(T) == typeof(Texture2D))
		{
			array = AcceptableExtensionsTexture;
		}
		else
		{
			if (!(typeof(T) == typeof(string)))
			{
				Log.Error("Unknown content type " + typeof(T));
				return false;
			}
			array = AcceptableExtensionsString;
		}
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (extension.ToLower() == text)
			{
				return true;
			}
		}
		return false;
	}

	[IteratorStateMachine(typeof(ModContentLoader<>._003CLoadAllForMod_003Ed__4))]
	public static IEnumerable<Pair<string, LoadedContentItem<T>>> LoadAllForMod(ModContentPack mod)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CLoadAllForMod_003Ed__4(-2)
		{
			_003C_003E3__mod = mod
		};
	}

	private static bool ShadowedByDds(string filename, HashSet<string> ddsFiles)
	{
		if (typeof(T) == typeof(Texture2D) && filename.Length > 4)
		{
			if (!filename.Substring(filename.Length - 4).Equals(".dds", StringComparison.OrdinalIgnoreCase))
			{
				string text = filename.ToLowerInvariant();
				return ddsFiles.Contains(text.Substring(0, text.Length - 4) + ".dds");
			}
		}
		return false;
	}

	public static List<string> PrefetchPaths(ModContentPack mod)
	{
		Dictionary<string, FileInfo> allFilesForMod = ModContentPack.GetAllFilesForMod(mod, GenFilePaths.ContentPath<T>(), IsAcceptableExtension);
		prefetchScans[mod] = allFilesForMod;
		HashSet<string> ddsFiles = (from k in allFilesForMod.Keys
			select k.ToLowerInvariant() into k
			where k.EndsWith(".dds")
			select k).ToHashSet();
		List<string> list = new List<string>(allFilesForMod.Count);
		foreach (var (filename, fileInfo2) in allFilesForMod)
		{
			if (!ShadowedByDds(filename, ddsFiles))
			{
				list.Add(fileInfo2.FullName);
			}
		}
		return list;
	}

	public static LoadedContentItem<T> LoadItem(VirtualFile file)
	{
		try
		{
			if (typeof(T) == typeof(string))
			{
				return new LoadedContentItem<T>(file, (T)(object)file.ReadAllText());
			}
			if (typeof(T) == typeof(Texture2D))
			{
				return new LoadedContentItem<T>(file, (T)(object)LoadTexture(file));
			}
			if (typeof(T) == typeof(AudioClip))
			{
				T val = (T)(object)AudioClip.LoadFromFile(file.FullPath);
				UnityEngine.Object @object = val as UnityEngine.Object;
				if (@object != null)
				{
					@object.name = Path.GetFileNameWithoutExtension(file.Name);
				}
				return new LoadedContentItem<T>(file, val);
			}
		}
		catch (Exception value)
		{
			Log.Error($"Exception loading {typeof(T)} from file.\nabsFilePath: {file.FullPath}\nException: {value}");
		}
		if (typeof(T) == typeof(Texture2D))
		{
			return (LoadedContentItem<T>)(object)new LoadedContentItem<Texture2D>(file, BaseContent.BadTex);
		}
		return null;
	}

	private static Texture2D LoadTexture(VirtualFile file)
	{
		if ((file is FilesystemFile filesystemFile) ? (!ContentPacks.FileExists(filesystemFile.FullPath)) : (!file.Exists))
		{
			return null;
		}
		if (file.Name.EndsWith(".dds", StringComparison.OrdinalIgnoreCase))
		{
			return ModDdsLoader.TryLoadDds(file);
		}
		Texture2D texture2D = LoadTextureViaImageConversion(file);
		texture2D.name = Path.GetFileNameWithoutExtension(file.Name);
		return texture2D;
	}

	private static Texture2D LoadTextureViaImageConversion(VirtualFile file)
	{
		Texture2D texture2D;
		if (file is FilesystemFile filesystemFile)
		{
			texture2D = Texture2D.LoadFromFile(filesystemFile.FullPath);
		}
		else
		{
			texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, mipChain: true);
			texture2D.LoadImage(file.ReadAllBytes());
		}
		if ((texture2D.width < 4 || texture2D.height < 4 || !Mathf.IsPowerOfTwo(texture2D.width) || !Mathf.IsPowerOfTwo(texture2D.height)) && Prefs.TextureCompression)
		{
			int num = StaticTextureAtlas.CalculateMaxMipmapsForDxtSupport(texture2D);
			if (Prefs.LogVerbose)
			{
				Log.Warning($"Texture {file.Name} is being reloaded with reduced mipmap count (clamped to {num}) due to non-power-of-two dimensions: ({texture2D.width}x{texture2D.height}). This will be slower to load, and will look worse when zoomed out. Consider using a power-of-two texture size instead.");
			}
			if (!UnityData.ComputeShadersSupported && !(file is FilesystemFile))
			{
				Texture2D texture2D2 = new Texture2D(texture2D.width, texture2D.height, TextureFormat.Alpha8, num, linear: false);
				UnityEngine.Object.DestroyImmediate(texture2D);
				texture2D = texture2D2;
				texture2D.LoadImage(file.ReadAllBytes());
			}
		}
		bool flag = true;
		if (Prefs.TextureCompression && flag)
		{
			if (!UnityData.ComputeShadersSupported)
			{
				texture2D.Compress(_: true);
				texture2D.filterMode = FilterMode.Trilinear;
				texture2D.anisoLevel = 2;
				texture2D.Apply(updateMipmaps: true, makeNoLongerReadable: true);
			}
			else
			{
				texture2D.filterMode = FilterMode.Trilinear;
				texture2D.anisoLevel = 2;
				texture2D.Apply(updateMipmaps: true, makeNoLongerReadable: true);
				texture2D = StaticTextureAtlas.FastCompressDXT(texture2D, deleteOriginal: true);
			}
		}
		else
		{
			texture2D.filterMode = FilterMode.Trilinear;
			texture2D.anisoLevel = 2;
			texture2D.Apply(updateMipmaps: true, makeNoLongerReadable: true);
		}
		return texture2D;
	}
}
