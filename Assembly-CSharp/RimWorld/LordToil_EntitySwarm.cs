using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016F5 RID: 5877
	public abstract class LordToil_EntitySwarm : LordToil
	{
		// Token: 0x17001547 RID: 5447
		// (get) Token: 0x06008A9A RID: 35482 RVA: 0x00295538 File Offset: 0x00293738
		protected LordToilData_EntitySwarm Data
		{
			get
			{
				return (LordToilData_EntitySwarm)this.data;
			}
		}

		// Token: 0x06008A9B RID: 35483
		protected abstract DutyDef GetDutyDef();

		// Token: 0x06008A9C RID: 35484 RVA: 0x00295545 File Offset: 0x00293745
		public LordToil_EntitySwarm(IntVec3 start, IntVec3 dest)
		{
			this.data = new LordToilData_EntitySwarm
			{
				pos = start,
				dest = dest,
				lastMoved = GenTicks.TicksGame
			};
		}

		// Token: 0x06008A9D RID: 35485 RVA: 0x00295574 File Offset: 0x00293774
		private void GetPath()
		{
			this.path = this.lord.Map.pathFinder.FindPathNow(this.Data.pos, this.Data.dest, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), null, PathEndMode.OnCell, null);
		}

		// Token: 0x06008A9E RID: 35486 RVA: 0x002955D0 File Offset: 0x002937D0
		public override void Init()
		{
			this.GetPath();
			for (int i = 0; i < 7; i++)
			{
				this.Data.pos = this.path.ConsumeNextNode();
			}
		}

		// Token: 0x06008A9F RID: 35487 RVA: 0x00295608 File Offset: 0x00293808
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(this.GetDutyDef());
			}
		}

		// Token: 0x06008AA0 RID: 35488 RVA: 0x00295658 File Offset: 0x00293858
		public override void LordToilTick()
		{
			if (this.path == null)
			{
				this.GetPath();
			}
			if (this.path.NodesLeftCount <= 1 || !this.path.Found || this.lord.ownedPawns.Count == 0)
			{
				this.lord.ReceiveMemo("TravelArrived");
				return;
			}
			if (Find.TickManager.TicksGame > this.Data.lastMoved + 600)
			{
				this.Data.pos = this.path.ConsumeNextNode();
				this.Data.lastMoved = Find.TickManager.TicksGame;
			}
			int index = Find.TickManager.TicksGame % this.lord.ownedPawns.Count;
			Pawn pawn = this.lord.ownedPawns[index];
			if (!pawn.Position.InHorDistOf(this.Data.pos, 7f) && !pawn.pather.Moving)
			{
				IntVec3 c;
				CellFinder.TryFindRandomReachableCellNearPosition(pawn.Position, this.Data.pos, this.lord.Map, 7f, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), (IntVec3 x) => x.Standable(this.lord.Map), null, out c, 999999);
				if (!c.IsValid)
				{
					this.lord.RemovePawn(pawn);
				}
				pawn.mindState.duty = new PawnDuty(this.GetDutyDef(), c, -1f);
			}
			base.LordToilTick();
		}

		// Token: 0x06008AA1 RID: 35489 RVA: 0x002957DC File Offset: 0x002939DC
		public override void Notify_PawnAcquiredTarget(Pawn detector, Thing newTarg)
		{
			this.lord.Notify_PawnLost(detector, PawnLostCondition.LeftVoluntarily, null);
		}

		// Token: 0x04005A41 RID: 23105
		private PawnPath path;

		// Token: 0x04005A42 RID: 23106
		private const int SwarmRadius = 7;

		// Token: 0x04005A43 RID: 23107
		private const int MoveInterval = 600;
	}
}
