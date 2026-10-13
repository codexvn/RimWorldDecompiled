using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001709 RID: 5897
	public class LordToil_KidnapCover : LordToil_DoOpportunisticTaskOrCover
	{
		// Token: 0x17001550 RID: 5456
		// (get) Token: 0x06008AEA RID: 35562 RVA: 0x00296BB3 File Offset: 0x00294DB3
		protected override DutyDef DutyDef
		{
			get
			{
				return DutyDefOf.Kidnap;
			}
		}

		// Token: 0x17001551 RID: 5457
		// (get) Token: 0x06008AEB RID: 35563 RVA: 0x00296BBA File Offset: 0x00294DBA
		public override bool ForceHighStoryDanger
		{
			get
			{
				return this.cover;
			}
		}

		// Token: 0x17001552 RID: 5458
		// (get) Token: 0x06008AEC RID: 35564 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSelfTend
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008AED RID: 35565 RVA: 0x00296BC4 File Offset: 0x00294DC4
		protected override bool TryFindGoodOpportunisticTaskTarget(Pawn pawn, out Thing target, List<Thing> alreadyTakenTargets)
		{
			if (pawn.mindState.duty != null && pawn.mindState.duty.def == this.DutyDef && pawn.carryTracker.CarriedThing is Pawn)
			{
				target = pawn.carryTracker.CarriedThing;
				return true;
			}
			Pawn pawn2;
			bool result = KidnapAIUtility.TryFindGoodKidnapVictim(pawn, 8f, out pawn2, alreadyTakenTargets);
			target = pawn2;
			return result;
		}
	}
}
