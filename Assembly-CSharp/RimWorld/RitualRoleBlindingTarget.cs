using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020027B7 RID: 10167
	public class RitualRoleBlindingTarget : RitualRole
	{
		// Token: 0x0600DECA RID: 57034 RVA: 0x00418258 File Offset: 0x00416458
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
			if (p.Ideo != null)
			{
				if (p.Ideo.PreceptsListForReading.Any((Precept pc) => pc.def.issue == IssueDefOf.Blindness))
				{
					if (p.health != null)
					{
						if (p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord part) => part.def == BodyPartDefOf.Eye))
						{
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
					}
					if (!skipReason)
					{
						reason = "MessageRitualRoleMustHaveEyes".Translate(base.LabelCap);
					}
					return false;
				}
			}
			if (!skipReason)
			{
				reason = "MessageRitualRoleMustRequireBlinding".Translate(p);
			}
			return false;
		}

		// Token: 0x0600DECB RID: 57035 RVA: 0x00417DBD File Offset: 0x00415FBD
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			return false;
		}

		// Token: 0x0600DECC RID: 57036 RVA: 0x0041838C File Offset: 0x0041658C
		public override string ExtraInfoForDialog(IEnumerable<Pawn> selected)
		{
			Pawn pawn = selected.FirstOrDefault<Pawn>();
			if (pawn != null && pawn.story != null && pawn.story.traits.HasTrait(TraitDefOf.Wimp))
			{
				return "RitualBlindingWarnWimp".Translate(pawn.Named("PAWN"));
			}
			return null;
		}
	}
}
