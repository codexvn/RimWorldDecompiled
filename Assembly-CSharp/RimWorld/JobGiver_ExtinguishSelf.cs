using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001481 RID: 5249
	public class JobGiver_ExtinguishSelf : ThinkNode_JobGiver
	{
		// Token: 0x06007E65 RID: 32357 RVA: 0x00260B2C File Offset: 0x0025ED2C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Rand.Value < 0.1f)
			{
				Fire fire = (Fire)pawn.GetAttachment(ThingDefOf.Fire);
				if (fire != null)
				{
					return JobMaker.MakeJob(JobDefOf.ExtinguishSelf, fire);
				}
			}
			return null;
		}

		// Token: 0x040054C1 RID: 21697
		private const float ActivateChance = 0.1f;
	}
}
