using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020031D8 RID: 12760
	public class Dialog_ScenarioList_Load : Dialog_ScenarioList
	{
		// Token: 0x06011F25 RID: 73509 RVA: 0x00539517 File Offset: 0x00537717
		public Dialog_ScenarioList_Load(Action<Scenario> scenarioReturner)
		{
			this.interactButLabel = "LoadGameButton".Translate();
			this.scenarioReturner = scenarioReturner;
		}

		// Token: 0x06011F26 RID: 73510 RVA: 0x0053953C File Offset: 0x0053773C
		protected override void DoFileInteraction(string fileName)
		{
			string filePath = GenFilePaths.AbsPathForScenario(fileName);
			PreLoadUtility.CheckVersionAndLoad(filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.Scenario, delegate
			{
				Scenario obj;
				if (GameDataSaveLoader.TryLoadScenario(filePath, ScenarioCategory.CustomLocal, out obj))
				{
					this.scenarioReturner(obj);
				}
				this.Close(true);
			}, false);
		}

		// Token: 0x0400B9D3 RID: 47571
		private Action<Scenario> scenarioReturner;
	}
}
