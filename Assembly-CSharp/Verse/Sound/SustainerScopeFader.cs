using System;

namespace Verse.Sound
{
	// Token: 0x02000E49 RID: 3657
	public class SustainerScopeFader
	{
		// Token: 0x06005C23 RID: 23587 RVA: 0x001D777C File Offset: 0x001D597C
		public void SustainerScopeUpdate()
		{
			if (this.inScope)
			{
				float num = this.inScopePercent + 0.05f;
				this.inScopePercent = num;
				if (this.inScopePercent > 1f)
				{
					this.inScopePercent = 1f;
					return;
				}
			}
			else
			{
				this.inScopePercent -= 0.03f;
				if (this.inScopePercent <= 0.001f)
				{
					this.inScopePercent = 0f;
				}
			}
		}

		// Token: 0x040042C1 RID: 17089
		public bool inScope = true;

		// Token: 0x040042C2 RID: 17090
		public float inScopePercent = 1f;

		// Token: 0x040042C3 RID: 17091
		private const float ScopeMatchFallRate = 0.03f;

		// Token: 0x040042C4 RID: 17092
		private const float ScopeMatchRiseRate = 0.05f;
	}
}
