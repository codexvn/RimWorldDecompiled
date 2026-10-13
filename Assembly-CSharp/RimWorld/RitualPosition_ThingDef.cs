using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002799 RID: 10137
	public abstract class RitualPosition_ThingDef : RitualPosition_NearbyThing
	{
		// Token: 0x170021EC RID: 8684
		// (get) Token: 0x0600DE0C RID: 56844
		protected abstract ThingDef ThingDef { get; }

		// Token: 0x0600DE0D RID: 56845 RVA: 0x00415710 File Offset: 0x00413910
		public override IEnumerable<Thing> CandidateThings(LordJob_Ritual ritual)
		{
			foreach (Thing thing in ritual.Map.listerThings.ThingsOfDef(this.ThingDef))
			{
				if (thing.Position.InHorDistOf(ritual.selectedTarget.Cell, 50f))
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600DE0E RID: 56846 RVA: 0x00415728 File Offset: 0x00413928
		public override IntVec3 PositionForThing(Thing t)
		{
			return t.Position + t.Rotation.Opposite.FacingCell;
		}

		// Token: 0x0600DE0F RID: 56847 RVA: 0x00415758 File Offset: 0x00413958
		public override bool IsUsableThing(Thing thing, IntVec3 spot, TargetInfo ritualTarget)
		{
			IntVec3 c = this.PositionForThing(thing);
			Map map = ritualTarget.Map;
			Building edifice = c.GetEdifice(map);
			return c.Standable(map) && (edifice == null || edifice.def.Fillage == FillCategory.None) && base.IsUsableThing(thing, spot, ritualTarget);
		}

		// Token: 0x0600DE10 RID: 56848 RVA: 0x0041579F File Offset: 0x0041399F
		protected override Rot4 FacingDir(Thing t)
		{
			return t.Rotation;
		}
	}
}
