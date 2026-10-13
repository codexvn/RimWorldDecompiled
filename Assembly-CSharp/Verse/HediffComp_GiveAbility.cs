using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006BB RID: 1723
	public class HediffComp_GiveAbility : HediffComp
	{
		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06002B0C RID: 11020 RVA: 0x000E1764 File Offset: 0x000DF964
		private HediffCompProperties_GiveAbility Props
		{
			get
			{
				return (HediffCompProperties_GiveAbility)this.props;
			}
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x000E1774 File Offset: 0x000DF974
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			if (this.Props.abilityDef != null)
			{
				this.parent.pawn.abilities.GainAbility(this.Props.abilityDef);
			}
			if (!this.Props.abilityDefs.NullOrEmpty<AbilityDef>())
			{
				for (int i = 0; i < this.Props.abilityDefs.Count; i++)
				{
					this.parent.pawn.abilities.GainAbility(this.Props.abilityDefs[i]);
				}
			}
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x000E1804 File Offset: 0x000DFA04
		public override void CompPostPostRemoved()
		{
			if (this.Props.abilityDef != null)
			{
				this.parent.pawn.abilities.RemoveAbility(this.Props.abilityDef);
			}
			if (!this.Props.abilityDefs.NullOrEmpty<AbilityDef>())
			{
				for (int i = 0; i < this.Props.abilityDefs.Count; i++)
				{
					this.parent.pawn.abilities.RemoveAbility(this.Props.abilityDefs[i]);
				}
			}
		}
	}
}
