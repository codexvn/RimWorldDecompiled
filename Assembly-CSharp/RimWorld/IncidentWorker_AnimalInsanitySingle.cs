using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D7D RID: 7549
	public class IncidentWorker_AnimalInsanitySingle : IncidentWorker
	{
		// Token: 0x0600A91C RID: 43292 RVA: 0x00306558 File Offset: 0x00304758
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			Pawn pawn;
			return this.TryFindRandomAnimal(map, out pawn);
		}

		// Token: 0x0600A91D RID: 43293 RVA: 0x00306588 File Offset: 0x00304788
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Pawn pawn;
			if (!this.TryFindRandomAnimal(map, out pawn))
			{
				return false;
			}
			IncidentWorker_AnimalInsanityMass.DriveInsane(pawn);
			string str = "AnimalInsanitySingle".Translate(pawn.Label, pawn.Named("ANIMAL")).CapitalizeFirst();
			base.SendStandardLetter("LetterLabelAnimalInsanitySingle".Translate(pawn.Label, pawn.Named("ANIMAL")).CapitalizeFirst(), str, LetterDefOf.ThreatSmall, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600A91E RID: 43294 RVA: 0x00306628 File Offset: 0x00304828
		private bool TryFindRandomAnimal(Map map, out Pawn animal)
		{
			int maxPoints = 150;
			if (GenDate.DaysPassedSinceSettle < 7)
			{
				maxPoints = 40;
			}
			return (from p in map.mapPawns.AllPawnsSpawned
			where p.IsAnimal && p.kindDef.combatPower <= (float)maxPoints && IncidentWorker_AnimalInsanityMass.AnimalUsable(p)
			select p).TryRandomElement(out animal);
		}

		// Token: 0x0400750A RID: 29962
		private const int FixedPoints = 30;
	}
}
