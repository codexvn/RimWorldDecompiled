using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002AC3 RID: 10947
	public class SignalAction_SoundOneShot : SignalAction
	{
		// Token: 0x0600F3E3 RID: 62435 RVA: 0x0046DB1F File Offset: 0x0046BD1F
		protected override void DoAction(SignalArgs args)
		{
			this.sound.PlayOneShot(this);
		}

		// Token: 0x0600F3E4 RID: 62436 RVA: 0x0046DB32 File Offset: 0x0046BD32
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<SoundDef>(ref this.sound, "sound");
		}

		// Token: 0x0400A0B2 RID: 41138
		public SoundDef sound;
	}
}
