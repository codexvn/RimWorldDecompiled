using System;
using System.Collections.Generic;
using LudeonTK;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02003888 RID: 14472
	public static class GrowthUtility
	{
		// Token: 0x06014014 RID: 81940 RVA: 0x005F0B00 File Offset: 0x005EED00
		public static bool IsGrowthBirthday(int age)
		{
			for (int i = 0; i < GrowthUtility.GrowthMomentAges.Length; i++)
			{
				if (age == GrowthUtility.GrowthMomentAges[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06014015 RID: 81941 RVA: 0x005F0B2C File Offset: 0x005EED2C
		[DebugOutput("Text generation", true)]
		private static void GrowthMomentFlavor()
		{
			Pawn targetPawn;
			if (!PawnsFinder.AllMaps_FreeColonists.TryRandomElement(out targetPawn))
			{
				Log.Error("No colonists to generate growth moment flavor for.");
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			for (int i = 0; i < GrowthUtility.GrowthTiers.Length; i++)
			{
				int tier = i;
				list.Add(new DebugMenuOption("Tier " + tier.ToString(), DebugMenuOptionMode.Action, delegate()
				{
					string text = string.Concat(new string[]
					{
						"Samples for ",
						targetPawn.NameShortColored.Resolve(),
						" at tier ",
						tier.ToString(),
						":"
					});
					for (int j = 0; j < 10; j++)
					{
						text = text + "\n  - " + GrowthUtility.GrowthFlavorForTier(targetPawn, tier);
					}
					Log.Message(text);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x06014016 RID: 81942 RVA: 0x005F0BC8 File Offset: 0x005EEDC8
		public static string GrowthFlavorForTier(Pawn pawn, int growthTier)
		{
			int num;
			if (growthTier >= 8)
			{
				num = 4;
			}
			else if (growthTier >= 6)
			{
				num = 3;
			}
			else if (growthTier >= 4)
			{
				num = 2;
			}
			else if (growthTier >= 1)
			{
				num = 1;
			}
			else
			{
				num = 0;
			}
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(RulePackDefOf.GrowthMomentFlavor);
			request.Constants.Add("tierSection", num.ToString());
			request.Rules.AddRange(GrammarUtility.RulesForPawn("PAWN", pawn, request.Constants, false, true));
			return GrammarResolver.Resolve("r_root", request, null, false, null, null, null, true);
		}

		// Token: 0x0400DD0E RID: 56590
		public static readonly GrowthUtility.GrowthTier[] GrowthTiers = new GrowthUtility.GrowthTier[]
		{
			new GrowthUtility.GrowthTier(0, 0f, ByteRange.Zero, 0, 1, 1),
			new GrowthUtility.GrowthTier(1, 30f, ByteRange.Zero, 0, 1, 2),
			new GrowthUtility.GrowthTier(2, 55f, ByteRange.Zero, 0, 1, 3),
			new GrowthUtility.GrowthTier(3, 80f, ByteRange.Zero, 0, 1, 4),
			new GrowthUtility.GrowthTier(4, 100f, ByteRange.One, 1, 1, 4),
			new GrowthUtility.GrowthTier(5, 120f, ByteRange.One, 2, 1, 4),
			new GrowthUtility.GrowthTier(6, 135f, ByteRange.One, 3, 1, 4),
			new GrowthUtility.GrowthTier(7, 150f, new ByteRange(2, 2), 4, 1, 4),
			new GrowthUtility.GrowthTier(8, 162f, new ByteRange(3, 4), 6, 1, 6)
		};

		// Token: 0x0400DD0F RID: 56591
		public static readonly int[] GrowthMomentAges = new int[]
		{
			7,
			10,
			13
		};

		// Token: 0x02003889 RID: 14473
		public readonly struct GrowthTier
		{
			// Token: 0x06014018 RID: 81944 RVA: 0x005F0D79 File Offset: 0x005EEF79
			public GrowthTier(byte tier, float pointsRequirement, ByteRange passionGainsRange, byte passionChoices, byte traitGains, byte traitChoices)
			{
				this.tier = tier;
				this.pointsRequirement = pointsRequirement;
				this.passionGainsRange = passionGainsRange;
				this.passionChoices = passionChoices;
				this.traitGains = traitGains;
				this.traitChoices = traitChoices;
			}

			// Token: 0x06014019 RID: 81945 RVA: 0x005F0DA8 File Offset: 0x005EEFA8
			public int PassionGainFor(Pawn pawn)
			{
				int randomInRange;
				using (new RandBlock(Gen.HashCombineInt(pawn.GetHashCode(), (int)this.tier)))
				{
					randomInRange = this.passionGainsRange.RandomInRange;
				}
				return randomInRange;
			}

			// Token: 0x0400DD10 RID: 56592
			public readonly byte tier;

			// Token: 0x0400DD11 RID: 56593
			public readonly float pointsRequirement;

			// Token: 0x0400DD12 RID: 56594
			public readonly ByteRange passionGainsRange;

			// Token: 0x0400DD13 RID: 56595
			public readonly byte passionChoices;

			// Token: 0x0400DD14 RID: 56596
			public readonly byte traitGains;

			// Token: 0x0400DD15 RID: 56597
			public readonly byte traitChoices;
		}
	}
}
