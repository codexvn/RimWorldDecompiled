using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027C7 RID: 10183
	public class RitualStageAction_Message : RitualStageAction
	{
		// Token: 0x0600DEFD RID: 57085 RVA: 0x00418DE0 File Offset: 0x00416FE0
		public override void Apply(LordJob_Ritual ritual)
		{
			Messages.Message(this.text.Formatted(ritual.Ritual.Label).CapitalizeFirst(), ritual.selectedTarget, this.messageTypeDef, false);
		}

		// Token: 0x0600DEFE RID: 57086 RVA: 0x00418E2C File Offset: 0x0041702C
		public override void ExposeData()
		{
			Scribe_Defs.Look<MessageTypeDef>(ref this.messageTypeDef, "messageTypeDef");
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
		}

		// Token: 0x04009626 RID: 38438
		[MustTranslate]
		public string text;

		// Token: 0x04009627 RID: 38439
		public MessageTypeDef messageTypeDef;
	}
}
