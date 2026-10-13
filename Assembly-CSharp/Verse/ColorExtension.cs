using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C64 RID: 3172
	public static class ColorExtension
	{
		// Token: 0x0600512B RID: 20779 RVA: 0x001A5868 File Offset: 0x001A3A68
		public static Color ToOpaque(this Color c)
		{
			c.a = 1f;
			return c;
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x001A5877 File Offset: 0x001A3A77
		public static Color ToTransparent(this Color c, float transparency)
		{
			c.a = transparency;
			return c;
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x001A5884 File Offset: 0x001A3A84
		public static Color Min(this Color c, Color other)
		{
			c.r = Mathf.Min(c.r, other.r);
			c.g = Mathf.Min(c.g, other.g);
			c.b = Mathf.Min(c.b, other.b);
			return c;
		}
	}
}
