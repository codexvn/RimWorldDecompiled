using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002622 RID: 9762
	public static class IdeoUtility
	{
		// Token: 0x0600D5EA RID: 54762 RVA: 0x003F76EC File Offset: 0x003F58EC
		public static void Notify_HistoryEvent(HistoryEvent ev, bool canApplySelfTookThoughts = true)
		{
			IdeoUtility.<>c__DisplayClass1_0 CS$<>8__locals1;
			CS$<>8__locals1.ev = ev;
			if (CS$<>8__locals1.ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out CS$<>8__locals1.pawn))
			{
				if (CS$<>8__locals1.pawn.Ideo != null)
				{
					CS$<>8__locals1.pawn.Ideo.Notify_MemberTookAction(CS$<>8__locals1.ev, canApplySelfTookThoughts);
				}
				if (CS$<>8__locals1.pawn.IsCaravanMember())
				{
					Caravan caravan = CS$<>8__locals1.pawn.GetCaravan();
					for (int i = 0; i < caravan.pawns.Count; i++)
					{
						IdeoUtility.<Notify_HistoryEvent>g__CheckKnows|1_0(caravan.pawns[i], ref CS$<>8__locals1);
					}
				}
				else if (CS$<>8__locals1.pawn.Spawned)
				{
					IReadOnlyList<Pawn> allPawnsSpawned = CS$<>8__locals1.pawn.Map.mapPawns.AllPawnsSpawned;
					for (int j = 0; j < allPawnsSpawned.Count; j++)
					{
						IdeoUtility.<Notify_HistoryEvent>g__CheckKnows|1_0(allPawnsSpawned[j], ref CS$<>8__locals1);
					}
				}
			}
			else
			{
				List<Pawn> allMapsCaravansAndTravellingTransporters_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists;
				for (int k = 0; k < allMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Count; k++)
				{
					Pawn pawn = allMapsCaravansAndTravellingTransporters_Alive_FreeColonists[k];
					if (pawn.Ideo != null)
					{
						pawn.Ideo.Notify_MemberKnows(CS$<>8__locals1.ev, pawn);
					}
				}
			}
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
				for (int l = 0; l < preceptsListForReading.Count; l++)
				{
					preceptsListForReading[l].Notify_HistoryEvent(CS$<>8__locals1.ev);
				}
			}
		}

		// Token: 0x0600D5EB RID: 54763 RVA: 0x003F7898 File Offset: 0x003F5A98
		public static bool DoerWillingToDo(this HistoryEvent ev)
		{
			Pawn arg = ev.args.GetArg<Pawn>(HistoryEventArgsNames.Doer);
			return ((arg != null) ? arg.Ideo : null) == null || arg.Ideo.MemberWillingToDo(ev);
		}

		// Token: 0x0600D5EC RID: 54764 RVA: 0x003F78D3 File Offset: 0x003F5AD3
		public static bool DoerWillingToDo(HistoryEventDef def, Pawn pawn)
		{
			return new HistoryEvent(def, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo();
		}

		// Token: 0x0600D5ED RID: 54765 RVA: 0x003F78EC File Offset: 0x003F5AEC
		public static bool Notify_PawnAboutToDo(this HistoryEvent ev, string messageKey = "MessagePawnUnwillingToDoDueToIdeo")
		{
			if (!ev.DoerWillingToDo())
			{
				Pawn arg = ev.args.GetArg<Pawn>(HistoryEventArgsNames.Doer);
				Messages.Message(messageKey.Translate(arg), arg, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			return true;
		}

		// Token: 0x0600D5EE RID: 54766 RVA: 0x003F7938 File Offset: 0x003F5B38
		public static bool Notify_PawnAboutToDo(this HistoryEvent ev, out FloatMenuOption opt, string baseText)
		{
			if (!ev.DoerWillingToDo())
			{
				opt = new FloatMenuOption(baseText + ": " + "IdeoligionForbids".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return false;
			}
			opt = null;
			return true;
		}

		// Token: 0x0600D5EF RID: 54767 RVA: 0x003F7986 File Offset: 0x003F5B86
		public static bool Notify_PawnAboutToDo_Job(this HistoryEvent ev)
		{
			if (!ev.DoerWillingToDo())
			{
				JobFailReason.Is("IdeoligionForbids".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x0600D5F0 RID: 54768 RVA: 0x003F79A8 File Offset: 0x003F5BA8
		public static List<MemeDef> GenerateRandomMemes(IdeoGenerationParms parms)
		{
			return IdeoUtility.GenerateRandomMemes(parms.forNewFluidIdeo ? IdeoFoundation.MemeCountRangeFluidAbsolute.RandomInRange : IdeoFoundation.MemeCountRangeNPCInitial.RandomInRange, parms);
		}

		// Token: 0x0600D5F1 RID: 54769 RVA: 0x003F79E0 File Offset: 0x003F5BE0
		private static bool CanAdd(MemeDef meme, List<MemeDef> memes, FactionDef forFaction = null, bool forNewFluidIdeo = false)
		{
			if (memes.Contains(meme))
			{
				return false;
			}
			if (forFaction != null && !IdeoUtility.IsMemeAllowedFor(meme, forFaction))
			{
				return false;
			}
			if (forNewFluidIdeo && !IdeoUtility.IsMemeAllowedForInitialFluidIdeo(meme))
			{
				return false;
			}
			for (int i = 0; i < memes.Count; i++)
			{
				for (int j = 0; j < meme.exclusionTags.Count; j++)
				{
					if (memes[i].exclusionTags.Contains(meme.exclusionTags[j]))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600D5F2 RID: 54770 RVA: 0x003F7A5C File Offset: 0x003F5C5C
		public static List<MemeDef> GenerateRandomMemes(int count, IdeoGenerationParms parms)
		{
			FactionDef forFaction = parms.forFaction;
			bool forPlayerFaction = forFaction != null && forFaction.isPlayer;
			List<MemeDef> memes = new List<MemeDef>();
			bool flag = false;
			if (forFaction != null && forFaction.requiredMemes != null)
			{
				for (int i = 0; i < forFaction.requiredMemes.Count; i++)
				{
					memes.Add(forFaction.requiredMemes[i]);
					if (forFaction.requiredMemes[i].category == MemeCategory.Normal)
					{
						count--;
					}
					else if (forFaction.requiredMemes[i].category == MemeCategory.Structure)
					{
						flag = true;
					}
				}
			}
			if (parms.forcedMemes != null)
			{
				using (List<MemeDef>.Enumerator enumerator = parms.forcedMemes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.category == MemeCategory.Structure)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (forFaction != null && forFaction.structureMemeWeights != null && !flag)
			{
				MemeWeight memeWeight;
				MemeWeight memeWeight2;
				if ((from x in forFaction.structureMemeWeights
				where IdeoUtility.CanAdd(x.meme, memes, forFaction, parms.forNewFluidIdeo) && (forPlayerFaction || !IdeoUtility.AnyIdeoHas(x.meme))
				select x).TryRandomElementByWeight((MemeWeight x) => x.selectionWeight * x.meme.randomizationSelectionWeightFactor, out memeWeight))
				{
					memes.Add(memeWeight.meme);
					flag = true;
				}
				else if ((from x in forFaction.structureMemeWeights
				where IdeoUtility.CanAdd(x.meme, memes, forFaction, parms.forNewFluidIdeo)
				select x).TryRandomElementByWeight((MemeWeight x) => x.selectionWeight * x.meme.randomizationSelectionWeightFactor, out memeWeight2))
				{
					memes.Add(memeWeight2.meme);
					flag = true;
				}
			}
			if (!flag)
			{
				MemeDef item;
				MemeDef item2;
				if ((from x in DefDatabase<MemeDef>.AllDefs
				where x.category == MemeCategory.Structure && IdeoUtility.CanAdd(x, memes, forFaction, parms.forNewFluidIdeo) && (forPlayerFaction || !IdeoUtility.AnyIdeoHas(x))
				select x).TryRandomElement(out item))
				{
					memes.Add(item);
				}
				else if ((from x in DefDatabase<MemeDef>.AllDefs
				where x.category == MemeCategory.Structure && IdeoUtility.CanAdd(x, memes, forFaction, parms.forNewFluidIdeo)
				select x).TryRandomElementByWeight((MemeDef x) => x.randomizationSelectionWeightFactor, out item2))
				{
					memes.Add(item2);
				}
			}
			if (parms.forcedMemes != null)
			{
				memes.AddRange(parms.forcedMemes);
				return memes;
			}
			Func<MemeDef, bool> <>9__7;
			Func<MemeDef, bool> <>9__9;
			for (int j = 0; j < count; j++)
			{
				IEnumerable<MemeDef> allDefs = DefDatabase<MemeDef>.AllDefs;
				Func<MemeDef, bool> predicate;
				if ((predicate = <>9__7) == null)
				{
					predicate = (<>9__7 = ((MemeDef x) => x.category == MemeCategory.Normal && IdeoUtility.CanAdd(x, memes, forFaction, parms.forNewFluidIdeo) && (forPlayerFaction || !IdeoUtility.AnyIdeoHas(x)) && (parms.disallowedMemes == null || !parms.disallowedMemes.Contains(x))));
				}
				MemeDef item3;
				if (allDefs.Where(predicate).TryRandomElementByWeight((MemeDef x) => x.randomizationSelectionWeightFactor, out item3))
				{
					memes.Add(item3);
				}
				else
				{
					IEnumerable<MemeDef> allDefs2 = DefDatabase<MemeDef>.AllDefs;
					Func<MemeDef, bool> predicate2;
					if ((predicate2 = <>9__9) == null)
					{
						predicate2 = (<>9__9 = ((MemeDef x) => x.category == MemeCategory.Normal && IdeoUtility.CanAdd(x, memes, forFaction, parms.forNewFluidIdeo) && (parms.disallowedMemes == null || !parms.disallowedMemes.Contains(x))));
					}
					MemeDef item4;
					if (allDefs2.Where(predicate2).TryRandomElementByWeight((MemeDef x) => x.randomizationSelectionWeightFactor, out item4))
					{
						memes.Add(item4);
					}
				}
			}
			return memes;
		}

		// Token: 0x0600D5F3 RID: 54771 RVA: 0x003F7DEC File Offset: 0x003F5FEC
		public static List<MemeDef> RandomizeNormalMemes(int count, List<MemeDef> previousMemes, FactionDef forFaction = null, bool forNewFluidIdeo = false)
		{
			List<MemeDef> memes = new List<MemeDef>();
			foreach (MemeDef memeDef in previousMemes)
			{
				if (memeDef.category != MemeCategory.Normal)
				{
					memes.Add(memeDef);
				}
			}
			if (forFaction != null && forFaction.requiredMemes != null)
			{
				for (int i = 0; i < forFaction.requiredMemes.Count; i++)
				{
					MemeDef memeDef2 = forFaction.requiredMemes[i];
					if (memeDef2.category == MemeCategory.Normal)
					{
						memes.Add(memeDef2);
						count--;
					}
				}
			}
			Func<MemeDef, bool> <>9__0;
			for (int j = 0; j < count; j++)
			{
				IEnumerable<MemeDef> allDefs = DefDatabase<MemeDef>.AllDefs;
				Func<MemeDef, bool> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((MemeDef x) => x.category == MemeCategory.Normal && IdeoUtility.CanAdd(x, memes, forFaction, forNewFluidIdeo)));
				}
				MemeDef item;
				if (allDefs.Where(predicate).TryRandomElement(out item))
				{
					memes.Add(item);
				}
			}
			return memes;
		}

		// Token: 0x0600D5F4 RID: 54772 RVA: 0x003F7F1C File Offset: 0x003F611C
		public static List<MemeDef> RandomizeNormalMemesForReforming(int maxMemes, List<MemeDef> existingMemes, FactionDef forFaction = null)
		{
			List<MemeDef> memes = new List<MemeDef>();
			memes.AddRange(existingMemes);
			int num = memes.Count((MemeDef m) => m.category == MemeCategory.Normal);
			MemeDef item;
			if (num > 1 && (num >= maxMemes || Rand.Bool))
			{
				memes.Remove((from m in memes
				where m.category == MemeCategory.Normal
				select m).RandomElement<MemeDef>());
			}
			else if (num < maxMemes && (from x in DefDatabase<MemeDef>.AllDefs
			where x.category == MemeCategory.Normal && IdeoUtility.CanAdd(x, memes, forFaction, false)
			select x).TryRandomElement(out item))
			{
				memes.Add(item);
			}
			return memes;
		}

		// Token: 0x0600D5F5 RID: 54773 RVA: 0x003F7FFC File Offset: 0x003F61FC
		public static List<MemeDef> RandomizeStructureMeme(List<MemeDef> previousMemes, FactionDef forFaction = null)
		{
			List<MemeDef> memes = new List<MemeDef>();
			foreach (MemeDef memeDef in previousMemes)
			{
				if (memeDef.category != MemeCategory.Structure)
				{
					memes.Add(memeDef);
				}
			}
			bool flag = false;
			if (forFaction != null && forFaction.requiredMemes != null)
			{
				for (int i = 0; i < forFaction.requiredMemes.Count; i++)
				{
					MemeDef memeDef2 = forFaction.requiredMemes[i];
					if (memeDef2.category == MemeCategory.Structure)
					{
						memes.Add(memeDef2);
						flag = true;
					}
				}
			}
			MemeDef item;
			if (!flag && (from x in DefDatabase<MemeDef>.AllDefs
			where x.category == MemeCategory.Structure && IdeoUtility.CanAdd(x, memes, forFaction, false)
			select x).TryRandomElement(out item))
			{
				memes.Add(item);
			}
			return memes;
		}

		// Token: 0x0600D5F6 RID: 54774 RVA: 0x003F810C File Offset: 0x003F630C
		private static bool AnyIdeoHas(MemeDef meme)
		{
			if (Find.World == null)
			{
				return false;
			}
			List<Ideo> ideosListForReading = Find.IdeoManager.IdeosListForReading;
			for (int i = 0; i < ideosListForReading.Count; i++)
			{
				if (ideosListForReading[i].memes.Contains(meme))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D5F7 RID: 54775 RVA: 0x003F8158 File Offset: 0x003F6358
		public static bool CanUseIdeo(FactionDef factionDef, Ideo ideo, IdeoGenerationParms parms)
		{
			if (factionDef.allowedCultures != null && !factionDef.allowedCultures.Contains(ideo.culture))
			{
				return false;
			}
			if (factionDef.requiredMemes != null)
			{
				for (int i = 0; i < factionDef.requiredMemes.Count; i++)
				{
					if (!ideo.memes.Contains(factionDef.requiredMemes[i]))
					{
						return false;
					}
				}
			}
			for (int j = 0; j < ideo.memes.Count; j++)
			{
				if (!IdeoUtility.IsMemeAllowedFor(ideo.memes[j], factionDef))
				{
					return false;
				}
			}
			Predicate<PreceptApparelRequirement> <>9__5;
			return (parms.disallowedPrecepts == null || !ideo.PreceptsListForReading.Any((Precept p) => parms.disallowedPrecepts.Contains(p.def))) && (parms.forcedMemes == null || ideo.memes.Any(new Predicate<MemeDef>(parms.forcedMemes.Contains))) && (parms.disallowedMemes == null || !ideo.memes.Any(new Predicate<MemeDef>(parms.disallowedMemes.Contains))) && (parms.styles == null || parms.styles.All((StyleCategoryDef style) => ideo.thingStyleCategories.Any((ThingStyleCategoryWithPriority inner) => inner.category == style))) && !ideo.PreceptsListForReading.OfType<Precept_Ritual>().Any((Precept_Ritual p) => !RitualPatternDef.CanUseWithTechLevel(factionDef.techLevel, p.minTechLevel, p.maxTechLevel)) && !ideo.PreceptsListForReading.OfType<Precept_Role>().Any(delegate(Precept_Role p)
			{
				if (!p.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
				{
					List<PreceptApparelRequirement> apparelRequirements = p.apparelRequirements;
					Predicate<PreceptApparelRequirement> predicate;
					if ((predicate = <>9__5) == null)
					{
						predicate = (<>9__5 = ((PreceptApparelRequirement req) => !req.Compatible(ideo, factionDef)));
					}
					return apparelRequirements.Any(predicate);
				}
				return false;
			});
		}

		// Token: 0x0600D5F8 RID: 54776 RVA: 0x003F834C File Offset: 0x003F654C
		public static bool IsMemeAllowedFor(MemeDef meme, FactionDef faction)
		{
			return (faction.structureMemeWeights != null && meme.category == MemeCategory.Structure && faction.structureMemeWeights.Any((MemeWeight x) => x.meme == meme && x.selectionWeight != 0f)) || ((meme.category != MemeCategory.Normal || meme.allowDuringTutorial || !faction.classicIdeo) && (faction.disallowedMemes == null || !faction.disallowedMemes.Contains(meme)) && ((faction.requiredMemes != null && faction.requiredMemes.Contains(meme)) || ((faction.allowedMemes == null || faction.allowedMemes.Contains(meme)) && (meme.factionWhitelist == null || meme.factionWhitelist.Contains(faction)))));
		}

		// Token: 0x0600D5F9 RID: 54777 RVA: 0x003F8438 File Offset: 0x003F6638
		public static bool PlayerHasPreceptForBuilding(ThingDef buildingDef)
		{
			using (IEnumerator<Ideo> enumerator = Faction.OfPlayer.ideos.AllIdeos.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.HasPreceptForBuilding(buildingDef))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600D5FA RID: 54778 RVA: 0x003F8498 File Offset: 0x003F6698
		public static IEnumerable<Pawn> AllColonistsWithCharityPrecept()
		{
			if (!ModsConfig.IdeologyActive)
			{
				yield break;
			}
			List<Pawn> colonists = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists;
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				List<Precept> precepts = ideo.PreceptsListForReading;
				int num;
				for (int i = 0; i < precepts.Count; i = num + 1)
				{
					if (precepts[i].def.issue == IssueDefOf.Charity)
					{
						for (int j = 0; j < colonists.Count; j = num + 1)
						{
							if (colonists[j].Ideo == ideo)
							{
								yield return colonists[j];
							}
							num = j;
						}
					}
					num = i;
				}
				precepts = null;
				ideo = null;
			}
			IEnumerator<Ideo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600D5FB RID: 54779 RVA: 0x003F84A4 File Offset: 0x003F66A4
		public static bool AnyColonistWithRanchingIssue()
		{
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
				for (int i = 0; i < preceptsListForReading.Count; i++)
				{
					if (preceptsListForReading[i].def.issue == IssueDefOf.Ranching)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600D5FC RID: 54780 RVA: 0x003F8528 File Offset: 0x003F6728
		public static void Notify_PlayerRaidedSomeone(IEnumerable<Pawn> allRaiders)
		{
			if (allRaiders.EnumerableNullOrEmpty<Pawn>())
			{
				return;
			}
			allRaiders = from p in allRaiders
			where !p.Dead
			select p;
			Find.History.Notify_PlayerRaidedSomeone();
			if (ModsConfig.IdeologyActive)
			{
				foreach (Pawn arg in allRaiders)
				{
					HistoryEvent historyEvent = new HistoryEvent(HistoryEventDefOf.Raided, arg.Named(HistoryEventArgsNames.Doer));
					Find.HistoryEventsManager.RecordEvent(historyEvent, true);
				}
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.PlayerRaidedSomeone), true);
			}
		}

		// Token: 0x0600D5FD RID: 54781 RVA: 0x003F85E4 File Offset: 0x003F67E4
		public static void Notify_QuestCleanedUp(Quest quest, QuestState state)
		{
			if (state == QuestState.EndedOfferExpired || state == QuestState.EndedFailed)
			{
				if (quest.root.failedOrExpiredHistoryEvent != null)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(quest.root.failedOrExpiredHistoryEvent), true);
				}
				if (quest.charity && ModsConfig.IdeologyActive && IdeoUtility.AllColonistsWithCharityPrecept().Any<Pawn>())
				{
					TaggedString taggedString = "MessageCharityEventRefused".Translate();
					if (!quest.hidden)
					{
						taggedString += ": " + "MessageCharityQuestEndedFailed".Translate(quest.name);
					}
					Messages.Message(taggedString, null, MessageTypeDefOf.NegativeEvent, (!quest.hidden) ? quest : null, true);
					return;
				}
			}
			else if (state == QuestState.EndedSuccess)
			{
				if (quest.root.successHistoryEvent != null)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(quest.root.successHistoryEvent, quest.Named(HistoryEventArgsNames.Quest)), true);
				}
				if (quest.charity && ModsConfig.IdeologyActive && IdeoUtility.AllColonistsWithCharityPrecept().Any<Pawn>())
				{
					TaggedString taggedString2 = "MessageCharityEventFulfilled".Translate();
					if (!quest.hidden)
					{
						taggedString2 += ": " + "MessageCharityQuestEndedSuccess".Translate(quest.name);
					}
					Messages.Message(taggedString2, null, MessageTypeDefOf.PositiveEvent, (!quest.hidden) ? quest : null, true);
				}
			}
		}

		// Token: 0x0600D5FE RID: 54782 RVA: 0x003F874C File Offset: 0x003F694C
		public static float GetStyleDominanceFromCellsCenteredOn(IntVec3 center, IntVec3 rootCell, Map map, Ideo ideo)
		{
			IdeoUtility.<>c__DisplayClass22_0 CS$<>8__locals1;
			CS$<>8__locals1.map = map;
			bool flag = false;
			float num = 0f;
			IdeoUtility.GetJoinedRooms(rootCell, CS$<>8__locals1.map, IdeoUtility.tmpCheckRooms);
			if (!IdeoUtility.tmpCheckRooms.Any<Room>())
			{
				return num;
			}
			int num2 = GenRadial.NumCellsInRadius(24.9f);
			for (int i = 0; i < num2; i++)
			{
				IntVec3 c = center + GenRadial.RadialPattern[i];
				if (IdeoUtility.<GetStyleDominanceFromCellsCenteredOn>g__CellIsValid|22_0(c, ref CS$<>8__locals1))
				{
					if (flag)
					{
						CS$<>8__locals1.map.debugDrawer.FlashCell(c, 0.1f, "d", 50);
					}
					TerrainDef terrain = c.GetTerrain(CS$<>8__locals1.map);
					if (ideo.cachedPossibleBuildables.Contains(terrain))
					{
						num += terrain.GetStatValueAbstract(StatDefOf.StyleDominance, null);
					}
					else if (!terrain.canGenerateDefaultDesignator)
					{
						num -= terrain.GetStatValueAbstract(StatDefOf.StyleDominance, null);
					}
					foreach (Thing t in c.GetThingList(CS$<>8__locals1.map))
					{
						num += IdeoUtility.GetStyleDominance(t, ideo);
					}
				}
			}
			IdeoUtility.tmpCheckRooms.Clear();
			return num;
		}

		// Token: 0x0600D5FF RID: 54783 RVA: 0x003F8894 File Offset: 0x003F6A94
		private static void GetJoinedRooms(IntVec3 rootCell, Map map, HashSet<Room> rooms)
		{
			Room room = rootCell.GetRoom(map);
			if (room == null)
			{
				return;
			}
			rooms.Clear();
			rooms.Add(room);
			if (room.IsDoorway)
			{
				foreach (Region region in room.FirstRegion.Neighbors)
				{
					if (!rooms.Contains(region.Room))
					{
						rooms.Add(region.Room);
					}
				}
			}
		}

		// Token: 0x0600D600 RID: 54784 RVA: 0x003F891C File Offset: 0x003F6B1C
		public static bool ThingSatisfiesIdeo(Thing thing, Ideo ideo)
		{
			return IdeoUtility.GetStyleDominance(thing, ideo) > 0f;
		}

		// Token: 0x0600D601 RID: 54785 RVA: 0x003F892C File Offset: 0x003F6B2C
		private static float GetStyleDominance(Thing t, Ideo ideo)
		{
			float statValue = t.GetStatValue(StatDefOf.StyleDominance, true, -1);
			CompRelicContainer compRelicContainer = t.TryGetComp<CompRelicContainer>();
			if (compRelicContainer != null && compRelicContainer.Full)
			{
				ThingWithComps thingWithComps = compRelicContainer.ContainedThing as ThingWithComps;
				CompStyleable compStyleable = (thingWithComps != null) ? thingWithComps.compStyleable : null;
				bool flag;
				if (compStyleable == null)
				{
					flag = (null != null);
				}
				else
				{
					Precept_ThingStyle sourcePrecept = compStyleable.SourcePrecept;
					flag = (((sourcePrecept != null) ? sourcePrecept.ideo : null) != null);
				}
				if (flag)
				{
					if (compStyleable.SourcePrecept.ideo == ideo)
					{
						return statValue;
					}
					return -statValue;
				}
			}
			ThingStyleDef styleDef = t.GetStyleDef();
			if (styleDef != null)
			{
				if (ideo.GetStyleFor(t.def) == styleDef)
				{
					return statValue;
				}
				return -statValue;
			}
			else
			{
				if (t.def.canGenerateDefaultDesignator)
				{
					return 0f;
				}
				if (ideo.cachedPossibleBuildables.Contains(t.def) || (t.StyleSourcePrecept != null && ideo.PreceptsListForReading.Contains(t.StyleSourcePrecept)))
				{
					return statValue;
				}
				return -statValue;
			}
		}

		// Token: 0x0600D602 RID: 54786 RVA: 0x003F8A04 File Offset: 0x003F6C04
		public static List<TreeSighting> TreeSightingsNearPawn(IntVec3 rootCell, Map map, Ideo ideo)
		{
			IdeoUtility.tmpTreeSightings.Clear();
			int num = GenRadial.NumCellsInRadius(11.9f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = rootCell + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && !intVec.Fogged(map) && GenSight.LineOfSight(rootCell, intVec, map))
				{
					foreach (Thing thing in intVec.GetThingList(map))
					{
						Plant plant = thing as Plant;
						if (plant != null && plant.def.plant.IsTree)
						{
							IdeoUtility.tmpTreeSightings.Add(new TreeSighting(thing, Find.TickManager.TicksGame));
						}
					}
				}
			}
			return IdeoUtility.tmpTreeSightings;
		}

		// Token: 0x0600D603 RID: 54787 RVA: 0x003F8AE8 File Offset: 0x003F6CE8
		public static bool IdeoCausesHumanMeatCravings(this Ideo ideo)
		{
			return ideo.cachedPossibleSituationalThoughts.Contains(ThoughtDefOf.NoRecentHumanMeat_Preferred) || ideo.cachedPossibleSituationalThoughts.Contains(ThoughtDefOf.NoRecentHumanMeat_RequiredStrong) || ideo.cachedPossibleSituationalThoughts.Contains(ThoughtDefOf.NoRecentHumanMeat_RequiredRavenous);
		}

		// Token: 0x0600D604 RID: 54788 RVA: 0x003F8B20 File Offset: 0x003F6D20
		public static bool IdeoPrefersNudity(this Ideo ideo)
		{
			using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.prefersNudity)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600D605 RID: 54789 RVA: 0x003F8B80 File Offset: 0x003F6D80
		public static bool IdeoPrefersNudityForGender(this Ideo ideo, Gender gender)
		{
			foreach (Precept precept in ideo.PreceptsListForReading)
			{
				if (precept.def.prefersNudity && (precept.def.genderPrefersNudity == Gender.None || precept.def.genderPrefersNudity == gender))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D606 RID: 54790 RVA: 0x003F8BFC File Offset: 0x003F6DFC
		public static bool IdeoApprovesOfSlavery(this Ideo ideo)
		{
			using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.approvesOfSlavery)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600D607 RID: 54791 RVA: 0x003F8C5C File Offset: 0x003F6E5C
		public static bool IdeoPrefersDarkness(this Ideo ideo)
		{
			using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.prefersDarkness)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600D608 RID: 54792 RVA: 0x003F8CBC File Offset: 0x003F6EBC
		public static bool IdeoApprovesOfBlindness(this Ideo ideo)
		{
			using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.approvesOfBlindness)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600D609 RID: 54793 RVA: 0x003F8D1C File Offset: 0x003F6F1C
		public static void Notify_NewColonyStarted()
		{
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				ideo.relicsCollected = false;
			}
			foreach (Ideo ideo2 in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept precept in ideo2.PreceptsListForReading)
				{
					Precept_Relic precept_Relic = precept as Precept_Relic;
					if (precept_Relic != null)
					{
						precept_Relic.Notify_NewColonyStarted();
					}
				}
			}
		}

		// Token: 0x0600D60A RID: 54794 RVA: 0x003F8DF8 File Offset: 0x003F6FF8
		public static Pawn FindFirstPawnWithLeaderRole(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = 0; i < pawnsListForReading.Count; i++)
			{
				Pawn pawn = pawnsListForReading[i];
				if (pawn.Ideo != null)
				{
					Precept_Role role = pawn.Ideo.GetRole(pawn);
					if (role != null && role.def.leaderRole)
					{
						return pawn;
					}
				}
			}
			return null;
		}

		// Token: 0x0600D60B RID: 54795 RVA: 0x003F8E4D File Offset: 0x003F704D
		public static Ideo MakeEmptyIdeo()
		{
			Ideo ideo = IdeoGenerator.MakeIdeo(DefDatabase<IdeoFoundationDef>.AllDefs.RandomElement<IdeoFoundationDef>());
			ideo.foundation.RandomizeIcon();
			return ideo;
		}

		// Token: 0x0600D60C RID: 54796 RVA: 0x003F8E6C File Offset: 0x003F706C
		public static Color? GetIdeoColorForBuilding(ThingDef def, Faction faction)
		{
			if (def.building.useIdeoColor && faction != null && faction.ideos != null)
			{
				Ideo ideo = null;
				Predicate<ThingStyleCategoryWithPriority> <>9__0;
				foreach (Ideo ideo2 in faction.ideos.AllIdeos)
				{
					if (def.dominantStyleCategory != null)
					{
						List<ThingStyleCategoryWithPriority> thingStyleCategories = ideo2.thingStyleCategories;
						Predicate<ThingStyleCategoryWithPriority> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((ThingStyleCategoryWithPriority sc) => sc.category == def.dominantStyleCategory));
						}
						if (thingStyleCategories.Any(predicate))
						{
							ideo = ideo2;
							break;
						}
					}
					using (List<MemeDef>.Enumerator enumerator2 = ideo2.memes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.AllDesignatorBuildables.Contains(def))
							{
								ideo = ideo2;
								break;
							}
						}
					}
					if (ideo != null)
					{
						break;
					}
				}
				ideo = (ideo ?? faction.ideos.PrimaryIdeo);
				if (ideo != null)
				{
					return new Color?(ideo.Color);
				}
			}
			return null;
		}

		// Token: 0x0600D60D RID: 54797 RVA: 0x003F8FB4 File Offset: 0x003F71B4
		public static bool IsMemeAllowedForInitialFluidIdeo(MemeDef memeDef)
		{
			return memeDef.impact <= 2;
		}

		// Token: 0x0600D60E RID: 54798 RVA: 0x003F8FC2 File Offset: 0x003F71C2
		public static bool CountsAsNonNPCForPrecepts(this Pawn pawn)
		{
			return pawn.IsColonist || pawn.IsPrisonerOfColony || pawn.IsSlaveOfColony;
		}

		// Token: 0x0600D60F RID: 54799 RVA: 0x003F8FDC File Offset: 0x003F71DC
		public static float IdeoChangeToWeight(Pawn pawn, Ideo ideo)
		{
			if (ModsConfig.AnomalyActive && ideo == Find.IdeoManager.Horaxian)
			{
				return 0f;
			}
			if (pawn == null)
			{
				return 1f;
			}
			if (ideo == pawn.Ideo)
			{
				return 0f;
			}
			float num = 1f;
			if (pawn.Faction != null)
			{
				foreach (Pawn pawn2 in PawnsFinder.AllMaps_SpawnedPawnsInFaction(pawn.Faction))
				{
					if (pawn2.ideo != null && pawn2.Ideo == ideo)
					{
						num += 1f;
						break;
					}
				}
				foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
				{
					if (faction != pawn.Faction && faction.RelationKindWith(pawn.Faction) == FactionRelationKind.Ally && faction.ideos.IsPrimary(ideo))
					{
						num += 1f;
						break;
					}
				}
			}
			if (pawn.Spawned)
			{
				using (IEnumerator<Pawn> enumerator3 = pawn.Map.mapPawns.AllPawnsSpawned.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Pawn pawn3 = enumerator3.Current;
						if (pawn3.Faction != null && pawn3.ideo != null && pawn3.Ideo == ideo && (pawn3.Faction == pawn.Faction || !pawn3.Faction.HostileTo(pawn.Faction)))
						{
							num += 1f;
							break;
						}
					}
					return num;
				}
			}
			Caravan caravan = pawn.GetCaravan();
			if (caravan != null)
			{
				foreach (Pawn pawn4 in caravan.PawnsListForReading)
				{
					if (pawn4.ideo != null && pawn4.Ideo == ideo)
					{
						num += 1f;
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x0600D611 RID: 54801 RVA: 0x003F920E File Offset: 0x003F740E
		[CompilerGenerated]
		internal static void <Notify_HistoryEvent>g__CheckKnows|1_0(Pawn p, ref IdeoUtility.<>c__DisplayClass1_0 A_1)
		{
			if (p != A_1.pawn && p.Ideo != null)
			{
				p.Ideo.Notify_MemberKnows(A_1.ev, p);
			}
		}

		// Token: 0x0600D612 RID: 54802 RVA: 0x003F9234 File Offset: 0x003F7434
		[CompilerGenerated]
		internal static bool <GetStyleDominanceFromCellsCenteredOn>g__CellIsValid|22_0(IntVec3 c, ref IdeoUtility.<>c__DisplayClass22_0 A_1)
		{
			if (!c.InBounds(A_1.map) || c.Fogged(A_1.map))
			{
				return false;
			}
			Room room = c.GetRoom(A_1.map);
			return room != null && IdeoUtility.tmpCheckRooms.Contains(room);
		}

		// Token: 0x0400917C RID: 37244
		public const int MinFluidIdeoNormalMemes = 1;

		// Token: 0x0400917D RID: 37245
		private static HashSet<Room> tmpCheckRooms = new HashSet<Room>();

		// Token: 0x0400917E RID: 37246
		private static List<TreeSighting> tmpTreeSightings = new List<TreeSighting>();
	}
}
