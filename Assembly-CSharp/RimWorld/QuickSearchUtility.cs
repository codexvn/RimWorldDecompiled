using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200388E RID: 14478
	public static class QuickSearchUtility
	{
		// Token: 0x1700330A RID: 13066
		// (get) Token: 0x06014020 RID: 81952 RVA: 0x005F0EB8 File Offset: 0x005EF0B8
		private static string[] SearchingTexts
		{
			get
			{
				if (QuickSearchUtility.searchingTexts == null)
				{
					string str = "Searching".Translate();
					QuickSearchUtility.searchingTexts = new string[]
					{
						str + ".",
						str + "..",
						str + "..."
					};
				}
				return QuickSearchUtility.searchingTexts;
			}
		}

		// Token: 0x1700330B RID: 13067
		// (get) Token: 0x06014021 RID: 81953 RVA: 0x005F0F16 File Offset: 0x005EF116
		public static string CurrentSearchText
		{
			get
			{
				return QuickSearchUtility.SearchingTexts[Time.frameCount / 20 % QuickSearchUtility.searchingTexts.Length];
			}
		}

		// Token: 0x06014022 RID: 81954 RVA: 0x005F0F2E File Offset: 0x005EF12E
		public static void ResetStaticData()
		{
			QuickSearchUtility.searchingTexts = null;
		}

		// Token: 0x0400DD19 RID: 56601
		private static string[] searchingTexts;
	}
}
