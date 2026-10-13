using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200170E RID: 5902
	public class LordToil_MoveInBossgroup : LordToil
	{
		// Token: 0x17001557 RID: 5463
		// (get) Token: 0x06008AFC RID: 35580 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001558 RID: 5464
		// (get) Token: 0x06008AFD RID: 35581 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008AFE RID: 35582 RVA: 0x00296D81 File Offset: 0x00294F81
		public LordToil_MoveInBossgroup(IEnumerable<Pawn> bosses)
		{
			this.bosses.AddRange(bosses);
		}

		// Token: 0x06008AFF RID: 35583 RVA: 0x00296DA0 File Offset: 0x00294FA0
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (this.bosses.Contains(this.lord.ownedPawns[i]))
				{
					this.bosses[i].mindState.duty = new PawnDuty(this.bosses[i].RaceProps.dutyBoss ?? DutyDefOf.AssaultColony);
				}
				else
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.Escort, this.bosses.RandomElement<Pawn>(), LordToil_MoveInBossgroup.EscortRadiusRanged.RandomInRange);
				}
			}
		}

		// Token: 0x04005A60 RID: 23136
		private static readonly FloatRange EscortRadiusRanged = new FloatRange(5f, 10f);

		// Token: 0x04005A61 RID: 23137
		private List<Pawn> bosses = new List<Pawn>();
	}
}
