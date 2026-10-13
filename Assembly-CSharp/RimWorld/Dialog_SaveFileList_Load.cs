using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020031D2 RID: 12754
	public class Dialog_SaveFileList_Load : Dialog_SaveFileList
	{
		// Token: 0x17002E94 RID: 11924
		// (get) Token: 0x06011F15 RID: 73493 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool FocusSearchField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06011F16 RID: 73494 RVA: 0x00539288 File Offset: 0x00537488
		public Dialog_SaveFileList_Load()
		{
			this.interactButLabel = "LoadGameButton".Translate();
		}

		// Token: 0x06011F17 RID: 73495 RVA: 0x005392A5 File Offset: 0x005374A5
		protected override void DoFileInteraction(string saveFileName)
		{
			GameDataSaveLoader.CheckVersionAndLoadGame(saveFileName);
		}
	}
}
