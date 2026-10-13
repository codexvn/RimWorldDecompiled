using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002708 RID: 9992
	public class RitualObligationTargetWorker_AnyRitualSpotOrAltar_Blinding : RitualObligationTargetWorker_AnyRitualSpotOrAltar
	{
		// Token: 0x0600DB45 RID: 56133 RVA: 0x0040B2D3 File Offset: 0x004094D3
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar_Blinding()
		{
		}

		// Token: 0x0600DB46 RID: 56134 RVA: 0x0040B2DB File Offset: 0x004094DB
		public RitualObligationTargetWorker_AnyRitualSpotOrAltar_Blinding(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DB47 RID: 56135 RVA: 0x0040B598 File Offset: 0x00409798
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
			return pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord p) => p.def == BodyPartDefOf.Eye);
		}
	}
}
