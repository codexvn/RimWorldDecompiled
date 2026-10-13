using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036F9 RID: 14073
	public class StatPart_Food : StatPart
	{
		// Token: 0x0601383C RID: 79932 RVA: 0x005CA7B0 File Offset: 0x005C89B0
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.food != null)
				{
					val *= this.FoodMultiplier(pawn.needs.food.CurCategory);
				}
			}
		}

		// Token: 0x0601383D RID: 79933 RVA: 0x005CA800 File Offset: 0x005C8A00
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.food != null)
				{
					return pawn.needs.food.CurCategory.GetLabel() + ": x" + this.FoodMultiplier(pawn.needs.food.CurCategory).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0601383E RID: 79934 RVA: 0x005CA86F File Offset: 0x005C8A6F
		private float FoodMultiplier(HungerCategory hunger)
		{
			switch (hunger)
			{
			case HungerCategory.Fed:
				return this.factorFed;
			case HungerCategory.Hungry:
				return this.factorHungry;
			case HungerCategory.UrgentlyHungry:
				return this.factorUrgentlyHungry;
			case HungerCategory.Starving:
				return this.factorStarving;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0400D8A0 RID: 55456
		public float factorStarving = 1f;

		// Token: 0x0400D8A1 RID: 55457
		public float factorUrgentlyHungry = 1f;

		// Token: 0x0400D8A2 RID: 55458
		public float factorHungry = 1f;

		// Token: 0x0400D8A3 RID: 55459
		public float factorFed = 1f;
	}
}
