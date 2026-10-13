using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200132E RID: 4910
	public class JobDriver_TakeFromOtherInventory : JobDriver
	{
		// Token: 0x17001304 RID: 4868
		// (get) Token: 0x06007893 RID: 30867 RVA: 0x00248084 File Offset: 0x00246284
		public Pawn ItemHoldingPawn
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Pawn;
			}
		}

		// Token: 0x17001305 RID: 4869
		// (get) Token: 0x06007894 RID: 30868 RVA: 0x002480A5 File Offset: 0x002462A5
		public Pawn_InventoryTracker ItemHoldingInventory
		{
			get
			{
				return base.TargetThingA.ParentHolder as Pawn_InventoryTracker;
			}
		}

		// Token: 0x06007895 RID: 30869 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06007896 RID: 30870 RVA: 0x002480B7 File Offset: 0x002462B7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch, false).FailOn(delegate()
			{
				Pawn itemHoldingPawn = this.ItemHoldingPawn;
				Pawn_InventoryTracker itemHoldingInventory = this.ItemHoldingInventory;
				return itemHoldingPawn != ((itemHoldingInventory != null) ? itemHoldingInventory.pawn : null) || this.ItemHoldingPawn.IsForbidden(this.pawn);
			});
			yield return Toils_Haul.TakeFromOtherInventory(base.TargetThingA, this.pawn.inventory.innerContainer, this.ItemHoldingInventory.innerContainer, -1, TargetIndex.None);
			yield break;
		}

		// Token: 0x040051F7 RID: 20983
		public const TargetIndex ItemIndex = TargetIndex.A;

		// Token: 0x040051F8 RID: 20984
		public const TargetIndex ItemHolderIndex = TargetIndex.B;
	}
}
