using System;
using System.Collections.Generic;
using RimWorld.Utility;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BE6 RID: 11238
	public class CompApparelVerbOwner_Charged : CompApparelVerbOwner, ICompWithCharges
	{
		// Token: 0x1700277F RID: 10111
		// (get) Token: 0x0600FB91 RID: 64401 RVA: 0x00490B3A File Offset: 0x0048ED3A
		public new CompProperties_ApparelVerbOwnerCharged Props
		{
			get
			{
				return this.props as CompProperties_ApparelVerbOwnerCharged;
			}
		}

		// Token: 0x17002780 RID: 10112
		// (get) Token: 0x0600FB92 RID: 64402 RVA: 0x00490B47 File Offset: 0x0048ED47
		public int RemainingCharges
		{
			get
			{
				return this.remainingCharges;
			}
		}

		// Token: 0x17002781 RID: 10113
		// (get) Token: 0x0600FB93 RID: 64403 RVA: 0x00490B4F File Offset: 0x0048ED4F
		public int MaxCharges
		{
			get
			{
				return this.Props.maxCharges;
			}
		}

		// Token: 0x17002782 RID: 10114
		// (get) Token: 0x0600FB94 RID: 64404 RVA: 0x00490B5C File Offset: 0x0048ED5C
		public string LabelRemaining
		{
			get
			{
				return string.Format("{0} / {1}", this.RemainingCharges, this.MaxCharges);
			}
		}

		// Token: 0x17002783 RID: 10115
		// (get) Token: 0x0600FB95 RID: 64405 RVA: 0x00490B7E File Offset: 0x0048ED7E
		public override string GizmoExtraLabel
		{
			get
			{
				return this.LabelRemaining;
			}
		}

		// Token: 0x0600FB96 RID: 64406 RVA: 0x00490B86 File Offset: 0x0048ED86
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.remainingCharges = this.MaxCharges;
		}

		// Token: 0x0600FB97 RID: 64407 RVA: 0x00490B9A File Offset: 0x0048ED9A
		public override string CompInspectStringExtra()
		{
			return "ChargesRemaining".Translate(this.Props.ChargeNounArgument) + ": " + this.LabelRemaining;
		}

		// Token: 0x0600FB98 RID: 64408 RVA: 0x00490BCB File Offset: 0x0048EDCB
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
			yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_ReloadChargesRemaining_Name".Translate(this.Props.ChargeNounArgument), this.LabelRemaining, "Stat_Thing_ReloadChargesRemaining_Desc".Translate(this.Props.ChargeNounArgument), 2749, null, null, false, false);
			yield break;
			yield break;
		}

		// Token: 0x0600FB99 RID: 64409 RVA: 0x00490BDB File Offset: 0x0048EDDB
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.remainingCharges, "remainingCharges", -999, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.remainingCharges == -999)
			{
				this.remainingCharges = this.MaxCharges;
			}
		}

		// Token: 0x0600FB9A RID: 64410 RVA: 0x00490C1A File Offset: 0x0048EE1A
		public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetWornGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Reload to full",
					action = delegate()
					{
						this.remainingCharges = this.MaxCharges;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600FB9B RID: 64411 RVA: 0x00490C2C File Offset: 0x0048EE2C
		public override string CompTipStringExtra()
		{
			TaggedString taggedString = "Stat_Thing_ReloadChargesRemaining_Name".Translate(this.Props.ChargeNounArgument).CapitalizeFirst();
			return string.Format("\n\n{0}: {1} / {2}", taggedString, this.RemainingCharges, this.MaxCharges);
		}

		// Token: 0x0600FB9C RID: 64412 RVA: 0x00490C80 File Offset: 0x0048EE80
		public override void UsedOnce()
		{
			base.UsedOnce();
			if (this.remainingCharges > 0)
			{
				this.remainingCharges--;
			}
			if (this.Props.destroyOnEmpty && this.remainingCharges == 0 && !this.parent.Destroyed)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0400A511 RID: 42257
		protected int remainingCharges;
	}
}
