using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002705 RID: 9989
	public class RitualObligationTargetWorker_AnyRitualSpotOrAltar_Scarification : RitualObligationTargetWorker_AnyRitualSpotOrAltar
	{
		// Token: 0x0600DB33 RID: 56115 RVA: 0x0040B2D3 File Offset: 0x004094D3
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar_Scarification()
		{
		}

		// Token: 0x0600DB34 RID: 56116 RVA: 0x0040B2DB File Offset: 0x004094DB
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar_Scarification(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DB35 RID: 56117 RVA: 0x0040B2E4 File Offset: 0x004094E4
		public override bool ObligationTargetsValid(RitualObligation obligation)
		{
			Pawn pawn = obligation.targetA.Thing as Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (pawn.Dead)
			{
				return false;
			}
			int hediffCount = pawn.health.hediffSet.GetHediffCount(HediffDefOf.Scarification);
			return pawn.Ideo != null && pawn.Ideo.RequiredScars > hediffCount;
		}

		// Token: 0x0600DB36 RID: 56118 RVA: 0x0040B33F File Offset: 0x0040953F
		public override string LabelExtraPart(RitualObligation obligation)
		{
			return obligation.targetA.Thing.LabelShort;
		}
	}
}
