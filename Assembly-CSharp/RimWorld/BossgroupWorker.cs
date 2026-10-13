using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001A11 RID: 6673
	public class BossgroupWorker
	{
		// Token: 0x0600967C RID: 38524 RVA: 0x002C084C File Offset: 0x002BEA4C
		public virtual AcceptanceReport CanResolve(Pawn caller)
		{
			int lastBossgroupCalled = Find.BossgroupManager.lastBossgroupCalled;
			int num = Find.TickManager.TicksGame - lastBossgroupCalled;
			if (num < 120000)
			{
				return "BossgroupAvailableIn".Translate((120000 - num).ToStringTicksToPeriod(true, false, true, true, false));
			}
			PawnKindDef pendingBossgroup = CallBossgroupUtility.GetPendingBossgroup();
			if (pendingBossgroup != null)
			{
				return "BossgroupIncoming".Translate(pendingBossgroup.label);
			}
			return true;
		}

		// Token: 0x0600967D RID: 38525 RVA: 0x002C08CC File Offset: 0x002BEACC
		public virtual AcceptanceReport ShouldSummonNow(Map map)
		{
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob lordJob = lords[i].LordJob;
				if (lordJob is LordJob_TradeWithColony || lordJob is LordJob_BestowingCeremony || lordJob is LordJob_VisitColony)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600967E RID: 38526 RVA: 0x002C0928 File Offset: 0x002BEB28
		public virtual void Resolve(Map map, int wave)
		{
			Find.BossgroupManager.Notify_BossgroupCalled(this.def);
			Slate slate = new Slate();
			slate.Set<BossgroupDef>("bossgroup", this.def, false);
			slate.Set<Map>("map", map, false);
			slate.Set<ThingDef>("reward", this.def.rewardDef, false);
			slate.Set<int>("wave", wave, false);
			QuestUtility.GenerateQuestAndMakeAvailable(this.def.quest, slate);
		}

		// Token: 0x040062C1 RID: 25281
		public const int TimeBetweenAllBossgroups = 120000;

		// Token: 0x040062C2 RID: 25282
		public BossgroupDef def;
	}
}
