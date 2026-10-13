using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DB5 RID: 7605
	public class IncidentWorker_MetalhorrorImplantation : IncidentWorker
	{
		// Token: 0x0600A9E3 RID: 43491 RVA: 0x00309B40 File Offset: 0x00307D40
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return Find.Anomaly.CanNewMetalhorrorBiosignatureImplantOccur && this.GetPossiblePawns(map).Any<Pawn>();
		}

		// Token: 0x0600A9E4 RID: 43492 RVA: 0x00309B74 File Offset: 0x00307D74
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			this.GetAllPawnInfectionPathways(IncidentWorker_MetalhorrorImplantation.TmpPathways, map);
			if (IncidentWorker_MetalhorrorImplantation.TmpPathways.Empty<InfectionPathway>())
			{
				return false;
			}
			InfectionPathway infectionPathway = IncidentWorker_MetalhorrorImplantation.TmpPathways.RandomElementByWeight((InfectionPathway pathway) => IncidentWorker_MetalhorrorImplantation.PathwayAgeWeightCurve.Evaluate(pathway.AgePercent));
			IncidentWorker_MetalhorrorImplantation.TmpPathways.Clear();
			string explanation = infectionPathway.GetExplanation(HediffDefOf.MetalhorrorImplant);
			MetalhorrorUtility.Infect(infectionPathway.OwnerPawn, null, "InsectImplant", explanation);
			return true;
		}

		// Token: 0x0600A9E5 RID: 43493 RVA: 0x00309BF8 File Offset: 0x00307DF8
		private List<Pawn> GetPossiblePawns(Map map)
		{
			List<Pawn> freeColonistsAndPrisoners = map.mapPawns.FreeColonistsAndPrisoners;
			for (int i = freeColonistsAndPrisoners.Count - 1; i >= 0; i--)
			{
				Pawn pawn = freeColonistsAndPrisoners[i];
				if (!MetalhorrorUtility.CanBeInfected(pawn) || !pawn.infectionVectors.AnyPathwayForHediff(HediffDefOf.MetalhorrorImplant))
				{
					freeColonistsAndPrisoners.RemoveAt(i);
				}
			}
			return freeColonistsAndPrisoners;
		}

		// Token: 0x0600A9E6 RID: 43494 RVA: 0x00309C50 File Offset: 0x00307E50
		private void GetAllPawnInfectionPathways(List<InfectionPathway> pathways, Map map)
		{
			pathways.Clear();
			List<Pawn> freeColonistsAndPrisoners = map.mapPawns.FreeColonistsAndPrisoners;
			for (int i = 0; i < freeColonistsAndPrisoners.Count; i++)
			{
				Pawn pawn = freeColonistsAndPrisoners[i];
				if (MetalhorrorUtility.CanBeInfected(pawn))
				{
					pathways.AddRange(pawn.infectionVectors.GetPathwaysForHediff(HediffDefOf.MetalhorrorImplant));
				}
			}
		}

		// Token: 0x0400755D RID: 30045
		private static readonly SimpleCurve PathwayAgeWeightCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(0.7f, 0.3f),
				true
			}
		};

		// Token: 0x0400755E RID: 30046
		private static readonly List<InfectionPathway> TmpPathways = new List<InfectionPathway>();
	}
}
