using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200174A RID: 5962
	public class LordToil_Speech : LordToil_Ritual
	{
		// Token: 0x1700158D RID: 5517
		// (get) Token: 0x06008C07 RID: 35847 RVA: 0x0029B07A File Offset: 0x0029927A
		public new LordToilData_Speech Data
		{
			get
			{
				return (LordToilData_Speech)this.data;
			}
		}

		// Token: 0x06008C08 RID: 35848 RVA: 0x0029B087 File Offset: 0x00299287
		public LordToil_Speech(IntVec3 spot, Precept_Ritual ritual, LordJob_Ritual lordJob, Pawn organizer) : base(spot, lordJob, null, organizer)
		{
			this.organizer = organizer;
			this.data = new LordToilData_Speech();
		}

		// Token: 0x06008C09 RID: 35849 RVA: 0x0029B0A8 File Offset: 0x002992A8
		public override void Init()
		{
			base.Init();
			this.Data.spectateRect = CellRect.CenteredOn(this.spot, 0);
			Rot4 rotation = this.spot.GetFirstThing(this.organizer.MapHeld).Rotation;
			SpectateRectSide asSpectateSide = rotation.Opposite.AsSpectateSide;
			this.Data.spectateRectAllowedSides = (SpectateRectSide.All & ~asSpectateSide);
			this.Data.spectateRectPreferredSide = rotation.AsSpectateSide;
		}

		// Token: 0x06008C0A RID: 35850 RVA: 0x0029B120 File Offset: 0x00299320
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			if (p == this.organizer)
			{
				return DutyDefOf.GiveSpeech.hook;
			}
			return DutyDefOf.Spectate.hook;
		}

		// Token: 0x06008C0B RID: 35851 RVA: 0x0029B140 File Offset: 0x00299340
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn == this.organizer)
				{
					Building_Throne firstThing = this.spot.GetFirstThing(base.Map);
					pawn.mindState.duty = new PawnDuty(DutyDefOf.GiveSpeech, this.spot, firstThing, -1f);
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				else
				{
					PawnDuty pawnDuty = new PawnDuty(DutyDefOf.Spectate);
					pawnDuty.spectateRect = this.Data.spectateRect;
					pawnDuty.spectateRectAllowedSides = this.Data.spectateRectAllowedSides;
					pawnDuty.spectateRectPreferredSide = this.Data.spectateRectPreferredSide;
					pawn.mindState.duty = pawnDuty;
				}
			}
		}
	}
}
