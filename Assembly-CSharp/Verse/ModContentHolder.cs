using System;
using System.Collections.Generic;
using System.IO;
using KTrie;
using RimWorld.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200056A RID: 1386
	public class ModContentHolder<T> where T : class
	{
		// Token: 0x060023F7 RID: 9207 RVA: 0x000BAE30 File Offset: 0x000B9030
		public ModContentHolder(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x000BAE60 File Offset: 0x000B9060
		public void ClearDestroy()
		{
			if (typeof(UnityEngine.Object).IsAssignableFrom(typeof(T)))
			{
				foreach (T localObj2 in this.contentList.Values)
				{
					T localObj = localObj2;
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						UnityEngine.Object.Destroy((UnityEngine.Object)((object)localObj));
					});
				}
			}
			for (int i = 0; i < this.extraDisposables.Count; i++)
			{
				this.extraDisposables[i].Dispose();
			}
			this.extraDisposables.Clear();
			this.contentList.Clear();
			this.contentListTrie.Clear();
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x000BAF30 File Offset: 0x000B9130
		public void ReloadAll(bool hotReload = false)
		{
			foreach (Pair<string, LoadedContentItem<T>> pair in ModContentLoader<T>.LoadAllForMod(this.mod))
			{
				string text = pair.First;
				text = text.Replace('\\', '/');
				string text2 = GenFilePaths.ContentPath<T>();
				if (text.StartsWith(text2))
				{
					text = text.Substring(text2.Length);
				}
				if (text.EndsWith(Path.GetExtension(text)))
				{
					text = text.Substring(0, text.Length - Path.GetExtension(text).Length);
				}
				if (this.contentList.ContainsKey(text))
				{
					if (!hotReload)
					{
						string[] array = new string[6];
						array[0] = "Tried to load duplicate ";
						int num = 1;
						Type typeFromHandle = typeof(T);
						array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
						array[2] = " with path: ";
						int num2 = 3;
						VirtualFile internalFile = pair.Second.internalFile;
						array[num2] = ((internalFile != null) ? internalFile.ToString() : null);
						array[4] = " and internal path: ";
						array[5] = text;
						Log.Warning(string.Concat(array));
					}
				}
				else
				{
					this.contentList.Add(text, pair.Second.contentItem);
					this.contentListTrie.Add(text);
					if (pair.Second.extraDisposable != null)
					{
						this.extraDisposables.Add(pair.Second.extraDisposable);
					}
				}
			}
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x000BB0A4 File Offset: 0x000B92A4
		public T Get(string path)
		{
			T result;
			if (this.contentList.TryGetValue(path, out result))
			{
				return result;
			}
			return default(T);
		}

		// Token: 0x060023FB RID: 9211 RVA: 0x000BB0CC File Offset: 0x000B92CC
		public IEnumerable<T> GetAllUnderPath(string pathRoot)
		{
			string prefix = (pathRoot.NullOrEmpty() || pathRoot[pathRoot.Length - 1] == '/') ? pathRoot : (pathRoot + "/");
			foreach (string key in this.contentListTrie.GetByPrefix(prefix))
			{
				yield return this.contentList[key];
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04001DCA RID: 7626
		private ModContentPack mod;

		// Token: 0x04001DCB RID: 7627
		public Dictionary<string, T> contentList = new Dictionary<string, T>();

		// Token: 0x04001DCC RID: 7628
		public List<IDisposable> extraDisposables = new List<IDisposable>();

		// Token: 0x04001DCD RID: 7629
		private StringTrieSet contentListTrie = new StringTrieSet();
	}
}
