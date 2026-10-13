using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200240D RID: 9229
	public class InteractionWorker_Suppress : InteractionWorker
	{
		// Token: 0x0600C7E0 RID: 51168 RVA: 0x003A5E36 File Offset: 0x003A4036
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
			SlaveRebellionUtility.IncrementInteractionSuppression(initiator, recipient);
		}
	}
}
