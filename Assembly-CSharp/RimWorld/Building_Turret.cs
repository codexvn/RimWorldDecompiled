using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002980 RID: 10624
	public abstract class Building_Turret : Building, IAttackTarget, ILoadReferenceable, IAttackTargetSearcher
	{
		// Token: 0x17002397 RID: 9111
		// (get) Token: 0x0600E967 RID: 59751
		public abstract LocalTargetInfo CurrentTarget { get; }

		// Token: 0x17002398 RID: 9112
		// (get) Token: 0x0600E968 RID: 59752
		public abstract Verb AttackVerb { get; }

		// Token: 0x17002399 RID: 9113
		// (get) Token: 0x0600E969 RID: 59753 RVA: 0x0043EC02 File Offset: 0x0043CE02
		public virtual Material TurretTopMaterial
		{
			get
			{
				return this.def.building.turretTopMat;
			}
		}

		// Token: 0x1700239A RID: 9114
		// (get) Token: 0x0600E96A RID: 59754 RVA: 0x0043EC14 File Offset: 0x0043CE14
		protected bool IsStunned
		{
			get
			{
				if (!this.triedGettingStunner)
				{
					CompStunnable comp = base.GetComp<CompStunnable>();
					this.stunner = ((comp != null) ? comp.StunHandler : null);
					this.triedGettingStunner = true;
				}
				return this.stunner != null && this.stunner.Stunned;
			}
		}

		// Token: 0x1700239B RID: 9115
		// (get) Token: 0x0600E96B RID: 59755 RVA: 0x000FD030 File Offset: 0x000FB230
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700239C RID: 9116
		// (get) Token: 0x0600E96C RID: 59756 RVA: 0x0043EC52 File Offset: 0x0043CE52
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				return this.CurrentTarget;
			}
		}

		// Token: 0x1700239D RID: 9117
		// (get) Token: 0x0600E96D RID: 59757 RVA: 0x000FD030 File Offset: 0x000FB230
		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700239E RID: 9118
		// (get) Token: 0x0600E96E RID: 59758 RVA: 0x0043EC5A File Offset: 0x0043CE5A
		public Verb CurrentEffectiveVerb
		{
			get
			{
				return this.AttackVerb;
			}
		}

		// Token: 0x1700239F RID: 9119
		// (get) Token: 0x0600E96F RID: 59759 RVA: 0x0043EC62 File Offset: 0x0043CE62
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.lastAttackedTarget;
			}
		}

		// Token: 0x170023A0 RID: 9120
		// (get) Token: 0x0600E970 RID: 59760 RVA: 0x0043EC6A File Offset: 0x0043CE6A
		public int LastAttackTargetTick
		{
			get
			{
				return this.lastAttackTargetTick;
			}
		}

		// Token: 0x170023A1 RID: 9121
		// (get) Token: 0x0600E971 RID: 59761 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public float TargetPriorityFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170023A2 RID: 9122
		// (get) Token: 0x0600E972 RID: 59762 RVA: 0x0043EC72 File Offset: 0x0043CE72
		public LocalTargetInfo ForcedTarget
		{
			get
			{
				return this.forcedTarget;
			}
		}

		// Token: 0x170023A3 RID: 9123
		// (get) Token: 0x0600E973 RID: 59763 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool IsEverThreat
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600E974 RID: 59764 RVA: 0x0043EC7C File Offset: 0x0043CE7C
		protected override void Tick()
		{
			base.Tick();
			if (this.forcedTarget.HasThing && (!this.forcedTarget.Thing.Spawned || !base.Spawned || this.forcedTarget.Thing.Map != base.Map))
			{
				this.forcedTarget = LocalTargetInfo.Invalid;
			}
		}

		// Token: 0x0600E975 RID: 59765 RVA: 0x0043ECD9 File Offset: 0x0043CED9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_TargetInfo.Look(ref this.forcedTarget, "forcedTarget");
			Scribe_TargetInfo.Look(ref this.lastAttackedTarget, "lastAttackedTarget");
			Scribe_Values.Look<int>(ref this.lastAttackTargetTick, "lastAttackTargetTick", 0, false);
		}

		// Token: 0x0600E976 RID: 59766 RVA: 0x0043ED13 File Offset: 0x0043CF13
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			absorbed = false;
		}

		// Token: 0x0600E977 RID: 59767
		public abstract void OrderAttack(LocalTargetInfo targ);

		// Token: 0x0600E978 RID: 59768 RVA: 0x0043ED28 File Offset: 0x0043CF28
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			if (!this.IsEverThreat)
			{
				return true;
			}
			CompPowerTrader comp = base.GetComp<CompPowerTrader>();
			if (comp != null && !comp.PowerOn)
			{
				return true;
			}
			CompMannable comp2 = base.GetComp<CompMannable>();
			if (comp2 != null && !comp2.MannedNow)
			{
				return true;
			}
			CompCanBeDormant comp3 = base.GetComp<CompCanBeDormant>();
			if (comp3 != null && !comp3.Awake)
			{
				return true;
			}
			CompInitiatable comp4 = base.GetComp<CompInitiatable>();
			if (comp4 != null && !comp4.Initiated)
			{
				return true;
			}
			CompMechPowerCell comp5 = base.GetComp<CompMechPowerCell>();
			if (comp5 != null && comp5.depleted)
			{
				return true;
			}
			CompHackable comp6 = base.GetComp<CompHackable>();
			return comp6 != null && comp6.IsHacked;
		}

		// Token: 0x0600E979 RID: 59769 RVA: 0x0043EDBE File Offset: 0x0043CFBE
		protected void OnAttackedTarget(LocalTargetInfo target)
		{
			this.lastAttackTargetTick = Find.TickManager.TicksGame;
			this.lastAttackedTarget = target;
		}

		// Token: 0x04009B63 RID: 39779
		protected LocalTargetInfo forcedTarget = LocalTargetInfo.Invalid;

		// Token: 0x04009B64 RID: 39780
		private LocalTargetInfo lastAttackedTarget;

		// Token: 0x04009B65 RID: 39781
		private int lastAttackTargetTick;

		// Token: 0x04009B66 RID: 39782
		private StunHandler stunner;

		// Token: 0x04009B67 RID: 39783
		private bool triedGettingStunner;

		// Token: 0x04009B68 RID: 39784
		private const float SightRadiusTurret = 13.4f;
	}
}
