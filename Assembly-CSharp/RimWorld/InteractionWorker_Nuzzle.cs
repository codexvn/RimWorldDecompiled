using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002402 RID: 9218
	public class InteractionWorker_Nuzzle : InteractionWorker
	{
		// Token: 0x0600C7B7 RID: 51127 RVA: 0x003A41A0 File Offset: 0x003A23A0
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets)
		{
			this.AddNuzzledThought(initiator, recipient);
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
		}

		// Token: 0x0600C7B8 RID: 51128 RVA: 0x003A41BC File Offset: 0x003A23BC
		private void AddNuzzledThought(Pawn initiator, Pawn recipient)
		{
			Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.Nuzzled);
			thought_Memory.SetForcedStage(NuzzleUtility.GetNuzzleStageIndex(initiator));
			if (recipient.needs.mood != null)
			{
				recipient.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
			}
		}
	}
}
