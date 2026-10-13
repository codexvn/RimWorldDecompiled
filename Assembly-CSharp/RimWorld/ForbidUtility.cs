using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002CA8 RID: 11432
	public static class ForbidUtility
	{
		// Token: 0x060100C4 RID: 65732 RVA: 0x004A548C File Offset: 0x004A368C
		public static void TrySetForbidden(this Thing t, bool value)
		{
			ThingWithComps thingWithComps = t as ThingWithComps;
			CompForbiddable compForbiddable;
			if (thingWithComps != null && thingWithComps.TryGetComp(out compForbiddable))
			{
				compForbiddable.Forbidden = value;
			}
		}

		// Token: 0x060100C5 RID: 65733 RVA: 0x004A54B4 File Offset: 0x004A36B4
		public static void SetForbidden(this Thing t, bool value, bool warnOnFail = true)
		{
			if (t == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on null Thing.");
				}
				return;
			}
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on non-ThingWithComps Thing " + ((t != null) ? t.ToString() : null));
				}
				return;
			}
			CompForbiddable comp = thingWithComps.GetComp<CompForbiddable>();
			if (comp == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on non-Forbiddable Thing " + ((t != null) ? t.ToString() : null));
				}
				return;
			}
			comp.Forbidden = value;
		}

		// Token: 0x060100C6 RID: 65734 RVA: 0x004A5530 File Offset: 0x004A3730
		public static void SetForbiddenIfOutsideHomeArea(this Thing t)
		{
			if (!t.Spawned)
			{
				Log.Error("SetForbiddenIfOutsideHomeArea unspawned thing " + ((t != null) ? t.ToString() : null));
			}
			if (t.Position.InBounds(t.Map) && !t.Map.areaManager.Home[t.Position])
			{
				t.SetForbidden(true, false);
			}
		}

		// Token: 0x060100C7 RID: 65735 RVA: 0x004A559C File Offset: 0x004A379C
		public static bool CaresAboutForbidden(Pawn pawn, bool cellTarget, bool bypassDraftedCheck = false)
		{
			return (pawn.HostFaction == null || (pawn.HostFaction == Faction.OfPlayer && pawn.Spawned && !pawn.Map.IsPlayerHome && (pawn.GetRoom(RegionType.Set_All) == null || !pawn.GetRoom(RegionType.Set_All).IsPrisonCell) && (!pawn.IsPrisoner || pawn.guest.PrisonerIsSecure))) && (bypassDraftedCheck || !pawn.Drafted) && !pawn.InMentalState && !SlaveRebellionUtility.IsRebelling(pawn) && (!cellTarget || !ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn)) && !pawn.IsColonyMechRequiringMechanitor() && (!ModsConfig.AnomalyActive || pawn.kindDef != PawnKindDefOf.Revenant);
		}

		// Token: 0x060100C8 RID: 65736 RVA: 0x004A5658 File Offset: 0x004A3858
		public static bool InAllowedArea(this IntVec3 c, Pawn forPawn)
		{
			if (forPawn.playerSettings != null)
			{
				Area effectiveAreaRestrictionInPawnCurrentMap = forPawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap;
				if (effectiveAreaRestrictionInPawnCurrentMap != null && effectiveAreaRestrictionInPawnCurrentMap.TrueCount > 0 && !effectiveAreaRestrictionInPawnCurrentMap[c])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060100C9 RID: 65737 RVA: 0x004A5691 File Offset: 0x004A3891
		public static bool IsForbiddenHeld(this Thing t, Pawn pawn)
		{
			return t.SpawnedParentOrMe.IsForbidden(pawn);
		}

		// Token: 0x060100CA RID: 65738 RVA: 0x004A56A0 File Offset: 0x004A38A0
		public static bool IsForbidden(this Thing t, Pawn pawn)
		{
			if (!ForbidUtility.CaresAboutForbidden(pawn, false, false))
			{
				return false;
			}
			if ((t.Spawned || t.SpawnedParentOrMe != pawn) && t.PositionHeld.IsForbidden(pawn))
			{
				return true;
			}
			if (t.IsForbidden(pawn.Faction) || t.IsForbidden(pawn.HostFaction))
			{
				return true;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && lord.extraForbiddenThings.Contains(t))
			{
				return true;
			}
			foreach (Lord lord2 in pawn.MapHeld.lordManager.lords)
			{
				LordToil_Ritual lordToil_Ritual = lord2.CurLordToil as LordToil_Ritual;
				if (lordToil_Ritual != null && lordToil_Ritual.ReservedThings.Contains(t) && lord2 != lord)
				{
					return true;
				}
				LordToil_PsychicRitual lordToil_PsychicRitual = lord2.CurLordToil as LordToil_PsychicRitual;
				if (lordToil_PsychicRitual != null)
				{
					PsychicRitualDef_InvocationCircle psychicRitualDef_InvocationCircle = lordToil_PsychicRitual.RitualData.psychicRitual.def as PsychicRitualDef_InvocationCircle;
					if (psychicRitualDef_InvocationCircle != null && psychicRitualDef_InvocationCircle.TargetRole != null && lordToil_PsychicRitual.RitualData.psychicRitual.assignments.FirstAssignedPawn(psychicRitualDef_InvocationCircle.TargetRole) == t && !(lordToil_PsychicRitual.RitualData.CurPsychicRitualToil is PsychicRitualToil_TargetCleanup) && lord2 != lord)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060100CB RID: 65739 RVA: 0x004A5808 File Offset: 0x004A3A08
		public static bool IsForbiddenToPass(this Building_Door t, Pawn pawn)
		{
			return ForbidUtility.CaresAboutForbidden(pawn, false, true) && t.IsForbidden(pawn.Faction);
		}

		// Token: 0x060100CC RID: 65740 RVA: 0x004A5828 File Offset: 0x004A3A28
		public static bool IsForbidden(this IntVec3 c, Pawn pawn)
		{
			return ForbidUtility.CaresAboutForbidden(pawn, true, false) && (!c.InAllowedArea(pawn) || (pawn.mindState.maxDistToSquadFlag > 0f && !c.InHorDistOf(pawn.DutyLocation(), pawn.mindState.maxDistToSquadFlag)));
		}

		// Token: 0x060100CD RID: 65741 RVA: 0x004A587C File Offset: 0x004A3A7C
		public static bool IsForbiddenEntirely(this Region r, Pawn pawn)
		{
			if (!ForbidUtility.CaresAboutForbidden(pawn, true, false))
			{
				return false;
			}
			if (pawn.playerSettings != null)
			{
				Area effectiveAreaRestrictionInPawnCurrentMap = pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap;
				if (effectiveAreaRestrictionInPawnCurrentMap != null && effectiveAreaRestrictionInPawnCurrentMap.TrueCount > 0 && effectiveAreaRestrictionInPawnCurrentMap.Map == r.Map && r.OverlapWith(effectiveAreaRestrictionInPawnCurrentMap) == AreaOverlap.None)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060100CE RID: 65742 RVA: 0x004A58D0 File Offset: 0x004A3AD0
		public static bool IsForbidden(this Thing t, Faction faction)
		{
			if (faction == null)
			{
				return false;
			}
			if (faction != Faction.OfPlayer)
			{
				return false;
			}
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				return false;
			}
			CompForbiddable compForbiddable = thingWithComps.compForbiddable;
			return compForbiddable != null && compForbiddable.Forbidden;
		}
	}
}
