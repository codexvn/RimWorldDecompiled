using System;
using UnityEngine.SceneManagement;

namespace Verse
{
	// Token: 0x02000354 RID: 852
	public static class QuickStarter
	{
		// Token: 0x060015D6 RID: 5590 RVA: 0x00066BCD File Offset: 0x00064DCD
		public static bool CheckQuickStart()
		{
			if (GenCommandLine.CommandLineArgPassed("quicktest") && !QuickStarter.quickStarted && GenScene.InEntryScene)
			{
				QuickStarter.quickStarted = true;
				SceneManager.LoadScene("Play");
				return true;
			}
			return false;
		}

		// Token: 0x04001428 RID: 5160
		private static bool quickStarted;
	}
}
