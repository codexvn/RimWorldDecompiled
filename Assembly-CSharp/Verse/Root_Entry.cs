using System;
using System.IO;
using RimWorld;

namespace Verse
{
	// Token: 0x02000359 RID: 857
	public class Root_Entry : Root
	{
		// Token: 0x060015F1 RID: 5617 RVA: 0x000671F0 File Offset: 0x000653F0
		public override void Start()
		{
			base.Start();
			try
			{
				Current.Game = null;
				this.musicManagerEntry = new MusicManagerEntry();
				FileInfo fileInfo = Root.checkedAutostartSaveFile ? null : SaveGameFilesUtility.GetAutostartSaveFile();
				Root.checkedAutostartSaveFile = true;
				if (fileInfo != null)
				{
					GameDataSaveLoader.LoadGame(fileInfo);
				}
			}
			catch (Exception ex)
			{
				string str = "Critical error in root Start(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x060015F2 RID: 5618 RVA: 0x00067264 File Offset: 0x00065464
		public override void Update()
		{
			base.Update();
			if (LongEventHandler.ShouldWaitForEvent || this.destroyed)
			{
				return;
			}
			try
			{
				this.musicManagerEntry.MusicManagerEntryUpdate();
				if (Find.World != null)
				{
					Find.World.WorldUpdate();
				}
				if (Current.Game != null)
				{
					Current.Game.UpdateEntry();
				}
			}
			catch (Exception ex)
			{
				string str = "Root level exception in Update(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x04001441 RID: 5185
		public MusicManagerEntry musicManagerEntry;
	}
}
