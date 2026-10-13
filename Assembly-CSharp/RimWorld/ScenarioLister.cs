using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002869 RID: 10345
	public static class ScenarioLister
	{
		// Token: 0x0600E25B RID: 57947 RVA: 0x00424BE2 File Offset: 0x00422DE2
		public static IEnumerable<Scenario> AllScenarios()
		{
			ScenarioLister.RecacheIfDirty();
			foreach (ScenarioDef scenarioDef in DefDatabase<ScenarioDef>.AllDefs)
			{
				yield return scenarioDef.scenario;
			}
			IEnumerator<ScenarioDef> enumerator = null;
			foreach (Scenario scenario in ScenarioFiles.AllScenariosLocal)
			{
				yield return scenario;
			}
			IEnumerator<Scenario> enumerator2 = null;
			foreach (Scenario scenario2 in ScenarioFiles.AllScenariosWorkshop)
			{
				yield return scenario2;
			}
			enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x0600E25C RID: 57948 RVA: 0x00424BEB File Offset: 0x00422DEB
		public static IEnumerable<Scenario> ScenariosInCategory(ScenarioCategory cat)
		{
			ScenarioLister.RecacheIfDirty();
			if (cat == ScenarioCategory.FromDef)
			{
				foreach (ScenarioDef scenarioDef in DefDatabase<ScenarioDef>.AllDefs)
				{
					yield return scenarioDef.scenario;
				}
				IEnumerator<ScenarioDef> enumerator = null;
			}
			else if (cat == ScenarioCategory.CustomLocal)
			{
				foreach (Scenario scenario in ScenarioFiles.AllScenariosLocal)
				{
					yield return scenario;
				}
				IEnumerator<Scenario> enumerator2 = null;
			}
			else if (cat == ScenarioCategory.SteamWorkshop)
			{
				foreach (Scenario scenario2 in ScenarioFiles.AllScenariosWorkshop)
				{
					yield return scenario2;
				}
				IEnumerator<Scenario> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600E25D RID: 57949 RVA: 0x00424BFC File Offset: 0x00422DFC
		public static bool ScenarioIsListedAnywhere(Scenario scen)
		{
			ScenarioLister.RecacheIfDirty();
			using (IEnumerator<ScenarioDef> enumerator = DefDatabase<ScenarioDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.scenario == scen)
					{
						return true;
					}
				}
			}
			foreach (Scenario scenario in ScenarioFiles.AllScenariosLocal)
			{
				if (scen == scenario)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E25E RID: 57950 RVA: 0x00424C90 File Offset: 0x00422E90
		public static void MarkDirty()
		{
			ScenarioLister.dirty = true;
		}

		// Token: 0x0600E25F RID: 57951 RVA: 0x00424C98 File Offset: 0x00422E98
		private static void RecacheIfDirty()
		{
			if (ScenarioLister.dirty)
			{
				ScenarioLister.RecacheData();
			}
		}

		// Token: 0x0600E260 RID: 57952 RVA: 0x00424CA8 File Offset: 0x00422EA8
		private static void RecacheData()
		{
			ScenarioLister.dirty = false;
			int num = ScenarioLister.ScenarioListHash();
			ScenarioFiles.RecacheData();
			if (ScenarioLister.ScenarioListHash() != num && !LongEventHandler.ShouldWaitForEvent)
			{
				Page_SelectScenario page_SelectScenario = Find.WindowStack.WindowOfType<Page_SelectScenario>();
				if (page_SelectScenario != null)
				{
					page_SelectScenario.Notify_ScenarioListChanged();
				}
			}
		}

		// Token: 0x0600E261 RID: 57953 RVA: 0x00424CEC File Offset: 0x00422EEC
		public static int ScenarioListHash()
		{
			int num = 9826121;
			foreach (Scenario scenario in ScenarioLister.AllScenarios())
			{
				num ^= 791 * scenario.GetHashCode() * 6121;
			}
			return num;
		}

		// Token: 0x040097E4 RID: 38884
		private static bool dirty = true;
	}
}
