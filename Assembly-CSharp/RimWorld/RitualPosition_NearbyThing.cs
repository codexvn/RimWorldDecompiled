using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002798 RID: 10136
	public abstract class RitualPosition_NearbyThing : RitualPosition
	{
		// Token: 0x0600DE04 RID: 56836
		public abstract IEnumerable<Thing> CandidateThings(LordJob_Ritual ritual);

		// Token: 0x0600DE05 RID: 56837
		public abstract IntVec3 PositionForThing(Thing t);

		// Token: 0x0600DE06 RID: 56838 RVA: 0x004155AC File Offset: 0x004137AC
		public override PawnStagePosition GetCell(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			foreach (Thing thing in this.CandidateThings(ritual))
			{
				if (this.IsUsableThing(thing, spot, ritual))
				{
					return new PawnStagePosition(this.PositionForThing(thing), thing, this.FacingDir(thing), this.highlight);
				}
			}
			return new PawnStagePosition(IntVec3.Invalid, null, Rot4.Invalid, this.highlight);
		}

		// Token: 0x0600DE07 RID: 56839 RVA: 0x00415634 File Offset: 0x00413834
		public virtual bool IsUsableThing(Thing thing, IntVec3 spot, TargetInfo ritualTarget)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (IntVec3 intVec in (ritualTarget.HasThing ? ritualTarget.Thing.OccupiedRect() : CellRect.CenteredOn(spot, 0)).Cells)
			{
				if (thing.Position.InHorDistOf(intVec, (float)this.maxDistanceToFocus))
				{
					flag = true;
				}
				if (GenSight.LineOfSight(thing.Position, intVec, ritualTarget.Map, true, null, 0, 0))
				{
					flag2 = true;
				}
			}
			return flag && flag2;
		}

		// Token: 0x0600DE08 RID: 56840 RVA: 0x004156DC File Offset: 0x004138DC
		public bool IsUsableThing(Thing thing, IntVec3 spot, LordJob_Ritual ritual)
		{
			return this.IsUsableThing(thing, spot, ritual.selectedTarget);
		}

		// Token: 0x0600DE09 RID: 56841 RVA: 0x004156EC File Offset: 0x004138EC
		public override bool CanUse(IntVec3 spot, Pawn p, LordJob_Ritual ritual)
		{
			return this.GetCell(spot, p, ritual).cell.IsValid;
		}

		// Token: 0x0600DE0A RID: 56842 RVA: 0x001E5B21 File Offset: 0x001E3D21
		protected virtual Rot4 FacingDir(Thing t)
		{
			return Rot4.Invalid;
		}

		// Token: 0x040095B0 RID: 38320
		public int maxDistanceToFocus = 1;
	}
}
