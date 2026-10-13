using System;
using System.IO;
using RimWorld;

namespace Verse
{
	// Token: 0x020001DD RID: 477
	public abstract class Dialog_CameraConfigList : Dialog_FileList
	{
		// Token: 0x06000CF9 RID: 3321 RVA: 0x0003DCB8 File Offset: 0x0003BEB8
		protected override void ReloadFiles()
		{
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllCameraConfigFiles)
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
