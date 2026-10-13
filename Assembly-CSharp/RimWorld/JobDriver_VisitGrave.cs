using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011F3 RID: 4595
	public class JobDriver_VisitGrave : JobDriver_VisitJoyThing
	{
		// Token: 0x1700116B RID: 4459
		// (get) Token: 0x06007045 RID: 28741 RVA: 0x0022DAE0 File Offset: 0x0022BCE0
		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06007046 RID: 28742 RVA: 0x0022DB08 File Offset: 0x0022BD08
		protected override void WaitTickAction(int delta)
		{
			float num = 1f;
			Room room = this.pawn.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				num *= room.GetStat(RoomStatDefOf.GraveVisitingJoyGainFactor);
			}
			this.pawn.GainComfortFromCellIfPossible(delta, false);
			JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.EndJob, num, this.Grave);
		}

		// Token: 0x06007047 RID: 28743 RVA: 0x0022DB5C File Offset: 0x0022BD5C
		public override object[] TaleParameters()
		{
			object[] array = new object[2];
			array[0] = this.pawn;
			int num = 1;
			Corpse corpse = this.Grave.Corpse;
			array[num] = ((corpse != null) ? corpse.InnerPawn : null);
			return array;
		}
	}
}
