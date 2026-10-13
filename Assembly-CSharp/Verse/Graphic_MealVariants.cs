using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020008A2 RID: 2210
	public class Graphic_MealVariants : Graphic_StackCount
	{
		// Token: 0x060037F1 RID: 14321 RVA: 0x0012ADB7 File Offset: 0x00128FB7
		public override Graphic SubGraphicFor(Thing thing)
		{
			return this.subGraphics[this.SubGraphicTypeIndex(thing) + this.SubGraphicIndexOffset(thing)];
		}

		// Token: 0x060037F2 RID: 14322 RVA: 0x0012ADD0 File Offset: 0x00128FD0
		public int SubGraphicTypeIndex(Thing thing)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return 0;
			}
			FoodKind foodKind = FoodUtility.GetFoodKind(thing);
			if (foodKind == FoodKind.Meat)
			{
				return this.subGraphics.Length / 3;
			}
			if (foodKind != FoodKind.NonMeat)
			{
				return 0;
			}
			return this.subGraphics.Length / 3 * 2;
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x0012AE10 File Offset: 0x00129010
		public int SubGraphicIndexOffset(Thing thing)
		{
			if (thing == null)
			{
				return 0;
			}
			switch (this.subGraphics.Length / 3)
			{
			case 1:
				return 0;
			case 2:
				if (thing.stackCount == 1)
				{
					return 0;
				}
				return 1;
			case 3:
				if (thing.stackCount == 1)
				{
					return 0;
				}
				if (thing.stackCount == thing.def.stackLimit)
				{
					return 2;
				}
				return 1;
			default:
				throw new NotImplementedException("More than 3 different stack size meal graphics per meal type not supported");
			}
		}
	}
}
