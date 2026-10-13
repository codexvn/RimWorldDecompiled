using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017D3 RID: 6099
	public class ThinkNode_ConditionalUnderCombatPressure : ThinkNode_Conditional
	{
		// Token: 0x06008DFB RID: 36347 RVA: 0x002A19F1 File Offset: 0x0029FBF1
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Spawned && !pawn.Downed && PawnUtility.EnemiesAreNearby(pawn, 9, true, this.maxThreatDistance, this.minCloseTargets, false);
		}

		// Token: 0x06008DFC RID: 36348 RVA: 0x002A1A1B File Offset: 0x0029FC1B
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalUnderCombatPressure thinkNode_ConditionalUnderCombatPressure = (ThinkNode_ConditionalUnderCombatPressure)base.DeepCopy(resolve);
			thinkNode_ConditionalUnderCombatPressure.maxThreatDistance = this.maxThreatDistance;
			thinkNode_ConditionalUnderCombatPressure.minCloseTargets = this.minCloseTargets;
			return thinkNode_ConditionalUnderCombatPressure;
		}

		// Token: 0x04005BE3 RID: 23523
		public float maxThreatDistance = 2f;

		// Token: 0x04005BE4 RID: 23524
		public int minCloseTargets = 2;
	}
}
