using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000231 RID: 561
	public class ColorOption
	{
		// Token: 0x06000EA3 RID: 3747 RVA: 0x0004433C File Offset: 0x0004253C
		public Color RandomizedColor()
		{
			if (this.only.a >= 0f)
			{
				return this.only;
			}
			return new Color(Rand.Range(this.min.r, this.max.r), Rand.Range(this.min.g, this.max.g), Rand.Range(this.min.b, this.max.b), Rand.Range(this.min.a, this.max.a));
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x000443D3 File Offset: 0x000425D3
		public void SetSingle(Color color)
		{
			this.only = color;
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x000443DC File Offset: 0x000425DC
		public void SetMin(Color color)
		{
			this.min = color;
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x000443E5 File Offset: 0x000425E5
		public void SetMax(Color color)
		{
			this.max = color;
		}

		// Token: 0x0400096F RID: 2415
		public float weight = 1f;

		// Token: 0x04000970 RID: 2416
		public Color min = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x04000971 RID: 2417
		public Color max = new Color(-1f, -1f, -1f, -1f);

		// Token: 0x04000972 RID: 2418
		public Color only = new Color(-1f, -1f, -1f, -1f);
	}
}
