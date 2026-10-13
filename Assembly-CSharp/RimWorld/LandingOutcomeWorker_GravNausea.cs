using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020025B8 RID: 9656
	public class LandingOutcomeWorker_GravNausea : LandingOutcomeWorker
	{
		// Token: 0x0600D389 RID: 54153 RVA: 0x003E551B File Offset: 0x003E371B
		public LandingOutcomeWorker_GravNausea(LandingOutcomeDef def) : base(def)
		{
		}

		// Token: 0x0600D38A RID: 54154 RVA: 0x003E584C File Offset: 0x003E3A4C
		public override void ApplyOutcome(Gravship gravship)
		{
			foreach (Pawn pawn in gravship.Pawns)
			{
				if (pawn.RaceProps.IsFlesh)
				{
					Pawn_HealthTracker health = pawn.health;
					if (health != null)
					{
						health.AddHediff(HediffDefOf.GravNausea, null, null, null);
					}
				}
			}
			base.SendStandardLetter(gravship.Engine, null, new LookTargets(gravship.Pawns));
		}
	}
}
