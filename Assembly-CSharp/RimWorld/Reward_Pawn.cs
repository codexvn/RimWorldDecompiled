using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02003694 RID: 13972
	public class Reward_Pawn : Reward
	{
		// Token: 0x170031FB RID: 12795
		// (get) Token: 0x06013661 RID: 79457 RVA: 0x005C45BF File Offset: 0x005C27BF
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				if (this.pawn == null)
				{
					yield break;
				}
				foreach (GenUI.AnonymousStackElement anonymousStackElement in QuestPartUtility.GetRewardStackElementsForThings(Gen.YieldSingle<Pawn>(this.pawn), this.detailsHidden))
				{
					yield return anonymousStackElement;
				}
				IEnumerator<GenUI.AnonymousStackElement> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06013662 RID: 79458 RVA: 0x005C45D0 File Offset: 0x005C27D0
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, true, false, false, -1, 0, false);
			this.pawn = PawnGenerator.GeneratePawn(request);
			this.arrivalMode = (Rand.Bool ? Reward_Pawn.ArrivalMode.WalkIn : Reward_Pawn.ArrivalMode.DropPod);
			valueActuallyUsed = rewardValue;
		}

		// Token: 0x06013663 RID: 79459 RVA: 0x005C468D File Offset: 0x005C288D
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			Slate slate = QuestGen.slate;
			QuestGen.AddToGeneratedPawns(this.pawn);
			if (!this.pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(this.pawn, PawnDiscardDecideMode.Decide);
			}
			if (parms.giveToCaravan)
			{
				yield return new QuestPart_GiveToCaravan
				{
					inSignal = slate.Get<string>("inSignal", null, false),
					Things = Gen.YieldSingle<Pawn>(this.pawn)
				};
			}
			else
			{
				QuestPart_PawnsArrive pawnsArrive = new QuestPart_PawnsArrive();
				pawnsArrive.inSignal = slate.Get<string>("inSignal", null, false);
				pawnsArrive.pawns.Add(this.pawn);
				pawnsArrive.arrivalMode = ((this.arrivalMode == Reward_Pawn.ArrivalMode.DropPod) ? PawnsArrivalModeDefOf.CenterDrop : PawnsArrivalModeDefOf.EdgeWalkIn);
				pawnsArrive.joinPlayer = true;
				pawnsArrive.mapParent = slate.Get<Map>("map", null, false).Parent;
				if (!customLetterLabel.NullOrEmpty() || customLetterLabelRules != null)
				{
					QuestGen.AddTextRequest("root", delegate(string x)
					{
						pawnsArrive.customLetterLabel = x;
					}, QuestGenUtility.MergeRules(customLetterLabelRules, customLetterLabel, "root"));
				}
				if (!customLetterText.NullOrEmpty() || customLetterTextRules != null)
				{
					QuestGen.AddTextRequest("root", delegate(string x)
					{
						pawnsArrive.customLetterText = x;
					}, QuestGenUtility.MergeRules(customLetterTextRules, customLetterText, "root"));
				}
				yield return pawnsArrive;
			}
			yield break;
		}

		// Token: 0x06013664 RID: 79460 RVA: 0x005C46C4 File Offset: 0x005C28C4
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (parms.giveToCaravan)
			{
				return "Reward_Pawn_Caravan".Translate(this.pawn);
			}
			Reward_Pawn.ArrivalMode arrivalMode = this.arrivalMode;
			if (arrivalMode == Reward_Pawn.ArrivalMode.WalkIn)
			{
				return "Reward_Pawn_WalkIn".Translate(this.pawn);
			}
			if (arrivalMode != Reward_Pawn.ArrivalMode.DropPod)
			{
				throw new Exception("Unknown arrival mode: " + this.arrivalMode.ToString());
			}
			return "Reward_Pawn_DropPod".Translate(this.pawn);
		}

		// Token: 0x06013665 RID: 79461 RVA: 0x005C475C File Offset: 0x005C295C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				base.GetType().Name,
				" (",
				this.pawn.MarketValue.ToStringMoney(null),
				" pawn=",
				this.pawn.ToStringSafe<Pawn>(),
				", arrivalMode=",
				this.arrivalMode.ToString(),
				")"
			});
		}

		// Token: 0x06013666 RID: 79462 RVA: 0x005C47D8 File Offset: 0x005C29D8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", true);
			Scribe_Values.Look<Reward_Pawn.ArrivalMode>(ref this.arrivalMode, "arrivalMode", Reward_Pawn.ArrivalMode.WalkIn, false);
			Scribe_Values.Look<bool>(ref this.detailsHidden, "detailsHidden", false, false);
		}

		// Token: 0x0400D712 RID: 55058
		public Pawn pawn;

		// Token: 0x0400D713 RID: 55059
		public Reward_Pawn.ArrivalMode arrivalMode;

		// Token: 0x0400D714 RID: 55060
		public bool detailsHidden;

		// Token: 0x0400D715 RID: 55061
		private const string RootSymbol = "root";

		// Token: 0x02003695 RID: 13973
		public enum ArrivalMode
		{
			// Token: 0x0400D717 RID: 55063
			WalkIn,
			// Token: 0x0400D718 RID: 55064
			DropPod
		}
	}
}
