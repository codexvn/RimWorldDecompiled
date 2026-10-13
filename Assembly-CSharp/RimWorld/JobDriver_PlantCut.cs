using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001365 RID: 4965
	public class JobDriver_PlantCut : JobDriver_PlantWork
	{
		// Token: 0x17001337 RID: 4919
		// (get) Token: 0x060079D4 RID: 31188 RVA: 0x00075162 File Offset: 0x00073362
		protected override PlantDestructionMode PlantDestructionMode
		{
			get
			{
				return PlantDestructionMode.Cut;
			}
		}

		// Token: 0x060079D5 RID: 31189 RVA: 0x0024C440 File Offset: 0x0024A640
		protected override void Init()
		{
			Plant plant = base.Plant;
			if (((plant != null) ? plant.def.plant.harvestedThingDef : null) != null && base.Plant.CanYieldNow())
			{
				this.xpPerTick = 0.085f;
				return;
			}
			this.xpPerTick = 0f;
		}

		// Token: 0x060079D6 RID: 31190 RVA: 0x0024C48F File Offset: 0x0024A68F
		protected override Toil PlantWorkDoneToil()
		{
			return Toils_Interact.DestroyThing(TargetIndex.A);
		}
	}
}
