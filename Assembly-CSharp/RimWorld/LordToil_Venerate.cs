using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200172C RID: 5932
	public class LordToil_Venerate : LordToil
	{
		// Token: 0x1700157A RID: 5498
		// (get) Token: 0x06008B8F RID: 35727 RVA: 0x00299181 File Offset: 0x00297381
		private LordToilData_Venerate Data
		{
			get
			{
				return (LordToilData_Venerate)this.data;
			}
		}

		// Token: 0x06008B90 RID: 35728 RVA: 0x00299190 File Offset: 0x00297390
		public LordToil_Venerate(Thing target)
		{
			this.target = target;
			this.data = new LordToilData_Venerate();
		}

		// Token: 0x06008B91 RID: 35729 RVA: 0x002991E0 File Offset: 0x002973E0
		public override void UpdateAllDuties()
		{
			LordToilData_Venerate data = this.Data;
			CellRect cellRect = CellRect.CenteredOn(this.target.Position, this.target.def.size.x, this.target.def.size.z);
			CellRect cellRect2 = cellRect.ExpandedBy(this.FarSpectateRectExpansion);
			SpectateRectSide preferredSpectateSide = this.GetPreferredSpectateSide();
			int num = -1;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.mindState.duty != null && pawn.mindState.duty.def == DutyDefOf.Pilgrims_Spectate && pawn.mindState.duty.spectateRect == cellRect)
				{
					num = i;
					break;
				}
			}
			if (num >= 0 && this.lord.ownedPawns.Count > 1)
			{
				this.MakePawnSpectate(this.lord.ownedPawns[num], cellRect2, preferredSpectateSide);
				data.currentNearVeneratorTicks = this.VenerateHandoverTicksRange.RandomInRange;
				return;
			}
			num = (data.lastNearVeneratorIndex + 1) % this.lord.ownedPawns.Count;
			for (int j = 0; j < this.lord.ownedPawns.Count; j++)
			{
				Pawn pawn2 = this.lord.ownedPawns[j];
				CellRect cellRect3 = (num == j) ? cellRect : cellRect2;
				if (pawn2.mindState.duty == null || pawn2.mindState.duty.def != DutyDefOf.Pilgrims_Spectate || pawn2.mindState.duty.spectateRect != cellRect3)
				{
					this.MakePawnSpectate(pawn2, cellRect3, preferredSpectateSide);
				}
			}
			data.currentNearVeneratorTicks = this.VenerateCloseTicksRange.RandomInRange;
			data.lastNearVeneratorIndex = num;
		}

		// Token: 0x06008B92 RID: 35730 RVA: 0x002993C4 File Offset: 0x002975C4
		private void MakePawnSpectate(Pawn p, CellRect rect, SpectateRectSide side)
		{
			PawnDuty pawnDuty = new PawnDuty(DutyDefOf.Pilgrims_Spectate);
			pawnDuty.spectateRect = rect;
			pawnDuty.spectateRectAllowedSides = side;
			pawnDuty.locomotion = LocomotionUrgency.Amble;
			pawnDuty.focus = this.target;
			p.mindState.duty = pawnDuty;
			if (p.jobs.curJob != null)
			{
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06008B93 RID: 35731 RVA: 0x0029942A File Offset: 0x0029762A
		public override void LordToilTick()
		{
			LordToilData_Venerate data = this.Data;
			if (data.currentNearVeneratorTicks < 0)
			{
				this.UpdateAllDuties();
			}
			data.currentNearVeneratorTicks--;
		}

		// Token: 0x06008B94 RID: 35732 RVA: 0x00299450 File Offset: 0x00297650
		private SpectateRectSide GetPreferredSpectateSide()
		{
			IntVec3 interactionCell = this.target.InteractionCell;
			IntVec3 intVec = this.target.Position - interactionCell;
			if (intVec.x > 0)
			{
				return SpectateRectSide.Left;
			}
			if (intVec.x < 0)
			{
				return SpectateRectSide.Right;
			}
			if (intVec.z > 0)
			{
				return SpectateRectSide.Down;
			}
			if (intVec.z < 0)
			{
				return SpectateRectSide.Up;
			}
			return SpectateRectSide.All;
		}

		// Token: 0x04005AAA RID: 23210
		private Thing target;

		// Token: 0x04005AAB RID: 23211
		private int FarSpectateRectExpansion = 2;

		// Token: 0x04005AAC RID: 23212
		private IntRange VenerateCloseTicksRange = new IntRange(750, 1500);

		// Token: 0x04005AAD RID: 23213
		private IntRange VenerateHandoverTicksRange = new IntRange(60, 120);
	}
}
