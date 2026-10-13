using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016CD RID: 5837
	public class LordJob_Joinable_Concert : LordJob_Joinable_Party
	{
		// Token: 0x17001512 RID: 5394
		// (get) Token: 0x060089BF RID: 35263 RVA: 0x00291DC6 File Offset: 0x0028FFC6
		protected override ThoughtDef AttendeeThought
		{
			get
			{
				return ThoughtDefOf.AttendedConcert;
			}
		}

		// Token: 0x17001513 RID: 5395
		// (get) Token: 0x060089C0 RID: 35264 RVA: 0x00291DCD File Offset: 0x0028FFCD
		protected override TaleDef AttendeeTale
		{
			get
			{
				return TaleDefOf.AttendedConcert;
			}
		}

		// Token: 0x17001514 RID: 5396
		// (get) Token: 0x060089C1 RID: 35265 RVA: 0x00291DD4 File Offset: 0x0028FFD4
		protected override ThoughtDef OrganizerThought
		{
			get
			{
				return ThoughtDefOf.HeldConcert;
			}
		}

		// Token: 0x17001515 RID: 5397
		// (get) Token: 0x060089C2 RID: 35266 RVA: 0x00291DDB File Offset: 0x0028FFDB
		protected override TaleDef OrganizerTale
		{
			get
			{
				return TaleDefOf.HeldConcert;
			}
		}

		// Token: 0x060089C3 RID: 35267 RVA: 0x00291DE2 File Offset: 0x0028FFE2
		public LordJob_Joinable_Concert()
		{
		}

		// Token: 0x060089C4 RID: 35268 RVA: 0x00291DEA File Offset: 0x0028FFEA
		public LordJob_Joinable_Concert(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef) : base(spot, organizer, gatheringDef)
		{
		}

		// Token: 0x060089C5 RID: 35269 RVA: 0x00291DF5 File Offset: 0x0028FFF5
		public override string GetReport(Pawn pawn)
		{
			if (pawn != this.organizer)
			{
				return "LordReportAttendingConcert".Translate();
			}
			return "LordReportHoldingConcert".Translate();
		}

		// Token: 0x060089C6 RID: 35270 RVA: 0x00291E1F File Offset: 0x0029001F
		protected override LordToil CreateGatheringToil(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef)
		{
			return new LordToil_Concert(spot, organizer, gatheringDef, 3.5E-05f);
		}

		// Token: 0x060089C7 RID: 35271 RVA: 0x00291E2E File Offset: 0x0029002E
		protected override Trigger_TicksPassed GetTimeoutTrigger()
		{
			return new Trigger_TicksPassedAfterConditionMet(base.DurationTicks, () => GatheringsUtility.InGatheringArea(this.organizer.Position, this.spot, this.organizer.Map), 60);
		}
	}
}
