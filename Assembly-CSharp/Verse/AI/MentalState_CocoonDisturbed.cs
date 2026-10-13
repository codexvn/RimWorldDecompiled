using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F80 RID: 3968
	public class MentalState_CocoonDisturbed : MentalState
	{
		// Token: 0x06006203 RID: 25091 RVA: 0x001EF9C4 File Offset: 0x001EDBC4
		public override bool ForceHostileTo(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (pawn.RaceProps.Insect)
				{
					return false;
				}
				if (pawn.RaceProps.Animal && (pawn.RaceProps.Roamer || pawn.Faction == null))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006204 RID: 25092 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x06006205 RID: 25093 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
