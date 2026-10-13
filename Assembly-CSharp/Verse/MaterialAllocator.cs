using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000808 RID: 2056
	internal static class MaterialAllocator
	{
		// Token: 0x06003466 RID: 13414 RVA: 0x00113590 File Offset: 0x00111790
		public static Material Create(Material material)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Attempted to create duplicate material off main thread, do this in ensure materials initialized.");
				return null;
			}
			Material material2 = new Material(material);
			MaterialAllocator.references[material2] = new MaterialAllocator.MaterialInfo
			{
				stackTrace = (MaterialAllocator.trackMaterialStackTrace ? Environment.StackTrace : "(unavailable)")
			};
			MaterialAllocator.TryReport();
			return material2;
		}

		// Token: 0x06003467 RID: 13415 RVA: 0x001135EC File Offset: 0x001117EC
		public static Material Create(Shader shader)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Attempted to create material off main thread, do this in ensure materials initialized.");
				return null;
			}
			Material material = new Material(shader);
			MaterialAllocator.references[material] = new MaterialAllocator.MaterialInfo
			{
				stackTrace = (MaterialAllocator.trackMaterialStackTrace ? Environment.StackTrace : "(unavailable)")
			};
			MaterialAllocator.TryReport();
			return material;
		}

		// Token: 0x06003468 RID: 13416 RVA: 0x00113647 File Offset: 0x00111847
		public static void Destroy(Material material)
		{
			if (!MaterialAllocator.references.ContainsKey(material))
			{
				Log.Error(string.Format("Destroying material {0}, but that material was not created through the MaterialTracker", material));
			}
			MaterialAllocator.references.Remove(material);
			UnityEngine.Object.Destroy(material);
		}

		// Token: 0x06003469 RID: 13417 RVA: 0x00113678 File Offset: 0x00111878
		private static void TryReport()
		{
			if (MaterialAllocator.MaterialWarningThreshold() > MaterialAllocator.nextWarningThreshold)
			{
				MaterialAllocator.nextWarningThreshold = MaterialAllocator.MaterialWarningThreshold();
			}
			if (MaterialAllocator.references.Count > MaterialAllocator.nextWarningThreshold)
			{
				Log.Error(string.Format("Material allocator has allocated {0} materials; this may be a sign of a material leak", MaterialAllocator.references.Count));
				if (Prefs.DevMode)
				{
					MaterialAllocator.MaterialReport();
				}
				MaterialAllocator.nextWarningThreshold *= 2;
			}
		}

		// Token: 0x0600346A RID: 13418 RVA: 0x001136E2 File Offset: 0x001118E2
		public static int MaterialWarningThreshold()
		{
			return int.MaxValue;
		}

		// Token: 0x0600346B RID: 13419 RVA: 0x001136EC File Offset: 0x001118EC
		[DebugOutput("System", false)]
		public static void MaterialReport()
		{
			foreach (string text in (from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace into g
			orderby g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>() descending
			select string.Format("{0}: {1}", g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>(), g.FirstOrDefault<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>().Value.stackTrace)).Take(20))
			{
				Log.Error(text);
			}
		}

		// Token: 0x0600346C RID: 13420 RVA: 0x001137A8 File Offset: 0x001119A8
		[DebugOutput("System", false)]
		public static void MaterialSnapshot()
		{
			MaterialAllocator.snapshot = new Dictionary<string, int>();
			foreach (IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>> grouping in from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace)
			{
				MaterialAllocator.snapshot[grouping.Key] = grouping.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>();
			}
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x00113834 File Offset: 0x00111A34
		[DebugOutput("System", false)]
		public static void MaterialDelta()
		{
			IEnumerable<string> source = (from v in MaterialAllocator.references.Values
			select v.stackTrace).Concat(MaterialAllocator.snapshot.Keys).Distinct<string>();
			Dictionary<string, int> currentSnapshot = new Dictionary<string, int>();
			foreach (IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>> grouping in from kvp in MaterialAllocator.references
			group kvp by kvp.Value.stackTrace)
			{
				currentSnapshot[grouping.Key] = grouping.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>();
			}
			foreach (string text in (from k in source
			select new KeyValuePair<string, int>(k, currentSnapshot.TryGetValue(k, 0) - MaterialAllocator.snapshot.TryGetValue(k, 0)) into kvp
			orderby kvp.Value descending
			select kvp into g
			select string.Format("{0}: {1}", g.Value, g.Key)).Take(20))
			{
				Log.Error(text);
			}
		}

		// Token: 0x04002784 RID: 10116
		private static bool trackMaterialStackTrace = false;

		// Token: 0x04002785 RID: 10117
		private static readonly Dictionary<Material, MaterialAllocator.MaterialInfo> references = new Dictionary<Material, MaterialAllocator.MaterialInfo>();

		// Token: 0x04002786 RID: 10118
		private static int nextWarningThreshold;

		// Token: 0x04002787 RID: 10119
		private static Dictionary<string, int> snapshot = new Dictionary<string, int>();

		// Token: 0x02000809 RID: 2057
		private struct MaterialInfo
		{
			// Token: 0x04002788 RID: 10120
			public string stackTrace;
		}
	}
}
