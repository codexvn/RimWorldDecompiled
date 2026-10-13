using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C2E RID: 7214
	public static class QuestPartDialogCloseActionRegistry
	{
		// Token: 0x0600A0A2 RID: 41122 RVA: 0x002E420F File Offset: 0x002E240F
		public static QuestPartDialogCloseAction CloseActionOf(QuestPartDialogCloseAction.CloseActionKey key)
		{
			return QuestPartDialogCloseActionRegistry.CloseActions.TryGetValue(key, null);
		}

		// Token: 0x04006F0F RID: 28431
		private static readonly Dictionary<QuestPartDialogCloseAction.CloseActionKey, QuestPartDialogCloseAction> CloseActions = new Dictionary<QuestPartDialogCloseAction.CloseActionKey, QuestPartDialogCloseAction>
		{
			{
				QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound2nd,
				new QuestPartDialogCloseAction
				{
					dialogCloseSound = SoundDefOf.GameStartSting_FirstArchonexusCycle,
					dialogCloseAction = delegate()
					{
						Find.MusicManagerPlay.ForceSilenceFor(8f);
					}
				}
			},
			{
				QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound3rd,
				new QuestPartDialogCloseAction
				{
					dialogCloseSound = SoundDefOf.GameStartSting_SecondArchonexusCycle,
					dialogCloseAction = delegate()
					{
						Find.MusicManagerPlay.ForceSilenceFor(8f);
					}
				}
			}
		};
	}
}
