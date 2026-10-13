using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005FE RID: 1534
	public static class InvisibilityMatPool
	{
		// Token: 0x06002750 RID: 10064 RVA: 0x000D1774 File Offset: 0x000CF974
		public static Material GetInvisibleMat(Material baseMat)
		{
			if (baseMat == null)
			{
				return null;
			}
			Material material;
			if (!InvisibilityMatPool.materials.TryGetValue(baseMat, out material))
			{
				material = MaterialAllocator.Create(baseMat);
				material.shader = ShaderDatabase.Invisible;
				material.SetTexture(InvisibilityMatPool.NoiseTex, TexGame.InvisDistortion);
				material.color = InvisibilityMatPool.color;
				InvisibilityMatPool.materials.Add(baseMat, material);
			}
			return material;
		}

		// Token: 0x040020A5 RID: 8357
		private static readonly Dictionary<Material, Material> materials = new Dictionary<Material, Material>();

		// Token: 0x040020A6 RID: 8358
		private static readonly Color color = new Color(0.75f, 0.93f, 0.98f, 0.5f);

		// Token: 0x040020A7 RID: 8359
		private static readonly int NoiseTex = Shader.PropertyToID("_NoiseTex");
	}
}
