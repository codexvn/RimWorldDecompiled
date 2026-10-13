using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020001DE RID: 478
	public class Dialog_CameraConfigList_Save : Dialog_CameraConfigList
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool ShouldDoTypeInField
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x0003DD5C File Offset: 0x0003BF5C
		public Dialog_CameraConfigList_Save(CameraMapConfig config)
		{
			this.interactButLabel = "OverwriteButton".Translate();
			this.config = config;
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0003DD80 File Offset: 0x0003BF80
		protected override void DoFileInteraction(string fileName)
		{
			fileName = GenFile.SanitizedFileName(fileName);
			string absPath = GenFilePaths.AbsFilePathForCameraConfig(fileName);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GameDataSaveLoader.SaveCameraConfig(this.config, absPath);
			}, "SavingLongEvent", false, null, true, false, null);
			Messages.Message("SavedAs".Translate(fileName), MessageTypeDefOf.SilentInput, false);
			this.Close(true);
		}

		// Token: 0x04000787 RID: 1927
		private CameraMapConfig config;
	}
}
