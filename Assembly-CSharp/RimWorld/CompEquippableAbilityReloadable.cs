using System;
using System.Collections.Generic;
using RimWorld.Utility;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002C78 RID: 11384
	public class CompEquippableAbilityReloadable : CompEquippableAbility, IReloadableComp, ICompWithCharges
	{
		// Token: 0x17002843 RID: 10307
		// (get) Token: 0x0600FF51 RID: 65361 RVA: 0x0049FCB7 File Offset: 0x0049DEB7
		public CompProperties_EquippableAbilityReloadable Props
		{
			get
			{
				return this.props as CompProperties_EquippableAbilityReloadable;
			}
		}

		// Token: 0x17002844 RID: 10308
		// (get) Token: 0x0600FF52 RID: 65362 RVA: 0x0048FDB8 File Offset: 0x0048DFB8
		public Thing ReloadableThing
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17002845 RID: 10309
		// (get) Token: 0x0600FF53 RID: 65363 RVA: 0x0049FCC4 File Offset: 0x0049DEC4
		public ThingDef AmmoDef
		{
			get
			{
				return this.Props.ammoDef;
			}
		}

		// Token: 0x17002846 RID: 10310
		// (get) Token: 0x0600FF54 RID: 65364 RVA: 0x0049FCD1 File Offset: 0x0049DED1
		public int MaxCharges
		{
			get
			{
				return this.Props.maxCharges;
			}
		}

		// Token: 0x17002847 RID: 10311
		// (get) Token: 0x0600FF55 RID: 65365 RVA: 0x0049FCDE File Offset: 0x0049DEDE
		public int BaseReloadTicks
		{
			get
			{
				return this.Props.baseReloadTicks;
			}
		}

		// Token: 0x17002848 RID: 10312
		// (get) Token: 0x0600FF56 RID: 65366 RVA: 0x0049FCEB File Offset: 0x0049DEEB
		public string LabelRemaining
		{
			get
			{
				return string.Format("{0} / {1}", this.RemainingCharges, this.MaxCharges);
			}
		}

		// Token: 0x17002849 RID: 10313
		// (get) Token: 0x0600FF57 RID: 65367 RVA: 0x0049FD0D File Offset: 0x0049DF0D
		public bool AbilityUsesCharges
		{
			get
			{
				Ability abilityForReading = base.AbilityForReading;
				return abilityForReading != null && abilityForReading.UsesCharges;
			}
		}

		// Token: 0x1700284A RID: 10314
		// (get) Token: 0x0600FF58 RID: 65368 RVA: 0x0049FD20 File Offset: 0x0049DF20
		// (set) Token: 0x0600FF59 RID: 65369 RVA: 0x0049FD33 File Offset: 0x0049DF33
		public int RemainingCharges
		{
			get
			{
				Ability abilityForReading = base.AbilityForReading;
				if (abilityForReading == null)
				{
					return 0;
				}
				return abilityForReading.RemainingCharges;
			}
			set
			{
				if (base.AbilityForReading != null)
				{
					base.AbilityForReading.RemainingCharges = value;
				}
			}
		}

		// Token: 0x0600FF5A RID: 65370 RVA: 0x0049FD49 File Offset: 0x0049DF49
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.replenishInTicks, "replenishInTicks", -1, false);
		}

		// Token: 0x0600FF5B RID: 65371 RVA: 0x0049FD63 File Offset: 0x0049DF63
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.Notify_PropsChanged();
		}

		// Token: 0x0600FF5C RID: 65372 RVA: 0x0049FD71 File Offset: 0x0049DF71
		public void Notify_PropsChanged()
		{
			if (base.AbilityForReading == null)
			{
				return;
			}
			base.AbilityForReading.maxCharges = this.MaxCharges;
			this.RemainingCharges = this.MaxCharges;
		}

		// Token: 0x0600FF5D RID: 65373 RVA: 0x0049FD9C File Offset: 0x0049DF9C
		public override void CompTick()
		{
			base.CompTick();
			if (base.AbilityForReading == null)
			{
				return;
			}
			if (this.Props.replenishAfterCooldown && this.RemainingCharges == 0)
			{
				if (this.replenishInTicks > 0)
				{
					this.replenishInTicks--;
					return;
				}
				this.RemainingCharges = this.MaxCharges;
			}
		}

		// Token: 0x0600FF5E RID: 65374 RVA: 0x0049FDF4 File Offset: 0x0049DFF4
		public override string CompInspectStringExtra()
		{
			if (base.AbilityForReading == null)
			{
				return null;
			}
			if (!this.AbilityUsesCharges)
			{
				return null;
			}
			return "ChargesRemaining".Translate(this.Props.ChargeNounArgument) + ": " + this.LabelRemaining;
		}

		// Token: 0x0600FF5F RID: 65375 RVA: 0x0049FE44 File Offset: 0x0049E044
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			IEnumerable<StatDrawEntry> enumerable = base.SpecialDisplayStats();
			if (enumerable != null)
			{
				foreach (StatDrawEntry statDrawEntry in enumerable)
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			if (base.AbilityForReading == null)
			{
				yield break;
			}
			if (base.AbilityForReading.UsesCharges)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Weapon, "Stat_Thing_ReloadChargesRemaining_Name".Translate(this.Props.ChargeNounArgument), this.LabelRemaining, "Stat_Thing_ReloadChargesRemaining_Desc".Translate(this.Props.ChargeNounArgument), 5440, null, null, false, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600FF60 RID: 65376 RVA: 0x0049FE54 File Offset: 0x0049E054
		public override IEnumerable<Gizmo> CompGetEquippedGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetEquippedGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (base.AbilityForReading == null)
			{
				yield break;
			}
			if (DebugSettings.ShowDevGizmos && this.NeedsReload(false))
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Reload to full",
					action = delegate()
					{
						this.RemainingCharges = this.MaxCharges;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600FF61 RID: 65377 RVA: 0x0049FE64 File Offset: 0x0049E064
		public bool NeedsReload(bool allowForcedReload)
		{
			if (base.AbilityForReading == null)
			{
				return false;
			}
			if (this.Props.ammoDef == null)
			{
				return false;
			}
			if (this.Props.ammoCountToRefill == 0)
			{
				return this.RemainingCharges != this.MaxCharges;
			}
			if (!allowForcedReload)
			{
				return this.RemainingCharges == 0;
			}
			return this.RemainingCharges != this.MaxCharges;
		}

		// Token: 0x0600FF62 RID: 65378 RVA: 0x0049FEC8 File Offset: 0x0049E0C8
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
				this.RemainingCharges = this.MaxCharges;
			}
			else
			{
				if (ammo.stackCount < this.Props.ammoCountPerCharge)
				{
					return;
				}
				int num = Mathf.Clamp(ammo.stackCount / this.Props.ammoCountPerCharge, 0, this.MaxCharges - this.RemainingCharges);
				ammo.SplitOff(num * this.Props.ammoCountPerCharge).Destroy(DestroyMode.Vanish);
				this.RemainingCharges += num;
			}
			if (this.Props.soundReload != null)
			{
				this.Props.soundReload.PlayOneShot(new TargetInfo(this.parent.PositionHeld, this.parent.MapHeld, false));
			}
		}

		// Token: 0x0600FF63 RID: 65379 RVA: 0x0049FFC4 File Offset: 0x0049E1C4
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

		// Token: 0x0600FF64 RID: 65380 RVA: 0x0049FFF5 File Offset: 0x0049E1F5
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
			return this.Props.ammoCountPerCharge * (this.MaxCharges - this.RemainingCharges);
		}

		// Token: 0x0600FF65 RID: 65381 RVA: 0x004A0034 File Offset: 0x0049E234
		public int MaxAmmoAmount()
		{
			if (base.AbilityForReading == null)
			{
				return 0;
			}
			if (this.Props.ammoDef == null)
			{
				return 0;
			}
			if (this.Props.ammoCountToRefill == 0)
			{
				return this.Props.ammoCountPerCharge * this.MaxCharges;
			}
			return this.Props.ammoCountToRefill;
		}

		// Token: 0x0600FF66 RID: 65382 RVA: 0x004A0085 File Offset: 0x0049E285
		public override void UsedOnce()
		{
			if (base.AbilityForReading == null)
			{
				return;
			}
			if (this.Props.replenishAfterCooldown && this.RemainingCharges == 0)
			{
				this.replenishInTicks = this.Props.baseReloadTicks;
			}
		}

		// Token: 0x0600FF67 RID: 65383 RVA: 0x004A00B6 File Offset: 0x0049E2B6
		public bool CanBeUsed(out string reason)
		{
			reason = null;
			if (base.AbilityForReading == null)
			{
				return false;
			}
			if (this.RemainingCharges <= 0)
			{
				reason = this.DisabledReason(this.MinAmmoNeeded(false), this.MaxAmmoNeeded(false));
				return false;
			}
			return true;
		}

		// Token: 0x0600FF68 RID: 65384 RVA: 0x004A00E8 File Offset: 0x0049E2E8
		public string DisabledReason(int minNeeded, int maxNeeded)
		{
			if (base.AbilityForReading == null)
			{
				return null;
			}
			if (this.Props.replenishAfterCooldown)
			{
				return "CommandReload_Cooldown".Translate(this.Props.CooldownVerbArgument, this.replenishInTicks.ToStringTicksToPeriod(true, false, true, true, false).Named("TIME"));
			}
			if (this.Props.ammoDef == null)
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
			return "CommandReload_NoAmmo".Translate(this.Props.ChargeNounArgument, this.Props.ammoDef.Named("AMMO"), arg.Named("COUNT"));
		}

		// Token: 0x0400A725 RID: 42789
		private int replenishInTicks = -1;
	}
}
