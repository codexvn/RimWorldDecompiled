using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

// Token: 0x02000049 RID: 73
public static class CallBossgroupUtility
{
	// Token: 0x0600017F RID: 383 RVA: 0x00009EF0 File Offset: 0x000080F0
	public static void TryStartSummonBossgroupJob(BossgroupDef def, Pawn pawn, bool forced = true)
	{
		ThingDef bossgroupCaller = CallBossgroupUtility.GetBossgroupCaller(def);
		List<Thing> list = pawn.Map.listerThings.ThingsOfDef(bossgroupCaller);
		list.SortBy((Thing t) => t.Position.DistanceToSquared(pawn.Position));
		for (int i = 0; i < list.Count; i++)
		{
			if (pawn.CanReserveAndReach(list[i], PathEndMode.Touch, Danger.Deadly, 1, -1, null, forced) && list[i].TryGetComp<CompUsable>().CanBeUsedBy(pawn, false, false))
			{
				list[i].TryGetComp<CompUsable>().TryStartUseJob(pawn, null, forced);
				return;
			}
		}
	}

	// Token: 0x06000180 RID: 384 RVA: 0x00009FAC File Offset: 0x000081AC
	public static AcceptanceReport BossgroupEverCallable(Pawn pawn, BossgroupDef def, bool forced = true)
	{
		if (!pawn.Spawned)
		{
			return false;
		}
		ThingDef bossgroupCaller = CallBossgroupUtility.GetBossgroupCaller(def);
		List<Thing> list = pawn.Map.listerThings.ThingsOfDef(bossgroupCaller);
		if (list.Count <= 0)
		{
			return "NoSubject".Translate(bossgroupCaller.label);
		}
		bool flag = false;
		for (int i = 0; i < list.Count; i++)
		{
			if (pawn.CanReach(list[i], PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return "NoReachableBossgroupCaller".Translate(bossgroupCaller.label);
		}
		bool flag2 = false;
		for (int j = 0; j < list.Count; j++)
		{
			if (pawn.CanReserve(list[j], 1, -1, null, forced))
			{
				flag2 = true;
				break;
			}
		}
		if (!flag2)
		{
			return "NoReservableBossgroupCaller".Translate(bossgroupCaller.label);
		}
		if (bossgroupCaller.HasComp(typeof(CompPowerTrader)))
		{
			bool flag3 = false;
			for (int k = 0; k < list.Count; k++)
			{
				if (list[k].TryGetComp<CompPowerTrader>().PowerOn)
				{
					flag3 = true;
					break;
				}
			}
			if (!flag3)
			{
				return "NoPoweredBossgroupCaller".Translate(bossgroupCaller.label);
			}
		}
		return true;
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000A110 File Offset: 0x00008310
	public static ThingDef GetBossgroupCaller(BossgroupDef def)
	{
		if (CallBossgroupUtility.cachedBossCallers == null)
		{
			CallBossgroupUtility.cachedBossCallers = (from x in DefDatabase<ThingDef>.AllDefs
			where x.GetCompProperties<CompProperties_Useable_CallBossgroup>() != null
			select x).ToList<ThingDef>();
		}
		foreach (ThingDef thingDef in CallBossgroupUtility.cachedBossCallers)
		{
			CompProperties_Useable_CallBossgroup compProperties = thingDef.GetCompProperties<CompProperties_Useable_CallBossgroup>();
			if (compProperties != null && compProperties.bossgroupDef == def)
			{
				return thingDef;
			}
		}
		return null;
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000A1B0 File Offset: 0x000083B0
	public static PawnKindDef GetPendingBossgroup()
	{
		List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
		for (int i = 0; i < questsListForReading.Count; i++)
		{
			if (questsListForReading[i].State == QuestState.Ongoing)
			{
				List<QuestPart> partsListForReading = questsListForReading[i].PartsListForReading;
				for (int j = 0; j < partsListForReading.Count; j++)
				{
					QuestPart_Bossgroup questPart_Bossgroup = partsListForReading[j] as QuestPart_Bossgroup;
					if (questPart_Bossgroup != null && questPart_Bossgroup.bosses.Count > 0)
					{
						if (questPart_Bossgroup.bosses.All((Pawn p) => !p.Spawned && !p.Dead))
						{
							return questPart_Bossgroup.bosses[0].kindDef;
						}
					}
				}
			}
		}
		return null;
	}

	// Token: 0x040000BE RID: 190
	private static List<ThingDef> cachedBossCallers;
}
