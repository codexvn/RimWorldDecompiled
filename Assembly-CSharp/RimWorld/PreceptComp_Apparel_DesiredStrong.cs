using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002635 RID: 9781
	public class PreceptComp_Apparel_DesiredStrong : PreceptComp_Apparel
	{
		// Token: 0x0600D659 RID: 54873 RVA: 0x003F9BD4 File Offset: 0x003F7DD4
		public override void Notify_MemberGenerated(Pawn pawn, Precept precept, bool newborn, bool ignoreApparel = false)
		{
			if (newborn || !base.AppliesToPawn(pawn, precept) || ignoreApparel)
			{
				return;
			}
			Precept_Apparel precept_Apparel = (Precept_Apparel)precept;
			foreach (Apparel apparel in pawn.apparel.WornApparel)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, precept_Apparel.apparelDef, pawn.RaceProps.body))
				{
					List<ThingDef> apparelRequired = pawn.kindDef.apparelRequired;
					if (apparelRequired != null && apparelRequired.Contains(apparel.def))
					{
						return;
					}
				}
			}
			base.GiveApparelToPawn(pawn, precept_Apparel);
		}
	}
}
