using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace RimWorld;

public abstract class Dialog_SaveFileList : Dialog_FileList
{
	private static readonly Color AutosaveTextColor = new Color(0.75f, 0.75f, 0.75f);

	private Task loadSavesTask;

	protected override Color FileNameColor(SaveFileInfo sfi)
	{
		if (SaveGameFilesUtility.IsAutoSave(Path.GetFileNameWithoutExtension(sfi.FileName)))
		{
			GUI.color = AutosaveTextColor;
		}
		return base.FileNameColor(sfi);
	}

	private void ReloadFilesTask()
	{
		Parallel.ForEach(files, delegate(SaveFileInfo file)
		{
			try
			{
				file.LoadData();
			}
			catch (Exception value)
			{
				Log.Error($"Exception loading {file.FileInfo.Name}: {value}");
			}
		});
	}

	protected override void ReloadFiles()
	{
		if (loadSavesTask != null && loadSavesTask.Status != TaskStatus.RanToCompletion)
		{
			loadSavesTask.Wait();
		}
		files.Clear();
		foreach (FileInfo allSavedGameFile in GenFilePaths.AllSavedGameFiles)
		{
			try
			{
				SaveFileInfo item = new SaveFileInfo(allSavedGameFile);
				files.Add(item);
			}
			catch (Exception value)
			{
				Log.Error($"Exception loading {allSavedGameFile.Name}: {value}");
			}
		}
		loadSavesTask = Task.Run((Action)ReloadFilesTask);
	}

	public override void PostClose()
	{
		if (Current.ProgramState == ProgramState.Playing)
		{
			Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Menu);
		}
	}
}
