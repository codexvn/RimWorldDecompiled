using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020001BB RID: 443
	public class ConditionalStatAffecter_Unclothed : ConditionalStatAffecter
	{
		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x0003A4D8 File Offset: 0x000386D8
		public override string Label
		{
			get
			{
				return "StatsReport_Unclothed".Translate();
			}
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x0003A4EC File Offset: 0x000386EC
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
								return false;
							}
						}
					}
					return true;
				}
			}
			return true;
		}
	}
}
