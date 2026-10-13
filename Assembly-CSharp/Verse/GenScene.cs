using System;
using UnityEngine.SceneManagement;
using Verse.Profile;

namespace Verse
{
	// Token: 0x02000352 RID: 850
	public static class GenScene
	{
		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x060015D0 RID: 5584 RVA: 0x00066B18 File Offset: 0x00064D18
		public static bool InEntryScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Entry";
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x060015D1 RID: 5585 RVA: 0x00066B3C File Offset: 0x00064D3C
		public static bool InPlayScene
		{
			get
			{
				return SceneManager.GetActiveScene().name == "Play";
			}
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x00066B60 File Offset: 0x00064D60
		public static void GoToMainMenu()
		{
			LongEventHandler.ClearQueuedEvents();
			Game game = Current.Game;
			if (game != null)
			{
				game.Dispose();
			}
			LongEventHandler.QueueLongEvent(delegate()
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = null;
			}, "Entry", "LoadingLongEvent", true, null, false, false);
		}

		// Token: 0x04001424 RID: 5156
		public const string EntrySceneName = "Entry";

		// Token: 0x04001425 RID: 5157
		public const string PlaySceneName = "Play";
	}
}
