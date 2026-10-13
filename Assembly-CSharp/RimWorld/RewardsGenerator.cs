using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020036A6 RID: 13990
	public static class RewardsGenerator
	{
		// Token: 0x060136E3 RID: 79587 RVA: 0x005C57D0 File Offset: 0x005C39D0
		public static void ResetStaticData()
		{
			RewardsGenerator.MarketValueFillers.Clear();
			RewardsGenerator.MarketValueFillers.Add(ThingDefOf.Silver);
			RewardsGenerator.MarketValueFillers.Add(ThingDefOf.Gold);
			RewardsGenerator.MarketValueFillers.Add(ThingDefOf.Uranium);
			RewardsGenerator.MarketValueFillers.Add(ThingDefOf.Jade);
			RewardsGenerator.MarketValueFillers.Add(ThingDefOf.Plasteel);
		}

		// Token: 0x060136E4 RID: 79588 RVA: 0x005C5834 File Offset: 0x005C3A34
		public static List<Reward> Generate(RewardsGeneratorParams parms)
		{
			float num;
			return RewardsGenerator.Generate(parms, out num);
		}

		// Token: 0x060136E5 RID: 79589 RVA: 0x005C584C File Offset: 0x005C3A4C
		public static List<Reward> Generate(RewardsGeneratorParams parms, out float generatedRewardValue)
		{
			List<Reward> result;
			try
			{
				result = RewardsGenerator.DoGenerate(parms, out generatedRewardValue);
			}
			finally
			{
			}
			return result;
		}

		// Token: 0x060136E6 RID: 79590 RVA: 0x005C5878 File Offset: 0x005C3A78
		private static List<Reward> DoGenerate(RewardsGeneratorParams parms, out float generatedRewardValue)
		{
			List<Reward> list = new List<Reward>();
			string text = parms.ConfigError();
			if (text != null)
			{
				Log.Error("Invalid reward generation params: " + text);
				generatedRewardValue = 0f;
				return list;
			}
			parms.rewardValue = Mathf.Max(parms.rewardValue, parms.minGeneratedRewardValue);
			bool flag = parms.allowGoodwill && parms.giverFaction != null && parms.giverFaction != Faction.OfPlayer && parms.giverFaction.CanEverGiveGoodwillRewards && parms.giverFaction.allowGoodwillRewards && parms.giverFaction.PlayerGoodwill <= 92;
			bool flag2 = parms.allowRoyalFavor && parms.giverFaction != null && parms.giverFaction.allowRoyalFavorRewards && parms.giverFaction.def.HasRoyalTitles;
			bool flag3 = flag2 || flag;
			bool flag4 = parms.giverFaction != null && parms.giverFaction.HostileTo(Faction.OfPlayer);
			bool flag5 = flag2 && Faction.OfEmpire != null && parms.giverFaction == Faction.OfEmpire && !parms.thingRewardItemsOnly;
			bool flag6;
			bool flag7;
			if (!parms.thingRewardDisallowed && !flag3)
			{
				flag6 = true;
				flag7 = false;
			}
			else if (parms.thingRewardDisallowed && flag3)
			{
				flag6 = false;
				flag7 = true;
			}
			else if (parms.thingRewardDisallowed && !flag3)
			{
				flag6 = false;
				flag7 = false;
			}
			else
			{
				float num = flag2 ? 0.6f : 0.3f;
				float value = Rand.Value;
				if (value < 0.3f && !flag5 && !flag4)
				{
					flag6 = true;
					flag7 = false;
				}
				else if (parms.thingRewardRequired)
				{
					flag6 = true;
					flag7 = true;
				}
				else if (value < 0.3f + num)
				{
					flag6 = false;
					flag7 = true;
				}
				else
				{
					flag6 = !flag4;
					flag7 = true;
				}
			}
			float num2;
			float num3;
			if (flag6 && !flag7)
			{
				num2 = parms.rewardValue;
				num3 = 0f;
			}
			else if (!flag6 && flag7)
			{
				num2 = 0f;
				num3 = parms.rewardValue;
			}
			else
			{
				if (!flag6 || !flag7)
				{
					generatedRewardValue = 0f;
					return list;
				}
				float num4 = Rand.Range(0.3f, 0.7f);
				float num5 = 1f - num4;
				num2 = parms.rewardValue * num4;
				num3 = parms.rewardValue * num5;
			}
			float num6 = 0f;
			float num7 = 0f;
			Reward reward = null;
			Reward reward2 = null;
			if (Rand.Value < 0.65f && !flag5)
			{
				if (num2 > 0f)
				{
					reward = RewardsGenerator.GenerateThingReward(num2, parms, out num6);
					if (flag7 || (flag3 && num2 - num6 >= 600f))
					{
						num3 += num2 - num6;
					}
				}
				if (num3 > 0f)
				{
					reward2 = RewardsGenerator.GenerateSocialReward(num3, parms, flag, flag2, out num7);
				}
			}
			else
			{
				if (num3 > 0f)
				{
					reward2 = RewardsGenerator.GenerateSocialReward(num3, parms, flag, flag2, out num7);
					if (flag6 || (!parms.thingRewardDisallowed && num3 - num7 >= 600f && !flag4))
					{
						num2 += num3 - num7;
					}
				}
				if (num2 > 0f)
				{
					reward = RewardsGenerator.GenerateThingReward(num2, parms, out num6);
				}
			}
			generatedRewardValue = num6 + num7;
			Reward_Items reward_Items = null;
			float num8 = parms.rewardValue - num6 - num7;
			if ((num8 >= 200f || num6 + num7 < parms.minGeneratedRewardValue) && !parms.thingRewardDisallowed)
			{
				reward_Items = RewardsGenerator.AddMarketValueFillers(num8, ref generatedRewardValue, reward);
			}
			if (reward != null)
			{
				list.Add(reward);
			}
			if (reward2 != null)
			{
				list.Add(reward2);
			}
			if (reward_Items != null)
			{
				list.Add(reward_Items);
			}
			return list;
		}

		// Token: 0x060136E7 RID: 79591 RVA: 0x005C5BCC File Offset: 0x005C3DCC
		private static Reward GenerateSocialReward(float rewardValue, RewardsGeneratorParams parms, bool allowGoodwill, bool allowRoyalFavor, out float valueActuallyUsed)
		{
			if (!allowGoodwill && !allowRoyalFavor)
			{
				string str = "GenerateSocialReward could not generate any reward for parms=";
				RewardsGeneratorParams parms2 = parms;
				Log.Error(str + parms2.ToString());
				allowGoodwill = true;
			}
			float valueActuallyUsedLocal = 0f;
			Func<Reward> func = () => RewardsGenerator.GenerateReward<Reward_Goodwill>(rewardValue, parms, out valueActuallyUsedLocal);
			Func<Reward> b = () => RewardsGenerator.GenerateReward<Reward_RoyalFavor>(rewardValue, parms, out valueActuallyUsedLocal);
			if (allowGoodwill && parms.giverFaction != null && parms.giverFaction.HostileTo(Faction.OfPlayer))
			{
				Reward result = func();
				valueActuallyUsed = valueActuallyUsedLocal;
				return result;
			}
			float weightA = allowGoodwill ? 1f : 0f;
			float weightB = allowRoyalFavor ? 9f : 0f;
			Reward result2 = Rand.ElementByWeight<Func<Reward>>(func, weightA, b, weightB)();
			valueActuallyUsed = valueActuallyUsedLocal;
			return result2;
		}

		// Token: 0x060136E8 RID: 79592 RVA: 0x005C5CB0 File Offset: 0x005C3EB0
		private static Reward GenerateThingReward(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			float valueActuallyUsedLocal = 0f;
			Func<Reward> func = () => RewardsGenerator.GenerateReward<Reward_Items>(rewardValue, parms, out valueActuallyUsedLocal);
			Func<Reward> b = () => RewardsGenerator.GenerateReward<Reward_Pawn>(rewardValue, parms, out valueActuallyUsedLocal);
			if (parms.thingRewardItemsOnly)
			{
				Reward result = func();
				valueActuallyUsed = valueActuallyUsedLocal;
				return result;
			}
			float weightB = Mathf.Max(0f, 1f * parms.populationIntent);
			Reward result2 = Rand.ElementByWeight<Func<Reward>>(func, 3f, b, weightB)();
			valueActuallyUsed = valueActuallyUsedLocal;
			return result2;
		}

		// Token: 0x060136E9 RID: 79593 RVA: 0x005C5D48 File Offset: 0x005C3F48
		private static T GenerateReward<T>(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed) where T : Reward, new()
		{
			T result;
			try
			{
				T t = Activator.CreateInstance<T>();
				t.InitFromValue(rewardValue, parms, out valueActuallyUsed);
				result = t;
			}
			finally
			{
			}
			return result;
		}

		// Token: 0x060136EA RID: 79594 RVA: 0x005C5D80 File Offset: 0x005C3F80
		private static Reward_Items AddMarketValueFillers(float remainingValue, ref float generatedRewardValue, Reward thingReward)
		{
			IEnumerable<ThingDef> source = from x in RewardsGenerator.MarketValueFillers
			where remainingValue / x.BaseMarketValue >= 15f
			select x;
			if (!source.Any<ThingDef>())
			{
				return null;
			}
			ThingDef thingDef = null;
			Reward_Items existingItemsReward = thingReward as Reward_Items;
			if (existingItemsReward != null)
			{
				thingDef = source.FirstOrDefault((ThingDef x) => existingItemsReward.items.Any((Thing y) => y.def == x));
			}
			if (thingDef == null)
			{
				thingDef = source.RandomElement<ThingDef>();
			}
			int num = GenMath.RoundRandom(remainingValue / thingDef.BaseMarketValue);
			if (num >= 1)
			{
				Thing thing = ThingMaker.MakeThing(thingDef, null);
				thing.stackCount = num;
				generatedRewardValue += thing.MarketValue * (float)thing.stackCount;
				Reward_Items reward_Items = thingReward as Reward_Items;
				if (reward_Items == null)
				{
					return new Reward_Items
					{
						items = 
						{
							thing
						}
					};
				}
				reward_Items.items.Add(thing);
			}
			return null;
		}

		// Token: 0x0400D754 RID: 55124
		private const float ThingRewardOnlyChance = 0.3f;

		// Token: 0x0400D755 RID: 55125
		private const float SocialRewardOnlyChance = 0.3f;

		// Token: 0x0400D756 RID: 55126
		private const float SocialRewardOnlyChance_RoyalFavorPossible = 0.6f;

		// Token: 0x0400D757 RID: 55127
		private const float ThingRewardMinFractionOfTotal = 0.3f;

		// Token: 0x0400D758 RID: 55128
		private const float SocialRewardMinFractionOfTotal = 0.3f;

		// Token: 0x0400D759 RID: 55129
		public const float ItemsValueFractionMaxVariance = 0.3f;

		// Token: 0x0400D75A RID: 55130
		private const float ThingRewardGenerateFirstChance = 0.65f;

		// Token: 0x0400D75B RID: 55131
		private const float ThingRewardSelectionWeight_Items = 3f;

		// Token: 0x0400D75C RID: 55132
		private const float ThingRewardSelectionWeight_Pawn = 1f;

		// Token: 0x0400D75D RID: 55133
		private const float SocialRewardSelectionWeight_RoyalFavor = 9f;

		// Token: 0x0400D75E RID: 55134
		private const float SocialRewardSelectionWeight_Goodwill = 1f;

		// Token: 0x0400D75F RID: 55135
		private const float MinValueForExtraSilverReward = 200f;

		// Token: 0x0400D760 RID: 55136
		private const float MinValueToGenerateSecondRewardType = 600f;

		// Token: 0x0400D761 RID: 55137
		private static readonly List<ThingDef> MarketValueFillers = new List<ThingDef>();

		// Token: 0x0400D762 RID: 55138
		public static readonly SimpleCurve RewardValueToRoyalFavorCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 2f),
				true
			},
			{
				new CurvePoint(500f, 4f),
				true
			},
			{
				new CurvePoint(2000f, 10f),
				true
			},
			{
				new CurvePoint(5000f, 18f),
				true
			}
		};

		// Token: 0x0400D763 RID: 55139
		public static readonly SimpleCurve RewardValueToGoodwillCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 10f),
				true
			},
			{
				new CurvePoint(500f, 15f),
				true
			},
			{
				new CurvePoint(1000f, 20f),
				true
			},
			{
				new CurvePoint(2000f, 35f),
				true
			},
			{
				new CurvePoint(5000f, 50f),
				true
			}
		};
	}
}
