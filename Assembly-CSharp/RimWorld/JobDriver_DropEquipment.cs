using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013BF RID: 5055
	public class JobDriver_DropEquipment : JobDriver
	{
		// Token: 0x17001379 RID: 4985
		// (get) Token: 0x06007BB6 RID: 31670 RVA: 0x00254428 File Offset: 0x00252628
		private ThingWithComps TargetEquipment
		{
			get
			{
				return (ThingWithComps)base.TargetA.Thing;
			}
		}

		// Token: 0x06007BB7 RID: 31671 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06007BB8 RID: 31672 RVA: 0x00254448 File Offset: 0x00252648
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.pawn.pather.StopDead();
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 30;
			yield return toil;
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.initAction = delegate()
			{
				ThingWithComps thingWithComps;
				if (!this.pawn.equipment.TryDropEquipment(this.TargetEquipment, out thingWithComps, this.pawn.Position, true))
				{
					base.EndJobWith(JobCondition.Incompletable);
				}
			};
			yield return toil2;
			yield break;
		}

		// Token: 0x0400539E RID: 21406
		private const int DurationTicks = 30;
	}
}
