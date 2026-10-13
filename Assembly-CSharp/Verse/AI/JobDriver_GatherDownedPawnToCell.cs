using System;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x02000EC1 RID: 3777
	public class JobDriver_GatherDownedPawnToCell : JobDriver_HaulToCell
	{
		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x06005F04 RID: 24324 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool DropCarriedThingIfNotTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005F05 RID: 24325 RVA: 0x001E34E3 File Offset: 0x001E16E3
		protected override Toil BeforeDrop()
		{
			return Toils_General.DoAtomic(delegate
			{
				Pawn pawn = this.pawn.carryTracker.CarriedThing as Pawn;
				if (pawn == null)
				{
					Log.Error("Carried thing wasn't a Pawn in JobDriver_GatherDownedPawnToCell.");
					return;
				}
				LordToil curLordToil = this.pawn.GetLord().CurLordToil;
				if (curLordToil == null)
				{
					return;
				}
				curLordToil.Notify_ReachedDutyLocation(pawn);
			});
		}
	}
}
