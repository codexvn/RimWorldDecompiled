using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020031D0 RID: 12752
	public abstract class Dialog_SaveFileList : Dialog_FileList
	{
		// Token: 0x06011F0C RID: 73484 RVA: 0x005390E8 File Offset: 0x005372E8
		protected override Color FileNameColor(SaveFileInfo sfi)
		{
			if (SaveGameFilesUtility.IsAutoSave(Path.GetFileNameWithoutExtension(sfi.FileName)))
			{
				GUI.color = Dialog_SaveFileList.AutosaveTextColor;
			}
			return base.FileNameColor(sfi);
		}

		// Token: 0x06011F0D RID: 73485 RVA: 0x0053910D File Offset: 0x0053730D
		private void ReloadFilesTask()
		{
			Parallel.ForEach<SaveFileInfo>(this.files, delegate(SaveFileInfo file)
			{
				try
				{
					file.LoadData();
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Exception loading {0}: {1}", file.FileInfo.Name, arg));
				}
			});
		}

		// Token: 0x06011F0E RID: 73486 RVA: 0x0053913C File Offset: 0x0053733C
		protected override void ReloadFiles()
		{
			if (this.loadSavesTask != null && this.loadSavesTask.Status != TaskStatus.RanToCompletion)
			{
				this.loadSavesTask.Wait();
			}
			this.files.Clear();
			foreach (FileInfo fileInfo in GenFilePaths.AllSavedGameFiles)
			{
				try
				{
					SaveFileInfo item = new SaveFileInfo(fileInfo);
					this.files.Add(item);
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Exception loading {0}: {1}", fileInfo.Name, arg));
				}
			}
			this.loadSavesTask = Task.Run(new Action(this.ReloadFilesTask));
		}

		// Token: 0x06011F0F RID: 73487 RVA: 0x00539200 File Offset: 0x00537400
		public override void PostClose()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu, true);
			}
		}

		// Token: 0x0400B9CA RID: 47562
		private static readonly Color AutosaveTextColor = new Color(0.75f, 0.75f, 0.75f);

		// Token: 0x0400B9CB RID: 47563
		private Task loadSavesTask;
	}
}
