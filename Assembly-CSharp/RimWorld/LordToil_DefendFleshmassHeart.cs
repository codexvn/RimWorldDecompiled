using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016EF RID: 5871
	public class LordToil_DefendFleshmassHeart : LordToil
	{
		// Token: 0x17001541 RID: 5441
		// (get) Token: 0x06008A7E RID: 35454 RVA: 0x00294CC4 File Offset: 0x00292EC4
		private LordToilData_DefendFleshmassHeart Data
		{
			get
			{
				return (LordToilData_DefendFleshmassHeart)this.data;
			}
		}

		// Token: 0x06008A7F RID: 35455 RVA: 0x00294CD1 File Offset: 0x00292ED1
		private LordToil_DefendFleshmassHeart()
		{
			this.data = new LordToilData_DefendFleshmassHeart();
		}

		// Token: 0x06008A80 RID: 35456 RVA: 0x00294CE4 File Offset: 0x00292EE4
		public LordToil_DefendFleshmassHeart(Building_FleshmassHeart heart) : this()
		{
			this.Data.heart = heart;
		}

		// Token: 0x06008A81 RID: 35457 RVA: 0x00294CF8 File Offset: 0x00292EF8
		public override void UpdateAllDuties()
		{
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				this.UpdateDefendPoint(pawn);
			}
		}

		// Token: 0x06008A82 RID: 35458 RVA: 0x00294D50 File Offset: 0x00292F50
		public override void LordToilTick()
		{
			if (this.Data.heart.DestroyedOrNull())
			{
				base.Map.lordManager.RemoveLord(this.lord);
				return;
			}
			foreach (Pawn pawn in this.lord.ownedPawns)
			{
				if (pawn.IsHashIntervalTick(3600))
				{
					this.UpdateDefendPoint(pawn);
				}
			}
		}

		// Token: 0x06008A83 RID: 35459 RVA: 0x00294DE0 File Offset: 0x00292FE0
		private void UpdateDefendPoint(Pawn pawn)
		{
			if (pawn.mindState == null)
			{
				return;
			}
			if (pawn.mindState.CombatantRecently)
			{
				return;
			}
			IntVec3 intVec = IntVec3.Invalid;
			int i;
			for (i = 0; i < 100; i++)
			{
				if (!this.Data.heart.Comp.ContiguousFleshmass.TryRandomElement(out intVec))
				{
					intVec = this.Data.heart.Position;
					break;
				}
				bool flag = false;
				foreach (IntVec3 b in GenAdj.CardinalDirections)
				{
					IntVec3 c = intVec + b;
					if (c.InBounds(base.Map) && c.Standable(base.Map))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (i == 100 || intVec == IntVec3.Invalid)
			{
				intVec = this.Data.heart.Position;
			}
			pawn.mindState.duty = new PawnDuty(DutyDefOf.DefendFleshmassHeart, intVec, -1f);
			pawn.mindState.duty.focusSecond = intVec;
			pawn.mindState.duty.radius = 50f;
			pawn.mindState.duty.wanderRadius = new float?(12f);
		}

		// Token: 0x06008A84 RID: 35460 RVA: 0x00294F2C File Offset: 0x0029312C
		public override void Notify_PawnDamaged(Pawn victim, DamageInfo dinfo)
		{
			if (dinfo.Instigator == null)
			{
				return;
			}
			if (!victim.HostileTo(dinfo.Instigator))
			{
				return;
			}
			this.lord.Notify_PawnLost(victim, PawnLostCondition.LeftVoluntarily, null);
			victim.mindState.enemyTarget = dinfo.Instigator;
			victim.mindState.Notify_EngagedTarget();
			victim.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
		}

		// Token: 0x04005A38 RID: 23096
		private const float DefendRadius = 50f;

		// Token: 0x04005A39 RID: 23097
		private const float WanderRadius = 12f;

		// Token: 0x04005A3A RID: 23098
		private const int SwitchDefendPointInterval = 3600;
	}
}
