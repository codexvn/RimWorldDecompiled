using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B60 RID: 11104
	public class ThingSetMaker_RandomOption : ThingSetMaker
	{
		// Token: 0x0600F846 RID: 63558 RVA: 0x004834C8 File Offset: 0x004816C8
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			for (int i = 0; i < this.options.Count; i++)
			{
				if (this.options[i].thingSetMaker.CanGenerate(parms) && this.GetSelectionWeight(this.options[i], parms) > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600F847 RID: 63559 RVA: 0x00483524 File Offset: 0x00481724
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			ThingSetMaker_RandomOption.Option option;
			if (!(from x in this.options
			where x.thingSetMaker.CanGenerate(parms)
			select x).TryRandomElementByWeight((ThingSetMaker_RandomOption.Option x) => this.GetSelectionWeight(x, parms), out option))
			{
				return;
			}
			outThings.AddRange(option.thingSetMaker.Generate(parms));
		}

		// Token: 0x0600F848 RID: 63560 RVA: 0x0048358C File Offset: 0x0048178C
		private float GetSelectionWeight(ThingSetMaker_RandomOption.Option option, ThingSetMakerParams parms)
		{
			if (option.weightIfPlayerHasNoItem != null && !PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(option.weightIfPlayerHasNoItemItem, 1))
			{
				return option.weightIfPlayerHasNoItem.Value * option.thingSetMaker.ExtraSelectionWeightFactor(parms);
			}
			if (ModsConfig.BiotechActive && option.weightIfPlayerHasXenotype != null && option.weightIfPlayerHasXenotypeXenotype != null && PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists_OfXenotype(option.weightIfPlayerHasXenotypeXenotype).Count > 0)
			{
				return option.weightIfPlayerHasXenotype.Value * option.thingSetMaker.ExtraSelectionWeightFactor(parms);
			}
			return option.weight * option.thingSetMaker.ExtraSelectionWeightFactor(parms);
		}

		// Token: 0x0600F849 RID: 63561 RVA: 0x00483628 File Offset: 0x00481828
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.options.Count; i++)
			{
				this.options[i].thingSetMaker.ResolveReferences();
			}
		}

		// Token: 0x0600F84A RID: 63562 RVA: 0x00483667 File Offset: 0x00481867
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			int num2;
			for (int i = 0; i < this.options.Count; i = num2 + 1)
			{
				float num = this.options[i].weight;
				if (this.options[i].weightIfPlayerHasNoItem != null)
				{
					num = Mathf.Max(num, this.options[i].weightIfPlayerHasNoItem.Value);
				}
				if (num > 0f)
				{
					foreach (ThingDef thingDef in this.options[i].thingSetMaker.AllGeneratableThingsDebug(parms))
					{
						yield return thingDef;
					}
					IEnumerator<ThingDef> enumerator = null;
				}
				num2 = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F84B RID: 63563 RVA: 0x0048367E File Offset: 0x0048187E
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.options.NullOrEmpty<ThingSetMaker_RandomOption.Option>())
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
						yield return "options[" + i.ToString() + "]=null.";
					}
					else
					{
						foreach (string str in this.options[i].thingSetMaker.ConfigErrors())
						{
							yield return "options[" + i.ToString() + "]: " + str;
						}
						IEnumerator<string> enumerator = null;
					}
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0400A341 RID: 41793
		public List<ThingSetMaker_RandomOption.Option> options;

		// Token: 0x02002B61 RID: 11105
		public class Option
		{
			// Token: 0x0400A342 RID: 41794
			public ThingSetMaker thingSetMaker;

			// Token: 0x0400A343 RID: 41795
			public float weight;

			// Token: 0x0400A344 RID: 41796
			public float? weightIfPlayerHasNoItem;

			// Token: 0x0400A345 RID: 41797
			public ThingDef weightIfPlayerHasNoItemItem;

			// Token: 0x0400A346 RID: 41798
			public float? weightIfPlayerHasXenotype;

			// Token: 0x0400A347 RID: 41799
			public XenotypeDef weightIfPlayerHasXenotypeXenotype;
		}
	}
}
