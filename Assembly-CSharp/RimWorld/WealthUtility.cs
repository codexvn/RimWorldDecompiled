using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003777 RID: 14199
	public class WealthUtility
	{
		// Token: 0x17003251 RID: 12881
		// (get) Token: 0x06013ACD RID: 80589 RVA: 0x005D8908 File Offset: 0x005D6B08
		public static float PlayerWealth
		{
			get
			{
				List<Map> maps = Find.Maps;
				float num = 0f;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						num += maps[i].wealthWatcher.WealthTotal;
					}
				}
				return num;
			}
		}
	}
}
