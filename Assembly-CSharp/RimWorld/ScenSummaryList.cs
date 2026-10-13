using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020028FF RID: 10495
	public static class ScenSummaryList
	{
		// Token: 0x0600E57F RID: 58751 RVA: 0x0042F510 File Offset: 0x0042D710
		public static string SummaryWithList(Scenario scen, string tag, string intro)
		{
			string text = ScenSummaryList.SummaryList(scen, tag);
			if (!text.NullOrEmpty())
			{
				return "\n" + intro + ":\n" + text;
			}
			return null;
		}

		// Token: 0x0600E580 RID: 58752 RVA: 0x0042F540 File Offset: 0x0042D740
		private static string SummaryList(Scenario scen, string tag)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (ScenPart scenPart in scen.AllParts)
			{
				if (!scenPart.summarized)
				{
					foreach (string str in scenPart.GetSummaryListEntries(tag))
					{
						if (!flag)
						{
							stringBuilder.Append("\n");
						}
						stringBuilder.Append("   -" + str);
						scenPart.summarized = true;
						flag = false;
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
