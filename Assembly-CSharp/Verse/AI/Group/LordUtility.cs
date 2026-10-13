using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020010A9 RID: 4265
	public static class LordUtility
	{
		// Token: 0x060068B4 RID: 26804 RVA: 0x00211277 File Offset: 0x0020F477
		public static Lord GetLord(this Pawn p)
		{
			return p.lord;
		}

		// Token: 0x060068B5 RID: 26805 RVA: 0x0021127F File Offset: 0x0020F47F
		public static bool TryGetLord(this Pawn p, out Lord lord)
		{
			lord = p.GetLord();
			return lord != null;
		}

		// Token: 0x060068B6 RID: 26806 RVA: 0x00211290 File Offset: 0x0020F490
		public static Lord GetLord(this Building b)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Lord lord = maps[i].lordManager.LordOf(b);
				if (lord != null)
				{
					return lord;
				}
			}
			return null;
		}

		// Token: 0x060068B7 RID: 26807 RVA: 0x002112D0 File Offset: 0x0020F4D0
		public static Lord GetLord(this Corpse c)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Lord lord = maps[i].lordManager.LordOf(c);
				if (lord != null)
				{
					return lord;
				}
			}
			return null;
		}
	}
}
