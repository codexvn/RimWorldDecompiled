using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DA2 RID: 7586
	public class IncidentWorker_GoldenCubeArrival : IncidentWorker_MeteoriteImpact
	{
		// Token: 0x0600A9A5 RID: 43429 RVA: 0x00308D9C File Offset: 0x00306F9C
		protected override List<Thing> GenerateMeteorContents(IncidentParms parms)
		{
			Thing item = ThingMaker.MakeThing(ThingDefOf.GoldenCube, null);
			if (!QuestUtility.TryGetIdealColonist(out this.affectedPawn, (Map)parms.target, new Func<Pawn, bool>(this.ValidatePawn)))
			{
				return null;
			}
			this.affectedPawn.health.AddHediff(HediffDefOf.CubeInterest, null, null, null);
			return new List<Thing>
			{
				item
			};
		}

		// Token: 0x0600A9A6 RID: 43430 RVA: 0x00308E08 File Offset: 0x00307008
		protected override Letter MakeLetter(Skyfaller meteorite, List<Thing> contents)
		{
			return LetterMaker.MakeLetter("GoldenCubeArrivalLabel".Translate(), "GoldenCubeArrivalText".Translate(this.affectedPawn.Named("PAWN")), LetterDefOf.ThreatSmall, new TargetInfo(meteorite.Position, meteorite.Map, false), null, null, null);
		}

		// Token: 0x0600A9A7 RID: 43431 RVA: 0x00308E60 File Offset: 0x00307060
		private bool ValidatePawn(Pawn pawn)
		{
			return (pawn.IsColonist || pawn.IsSlaveOfColony) && !pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false) && !pawn.health.hediffSet.HasHediff(HediffDefOf.CubeComa, false);
		}

		// Token: 0x04007548 RID: 30024
		private Pawn affectedPawn;
	}
}
