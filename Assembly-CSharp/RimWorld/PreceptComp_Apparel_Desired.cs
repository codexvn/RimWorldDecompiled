using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002634 RID: 9780
	public class PreceptComp_Apparel_Desired : PreceptComp_Apparel
	{
		// Token: 0x0600D657 RID: 54871 RVA: 0x003F9AFC File Offset: 0x003F7CFC
		public override void Notify_MemberGenerated(Pawn pawn, Precept precept, bool newborn, bool ignoreApparel = false)
		{
			if (newborn || !base.AppliesToPawn(pawn, precept) || ignoreApparel)
			{
				return;
			}
			Precept_Apparel precept_Apparel = (Precept_Apparel)precept;
			foreach (Apparel apparel in pawn.apparel.WornApparel)
			{
				if (ApparelUtility.CanWearTogether(apparel.def, precept_Apparel.apparelDef, pawn.RaceProps.body) || (!apparel.HasThingCategory(ThingCategoryDefOf.ArmorHeadgear) && !apparel.HasThingCategory(ThingCategoryDefOf.ApparelArmor)))
				{
					List<ThingDef> apparelRequired = pawn.kindDef.apparelRequired;
					if (apparelRequired == null || !apparelRequired.Contains(apparel.def))
					{
						continue;
					}
				}
				return;
			}
			base.GiveApparelToPawn(pawn, precept_Apparel);
		}
	}
}
