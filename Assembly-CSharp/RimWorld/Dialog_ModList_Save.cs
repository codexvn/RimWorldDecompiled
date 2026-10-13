using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020031CD RID: 12749
	public class Dialog_ModList_Save : Dialog_ModList
	{
		// Token: 0x17002E93 RID: 11923
		// (get) Token: 0x06011F04 RID: 73476 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06011F05 RID: 73477 RVA: 0x00538F6C File Offset: 0x0053716C
		public Dialog_ModList_Save(ModList modList, Action onClosed = null)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.savingModList = modList;
			this.onClosed = onClosed;
		}

		// Token: 0x06011F06 RID: 73478 RVA: 0x00538F98 File Offset: 0x00537198
		protected override void DoFileInteraction(string fileName)
		{
			fileName = GenFile.SanitizedFileName(fileName);
			string absPath = GenFilePaths.AbsFilePathForModList(fileName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveModList(this.savingModList, absPath);
			}, "SavingLongEvent", false, null, true, false, null);
			Messages.Message("SavedAs".Translate(fileName), MessageTypeDefOf.SilentInput, false);
			this.Close(true);
		}

		// Token: 0x06011F07 RID: 73479 RVA: 0x00539006 File Offset: 0x00537206
		public override void Close(bool doCloseSound = true)
		{
			base.Close(doCloseSound);
			Action action = this.onClosed;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0400B9C5 RID: 47557
		private ModList savingModList;

		// Token: 0x0400B9C6 RID: 47558
		private Action onClosed;
	}
}
