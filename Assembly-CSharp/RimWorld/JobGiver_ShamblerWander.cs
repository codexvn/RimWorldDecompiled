using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014CE RID: 5326
	public class JobGiver_ShamblerWander : JobGiver_Wander
	{
		// Token: 0x06007F63 RID: 32611 RVA: 0x00265A9D File Offset: 0x00263C9D
		public JobGiver_ShamblerWander()
		{
			this.locomotionUrgency = LocomotionUrgency.Amble;
			this.ticksBetweenWandersRange = new IntRange(480, 900);
		}

		// Token: 0x06007F64 RID: 32612 RVA: 0x00265AC1 File Offset: 0x00263CC1
		protected override Job TryGiveJob(Pawn pawn)
		{
			pawn.mindState.nextMoveOrderIsWait = !Rand.Chance(JobGiver_ShamblerWander.ChanceToWander);
			return base.TryGiveJob(pawn);
		}

		// Token: 0x06007F65 RID: 32613 RVA: 0x00265AE2 File Offset: 0x00263CE2
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			if (pawn.IsGhoul)
			{
				return pawn.Position;
			}
			return this.GetWanderRootHerd(pawn);
		}

		// Token: 0x06007F66 RID: 32614 RVA: 0x00265AFC File Offset: 0x00263CFC
		private IntVec3 GetWanderRootHerd(Pawn pawn)
		{
			Predicate<Thing> isHerdValidator = (Thing t) => ((Pawn)t).IsShambler && t != pawn && t.Faction == pawn.Faction;
			return WanderUtility.GetHerdWanderRoot(pawn, isHerdValidator);
		}

		// Token: 0x04005542 RID: 21826
		private static float ChanceToWander = 0.2f;
	}
}
