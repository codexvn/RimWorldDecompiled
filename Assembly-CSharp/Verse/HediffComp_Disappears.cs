using System;
using RimWorld;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x0200069F RID: 1695
	public class HediffComp_Disappears : HediffComp
	{
		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002A9F RID: 10911 RVA: 0x000E00CF File Offset: 0x000DE2CF
		public HediffCompProperties_Disappears Props
		{
			get
			{
				return (HediffCompProperties_Disappears)this.props;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002AA0 RID: 10912 RVA: 0x000E00DC File Offset: 0x000DE2DC
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || this.ticksToDisappear <= 0 || (this.Props.requiredMentalState != null && base.Pawn.MentalStateDef != this.Props.requiredMentalState);
			}
		}

		// Token: 0x06002AA1 RID: 10913 RVA: 0x000E011C File Offset: 0x000DE31C
		private static float AddNoiseToProgress(float progress, int seed)
		{
			float num = (float)Perlin.GetValue((double)progress, 0.0, 0.0, 9.0, seed, 2.0, 0.5, 6, false, false, QualityMode.Medium);
			float num2 = 0.25f * (1f - progress);
			return Mathf.Clamp01(progress + num2 * num);
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002AA2 RID: 10914 RVA: 0x000E017C File Offset: 0x000DE37C
		public float Progress
		{
			get
			{
				return 1f - (float)this.ticksToDisappear / (float)Math.Max(1, this.disappearsAfterTicks);
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06002AA3 RID: 10915 RVA: 0x000E0199 File Offset: 0x000DE399
		public int EffectiveTicksToDisappear
		{
			get
			{
				return this.ticksToDisappear / this.TicksLostPerTick;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06002AA4 RID: 10916 RVA: 0x000E01A8 File Offset: 0x000DE3A8
		public float NoisyProgress
		{
			get
			{
				return HediffComp_Disappears.AddNoiseToProgress(this.Progress, this.seed);
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06002AA5 RID: 10917 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual int TicksLostPerTick
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06002AA6 RID: 10918 RVA: 0x000E01BC File Offset: 0x000DE3BC
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Props.showRemainingTime)
				{
					return base.CompLabelInBracketsExtra;
				}
				if (this.EffectiveTicksToDisappear < 2500)
				{
					return this.EffectiveTicksToDisappear.ToStringSecondsFromTicks("F0");
				}
				return this.EffectiveTicksToDisappear.ToStringTicksToPeriod(true, true, true, true, this.Props.canUseDecimalsShortForm);
			}
		}

		// Token: 0x06002AA7 RID: 10919 RVA: 0x000E0215 File Offset: 0x000DE415
		public override void CompPostMake()
		{
			base.CompPostMake();
			this.disappearsAfterTicks = this.Props.disappearsAfterTicks.RandomInRange;
			this.seed = Rand.Int;
			this.ticksToDisappear = this.disappearsAfterTicks;
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x000E024A File Offset: 0x000DE44A
		public void ResetElapsedTicks()
		{
			this.ticksToDisappear = this.disappearsAfterTicks;
		}

		// Token: 0x06002AA9 RID: 10921 RVA: 0x000E0258 File Offset: 0x000DE458
		public void SetDuration(int ticks)
		{
			this.disappearsAfterTicks = ticks;
			this.ticksToDisappear = ticks;
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x000E0268 File Offset: 0x000DE468
		public override void CompPostPostRemoved()
		{
			if (!this.Props.leaveFreshWounds)
			{
				foreach (BodyPartRecord part in this.parent.Part.GetPartAndAllChildParts())
				{
					Hediff_MissingPart hediff_MissingPart = base.Pawn.health.hediffSet.GetMissingPartFor(part) as Hediff_MissingPart;
					if (hediff_MissingPart != null)
					{
						hediff_MissingPart.IsFresh = false;
					}
				}
			}
			if (this.CompShouldRemove && !this.Props.messageOnDisappear.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				Messages.Message(this.Props.messageOnDisappear.Formatted(base.Pawn.Named("PAWN")), base.Pawn, MessageTypeDefOf.PositiveEvent, true);
			}
			if (!this.Props.letterTextOnDisappear.NullOrEmpty() && !this.Props.letterLabelOnDisappear.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(base.Pawn) && (!base.Pawn.Dead || this.Props.sendLetterOnDisappearIfDead))
			{
				Find.LetterStack.ReceiveLetter(this.Props.letterLabelOnDisappear.Formatted(base.Pawn.Named("PAWN")), this.Props.letterTextOnDisappear.Formatted(base.Pawn.Named("PAWN")), LetterDefOf.PositiveEvent, base.Pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x000E0400 File Offset: 0x000DE600
		public override void CompPostTick(ref float severityAdjustment)
		{
			this.ticksToDisappear -= this.TicksLostPerTick;
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x000E0418 File Offset: 0x000DE618
		public override void CompPostMerged(Hediff other)
		{
			base.CompPostMerged(other);
			HediffComp_Disappears hediffComp_Disappears = other.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null && hediffComp_Disappears.ticksToDisappear > this.ticksToDisappear)
			{
				this.ticksToDisappear = hediffComp_Disappears.ticksToDisappear;
			}
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x000E0450 File Offset: 0x000DE650
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksToDisappear, "ticksToDisappear", 0, false);
			Scribe_Values.Look<int>(ref this.disappearsAfterTicks, "disappearsAfterTicks", 0, false);
			Scribe_Values.Look<int>(ref this.seed, "seed", 0, false);
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x000E0488 File Offset: 0x000DE688
		public override string CompDebugString()
		{
			return "ticksToDisappear: " + this.ticksToDisappear.ToString();
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x000E04A0 File Offset: 0x000DE6A0
		public override void CopyFrom(HediffComp other)
		{
			HediffComp_Disappears hediffComp_Disappears = other as HediffComp_Disappears;
			if (hediffComp_Disappears != null)
			{
				this.ticksToDisappear = hediffComp_Disappears.ticksToDisappear;
				this.disappearsAfterTicks = hediffComp_Disappears.disappearsAfterTicks;
			}
		}

		// Token: 0x0400228B RID: 8843
		public int ticksToDisappear;

		// Token: 0x0400228C RID: 8844
		public int disappearsAfterTicks;

		// Token: 0x0400228D RID: 8845
		public int seed;

		// Token: 0x0400228E RID: 8846
		private const float NoiseScale = 0.25f;

		// Token: 0x0400228F RID: 8847
		private const float NoiseWiggliness = 9f;
	}
}
