using System;

namespace RimWorld
{
	// Token: 0x02003265 RID: 12901
	public struct TransferableCountToTransferStoppingPoint
	{
		// Token: 0x06012260 RID: 74336 RVA: 0x0055050D File Offset: 0x0054E70D
		public TransferableCountToTransferStoppingPoint(int threshold, string leftLabel, string rightLabel)
		{
			this.threshold = threshold;
			this.leftLabel = leftLabel;
			this.rightLabel = rightLabel;
		}

		// Token: 0x0400BC8F RID: 48271
		public int threshold;

		// Token: 0x0400BC90 RID: 48272
		public string leftLabel;

		// Token: 0x0400BC91 RID: 48273
		public string rightLabel;
	}
}
