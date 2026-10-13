using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B70 RID: 7024
	public class HistoryAutoRecorderWorker_WealthPawns : HistoryAutoRecorderWorker
	{
		// Token: 0x06009CBA RID: 40122 RVA: 0x002D7424 File Offset: 0x002D5624
		public override float PullRecord()
		{
			float num = 0f;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome)
				{
					num += maps[i].wealthWatcher.WealthPawns;
				}
			}
			return num;
		}
	}
}
