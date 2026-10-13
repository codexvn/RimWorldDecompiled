using System;

namespace Verse
{
	// Token: 0x02000237 RID: 567
	public class CompProperties_TemperatureDamaged : CompProperties
	{
		// Token: 0x06000EBB RID: 3771 RVA: 0x0004462A File Offset: 0x0004282A
		public CompProperties_TemperatureDamaged()
		{
			this.compClass = typeof(CompTemperatureDamaged);
		}

		// Token: 0x04000985 RID: 2437
		public FloatRange safeTemperatureRange = new FloatRange(-30f, 30f);

		// Token: 0x04000986 RID: 2438
		public int damagePerTickRare = 1;
	}
}
