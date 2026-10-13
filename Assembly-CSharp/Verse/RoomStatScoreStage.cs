using System;

namespace Verse
{
	// Token: 0x020002DB RID: 731
	public class RoomStatScoreStage
	{
		// Token: 0x06001208 RID: 4616 RVA: 0x00054240 File Offset: 0x00052440
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
		}

		// Token: 0x04001044 RID: 4164
		public float minScore = float.MinValue;

		// Token: 0x04001045 RID: 4165
		public string label;

		// Token: 0x04001046 RID: 4166
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabel;
	}
}
