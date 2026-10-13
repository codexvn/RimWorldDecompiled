using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001672 RID: 5746
	public class LordJob_ShamblerSwarm : LordJob_EntitySwarm
	{
		// Token: 0x0600876D RID: 34669 RVA: 0x002871DB File Offset: 0x002853DB
		public LordJob_ShamblerSwarm()
		{
		}

		// Token: 0x0600876E RID: 34670 RVA: 0x002871E3 File Offset: 0x002853E3
		public LordJob_ShamblerSwarm(IntVec3 startPos, IntVec3 destPos) : base(startPos, destPos)
		{
		}

		// Token: 0x0600876F RID: 34671 RVA: 0x002871ED File Offset: 0x002853ED
		protected override LordToil CreateTravelingToil(IntVec3 start, IntVec3 dest)
		{
			return new LordToil_ShamblerSwarm(start, dest);
		}

		// Token: 0x06008770 RID: 34672 RVA: 0x0028716B File Offset: 0x0028536B
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return (reason != PawnLostCondition.Incapped || !p.IsMutant || !p.mutant.Def.canAttackWhileCrawling || !p.health.CanCrawl) && base.ShouldRemovePawn(p, reason);
		}

		// Token: 0x06008771 RID: 34673 RVA: 0x002871F8 File Offset: 0x002853F8
		public override void Notify_PawnDowned(Pawn p)
		{
			base.Notify_PawnDowned(p);
			if (!p.health.CanCrawl)
			{
				this.lord.Notify_PawnLost(p, PawnLostCondition.Incapped, null);
			}
		}
	}
}
