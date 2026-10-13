using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02001068 RID: 4200
	public class PsychicRitualToil_CarryAndGoto : PsychicRitualToil_Goto
	{
		// Token: 0x060066B3 RID: 26291 RVA: 0x002078AA File Offset: 0x00205AAA
		protected PsychicRitualToil_CarryAndGoto()
		{
		}

		// Token: 0x060066B4 RID: 26292 RVA: 0x002078B2 File Offset: 0x00205AB2
		public PsychicRitualToil_CarryAndGoto(PsychicRitualRoleDef carrierRole, PsychicRitualRoleDef payloadRole, IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions) : base(rolePositions.Slice(new PsychicRitualRoleDef[]
		{
			carrierRole
		}))
		{
			this.payloadPositions = new List<IntVec3>(rolePositions[payloadRole]);
			this.carrierRole = carrierRole;
			this.payloadRole = payloadRole;
		}

		// Token: 0x060066B5 RID: 26293 RVA: 0x002078EC File Offset: 0x00205AEC
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.UpdateAllDuties(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.payloadRole);
			if (pawn == null)
			{
				return;
			}
			Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(this.carrierRole);
			if (pawn2 == null)
			{
				return;
			}
			if (pawn.IsPrisoner)
			{
				this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.Idle, null, null, null, null, 0f, null);
				pawn.mindState.duty.focus = psychicRitual.assignments.Target.Cell;
			}
			this.SetPawnDuty(pawn2, psychicRitual, parent, DutyDefOf.DeliverPawnToPsychicRitualCell, new LocalTargetInfo?(pawn2.mindState.duty.focus), new LocalTargetInfo?(pawn), new LocalTargetInfo?(this.payloadPositions[0]), base.FinalGatherPhase ? "final" : "initial", 0f, null);
		}

		// Token: 0x060066B6 RID: 26294 RVA: 0x002079FC File Offset: 0x00205BFC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.carrierRole, "carrierRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.payloadRole, "payloadRole");
			Scribe_Collections.Look<IntVec3>(ref this.payloadPositions, "payloadPositions", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0400494A RID: 18762
		public PsychicRitualRoleDef carrierRole;

		// Token: 0x0400494B RID: 18763
		public PsychicRitualRoleDef payloadRole;

		// Token: 0x0400494C RID: 18764
		protected List<IntVec3> payloadPositions;
	}
}
