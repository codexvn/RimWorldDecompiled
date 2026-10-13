using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020031C7 RID: 12743
	public class Dialog_IdeoList_Save : Dialog_IdeoList
	{
		// Token: 0x17002E92 RID: 11922
		// (get) Token: 0x06011EF4 RID: 73460 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06011EF5 RID: 73461 RVA: 0x00538AE0 File Offset: 0x00536CE0
		public Dialog_IdeoList_Save(Ideo ideo)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.typingName = ideo.name;
			this.savingIdeo = ideo;
		}

		// Token: 0x06011EF6 RID: 73462 RVA: 0x00538B10 File Offset: 0x00536D10
		protected override void DoFileInteraction(string fileName)
		{
			fileName = GenFile.SanitizedFileName(fileName);
			string absPath = GenFilePaths.AbsPathForIdeo(fileName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveIdeo(this.savingIdeo, absPath);
			}, "SavingLongEvent", false, null, true, false, null);
			Messages.Message("SavedAs".Translate(fileName), MessageTypeDefOf.SilentInput, false);
			this.Close(true);
		}

		// Token: 0x0400B9BA RID: 47546
		private Ideo savingIdeo;
	}
}
