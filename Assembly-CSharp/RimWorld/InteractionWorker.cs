using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A5B RID: 6747
	public class InteractionWorker
	{
		// Token: 0x060097A2 RID: 38818 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0f;
		}

		// Token: 0x060097A3 RID: 38819 RVA: 0x002C4A61 File Offset: 0x002C2C61
		public virtual void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
		}

		// Token: 0x040064FA RID: 25850
		public InteractionDef interaction;
	}
}
