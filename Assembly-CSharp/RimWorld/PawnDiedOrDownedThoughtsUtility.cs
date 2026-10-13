using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002337 RID: 9015
	public static class PawnDiedOrDownedThoughtsUtility
	{
		// Token: 0x0600C39B RID: 50075 RVA: 0x0038BCB0 File Offset: 0x00389EB0
		public static void TryGiveThoughts(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind)
		{
			try
			{
				if (!PawnGenerator.IsBeingGenerated(victim))
				{
					if (Current.ProgramState == ProgramState.Playing)
					{
						if (!victim.wasLeftBehindStartingPawn)
						{
							PawnDiedOrDownedThoughtsUtility.GetThoughts(victim, dinfo, thoughtsKind, PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd, PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts);
							for (int i = 0; i < PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Count; i++)
							{
								PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd[i].Add();
							}
							if (PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Any<ThoughtToAddToAll>())
							{
								foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
								{
									if (pawn != victim)
									{
										for (int j = 0; j < PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Count; j++)
										{
											PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts[j].Add(pawn);
										}
									}
								}
							}
							PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Clear();
							PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Clear();
							if ((dinfo == null || !dinfo.Value.Def.execution) && thoughtsKind == PawnDiedOrDownedThoughtsKind.Died && victim.IsPrisonerOfColony)
							{
								Pawn arg = PawnDiedOrDownedThoughtsUtility.FindResponsibleColonist(victim, dinfo);
								if (!victim.guilt.IsGuilty && !victim.InAggroMentalState)
								{
									Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.InnocentPrisonerDied, arg.Named(HistoryEventArgsNames.Doer)), true);
								}
								else
								{
									Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.GuiltyPrisonerDied, arg.Named(HistoryEventArgsNames.Doer)), true);
								}
								Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.PrisonerDied, arg.Named(HistoryEventArgsNames.Doer)), true);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				string str = "Could not give thoughts: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x0600C39C RID: 50076 RVA: 0x0038BEB0 File Offset: 0x0038A0B0
		public static void TryGiveThoughts(IEnumerable<Pawn> victims, PawnDiedOrDownedThoughtsKind thoughtsKind)
		{
			foreach (Pawn victim in victims)
			{
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(victim, null, thoughtsKind);
			}
		}

		// Token: 0x0600C39D RID: 50077 RVA: 0x0038BF00 File Offset: 0x0038A100
		public static void GetThoughts(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, List<IndividualThoughtToAdd> outIndividualThoughts, List<ThoughtToAddToAll> outAllColonistsThoughts)
		{
			outIndividualThoughts.Clear();
			outAllColonistsThoughts.Clear();
			if (victim.RaceProps.Humanlike)
			{
				PawnDiedOrDownedThoughtsUtility.AppendThoughts_ForHumanlike(victim, dinfo, thoughtsKind, outIndividualThoughts, outAllColonistsThoughts);
			}
			if (victim.relations != null && victim.relations.everSeenByPlayer)
			{
				PawnDiedOrDownedThoughtsUtility.AppendThoughts_Relations(victim, dinfo, thoughtsKind, outIndividualThoughts, outAllColonistsThoughts);
			}
		}

		// Token: 0x0600C39E RID: 50078 RVA: 0x0038BF54 File Offset: 0x0038A154
		public static void BuildMoodThoughtsListString(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, StringBuilder sb, string individualThoughtsHeader, string allColonistsThoughtsHeader)
		{
			PawnDiedOrDownedThoughtsUtility.GetThoughts(victim, dinfo, thoughtsKind, PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd, PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts);
			if (PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Any<ThoughtToAddToAll>())
			{
				if (!allColonistsThoughtsHeader.NullOrEmpty())
				{
					sb.Append(allColonistsThoughtsHeader);
					sb.AppendLine();
				}
				for (int i = 0; i < PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Count; i++)
				{
					ThoughtToAddToAll thoughtToAddToAll = PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts[i];
					if (sb.Length > 0)
					{
						sb.AppendLine();
					}
					sb.Append("  - " + thoughtToAddToAll.thoughtDef.stages[0].LabelCap + " " + Mathf.RoundToInt(thoughtToAddToAll.thoughtDef.stages[0].baseMoodEffect).ToStringWithSign());
				}
			}
			if (PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Any((IndividualThoughtToAdd x) => x.thought.MoodOffset() != 0f))
			{
				if (!individualThoughtsHeader.NullOrEmpty())
				{
					sb.Append(individualThoughtsHeader);
				}
				foreach (IGrouping<Pawn, IndividualThoughtToAdd> grouping in from x in PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd
				where x.thought.MoodOffset() != 0f
				group x by x.addTo)
				{
					if (sb.Length > 0)
					{
						sb.AppendLine();
						sb.AppendLine();
					}
					string value = grouping.Key.KindLabel.CapitalizeFirst() + " " + grouping.Key.LabelShort;
					sb.Append(value);
					sb.Append(":");
					foreach (IndividualThoughtToAdd individualThoughtToAdd in grouping)
					{
						sb.AppendLine();
						sb.Append("    " + individualThoughtToAdd.LabelCap);
					}
				}
			}
		}

		// Token: 0x0600C39F RID: 50079 RVA: 0x0038C188 File Offset: 0x0038A388
		public static void BuildMoodThoughtsListString(IEnumerable<Pawn> victims, PawnDiedOrDownedThoughtsKind thoughtsKind, StringBuilder sb, string individualThoughtsHeader, string allColonistsThoughtsHeader, string victimLabelKey)
		{
			foreach (Pawn pawn in victims)
			{
				PawnDiedOrDownedThoughtsUtility.GetThoughts(pawn, null, thoughtsKind, PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd, PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts);
				if (PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Any<IndividualThoughtToAdd>() || PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Any<ThoughtToAddToAll>())
				{
					if (sb.Length > 0)
					{
						sb.AppendLine();
						sb.AppendLine();
					}
					string text = pawn.KindLabel.CapitalizeFirst() + " " + pawn.LabelShort;
					if (victimLabelKey.NullOrEmpty())
					{
						sb.Append(text + ":");
					}
					else
					{
						sb.Append(victimLabelKey.Translate(text));
					}
					PawnDiedOrDownedThoughtsUtility.BuildMoodThoughtsListString(pawn, null, thoughtsKind, sb, individualThoughtsHeader, allColonistsThoughtsHeader);
				}
			}
		}

		// Token: 0x0600C3A0 RID: 50080 RVA: 0x0038C280 File Offset: 0x0038A480
		private static void AppendThoughts_ForHumanlike(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, List<IndividualThoughtToAdd> outIndividualThoughts, List<ThoughtToAddToAll> outAllColonistsThoughts)
		{
			bool flag = dinfo != null && dinfo.Value.Def.execution;
			if (dinfo != null && dinfo.Value.Def.ExternalViolenceFor(victim) && dinfo.Value.Instigator != null && dinfo.Value.Instigator is Pawn)
			{
				Pawn pawn = (Pawn)dinfo.Value.Instigator;
				if (!pawn.Dead && pawn.needs.mood != null && pawn.story != null && pawn != victim && PawnUtility.ShouldGetThoughtAbout(pawn, victim))
				{
					if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died)
					{
						outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KilledHumanlikeBloodlust, pawn, null, 1f, 1f));
					}
					if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died && victim.HostileTo(pawn) && victim.Faction != null && PawnUtility.IsFactionLeader(victim) && victim.Faction.HostileTo(pawn.Faction))
					{
						outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.DefeatedHostileFactionLeader, pawn, victim, 1f, 1f));
					}
					if (ModsConfig.BiotechActive && thoughtsKind == PawnDiedOrDownedThoughtsKind.Died && !victim.DevelopmentalStage.Adult() && pawn.DevelopmentalStage.Adult())
					{
						outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KilledChild, pawn, victim, 1f, 1f));
					}
				}
			}
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died && !flag)
			{
				foreach (Pawn pawn2 in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive)
				{
					if (pawn2 != victim && pawn2.needs != null && pawn2.needs.mood != null && PawnUtility.ShouldGetThoughtAbout(pawn2, victim) && (pawn2.MentalStateDef != MentalStateDefOf.SocialFighting || ((MentalState_SocialFighting)pawn2.MentalState).otherPawn != victim))
					{
						if (ThoughtUtility.Witnessed(pawn2, victim))
						{
							bool flag2 = pawn2.Faction == Faction.OfPlayer && victim.IsQuestLodger();
							if (pawn2.Faction == victim.Faction && !flag2 && !victim.IsSlave)
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathAlly, pawn2, null, 1f, 1f));
							}
							else if (victim.Faction == null || !victim.Faction.HostileTo(pawn2.Faction) || flag2 || victim.IsSlave)
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathNonAlly, pawn2, null, 1f, 1f));
							}
							if (pawn2.relations.FamilyByBlood.Contains(victim))
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathFamily, pawn2, null, 1f, 1f));
							}
							outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathBloodlust, pawn2, null, 1f, 1f));
						}
						else if (victim.Faction == Faction.OfPlayer && victim.Faction == pawn2.Faction && victim.HostFaction != pawn2.Faction && !victim.IsQuestLodger() && !victim.IsSubhuman && !victim.IsSlave)
						{
							outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KnowColonistDied, pawn2, victim, 1f, 1f));
						}
					}
				}
			}
			if (victim.guilt == null || !victim.guilt.IsGuilty)
			{
				if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Banished && victim.IsColonist && !victim.IsSlave)
				{
					outAllColonistsThoughts.Add(new ThoughtToAddToAll(ThoughtDefOf.ColonistBanished, victim));
				}
				if (thoughtsKind == PawnDiedOrDownedThoughtsKind.DeniedJoining)
				{
					outAllColonistsThoughts.Add(new ThoughtToAddToAll(ThoughtDefOf.DeniedJoining, victim));
				}
				if (thoughtsKind == PawnDiedOrDownedThoughtsKind.BanishedToDie)
				{
					if (victim.IsColonist && !victim.IsSlave)
					{
						outAllColonistsThoughts.Add(new ThoughtToAddToAll(ThoughtDefOf.ColonistBanishedToDie, victim));
					}
					else if (victim.IsPrisonerOfColony || victim.IsSlaveOfColony)
					{
						outAllColonistsThoughts.Add(new ThoughtToAddToAll(ThoughtDefOf.PrisonerBanishedToDie, victim));
					}
				}
				if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Lost && victim.IsColonist && !victim.IsQuestLodger() && !victim.IsSlave)
				{
					outAllColonistsThoughts.Add(new ThoughtToAddToAll(ThoughtDefOf.ColonistLost, victim));
				}
			}
		}

		// Token: 0x0600C3A1 RID: 50081 RVA: 0x0038C6D8 File Offset: 0x0038A8D8
		private static void AppendThoughts_Relations(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, List<IndividualThoughtToAdd> outIndividualThoughts, List<ThoughtToAddToAll> outAllColonistsThoughts)
		{
			PawnDiedOrDownedThoughtsUtility.<>c__DisplayClass8_0 CS$<>8__locals1;
			CS$<>8__locals1.victim = victim;
			CS$<>8__locals1.outIndividualThoughts = outIndividualThoughts;
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Banished && CS$<>8__locals1.victim.RaceProps.Animal)
			{
				PawnDiedOrDownedThoughtsUtility.<AppendThoughts_Relations>g__GiveThoughtsForAnimalBond|8_0(ThoughtDefOf.BondedAnimalBanished, ref CS$<>8__locals1);
			}
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.ReleasedToWild && CS$<>8__locals1.victim.RaceProps.Animal)
			{
				PawnDiedOrDownedThoughtsUtility.<AppendThoughts_Relations>g__GiveThoughtsForAnimalBond|8_0(ThoughtDefOf.BondedAnimalReleased, ref CS$<>8__locals1);
			}
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died || thoughtsKind == PawnDiedOrDownedThoughtsKind.BanishedToDie || thoughtsKind == PawnDiedOrDownedThoughtsKind.Lost)
			{
				List<Pawn> list = CS$<>8__locals1.victim.relations.PotentiallyRelatedPawns.Where(delegate(Pawn x)
				{
					Pawn_NeedsTracker needs = x.needs;
					return ((needs != null) ? needs.mood : null) != null;
				}).ToList<Pawn>();
				foreach (Pawn pawn in list)
				{
					if (PawnUtility.ShouldGetThoughtAbout(pawn, CS$<>8__locals1.victim))
					{
						PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(CS$<>8__locals1.victim);
						if (mostImportantRelation != null)
						{
							ThoughtDef genderSpecificThought = mostImportantRelation.GetGenderSpecificThought(CS$<>8__locals1.victim, thoughtsKind);
							if (genderSpecificThought != null)
							{
								CS$<>8__locals1.outIndividualThoughts.Add(new IndividualThoughtToAdd(genderSpecificThought, pawn, CS$<>8__locals1.victim, 1f, 1f));
							}
						}
					}
				}
				if (dinfo != null && thoughtsKind != PawnDiedOrDownedThoughtsKind.Lost)
				{
					Pawn pawn2 = dinfo.Value.Instigator as Pawn;
					if (pawn2 != null && pawn2 != CS$<>8__locals1.victim)
					{
						foreach (Pawn pawn3 in list)
						{
							if (pawn2 != pawn3 && PawnUtility.ShouldGetThoughtAbout(pawn3, CS$<>8__locals1.victim))
							{
								PawnRelationDef mostImportantRelation2 = pawn3.GetMostImportantRelation(CS$<>8__locals1.victim);
								ThoughtDef thoughtDef = (mostImportantRelation2 != null) ? mostImportantRelation2.GetGenderSpecificKilledThought(CS$<>8__locals1.victim) : null;
								if (thoughtDef != null)
								{
									CS$<>8__locals1.outIndividualThoughts.Add(new IndividualThoughtToAdd(thoughtDef, pawn3, pawn2, 1f, 1f));
								}
								if (pawn3.RaceProps.IsFlesh)
								{
									int num = pawn3.relations.OpinionOf(CS$<>8__locals1.victim);
									if (num >= 20)
									{
										CS$<>8__locals1.outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KilledMyFriend, pawn3, pawn2, 1f, CS$<>8__locals1.victim.relations.GetFriendDiedThoughtPowerFactor(num)));
									}
									else if (num <= -20)
									{
										CS$<>8__locals1.outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KilledMyRival, pawn3, pawn2, 1f, CS$<>8__locals1.victim.relations.GetRivalDiedThoughtPowerFactor(num)));
									}
								}
							}
						}
					}
				}
				if (CS$<>8__locals1.victim.RaceProps.Humanlike)
				{
					ThoughtDef thoughtDef2;
					ThoughtDef thoughtDef3;
					if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Lost)
					{
						thoughtDef2 = ThoughtDefOf.PawnWithGoodOpinionLost;
						thoughtDef3 = ThoughtDefOf.PawnWithBadOpinionLost;
					}
					else
					{
						thoughtDef2 = ThoughtDefOf.PawnWithGoodOpinionDied;
						thoughtDef3 = ThoughtDefOf.PawnWithBadOpinionDied;
					}
					List<Pawn> allMapsCaravansAndTravellingTransporters_Alive = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive;
					for (int i = 0; i < allMapsCaravansAndTravellingTransporters_Alive.Count; i++)
					{
						Pawn pawn4 = allMapsCaravansAndTravellingTransporters_Alive[i];
						if (pawn4.needs != null && pawn4.RaceProps.IsFlesh && pawn4.needs.mood != null && PawnUtility.ShouldGetThoughtAbout(pawn4, CS$<>8__locals1.victim))
						{
							int num2 = pawn4.relations.OpinionOf(CS$<>8__locals1.victim);
							if (num2 >= 20)
							{
								CS$<>8__locals1.outIndividualThoughts.Add(new IndividualThoughtToAdd(thoughtDef2, pawn4, CS$<>8__locals1.victim, CS$<>8__locals1.victim.relations.GetFriendDiedThoughtPowerFactor(num2), 1f));
							}
							else if (num2 <= -20)
							{
								CS$<>8__locals1.outIndividualThoughts.Add(new IndividualThoughtToAdd(thoughtDef3, pawn4, CS$<>8__locals1.victim, CS$<>8__locals1.victim.relations.GetRivalDiedThoughtPowerFactor(num2), 1f));
							}
						}
					}
				}
			}
		}

		// Token: 0x0600C3A2 RID: 50082 RVA: 0x0038CAC4 File Offset: 0x0038ACC4
		private static Pawn FindResponsibleColonist(Pawn victim, DamageInfo? dinfo)
		{
			if (dinfo != null)
			{
				Pawn pawn = dinfo.Value.Instigator as Pawn;
				if (pawn != null && pawn.IsColonist)
				{
					return pawn;
				}
			}
			if (victim.Spawned)
			{
				if (victim.Map.mapPawns.FreeColonistsSpawned.Any((Pawn x) => !x.Downed))
				{
					return (from x in victim.Map.mapPawns.FreeColonistsSpawned
					where !x.Downed
					select x).MinBy((Pawn x) => x.Position.DistanceToSquared(victim.Position));
				}
				if (victim.Map.mapPawns.FreeColonistsSpawned.Any<Pawn>())
				{
					return victim.Map.mapPawns.FreeColonistsSpawned.MinBy((Pawn x) => x.Position.DistanceToSquared(victim.Position));
				}
			}
			List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists;
			if (allMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Any<Pawn>())
			{
				return allMapsCaravansAndTravellingTransporters_Alive_FreeColonists.First<Pawn>();
			}
			return null;
		}

		// Token: 0x0600C3A3 RID: 50083 RVA: 0x0038CBF8 File Offset: 0x0038ADF8
		public static void RemoveDiedThoughts(Pawn pawn)
		{
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_Alive)
			{
				if (pawn2.needs != null && pawn2.needs.mood != null && pawn2 != pawn)
				{
					MemoryThoughtHandler memories = pawn2.needs.mood.thoughts.memories;
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.KnowColonistDied, pawn);
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.KnowPrisonerDiedInnocent, pawn);
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.PawnWithGoodOpinionDied, pawn);
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.PawnWithBadOpinionDied, pawn);
					if (ModsConfig.BiotechActive)
					{
						memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.Stillbirth, pawn);
					}
					List<PawnRelationDef> allDefsListForReading = DefDatabase<PawnRelationDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						ThoughtDef genderSpecificDiedThought = allDefsListForReading[i].GetGenderSpecificDiedThought(pawn);
						if (genderSpecificDiedThought != null)
						{
							memories.RemoveMemoriesOfDefWhereOtherPawnIs(genderSpecificDiedThought, pawn);
						}
					}
				}
			}
		}

		// Token: 0x0600C3A4 RID: 50084 RVA: 0x0038CCFC File Offset: 0x0038AEFC
		public static void RemoveLostThoughts(Pawn pawn)
		{
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_Alive)
			{
				if (pawn2.needs != null && pawn2.needs.mood != null && pawn2 != pawn)
				{
					MemoryThoughtHandler memories = pawn2.needs.mood.thoughts.memories;
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.ColonistLost, pawn);
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.PawnWithGoodOpinionLost, pawn);
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.PawnWithBadOpinionLost, pawn);
					List<PawnRelationDef> allDefsListForReading = DefDatabase<PawnRelationDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						ThoughtDef genderSpecificLostThought = allDefsListForReading[i].GetGenderSpecificLostThought(pawn);
						if (genderSpecificLostThought != null)
						{
							memories.RemoveMemoriesOfDefWhereOtherPawnIs(genderSpecificLostThought, pawn);
						}
					}
				}
			}
		}

		// Token: 0x0600C3A5 RID: 50085 RVA: 0x0038CDDC File Offset: 0x0038AFDC
		public static void RemoveResuedRelativeThought(Pawn pawn)
		{
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_Alive)
			{
				if (pawn2.needs != null && pawn2.needs.mood != null && pawn2 != pawn)
				{
					pawn2.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.RescuedRelative, pawn);
				}
			}
		}

		// Token: 0x0600C3A6 RID: 50086 RVA: 0x0038CE60 File Offset: 0x0038B060
		public static void GiveVeneratedAnimalDiedThoughts(Pawn victim, Map map)
		{
			if (ModsConfig.IdeologyActive && victim.Faction != null && map != null)
			{
				foreach (Pawn pawn in map.mapPawns.PawnsInFaction(victim.Faction))
				{
					if (pawn != victim && pawn.Ideo != null)
					{
						Pawn_NeedsTracker needs = pawn.needs;
						bool flag;
						if (needs == null)
						{
							flag = (null != null);
						}
						else
						{
							Need_Mood mood = needs.mood;
							flag = (((mood != null) ? mood.thoughts : null) != null);
						}
						if (flag && pawn.Ideo.IsVeneratedAnimal(victim))
						{
							Thought_TameVeneratedAnimalDied thought_TameVeneratedAnimalDied = (Thought_TameVeneratedAnimalDied)ThoughtMaker.MakeThought(ThoughtDefOf.TameVeneratedAnimalDied);
							thought_TameVeneratedAnimalDied.animalKindLabel = victim.KindLabel;
							foreach (Precept precept in pawn.Ideo.PreceptsListForReading)
							{
								Precept_Animal precept_Animal = precept as Precept_Animal;
								if (precept_Animal != null && precept_Animal.ThingDef == victim.def)
								{
									thought_TameVeneratedAnimalDied.sourcePrecept = precept;
									break;
								}
							}
							pawn.needs.mood.thoughts.memories.TryGainMemory(thought_TameVeneratedAnimalDied, null);
						}
					}
				}
			}
		}

		// Token: 0x0600C3A8 RID: 50088 RVA: 0x0038CFD8 File Offset: 0x0038B1D8
		[CompilerGenerated]
		internal static void <AppendThoughts_Relations>g__GiveThoughtsForAnimalBond|8_0(ThoughtDef thoughtDef, ref PawnDiedOrDownedThoughtsUtility.<>c__DisplayClass8_0 A_1)
		{
			List<DirectPawnRelation> directRelations = A_1.victim.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].otherPawn.needs != null && directRelations[i].otherPawn.needs.mood != null && PawnUtility.ShouldGetThoughtAbout(directRelations[i].otherPawn, A_1.victim) && directRelations[i].def == PawnRelationDefOf.Bond)
				{
					A_1.outIndividualThoughts.Add(new IndividualThoughtToAdd(thoughtDef, directRelations[i].otherPawn, A_1.victim, 1f, 1f));
				}
			}
		}

		// Token: 0x04008548 RID: 34120
		private static List<IndividualThoughtToAdd> tmpIndividualThoughtsToAdd = new List<IndividualThoughtToAdd>();

		// Token: 0x04008549 RID: 34121
		private static List<ThoughtToAddToAll> tmpAllColonistsThoughts = new List<ThoughtToAddToAll>();
	}
}
