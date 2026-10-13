using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020031DF RID: 12767
	public class Dialog_XenotypeList_Load : Dialog_XenotypeList
	{
		// Token: 0x06011F39 RID: 73529 RVA: 0x00539CF0 File Offset: 0x00537EF0
		public Dialog_XenotypeList_Load(Action<CustomXenotype> xenotypeReturner)
		{
			this.xenotypeReturner = xenotypeReturner;
			this.interactButLabel = "LoadGameButton".Translate();
			this.deleteTipKey = "DeleteThisXenotype";
		}

		// Token: 0x06011F3A RID: 73530 RVA: 0x00539D20 File Offset: 0x00537F20
		protected override void DoFileInteraction(string fileName)
		{
			string filePath = GenFilePaths.AbsFilePathForXenotype(fileName);
			PreLoadUtility.CheckVersionAndLoad(filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.Xenotype, delegate
			{
				CustomXenotype obj;
				if (GameDataSaveLoader.TryLoadXenotype(filePath, out obj))
				{
					this.xenotypeReturner(obj);
				}
				this.Close(true);
			}, false);
		}

		// Token: 0x0400B9E1 RID: 47585
		private Action<CustomXenotype> xenotypeReturner;
	}
}
