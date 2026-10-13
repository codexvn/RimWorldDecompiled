using System;
using System.Collections.Generic;
using System.IO;
using RimWorld.IO;
using RuntimeAudioClipLoader;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200056E RID: 1390
	public static class ModContentLoader<T> where T : class
	{
		// Token: 0x06002408 RID: 9224 RVA: 0x000BB2F4 File Offset: 0x000B94F4
		public static bool IsAcceptableExtension(string extension)
		{
			string[] array;
			if (typeof(T) == typeof(AudioClip))
			{
				array = ModContentLoader<T>.AcceptableExtensionsAudio;
			}
			else if (typeof(T) == typeof(Texture2D))
			{
				array = ModContentLoader<T>.AcceptableExtensionsTexture;
			}
			else
			{
				if (!(typeof(T) == typeof(string)))
				{
					string str = "Unknown content type ";
					Type typeFromHandle = typeof(T);
					Log.Error(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
					return false;
				}
				array = ModContentLoader<T>.AcceptableExtensionsString;
			}
			foreach (string b in array)
			{
				if (extension.ToLower() == b)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x000BB3B6 File Offset: 0x000B95B6
		public static IEnumerable<Pair<string, LoadedContentItem<T>>> LoadAllForMod(ModContentPack mod)
		{
			string str = "Loading assets of type ";
			Type typeFromHandle = typeof(T);
			DeepProfiler.Start(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " for mod " + ((mod != null) ? mod.ToString() : null));
			Dictionary<string, FileInfo> allFilesForMod = ModContentPack.GetAllFilesForMod(mod, GenFilePaths.ContentPath<T>(), new Func<string, bool>(ModContentLoader<T>.IsAcceptableExtension), null);
			foreach (KeyValuePair<string, FileInfo> keyValuePair in allFilesForMod)
			{
				LoadedContentItem<T> loadedContentItem = ModContentLoader<T>.LoadItem(keyValuePair.Value);
				if (loadedContentItem != null)
				{
					yield return new Pair<string, LoadedContentItem<T>>(keyValuePair.Key, loadedContentItem);
				}
			}
			Dictionary<string, FileInfo>.Enumerator enumerator = default(Dictionary<string, FileInfo>.Enumerator);
			DeepProfiler.End();
			yield break;
			yield break;
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x000BB3C8 File Offset: 0x000B95C8
		public static LoadedContentItem<T> LoadItem(VirtualFile file)
		{
			try
			{
				if (typeof(T) == typeof(string))
				{
					return new LoadedContentItem<T>(file, (T)((object)file.ReadAllText()), null);
				}
				if (typeof(T) == typeof(Texture2D))
				{
					return new LoadedContentItem<T>(file, (T)((object)ModContentLoader<T>.LoadTexture(file)), null);
				}
				if (typeof(T) == typeof(AudioClip))
				{
					bool doStream = ModContentLoader<T>.ShouldStreamAudioClipFromFile(file);
					Stream stream = file.CreateReadStream();
					T t;
					try
					{
						t = (T)((object)Manager.Load(stream, ModContentLoader<T>.GetFormat(file.Name), file.Name, doStream, true, true));
					}
					catch (Exception)
					{
						stream.Dispose();
						throw;
					}
					IDisposable extraDisposable = stream;
					UnityEngine.Object @object = t as UnityEngine.Object;
					if (@object != null)
					{
						@object.name = Path.GetFileNameWithoutExtension(file.Name);
					}
					return new LoadedContentItem<T>(file, t, extraDisposable);
				}
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("Exception loading {0} from file.\nabsFilePath: {1}\nException: {2}", typeof(T), file.FullPath, arg));
			}
			if (typeof(T) == typeof(Texture2D))
			{
				return (LoadedContentItem<T>)new LoadedContentItem<Texture2D>(file, BaseContent.BadTex, null);
			}
			return null;
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x000BB544 File Offset: 0x000B9744
		private static AudioFormat GetFormat(string filename)
		{
			string extension = Path.GetExtension(filename);
			if (extension == ".ogg")
			{
				return AudioFormat.ogg;
			}
			if (extension == ".mp3")
			{
				return AudioFormat.mp3;
			}
			if (extension == ".aiff" || extension == ".aif" || extension == ".aifc")
			{
				return AudioFormat.aiff;
			}
			if (!(extension == ".wav"))
			{
				return AudioFormat.unknown;
			}
			return AudioFormat.wav;
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x000BB5B1 File Offset: 0x000B97B1
		private static AudioType GetAudioTypeFromURI(string uri)
		{
			if (uri.EndsWith(".ogg"))
			{
				return AudioType.OGGVORBIS;
			}
			return AudioType.WAV;
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x000BB5C5 File Offset: 0x000B97C5
		private static bool ShouldStreamAudioClipFromFile(VirtualFile file)
		{
			return file is FilesystemFile && file.Exists && file.Length > 307200L;
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x000BB5E8 File Offset: 0x000B97E8
		private static Texture2D LoadTexture(VirtualFile file)
		{
			if (!file.Exists)
			{
				return null;
			}
			byte[] data = file.ReadAllBytes();
			Texture2D texture2D = new Texture2D(2, 2, TextureFormat.Alpha8, true);
			texture2D.LoadImage(data);
			if ((texture2D.width < 4 || texture2D.height < 4 || !Mathf.IsPowerOfTwo(texture2D.width) || !Mathf.IsPowerOfTwo(texture2D.height)) && Prefs.TextureCompression)
			{
				int num = StaticTextureAtlas.CalculateMaxMipmapsForDxtSupport(texture2D);
				if (Prefs.LogVerbose)
				{
					Log.Warning(string.Format("Texture {0} is being reloaded with reduced mipmap count (clamped to {1}) due to non-power-of-two dimensions: ({2}x{3}). This will be slower to load, and will look worse when zoomed out. Consider using a power-of-two texture size instead.", new object[]
					{
						file.Name,
						num,
						texture2D.width,
						texture2D.height
					}));
				}
				if (!UnityData.ComputeShadersSupported)
				{
					Texture2D texture2D2 = new Texture2D(texture2D.width, texture2D.height, TextureFormat.Alpha8, num, false);
					UnityEngine.Object.DestroyImmediate(texture2D);
					texture2D = texture2D2;
					texture2D.LoadImage(data);
				}
			}
			bool flag = texture2D.width % 4 == 0 && texture2D.height % 4 == 0;
			if (Prefs.TextureCompression && flag)
			{
				if (!UnityData.ComputeShadersSupported)
				{
					texture2D.Compress(true);
					texture2D.filterMode = FilterMode.Trilinear;
					texture2D.anisoLevel = 2;
					texture2D.Apply(true, true);
				}
				else
				{
					texture2D.filterMode = FilterMode.Trilinear;
					texture2D.anisoLevel = 2;
					texture2D.Apply(true, true);
					texture2D = StaticTextureAtlas.FastCompressDXT(texture2D, true);
				}
			}
			else
			{
				texture2D.filterMode = FilterMode.Trilinear;
				texture2D.anisoLevel = 2;
				texture2D.Apply(true, true);
			}
			texture2D.name = Path.GetFileNameWithoutExtension(file.Name);
			return texture2D;
		}

		// Token: 0x04001DD9 RID: 7641
		private static string[] AcceptableExtensionsAudio = new string[]
		{
			".wav",
			".mp3",
			".ogg",
			".xm",
			".it",
			".mod",
			".s3m"
		};

		// Token: 0x04001DDA RID: 7642
		private static string[] AcceptableExtensionsTexture = new string[]
		{
			".png",
			".jpg",
			".jpeg",
			".psd"
		};

		// Token: 0x04001DDB RID: 7643
		private static string[] AcceptableExtensionsString = new string[]
		{
			".txt"
		};
	}
}
