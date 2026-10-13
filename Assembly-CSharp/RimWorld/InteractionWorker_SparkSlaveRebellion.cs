using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200240C RID: 9228
	public class InteractionWorker_SparkSlaveRebellion : InteractionWorker
	{
		// Token: 0x0600C7DE RID: 51166 RVA: 0x003A5DD8 File Offset: 0x003A3FD8
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			if (!SlaveRebellionUtility.CanParticipateInSlaveRebellion(recipient))
			{
				letterText = null;
				letterLabel = null;
				letterDef = null;
				lookTargets = null;
				return;
			}
			SlaveRebellionUtility.StartSlaveRebellion(recipient, out letterText, out letterLabel, out letterDef, out lookTargets, false);
			lookTargets = new LookTargets(new TargetInfo[]
			{
				initiator,
				recipient
			});
		}
	}
}
