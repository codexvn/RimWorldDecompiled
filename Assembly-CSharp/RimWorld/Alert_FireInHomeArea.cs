using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FC9 RID: 12233
	public class Alert_FireInHomeArea : Alert_Critical
	{
		// Token: 0x060113DE RID: 70622 RVA: 0x004F0B2C File Offset: 0x004EED2C
		public Alert_FireInHomeArea()
		{
			this.defaultLabel = "FireInHomeArea".Translate();
			this.defaultExplanation = "FireInHomeAreaDesc".Translate();
		}

		// Token: 0x17002CB3 RID: 11443
		// (get) Token: 0x060113DF RID: 70623 RVA: 0x004F0B60 File Offset: 0x004EED60
		private Fire FireInHomeArea
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.Fire);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						if (maps[i].areaManager.Home[thing.Position] && !thing.Position.Fogged(thing.Map))
						{
							return (Fire)thing;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x060113E0 RID: 70624 RVA: 0x004F0BF2 File Offset: 0x004EEDF2
		public override AlertReport GetReport()
		{
			return this.FireInHomeArea;
		}
	}
}
