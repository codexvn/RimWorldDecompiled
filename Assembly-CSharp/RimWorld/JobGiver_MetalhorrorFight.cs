using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AA RID: 5290
	public class JobGiver_MetalhorrorFight : JobGiver_AIFightEnemy
	{
		// Token: 0x06007EE8 RID: 32488 RVA: 0x00262D60 File Offset: 0x00260F60
		public JobGiver_MetalhorrorFight()
		{
			this.targetAcquireRadius = 29.9f;
			this.targetKeepRadius = this.targetAcquireRadius * 2f;
		}

		// Token: 0x06007EE9 RID: 32489 RVA: 0x00262D85 File Offset: 0x00260F85
		protected override Thing FindAttackTarget(Pawn pawn)
		{
			return MetalhorrorUtility.FindTarget(pawn);
		}

		// Token: 0x06007EEA RID: 32490 RVA: 0x00262D8D File Offset: 0x00260F8D
		protected override bool TryFindShootingPosition(Pawn pawn, out IntVec3 dest, Verb verbToUse = null)
		{
			dest = IntVec3.Invalid;
			return false;
		}
	}
}
