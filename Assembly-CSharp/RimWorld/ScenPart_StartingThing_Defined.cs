using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020028F7 RID: 10487
	public class ScenPart_StartingThing_Defined : ScenPart_ThingCount
	{
		// Token: 0x170022CE RID: 8910
		// (get) Token: 0x0600E555 RID: 58709 RVA: 0x0042EACB File Offset: 0x0042CCCB
		public static string PlayerStartWithIntro
		{
			get
			{
				return "ScenPart_StartWith".Translate();
			}
		}

		// Token: 0x0600E556 RID: 58710 RVA: 0x0042D59E File Offset: 0x0042B79E
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x0600E557 RID: 58711 RVA: 0x0042EADC File Offset: 0x0042CCDC
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x0600E558 RID: 58712 RVA: 0x0042EAF3 File Offset: 0x0042CCF3
		public override IEnumerable<Thing> PlayerStartingThings()
		{
			if (this.thingDef.stackLimit == 1)
			{
				int num;
				for (int i = 0; i < this.count; i = num + 1)
				{
					yield return this.GenerateThing(1);
					num = i;
				}
			}
			else
			{
				yield return this.GenerateThing(this.count);
			}
			yield break;
		}

		// Token: 0x0600E559 RID: 58713 RVA: 0x0042EB04 File Offset: 0x0042CD04
		private Thing GenerateThing(int num)
		{
			Thing thing = ThingMaker.MakeThing(this.thingDef, this.stuff);
			CompQuality compQuality;
			if (thing.TryGetComp(out compQuality))
			{
				compQuality.SetQuality(this.quality ?? QualityCategory.Normal, new ArtGenerationContext?(ArtGenerationContext.Outsider));
			}
			if (this.thingDef.Minifiable)
			{
				thing = thing.MakeMinified(DestroyMode.Vanish);
			}
			if (this.thingDef.IsIngestible && this.thingDef.ingestible.IsMeal)
			{
				FoodUtility.GenerateGoodIngredients(thing, Faction.OfPlayer.ideos.PrimaryIdeo);
			}
			thing.stackCount = num;
			return thing;
		}

		// Token: 0x0400996E RID: 39278
		public const string PlayerStartWithTag = "PlayerStartsWith";
	}
}
