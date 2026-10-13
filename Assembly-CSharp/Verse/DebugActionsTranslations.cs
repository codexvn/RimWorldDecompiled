using System;
using LudeonTK;

namespace Verse
{
	// Token: 0x020009B5 RID: 2485
	public static class DebugActionsTranslations
	{
		// Token: 0x06003EE5 RID: 16101 RVA: 0x00149570 File Offset: 0x00147770
		[DebugAction("Translation", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Entry)]
		private static void WriteBackstoryTranslationFile()
		{
			LanguageDataWriter.WriteBackstoryFile();
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x00149577 File Offset: 0x00147777
		[DebugAction("Translation", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Entry)]
		private static void SaveTranslationReport()
		{
			LanguageReportGenerator.SaveTranslationReport();
		}
	}
}
