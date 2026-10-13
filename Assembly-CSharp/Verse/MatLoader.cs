using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D38 RID: 3384
	public static class MatLoader
	{
		// Token: 0x0600561F RID: 22047 RVA: 0x001BCD1E File Offset: 0x001BAF1E
		public static void ClearCache()
		{
			MatLoader.dict.Clear();
		}

		// Token: 0x06005620 RID: 22048 RVA: 0x001BCD2C File Offset: 0x001BAF2C
		public static Material LoadMat(string matPath, int renderQueue = -1)
		{
			Material material = (Material)Resources.Load("Materials/" + matPath, typeof(Material));
			if (material == null)
			{
				Log.Warning("Could not load material " + matPath);
			}
			MatLoader.Request key = new MatLoader.Request
			{
				path = matPath,
				renderQueue = renderQueue
			};
			Material material2;
			if (!MatLoader.dict.TryGetValue(key, out material2))
			{
				material2 = MaterialAllocator.Create(material);
				if (renderQueue != -1)
				{
					material2.renderQueue = renderQueue;
				}
				MatLoader.dict.Add(key, material2);
			}
			return material2;
		}

		// Token: 0x06005621 RID: 22049 RVA: 0x001BCDBC File Offset: 0x001BAFBC
		public static Material LoadMatDirect(string matPath)
		{
			Material material = Resources.Load<Material>("Materials/" + matPath);
			if (material == null)
			{
				throw new Exception("Material not found: " + matPath);
			}
			if (MatLoader.loadedDirect.ContainsKey(material))
			{
				Debug.LogWarning("Material '" + matPath + "' has been loaded multiple times. Mutations to this material might cause bugs");
			}
			return material;
		}

		// Token: 0x04003E51 RID: 15953
		private static Dictionary<MatLoader.Request, Material> dict = new Dictionary<MatLoader.Request, Material>();

		// Token: 0x04003E52 RID: 15954
		private static Dictionary<Material, string> loadedDirect = new Dictionary<Material, string>();

		// Token: 0x02000D39 RID: 3385
		private struct Request
		{
			// Token: 0x06005623 RID: 22051 RVA: 0x001BCE2D File Offset: 0x001BB02D
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(Gen.HashCombine<string>(0, this.path), this.renderQueue);
			}

			// Token: 0x06005624 RID: 22052 RVA: 0x001BCE46 File Offset: 0x001BB046
			public override bool Equals(object obj)
			{
				return obj is MatLoader.Request && this.Equals((MatLoader.Request)obj);
			}

			// Token: 0x06005625 RID: 22053 RVA: 0x001BCE5E File Offset: 0x001BB05E
			public bool Equals(MatLoader.Request other)
			{
				return other.path == this.path && other.renderQueue == this.renderQueue;
			}

			// Token: 0x06005626 RID: 22054 RVA: 0x001BCE83 File Offset: 0x001BB083
			public static bool operator ==(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06005627 RID: 22055 RVA: 0x001BCE8D File Offset: 0x001BB08D
			public static bool operator !=(MatLoader.Request lhs, MatLoader.Request rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x06005628 RID: 22056 RVA: 0x001BCE99 File Offset: 0x001BB099
			public override string ToString()
			{
				return string.Concat(new string[]
				{
					"MatLoader.Request(",
					this.path,
					", ",
					this.renderQueue.ToString(),
					")"
				});
			}

			// Token: 0x04003E53 RID: 15955
			public string path;

			// Token: 0x04003E54 RID: 15956
			public int renderQueue;
		}
	}
}
