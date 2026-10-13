using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C2C RID: 7212
	public class QuestPartDialogCloseAction
	{
		// Token: 0x04006F09 RID: 28425
		public SoundDef dialogCloseSound;

		// Token: 0x04006F0A RID: 28426
		public Action dialogCloseAction;

		// Token: 0x02001C2D RID: 7213
		public enum CloseActionKey
		{
			// Token: 0x04006F0C RID: 28428
			None,
			// Token: 0x04006F0D RID: 28429
			ArchonexusVictorySound2nd,
			// Token: 0x04006F0E RID: 28430
			ArchonexusVictorySound3rd
		}
	}
}
