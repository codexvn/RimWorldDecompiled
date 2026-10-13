using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B14 RID: 6932
	public class TraderKindDef : Def
	{
		// Token: 0x170017CE RID: 6094
		// (get) Token: 0x06009AD5 RID: 39637 RVA: 0x002CF878 File Offset: 0x002CDA78
		public float CalculatedCommonality
		{
			get
			{
				float num = this.commonality;
				if (this.commonalityMultFromPopulationIntent != null)
				{
					num *= this.commonalityMultFromPopulationIntent.Evaluate(StorytellerUtilityPopulation.PopulationIntent);
				}
				return num;
			}
		}

		// Token: 0x170017CF RID: 6095
		// (get) Token: 0x06009AD6 RID: 39638 RVA: 0x002CF8A8 File Offset: 0x002CDAA8
		public RoyalTitleDef TitleRequiredToTrade
		{
			get
			{
				if (this.permitRequiredForTrading != null)
				{
					RoyalTitleDef royalTitleDef = this.faction.RoyalTitlesAwardableInSeniorityOrderForReading.FirstOrDefault((RoyalTitleDef x) => x.permits != null && x.permits.Contains(this.permitRequiredForTrading));
					if (royalTitleDef != null)
					{
						return royalTitleDef;
					}
				}
				return null;
			}
		}

		// Token: 0x06009AD7 RID: 39639 RVA: 0x002CF8E0 File Offset: 0x002CDAE0
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			foreach (StockGenerator stockGenerator in this.stockGenerators)
			{
				stockGenerator.ResolveReferences(this);
			}
		}

		// Token: 0x06009AD8 RID: 39640 RVA: 0x002CF938 File Offset: 0x002CDB38
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			foreach (StockGenerator stockGenerator in this.stockGenerators)
			{
				foreach (string text2 in stockGenerator.ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
			}
			List<StockGenerator>.Enumerator enumerator2 = default(List<StockGenerator>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06009AD9 RID: 39641 RVA: 0x002CF948 File Offset: 0x002CDB48
		public bool WillTrade(ThingDef td)
		{
			for (int i = 0; i < this.stockGenerators.Count; i++)
			{
				if (this.stockGenerators[i].HandlesThingDef(td))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009ADA RID: 39642 RVA: 0x002CF984 File Offset: 0x002CDB84
		public PriceType PriceTypeFor(ThingDef thingDef, TradeAction action)
		{
			if (thingDef == ThingDefOf.Silver)
			{
				return PriceType.Undefined;
			}
			if (action == TradeAction.PlayerBuys)
			{
				for (int i = 0; i < this.stockGenerators.Count; i++)
				{
					PriceType result;
					if (this.stockGenerators[i].TryGetPriceType(thingDef, action, out result))
					{
						return result;
					}
				}
			}
			return PriceType.Normal;
		}

		// Token: 0x04006AF4 RID: 27380
		public List<StockGenerator> stockGenerators = new List<StockGenerator>();

		// Token: 0x04006AF5 RID: 27381
		public bool orbital;

		// Token: 0x04006AF6 RID: 27382
		public bool requestable = true;

		// Token: 0x04006AF7 RID: 27383
		public bool hideThingsNotWillingToTrade;

		// Token: 0x04006AF8 RID: 27384
		public float commonality = 1f;

		// Token: 0x04006AF9 RID: 27385
		public string category;

		// Token: 0x04006AFA RID: 27386
		public TradeCurrency tradeCurrency;

		// Token: 0x04006AFB RID: 27387
		public SimpleCurve commonalityMultFromPopulationIntent;

		// Token: 0x04006AFC RID: 27388
		public FactionDef faction;

		// Token: 0x04006AFD RID: 27389
		public RoyalTitlePermitDef permitRequiredForTrading;
	}
}
