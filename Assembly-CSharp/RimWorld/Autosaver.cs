using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Profile;

namespace RimWorld
{
	// Token: 0x020034FE RID: 13566
	public sealed class Autosaver
	{
		// Token: 0x170031A4 RID: 12708
		// (get) Token: 0x060130F9 RID: 78073 RVA: 0x005A7474 File Offset: 0x005A5674
		private float AutosaveIntervalDays
		{
			get
			{
				float num = Prefs.AutosaveIntervalDays;
				if (Current.Game.Info.permadeathMode && num > 1f)
				{
					num = 1f;
				}
				return num;
			}
		}

		// Token: 0x170031A5 RID: 12709
		// (get) Token: 0x060130FA RID: 78074 RVA: 0x005A74A7 File Offset: 0x005A56A7
		private int AutosaveIntervalTicks
		{
			get
			{
				return Mathf.RoundToInt(this.AutosaveIntervalDays * 60000f);
			}
		}

		// Token: 0x060130FB RID: 78075 RVA: 0x005A74BC File Offset: 0x005A56BC
		public void AutosaverTick()
		{
			this.ticksSinceSave++;
			if (this.ticksSinceSave >= this.AutosaveIntervalTicks && !GameDataSaveLoader.SavingIsTemporarilyDisabled)
			{
				LongEventHandler.QueueLongEvent(new Action(this.DoAutosave), "Autosaving", false, null, true, false, null);
				this.ticksSinceSave = 0;
			}
		}

		// Token: 0x060130FC RID: 78076 RVA: 0x005A7510 File Offset: 0x005A5710
		public void DoAutosave()
		{
			string fileName;
			if (Current.Game.Info.permadeathMode)
			{
				fileName = Current.Game.Info.permadeathModeUniqueName;
			}
			else
			{
				fileName = this.NewAutosaveFileName();
			}
			GameDataSaveLoader.SaveGame(fileName);
		}

		// Token: 0x060130FD RID: 78077 RVA: 0x001433EA File Offset: 0x001415EA
		private void DoMemoryCleanup()
		{
			MemoryUtility.UnloadUnusedUnityAssets();
		}

		// Token: 0x060130FE RID: 78078 RVA: 0x005A7550 File Offset: 0x005A5750
		private string NewAutosaveFileName()
		{
			string text = this.AutoSaveNames().FirstOrDefault((string name) => !SaveGameFilesUtility.SavedGameNamedExists(name));
			if (text != null)
			{
				return text;
			}
			return this.AutoSaveNames().MinBy((string name) => new FileInfo(GenFilePaths.FilePathForSavedGame(name)).LastWriteTime);
		}

		// Token: 0x060130FF RID: 78079 RVA: 0x005A75B7 File Offset: 0x005A57B7
		private IEnumerable<string> AutoSaveNames()
		{
			int num;
			for (int i = 1; i <= Prefs.AutosavesCount; i = num + 1)
			{
				yield return "Autosave-" + i.ToString();
				num = i;
			}
			yield break;
		}

		// Token: 0x0400C5A1 RID: 50593
		private int ticksSinceSave;

		// Token: 0x0400C5A2 RID: 50594
		public const float MaxPermadeathModeAutosaveInterval = 1f;
	}
}
