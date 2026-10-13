using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F4C RID: 12108
	public abstract class CompRitualEffectSpawner : ThingComp
	{
		// Token: 0x06011131 RID: 69937 RVA: 0x004E7914 File Offset: 0x004E5B14
		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(30))
			{
				this.ritual = this.parent.TargetOfRitual();
			}
			if (this.ritual != null)
			{
				this.Tick_InRitualInterval(this.ritual);
				return;
			}
			this.Tick_OutOfRitualInterval();
		}

		// Token: 0x06011132 RID: 69938
		protected abstract void Tick_InRitual(LordJob_Ritual ritual);

		// Token: 0x06011133 RID: 69939
		protected abstract void Tick_InRitualInterval(LordJob_Ritual ritual);

		// Token: 0x06011134 RID: 69940
		protected abstract void Tick_OutOfRitualInterval();

		// Token: 0x0400B150 RID: 45392
		protected LordJob_Ritual ritual;

		// Token: 0x0400B151 RID: 45393
		private const int RitualCheckInterval = 30;
	}
}
