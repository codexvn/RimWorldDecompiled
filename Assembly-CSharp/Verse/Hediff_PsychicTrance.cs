using System;
using RimWorld;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000763 RID: 1891
	public class Hediff_PsychicTrance : HediffWithComps
	{
		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06002E65 RID: 11877 RVA: 0x000EEC24 File Offset: 0x000ECE24
		public override bool ShouldRemove
		{
			get
			{
				if (base.ShouldRemove)
				{
					return true;
				}
				if (this.pawn.mindState.duty != null && this.pawn.mindState.duty.def == DutyDefOf.PerformHateChant)
				{
					return false;
				}
				Lord lord = this.pawn.GetLord();
				return lord == null || !(lord.LordJob is LordJob_PsychicRitual);
			}
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x000EEC8A File Offset: 0x000ECE8A
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Psychic trance"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}
	}
}
