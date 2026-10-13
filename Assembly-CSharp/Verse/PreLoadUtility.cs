using System;

namespace Verse
{
	// Token: 0x02000848 RID: 2120
	public static class PreLoadUtility
	{
		// Token: 0x060035AD RID: 13741 RVA: 0x0011DA68 File Offset: 0x0011BC68
		public static void CheckVersionAndLoad(string path, ScribeMetaHeaderUtility.ScribeHeaderMode mode, Action loadAct, bool skipOnMismatch = false)
		{
			try
			{
				Scribe.loader.InitLoadingMetaHeaderOnly(path);
				ScribeMetaHeaderUtility.LoadGameDataHeader(mode, false);
				Scribe.loader.FinalizeLoading();
			}
			catch (Exception ex)
			{
				string str = "Exception loading ";
				string str2 = ": ";
				Exception ex2 = ex;
				Log.Warning(str + path + str2 + ((ex2 != null) ? ex2.ToString() : null));
				Scribe.ForceStop();
			}
			string text;
			string text2;
			if (skipOnMismatch && !ScribeMetaHeaderUtility.LoadedModsMatchesActiveMods(out text, out text2))
			{
				return;
			}
			if (!ScribeMetaHeaderUtility.TryCreateDialogsForVersionMismatchWarnings(loadAct))
			{
				loadAct();
			}
		}
	}
}
