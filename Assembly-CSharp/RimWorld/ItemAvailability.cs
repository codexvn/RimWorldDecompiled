using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001564 RID: 5476
	public class ItemAvailability
	{
		// Token: 0x060081E9 RID: 33257 RVA: 0x00271AC7 File Offset: 0x0026FCC7
		public ItemAvailability(Map map)
		{
			this.map = map;
		}

		// Token: 0x060081EA RID: 33258 RVA: 0x00271AE1 File Offset: 0x0026FCE1
		public void Tick()
		{
			this.cachedResults.Clear();
		}

		// Token: 0x060081EB RID: 33259 RVA: 0x00271AF0 File Offset: 0x0026FCF0
		public bool ThingsAvailableAnywhere(ThingDef need, int amount, Pawn pawn)
		{
			int key = Gen.HashCombine<Faction>(need.GetHashCode(), pawn.Faction);
			bool flag;
			if (!this.cachedResults.TryGetValue(key, out flag))
			{
				List<Thing> list = this.map.listerThings.ThingsOfDef(need);
				int num = 0;
				for (int i = 0; i < list.Count; i++)
				{
					if (!list[i].IsForbidden(pawn))
					{
						num += list[i].stackCount;
						if (num >= amount)
						{
							break;
						}
					}
				}
				flag = (num >= amount);
				this.cachedResults.Add(key, flag);
			}
			return flag;
		}

		// Token: 0x0400566D RID: 22125
		private readonly Map map;

		// Token: 0x0400566E RID: 22126
		private readonly Dictionary<int, bool> cachedResults = new Dictionary<int, bool>();
	}
}
