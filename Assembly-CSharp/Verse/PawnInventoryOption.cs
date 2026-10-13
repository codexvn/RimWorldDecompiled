using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002BC RID: 700
	public class PawnInventoryOption
	{
		// Token: 0x06001123 RID: 4387 RVA: 0x00050A03 File Offset: 0x0004EC03
		public IEnumerable<Thing> GenerateThings()
		{
			if (Rand.Value < this.skipChance)
			{
				yield break;
			}
			if (this.thingDef != null && this.countRange.max > 0)
			{
				Thing thing = ThingMaker.MakeThing(this.thingDef, null);
				thing.stackCount = this.countRange.RandomInRange;
				yield return thing;
			}
			if (this.subOptionsTakeAll != null)
			{
				foreach (PawnInventoryOption pawnInventoryOption in this.subOptionsTakeAll)
				{
					foreach (Thing thing2 in pawnInventoryOption.GenerateThings())
					{
						yield return thing2;
					}
					IEnumerator<Thing> enumerator2 = null;
				}
				List<PawnInventoryOption>.Enumerator enumerator = default(List<PawnInventoryOption>.Enumerator);
			}
			if (this.subOptionsChooseOne != null)
			{
				PawnInventoryOption pawnInventoryOption2 = this.subOptionsChooseOne.RandomElementByWeight((PawnInventoryOption o) => o.choiceChance);
				foreach (Thing thing3 in pawnInventoryOption2.GenerateThings())
				{
					yield return thing3;
				}
				IEnumerator<Thing> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04000F26 RID: 3878
		public ThingDef thingDef;

		// Token: 0x04000F27 RID: 3879
		public IntRange countRange = IntRange.One;

		// Token: 0x04000F28 RID: 3880
		public float choiceChance = 1f;

		// Token: 0x04000F29 RID: 3881
		public float skipChance;

		// Token: 0x04000F2A RID: 3882
		public List<PawnInventoryOption> subOptionsTakeAll;

		// Token: 0x04000F2B RID: 3883
		public List<PawnInventoryOption> subOptionsChooseOne;
	}
}
