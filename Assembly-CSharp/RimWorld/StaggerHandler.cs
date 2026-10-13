using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B16 RID: 11030
	public class StaggerHandler : IExposable
	{
		// Token: 0x17002690 RID: 9872
		// (get) Token: 0x0600F664 RID: 63076 RVA: 0x004787E3 File Offset: 0x004769E3
		public bool Staggered
		{
			get
			{
				return this.staggerTicksLeft > 0;
			}
		}

		// Token: 0x17002691 RID: 9873
		// (get) Token: 0x0600F665 RID: 63077 RVA: 0x004787EE File Offset: 0x004769EE
		public float StaggerMoveSpeedFactor
		{
			get
			{
				return this.staggerMoveSpeedFactor;
			}
		}

		// Token: 0x17002692 RID: 9874
		// (get) Token: 0x0600F666 RID: 63078 RVA: 0x004787F6 File Offset: 0x004769F6
		public int StaggerTicksLeft
		{
			get
			{
				return this.staggerTicksLeft;
			}
		}

		// Token: 0x0600F667 RID: 63079 RVA: 0x004787FE File Offset: 0x004769FE
		public StaggerHandler(Pawn parent)
		{
			this.parent = parent;
		}

		// Token: 0x0600F668 RID: 63080 RVA: 0x00478810 File Offset: 0x00476A10
		public bool StaggerFor(int ticks, float moveSpeedFactor = 0.17f)
		{
			if (ModsConfig.AnomalyActive && this.parent.health.hediffSet.HasHediff(HediffDefOf.AwokenCorpse, false))
			{
				return false;
			}
			ticks = Mathf.RoundToInt((float)ticks * this.parent.GetStatValue(StatDefOf.StaggerDurationFactor, true, -1));
			if (ticks > 0)
			{
				this.staggerTicksLeft = Mathf.Max(this.staggerTicksLeft, ticks);
				this.staggerMoveSpeedFactor = Mathf.Min(this.staggerMoveSpeedFactor, moveSpeedFactor);
				return true;
			}
			return false;
		}

		// Token: 0x0600F669 RID: 63081 RVA: 0x0047888C File Offset: 0x00476A8C
		public void Notify_BulletImpact(Bullet bullet)
		{
			if (this.parent.RaceProps.bulletStaggerIgnoreBodySize || this.parent.BodySize <= bullet.stoppingPower + 0.001f)
			{
				int ticks = this.parent.RaceProps.bulletStaggerDelayTicks ?? 95;
				if (this.StaggerFor(ticks, this.parent.RaceProps.bulletStaggerSpeedFactor ?? 0.17f) && this.parent.RaceProps.bulletStaggerEffecterDef != null)
				{
					Effecter effecter = this.effecter;
					if (effecter != null)
					{
						effecter.Cleanup();
					}
					this.effecter = this.parent.RaceProps.bulletStaggerEffecterDef.Spawn();
				}
			}
		}

		// Token: 0x0600F66A RID: 63082 RVA: 0x0047895C File Offset: 0x00476B5C
		public void StaggerHandlerTick()
		{
			if (this.staggerTicksLeft > 0)
			{
				this.staggerTicksLeft--;
				if (this.staggerTicksLeft <= 0)
				{
					this.staggerMoveSpeedFactor = 0.17f;
				}
			}
			if (this.staggerEffectTicksLeft > 0)
			{
				this.staggerEffectTicksLeft--;
				if (this.staggerEffectTicksLeft <= 0)
				{
					Effecter effecter = this.effecter;
					if (effecter != null)
					{
						effecter.Cleanup();
					}
					this.effecter = null;
				}
				if (this.effecter != null)
				{
					this.effecter.EffectTick(this.parent, this.parent);
				}
			}
		}

		// Token: 0x0600F66B RID: 63083 RVA: 0x004789F5 File Offset: 0x00476BF5
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.staggerTicksLeft, "staggerTicksLeft", 0, false);
			Scribe_Values.Look<float>(ref this.staggerMoveSpeedFactor, "staggerMoveSpeedFactor", 0f, false);
			Scribe_Values.Look<int>(ref this.staggerEffectTicksLeft, "staggerEffectTicksLeft", 0, false);
		}

		// Token: 0x0400A22E RID: 41518
		public const float DefaultStaggerMoveSpeedFactor = 0.17f;

		// Token: 0x0400A22F RID: 41519
		public Pawn parent;

		// Token: 0x0400A230 RID: 41520
		private int staggerTicksLeft;

		// Token: 0x0400A231 RID: 41521
		private float staggerMoveSpeedFactor;

		// Token: 0x0400A232 RID: 41522
		private int staggerEffectTicksLeft;

		// Token: 0x0400A233 RID: 41523
		private Effecter effecter;
	}
}
