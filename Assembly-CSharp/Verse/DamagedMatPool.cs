using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005EB RID: 1515
	public static class DamagedMatPool
	{
		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x060026D2 RID: 9938 RVA: 0x000CD736 File Offset: 0x000CB936
		public static int MatCount
		{
			get
			{
				return DamagedMatPool.damagedMats.Count;
			}
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x000CD744 File Offset: 0x000CB944
		public static Material GetDamageFlashMat(Material baseMat, float damPct)
		{
			if (damPct < 0.01f)
			{
				return baseMat;
			}
			Material material;
			if (!DamagedMatPool.damagedMats.TryGetValue(baseMat, out material))
			{
				material = MaterialAllocator.Create(baseMat);
				DamagedMatPool.damagedMats.Add(baseMat, material);
			}
			material.color = Color.Lerp(baseMat.color, DamagedMatPool.DamagedMatStartingColor, damPct);
			return material;
		}

		// Token: 0x04002023 RID: 8227
		private static Dictionary<Material, Material> damagedMats = new Dictionary<Material, Material>();

		// Token: 0x04002024 RID: 8228
		public static readonly Color DamagedMatStartingColor = Color.red;
	}
}
