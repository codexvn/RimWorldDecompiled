using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020023FF RID: 9215
	public class InteractionWorker_InterrogateIdentity : InteractionWorker
	{
		// Token: 0x0600C7B0 RID: 51120 RVA: 0x003A3AD4 File Offset: 0x003A1CD4
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
			if (Rand.Chance(InterrogateUtility.GetChance(initiator)))
			{
				TaggedString taggedString = "MetalhorrorReasonInterrogation".Translate(initiator.Named("PAWN"), recipient.Named("INFECTED"));
				TaggedString taggedString2 = "MetalhorrorNoticedDetailsAppended".Translate(initiator.Named("PAWN"), recipient.Named("INFECTED"));
				TaggedString taggedString3 = "InteractionDetectedDesc".Translate(initiator.Named("PAWN"), recipient.Named("INFECTED"));
				taggedString3 += string.Format("\n\n{0}", taggedString2);
				MetalhorrorUtility.Detect(recipient, taggedString, taggedString3, 0.5f);
			}
		}

		// Token: 0x0400883C RID: 34876
		private const float MetalhorrorDetectionNoticed = 0.5f;
	}
}
