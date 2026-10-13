using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B89 RID: 11145
	public class ThingSetMaker_ResourcePod : ThingSetMaker
	{
		// Token: 0x0600F938 RID: 63800 RVA: 0x00487198 File Offset: 0x00485398
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			ThingDef thingDef = ThingSetMaker_ResourcePod.RandomPodContentsDef(false);
			float num = Rand.Range(150f, 600f);
			while (num > thingDef.BaseMarketValue)
			{
				ThingDef stuff = GenStuff.RandomStuffByCommonalityFor(thingDef, TechLevel.Undefined);
				Thing thing = ThingMaker.MakeThing(thingDef, stuff);
				int num2 = Rand.Range(20, 40);
				if (num2 > thing.def.stackLimit)
				{
					num2 = thing.def.stackLimit;
				}
				float statValue = thing.GetStatValue(StatDefOf.MarketValue, true, -1);
				if ((float)num2 * statValue > num)
				{
					num2 = Mathf.FloorToInt(num / statValue);
				}
				if (num2 == 0)
				{
					num2 = 1;
				}
				thing.stackCount = num2;
				outThings.Add(thing);
				num -= (float)num2 * statValue;
				if (outThings.Count >= 7 || num <= statValue)
				{
					break;
				}
			}
		}

		// Token: 0x0600F939 RID: 63801 RVA: 0x00487252 File Offset: 0x00485452
		private static IEnumerable<ThingDef> PossiblePodContentsDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Item && d.tradeability.TraderCanSell() && d.equipmentType == EquipmentType.None && d.BaseMarketValue >= 1f && d.BaseMarketValue < 40f && !d.preventSpawningInResourcePod && !d.HasComp(typeof(CompHatcher))
			select d;
		}

		// Token: 0x0600F93A RID: 63802 RVA: 0x00487280 File Offset: 0x00485480
		public static ThingDef RandomPodContentsDef(bool mustBeResource = false)
		{
			IEnumerable<ThingDef> source = ThingSetMaker_ResourcePod.PossiblePodContentsDefs();
			if (mustBeResource)
			{
				source = from x in source
				where x.stackLimit > 1
				select x;
			}
			int numMeats = source.Count((ThingDef x) => x.IsMeat);
			int numLeathers = source.Count((ThingDef x) => x.IsLeather);
			int numEggs = source.Count((ThingDef x) => x.IsEgg);
			return source.RandomElementByWeight((ThingDef d) => ThingSetMakerUtility.AdjustedBigCategoriesSelectionWeight(d, numMeats, numLeathers, numEggs));
		}

		// Token: 0x0600F93B RID: 63803 RVA: 0x00487358 File Offset: 0x00485558
		[DebugOutput("Incidents", false)]
		private static void PodContentsPossibleDefs()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("ThingDefs that can go in the resource pod crash incident.");
			foreach (ThingDef thingDef in ThingSetMaker_ResourcePod.PossiblePodContentsDefs())
			{
				stringBuilder.AppendLine(thingDef.defName);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600F93C RID: 63804 RVA: 0x004873C8 File Offset: 0x004855C8
		[DebugOutput("Incidents", false)]
		private static void PodContentsTest()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 100; i++)
			{
				stringBuilder.AppendLine(ThingSetMaker_ResourcePod.RandomPodContentsDef(false).LabelCap);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600F93D RID: 63805 RVA: 0x0048740A File Offset: 0x0048560A
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_ResourcePod.PossiblePodContentsDefs();
		}

		// Token: 0x0400A3DE RID: 41950
		private const int MaxStacks = 7;

		// Token: 0x0400A3DF RID: 41951
		private const float MaxMarketValue = 40f;

		// Token: 0x0400A3E0 RID: 41952
		private const float MinMoney = 150f;

		// Token: 0x0400A3E1 RID: 41953
		private const float MaxMoney = 600f;
	}
}
