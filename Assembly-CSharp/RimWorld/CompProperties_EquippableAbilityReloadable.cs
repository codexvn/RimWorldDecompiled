using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C75 RID: 11381
	public class CompProperties_EquippableAbilityReloadable : CompProperties_EquippableAbility
	{
		// Token: 0x1700283D RID: 10301
		// (get) Token: 0x0600FF38 RID: 65336 RVA: 0x0049F723 File Offset: 0x0049D923
		public NamedArgument CooldownVerbArgument
		{
			get
			{
				return this.cooldownGerund.CapitalizeFirst().Named("COOLDOWNGERUND");
			}
		}

		// Token: 0x1700283E RID: 10302
		// (get) Token: 0x0600FF39 RID: 65337 RVA: 0x0049F73A File Offset: 0x0049D93A
		public NamedArgument ChargeNounArgument
		{
			get
			{
				return this.chargeNoun.Named("CHARGENOUN");
			}
		}

		// Token: 0x0600FF3A RID: 65338 RVA: 0x0049F74C File Offset: 0x0049D94C
		public CompProperties_EquippableAbilityReloadable()
		{
			this.compClass = typeof(CompEquippableAbilityReloadable);
		}

		// Token: 0x0600FF3B RID: 65339 RVA: 0x0049F782 File Offset: 0x0049D982
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.ammoDef != null && this.ammoCountToRefill == 0 && this.ammoCountPerCharge == 0)
			{
				yield return "Reloadable component has ammoDef but one of ammoCountToRefill or ammoCountPerCharge must be set";
			}
			if (this.ammoCountToRefill != 0 && this.ammoCountPerCharge != 0)
			{
				yield return "Reloadable component: specify only one of ammoCountToRefill and ammoCountPerCharge";
			}
			yield break;
			yield break;
		}

		// Token: 0x0600FF3C RID: 65340 RVA: 0x0049F799 File Offset: 0x0049D999
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (!req.HasThing)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Stat_Thing_ReloadMaxCharges_Name".Translate(this.ChargeNounArgument), this.maxCharges.ToString(), "Stat_Thing_ReloadMaxCharges_Desc".Translate(this.ChargeNounArgument), 5440, null, null, false, false);
			}
			if (this.ammoDef != null)
			{
				if (this.ammoCountToRefill != 0)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Stat_Thing_ReloadRefill_Name".Translate(this.ChargeNounArgument), string.Format("{0} {1}", this.ammoCountToRefill, this.ammoDef.label), "Stat_Thing_ReloadRefill_Desc".Translate(this.ChargeNounArgument), 5440, null, null, false, false);
				}
				else
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Stat_Thing_ReloadPerCharge_Name".Translate(this.ChargeNounArgument), string.Format("{0} {1}", this.ammoCountPerCharge, this.ammoDef.label), "Stat_Thing_ReloadPerCharge_Desc".Translate(this.ChargeNounArgument), 5440, null, null, false, false);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0400A70E RID: 42766
		public int maxCharges;

		// Token: 0x0400A70F RID: 42767
		public ThingDef ammoDef;

		// Token: 0x0400A710 RID: 42768
		public int ammoCountToRefill;

		// Token: 0x0400A711 RID: 42769
		public int ammoCountPerCharge;

		// Token: 0x0400A712 RID: 42770
		public int baseReloadTicks = 60;

		// Token: 0x0400A713 RID: 42771
		public bool replenishAfterCooldown;

		// Token: 0x0400A714 RID: 42772
		public SoundDef soundReload;

		// Token: 0x0400A715 RID: 42773
		[MustTranslate]
		public string chargeNoun = "charge";

		// Token: 0x0400A716 RID: 42774
		[MustTranslate]
		public string cooldownGerund = "on cooldown";
	}
}
