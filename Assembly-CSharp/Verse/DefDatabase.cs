using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001EB RID: 491
	public static class DefDatabase<T> where T : Def
	{
		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000D30 RID: 3376 RVA: 0x0003E695 File Offset: 0x0003C895
		public static IEnumerable<T> AllDefs
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x0003E695 File Offset: 0x0003C895
		public static List<T> AllDefsListForReading
		{
			get
			{
				return DefDatabase<T>.defsList;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000D32 RID: 3378 RVA: 0x0003E69C File Offset: 0x0003C89C
		public static int DefCount
		{
			get
			{
				return DefDatabase<T>.defsList.Count;
			}
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0003E6A8 File Offset: 0x0003C8A8
		public static void AddAllInMods()
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ModContentPack modContentPack in (from m in LoadedModManager.RunningMods
			orderby m.OverwritePriority
			select m).ThenBy((ModContentPack x) => LoadedModManager.RunningModsListForReading.IndexOf(x)))
			{
				string sourceName = modContentPack.ToString();
				hashSet.Clear();
				foreach (T t in GenDefDatabase.DefsToGoInDatabase<T>(modContentPack))
				{
					if (!hashSet.Add(t.defName))
					{
						string[] array = new string[7];
						array[0] = "Mod ";
						int num = 1;
						ModContentPack modContentPack2 = modContentPack;
						array[num] = ((modContentPack2 != null) ? modContentPack2.ToString() : null);
						array[2] = " has multiple ";
						int num2 = 3;
						Type typeFromHandle = typeof(T);
						array[num2] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
						array[4] = "s named ";
						array[5] = t.defName;
						array[6] = ". Skipping.";
						Log.Error(string.Concat(array));
					}
					else
					{
						DefDatabase<T>.<AddAllInMods>g__AddDef|9_0(t, sourceName);
					}
				}
			}
			foreach (!0 def in LoadedModManager.PatchedDefsForReading.OfType<T>())
			{
				DefDatabase<T>.<AddAllInMods>g__AddDef|9_0(def, "Patches");
			}
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0003E85C File Offset: 0x0003CA5C
		public static void Add(IEnumerable<T> defs)
		{
			foreach (!0 def in defs)
			{
				DefDatabase<T>.Add(def);
			}
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0003E8A4 File Offset: 0x0003CAA4
		public static void Add(T def)
		{
			if (def == null)
			{
				Log.Error("Tried to add null def to DefDatabase.");
				return;
			}
			while (DefDatabase<T>.defsByName.ContainsKey(def.defName))
			{
				string str = "Adding duplicate ";
				Type typeFromHandle = typeof(T);
				Log.Error(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " name: " + def.defName);
				T t = def;
				t.defName += Mathf.RoundToInt(Rand.Value * 1000f).ToString();
				def.ResolveDefNameHash();
			}
			DefDatabase<T>.defsList.Add(def);
			DefDatabase<T>.defsByName.Add(def.defName, def);
			if (DefDatabase<T>.defsList.Count > 65535)
			{
				string str2 = "Too many ";
				Type typeFromHandle2 = typeof(T);
				Log.Error(str2 + ((typeFromHandle2 != null) ? typeFromHandle2.ToString() : null) + "; over " + ushort.MaxValue.ToString());
			}
			def.index = (ushort)(DefDatabase<T>.defsList.Count - 1);
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x0003E9CD File Offset: 0x0003CBCD
		private static void Remove(T def)
		{
			DefDatabase<T>.defsByName.Remove(def.defName);
			DefDatabase<T>.defsList.Remove(def);
			DefDatabase<T>.SetIndices();
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x0003E9F6 File Offset: 0x0003CBF6
		public static void Clear()
		{
			DefDatabase<T>.defsList.Clear();
			DefDatabase<T>.defsByName.Clear();
			DefDatabase<T>.defsByShortHash.Clear();
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0003EA18 File Offset: 0x0003CC18
		public static void ClearCachedData()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].ClearCachedData();
			}
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x0003EA50 File Offset: 0x0003CC50
		public static void ResolveAllReferences(bool onlyExactlyMyType = true, bool parallel = false)
		{
			DeepProfiler.Start("SetIndices");
			try
			{
				DefDatabase<T>.SetIndices();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ResolveAllReferences " + typeof(T).FullName);
			try
			{
				Action<T> action = delegate(T def)
				{
					if (onlyExactlyMyType && def.GetType() != typeof(T))
					{
						return;
					}
					DeepProfiler.Start("Resolver call");
					try
					{
						def.ResolveReferences();
					}
					catch (Exception ex)
					{
						string str = "Error while resolving references for def ";
						T t = def;
						string str2 = (t != null) ? t.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
					finally
					{
						DeepProfiler.End();
					}
				};
				if (parallel)
				{
					GenThreading.ParallelForEach<T>(DefDatabase<T>.defsList, action, -1);
				}
				else
				{
					for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
					{
						action(DefDatabase<T>.defsList[i]);
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("SetIndices");
			try
			{
				DefDatabase<T>.SetIndices();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0003EB2C File Offset: 0x0003CD2C
		private static void SetIndices()
		{
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsList[i].index = (ushort)i;
			}
			for (int j = 0; j < DefDatabase<T>.defsList.Count; j++)
			{
				DefDatabase<T>.defsList[j].PostSetIndices();
			}
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0003EB90 File Offset: 0x0003CD90
		public static void ErrorCheckAllDefs()
		{
			foreach (T t in DefDatabase<T>.AllDefs)
			{
				try
				{
					if (!t.ignoreConfigErrors)
					{
						foreach (string str in t.ConfigErrors())
						{
							string str2 = "Config error in ";
							T t2 = t;
							Log.Error(str2 + ((t2 != null) ? t2.ToString() : null) + ": " + str);
						}
					}
				}
				catch (Exception ex)
				{
					string str3 = "Exception in ConfigErrors() of ";
					string defName = t.defName;
					string str4 = ": ";
					Exception ex2 = ex;
					Log.Error(str3 + defName + str4 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x0003EC88 File Offset: 0x0003CE88
		public static T GetNamed(string defName, bool errorOnFail = true)
		{
			if (errorOnFail)
			{
				T result;
				if (DefDatabase<T>.defsByName.TryGetValue(defName, out result))
				{
					return result;
				}
				string[] array = new string[7];
				array[0] = "Failed to find ";
				int num = 1;
				Type typeFromHandle = typeof(T);
				array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
				array[2] = " named ";
				array[3] = defName;
				array[4] = ". There are ";
				array[5] = DefDatabase<T>.defsList.Count.ToString();
				array[6] = " defs of this type loaded.";
				Log.Error(string.Concat(array));
				return default(T);
			}
			else
			{
				T result2;
				if (DefDatabase<T>.defsByName.TryGetValue(defName, out result2))
				{
					return result2;
				}
				return default(T);
			}
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0003ED2F File Offset: 0x0003CF2F
		public static T GetNamedSilentFail(string defName)
		{
			return DefDatabase<T>.GetNamed(defName, false);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0003ED38 File Offset: 0x0003CF38
		public static T GetByShortHash(ushort shortHash)
		{
			T result;
			if (DefDatabase<T>.defsByShortHash.TryGetValue(shortHash, out result))
			{
				return result;
			}
			return default(T);
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0003ED60 File Offset: 0x0003CF60
		public static void InitializeShortHashDictionary()
		{
			DefDatabase<T>.defsByShortHash.EnsureCapacity(DefDatabase<T>.defsList.Count);
			for (int i = 0; i < DefDatabase<T>.defsList.Count; i++)
			{
				DefDatabase<T>.defsByShortHash[DefDatabase<T>.defsList[i].shortHash] = DefDatabase<T>.defsList[i];
			}
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0003EDC1 File Offset: 0x0003CFC1
		public static T GetRandom()
		{
			return DefDatabase<T>.defsList.RandomElement<T>();
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0003EDF0 File Offset: 0x0003CFF0
		[CompilerGenerated]
		internal static void <AddAllInMods>g__AddDef|9_0(T def, string sourceName)
		{
			if (def.defName == "UnnamedDef")
			{
				string text = "Unnamed" + typeof(T).Name + Rand.Range(1, 100000).ToString() + "A";
				Log.Error(string.Concat(new string[]
				{
					typeof(T).Name,
					" in ",
					sourceName,
					" with label ",
					def.label,
					" lacks a defName. Giving name ",
					text
				}));
				def.defName = text;
				def.ResolveDefNameHash();
			}
			T def2;
			if (DefDatabase<T>.defsByName.TryGetValue(def.defName, out def2))
			{
				DefDatabase<T>.Remove(def2);
			}
			DefDatabase<T>.Add(def);
		}

		// Token: 0x040007A9 RID: 1961
		private static readonly List<T> defsList = new List<T>();

		// Token: 0x040007AA RID: 1962
		private static readonly Dictionary<string, T> defsByName = new Dictionary<string, T>();

		// Token: 0x040007AB RID: 1963
		private static readonly Dictionary<ushort, T> defsByShortHash = new Dictionary<ushort, T>();
	}
}
