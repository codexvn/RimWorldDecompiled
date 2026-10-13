using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003DD RID: 989
	public static class DebugSolidColorMats
	{
		// Token: 0x06001951 RID: 6481 RVA: 0x0007FD4C File Offset: 0x0007DF4C
		public static Material MaterialOf(Color col)
		{
			Material material;
			if (DebugSolidColorMats.colorMatDict.TryGetValue(col, out material))
			{
				return material;
			}
			material = SolidColorMaterials.SimpleSolidColorMaterial(col, false);
			DebugSolidColorMats.colorMatDict.Add(col, material);
			return material;
		}

		// Token: 0x0400165E RID: 5726
		private static Dictionary<Color, Material> colorMatDict = new Dictionary<Color, Material>();
	}
}
