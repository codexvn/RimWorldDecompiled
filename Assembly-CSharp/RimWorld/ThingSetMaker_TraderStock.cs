using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B8E RID: 11150
	public class ThingSetMaker_TraderStock : ThingSetMaker
	{
		// Token: 0x0600F951 RID: 63825 RVA: 0x004877BC File Offset: 0x004859BC
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			TraderKindDef traderKindDef = parms.traderDef ?? DefDatabase<TraderKindDef>.AllDefsListForReading.RandomElement<TraderKindDef>();
			Faction makingFaction = parms.makingFaction;
			PlanetTile forTile;
			if (parms.tile != null)
			{
				forTile = parms.tile.Value;
			}
			else if (Find.AnyPlayerHomeMap != null)
			{
				forTile = Find.AnyPlayerHomeMap.Tile;
			}
			else if (Find.CurrentMap != null)
			{
				forTile = Find.CurrentMap.Tile;
			}
			else
			{
				forTile = PlanetTile.Invalid;
			}
			for (int i = 0; i < traderKindDef.stockGenerators.Count; i++)
			{
				foreach (Thing thing in traderKindDef.stockGenerators[i].GenerateThings(forTile, parms.makingFaction))
				{
					if (!thing.def.tradeability.TraderCanSell())
					{
						TraderKindDef traderKindDef2 = traderKindDef;
						string str = (traderKindDef2 != null) ? traderKindDef2.ToString() : null;
						string str2 = " generated carrying ";
						Thing thing2 = thing;
						Log.Error(str + str2 + ((thing2 != null) ? thing2.ToString() : null) + " which can't be sold by traders. Ignoring...");
					}
					else
					{
						thing.PostGeneratedForTrader(traderKindDef, forTile, makingFaction);
						outThings.Add(thing);
					}
				}
			}
		}

		// Token: 0x0600F952 RID: 63826 RVA: 0x004878F4 File Offset: 0x00485AF4
		public float DebugAverageTotalStockValue(TraderKindDef td)
		{
			ThingSetMakerParams parms = new ThingSetMakerParams
			{
				traderDef = td,
				tile = new PlanetTile?(PlanetTile.Invalid)
			};
			float num = 0f;
			for (int i = 0; i < 50; i++)
			{
				foreach (Thing thing in base.Generate(parms))
				{
					num += thing.MarketValue * (float)thing.stackCount;
				}
			}
			return num / 50f;
		}

		// Token: 0x0600F953 RID: 63827 RVA: 0x00487994 File Offset: 0x00485B94
		public string DebugGenerationDataFor(TraderKindDef td)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(td.defName);
			stringBuilder.AppendLine("Average total market value:" + this.DebugAverageTotalStockValue(td).ToString("F0"));
			ThingSetMakerParams parms = new ThingSetMakerParams
			{
				traderDef = td,
				tile = new PlanetTile?(PlanetTile.Invalid)
			};
			(from x in Find.FactionManager.AllFactionsListForReading
			where x.def.baseTraderKinds.Contains(td) || x.def.visitorTraderKinds.Contains(td) || x.def.caravanTraderKinds.Contains(td)
			select x).TryRandomElement(out parms.makingFaction);
			stringBuilder.AppendLine("Example generated stock:\n\n");
			foreach (Thing thing in base.Generate(parms))
			{
				MinifiedThing minifiedThing = thing as MinifiedThing;
				Thing thing2;
				if (minifiedThing != null)
				{
					thing2 = minifiedThing.InnerThing;
				}
				else
				{
					thing2 = thing;
				}
				string text = thing2.LabelCap;
				Book book = thing2 as Book;
				if (book != null)
				{
					text = "[" + book.def.defName + "]: " + text;
				}
				text = text + " [" + (thing2.MarketValue * (float)thing2.stackCount).ToString("F0") + "]";
				stringBuilder.AppendLine(text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600F954 RID: 63828 RVA: 0x00487B28 File Offset: 0x00485D28
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			if (parms.traderDef == null)
			{
				yield break;
			}
			int num;
			for (int i = 0; i < parms.traderDef.stockGenerators.Count; i = num + 1)
			{
				StockGenerator stock = parms.traderDef.stockGenerators[i];
				IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
				Func<ThingDef, bool> predicate;
				Func<ThingDef, bool> <>9__0;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((ThingDef x) => x.tradeability.TraderCanSell() && stock.HandlesThingDef(x)));
				}
				foreach (ThingDef thingDef in allDefs.Where(predicate))
				{
					yield return thingDef;
				}
				IEnumerator<ThingDef> enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}
	}
}
