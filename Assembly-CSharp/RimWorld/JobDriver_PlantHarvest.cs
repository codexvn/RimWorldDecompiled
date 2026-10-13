using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001364 RID: 4964
	public class JobDriver_PlantHarvest : JobDriver_PlantWork
	{
		// Token: 0x17001336 RID: 4918
		// (get) Token: 0x060079D0 RID: 31184 RVA: 0x000E96BB File Offset: 0x000E78BB
		protected override PlantDestructionMode PlantDestructionMode
		{
			get
			{
				return PlantDestructionMode.Chop;
			}
		}

		// Token: 0x060079D1 RID: 31185 RVA: 0x0024C41B File Offset: 0x0024A61B
		protected override void Init()
		{
			this.xpPerTick = 0.085f;
		}

		// Token: 0x060079D2 RID: 31186 RVA: 0x0024C428 File Offset: 0x0024A628
		protected override Toil PlantWorkDoneToil()
		{
			return Toils_General.RemoveDesignationsOnThing(TargetIndex.A, DesignationDefOf.HarvestPlant);
		}
	}
}
