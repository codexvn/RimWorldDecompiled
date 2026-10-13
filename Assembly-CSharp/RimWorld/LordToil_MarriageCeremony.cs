using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001746 RID: 5958
	public class LordToil_MarriageCeremony : LordToil
	{
		// Token: 0x1700158C RID: 5516
		// (get) Token: 0x06008BF5 RID: 35829 RVA: 0x0029AC6E File Offset: 0x00298E6E
		public LordToilData_MarriageCeremony Data
		{
			get
			{
				return (LordToilData_MarriageCeremony)this.data;
			}
		}

		// Token: 0x06008BF6 RID: 35830 RVA: 0x0029AC7B File Offset: 0x00298E7B
		public LordToil_MarriageCeremony(Pawn firstPawn, Pawn secondPawn, IntVec3 spot)
		{
			this.firstPawn = firstPawn;
			this.secondPawn = secondPawn;
			this.spot = spot;
			this.data = new LordToilData_MarriageCeremony();
		}

		// Token: 0x06008BF7 RID: 35831 RVA: 0x0029ACA4 File Offset: 0x00298EA4
		public override void Init()
		{
			base.Init();
			this.Data.spectateRect = this.CalculateSpectateRect();
			SpectateRectSide allowedSides = SpectateRectSide.All;
			if (this.Data.spectateRect.Width > this.Data.spectateRect.Height)
			{
				allowedSides = SpectateRectSide.Vertical;
			}
			else if (this.Data.spectateRect.Height > this.Data.spectateRect.Width)
			{
				allowedSides = SpectateRectSide.Horizontal;
			}
			this.Data.spectateRectAllowedSides = SpectatorCellFinder.FindSingleBestSide(this.Data.spectateRect, base.Map, allowedSides, 1, null);
		}

		// Token: 0x06008BF8 RID: 35832 RVA: 0x0029AD3A File Offset: 0x00298F3A
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			if (this.IsFiance(p))
			{
				return DutyDefOf.MarryPawn.hook;
			}
			return DutyDefOf.Spectate.hook;
		}

		// Token: 0x06008BF9 RID: 35833 RVA: 0x0029AD5C File Offset: 0x00298F5C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (this.IsFiance(pawn))
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.MarryPawn, this.FianceStandingSpotFor(pawn), -1f);
				}
				else
				{
					PawnDuty pawnDuty = new PawnDuty(DutyDefOf.Spectate);
					pawnDuty.spectateRect = this.Data.spectateRect;
					pawnDuty.spectateRectAllowedSides = this.Data.spectateRectAllowedSides;
					pawn.mindState.duty = pawnDuty;
				}
			}
		}

		// Token: 0x06008BFA RID: 35834 RVA: 0x0029AE06 File Offset: 0x00299006
		private bool IsFiance(Pawn p)
		{
			return p == this.firstPawn || p == this.secondPawn;
		}

		// Token: 0x06008BFB RID: 35835 RVA: 0x0029AE1C File Offset: 0x0029901C
		public IntVec3 FianceStandingSpotFor(Pawn pawn)
		{
			Pawn pawn2;
			if (this.firstPawn == pawn)
			{
				pawn2 = this.secondPawn;
			}
			else
			{
				if (this.secondPawn != pawn)
				{
					Log.Warning("Called ExactStandingSpotFor but it's not this pawn's ceremony.");
					return IntVec3.Invalid;
				}
				pawn2 = this.firstPawn;
			}
			if (pawn.thingIDNumber < pawn2.thingIDNumber)
			{
				return this.spot;
			}
			if (this.GetMarriageSpotAt(this.spot) != null)
			{
				return this.FindCellForOtherPawnAtMarriageSpot(this.spot);
			}
			return this.spot + LordToil_MarriageCeremony.OtherFianceNoMarriageSpotCellOffset;
		}

		// Token: 0x06008BFC RID: 35836 RVA: 0x0029AE9F File Offset: 0x0029909F
		private Thing GetMarriageSpotAt(IntVec3 cell)
		{
			return cell.GetThingList(base.Map).Find((Thing x) => x.def == ThingDefOf.MarriageSpot);
		}

		// Token: 0x06008BFD RID: 35837 RVA: 0x0029AED4 File Offset: 0x002990D4
		private IntVec3 FindCellForOtherPawnAtMarriageSpot(IntVec3 cell)
		{
			CellRect cellRect = this.GetMarriageSpotAt(cell).OccupiedRect();
			for (int i = cellRect.minX; i <= cellRect.maxX; i++)
			{
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					if (cell.x != i || cell.z != j)
					{
						return new IntVec3(i, 0, j);
					}
				}
			}
			Log.Warning("Marriage spot is 1x1. There's no place for 2 pawns.");
			return IntVec3.Invalid;
		}

		// Token: 0x06008BFE RID: 35838 RVA: 0x0029AF44 File Offset: 0x00299144
		private CellRect CalculateSpectateRect()
		{
			IntVec3 first = this.FianceStandingSpotFor(this.firstPawn);
			IntVec3 second = this.FianceStandingSpotFor(this.secondPawn);
			return CellRect.FromLimits(first, second);
		}

		// Token: 0x04005AFA RID: 23290
		private Pawn firstPawn;

		// Token: 0x04005AFB RID: 23291
		private Pawn secondPawn;

		// Token: 0x04005AFC RID: 23292
		private IntVec3 spot;

		// Token: 0x04005AFD RID: 23293
		public static readonly IntVec3 OtherFianceNoMarriageSpotCellOffset = new IntVec3(-1, 0, 0);
	}
}
