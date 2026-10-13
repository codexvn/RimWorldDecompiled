using System;
using System.Collections.Generic;
using System.IO;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02002868 RID: 10344
	public static class ScenarioFiles
	{
		// Token: 0x1700227F RID: 8831
		// (get) Token: 0x0600E257 RID: 57943 RVA: 0x00424AFB File Offset: 0x00422CFB
		public static IEnumerable<Scenario> AllScenariosLocal
		{
			get
			{
				return ScenarioFiles.scenariosLocal;
			}
		}

		// Token: 0x17002280 RID: 8832
		// (get) Token: 0x0600E258 RID: 57944 RVA: 0x00424B02 File Offset: 0x00422D02
		public static IEnumerable<Scenario> AllScenariosWorkshop
		{
			get
			{
				return ScenarioFiles.scenariosWorkshop;
			}
		}

		// Token: 0x0600E259 RID: 57945 RVA: 0x00424B0C File Offset: 0x00422D0C
		public static void RecacheData()
		{
			ScenarioFiles.scenariosLocal.Clear();
			using (IEnumerator<FileInfo> enumerator = GenFilePaths.AllCustomScenarioFiles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Scenario item;
					if (GameDataSaveLoader.TryLoadScenario(enumerator.Current.FullName, ScenarioCategory.CustomLocal, out item))
					{
						ScenarioFiles.scenariosLocal.Add(item);
					}
				}
			}
			ScenarioFiles.scenariosWorkshop.Clear();
			foreach (WorkshopItem workshopItem in WorkshopItems.AllSubscribedItems)
			{
				WorkshopItem_Scenario workshopItem_Scenario = workshopItem as WorkshopItem_Scenario;
				if (workshopItem_Scenario != null)
				{
					ScenarioFiles.scenariosWorkshop.Add(workshopItem_Scenario.GetScenario());
				}
			}
		}

		// Token: 0x040097E2 RID: 38882
		private static readonly List<Scenario> scenariosLocal = new List<Scenario>();

		// Token: 0x040097E3 RID: 38883
		private static readonly List<Scenario> scenariosWorkshop = new List<Scenario>();
	}
}
