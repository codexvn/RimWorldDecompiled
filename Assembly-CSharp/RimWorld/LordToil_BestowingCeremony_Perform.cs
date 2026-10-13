using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200163E RID: 5694
	public class LordToil_BestowingCeremony_Perform : LordToil_Wait
	{
		// Token: 0x17001493 RID: 5267
		// (get) Token: 0x0600862B RID: 34347 RVA: 0x00280E7F File Offset: 0x0027F07F
		public LordToilData_Gathering Data
		{
			get
			{
				return (LordToilData_Gathering)this.data;
			}
		}

		// Token: 0x0600862C RID: 34348 RVA: 0x00280E8C File Offset: 0x0027F08C
		public LordToil_BestowingCeremony_Perform(Pawn target, Pawn bestower) : base(true)
		{
			this.target = target;
			this.bestower = bestower;
			this.data = new LordToilData_Gathering();
		}

		// Token: 0x0600862D RID: 34349 RVA: 0x00280EAE File Offset: 0x0027F0AE
		public override void Init()
		{
			base.Init();
			if (!this.target.Awake())
			{
				RestUtility.WakeUp(this.target, true);
			}
		}

		// Token: 0x0600862E RID: 34350 RVA: 0x00280ED0 File Offset: 0x0027F0D0
		public override void LordToilTick()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (GatheringsUtility.InGatheringArea(ownedPawns[i].Position, this.target.Position, base.Map))
				{
					if (!this.Data.presentForTicks.ContainsKey(ownedPawns[i]))
					{
						this.Data.presentForTicks.Add(ownedPawns[i], 0);
					}
					Dictionary<Pawn, int> presentForTicks = this.Data.presentForTicks;
					Pawn key = ownedPawns[i];
					int num = presentForTicks[key];
					presentForTicks[key] = num + 1;
				}
			}
		}

		// Token: 0x0600862F RID: 34351 RVA: 0x00280F78 File Offset: 0x0027F178
		public override void UpdateAllDuties()
		{
			IntVec3 spot = ((LordJob_BestowingCeremony)this.lord.LordJob).GetSpot();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!pawn.Awake())
				{
					RestUtility.WakeUp(pawn, true);
				}
				if (pawn == this.bestower)
				{
					PawnDuty pawnDuty = new PawnDuty(DutyDefOf.Idle);
					pawnDuty.focus = spot;
					pawn.mindState.duty = pawnDuty;
				}
				else if (pawn == this.target)
				{
					PawnDuty duty = new PawnDuty(DutyDefOf.Bestow, this.bestower, spot, -1f);
					pawn.mindState.duty = duty;
				}
				else
				{
					PawnDuty pawnDuty2 = new PawnDuty(DutyDefOf.Spectate, spot, -1f);
					pawnDuty2.spectateRect = CellRect.CenteredOn(spot, 0);
					pawnDuty2.spectateRectAllowedSides = SpectateRectSide.All;
					pawnDuty2.spectateDistance = new IntRange(2, 2);
					pawn.mindState.duty = pawnDuty2;
				}
				Pawn_JobTracker jobs = pawn.jobs;
				if (jobs != null)
				{
					jobs.CheckForJobOverride(0f, true);
				}
			}
		}

		// Token: 0x040057AF RID: 22447
		public Pawn target;

		// Token: 0x040057B0 RID: 22448
		public Pawn bestower;
	}
}
