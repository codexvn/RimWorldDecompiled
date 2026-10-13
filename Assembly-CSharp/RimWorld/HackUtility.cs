using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CD6 RID: 11478
	public static class HackUtility
	{
		// Token: 0x060101F5 RID: 66037 RVA: 0x004AA49B File Offset: 0x004A869B
		public static bool IsHackable(this Thing thing)
		{
			return thing.TryGetComp<CompHackable>() != null;
		}

		// Token: 0x060101F6 RID: 66038 RVA: 0x004AA4A8 File Offset: 0x004A86A8
		public static bool IsHacked(this Thing thing)
		{
			CompHackable compHackable = thing.TryGetComp<CompHackable>();
			return compHackable != null && compHackable.IsHacked;
		}

		// Token: 0x060101F7 RID: 66039 RVA: 0x004AA4C8 File Offset: 0x004A86C8
		public static bool IsCapableOfHacking(Pawn pawn)
		{
			return !pawn.IsSubhuman && pawn.skills != null && !pawn.skills.GetSkill(SkillDefOf.Intellectual).TotallyDisabled && !StatDefOf.HackingSpeed.Worker.IsDisabledFor(pawn) && !pawn.WorkTagIsDisabled(WorkTags.Intellectual) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
		}
	}
}
