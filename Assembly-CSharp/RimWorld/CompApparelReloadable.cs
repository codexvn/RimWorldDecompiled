using System;
using RimWorld.Utility;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002BDE RID: 11230
	public class CompApparelReloadable : CompApparelVerbOwner_Charged, IReloadableComp, ICompWithCharges
	{
		// Token: 0x1700276A RID: 10090
		// (get) Token: 0x0600FB4C RID: 64332 RVA: 0x0048FDAB File Offset: 0x0048DFAB
		public new CompProperties_ApparelReloadable Props
		{
			get
			{
				return this.props as CompProperties_ApparelReloadable;
			}
		}

		// Token: 0x1700276B RID: 10091
		// (get) Token: 0x0600FB4D RID: 64333 RVA: 0x0048FDB8 File Offset: 0x0048DFB8
		public Thing ReloadableThing
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x1700276C RID: 10092
		// (get) Token: 0x0600FB4E RID: 64334 RVA: 0x0048FDC0 File Offset: 0x0048DFC0
		public ThingDef AmmoDef
		{
			get
			{
				return this.Props.ammoDef;
			}
		}

		// Token: 0x1700276D RID: 10093
		// (get) Token: 0x0600FB4F RID: 64335 RVA: 0x0048FDCD File Offset: 0x0048DFCD
		public int BaseReloadTicks
		{
			get
			{
				return this.Props.baseReloadTicks;
			}
		}

		// Token: 0x0600FB50 RID: 64336 RVA: 0x0048FDDA File Offset: 0x0048DFDA
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.replenishInTicks, "replenishInTicks", -1, false);
		}

		// Token: 0x0600FB51 RID: 64337 RVA: 0x0048FDF4 File Offset: 0x0048DFF4
		public override void CompTick()
		{
			base.CompTick();
			if (this.Props.replenishAfterCooldown && base.RemainingCharges == 0)
			{
				if (this.replenishInTicks > 0)
				{
					this.replenishInTicks--;
					return;
				}
				this.remainingCharges = base.MaxCharges;
			}
		}

		// Token: 0x0600FB52 RID: 64338 RVA: 0x0048FE40 File Offset: 0x0048E040
		public string DisabledReason(int minNeeded, int maxNeeded)
		{
			if (this.Props.replenishAfterCooldown)
			{
				return "CommandReload_Cooldown".Translate(this.Props.CooldownVerbArgument, this.replenishInTicks.ToStringTicksToPeriod(true, false, true, true, false).Named("TIME"));
			}
			if (this.AmmoDef == null)
			{
				return "CommandReload_NoCharges".Translate(this.Props.ChargeNounArgument);
			}
			string arg;
			if (this.Props.ammoCountToRefill != 0)
			{
				arg = this.Props.ammoCountToRefill.ToString();
			}
			else
			{
				arg = ((minNeeded == maxNeeded) ? minNeeded.ToString() : string.Format("{0}-{1}", minNeeded, maxNeeded));
			}
			return "CommandReload_NoAmmo".Translate(this.Props.ChargeNounArgument, this.AmmoDef.Named("AMMO"), arg.Named("COUNT"));
		}

		// Token: 0x0600FB53 RID: 64339 RVA: 0x0048FF2C File Offset: 0x0048E12C
		public bool NeedsReload(bool allowForcedReload)
		{
			if (this.AmmoDef == null)
			{
				return false;
			}
			if (this.Props.ammoCountToRefill == 0)
			{
				return base.RemainingCharges != base.MaxCharges;
			}
			if (!allowForcedReload)
			{
				return this.remainingCharges == 0;
			}
			return base.RemainingCharges != base.MaxCharges;
		}

		// Token: 0x0600FB54 RID: 64340 RVA: 0x0048FF80 File Offset: 0x0048E180
		public void ReloadFrom(Thing ammo)
		{
			if (!this.NeedsReload(true))
			{
				return;
			}
			if (this.Props.ammoCountToRefill != 0)
			{
				if (ammo.stackCount < this.Props.ammoCountToRefill)
				{
					return;
				}
				ammo.SplitOff(this.Props.ammoCountToRefill).Destroy(DestroyMode.Vanish);
				this.remainingCharges = base.MaxCharges;
			}
			else
			{
				if (ammo.stackCount < this.Props.ammoCountPerCharge)
				{
					return;
				}
				int num = Mathf.Clamp(ammo.stackCount / this.Props.ammoCountPerCharge, 0, base.MaxCharges - base.RemainingCharges);
				ammo.SplitOff(num * this.Props.ammoCountPerCharge).Destroy(DestroyMode.Vanish);
				this.remainingCharges += num;
			}
			if (this.Props.soundReload != null)
			{
				this.Props.soundReload.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
			}
		}

		// Token: 0x0600FB55 RID: 64341 RVA: 0x0049007C File Offset: 0x0048E27C
		public int MinAmmoNeeded(bool allowForcedReload)
		{
			if (!this.NeedsReload(allowForcedReload))
			{
				return 0;
			}
			if (this.Props.ammoCountToRefill != 0)
			{
				return this.Props.ammoCountToRefill;
			}
			return this.Props.ammoCountPerCharge;
		}

		// Token: 0x0600FB56 RID: 64342 RVA: 0x004900AD File Offset: 0x0048E2AD
		public int MaxAmmoNeeded(bool allowForcedReload)
		{
			if (!this.NeedsReload(allowForcedReload))
			{
				return 0;
			}
			if (this.Props.ammoCountToRefill != 0)
			{
				return this.Props.ammoCountToRefill;
			}
			return this.Props.ammoCountPerCharge * (base.MaxCharges - base.RemainingCharges);
		}

		// Token: 0x0600FB57 RID: 64343 RVA: 0x004900EC File Offset: 0x0048E2EC
		public int MaxAmmoAmount()
		{
			if (this.AmmoDef == null)
			{
				return 0;
			}
			if (this.Props.ammoCountToRefill == 0)
			{
				return this.Props.ammoCountPerCharge * base.MaxCharges;
			}
			return this.Props.ammoCountToRefill;
		}

		// Token: 0x0600FB58 RID: 64344 RVA: 0x00490123 File Offset: 0x0048E323
		public override void UsedOnce()
		{
			base.UsedOnce();
			if (this.Props.replenishAfterCooldown && this.remainingCharges == 0)
			{
				this.replenishInTicks = this.Props.baseReloadTicks;
			}
		}

		// Token: 0x0600FB59 RID: 64345 RVA: 0x00490151 File Offset: 0x0048E351
		public override bool CanBeUsed(out string reason)
		{
			reason = "";
			if (this.remainingCharges <= 0)
			{
				reason = this.DisabledReason(this.MinAmmoNeeded(false), this.MaxAmmoNeeded(false));
				return false;
			}
			return base.CanBeUsed(out reason);
		}

		// Token: 0x0400A4F6 RID: 42230
		private int replenishInTicks = -1;
	}
}
