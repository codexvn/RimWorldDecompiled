using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F50 RID: 12112
	public class CompRitualTargetEffecterSpawner : CompRitualEffectSpawner
	{
		// Token: 0x17002C2A RID: 11306
		// (get) Token: 0x0601113D RID: 69949 RVA: 0x004E7A39 File Offset: 0x004E5C39
		private CompProperties_RitualTargetEffecterSpawner Props
		{
			get
			{
				return (CompProperties_RitualTargetEffecterSpawner)this.props;
			}
		}

		// Token: 0x0601113E RID: 69950 RVA: 0x004E7A46 File Offset: 0x004E5C46
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecter = null;
		}

		// Token: 0x0601113F RID: 69951 RVA: 0x000026EA File Offset: 0x000008EA
		protected override void Tick_InRitual(LordJob_Ritual ritual)
		{
		}

		// Token: 0x06011140 RID: 69952 RVA: 0x004E7A68 File Offset: 0x004E5C68
		protected override void Tick_InRitualInterval(LordJob_Ritual ritual)
		{
			if (this.Props.minRitualProgress > 0f && ritual.Progress < this.Props.minRitualProgress)
			{
				return;
			}
			if (this.effecter == null)
			{
				this.effecter = this.Props.effecter.Spawn();
				this.effecter.Trigger(this.parent, this.parent, -1);
			}
			this.effecter.EffectTick(this.parent, this.parent);
		}

		// Token: 0x06011141 RID: 69953 RVA: 0x004E7AFD File Offset: 0x004E5CFD
		protected override void Tick_OutOfRitualInterval()
		{
			Effecter effecter = this.effecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effecter = null;
		}

		// Token: 0x0400B156 RID: 45398
		private Effecter effecter;
	}
}
