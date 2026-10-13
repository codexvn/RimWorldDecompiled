using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FA0 RID: 12192
	public static class CountChanceUtility
	{
		// Token: 0x060112B5 RID: 70325 RVA: 0x004EBE1C File Offset: 0x004EA01C
		public static int RandomCount(List<CountChance> chances)
		{
			float value = Rand.Value;
			float num = 0f;
			for (int i = 0; i < chances.Count; i++)
			{
				num += chances[i].chance;
				if (value < num)
				{
					if (num > 1f)
					{
						Log.Error("CountChances error: Total chance is " + num.ToString() + " but it should not be above 1.");
					}
					return chances[i].count;
				}
			}
			return 0;
		}
	}
}
