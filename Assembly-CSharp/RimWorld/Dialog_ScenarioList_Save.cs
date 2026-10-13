using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020031D6 RID: 12758
	public class Dialog_ScenarioList_Save : Dialog_ScenarioList
	{
		// Token: 0x17002E96 RID: 11926
		// (get) Token: 0x06011F1F RID: 73503 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06011F20 RID: 73504 RVA: 0x00539428 File Offset: 0x00537628
		public Dialog_ScenarioList_Save(Scenario scen, Action onClosed = null)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			string name = scen.name;
			this.typingName = ((name != null) ? name.Trim() : null);
			this.savingScen = scen;
			this.onClosed = onClosed;
		}

		// Token: 0x06011F21 RID: 73505 RVA: 0x00539478 File Offset: 0x00537678
		protected override void DoFileInteraction(string fileName)
		{
			fileName = GenFile.SanitizedFileName(fileName);
			string absPath = GenFilePaths.AbsPathForScenario(fileName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveScenario(this.savingScen, absPath);
			}, "SavingLongEvent", false, null, true, false, null);
			Messages.Message("SavedAs".Translate(fileName), MessageTypeDefOf.SilentInput, false);
			this.Close(true);
		}

		// Token: 0x06011F22 RID: 73506 RVA: 0x005394E6 File Offset: 0x005376E6
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

		// Token: 0x0400B9CF RID: 47567
		private Scenario savingScen;

		// Token: 0x0400B9D0 RID: 47568
		private Action onClosed;
	}
}
