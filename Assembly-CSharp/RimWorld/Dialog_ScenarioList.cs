using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x020031D5 RID: 12757
	public abstract class Dialog_ScenarioList : Dialog_FileList
	{
		// Token: 0x06011F1D RID: 73501 RVA: 0x00539398 File Offset: 0x00537598
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllCustomScenarioFiles)
			{
				try
				{
					SaveFileInfo saveFileInfo = new SaveFileInfo(fileInfo);
					saveFileInfo.LoadData();
					this.files.Add(saveFileInfo);
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Exception loading {0}: {1}", fileInfo.Name, arg));
				}
			}
		}
	}
}
