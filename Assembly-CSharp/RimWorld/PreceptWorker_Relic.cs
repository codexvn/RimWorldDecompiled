using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002668 RID: 9832
	public class PreceptWorker_Relic : PreceptWorker
	{
		// Token: 0x0600D7AE RID: 55214 RVA: 0x003FF653 File Offset: 0x003FD853
		public override float GetThingOrder(PreceptThingChance thingChance)
		{
			return -thingChance.chance;
		}

		// Token: 0x17002107 RID: 8455
		// (get) Token: 0x0600D7AF RID: 55215 RVA: 0x003FF65C File Offset: 0x003FD85C
		public override IEnumerable<PreceptThingChance> ThingDefs
		{
			get
			{
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
				{
					if (thingDef.relicChance != 0f)
					{
						yield return new PreceptThingChance
						{
							chance = thingDef.relicChance,
							def = thingDef
						};
					}
				}
				List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
				yield break;
				yield break;
			}
		}
	}
}
