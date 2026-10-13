using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02003D89 RID: 15753
	public static class SettleInExistingMapUtility
	{
		// Token: 0x06015750 RID: 87888 RVA: 0x0066DEA4 File Offset: 0x0066C0A4
		public static Command SettleCommand(Map map, bool requiresNoEnemies)
		{
			Command_Settle command_Settle = new Command_Settle();
			command_Settle.defaultLabel = "CommandSettle".Translate();
			command_Settle.defaultDesc = "CommandSettleDesc".Translate();
			command_Settle.icon = SettleUtility.SettleCommandTex;
			Action <>9__1;
			command_Settle.action = delegate()
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				PlanetTile tile = map.Tile;
				Action settleAction;
				if ((settleAction = <>9__1) == null)
				{
					settleAction = (<>9__1 = delegate()
					{
						SettleInExistingMapUtility.Settle(map);
					});
				}
				SettlementProximityGoodwillUtility.CheckConfirmSettle(tile, settleAction, null, null);
			};
			AcceptanceReport canBeSettled = map.Parent.CanBeSettled;
			if (!canBeSettled.Accepted)
			{
				command_Settle.Disable(canBeSettled.Reason.NullOrEmpty() ? "CommandSettleFailGeneric".Translate() : canBeSettled.Reason);
			}
			else if (SettleUtility.PlayerSettlementsCountLimitReached)
			{
				if (Prefs.MaxNumberOfPlayerSettlements > 1)
				{
					command_Settle.Disable("CommandSettleFailReachedMaximumNumberOfBases".Translate());
				}
				else
				{
					command_Settle.Disable("CommandSettleFailAlreadyHaveBase".Translate());
				}
			}
			if (!command_Settle.Disabled)
			{
				if (map.mapPawns.FreeColonistsCount == 0)
				{
					command_Settle.Disable("CommandSettleFailNoColonists".Translate());
				}
				else if (requiresNoEnemies)
				{
					using (HashSet<IAttackTarget>.Enumerator enumerator = map.attackTargetsCache.TargetsHostileToColony.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (GenHostility.IsActiveThreatToPlayer(enumerator.Current, false))
							{
								command_Settle.Disable("CommandSettleFailEnemies".Translate());
								break;
							}
						}
					}
				}
			}
			return command_Settle;
		}

		// Token: 0x06015751 RID: 87889 RVA: 0x0066E030 File Offset: 0x0066C230
		public static void Settle(Map map)
		{
			MapParent parent = map.Parent;
			Settlement settlement = SettleUtility.AddNewHome(map.Tile, Faction.OfPlayer);
			map.info.parent = settlement;
			if (parent != null)
			{
				parent.Notify_MyMapSettled(map);
				parent.Destroy();
			}
			Messages.Message("MessageSettledInExistingMap".Translate(), settlement, MessageTypeDefOf.PositiveEvent, false);
			SettleInExistingMapUtility.tmpPlayerPawns.Clear();
			SettleInExistingMapUtility.tmpPlayerPawns.AddRange(from x in map.mapPawns.AllPawnsSpawned
			where x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer
			select x);
			CaravanEnterMapUtility.DropAllInventory(SettleInExistingMapUtility.tmpPlayerPawns);
			SettleInExistingMapUtility.tmpPlayerPawns.Clear();
			List<Pawn> prisonersOfColonySpawned = map.mapPawns.PrisonersOfColonySpawned;
			for (int i = 0; i < prisonersOfColonySpawned.Count; i++)
			{
				prisonersOfColonySpawned[i].guest.WaitInsteadOfEscapingForDefaultTicks();
			}
			settlement.Notify_MyMapSettled(map);
		}

		// Token: 0x0400EEE1 RID: 61153
		private static List<Pawn> tmpPlayerPawns = new List<Pawn>();
	}
}
