using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F4E RID: 12110
	public class CompRitualTargetMoteSpawner : CompRitualEffectSpawner
	{
		// Token: 0x17002C29 RID: 11305
		// (get) Token: 0x06011137 RID: 69943 RVA: 0x004E7969 File Offset: 0x004E5B69
		private CompProperties_RitualTargetMoteSpawner Props
		{
			get
			{
				return (CompProperties_RitualTargetMoteSpawner)this.props;
			}
		}

		// Token: 0x06011138 RID: 69944 RVA: 0x004E7976 File Offset: 0x004E5B76
		protected override void Tick_InRitual(LordJob_Ritual ritual)
		{
			Mote mote = this.mote;
			if (mote == null)
			{
				return;
			}
			mote.Maintain();
		}

		// Token: 0x06011139 RID: 69945 RVA: 0x004E7988 File Offset: 0x004E5B88
		protected override void Tick_InRitualInterval(LordJob_Ritual ritual)
		{
			if (this.mote == null || this.mote.Destroyed)
			{
				this.mote = MoteMaker.MakeStaticMote(this.parent.Position.ToVector3Shifted(), this.parent.Map, this.Props.mote, 1f, false, 0f);
			}
		}

		// Token: 0x0601113A RID: 69946 RVA: 0x004E79E9 File Offset: 0x004E5BE9
		protected override void Tick_OutOfRitualInterval()
		{
			if (this.mote != null && !this.mote.Destroyed)
			{
				Mote mote = this.mote;
				if (mote != null)
				{
					mote.Destroy(DestroyMode.Vanish);
				}
			}
			this.mote = null;
		}

		// Token: 0x0400B153 RID: 45395
		private Mote mote;
	}
}
