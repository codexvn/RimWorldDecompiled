using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200231F RID: 8991
	public static class InvisibilityUtility
	{
		// Token: 0x0600C32F RID: 49967 RVA: 0x003894FC File Offset: 0x003876FC
		public static bool IsPsychologicallyInvisible(this Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffComp_Invisibility hediffComp_Invisibility = hediffs[i].TryGetComp<HediffComp_Invisibility>();
				if (hediffComp_Invisibility != null && !hediffComp_Invisibility.PsychologicallyVisible)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C330 RID: 49968 RVA: 0x00389548 File Offset: 0x00387748
		public static bool IsHiddenFromPlayer(this Pawn pawn)
		{
			if (DebugSettings.showHiddenPawns)
			{
				return false;
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				return false;
			}
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffComp_Invisibility hediffComp_Invisibility = hediffs[i].TryGetComp<HediffComp_Invisibility>();
				if (hediffComp_Invisibility != null && !hediffComp_Invisibility.Props.visibleToPlayer && !hediffComp_Invisibility.PsychologicallyVisible)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C331 RID: 49969 RVA: 0x003895B8 File Offset: 0x003877B8
		public static float GetAlpha(Pawn pawn)
		{
			if (DebugSettings.showHiddenPawns)
			{
				return 1f;
			}
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			if (hediffs.NullOrEmpty<Hediff>())
			{
				return 1f;
			}
			foreach (Hediff hd in hediffs)
			{
				HediffComp_Invisibility hediffComp_Invisibility = hd.TryGetComp<HediffComp_Invisibility>();
				if (hediffComp_Invisibility != null)
				{
					return hediffComp_Invisibility.GetAlpha();
				}
			}
			return 1f;
		}

		// Token: 0x0600C332 RID: 49970 RVA: 0x00389644 File Offset: 0x00387844
		public static HediffComp_Invisibility GetInvisibilityComp(this Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			if (hediffs.NullOrEmpty<Hediff>())
			{
				return null;
			}
			foreach (Hediff hd in hediffs)
			{
				HediffComp_Invisibility hediffComp_Invisibility = hd.TryGetComp<HediffComp_Invisibility>();
				if (hediffComp_Invisibility != null)
				{
					return hediffComp_Invisibility;
				}
			}
			return null;
		}
	}
}
