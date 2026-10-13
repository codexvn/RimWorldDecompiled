using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011BD RID: 4541
	public class JobDriver_StandAndBeSociallyActive : JobDriver
	{
		// Token: 0x06006F0B RID: 28427 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06006F0C RID: 28428 RVA: 0x002293D3 File Offset: 0x002275D3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (base.TargetLocA.IsValid)
			{
				yield return Toils_Goto.GotoCell(base.TargetLocA, PathEndMode.OnCell);
			}
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.pawn.pather.StopDead();
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				this.pawn.pather.StopDead();
				if (!this.job.forceMaintainFacing)
				{
					if (this.job.lookDirection != Direction8Way.Invalid)
					{
						this.pawn.rotationTracker.Face(this.pawn.Position.ToVector3() + this.job.lookDirection.AsVector());
					}
					else
					{
						Pawn pawn = JobDriver_StandAndBeSociallyActive.FindClosePawn(this.pawn);
						if (pawn != null)
						{
							this.pawn.rotationTracker.FaceCell(pawn.Position);
						}
					}
				}
				this.pawn.GainComfortFromCellIfPossible(delta, false);
			};
			toil.socialMode = RandomSocialMode.SuperActive;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.handlingFacing = true;
			yield return toil;
			yield break;
		}

		// Token: 0x06006F0D RID: 28429 RVA: 0x002293E4 File Offset: 0x002275E4
		public static Pawn FindClosePawn(Pawn pawn)
		{
			IntVec3 position = pawn.Position;
			Map map = pawn.Map;
			for (int i = 0; i < 24; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					Thing thing = intVec.GetThingList(map).Find((Thing x) => x is Pawn);
					if (thing != null && thing != pawn && GenSight.LineOfSight(position, intVec, map))
					{
						return (Pawn)thing;
					}
				}
			}
			return null;
		}
	}
}
