using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020025B7 RID: 9655
	public class LandingOutcomeWorker_OverheatedGravEngine : LandingOutcomeWorker
	{
		// Token: 0x0600D387 RID: 54151 RVA: 0x003E551B File Offset: 0x003E371B
		public LandingOutcomeWorker_OverheatedGravEngine(LandingOutcomeDef def) : base(def)
		{
		}

		// Token: 0x0600D388 RID: 54152 RVA: 0x003E5810 File Offset: 0x003E3A10
		public override void ApplyOutcome(Gravship gravship)
		{
			gravship.Engine.cooldownCompleteTick += Rand.Range(120000, 240000);
			base.SendStandardLetter(gravship.Engine, null, gravship.Engine);
		}
	}
}
