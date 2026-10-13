using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001401 RID: 5121
	public class JobGiver_AIDefendMaster : JobGiver_AIDefendPawn
	{
		// Token: 0x06007CCF RID: 31951 RVA: 0x00259633 File Offset: 0x00257833
		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn.playerSettings.Master;
		}

		// Token: 0x06007CD0 RID: 31952 RVA: 0x00259640 File Offset: 0x00257840
		protected override float GetFlagRadius(Pawn pawn)
		{
			if (pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release))
			{
				return 50f;
			}
			return 5f;
		}

		// Token: 0x04005412 RID: 21522
		private const float RadiusUnreleased = 5f;
	}
}
