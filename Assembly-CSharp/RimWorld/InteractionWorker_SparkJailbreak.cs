using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200240B RID: 9227
	public class InteractionWorker_SparkJailbreak : InteractionWorker
	{
		// Token: 0x0600C7DC RID: 51164 RVA: 0x003A5D50 File Offset: 0x003A3F50
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			if (!recipient.IsPrisoner || !recipient.guest.PrisonerIsSecure || !PrisonBreakUtility.CanParticipateInPrisonBreak(recipient))
			{
				letterText = null;
				letterLabel = null;
				letterDef = null;
				lookTargets = null;
				return;
			}
			List<Pawn> list;
			PrisonBreakUtility.StartPrisonBreak(recipient, out letterText, out letterLabel, out letterDef, out list);
			lookTargets = new LookTargets(new TargetInfo[]
			{
				initiator,
				recipient
			});
			MentalState_Jailbreaker mentalState_Jailbreaker = initiator.MentalState as MentalState_Jailbreaker;
			if (mentalState_Jailbreaker != null)
			{
				mentalState_Jailbreaker.Notify_InducedPrisonerToEscape();
			}
		}
	}
}
