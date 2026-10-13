using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F78 RID: 3960
	public class MentalState_Manhunter : MentalState
	{
		// Token: 0x060061E3 RID: 25059 RVA: 0x001EF4A0 File Offset: 0x001ED6A0
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AnimalsDontAttackDoors, OpportunityType.Critical);
		}

		// Token: 0x060061E4 RID: 25060 RVA: 0x001EF4B4 File Offset: 0x001ED6B4
		public override bool ForceHostileTo(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null && pawn.RaceProps.Roamer)
			{
				return false;
			}
			if (t.Faction == null)
			{
				Pawn pawn2 = t as Pawn;
				return pawn2 != null && pawn2.RaceProps.Humanlike;
			}
			return this.ForceHostileTo(t.Faction);
		}

		// Token: 0x060061E5 RID: 25061 RVA: 0x001EF506 File Offset: 0x001ED706
		public override bool ForceHostileTo(Faction f)
		{
			return (ModsConfig.AnomalyActive && f == Faction.OfEntities) || f.def.humanlikeFaction || f == Faction.OfMechanoids;
		}

		// Token: 0x060061E6 RID: 25062 RVA: 0x00002501 File Offset: 0x00000701
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
