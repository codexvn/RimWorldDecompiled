using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000CC RID: 204
	public static class ColorIntUtility
	{
		// Token: 0x0600050E RID: 1294 RVA: 0x0001B92E File Offset: 0x00019B2E
		public static ColorInt AsColorInt(this Color32 col)
		{
			return new ColorInt(col);
		}
	}
}
