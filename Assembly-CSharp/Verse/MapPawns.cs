using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000452 RID: 1106
	public sealed class MapPawns
	{
		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001C9A RID: 7322 RVA: 0x0008FDFC File Offset: 0x0008DFFC
		public List<Pawn> AllPawns
		{
			get
			{
				List<Pawn> allPawnsUnspawned = this.AllPawnsUnspawned;
				if (allPawnsUnspawned.Count == 0)
				{
					return this.pawnsSpawned;
				}
				this.allPawnsResult.Clear();
				this.allPawnsResult.AddRange(this.pawnsSpawned);
				this.allPawnsResult.AddRange(allPawnsUnspawned);
				return this.allPawnsResult;
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001C9B RID: 7323 RVA: 0x0008FE50 File Offset: 0x0008E050
		public List<Pawn> AllPawnsUnspawned
		{
			get
			{
				this.allPawnsUnspawnedResult.Clear();
				ThingOwnerUtility.GetAllThingsRecursively<Pawn>(this.map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), this.allPawnsUnspawnedResult, true, null, false);
				for (int i = this.allPawnsUnspawnedResult.Count - 1; i >= 0; i--)
				{
					if (this.allPawnsUnspawnedResult[i].Dead)
					{
						this.allPawnsUnspawnedResult.RemoveAt(i);
					}
				}
				return this.allPawnsUnspawnedResult;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001C9C RID: 7324 RVA: 0x0008FEC0 File Offset: 0x0008E0C0
		public List<Pawn> FreeColonists
		{
			get
			{
				return this.FreeHumanlikesOfFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001C9D RID: 7325 RVA: 0x0008FED0 File Offset: 0x0008E0D0
		public List<Pawn> PrisonersOfColony
		{
			get
			{
				this.prisonersOfColonyResult.Clear();
				List<Pawn> allPawns = this.AllPawns;
				for (int i = 0; i < allPawns.Count; i++)
				{
					if (allPawns[i].IsPrisonerOfColony)
					{
						this.prisonersOfColonyResult.Add(allPawns[i]);
					}
				}
				return this.prisonersOfColonyResult;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001C9E RID: 7326 RVA: 0x0008FF28 File Offset: 0x0008E128
		public List<Pawn> AllHumanlike
		{
			get
			{
				this.humanlikePawnsResult.Clear();
				List<Pawn> allPawns = this.AllPawns;
				for (int i = 0; i < allPawns.Count; i++)
				{
					if (allPawns[i].RaceProps.Humanlike)
					{
						this.humanlikePawnsResult.Add(allPawns[i]);
					}
				}
				return this.humanlikePawnsResult;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001C9F RID: 7327 RVA: 0x0008FF84 File Offset: 0x0008E184
		public List<Pawn> AllHumanlikeSpawned
		{
			get
			{
				this.humanlikeSpawnedPawnsResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].RaceProps.Humanlike)
					{
						this.humanlikeSpawnedPawnsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.humanlikeSpawnedPawnsResult;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001CA0 RID: 7328 RVA: 0x0008FFE0 File Offset: 0x0008E1E0
		public List<Pawn> FreeColonistsAndPrisoners
		{
			get
			{
				List<Pawn> freeColonists = this.FreeColonists;
				List<Pawn> prisonersOfColony = this.PrisonersOfColony;
				if (prisonersOfColony.Count == 0)
				{
					return freeColonists;
				}
				this.freeColonistsAndPrisonersResult.Clear();
				this.freeColonistsAndPrisonersResult.AddRange(freeColonists);
				this.freeColonistsAndPrisonersResult.AddRange(prisonersOfColony);
				return this.freeColonistsAndPrisonersResult;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001CA1 RID: 7329 RVA: 0x00090030 File Offset: 0x0008E230
		public int ColonistCount
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					Log.Error("ColonistCount while not playing. This should get the starting player pawn count.");
					return 3;
				}
				int num = 0;
				List<Pawn> allPawns = this.AllPawns;
				for (int i = 0; i < allPawns.Count; i++)
				{
					if (allPawns[i].IsColonist)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001CA2 RID: 7330 RVA: 0x0009007E File Offset: 0x0008E27E
		public int AllPawnsCount
		{
			get
			{
				return this.AllPawns.Count;
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001CA3 RID: 7331 RVA: 0x0009008B File Offset: 0x0008E28B
		public int AllPawnsUnspawnedCount
		{
			get
			{
				return this.AllPawnsUnspawned.Count;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001CA4 RID: 7332 RVA: 0x00090098 File Offset: 0x0008E298
		public int FreeColonistsCount
		{
			get
			{
				return this.FreeColonists.Count;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001CA5 RID: 7333 RVA: 0x000900A5 File Offset: 0x0008E2A5
		public int PrisonersOfColonyCount
		{
			get
			{
				return this.PrisonersOfColony.Count;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001CA6 RID: 7334 RVA: 0x000900B2 File Offset: 0x0008E2B2
		public int FreeColonistsAndPrisonersCount
		{
			get
			{
				return this.FreeColonistsCount + this.PrisonersOfColonyCount;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001CA7 RID: 7335 RVA: 0x000900C4 File Offset: 0x0008E2C4
		public bool AnyPawnBlockingMapRemoval
		{
			get
			{
				Faction ofPlayer = Faction.OfPlayer;
				foreach (Pawn pawn in this.pawnsSpawned)
				{
					if (MapPawns.IsValidColonyPawn(pawn))
					{
						return true;
					}
					Pawn_RelationsTracker relations = pawn.relations;
					if (((relations != null) ? relations.relativeInvolvedInRescueQuest : null) != null)
					{
						return true;
					}
					if (pawn.Faction == ofPlayer || pawn.HostFaction == ofPlayer)
					{
						Job curJob = pawn.CurJob;
						if (curJob != null && curJob.exitMapOnArrival)
						{
							return true;
						}
						if (pawn.health.hediffSet.InLabor(true))
						{
							return true;
						}
					}
					if (CaravanExitMapUtility.FindCaravanToJoinFor(pawn) != null && !pawn.Downed)
					{
						return true;
					}
					if (ModsConfig.BiotechActive && pawn.IsColonyMechPlayerControlled && !pawn.Downed && pawn.GetOverseer() != null)
					{
						return true;
					}
				}
				foreach (Pawn pawn2 in this.AllPawnsUnspawned)
				{
					if (pawn2.SpawnedOrAnyParentSpawned && MapPawns.IsValidColonyPawn(pawn2))
					{
						return true;
					}
				}
				foreach (Thing thing in this.map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse))
				{
					Corpse corpse = thing as Corpse;
					if (corpse != null && MapPawns.IsValidColonyPawn(corpse.InnerPawn))
					{
						return true;
					}
				}
				List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder);
				for (int i = 0; i < list.Count; i++)
				{
					IThingHolder thingHolder = MapPawns.PlayerEjectablePodHolder(list[i], false);
					if (thingHolder != null)
					{
						this.tmpThings.Clear();
						ThingOwnerUtility.GetAllThingsRecursively(thingHolder, this.tmpThings, true, null);
						for (int j = 0; j < this.tmpThings.Count; j++)
						{
							Pawn pawn3 = this.tmpThings[j] as Pawn;
							if (pawn3 != null && !pawn3.Dead && !pawn3.Downed && (pawn3.IsColonist || pawn3.IsColonyMech))
							{
								this.tmpThings.Clear();
								return true;
							}
						}
					}
				}
				using (IEnumerator<Map> enumerator3 = this.map.ChildPocketMaps.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current.mapPawns.AnyPawnBlockingMapRemoval)
						{
							return true;
						}
					}
				}
				this.tmpThings.Clear();
				return false;
			}
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x000903AC File Offset: 0x0008E5AC
		private static bool IsValidColonyPawn(Pawn pawn)
		{
			return ((!pawn.Dead || pawn.HasDeathRefusalOrResurrecting) && pawn.IsColonist) || ((!pawn.Dead || pawn.HasDeathRefusalOrResurrecting) && pawn.IsColonySubhuman && pawn.mutant.Def.canTravelInCaravan);
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001CA9 RID: 7337 RVA: 0x00090400 File Offset: 0x0008E600
		public IReadOnlyList<Pawn> AllPawnsSpawned
		{
			get
			{
				return this.pawnsSpawned;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001CAA RID: 7338 RVA: 0x00090408 File Offset: 0x0008E608
		public List<Pawn> FreeColonistsSpawned
		{
			get
			{
				return this.FreeHumanlikesSpawnedOfFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001CAB RID: 7339 RVA: 0x00090418 File Offset: 0x0008E618
		public List<Pawn> FreeAdultColonistsSpawned
		{
			get
			{
				this.freeAdultColonistsSpawnedResult.Clear();
				foreach (Pawn pawn in this.FreeColonistsSpawned)
				{
					if (pawn.DevelopmentalStage.Adult())
					{
						this.freeAdultColonistsSpawnedResult.Add(pawn);
					}
				}
				return this.freeAdultColonistsSpawnedResult;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001CAC RID: 7340 RVA: 0x00090490 File Offset: 0x0008E690
		public List<Pawn> PrisonersOfColonySpawned
		{
			get
			{
				return this.prisonersOfColonySpawned;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001CAD RID: 7341 RVA: 0x00090498 File Offset: 0x0008E698
		public List<Pawn> SlavesOfColonySpawned
		{
			get
			{
				return this.slavesOfColonySpawned;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001CAE RID: 7342 RVA: 0x000904A0 File Offset: 0x0008E6A0
		public List<Pawn> FreeColonistsAndPrisonersSpawned
		{
			get
			{
				List<Pawn> freeColonistsSpawned = this.FreeColonistsSpawned;
				List<Pawn> list = this.PrisonersOfColonySpawned;
				if (list.Count == 0)
				{
					return freeColonistsSpawned;
				}
				this.freeColonistsAndPrisonersSpawnedResult.Clear();
				this.freeColonistsAndPrisonersSpawnedResult.AddRange(freeColonistsSpawned);
				this.freeColonistsAndPrisonersSpawnedResult.AddRange(list);
				return this.freeColonistsAndPrisonersSpawnedResult;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001CAF RID: 7343 RVA: 0x000904F0 File Offset: 0x0008E6F0
		public List<Pawn> SpawnedPawnsWithAnyHediff
		{
			get
			{
				this.spawnedPawnsWithAnyHediffResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].health.hediffSet.hediffs.Count != 0)
					{
						this.spawnedPawnsWithAnyHediffResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWithAnyHediffResult;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001CB0 RID: 7344 RVA: 0x00090558 File Offset: 0x0008E758
		public List<Pawn> SpawnedHumanlikesWithAnyHediff
		{
			get
			{
				this.spawnedHumanlikesWithAnyHediffResult.Clear();
				List<Pawn> allHumanlikeSpawned = this.AllHumanlikeSpawned;
				for (int i = 0; i < allHumanlikeSpawned.Count; i++)
				{
					if (allHumanlikeSpawned[i].health.hediffSet.hediffs.Count != 0)
					{
						this.spawnedHumanlikesWithAnyHediffResult.Add(allHumanlikeSpawned[i]);
					}
				}
				return this.spawnedHumanlikesWithAnyHediffResult;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06001CB1 RID: 7345 RVA: 0x000905C0 File Offset: 0x0008E7C0
		public List<Pawn> SpawnedAnimalsWithAnyHediff
		{
			get
			{
				this.spawnedAnimalsWithAnyHediffResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].IsAnimal && allPawnsSpawned[i].health.hediffSet.hediffs.Count != 0)
					{
						this.spawnedAnimalsWithAnyHediffResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedAnimalsWithAnyHediffResult;
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06001CB2 RID: 7346 RVA: 0x00090634 File Offset: 0x0008E834
		public List<Pawn> SpawnedHungryPawns
		{
			get
			{
				this.spawnedHungryPawnsResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (FeedPatientUtility.IsHungry(allPawnsSpawned[i]))
					{
						this.spawnedHungryPawnsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedHungryPawnsResult;
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06001CB3 RID: 7347 RVA: 0x0009068C File Offset: 0x0008E88C
		public List<Pawn> SpawnedPawnsWithMiscNeeds
		{
			get
			{
				this.spawnedPawnsWithMiscNeedsResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (!allPawnsSpawned[i].needs.MiscNeeds.NullOrEmpty<Need>())
					{
						this.spawnedPawnsWithMiscNeedsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWithMiscNeedsResult;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06001CB4 RID: 7348 RVA: 0x000906EC File Offset: 0x0008E8EC
		public List<Pawn> ColonyAnimals
		{
			get
			{
				this.colonyAnimalsResult.Clear();
				List<Pawn> list = this.PawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsAnimal)
					{
						this.colonyAnimalsResult.Add(list[i]);
					}
				}
				return this.colonyAnimalsResult;
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001CB5 RID: 7349 RVA: 0x00090748 File Offset: 0x0008E948
		public List<Pawn> SpawnedColonyAnimals
		{
			get
			{
				this.spawnedColonyAnimalsResult.Clear();
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsAnimal)
					{
						this.spawnedColonyAnimalsResult.Add(list[i]);
					}
				}
				return this.spawnedColonyAnimalsResult;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06001CB6 RID: 7350 RVA: 0x000907A4 File Offset: 0x0008E9A4
		public List<Pawn> SpawnedColonyMechs
		{
			get
			{
				this.spawnedColonyMechsResult.Clear();
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonyMech)
					{
						this.spawnedColonyMechsResult.Add(list[i]);
					}
				}
				return this.spawnedColonyMechsResult;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001CB7 RID: 7351 RVA: 0x00090800 File Offset: 0x0008EA00
		public List<Pawn> ColonySubhumansControllable
		{
			get
			{
				this.colonySubhumansResult.Clear();
				List<Pawn> list = this.PawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonySubhuman && list[i].mutant.Def.canBeDrafted)
					{
						this.colonySubhumansResult.Add(list[i]);
					}
				}
				return this.colonySubhumansResult;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001CB8 RID: 7352 RVA: 0x00090874 File Offset: 0x0008EA74
		public List<Pawn> SpawnedColonySubhumansPlayerControlled
		{
			get
			{
				this.spawnedColonySubhumansResult.Clear();
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonySubhumanPlayerControlled)
					{
						this.spawnedColonySubhumansResult.Add(list[i]);
					}
				}
				return this.spawnedColonySubhumansResult;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001CB9 RID: 7353 RVA: 0x000908D0 File Offset: 0x0008EAD0
		public List<Pawn> SpawnedDownedPawns
		{
			get
			{
				this.spawnedDownedPawnsResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].Downed)
					{
						this.spawnedDownedPawnsResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedDownedPawnsResult;
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001CBA RID: 7354 RVA: 0x00090928 File Offset: 0x0008EB28
		public List<Pawn> SpawnedPawnsWhoShouldHaveSurgeryDoneNow
		{
			get
			{
				this.spawnedPawnsWhoShouldHaveSurgeryDoneNowResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (HealthAIUtility.ShouldHaveSurgeryDoneNow(allPawnsSpawned[i]))
					{
						this.spawnedPawnsWhoShouldHaveSurgeryDoneNowResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWhoShouldHaveSurgeryDoneNowResult;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001CBB RID: 7355 RVA: 0x00090980 File Offset: 0x0008EB80
		public List<Pawn> SpawnedPawnsWhoShouldHaveInventoryUnloaded
		{
			get
			{
				this.spawnedPawnsWhoShouldHaveInventoryUnloadedResult.Clear();
				IReadOnlyList<Pawn> allPawnsSpawned = this.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].inventory.UnloadEverything)
					{
						this.spawnedPawnsWhoShouldHaveInventoryUnloadedResult.Add(allPawnsSpawned[i]);
					}
				}
				return this.spawnedPawnsWhoShouldHaveInventoryUnloadedResult;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001CBC RID: 7356 RVA: 0x000909DB File Offset: 0x0008EBDB
		public int AllPawnsSpawnedCount
		{
			get
			{
				return this.pawnsSpawned.Count;
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001CBD RID: 7357 RVA: 0x000909E8 File Offset: 0x0008EBE8
		public int FreeColonistsSpawnedCount
		{
			get
			{
				return this.FreeColonistsSpawned.Count;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001CBE RID: 7358 RVA: 0x000909F5 File Offset: 0x0008EBF5
		public int FreeAdultColonistsSpawnedCount
		{
			get
			{
				return this.FreeAdultColonistsSpawned.Count;
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x06001CBF RID: 7359 RVA: 0x00090A02 File Offset: 0x0008EC02
		public int PrisonersOfColonySpawnedCount
		{
			get
			{
				return this.PrisonersOfColonySpawned.Count;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x06001CC0 RID: 7360 RVA: 0x00090A0F File Offset: 0x0008EC0F
		public int FreeColonistsAndPrisonersSpawnedCount
		{
			get
			{
				return this.FreeColonistsAndPrisonersSpawned.Count;
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06001CC1 RID: 7361 RVA: 0x00090A1C File Offset: 0x0008EC1C
		public int ColonistsSpawnedCount
		{
			get
			{
				int num = 0;
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonist)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001CC2 RID: 7362 RVA: 0x00090A5C File Offset: 0x0008EC5C
		public int FreeColonistsSpawnedOrInPlayerEjectablePodsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.pawnsSpawned.Count; i++)
				{
					if (this.pawnsSpawned[i].IsFreeColonist)
					{
						num++;
					}
				}
				List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder);
				for (int j = 0; j < list.Count; j++)
				{
					IThingHolder thingHolder = MapPawns.PlayerEjectablePodHolder(list[j], true);
					if (thingHolder != null)
					{
						this.tmpThings.Clear();
						ThingOwnerUtility.GetAllThingsRecursively(thingHolder, this.tmpThings, true, null);
						for (int k = 0; k < this.tmpThings.Count; k++)
						{
							Pawn pawn = this.tmpThings[k] as Pawn;
							if (pawn != null && !pawn.Dead && pawn.IsFreeColonist)
							{
								num++;
							}
						}
					}
				}
				this.tmpThings.Clear();
				return num;
			}
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x00090B40 File Offset: 0x0008ED40
		private static IThingHolder PlayerEjectablePodHolder(Thing thing, bool includeCryptosleepCaskets = true)
		{
			Building_CryptosleepCasket building_CryptosleepCasket = thing as Building_CryptosleepCasket;
			CompTransporter compTransporter = thing.TryGetComp<CompTransporter>();
			CompBiosculpterPod compBiosculpterPod = thing.TryGetComp<CompBiosculpterPod>();
			if ((includeCryptosleepCaskets && building_CryptosleepCasket != null && building_CryptosleepCasket.def.building.isPlayerEjectable) || thing is IActiveTransporter || thing is PawnFlyer || thing is Building_Enterable || compTransporter != null || compBiosculpterPod != null)
			{
				IThingHolder thingHolder = compTransporter;
				IThingHolder result;
				if ((result = thingHolder) == null)
				{
					thingHolder = compBiosculpterPod;
					result = (thingHolder ?? (thing as IThingHolder));
				}
				return result;
			}
			return null;
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001CC4 RID: 7364 RVA: 0x00090BAE File Offset: 0x0008EDAE
		public int SlavesAndPrisonersOfColonySpawnedCount
		{
			get
			{
				return this.SlavesAndPrisonersOfColonySpawned.Count;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001CC5 RID: 7365 RVA: 0x00090BBC File Offset: 0x0008EDBC
		public bool AnyColonistSpawned
		{
			get
			{
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonist)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x06001CC6 RID: 7366 RVA: 0x00090BF8 File Offset: 0x0008EDF8
		public bool AnyFreeColonistSpawned
		{
			get
			{
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsFreeColonist)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x06001CC7 RID: 7367 RVA: 0x00090C33 File Offset: 0x0008EE33
		public List<Pawn> SlavesAndPrisonersOfColonySpawned
		{
			get
			{
				this.slavesAndPrisonersOfColonySpawnedResult.Clear();
				this.slavesAndPrisonersOfColonySpawnedResult.AddRange(this.prisonersOfColonySpawned);
				this.slavesAndPrisonersOfColonySpawnedResult.AddRange(this.slavesOfColonySpawned);
				return this.slavesAndPrisonersOfColonySpawnedResult;
			}
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x00090C68 File Offset: 0x0008EE68
		public MapPawns(Map map)
		{
			this.map = map;
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x00090DF0 File Offset: 0x0008EFF0
		private void EnsureFactionsListsInit()
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				this.pawnsInFactionSpawned.GetPawnList(allFactionsListForReading[i]);
			}
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x00090E2C File Offset: 0x0008F02C
		public List<Faction> FactionsOnMap()
		{
			this.tmpFactionsOnMap.Clear();
			foreach (Faction faction in this.pawnsInFactionResult.KnownFactions())
			{
				if (this.pawnsInFactionResult.GetPawnList(faction).Count > 0)
				{
					this.tmpFactionsOnMap.Add(faction);
				}
			}
			return this.tmpFactionsOnMap;
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x00090EA8 File Offset: 0x0008F0A8
		public List<Pawn> PawnsInFaction(Faction faction)
		{
			List<Pawn> pawnList = this.pawnsInFactionResult.GetPawnList(faction);
			pawnList.Clear();
			List<Pawn> allPawns = this.AllPawns;
			for (int i = 0; i < allPawns.Count; i++)
			{
				if (allPawns[i].Faction == faction)
				{
					pawnList.Add(allPawns[i]);
				}
			}
			return pawnList;
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x00090EFD File Offset: 0x0008F0FD
		public List<Pawn> SpawnedPawnsInFaction(Faction faction)
		{
			this.EnsureFactionsListsInit();
			return this.pawnsInFactionSpawned.GetPawnList(faction);
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x00090F14 File Offset: 0x0008F114
		public List<Pawn> FreeHumanlikesOfFaction(Faction faction)
		{
			List<Pawn> pawnList = this.freeHumanlikesOfFactionResult.GetPawnList(faction);
			pawnList.Clear();
			List<Pawn> allPawns = this.AllPawns;
			for (int i = 0; i < allPawns.Count; i++)
			{
				if ((!ModsConfig.AnomalyActive || !allPawns[i].IsSubhuman) && allPawns[i].Faction == faction && (allPawns[i].HostFaction == null || allPawns[i].IsSlave) && allPawns[i].RaceProps.Humanlike)
				{
					pawnList.Add(allPawns[i]);
				}
			}
			return pawnList;
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x00090FB0 File Offset: 0x0008F1B0
		public List<Pawn> FreeHumanlikesSpawnedOfFaction(Faction faction)
		{
			List<Pawn> pawnList = this.freeHumanlikesSpawnedOfFactionResult.GetPawnList(faction);
			pawnList.Clear();
			List<Pawn> list = this.SpawnedPawnsInFaction(faction);
			for (int i = 0; i < list.Count; i++)
			{
				if ((!ModsConfig.AnomalyActive || !list[i].IsSubhuman) && list[i].HostFaction == null && list[i].RaceProps.Humanlike)
				{
					pawnList.Add(list[i]);
				}
			}
			return pawnList;
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x00091030 File Offset: 0x0008F230
		public List<Pawn> SpawnedBabiesInFaction(Faction faction)
		{
			List<Pawn> pawnList = this.spawnedBabiesInFactionResult.GetPawnList(faction);
			pawnList.Clear();
			List<Pawn> list = this.SpawnedPawnsInFaction(faction);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].DevelopmentalStage.Baby())
				{
					pawnList.Add(list[i]);
				}
			}
			return pawnList;
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x0009108C File Offset: 0x0008F28C
		public void RegisterPawn(Pawn p)
		{
			if (p.Dead)
			{
				string[] array = new string[5];
				array[0] = "Tried to register dead pawn ";
				array[1] = ((p != null) ? p.ToString() : null);
				array[2] = " in ";
				int num = 3;
				Type type = base.GetType();
				array[num] = ((type != null) ? type.ToString() : null);
				array[4] = ".";
				Log.Warning(string.Concat(array));
				return;
			}
			if (!p.Spawned)
			{
				string[] array2 = new string[5];
				array2[0] = "Tried to register despawned pawn ";
				array2[1] = ((p != null) ? p.ToString() : null);
				array2[2] = " in ";
				int num2 = 3;
				Type type2 = base.GetType();
				array2[num2] = ((type2 != null) ? type2.ToString() : null);
				array2[4] = ".";
				Log.Warning(string.Concat(array2));
				return;
			}
			if (p.Map != this.map)
			{
				Log.Warning("Tried to register pawn " + ((p != null) ? p.ToString() : null) + " but his Map is not this one.");
				return;
			}
			if (!p.mindState.Active)
			{
				return;
			}
			this.EnsureFactionsListsInit();
			if (!this.pawnsSpawned.Contains(p))
			{
				this.pawnsSpawned.Add(p);
			}
			if (p.Faction != null)
			{
				List<Pawn> pawnList = this.pawnsInFactionSpawned.GetPawnList(p.Faction);
				if (!pawnList.Contains(p))
				{
					pawnList.Add(p);
					if (p.Faction == Faction.OfPlayer)
					{
						pawnList.InsertionSort(delegate(Pawn a, Pawn b)
						{
							Pawn_PlayerSettings playerSettings = a.playerSettings;
							int num3 = (playerSettings != null) ? playerSettings.joinTick : 0;
							Pawn_PlayerSettings playerSettings2 = b.playerSettings;
							int value = (playerSettings2 != null) ? playerSettings2.joinTick : 0;
							return num3.CompareTo(value);
						});
					}
				}
			}
			if (p.IsPrisonerOfColony && !this.prisonersOfColonySpawned.Contains(p))
			{
				this.prisonersOfColonySpawned.Add(p);
			}
			if (p.IsSlaveOfColony && !this.slavesOfColonySpawned.Contains(p))
			{
				this.slavesOfColonySpawned.Add(p);
			}
			this.DoListChangedNotifications();
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x0009124C File Offset: 0x0008F44C
		public void DeRegisterPawn(Pawn p)
		{
			this.EnsureFactionsListsInit();
			this.pawnsSpawned.Remove(p);
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				Faction faction = allFactionsListForReading[i];
				this.pawnsInFactionSpawned.GetPawnList(faction).Remove(p);
			}
			this.prisonersOfColonySpawned.Remove(p);
			this.slavesOfColonySpawned.Remove(p);
			this.DoListChangedNotifications();
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x000912C3 File Offset: 0x0008F4C3
		public void UpdateRegistryForPawn(Pawn p)
		{
			this.DeRegisterPawn(p);
			if (p.Spawned && p.Map == this.map)
			{
				this.RegisterPawn(p);
			}
			this.DoListChangedNotifications();
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001CD3 RID: 7379 RVA: 0x000912EF File Offset: 0x0008F4EF
		public List<Pawn> SpawnedShamblers
		{
			get
			{
				return this.shamblersSpawned;
			}
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x000912F7 File Offset: 0x0008F4F7
		public void RegisterShambler(Pawn p)
		{
			if (!ModLister.CheckAnomaly("Shambler registration"))
			{
				return;
			}
			if (!this.shamblersSpawned.Contains(p))
			{
				this.shamblersSpawned.Add(p);
			}
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x00091320 File Offset: 0x0008F520
		public void DeregisterShambler(Pawn p)
		{
			if (!ModLister.CheckAnomaly("Shambler deregistration"))
			{
				return;
			}
			this.shamblersSpawned.Remove(p);
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x0009133C File Offset: 0x0008F53C
		private void DoListChangedNotifications()
		{
			MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
			if (Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x00091354 File Offset: 0x0008F554
		public void LogListedPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MapPawns:");
			stringBuilder.AppendLine("pawnsSpawned");
			foreach (Pawn pawn in this.pawnsSpawned)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string str = "    ";
				Pawn pawn2 = pawn;
				stringBuilder2.AppendLine(str + ((pawn2 != null) ? pawn2.ToString() : null));
			}
			stringBuilder.AppendLine("AllPawnsUnspawned");
			foreach (Pawn pawn3 in this.AllPawnsUnspawned)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				string str2 = "    ";
				Pawn pawn4 = pawn3;
				stringBuilder3.AppendLine(str2 + ((pawn4 != null) ? pawn4.ToString() : null));
			}
			foreach (Faction faction in this.pawnsInFactionSpawned.KnownFactions())
			{
				stringBuilder.AppendLine("pawnsInFactionSpawned[" + faction.ToStringSafe<Faction>() + "]");
				foreach (Pawn pawn5 in this.pawnsInFactionSpawned.GetPawnList(faction))
				{
					StringBuilder stringBuilder4 = stringBuilder;
					string str3 = "    ";
					Pawn pawn6 = pawn5;
					stringBuilder4.AppendLine(str3 + ((pawn6 != null) ? pawn6.ToString() : null));
				}
			}
			stringBuilder.AppendLine("prisonersOfColonySpawned");
			foreach (Pawn pawn7 in this.prisonersOfColonySpawned)
			{
				StringBuilder stringBuilder5 = stringBuilder;
				string str4 = "    ";
				Pawn pawn8 = pawn7;
				stringBuilder5.AppendLine(str4 + ((pawn8 != null) ? pawn8.ToString() : null));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x040018C6 RID: 6342
		private Map map;

		// Token: 0x040018C7 RID: 6343
		private List<Pawn> pawnsSpawned = new List<Pawn>();

		// Token: 0x040018C8 RID: 6344
		private MapPawns.FactionDictionary pawnsInFactionSpawned = new MapPawns.FactionDictionary();

		// Token: 0x040018C9 RID: 6345
		private List<Pawn> prisonersOfColonySpawned = new List<Pawn>();

		// Token: 0x040018CA RID: 6346
		private List<Pawn> slavesOfColonySpawned = new List<Pawn>();

		// Token: 0x040018CB RID: 6347
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x040018CC RID: 6348
		private List<Pawn> allPawnsResult = new List<Pawn>();

		// Token: 0x040018CD RID: 6349
		private List<Pawn> allPawnsUnspawnedResult = new List<Pawn>();

		// Token: 0x040018CE RID: 6350
		private List<Pawn> prisonersOfColonyResult = new List<Pawn>();

		// Token: 0x040018CF RID: 6351
		private List<Pawn> humanlikePawnsResult = new List<Pawn>();

		// Token: 0x040018D0 RID: 6352
		private List<Pawn> humanlikeSpawnedPawnsResult = new List<Pawn>();

		// Token: 0x040018D1 RID: 6353
		private List<Pawn> freeColonistsAndPrisonersResult = new List<Pawn>();

		// Token: 0x040018D2 RID: 6354
		private readonly List<Pawn> freeAdultColonistsSpawnedResult = new List<Pawn>();

		// Token: 0x040018D3 RID: 6355
		private List<Pawn> freeColonistsAndPrisonersSpawnedResult = new List<Pawn>();

		// Token: 0x040018D4 RID: 6356
		private List<Pawn> spawnedPawnsWithAnyHediffResult = new List<Pawn>();

		// Token: 0x040018D5 RID: 6357
		private List<Pawn> spawnedHumanlikesWithAnyHediffResult = new List<Pawn>();

		// Token: 0x040018D6 RID: 6358
		private List<Pawn> spawnedAnimalsWithAnyHediffResult = new List<Pawn>();

		// Token: 0x040018D7 RID: 6359
		private List<Pawn> spawnedHungryPawnsResult = new List<Pawn>();

		// Token: 0x040018D8 RID: 6360
		private List<Pawn> spawnedPawnsWithMiscNeedsResult = new List<Pawn>();

		// Token: 0x040018D9 RID: 6361
		private List<Pawn> colonyAnimalsResult = new List<Pawn>();

		// Token: 0x040018DA RID: 6362
		private List<Pawn> spawnedColonyAnimalsResult = new List<Pawn>();

		// Token: 0x040018DB RID: 6363
		private List<Pawn> spawnedColonyMechsResult = new List<Pawn>();

		// Token: 0x040018DC RID: 6364
		private List<Pawn> colonySubhumansResult = new List<Pawn>();

		// Token: 0x040018DD RID: 6365
		private List<Pawn> spawnedColonySubhumansResult = new List<Pawn>();

		// Token: 0x040018DE RID: 6366
		private List<Pawn> spawnedDownedPawnsResult = new List<Pawn>();

		// Token: 0x040018DF RID: 6367
		private List<Pawn> spawnedPawnsWhoShouldHaveSurgeryDoneNowResult = new List<Pawn>();

		// Token: 0x040018E0 RID: 6368
		private List<Pawn> spawnedPawnsWhoShouldHaveInventoryUnloadedResult = new List<Pawn>();

		// Token: 0x040018E1 RID: 6369
		private List<Pawn> slavesAndPrisonersOfColonySpawnedResult = new List<Pawn>();

		// Token: 0x040018E2 RID: 6370
		private List<Faction> tmpFactionsOnMap = new List<Faction>(16);

		// Token: 0x040018E3 RID: 6371
		private MapPawns.FactionDictionary pawnsInFactionResult = new MapPawns.FactionDictionary();

		// Token: 0x040018E4 RID: 6372
		private MapPawns.FactionDictionary freeHumanlikesOfFactionResult = new MapPawns.FactionDictionary();

		// Token: 0x040018E5 RID: 6373
		private MapPawns.FactionDictionary freeHumanlikesSpawnedOfFactionResult = new MapPawns.FactionDictionary();

		// Token: 0x040018E6 RID: 6374
		private MapPawns.FactionDictionary spawnedBabiesInFactionResult = new MapPawns.FactionDictionary();

		// Token: 0x040018E7 RID: 6375
		private List<Pawn> shamblersSpawned = new List<Pawn>();

		// Token: 0x02000453 RID: 1107
		private class FactionDictionary
		{
			// Token: 0x06001CD8 RID: 7384 RVA: 0x00091578 File Offset: 0x0008F778
			public List<Pawn> GetPawnList(Faction faction)
			{
				List<Pawn> list;
				List<Pawn> list2;
				if (faction == null)
				{
					list = this.nullFactionPawns;
				}
				else if (this.pawnList.TryGetValue(faction, out list2))
				{
					list = list2;
				}
				else
				{
					list = (this.pawnList[faction] = new List<Pawn>(32));
				}
				if (list.Count > 0)
				{
					for (int i = list.Count - 1; i >= 0; i--)
					{
						if (list[i] == null)
						{
							list.RemoveAt(i);
						}
					}
				}
				return list;
			}

			// Token: 0x06001CD9 RID: 7385 RVA: 0x000915E7 File Offset: 0x0008F7E7
			public IEnumerable<Faction> KnownFactions()
			{
				return this.pawnList.Keys.Concat(null);
			}

			// Token: 0x040018E8 RID: 6376
			private Dictionary<Faction, List<Pawn>> pawnList = new Dictionary<Faction, List<Pawn>>(16);

			// Token: 0x040018E9 RID: 6377
			private List<Pawn> nullFactionPawns = new List<Pawn>(32);
		}
	}
}
