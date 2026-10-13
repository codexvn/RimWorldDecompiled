using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020031CF RID: 12751
	public class Dialog_ModList_Load : Dialog_ModList
	{
		// Token: 0x06011F0A RID: 73482 RVA: 0x00539037 File Offset: 0x00537237
		public Dialog_ModList_Load(Action<ModList> modListReturner)
		{
			this.interactButLabel = "Load".Translate();
			this.modListReturner = modListReturner;
		}

		// Token: 0x06011F0B RID: 73483 RVA: 0x0053905C File Offset: 0x0053725C
		protected override void DoFileInteraction(string fileName)
		{
			string text = GenFilePaths.AbsFilePathForModList(fileName);
			try
			{
				Scribe.loader.InitLoadingMetaHeaderOnly(text);
				ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.ModList, false);
				Scribe.loader.FinalizeLoading();
				ModList obj = null;
				if (GameDataSaveLoader.TryLoadModList(text, out obj))
				{
					this.modListReturner(obj);
				}
				this.Close(true);
			}
			catch (Exception ex)
			{
				string str = "Exception loading ";
				string str2 = text;
				string str3 = ": ";
				Exception ex2 = ex;
				Log.Warning(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				Scribe.ForceStop();
			}
		}

		// Token: 0x0400B9C9 RID: 47561
		private Action<ModList> modListReturner;
	}
}
