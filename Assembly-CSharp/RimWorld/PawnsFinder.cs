using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020023A6 RID: 9126
	public static class PawnsFinder
	{
		// Token: 0x17001DEC RID: 7660
		// (get) Token: 0x0600C594 RID: 50580 RVA: 0x00397C4C File Offset: 0x00395E4C
		public static List<Pawn> AllMapsWorldAndTemporary_AliveOrDead
		{
			get
			{
				PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.Clear();
				PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.AddRange(PawnsFinder.AllMapsWorldAndTemporary_Alive);
				if (Find.World != null)
				{
					PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.AddRange(Find.WorldPawns.AllPawnsDead);
				}
				PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.AddRange(PawnsFinder.Temporary_Dead);
				if (Find.CurrentGravship != null)
				{
					PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.AddRange(Find.CurrentGravship.Pawns);
				}
				return PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result;
			}
		}

		// Token: 0x17001DED RID: 7661
		// (get) Token: 0x0600C595 RID: 50581 RVA: 0x00397CBC File Offset: 0x00395EBC
		public static List<Pawn> AllMapsWorldAndTemporary_Alive
		{
			get
			{
				PawnsFinder.allMapsWorldAndTemporary_Alive_Result.Clear();
				PawnsFinder.allMapsWorldAndTemporary_Alive_Result.AddRange(PawnsFinder.AllMaps);
				if (Find.World != null)
				{
					PawnsFinder.allMapsWorldAndTemporary_Alive_Result.AddRange(Find.WorldPawns.AllPawnsAlive);
				}
				PawnsFinder.allMapsWorldAndTemporary_Alive_Result.AddRange(PawnsFinder.Temporary_Alive);
				if (Find.CurrentGravship != null)
				{
					PawnsFinder.allMapsWorldAndTemporary_Alive_Result.AddRange(Find.CurrentGravship.Pawns);
				}
				return PawnsFinder.allMapsWorldAndTemporary_Alive_Result;
			}
		}

		// Token: 0x17001DEE RID: 7662
		// (get) Token: 0x0600C596 RID: 50582 RVA: 0x00397D2C File Offset: 0x00395F2C
		public static List<Pawn> AllMapsAndWorld_Alive
		{
			get
			{
				PawnsFinder.allMapsAndWorld_Alive_Result.Clear();
				PawnsFinder.allMapsAndWorld_Alive_Result.AddRange(PawnsFinder.AllMaps);
				if (Find.World != null)
				{
					PawnsFinder.allMapsAndWorld_Alive_Result.AddRange(Find.WorldPawns.AllPawnsAlive);
				}
				if (Find.CurrentGravship != null)
				{
					PawnsFinder.allMapsAndWorld_Alive_Result.AddRange(Find.CurrentGravship.Pawns);
				}
				return PawnsFinder.allMapsAndWorld_Alive_Result;
			}
		}

		// Token: 0x17001DEF RID: 7663
		// (get) Token: 0x0600C597 RID: 50583 RVA: 0x00397D90 File Offset: 0x00395F90
		public static List<Pawn> AllMaps
		{
			get
			{
				PawnsFinder.allMaps_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.AllPawns;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_Result.AddRange(maps[i].mapPawns.AllPawns);
					}
				}
				return PawnsFinder.allMaps_Result;
			}
		}

		// Token: 0x17001DF0 RID: 7664
		// (get) Token: 0x0600C598 RID: 50584 RVA: 0x00397E00 File Offset: 0x00396000
		public static IReadOnlyList<Pawn> AllMaps_Spawned
		{
			get
			{
				PawnsFinder.allMaps_Spawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.AllPawnsSpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_Spawned_Result.AddRange(maps[i].mapPawns.AllPawnsSpawned);
					}
				}
				return PawnsFinder.allMaps_Spawned_Result;
			}
		}

		// Token: 0x17001DF1 RID: 7665
		// (get) Token: 0x0600C599 RID: 50585 RVA: 0x00397E70 File Offset: 0x00396070
		public static List<Pawn> All_AliveOrDead
		{
			get
			{
				List<Pawn> allMapsWorldAndTemporary_AliveOrDead = PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead;
				List<Pawn> allCaravansAndTravellingTransporters_AliveOrDead = PawnsFinder.AllCaravansAndTravellingTransporters_AliveOrDead;
				if (allCaravansAndTravellingTransporters_AliveOrDead.Count == 0)
				{
					return allMapsWorldAndTemporary_AliveOrDead;
				}
				PawnsFinder.all_AliveOrDead_Result.Clear();
				PawnsFinder.all_AliveOrDead_Result.AddRange(allMapsWorldAndTemporary_AliveOrDead);
				PawnsFinder.all_AliveOrDead_Result.AddRange(allCaravansAndTravellingTransporters_AliveOrDead);
				return PawnsFinder.all_AliveOrDead_Result;
			}
		}

		// Token: 0x17001DF2 RID: 7666
		// (get) Token: 0x0600C59A RID: 50586 RVA: 0x00397EB8 File Offset: 0x003960B8
		public static List<Pawn> Temporary
		{
			get
			{
				PawnsFinder.temporary_Result.Clear();
				List<List<Pawn>> pawnsBeingGeneratedNow = PawnGroupKindWorker.pawnsBeingGeneratedNow;
				for (int i = 0; i < pawnsBeingGeneratedNow.Count; i++)
				{
					PawnsFinder.temporary_Result.AddRange(pawnsBeingGeneratedNow[i]);
				}
				List<List<Thing>> thingsBeingGeneratedNow = ThingSetMaker.thingsBeingGeneratedNow;
				for (int j = 0; j < thingsBeingGeneratedNow.Count; j++)
				{
					List<Thing> list = thingsBeingGeneratedNow[j];
					for (int k = 0; k < list.Count; k++)
					{
						Pawn pawn = list[k] as Pawn;
						if (pawn != null)
						{
							PawnsFinder.temporary_Result.Add(pawn);
						}
					}
				}
				if (Current.ProgramState != ProgramState.Playing && Find.GameInitData != null)
				{
					List<Pawn> startingAndOptionalPawns = Find.GameInitData.startingAndOptionalPawns;
					for (int l = 0; l < startingAndOptionalPawns.Count; l++)
					{
						if (startingAndOptionalPawns[l] != null)
						{
							PawnsFinder.temporary_Result.Add(startingAndOptionalPawns[l]);
						}
					}
				}
				if (Find.World != null)
				{
					List<Site> sites = Find.WorldObjects.Sites;
					for (int m = 0; m < sites.Count; m++)
					{
						for (int n = 0; n < sites[m].parts.Count; n++)
						{
							if (sites[m].parts[n].things != null && sites[m].parts[n].things.contentsLookMode == LookMode.Deep)
							{
								ThingOwner things = sites[m].parts[n].things;
								for (int num = 0; num < things.Count; num++)
								{
									Pawn pawn2 = things[num] as Pawn;
									if (pawn2 != null)
									{
										PawnsFinder.temporary_Result.Add(pawn2);
									}
								}
							}
						}
					}
				}
				if (Find.World != null)
				{
					List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
					for (int num2 = 0; num2 < allWorldObjects.Count; num2++)
					{
						DownedRefugeeComp component = allWorldObjects[num2].GetComponent<DownedRefugeeComp>();
						if (component != null && component.pawn != null && component.pawn.Any)
						{
							PawnsFinder.temporary_Result.Add(component.pawn[0]);
						}
						PrisonerWillingToJoinComp component2 = allWorldObjects[num2].GetComponent<PrisonerWillingToJoinComp>();
						if (component2 != null && component2.pawn != null && component2.pawn.Any)
						{
							PawnsFinder.temporary_Result.Add(component2.pawn[0]);
						}
					}
				}
				return PawnsFinder.temporary_Result;
			}
		}

		// Token: 0x17001DF3 RID: 7667
		// (get) Token: 0x0600C59B RID: 50587 RVA: 0x00398144 File Offset: 0x00396344
		public static List<Pawn> Temporary_Alive
		{
			get
			{
				PawnsFinder.temporary_Alive_Result.Clear();
				List<Pawn> temporary = PawnsFinder.Temporary;
				for (int i = 0; i < temporary.Count; i++)
				{
					if (!temporary[i].Dead)
					{
						PawnsFinder.temporary_Alive_Result.Add(temporary[i]);
					}
				}
				return PawnsFinder.temporary_Alive_Result;
			}
		}

		// Token: 0x17001DF4 RID: 7668
		// (get) Token: 0x0600C59C RID: 50588 RVA: 0x00398198 File Offset: 0x00396398
		public static List<Pawn> Temporary_Dead
		{
			get
			{
				PawnsFinder.temporary_Dead_Result.Clear();
				List<Pawn> temporary = PawnsFinder.Temporary;
				for (int i = 0; i < temporary.Count; i++)
				{
					if (temporary[i].Dead)
					{
						PawnsFinder.temporary_Dead_Result.Add(temporary[i]);
					}
				}
				return PawnsFinder.temporary_Dead_Result;
			}
		}

		// Token: 0x17001DF5 RID: 7669
		// (get) Token: 0x0600C59D RID: 50589 RVA: 0x003981EC File Offset: 0x003963EC
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive
		{
			get
			{
				List<Pawn> allMaps = PawnsFinder.AllMaps;
				List<Pawn> allCaravansAndTravellingTransporters_Alive = PawnsFinder.AllCaravansAndTravellingTransporters_Alive;
				if (allCaravansAndTravellingTransporters_Alive.Count == 0)
				{
					return allMaps;
				}
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Result.Clear();
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Result.AddRange(allMaps);
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Result.AddRange(allCaravansAndTravellingTransporters_Alive);
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Result;
			}
		}

		// Token: 0x17001DF6 RID: 7670
		// (get) Token: 0x0600C59E RID: 50590 RVA: 0x00398234 File Offset: 0x00396434
		public static IReadOnlyList<Pawn> AllMapsCaravansAndTravellingTransporters_AliveSpawned
		{
			get
			{
				IReadOnlyList<Pawn> allMaps_Spawned = PawnsFinder.AllMaps_Spawned;
				List<Pawn> allCaravansAndTravellingTransporters_Alive = PawnsFinder.AllCaravansAndTravellingTransporters_Alive;
				if (allCaravansAndTravellingTransporters_Alive.Count == 0)
				{
					return allMaps_Spawned;
				}
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_Result.Clear();
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_Result.AddRange(allMaps_Spawned);
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_Result.AddRange(allCaravansAndTravellingTransporters_Alive);
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_Result;
			}
		}

		// Token: 0x17001DF7 RID: 7671
		// (get) Token: 0x0600C59F RID: 50591 RVA: 0x0039827C File Offset: 0x0039647C
		public static List<Pawn> AllCaravansAndTravellingTransporters_Alive
		{
			get
			{
				PawnsFinder.allCaravansAndTravellingTransporters_Alive_Result.Clear();
				List<Pawn> allCaravansAndTravellingTransporters_AliveOrDead = PawnsFinder.AllCaravansAndTravellingTransporters_AliveOrDead;
				for (int i = 0; i < allCaravansAndTravellingTransporters_AliveOrDead.Count; i++)
				{
					if (!allCaravansAndTravellingTransporters_AliveOrDead[i].Dead)
					{
						PawnsFinder.allCaravansAndTravellingTransporters_Alive_Result.Add(allCaravansAndTravellingTransporters_AliveOrDead[i]);
					}
				}
				return PawnsFinder.allCaravansAndTravellingTransporters_Alive_Result;
			}
		}

		// Token: 0x17001DF8 RID: 7672
		// (get) Token: 0x0600C5A0 RID: 50592 RVA: 0x003982D0 File Offset: 0x003964D0
		public static List<Pawn> AllCaravansAndTravellingTransporters_AliveOrDead
		{
			get
			{
				PawnsFinder.allCaravansAndTravellingTransporters_AliveOrDead_Result.Clear();
				if (Find.World != null)
				{
					List<Caravan> caravans = Find.WorldObjects.Caravans;
					for (int i = 0; i < caravans.Count; i++)
					{
						PawnsFinder.allCaravansAndTravellingTransporters_AliveOrDead_Result.AddRange(caravans[i].PawnsListForReading);
					}
					List<TravellingTransporters> travellingTransporters = Find.WorldObjects.TravellingTransporters;
					for (int j = 0; j < travellingTransporters.Count; j++)
					{
						PawnsFinder.allCaravansAndTravellingTransporters_AliveOrDead_Result.AddRange(travellingTransporters[j].Pawns);
					}
					if (Find.CurrentGravship != null)
					{
						foreach (Pawn pawn in Find.CurrentGravship.Pawns)
						{
							if (!pawn.SpawnedOrAnyParentSpawned)
							{
								PawnsFinder.allCaravansAndTravellingTransporters_AliveOrDead_Result.Add(pawn);
							}
						}
					}
				}
				return PawnsFinder.allCaravansAndTravellingTransporters_AliveOrDead_Result;
			}
		}

		// Token: 0x17001DF9 RID: 7673
		// (get) Token: 0x0600C5A1 RID: 50593 RVA: 0x003983BC File Offset: 0x003965BC
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_Colonists
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsColonist)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_Result;
			}
		}

		// Token: 0x17001DFA RID: 7674
		// (get) Token: 0x0600C5A2 RID: 50594 RVA: 0x00398410 File Offset: 0x00396610
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_Colonists_NoSlaves
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_NoSlaves_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsColonist && !allMapsCaravansAndTravellingTransporters_Alive[i].IsSlave)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_NoSlaves_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_NoSlaves_Result;
			}
		}

		// Token: 0x17001DFB RID: 7675
		// (get) Token: 0x0600C5A3 RID: 50595 RVA: 0x00398470 File Offset: 0x00396670
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsFreeColonist)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_Result;
			}
		}

		// Token: 0x17001DFC RID: 7676
		// (get) Token: 0x0600C5A4 RID: 50596 RVA: 0x003984C4 File Offset: 0x003966C4
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_Result.Clear();
				IReadOnlyList<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned = PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_AliveSpawned.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_AliveSpawned[i].IsFreeColonist)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_Result.Add(allMapsCaravansAndTravellingTransporters_AliveSpawned[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_Result;
			}
		}

		// Token: 0x17001DFD RID: 7677
		// (get) Token: 0x0600C5A5 RID: 50597 RVA: 0x00398518 File Offset: 0x00396718
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsFreeColonist && !allMapsCaravansAndTravellingTransporters_Alive[i].IsQuestLodger())
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers_Result;
			}
		}

		// Token: 0x17001DFE RID: 7678
		// (get) Token: 0x0600C5A6 RID: 50598 RVA: 0x00398578 File Offset: 0x00396778
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsFreeColonist && !allMapsCaravansAndTravellingTransporters_Alive[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended_Result;
			}
		}

		// Token: 0x17001DFF RID: 7679
		// (get) Token: 0x0600C5A7 RID: 50599 RVA: 0x003985D8 File Offset: 0x003967D8
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended_Result.Clear();
				IReadOnlyList<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned = PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_AliveSpawned.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_AliveSpawned[i].IsFreeColonist && !allMapsCaravansAndTravellingTransporters_AliveSpawned[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended_Result.Add(allMapsCaravansAndTravellingTransporters_AliveSpawned[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended_Result;
			}
		}

		// Token: 0x17001E00 RID: 7680
		// (get) Token: 0x0600C5A8 RID: 50600 RVA: 0x00398638 File Offset: 0x00396838
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsFreeColonist && !allMapsCaravansAndTravellingTransporters_Alive[i].InCryptosleep)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep_Result;
			}
		}

		// Token: 0x17001E01 RID: 7681
		// (get) Token: 0x0600C5A9 RID: 50601 RVA: 0x00398698 File Offset: 0x00396898
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_Result.Clear();
				Faction ofPlayer = Faction.OfPlayer;
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].Faction == ofPlayer)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_Result;
			}
		}

		// Token: 0x17001E02 RID: 7682
		// (get) Token: 0x0600C5AA RID: 50602 RVA: 0x003986F4 File Offset: 0x003968F4
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_NoCryptosleep
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_NoCryptosleep_Result.Clear();
				Faction ofPlayer = Faction.OfPlayer;
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].Faction == ofPlayer && !allMapsCaravansAndTravellingTransporters_Alive[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_NoCryptosleep_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_NoCryptosleep_Result;
			}
		}

		// Token: 0x17001E03 RID: 7683
		// (get) Token: 0x0600C5AB RID: 50603 RVA: 0x0039875C File Offset: 0x0039695C
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsPrisonerOfColony)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony_Result;
			}
		}

		// Token: 0x17001E04 RID: 7684
		// (get) Token: 0x0600C5AC RID: 50604 RVA: 0x003987B0 File Offset: 0x003969B0
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony_Result.Clear();
				IReadOnlyList<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned = PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_AliveSpawned.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_AliveSpawned[i].IsPrisonerOfColony)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony_Result.Add(allMapsCaravansAndTravellingTransporters_AliveSpawned[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony_Result;
			}
		}

		// Token: 0x17001E05 RID: 7685
		// (get) Token: 0x0600C5AD RID: 50605 RVA: 0x00398804 File Offset: 0x00396A04
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_SlavesOfColony
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_SlavesOfColony_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive[i].IsSlaveOfColony)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_SlavesOfColony_Result.Add(allMapsCaravansAndTravellingTransporters_Alive[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_SlavesOfColony_Result;
			}
		}

		// Token: 0x17001E06 RID: 7686
		// (get) Token: 0x0600C5AE RID: 50606 RVA: 0x00398858 File Offset: 0x00396A58
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners
		{
			get
			{
				List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists = PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists;
				List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony = PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony;
				if (allMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony.Count == 0)
				{
					return allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists;
				}
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_Result.Clear();
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_Result.AddRange(allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists);
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_Result.AddRange(allMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony);
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_Result;
			}
		}

		// Token: 0x17001E07 RID: 7687
		// (get) Token: 0x0600C5AF RID: 50607 RVA: 0x003988A0 File Offset: 0x00396AA0
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners
		{
			get
			{
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists;
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony;
				if (allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony.Count == 0)
				{
					return allMapsCaravansAndTravellingTransporters_Alive_FreeColonists;
				}
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_Result.Clear();
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_Result.AddRange(allMapsCaravansAndTravellingTransporters_Alive_FreeColonists);
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_Result.AddRange(allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony);
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_Result;
			}
		}

		// Token: 0x17001E08 RID: 7688
		// (get) Token: 0x0600C5B0 RID: 50608 RVA: 0x003988E8 File Offset: 0x00396AE8
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_NoCryptosleep
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners.Count; i++)
				{
					if (!allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result.Add(allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result;
			}
		}

		// Token: 0x17001E09 RID: 7689
		// (get) Token: 0x0600C5B1 RID: 50609 RVA: 0x0039893C File Offset: 0x00396B3C
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_NoCryptosleep
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_NoCryptosleep_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners = PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners.Count; i++)
				{
					if (!allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_NoCryptosleep_Result.Add(allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_NoCryptosleep_Result;
			}
		}

		// Token: 0x17001E0A RID: 7690
		// (get) Token: 0x0600C5B2 RID: 50610 RVA: 0x00398990 File Offset: 0x00396B90
		public static List<Pawn> AllMaps_PrisonersOfColonySpawned
		{
			get
			{
				PawnsFinder.allMaps_PrisonersOfColonySpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.PrisonersOfColonySpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_PrisonersOfColonySpawned_Result.AddRange(maps[i].mapPawns.PrisonersOfColonySpawned);
					}
				}
				return PawnsFinder.allMaps_PrisonersOfColonySpawned_Result;
			}
		}

		// Token: 0x17001E0B RID: 7691
		// (get) Token: 0x0600C5B3 RID: 50611 RVA: 0x00398A00 File Offset: 0x00396C00
		public static List<Pawn> AllMaps_PrisonersOfColony
		{
			get
			{
				PawnsFinder.allMaps_PrisonersOfColony_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.PrisonersOfColony;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_PrisonersOfColony_Result.AddRange(maps[i].mapPawns.PrisonersOfColony);
					}
				}
				return PawnsFinder.allMaps_PrisonersOfColony_Result;
			}
		}

		// Token: 0x17001E0C RID: 7692
		// (get) Token: 0x0600C5B4 RID: 50612 RVA: 0x00398A70 File Offset: 0x00396C70
		public static List<Pawn> AllMaps_FreeColonists
		{
			get
			{
				PawnsFinder.allMaps_FreeColonists_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonists;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonists_Result.AddRange(maps[i].mapPawns.FreeColonists);
					}
				}
				return PawnsFinder.allMaps_FreeColonists_Result;
			}
		}

		// Token: 0x17001E0D RID: 7693
		// (get) Token: 0x0600C5B5 RID: 50613 RVA: 0x00398AE0 File Offset: 0x00396CE0
		public static List<Pawn> AllMaps_FreeColonistsSpawned
		{
			get
			{
				PawnsFinder.allMaps_FreeColonistsSpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonistsSpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonistsSpawned_Result.AddRange(maps[i].mapPawns.FreeColonistsSpawned);
					}
				}
				return PawnsFinder.allMaps_FreeColonistsSpawned_Result;
			}
		}

		// Token: 0x17001E0E RID: 7694
		// (get) Token: 0x0600C5B6 RID: 50614 RVA: 0x00398B50 File Offset: 0x00396D50
		public static List<Pawn> AllMaps_FreeColonistsAndPrisonersSpawned
		{
			get
			{
				PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonistsAndPrisonersSpawned;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result.AddRange(maps[i].mapPawns.FreeColonistsAndPrisonersSpawned);
					}
				}
				return PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result;
			}
		}

		// Token: 0x17001E0F RID: 7695
		// (get) Token: 0x0600C5B7 RID: 50615 RVA: 0x00398BC0 File Offset: 0x00396DC0
		public static List<Pawn> AllMaps_FreeColonistsAndPrisoners
		{
			get
			{
				PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.FreeColonistsAndPrisoners;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result.AddRange(maps[i].mapPawns.FreeColonistsAndPrisoners);
					}
				}
				return PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result;
			}
		}

		// Token: 0x17001E10 RID: 7696
		// (get) Token: 0x0600C5B8 RID: 50616 RVA: 0x00398C30 File Offset: 0x00396E30
		public static List<Pawn> AllMaps_ColonySubhumansSpawnedPlayerControlled
		{
			get
			{
				PawnsFinder.allMaps_ColonySubhumansSpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						return maps[0].mapPawns.SpawnedColonySubhumansPlayerControlled;
					}
					for (int i = 0; i < maps.Count; i++)
					{
						PawnsFinder.allMaps_ColonySubhumansSpawned_Result.AddRange(maps[i].mapPawns.SpawnedColonySubhumansPlayerControlled);
					}
				}
				return PawnsFinder.allMaps_ColonySubhumansSpawned_Result;
			}
		}

		// Token: 0x17001E11 RID: 7697
		// (get) Token: 0x0600C5B9 RID: 50617 RVA: 0x00398CA0 File Offset: 0x00396EA0
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_ColonySubhumans_NoSuspended
		{
			get
			{
				PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_ColonySubhumans_NoSuspended_Result.Clear();
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction;
				for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction.Count; i++)
				{
					if (allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction[i].IsColonySubhuman && !allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction[i].Suspended)
					{
						PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_ColonySubhumans_NoSuspended_Result.Add(allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction[i]);
					}
				}
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_ColonySubhumans_NoSuspended_Result;
			}
		}

		// Token: 0x0600C5BA RID: 50618 RVA: 0x00398D00 File Offset: 0x00396F00
		public static List<Pawn> AllMaps_SpawnedPawnsInFaction(Faction faction)
		{
			List<Pawn> list;
			if (!PawnsFinder.allMaps_SpawnedPawnsInFaction_Result.TryGetValue(faction, out list))
			{
				list = new List<Pawn>();
				PawnsFinder.allMaps_SpawnedPawnsInFaction_Result.Add(faction, list);
			}
			list.Clear();
			if (Current.ProgramState != ProgramState.Entry)
			{
				List<Map> maps = Find.Maps;
				if (maps.Count == 1)
				{
					return maps[0].mapPawns.SpawnedPawnsInFaction(faction);
				}
				for (int i = 0; i < maps.Count; i++)
				{
					list.AddRange(maps[i].mapPawns.SpawnedPawnsInFaction(faction));
				}
			}
			return list;
		}

		// Token: 0x0600C5BB RID: 50619 RVA: 0x00398D88 File Offset: 0x00396F88
		public static List<Pawn> AllMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype(XenotypeDef def)
		{
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype_Result.Clear();
			if (!ModsConfig.BiotechActive)
			{
				return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype_Result;
			}
			List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_Colonists = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists;
			for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive_Colonists.Count; i++)
			{
				if (allMapsCaravansAndTravellingTransporters_Alive_Colonists[i].genes != null && allMapsCaravansAndTravellingTransporters_Alive_Colonists[i].genes.Xenotype == def)
				{
					PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype_Result.Add(allMapsCaravansAndTravellingTransporters_Alive_Colonists[i]);
				}
			}
			return PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype_Result;
		}

		// Token: 0x17001E12 RID: 7698
		// (get) Token: 0x0600C5BC RID: 50620 RVA: 0x00398DFC File Offset: 0x00396FFC
		public static List<Pawn> HomeMaps_FreeColonistsSpawned
		{
			get
			{
				PawnsFinder.homeMaps_FreeColonistsSpawned_Result.Clear();
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					if (maps.Count == 1)
					{
						if (!maps[0].IsPlayerHome)
						{
							return PawnsFinder.homeMaps_FreeColonistsSpawned_Result;
						}
						return maps[0].mapPawns.FreeColonistsSpawned;
					}
					else
					{
						for (int i = 0; i < maps.Count; i++)
						{
							if (maps[i].IsPlayerHome)
							{
								PawnsFinder.homeMaps_FreeColonistsSpawned_Result.AddRange(maps[i].mapPawns.FreeColonistsSpawned);
							}
						}
					}
				}
				return PawnsFinder.homeMaps_FreeColonistsSpawned_Result;
			}
		}

		// Token: 0x0600C5BD RID: 50621 RVA: 0x00398E90 File Offset: 0x00397090
		public static void Clear()
		{
			PawnsFinder.allMapsWorldAndTemporary_AliveOrDead_Result.Clear();
			PawnsFinder.allMapsWorldAndTemporary_Alive_Result.Clear();
			PawnsFinder.allMapsAndWorld_Alive_Result.Clear();
			PawnsFinder.allMaps_Result.Clear();
			PawnsFinder.allMaps_Spawned_Result.Clear();
			PawnsFinder.all_AliveOrDead_Result.Clear();
			PawnsFinder.temporary_Result.Clear();
			PawnsFinder.temporary_Alive_Result.Clear();
			PawnsFinder.temporary_Dead_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Result.Clear();
			PawnsFinder.allCaravansAndTravellingTransporters_Alive_Result.Clear();
			PawnsFinder.allCaravansAndTravellingTransporters_AliveOrDead_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_NoCryptosleep_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result.Clear();
			PawnsFinder.allMaps_PrisonersOfColonySpawned_Result.Clear();
			PawnsFinder.allMaps_PrisonersOfColony_Result.Clear();
			PawnsFinder.allMaps_FreeColonists_Result.Clear();
			PawnsFinder.allMaps_FreeColonistsSpawned_Result.Clear();
			PawnsFinder.allMaps_FreeColonistsAndPrisonersSpawned_Result.Clear();
			PawnsFinder.allMaps_FreeColonistsAndPrisoners_Result.Clear();
			PawnsFinder.allMaps_SpawnedPawnsInFaction_Result.Clear();
			PawnsFinder.homeMaps_FreeColonistsSpawned_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_NoSlaves_Result.Clear();
			PawnsFinder.allMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype_Result.Clear();
		}

		// Token: 0x040086BC RID: 34492
		private static List<Pawn> allMapsWorldAndTemporary_AliveOrDead_Result = new List<Pawn>();

		// Token: 0x040086BD RID: 34493
		private static List<Pawn> allMapsWorldAndTemporary_Alive_Result = new List<Pawn>();

		// Token: 0x040086BE RID: 34494
		private static List<Pawn> allMapsAndWorld_Alive_Result = new List<Pawn>();

		// Token: 0x040086BF RID: 34495
		private static List<Pawn> allMaps_Result = new List<Pawn>();

		// Token: 0x040086C0 RID: 34496
		private static List<Pawn> allMaps_Spawned_Result = new List<Pawn>();

		// Token: 0x040086C1 RID: 34497
		private static List<Pawn> all_AliveOrDead_Result = new List<Pawn>();

		// Token: 0x040086C2 RID: 34498
		private static List<Pawn> temporary_Result = new List<Pawn>();

		// Token: 0x040086C3 RID: 34499
		private static List<Pawn> temporary_Alive_Result = new List<Pawn>();

		// Token: 0x040086C4 RID: 34500
		private static List<Pawn> temporary_Dead_Result = new List<Pawn>();

		// Token: 0x040086C5 RID: 34501
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_Result = new List<Pawn>();

		// Token: 0x040086C6 RID: 34502
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_Result = new List<Pawn>();

		// Token: 0x040086C7 RID: 34503
		private static List<Pawn> allCaravansAndTravellingTransporters_Alive_Result = new List<Pawn>();

		// Token: 0x040086C8 RID: 34504
		private static List<Pawn> allCaravansAndTravellingTransporters_AliveOrDead_Result = new List<Pawn>();

		// Token: 0x040086C9 RID: 34505
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_Colonists_Result = new List<Pawn>();

		// Token: 0x040086CA RID: 34506
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_Colonists_NoSlaves_Result = new List<Pawn>();

		// Token: 0x040086CB RID: 34507
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_Result = new List<Pawn>();

		// Token: 0x040086CC RID: 34508
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_Result = new List<Pawn>();

		// Token: 0x040086CD RID: 34509
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers_Result = new List<Pawn>();

		// Token: 0x040086CE RID: 34510
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended_Result = new List<Pawn>();

		// Token: 0x040086CF RID: 34511
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended_Result = new List<Pawn>();

		// Token: 0x040086D0 RID: 34512
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x040086D1 RID: 34513
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_Result = new List<Pawn>();

		// Token: 0x040086D2 RID: 34514
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x040086D3 RID: 34515
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony_Result = new List<Pawn>();

		// Token: 0x040086D4 RID: 34516
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_PrisonersOfColony_Result = new List<Pawn>();

		// Token: 0x040086D5 RID: 34517
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_SlavesOfColony_Result = new List<Pawn>();

		// Token: 0x040086D6 RID: 34518
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_Result = new List<Pawn>();

		// Token: 0x040086D7 RID: 34519
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_Result = new List<Pawn>();

		// Token: 0x040086D8 RID: 34520
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x040086D9 RID: 34521
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_NoCryptosleep_Result = new List<Pawn>();

		// Token: 0x040086DA RID: 34522
		private static List<Pawn> allMaps_PrisonersOfColonySpawned_Result = new List<Pawn>();

		// Token: 0x040086DB RID: 34523
		private static List<Pawn> allMaps_PrisonersOfColony_Result = new List<Pawn>();

		// Token: 0x040086DC RID: 34524
		private static List<Pawn> allMaps_FreeColonists_Result = new List<Pawn>();

		// Token: 0x040086DD RID: 34525
		private static List<Pawn> allMaps_FreeColonistsSpawned_Result = new List<Pawn>();

		// Token: 0x040086DE RID: 34526
		private static List<Pawn> allMaps_FreeColonistsAndPrisonersSpawned_Result = new List<Pawn>();

		// Token: 0x040086DF RID: 34527
		private static List<Pawn> allMaps_FreeColonistsAndPrisoners_Result = new List<Pawn>();

		// Token: 0x040086E0 RID: 34528
		private static List<Pawn> allMaps_ColonySubhumansSpawned_Result = new List<Pawn>();

		// Token: 0x040086E1 RID: 34529
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_ColonySubhumans_NoSuspended_Result = new List<Pawn>();

		// Token: 0x040086E2 RID: 34530
		private static Dictionary<Faction, List<Pawn>> allMaps_SpawnedPawnsInFaction_Result = new Dictionary<Faction, List<Pawn>>();

		// Token: 0x040086E3 RID: 34531
		private static List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype_Result = new List<Pawn>();

		// Token: 0x040086E4 RID: 34532
		private static List<Pawn> homeMaps_FreeColonistsSpawned_Result = new List<Pawn>();
	}
}
