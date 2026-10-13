using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D3A RID: 3386
	public static class MatsFromSpectrum
	{
		// Token: 0x06005629 RID: 22057 RVA: 0x001BCED5 File Offset: 0x001BB0D5
		public static Material Get(Color[] spectrum, float val)
		{
			return MatsFromSpectrum.Get(spectrum, val, ShaderDatabase.MetaOverlay);
		}

		// Token: 0x0600562A RID: 22058 RVA: 0x001BCEE3 File Offset: 0x001BB0E3
		public static Material Get(Color[] spectrum, float val, Shader shader)
		{
			return SolidColorMaterials.NewSolidColorMaterial(ColorsFromSpectrum.Get(spectrum, val), shader);
		}
	}
}
