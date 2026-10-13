using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001515 RID: 5397
	public class JoyGiver_Meditate : JoyGiver_InPrivateRoom
	{
		// Token: 0x0600809B RID: 32923 RVA: 0x0026C19E File Offset: 0x0026A39E
		public override Job TryGiveJob(Pawn pawn)
		{
			if (ModsConfig.RoyaltyActive)
			{
				return MeditationUtility.GetMeditationJob(pawn, true);
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x0600809C RID: 32924 RVA: 0x0026C1B6 File Offset: 0x0026A3B6
		public override bool CanBeGivenTo(Pawn pawn)
		{
			return (!ModsConfig.RoyaltyActive || MeditationUtility.CanMeditateNow(pawn)) && base.CanBeGivenTo(pawn);
		}
	}
}
