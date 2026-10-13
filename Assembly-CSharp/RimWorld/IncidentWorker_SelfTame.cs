using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DCA RID: 7626
	public class IncidentWorker_SelfTame : IncidentWorker
	{
		// Token: 0x0600AA2C RID: 43564 RVA: 0x0030AE53 File Offset: 0x00309053
		private IEnumerable<Pawn> Candidates(Map map)
		{
			return from x in map.mapPawns.AllPawnsSpawned
			where x.IsAnimal && x.Faction == null && !x.Position.Fogged(x.Map) && !x.InMentalState && !x.Downed && !x.health.hediffSet.HasHediff(HediffDefOf.Scaria, false) && x.GetStatValue(StatDefOf.Wildness, true, -1) > 0f
			select x;
		}

		// Token: 0x0600AA2D RID: 43565 RVA: 0x0030AE84 File Offset: 0x00309084
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return this.Candidates(map).Any<Pawn>();
		}

		// Token: 0x0600AA2E RID: 43566 RVA: 0x0030AEAC File Offset: 0x003090AC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Pawn pawn = null;
			if (!this.Candidates(map).TryRandomElement(out pawn))
			{
				return false;
			}
			string value = pawn.LabelIndefinite();
			bool flag = pawn.Name != null;
			pawn.SetFaction(Faction.OfPlayer, null);
			string str;
			if (!flag && pawn.Name != null)
			{
				if (pawn.Name.Numerical)
				{
					str = "LetterAnimalSelfTameAndNameNumerical".Translate(value, pawn.Name.ToStringFull, pawn.Named("ANIMAL")).CapitalizeFirst();
				}
				else
				{
					str = "LetterAnimalSelfTameAndName".Translate(value, pawn.Name.ToStringFull, pawn.Named("ANIMAL")).CapitalizeFirst();
				}
			}
			else
			{
				str = "LetterAnimalSelfTame".Translate(pawn).CapitalizeFirst();
			}
			base.SendStandardLetter("LetterLabelAnimalSelfTame".Translate(pawn.KindLabel, pawn).CapitalizeFirst(), str, LetterDefOf.PositiveEvent, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}
	}
}
