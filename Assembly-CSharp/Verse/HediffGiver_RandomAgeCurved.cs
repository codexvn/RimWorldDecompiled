using System;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000781 RID: 1921
	public class HediffGiver_RandomAgeCurved : HediffGiver
	{
		// Token: 0x06002EF6 RID: 12022 RVA: 0x000F159E File Offset: 0x000EF79E
		public override float ChanceFactor(Pawn pawn)
		{
			if (pawn.IsMutant && pawn.mutant.Def.disableAging)
			{
				return 0f;
			}
			return base.ChanceFactor(pawn);
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x000F15C8 File Offset: 0x000EF7C8
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			float x = (float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy;
			if (Rand.MTBEventOccurs(this.ageFractionMtbDaysCurve.Evaluate(x), 60000f, 60f))
			{
				if (this.minPlayerPopulation > 0 && pawn.Faction == Faction.OfPlayer && PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended.Count<Pawn>() < this.minPlayerPopulation)
				{
					return;
				}
				if (base.TryApply(pawn, null))
				{
					base.SendLetter(pawn, cause);
				}
			}
		}

		// Token: 0x040024BA RID: 9402
		public SimpleCurve ageFractionMtbDaysCurve;

		// Token: 0x040024BB RID: 9403
		public int minPlayerPopulation;
	}
}
