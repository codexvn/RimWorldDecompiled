using System;

namespace RimWorld
{
	// Token: 0x02002F23 RID: 12067
	public class CompProperties_Targetable_SingleAnimal : CompProperties_Targetable
	{
		// Token: 0x0601109B RID: 69787 RVA: 0x004E63FF File Offset: 0x004E45FF
		public CompProperties_Targetable_SingleAnimal()
		{
			this.compClass = typeof(CompTargetable_SingleAnimal);
		}

		// Token: 0x0400B10E RID: 45326
		public bool allowWildMan = true;
	}
}
