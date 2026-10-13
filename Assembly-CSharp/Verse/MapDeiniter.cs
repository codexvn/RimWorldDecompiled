using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000467 RID: 1127
	public static class MapDeiniter
	{
		// Token: 0x06001D9B RID: 7579 RVA: 0x00096548 File Offset: 0x00094748
		public static void Deinit(Map map, bool notifyPlayer)
		{
			try
			{
				MapDeiniter.DoQueuedPowerTasks(map);
			}
			catch (Exception ex)
			{
				string str = "Error while deiniting map: could not execute power related tasks: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			try
			{
				MapDeiniter.PassPawnsToWorld(map, notifyPlayer);
			}
			catch (Exception ex3)
			{
				string str2 = "Error while deiniting map: could not pass pawns to world: ";
				Exception ex4 = ex3;
				Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
			}
			try
			{
				foreach (Pawn pawn in PawnsFinder.All_AliveOrDead)
				{
					if (pawn.playerSettings != null)
					{
						pawn.playerSettings.Notify_MapRemoved(map);
					}
				}
			}
			catch (Exception ex5)
			{
				string str3 = "Error while deiniting map: could not notify pawn player settings: ";
				Exception ex6 = ex5;
				Log.Error(str3 + ((ex6 != null) ? ex6.ToString() : null));
			}
			try
			{
				map.weatherManager.EndAllSustainers();
			}
			catch (Exception ex7)
			{
				string str4 = "Error while deiniting map: could not end all weather sustainers: ";
				Exception ex8 = ex7;
				Log.Error(str4 + ((ex8 != null) ? ex8.ToString() : null));
			}
			try
			{
				Find.SoundRoot.sustainerManager.EndAllInMap(map);
			}
			catch (Exception ex9)
			{
				string str5 = "Error while deiniting map: could not end all effect sustainers: ";
				Exception ex10 = ex9;
				Log.Error(str5 + ((ex10 != null) ? ex10.ToString() : null));
			}
			try
			{
				map.areaManager.Notify_MapRemoved();
			}
			catch (Exception ex11)
			{
				string str6 = "Error while deiniting map: could not remove areas: ";
				Exception ex12 = ex11;
				Log.Error(str6 + ((ex12 != null) ? ex12.ToString() : null));
			}
			try
			{
				map.lordManager.Notify_MapRemoved();
			}
			catch (Exception ex13)
			{
				string str7 = "Error while deiniting map and notifying LordManager: ";
				Exception ex14 = ex13;
				Log.Error(str7 + ((ex14 != null) ? ex14.ToString() : null));
			}
			try
			{
				map.deferredSpawner.Notify_MapRemoved();
			}
			catch (Exception ex15)
			{
				string str8 = "Error while deiniting map and notifying DeferredSpawner: ";
				Exception ex16 = ex15;
				Log.Error(str8 + ((ex16 != null) ? ex16.ToString() : null));
			}
			try
			{
				Find.Anomaly.Notify_MapRemoved(map);
			}
			catch (Exception ex17)
			{
				string str9 = "Error while deiniting map and notifying Anomaly component: ";
				Exception ex18 = ex17;
				Log.Error(str9 + ((ex18 != null) ? ex18.ToString() : null));
			}
			try
			{
				Find.TickManager.RemoveAllFromMap(map);
			}
			catch (Exception ex19)
			{
				string str10 = "Error while deiniting map: could not remove things from the tick manager: ";
				Exception ex20 = ex19;
				Log.Error(str10 + ((ex20 != null) ? ex20.ToString() : null));
			}
			try
			{
				MapDeiniter.NotifyEverythingWhichUsesMapReference(map);
			}
			catch (Exception ex21)
			{
				string str11 = "Error while deiniting map: could not notify things/regions/rooms/etc: ";
				Exception ex22 = ex21;
				Log.Error(str11 + ((ex22 != null) ? ex22.ToString() : null));
			}
			try
			{
				map.listerThings.Clear();
				map.spawnedThings.Clear();
			}
			catch (Exception ex23)
			{
				string str12 = "Error while deiniting map: could not remove things from thing listers: ";
				Exception ex24 = ex23;
				Log.Error(str12 + ((ex24 != null) ? ex24.ToString() : null));
			}
			try
			{
				Find.Archive.Notify_MapRemoved(map);
			}
			catch (Exception ex25)
			{
				string str13 = "Error while deiniting map: could not remove look targets: ";
				Exception ex26 = ex25;
				Log.Error(str13 + ((ex26 != null) ? ex26.ToString() : null));
			}
			try
			{
				Find.Storyteller.incidentQueue.Notify_MapRemoved(map);
			}
			catch (Exception ex27)
			{
				string str14 = "Error while deiniting map: could not remove queued incidents: ";
				Exception ex28 = ex27;
				Log.Error(str14 + ((ex28 != null) ? ex28.ToString() : null));
			}
		}

		// Token: 0x06001D9C RID: 7580 RVA: 0x000968D8 File Offset: 0x00094AD8
		private static void DoQueuedPowerTasks(Map map)
		{
			map.powerNetManager.UpdatePowerNetsAndConnections_First();
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x000968E8 File Offset: 0x00094AE8
		private static void PassPawnsToWorld(Map map, bool notifyPlayer)
		{
			List<Pawn> list = new List<Pawn>();
			List<Pawn> list2 = new List<Pawn>();
			bool flag = map.ParentFaction != null && map.ParentFaction.HostileTo(Faction.OfPlayer);
			List<Pawn> list3 = map.mapPawns.AllPawns.ToList<Pawn>();
			for (int i = 0; i < list3.Count; i++)
			{
				Find.Storyteller.Notify_PawnEvent(list3[i], AdaptationEvent.LostBecauseMapClosed, null);
				try
				{
					Pawn pawn = list3[i];
					pawn.DeSpawnOrDeselect(DestroyMode.Vanish);
					if (pawn.IsColonist && flag)
					{
						list.Add(pawn);
						map.ParentFaction.kidnapped.Kidnap(pawn, null);
					}
					else
					{
						if (pawn.Faction == Faction.OfPlayer || pawn.HostFaction == Faction.OfPlayer)
						{
							list2.Add(pawn);
							PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(pawn, null, PawnDiedOrDownedThoughtsKind.Lost);
						}
						MapDeiniter.CleanUpAndPassToWorld(pawn);
					}
				}
				catch (Exception ex)
				{
					string str = "Could not despawn and pass to world ";
					Pawn pawn2 = list3[i];
					string str2 = (pawn2 != null) ? pawn2.ToString() : null;
					string str3 = ": ";
					Exception ex2 = ex;
					Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				QuestUtility.SendQuestTargetSignals(list[j].questTags, "LeftMap", list[j].Named("SUBJECT"));
			}
			for (int k = 0; k < list2.Count; k++)
			{
				QuestUtility.SendQuestTargetSignals(list2[k].questTags, "LeftMap", list2[k].Named("SUBJECT"));
			}
			if (notifyPlayer && (list.Any<Pawn>() || list2.Any<Pawn>()))
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (list.Any<Pawn>())
				{
					list.SortByDescending((Pawn x) => x.RaceProps.Humanlike);
					for (int l = 0; l < list.Count; l++)
					{
						stringBuilder.AppendLineTagged("  - " + list[l].NameShortColored.CapitalizeFirst() + ": " + "capturedBy".Translate(map.ParentFaction.NameColored).CapitalizeFirst());
					}
				}
				if (list2.Any<Pawn>())
				{
					list2.SortByDescending((Pawn x) => x.RaceProps.Humanlike);
					for (int m = 0; m < list2.Count; m++)
					{
						stringBuilder.AppendLine("  - " + list2[m].NameShortColored.Resolve().CapitalizeFirst());
					}
				}
				string str4;
				string text;
				if (map.IsPlayerHome || map.IsPocketMap)
				{
					str4 = "LetterLabelPawnsLostBecauseMapClosed_Home".Translate();
					text = (ModsConfig.BiotechActive ? "LetterPawnsLostIncMechsBecauseMapClosed_Home".Translate() : "LetterPawnsLostBecauseMapClosed_Home".Translate());
				}
				else
				{
					str4 = "LetterLabelPawnsLostBecauseMapClosed_Caravan".Translate();
					text = (ModsConfig.BiotechActive ? "LetterPawnsLostIncMechsBecauseMapClosed_Caravan".Translate() : "LetterPawnsLostBecauseMapClosed_Caravan".Translate());
				}
				text = text + ":\n\n" + stringBuilder.ToString().TrimEndNewlines();
				Find.LetterStack.ReceiveLetter(str4, text, LetterDefOf.NegativeEvent, new GlobalTargetInfo(map.Tile), null, null, null, null, 0, true);
			}
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x00096CA0 File Offset: 0x00094EA0
		private static void CleanUpAndPassToWorld(Pawn p)
		{
			if (p.ownership != null)
			{
				p.ownership.UnclaimAll();
			}
			if (p.guest != null)
			{
				p.guest.SetGuestStatus(null, GuestStatus.Guest);
			}
			p.inventory.UnloadEverything = false;
			if (ModsConfig.BiotechActive && p.IsColonyMech)
			{
				Pawn overseer = p.GetOverseer();
				if (overseer != null)
				{
					p.relations.RemoveDirectRelation(PawnRelationDefOf.Overseer, overseer);
				}
			}
			Find.WorldPawns.PassToWorld(p, PawnDiscardDecideMode.Decide);
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x00096D18 File Offset: 0x00094F18
		private static void NotifyEverythingWhichUsesMapReference(Map map)
		{
			List<Map> maps = Find.Maps;
			int num = maps.IndexOf(map);
			ThingOwnerUtility.GetAllThingsRecursively(map, MapDeiniter.tmpThings, true, null);
			for (int i = 0; i < MapDeiniter.tmpThings.Count; i++)
			{
				MapDeiniter.tmpThings[i].Notify_MyMapRemoved();
				if (MapDeiniter.tmpThings[i].Discarded)
				{
					MapDeiniter.tmpThings[i].Notify_AbandonedAtTile(map.Tile);
				}
			}
			MapDeiniter.tmpThings.Clear();
			map.spawnedThings.Clear();
			for (int j = num; j < maps.Count; j++)
			{
				ThingOwner spawnedThings = maps[j].spawnedThings;
				for (int k = 0; k < spawnedThings.Count; k++)
				{
					if (j != num)
					{
						spawnedThings[k].DecrementMapIndex();
					}
				}
				IReadOnlyList<Room> allRooms = maps[j].regionGrid.AllRooms;
				for (int l = 0; l < allRooms.Count; l++)
				{
					List<District> districts = allRooms[l].Districts;
					for (int m = 0; m < districts.Count; m++)
					{
						if (j == num)
						{
							districts[m].Notify_MyMapRemoved();
						}
						else
						{
							districts[m].DecrementMapIndex();
						}
					}
				}
				foreach (Region region in maps[j].regionGrid.AllRegions_NoRebuild_InvalidAllowed)
				{
					if (j == num)
					{
						region.Notify_MyMapRemoved();
					}
					else
					{
						region.DecrementMapIndex();
					}
				}
			}
		}

		// Token: 0x040019B7 RID: 6583
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
