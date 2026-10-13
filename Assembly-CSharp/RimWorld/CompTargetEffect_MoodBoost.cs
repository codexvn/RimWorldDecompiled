using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F37 RID: 12087
	public class CompTargetEffect_MoodBoost : CompTargetEffect
	{
		// Token: 0x060110DE RID: 69854 RVA: 0x004E6A64 File Offset: 0x004E4C64
		public override void DoEffectOn(Pawn user, Thing target)
		{
			Pawn pawn = (Pawn)target;
			if (pawn.Dead || pawn.needs == null || pawn.needs.mood == null)
			{
				return;
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(ThoughtDefOf.ArtifactMoodBoost), null);
		}
	}
}
