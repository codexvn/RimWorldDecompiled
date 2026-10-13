using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001BA RID: 442
	public class ConditionalStatAffecter_Clothed : ConditionalStatAffecter
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x0003A430 File Offset: 0x00038630
		public override string Label
		{
			get
			{
				return "StatsReport_Clothed".Translate();
			}
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x0003A444 File Offset: 0x00038644
		public override bool Applies(StatRequest req)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.apparel != null)
				{
					using (List<Apparel>.Enumerator enumerator = pawn.apparel.WornApparel.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.def.apparel.countsAsClothingForNudity)
							{
								return true;
							}
						}
					}
					return false;
				}
			}
			return false;
		}
	}
}
