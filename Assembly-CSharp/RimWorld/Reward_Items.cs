using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200368B RID: 13963
	public class Reward_Items : Reward
	{
		// Token: 0x170031F1 RID: 12785
		// (get) Token: 0x0601362A RID: 79402 RVA: 0x005C3A03 File Offset: 0x005C1C03
		public List<Thing> ItemsListForReading
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x170031F2 RID: 12786
		// (get) Token: 0x0601362B RID: 79403 RVA: 0x005C3A0B File Offset: 0x005C1C0B
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				if (this.usedOrCleanedUp)
				{
					foreach (GenUI.AnonymousStackElement anonymousStackElement in QuestPartUtility.GetRewardStackElementsForThings(this.itemDefs))
					{
						yield return anonymousStackElement;
					}
					IEnumerator<GenUI.AnonymousStackElement> enumerator = null;
				}
				else
				{
					foreach (GenUI.AnonymousStackElement anonymousStackElement2 in QuestPartUtility.GetRewardStackElementsForThings(this.items, false))
					{
						yield return anonymousStackElement2;
					}
					IEnumerator<GenUI.AnonymousStackElement> enumerator = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x170031F3 RID: 12787
		// (get) Token: 0x0601362C RID: 79404 RVA: 0x005C3A1C File Offset: 0x005C1C1C
		public override float TotalMarketValue
		{
			get
			{
				if (this.usedOrCleanedUp)
				{
					return this.lastTotalMarketValue;
				}
				float num = 0f;
				for (int i = 0; i < this.items.Count; i++)
				{
					Thing innerIfMinified = this.items[i].GetInnerIfMinified();
					num += innerIfMinified.MarketValue * (float)this.items[i].stackCount;
				}
				return num;
			}
		}

		// Token: 0x0601362D RID: 79405 RVA: 0x005C3A83 File Offset: 0x005C1C83
		public override void Notify_Used()
		{
			this.RememberItems();
			base.Notify_Used();
		}

		// Token: 0x0601362E RID: 79406 RVA: 0x005C3A91 File Offset: 0x005C1C91
		public override void Notify_PreCleanup()
		{
			this.RememberItems();
			base.Notify_PreCleanup();
		}

		// Token: 0x0601362F RID: 79407 RVA: 0x005C3AA0 File Offset: 0x005C1CA0
		private void RememberItems()
		{
			if (this.usedOrCleanedUp)
			{
				return;
			}
			this.itemDefs.Clear();
			this.lastTotalMarketValue = 0f;
			for (int i = 0; i < this.items.Count; i++)
			{
				Thing innerIfMinified = this.items[i].GetInnerIfMinified();
				if (innerIfMinified != null && !innerIfMinified.Destroyed)
				{
					QualityCategory quality;
					if (!innerIfMinified.TryGetQuality(out quality))
					{
						quality = QualityCategory.Normal;
					}
					this.itemDefs.Add(new Reward_Items.RememberedItem(new ThingStuffPairWithQuality(innerIfMinified.def, innerIfMinified.Stuff, quality), this.items[i].stackCount, this.items[i].LabelNoCount));
					this.lastTotalMarketValue += innerIfMinified.MarketValue * (float)this.items[i].stackCount;
				}
			}
		}

		// Token: 0x06013630 RID: 79408 RVA: 0x005C3B7C File Offset: 0x005C1D7C
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			this.items.Clear();
			bool flag = true;
			float x2 = (Find.TickManager.TicksGame - Find.History.lastPsylinkAvailable).TicksToDays();
			if (Rand.Chance(QuestTuning.DaysSincePsylinkAvailableToGuaranteedNeuroformerChance.Evaluate(x2)) && ModsConfig.RoyaltyActive && (parms.disallowedThingDefs == null || !parms.disallowedThingDefs.Contains(ThingDefOf.PsychicAmplifier)) && rewardValue >= 600f && (Faction.OfEmpire == null || parms.giverFaction != Faction.OfEmpire))
			{
				this.items.Add(ThingMaker.MakeThing(ThingDefOf.PsychicAmplifier, null));
				rewardValue -= this.items[0].MarketValue;
				if (rewardValue < 100f)
				{
					flag = false;
				}
			}
			if (flag)
			{
				FloatRange value = rewardValue * new FloatRange(0.7f, 1.3f);
				ThingSetMakerParams parms2 = default(ThingSetMakerParams);
				parms2.totalMarketValueRange = new FloatRange?(value);
				parms2.makingFaction = parms.giverFaction;
				if (!parms.disallowedThingDefs.NullOrEmpty<ThingDef>())
				{
					parms2.validator = ((ThingDef x) => !parms.disallowedThingDefs.Contains(x));
				}
				this.items.AddRange(ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms2));
			}
			valueActuallyUsed = this.TotalMarketValue;
		}

		// Token: 0x06013631 RID: 79409 RVA: 0x005C3CDD File Offset: 0x005C1EDD
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			Slate slate = QuestGen.slate;
			for (int i = 0; i < this.items.Count; i++)
			{
				Pawn pawn = this.items[i] as Pawn;
				if (pawn != null)
				{
					QuestGen.AddToGeneratedPawns(pawn);
					if (!pawn.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
			}
			if (parms.giveToCaravan)
			{
				yield return new QuestPart_GiveToCaravan
				{
					inSignal = slate.Get<string>("inSignal", null, false),
					Things = this.items
				};
			}
			else
			{
				QuestPart_DropPods dropPods = new QuestPart_DropPods();
				dropPods.inSignal = slate.Get<string>("inSignal", null, false);
				if (!customLetterLabel.NullOrEmpty() || customLetterLabelRules != null)
				{
					QuestGen.AddTextRequest("root", delegate(string x)
					{
						dropPods.customLetterLabel = x;
					}, QuestGenUtility.MergeRules(customLetterLabelRules, customLetterLabel, "root"));
				}
				if (!customLetterText.NullOrEmpty() || customLetterTextRules != null)
				{
					QuestGen.AddTextRequest("root", delegate(string x)
					{
						dropPods.customLetterText = x;
					}, QuestGenUtility.MergeRules(customLetterTextRules, customLetterText, "root"));
				}
				dropPods.mapParent = slate.Get<Map>("map", null, false).Parent;
				dropPods.useTradeDropSpot = true;
				dropPods.Things = this.items;
				yield return dropPods;
			}
			slate.Set<List<Thing>>("itemsReward_items", this.items, false);
			slate.Set<float>("itemsReward_totalMarketValue", this.TotalMarketValue, false);
			yield break;
		}

		// Token: 0x06013632 RID: 79410 RVA: 0x005C3D14 File Offset: 0x005C1F14
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			if (parms.giveToCaravan)
			{
				return "Reward_Items_Caravan".Translate(GenLabel.ThingsLabel(this.items, "  - "), this.TotalMarketValue.ToStringMoney(null));
			}
			return "Reward_Items".Translate(GenLabel.ThingsLabel(this.items, "  - "), this.TotalMarketValue.ToStringMoney(null));
		}

		// Token: 0x06013633 RID: 79411 RVA: 0x005C3D94 File Offset: 0x005C1F94
		public override string ToString()
		{
			string text = base.GetType().Name;
			text = text + "(value " + this.TotalMarketValue.ToStringMoney(null) + ")";
			foreach (Thing thing in this.items)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n  -",
					thing.LabelCap,
					" ",
					(thing.MarketValue * (float)thing.stackCount).ToStringMoney(null)
				});
			}
			return text;
		}

		// Token: 0x06013634 RID: 79412 RVA: 0x005C3E4C File Offset: 0x005C204C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.items, "items", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Reward_Items.RememberedItem>(ref this.itemDefs, "itemDefs", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.lastTotalMarketValue, "lastTotalMarketValue", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.items.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x0400D6EE RID: 55022
		public List<Thing> items = new List<Thing>();

		// Token: 0x0400D6EF RID: 55023
		private List<Reward_Items.RememberedItem> itemDefs = new List<Reward_Items.RememberedItem>();

		// Token: 0x0400D6F0 RID: 55024
		private float lastTotalMarketValue;

		// Token: 0x0400D6F1 RID: 55025
		private const string RootSymbol = "root";

		// Token: 0x0200368C RID: 13964
		public struct RememberedItem : IExposable
		{
			// Token: 0x06013636 RID: 79414 RVA: 0x005C3EF2 File Offset: 0x005C20F2
			public RememberedItem(ThingStuffPairWithQuality thing, int stackCount, string label)
			{
				this.thing = thing;
				this.stackCount = stackCount;
				this.label = label;
			}

			// Token: 0x06013637 RID: 79415 RVA: 0x005C3F09 File Offset: 0x005C2109
			public void ExposeData()
			{
				Scribe_Deep.Look<ThingStuffPairWithQuality>(ref this.thing, "thing", Array.Empty<object>());
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
				Scribe_Values.Look<string>(ref this.label, "label", null, false);
			}

			// Token: 0x0400D6F2 RID: 55026
			public ThingStuffPairWithQuality thing;

			// Token: 0x0400D6F3 RID: 55027
			public int stackCount;

			// Token: 0x0400D6F4 RID: 55028
			public string label;
		}
	}
}
