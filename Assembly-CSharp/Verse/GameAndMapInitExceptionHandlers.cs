using System;
using System.Linq;

namespace Verse
{
	// Token: 0x020004C2 RID: 1218
	public static class GameAndMapInitExceptionHandlers
	{
		// Token: 0x06001F87 RID: 8071 RVA: 0x000A5C9C File Offset: 0x000A3E9C
		public static void ErrorWhileLoadingAssets(Exception e)
		{
			string text = "ErrorWhileLoadingAssets".Translate();
			if (!ModsConfig.ActiveModsInLoadOrder.Any((ModMetaData x) => !x.Official))
			{
				if (ModsConfig.ActiveModsInLoadOrder.Any((ModMetaData x) => x.IsCoreMod))
				{
					goto IL_86;
				}
			}
			text += "\n\n" + "ErrorWhileLoadingAssets_ModsInfo".Translate();
			IL_86:
			DelayedErrorWindowRequest.Add(text, "ErrorWhileLoadingAssetsTitle".Translate());
			GenScene.GoToMainMenu();
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x000A5D49 File Offset: 0x000A3F49
		public static void ErrorWhileGeneratingMap(Exception e)
		{
			DelayedErrorWindowRequest.Add("ErrorWhileGeneratingMap".Translate(), "ErrorWhileGeneratingMapTitle".Translate());
			Scribe.ForceStop();
			GenScene.GoToMainMenu();
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x000A5D78 File Offset: 0x000A3F78
		public static void ErrorWhileLoadingGame(Exception e)
		{
			string text = "ErrorWhileLoadingMap".Translate();
			string value;
			string value2;
			if (!ScribeMetaHeaderUtility.LoadedModsMatchesActiveMods(out value, out value2))
			{
				text += "\n\n" + "ModsMismatchWarningText".Translate(value, value2);
			}
			DelayedErrorWindowRequest.Add(text, "ErrorWhileLoadingMapTitle".Translate());
			Scribe.ForceStop();
			GenScene.GoToMainMenu();
		}
	}
}
