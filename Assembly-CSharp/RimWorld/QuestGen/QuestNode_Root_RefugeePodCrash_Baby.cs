using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B83 RID: 15235
	public class QuestNode_Root_RefugeePodCrash_Baby : QuestNode_Root_WandererJoin
	{
		// Token: 0x06014B20 RID: 84768 RVA: 0x0063005E File Offset: 0x0062E25E
		protected override bool TestRunInt(Slate slate)
		{
			return Find.Storyteller.difficulty.ChildrenAllowed && base.TestRunInt(slate);
		}

		// Token: 0x06014B21 RID: 84769 RVA: 0x0063007C File Offset: 0x0062E27C
		public override Pawn GeneratePawn()
		{
			Faction faction;
			Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, true, false, TechLevel.Undefined, TechLevel.Undefined, false, false);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, faction, PawnGenerationContext.NonPlayer, null, true, false, true, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, true, false, false, -1, 0, false)
			{
				AllowedDevelopmentalStages = DevelopmentalStage.Baby
			});
			pawn.ageTracker.AgeChronologicalTicks = pawn.ageTracker.AgeBiologicalTicks;
			return pawn;
		}

		// Token: 0x06014B22 RID: 84770 RVA: 0x00630158 File Offset: 0x0062E358
		protected override void AddSpawnPawnQuestParts(Quest quest, Map map, Pawn pawn)
		{
			List<Thing> list = new List<Thing>
			{
				pawn
			};
			if (Rand.Value < 0.5f)
			{
				Pawn mother = pawn.GetMother();
				bool flag = mother == null || Find.WorldPawns.GetSituation(mother) == WorldPawnSituation.None;
				Pawn father = pawn.GetFather();
				bool flag2 = father == null || Find.WorldPawns.GetSituation(father) == WorldPawnSituation.None;
				if (flag || flag2)
				{
					QuestGen.slate.Set<bool>("hasParent", true, false);
					PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, pawn.Faction, PawnGenerationContext.NonPlayer, null, true, true, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, true, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
					if (flag && !flag2)
					{
						request.FixedGender = new Gender?(Gender.Female);
					}
					else if (flag2 && !flag)
					{
						request.FixedGender = new Gender?(Gender.Male);
					}
					else if (Rand.Value < 0.5f)
					{
						request.FixedGender = new Gender?(Gender.Female);
					}
					else
					{
						request.FixedGender = new Gender?(Gender.Male);
					}
					Pawn pawn2 = PawnGenerator.GeneratePawn(request);
					pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, pawn2);
					list.Add(pawn2.Corpse);
				}
			}
			else
			{
				QuestGen.slate.Set<bool>("hasParent", false, false);
			}
			quest.DropPods(map.Parent, list, null, null, null, null, new bool?(false), false, false, false, null, null, QuestPart.SignalListenMode.OngoingOnly, null, true, true, false, null);
		}

		// Token: 0x06014B23 RID: 84771 RVA: 0x0063032C File Offset: 0x0062E52C
		public override void SendLetter(Quest quest, Pawn pawn)
		{
			TaggedString label = "LetterLabelRefugeePodCrash".Translate();
			TaggedString taggedString = "RefugeePodCrashBaby".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			if (QuestGen.slate.Get<bool>("hasParent", false, false))
			{
				taggedString += "\n\n" + "RefugeePodCrashBabyHasParent".Translate(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			}
			QuestNode_Root_WandererJoin_WalkIn.AppendCharityInfoToLetter("JoinerCharityInfo".Translate(pawn), ref taggedString);
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref taggedString, ref label, pawn);
			Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.NeutralEvent, new TargetInfo(pawn), null, null, null, null, 0, true);
		}

		// Token: 0x0400E5F2 RID: 58866
		private const float ChanceToTryGenerateParent = 0.5f;

		// Token: 0x0400E5F3 RID: 58867
		private const string HasParentFlagName = "hasParent";
	}
}
