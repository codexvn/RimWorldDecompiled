using System;

namespace Verse
{
	// Token: 0x02000CB8 RID: 3256
	public class SubEffecter_CameraShakeChance : SubEffecter_CameraShake
	{
		// Token: 0x06005362 RID: 21346 RVA: 0x001AD9C6 File Offset: 0x001ABBC6
		public SubEffecter_CameraShakeChance(SubEffecterDef subDef, Effecter parent) : base(subDef, parent)
		{
			this.ticksUntilEffect = this.def.initialDelayTicks;
			this.lifespanMaxTicks = Find.TickManager.TicksGame + this.def.lifespanMaxTicks + this.ticksUntilEffect;
		}

		// Token: 0x06005363 RID: 21347 RVA: 0x001ADA04 File Offset: 0x001ABC04
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilEffect--;
			if (Find.TickManager.TicksGame >= this.lastEffectTicks + this.def.chancePeriodTicks && Find.TickManager.TicksGame < this.lifespanMaxTicks && this.ticksUntilEffect <= 0 && Rand.Chance(base.EffectiveChancePerTick))
			{
				this.lastEffectTicks = Find.TickManager.TicksGame;
				base.DoShake(A);
			}
		}

		// Token: 0x04003C31 RID: 15409
		private int lastEffectTicks;

		// Token: 0x04003C32 RID: 15410
		private int ticksUntilEffect;

		// Token: 0x04003C33 RID: 15411
		private int lifespanMaxTicks;
	}
}
