using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000CC1 RID: 3265
	public class SubEffecter_SprayerChance : SubEffecter_Sprayer
	{
		// Token: 0x0600537D RID: 21373 RVA: 0x001AEA34 File Offset: 0x001ACC34
		public SubEffecter_SprayerChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilMote = def.initialDelayTicks;
			this.lifespanMaxTicks = Find.TickManager.TicksGame + def.lifespanMaxTicks + this.ticksUntilMote;
		}

		// Token: 0x0600537E RID: 21374 RVA: 0x001AEA68 File Offset: 0x001ACC68
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			float num = base.EffectiveChancePerTick;
			if (base.EffectiveSpawnLocType == MoteSpawnLocType.RandomCellOnTarget && B.HasThing)
			{
				num *= (float)(B.Thing.def.size.x * B.Thing.def.size.z);
			}
			this.ticksUntilMote--;
			if (Find.TickManager.TicksGame >= this.lastSpawnedTicks + this.def.chancePeriodTicks && Find.TickManager.TicksGame < this.lifespanMaxTicks && this.ticksUntilMote <= 0 && Rand.Chance(num))
			{
				this.lastSpawnedTicks = Find.TickManager.TicksGame;
				base.MakeMote(A, B, -1);
				if (this.def.soundDef != null)
				{
					this.def.soundDef.PlayOneShot(A);
				}
			}
		}

		// Token: 0x04003C41 RID: 15425
		private int lastSpawnedTicks;

		// Token: 0x04003C42 RID: 15426
		private int ticksUntilMote;

		// Token: 0x04003C43 RID: 15427
		private int lifespanMaxTicks;
	}
}
