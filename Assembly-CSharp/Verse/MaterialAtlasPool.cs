using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200080C RID: 2060
	public static class MaterialAtlasPool
	{
		// Token: 0x0600347B RID: 13435 RVA: 0x00113A6B File Offset: 0x00111C6B
		public static Material SubMaterialFromAtlas(Material mat, LinkDirections LinkSet)
		{
			if (!MaterialAtlasPool.atlasDict.ContainsKey(mat))
			{
				MaterialAtlasPool.atlasDict.Add(mat, new MaterialAtlasPool.MaterialAtlas(mat));
			}
			return MaterialAtlasPool.atlasDict[mat].SubMat(LinkSet);
		}

		// Token: 0x04002793 RID: 10131
		private static Dictionary<Material, MaterialAtlasPool.MaterialAtlas> atlasDict = new Dictionary<Material, MaterialAtlasPool.MaterialAtlas>();

		// Token: 0x0200080D RID: 2061
		private class MaterialAtlas
		{
			// Token: 0x0600347D RID: 13437 RVA: 0x00113AA8 File Offset: 0x00111CA8
			public MaterialAtlas(Material newRootMat)
			{
				Vector2 mainTextureScale = new Vector2(0.1875f, 0.1875f);
				for (int i = 0; i < 16; i++)
				{
					float x = (float)(i % 4) * 0.25f + 0.03125f;
					float y = (float)(i / 4) * 0.25f + 0.03125f;
					Vector2 mainTextureOffset = new Vector2(x, y);
					Material material = MaterialAllocator.Create(newRootMat);
					material.name = newRootMat.name + "_ASM" + i.ToString();
					material.mainTextureScale = mainTextureScale;
					material.mainTextureOffset = mainTextureOffset;
					this.subMats[i] = material;
				}
			}

			// Token: 0x0600347E RID: 13438 RVA: 0x00113B54 File Offset: 0x00111D54
			public Material SubMat(LinkDirections linkSet)
			{
				int num = (int)linkSet;
				if (num >= this.subMats.Length)
				{
					Log.Warning("Cannot get submat of index " + num.ToString() + ": out of range.");
					return BaseContent.BadMat;
				}
				return this.subMats[num];
			}

			// Token: 0x04002794 RID: 10132
			protected Material[] subMats = new Material[16];

			// Token: 0x04002795 RID: 10133
			private const float TexPadding = 0.03125f;
		}
	}
}
