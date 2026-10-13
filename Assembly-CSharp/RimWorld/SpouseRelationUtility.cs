using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002434 RID: 9268
	public static class SpouseRelationUtility
	{
		// Token: 0x0600C87E RID: 51326 RVA: 0x003A8EA2 File Offset: 0x003A70A2
		public static Pawn GetFirstSpouse(this Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			return pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
		}

		// Token: 0x0600C87F RID: 51327 RVA: 0x003A8EC4 File Offset: 0x003A70C4
		public static List<Pawn> GetSpouses(this Pawn pawn, bool includeDead)
		{
			SpouseRelationUtility.tmpSpouses.Clear();
			if (!pawn.RaceProps.IsFlesh)
			{
				return SpouseRelationUtility.tmpSpouses;
			}
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].def == PawnRelationDefOf.Spouse && (includeDead || !directRelations[i].otherPawn.Dead))
				{
					SpouseRelationUtility.tmpSpouses.Add(directRelations[i].otherPawn);
				}
			}
			return SpouseRelationUtility.tmpSpouses;
		}

		// Token: 0x0600C880 RID: 51328 RVA: 0x003A8F50 File Offset: 0x003A7150
		public static List<DirectPawnRelation> GetLoveRelations(this Pawn pawn, bool includeDead, bool orderByCommitmentLevel = false)
		{
			SpouseRelationUtility.tmpLoveRelations.Clear();
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def) && (includeDead || !directRelations[i].otherPawn.Dead))
				{
					SpouseRelationUtility.tmpLoveRelations.Add(directRelations[i]);
				}
			}
			if (orderByCommitmentLevel)
			{
				SpouseRelationUtility.tmpLoveRelations.SortBy(delegate(DirectPawnRelation r)
				{
					if (r.def != PawnRelationDefOf.Spouse)
					{
						return int.MaxValue;
					}
					return -pawn.relations.OpinionOf(r.otherPawn);
				}, delegate(DirectPawnRelation r)
				{
					if (r.def != PawnRelationDefOf.Fiance)
					{
						return int.MaxValue;
					}
					return -pawn.relations.OpinionOf(r.otherPawn);
				}, delegate(DirectPawnRelation r)
				{
					if (r.def != PawnRelationDefOf.Lover)
					{
						return int.MaxValue;
					}
					return -pawn.relations.OpinionOf(r.otherPawn);
				});
			}
			return SpouseRelationUtility.tmpLoveRelations;
		}

		// Token: 0x0600C881 RID: 51329 RVA: 0x003A9008 File Offset: 0x003A7208
		public static DirectPawnRelation GetMostLikedSpouseRelation(this Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			DirectPawnRelation directPawnRelation = null;
			int num = int.MinValue;
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].def == PawnRelationDefOf.Spouse && !directRelations[i].otherPawn.Dead && (directPawnRelation == null || pawn.relations.OpinionOf(directRelations[i].otherPawn) > num))
				{
					directPawnRelation = directRelations[i];
					num = pawn.relations.OpinionOf(directRelations[i].otherPawn);
				}
			}
			return directPawnRelation;
		}

		// Token: 0x0600C882 RID: 51330 RVA: 0x003A90B0 File Offset: 0x003A72B0
		public static DirectPawnRelation GetLeastLikedSpouseRelation(this Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			DirectPawnRelation directPawnRelation = null;
			int num = int.MaxValue;
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].def == PawnRelationDefOf.Spouse && !directRelations[i].otherPawn.Dead && (directPawnRelation == null || pawn.relations.OpinionOf(directRelations[i].otherPawn) < num))
				{
					directPawnRelation = directRelations[i];
					num = pawn.relations.OpinionOf(directRelations[i].otherPawn);
				}
			}
			return directPawnRelation;
		}

		// Token: 0x0600C883 RID: 51331 RVA: 0x003A9158 File Offset: 0x003A7358
		public static int GetSpouseCount(this Pawn pawn, bool includeDead)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return 0;
			}
			return pawn.relations.GetDirectRelationsCount(PawnRelationDefOf.Spouse, (Pawn x) => includeDead || !x.Dead);
		}

		// Token: 0x0600C884 RID: 51332 RVA: 0x003A91A0 File Offset: 0x003A73A0
		public static List<Pawn> GetLoveCluster(this Pawn pawn)
		{
			SpouseRelationUtility.tmpLoveClusterPawns.Clear();
			SpouseRelationUtility.tmpLoveClusterPawns.Add(pawn);
			SpouseRelationUtility.tmpStack.Clear();
			SpouseRelationUtility.tmpStack.Push(pawn);
			int num = 200;
			while (SpouseRelationUtility.tmpStack.Count > 0)
			{
				List<DirectPawnRelation> loveRelations = SpouseRelationUtility.tmpStack.Pop().GetLoveRelations(false, false);
				for (int i = 0; i < loveRelations.Count; i++)
				{
					if (!SpouseRelationUtility.tmpLoveClusterPawns.Contains(loveRelations[i].otherPawn))
					{
						SpouseRelationUtility.tmpLoveClusterPawns.Add(loveRelations[i].otherPawn);
						SpouseRelationUtility.tmpStack.Push(loveRelations[i].otherPawn);
					}
				}
				num--;
				if (num <= 0)
				{
					Log.ErrorOnce("GetLoveCluster exceeded iterations limit.", 1462229);
					break;
				}
			}
			return SpouseRelationUtility.tmpLoveClusterPawns;
		}

		// Token: 0x0600C885 RID: 51333 RVA: 0x003A9278 File Offset: 0x003A7478
		public static void RemoveSpousesAsForbiddenByIdeo(Pawn pawn)
		{
			SpouseRelationUtility.tmpDivorcedPawnNames.Clear();
			HistoryEvent ev = new HistoryEvent(pawn.GetHistoryEventForSpouseCount(), pawn.Named(HistoryEventArgsNames.Doer));
			int num = 200;
			while (!ev.DoerWillingToDo())
			{
				DirectPawnRelation leastLikedSpouseRelation = pawn.GetLeastLikedSpouseRelation();
				if (leastLikedSpouseRelation == null)
				{
					break;
				}
				SpouseRelationUtility.DoDivorce(pawn, leastLikedSpouseRelation.otherPawn);
				SpouseRelationUtility.tmpDivorcedPawnNames.Add(leastLikedSpouseRelation.otherPawn.NameShortColored.Resolve());
				ev.def = pawn.GetHistoryEventForSpouseCount();
				num--;
				if (num <= 0)
				{
					Log.ErrorOnce("RemoveSpousesAsForbiddenByIdeo exceeded iterations limit.", 18483836);
					break;
				}
			}
			if (SpouseRelationUtility.tmpDivorcedPawnNames.Any<string>() && PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				Find.LetterStack.ReceiveLetter("LetterIdeoChangedDivorce".Translate() + ": " + pawn.LabelShortCap, "LetterIdeoChangedDivorcedPawns".Translate(pawn.Named("PAWN"), SpouseRelationUtility.tmpDivorcedPawnNames.ToLineList("  - ")), LetterDefOf.NeutralEvent, new LookTargets(pawn), null, null, null, null, 0, true);
			}
			SpouseRelationUtility.tmpDivorcedPawnNames.Clear();
		}

		// Token: 0x0600C886 RID: 51334 RVA: 0x003A9394 File Offset: 0x003A7594
		public static void DoDivorce(Pawn initiator, Pawn recipient)
		{
			initiator.relations.RemoveDirectRelation(PawnRelationDefOf.Spouse, recipient);
			initiator.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, recipient);
			SpouseRelationUtility.RemoveGotMarriedThoughts(initiator, recipient);
			if (initiator.ownership.OwnedBed != null && initiator.ownership.OwnedBed == recipient.ownership.OwnedBed)
			{
				((Rand.Value < 0.5f) ? initiator : recipient).ownership.UnclaimBed();
			}
			SpouseRelationUtility.ChangeNameAfterDivorce(initiator, -1f);
			SpouseRelationUtility.ChangeNameAfterDivorce(recipient, -1f);
			TaleRecorder.RecordTale(TaleDefOf.Breakup, new object[]
			{
				initiator,
				recipient
			});
		}

		// Token: 0x0600C887 RID: 51335 RVA: 0x003A943C File Offset: 0x003A763C
		public static void RemoveGotMarriedThoughts(Pawn initiator, Pawn recipient)
		{
			if (initiator.needs.mood != null)
			{
				initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
				initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.HoneymoonPhase, recipient);
			}
			if (recipient.needs.mood != null)
			{
				recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DivorcedMe, initiator, null);
				recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
				recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.HoneymoonPhase, initiator);
			}
		}

		// Token: 0x0600C888 RID: 51336 RVA: 0x003A9502 File Offset: 0x003A7702
		public static HistoryEventDef GetHistoryEventForSpouseCount(this Pawn pawn)
		{
			return SpouseRelationUtility.GetHistoryEventForSpouseCount(pawn.GetSpouseCount(false));
		}

		// Token: 0x0600C889 RID: 51337 RVA: 0x003A9510 File Offset: 0x003A7710
		public static HistoryEventDef GetHistoryEventForSpouseCount(int spouseCount)
		{
			if (spouseCount <= 1)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_OneOrFewer;
			}
			if (spouseCount <= 2)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Two;
			}
			if (spouseCount <= 3)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Three;
			}
			if (spouseCount <= 4)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Four;
			}
			return HistoryEventDefOf.GotMarried_SpouseCount_FiveOrMore;
		}

		// Token: 0x0600C88A RID: 51338 RVA: 0x003A953F File Offset: 0x003A773F
		public static HistoryEventDef GetHistoryEventForSpouseCountPlusOne(this Pawn pawn)
		{
			return SpouseRelationUtility.GetHistoryEventForSpouseCount(pawn.GetSpouseCount(false) + 1);
		}

		// Token: 0x0600C88B RID: 51339 RVA: 0x003A9550 File Offset: 0x003A7750
		public static HistoryEventDef GetHistoryEventForSpouseAndFianceCountPlusOne(this Pawn pawn)
		{
			List<DirectPawnRelation> list = LovePartnerRelationUtility.ExistingLovePartners(pawn, false);
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def != PawnRelationDefOf.Lover)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_OneOrFewer;
			}
			if (num < 2)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Two;
			}
			if (num < 3)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Three;
			}
			if (num < 4)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Four;
			}
			return HistoryEventDefOf.GotMarried_SpouseCount_FiveOrMore;
		}

		// Token: 0x0600C88C RID: 51340 RVA: 0x003A95BC File Offset: 0x003A77BC
		public static Pawn GetFirstSpouseOfOppositeGender(this Pawn pawn)
		{
			foreach (Pawn pawn2 in pawn.GetSpouses(true))
			{
				if (pawn.gender.Opposite() == pawn2.gender)
				{
					return pawn2;
				}
			}
			return null;
		}

		// Token: 0x0600C88D RID: 51341 RVA: 0x003A9624 File Offset: 0x003A7824
		public static MarriageNameChange Roll_NameChangeOnMarriage(Pawn pawn)
		{
			List<MarriageNameChange> list = new List<MarriageNameChange>();
			if (new HistoryEvent(HistoryEventDefOf.GotMarried_TookMansName, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
			{
				list.Add(MarriageNameChange.MansName);
			}
			if (new HistoryEvent(HistoryEventDefOf.GotMarried_TookWomansName, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
			{
				list.Add(MarriageNameChange.WomansName);
			}
			if (new HistoryEvent(HistoryEventDefOf.GotMarried_KeptName, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
			{
				list.Add(MarriageNameChange.NoChange);
			}
			if (!list.Any<MarriageNameChange>())
			{
				return MarriageNameChange.NoChange;
			}
			return list.RandomElement<MarriageNameChange>();
		}

		// Token: 0x0600C88E RID: 51342 RVA: 0x003A96B0 File Offset: 0x003A78B0
		public static bool Roll_BackToBirthNameAfterDivorce()
		{
			return Rand.Value < 0.6f;
		}

		// Token: 0x0600C88F RID: 51343 RVA: 0x003A96C0 File Offset: 0x003A78C0
		public static void DetermineManAndWomanSpouses(Pawn firstPawn, Pawn secondPawn, out Pawn man, out Pawn woman)
		{
			if (firstPawn.gender == secondPawn.gender)
			{
				man = ((firstPawn.thingIDNumber < secondPawn.thingIDNumber) ? firstPawn : secondPawn);
				woman = ((firstPawn.thingIDNumber < secondPawn.thingIDNumber) ? secondPawn : firstPawn);
				return;
			}
			man = ((firstPawn.gender == Gender.Male) ? firstPawn : secondPawn);
			woman = ((firstPawn.gender == Gender.Female) ? firstPawn : secondPawn);
		}

		// Token: 0x0600C890 RID: 51344 RVA: 0x003A9724 File Offset: 0x003A7924
		public static bool ChangeNameAfterMarriage(Pawn firstPawn, Pawn secondPawn, MarriageNameChange changeName)
		{
			if (changeName == MarriageNameChange.NoChange)
			{
				return false;
			}
			Pawn pawn = null;
			Pawn pawn2 = null;
			SpouseRelationUtility.DetermineManAndWomanSpouses(firstPawn, secondPawn, out pawn, out pawn2);
			NameTriple nameTriple = pawn.Name as NameTriple;
			NameTriple nameTriple2 = pawn2.Name as NameTriple;
			if (nameTriple == null || nameTriple2 == null)
			{
				return false;
			}
			string last = (changeName == MarriageNameChange.MansName) ? nameTriple.Last : nameTriple2.Last;
			pawn.Name = new NameTriple(nameTriple.First, nameTriple.Nick, last);
			pawn2.Name = new NameTriple(nameTriple2.First, nameTriple2.Nick, last);
			return true;
		}

		// Token: 0x0600C891 RID: 51345 RVA: 0x003A97AC File Offset: 0x003A79AC
		public static bool ChangeNameAfterDivorce(Pawn pawn, float chance = -1f)
		{
			NameTriple nameTriple = pawn.Name as NameTriple;
			if (nameTriple != null && pawn.story != null && pawn.story.birthLastName != null && nameTriple.Last != pawn.story.birthLastName && SpouseRelationUtility.Roll_BackToBirthNameAfterDivorce())
			{
				pawn.Name = new NameTriple(nameTriple.First, nameTriple.Nick, pawn.story.birthLastName);
				return true;
			}
			return false;
		}

		// Token: 0x0600C892 RID: 51346 RVA: 0x003A9824 File Offset: 0x003A7A24
		public static void Notify_PawnRegenerated(Pawn regenerated)
		{
			if (regenerated.relations != null)
			{
				Pawn firstDirectRelationPawn = regenerated.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
				if (firstDirectRelationPawn != null && regenerated.Name is NameTriple && firstDirectRelationPawn.Name is NameTriple)
				{
					NameTriple nameTriple = firstDirectRelationPawn.Name as NameTriple;
					firstDirectRelationPawn.Name = new NameTriple(nameTriple.First, nameTriple.Nick, nameTriple.Last);
				}
			}
		}

		// Token: 0x0600C893 RID: 51347 RVA: 0x003A9891 File Offset: 0x003A7A91
		public static string GetRandomBirthName(Pawn forPawn)
		{
			NameStyle style = NameStyle.Full;
			string forcedLastName = null;
			bool forceNoNick = false;
			Pawn_GeneTracker genes = forPawn.genes;
			return (PawnBioAndNameGenerator.GeneratePawnName(forPawn, style, forcedLastName, forceNoNick, (genes != null) ? genes.Xenotype : null) as NameTriple).Last;
		}

		// Token: 0x0600C894 RID: 51348 RVA: 0x003A98B8 File Offset: 0x003A7AB8
		public static void ResolveNameForSpouseOnGeneration(ref PawnGenerationRequest request, Pawn generated)
		{
			if (request.FixedLastName != null)
			{
				return;
			}
			MarriageNameChange marriageNameChange = SpouseRelationUtility.Roll_NameChangeOnMarriage(generated);
			if (marriageNameChange != MarriageNameChange.NoChange)
			{
				Pawn firstSpouse = generated.GetFirstSpouse();
				Pawn pawn;
				Pawn pawn2;
				SpouseRelationUtility.DetermineManAndWomanSpouses(generated, firstSpouse, out pawn, out pawn2);
				NameTriple nameTriple = pawn.Name as NameTriple;
				NameTriple nameTriple2 = pawn2.Name as NameTriple;
				if (generated == pawn2 && marriageNameChange == MarriageNameChange.WomansName)
				{
					pawn.Name = new NameTriple(nameTriple.First, nameTriple.Nick, nameTriple.Last);
					if (pawn.story != null)
					{
						pawn.story.birthLastName = SpouseRelationUtility.GetRandomBirthName(pawn);
					}
					request.SetFixedLastName(nameTriple.Last);
					return;
				}
				if (generated == pawn && marriageNameChange == MarriageNameChange.WomansName)
				{
					request.SetFixedLastName(nameTriple2.Last);
					request.SetFixedBirthName(SpouseRelationUtility.GetRandomBirthName(pawn));
					return;
				}
				if (generated == pawn2 && marriageNameChange == MarriageNameChange.MansName)
				{
					request.SetFixedLastName(nameTriple.Last);
					request.SetFixedBirthName(SpouseRelationUtility.GetRandomBirthName(pawn2));
					return;
				}
				if (generated == pawn && marriageNameChange == MarriageNameChange.MansName)
				{
					pawn2.Name = new NameTriple(nameTriple2.First, nameTriple2.Nick, nameTriple2.Last);
					if (pawn2.story != null)
					{
						pawn2.story.birthLastName = SpouseRelationUtility.GetRandomBirthName(pawn);
					}
					request.SetFixedLastName(nameTriple2.Last);
				}
			}
		}

		// Token: 0x04008878 RID: 34936
		public const float NoNameChangeOnMarriageChance = 0.25f;

		// Token: 0x04008879 RID: 34937
		public const float WomansNameChangeOnMarriageChance = 0.05f;

		// Token: 0x0400887A RID: 34938
		public const float MansNameOnMarriageChance = 0.7f;

		// Token: 0x0400887B RID: 34939
		public const float ChanceForSpousesToHaveTheSameName = 0.75f;

		// Token: 0x0400887C RID: 34940
		private static List<Pawn> tmpSpouses = new List<Pawn>();

		// Token: 0x0400887D RID: 34941
		private static List<DirectPawnRelation> tmpLoveRelations = new List<DirectPawnRelation>();

		// Token: 0x0400887E RID: 34942
		private static List<Pawn> tmpLoveClusterPawns = new List<Pawn>();

		// Token: 0x0400887F RID: 34943
		private static Stack<Pawn> tmpStack = new Stack<Pawn>();

		// Token: 0x04008880 RID: 34944
		private static readonly List<string> tmpDivorcedPawnNames = new List<string>();
	}
}
