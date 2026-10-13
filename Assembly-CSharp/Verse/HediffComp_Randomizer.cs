using System;

namespace Verse
{
	// Token: 0x020006F0 RID: 1776
	public abstract class HediffComp_Randomizer : HediffComp
	{
		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06002BBC RID: 11196 RVA: 0x000E4178 File Offset: 0x000E2378
		private HediffCompProperties_Randomizer Props
		{
			get
			{
				return (HediffCompProperties_Randomizer)this.props;
			}
		}

		// Token: 0x06002BBD RID: 11197
		public abstract void Randomize();

		// Token: 0x06002BBE RID: 11198 RVA: 0x000E4185 File Offset: 0x000E2385
		public override void CompPostMake()
		{
			this.SetNextRandomizationTick();
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x000E418D File Offset: 0x000E238D
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Find.TickManager.TicksGame >= this.nextRandomizationTick)
			{
				this.Randomize();
				this.SetNextRandomizationTick();
			}
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x000E41AD File Offset: 0x000E23AD
		public void SetNextRandomizationTick()
		{
			this.nextRandomizationTick = Find.TickManager.TicksGame + this.Props.ticksToRandomize.RandomInRange;
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x000E41D0 File Offset: 0x000E23D0
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<int>(ref this.nextRandomizationTick, "nextRandomizationTick", 0, false);
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x000E41EA File Offset: 0x000E23EA
		public override string CompDebugString()
		{
			return string.Format("ticks until randomization: {0}", this.nextRandomizationTick - Find.TickManager.TicksGame);
		}

		// Token: 0x04002337 RID: 9015
		private int nextRandomizationTick;
	}
}
