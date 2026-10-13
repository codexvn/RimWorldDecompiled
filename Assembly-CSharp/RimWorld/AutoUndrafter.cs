using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002442 RID: 9282
	public class AutoUndrafter : IExposable
	{
		// Token: 0x0600C8E0 RID: 51424 RVA: 0x003AAEAB File Offset: 0x003A90AB
		public AutoUndrafter(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600C8E1 RID: 51425 RVA: 0x003AAEBA File Offset: 0x003A90BA
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastNonWaitingTick, "lastNonWaitingTick", 0, false);
		}

		// Token: 0x0600C8E2 RID: 51426 RVA: 0x003AAED0 File Offset: 0x003A90D0
		public void AutoUndraftTickInterval(int delta)
		{
			if (GenTicks.IsTickIntervalDelta(100, delta) || !this.pawn.Drafted)
			{
				return;
			}
			if ((this.pawn.jobs.curJob != null && this.pawn.jobs.curJob.def != JobDefOf.Wait_Combat) || this.AnyHostilePreventingAutoUndraft())
			{
				this.lastNonWaitingTick = Find.TickManager.TicksGame;
			}
			if (this.ShouldAutoUndraft())
			{
				this.pawn.drafter.Drafted = false;
			}
		}

		// Token: 0x0600C8E3 RID: 51427 RVA: 0x003AAF54 File Offset: 0x003A9154
		public void Notify_Drafted()
		{
			this.lastNonWaitingTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600C8E4 RID: 51428 RVA: 0x003AAF66 File Offset: 0x003A9166
		private bool ShouldAutoUndraft()
		{
			return !this.pawn.IsColonyMech && Find.TickManager.TicksGame - this.lastNonWaitingTick >= 10000 && !this.AnyHostilePreventingAutoUndraft();
		}

		// Token: 0x0600C8E5 RID: 51429 RVA: 0x003AAF9C File Offset: 0x003A919C
		private bool AnyHostilePreventingAutoUndraft()
		{
			List<IAttackTarget> potentialTargetsFor = this.pawn.Map.attackTargetsCache.GetPotentialTargetsFor(this.pawn);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				if (GenHostility.IsActiveThreatToPlayer(potentialTargetsFor[i], false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040088B5 RID: 34997
		private Pawn pawn;

		// Token: 0x040088B6 RID: 34998
		private int lastNonWaitingTick;

		// Token: 0x040088B7 RID: 34999
		private const int UndraftDelay = 10000;
	}
}
