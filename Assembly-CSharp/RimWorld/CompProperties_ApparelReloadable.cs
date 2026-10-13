using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BDB RID: 11227
	public class CompProperties_ApparelReloadable : CompProperties_ApparelVerbOwnerCharged
	{
		// Token: 0x17002765 RID: 10085
		// (get) Token: 0x0600FB34 RID: 64308 RVA: 0x0048F7BB File Offset: 0x0048D9BB
		public NamedArgument CooldownVerbArgument
		{
			get
			{
				return this.cooldownGerund.CapitalizeFirst().Named("COOLDOWNGERUND");
			}
		}

		// Token: 0x0600FB35 RID: 64309 RVA: 0x0048F7D2 File Offset: 0x0048D9D2
		public CompProperties_ApparelReloadable()
		{
			this.compClass = typeof(CompApparelReloadable);
		}

		// Token: 0x0600FB36 RID: 64310 RVA: 0x0048F7FD File Offset: 0x0048D9FD
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

		// Token: 0x0600FB37 RID: 64311 RVA: 0x0048F814 File Offset: 0x0048DA14
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (!req.HasThing)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadMaxCharges_Name".Translate(base.ChargeNounArgument), this.maxCharges.ToString(), "Stat_Thing_ReloadMaxCharges_Desc".Translate(base.ChargeNounArgument), 2749, null, null, false, false);
			}
			if (this.ammoDef != null)
			{
				if (this.ammoCountToRefill != 0)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadRefill_Name".Translate(base.ChargeNounArgument), string.Format("{0} {1}", this.ammoCountToRefill, this.ammoDef.label), "Stat_Thing_ReloadRefill_Desc".Translate(base.ChargeNounArgument), 2749, null, null, false, false);
				}
				else
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadPerCharge_Name".Translate(base.ChargeNounArgument), string.Format("{0} {1}", this.ammoCountPerCharge, this.ammoDef.label), "Stat_Thing_ReloadPerCharge_Desc".Translate(base.ChargeNounArgument), 2749, null, null, false, false);
				}
			}
			if (this.destroyOnEmpty)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadDestroyOnEmpty_Name".Translate(base.ChargeNounArgument), "Yes".Translate(), "Stat_Thing_ReloadDestroyOnEmpty_Desc".Translate(base.ChargeNounArgument), 2749, null, null, false, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400A4E1 RID: 42209
		public ThingDef ammoDef;

		// Token: 0x0400A4E2 RID: 42210
		public int ammoCountToRefill;

		// Token: 0x0400A4E3 RID: 42211
		public int ammoCountPerCharge;

		// Token: 0x0400A4E4 RID: 42212
		public int baseReloadTicks = 60;

		// Token: 0x0400A4E5 RID: 42213
		public bool replenishAfterCooldown;

		// Token: 0x0400A4E6 RID: 42214
		public SoundDef soundReload;

		// Token: 0x0400A4E7 RID: 42215
		[MustTranslate]
		public string cooldownGerund = "on cooldown";
	}
}
