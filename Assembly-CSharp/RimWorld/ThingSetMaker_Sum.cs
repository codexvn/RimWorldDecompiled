using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B66 RID: 11110
	public class ThingSetMaker_Sum : ThingSetMaker
	{
		// Token: 0x0600F867 RID: 63591 RVA: 0x00483B9C File Offset: 0x00481D9C
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			for (int i = 0; i < this.options.Count; i++)
			{
				if (this.options[i].chance > 0f && this.options[i].thingSetMaker.CanGenerate(parms))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600F868 RID: 63592 RVA: 0x00483BF4 File Offset: 0x00481DF4
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			int num = 0;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			this.optionsInResolveOrder.Clear();
			this.optionsInResolveOrder.AddRange(this.options);
			if (!this.resolveInOrder)
			{
				this.optionsInResolveOrder.Shuffle<ThingSetMaker_Sum.Option>();
			}
			int i = 0;
			while (i < this.optionsInResolveOrder.Count)
			{
				ThingSetMakerParams parms2 = parms;
				if (parms2.countRange == null)
				{
					goto IL_B0;
				}
				parms2.countRange = new IntRange?(new IntRange(Mathf.Max(parms2.countRange.Value.min - num, 0), parms2.countRange.Value.max - num));
				if (parms2.countRange.Value.max > 0)
				{
					goto IL_B0;
				}
				IL_3D6:
				i++;
				continue;
				IL_B0:
				if (parms2.totalMarketValueRange != null)
				{
					if (parms2.totalMarketValueRange.Value.max < this.optionsInResolveOrder[i].minTotalMarketValue)
					{
						goto IL_3D6;
					}
					float min = (i < this.optionsInResolveOrder.Count - 1) ? 0f : Mathf.Max(parms2.totalMarketValueRange.Value.min - num2, 0f);
					float max = parms2.totalMarketValueRange.Value.max - num2;
					parms2.totalMarketValueRange = new FloatRange?(new FloatRange(min, max));
					if (this.optionsInResolveOrder[i].maxMarketValue != null)
					{
						parms2.totalMarketValueRange = new FloatRange?(new FloatRange(Mathf.Min(parms2.totalMarketValueRange.Value.min, this.optionsInResolveOrder[i].maxMarketValue.Value * 0.8f), Mathf.Min(new float[]
						{
							Mathf.Min(parms2.totalMarketValueRange.Value.max, this.optionsInResolveOrder[i].maxMarketValue.Value)
						})));
					}
					if (parms2.totalMarketValueRange.Value.max <= 0f || parms2.totalMarketValueRange.Value.max < this.optionsInResolveOrder[i].minMarketValue)
					{
						goto IL_3D6;
					}
				}
				if (parms2.totalNutritionRange != null)
				{
					parms2.totalNutritionRange = new FloatRange?(new FloatRange(Mathf.Max(parms2.totalNutritionRange.Value.min - num3, 0f), parms2.totalNutritionRange.Value.max - num3));
				}
				if (parms2.maxTotalMass != null)
				{
					parms2.maxTotalMass -= num4;
				}
				if (Rand.Chance(this.optionsInResolveOrder[i].chance) && this.optionsInResolveOrder[i].thingSetMaker.CanGenerate(parms2))
				{
					List<Thing> list = this.optionsInResolveOrder[i].thingSetMaker.Generate(parms2);
					num += list.Count;
					for (int j = 0; j < list.Count; j++)
					{
						num2 += list[j].MarketValue * (float)list[j].stackCount;
						if (list[j].def.IsIngestible)
						{
							num3 += list[j].GetStatValue(StatDefOf.Nutrition, true, -1) * (float)list[j].stackCount;
						}
						if (!(list[j] is Pawn))
						{
							num4 += list[j].GetStatValue(StatDefOf.Mass, true, -1) * (float)list[j].stackCount;
						}
					}
					outThings.AddRange(list);
					goto IL_3D6;
				}
				goto IL_3D6;
			}
		}

		// Token: 0x0600F869 RID: 63593 RVA: 0x00483FF0 File Offset: 0x004821F0
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.options.Count; i++)
			{
				this.options[i].thingSetMaker.ResolveReferences();
			}
		}

		// Token: 0x0600F86A RID: 63594 RVA: 0x0048402F File Offset: 0x0048222F
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			int num;
			for (int i = 0; i < this.options.Count; i = num + 1)
			{
				if (this.options[i].chance > 0f)
				{
					foreach (ThingDef thingDef in this.options[i].thingSetMaker.AllGeneratableThingsDebug(parms))
					{
						yield return thingDef;
					}
					IEnumerator<ThingDef> enumerator = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F86B RID: 63595 RVA: 0x00484046 File Offset: 0x00482246
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.options.NullOrEmpty<ThingSetMaker_Sum.Option>())
			{
				yield return "no options.";
			}
			else
			{
				int num;
				for (int i = 0; i < this.options.Count; i = num + 1)
				{
					if (this.options[i].thingSetMaker == null)
					{
						yield return "null thingSetMaker";
					}
					else
					{
						foreach (string text in this.options[i].thingSetMaker.ConfigErrors())
						{
							yield return text;
						}
						IEnumerator<string> enumerator = null;
					}
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0400A359 RID: 41817
		public List<ThingSetMaker_Sum.Option> options;

		// Token: 0x0400A35A RID: 41818
		public bool resolveInOrder;

		// Token: 0x0400A35B RID: 41819
		private List<ThingSetMaker_Sum.Option> optionsInResolveOrder = new List<ThingSetMaker_Sum.Option>();

		// Token: 0x02002B67 RID: 11111
		public class Option
		{
			// Token: 0x0400A35C RID: 41820
			public ThingSetMaker thingSetMaker;

			// Token: 0x0400A35D RID: 41821
			public float chance = 1f;

			// Token: 0x0400A35E RID: 41822
			public float? maxMarketValue;

			// Token: 0x0400A35F RID: 41823
			public float minMarketValue;

			// Token: 0x0400A360 RID: 41824
			public float minTotalMarketValue;

			// Token: 0x0400A361 RID: 41825
			public const float MaxMarketValueLeewayFactor = 0.8f;
		}
	}
}
