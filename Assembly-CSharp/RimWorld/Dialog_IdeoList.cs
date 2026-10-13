using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x020031C6 RID: 12742
	public abstract class Dialog_IdeoList : Dialog_FileList
	{
		// Token: 0x06011EF2 RID: 73458 RVA: 0x00538A44 File Offset: 0x00536C44
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllCustomIdeoFiles)
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
