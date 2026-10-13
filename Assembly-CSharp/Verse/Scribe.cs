using System;

namespace Verse
{
	// Token: 0x0200085D RID: 2141
	public static class Scribe
	{
		// Token: 0x06003613 RID: 13843 RVA: 0x00120BCF File Offset: 0x0011EDCF
		public static void ForceStop()
		{
			Scribe.mode = LoadSaveMode.Inactive;
			Scribe.saver.ForceStop();
			Scribe.loader.ForceStop();
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x00120BEC File Offset: 0x0011EDEC
		public static bool EnterNode(string nodeName)
		{
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				return false;
			}
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				return Scribe.saver.EnterNode(nodeName);
			}
			return (Scribe.mode != LoadSaveMode.LoadingVars && Scribe.mode != LoadSaveMode.ResolvingCrossRefs && Scribe.mode != LoadSaveMode.PostLoadInit) || Scribe.loader.EnterNode(nodeName);
		}

		// Token: 0x06003615 RID: 13845 RVA: 0x00120C3B File Offset: 0x0011EE3B
		public static void ExitNode()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Scribe.saver.ExitNode();
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Scribe.loader.ExitNode();
			}
		}

		// Token: 0x040028C1 RID: 10433
		public static ScribeSaver saver = new ScribeSaver();

		// Token: 0x040028C2 RID: 10434
		public static ScribeLoader loader = new ScribeLoader();

		// Token: 0x040028C3 RID: 10435
		public static LoadSaveMode mode = LoadSaveMode.Inactive;
	}
}
