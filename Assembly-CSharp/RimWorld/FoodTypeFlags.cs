using System;

namespace RimWorld
{
	// Token: 0x02001999 RID: 6553
	[Flags]
	public enum FoodTypeFlags
	{
		// Token: 0x0400601D RID: 24605
		None = 0,
		// Token: 0x0400601E RID: 24606
		VegetableOrFruit = 1,
		// Token: 0x0400601F RID: 24607
		Meat = 2,
		// Token: 0x04006020 RID: 24608
		Fluid = 4,
		// Token: 0x04006021 RID: 24609
		Corpse = 8,
		// Token: 0x04006022 RID: 24610
		Seed = 16,
		// Token: 0x04006023 RID: 24611
		AnimalProduct = 32,
		// Token: 0x04006024 RID: 24612
		Plant = 64,
		// Token: 0x04006025 RID: 24613
		Tree = 128,
		// Token: 0x04006026 RID: 24614
		Meal = 256,
		// Token: 0x04006027 RID: 24615
		Processed = 512,
		// Token: 0x04006028 RID: 24616
		Liquor = 1024,
		// Token: 0x04006029 RID: 24617
		Kibble = 2048,
		// Token: 0x0400602A RID: 24618
		Fungus = 4097,
		// Token: 0x0400602B RID: 24619
		VegetarianAnimal = 7953,
		// Token: 0x0400602C RID: 24620
		VegetarianRoughAnimal = 8017,
		// Token: 0x0400602D RID: 24621
		CarnivoreAnimal = 2826,
		// Token: 0x0400602E RID: 24622
		CarnivoreAnimalStrict = 10,
		// Token: 0x0400602F RID: 24623
		OmnivoreAnimal = 7963,
		// Token: 0x04006030 RID: 24624
		OmnivoreRoughAnimal = 8027,
		// Token: 0x04006031 RID: 24625
		DendrovoreAnimal = 6801,
		// Token: 0x04006032 RID: 24626
		OvivoreAnimal = 2848,
		// Token: 0x04006033 RID: 24627
		OmnivoreHuman = 7999
	}
}
