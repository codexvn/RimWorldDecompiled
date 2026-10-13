using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D2C RID: 11564
	public class CompProperties_MechCarrier : CompProperties
	{
		// Token: 0x060103EB RID: 66539 RVA: 0x004B23A0 File Offset: 0x004B05A0
		public CompProperties_MechCarrier()
		{
			this.compClass = typeof(CompMechCarrier);
		}

		// Token: 0x060103EC RID: 66540 RVA: 0x004B23CA File Offset: 0x004B05CA
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400A9A4 RID: 43428
		public ThingDef fixedIngredient;

		// Token: 0x0400A9A5 RID: 43429
		public int costPerPawn;

		// Token: 0x0400A9A6 RID: 43430
		public int maxIngredientCount;

		// Token: 0x0400A9A7 RID: 43431
		public int startingIngredientCount;

		// Token: 0x0400A9A8 RID: 43432
		public PawnKindDef spawnPawnKind;

		// Token: 0x0400A9A9 RID: 43433
		public int cooldownTicks = 900;

		// Token: 0x0400A9AA RID: 43434
		public int maxPawnsToSpawn = 3;

		// Token: 0x0400A9AB RID: 43435
		public EffecterDef spawnEffecter;

		// Token: 0x0400A9AC RID: 43436
		public EffecterDef spawnedMechEffecter;

		// Token: 0x0400A9AD RID: 43437
		public bool attachSpawnedEffecter;

		// Token: 0x0400A9AE RID: 43438
		public bool attachSpawnedMechEffecter;
	}
}
