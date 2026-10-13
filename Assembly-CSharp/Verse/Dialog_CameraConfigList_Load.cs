using System;

namespace Verse
{
	// Token: 0x020001E0 RID: 480
	public class Dialog_CameraConfigList_Load : Dialog_CameraConfigList
	{
		// Token: 0x06000D00 RID: 3328 RVA: 0x0003DE06 File Offset: 0x0003C006
		public Dialog_CameraConfigList_Load(Action<CameraMapConfig> cfgReturner)
		{
			this.configReturner = cfgReturner;
			this.interactButLabel = "Load config";
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0003DE20 File Offset: 0x0003C020
		protected override void DoFileInteraction(string fileName)
		{
			string filePath = GenFilePaths.AbsFilePathForCameraConfig(fileName);
			PreLoadUtility.CheckVersionAndLoad(filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.CameraConfig, delegate
			{
				CameraMapConfig obj;
				if (GameDataSaveLoader.TryLoadCameraConfig(filePath, out obj))
				{
					this.configReturner(obj);
				}
				this.Close(true);
			}, false);
		}

		// Token: 0x0400078A RID: 1930
		private Action<CameraMapConfig> configReturner;
	}
}
