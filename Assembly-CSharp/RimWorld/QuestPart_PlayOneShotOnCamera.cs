using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001C79 RID: 7289
	public class QuestPart_PlayOneShotOnCamera : QuestPart
	{
		// Token: 0x0600A278 RID: 41592 RVA: 0x002EB0A0 File Offset: 0x002E92A0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.soundDef.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600A279 RID: 41593 RVA: 0x002EB0C8 File Offset: 0x002E92C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<SoundDef>(ref this.soundDef, "soundDef");
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x04007042 RID: 28738
		public SoundDef soundDef;

		// Token: 0x04007043 RID: 28739
		public string inSignal;
	}
}
