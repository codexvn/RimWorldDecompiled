using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016DD RID: 5853
	public class LordToil_ReturnedCaravan_PenAnimals : LordToil_PrepareCaravan_RopeAnimals
	{
		// Token: 0x06008A2E RID: 35374 RVA: 0x002937C6 File Offset: 0x002919C6
		public LordToil_ReturnedCaravan_PenAnimals(IntVec3 entryPoint) : base(entryPoint, new int?(int.MaxValue))
		{
		}

		// Token: 0x06008A2F RID: 35375 RVA: 0x002937E4 File Offset: 0x002919E4
		public override void LordToilTick()
		{
			if (Find.TickManager.TicksGame % 100 == 0)
			{
				this.tmpOwnedPawns.Clear();
				this.tmpOwnedPawns.AddRange(this.lord.ownedPawns);
				bool flag = false;
				bool flag2 = false;
				foreach (Pawn pawn in this.tmpOwnedPawns)
				{
					if (AnimalPenUtility.NeedsToBeManagedByRope(pawn))
					{
						if (this.tmpOwnedPawns.Contains(pawn.roping.RopedByPawn))
						{
							flag2 = true;
						}
						else
						{
							this.lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
						}
					}
					else if (pawn.Spawned && pawn.IsColonist && !pawn.Downed && !pawn.Dead && pawn.roping.HasAnyRope)
					{
						flag = true;
					}
					else
					{
						this.lord.Notify_PawnLost(pawn, PawnLostCondition.LeftVoluntarily, null);
					}
				}
				if (!flag || !flag2)
				{
					this.lord.ReceiveMemo("RepenningFinished");
				}
				this.tmpOwnedPawns.Clear();
			}
		}

		// Token: 0x06008A30 RID: 35376 RVA: 0x00293914 File Offset: 0x00291B14
		protected override PawnDuty MakeRopeDuty()
		{
			return new PawnDuty(DutyDefOf.ReturnedCaravan_PenAnimals);
		}

		// Token: 0x04005A17 RID: 23063
		private List<Pawn> tmpOwnedPawns = new List<Pawn>();
	}
}
