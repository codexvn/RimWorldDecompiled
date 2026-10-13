using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F68 RID: 12136
	public class CompUseEffect_OffsetDeathrestCapacity : CompUseEffect
	{
		// Token: 0x17002C37 RID: 11319
		// (get) Token: 0x06011193 RID: 70035 RVA: 0x004E8E51 File Offset: 0x004E7051
		private CompProperties_UseEffectOffsetDeathrestCapacity Props
		{
			get
			{
				return (CompProperties_UseEffectOffsetDeathrestCapacity)this.props;
			}
		}

		// Token: 0x06011194 RID: 70036 RVA: 0x004E8E5E File Offset: 0x004E705E
		public override void DoEffect(Pawn user)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			base.DoEffect(user);
			Pawn_GeneTracker genes = user.genes;
			if (genes == null)
			{
				return;
			}
			Gene_Deathrest firstGeneOfType = genes.GetFirstGeneOfType<Gene_Deathrest>();
			if (firstGeneOfType == null)
			{
				return;
			}
			firstGeneOfType.OffsetCapacity(this.Props.offset, true);
		}

		// Token: 0x06011195 RID: 70037 RVA: 0x004E8E95 File Offset: 0x004E7095
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			Pawn_GeneTracker genes = p.genes;
			if (((genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null) == null)
			{
				return "CannotDeathrest".Translate();
			}
			return base.CanBeUsedBy(p);
		}

		// Token: 0x06011196 RID: 70038 RVA: 0x004E8EC2 File Offset: 0x004E70C2
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (ModsConfig.BiotechActive)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "DeathrestCapacity".Translate().CapitalizeFirst(), this.Props.offset.ToStringWithSign(), "DeathrestCapacityDesc".Translate(), 1010, null, null, false, false);
			}
			yield break;
		}
	}
}
