using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027C6 RID: 10182
	public class RitualStageAction_DropThing : RitualStageAction
	{
		// Token: 0x0600DEF9 RID: 57081 RVA: 0x00418CD0 File Offset: 0x00416ED0
		public override void Apply(LordJob_Ritual ritual)
		{
			foreach (Pawn pawn in ritual.assignments.Participants)
			{
				this.ApplyToPawn(ritual, pawn);
			}
		}

		// Token: 0x0600DEFA RID: 57082 RVA: 0x00418D2C File Offset: 0x00416F2C
		public override void ApplyToPawn(LordJob_Ritual ritual, Pawn pawn)
		{
			Thing carriedThing = pawn.carryTracker.CarriedThing;
			Thing thing;
			if (carriedThing != null && carriedThing.def == this.def && carriedThing.stackCount >= this.count && pawn.carryTracker.TryDropCarriedThing(pawn.Position, ThingPlaceMode.Near, out thing, null))
			{
				return;
			}
			int num = Math.Min(pawn.inventory.Count(this.def), this.count);
			pawn.inventory.DropCount(this.def, num, false, false);
		}

		// Token: 0x0600DEFB RID: 57083 RVA: 0x00418DAD File Offset: 0x00416FAD
		public override void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
		}

		// Token: 0x04009624 RID: 38436
		public ThingDef def;

		// Token: 0x04009625 RID: 38437
		public int count = 1;
	}
}
