using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x020031CC RID: 12748
	public abstract class Dialog_ModList : Dialog_FileList
	{
		// Token: 0x06011F02 RID: 73474 RVA: 0x00538ED0 File Offset: 0x005370D0
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllModListFiles)
			{
				try
				{
					SaveFileInfo saveFileInfo = new SaveFileInfo(fileInfo);
					saveFileInfo.LoadData();
					this.files.Add(saveFileInfo);
				}
				catch (Exception ex)
				{
					Log.Error("Exception loading " + fileInfo.Name + ": " + ex.ToString());
				}
			}
		}
	}
}
