using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020031D3 RID: 12755
	public class Dialog_SaveFileList_Save : Dialog_SaveFileList
	{
		// Token: 0x17002E95 RID: 11925
		// (get) Token: 0x06011F18 RID: 73496 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06011F19 RID: 73497 RVA: 0x005392B0 File Offset: 0x005374B0
		public Dialog_SaveFileList_Save()
		{
			this.interactButLabel = "OverwriteButton".Translate();
			if (Faction.OfPlayer.HasName)
			{
				this.typingName = Faction.OfPlayer.Name;
				return;
			}
			this.typingName = SaveGameFilesUtility.UnusedDefaultFileName(Faction.OfPlayer.def.LabelCap);
		}

		// Token: 0x06011F1A RID: 73498 RVA: 0x00539314 File Offset: 0x00537514
		protected override void DoFileInteraction(string mapName)
		{
			mapName = GenFile.SanitizedFileName(mapName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveGame(mapName);
			}, "SavingLongEvent", false, null, true, false, null);
			Messages.Message("SavedAs".Translate(mapName), MessageTypeDefOf.SilentInput, false);
			PlayerKnowledgeDatabase.Save();
			this.Close(true);
		}
	}
}
