using System;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x020031DE RID: 12766
	public abstract class Dialog_XenotypeList : Dialog_FileList
	{
		// Token: 0x06011F37 RID: 73527 RVA: 0x00539C50 File Offset: 0x00537E50
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllCustomXenotypeFiles)
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
			CharacterCardUtility.cachedCustomXenotypes = null;
		}
	}
}
