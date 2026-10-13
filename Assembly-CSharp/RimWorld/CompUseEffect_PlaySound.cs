using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002F6B RID: 12139
	public class CompUseEffect_PlaySound : CompUseEffect
	{
		// Token: 0x17002C3A RID: 11322
		// (get) Token: 0x060111A1 RID: 70049 RVA: 0x004E8FEB File Offset: 0x004E71EB
		private CompProperties_UseEffectPlaySound Props
		{
			get
			{
				return (CompProperties_UseEffectPlaySound)this.props;
			}
		}

		// Token: 0x060111A2 RID: 70050 RVA: 0x004E8FF8 File Offset: 0x004E71F8
		public override void DoEffect(Pawn usedBy)
		{
			if (usedBy.Map == Find.CurrentMap && this.Props.soundOnUsed != null)
			{
				this.Props.soundOnUsed.PlayOneShot(SoundInfo.InMap(usedBy, MaintenanceType.None));
			}
		}
	}
}
