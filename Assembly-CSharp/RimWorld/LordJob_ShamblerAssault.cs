using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001671 RID: 5745
	public class LordJob_ShamblerAssault : LordJob
	{
		// Token: 0x06008769 RID: 34665 RVA: 0x0028714C File Offset: 0x0028534C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil toil = new LordToil_ShamblerAssault();
			stateGraph.AddToil(toil);
			return stateGraph;
		}

		// Token: 0x0600876A RID: 34666 RVA: 0x0028716B File Offset: 0x0028536B
		public override bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return (reason != PawnLostCondition.Incapped || !p.IsMutant || !p.mutant.Def.canAttackWhileCrawling || !p.health.CanCrawl) && base.ShouldRemovePawn(p, reason);
		}

		// Token: 0x0600876B RID: 34667 RVA: 0x002871A4 File Offset: 0x002853A4
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
