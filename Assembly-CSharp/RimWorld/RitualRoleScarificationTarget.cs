using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020027B5 RID: 10165
	public class RitualRoleScarificationTarget : RitualRole
	{
		// Token: 0x0600DEC3 RID: 57027 RVA: 0x0041803C File Offset: 0x0041623C
		public override bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!base.AppliesIfChild(p, out reason, skipReason))
			{
				return false;
			}
			if (!p.RaceProps.Humanlike)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeHumanlike".Translate(base.LabelCap);
				}
				return false;
			}
			if (p.Ideo == null || p.Ideo.RequiredScars <= p.health.hediffSet.GetHediffCount(HediffDefOf.Scarification))
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustRequireScarification".Translate(p);
				}
				return false;
			}
			if (!p.Faction.IsPlayerSafe())
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeColonist".Translate(base.Label);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600DEC4 RID: 57028 RVA: 0x004180FD File Offset: 0x004162FD
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return true;
		}

		// Token: 0x0600DEC5 RID: 57029 RVA: 0x00418104 File Offset: 0x00416304
		public override string ExtraInfoForDialog(IEnumerable<Pawn> selected)
		{
			Pawn pawn = selected.FirstOrDefault<Pawn>();
			if (pawn != null)
			{
				if (pawn.Ideo == null || pawn.Ideo.RequiredScars <= 0)
				{
					return "RitualScarificationWarnNonScarificationIdeo".Translate(pawn.Named("PAWN"));
				}
				if (pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.Wimp))
				{
					return "RitualScarificationWarnWimp".Translate(pawn.Named("PAWN"));
				}
			}
			return null;
		}
	}
}
