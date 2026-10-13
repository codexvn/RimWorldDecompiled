using System;

namespace Verse
{
	// Token: 0x0200028B RID: 651
	public class InjuryProps
	{
		// Token: 0x04000D35 RID: 3381
		public float painPerSeverity = 1f;

		// Token: 0x04000D36 RID: 3382
		public float averagePainPerSeverityPermanent = 0.5f;

		// Token: 0x04000D37 RID: 3383
		public float bleedRate;

		// Token: 0x04000D38 RID: 3384
		public bool canMerge;

		// Token: 0x04000D39 RID: 3385
		public string destroyedLabel;

		// Token: 0x04000D3A RID: 3386
		public string destroyedOutLabel;

		// Token: 0x04000D3B RID: 3387
		public bool useRemovedLabel;

		// Token: 0x04000D3C RID: 3388
		public bool alwaysUseDestroyedLabel;
	}
}
