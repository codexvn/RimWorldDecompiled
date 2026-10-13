using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F33 RID: 12083
	public class CompTargetEffect_GoodwillImpact : CompTargetEffect
	{
		// Token: 0x17002C11 RID: 11281
		// (get) Token: 0x060110D5 RID: 69845 RVA: 0x004E691B File Offset: 0x004E4B1B
		protected CompProperties_TargetEffect_GoodwillImpact PropsGoodwillImpact
		{
			get
			{
				return (CompProperties_TargetEffect_GoodwillImpact)this.props;
			}
		}

		// Token: 0x060110D6 RID: 69846 RVA: 0x004E6928 File Offset: 0x004E4B28
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = target as Pawn;
			Faction faction = (pawn != null) ? pawn.HomeFaction : target.Faction;
			if (user.Faction == Faction.OfPlayer && faction != null && !faction.HostileTo(user.Faction) && (pawn == null || !pawn.IsSlaveOfColony))
			{
				Faction.OfPlayer.TryAffectGoodwillWith(faction, this.PropsGoodwillImpact.goodwillImpact, true, true, HistoryEventDefOf.UsedHarmfulItem, null);
			}
		}
	}
}
