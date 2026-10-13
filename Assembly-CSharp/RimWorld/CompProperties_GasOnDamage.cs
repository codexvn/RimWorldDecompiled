using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CA9 RID: 11433
	public class CompProperties_GasOnDamage : CompProperties
	{
		// Token: 0x060100CF RID: 65743 RVA: 0x004A590A File Offset: 0x004A3B0A
		public CompProperties_GasOnDamage()
		{
			this.compClass = typeof(CompGasOnDamage);
		}

		// Token: 0x0400A7C5 RID: 42949
		public GasType type;

		// Token: 0x0400A7C6 RID: 42950
		public float damageFactor = 6f;

		// Token: 0x0400A7C7 RID: 42951
		public DamageDef damage;

		// Token: 0x0400A7C8 RID: 42952
		public bool useStackCountAsFactor;
	}
}
