using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027BA RID: 10170
	public class RitualRoleColonistConnectable : RitualRoleColonist
	{
		// Token: 0x0600DED7 RID: 57047 RVA: 0x004185E0 File Offset: 0x004167E0
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!base.AppliesToPawn(p, out reason, selectedTarget, ritual, assignments, precept, false))
			{
				return false;
			}
			if (((precept != null) ? precept.ideo : null) != null && precept.ritualOnlyForIdeoMembers && p.Ideo != precept.ideo)
			{
				if (!skipReason)
				{
					reason = "CantStartRitualSelectedPawnMustBeMember".Translate(p.Named("PAWN"), precept.ideo.Named("IDEO"));
				}
				return false;
			}
			if (StatDefOf.PsychicSensitivity.Worker.IsDisabledFor(p) || p.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) <= 0f)
			{
				if (!skipReason)
				{
					reason = "CantStartRitualSelectedPawnPsychicallyDeaf".Translate(p.Named("PAWN"));
				}
				return false;
			}
			Pawn_NeedsTracker needs = p.needs;
			bool flag;
			if (needs == null)
			{
				flag = (null != null);
			}
			else
			{
				Need_Mood mood = needs.mood;
				if (mood == null)
				{
					flag = (null != null);
				}
				else
				{
					ThoughtHandler thoughts = mood.thoughts;
					flag = (((thoughts != null) ? thoughts.memories : null) != null);
				}
			}
			if (flag && p.needs.mood.thoughts.memories.GetFirstMemoryOfDef(ThoughtDefOf.ConnectedTreeDied) != null)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRolePawnRecentlyTornConnection".Translate(p);
				}
				return false;
			}
			return true;
		}
	}
}
