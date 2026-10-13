using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020027C9 RID: 10185
	public class RitualStageAction_SoundOneshotOnTarget : RitualStageAction
	{
		// Token: 0x0600DF03 RID: 57091 RVA: 0x00418EB7 File Offset: 0x004170B7
		public override void Apply(LordJob_Ritual ritual)
		{
			this.sound.PlayOneShot(SoundInfo.InMap(ritual.selectedTarget, MaintenanceType.None));
		}

		// Token: 0x0600DF04 RID: 57092 RVA: 0x00418ED0 File Offset: 0x004170D0
		public override void ExposeData()
		{
			Scribe_Defs.Look<SoundDef>(ref this.sound, "sound");
		}

		// Token: 0x0400962A RID: 38442
		public SoundDef sound;
	}
}
